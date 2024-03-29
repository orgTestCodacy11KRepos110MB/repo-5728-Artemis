﻿using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Artemis.UI.Screens.Sidebar;

public class SidebarProfileConfigurationView : ReactiveUserControl<SidebarProfileConfigurationViewModel>
{
    public SidebarProfileConfigurationView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}