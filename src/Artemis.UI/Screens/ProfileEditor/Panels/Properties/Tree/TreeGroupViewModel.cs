﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading.Tasks;
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Core.LayerEffects;
using Artemis.UI.Exceptions;
using Artemis.UI.Screens.ProfileEditor.Properties.Tree.ContentDialogs;
using Artemis.UI.Screens.ProfileEditor.Properties.Windows;
using Artemis.UI.Shared;
using Artemis.UI.Shared.LayerBrushes;
using Artemis.UI.Shared.LayerEffects;
using Artemis.UI.Shared.Services;
using Artemis.UI.Shared.Services.Builders;
using Artemis.UI.Shared.Services.ProfileEditor;
using Artemis.UI.Shared.Services.ProfileEditor.Commands;
using Ninject;
using Ninject.Parameters;
using ReactiveUI;

namespace Artemis.UI.Screens.ProfileEditor.Properties.Tree;

public class TreeGroupViewModel : ActivatableViewModelBase
{
    private readonly IProfileEditorService _profileEditorService;
    private readonly IWindowService _windowService;
    private BrushConfigurationWindowViewModel? _brushConfigurationWindowViewModel;
    private EffectConfigurationWindowViewModel? _effectConfigurationWindowViewModel;

    public TreeGroupViewModel(PropertyGroupViewModel propertyGroupViewModel, IWindowService windowService, IProfileEditorService profileEditorService)
    {
        _windowService = windowService;
        _profileEditorService = profileEditorService;
        PropertyGroupViewModel = propertyGroupViewModel;
        DetermineGroupType();

        this.WhenActivated(d =>
        {
            PropertyGroupViewModel.WhenAnyValue(vm => vm.IsExpanded).Subscribe(_ => this.RaisePropertyChanged(nameof(Children))).DisposeWith(d);
            Disposable.Create(CloseViewModels).DisposeWith(d);
        });

        // TODO: Update ProfileElementPropertyGroupViewModel visibility on change (can remove the sub on line 41 as well then)
        OpenBrushSettings = ReactiveCommand.CreateFromTask(ExecuteOpenBrushSettings);
        OpenEffectSettings = ReactiveCommand.CreateFromTask(ExecuteOpenEffectSettings);
        RenameEffect = ReactiveCommand.CreateFromTask(ExecuteRenameEffect);
        DeleteEffect = ReactiveCommand.Create(ExecuteDeleteEffect);
    }

    public PropertyGroupViewModel PropertyGroupViewModel { get; }
    public LayerPropertyGroup LayerPropertyGroup => PropertyGroupViewModel.LayerPropertyGroup;
    public BaseLayerBrush? LayerBrush => PropertyGroupViewModel.LayerBrush;
    public BaseLayerEffect? LayerEffect => PropertyGroupViewModel.LayerEffect;
    public LayerPropertyGroupType GroupType { get; private set; }
    public ObservableCollection<PropertyViewModelBase>? Children => PropertyGroupViewModel.IsExpanded ? PropertyGroupViewModel.Children : null;

    public ReactiveCommand<Unit, Unit> OpenBrushSettings { get; }
    public ReactiveCommand<Unit, Unit> OpenEffectSettings { get; }
    public ReactiveCommand<Unit, Unit> RenameEffect { get; }
    public ReactiveCommand<Unit, Unit> DeleteEffect { get; }

    public double GetDepth()
    {
        int depth = 0;
        LayerPropertyGroup? current = LayerPropertyGroup.Parent;
        while (current != null)
        {
            depth++;
            current = current.Parent;
        }

        return depth;
    }

    private async Task ExecuteOpenBrushSettings()
    {
        if (LayerBrush?.ConfigurationDialog is not LayerBrushConfigurationDialog configurationViewModel)
            return;

        try
        {
            // Limit to one constructor, there's no need to have more and it complicates things anyway
            ConstructorInfo[] constructors = configurationViewModel.Type.GetConstructors();
            if (constructors.Length != 1)
                throw new ArtemisUIException("Brush configuration dialogs must have exactly one constructor");

            // Find the BaseLayerBrush parameter, it is required by the base constructor so its there for sure
            ParameterInfo brushParameter = constructors.First().GetParameters().First(p => typeof(BaseLayerBrush).IsAssignableFrom(p.ParameterType));
            ConstructorArgument argument = new(brushParameter.Name!, LayerBrush);
            BrushConfigurationViewModel viewModel =
                (BrushConfigurationViewModel) LayerBrush.Descriptor.Provider.Plugin.Kernel!.Get(configurationViewModel.Type, argument);

            _brushConfigurationWindowViewModel = new BrushConfigurationWindowViewModel(viewModel, configurationViewModel);
            await _windowService.ShowDialogAsync(_brushConfigurationWindowViewModel);

            // Save changes after the dialog closes
            await _profileEditorService.SaveProfileAsync();
        }
        catch (Exception e)
        {
            _windowService.ShowExceptionDialog("An exception occurred while trying to show the brush's settings window", e);
        }
    }

    private async Task ExecuteOpenEffectSettings()
    {
        if (LayerEffect?.ConfigurationDialog is not LayerEffectConfigurationDialog configurationViewModel)
            return;

        try
        {
            // Limit to one constructor, there's no need to have more and it complicates things anyway
            ConstructorInfo[] constructors = configurationViewModel.Type.GetConstructors();
            if (constructors.Length != 1)
                throw new ArtemisUIException("Effect configuration dialogs must have exactly one constructor");

            // Find the BaseLayerEffect parameter, it is required by the base constructor so its there for sure
            ParameterInfo effectParameter = constructors.First().GetParameters().First(p => typeof(BaseLayerEffect).IsAssignableFrom(p.ParameterType));
            ConstructorArgument argument = new(effectParameter.Name!, LayerEffect);
            EffectConfigurationViewModel viewModel =
                (EffectConfigurationViewModel) LayerEffect.Descriptor.Provider.Plugin.Kernel!.Get(configurationViewModel.Type, argument);

            _effectConfigurationWindowViewModel = new EffectConfigurationWindowViewModel(viewModel, configurationViewModel);
            await _windowService.ShowDialogAsync(_effectConfigurationWindowViewModel);

            // Save changes after the dialog closes
            await _profileEditorService.SaveProfileAsync();
        }
        catch (Exception e)
        {
            _windowService.ShowExceptionDialog("An exception occurred while trying to show the effect's settings window", e);
        }
    }

    private async Task ExecuteRenameEffect()
    {
        if (LayerEffect == null)
            return;

        await _windowService.CreateContentDialog()
            .WithTitle("Rename layer effect")
            .WithViewModel(out LayerEffectRenameViewModel vm, ("layerEffect", LayerEffect))
            .HavingPrimaryButton(b => b.WithText("Confirm").WithCommand(vm.Confirm))
            .WithCloseButtonText("Cancel")
            .WithDefaultButton(ContentDialogButton.Primary)
            .ShowAsync();
    }

    private void ExecuteDeleteEffect()
    {
        if (LayerEffect == null)
            return;

        _profileEditorService.ExecuteCommand(new RemoveLayerEffect(LayerEffect));
    }

    private void CloseViewModels()
    {
        _effectConfigurationWindowViewModel?.Close(null);
        _brushConfigurationWindowViewModel?.Close(null);
    }

    private void DetermineGroupType()
    {
        if (LayerPropertyGroup is LayerGeneralProperties)
            GroupType = LayerPropertyGroupType.General;
        else if (LayerPropertyGroup is LayerTransformProperties)
            GroupType = LayerPropertyGroupType.Transform;
        else if (LayerPropertyGroup.Parent == null && PropertyGroupViewModel.LayerBrush != null)
            GroupType = LayerPropertyGroupType.LayerBrushRoot;
        else if (LayerPropertyGroup.Parent == null && PropertyGroupViewModel.LayerEffect != null)
            GroupType = LayerPropertyGroupType.LayerEffectRoot;
        else
            GroupType = LayerPropertyGroupType.None;
    }
}

public enum LayerPropertyGroupType
{
    General,
    Transform,
    LayerBrushRoot,
    LayerEffectRoot,
    None
}