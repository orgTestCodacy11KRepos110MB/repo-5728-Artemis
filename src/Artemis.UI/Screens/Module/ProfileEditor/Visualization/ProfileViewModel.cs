﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Artemis.Core.Events;
using Artemis.Core.Models.Profile;
using Artemis.Core.Models.Surface;
using Artemis.Core.Plugins.Models;
using Artemis.Core.Services;
using Artemis.Core.Services.Storage.Interfaces;
using Artemis.UI.Events;
using Artemis.UI.Extensions;
using Artemis.UI.Ninject.Factories;
using Artemis.UI.Screens.Module.ProfileEditor.Visualization.Tools;
using Artemis.UI.Screens.Shared;
using Artemis.UI.Services.Interfaces;
using RGB.NET.Core;
using Stylet;

namespace Artemis.UI.Screens.Module.ProfileEditor.Visualization
{
    public class ProfileViewModel : ProfileEditorPanelViewModel, IHandle<MainWindowFocusChangedEvent>, IHandle<MainWindowKeyEvent>
    {
        private readonly ILayerEditorService _layerEditorService;
        private readonly IProfileEditorService _profileEditorService;
        private readonly IProfileLayerVmFactory _profileLayerVmFactory;
        private readonly ISettingsService _settingsService;
        private readonly ISurfaceService _surfaceService;
        private int _activeToolIndex;
        private VisualizationToolViewModel _activeToolViewModel;
        private int _previousTool;

        public ProfileViewModel(IProfileEditorService profileEditorService,
            ILayerEditorService layerEditorService,
            ISurfaceService surfaceService,
            ISettingsService settingsService,
            IEventAggregator eventAggregator,
            IProfileLayerVmFactory profileLayerVmFactory)
        {
            _profileEditorService = profileEditorService;
            _layerEditorService = layerEditorService;
            _surfaceService = surfaceService;
            _settingsService = settingsService;
            _profileLayerVmFactory = profileLayerVmFactory;

            Execute.OnUIThreadSync(() =>
            {
                PanZoomViewModel = new PanZoomViewModel {LimitToZero = false};

                CanvasViewModels = new BindableCollection<CanvasViewModel>();
                Devices = new BindableCollection<ArtemisDevice>();
                DimmedLeds = new BindableCollection<ArtemisLed>();
                SelectedLeds = new BindableCollection<ArtemisLed>();
            });

            ApplySurfaceConfiguration(_surfaceService.ActiveSurface);
            ApplyActiveProfile();
            ActivateToolByIndex(0);

            eventAggregator.Subscribe(this);
        }


        public bool IsInitializing { get; private set; }

        public PanZoomViewModel PanZoomViewModel { get; set; }

        public BindableCollection<CanvasViewModel> CanvasViewModels { get; set; }
        public BindableCollection<ArtemisDevice> Devices { get; set; }
        public BindableCollection<ArtemisLed> DimmedLeds { get; set; }
        public BindableCollection<ArtemisLed> SelectedLeds { get; set; }

        public PluginSetting<bool> HighlightSelectedLayer { get; set; }
        public PluginSetting<bool> PauseRenderingOnFocusLoss { get; set; }

        public VisualizationToolViewModel ActiveToolViewModel
        {
            get => _activeToolViewModel;
            set
            {
                // Remove the tool from the canvas
                if (_activeToolViewModel != null)
                {
                    lock (CanvasViewModels)
                    {
                        CanvasViewModels.Remove(_activeToolViewModel);
                        NotifyOfPropertyChange(() => CanvasViewModels);
                    }
                }

                // Set the new tool
                _activeToolViewModel = value;
                // Add the new tool to the canvas
                if (_activeToolViewModel != null)
                {
                    lock (CanvasViewModels)
                    {
                        CanvasViewModels.Add(_activeToolViewModel);
                        NotifyOfPropertyChange(() => CanvasViewModels);
                    }
                }
            }
        }

        public int ActiveToolIndex
        {
            get => _activeToolIndex;
            set
            {
                if (_activeToolIndex != value)
                {
                    _activeToolIndex = value;
                    ActivateToolByIndex(value);
                    NotifyOfPropertyChange(() => ActiveToolIndex);
                }
            }
        }

        protected override void OnInitialActivate()
        {
            HighlightSelectedLayer = _settingsService.GetSetting("ProfileEditor.HighlightSelectedLayer", true);
            PauseRenderingOnFocusLoss = _settingsService.GetSetting("ProfileEditor.PauseRenderingOnFocusLoss", true);

            HighlightSelectedLayer.SettingChanged += HighlightSelectedLayerOnSettingChanged;
            _surfaceService.ActiveSurfaceConfigurationSelected += OnActiveSurfaceConfigurationSelected;
            _profileEditorService.ProfileSelected += OnProfileSelected;
            _profileEditorService.ProfileElementSelected += OnProfileElementSelected;
            _profileEditorService.SelectedProfileElementUpdated += OnSelectedProfileElementUpdated;

            base.OnInitialActivate();
        }

        protected override void OnClose()
        {
            HighlightSelectedLayer.SettingChanged -= HighlightSelectedLayerOnSettingChanged;
            _surfaceService.ActiveSurfaceConfigurationSelected -= OnActiveSurfaceConfigurationSelected;
            _profileEditorService.ProfileSelected -= OnProfileSelected;
            _profileEditorService.ProfileElementSelected -= OnProfileElementSelected;
            _profileEditorService.SelectedProfileElementUpdated -= OnSelectedProfileElementUpdated;

            HighlightSelectedLayer.Save();
            PauseRenderingOnFocusLoss.Save();

            base.OnClose();
        }

        private void OnActiveSurfaceConfigurationSelected(object sender, SurfaceConfigurationEventArgs e)
        {
            ApplySurfaceConfiguration(e.Surface);
        }

        private void ApplyActiveProfile()
        {
            Execute.PostToUIThread(() =>
            {
                lock (CanvasViewModels)
                {
                    var layerViewModels = CanvasViewModels.Where(vm => vm is ProfileLayerViewModel).Cast<ProfileLayerViewModel>().ToList();
                    var layers = _profileEditorService.SelectedProfile?.GetAllLayers() ?? new List<Layer>();

                    // Add new layers missing a VM
                    foreach (var layer in layers)
                    {
                        if (layerViewModels.All(vm => vm.Layer != layer))
                            CanvasViewModels.Add(_profileLayerVmFactory.Create(layer));
                    }

                    // Remove layers that no longer exist
                    var toRemove = layerViewModels.Where(vm => !layers.Contains(vm.Layer));
                    foreach (var profileLayerViewModel in toRemove)
                    {
                        profileLayerViewModel.Dispose();
                        CanvasViewModels.Remove(profileLayerViewModel);
                    }
                }
            });
        }

        private void ApplySurfaceConfiguration(ArtemisSurface surface)
        {
            Devices.Clear();
            Devices.AddRange(surface.Devices.OrderBy(d => d.ZIndex));
        }

        private void UpdateLedsDimStatus()
        {
            DimmedLeds.Clear();
            if (HighlightSelectedLayer.Value && _profileEditorService.SelectedProfileElement is Layer layer)
                DimmedLeds.AddRange(layer.Leds);
        }

        #region Buttons

        private void ActivateToolByIndex(int value)
        {
            // Consider using DI if dependencies start to add up
            switch (value)
            {
                case 0:
                    ActiveToolViewModel = new ViewpointMoveToolViewModel(this, _profileEditorService);
                    break;
                case 1:
                    ActiveToolViewModel = new EditToolViewModel(this, _profileEditorService, _layerEditorService);
                    break;
                case 2:
                    ActiveToolViewModel = new SelectionToolViewModel(this, _profileEditorService);
                    break;
                case 3:
                    ActiveToolViewModel = new SelectionRemoveToolViewModel(this, _profileEditorService);
                    break;
            }

            ActiveToolIndex = value;
        }

        public void ResetZoomAndPan()
        {
            PanZoomViewModel.Reset();
        }

        #endregion

        #region Mouse

        public void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((IInputElement) sender).CaptureMouse();
            ActiveToolViewModel?.MouseDown(sender, e);
        }

        public void CanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            ((IInputElement) sender).ReleaseMouseCapture();
            ActiveToolViewModel?.MouseUp(sender, e);
        }

        public void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            ActiveToolViewModel?.MouseMove(sender, e);
        }

        public void CanvasMouseWheel(object sender, MouseWheelEventArgs e)
        {
            PanZoomViewModel.ProcessMouseScroll(sender, e);
            ActiveToolViewModel?.MouseWheel(sender, e);
        }

        #endregion

        #region Context menu actions

        public bool CanApplyToLayer { get; set; }

        public void CreateLayer()
        {
        }

        public void ApplyToLayer()
        {
            if (!(_profileEditorService.SelectedProfileElement is Layer layer))
                return;

            layer.ClearLeds();
            layer.AddLeds(SelectedLeds);

            _profileEditorService.UpdateSelectedProfileElement();
        }

        public void SelectAll()
        {
            SelectedLeds.Clear();
            SelectedLeds.AddRange(Devices.SelectMany(d => d.Leds));
        }

        public void InverseSelection()
        {
            var current = SelectedLeds.ToList();
            SelectedLeds.Clear();
            SelectedLeds.AddRange(Devices.SelectMany(d => d.Leds).Except(current));
        }

        public void ClearSelection()
        {
            SelectedLeds.Clear();
        }

        #endregion

        #region Event handlers

        private void HighlightSelectedLayerOnSettingChanged(object sender, EventArgs e)
        {
            UpdateLedsDimStatus();
        }

        private void OnProfileSelected(object sender, EventArgs e)
        {
            ApplyActiveProfile();
        }

        private void OnProfileElementSelected(object sender, EventArgs e)
        {
            UpdateLedsDimStatus();
            CanApplyToLayer = _profileEditorService.SelectedProfileElement is Layer;
        }

        private void OnSelectedProfileElementUpdated(object sender, EventArgs e)
        {
            ApplyActiveProfile();
            UpdateLedsDimStatus();
            CanApplyToLayer = _profileEditorService.SelectedProfileElement is Layer;
        }

        public void Handle(MainWindowFocusChangedEvent message)
        {
            // if (PauseRenderingOnFocusLoss == null || ScreenState != ScreenState.Active)
            //     return;
            //
            // try
            // {
            //     if (PauseRenderingOnFocusLoss.Value && !message.IsFocused)
            //         _updateTrigger.Stop();
            //     else if (PauseRenderingOnFocusLoss.Value && message.IsFocused)
            //         _updateTrigger.Start();
            // }
            // catch (NullReferenceException)
            // {
            //     // TODO: Remove when fixed in RGB.NET, or avoid double stopping
            // }
        }

        public void Handle(MainWindowKeyEvent message)
        {
            if (message.KeyDown)
            {
                if (ActiveToolIndex != 0)
                {
                    _previousTool = ActiveToolIndex;
                    if ((message.EventArgs.Key == Key.LeftCtrl || message.EventArgs.Key == Key.RightCtrl) && message.EventArgs.IsDown)
                        ActivateToolByIndex(0);
                }

                ActiveToolViewModel?.KeyDown(message.EventArgs);
            }
            else
            {
                if ((message.EventArgs.Key == Key.LeftCtrl || message.EventArgs.Key == Key.RightCtrl) && message.EventArgs.IsUp)
                    ActivateToolByIndex(_previousTool);

                ActiveToolViewModel?.KeyUp(message.EventArgs);
            }
        }

        #endregion

        public List<ArtemisLed> GetLedsInRectangle(Rect selectedRect)
        {
            return Devices.SelectMany(d => d.Leds)
                .Where(led => led.RgbLed.AbsoluteLedRectangle.ToWindowsRect(1).IntersectsWith(selectedRect))
                .ToList();
        }
    }
}