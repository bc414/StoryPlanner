using System.Text.Json;
using StoryPlanner.DataOps;

var ops = new Dictionary<string, IDataOperation>(StringComparer.OrdinalIgnoreCase)
{
    ["assign-stories"] = new AssignStories(),
    ["convert-world-dates"] = new ConvertWorldDates(),
    ["seed-timeline-defaults"] = new SeedTimelineDefaults(),
    ["seed-source-material"] = new SeedSourceMaterial(),
    ["seed-narrative-properties"] = new SeedNarrativeProperties(),
    ["wipe-block-summaries"] = new WipeBlockSummaries(),
};

var positional = args.Where(a => a != "--apply").ToList();
var apply = args.Contains("--apply");

if (positional.Count != 3)
{
    Console.Error.WriteLine("Usage: dotnet run -- <op-name> <path-to.storyplan> <config.json> [--apply]");
    Console.Error.WriteLine($"Known ops: {string.Join(", ", ops.Keys)}");
    return 2;
}

var (opName, dbPath, configPath) = (positional[0], positional[1], positional[2]);

if (!ops.TryGetValue(opName, out var op))
{
    Console.Error.WriteLine($"Unknown op '{opName}'. Known ops: {string.Join(", ", ops.Keys)}");
    return 2;
}

if (!File.Exists(configPath))
{
    Console.Error.WriteLine($"Config file not found: {configPath}");
    return 2;
}

using var doc = JsonDocument.Parse(await File.ReadAllTextAsync(configPath));
return await DataOpEnvelope.RunAsync(op, dbPath, doc.RootElement, apply);
