using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Artemis.UI.Screens.Plugins;

public class PluginFeatureView : ReactiveUserControl<PluginFeatureViewModel>
{
    public PluginFeatureView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}