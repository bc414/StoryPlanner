using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>One owner that has no value assigned on a given narrative property.</summary>
public sealed class PropertyGapRow
{
    public required int OwnerId { get; init; }
    public required OwnerType OwnerType { get; init; }
    public required string OwnerLabel { get; init; }
}

/// <summary>
/// One narrative property and the owners currently missing a value on it. Ordering inside
/// <see cref="Gaps"/> is the library's own (by name) — never by anything that would rank one
/// owner as more worth doing than another.
/// </summary>
public sealed class PropertyGapGroup
{
    public required int PropertyDefinitionId { get; init; }
    public required string PropertyName { get; init; }
    public required string Scope { get; init; }
    public required string GatingPhase { get; init; }
    public required int TotalOwners { get; init; }
    public required IReadOnlyList<PropertyGapRow> Gaps { get; init; }

    public string Header => $"{PropertyName} — {Gaps.Count} of {TotalOwners} unset";
}
