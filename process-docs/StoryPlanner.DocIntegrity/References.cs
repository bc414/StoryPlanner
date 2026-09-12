using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// The engine resolves every typed reference itself (decisions.md, the Shape grammar):
/// <c>token of &lt;class&gt;</c> to an entry heading in one of that class's files, <c>id of
/// &lt;class&gt;</c> to a file of that class, <c>path to &lt;class&gt;</c> and <c>path</c> to
/// a file relative to the referencing one. The classes' files are found through the
/// Artifacts table, never by a path the engine knows.
/// </summary>
public static class References
{
    static readonly Regex EntryHeading = new(@"^### (?<h>.+)$", RegexOptions.Compiled);

    /// <summary>Every problem a reference in the document raises: an unresolved token, id or path.</summary>
    public static IReadOnlyList<EngineProblem> Resolve(Shape shape, ParsedDocument doc, string filePath, CheckContext ctx)
    {
        var problems = new List<EngineProblem>();
        foreach (var (section, field, type) in shape.References())
        {
            var values = Values(doc, section, field);
            foreach (var (value, line) in values)
            {
                var problem = type.Name switch
                {
                    TypeName.Token => ResolveToken(type.TargetClass!, value, ctx),
                    TypeName.Id => ResolveId(type.TargetClass!, value, ctx),
                    TypeName.PathTo => ResolvePath(value, filePath),
                    TypeName.Path => ResolvePath(value, filePath),
                    _ => null,
                };
                if (problem is not null)
                    problems.Add(new EngineProblem(section.Property, field.Key, ProblemKind.Reference, $"line {line}: {field.Key}: {problem}", line));
            }
        }
        return problems;
    }

    static IEnumerable<(string Value, int Line)> Values(ParsedDocument doc, ShapeSection section, ShapeField field)
    {
        var node = doc.Root[section.Property];
        if (node is null) yield break;
        IEnumerable<(JsonObject Obj, int Line)> objects = section.Holds switch
        {
            Holds.Fields => [((JsonObject)node, doc.TitleLine)],
            Holds.Entries => ((JsonArray)node).Select((n, i) => ((JsonObject)n!, doc.Entries.Where(e => e.Section == section.Name).Skip(i).FirstOrDefault()?.Line ?? doc.TitleLine)),
            Holds.Table => ((JsonArray)node).Select(n => ((JsonObject)n!, n!["_line"]?.GetValue<int>() ?? doc.TitleLine)),
            _ => [],
        };
        foreach (var (obj, line) in objects)
        {
            var v = obj[field.Key];
            if (v is null) continue;
            var fieldLine = doc.Entries.FirstOrDefault(e => e.Line == line)?.FieldLines.GetValueOrDefault(field.Key, line) ?? line;
            if (v is JsonArray arr) foreach (var item in arr) yield return (item?.ToString() ?? "", fieldLine);
            else yield return (v.ToString(), fieldLine);
        }
    }

    /// <summary>The files of a class, through the Artifacts table of the governing skill folder; null when the table has no row for it.</summary>
    public static IReadOnlyList<string>? FilesOf(string artifactClass, CheckContext ctx)
    {
        IReadOnlyList<ArtifactRow> rows;
        try { rows = SkillReader.ReadArtifacts(ctx.SkillFolder); }
        catch (MapFormatException) { return null; }
        var row = rows.FirstOrDefault(r => r.Id == artifactClass) ?? rows.FirstOrDefault(r => r.Id.StartsWith(artifactClass, StringComparison.Ordinal));
        if (row is null || !ArtifactPath.TryParse(row.Path, out var ap, out _) || ap!.OutsideRepo) return null;
        return StateBuilder.Matches(ctx.RepoRoot, ap, null, null);
    }

    /// <summary>The class a token type's target names, as the Artifacts table calls it: a schema-ish name maps to the row it describes.</summary>
    static string ClassRow(string target) => target switch
    {
        "question-list" => WellKnown.QuestionList,
        "hypothesis-file" => WellKnown.HypothesisRecord,
        _ => target,
    };

    static string? ResolveToken(string targetClass, string token, CheckContext ctx)
    {
        var slash = token.IndexOf('/');
        if (slash <= 0) return $"'{token}' is not <file>/<slug>";
        var files = FilesOf(ClassRow(targetClass), ctx);
        if (files is null) return null; // no row: reported as information by the checker that asked
        var owner = token[..slash];
        var file = files.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == owner || Path.GetFileName(Path.GetDirectoryName(f)!) == owner);
        if (file is null) return $"'{token}' names no {targetClass} file '{owner}'";
        var headings = File.ReadAllLines(file).Select(l => EntryHeading.Match(l)).Where(m => m.Success).Select(m => m.Groups["h"].Value.Trim());
        return headings.Contains(token, StringComparer.Ordinal) ? null : $"'{token}' names no entry in {Path.GetFileName(file)}";
    }

    static string? ResolveId(string targetClass, string id, CheckContext ctx)
    {
        var files = FilesOf(ClassRow(targetClass), ctx);
        if (files is null) return null;
        return files.Any(f => Path.GetFileName(f).StartsWith(id + "-", StringComparison.Ordinal)) ? null : $"{id} names no {targetClass}";
    }

    static string? ResolvePath(string relative, string fromFile)
    {
        if (relative.Length == 0) return "the path is empty";
        var full = Path.GetFullPath(relative, Path.GetDirectoryName(Path.GetFullPath(fromFile))!);
        return File.Exists(full) ? null : $"'{relative}' does not resolve to a file";
    }

    /// <summary>The absolute target of a path-typed value, resolved from the referencing file; null when empty.</summary>
    public static string? Target(string? relative, string fromFile)
        => string.IsNullOrWhiteSpace(relative) ? null : Path.GetFullPath(relative, Path.GetDirectoryName(Path.GetFullPath(fromFile))!);
}
