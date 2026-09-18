using System.Text.Json;
using StoryPlanner.BatchFiles;
using StoryPlanner.TurnItemizer;

// The turn itemizer: cuts dialogue into one batch's items and writes its index.md, with an
// itemizer line, and the bodies under items/, refusing a batch that already has an index.
//
//   dotnet run --project tools/StoryPlanner.TurnItemizer -- <config.json> <batch-folder>
//
// The config is authored and lives with the tool:
//   {"unit": "user-turns" | "question-endings", "layers": ["gemini", "aistudio", "notebooklm", "conversations"],
//    "lineage": "<lineage.db>", "plan": "<working-plan .storyplan, for the conversations layer>"}
//   {"unit": "session-decisions", "codeSessions": "<codesessions.db>", "kinds": ["answers", "verdicts", "prompts"]}
// Layers the config leaves out are named in the narrowing.

if (args.Length < 2) return Usage();
var configPath = Path.GetFullPath(args[0]);
var batchDir = Path.GetFullPath(args[1]);
if (!File.Exists(configPath)) { Console.Error.WriteLine($"config not found: {configPath}"); return 2; }
if (ItemizerOutput.Refusal(batchDir) is { } refusal) { Console.Error.WriteLine(refusal); return 2; }
var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

IReadOnlyList<ItemizerOutput.Item> items;
string locatorNotation;
string narrowing;
switch (config?.Unit)
{
    case "user-turns" or "question-endings":
    {
        var layers = config.Layers ?? [];
        var unknown = layers.Where(l => !Sources.Layers.Contains(l)).ToList();
        if (layers.Length == 0 || unknown.Count > 0) { Console.Error.WriteLine($"layers must name some of {string.Join(", ", Sources.Layers)}"); return 2; }
        var needsLineage = layers.Any(l => l != Sources.Conversations);
        if (needsLineage && (config.Lineage is null || !File.Exists(config.Lineage))) { Console.Error.WriteLine($"lineage not found: {config.Lineage}"); return 2; }
        if (layers.Contains(Sources.Conversations) && (config.Plan is null || !File.Exists(config.Plan))) { Console.Error.WriteLine($"plan not found: {config.Plan}"); return 2; }

        var conversations = new List<Turns.Conversation>();
        foreach (var layer in Sources.Layers.Where(layers.Contains))
            conversations.AddRange(layer switch
            {
                Sources.Gemini => Sources.ReadGemini(config.Lineage!),
                Sources.AiStudio => Sources.ReadAiStudio(config.Lineage!),
                Sources.NotebookLm => Sources.ReadNotebookLm(config.Lineage!),
                _ => Sources.ReadConversations(config.Plan!),
            });
        items = config.Unit == "user-turns" ? Turns.UserTurns(conversations) : Turns.QuestionEndings(conversations);
        locatorNotation = Turns.LocatorNotation;
        var left = Sources.Layers.Where(l => !layers.Contains(l)).ToList();
        narrowing = (config.Unit == "user-turns" ? Turns.UserTurnsNarrowing : Turns.QuestionEndingsNarrowing)
            + $"; in the layers {string.Join(", ", Sources.Layers.Where(layers.Contains))}"
            + (left.Count > 0 ? $", the layers {string.Join(", ", left)} left out by the config" : "");
        break;
    }
    case "session-decisions":
    {
        if (config.CodeSessions is null || !File.Exists(config.CodeSessions)) { Console.Error.WriteLine($"codeSessions not found: {config.CodeSessions}"); return 2; }
        var kinds = (config.Kinds ?? []).ToHashSet(StringComparer.Ordinal);
        if (kinds.Count == 0 || kinds.Any(k => !SessionDecisions.Kinds.Contains(k))) { Console.Error.WriteLine($"kinds must name some of {string.Join(", ", SessionDecisions.Kinds)}"); return 2; }
        var (sessions, records) = SessionDecisions.Load(config.CodeSessions);
        items = SessionDecisions.Cut(sessions, records, kinds);
        locatorNotation = SessionDecisions.LocatorNotation;
        narrowing = SessionDecisions.Narrowing(SessionDecisions.Kinds.Where(kinds.Contains).ToList());
        break;
    }
    default:
        Console.Error.WriteLine($"unknown unit '{config?.Unit}': user-turns, question-endings or session-decisions");
        return 2;
}

var problems = ItemizerOutput.Problems(items);
if (problems.Count > 0) { foreach (var p in problems) Console.Error.WriteLine(p); return 2; }
ItemizerOutput.Write(batchDir, ItemizerOutput.ItemizerLine("StoryPlanner.TurnItemizer"), locatorNotation, items, narrowing);
Console.WriteLine($"{items.Count} items ({config.Unit}) → {batchDir} (index.md and items/)");
return 0;

static int Usage()
{
    Console.Error.WriteLine("Usage: TurnItemizer <config.json> <batch-folder>");
    return 2;
}

sealed record Config(string? Unit, string[]? Layers, string? Lineage, string? Plan, string? CodeSessions, string[]? Kinds);
