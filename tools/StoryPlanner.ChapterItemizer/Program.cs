using System.Text.Json;
using StoryPlanner.BatchFiles;
using StoryPlanner.ChapterItemizer;

// The chapter itemizer (d-2026-09-14-4): cuts a folder of converter-format story files into one
// item per `## Chapter` heading, or with `"unit": "story"` into one item per story whole, and
// writes a batch's index.md, with an itemizer line, and the item bodies under items/, refusing a
// batch that already has an index — an itemizer runs once per batch.
//
//   dotnet run --project tools/StoryPlanner.ChapterItemizer -- <config.json> <batch-folder>
//
// The config is authored and lives with the tool: {"source": "<folder of story files>",
// "pattern": "*.md", "unit": "chapter" | "story", "exclude": ["<slug>", ...], "maxWords": N},
// the source relative to the current directory, the pattern defaulting to *.md and the unit to
// chapter; maxWords, for the story unit only, leaves out a story longer than that. The excluded
// stories and those over the limit are the batch's narrowing, recorded in the index head by name.

if (args.Length < 2) return Usage();
var configPath = Path.GetFullPath(args[0]);
var batchDir = Path.GetFullPath(args[1]);
if (!File.Exists(configPath)) { Console.Error.WriteLine($"config not found: {configPath}"); return 2; }
if (ItemizerOutput.Refusal(batchDir) is { } refusal) { Console.Error.WriteLine(refusal); return 2; }

var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
if (config?.Source is null) { Console.Error.WriteLine("config names no source folder"); return 2; }
var source = Path.GetFullPath(config.Source);
if (!Directory.Exists(source)) { Console.Error.WriteLine($"source folder not found: {source}"); return 2; }
var unit = config.Unit ?? "chapter";
if (unit is not ("chapter" or "story")) { Console.Error.WriteLine($"unknown unit '{unit}': chapter or story"); return 2; }
if (config.MaxWords is not null && unit != "story") { Console.Error.WriteLine("maxWords applies to the story unit only"); return 2; }

var pattern = config.Pattern ?? "*.md";
var extension = Path.GetExtension(pattern);
var exclude = new HashSet<string>(config.Exclude ?? [], StringComparer.Ordinal);
var stories = Directory.GetFiles(source, pattern)
    .Select(f => new Chapters.Story(Path.GetFileNameWithoutExtension(f), File.ReadAllText(f)))
    .ToList();
var present = stories.Select(s => s.Slug).ToHashSet(StringComparer.Ordinal);
foreach (var e in exclude.Where(e => !present.Contains(e)))
    Console.Error.WriteLine($"warning: excluded story not in the source folder: {e}");
var excluded = exclude.Where(present.Contains).OrderBy(e => e, StringComparer.Ordinal).ToList();

IReadOnlyList<Chapters.Item> items;
string locatorNotation;
string? narrowing;
if (unit == "story")
{
    var overLimit = new List<string>();
    items = Chapters.WholeAll(stories, exclude, config.MaxWords, overLimit);
    locatorNotation = Chapters.StoryLocatorNotationFor(extension);
    narrowing = Chapters.Narrowing(excluded, overLimit, config.MaxWords);
}
else
{
    try { items = Chapters.CutAll(stories, exclude); }
    catch (InvalidOperationException ex) { Console.Error.WriteLine(ex.Message); return 2; }
    locatorNotation = Chapters.LocatorNotationFor(extension);
    narrowing = Chapters.Narrowing(excluded);
}

var cut = items.Select(it => new ItemizerOutput.Item(it.Id, it.Body, it.Locator, it.Description)).ToList();
var problems = ItemizerOutput.Problems(cut);
if (problems.Count > 0) { foreach (var p in problems) Console.Error.WriteLine(p); return 2; }
ItemizerOutput.Write(batchDir, ItemizerOutput.ItemizerLine("StoryPlanner.ChapterItemizer"), locatorNotation, cut, narrowing);
Console.WriteLine($"{cut.Count} items ({unit}) from {stories.Count - excluded.Count} stories → {batchDir} (index.md and items/)");
return 0;

static int Usage()
{
    Console.Error.WriteLine("Usage: ChapterItemizer <config.json> <batch-folder>");
    return 2;
}

sealed record Config(string? Source, string[]? Exclude, string? Pattern, string? Unit, int? MaxWords);
