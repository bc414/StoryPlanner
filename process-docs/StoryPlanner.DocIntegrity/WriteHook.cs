using System.Text.Json;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// The fields of a Claude Code tool-event payload the write hook reads. The harness writes the
/// whole event as JSON on the hook's stdin; only the tool name and the written path matter
/// here, and a tool with no <c>file_path</c> (a notebook edit, a shell command) yields null.
/// </summary>
public sealed record HookPayload(string HookEventName, string ToolName, string? FilePath, string? Cwd)
{
    public static HookPayload Parse(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        string? filePath = null;
        if (root.TryGetProperty("tool_input", out var input) && input.ValueKind == JsonValueKind.Object
            && input.TryGetProperty("file_path", out var fp) && fp.ValueKind == JsonValueKind.String)
            filePath = fp.GetString();
        return new HookPayload(
            Str(root, "hook_event_name") ?? "",
            Str(root, "tool_name") ?? "",
            filePath,
            Str(root, "cwd"));
    }

    static string? Str(JsonElement e, string name)
        => e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
}

/// <summary>
/// Which folders the write hook governs, decided by shape and never by name: a folder at
/// <c>.claude/skills/&lt;name&gt;/</c> whose <c>SKILL.md</c> holds an Artifacts table is a
/// revision-2 method skill and its tables are what <c>check</c> holds to the schema. The live
/// revision-1 skill has no such table and is therefore not governed, which is right: it is
/// not edited before the swap, and the hook has nothing to check it against.
/// </summary>
public static class GovernedSkill
{
    public const string SkillsFolder = "skills";
    public const string ClaudeFolder = ".claude";

    /// <summary>Every governed skill folder under a repository root, by the same shape rule.</summary>
    public static IEnumerable<string> All(string repoRoot)
    {
        var skills = Path.Combine(repoRoot, ClaudeFolder, SkillsFolder);
        if (!Directory.Exists(skills)) yield break;
        foreach (var dir in Directory.GetDirectories(skills).OrderBy(d => d, StringComparer.Ordinal))
            if (IsGoverned(new DirectoryInfo(dir)))
                yield return dir;
    }

    /// <summary>The governed skill folder that is, or contains, <paramref name="path"/>, or null.</summary>
    public static string? Locate(string path)
    {
        var full = Path.GetFullPath(path);
        var dir = Directory.Exists(full) ? new DirectoryInfo(full) : new DirectoryInfo(full).Parent;
        while (dir is not null)
        {
            if (IsGoverned(dir)) return dir.FullName;
            dir = dir.Parent;
        }
        return null;
    }

    /// <summary>The repository root a governed skill folder sits under, by the same shape: three levels up.</summary>
    public static string RepoRootOf(string skillFolder)
        => Path.GetFullPath(Path.Combine(skillFolder, "..", "..", ".."));

    static bool IsGoverned(DirectoryInfo dir)
    {
        var parent = dir.Parent;
        var grand = parent?.Parent;
        return parent is not null && grand is not null
               && string.Equals(parent.Name, SkillsFolder, StringComparison.OrdinalIgnoreCase)
               && string.Equals(grand.Name, ClaudeFolder, StringComparison.OrdinalIgnoreCase)
               && SkillReader.HasArtifactsTable(Path.Combine(dir.FullName, "SKILL.md"));
    }
}

public enum HookOutcomeKind
{
    /// <summary>Not a governed write, or a governed write that checks clean: nothing to say.</summary>
    Silent,
    /// <summary>A governed write left its folder or its file failing check; the message is the report.</summary>
    Failed,
    /// <summary>The payload could not be read; the message says why, so the gap is visible.</summary>
    PayloadError,
}

public sealed record HookOutcome(HookOutcomeKind Kind, int ExitCode, string Message, IReadOnlyList<string> Regenerated)
{
    public static readonly HookOutcome Nothing = new(HookOutcomeKind.Silent, WriteHook.Silent, "", []);
}

/// <summary>
/// The PostToolUse hook on Edit and Write: after a session writes a file, the hook does what
/// the write implies. A file inside a governed skill folder has the folder's shape checked and,
/// if its class has a schema, that too; a governed file anywhere else has its schema checked.
/// A failing report goes back to the session as feedback: the harness shows a hook's stderr to
/// the model only on exit code 2, so that is the code for feedback; every other case is exit 0
/// and silence. On a pass the governing folder's generated files are rewritten, so
/// <c>map.md</c> is a function of the tables and <c>state.md</c> of the governed files at every
/// moment, and a session, which is denied those files by path, never has to remember them.
///
/// This is enforcement at the boundary where the model's text becomes a file. It sees Edit and
/// Write and nothing else: a write through the shell never reaches it, which is why CLAUDE.md
/// routes file content through the file tools. The check is the same one the CLI runs; the
/// hook only decides when it runs.
/// </summary>
public static class WriteHook
{
    public const int Silent = 0;
    public const int Feedback = 2;

    public static HookOutcome Run(string payloadJson, Func<string, ValidationReport>? validate = null, bool regenerate = true)
    {
        HookPayload payload;
        try
        {
            payload = HookPayload.Parse(payloadJson);
        }
        catch (JsonException ex)
        {
            return new HookOutcome(HookOutcomeKind.PayloadError, Feedback,
                $"DocIntegrity hook: could not read the event payload ({ex.Message}). The write was not checked.", []);
        }
        return Run(payload, validate, regenerate);
    }

    public static HookOutcome Run(HookPayload payload, Func<string, ValidationReport>? validate = null, bool regenerate = true)
    {
        if (payload.FilePath is null) return HookOutcome.Nothing;
        var check = validate ?? Validator.Validate;

        var folder = GovernedSkill.Locate(payload.FilePath);
        if (folder is not null)
        {
            var report = check(folder);
            if (!report.Passed) return FolderFailed(folder, payload.FilePath, report);

            HookOutcome? refused = null;
            var regenerated = regenerate ? Regenerate(folder, report, out refused) : [];
            if (refused is not null) return refused;

            // A file in the skill folder may also be a governed file with a schema of its own (the corpora file).
            var inside = ArtifactScope.Locate(GovernedSkill.RepoRootOf(folder), payload.FilePath);
            return inside is null
                ? new HookOutcome(HookOutcomeKind.Silent, Silent, "", regenerated)
                : CheckGoverned(inside, payload.FilePath, regenerated);
        }

        // Outside every skill folder: the file's schema, and on a pass the governing folder's
        // generated files, so state.md follows a governed write the moment it checks clean.
        var governed = ArtifactScope.Locate(payload.FilePath);
        if (governed is null) return HookOutcome.Nothing;

        var outcome = CheckGoverned(governed, payload.FilePath, []);
        if (outcome.Kind != HookOutcomeKind.Silent || !regenerate) return outcome;

        var folderReport = check(governed.SkillFolder);
        if (!folderReport.Passed) return outcome; // the folder's own writes report that; its generated files wait for a pass
        var written = Regenerate(governed.SkillFolder, folderReport, out var refusedAfter);
        return refusedAfter ?? outcome with { Regenerated = written };
    }

    static IReadOnlyList<string> Regenerate(string folder, ValidationReport report, out HookOutcome? refused)
    {
        refused = null;
        try
        {
            return Render.Write(GovernedSkill.RepoRootOf(folder), folder, SkillReader.Read(folder), report, forced: false);
        }
        catch (MapFormatException ex)
        {
            refused = new HookOutcome(HookOutcomeKind.Failed, Feedback,
                $"DocIntegrity: the tables check clean but render refuses ({ex.RuleId}): {ex.Message}", []);
            return [];
        }
    }

    static HookOutcome FolderFailed(string folder, string filePath, ValidationReport report)
    {
        var folderName = Path.GetFileName(folder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        var written = Path.GetFileName(filePath);
        var message =
            $"DocIntegrity: after the write to {written}, the skill folder {folderName} fails check " +
            $"({report.Failures} failure(s)):\n" +
            ReportText.FormatFailures(report) +
            "Fix the row and its prose together, then re-run check until it passes:\n" +
            $"  dotnet run --project process-docs/StoryPlanner.DocIntegrity -- check {DisplayPath(folder)}\n" +
            "Do not work around this check by writing through the shell; every write to this folder goes " +
            "through Edit or Write so the check sees it.";
        return new HookOutcome(HookOutcomeKind.Failed, Feedback, message, []);
    }

    /// <summary>
    /// The governed-file half of the hook: the written path already resolved against the
    /// artifacts table; its class's checker runs and the failures are the feedback.
    /// </summary>
    static HookOutcome CheckGoverned(GovernedFile governed, string filePath, IReadOnlyList<string> regenerated)
    {
        IReadOnlyList<Finding> findings;
        try { findings = governed.Checker(governed.Context, filePath); }
        catch (MapFormatException ex) { findings = [Finding.Fail(ex.RuleId, Path.GetFileName(filePath), ex.Message)]; }

        var report = new ValidationReport(findings);
        if (report.Passed) return new HookOutcome(HookOutcomeKind.Silent, Silent, "", regenerated);

        var rel = Path.GetRelativePath(governed.RepoRoot, Path.GetFullPath(filePath)).Replace('\\', '/');
        var message =
            $"DocIntegrity: after the write to {Path.GetFileName(filePath)}, it fails the schema of `{governed.Row.Id}` " +
            $"({report.Failures} failure(s)):\n" +
            ReportText.FormatFailures(report) +
            $"The schema is {SkillReader.SchemasFolder}/{governed.Row.Schema}.md. Fix the file, then re-run check until it passes:\n" +
            $"  dotnet run --project process-docs/StoryPlanner.DocIntegrity -- check {rel}\n" +
            "Do not work around this check by writing through the shell; every write to a governed file goes " +
            "through Edit or Write so the check sees it.";
        return new HookOutcome(HookOutcomeKind.Failed, Feedback, message, regenerated);
    }

    /// <summary>The failing folder as the session would type it: repo-relative inside a repository, absolute otherwise.</summary>
    public static string DisplayPath(string folder)
    {
        var root = RepoLocator.FindRoot(folder);
        if (root is null) return folder;
        var relative = Path.GetRelativePath(root, folder);
        return relative.Replace(Path.DirectorySeparatorChar, '/');
    }
}
