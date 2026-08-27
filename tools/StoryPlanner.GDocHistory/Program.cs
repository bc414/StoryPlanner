using System.Text.Json;
using StoryPlanner.GDocHistory;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: dotnet run --project tools/StoryPlanner.GDocHistory -- <config.json> [--apply]");
    return 2;
}

var configPath = args[0];
var apply = args.Contains("--apply");

if (!File.Exists(configPath))
{
    Console.Error.WriteLine($"Config not found: {configPath}");
    return 2;
}

var config = JsonSerializer.Deserialize<GDocHistoryConfig>(
    File.ReadAllText(configPath),
    new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip, PropertyNameCaseInsensitive = true });

if (string.IsNullOrWhiteSpace(config?.Output))
{
    Console.Error.WriteLine("Config missing 'output' (path to lineage.db).");
    return 2;
}
if (string.IsNullOrWhiteSpace(config.SnapshotsDir))
{
    Console.Error.WriteLine("Config missing 'snapshotsDir'.");
    return 2;
}
if (!Directory.Exists(config.SnapshotsDir))
{
    Console.Error.WriteLine($"Snapshots directory not found: {config.SnapshotsDir}");
    return 2;
}

var manifestPath = Path.Combine(config.SnapshotsDir, "manifest.json");
if (!File.Exists(manifestPath))
{
    Console.Error.WriteLine($"manifest.json not found in {config.SnapshotsDir}");
    return 2;
}

var manifest = JsonSerializer.Deserialize<Manifest>(
    File.ReadAllText(manifestPath),
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

if (manifest?.Days == null || manifest.Days.Count == 0)
{
    Console.Error.WriteLine("manifest.json contains no days.");
    return 2;
}

Console.Error.WriteLine($"Google Doc revision history — {manifest.Days.Count} snapshots");
Console.Error.WriteLine($"Snapshots dir: {config.SnapshotsDir}");
Console.Error.WriteLine($"Output: {config.Output}");
Console.Error.WriteLine();

// Load all snapshot texts in date order
var snapshots = new List<GDocSnapshot>();
var refusals = new List<string>();

foreach (var day in manifest.Days)
{
    var filePath = Path.Combine(config.SnapshotsDir, $"{day.Date}.txt");
    if (!File.Exists(filePath))
    {
        refusals.Add($"Missing snapshot file: {filePath}");
        continue;
    }
    var text = File.ReadAllText(filePath);
    snapshots.Add(new GDocSnapshot(day.Date, text, day.FileBytes, day.Source));
}

if (refusals.Count > 0)
{
    Console.Error.WriteLine("REFUSALS:");
    foreach (var r in refusals) Console.Error.WriteLine($"  {r}");
    return 1;
}

// Compute diffs between consecutive snapshots
var diffs = new List<GDocDiffEntry>();

for (int i = 1; i < snapshots.Count; i++)
{
    var prev = snapshots[i - 1];
    var curr = snapshots[i];
    var deltaBytes = curr.FileBytes - prev.FileBytes;
    var result = GDocDiffer.ComputeDiff(prev.Body, curr.Body, prev.Date, curr.Date,
        prev.FileBytes, curr.FileBytes);
    diffs.Add(new GDocDiffEntry(curr.Date, prev.Date, result.FormattedDiff,
        result.LinesAdded, result.LinesRemoved, deltaBytes));
}

Console.Error.WriteLine($"Snapshots: {snapshots.Count}");
Console.Error.WriteLine($"Diffs:     {diffs.Count}");
Console.Error.WriteLine($"Date range: {snapshots[0].Date} → {snapshots[^1].Date}");
Console.Error.WriteLine();

// Diff summary
Console.Error.WriteLine("Per-diff metrics:");
foreach (var d in diffs)
{
    Console.Error.WriteLine($"  {d.Date} (from {d.FromDate})  +{d.LinesAdded}/-{d.LinesRemoved}  " +
                            $"{d.DeltaBytes:+#;-#;0} bytes  ({d.Body.Length:N0} diff chars)");
}
Console.Error.WriteLine();

var totalSnapshotChars = snapshots.Sum(s => (long)s.Body.Length);
var totalDiffChars = diffs.Sum(d => (long)d.Body.Length);
Console.Error.WriteLine($"Total snapshot text: {totalSnapshotChars:N0} chars");
Console.Error.WriteLine($"Total diff text:     {totalDiffChars:N0} chars");

if (!apply)
{
    Console.Error.WriteLine();
    Console.Error.WriteLine("DRY RUN — no database written. Pass --apply to write.");
    return 0;
}

Console.Error.WriteLine();
Console.Error.WriteLine($"Writing to {config.Output}...");

using var conn = GDocHistoryDb.OpenWrite(config.Output);
var rows = GDocHistoryDb.ReplaceGDocHistory(conn, snapshots, diffs);
GDocHistoryDb.RecordIngestRun(conn, rows);

var fi = new FileInfo(config.Output);
Console.Error.WriteLine($"Done. {rows} rows written. Database: {fi.Length:N0} bytes.");
return 0;

internal sealed record GDocHistoryConfig(string? Output, string? SnapshotsDir);

internal sealed record ManifestDay(string Date, int FileBytes, int DeltaBytes, string Source);
internal sealed record Manifest(IReadOnlyList<ManifestDay>? Days);
