namespace StoryPlanner.SourceTexts;

/// <summary>
/// One addressable unit of source text, keyed to the plan by (WorkName, PartCode) — the same
/// pair a citation renders as "FiM·S3E01". Ids are deliberately NOT used: sources.db is a
/// separate file from the .storyplan, and a reseed there must not silently re-point text here.
///
/// UnitKey subdivides a Part where the source is itself a list of keyed entries — every EaW
/// localisation key is one unit, so a citation can name "EQS_Crystal_Fair_desc" rather than
/// "everything Equestria says". Prose and transcripts leave it empty: one unit per Part.
/// </summary>
public sealed record SourceTextUnit
{
    public required string WorkName { get; init; }

    /// <summary>Empty = the text belongs to the Work as a whole (a Work with no Parts).</summary>
    public required string PartCode { get; init; }

    public string UnitKey { get; init; } = "";
    public string UnitLabel { get; init; } = "";
    public required string Kind { get; init; }
    public int OrderIndex { get; init; }
    public required string Body { get; init; }
    public string SourceRef { get; init; } = "";
}

public static class SourceTextKind
{
    public const string Transcript = "transcript";
    public const string Prose = "prose";
    public const string Flavor = "flavor";
}
