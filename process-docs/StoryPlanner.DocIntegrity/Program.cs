// StoryPlanner.DocIntegrity — the validator, renderer and state reader for the v3-buildout skill.
// Named StoryPlanner.ProcessMap under tools/ until 2026-09-06, when it moved to process-docs/ as
// the first of the binaries that hold the shape of the buildout's process documents.
//
// The skill is a Type Object for the buildout method: three tables with fixed columns
// (SKILL.md § Schema) — Activities in the router, Processes at the head of each activity file,
// Artifacts in artifacts.md — plus this tool are the schema; the rows are in flux. Iterating
// the method is editing rows and re-running validation, never rewriting a document.
//
//   dotnet run --project process-docs/StoryPlanner.DocIntegrity -- validate .claude/skills/v3-buildout
//   dotnet run --project process-docs/StoryPlanner.DocIntegrity -- render   .claude/skills/v3-buildout [--force]
//   dotnet run --project process-docs/StoryPlanner.DocIntegrity -- state    .claude/skills/v3-buildout [--force] [--repo <path>]
//   dotnet run --project process-docs/StoryPlanner.DocIntegrity -- nodes    <file.md>
//   <publish>/StoryPlanner.DocIntegrity.exe hook        (a Claude Code PostToolUse event on stdin)
//
// hook is the write boundary: registered in .claude/settings.json on Edit|Write, it reads the
// event, and when the written file lies in a governed skill folder (see GovernedSkill) it runs
// validate there, returns the failures to the session on exit code 2, and on a pass rewrites
// map.md so the generated file is never stale. It is called from the published exe, never
// bin/Debug, so a build never breaks a live hook.
//
// render writes map.md whole (and removes any generated block still inside an authored file,
// the convention retired on 2026-09-06); state writes state.md whole from the instance
// registry, the question lists, the hypothesis files and the instance folders under the repo
// root. Both refuse unless validate passes. --force writes anyway and stamps the file UNVALIDATED; it
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

using StoryPlanner.DocIntegrity;

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
        "hook" => RunHook(),
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
    var written = Render.Write(skillFolder, doc, report, forced: !report.Passed);
    foreach (var path in written) Console.WriteLine($"Wrote {path}");
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

int RunHook()
{
    var outcome = WriteHook.Run(Console.In.ReadToEnd());
    if (outcome.Message.Length > 0) Console.Error.WriteLine(outcome.Message);
    return outcome.ExitCode;
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

void PrintReport(ValidationReport report) => Console.Write(ReportText.Format(report));

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

    var root = RepoLocator.FindRoot(skillFolder)
        ?? throw new MapFormatException(
            $"no repository root above {skillFolder}. Artifact paths are repo-relative; pass --repo <path>.",
            "folder.missing");
    return (root, skillFolder);
}

int Usage(string? message = null)
{
    if (message is not null) Console.Error.WriteLine(message);
    Console.Error.WriteLine("""
        Usage:
          DocIntegrity validate <skill-folder> [--repo <path>]
          DocIntegrity render   <skill-folder> [--force] [--repo <path>]
          DocIntegrity state    <skill-folder> [--force] [--repo <path>]
          DocIntegrity nodes    <file.md>
          DocIntegrity hook     (reads a Claude Code PostToolUse event from stdin)
        """);
    return 2;
}
