using System.Text.Json;
using StoryPlanner.BatchFiles;
using StoryPlanner.V1Itemizer;

// The v1 itemizer: cuts the v1-archive corpus into one batch's items and writes its index.md, with
// an itemizer line, and the bodies under items/, refusing a batch that already has an index.
//
//   dotnet run --project tools/StoryPlanner.V1Itemizer -- <config.json> <batch-folder>
//
// The config is authored and lives with the tool, paths relative to the current directory:
//   {"unit": "plot-points", "snapshot": "<native v1 .db>", "excludeChapters": [<chapter id>...], "includeUnplaced": true}
//   {"unit": "theme-commentaries", "snapshot": "<native v1 .db>"}
//   {"unit": "subjects", "archive": "<v1 archive .storyplan>"}
//   {"unit": "own-voice-notes", "archive": "<v1 archive .storyplan>", "lineage": "<lineage .db>",
//    "snapshots": "<dir of dated v1 snapshots>", "excludeStory": "Paratext"}
// The native snapshot is a dated backup in the v1 planner's own schema; the archive is the
// converted .storyplan, read for its subjects and their triage labels. The own-voice unit runs the
// lineage attribution itself over the archive, lineage.db and the snapshot directory — the sidecar
// attribution.csv is a derived artifact and is never an input — and keeps the notes the
// v1-archive-mining skill's label table credits to the author's own voice.

if (args.Length < 2) return Usage();
var configPath = Path.GetFullPath(args[0]);
var batchDir = Path.GetFullPath(args[1]);
if (!File.Exists(configPath)) { Console.Error.WriteLine($"config not found: {configPath}"); return 2; }
if (ItemizerOutput.Refusal(batchDir) is { } refusal) { Console.Error.WriteLine(refusal); return 2; }
var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
if (config?.Unit is null) { Console.Error.WriteLine("config names no unit: plot-points, theme-commentaries, subjects or own-voice-notes"); return 2; }

IReadOnlyList<ItemizerOutput.Item> items;
string locatorNotation;
string? narrowing;
switch (config.Unit)
{
    case "plot-points" or "theme-commentaries":
    {
        if (config.Snapshot is null || !File.Exists(config.Snapshot)) { Console.Error.WriteLine($"snapshot not found: {config.Snapshot}"); return 2; }
        var plan = NativeV1.Load(config.Snapshot);
        if (config.Unit == "plot-points")
        {
            var exclude = new HashSet<int>(config.ExcludeChapters ?? []);
            foreach (var id in exclude.Where(id => plan.Chapters.All(c => c.Id != id)))
                Console.Error.WriteLine($"warning: excluded chapter {id} is not in the snapshot");
            var unplaced = config.IncludeUnplaced ?? true;
            items = NativeCuts.PlotPoints(plan, exclude, unplaced);
            locatorNotation = NativeCuts.PlotPointLocatorNotation;
            narrowing = NativeCuts.PlotPointNarrowing(plan, exclude, unplaced);
        }
        else
        {
            items = NativeCuts.ThemeCommentaries(plan);
            locatorNotation = NativeCuts.ThemeCommentaryLocatorNotation;
            narrowing = NativeCuts.ThemeCommentaryNarrowing;
        }
        break;
    }
    case "subjects":
    {
        if (config.Archive is null || !File.Exists(config.Archive)) { Console.Error.WriteLine($"archive not found: {config.Archive}"); return 2; }
        var (subjects, notes) = ArchiveSubjects.Load(config.Archive);
        items = ArchiveSubjects.Cut(subjects, notes);
        locatorNotation = ArchiveSubjects.LocatorNotation;
        narrowing = ArchiveSubjects.Narrowing;
        break;
    }
    case "own-voice-notes":
    {
        if (config.Archive is null || !File.Exists(config.Archive)) { Console.Error.WriteLine($"archive not found: {config.Archive}"); return 2; }
        if (config.Lineage is null || !File.Exists(config.Lineage)) { Console.Error.WriteLine($"lineage not found: {config.Lineage}"); return 2; }
        if (config.Snapshots is not null && !Directory.Exists(config.Snapshots)) { Console.Error.WriteLine($"snapshot directory not found: {config.Snapshots}"); return 2; }
        var notes = OwnVoice.Attribute(config.Archive, config.Lineage, config.Snapshots, config.ExcludeStory, Console.Error.WriteLine);
        items = OwnVoice.Cut(notes);
        locatorNotation = OwnVoice.LocatorNotation;
        narrowing = OwnVoice.Narrowing + (config.ExcludeStory is null ? "" : $"; and except the notes of any story named with \"{config.ExcludeStory}\"");
        break;
    }
    default:
        Console.Error.WriteLine($"unknown unit '{config.Unit}': plot-points, theme-commentaries, subjects or own-voice-notes");
        return 2;
}

var problems = ItemizerOutput.Problems(items);
if (problems.Count > 0) { foreach (var p in problems) Console.Error.WriteLine(p); return 2; }
ItemizerOutput.Write(batchDir, ItemizerOutput.ItemizerLine("StoryPlanner.V1Itemizer"), locatorNotation, items, narrowing);
Console.WriteLine($"{items.Count} items ({config.Unit}) → {batchDir} (index.md and items/)");
return 0;

static int Usage()
{
    Console.Error.WriteLine("Usage: V1Itemizer <config.json> <batch-folder>");
    return 2;
}

sealed record Config(string? Unit, string? Snapshot, string? Archive, int[]? ExcludeChapters, bool? IncludeUnplaced,
    string? Lineage, string? Snapshots, string? ExcludeStory);
