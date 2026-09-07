// StoryPlanner.DocIntegrity — holds the shape of the buildout's process documents: the skill's
// three tables and every governed file, checked at the write. Named StoryPlanner.ProcessMap
// under tools/ until 2026-09-06, when it moved to process-docs/ as the first of the binaries
// that hold the shape of the buildout's process documents.
//
// The skill is a Type Object for the buildout method: three tables with fixed columns
// (schemas/skill-schema.md) — Activities and Artifacts in the router, Processes at the head of each
// activity file — plus this tool are the schema; the rows are in flux. Iterating
// the method is editing rows and re-running the check, never rewriting a document. A file of
// an artifact class that has a schema is a governed file, and its checker is that schema.
//
//   dotnet run --project process-docs/StoryPlanner.DocIntegrity -- check  <path>         [--repo <path>]
//   dotnet run --project process-docs/StoryPlanner.DocIntegrity -- render <skill-folder> [--force] [--repo <path>]
//   <publish>/StoryPlanner.DocIntegrity.exe hook        (a Claude Code PostToolUse event on stdin)
//
// Three verbs since 2026-09-07 (decisions.md, "The tool has three verbs"); validate, records
// and state folded into them and nodes retired with the draft-1 comparison it served.
//
// check takes one path and checks everything governed at or under it, following the
// artifacts tables: a skill folder gets the checks of the method's shape (Validator), a
// governed file gets its class's schema (SchemaCheckers), a folder gets every skill folder and
// every governed file under it, so `check .` is the repository and the pre-commit gate's call,
// and a narrower folder bounds the check. Exit 1 on any failure.
//
// render writes every generated file of a skill folder whole — map.md from the tables,
// state.md from the study registry, the question lists, the hypothesis files and the study
// folders under the repository root — and removes any generated block still inside an authored
// file (the convention retired on 2026-09-06). It refuses unless the folder checks clean;
// --force writes anyway and stamps the files UNVALIDATED, for reviewing a scratchpad COPY,
// never the real folder. A copy outside a repository has no root above it, so pass
// --repo <path> to name the real one; artifact paths are repo-relative.
//
// hook is the write boundary: registered in .claude/settings.json on Edit|Write, it reads the
// event and does what the write implies — check, and on a pass render — returning the failures
// to the session on exit code 2. It is called from the published exe, never bin/Debug, so a
// build never breaks a live hook.
//
// Exit codes follow the other tools: 0 ok, 1 failure, 2 usage.
//
// Why no ProjectReference to StoryPlanner.AgentRunner for its UnitSplitter: AgentRunner is a
// Microsoft.NET.Sdk.Web project with Markdig and OutputType=Exe, so referencing it drags the
// ASP.NET framework reference into a plain console tool. MapTables.cs carries the same unit
// rule — header and separator are structure, every body row is a unit — pinned by its own tests.

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
        "check" => RunCheck(),
        "render" => RunRender(),
        "hook" => RunHook(),
        _ => Usage($"Unknown verb '{verb}'."),
    };
}
catch (MapFormatException ex)
{
    Console.Error.WriteLine($"Refusing to guess ({ex.RuleId}): {ex.Message}");
    return 1;
}

int RunCheck()
{
    if (positional.Count != 1) return Usage("check takes one argument: a governed file, a skill folder, or a folder.");
    var path = Path.GetFullPath(positional[0]);
    if (!File.Exists(path) && !Directory.Exists(path)) { Console.Error.WriteLine($"No such file or folder: {path}"); return 2; }

    var result = Check.Run(RepoRootFor(path), path);
    Console.Error.WriteLine($"check: {result.SkillFolders.Count} skill folder(s), {result.GovernedFiles.Count} governed file(s).");
    Console.Write(ReportText.Format(result.Report));
    return result.Report.Passed ? 0 : 1;
}

int RunRender()
{
    if (positional.Count != 1) return Usage("render takes one argument: the skill folder.");
    var skillFolder = Path.GetFullPath(positional[0]);
    if (!Directory.Exists(skillFolder))
        throw new MapFormatException($"no such folder: {skillFolder}", "folder.missing");
    var repoRoot = RepoRootFor(skillFolder);

    var report = Validator.Validate(skillFolder);
    if (!report.Passed)
    {
        if (!force || report.Findings.Any(f => f.RuleId.StartsWith("table.") || f.RuleId.EndsWith(".missing")))
        {
            Console.Error.WriteLine(
                $"render refuses: check reports {report.Failures} failure(s). " +
                (force ? "The tables do not parse, so --force cannot help." :
                    "Fix the rows, or run on a scratchpad copy with --force to review the output."));
            Console.Write(ReportText.Format(report));
            return 1;
        }
        Console.Error.WriteLine($"--force: render over {report.Failures} failure(s); the output is stamped UNVALIDATED.");
    }

    var doc = SkillReader.Read(skillFolder);
    var written = Render.Write(repoRoot, skillFolder, doc, report, forced: !report.Passed);
    foreach (var path in written) Console.WriteLine($"Wrote {path}");
    return 0;
}

int RunHook()
{
    var outcome = WriteHook.Run(Console.In.ReadToEnd());
    if (outcome.Message.Length > 0) Console.Error.WriteLine(outcome.Message);
    return outcome.ExitCode;
}

/// <summary>
/// --repo when given; else the repository root above the path; else the root the shape of a
/// governed skill folder implies (three levels up), which is what a scratchpad copy at
/// <c>.claude/skills/&lt;name&gt;/</c> has; else a refusal, since artifact paths are repo-relative.
/// </summary>
string RepoRootFor(string path)
{
    if (repoOverride is not null)
    {
        var explicitRoot = Path.GetFullPath(repoOverride);
        if (!Directory.Exists(explicitRoot))
            throw new MapFormatException($"--repo: no such folder: {explicitRoot}", "folder.missing");
        return explicitRoot;
    }

    var root = RepoLocator.FindRoot(path);
    if (root is not null) return root;

    var folder = GovernedSkill.Locate(path);
    if (folder is not null) return GovernedSkill.RepoRootOf(folder);

    throw new MapFormatException(
        $"no repository root above {path}. Artifact paths are repo-relative; pass --repo <path>.",
        "folder.missing");
}

int Usage(string? message = null)
{
    if (message is not null) Console.Error.WriteLine(message);
    Console.Error.WriteLine("""
        Usage:
          DocIntegrity check  <path>         [--repo <path>]            everything governed at or under the path:
                                                                        a skill folder's shape, a governed file's schema,
                                                                        a folder's whole set; `check .` is the repository
          DocIntegrity render <skill-folder> [--force] [--repo <path>]  map.md and state.md, whole
          DocIntegrity hook                                             (reads a Claude Code PostToolUse event from stdin)
        """);
    return 2;
}
