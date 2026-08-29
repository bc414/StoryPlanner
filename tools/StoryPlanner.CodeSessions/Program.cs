using System.Text.Json;
using StoryPlanner.CodeSessions;

// Progressive ingest of Claude Code session transcripts into codesessions.db — the
// sealed-but-greppable engineering-provenance archive. NOT served by the MCP server; future
// Claude Code sessions query the db directly (recipes: .claude/skills/code-sessions).
//
//   dotnet run --project tools/StoryPlanner.CodeSessions -- <config.json> [--apply] [--project NAME]
//
// Per-session replace, keyed on the (bytes, mtime) stamp: new and changed files are
// re-extracted; unchanged files are touched; a session whose file has aged off disk is
// RETAINED — the whole point, since Claude Code deletes transcripts after its retention window.

var positional = args.Where(a => !a.StartsWith("--")).ToList();
var apply = args.Contains("--apply");
var purgeStubs = args.Contains("--purge-stubs");
var projectArgIdx = Array.IndexOf(args, "--project");
string? onlyProject = projectArgIdx >= 0 && projectArgIdx + 1 < args.Length ? args[projectArgIdx + 1] : null;
if (onlyProject is not null) positional.Remove(onlyProject);

if (positional.Count != 1)
{
    Console.Error.WriteLine("Usage: dotnet run -- <config.json> [--apply] [--project NAME]");
    return 2;
}

var configPath = positional[0];
if (!File.Exists(configPath))
{
    Console.Error.WriteLine($"Config not found: {configPath}");
    return 2;
}

CodeSessionsConfig config;
try
{
    config = JsonSerializer.Deserialize<CodeSessionsConfig>(
        File.ReadAllText(configPath),
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true, ReadCommentHandling = JsonCommentHandling.Skip })
        ?? throw new JsonException("config deserialized to null");
}
catch (JsonException ex)
{
    Console.Error.WriteLine($"Config unreadable: {ex.Message}");
    return 2;
}
if (string.IsNullOrWhiteSpace(config.ProjectsRoot) || string.IsNullOrWhiteSpace(config.Output) || config.Projects.Count == 0)
{
    Console.Error.WriteLine("Config needs projectsRoot, output, and a non-empty projects include-list.");
    return 2;
}
if (onlyProject is not null && !config.Projects.Contains(onlyProject, StringComparer.OrdinalIgnoreCase))
{
    Console.Error.WriteLine($"--project \"{onlyProject}\" is not in the config's include-list. Configured: " +
                            string.Join(", ", config.Projects));
    return 2;
}

List<string> projects = onlyProject is not null ? [onlyProject] : config.Projects;

const int MinAssistantChars = 200;

Console.WriteLine($"code-sessions ingest ({(apply ? "APPLY" : "DRY RUN")})");
Console.WriteLine($"  root   : {config.ProjectsRoot}");
Console.WriteLine($"  output : {config.Output}");
Console.WriteLine();

using var conn = CodeSessionDb.OpenWrite(config.Output);

if (purgeStubs)
{
    Console.WriteLine($"Purging stub sessions (assistant content < {MinAssistantChars} chars)...");
    using var findCmd = conn.CreateCommand();
    findCmd.CommandText = """
        SELECT s.SessionId FROM Sessions s
        WHERE (SELECT COALESCE(SUM(r.BodyChars), 0) FROM Records r WHERE r.SessionId = s.SessionId AND r.Role = 'assistant') < $min
        """;
    findCmd.Parameters.AddWithValue("$min", MinAssistantChars);
    var stubIds = new List<string>();
    using (var reader = findCmd.ExecuteReader())
        while (reader.Read()) stubIds.Add(reader.GetString(0));

    if (stubIds.Count == 0)
    {
        Console.WriteLine("No stubs found.");
    }
    else
    {
        Console.WriteLine($"Found {stubIds.Count} stub session(s).");
        if (apply)
        {
            CodeSessionDb.DeleteSessions(conn, stubIds);
            Console.WriteLine($"Purged {stubIds.Count} stub session(s). " +
                              $"Database {new FileInfo(config.Output).Length / (1024.0 * 1024.0):F1} MB — " +
                              $"run VACUUM externally to reclaim space.");
        }
        else
        {
            Console.WriteLine("DRY RUN — pass --apply to purge.");
        }
    }

    if (!apply) return 0;
    Console.WriteLine();
}

var stamps = CodeSessionDb.LoadStamps(conn);

// (SessionId, ProjectDir, Kind, ParentSessionId, file path, subagent count) per file found.
var found = new List<(string SessionId, string ProjectDir, string Kind, string? Parent, string Path, int Subagents)>();

foreach (var project in projects)
{
    var dir = Path.Combine(config.ProjectsRoot, project);
    if (!Directory.Exists(dir))
    {
        // A listed dir aging out entirely is expected eventually — warn so a typo is visible,
        // retain everything already ingested from it.
        Console.WriteLine($"⚠ {project}: directory not found (previously ingested sessions are retained).");
        continue;
    }

    foreach (var file in Directory.GetFiles(dir, "*.jsonl", SearchOption.TopDirectoryOnly)
                 .OrderBy(f => Path.GetFileName(f), StringComparer.OrdinalIgnoreCase))
    {
        var stem = Path.GetFileNameWithoutExtension(file);
        var subDir = Path.Combine(dir, stem, "subagents");
        var subFiles = Directory.Exists(subDir)
            ? Directory.GetFiles(subDir, "agent-*.jsonl", SearchOption.TopDirectoryOnly)
            : Array.Empty<string>();
        found.Add((stem, project, "main", null, file, subFiles.Length));

        foreach (var subFile in subFiles.OrderBy(f => Path.GetFileName(f), StringComparer.OrdinalIgnoreCase))
            found.Add((Path.GetFileNameWithoutExtension(subFile), project, "subagent", stem, subFile, 0));
    }
}

var totals = new Dictionary<string, (int New, int Changed, int Unchanged)>();
var work = new List<((string SessionId, string ProjectDir, string Kind, string? Parent, string Path, int Subagents) File, IngestPlan.Change Change)>();

foreach (var f in found)
{
    var info = new FileInfo(f.Path);
    var change = IngestPlan.Classify(
        stamps.TryGetValue(f.SessionId, out var s) ? (s.SourceBytes, s.SourceMtimeUtc) : null,
        info.Length, info.LastWriteTimeUtc.ToString("o"));
    work.Add((f, change));

    (int New, int Changed, int Unchanged) t = totals.TryGetValue(f.ProjectDir, out var v) ? v : (0, 0, 0);
    totals[f.ProjectDir] = change switch
    {
        IngestPlan.Change.New => (t.New + 1, t.Changed, t.Unchanged),
        IngestPlan.Change.Changed => (t.New, t.Changed + 1, t.Unchanged),
        _ => (t.New, t.Changed, t.Unchanged + 1)
    };
}

// Absent-but-retained: stored sessions (scoped to the projects this run covers) with no file.
var scopedStored = stamps.Where(kv => projects.Contains(kv.Value.ProjectDir, StringComparer.OrdinalIgnoreCase));
var absent = IngestPlan.AbsentRetained(scopedStored.Select(kv => kv.Key), found.Select(f => f.SessionId));

foreach (var project in projects)
{
    (int New, int Changed, int Unchanged) t = totals.TryGetValue(project, out var v) ? v : (0, 0, 0);
    var absentHere = absent.Count(id => stamps[id].ProjectDir.Equals(project, StringComparison.OrdinalIgnoreCase));
    Console.WriteLine($"{project}: new {t.New} / changed {t.Changed} / unchanged {t.Unchanged} / absent-but-retained {absentHere}");
}
Console.WriteLine($"TOTAL: new {work.Count(w => w.Change == IngestPlan.Change.New)} / " +
                  $"changed {work.Count(w => w.Change == IngestPlan.Change.Changed)} / " +
                  $"unchanged {work.Count(w => w.Change == IngestPlan.Change.Unchanged)} / " +
                  $"absent-but-retained {absent.Count}");
Console.WriteLine();

var toIngest = work.Where(w => w.Change != IngestPlan.Change.Unchanged).ToList();
if (toIngest.Count > 0)
{
    Console.WriteLine("Sessions to ingest:");
    foreach (var (f, change) in toIngest)
    {
        var label = change == IngestPlan.Change.New ? "new    " : "changed";
        var kind = f.Kind == "subagent" ? $" (subagent of {Shorten(f.Parent!)})" : $" ({f.Subagents} subagent(s))";
        Console.WriteLine($"  [{label}] {f.ProjectDir}/{Shorten(f.SessionId)}{kind} — {new FileInfo(f.Path).Length / 1024.0:F0} KB");
    }
    Console.WriteLine();
}

if (!apply)
{
    Console.WriteLine("DRY RUN — no database written. Pass --apply to write.");
    return 0;
}

Console.WriteLine("Writing...");
var written = 0;
var skipped = 0;
foreach (var (f, change) in work)
{
    if (change == IngestPlan.Change.Unchanged)
    {
        CodeSessionDb.TouchSeen(conn, f.SessionId);
        continue;
    }

    var info = new FileInfo(f.Path);
    var extracted = CodeSessionExtractor.Extract(File.ReadLines(f.Path));

    if (extracted.AssistantChars < MinAssistantChars)
    {
        skipped++;
        continue;
    }

    var records = extracted.Records;
    var session = new SessionRow(
        SessionId: f.SessionId,
        ProjectDir: f.ProjectDir,
        Kind: f.Kind,
        ParentSessionId: f.Parent,
        Title: extracted.Title,
        Slug: extracted.Slug,
        FirstTimestamp: records.Count > 0 ? records[0].Timestamp : "",
        LastTimestamp: records.Count > 0 ? records[^1].Timestamp : "",
        RecordCount: records.Count,
        TotalChars: records.Sum(r => (long)r.Body.Length),
        SubagentCount: f.Subagents,
        MalformedLines: extracted.MalformedLines,
        SourceBytes: info.Length,
        SourceMtimeUtc: info.LastWriteTimeUtc.ToString("o"));

    CodeSessionDb.ReplaceSession(conn, session, records);
    written++;
    Console.WriteLine($"  {f.ProjectDir}/{Shorten(f.SessionId)} — {records.Count} records, " +
                      $"{session.TotalChars:N0} chars" +
                      (extracted.LargePasteStubs > 0 ? $", {extracted.LargePasteStubs} large-paste stub(s)" : "") +
                      (extracted.MalformedLines > 0 ? $", {extracted.MalformedLines} malformed line(s)" : ""));
}

Console.WriteLine();
Console.WriteLine($"Wrote {written} session(s); {skipped} skipped (< {MinAssistantChars} assistant chars); " +
                  $"{absent.Count} absent-but-retained; database " +
                  $"{new FileInfo(config.Output).Length / (1024.0 * 1024.0):F1} MB");
return 0;

static string Shorten(string sessionId) =>
    sessionId.Length > 8 && sessionId.StartsWith("agent-") ? sessionId[..14] :
    sessionId.Length > 8 ? sessionId[..8] : sessionId;

internal sealed record CodeSessionsConfig(string? ProjectsRoot, string? Output, List<string> Projects)
{
    public List<string> Projects { get; init; } = Projects ?? [];
}
