namespace StoryPlanner.Core;

/// <summary>
/// What kind of entity a SearchHit points at. Distinct from OwnerType — a note hit
/// carries the Note kind itself, not the OwnerType of whatever owns it (that's a
/// separate lookup the caller does with the note's OwnerId/OwnerType).
/// </summary>
public enum SearchHitKind
{
    Subject,
    PlotPoint,
    Chapter,
    Theme,
    SourceMaterial,
    Note
}
