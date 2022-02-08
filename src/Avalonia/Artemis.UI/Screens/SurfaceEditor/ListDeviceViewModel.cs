﻿using Artemis.Core;
using Artemis.UI.Shared;
using ReactiveUI;
using SkiaSharp;

namespace Artemis.UI.Screens.SurfaceEditor
{
    public class ListDeviceViewModel : ViewModelBase
    {
        private SKColor _color;
        private bool _isSelected;
        
        public ListDeviceViewModel(ArtemisDevice device)
        {
            Device = device;
        }

        public ArtemisDevice Device { get; }
        
        public bool IsSelected
        {
            get => _isSelected;
            set => RaiseAndSetIfChanged(ref _isSelected, value);    
        }

        public SKColor Color
        {
            get => _color;
            set => RaiseAndSetIfChanged(ref _color, value);
        }   
    }
}