using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Artemis.UI.Screens.ProfileEditor.Properties.Tree;

public class TreeGroupView : ReactiveUserControl<TreeGroupViewModel>
{
    public TreeGroupView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (ViewModel != null)
            ViewModel.PropertyGroupViewModel.IsExpanded = !ViewModel.PropertyGroupViewModel.IsExpanded;
    }
}