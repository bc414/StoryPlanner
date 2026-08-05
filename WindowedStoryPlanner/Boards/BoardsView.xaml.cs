using System.Windows.Controls;

namespace WindowedStoryPlanner;

/// <summary>
/// Recomputes on tab-show, the pattern PropertyGapsView documents: the views derive from several
/// collections at once — boards, properties, allowed values, assignments, relations, subjects —
/// and the service ones are reassigned on project load, so subscribing to each would be several
/// sets of bookkeeping that a later input would silently outgrow.
/// </summary>
public partial class BoardsView : UserControl
{
    public BoardsView()
    {
        InitializeComponent();

        IsVisibleChanged += (_, e) =>
        {
            if (e.NewValue is true && DataContext is BoardsViewModel vm) vm.Rebuild();
        };
    }
}
