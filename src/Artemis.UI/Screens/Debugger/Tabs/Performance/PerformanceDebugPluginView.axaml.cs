using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Artemis.UI.Screens.Debugger.Performance;

public class PerformanceDebugPluginView : UserControl
{
    public PerformanceDebugPluginView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}