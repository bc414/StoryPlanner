using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner;

public partial class PropertyGapsView : UserControl
{
    public PropertyGapsView()
    {
        InitializeComponent();

        // Rebuild whenever the tab is switched to. The report is derived from several collections
        // at once — property definitions, their allowed values, the assignments, and the owners
        // themselves — so subscribing to each would be four sets of CollectionChanged bookkeeping
        // (and the service collections are REASSIGNED on project load, so each would need
        // re-subscribing too). Recomputing on show costs nothing while the tab is hidden and
        // cannot go stale for any reason, including ones added later.
        IsVisibleChanged += (_, e) =>
        {
            if (e.NewValue is true && DataContext is PropertyGapsViewModel vm)
                vm.Rebuild();
        };
    }
}
