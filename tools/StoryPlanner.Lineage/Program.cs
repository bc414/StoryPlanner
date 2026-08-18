using System.Text.Json;
using StoryPlanner.Lineage;

// Ingests the AI Studio and NotebookLM layers of the LINEAGE corpus into lineage.db — the same
// file tools/StoryPlanner.GeminiCorpus writes its Entries/Reports into. Each source replaces
// only its own tables, so the three founding-era sources re-ingest independently.
//
//   dotnet run --project tools/StoryPlanner.Lineage -- <config.json> [--apply] [--source aistudio|notebooklm]

var positional = args.Where(a => !a.StartsWith("--")).ToList();
var apply = args.Contains("--apply");
var sourceArgIdx = Array.IndexOf(args, "--source");
string? onlySource = sourceArgIdx >= 0 && sourceArgIdx + 1 < args.Length ? args[sourceArgIdx + 1] : null;
if (onlySource is not null) positional.Remove(onlySource);

if (positional.Count != 1)
{
    Console.Error.WriteLine("Usage: dotnet run -- <config.json> [--apply] [--source aistudio|notebooklm]");
    return 2;
}
if (onlySource is not null && onlySource is not ("aistudio" or "notebooklm"))
{
    Console.Error.WriteLine($"Unknown --source \"{onlySource}\" — pass aistudio or notebooklm.");
    return 2;
}

var configPath = positional[0];
if (!File.Exists(configPath))
{
    Console.Error.WriteLine($"Config not found: {configPath}");
    return 2;
}

LineageConfig config;
try
{
    config = JsonSerializer.Deserialize<LineageConfig>(
        File.ReadAllText(configPath),
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true, ReadCommentHandling = JsonCommentHandling.Skip })
        ?? throw new JsonException("config deserialized to null");
}
catch (JsonException ex)
{
    Console.Error.WriteLine($"Config unreadable: {ex.Message}");
    return 2;
}
if (string.IsNullOrWhiteSpace(config.Output))
{
    Console.Error.WriteLine("Config has no \"output\" path.");
    return 2;
}

var doAiStudio = onlySource is null or "aistudio";
var doNotebookLm = onlySource is null or "notebooklm";
var refusals = new List<string>();

Console.WriteLine($"lineage ingest ({(apply ? "APPLY" : "DRY RUN")})");
Console.WriteLine($"  config : {configPath}");
Console.WriteLine($"  output : {config.Output}");
if (onlySource is not null) Console.WriteLine($"  source : {onlySource} only");
Console.WriteLine();

// ── AI Studio ────────────────────────────────────────────────────────────────

var aiChats = new List<AiStudioChat>();
if (doAiStudio)
{
    if (config.AiStudio is null || string.IsNullOrWhiteSpace(config.AiStudio.ChatsDir))
    {
        refusals.Add("aistudio: config has no aistudio.chatsDir.");
    }
    else if (!Directory.Exists(config.AiStudio.ChatsDir))
    {
        refusals.Add($"aistudio: chatsDir not found: {config.AiStudio.ChatsDir}");
    }
    else
    {
        var a = config.AiStudio;
        var excludeDir = string.IsNullOrWhiteSpace(a.ExcludeImportedVia)
            ? null : Path.Combine(a.ChatsDir, a.ExcludeImportedVia);

        // A raw AI Studio chat is an extensionless file whose NAME may legally contain dots
        // ("MCU as Faust vs. Mandate"), so candidacy is decided by parsing, not by extension —
        // only files with a known non-chat extension are skipped up front.
        string[] nonChat = [".md", ".json", ".txt", ".htm", ".html", ".csv", ".zip", ".pdf", ".png", ".jpg", ".xlsx", ".xls", ".docx"];
        var candidates = Directory.GetFiles(a.ChatsDir, "*", SearchOption.TopDirectoryOnly)
            .Where(f => !nonChat.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
            .OrderBy(f => Path.GetFileName(f), StringComparer.OrdinalIgnoreCase)
            .ToList();

        var excludedImported = new List<string>();
        var excludedByConfig = new List<string>();
        var ignored = new List<string>();
        var dropped = new List<AiStudioChatParser.ParseFailure>();

        foreach (var file in candidates)
        {
            var name = Path.GetFileName(file);

            if (a.Exclude.Contains(name, StringComparer.OrdinalIgnoreCase)) { excludedByConfig.Add(name); continue; }
            if (excludeDir is not null && File.Exists(Path.Combine(excludeDir, name + ".json")))
            {
                excludedImported.Add(name);
                continue;
            }
            if (a.Ignore.Contains(name, StringComparer.OrdinalIgnoreCase)) { ignored.Add(name); continue; }

            var chat = AiStudioChatParser.Parse(File.ReadAllText(file), name, out var failure);
            if (chat is null)
            {
                if (failure!.Reason.StartsWith("dropped:"))
                    dropped.Add(failure);
                else
                    refusals.Add($"aistudio: {name} — {failure.Reason} (add it to \"ignore\" if it is not a chat).");
                continue;
            }
            aiChats.Add(chat);
        }

        aiChats = aiChats.OrderBy(c => c.Date, StringComparer.Ordinal).ToList();

        Console.WriteLine($"AI Studio — {a.ChatsDir}");
        Console.WriteLine($"  {aiChats.Count} chat(s) included, {aiChats.Sum(c => c.Turns.Count)} turns, " +
                          $"{aiChats.Sum(c => (long)c.Turns.Sum(t => t.Body.Length)):N0} chars");
        foreach (var c in aiChats)
        {
            var dateNote = c.Date.Length > 0 ? c.Date[..Math.Min(10, c.Date.Length)] : "NO DATE";
            var missing = c.TurnsMissingCreateTime > 0 ? $" [{c.TurnsMissingCreateTime} turn(s) missing createTime]" : "";
            Console.WriteLine($"    [{dateNote}] \"{c.Title}\" — {c.Turns.Count} turns{missing} (key: \"{c.ChatKey}\")");
        }
        Print("excluded — already imported into Conversations (name.json in " + (a.ExcludeImportedVia ?? "?") + ")", excludedImported);
        Print("excluded by config", excludedByConfig);
        Print("ignored (configured non-chat files)", ignored);
        if (dropped.Count > 0)
        {
            Console.WriteLine($"  dropped ({dropped.Count}) — two or fewer surviving turns:");
            foreach (var d in dropped) Console.WriteLine($"    {d.FileName} — {d.Reason}");
        }
        var undated = aiChats.Where(c => c.Date.Length == 0).ToList();
        if (undated.Count > 0)
        {
            Console.WriteLine($"  ⚠ {undated.Count} chat(s) carry NO createTime at all — lineage needs dates:");
            foreach (var c in undated) Console.WriteLine($"    \"{c.ChatKey}\"");
        }
        if (aiChats.Count == 0)
            refusals.Add("aistudio: zero chats included — population rule or chatsDir is wrong.");
        Console.WriteLine();
    }
}

// ── NotebookLM ───────────────────────────────────────────────────────────────

var notebooks = new List<NlmNotebook>();
var notesWithoutBodies = 0;
if (doNotebookLm)
{
    if (config.NotebookLm is null || string.IsNullOrWhiteSpace(config.NotebookLm.CapturesDir))
    {
        refusals.Add("notebooklm: config has no notebooklm.capturesDir.");
    }
    else
    {
        var n = config.NotebookLm;
        Console.WriteLine($"NotebookLM — {n.CapturesDir}");

        if (!Directory.Exists(n.CapturesDir))
        {
            // The captures dir not existing yet is a legal early state (captures are manual);
            // it only becomes a refusal if the config also names notebooks expecting files.
            Console.WriteLine("  captures dir does not exist yet — nothing to ingest.");
            if (n.Notebooks.Count > 0)
                refusals.Add($"notebooklm: {n.Notebooks.Count} notebook(s) configured but capturesDir is missing.");
        }
        else
        {
            var configured = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var nb in n.Notebooks)
            {
                if (string.IsNullOrWhiteSpace(nb.Slug) || string.IsNullOrWhiteSpace(nb.File))
                {
                    refusals.Add("notebooklm: a notebooks entry is missing slug or file.");
                    continue;
                }
                configured.Add(nb.File);
                var path = Path.Combine(n.CapturesDir, nb.File);
                if (!File.Exists(path))
                {
                    refusals.Add($"notebooklm: configured capture not found: {nb.File}");
                    continue;
                }

                var capture = NlmCaptureParser.Parse(File.ReadAllText(path, System.Text.Encoding.UTF8));
                if (capture.Turns.Count == 0 && capture.Notes.Count == 0)
                {
                    refusals.Add($"notebooklm: {nb.File} parsed to zero turns and zero notes — not a NotebookLM capture?");
                    continue;
                }

                var bodyless = capture.Notes.Count(o => o.Body.Length == 0);
                notesWithoutBodies += bodyless;
                notebooks.Add(new NlmNotebook(
                    Slug: nb.Slug,
                    Title: !string.IsNullOrWhiteSpace(nb.Title) ? nb.Title!
                        : capture.Title.Length > 0 ? capture.Title : nb.Slug,
                    AuthoredDate: string.IsNullOrWhiteSpace(nb.AuthoredDate) ? null : nb.AuthoredDate,
                    CaptureFile: nb.File,
                    CapturedUtc: File.GetLastWriteTimeUtc(path).ToString("o"),
                    Turns: capture.Turns,
                    Notes: capture.Notes));

                var last = notebooks[^1];
                var dateLabel = last.AuthoredDate ?? "UNDATED — authored date pending";
                Console.WriteLine($"    [{dateLabel}] \"{last.Title}\" ({last.Slug}) — " +
                                  $"{last.Turns.Count} turns, {last.Notes.Count} note(s)" +
                                  (bodyless > 0 ? $" ({bodyless} title-only — bodies not in the capture)" : "") +
                                  (last.Turns.Count == 0 ? " ⚠ zero chat turns — under-scrolled capture?" : ""));
            }

            var unconfigured = Directory.GetFiles(n.CapturesDir, "*.htm*", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileName)
                .Where(f => f is not null && !configured.Contains(f))
                .ToList();
            if (unconfigured.Count > 0)
            {
                Console.WriteLine($"  not configured ({unconfigured.Count}) — add a notebooks entry (slug + authoredDate are authored) to ingest:");
                foreach (var f in unconfigured) Console.WriteLine($"    {f}");
            }
            if (n.Notebooks.Count == 0)
                Console.WriteLine("  no notebooks configured yet.");
        }
        Console.WriteLine();
    }
}

// ── Refusals / dry run / write ───────────────────────────────────────────────

if (refusals.Count > 0)
{
    Console.Error.WriteLine("Refusing to write: fix the problems below first.");
    foreach (var r in refusals) Console.Error.WriteLine($"  - {r}");
    return 1;
}

if (!apply)
{
    Console.WriteLine("DRY RUN — no database written. Pass --apply to write.");
    return 0;
}

Console.WriteLine("Writing database...");
using var conn = LineageDb.OpenWrite(config.Output);
if (doAiStudio)
{
    var rows = LineageDb.ReplaceAiStudio(conn, aiChats);
    LineageDb.RecordIngestRun(conn, "aistudio", rows);
    Console.WriteLine($"  aistudio: {aiChats.Count} chats / {rows:N0} turns written.");
}
if (doNotebookLm)
{
    var rows = LineageDb.ReplaceNotebookLm(conn, notebooks);
    LineageDb.RecordIngestRun(conn, "notebooklm", rows);
    Console.WriteLine($"  notebooklm: {notebooks.Count} notebook(s) / {rows:N0} turn+note rows written.");
}
Console.WriteLine($"  Database size: {new FileInfo(config.Output).Length / (1024.0 * 1024.0):F1} MB");

// The standing follow-up (author's instruction, 2026-08-17): NotebookLM dates must not be
// forgotten. Chat turns carry no timestamps in a capture, so dates are authored per notebook
// in the config — surface what is still unresolved at the end of every apply run.
if (doNotebookLm && notebooks.Count > 0)
{
    var undatedNbs = notebooks.Where(nb => nb.AuthoredDate is null).ToList();
    Console.WriteLine();
    Console.WriteLine("NotebookLM date status:");
    foreach (var nb in notebooks)
        Console.WriteLine($"  {nb.Slug}: {(nb.AuthoredDate ?? "UNDATED")}");
    if (undatedNbs.Count > 0)
        Console.WriteLine($"  ⚠ {undatedNbs.Count} notebook(s) undated — revisit manually (authoredDate in the config) or by other means.");
    if (notesWithoutBodies > 0)
        Console.WriteLine($"  note: {notesWithoutBodies} studio note(s) are title-only — the saved panel renders previews; " +
                          "a capture that opens each note would be needed for bodies.");
}

return 0;

static void Print(string label, List<string> items)
{
    if (items.Count == 0) return;
    Console.WriteLine($"  {label} ({items.Count}):");
    foreach (var i in items) Console.WriteLine($"    {i}");
}

internal sealed record LineageConfig(
    string? Output,
    AiStudioConfig? AiStudio,
    NotebookLmConfig? NotebookLm);

internal sealed record AiStudioConfig(
    string? ChatsDir,
    string? ExcludeImportedVia,
    List<string> Exclude,
    List<string> Ignore)
{
    public List<string> Exclude { get; init; } = Exclude ?? [];
    public List<string> Ignore { get; init; } = Ignore ?? [];
}

internal sealed record NotebookLmConfig(
    string? CapturesDir,
    List<NotebookEntry> Notebooks)
{
    public List<NotebookEntry> Notebooks { get; init; } = Notebooks ?? [];
}

internal sealed record NotebookEntry(string? Slug, string? File, string? Title, string? AuthoredDate);
