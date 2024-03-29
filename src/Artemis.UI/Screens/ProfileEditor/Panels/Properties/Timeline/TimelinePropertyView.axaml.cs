using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Artemis.UI.Screens.ProfileEditor.Properties.Timeline;

public class TimelinePropertyView : ReactiveUserControl<ITimelinePropertyViewModel>
{
    public TimelinePropertyView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}