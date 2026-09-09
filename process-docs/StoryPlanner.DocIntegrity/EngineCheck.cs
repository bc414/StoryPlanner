namespace StoryPlanner.DocIntegrity;

/// <summary>Everything the engine produced for one governed file: the Shape it was held to, the object it read, and every problem, to be mapped to the class's check ids.</summary>
public sealed record EngineResult(Shape Shape, ParsedDocument Document, IReadOnlyList<EngineProblem> Problems, bool ShapeUnavailable)
{
    public IEnumerable<EngineProblem> In(string section) => Problems.Where(p => p.Section == section);
    public IEnumerable<EngineProblem> ForKey(string key) => Problems.Where(p => p.Key == key);
}

/// <summary>
/// One entry point for a checker built on the engine: read the class's Shape from its schema
/// file in the governing skill folder, read the governed file to its object, validate it by
/// the standard, resolve its references, and hand back every problem. The class's own rules
/// run in the checker afterwards. A schema file that cannot be read is reported as
/// information, never as a failure of the governed file.
/// </summary>
public static class EngineCheck
{
    static readonly Dictionary<string, Shape> Cache = new(StringComparer.Ordinal);

    public static Shape? ShapeOf(string schemaId, CheckContext ctx)
    {
        var path = Path.Combine(ctx.SkillFolder, SkillReader.SchemasFolder, schemaId + ".md");
        if (!File.Exists(path)) return null;
        var key = path + "|" + File.GetLastWriteTimeUtc(path).Ticks;
        lock (Cache)
        {
            if (!Cache.TryGetValue(key, out var shape)) Cache[key] = shape = ShapeReader.Read(path);
            return shape;
        }
    }

    public static EngineResult Run(string schemaId, CheckContext ctx, string filePath, IReadOnlyDictionary<string, IReadOnlyCollection<string>>? openEnums = null)
    {
        var shape = ShapeOf(schemaId, ctx);
        if (shape is null || shape.Problems.Count > 0 && shape.Sections.Count == 0)
            return new EngineResult(shape ?? new Shape([], []), new ParsedDocument(new(), null, 0, [], [], new Dictionary<string, IReadOnlyList<string>>()), [], ShapeUnavailable: true);
        var doc = DocumentReader.Read(shape, File.ReadAllText(filePath));
        var problems = new List<EngineProblem>(doc.Problems);
        problems.AddRange(ShapeEngine.Validate(shape, doc, openEnums));
        problems.AddRange(References.Resolve(shape, doc, filePath, ctx));
        return new EngineResult(shape, doc, problems.DistinctBy(p => (p.Kind, p.Message)).ToList(), ShapeUnavailable: false);
    }

    public static Finding Unavailable(string schemaId, string file)
        => Finding.Info("check.schema-unavailable", file, $"schemas/{schemaId}.md could not be read in the governing skill folder; the engine's checks did not run");
}
