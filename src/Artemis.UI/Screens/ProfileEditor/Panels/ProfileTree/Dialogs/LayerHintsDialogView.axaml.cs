﻿using Artemis.UI.Shared;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace Artemis.UI.Screens.ProfileEditor.ProfileTree.Dialogs;

public class LayerHintsDialogView : ReactiveCoreWindow<LayerHintsDialogViewModel>
{
    public LayerHintsDialogView()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}