namespace WindowedStoryPlanner;

/// <summary>
/// Note-state rollup for one subject, with a per-track breakdown. Plain counts — the columns are
/// sortable in the grid, and no ordering here implies anything about what to work on next.
/// </summary>
public sealed class ProgressSubjectRow
{
    public required int SubjectId { get; init; }
    public required string Name { get; init; }
    public required string SubjectType { get; init; }
    public required int Confirmed { get; init; }
    public required int Unset { get; init; }
    public required int Flagged { get; init; }
    public required IReadOnlyList<ProgressTrackRow> Tracks { get; init; }

    public int Total => Confirmed + Unset + Flagged;
}

/// <summary>Note-state rollup for one track within one subject.</summary>
public sealed class ProgressTrackRow
{
    public required string TrackName { get; init; }
    public required int Confirmed { get; init; }
    public required int Unset { get; init; }
    public required int Flagged { get; init; }

    public int Total => Confirmed + Unset + Flagged;
}
