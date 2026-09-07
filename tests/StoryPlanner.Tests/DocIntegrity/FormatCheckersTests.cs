using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The format checkers, pure tier. Each class's passing case is the example block of its
/// format in the real artifacts.md (<see cref="FormatExamples"/>), written into a temp tree at
/// the path its artifact row names; each failing case is that example with one thing broken.
/// Assertions are on rule ids, never on message prose.
/// </summary>
public class FormatCheckersTests : IDisposable
{
    readonly string _root = Path.Combine(Path.GetTempPath(), "checkers-" + Guid.NewGuid().ToString("N"));
    readonly string _skill;

    public FormatCheckersTests()
    {
        _skill = Path.Combine(_root, ".claude", "skills", "example");
        Directory.CreateDirectory(_skill);
        Directory.CreateDirectory(Path.Combine(_root, ".git"));
    }

    public void Dispose()
    {
        try { Directory.Delete(_root, recursive: true); } catch (IOException) { }
    }

    CheckContext Ctx(params string[] corpora) => new(_root, _skill, corpora.ToHashSet(StringComparer.Ordinal));

    string Write(string relative, string content)
    {
        var path = Path.Combine(_root, relative.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content);
        return path;
    }

    static string[] Rules(IReadOnlyList<Finding> findings)
        => findings.Where(f => f.Level == FindingLevel.Failure).Select(f => f.RuleId).Distinct().ToArray();

    // ---- hypothesis file ----

    const string HypothesisPath = "docs/v3-framework/hypotheses/017-example.md";

    [Fact]
    public void The_hypothesis_example_passes()
    {
        var path = Write(HypothesisPath, FormatExamples.Block("Hypothesis file"));
        Assert.Empty(Rules(HypothesisFile.Check(Ctx(), path)));
    }

    [Theory]
    [InlineData("  Falsifier: <verbatim from the referee's line>\n", "", "hypothesis.evidence.no-falsifier")]
    [InlineData("(round-of-fimfiction-stories-1 C-014; codebook-3@3f9a1c) [supporting]", "(WU1.1) [supporting]", "hypothesis.evidence.citation")]
    [InlineData("status: evidenced", "status: untested", "hypothesis.status.mismatch")]
    [InlineData("created: 2026-09-01\n", "created: 2026-09-01\nnote: x\n", "hypothesis.frontmatter")]
    [InlineData("## Record", "## Records", "hypothesis.sections")]
    [InlineData("- baselined | 2026-09-20T16:00: <Brian's rationale, in his words>\n", "", "hypothesis.baselined")]
    [InlineData("- evidence | 2026-09-14T15:20 |", "- evidence | 2026-09-14 |", "hypothesis.entry")]
    public void A_hypothesis_example_with_one_thing_broken_fails_on_that_rule(string find, string replace, string rule)
    {
        var text = FormatExamples.Block("Hypothesis file");
        Assert.Contains(find, text);
        var path = Write(HypothesisPath, text.Replace(find, replace));
        Assert.Contains(rule, Rules(HypothesisFile.Check(Ctx(), path)));
    }

    [Fact]
    public void A_created_entry_that_is_not_first_fails()
    {
        var text = FormatExamples.Block("Hypothesis file");
        var created = "- created | 2026-09-01T10:00: <why the hypothesis exists: the observation, Brian's\n  assertion, the motivation; in Claude's voice with Brian's assertions as the content>\n";
        Assert.Contains(created, text);
        var moved = text.Replace(created, "") .Replace("## Record\n\n", "## Record\n\n- iteration | 2026-09-02T09:00: first.\n" + created);
        var path = Write(HypothesisPath, moved);
        Assert.Contains("hypothesis.created-first", Rules(HypothesisFile.Check(Ctx(), path)));
    }

    [Fact]
    public void A_stray_line_in_the_record_that_is_neither_entry_nor_continuation_fails()
    {
        var text = FormatExamples.Block("Hypothesis file").Replace("## Record\n\n", "## Record\n\nSome prose here.\n\n");
        var path = Write(HypothesisPath, text);
        Assert.Contains("hypothesis.entry", Rules(HypothesisFile.Check(Ctx(), path)));
    }

    [Fact]
    public void The_id_must_match_the_file_name()
    {
        var path = Write("docs/v3-framework/hypotheses/018-example.md", FormatExamples.Block("Hypothesis file"));
        Assert.Contains("hypothesis.frontmatter", Rules(HypothesisFile.Check(Ctx(), path)));
    }

    // ---- index ----

    [Fact]
    public void The_index_example_passes_when_its_files_exist()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/002-epistemic-method-provenance.md", "# stub\n");
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", FormatExamples.Block("Hypothesis index"));
        Assert.Empty(Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    [Fact]
    public void A_row_whose_file_is_absent_fails_the_link()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", FormatExamples.Block("Hypothesis index"));
        Assert.Contains("index.link", Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    [Fact]
    public void A_file_with_no_row_is_missing_from_the_index()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/002-epistemic-method-provenance.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/003-unlisted.md", "# stub\n");
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", FormatExamples.Block("Hypothesis index"));
        Assert.Contains("index.missing", Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    [Fact]
    public void Rows_out_of_id_order_fail()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/002-epistemic-method-provenance.md", "# stub\n");
        var lines = FormatExamples.Block("Hypothesis index").TrimEnd('\n').Split('\n');
        var swapped = string.Join('\n', [lines[0], lines[1], lines[3], lines[2]]) + "\n";
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", swapped);
        Assert.Contains("index.order", Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    // ---- registry ----

    const string RegistryPath = "docs/v3-framework/studies.md";

    [Fact]
    public void The_registry_example_passes_against_the_corpora_it_names()
    {
        var path = Write(RegistryPath, FormatExamples.Block("Study registry"));
        Assert.Empty(Rules(Registry.Check(Ctx("v1-archive", "fimfiction-stories"), path)));
    }

    [Theory]
    [InlineData("| referee-1 | verification | candidates | 2026-09-21 |", new string[0])]
    [InlineData("| exploration-of-v1-archive-2 | exploratory | v1-archive | 2026-09-21 |", new string[0])]
    [InlineData("| exploration-of-verified-artifacts | exploratory | verified-artifacts | 2026-09-21 |", new string[0])]
    [InlineData("| exploration-of-nowhere | exploratory | nowhere | 2026-09-21 |", new[] { "registry.corpus" })]
    [InlineData("| round-of-v1-archive | verification | v1-archive | 2026-09-21 |", new[] { "registry.corpus" })]
    [InlineData("| exploration-of-v1-archive | verification | v1-archive | 2026-09-21 |", new[] { "registry.type" })]
    [InlineData("| exploration-of-v1-archive | exploratory | v1-archive | soon |", new[] { "registry.go" })]
    [InlineData("| exploration-of-v1-archive | exploratory | v1-archive | 2026-09-12 |", new[] { "registry.duplicate" })]
    [InlineData("| something-else | exploratory | v1-archive | 2026-09-21 |", new[] { "registry.id" })]
    public void A_registry_row_is_held_to_its_form(string row, string[] expected)
    {
        var path = Write(RegistryPath, FormatExamples.Block("Study registry") + row + "\n");
        var rules = Rules(Registry.Check(Ctx("v1-archive", "fimfiction-stories"), path));
        if (expected.Length == 0) Assert.Empty(rules);
        else foreach (var r in expected) Assert.Contains(r, rules);
    }

    [Fact]
    public void With_no_corpus_ids_available_the_corpus_column_is_reported_not_failed()
    {
        var path = Write(RegistryPath, FormatExamples.Block("Study registry"));
        var findings = Registry.Check(Ctx(), path);
        Assert.Empty(Rules(findings));
        Assert.Contains("registry.corpora-unavailable", findings.Select(f => f.RuleId));
    }

    // ---- leads ----

    [Fact]
    public void The_leads_example_passes_at_its_study_folder()
    {
        var path = Write("docs/v3-framework/exploration-of-v1-archive/leads.md", FormatExamples.Block("Leads artifact"));
        Assert.Empty(Rules(Leads.Check(Ctx(), path)));
    }

    [Fact]
    public void A_leads_artifact_in_the_wrong_folder_fails_its_title()
    {
        var path = Write("docs/v3-framework/exploration-of-lineage/leads.md", FormatExamples.Block("Leads artifact"));
        Assert.Contains("leads.title", Rules(Leads.Check(Ctx(), path)));
    }

    [Fact]
    public void A_leads_artifact_missing_a_section_fails()
    {
        var text = FormatExamples.Block("Leads artifact").Replace("## Bins\n", "");
        var path = Write("docs/v3-framework/exploration-of-v1-archive/leads.md", text);
        Assert.Contains("leads.sections", Rules(Leads.Check(Ctx(), path)));
    }

    // ---- corpora ----

    [Fact]
    public void The_corpora_example_passes()
    {
        var path = Write(".claude/skills/example/CORPORA.md", FormatExamples.Block("Corpora"));
        Assert.Empty(Rules(Corpora.Check(Ctx(), path)));
    }

    [Fact]
    public void A_corpus_without_its_three_lines_fails()
    {
        var text = FormatExamples.Block("Corpora").Replace("- read by:", "- readers:");
        var path = Write(".claude/skills/example/CORPORA.md", text);
        Assert.Contains("corpora.fields", Rules(Corpora.Check(Ctx(), path)));
    }

    [Fact]
    public void A_duplicate_corpus_id_fails()
    {
        var text = FormatExamples.Block("Corpora");
        var path = Write(".claude/skills/example/CORPORA.md", text + "\n" + text);
        Assert.Contains("corpora.duplicate", Rules(Corpora.Check(Ctx(), path)));
    }

    [Fact]
    public void Corpus_ids_come_from_CORPORA_md_sections_or_the_legacy_id_table()
    {
        Assert.Empty(Corpora.Ids(_skill));
        Write(".claude/skills/example/CORPUS-STATUS.md", "# status\n\n## Corpus ids\n\n| id | corpus | where described |\n|---|---|---|\n| lineage | x | y |\n| google-keep | x | y |\n");
        Assert.Equal(["google-keep", "lineage"], Corpora.Ids(_skill).OrderBy(x => x).ToArray());
        Write(".claude/skills/example/CORPORA.md", FormatExamples.Block("Corpora"));
        Assert.Equal(["fimfiction-stories"], Corpora.Ids(_skill).ToArray());
    }

    // ---- scope ----

    [Fact]
    public void A_path_resolves_to_the_artifact_class_that_governs_it()
    {
        using var f = new MapFixture().WithStateTree();
        Directory.CreateDirectory(Path.Combine(f.RepoRoot, ".git"));
        var hypothesis = ArtifactScope.Locate(f.TreePath("docs", "v3-framework", "hypotheses", "031-dt-classes.md"));
        Assert.NotNull(hypothesis);
        Assert.Contains(hypothesis!.Row.Id, WellKnown.HypothesisArtifacts);
        var registry = ArtifactScope.Locate(f.TreePath("docs", "v3-framework", "studies.md"));
        Assert.Equal(WellKnown.Studies, registry!.Row.Id);
        Assert.Null(ArtifactScope.Locate(f.TreePath("docs", "note.md")));
    }

    [Fact]
    public void The_hook_checks_a_governed_file_write_and_stays_silent_on_a_well_formed_one()
    {
        using var f = new MapFixture().WithStateTree();
        Directory.CreateDirectory(Path.Combine(f.RepoRoot, ".git"));
        string Payload(string path) => $$"""
            { "hook_event_name": "PostToolUse", "tool_name": "Edit",
              "tool_input": { "file_path": {{System.Text.Json.JsonSerializer.Serialize(path)}} } }
            """;
        var good = WriteHook.Run(Payload(f.TreePath("docs", "v3-framework", "hypotheses", "031-dt-classes.md")));
        Assert.Equal(HookOutcomeKind.Silent, good.Kind);
        // 032 says untested while holding an evidence entry bound to its current wording.
        var bad = WriteHook.Run(Payload(f.TreePath("docs", "v3-framework", "hypotheses", "032-other.md")));
        Assert.Equal(HookOutcomeKind.Failed, bad.Kind);
        Assert.Contains("hypothesis.status.mismatch", bad.Message);
    }
}
