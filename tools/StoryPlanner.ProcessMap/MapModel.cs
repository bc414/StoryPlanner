namespace StoryPlanner.ProcessMap;

/// <summary>
/// The rows of the process map. One record per table in
/// <c>.claude/skills/v3-buildout/process-map.md</c>; the columns are the schema, the rows are
/// in flux. Every record carries the source line so a finding can name where it came from.
/// </summary>
public sealed record RootRow(
    string Id,
    string Kind,
    string Text,
    string Source,
    int Line);

public sealed record FileRow(
    string Id,
    string Path,
    string Keep,
    string GovernedBy,
    int Line);

public sealed record ProcessRow(
    string Id,
    string Level,
    string Kind,
    string Text,
    string Actor,
    IReadOnlyList<string> Inputs,
    IReadOnlyList<string> Outputs,
    IReadOnlyList<string> Roots,
    string GovernedBy,
    string State,
    int Line);

public sealed record EdgeRow(
    string From,
    string To,
    string Kind,
    string Label,
    int Line);

public sealed record BootstrapRow(
    string RowId,
    string RetiredBy,
    int Line);

public sealed record ProcessMapDocument(
    IReadOnlyList<RootRow> Roots,
    IReadOnlyList<FileRow> Files,
    IReadOnlyList<ProcessRow> Processes,
    IReadOnlyList<EdgeRow> Edges,
    IReadOnlyList<BootstrapRow> Bootstrap);

/// <summary>
/// A finding's weight. <see cref="Failure"/> sets exit 1. <see cref="Info"/> never does — it
/// carries the reports that are not verdicts (fan-in per governing file, declared enum values
/// no row uses). <see cref="Vacuous"/> is a check whose subject set is empty: it is reported
/// as vacuous rather than silently passing, because "no process writes a .storyplan" and
/// "every path to a .storyplan passes a Brian node" are different facts.
/// </summary>
public enum FindingLevel
{
    Failure,
    Info,
    Vacuous,
}

public sealed record Finding(
    string RuleId,
    string RowId,
    string Message,
    FindingLevel Level)
{
    public static Finding Fail(string ruleId, string rowId, string message)
        => new(ruleId, rowId, message, FindingLevel.Failure);

    public static Finding Info(string ruleId, string rowId, string message)
        => new(ruleId, rowId, message, FindingLevel.Info);

    public static Finding Vacuous(string ruleId, string message)
        => new(ruleId, "—", message, FindingLevel.Vacuous);
}

/// <summary>The closed sets of § Format, plus the two suffix-bearing actor forms.</summary>
public static class ClosedSets
{
    public static readonly string[] RootKinds = ["goal", "rule", "incident", "hypothesis"];
    public static readonly string[] Keeps = ["committed", "gitignored", "regenerable", "outside-repo"];
    public static readonly string[] Levels = ["P", "E", "V", "S", "I", "R", "F", "M"];
    public static readonly string[] ProcessKinds = ["sop", "bootstrap", "reactive"];
    public static readonly string[] States = ["exists", "specified", "unbuilt", "contradictory"];
    public static readonly string[] EdgeKinds = ["flow", "choice", "fork", "join", "optional"];

    /// <summary>Actors taking no suffix.</summary>
    public static readonly string[] BareActors = ["brian", "script", "tool"];

    /// <summary>Actors of the form <c>prefix:suffix</c>, any non-empty suffix.</summary>
    public static readonly string[] SuffixedActors = ["hitl", "agent"];

    public static bool IsActor(string actor)
    {
        if (BareActors.Contains(actor)) return true;
        var colon = actor.IndexOf(':');
        if (colon <= 0 || colon == actor.Length - 1) return false;
        return SuffixedActors.Contains(actor[..colon]);
    }
}
