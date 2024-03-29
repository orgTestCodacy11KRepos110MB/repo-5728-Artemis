using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Artemis.UI.Screens.Debugger.Performance;

public class PerformanceDebugProfilerView : UserControl
{
    public PerformanceDebugProfilerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}