using StoryPlanner.GeminiCorpus;

// Ingests the Gemini web-app corpus — story-tagged conversation entries and the weekly
// story-development reports — into a standalone SQLite database for the MCP server.
//
//   dotnet run --project tools/StoryPlanner.GeminiCorpus -- <corpus-dir> <output.db> [--apply]

var positional = args.Where(a => !a.StartsWith("--")).ToList();
var apply = args.Contains("--apply");

if (positional.Count != 2)
{
    Console.Error.WriteLine("Usage: dotnet run -- <gemini-markdown-dir> <output.db> [--apply]");
    Console.Error.WriteLine("  <gemini-markdown-dir>  Path to the gemini_markdown corpus directory (contains corpus_index.json)");
    Console.Error.WriteLine("  <output.db>            Path to the output SQLite database");
    return 2;
}

var corpusDir = positional[0];
var outputPath = positional[1];

if (!File.Exists(Path.Combine(corpusDir, "corpus_index.json")))
{
    Console.Error.WriteLine($"corpus_index.json not found in: {corpusDir}");
    return 2;
}

Console.WriteLine($"gemini-corpus ingest ({(apply ? "APPLY" : "DRY RUN")})");
Console.WriteLine($"  corpus : {corpusDir}");
Console.WriteLine($"  output : {outputPath}");
Console.WriteLine();

// --- Entries ---
Console.WriteLine("Parsing entries...");
var (entries, stubbed) = GeminiCorpusParser.ParseEntries(corpusDir);
var threads = entries.Select(e => e.ThreadId).Distinct().Count();
var bySubtopic = entries.GroupBy(e => e.Subtopic ?? "(none)")
    .OrderByDescending(g => g.Count());

Console.WriteLine($"  {entries.Count:N0} story-tagged entries across {threads} threads");
Console.WriteLine($"  {stubbed} plan-paste prompts stubbed (>{20_000:N0} words)");
Console.WriteLine();
Console.WriteLine("  By subtopic:");
foreach (var g in bySubtopic)
    Console.WriteLine($"    {g.Key}: {g.Count():N0}");

var totalPromptChars = entries.Sum(e => (long)e.Prompt.Length);
var totalResponseChars = entries.Sum(e => (long)e.Response.Length);
Console.WriteLine();
Console.WriteLine($"  Stored text: {totalPromptChars:N0} prompt chars + {totalResponseChars:N0} response chars = {totalPromptChars + totalResponseChars:N0} total");

// --- Reports ---
var reportDir = Path.Combine(Path.GetDirectoryName(corpusDir)!, "story_development_report");
var reports = new List<GeminiReport>();
if (Directory.Exists(reportDir))
{
    Console.WriteLine();
    Console.WriteLine("Parsing reports...");
    reports = GeminiCorpusParser.ParseReports(reportDir);
    var weekly = reports.Count(r => r.Kind == "weekly");
    var appendix = reports.Count(r => r.Kind == "appendix");
    var reportChars = reports.Sum(r => (long)r.Body.Length);
    Console.WriteLine($"  {reports.Count} reports ({weekly} weekly, {appendix} appendix), {reportChars:N0} chars");
    foreach (var r in reports)
        Console.WriteLine($"    [{r.Kind}] {r.Slug} — \"{r.Title}\" ({r.Body.Length:N0} chars)");
}
else
{
    Console.WriteLine($"\n  No report directory at: {reportDir}");
}

if (!apply)
{
    Console.WriteLine();
    Console.WriteLine("DRY RUN — no database written. Pass --apply to write.");
    return 0;
}

// --- Write ---
Console.WriteLine();
Console.WriteLine("Writing database...");
using var conn = GeminiCorpusDb.OpenWrite(outputPath);
var entryCount = GeminiCorpusDb.ReplaceEntries(conn, entries);
var reportCount = GeminiCorpusDb.ReplaceReports(conn, reports);
GeminiCorpusDb.RecordIngestRun(conn, entryCount + reportCount);
Console.WriteLine($"  Wrote {entryCount:N0} entries and {reportCount} reports to: {outputPath}");

var fileSize = new FileInfo(outputPath).Length;
Console.WriteLine($"  Database size: {fileSize / (1024.0 * 1024.0):F1} MB");

return 0;
