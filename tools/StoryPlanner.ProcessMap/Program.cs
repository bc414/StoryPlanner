// StoryPlanner.ProcessMap — the validator and renderer for the v3-buildout skill's process map.
//
// The map (.claude/skills/v3-buildout/process-map.md) is a Type Object for the buildout method:
// the columns plus this tool are the schema, the rows are in flux. Iterating the method is
// editing rows and re-running validation, never rewriting a document.
//
//   dotnet run --project tools/StoryPlanner.ProcessMap -- validate .claude/skills/v3-buildout
//   dotnet run --project tools/StoryPlanner.ProcessMap -- render   .claude/skills/v3-buildout [--force]
//   dotnet run --project tools/StoryPlanner.ProcessMap -- nodes    <file.md>
//
// render refuses unless validate passes. --force writes anyway and stamps every generated
// section UNVALIDATED; it exists for reviewing diagrams on a scratchpad COPY, never for the
// real file. A copy outside the repo has no repository root above it, so pass
// --repo <path> to resolve the map's repo-relative paths against the real one.
//
// Exit codes follow the other tools: 0 ok, 1 failure, 2 usage.
//
// Why no ProjectReference to StoryPlanner.AgentRunner for its UnitSplitter (the handoff asked
// this be recorded): AgentRunner is a Microsoft.NET.Sdk.Web project with Markdig and
// OutputType=Exe, so referencing it drags the ASP.NET framework reference into a plain console
// tool. UnitSplitter also emits raw row lines and never splits cells, which is most of the work
// here. MapTables.cs carries the same unit rule — header and separator are structure, every
// body row is a unit — pinned by its own tests.

using StoryPlanner.ProcessMap;

if (args.Length == 0) return Usage();

var verb = args[0];
var force = args.Contains("--force");

var repoIndex = Array.IndexOf(args, "--repo");
var repoOverride = repoIndex >= 0 && repoIndex + 1 < args.Length ? args[repoIndex + 1] : null;

var positional = args.Skip(1).Where(a => !a.StartsWith("--")).ToList();
if (repoOverride is not null) positional.Remove(repoOverride);

try
{
    return verb switch
    {
        "validate" => RunValidate(),
        "render" => RunRender(),
        "nodes" => RunNodes(),
        _ => Usage($"Unknown verb '{verb}'."),
    };
}
catch (MapFormatException ex)
{
    Console.Error.WriteLine("Refusing to guess: " + ex.Message);
    return 1;
}

int RunValidate()
{
    if (positional.Count != 1) return Usage("validate takes one argument: the skill folder.");
    var (repoRoot, skillFolder) = Resolve(positional[0]);
    var report = Validator.Validate(repoRoot, skillFolder);
    PrintReport(report);
    return report.Passed ? 0 : 1;
}

int RunRender()
{
    if (positional.Count != 1) return Usage("render takes one argument: the skill folder.");
    var (repoRoot, skillFolder) = Resolve(positional[0]);
    var report = Validator.Validate(repoRoot, skillFolder);

    if (!report.Passed && !force)
    {
        Console.Error.WriteLine(
            $"render refuses: validate reports {report.Failures} failure(s). " +
            "Fix the rows, or render a scratchpad copy with --force to review the diagrams.");
        PrintReport(report);
        return 1;
    }
    if (!report.Passed)
        Console.Error.WriteLine(
            $"--force: rendering over {report.Failures} failure(s); every section is stamped UNVALIDATED.");

    var mapPath = Path.Combine(skillFolder, "process-map.md");
    var doc = MapReader.Read(File.ReadAllText(mapPath));
    var sections = MermaidRenderer.RenderAll(doc, report, forced: !report.Passed);
    var updated = MarkerWriter.Write(File.ReadAllText(mapPath), sections);
    File.WriteAllText(mapPath, updated);

    Console.WriteLine($"Wrote {sections.Count} generated section(s) to {mapPath}.");
    return 0;
}

int RunNodes()
{
    if (positional.Count != 1) return Usage("nodes takes one argument: a markdown file.");
    var path = Path.GetFullPath(positional[0]);
    if (!File.Exists(path)) { Console.Error.WriteLine($"No such file: {path}"); return 2; }

    var scan = MermaidScanner.Scan(File.ReadAllText(path));
    foreach (var n in scan.Nodes) Console.WriteLine($"node {n}");
    foreach (var e in scan.Edges) Console.WriteLine($"edge {e}");

    var collisions = scan.Normalisation.GroupBy(kv => kv.Value).Where(g => g.Count() > 1).ToList();
    Console.Error.WriteLine($"{scan.Nodes.Count} node(s), {scan.Edges.Count} edge(s).");
    foreach (var kv in scan.Normalisation)
        if (kv.Key != kv.Value) Console.Error.WriteLine($"  {kv.Key} → {kv.Value}");
    foreach (var g in collisions)
        Console.Error.WriteLine($"  COLLISION on '{g.Key}': {string.Join(", ", g.Select(x => x.Key))}");
    return 0;
}

void PrintReport(ValidationReport report)
{
    foreach (var group in report.Findings.GroupBy(f => f.Level).OrderBy(g => (int)g.Key))
    {
        Console.WriteLine();
        Console.WriteLine($"== {group.Key.ToString().ToUpperInvariant()} ({group.Count()}) ==");
        foreach (var f in group)
            Console.WriteLine($"{f.RuleId,-30} {f.RowId,-14} {f.Message}");
    }
    Console.WriteLine();
    Console.WriteLine(report.Passed
        ? $"validate: passed, {report.Findings.Count} note(s)."
        : $"validate: {report.Failures} failure(s).");
}

(string RepoRoot, string SkillFolder) Resolve(string skillFolderArg)
{
    var skillFolder = Path.GetFullPath(skillFolderArg);
    if (!Directory.Exists(skillFolder))
        throw new MapFormatException($"no such folder: {skillFolder}");

    if (repoOverride is not null)
    {
        var explicitRoot = Path.GetFullPath(repoOverride);
        if (!Directory.Exists(explicitRoot))
            throw new MapFormatException($"--repo: no such folder: {explicitRoot}");
        return (explicitRoot, skillFolder);
    }

    var dir = new DirectoryInfo(skillFolder);
    while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, ".git")))
        dir = dir.Parent;
    if (dir is null)
        throw new MapFormatException(
            $"no repository root above {skillFolder}. Paths in the map are repo-relative.");
    return (dir.FullName, skillFolder);
}

int Usage(string? message = null)
{
    if (message is not null) Console.Error.WriteLine(message);
    Console.Error.WriteLine("""
        Usage:
          ProcessMap validate <skill-folder> [--repo <path>]
          ProcessMap render   <skill-folder> [--force] [--repo <path>]
          ProcessMap nodes    <file.md>
        """);
    return 2;
}
