﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Artemis.UI.Shared;
using Artemis.UI.Shared.Services.ProfileEditor;
using Avalonia.Controls.Mixins;
using ReactiveUI;

namespace Artemis.UI.Screens.ProfileEditor.Properties.Timeline;

public class TimelineGroupViewModel : ActivatableViewModelBase
{
    private ObservableCollection<double>? _keyframePositions;
    private int _pixelsPerSecond;

    public TimelineGroupViewModel(PropertyGroupViewModel propertyGroupViewModel, IProfileEditorService profileEditorService)
    {
        PropertyGroupViewModel = propertyGroupViewModel;

        this.WhenActivated(d =>
        {
            profileEditorService.PixelsPerSecond.Subscribe(p =>
            {
                _pixelsPerSecond = p;
                UpdateKeyframePositions();
            });
            this.WhenAnyValue(vm => vm.PropertyGroupViewModel.IsExpanded).Subscribe(_ => UpdateKeyframePositions()).DisposeWith(d);
            PropertyGroupViewModel.WhenAnyValue(vm => vm.IsExpanded).Subscribe(_ => this.RaisePropertyChanged(nameof(Children))).DisposeWith(d);
        });
    }

    public PropertyGroupViewModel PropertyGroupViewModel { get; }

    public ObservableCollection<ViewModelBase>? Children => PropertyGroupViewModel.IsExpanded ? PropertyGroupViewModel.Children : null;

    public ObservableCollection<double>? KeyframePositions
    {
        get => _keyframePositions;
        set => RaiseAndSetIfChanged(ref _keyframePositions, value);
    }

    private void UpdateKeyframePositions()
    {
        KeyframePositions = new ObservableCollection<double>(PropertyGroupViewModel
            .GetAllKeyframeViewModels(false)
            .Select(p => p.Position.TotalSeconds * _pixelsPerSecond));
    }
}