using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Artemis.UI.Screens.ProfileEditor;

public class ProfileEditorView : ReactiveUserControl<ProfileEditorViewModel>
{
    public ProfileEditorView()
    {
        InitializeComponent();
    }

    #region Overrides of Visual

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Focus();
    }

    #endregion

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void MenuItem_OnSubmenuOpened(object? sender, RoutedEventArgs e)
    {
    }
}