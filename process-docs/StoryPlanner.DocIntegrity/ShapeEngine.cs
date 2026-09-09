using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Schema;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// The engine that holds a Shape (decisions.md, "A Shape is one grammar … compiled to JSON
/// Schema and held by the standard"): compiles a Shape to a JSON Schema by substituting each
/// type's fragment, validates the object <see cref="DocumentReader"/> read by a standard
/// validator (JsonSchema.Net), turns the validator's findings back into problems named by
/// section and key, and resolves every reference type itself. The class's own rules, the ones
/// no schema language expresses, run after, in the class's checker.
/// </summary>
public static class ShapeEngine
{
    static readonly JsonSerializerOptions Compact = new() { WriteIndented = false };

    /// <summary>The fragment each type compiles to (schemas/skill-schema.md § A schema file).</summary>
    public static JsonObject Fragment(TypeSpec t, Func<string>? enumSource = null, IReadOnlyCollection<string>? openEnum = null) => t.Name switch
    {
        TypeName.Slug => new JsonObject { ["type"] = "string", ["pattern"] = "^[a-z0-9-]+$" },
        TypeName.Token => new JsonObject { ["type"] = "string", ["pattern"] = "^[a-z0-9-]+/[a-z0-9-]+$" },
        TypeName.Id => new JsonObject { ["type"] = "string", ["pattern"] = "^[0-9]{3}$" },
        TypeName.PathTo or TypeName.Path => new JsonObject { ["type"] = "string", ["minLength"] = 1 },
        TypeName.Date => new JsonObject { ["type"] = "string", ["pattern"] = "^[0-9]{4}-[0-9]{2}-[0-9]{2}$" },
        TypeName.Timestamp => new JsonObject { ["type"] = "string", ["pattern"] = "^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}$" },
        TypeName.Hash => new JsonObject { ["type"] = "string", ["pattern"] = "^[0-9a-f]{64}$" },
        TypeName.Enum => new JsonObject { ["enum"] = new JsonArray((t.EnumLiterals ?? []).Concat(openEnum ?? []).Distinct().Select(v => (JsonNode)v).ToArray()) },
        TypeName.Line => new JsonObject { ["type"] = "string", ["pattern"] = "^[^\\n]*$" },
        TypeName.Block => new JsonObject { ["type"] = "string" },
        TypeName.List => new JsonObject { ["type"] = "array", ["items"] = Fragment(t.Item!, enumSource, openEnum) },
        _ => throw new InvalidOperationException(),
    };

    /// <summary>
    /// The JSON Schema of a whole document under a Shape. <paramref name="openEnums"/> supplies,
    /// per field key, the values an open enum takes from outside the schema (the corpus ids).
    /// </summary>
    public static JsonObject Compile(Shape shape, IReadOnlyDictionary<string, IReadOnlyCollection<string>>? openEnums = null)
    {
        var properties = new JsonObject();
        var required = new JsonArray();
        foreach (var s in shape.Sections)
        {
            JsonObject value = s.Holds switch
            {
                Holds.Prose or Holds.Fenced => new JsonObject { ["type"] = "string" },
                Holds.Fields => FieldsSchema(s.Fields, openEnums),
                Holds.Table => new JsonObject { ["type"] = "array", ["items"] = FieldsSchema(s.Fields, openEnums, row: true) },
                Holds.Entries => s.Fields is null
                    ? new JsonObject { ["type"] = "array", ["items"] = new JsonObject { ["type"] = "string" } }
                    : new JsonObject { ["type"] = "array", ["items"] = FieldsSchema(s.Fields, openEnums, heading: true) },
                _ => throw new InvalidOperationException(),
            };
            if (s.Repeated)
                value = new JsonObject { ["type"] = "array", ["items"] = new JsonObject { ["type"] = "object", ["properties"] = new JsonObject { [DocumentReader.HeadingProperty] = new JsonObject { ["type"] = "string" }, [s.Property] = value }, ["required"] = new JsonArray(s.Property) } };
            properties[s.Property] = value;
            if (s.Required) required.Add(s.Property);
        }
        return new JsonObject { ["type"] = "object", ["properties"] = properties, ["required"] = required, ["additionalProperties"] = false };
    }

    static JsonObject FieldsSchema(FieldTable? table, IReadOnlyDictionary<string, IReadOnlyCollection<string>>? openEnums, bool row = false, bool heading = false)
    {
        var properties = new JsonObject();
        var required = new JsonArray();
        var obj = new JsonObject { ["type"] = "object", ["properties"] = properties };
        if (heading) properties[DocumentReader.HeadingProperty] = new JsonObject { ["type"] = "string" };
        if (row) properties["_line"] = new JsonObject { ["type"] = "integer" };
        if (table is not null)
            foreach (var f in table.Fields)
            {
                var fragment = Fragment(f.Type, null, f.Type.EnumOpen || (f.Type.Item?.EnumOpen ?? false) ? openEnums?.GetValueOrDefault(f.Key) : null);
                if (f.Wildcard)
                {
                    obj["patternProperties"] = new JsonObject { ["^[a-z0-9 -]+$"] = fragment };
                    if (f.Required) obj["minProperties"] = 1;
                    continue;
                }
                properties[f.Key] = fragment;
                if (f.Required) required.Add(f.Key);
            }
        obj["required"] = required;
        if (!obj.ContainsKey("patternProperties")) obj["additionalProperties"] = false;
        return obj;
    }

    /// <summary>Validates a parsed document and returns the validator's findings as problems named by section and key.</summary>
    public static IReadOnlyList<EngineProblem> Validate(Shape shape, ParsedDocument doc, IReadOnlyDictionary<string, IReadOnlyCollection<string>>? openEnums = null)
    {
        var schemaJson = Compile(shape, openEnums).ToJsonString(Compact);
        var schema = JsonSchema.FromText(schemaJson);
        using var instanceDoc = JsonDocument.Parse(doc.Root.ToJsonString(Compact));
        var results = schema.Evaluate(instanceDoc.RootElement, new EvaluationOptions { OutputFormat = OutputFormat.List });
        if (results.IsValid) return [];
        var problems = new List<EngineProblem>();
        foreach (var d in results.Details.Where(d => d.Errors is { Count: > 0 }))
        {
            var pointer = d.InstanceLocation;
            var segments = pointer.ToString().Split('/').Skip(1).Select(s => s.Replace("~1", "/").Replace("~0", "~")).ToList();
            var section = segments.Count > 0 ? segments[0] : "";
            var key = segments.Count > 1 ? segments.Skip(1).LastOrDefault(s => !int.TryParse(s, out _)) : null;
            if (key == section) key = null;
            pointer.TryEvaluate(doc.Root, out var instance);
            var value = instance is JsonValue v && v.TryGetValue<string>(out var str) ? str : instance?.ToJsonString() ?? "";
            var line = LineOf(doc, section, segments, key);
            foreach (var keyword in d.Errors!.Keys)
            {
                // A container keyword failing only says a child failed; the child's own detail names it.
                if (keyword is "properties" or "items" or "patternProperties" or "allOf" or "anyOf" or "$ref") continue;
                var message = keyword switch
                {
                    "pattern" => $"line {line}: '{key ?? section}' is not of its type; found '{value}'",
                    "enum" => $"line {line}: '{key ?? section}' is '{value}', not one of its values",
                    "type" => $"line {line}: '{key ?? section}' is not of its type",
                    "required" => $"line {line}: {section}: a required key is missing",
                    "additionalProperties" => $"line {line}: {section}: a key the shape does not declare",
                    "minProperties" => $"line {line}: {section}: at least one keyed line",
                    "minLength" => $"line {line}: '{key ?? section}' is empty",
                    _ => $"line {line}: {pointer} fails {keyword}",
                };
                problems.Add(new EngineProblem(section, key, ProblemKind.Type, message, line));
            }
        }
        return problems.DistinctBy(p => p.Message).ToList();
    }

    static int LineOf(ParsedDocument doc, string section, List<string> segments, string? key)
    {
        // An entry's line from its position, when the pointer runs through an entries array.
        if (segments.Count > 1 && int.TryParse(segments[1], out var idx))
        {
            var entries = doc.Entries.Where(e => e.Section == section).ToList();
            if (idx < entries.Count)
            {
                var e = entries[idx];
                return key is not null && e.FieldLines.TryGetValue(key, out var l) ? l : e.Line;
            }
            if (doc.Root[section] is JsonArray rows && idx < rows.Count && rows[idx] is JsonObject row && row["_line"] is JsonValue lv && lv.TryGetValue<int>(out var rl))
                return rl;
        }
        return doc.TitleLine;
    }
}
