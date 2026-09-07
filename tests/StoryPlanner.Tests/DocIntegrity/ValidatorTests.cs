using System.Linq;
using StoryPlanner.ProcessMap;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// One passing and one failing case per validation rule. Assertions are on rule ids, never on
/// message prose — the prose is allowed to improve without breaking a test.
/// </summary>
public class ValidatorTests
{
    const string Skill = MapFixture.SkillFile;
    const string Artifacts = MapFixture.ArtifactsFile;
    const string Refereeing = MapFixture.RefereeingFile;
    const string Promoting = MapFixture.PromotingFile;

    static string[] Rules(MapFixture f)
        => f.Report.Findings.Where(x => x.Level == FindingLevel.Failure).Select(x => x.RuleId).ToArray();

    static void Fails(string rule, MapFixture fixture)
    {
        using (fixture) Assert.Contains(rule, Rules(fixture));
    }

    [Fact]
    public void The_reference_skill_validates_clean()
    {
        using var f = new MapFixture();
        var report = f.Report;
        Assert.True(report.Passed, string.Join("\n", report.Findings
            .Where(x => x.Level == FindingLevel.Failure)
            .Select(x => $"{x.RuleId} {x.RowId} {x.Message}")));
    }

    // ---- structure ----

    [Fact]
    public void An_unparseable_table_is_one_finding_under_its_rule_id()
    {
        using var f = MapFixture.With(Promoting, "| Brian decides each candidate |", "| Brian decides each candidate | extra |");
        Assert.Equal([MapFormatException.Unparseable], Rules(f));
    }

    [Fact]
    public void A_table_of_an_unknown_signature_is_one_finding_under_its_rule_id()
    {
        using var f = MapFixture.With(Promoting,
            "Promotes what Brian did not decide.",
            "Promotes what Brian did not decide.\n\n| a | b |\n|---|---|\n| one | two |\n");
        Assert.Equal([SkillReader.UnknownSignature], Rules(f));
    }

    [Fact]
    public void A_markdown_file_no_router_row_names_is_an_orphan_activity()
        => Fails("file.orphan-activity", MapFixture.WithExtra("stray.md", "# stray\n"));

    // ---- ids and references ----

    [Fact]
    public void An_id_used_in_two_tables_is_a_duplicate()
        => Fails("id.duplicate", MapFixture.With(Artifacts, "| items | fanout/<instance>/<run>/items/", "| promote | fanout/<instance>/<run>/items/"));

    [Fact]
    public void An_id_outside_the_lowercase_slug_charset_fails()
        => Fails("id.charset", MapFixture.With(Refereeing, "referee-judge", "Referee-Judge"));

    [Fact]
    public void An_enables_cell_naming_no_activity_does_not_resolve()
        => Fails("ref.enables", MapFixture.With(Skill, "| refereeing-a-candidate | promoting-checked-candidates |", "| refereeing-a-candidate | nowhere |"));

    [Fact]
    public void A_read_naming_no_artifact_does_not_resolve()
        => Fails("ref.reads", MapFixture.With(Refereeing, "| codebook items | results |", "| codebook nope | results |"));

    [Fact]
    public void A_write_naming_no_artifact_does_not_resolve()
        => Fails("ref.writes", MapFixture.With(Refereeing, "| codebook items | results | specified |", "| codebook items | nope | specified |"));

    [Fact]
    public void A_format_naming_no_heading_in_artifacts_md_does_not_resolve()
        => Fails("ref.format", MapFixture.With(Artifacts, "| Candidate |", "| Candidates |"));

    [Fact]
    public void An_activity_that_is_not_the_terminus_needs_its_file()
        => Fails("ref.companion", MapFixture.Without(Promoting));

    [Fact]
    public void The_terminus_needs_no_file()
    {
        using var f = new MapFixture();
        Assert.DoesNotContain("ref.companion", Rules(f));
    }

    // ---- closed sets ----

    [Fact]
    public void A_mode_outside_the_closed_set_fails()
        => Fails("enum.mode", MapFixture.With(Refereeing, "| referee-judge | agent |", "| referee-judge | robot |"));

    [Fact]
    public void Two_modes_on_one_row_fail_because_a_process_is_one_run_of_one_mode()
        => Fails("row.mode-count", MapFixture.With(Refereeing, "| referee-judge | agent |", "| referee-judge | agent session |"));

    [Fact]
    public void A_state_outside_the_closed_set_fails()
        => Fails("enum.state", MapFixture.With(Refereeing, "| results | specified | Writes the falsifier blind |", "| results | maybe | Writes the falsifier blind |"));

    [Fact]
    public void A_mutation_outside_the_closed_set_fails()
        => Fails("enum.mutation", MapFixture.With(Artifacts, "| succeeded |", "| versioned |"));

    // ---- row minima ----

    [Fact]
    public void A_process_reading_nothing_fails_because_it_is_deriving_from_recall()
        => Fails("row.reads-empty", MapFixture.With(Refereeing, "| referee-judge | agent | | codebook items |", "| referee-judge | agent | |  |"));

    [Fact]
    public void A_process_writing_nothing_fails_because_it_is_indistinguishable_from_not_running()
        => Fails("row.writes-empty", MapFixture.With(Refereeing, "| codebook items | results | specified |", "| codebook items |  | specified |"));

    [Fact]
    public void An_hitl_process_writing_nothing_fails_under_its_own_rule_too()
        => Fails("row.hitl-writes-nothing", MapFixture.With(Promoting,
            "| hypothesis-record hypothesis-status candidates question-list verification-artifact | specified |",
            "|  | specified |"));

    [Fact]
    public void A_process_with_no_description_fails()
        => Fails("row.description-empty", MapFixture.With(Refereeing, "| specified | Writes the falsifier blind |", "| specified |  |"));

    // ---- artifacts ----

    [Fact]
    public void A_path_cell_naming_two_patterns_fails_the_syntax_rule()
        => Fails("artifact.path-syntax", MapFixture.With(Artifacts,
            "| fanout/<instance>/codebook-N.md |", "| fanout/<instance>/codebook-N.md, or fanout/referee/codebook-N.md |"));

    [Fact]
    public void An_artifact_no_process_reads_fails()
        => Fails("artifact.never-read", MapFixture.With(Artifacts,
            "| results | fanout/<instance>/<run>/results/ | frozen | | The agents' outputs |",
            "| results | fanout/<instance>/<run>/results/ | frozen | | The agents' outputs |\n| orphan | docs/orphan.md | frozen | | Nothing reads it |"));

    [Fact]
    public void An_artifact_no_process_writes_is_information_not_a_verdict()
    {
        using var f = new MapFixture();
        var report = f.Report;
        Assert.Contains(report.Findings, x => x.RuleId == "info.artifact.never-written" && x.RowId == "codebook" && x.Level == FindingLevel.Info);
        Assert.True(report.Passed);
    }

    // ---- the enables graph ----

    [Fact]
    public void A_cycle_in_enables_fails()
        => Fails("enables.cycle", MapFixture.With(Skill, "| changing-the-planner-for-v3 | |", "| changing-the-planner-for-v3 | refereeing-a-candidate |"));

    [Fact]
    public void Two_activities_enabling_nothing_fail_the_one_terminus_rule()
        => Fails("enables.terminus-count", MapFixture.With(Skill, "| promoting-checked-candidates | changing-the-planner-for-v3 |", "| promoting-checked-candidates |  |"));

    [Fact]
    public void A_terminus_that_owns_processes_fails()
        => Fails("enables.terminus-owns-processes", MapFixture.With(Skill, "| promoting-checked-candidates | changing-the-planner-for-v3 |", "| promoting-checked-candidates |  |"));

    [Fact]
    public void An_enables_edge_nothing_flows_along_fails()
        => Fails("enables.unbacked", MapFixture.With(Promoting,
            "| git | candidates hypothesis-record hypothesis-status question-list verification-artifact |",
            "| git | hypothesis-record hypothesis-status question-list verification-artifact |"));

    [Fact]
    public void An_edge_into_the_terminus_is_exempt_and_said_so()
    {
        using var f = new MapFixture();
        var report = f.Report;
        Assert.DoesNotContain("enables.unbacked", Rules(f));
        Assert.Contains(report.Findings, x => x.RuleId == "enables.vacuous" && x.Level == FindingLevel.Vacuous);
    }

    // ---- the hitl gate ----

    [Fact]
    public void A_session_row_reading_candidates_and_writing_a_hypothesis_artifact_is_ungated()
        => Fails("gate.ungated", MapFixture.With(Promoting, "| promote | hitl |", "| promote | session |"));

    [Fact]
    public void An_hitl_writer_gates_the_path()
    {
        using var f = new MapFixture();
        Assert.DoesNotContain("gate.ungated", Rules(f));
        Assert.DoesNotContain(f.Report.Findings, x => x.RuleId == "gate.vacuous");
    }

    [Fact]
    public void A_gate_with_no_hypothesis_writer_is_reported_as_vacuous_rather_than_passing()
    {
        using var f = MapFixture.With(Promoting,
            "| hypothesis-record hypothesis-status candidates question-list verification-artifact | specified |",
            "| candidates question-list verification-artifact | specified |");
        var report = f.Report;
        Assert.Contains(report.Findings, x => x.RuleId == "gate.vacuous" && x.Level == FindingLevel.Vacuous);
        Assert.True(report.Passed);
    }

    // ---- questions are Brian's ----

    [Fact]
    public void A_non_hitl_writer_of_the_question_list_fails()
        => Fails("question-list.writer-not-hitl", MapFixture.With(Refereeing, "| results candidates | candidates |", "| results candidates | candidates question-list |"));

    // ---- the mutation rule, frozen only, series exempt ----

    [Fact]
    public void A_process_reading_and_writing_a_frozen_artifact_fails()
        => Fails("mutation.read-and-write", MapFixture.With(Refereeing, "| codebook items | results |", "| codebook items | results items |"));

    [Fact]
    public void A_process_reading_and_writing_a_frozen_series_is_not_reported_because_it_writes_the_next_member()
    {
        // referee-run reads calibration-record (frozen, dated); make it write one too.
        using var f = MapFixture.With(Refereeing,
            "| instances calibration-record codebook candidates | items |",
            "| instances calibration-record codebook candidates | items calibration-record |");
        Assert.DoesNotContain("mutation.read-and-write", Rules(f));
    }

    [Fact]
    public void A_process_reading_and_writing_a_succeeded_or_append_artifact_is_not_reported()
    {
        // promote reads and writes candidates (append); make referee-judge read and write codebook (succeeded).
        using var f = MapFixture.With(Refereeing, "| codebook items | results |", "| codebook items | results codebook |");
        Assert.DoesNotContain("mutation.read-and-write", Rules(f));
    }

    // ---- the activity file's shape ----

    [Fact]
    public void A_section_that_is_not_a_process_id_fails_the_shape()
        => Fails("file.shape", MapFixture.With(Refereeing, "## referee-judge", "## referee-judging"));

    [Fact]
    public void A_title_that_is_not_the_activity_id_fails_the_shape()
        => Fails("file.shape", MapFixture.With(Refereeing, "# refereeing-a-candidate", "# Refereeing"));

    // ---- SKILL.md's published limits and the one-level-deep rule ----

    [Fact]
    public void A_companion_not_named_by_the_router_is_unreachable()
        => Fails("skill.companion-unlinked", MapFixture.With(Skill, "artifacts.md holds the Artifacts table; ", ""));

    [Fact]
    public void An_activity_file_is_linked_by_its_router_row()
    {
        using var f = new MapFixture();
        Assert.DoesNotContain("skill.companion-unlinked", Rules(f));
    }

    [Fact]
    public void A_description_over_the_published_limit_fails()
    {
        var long_ = new string('x', Validator.SkillDescriptionBudget + 1);
        using var f = MapFixture.With(Skill, "description: An example skill for the process map tests.", $"description: {long_}");
        Assert.Contains("skill.description-length", Rules(f));
    }

    [Fact]
    public void A_skill_over_the_published_line_budget_fails()
    {
        var padding = string.Join("\n", Enumerable.Repeat("filler", Validator.SkillLineBudget + 1));
        using var f = MapFixture.With(Skill, "## Companions", $"{padding}\n\n## Companions");
        Assert.Contains("skill.line-budget", Rules(f));
    }

    // ---- informational, never a verdict ----

    [Fact]
    public void Free_named_instruments_are_listed_once_without_failing()
    {
        using var f = new MapFixture();
        var info = Assert.Single(f.Report.Findings, x => x.RuleId == "info.instrument.free-name");
        Assert.Equal(FindingLevel.Info, info.Level);
        Assert.Contains("git", info.Message);
        Assert.Contains("runner", info.Message);
    }

    [Fact]
    public void A_schema_value_no_row_uses_is_reported_without_failing()
    {
        using var f = new MapFixture();
        var report = f.Report;
        Assert.Contains(report.Findings, x => x.RuleId == "info.unused-enum-value" && x.Level == FindingLevel.Info);
        Assert.True(report.Passed);
    }
}
