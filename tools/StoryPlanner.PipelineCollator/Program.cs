using System.Text;
using System.Text.RegularExpressions;
using StoryPlanner.BatchFiles;
using StoryPlanner.PipelineCollator;

// The collator of the evidence pipeline's batches (decisions d-2026-09-10-5, -6; -09-11-2, -3;
// -09-13-42, -47, -48). Each subcommand writes a batch's index.md, with a collator line, and the
// item bodies under items/, refusing a batch that already has an index — a collator runs once.
//
//   dotnet run --project tools/StoryPlanner.PipelineCollator -- claim <findings.md> <hypotheses-dir> <batch-folder>
//   dotnet run --project tools/StoryPlanner.PipelineCollator -- referee <claiming-batch-folder> <referee-batch-folder> <hypotheses-dir>
//   dotnet run --project tools/StoryPlanner.PipelineCollator -- reverify <hypothesis-file.md> <proposed-wording.md> <batch-folder>
//
// claim (surfacing) collates a verification's standing findings into one item per finding, each
// holding the current hypothesis set, every statement under its file name, then the finding. referee
// (surfacing) reads the claiming batch's results — which hypotheses each finding bears on — and
// collates them into one item per (finding, target) claim, each holding the target's current
// statement and the finding, blind; its locator is the candidate's identity, which compose-candidates
// reads back. reverify (iterating) collates a hypothesis's current-wording evidence entries into one
// item each, holding the proposed wording and that entry's frozen finding, blind, for gate-and-commit's
// re-verification; its locator is the entry's finding token.

if (args.Length == 0) return Usage();
return args[0] switch
{
    "claim" => Claim(),
    "referee" => Referee(),
    "reverify" => Reverify(),
    _ => Usage(),
};

int Usage()
{
    Console.Error.WriteLine("Usage:\n" +
        "  PipelineCollator claim <findings.md> <hypotheses-dir> <batch-folder>\n" +
        "  PipelineCollator referee <claiming-batch-folder> <referee-batch-folder> <hypotheses-dir>\n" +
        "  PipelineCollator reverify <hypothesis-file.md> <proposed-wording.md> <batch-folder>");
    return 2;
}

int Claim()
{
    if (args.Length < 4) return Usage();
    var findingsPath = Path.GetFullPath(args[1]);
    var hypothesesDir = Path.GetFullPath(args[2]);
    var batchDir = Path.GetFullPath(args[3]);
    if (!File.Exists(findingsPath)) { Console.Error.WriteLine($"findings not found: {findingsPath}"); return 2; }
    if (!Directory.Exists(hypothesesDir)) { Console.Error.WriteLine($"hypotheses folder not found: {hypothesesDir}"); return 2; }
    var hypotheses = Directory.GetFiles(hypothesesDir, "*.md")
        .Where(f => HypothesisFileName.IsMatch(Path.GetFileName(f)))
        .OrderBy(f => f, StringComparer.Ordinal)
        .Select(f => new Collators.HypothesisStatementOf(Path.GetFileNameWithoutExtension(f), Collators.HypothesisStatement(File.ReadAllText(f))))
        .ToList();
    var items = Collators.ClaimItems(File.ReadAllText(findingsPath), hypotheses);
    return WriteBatch(batchDir, $"tools/StoryPlanner.PipelineCollator, {Version()}, claim",
        "the finding's token, `<study>/<slug>`", items);
}

int Referee()
{
    if (args.Length < 4) return Usage();
    var claimingDir = Path.GetFullPath(args[1]);
    var refereeDir = Path.GetFullPath(args[2]);
    var hypothesesDir = Path.GetFullPath(args[3]);

    var definitionPath = Path.Combine(claimingDir, "definition.md");
    if (!File.Exists(definitionPath)) { Console.Error.WriteLine($"claiming batch definition not found: {definitionPath}"); return 2; }
    var claiming = DefinitionFile.Read(definitionPath);

    // A claiming result is one field, the bare list of hypothesis names the finding bears on
    // (d-2026-09-10-6); read its list items directly, independent of the field's name.
    var claims = new List<(string, string)>();
    if (Directory.Exists(claiming.ResultsDir))
        foreach (var rf in Directory.GetFiles(claiming.ResultsDir, "*.md").OrderBy(f => f, StringComparer.Ordinal))
        {
            var findingSlug = Path.GetFileNameWithoutExtension(rf);
            var block = KeyedLines.Read(File.ReadAllText(rf).Replace("\r\n", "\n").Split('\n'));
            foreach (var target in block.Fields.FirstOrDefault()?.ListItems ?? [])
                if (target.Length > 0) claims.Add((findingSlug, target));
        }

    var findingsPath = Path.Combine(claiming.StudyDir, "findings.md");
    var findings = File.Exists(findingsPath)
        ? Collators.StandingFindings(File.ReadAllText(findingsPath)).ToDictionary(f => f.Slug, f => f.Text, StringComparer.Ordinal)
        : new Dictionary<string, string>(StringComparer.Ordinal);
    var statements = new Dictionary<string, string>(StringComparer.Ordinal);
    foreach (var target in claims.Select(c => c.Item2).Distinct(StringComparer.Ordinal))
    {
        var hf = Path.Combine(hypothesesDir, target + ".md");
        statements[target] = File.Exists(hf) ? Collators.HypothesisStatement(File.ReadAllText(hf)) : "";
    }

    var items = Collators.RefereeItems(claims, findings, statements);
    return WriteBatch(refereeDir, $"tools/StoryPlanner.PipelineCollator, {Version()}, referee",
        "the candidate's identity, `<finding-slug> → <target>`", items);
}

int Reverify()
{
    if (args.Length < 4) return Usage();
    var hypothesisFile = Path.GetFullPath(args[1]);
    var wordingFile = Path.GetFullPath(args[2]);
    var batchDir = Path.GetFullPath(args[3]);
    if (!File.Exists(hypothesisFile)) { Console.Error.WriteLine($"hypothesis file not found: {hypothesisFile}"); return 2; }
    if (!File.Exists(wordingFile)) { Console.Error.WriteLine($"proposed wording file not found: {wordingFile}"); return 2; }
    var evidence = Collators.CurrentWordingEvidence(File.ReadAllText(hypothesisFile));
    var items = Collators.ReverifyItems(File.ReadAllText(wordingFile), evidence);
    return WriteBatch(batchDir, $"tools/StoryPlanner.PipelineCollator, {Version()}, reverify",
        "the evidence entry's finding token, `<owner>/<slug>`", items);
}

int WriteBatch(string batchDir, string collator, string locatorNotation, IReadOnlyList<Collators.Item> items)
{
    if (!Directory.Exists(batchDir)) { Console.Error.WriteLine($"batch folder not found: {batchDir}"); return 2; }
    var indexPath = Path.Combine(batchDir, "index.md");
    if (File.Exists(indexPath)) { Console.Error.WriteLine($"{indexPath} exists — a collator runs once per batch; a new collation is a new batch."); return 2; }
    var itemsDir = Path.Combine(batchDir, "items");
    Directory.CreateDirectory(itemsDir);
    foreach (var it in items)
        File.WriteAllText(Path.Combine(itemsDir, it.Id + ".md"), it.Body, new UTF8Encoding(false));
    var index = IndexFile.RenderCollated(Path.GetFileName(batchDir), collator, locatorNotation,
        items.Select(it => (it.Id, it.Locator, it.Description)));
    File.WriteAllText(indexPath, index, new UTF8Encoding(false));
    Console.WriteLine($"{items.Count} items → {batchDir} (index.md and items/)");
    return 0;
}

string Version() => typeof(Collators).Assembly.GetName().Version?.ToString() ?? "0";

partial class Program
{
    static readonly Regex HypothesisFileName = new(@"^\d{3}-[a-z0-9-]+\.md$", RegexOptions.Compiled);
}
