﻿using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using FluentAvalonia.Interop;
using FluentAvalonia.UI.Controls;
using ReactiveUI;

namespace Artemis.UI
{
    /// <summary>
    /// A ReactiveUI <see cref="Window"/> that implements the <see cref="IViewFor{TViewModel}"/> interface and will
    /// activate your ViewModel automatically if the view model implements <see cref="IActivatableViewModel"/>. When
    /// the DataContext property changes, this class will update the ViewModel property with the new DataContext value,
    /// and vice versa.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel type.</typeparam>
    public class ReactiveCoreWindow<TViewModel> : CoreWindow, IViewFor<TViewModel> where TViewModel : class
    {
        public static readonly StyledProperty<TViewModel?> ViewModelProperty = AvaloniaProperty
            .Register<ReactiveCoreWindow<TViewModel>, TViewModel?>(nameof(ViewModel));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveCoreWindow{TViewModel}"/> class.
        /// </summary>
        public ReactiveCoreWindow()
        {
            // This WhenActivated block calls ViewModel's WhenActivated
            // block if the ViewModel implements IActivatableViewModel.
            this.WhenActivated(disposables => { });
            this.GetObservable(DataContextProperty).Subscribe(OnDataContextChanged);
            this.GetObservable(ViewModelProperty).Subscribe(OnViewModelChanged);

            // TODO Remove
            Win32Interop.OSVERSIONINFOEX version = default;
            Win32Interop.RtlGetVersion(ref version);
            if (version.MajorVersion == 10)
                PseudoClasses.Add(":windows10");
        }
        
        /// <summary>
        /// The ViewModel.
        /// </summary>
        public TViewModel? ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel?)value;
        }

        private void OnDataContextChanged(object? value)
        {
            if (value is TViewModel viewModel)
            {
                ViewModel = viewModel;
            }
            else
            {
                ViewModel = null;
            }
        }

        private void OnViewModelChanged(object? value)
        {
            if (value == null)
            {
                ClearValue(DataContextProperty);
            }
            else if (DataContext != value)
            {
                DataContext = value;
            }
        }
    }
}