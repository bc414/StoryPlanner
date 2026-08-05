namespace WindowedStoryPlanner;

/// <summary>
/// One node of the subject tree: a card, its depth, and whether the walk stopped here.
///
/// A flat, pre-order list with an indent rather than a WPF TreeView with a
/// HierarchicalDataTemplate — the tree is rebuilt wholesale on every selection change, it is
/// never edited in place, and a flat list keeps it the same ItemsControl shape as everything else
/// in this layer (there is no TreeView anywhere in the app).
/// </summary>
public sealed class SubjectTreeNodeViewModel
{
    public required SubjectCardViewModel Card { get; init; }
    public required int Depth { get; init; }

    /// <summary>
    /// This subject already appeared on its own line of ancestry, so the walk stopped. Marked
    /// rather than hidden: a cycle is data worth seeing, and PlanIntegrity reports it separately
    /// as subjectrelation.cycle when the relation claims to be a hierarchy.
    /// </summary>
    public required bool StopsOnCycle { get; init; }

    public System.Windows.Thickness Indent => new(Depth * 22, 0, 0, 0);
}
