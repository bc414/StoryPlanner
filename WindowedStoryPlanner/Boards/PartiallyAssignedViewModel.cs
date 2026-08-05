namespace WindowedStoryPlanner;

/// <summary>
/// A subject left out of exact-match grouping because it lacks a value on at least one of the
/// board's properties.
///
/// Listed rather than hidden, and deliberately not matched on its known values: "these two agree
/// on all five" must not be said when three are unknown. An incomplete subject silently absent
/// from the view is how you fail to notice it is incomplete — but the count is a statement of
/// fact, not a to-do, and nothing here suggests which value it should get.
/// </summary>
public sealed class PartiallyAssignedViewModel
{
    public required SubjectCardViewModel Card { get; init; }
    public required int UnsetCount { get; init; }

    public string UnsetLabel => UnsetCount == 1 ? "1 unset" : $"{UnsetCount} unset";
}
