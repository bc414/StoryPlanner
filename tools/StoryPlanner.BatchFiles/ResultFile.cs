using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace StoryPlanner.BatchFiles;

/// <summary>
/// A result is <c>results/&lt;item&gt;.md</c>: the directions' What to produce fields in that
/// order, an enum or line as <c>- key: value</c>, a block with continuation lines, a list of
/// line as bullets. The runner renders the model's JSON to it and the tally and the checker
/// read it back through the same code, the render and the parse each other's inverse.
/// </summary>
public static class ResultFile
{
    /// <summary>The JSON Schema the CLI enforces on the answer, derived from What to produce and the Classes; written nowhere.</summary>
    public static JsonObject Schema(DirectionsFile directions)
    {
        var properties = new JsonObject();
        var required = new JsonArray();
        foreach (var f in directions.Output)
        {
            properties[f.Key] = f.Kind switch
            {
                OutputKind.Enum => new JsonObject { ["enum"] = new JsonArray(directions.Classes.Select(c => (JsonNode)c.Label).ToArray()) },
                OutputKind.Line => new JsonObject { ["type"] = "string", ["pattern"] = "^[^\\n]*$" },
                OutputKind.Block => new JsonObject { ["type"] = "string" },
                OutputKind.ListOfLine => new JsonObject { ["type"] = "array", ["items"] = new JsonObject { ["type"] = "string", ["pattern"] = "^[^\\n]*$" } },
                _ => throw new InvalidOperationException(),
            };
            required.Add(f.Key);
        }
        return new JsonObject
        {
            ["type"] = "object",
            ["properties"] = properties,
            ["required"] = required,
            ["additionalProperties"] = false,
        };
    }

    public static string SchemaText(DirectionsFile directions)
        => Schema(directions).ToJsonString(new JsonSerializerOptions { WriteIndented = false });

    /// <summary>The Markdown for one answer; a key the JSON lacks renders empty, a key it adds is left out, so the file is exactly the declaration.</summary>
    public static string Render(DirectionsFile directions, JsonObject answer)
    {
        var sb = new StringBuilder();
        foreach (var f in directions.Output)
        {
            var node = answer[f.Key];
            switch (f.Kind)
            {
                case OutputKind.Enum:
                case OutputKind.Line:
                    sb.Append(KeyedLines.RenderLine(f.Key, OneLine(node)));
                    break;
                case OutputKind.Block:
                    sb.Append(KeyedLines.RenderBlock(f.Key, node?.GetValueKind() == JsonValueKind.String ? node.GetValue<string>() : OneLine(node)));
                    break;
                case OutputKind.ListOfLine:
                    var items = node is JsonArray arr ? arr.Select(OneLine) : [];
                    sb.Append(KeyedLines.RenderList(f.Key, items));
                    break;
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }

    static string OneLine(JsonNode? node) => node switch
    {
        null => "",
        JsonValue v when v.TryGetValue<string>(out var s) => s.Replace("\r", "").Replace('\n', ' ').Trim(),
        _ => node.ToJsonString(),
    };

    /// <summary>Reads a result back to the object the schema validates; problems name what does not match the declaration.</summary>
    public static (JsonObject Answer, IReadOnlyList<string> Problems) Parse(DirectionsFile directions, string text)
    {
        var problems = new List<string>();
        var block = KeyedLines.Read(Hashing.NormalizeNewlines(text).Split('\n'));
        foreach (var s in block.Stray) problems.Add($"line {s.Line}: neither a keyed line nor a continuation");
        var answer = new JsonObject();
        var declared = directions.Output.Select(o => o.Key).ToHashSet(StringComparer.Ordinal);
        foreach (var f in block.Fields)
        {
            var spec = directions.Output.FirstOrDefault(o => o.Key == f.Key);
            if (spec is null) { problems.Add($"line {f.Line}: '{f.Key}' is not a declared field"); continue; }
            if (answer.ContainsKey(f.Key)) { problems.Add($"line {f.Line}: '{f.Key}' repeats"); continue; }
            answer[f.Key] = spec.Kind switch
            {
                OutputKind.ListOfLine => new JsonArray(f.ListItems.Select(i => (JsonNode)i).ToArray()),
                OutputKind.Block => f.BlockText,
                _ => f.Value,
            };
            if (spec.Kind is OutputKind.Enum or OutputKind.Line && f.HasContinuation)
                problems.Add($"line {f.Line}: '{f.Key}' is one line");
        }
        var order = block.Fields.Select(f => f.Key).Where(declared.Contains).ToList();
        var expected = directions.Output.Select(o => o.Key).Where(order.Contains).ToList();
        if (!order.SequenceEqual(expected)) problems.Add("the fields are not in the declared order");
        foreach (var o in directions.Output.Where(o => !answer.ContainsKey(o.Key))) problems.Add($"'{o.Key}' is missing");
        foreach (var o in directions.Output.Where(o => o.Kind == OutputKind.Enum && answer[o.Key] is JsonValue v && !directions.Classes.Any(c => c.Label == v.GetValue<string>())))
            problems.Add($"'{o.Key}' is '{answer[o.Key]}', not one of the classes");
        return (answer, problems);
    }
}
