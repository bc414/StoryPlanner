using System.IO;
using System.Linq;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// One passing and one failing case per validation rule. Assertions are on rule ids, never on
/// message prose — the prose is allowed to improve without breaking a test.
/// </summary>
public class ValidatorTests
{
    const string Skill = MapFixture.SkillFile;
    const string Artifacts = MapFixture.SkillFile;
    const string Refereeing = MapFixture.RefereeingFile;
    const string Promoting = MapFixture.PromotingFile;

    static string[] Rules(MapFixture f)
        => f.Report.Findings.Where(x => x.Level == FindingLevel.Failure).Select(x => x.CheckId).ToArray();

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
            .Select(x => $"{x.CheckId} {x.RowId} {x.Message}")));
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
        => Fails("id.duplicate", MapFixture.With(Artifacts, "| items | docs/v3-framework/studies/<study>/batches/<batch>/items/", "| promote | docs/v3-framework/studies/<study>/batches/<batch>/items/"));

    [Fact]
    public void An_id_outside_the_lowercase_slug_charset_fails()
        => Fails("id.charset", MapFixture.With(Refereeing, "assess-referee-items", "Assess-Referee-Items"));

    [Fact]
    public void An_enables_cell_naming_no_activity_does_not_resolve()
        => Fails("ref.enables", MapFixture.With(Skill, "| refereeing-candidates | promoting-refereed-candidates |", "| refereeing-candidates | nowhere |"));

    [Fact]
    public void A_read_naming_no_artifact_does_not_resolve()
        => Fails("ref.reads", MapFixture.With(Refereeing, "| directions items | results |", "| directions nope | results |"));

    [Fact]
    public void A_write_naming_no_artifact_does_not_resolve()
        => Fails("ref.writes", MapFixture.With(Refereeing, "| directions items | results | specified |", "| directions items | nope | specified |"));

    const string CandidateCell = "| [candidate-schema](schemas/candidate-schema.md) |";

    [Fact]
    public void A_schema_naming_no_file_under_schemas_does_not_resolve()
        => Fails("ref.schema", MapFixture.With(Artifacts, CandidateCell, "| [candidates-schema](schemas/candidates-schema.md) |"));

    [Fact]
    public void A_schema_whose_link_text_is_not_a_slug_does_not_resolve()
        => Fails("ref.schema", MapFixture.With(Artifacts, CandidateCell, "| [Candidate-schema](schemas/Candidate-schema.md) |"));

    [Fact]
    public void A_schema_cell_that_is_a_bare_slug_is_not_a_link()
        => Fails("ref.schema", MapFixture.With(Artifacts, CandidateCell, "| candidate-schema |"));

    [Fact]
    public void A_schema_link_whose_target_is_not_the_file_its_text_names_fails()
        => Fails("ref.schema", MapFixture.With(Artifacts, CandidateCell, "| [candidate-schema](schemas/directions-schema.md) |"));

    [Fact]
    public void A_schema_id_without_the_suffix_fails()
        => Fails("ref.schema", MapFixture.With(Artifacts, CandidateCell, "| [candidate](schemas/candidate.md) |"));

    [Fact]
    public void A_schema_id_that_is_an_artifact_id_fails()
        => Fails("ref.schema", MapFixture.With(Artifacts, "| candidates | docs/v3-framework/studies/<study>/candidates.md |", "| candidate-schema | docs/v3-framework/studies/<study>/candidates.md |"));

    [Fact]
    public void A_schema_file_whose_title_is_not_its_id_fails_the_shape()
    {
        using var f = new MapFixture();
        f.WriteSchema("candidate-schema", "# Candidate-schema\n\nThe shape.\n");
        Assert.Contains("schema.shape", Rules(f));
    }

    [Fact]
    public void A_schema_file_no_row_names_is_an_orphan()
    {
        using var f = new MapFixture();
        f.WriteSchema("stray-schema", "# stray-schema\n");
        Assert.Contains("file.orphan-schema", Rules(f));
    }

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
        => Fails("enum.mode", MapFixture.With(Refereeing, "| assess-referee-items | agent |", "| assess-referee-items | robot |"));

    [Fact]
    public void Two_modes_on_one_row_fail_because_a_process_is_one_run_of_one_mode()
        => Fails("row.mode-count", MapFixture.With(Refereeing, "| assess-referee-items | agent |", "| assess-referee-items | agent session |"));

    [Fact]
    public void A_state_outside_the_closed_set_fails()
        => Fails("enum.state", MapFixture.With(Refereeing, "| results | specified | Writes the falsifier blind |", "| results | maybe | Writes the falsifier blind |"));

    [Fact]
    public void A_mutation_outside_the_closed_set_fails()
        => Fails("enum.mutation", MapFixture.With(Artifacts, "| succeeded |", "| versioned |"));

    // ---- row minima ----

    [Fact]
    public void A_process_reading_nothing_fails_because_it_is_deriving_from_recall()
        => Fails("row.reads-empty", MapFixture.With(Refereeing, "| assess-referee-items | agent | | directions items |", "| assess-referee-items | agent | |  |"));

    [Fact]
    public void A_process_writing_nothing_fails_because_it_is_indistinguishable_from_not_running()
        => Fails("row.writes-empty", MapFixture.With(Refereeing, "| directions items | results | specified |", "| directions items |  | specified |"));

    [Fact]
    public void An_hitl_process_writing_nothing_fails_under_its_own_rule_too()
        => Fails("row.hitl-writes-nothing", MapFixture.With(Promoting,
            "| hypothesis-record hypothesis-status candidates question-list | specified |",
            "|  | specified |"));

    [Fact]
    public void A_process_with_no_description_fails()
        => Fails("row.description-empty", MapFixture.With(Refereeing, "| specified | Writes the falsifier blind |", "| specified |  |"));

    // ---- artifacts ----

    [Fact]
    public void A_path_cell_naming_two_patterns_fails_the_syntax_rule()
        => Fails("artifact.path-syntax", MapFixture.With(Artifacts,
            "| docs/v3-framework/studies/<study>/directions-N.md |", "| docs/v3-framework/studies/<study>/directions-N.md, or docs/v3-framework/referee/directions-N.md |"));

    [Fact]
    public void An_artifact_no_process_reads_fails()
        => Fails("artifact.never-read", MapFixture.With(Artifacts,
            "| results | docs/v3-framework/studies/<study>/batches/<batch>/results/ | frozen | | The model's answers as rendered |",
            "| results | docs/v3-framework/studies/<study>/batches/<batch>/results/ | frozen | | The model's answers as rendered |\n| orphan | docs/orphan.md | frozen | | Nothing reads it |"));

    [Fact]
    public void An_artifact_no_process_writes_is_information_not_a_verdict()
    {
        using var f = new MapFixture();
        var report = f.Report;
        Assert.Contains(report.Findings, x => x.CheckId == "info.artifact.never-written" && x.RowId == "directions" && x.Level == FindingLevel.Info);
        Assert.True(report.Passed);
    }

    // ---- the enables graph ----

    [Fact]
    public void A_cycle_in_enables_fails()
        => Fails("enables.cycle", MapFixture.With(Skill, "| changing-the-planner-for-v3 | |", "| changing-the-planner-for-v3 | refereeing-candidates |"));

    [Fact]
    public void Two_activities_enabling_nothing_fail_the_one_terminus_rule()
        => Fails("enables.terminus-count", MapFixture.With(Skill, "| promoting-refereed-candidates | changing-the-planner-for-v3 |", "| promoting-refereed-candidates |  |"));

    [Fact]
    public void A_terminus_that_owns_processes_fails()
        => Fails("enables.terminus-owns-processes", MapFixture.With(Skill, "| promoting-refereed-candidates | changing-the-planner-for-v3 |", "| promoting-refereed-candidates |  |"));

    [Fact]
    public void An_enables_edge_nothing_flows_along_fails()
        => Fails("enables.unbacked", MapFixture.With(Promoting,
            "| git | candidates findings hypothesis-record hypothesis-status question-list |",
            "| git | findings hypothesis-record hypothesis-status question-list |"));

    [Fact]
    public void An_edge_into_the_terminus_is_exempt_and_said_so()
    {
        using var f = new MapFixture();
        var report = f.Report;
        Assert.DoesNotContain("enables.unbacked", Rules(f));
        Assert.Contains(report.Findings, x => x.CheckId == "enables.vacuous" && x.Level == FindingLevel.Vacuous);
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
        Assert.DoesNotContain(f.Report.Findings, x => x.CheckId == "gate.vacuous");
    }

    [Fact]
    public void A_gate_with_no_hypothesis_writer_is_reported_as_vacuous_rather_than_passing()
    {
        using var f = MapFixture.With(Promoting,
            "| hypothesis-record hypothesis-status candidates question-list | specified |",
            "| candidates question-list | specified |");
        var report = f.Report;
        Assert.Contains(report.Findings, x => x.CheckId == "gate.vacuous" && x.Level == FindingLevel.Vacuous);
        Assert.True(report.Passed);
    }

    // ---- questions are Brian's ----

    [Fact]
    public void A_non_hitl_writer_of_the_question_list_fails()
        => Fails("question-list.writer-not-hitl", MapFixture.With(Refereeing, "| definition index results candidates | candidates |", "| definition index results candidates | candidates question-list |"));

    // ---- the mutation rule, frozen only, series exempt ----

    [Fact]
    public void A_process_reading_and_writing_a_frozen_artifact_fails()
        => Fails("mutation.read-and-write", MapFixture.With(Refereeing, "| directions items | results |", "| directions items | results items |"));

    [Fact]
    public void A_process_reading_and_writing_a_frozen_series_is_not_reported_because_it_writes_the_next_member()
    {
        // assemble-referee-batch reads calibration (frozen, dated); make it write one too.
        using var f = MapFixture.With(Refereeing,
            "| studies calibration directions candidates | definition index items |",
            "| studies calibration directions candidates | definition index items calibration |");
        Assert.DoesNotContain("mutation.read-and-write", Rules(f));
    }

    [Fact]
    public void A_process_reading_and_writing_a_succeeded_or_append_artifact_is_not_reported()
    {
        // promote reads and writes candidates (append); make assess-referee-items read and write directions (succeeded).
        using var f = MapFixture.With(Refereeing, "| directions items | results |", "| directions items | results directions |");
        Assert.DoesNotContain("mutation.read-and-write", Rules(f));
    }

    // ---- the activity file's shape ----

    [Fact]
    public void A_section_that_is_not_a_process_id_fails_the_shape()
        => Fails("file.shape", MapFixture.With(Refereeing, "## assess-referee-items", "## assessing-referee-items"));

    [Fact]
    public void A_title_that_is_not_the_activity_id_fails_the_shape()
        => Fails("file.shape", MapFixture.With(Refereeing, "# refereeing-candidates", "# Refereeing"));

    // ---- decisions are cited only in revising-the-method ----

    [Fact]
    public void A_decision_id_in_a_standard_operating_activity_file_is_a_failure()
        => Fails("decision.id-outside-revising", MapFixture.With(Promoting, "Brian decides.", "Brian decides (decision d-2026-09-06-2)."));

    [Fact]
    public void A_decision_id_in_the_router_is_a_failure()
        => Fails("decision.id-outside-revising", MapFixture.With(Skill, "## Companions", "See d-2026-09-05-3.\n\n## Companions"));

    [Fact]
    public void A_decision_id_inside_a_fenced_example_is_the_shape_not_a_citation()
    {
        using var f = new MapFixture();
        f.WriteSchema("candidate-schema", "# candidate-schema\n\n```markdown\n- supersedes: d-2026-09-04-16\n```\n");
        Assert.DoesNotContain("decision.id-outside-revising", Rules(f));
    }

    [Fact]
    public void A_decision_id_in_a_schema_file_outside_a_fence_is_a_failure()
    {
        using var f = new MapFixture();
        f.WriteSchema("candidate-schema", "# candidate-schema\n\nPer d-2026-09-05-3.\n");
        Assert.Contains("decision.id-outside-revising", Rules(f));
    }

    [Fact]
    public void A_decision_id_in_revising_the_method_is_allowed()
    {
        using var f = MapFixture.WithExtra("revising-the-method.md", """
            # revising-the-method

            Enables nothing yet.

            ## Preconditions

            A finding, per d-2026-09-06-1.

            ## Never

            Nothing.
            """);
        // The extra file is an orphan (no router row names it), which is its own finding; the
        // decision id inside it must not be one.
        Assert.DoesNotContain("decision.id-outside-revising", Rules(f));
    }

    // ---- SKILL.md's published limits and the one-level-deep rule ----

    [Fact]
    public void A_companion_not_named_by_the_router_is_unreachable()
    {
        using var f = MapFixture.With(Skill, "map.md and state.md are generated only", "generated files exist");
        File.WriteAllText(Path.Combine(f.SkillFolder, "map.md"), "# map\n");
        Assert.Contains("skill.companion-unlinked", Rules(f));
    }

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
        var info = Assert.Single(f.Report.Findings, x => x.CheckId == "info.instrument.free-name");
        Assert.Equal(FindingLevel.Info, info.Level);
        Assert.Contains("git", info.Message);
        Assert.Contains("runner", info.Message);
    }

    [Fact]
    public void A_schema_value_no_row_uses_is_reported_without_failing()
    {
        using var f = new MapFixture();
        var report = f.Report;
        Assert.Contains(report.Findings, x => x.CheckId == "info.unused-enum-value" && x.Level == FindingLevel.Info);
        Assert.True(report.Passed);
    }
}
