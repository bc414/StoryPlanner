// StoryPlanner.ProcessMap — the validator, renderer and state reader for the v3-buildout skill.
//
// The skill is a Type Object for the buildout method: three tables with fixed columns
// (SKILL.md § Schema) — Activities in the router, Processes at the head of each activity file,
// Artifacts in artifacts.md — plus this tool are the schema; the rows are in flux. Iterating
// the method is editing rows and re-running validation, never rewriting a document.
//
//   dotnet run --project tools/StoryPlanner.ProcessMap -- validate .claude/skills/v3-buildout
//   dotnet run --project tools/StoryPlanner.ProcessMap -- render   .claude/skills/v3-buildout [--force]
//   dotnet run --project tools/StoryPlanner.ProcessMap -- state    .claude/skills/v3-buildout [--force] [--repo <path>]
//   dotnet run --project tools/StoryPlanner.ProcessMap -- nodes    <file.md>
//
// render writes the level-1 section of SKILL.md, the activity section of every activity file,
// and map.md whole; state writes state.md whole from the instance registry, the question lists,
// the hypothesis files and the instance folders under the repo root. Both refuse unless
// validate passes. --force writes anyway and stamps every generated section UNVALIDATED; it
// exists for reviewing diagrams on a scratchpad COPY, never for the real folder. A copy outside
// the repo has no repository root above it, so pass --repo <path> to name the real one; only
// state reads anything under it.
//
// Exit codes follow the other tools: 0 ok, 1 failure, 2 usage.
//
// Reworked in place on 2026-09-05 (methodology revision 2, handoff 2 step 1) from the tool of
// 2026-09-04 that validated the previous schema; the rulings are in
// docs/v3-framework/methodology-revision-2-rulings.md. Why no ProjectReference to
// StoryPlanner.AgentRunner for its UnitSplitter: AgentRunner is a Microsoft.NET.Sdk.Web project
// with Markdig and OutputType=Exe, so referencing it drags the ASP.NET framework reference into
// a plain console tool. MapTables.cs carries the same unit rule — header and separator are
// structure, every body row is a unit — pinned by its own tests.

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
        "state" => RunState(),
        "nodes" => RunNodes(),
        _ => Usage($"Unknown verb '{verb}'."),
    };
}
catch (MapFormatException ex)
{
    Console.Error.WriteLine($"Refusing to guess ({ex.RuleId}): {ex.Message}");
    return 1;
}

int RunValidate()
{
    if (positional.Count != 1) return Usage("validate takes one argument: the skill folder.");
    var (_, skillFolder) = Resolve(positional[0]);
    var report = Validator.Validate(skillFolder);
    PrintReport(report);
    return report.Passed ? 0 : 1;
}

int RunRender()
{
    if (positional.Count != 1) return Usage("render takes one argument: the skill folder.");
    var (_, skillFolder) = Resolve(positional[0]);
    var report = Gate("render", skillFolder);
    if (report is null) return 1;

    var doc = SkillReader.Read(skillFolder);
    MermaidRenderer.CheckNodeIds(doc);
    var forced = !report.Passed;

    var written = 0;
    File.WriteAllText(doc.SkillPath, MarkerWriter.Write(File.ReadAllText(doc.SkillPath),
        new Dictionary<string, string> { [MermaidRenderer.Level1Section] = MermaidRenderer.Level1(doc, forced) }));
    written++;

    foreach (var a in doc.Activities)
    {
        var path = doc.ActivityPath(a.Id);
        if (!File.Exists(path)) continue;
        File.WriteAllText(path, MarkerWriter.Write(File.ReadAllText(path),
            new Dictionary<string, string> { [MermaidRenderer.ActivitySection] = MermaidRenderer.Activity(doc, a.Id, forced) }));
        written++;
    }

    var mapPath = Path.Combine(skillFolder, "map.md");
    File.WriteAllText(mapPath, MermaidRenderer.Map(doc, report, forced));
    written++;

    Console.WriteLine($"Wrote {written} file(s) under {skillFolder}.");
    return 0;
}

int RunState()
{
    if (positional.Count != 1) return Usage("state takes one argument: the skill folder.");
    var (repoRoot, skillFolder) = Resolve(positional[0]);
    var report = Gate("state", skillFolder);
    if (report is null) return 1;

    var doc = SkillReader.Read(skillFolder);
    var text = StateBuilder.Build(repoRoot, doc, forced: !report.Passed);

    var statePath = Path.Combine(skillFolder, "state.md");
    File.WriteAllText(statePath, text);
    Console.WriteLine($"Wrote {statePath}.");
    return 0;
}

/// <summary>validate first; null means refused (already printed). A failing report with --force is allowed through.</summary>
ValidationReport? Gate(string verb, string skillFolder)
{
    var report = Validator.Validate(skillFolder);
    if (report.Passed) return report;
    if (!force || report.Findings.Any(f => f.RuleId.StartsWith("table.") || f.RuleId.EndsWith(".missing")))
    {
        Console.Error.WriteLine(
            $"{verb} refuses: validate reports {report.Failures} failure(s). " +
            (force ? "The tables do not parse, so --force cannot help." :
                "Fix the rows, or run on a scratchpad copy with --force to review the output."));
        PrintReport(report);
        return null;
    }
    Console.Error.WriteLine(
        $"--force: {verb} over {report.Failures} failure(s); the output is stamped UNVALIDATED.");
    return report;
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
            Console.WriteLine($"{f.RuleId,-32} {f.RowId,-36} {f.Message}");
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
        throw new MapFormatException($"no such folder: {skillFolder}", "folder.missing");

    if (repoOverride is not null)
    {
        var explicitRoot = Path.GetFullPath(repoOverride);
        if (!Directory.Exists(explicitRoot))
            throw new MapFormatException($"--repo: no such folder: {explicitRoot}", "folder.missing");
        return (explicitRoot, skillFolder);
    }

    var dir = new DirectoryInfo(skillFolder);
    while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, ".git")))
        dir = dir.Parent;
    if (dir is null)
        throw new MapFormatException(
            $"no repository root above {skillFolder}. Artifact paths are repo-relative; pass --repo <path>.",
            "folder.missing");
    return (dir.FullName, skillFolder);
}

int Usage(string? message = null)
{
    if (message is not null) Console.Error.WriteLine(message);
    Console.Error.WriteLine("""
        Usage:
          ProcessMap validate <skill-folder> [--repo <path>]
          ProcessMap render   <skill-folder> [--force] [--repo <path>]
          ProcessMap state    <skill-folder> [--force] [--repo <path>]
          ProcessMap nodes    <file.md>
        """);
    return 2;
}
