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
/// <c>.claude/skills/&lt;name&gt;/</c> that holds both <c>SKILL.md</c> and <c>artifacts.md</c>
/// is a revision-2 method skill and its tables are what <c>validate</c> checks. The live
/// revision-1 skill has no artifacts table and is therefore not governed, which is right: it
/// is not edited before the swap, and the hook has nothing to check it against.
/// </summary>
public static class GovernedSkill
{
    public const string SkillsFolder = "skills";
    public const string ClaudeFolder = ".claude";

    /// <summary>The governed skill folder containing <paramref name="filePath"/>, or null.</summary>
    public static string? Locate(string filePath)
    {
        var full = Path.GetFullPath(filePath);
        var dir = new DirectoryInfo(full).Parent;
        while (dir is not null)
        {
            var parent = dir.Parent;
            var grand = parent?.Parent;
            if (parent is not null && grand is not null
                && string.Equals(parent.Name, SkillsFolder, StringComparison.OrdinalIgnoreCase)
                && string.Equals(grand.Name, ClaudeFolder, StringComparison.OrdinalIgnoreCase)
                && File.Exists(Path.Combine(dir.FullName, "SKILL.md"))
                && File.Exists(Path.Combine(dir.FullName, "artifacts.md")))
                return dir.FullName;
            dir = parent;
        }
        return null;
    }
}

public enum HookOutcomeKind
{
    /// <summary>Not a governed write, or a governed write that validates: nothing to say.</summary>
    Silent,
    /// <summary>A governed write left the folder failing validate; the message is the report.</summary>
    Failed,
    /// <summary>The payload could not be read; the message says why, so the gap is visible.</summary>
    PayloadError,
}

public sealed record HookOutcome(HookOutcomeKind Kind, int ExitCode, string Message, IReadOnlyList<string> Regenerated)
{
    public static readonly HookOutcome Nothing = new(HookOutcomeKind.Silent, WriteHook.Silent, "", []);
}

/// <summary>
/// The PostToolUse hook on Edit and Write: after a session writes a file, if the file lies in a
/// governed skill folder, <c>validate</c> runs over that folder and a failing report goes back
/// to the session as feedback. The harness shows a hook's stderr to the model only on exit
/// code 2, so that is the code for feedback; every other case is exit 0 and silence.
///
/// This is enforcement at the boundary where the model's text becomes a file. It sees Edit and
/// Write and nothing else: a write through the shell never reaches it, which is why CLAUDE.md
/// routes file content through the file tools. The check is the same one the CLI runs; the hook
/// only decides when it runs. On a pass it also renders, so map.md is a function of the tables
/// at every moment and a session, which is denied that file by path, never has to remember it.
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

        var folder = GovernedSkill.Locate(payload.FilePath);
        if (folder is null) return HookOutcome.Nothing;

        var report = (validate ?? Validator.Validate)(folder);
        if (report.Passed)
        {
            if (!regenerate) return HookOutcome.Nothing;
            try
            {
                var regenerated = Render.Write(folder, SkillReader.Read(folder), report, forced: false);
                return new HookOutcome(HookOutcomeKind.Silent, Silent, "", regenerated);
            }
            catch (MapFormatException ex)
            {
                return new HookOutcome(HookOutcomeKind.Failed, Feedback,
                    $"DocIntegrity: the tables validate but render refuses ({ex.RuleId}): {ex.Message}", []);
            }
        }

        var folderName = Path.GetFileName(folder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        var written = Path.GetFileName(payload.FilePath);
        var message =
            $"DocIntegrity: after the write to {written}, the skill folder {folderName} fails validate " +
            $"({report.Failures} failure(s)):\n" +
            ReportText.FormatFailures(report) +
            "Fix the row and its prose together, then re-run validate until it passes:\n" +
            $"  dotnet run --project process-docs/StoryPlanner.DocIntegrity -- validate {DisplayPath(folder)}\n" +
            "Do not work around this check by writing through the shell; every write to this folder goes " +
            "through Edit or Write so the check sees it.";
        return new HookOutcome(HookOutcomeKind.Failed, Feedback, message, []);
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
