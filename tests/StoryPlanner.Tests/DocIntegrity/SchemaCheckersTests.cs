using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StoryPlanner.BatchFiles;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The schema checkers, pure tier. Each class's passing case is the example block of its
/// schema file in the real skill folder (<see cref="SchemaExamples"/>), written into a temp tree at
/// the path its artifact row names; each failing case is that example with one thing broken.
/// The engine reads each class's Shape from a copy of the real schema file in the fixture's
/// skill folder. Assertions are on check ids, never on message prose.
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
        SchemaExamples.CopyInto(_skill, "decisions-schema", "question-entry-schema", "directions-schema", "index-schema", "definition-schema", "findings-schema", "declined-candidates-schema");
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
    [InlineData("(verification-of-fimfiction-stories-fid-primary/fid-primary-in-one-of-seven; directions-3@3f9a1c) [supporting]", "(WU1.1) [supporting]", "hypothesis.evidence.citation")]
    [InlineData("(verification-of-fimfiction-stories-fid-primary/fid-primary-in-one-of-seven; directions-3@3f9a1c) [supporting]", "(verification-of-fimfiction-stories-fid-primary/fid-primary-in-one-of-seven; codebook-3@3f9a1c) [supporting]", "hypothesis.evidence.citation")]
    [InlineData("(verification-of-fimfiction-stories-fid-primary/fid-primary-in-one-of-seven; directions-3@3f9a1c) [supporting]", "(verification-of-fimfiction-stories-fid-primary C-014; directions-3@3f9a1c) [supporting]", "hypothesis.evidence.citation")]
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

    // ---- hypothesis index ----

    [Fact]
    public void The_hypothesis_index_example_passes_when_its_files_exist()
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
        Assert.Contains("hypothesis-index.link", Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    [Fact]
    public void A_file_with_no_row_is_missing_from_the_index()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/002-epistemic-method-provenance.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/003-unlisted.md", "# stub\n");
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", SchemaExamples.Block("hypothesis-index-schema"));
        Assert.Contains("hypothesis-index.missing", Rules(HypothesisIndex.Check(Ctx(), path)));
    }

    [Fact]
    public void Rows_out_of_id_order_fail()
    {
        Write("docs/v3-framework/hypotheses/001-planner-purpose-trajectories.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/002-epistemic-method-provenance.md", "# stub\n");
        var lines = SchemaExamples.Block("hypothesis-index-schema").TrimEnd('\n').Split('\n');
        var swapped = string.Join('\n', [lines[0], lines[1], lines[3], lines[2]]) + "\n";
        var path = Write("docs/v3-framework/hypotheses/INDEX.md", swapped);
        Assert.Contains("hypothesis-index.order", Rules(HypothesisIndex.Check(Ctx(), path)));
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
    [InlineData("| exploration-of-v1-archive-second-reading | exploration | v1-archive | 2026-09-21 |", new string[0])]
    [InlineData("| exploration-of-verified-artifacts | exploration | verified-artifacts | 2026-09-21 |", new string[0])]
    [InlineData("| verification-of-v1-archive-links | verification | v1-archive | 2026-09-21 |", new string[0])]
    [InlineData("| audit-of-revision-4 | audit | skill | 2026-09-21 |", new string[0])]
    [InlineData("| exploration-of-nowhere | exploration | nowhere | 2026-09-21 |", new[] { "registry.corpus" })]
    [InlineData("| verification-of-v1-archive | verification | v1-archive | 2026-09-21 |", new[] { "registry.corpus" })]
    [InlineData("| verification-of-v1-archive-1 | verification | fimfiction-stories | 2026-09-21 |", new[] { "registry.corpus" })]
    [InlineData("| audit-of-revision-4 | audit | v1-archive | 2026-09-21 |", new[] { "registry.corpus" })]
    [InlineData("| round-of-v1-archive-1 | verification | v1-archive | 2026-09-21 |", new[] { "registry.id" })]
    [InlineData("| referee-1 | verification | candidates | 2026-09-21 |", new[] { "registry.id" })]
    [InlineData("| exploration-of-v1-archive-again | verification | v1-archive | 2026-09-21 |", new[] { "registry.type" })]
    [InlineData("| exploration-of-v1-archive-again | exploratory | v1-archive | 2026-09-21 |", new[] { "registry.type" })]
    [InlineData("| exploration-of-v1-archive-again | exploration | v1-archive | soon |", new[] { "registry.go" })]
    [InlineData("| exploration-of-v1-archive | exploration | v1-archive | 2026-09-12 |", new[] { "registry.duplicate" })]
    [InlineData("| something-else | exploration | v1-archive | 2026-09-21 |", new[] { "registry.id" })]
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
        var path = Write("docs/v3-framework/studies/exploration-of-v1-archive/leads.md", SchemaExamples.Block("leads-schema"));
        Assert.Empty(Rules(Leads.Check(Ctx(), path)));
    }

    [Fact]
    public void A_leads_artifact_in_the_wrong_folder_fails_its_title()
    {
        var path = Write("docs/v3-framework/studies/exploration-of-lineage/leads.md", SchemaExamples.Block("leads-schema"));
        Assert.Contains("leads.title", Rules(Leads.Check(Ctx(), path)));
    }

    [Fact]
    public void A_leads_artifact_missing_a_section_or_carrying_bins_fails()
    {
        var text = SchemaExamples.Block("leads-schema").Replace("## Proposed questions\n", "");
        var path = Write("docs/v3-framework/studies/exploration-of-v1-archive/leads.md", text);
        Assert.Contains("leads.sections", Rules(Leads.Check(Ctx(), path)));

        var bins = SchemaExamples.Block("leads-schema").Replace("## Proposed questions\n", "## Bins\n<none>\n\n## Proposed questions\n");
        path = Write("docs/v3-framework/studies/exploration-of-v1-archive/leads.md", bins);
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
        var (entries, findings) = Decisions.Read(Ctx(), path);
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
        var (entries, findings) = Decisions.Read(Ctx(), path);
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
    [InlineData("### own-fiction/heavy-dt-two-classes\n", "### own-fiction/Heavy-DT\n", "question.slug")]
    [InlineData("### own-fiction/narrator-register-outside-giyc\n", "### own-fiction/heavy-dt-two-classes\n", "question.slug")]
    [InlineData("### own-fiction/heavy-dt-two-classes\n", "### heavy-dt-two-classes\n", "question.slug")]
    [InlineData("### own-fiction/heavy-dt-two-classes\n", "### analysis-corpus/heavy-dt-two-classes\n", "question.slug")]
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

    // ---- directions ----

    const string StudyDir = "docs/v3-framework/studies/verification-of-v1-archive-stasis";

    string WriteDirections(string text, string folder = StudyDir, string name = "directions-1.md")
    {
        Write("docs/v3-framework/questions/v1-archive.md", "# v1-archive — questions\n\n### v1-archive/scene-notes-carry-designed-stasis\n\n- date: 2026-09-01\n- raised by: recall\n- question: q\n\n### v1-archive/links-name-the-gap\n\n- date: 2026-09-02\n- raised by: recall\n- question: q\n");
        return Write($"{folder}/{name}", text);
    }

    /// <summary>A fixture skill folder whose Artifacts table lets the engine find question lists, definitions and directions.</summary>
    CheckContext FullCtx(params string[] corpora)
    {
        File.WriteAllText(Path.Combine(_skill, "SKILL.md"), MapFixture.Skill);
        File.WriteAllText(Path.Combine(_skill, MapFixture.RefereeingFile), MapFixture.Refereeing);
        File.WriteAllText(Path.Combine(_skill, MapFixture.PromotingFile), MapFixture.Promoting);
        return Ctx(corpora);
    }

    [Fact]
    public void The_directions_example_passes_in_a_verification_study_and_resolves_its_questions()
    {
        var path = WriteDirections(SchemaExamples.Block("directions-schema"));
        var findings = Directions.Check(FullCtx(), path);
        Assert.Empty(Rules(findings));
        Assert.NotNull(SchemaCheckers.For(WellKnown.Directions));
    }

    [Theory]
    [InlineData("questions: v1-archive/scene-notes-carry-designed-stasis v1-archive/links-name-the-gap\n", "questions: v1-archive/nowhere\n", "directions.frontmatter")]
    [InlineData("questions: v1-archive/scene-notes-carry-designed-stasis v1-archive/links-name-the-gap\n", "questions: scene-notes\n", "directions.frontmatter")]
    [InlineData("questions: v1-archive/scene-notes-carry-designed-stasis v1-archive/links-name-the-gap\n", "", "directions.frontmatter")]
    [InlineData("## Criteria\n", "## Rules\n", "directions.sections")]
    [InlineData("## Classes\n", "## What to produce\n\n- x: line, y\n\n## Classes\n", "directions.sections")]
    [InlineData("- cannot-place: the criteria do not decide it\n", "", "directions.classes")]
    [InlineData("- no-stasis: <what an item shows>\n", "- designed-stasis: <what an item shows>\n", "directions.classes")]
    [InlineData("- no-stasis: <what an item shows>\n", "- No Stasis: <what an item shows>\n", "directions.classes")]
    [InlineData("2. <another>\n", "3. <another>\n", "directions.criteria")]
    [InlineData("- class: enum\n", "- class: label\n", "directions.output")]
    [InlineData("- decided by: line, the number of the criterion that decided it, or \"definition\" if none was needed\n", "- decided by: line\n", "directions.output")]
    [InlineData("- decided by: line, the number of the criterion that decided it, or \"definition\" if none was needed\n", "- class: line, again\n", "directions.output")]
    public void A_directions_example_with_one_thing_broken_fails_on_that_rule(string find, string replace, string rule)
    {
        var text = SchemaExamples.Block("directions-schema");
        Assert.Contains(find, text);
        var path = WriteDirections(text.Replace(find, replace));
        Assert.Contains(rule, Rules(Directions.Check(FullCtx(), path)));
    }

    [Fact]
    public void An_explorations_directions_have_how_to_read_and_no_classes_and_a_referees_no_questions()
    {
        const string exploration = "---\nquestions: v1-archive/links-name-the-gap\n---\n\n## What you are given\n\nA slice.\n\n## How to read\n\nWith the questions in view.\n\n## What to produce\n\n- leads: list of line, what was seen and where\n";
        var path = WriteDirections(exploration, "docs/v3-framework/studies/exploration-of-v1-archive");
        Assert.Empty(Rules(Directions.Check(FullCtx(), path)));

        var wrongKind = WriteDirections(exploration, StudyDir);
        Assert.Contains("directions.sections", Rules(Directions.Check(FullCtx(), wrongKind)));

        var referee = WriteDirections(SchemaExamples.Block("directions-schema").Replace("questions: v1-archive/scene-notes-carry-designed-stasis v1-archive/links-name-the-gap\n", ""), "docs/v3-framework/referee");
        Assert.Empty(Rules(Directions.Check(FullCtx(), referee)));
        var refereeWithQuestions = WriteDirections(SchemaExamples.Block("directions-schema"), "docs/v3-framework/referee");
        Assert.Contains("directions.frontmatter", Rules(Directions.Check(FullCtx(), refereeWithQuestions)));
    }

    [Fact]
    public void A_version_is_the_next_number_in_its_folder_and_the_body_hash_ignores_the_frontmatter()
    {
        WriteDirections(SchemaExamples.Block("directions-schema"));
        var third = WriteDirections(SchemaExamples.Block("directions-schema"), name: "directions-3.md");
        Assert.Contains("directions.version", Rules(Directions.Check(FullCtx(), third)));
        var second = WriteDirections(SchemaExamples.Block("directions-schema"), name: "directions-2.md");
        Assert.DoesNotContain("directions.version", Rules(Directions.Check(FullCtx(), second)));

        var a = DirectionsFile.Parse(SchemaExamples.Block("directions-schema"));
        var b = DirectionsFile.Parse(SchemaExamples.Block("directions-schema").Replace("v1-archive/links-name-the-gap", "v1-archive/other"));
        Assert.Equal(a.BodyHash, b.BodyHash);
        Assert.NotEqual(a.BodyHash, DirectionsFile.Parse(SchemaExamples.Block("directions-schema") + "\nMore.\n").BodyHash);
    }

    // ---- index ----

    const string BatchDir = StudyDir + "/batches/03-scene-notes";

    [Fact]
    public void The_index_example_passes_at_its_batch_folder()
    {
        var path = Write(BatchDir + "/index.md", SchemaExamples.Block("index-schema"));
        Assert.Empty(Rules(BatchIndex.Check(Ctx("v1-archive"), path)));
        Assert.NotNull(SchemaCheckers.For(WellKnown.Index));
    }

    [Theory]
    [InlineData("# 03-scene-notes — index\n", "# 03-scene-notes\n", "index.title")]
    [InlineData("- corpus: v1-archive\n", "- corpus: nowhere\n", "index.head")]
    [InlineData("- corpus: v1-archive\n", "", "index.head")]
    [InlineData("- itemizer: tools/StoryPlanner.ArchiveItemizer, 2026-09-19 a1b2c3d\n- corpus: v1-archive\n", "- corpus: v1-archive\n- itemizer: tools/StoryPlanner.ArchiveItemizer, 2026-09-19 a1b2c3d\n", "index.head")]
    [InlineData("- locator notation: a v1-archive note id, `note-<id>`, as the MCP archive tools take it\n", "- locator notation: a v1-archive note id, `note-<id>`, as the MCP archive tools take it\n- source hash: abc\n", "index.head")]
    [InlineData("| item | locator | description |\n", "| id | locator | description |\n", "index.table")]
    [InlineData("| note-1630 | note-1630 | Griffonian Republic, History track, 2026-03 |\n| note-1702 | note-1702 | Grover III's Enlightenment, Causality of Creation |\n", "", "index.table")]
    [InlineData("| note-1702 | note-1702 | Grover III's Enlightenment, Causality of Creation |\n", "| note-1702 | note-1702 | Grover III's Enlightenment, Causality of Creation |\nA stray line.\n", "index.table")]
    [InlineData("| note-1702 | note-1702 |", "| note-1630 | note-1702 |", "index.item")]
    [InlineData("| note-1702 | note-1702 |", "| Note 1702 | note-1702 |", "index.item")]
    [InlineData("| note-1702 | note-1702 |", "| note-1702 |  |", "index.locator")]
    public void An_index_example_with_one_thing_broken_fails_on_that_rule(string find, string replace, string rule)
    {
        var text = SchemaExamples.Block("index-schema");
        Assert.Contains(find, text);
        var path = Write(BatchDir + "/index.md", text.Replace(find, replace));
        Assert.Contains(rule, Rules(BatchIndex.Check(Ctx("v1-archive"), path)));
    }

    [Fact]
    public void An_index_over_the_candidates_or_the_skill_names_a_corpus_no_file_declares()
    {
        var text = SchemaExamples.Block("index-schema").Replace("- corpus: v1-archive\n", "- corpus: candidates\n");
        var path = Write(BatchDir + "/index.md", text);
        Assert.Empty(Rules(BatchIndex.Check(Ctx("v1-archive"), path)));
        var another = Write(BatchDir + "/index.md", text.Replace("candidates", "skill"));
        Assert.Empty(Rules(BatchIndex.Check(Ctx("v1-archive"), another)));
    }

    // ---- findings ----

    const string FindingsStudy = "docs/v3-framework/studies/verification-of-v1-archive-scene-stasis";

    /// <summary>The tree the findings example cites: the directions that froze its question, a full batch with its definition and index, and the corpus's question list.</summary>
    string WriteFindings(string text)
    {
        WriteDirections(SchemaExamples.Block("directions-schema"), FindingsStudy);
        Write("docs/v3-framework/questions/v1-archive.md", "# v1-archive — questions\n\n### v1-archive/scene-notes-carry-designed-stasis\n\n- date: 2026-09-01\n- raised by: recall\n- question: q\n\n### v1-archive/links-name-the-gap\n\n- date: 2026-09-02\n- raised by: recall\n- question: q\n\n### v1-archive/unfrozen-one\n\n- date: 2026-09-03\n- raised by: recall\n- question: q\n");
        Write($"{FindingsStudy}/batches/01-full/definition.md", "# 01-full — definition\n\n- directions: ../../directions-1.md\n- kind: full\n- model: sonnet\n");
        Write($"{FindingsStudy}/batches/01-full/index.md", "# 01-full — index\n\n- itemizer: tools/StoryPlanner.ArchiveItemizer, 1\n- corpus: v1-archive\n- locator notation: a note id\n\n| item | locator | description |\n|---|---|---|\n| note-1630 | note-1630 | a |\n| note-2044 | note-2044 | b |\n| note-2051 | note-2051 | c |\n");
        return Write($"{FindingsStudy}/findings.md", text);
    }

    [Fact]
    public void The_findings_example_passes_with_its_batch_and_questions_resolving_and_only_standing_findings_answer()
    {
        var path = WriteFindings(SchemaExamples.Block("findings-schema"));
        var findings = FindingsChecker.Check(FullCtx("v1-archive"), path);
        Assert.Empty(Rules(findings));
        Assert.NotNull(SchemaCheckers.For(WellKnown.Findings));
        // The withdrawn finding and its successor name no question; the one standing finding with a question answers it.
        Assert.Equal(["v1-archive/scene-notes-carry-designed-stasis"], FindingsChecker.AnsweredQuestions(File.ReadAllText(path)));
        var entries = FindingsChecker.ReadEntries(File.ReadAllText(path));
        Assert.Equal(4, entries.Count);
        Assert.True(entries[3].Withdrawn);
        Assert.Equal("verification-of-v1-archive-scene-stasis/unplaced-notes-are-paratext", entries[2].Supersedes);
    }

    [Theory]
    [InlineData("# verification-of-v1-archive-scene-stasis — findings\n", "# findings\n", "findings.title")]
    [InlineData("## Method\n", "## Methods\n", "findings.shape")]
    [InlineData("### verification-of-v1-archive-scene-stasis/stasis-in-one-of-nine-scene-notes\n", "### elsewhere/stasis-in-one-of-nine-scene-notes\n", "findings.entry")]
    [InlineData("- question: v1-archive/scene-notes-carry-designed-stasis\n", "- question: v1-archive/nowhere\n", "findings.question")]
    [InlineData("- question: v1-archive/scene-notes-carry-designed-stasis\n", "- question: v1-archive/unfrozen-one\n", "findings.question")]
    [InlineData("  - verification-of-v1-archive-scene-stasis/01-full/note-2051\n", "  - verification-of-v1-archive-scene-stasis/01-full/note-9999\n", "findings.cites")]
    [InlineData("  - verification-of-v1-archive-scene-stasis/01-full § class\n", "  - verification-of-v1-archive-scene-stasis/01-full § colour\n", "findings.cites")]
    [InlineData("  - verification-of-v1-archive-scene-stasis/01-full § class\n", "  - verification-of-v1-archive-scene-stasis/09-none § class\n", "findings.cites")]
    [InlineData("- supersedes: verification-of-v1-archive-scene-stasis/unplaced-notes-are-paratext\n", "- supersedes: verification-of-v1-archive-scene-stasis/nothing-here\n", "findings.supersedes")]
    [InlineData("- withdrawn: 2026-09-20 the locators put 402 of 803 event notes after chapter three\n", "- withdrawn: yesterday x\n", "findings.withdrawn")]
    [InlineData("### verification-of-v1-archive-scene-stasis/event-notes-cluster-early\n", "### verification-of-v1-archive-scene-stasis/event-notes-cluster-early\n- supersedes: verification-of-v1-archive-scene-stasis/unplaced-notes-are-paratext\n", "findings.supersedes")]
    [InlineData("- directions: the reserved class `unplaced` took 189 of 1,116 notes;", "- model: the reserved class `unplaced` took 189 of 1,116 notes;", "findings.shortcoming")]
    public void A_findings_example_with_one_thing_broken_fails_on_that_rule(string find, string replace, string rule)
    {
        var text = SchemaExamples.Block("findings-schema");
        Assert.Contains(find, text);
        var path = WriteFindings(text.Replace(find, replace));
        Assert.Contains(rule, Rules(FindingsChecker.Check(FullCtx("v1-archive"), path)));
    }

    // ---- declined candidates ----

    const string DeclinedStudy = "docs/v3-framework/studies/verification-of-v1-archive-scene-stasis";

    /// <summary>The tree the declined-candidates example references: this study's findings.md with the two standing findings its headings name, and the two hypothesis files its targets name.</summary>
    string WriteDeclined(string text)
    {
        Write($"{DeclinedStudy}/findings.md",
            "# verification-of-v1-archive-scene-stasis — findings\n\n## Findings\n\n"
            + "### verification-of-v1-archive-scene-stasis/unplaced-notes-are-paratext\n- finding: x\n\n"
            + "### verification-of-v1-archive-scene-stasis/event-notes-cluster-early\n- finding: y\n");
        Write("docs/v3-framework/hypotheses/031-dt-knowledge-asymmetry.md", "# stub\n");
        Write("docs/v3-framework/hypotheses/029-perception-gap-delivery.md", "# stub\n");
        return Write($"{DeclinedStudy}/declined-candidates.md", text);
    }

    [Fact]
    public void The_declined_candidates_example_passes_with_its_findings_and_hypotheses_resolving()
    {
        var path = WriteDeclined(SchemaExamples.Block("declined-candidates-schema"));
        Assert.Empty(Rules(DeclinedCandidates.Check(FullCtx("v1-archive"), path)));
        Assert.NotNull(SchemaCheckers.For(WellKnown.DeclinedCandidates));
        Assert.Contains(WellKnown.DeclinedCandidates, SchemaCheckers.CheckedIds);
    }

    [Theory]
    [InlineData("# verification-of-v1-archive-scene-stasis — declined candidates\n", "# elsewhere — declined candidates\n", "declined-candidates.title")]
    [InlineData("### unplaced-notes-are-paratext → 031-dt-knowledge-asymmetry\n", "### unplaced-notes-are-paratext 031-dt-knowledge-asymmetry\n", "declined-candidates.entry")]
    [InlineData("- reason: <Brian's reason for declining, in his words>\n", "- reason:\n", "declined-candidates.entry")]
    [InlineData("- date: 2026-09-22\n- reason: <Brian's reason for declining, in his words>\n", "- reason: <Brian's reason for declining, in his words>\n", "declined-candidates.entry")]
    [InlineData("### unplaced-notes-are-paratext → 031-dt-knowledge-asymmetry\n", "### no-such-finding → 031-dt-knowledge-asymmetry\n", "declined-candidates.references")]
    [InlineData("### event-notes-cluster-early → 029-perception-gap-delivery\n", "### event-notes-cluster-early → 099-nonexistent\n", "declined-candidates.references")]
    public void A_declined_candidates_example_with_one_thing_broken_fails_on_that_rule(string find, string replace, string rule)
    {
        var text = SchemaExamples.Block("declined-candidates-schema");
        Assert.Contains(find, text);
        var path = WriteDeclined(text.Replace(find, replace));
        Assert.Contains(rule, Rules(DeclinedCandidates.Check(FullCtx("v1-archive"), path)));
    }

    // ---- definition ----

    (string Definition, string Directions) WriteBatch(string definition, string batch = "01-full", string directions = "", string calibration = "")
    {
        WriteDirections(SchemaExamples.Block("directions-schema"), name: "directions-1.md");
        WriteDirections(SchemaExamples.Block("directions-schema"), name: "directions-2.md");
        var d = WriteDirections(directions.Length > 0 ? directions : SchemaExamples.Block("directions-schema"), name: "directions-3.md");
        var hash = DirectionsFile.Read(d).BodyHash;
        Write($"{StudyDir}/calibration-2026-09-19b.md", calibration.Length > 0 ? calibration :
            $"# Calibration — directions-3@{hash[..10]} — 2026-09-19b\n\n## Sample\nx\n\n## Verdict\nBrian: accepted at this hash.\n");
        var path = Write($"{StudyDir}/batches/{batch}/definition.md", definition.Replace("# 03-scene-notes — definition", $"# {batch} — definition"));
        return (path, d);
    }

    [Fact]
    public void The_definition_example_passes_with_its_directions_and_calibration_resolving()
    {
        var (path, _) = WriteBatch(SchemaExamples.Block("definition-schema"));
        var findings = Definition.Check(FullCtx(), path);
        Assert.Empty(Rules(findings));
        Assert.NotNull(SchemaCheckers.For(WellKnown.Definition));
    }

    [Theory]
    [InlineData("# 03-scene-notes — definition\n", "# scene-notes — definition\n", "definition.title")]
    [InlineData("- directions: ../../directions-3.md\n", "- directions: ../../directions-9.md\n", "definition.directions")]
    [InlineData("- directions: ../../directions-3.md\n", "", "definition.fields")]
    [InlineData("- directions: ../../directions-3.md\n- kind: full\n", "- kind: full\n- directions: ../../directions-3.md\n", "definition.fields")]
    [InlineData("- kind: full\n", "- kind: pilot\n", "definition.fields")]
    [InlineData("- kind: full\n", "", "definition.fields")]
    [InlineData("- effort: high\n", "- effort: extreme\n", "definition.fields")]
    [InlineData("- effort: high\n", "- effort: high\n- timeout: 20\n", "definition.fields")]
    [InlineData("- calibration: ../../calibration-2026-09-19b.md\n", "", "definition.calibration")]
    [InlineData("- calibration: ../../calibration-2026-09-19b.md\n", "- calibration: ../../calibration-2026-09-01.md\n", "definition.calibration")]
    [InlineData("- kind: full\n- calibration: ../../calibration-2026-09-19b.md\n", "- kind: sample\n- calibration: ../../calibration-2026-09-19b.md\n", "definition.calibration")]
    public void A_definition_example_with_one_thing_broken_fails_on_that_rule(string find, string replace, string rule)
    {
        var text = SchemaExamples.Block("definition-schema");
        Assert.Contains(find, text);
        var (path, _) = WriteBatch(text.Replace(find, replace));
        Assert.Contains(rule, Rules(Definition.Check(FullCtx(), path)));
    }

    [Fact]
    public void The_calibration_must_judge_the_named_directions_at_their_hash_and_accept_them()
    {
        var (path, _) = WriteBatch(SchemaExamples.Block("definition-schema"), calibration: "# Calibration — directions-3@abcdef123456 — 2026-09-19b\n\n## Verdict\nBrian: accepted.\n");
        Assert.Contains("definition.calibration", Rules(Definition.Check(FullCtx(), path)));

        var (rejected, d) = WriteBatch(SchemaExamples.Block("definition-schema"));
        var hash = DirectionsFile.Read(d).BodyHash;
        Write($"{StudyDir}/calibration-2026-09-19b.md", $"# Calibration — directions-3@{hash[..8]} — 2026-09-19b\n\n## Verdict\nBrian: not accepted; a new version.\n");
        Assert.Contains("definition.calibration", Rules(Definition.Check(FullCtx(), rejected)));
    }

    [Fact]
    public void A_definition_whose_directions_fail_their_schema_fails_directions()
    {
        var (path, _) = WriteBatch(SchemaExamples.Block("definition-schema"), directions: SchemaExamples.Block("directions-schema").Replace("2. <another>\n", "4. <another>\n"));
        Assert.Contains("definition.directions", Rules(Definition.Check(FullCtx(), path)));
    }

    [Fact]
    public void The_batch_folder_is_numbered_in_sequence_and_a_study_has_one_model()
    {
        var (first, _) = WriteBatch(SchemaExamples.Block("definition-schema"), batch: "01-sample");
        Assert.Empty(Rules(Definition.Check(FullCtx(), first)));
        var (third, _) = WriteBatch(SchemaExamples.Block("definition-schema"), batch: "03-full");
        Assert.Contains("definition.batch", Rules(Definition.Check(FullCtx(), third)));
        var (second, _) = WriteBatch(SchemaExamples.Block("definition-schema").Replace("claude-opus-4-6", "sonnet"), batch: "02-full");
        Assert.Contains("definition.model", Rules(Definition.Check(FullCtx(), second)));
        var (dup, _) = WriteBatch(SchemaExamples.Block("definition-schema"), batch: "02-sample");
        Assert.Contains("definition.batch", Rules(Definition.Check(FullCtx(), dup)));
        var (bad, _) = WriteBatch(SchemaExamples.Block("definition-schema"), batch: "2-full");
        Assert.Contains("definition.batch", Rules(Definition.Check(FullCtx(), bad)));
    }

    [Fact]
    public void A_definition_edited_after_its_first_execution_fails_frozen()
    {
        var (path, _) = WriteBatch(SchemaExamples.Block("definition-schema"));
        var hash = DefinitionFile.Read(path).Hash;
        Write($"{StudyDir}/batches/01-full/calls.md", CallsFile.RenderHead("01-full", hash));
        Assert.Empty(Rules(Definition.Check(FullCtx(), path)));
        File.AppendAllText(path, "- tools:\n  - Read\n");
        Assert.Contains("definition.frozen", Rules(Definition.Check(FullCtx(), path)));
    }

    // ---- results ----

    [Fact]
    public void Results_are_held_to_the_directions_declaration_through_the_definition()
    {
        var (path, _) = WriteBatch(SchemaExamples.Block("definition-schema"));
        Write($"{StudyDir}/batches/01-full/index.md", SchemaExamples.Block("index-schema").Replace("03-scene-notes", "01-full"));
        Write($"{StudyDir}/batches/01-full/results/note-1630.md", "- class: designed-stasis\n- decided by: 1\n");
        Assert.Empty(Rules(Results.CheckBatch(FullCtx(), path)));

        Write($"{StudyDir}/batches/01-full/results/note-1702.md", "- class: stasis\n- decided by: 1\n");
        Assert.Contains(Results.CheckId, Rules(Results.CheckBatch(FullCtx(), path)));
        Write($"{StudyDir}/batches/01-full/results/note-1702.md", "- decided by: 1\n- class: designed-stasis\n");
        Assert.Contains(Results.CheckId, Rules(Results.CheckBatch(FullCtx(), path)));
        Write($"{StudyDir}/batches/01-full/results/note-1702.md", "- class: designed-stasis\n- decided by: 1\n- extra: x\n");
        Assert.Contains(Results.CheckId, Rules(Results.CheckBatch(FullCtx(), path)));
        Write($"{StudyDir}/batches/01-full/results/note-1702.md", "- class: designed-stasis\n- decided by: 1\n");
        Write($"{StudyDir}/batches/01-full/results/note-9999.md", "- class: designed-stasis\n- decided by: 1\n");
        Assert.Contains(Results.CheckId, Rules(Results.CheckBatch(FullCtx(), path)));
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
        var definition = ArtifactScope.Locate(Path.Combine(f.BatchDir, "definition.md"));
        Assert.Equal(WellKnown.Definition, definition!.Row.Id);
        Assert.Null(ArtifactScope.Locate(f.TreePath("docs", "note.md")));
    }

    [Fact]
    public void A_file_no_row_matches_is_governed_by_a_definition_that_names_it()
    {
        using var f = new MapFixture().WithStateTree();
        Directory.CreateDirectory(Path.Combine(f.RepoRoot, ".git"));
        var referee = f.TreePath("docs", "v3-framework", "referee", "directions-1.md");
        Directory.CreateDirectory(Path.GetDirectoryName(referee)!);
        File.WriteAllText(referee, MapFixture.DirectionsText.Replace("questions: analysis-corpus/does-the-dt-class-split\n", ""));
        Assert.Null(ArtifactScope.Locate(referee));
        Assert.Null(ReferenceScope.Locate(f.RepoRoot, referee));

        var batch = Path.Combine(f.StudyDir, "batches", "02-referee");
        Directory.CreateDirectory(batch);
        File.WriteAllText(Path.Combine(batch, "definition.md"), "# 02-referee — definition\n\n- directions: ../../../../referee/directions-1.md\n- kind: sample\n- model: sonnet\n");
        var governed = ReferenceScope.Locate(f.RepoRoot, referee);
        Assert.NotNull(governed);
        Assert.Equal(WellKnown.Directions, governed!.Row.Id);
        Assert.Empty(governed.Checker(governed.Context, referee).Where(x => x.Level == FindingLevel.Failure));

        var result = Path.Combine(f.BatchDir, "results", "item-001.md");
        var byResult = ReferenceScope.LocateResult(f.RepoRoot, result);
        Assert.Equal(WellKnown.Results, byResult!.Row.Id);
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

        // A result written into a batch is held to its directions through the definition.
        var result = Path.Combine(f.BatchDir, "results", "item-001.md");
        Assert.Equal(HookOutcomeKind.Silent, WriteHook.Run(Payload(result)).Kind);
        File.WriteAllText(result, "- class: z\n- why: 1\n");
        var badResult = WriteHook.Run(Payload(result));
        Assert.Equal(HookOutcomeKind.Failed, badResult.Kind);
        Assert.Contains(Results.CheckId, badResult.Message);
    }
}
