using System.Text.RegularExpressions;

namespace StoryPlanner.BatchFiles;

public sealed record DefinitionProblem(string Message, int Line);

/// <summary>
/// A batch's definition (definition-schema): the one authored file in a batch folder, read
/// by every runner verb and held by the checker. Paths are relative to the file; the reader
/// resolves them and hashes the file's text, which the calls file records at the first
/// execution and the checker holds the file to afterwards.
/// </summary>
public sealed class DefinitionFile
{
    public static readonly string[] Keys = ["directions", "kind", "calibration", "model", "effort", "tools", "mcp"];
    public static readonly string[] Kinds = ["sample", "full"];
    public static readonly string[] Efforts = ["low", "medium", "high", "max"];
    static readonly Regex BatchName = new(@"^(?<n>[0-9]{2})-(?<slug>[a-z0-9-]+)$", RegexOptions.Compiled);

    public string Path { get; }
    public string BatchDir { get; }
    public string Batch { get; }
    public string? Title { get; }
    public string Hash { get; }
    public KeyedBlock Fields { get; }
    public IReadOnlyList<DefinitionProblem> Problems { get; }

    public string? DirectionsPath { get; }
    public string? Kind { get; }
    public string? CalibrationPath { get; }
    public string? Model { get; }
    public string? Effort { get; }
    public IReadOnlyList<string> Tools { get; }
    public string? McpPath { get; }

    /// <summary>The batch folder's number and slug, or null when the folder is not <c>nn-slug</c>.</summary>
    public (int Number, string Slug)? BatchParts
    {
        get
        {
            var m = BatchName.Match(Batch);
            return m.Success ? (int.Parse(m.Groups["n"].Value), m.Groups["slug"].Value) : null;
        }
    }

    public string IndexPath => System.IO.Path.Combine(BatchDir, "index.md");
    public string CallsPath => System.IO.Path.Combine(BatchDir, "calls.md");
    public string TallyPath => System.IO.Path.Combine(BatchDir, "tally.md");
    public string ItemsDir => System.IO.Path.Combine(BatchDir, "items");
    public string ResultsDir => System.IO.Path.Combine(BatchDir, "results");
    public string AttemptsDir => System.IO.Path.Combine(BatchDir, "attempts");
    /// <summary>The folder above <c>batches/</c>: the study, or wherever the batch was defined.</summary>
    public string StudyDir => System.IO.Path.GetFullPath(System.IO.Path.Combine(BatchDir, "..", ".."));
    public string StudyName => System.IO.Path.GetFileName(StudyDir);

    DefinitionFile(string path, string text)
    {
        Path = System.IO.Path.GetFullPath(path);
        BatchDir = System.IO.Path.GetDirectoryName(Path)!;
        Batch = System.IO.Path.GetFileName(BatchDir);
        Hash = Hashing.Sha256Hex(text);
        var problems = new List<DefinitionProblem>();
        var lines = Hashing.NormalizeNewlines(text).Split('\n');
        var i = 0;
        while (i < lines.Length && lines[i].Trim().Length == 0) i++;
        if (i < lines.Length && lines[i].StartsWith("# ", StringComparison.Ordinal)) { Title = lines[i][2..].Trim(); i++; }
        Fields = KeyedLines.Read(lines.Skip(i).ToList(), i + 1);
        foreach (var s in Fields.Stray) problems.Add(new DefinitionProblem($"line {s.Line}: neither a keyed line nor a continuation", s.Line));
        var keys = Fields.Fields.Select(f => f.Key).ToList();
        foreach (var f in Fields.Fields.Where(f => !Keys.Contains(f.Key))) problems.Add(new DefinitionProblem($"line {f.Line}: '{f.Key}' is not a key; the keys are {string.Join(", ", Keys)}", f.Line));
        foreach (var d in keys.GroupBy(k => k).Where(g => g.Count() > 1)) problems.Add(new DefinitionProblem($"'{d.Key}' appears twice", Fields.Field(d.Key)!.Line));
        foreach (var k in new[] { "directions", "model" }.Where(k => !keys.Contains(k))) problems.Add(new DefinitionProblem($"missing {k}", i + 1));
        var order = keys.Where(Keys.Contains).Select(k => Array.IndexOf(Keys, k)).ToList();
        if (order.Zip(order.Skip(1)).Any(p => p.Second <= p.First)) problems.Add(new DefinitionProblem("the keys are in the order " + string.Join(", ", Keys), i + 1));
        foreach (var f in Fields.Fields.Where(f => f.Key != "tools" && f.Value.Length == 0)) problems.Add(new DefinitionProblem($"line {f.Line}: '{f.Key}' has no value on its line", f.Line));
        foreach (var f in Fields.Fields.Where(f => f.Key != "tools" && f.HasContinuation)) problems.Add(new DefinitionProblem($"line {f.Line}: '{f.Key}' is one line", f.Line));

        DirectionsPath = Resolve(Fields.Value("directions"));
        Kind = Fields.Value("kind");
        if (Kind is not null && !Kinds.Contains(Kind)) problems.Add(new DefinitionProblem($"kind '{Kind}' is not sample or full", Fields.Field("kind")!.Line));
        CalibrationPath = Resolve(Fields.Value("calibration"));
        Model = Fields.Value("model");
        Effort = Fields.Value("effort");
        if (Effort is not null && !Efforts.Contains(Effort)) problems.Add(new DefinitionProblem($"effort '{Effort}' is not low, medium, high or max", Fields.Field("effort")!.Line));
        var tools = Fields.Field("tools");
        Tools = tools is null ? [] : tools.ListItems.Count > 0 ? tools.ListItems : tools.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        McpPath = Resolve(Fields.Value("mcp"));
        Problems = problems;
    }

    string? Resolve(string? relative) => string.IsNullOrWhiteSpace(relative) ? null : System.IO.Path.GetFullPath(relative, BatchDir);

    public static DefinitionFile Read(string path) => new(path, File.ReadAllText(path));

    /// <summary>The text a session writes; the runner never writes one.</summary>
    public static string Render(string batch, string directions, string? kind, string? calibration, string model, string? effort, IEnumerable<string>? tools, string? mcp)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("# ").Append(batch).Append(" — definition\n\n");
        sb.Append(KeyedLines.RenderLine("directions", directions)).Append('\n');
        if (kind is not null) sb.Append(KeyedLines.RenderLine("kind", kind)).Append('\n');
        if (calibration is not null) sb.Append(KeyedLines.RenderLine("calibration", calibration)).Append('\n');
        sb.Append(KeyedLines.RenderLine("model", model)).Append('\n');
        if (effort is not null) sb.Append(KeyedLines.RenderLine("effort", effort)).Append('\n');
        var toolList = tools?.ToList() ?? [];
        if (toolList.Count > 0) sb.Append(KeyedLines.RenderList("tools", toolList)).Append('\n');
        if (mcp is not null) sb.Append(KeyedLines.RenderLine("mcp", mcp)).Append('\n');
        return sb.ToString();
    }
}
