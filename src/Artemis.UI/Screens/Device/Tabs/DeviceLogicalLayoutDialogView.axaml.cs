﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;

namespace Artemis.UI.Screens.Device;

public class DeviceLogicalLayoutDialogView : ReactiveUserControl<DeviceLogicalLayoutDialogViewModel>
{
    private readonly AutoCompleteBox _autoCompleteBox;

    public DeviceLogicalLayoutDialogView()
    {
        InitializeComponent();

        _autoCompleteBox = this.Get<AutoCompleteBox>("RegionsAutoCompleteBox");
        _autoCompleteBox.ItemFilter += SearchRegions;

        Dispatcher.UIThread.InvokeAsync(DelayedAutoFocus);
    }

    private async Task DelayedAutoFocus()
    {
        await Task.Delay(200);
        _autoCompleteBox.Focus();
        _autoCompleteBox.PopulateComplete();
    }

    private bool SearchRegions(string search, object item)
    {
        if (item is not RegionInfo regionInfo)
            return false;

        return regionInfo.EnglishName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
               regionInfo.NativeName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
               regionInfo.TwoLetterISORegionName.Contains(search, StringComparison.OrdinalIgnoreCase);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}