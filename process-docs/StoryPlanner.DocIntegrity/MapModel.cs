using System.Text.RegularExpressions;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// The rows of the three tables the skill obeys (SKILL.md § Schema): Activities in the
/// router, Processes at the head of each activity file, Artifacts in <c>artifacts.md</c>. The
/// columns are the schema, the rows are in flux. Every row carries its file and line so a
/// finding can name where it came from.
/// </summary>
public sealed record ActivityRow(
    string Id,
    IReadOnlyList<string> Enables,
    string Description,
    string File,
    int Line);

public sealed record ProcessRow(
    string Id,
    string Activity,
    string Mode,
    IReadOnlyList<string> Instruments,
    IReadOnlyList<string> Reads,
    IReadOnlyList<string> Writes,
    string State,
    string Description,
    string File,
    int Line)
{
    /// <summary>An artifact named as an instrument counts as read (§ Schema).</summary>
    public bool ReadsOrInstruments(string artifactId)
        => Reads.Contains(artifactId) || Instruments.Contains(artifactId);
}

public sealed record ArtifactRow(
    string Id,
    string Path,
    string Mutation,
    string Format,
    string Description,
    string File,
    int Line);

public sealed record SkillDocument(
    string SkillFolder,
    IReadOnlyList<ActivityRow> Activities,
    IReadOnlyList<ProcessRow> Processes,
    IReadOnlyList<ArtifactRow> Artifacts,
    IReadOnlyList<string> OrphanActivityFiles)
{
    public string SkillPath => System.IO.Path.Combine(SkillFolder, "SKILL.md");
    public string ArtifactsPath => System.IO.Path.Combine(SkillFolder, "artifacts.md");
    public string ActivityPath(string activityId) => System.IO.Path.Combine(SkillFolder, activityId + ".md");

    public IEnumerable<ProcessRow> ProcessesOf(string activityId)
        => Processes.Where(p => p.Activity == activityId);

    public bool HasArtifact(string id) => Artifacts.Any(a => a.Id == id);

    public ArtifactRow? Artifact(string id) => Artifacts.FirstOrDefault(a => a.Id == id);
}

/// <summary>
/// A finding's weight. <see cref="Failure"/> sets exit 1. <see cref="Info"/> never does — it
/// carries the reports that are not verdicts (artifacts written by nothing, free-named
/// instruments, declared values no row uses). <see cref="Vacuous"/> is a check whose subject
/// set is empty: it is reported as vacuous rather than silently passing, because "no process
/// reads candidates" and "every path from candidates to a hypothesis write passes an hitl
/// process" are different facts.
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

/// <summary>The closed sets of SKILL.md § Schema.</summary>
public static class ClosedSets
{
    public static readonly string[] Modes = ["hitl", "session", "agent"];
    public static readonly string[] States = ["built", "specified"];
    public static readonly string[] Mutations = ["in-place", "succeeded", "append", "frozen"];

    /// <summary>Ids are lowercase slugs, unique across all three tables.</summary>
    public static readonly Regex IdPattern = new("^[a-z0-9-]+$", RegexOptions.Compiled);

    public const string Hitl = "hitl";
    public const string Frozen = "frozen";
}

/// <summary>
/// The artifact ids the checks in SKILL.md § Schema name by id, and the files in a skill
/// folder that are companions without being activities. These are ids in the Artifacts
/// table, not code: a rule whose id is absent from the table is reported vacuous, never
/// passing.
/// </summary>
public static class WellKnown
{
    public const string Candidates = "candidates";
    public const string QuestionList = "question-list";
    public const string Studies = "studies";
    public const string Codebook = "codebook";
    public const string Calibration = "calibration";
    public const string VerificationArtifact = "verification-artifact";
    public const string HypothesisStatus = "hypothesis-status";
    public const string HypothesisIndex = "hypothesis-index";
    public const string LeadsArtifact = "leads-artifact";
    public const string Corpora = "corpora";

    /// <summary>The three artifacts one hypothesis file holds; a write to any is a hypothesis write.</summary>
    public static readonly string[] HypothesisArtifacts =
        ["hypothesis-statement", "hypothesis-record", "hypothesis-status"];

    /// <summary>Files in the skill folder that are not activity files (SKILL.md § Companions).</summary>
    public static readonly string[] NonActivityFiles =
        ["SKILL.md", "artifacts.md", "map.md", "state.md", "CORPORA.md"];
}
