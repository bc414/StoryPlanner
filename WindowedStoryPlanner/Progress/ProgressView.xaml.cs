using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner;

public partial class ProgressView : UserControl
{
    public ProgressView()
    {
        InitializeComponent();

        // Same reasoning as PropertyGapsView: rebuild on show rather than subscribing. Here it
        // also avoids the cost the snapshot design existed for — a live subscription would
        // recount every subject on every note edit, across thousands of notes, while this
        // recounts once when you actually look at it.
        IsVisibleChanged += (_, e) =>
        {
            if (e.NewValue is true && DataContext is ProgressViewModel vm)
                vm.Rebuild();
        };
    }
}
