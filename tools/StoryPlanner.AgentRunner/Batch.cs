using System.Text.Json.Nodes;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.AgentRunner;

/// <summary>What one call of the CLI is made of, composed from the batch's files: the directions body as the system prompt, the item's text as the message, and the hashes every call cites.</summary>
public sealed record CallPlan(string Item, string SystemPrompt, string ItemText, string DirectionsHash, string ItemHash, string PromptHash, string SchemaJson)
{
    public int Characters => SystemPrompt.Length + ItemText.Length;
}

/// <summary>
/// A batch as the runner reads it (decisions.md, "A definition names its directions by path;
/// the runner takes a definition and knows no root"): the definition, the directions it
/// names, the index beside it and the item bodies. Every verb takes the definition's path and
/// resolves the rest relative to it; the runner holds no root, no folder rule and no notion
/// of a study, and never re-checks a definition against its schema, which is the checker's.
/// </summary>
public sealed class Batch
{
    public DefinitionFile Definition { get; }
    public DirectionsFile Directions { get; }
    public IndexFile Index { get; }
    /// <summary>The batch folder relative to the host's working directory with forward slashes, or absolute when it sits elsewhere.</summary>
    public string Id { get; }
    public string SchemaJson { get; }

    public string Dir => Definition.BatchDir;
    public string Name => Definition.Batch;
    public string Study => Definition.StudyName;
    public string Model => Definition.Model!;
    public string? Effort => Definition.Effort;
    public IReadOnlyList<string> Items => Index.Rows.Select(r => r.Item).ToList();

    Batch(DefinitionFile definition, DirectionsFile directions, IndexFile index, string id)
    {
        Definition = definition; Directions = directions; Index = index; Id = id;
        SchemaJson = ResultFile.SchemaText(directions);
    }

    public static string IdFor(string batchDir, string workingDir)
    {
        var rel = Path.GetRelativePath(workingDir, batchDir);
        return rel.StartsWith("..", StringComparison.Ordinal) || Path.IsPathRooted(rel)
            ? Path.GetFullPath(batchDir).Replace('\\', '/')
            : rel.Replace('\\', '/');
    }

    /// <summary>Reads a batch; the error names the first thing missing, so the CLI and the host say the same.</summary>
    public static (Batch? Batch, string? Error) Load(string definitionPath, string workingDir)
    {
        definitionPath = Path.GetFullPath(definitionPath);
        if (!File.Exists(definitionPath)) return (null, $"No definition at {definitionPath}");
        var definition = DefinitionFile.Read(definitionPath);
        if (definition.Problems.Count > 0) return (null, $"{definitionPath}: {definition.Problems[0].Message}");
        if (definition.DirectionsPath is null || !File.Exists(definition.DirectionsPath))
            return (null, $"{definitionPath}: the directions line resolves to no file ({definition.Fields.Value("directions")})");
        var directions = DirectionsFile.Read(definition.DirectionsPath);
        if (directions.Problems.Count > 0) return (null, $"{definition.DirectionsPath}: {directions.Problems[0].Message}");
        if (directions.Output.Count == 0) return (null, $"{definition.DirectionsPath}: What to produce declares no field");
        if (!File.Exists(definition.IndexPath)) return (null, $"No index beside the definition: {definition.IndexPath}");
        var index = IndexFile.Read(definition.IndexPath);
        if (index.Problems.Count > 0) return (null, $"{definition.IndexPath}: {index.Problems[0].Message}");
        if (index.Rows.Count == 0) return (null, $"{definition.IndexPath}: the index lists no item");
        if (definition.McpPath is not null && !File.Exists(definition.McpPath)) return (null, $"{definitionPath}: mcp resolves to no file ({definition.McpPath})");
        return (new Batch(definition, directions, index, IdFor(definition.BatchDir, workingDir)), null);
    }

    public string ItemPath(string item) => Path.Combine(Definition.ItemsDir, item + ".md");

    /// <summary>The items whose bodies are missing under items/; the itemizer regenerates them.</summary>
    public IReadOnlyList<string> MissingItems() => Items.Where(i => !File.Exists(ItemPath(i))).ToList();

    public CallPlan Compose(string item)
    {
        var text = Hashing.NormalizeNewlines(File.ReadAllText(ItemPath(item)));
        var itemHash = Hashing.Sha256Hex(text);
        var promptHash = Hashing.Sha256Hex(Directions.Body + "\n---\n" + text);
        return new CallPlan(item, Directions.Body, text, Directions.BodyHash, itemHash, promptHash, SchemaJson);
    }

    /// <summary>
    /// The <c>claude</c> argument list for one call: print mode, the batch's model and effort,
    /// no transcript, JSON events per line, restricted, the directions as the system prompt
    /// from a file (a body can be longer than a command line allows), the answer's JSON Schema,
    /// the exact toolset (<c>--tools ""</c> disables all), strict MCP with the definition's
    /// config only when it opts in. The model writes no file, so no directory is granted.
    /// </summary>
    public IReadOnlyList<string> BuildArgs(string systemPromptFile)
    {
        var args = new List<string>
        {
            "-p",
            "--model", Model,
            "--no-session-persistence",
            "--output-format", "stream-json",
            "--verbose",
            // Without this the answer itself is written in silence: the thinking_tokens lines
            // stop when thinking ends and the finished message is the next line, minutes later
            // (probe of 2026-09-09: 112 s of nothing for a 9,800-token answer). With it the
            // harness writes a delta line every few tokens, so the idle limit measures silence.
            "--include-partial-messages",
            "--permission-mode", "auto",
            "--restricted",
            "--disable-slash-commands",
            "--strict-mcp-config",
            "--system-prompt-file", systemPromptFile,
            "--json-schema", SchemaJson,
            "--tools",
        };
        if (Definition.Tools.Count == 0) args.Add("");
        else args.AddRange(Definition.Tools);
        if (Effort is not null) { args.Add("--effort"); args.Add(Effort); }
        if (Definition.McpPath is not null) { args.Add("--mcp-config"); args.Add(Definition.McpPath); }
        if (Definition.Tools.Count > 0) { args.Add("--allowed-tools"); args.AddRange(Definition.Tools); }
        return args;
    }

    /// <summary>The launch-folder invariants: outside the definition's repository, and carrying no instruction stack of its own.</summary>
    public static string? CheckLaunchDir(string launchDir, string definitionPath)
    {
        if (string.IsNullOrWhiteSpace(launchDir) || !Directory.Exists(launchDir))
            return $"launchDir does not exist: {launchDir}";
        launchDir = Path.GetFullPath(launchDir);
        var repoRoot = FindRepoRoot(definitionPath);
        if (repoRoot is not null && IsSameOrUnder(launchDir, repoRoot))
            return $"launchDir must be OUTSIDE the repo ({repoRoot}) — that is the whole point.";
        foreach (var forbidden in new[] { "CLAUDE.md", ".claude", ".mcp.json" })
            if (File.Exists(Path.Combine(launchDir, forbidden)) || Directory.Exists(Path.Combine(launchDir, forbidden)))
                return $"launchDir contains {forbidden}; it must carry no instruction stack of its own.";
        return null;
    }

    public static string? FindRepoRoot(string fromPath)
    {
        var d = new DirectoryInfo(Path.GetDirectoryName(Path.GetFullPath(fromPath))!);
        while (d != null)
        {
            if (Directory.Exists(Path.Combine(d.FullName, ".git"))) return d.FullName;
            d = d.Parent;
        }
        return null;
    }

    // Segment-aware: "…\StoryPlanner-fanout" is NOT under "…\StoryPlanner", though it starts with it.
    public static bool IsSameOrUnder(string path, string root)
    {
        var p = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
        var r = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
        return p.Equals(r, StringComparison.OrdinalIgnoreCase)
            || p.StartsWith(r + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            || p.StartsWith(r + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>The result file the runner renders from the model's answer, and what the tally reads back.</summary>
    public string ResultPath(string item) => Path.Combine(Definition.ResultsDir, item + ".md");

    public string Render(JsonObject answer) => ResultFile.Render(Directions, answer);
}
