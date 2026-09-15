using System.Text.Json.Nodes;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.ResultsQuery;

/// <summary>One line of a bar-part field in one result: the item, its story (the locator before the group separator), the field, the parts as split, and the line's ordinal in the result's list.</summary>
public sealed record ResultLine(string Item, string Story, string Field, IReadOnlyList<string> Parts, int Ordinal)
{
    /// <summary>The part at a 1-based position, or empty when the line has fewer parts.</summary>
    public string Part(int position) => position >= 1 && position <= Parts.Count ? Parts[position - 1] : "";
}

/// <summary>A line whose part count differs from the field's declared columns; kept in the lines, listed under health.</summary>
public sealed record ArityProblem(string Item, string Field, int Ordinal, int Parts, int Declared);

/// <summary>
/// A batch read for querying: what the tally reads (each index item answered, malformed or
/// missing) and, over the answered results, every line of every bar-part field split into its
/// parts. Reads the batch's own files and nothing else; the answered count is the snapshot a
/// query string carries.
/// </summary>
public sealed class LoadedBatch
{
    public const string DefaultGroupSeparator = "#";

    public required string BatchId { get; init; }
    public required string DirectionsHash { get; init; }
    public required IReadOnlyList<PartField> Fields { get; init; }
    public required IReadOnlyList<ResultLine> Lines { get; init; }
    public required IReadOnlyList<ArityProblem> Arity { get; init; }
    public required int Items { get; init; }
    public required int Answered { get; init; }
    public required IReadOnlyList<(string Item, string Problem)> Malformed { get; init; }
    public required IReadOnlyList<string> Missing { get; init; }
    /// <summary>Items per story from the index, answered and total, in index order of first appearance.</summary>
    public required IReadOnlyList<(string Story, int Answered, int Items)> Stories { get; init; }
    public required string GroupSeparator { get; init; }

    public PartField? Field(string key) => Fields.FirstOrDefault(f => f.Key == key);

    /// <summary>Reads a batch from its definition; the batch id is <c>&lt;study&gt;/&lt;batch&gt;</c>, the prefix of a cites token.</summary>
    public static LoadedBatch Load(string definitionPath, string groupSeparator = DefaultGroupSeparator)
    {
        var definition = DefinitionFile.Read(definitionPath);
        if (definition.Problems.Count > 0) throw new InvalidOperationException($"{definitionPath}: {definition.Problems[0].Message}");
        if (definition.DirectionsPath is null || !File.Exists(definition.DirectionsPath)) throw new InvalidOperationException($"{definitionPath}: the directions line resolves to no file");
        var directions = DirectionsFile.Read(definition.DirectionsPath);
        if (!File.Exists(definition.IndexPath)) throw new InvalidOperationException($"no index at {definition.IndexPath}");
        var index = IndexFile.Read(definition.IndexPath);
        var calls = CallsFile.Read(definition.CallsPath);
        var rows = index.Rows.Select(r =>
        {
            var path = Path.Combine(definition.ResultsDir, r.Item + ".md");
            var text = File.Exists(path) && calls.HasSucceeded(r.Item) ? File.ReadAllText(path) : null;
            return (r, text);
        });
        return Read($"{definition.StudyName}/{definition.Batch}", directions, rows, groupSeparator);
    }

    /// <summary>The pure read: each index row with its result text, or null when the item has no successful call.</summary>
    public static LoadedBatch Read(string batchId, DirectionsFile directions, IEnumerable<(IndexRow Row, string? Text)> rows, string groupSeparator = DefaultGroupSeparator)
    {
        var fields = PartField.Of(directions);
        var lines = new List<ResultLine>();
        var arity = new List<ArityProblem>();
        var malformed = new List<(string, string)>();
        var missing = new List<string>();
        var perStory = new Dictionary<string, (int Answered, int Items)>(StringComparer.Ordinal);
        var storyOrder = new List<string>();
        var items = 0;
        var answered = 0;
        foreach (var (row, text) in rows)
        {
            items++;
            var story = StoryOf(row.Locator, groupSeparator);
            if (!perStory.ContainsKey(story)) { perStory[story] = (0, 0); storyOrder.Add(story); }
            perStory[story] = (perStory[story].Answered, perStory[story].Items + 1);
            if (text is null) { missing.Add(row.Item); continue; }
            var (answer, problems) = ResultFile.Parse(directions, text);
            if (problems.Count > 0) { malformed.Add((row.Item, problems[0])); continue; }
            answered++;
            perStory[story] = (perStory[story].Answered + 1, perStory[story].Items);
            foreach (var f in fields)
            {
                if (answer[f.Key] is not JsonArray arr) continue;
                var ordinal = 0;
                foreach (var node in arr)
                {
                    ordinal++;
                    var value = node?.GetValue<string>() ?? "";
                    var parts = PartField.SplitParts(value);
                    if (parts.Count != f.Columns.Count) arity.Add(new ArityProblem(row.Item, f.Key, ordinal, parts.Count, f.Columns.Count));
                    lines.Add(new ResultLine(row.Item, story, f.Key, parts, ordinal));
                }
            }
        }
        return new LoadedBatch
        {
            BatchId = batchId,
            DirectionsHash = directions.BodyHash,
            Fields = fields,
            Lines = lines,
            Arity = arity,
            Items = items,
            Answered = answered,
            Malformed = malformed,
            Missing = missing,
            Stories = storyOrder.Select(s => (s, perStory[s].Answered, perStory[s].Items)).ToList(),
            GroupSeparator = groupSeparator,
        };
    }

    /// <summary>The locator's text before the group separator, or the whole locator when it has none.</summary>
    public static string StoryOf(string locator, string separator)
    {
        var i = locator.IndexOf(separator, StringComparison.Ordinal);
        return i < 0 ? locator : locator[..i];
    }
}
