using Artemis.UI.Shared;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace Artemis.UI.Windows.Screens.Update;

public class UpdateDialogView : ReactiveCoreWindow<UpdateDialogViewModel>
{
    public UpdateDialogView()
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