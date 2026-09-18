using System.Text.Json;
using StoryPlanner.BatchFiles;
using StoryPlanner.PlanNoteItemizer;

// The working-plan note itemizer: cuts the v2 .storyplan's notes into one batch's items and
// writes its index.md, with an itemizer line, and the bodies under items/, refusing a batch that
// already has an index.
//
//   dotnet run --project tools/StoryPlanner.PlanNoteItemizer -- <config.json> <batch-folder>
//
// The config is authored and lives with the tool:
//   {"plan": "<.storyplan>", "unit": "note" | "owner",
//    "select": "all" | "tracked" | "theme-tagged" | "tracks", "tracks": ["<track name>", ...],
//    "context": "none" | "owner-notes" | "owner-tracks", "contextTracks": ["<track name>", ...]}
// Context applies to the note unit. Flagged notes are never carried, and the narrowing says so.

if (args.Length < 2) return Usage();
var configPath = Path.GetFullPath(args[0]);
var batchDir = Path.GetFullPath(args[1]);
if (!File.Exists(configPath)) { Console.Error.WriteLine($"config not found: {configPath}"); return 2; }
if (ItemizerOutput.Refusal(batchDir) is { } refusal) { Console.Error.WriteLine(refusal); return 2; }
var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
if (config?.Plan is null || !File.Exists(config.Plan)) { Console.Error.WriteLine($"plan not found: {config?.Plan}"); return 2; }

var unit = config.Unit ?? "note";
var selection = new PlanNotes.Selection(config.Select ?? "all", config.Tracks ?? []);
var context = new PlanNotes.Context(config.Context ?? "none", config.ContextTracks ?? []);
if (unit is not ("note" or "owner")) { Console.Error.WriteLine($"unknown unit '{unit}': note or owner"); return 2; }
if (!PlanNotes.Selection.Kinds.Contains(selection.Kind)) { Console.Error.WriteLine($"unknown select '{selection.Kind}': {string.Join(", ", PlanNotes.Selection.Kinds)}"); return 2; }
if (!PlanNotes.Context.Kinds.Contains(context.Kind)) { Console.Error.WriteLine($"unknown context '{context.Kind}': {string.Join(", ", PlanNotes.Context.Kinds)}"); return 2; }
if (selection.Kind == "tracks" && selection.Tracks.Count == 0) { Console.Error.WriteLine("select 'tracks' names no tracks"); return 2; }
if (context.Kind == "owner-tracks" && context.Tracks.Count == 0) { Console.Error.WriteLine("context 'owner-tracks' names no contextTracks"); return 2; }
if (unit == "owner" && context.Kind != "none") { Console.Error.WriteLine("context applies to the note unit only; an owner item already holds the owner's notes"); return 2; }

var plan = PlanNotes.Load(config.Plan);
var known = plan.Tracks.Values.Select(t => t.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
foreach (var name in selection.Tracks.Concat(context.Tracks).Where(n => !known.Contains(n)))
    Console.Error.WriteLine($"warning: no track is named '{name}'");

var selected = PlanNotes.Select(plan, selection);
var items = unit == "note" ? PlanNotes.NoteItems(plan, selected, context) : PlanNotes.OwnerItems(plan, selected);
var problems = ItemizerOutput.Problems(items);
if (problems.Count > 0) { foreach (var p in problems) Console.Error.WriteLine(p); return 2; }
ItemizerOutput.Write(batchDir, ItemizerOutput.ItemizerLine("StoryPlanner.PlanNoteItemizer"),
    unit == "note" ? PlanNotes.NoteLocatorNotation : PlanNotes.OwnerLocatorNotation, items, PlanNotes.Narrowing(selection, context));
Console.WriteLine($"{items.Count} items ({unit}, {selection.Kind}; {plan.FlaggedLeftOut} flagged notes left out) → {batchDir} (index.md and items/)");
return 0;

static int Usage()
{
    Console.Error.WriteLine("Usage: PlanNoteItemizer <config.json> <batch-folder>");
    return 2;
}

sealed record Config(string? Plan, string? Unit, string? Select, string[]? Tracks, string? Context, string[]? ContextTracks);
