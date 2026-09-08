using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The schema checkers, pure tier. Each class's passing case is the example block of its
/// schema file in the real skill folder (<see cref="SchemaExamples"/>), written into a temp tree at
/// the path its artifact row names; each failing case is that example with one thing broken.
/// Assertions are on rule ids, never on message prose.
/// </summary>
public class SchemaCheckersTests : IDisposable
{
    readonly string _root = Path.Combine(Path.GetTempPath(), "checkers-" + Guid.NewGuid().ToString("N"));
    readonly string _skill;

    public SchemaCheckersTests()
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
        => findings.Where(f => f.Level == FindingLevel.Failure).Select(f => f.CheckId).Distinct().ToArray();

    // ---- hypothesis file ----

    const string HypothesisPath = "docs/v3-framework/hypotheses/017-example.md";

    [Fact]
    public void The_hypothesis_example_passes()
    {
        var path = Write(HypothesisPath, SchemaExamples.Block("hypothesis-file-schema"));
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
        var text = SchemaExamples.Block("hypothesis-file-schema");
        Assert.Contains(find, text);
        var path = Write(HypothesisPath, text.Replace(find, replace));
        Assert.Contains(rule, Rules(HypothesisFile.Check(Ctx(), path)));
    }

    [Fact]
    public void A_created_entry_that_is_not_first_fails()
    {
        var text = SchemaExamples.Block("hypothesis-file-schema");
        var created = "- created | 2026-09-01T10:00: <why the hypothesis exists: the observation, Brian's\n  assertion, the motivation; in Claude's voice with Brian's assertions as the content>\n";
        Assert.Contains(created, text);
        var moved = text.Replace(created, "") .Replace("## Record\n\n", "## Record\n\n- iteration | 2026-09-02T09:00: first.\n" + created);
        var path = Write(HypothesisPath, moved);
        Assert.Contains("hypothesis.created-first", Rules(HypothesisFile.Check(Ctx(), path)));
    }

    [Fact]
    public void A_stray_line_in_the_record_that_is_neither_entry_nor_continuation_fails()
    {
        var text = SchemaExamples.Block("hypothesis-file-schema").Replace("## Record\n\n", "## Record\n\nSome prose here.\n\n");
        var path = Write(HypothesisPath, text);
        Assert.Contains("hypothesis.entry", Rules(HypothesisFile.Check(Ctx(), path)));
    }

    [Fact]
    public void The_id_must_match_the_file_name()
    {
        var path = Write("docs/v3-framework/hypotheses/018-example.md", SchemaExamples.Block("hypothesis-file-schema"));
        Assert.Contains("hypothesis.frontmatter", Rules(HypothesisFile.Check(Ctx(), path)));
    }

    // ---- index ----

    [Fact]
    public void The_index_example_passes_when_its_files_exist()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/002-epistemic-method-provenance.md", "# stub\n");
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", SchemaExamples.Block("hypothesis-index-schema"));
        Assert.Empty(Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    [Fact]
    public void A_row_whose_file_is_absent_fails_the_link()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", SchemaExamples.Block("hypothesis-index-schema"));
        Assert.Contains("index.link", Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    [Fact]
    public void A_file_with_no_row_is_missing_from_the_index()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/002-epistemic-method-provenance.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/003-unlisted.md", "# stub\n");
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", SchemaExamples.Block("hypothesis-index-schema"));
        Assert.Contains("index.missing", Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    [Fact]
    public void Rows_out_of_id_order_fail()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/002-epistemic-method-provenance.md", "# stub\n");
        var lines = SchemaExamples.Block("hypothesis-index-schema").TrimEnd('\n').Split('\n');
        var swapped = string.Join('\n', [lines[0], lines[1], lines[3], lines[2]]) + "\n";
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", swapped);
        Assert.Contains("index.order", Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    // ---- registry ----

    const string RegistryPath = "docs/v3-framework/studies.md";

    [Fact]
    public void The_registry_example_passes_against_the_corpora_it_names()
    {
        var path = Write(RegistryPath, SchemaExamples.Block("study-registry-schema"));
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
        var path = Write(RegistryPath, SchemaExamples.Block("study-registry-schema") + row + "\n");
        var rules = Rules(Registry.Check(Ctx("v1-archive", "fimfiction-stories"), path));
        if (expected.Length == 0) Assert.Empty(rules);
        else foreach (var r in expected) Assert.Contains(r, rules);
    }

    [Fact]
    public void With_no_corpus_ids_available_the_corpus_column_is_reported_not_failed()
    {
        var path = Write(RegistryPath, SchemaExamples.Block("study-registry-schema"));
        var findings = Registry.Check(Ctx(), path);
        Assert.Empty(Rules(findings));
        Assert.Contains("registry.corpora-unavailable", findings.Select(f => f.CheckId));
    }

    // ---- leads ----

    [Fact]
    public void The_leads_example_passes_at_its_study_folder()
    {
        var path = Write("docs/v3-framework/exploration-of-v1-archive/leads.md", SchemaExamples.Block("leads-artifact-schema"));
        Assert.Empty(Rules(Leads.Check(Ctx(), path)));
    }

    [Fact]
    public void A_leads_artifact_in_the_wrong_folder_fails_its_title()
    {
        var path = Write("docs/v3-framework/exploration-of-lineage/leads.md", SchemaExamples.Block("leads-artifact-schema"));
        Assert.Contains("leads.title", Rules(Leads.Check(Ctx(), path)));
    }

    [Fact]
    public void A_leads_artifact_missing_a_section_fails()
    {
        var text = SchemaExamples.Block("leads-artifact-schema").Replace("## Bins\n", "");
        var path = Write("docs/v3-framework/exploration-of-v1-archive/leads.md", text);
        Assert.Contains("leads.sections", Rules(Leads.Check(Ctx(), path)));
    }

    // ---- corpora ----

    [Fact]
    public void The_corpora_example_passes()
    {
        var path = Write(".claude/skills/example/CORPORA.md", SchemaExamples.Block("corpora-schema"));
        Assert.Empty(Rules(Corpora.Check(Ctx(), path)));
    }

    [Fact]
    public void A_corpus_without_its_three_lines_fails()
    {
        var text = SchemaExamples.Block("corpora-schema").Replace("- read by:", "- readers:");
        var path = Write(".claude/skills/example/CORPORA.md", text);
        Assert.Contains("corpora.fields", Rules(Corpora.Check(Ctx(), path)));
    }

    [Fact]
    public void A_duplicate_corpus_id_fails()
    {
        var text = SchemaExamples.Block("corpora-schema");
        var path = Write(".claude/skills/example/CORPORA.md", text + "\n" + text);
        Assert.Contains("corpora.duplicate", Rules(Corpora.Check(Ctx(), path)));
    }

    [Fact]
    public void Corpus_ids_are_the_section_headings_of_CORPORA_md()
    {
        Assert.Empty(Corpora.Ids(_skill));
        Write(".claude/skills/example/CORPORA.md", SchemaExamples.Block("corpora-schema") + "\n## lineage\n\n- what: x\n- where: y\n- read by: z\n");
        Assert.Equal(["fimfiction-stories", "lineage"], Corpora.Ids(_skill).OrderBy(x => x).ToArray());
    }

    // ---- decisions ----

    const string DecisionsPath = "docs/v3-framework/decisions.md";

    [Fact]
    public void The_decisions_example_passes_and_its_written_ids_are_the_ones_the_rule_expects()
    {
        var path = Write(DecisionsPath, SchemaExamples.Block("decisions-schema"));
        var (entries, findings) = Decisions.Read(path);
        Assert.Empty(Rules(findings));
        Assert.Equal(["d-2026-09-07-1", "d-2026-09-08-1"], entries.Select(e => e.Id).ToArray());
        Assert.Equal(["d-2026-09-07-1"], entries[1].Supersedes.ToArray());
        Assert.NotNull(SchemaCheckers.For(WellKnown.Decisions));
        Assert.Contains(WellKnown.Decisions, SchemaCheckers.CheckedIds);
    }

    [Theory]
    [InlineData("- id: d-2026-09-08-1\n", "- id: d-2026-09-08-2\n", "decisions.entry.id")]
    [InlineData("- id: d-2026-09-08-1\n", "- id: d-2026-09-07-2\n", "decisions.entry.id")]
    [InlineData("- id: d-2026-09-07-1\n", "- id: d-2026-09-07-1\n  more\n", "decisions.entry.id")]
    [InlineData("- id: d-2026-09-08-1\n", "", "decisions.entry.fields")]
    [InlineData("- id: d-2026-09-08-1\n- date: 2026-09-08\n", "- date: 2026-09-08\n- id: d-2026-09-08-1\n", "decisions.entry.fields")]
    [InlineData("- not taken: <the options declined and why>\n", "", "decisions.entry.fields")]
    [InlineData("- raised by: <what raised it>\n", "- raised-by: <what raised it>\n", "decisions.entry.fields")]
    [InlineData("- date: 2026-09-08\n- supersedes: d-2026-09-07-1\n", "- supersedes: d-2026-09-07-1\n- date: 2026-09-08\n", "decisions.entry.fields")]
    [InlineData("\n  <a second paragraph", "\n<a second paragraph", "decisions.entry.fields")]
    [InlineData("- decision: <what was ruled>\n", "- decision: \n", "decisions.entry.fields")]
    [InlineData("- date: 2026-09-08\n", "- date: 2026-9-8\n", "decisions.entry.date")]
    [InlineData("- date: 2026-09-08\n", "- date: 2026-09-06\n", "decisions.entry.date")]
    [InlineData("- date: 2026-09-08\n", "- date: 2026-09-31\n", "decisions.entry.date")]
    [InlineData("- date: 2026-09-07\n", "- date: 2026-09-07\n  and more\n", "decisions.entry.date")]
    [InlineData("- supersedes: d-2026-09-07-1\n", "- supersedes: d-2026-09-07-1 (in part)\n", "decisions.supersedes")]
    [InlineData("- supersedes: d-2026-09-07-1\n", "- supersedes: d-2026-09-08-1\n", "decisions.supersedes")]
    [InlineData("- supersedes: d-2026-09-07-1\n", "- supersedes: d-2026-09-01-1\n", "decisions.supersedes")]
    [InlineData("- supersedes: d-2026-09-07-1\n", "- supersedes: d-2026-09-07-1  d-2026-09-07-1\n", "decisions.supersedes")]
    [InlineData("## Revision 2\n", "## Revision two\n", "decisions.shape")]
    [InlineData("<at most one paragraph>\n", "<at most one paragraph>\n\nA second paragraph.\n", "decisions.shape")]
    [InlineData("# Decisions\n", "# decisions\n", "decisions.shape")]
    [InlineData("## Revision 2\n", "### stray\n\n## Revision 2\n", "decisions.shape")]
    public void A_decisions_example_with_one_thing_broken_fails_on_that_rule(string find, string replace, string rule)
    {
        var text = SchemaExamples.Block("decisions-schema");
        Assert.Contains(find, text);
        var path = Write(DecisionsPath, text.Replace(find, replace));
        Assert.Contains(rule, Rules(Decisions.Check(Ctx(), path)));
    }

    const string ThirdEntry = "\n### <a third ruling>\n\n- id: d-2026-09-09-1\n- date: 2026-09-09\n- raised by: x\n- decision: y\n- not taken: z\n";

    [Fact]
    public void An_entry_already_superseded_is_never_superseded_again()
    {
        var text = SchemaExamples.Block("decisions-schema") + ThirdEntry.Replace("- date: 2026-09-09\n", "- date: 2026-09-09\n- supersedes: d-2026-09-07-1\n");
        var path = Write(DecisionsPath, text);
        Assert.Contains("decisions.supersedes", Rules(Decisions.Check(Ctx(), path)));
    }

    [Fact]
    public void Sections_ascend()
    {
        var path = Write(DecisionsPath, SchemaExamples.Block("decisions-schema") + "\n## Revision 1\n");
        Assert.Contains("decisions.shape", Rules(Decisions.Check(Ctx(), path)));
    }

    [Fact]
    public void A_second_entry_of_one_day_carries_the_next_number()
    {
        var third = ThirdEntry.Replace("d-2026-09-09-1", "d-2026-09-08-2").Replace("2026-09-09", "2026-09-08");
        var path = Write(DecisionsPath, SchemaExamples.Block("decisions-schema") + third);
        var (entries, findings) = Decisions.Read(path);
        Assert.Empty(Rules(findings));
        Assert.Equal("d-2026-09-08-2", entries[2].Id);
    }

    [Fact]
    public void A_wrong_id_fails_naming_the_expected_one()
    {
        var path = Write(DecisionsPath, SchemaExamples.Block("decisions-schema") + ThirdEntry.Replace("2026-09-09", "2026-09-08"));
        var findings = Decisions.Check(Ctx(), path);
        var wrong = Assert.Single(findings, f => f.CheckId == "decisions.entry.id");
        Assert.Contains("d-2026-09-08-2", wrong.Message);
    }

    // ---- questions ----

    /// <summary>A repo with the artifacts table and two hypothesis files, so hypothesis ids resolve; the list at its class's path.</summary>
    static (MapFixture Fixture, CheckContext Ctx, string Path) QuestionList(string text)
    {
        var f = new MapFixture().WithStateTree();
        Directory.CreateDirectory(Path.Combine(f.RepoRoot, ".git"));
        var path = f.TreePath("docs", "v3-framework", "questions", "own-fiction.md");
        File.WriteAllText(path, text);
        var ctx = new CheckContext(f.RepoRoot, f.SkillFolder, new HashSet<string>(StringComparer.Ordinal) { "own-fiction", "analysis-corpus" });
        return (f, ctx, path);
    }

    [Fact]
    public void The_question_example_passes_and_reads_its_entries()
    {
        var (f, ctx, path) = QuestionList(SchemaExamples.Block("question-entry-schema"));
        using (f)
        {
            var (entries, findings) = Questions.Read(ctx, path);
            Assert.Empty(Rules(findings));
            Assert.Equal(["heavy-dt-two-classes", "narrator-register-outside-giyc"], entries.Select(e => e.Slug).ToArray());
            Assert.Equal(["031", "032"], entries[0].Hypotheses.ToArray());
            Assert.False(entries[0].Withdrawn);
            Assert.True(entries[1].Withdrawn);
            Assert.NotNull(SchemaCheckers.For(WellKnown.QuestionList));
            Assert.Contains(WellKnown.QuestionList, SchemaCheckers.CheckedIds);
        }
    }

    [Theory]
    [InlineData("# own-fiction — questions\n", "# own-fiction\n", "question.title")]
    [InlineData("### heavy-dt-two-classes\n", "### Heavy-DT\n", "question.slug")]
    [InlineData("### narrator-register-outside-giyc\n", "### heavy-dt-two-classes\n", "question.slug")]
    [InlineData("- question: <the question, in Brian's words>\n- suggested test", "- suggested test", "question.entry.fields")]
    [InlineData("- raised by: recall", "- asked-by: recall", "question.entry.fields")]
    [InlineData("- date: 2026-09-07\n- hypotheses: 031 032\n", "- hypotheses: 031 032\n- date: 2026-09-07\n", "question.entry.fields")]
    [InlineData("- suggested test: <a naive note on how it might be tested>\n", "- suggested test: <a naive note on how it might be tested>\nA bare line.\n", "question.entry.fields")]
    [InlineData("# own-fiction — questions\n", "# own-fiction — questions\n\nA head paragraph.\n", "question.entry.fields")]
    [InlineData("- date: 2026-09-08\n", "- date: 2026-9-8\n", "question.entry.date")]
    [InlineData("- date: 2026-09-08\n", "- date: 2026-09-06\n", "question.entry.date")]
    [InlineData("- hypotheses: 031 032\n", "- hypotheses: 031, 032\n", "question.hypotheses")]
    [InlineData("- hypotheses: 031 032\n", "- hypotheses: 031 099\n", "question.hypotheses")]
    [InlineData("- withdrawn: 2026-09-09 <why>\n", "- withdrawn: <why>\n", "question.withdrawn")]
    [InlineData("- withdrawn: 2026-09-09 <why>\n", "- withdrawn: 2026-09-09 <why>\n- withdrawn: 2026-09-10 <again>\n", "question.withdrawn")]
    [InlineData("- date: 2026-09-08\n", "- withdrawn: 2026-09-09 <early>\n- date: 2026-09-08\n", "question.withdrawn")]
    public void A_question_example_with_one_thing_broken_fails_on_that_rule(string find, string replace, string rule)
    {
        var text = SchemaExamples.Block("question-entry-schema");
        Assert.Contains(find, text);
        var (f, ctx, path) = QuestionList(text.Replace(find, replace));
        using (f) Assert.Contains(rule, Rules(Questions.Check(ctx, path)));
    }

    [Fact]
    public void A_list_whose_corpus_is_not_in_CORPORA_fails_its_title()
    {
        var (f, ctx, path) = QuestionList(SchemaExamples.Block("question-entry-schema"));
        using (f)
        {
            var strict = new CheckContext(f.RepoRoot, f.SkillFolder, new HashSet<string>(StringComparer.Ordinal) { "lineage" });
            Assert.Contains("question.title", Rules(Questions.Check(strict, path)));
        }
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
