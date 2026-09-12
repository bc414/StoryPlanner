using System;
using System.IO;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// compose-candidates (decisions d-2026-09-10-5, -7): the generated candidates view. It joins the
/// referee results, findings.md, declined-candidates.md and the hypothesis records into one file:
/// diagnostic candidates in an opening section with their materialised finding, verdict, falsifier
/// and derived status; non-diagnostic claims at the foot. The view is not governed, so this tests
/// the generator's output directly rather than a checker.
/// </summary>
public class ComposeTests : IDisposable
{
    readonly MapFixture _f = new();
    string Repo => _f.RepoRoot;
    const string StudyId = "verification-of-analysis-corpus-dt-classes";
    string StudyDir => Path.Combine(Repo, "docs", "v3-framework", "studies", StudyId);
    CheckContext Ctx => CheckContext.From(Repo, _f.SkillFolder);

    public ComposeTests() => Directory.CreateDirectory(Path.Combine(Repo, ".git"));
    public void Dispose() => _f.Dispose();

    void Write(string rel, string content)
    {
        var path = Path.Combine(Repo, rel.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content);
    }

    const string RefereeDirections = """
        ## What you are given

        A statement and a finding.

        ## Classes

        - diagnostic-supporting: the finding shows the statement's observable
        - diagnostic-challenging: the finding shows its opposite
        - non-diagnostic: neither

        ## Criteria

        1. A rule that decides which side the finding shows.

        ## What to produce

        - falsifier: block, what the finding would have been if the statement were false
        - verdict: enum

        """;

    void BuildTree()
    {
        const string s = "docs/v3-framework/studies/" + StudyId;
        Write("docs/v3-framework/referee/directions-1.md", RefereeDirections);

        Write($"{s}/findings.md",
            $"# {StudyId} — findings\n\n## Findings\n\n"
            + $"### {StudyId}/most-are-a\n- finding: Most notes are class a.\n- cites:\n  - {StudyId}/01-full § class\n\n"
            + $"### {StudyId}/a-few-are-b\n- finding: A few are class b.\n- cites:\n  - {StudyId}/01-full § class\n\n"
            + $"### {StudyId}/noise\n- finding: Some noise with no bearing.\n- cites:\n  - {StudyId}/01-full § class\n");

        Write($"{s}/declined-candidates.md",
            $"# {StudyId} — declined candidates\n\n"
            + "### a-few-are-b → 031-dt-classes\n\n- date: 2026-09-22\n- reason: not compelling on reflection\n");

        Write("docs/v3-framework/hypotheses/031-dt-classes.md",
            "## Hypothesis\n\nDT has two classes.\n\n## Origin\n\n- date: 2026-09-01\n- reasoning: why\n\n## Record\n\n"
            + $"### evidence\n- date: 2026-09-14\n- candidate: {StudyId}/most-are-a\n- tag: supporting\n"
            + "- finding: Most notes are class a.\n- falsifier: they would have been evenly split\n");

        Write($"{s}/batches/02-referee/definition.md",
            "# 02-referee — definition\n\n- directions: ../../../../referee/directions-1.md\n- kind: full\n- model: sonnet\n");
        Write($"{s}/batches/02-referee/index.md",
            "# 02-referee — index\n\n- itemizer: tools/StoryPlanner.RefereeItemizer, 1\n- corpus: candidates\n- locator notation: <finding-slug> → <target>\n\n"
            + "| item | locator | description |\n|---|---|---|\n"
            + "| most-are-a-031 | most-are-a → 031-dt-classes | most-are-a against 031 |\n"
            + "| a-few-are-b-031 | a-few-are-b → 031-dt-classes | a-few-are-b against 031 |\n"
            + "| noise-031 | noise → 031-dt-classes | noise against 031 |\n");
        Write($"{s}/batches/02-referee/results/most-are-a-031.md", "- falsifier: they would be evenly split\n- verdict: diagnostic-supporting\n");
        Write($"{s}/batches/02-referee/results/a-few-are-b-031.md", "- falsifier: none would be b\n- verdict: diagnostic-supporting\n");
        Write($"{s}/batches/02-referee/results/noise-031.md", "- falsifier: the same either way\n- verdict: non-diagnostic\n");
    }

    [Fact]
    public void The_view_joins_the_referee_results_findings_declines_and_promotions()
    {
        BuildTree();
        var md = Compose.Build(Ctx, StudyDir);

        var diagnostic = md.IndexOf("## Diagnostic candidates", StringComparison.Ordinal);
        var nonDiagnostic = md.IndexOf("## Non-diagnostic claims", StringComparison.Ordinal);
        Assert.True(diagnostic >= 0 && nonDiagnostic > diagnostic);

        // The two diagnostic candidates open the document; the non-diagnostic one is at the foot.
        Assert.InRange(md.IndexOf("### most-are-a → 031-dt-classes", StringComparison.Ordinal), diagnostic, nonDiagnostic);
        Assert.InRange(md.IndexOf("### a-few-are-b → 031-dt-classes", StringComparison.Ordinal), diagnostic, nonDiagnostic);
        Assert.True(md.IndexOf("### noise → 031-dt-classes", StringComparison.Ordinal) > nonDiagnostic);

        // most-are-a is promoted (a hypothesis record cites it); a-few-are-b is declined, with its reason.
        Assert.Contains("- status: promoted", md);
        Assert.Contains("- status: declined", md);
        Assert.Contains("- reason: not compelling on reflection", md);
        Assert.Contains("- verdict: diagnostic supporting", md);
        // The finding text is materialised inline, not a bare token.
        Assert.Contains("- finding: Most notes are class a.", md);
        Assert.Contains("- falsifier: they would be evenly split", md);
    }

    [Fact]
    public void The_status_is_pending_when_neither_promoted_nor_declined()
    {
        BuildTree();
        // Drop the decline so a-few-are-b is neither promoted nor declined.
        File.Delete(Path.Combine(StudyDir, "declined-candidates.md"));
        var md = Compose.Build(Ctx, StudyDir);
        Assert.Contains("- status: pending", md);
        Assert.DoesNotContain("- status: declined", md);
    }

    [Fact]
    public void Write_produces_candidates_md_at_the_study_folder()
    {
        BuildTree();
        var path = Compose.Write(Ctx, StudyDir);
        Assert.Equal(Path.Combine(StudyDir, "candidates.md"), path);
        Assert.True(File.Exists(path));
        Assert.StartsWith($"# {StudyId} — candidates", File.ReadAllText(path));
    }

    [Fact]
    public void An_absent_referee_batch_yields_an_empty_but_well_formed_view()
    {
        // No batches at all: the view still has both sections, each empty.
        Directory.CreateDirectory(StudyDir);
        var md = Compose.Build(Ctx, StudyDir);
        Assert.Contains("## Diagnostic candidates", md);
        Assert.Contains("## Non-diagnostic claims", md);
        Assert.Contains("None.", md);
    }
}
