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
    static string[] Rules(MapFixture f)
        => Validator.Validate(f.RepoRoot, f.SkillFolder).Findings
            .Where(x => x.Level == FindingLevel.Failure)
            .Select(x => x.RuleId)
            .ToArray();

    static void Fails(string rule, MapFixture fixture)
    {
        using (fixture) Assert.Contains(rule, Rules(fixture));
    }

    [Fact]
    public void The_reference_map_validates_clean()
    {
        using var f = new MapFixture();
        var report = Validator.Validate(f.RepoRoot, f.SkillFolder);
        Assert.True(report.Passed, string.Join("\n", report.Findings
            .Where(x => x.Level == FindingLevel.Failure)
            .Select(x => $"{x.RuleId} {x.RowId} {x.Message}")));
    }

    // ---- ids and references ----

    [Fact]
    public void An_id_used_in_two_tables_is_a_duplicate()
        => Fails("id.duplicate", MapFixture.With("| f.a | docs/a.md", "| G1 | docs/a.md"));

    [Fact]
    public void An_input_naming_no_file_row_does_not_resolve()
        => Fails("ref.file", MapFixture.With("| hitl:fable | f.hyp | f.a |", "| hitl:fable | f.nope | f.a |"));

    [Fact]
    public void A_root_cell_naming_no_root_row_does_not_resolve()
        => Fails("ref.root", MapFixture.With("| f.a | G1 | docs/proc.md", "| f.a | G9 | docs/proc.md"));

    [Fact]
    public void An_edge_endpoint_naming_no_process_does_not_resolve()
        => Fails("ref.edge", MapFixture.With("| P.0 | P.1 | flow |", "| P.0 | P.99 | flow |"));

    // ---- closed sets ----

    [Fact]
    public void A_root_kind_outside_the_closed_set_fails()
        => Fails("enum.root-kind", MapFixture.With("| G1 | goal |", "| G1 | wish |"));

    [Fact]
    public void A_keep_value_outside_the_closed_set_fails()
        => Fails("enum.keep", MapFixture.With("| f.a | docs/a.md | committed |", "| f.a | docs/a.md | kept |"));

    [Fact]
    public void A_level_outside_the_closed_set_fails()
        => Fails("enum.level", MapFixture.With("| P.1 | P | sop |", "| P.1 | Z | sop |"));

    [Fact]
    public void A_state_outside_the_closed_set_fails()
        => Fails("enum.state", MapFixture.With("| docs/proc.md | exists |\n| P.1", "| docs/proc.md | maybe |\n| P.1"));

    [Fact]
    public void An_edge_kind_outside_the_closed_set_fails()
        => Fails("enum.edge-kind", MapFixture.With("| P.0 | P.1 | flow |", "| P.0 | P.1 | maybe |"));

    // ---- row minima ----

    [Fact]
    public void An_actor_outside_the_closed_set_fails()
        => Fails("row.actor", MapFixture.With("| script | f.a | f.b |", "| robot | f.a | f.b |"));

    [Fact]
    public void An_actor_prefix_with_no_model_after_the_colon_fails()
        => Fails("row.actor", MapFixture.With("| script | f.a | f.b |", "| agent: | f.a | f.b |"));

    [Fact]
    public void Any_model_name_after_the_prefix_is_accepted()
    {
        using var f = MapFixture.With("| script | f.a | f.b |", "| agent:model-varies | f.a | f.b |");
        Assert.DoesNotContain("row.actor", Rules(f));
    }

    [Fact]
    public void A_process_citing_no_root_fails()
        => Fails("row.roots-empty", MapFixture.With("| f.a | f.b | G1 |", "| f.a | f.b |  |"));

    [Fact]
    public void A_process_reading_nothing_fails_because_it_is_deriving_from_recall()
        => Fails("row.inputs-empty", MapFixture.With("| script | f.a | f.b |", "| script |  | f.b |"));

    [Fact]
    public void A_process_writing_nothing_fails_because_it_is_indistinguishable_from_not_running()
        => Fails("row.outputs-empty", MapFixture.With("| script | f.a | f.b |", "| script | f.a |  |"));

    [Fact]
    public void A_choice_edge_with_no_branch_condition_fails()
        => Fails("edge.choice-label",
            MapFixture.With("| P.1 | P.2 | choice | when there is something to propose |", "| P.1 | P.2 | choice |  |"));

    [Fact]
    public void A_flow_edge_may_carry_no_label()
    {
        using var f = new MapFixture();
        Assert.DoesNotContain("edge.choice-label", Rules(f));
    }

    // ---- governed-by is a file, never a section ----

    [Fact]
    public void A_governed_by_cell_addressing_a_section_fails()
        => Fails("governed-by.syntax", MapFixture.With("| G1 | docs/proc.md | exists |", "| G1 | docs/proc.md § What it does | exists |"));

    [Fact]
    public void A_governed_by_cell_with_a_parenthetical_fails()
        => Fails("governed-by.syntax", MapFixture.With("| G1 | docs/proc.md | exists |", "| G1 | docs/proc.md (record only) | exists |"));

    [Fact]
    public void A_bare_file_name_fails_because_there_is_no_search_order()
        => Fails("governed-by.syntax", MapFixture.With("| G1 | docs/proc.md | exists |", "| G1 | proc.md | exists |"));

    [Fact]
    public void A_governed_by_file_that_does_not_exist_fails()
        => Fails("governed-by.missing-file", MapFixture.With("| G1 | docs/proc.md | exists |", "| G1 | docs/nope.md | exists |"));

    [Fact]
    public void An_empty_governed_by_fails_because_precedence_needs_a_named_document()
        => Fails("governed-by.empty", MapFixture.With("| G1 | docs/proc.md | exists |", "| G1 |  | exists |"));

    // ---- Roots.source keeps the locus grammar ----

    [Fact]
    public void A_source_naming_a_heading_the_file_does_not_have_fails()
        => Fails("source.heading", MapFixture.With("docs/goal.md § Goal |", "docs/goal.md § Purpose |"));

    [Fact]
    public void A_source_item_beyond_the_list_length_fails()
        => Fails("source.item", MapFixture.With("docs/goal.md § Rules ¶ 2 |", "docs/goal.md § Rules ¶ 9 |"));

    [Fact]
    public void A_trailing_integer_is_a_syntax_error_and_is_never_read_as_an_item()
        => Fails("source.syntax", MapFixture.With("docs/goal.md § Rules ¶ 2 |", "docs/goal.md § Rules 2 |"));

    [Fact]
    public void A_source_may_list_several_places_separated_by_semicolons()
    {
        using var f = MapFixture.With("docs/goal.md § Goal |", "docs/goal.md § Goal; docs/proc.md § What it does |");
        Assert.DoesNotContain("source.syntax", Rules(f));
    }

    [Fact]
    public void A_root_no_process_cites_fails()
        => Fails("root.uncited", MapFixture.With("| C1 | docs/proc.md | exists |", "| G1 | docs/proc.md | exists |"));

    // ---- file traffic: three distinct kinds, no exemption ----

    [Fact]
    public void A_file_written_and_never_read_fails()
        => Fails("file.written-never-read", MapFixture.With("| P.0 | P | sop | Seed the cycle | hitl:fable | f.hyp | f.a |", "| P.0 | P | sop | Seed the cycle | hitl:fable | f.b | f.a |"));

    [Fact]
    public void A_file_read_and_never_written_fails()
        => Fails("file.read-never-written", MapFixture.With("| P.1 | P | sop | Transform a into b | script | f.a | f.b |", "| P.1 | P | sop | Transform a into b | script | f.a | f.cand |"));

    [Fact]
    public void A_file_no_process_touches_at_all_fails_as_its_own_kind()
        => Fails("file.uncited", MapFixture.With(
            "| f.hyp | docs/hyp.md | committed | docs/proc.md |",
            "| f.hyp | docs/hyp.md | committed | docs/proc.md |\n| f.orphan | docs/orphan.md | committed | docs/proc.md |"));

    // ---- the promotion gate ----

    [Fact]
    public void A_non_brian_row_reading_candidates_and_writing_hypotheses_is_ungated()
        => Fails("gate.ungated", MapFixture.With("| P.3 | M | sop | Promote | brian |", "| P.3 | M | sop | Promote | hitl:fable |"));

    [Fact]
    public void A_brian_row_on_the_path_before_the_write_gates_it()
    {
        using var f = new MapFixture();
        Assert.DoesNotContain("gate.ungated", Rules(f));
    }

    [Fact]
    public void A_check_with_no_subject_is_reported_as_vacuous_rather_than_passing()
    {
        using var f = new MapFixture();
        var report = Validator.Validate(f.RepoRoot, f.SkillFolder);
        Assert.Contains(report.Findings, x => x.Level == FindingLevel.Vacuous && x.RuleId == "gate.vacuous");
        Assert.True(report.Passed);
    }

    // ---- bootstrap ----

    [Fact]
    public void A_bootstrap_row_with_nothing_that_retires_it_fails()
        => Fails("bootstrap.unlisted", MapFixture.With("| P.2 | V | sop |", "| P.2 | V | bootstrap |"));

    [Fact]
    public void A_bootstrap_table_row_naming_no_process_fails()
        => Fails("bootstrap.unknown-row", MapFixture.With(
            "| row | retired by |\n|---|---|",
            "| row | retired by |\n|---|---|\n| P.99 | some commit |"));

    [Fact]
    public void A_listed_row_whose_kind_is_not_bootstrap_fails()
        => Fails("bootstrap.not-bootstrap", MapFixture.With(
            "| row | retired by |\n|---|---|",
            "| row | retired by |\n|---|---|\n| P.2 | some commit |"));

    // ---- SKILL.md's published limits ----

    [Fact]
    public void A_companion_named_by_no_skill_file_is_unreachable()
    {
        using var f = new MapFixture(skill: "---\nname: example\ndescription: d\n---\n\n# Example\n");
        Assert.Contains("skill.companion-unlinked", Rules(f));
    }

    [Fact]
    public void A_description_over_the_published_limit_fails()
    {
        var long_ = new string('x', Validator.SkillDescriptionBudget + 1);
        using var f = new MapFixture(skill: $"---\nname: example\ndescription: {long_}\n---\n\nprocess-map.md\n");
        Assert.Contains("skill.description-length", Rules(f));
    }

    [Fact]
    public void A_skill_over_the_published_line_budget_fails()
    {
        var padding = string.Join("\n", Enumerable.Repeat("filler", Validator.SkillLineBudget + 1));
        using var f = new MapFixture(skill: $"---\nname: example\ndescription: d\n---\n\nprocess-map.md\n{padding}\n");
        Assert.Contains("skill.line-budget", Rules(f));
    }

    // ---- the codebook's worked examples ----

    [Fact]
    public void A_worked_example_declaring_no_rules_fails()
    {
        using var f = new MapFixture();
        var codebook = System.IO.Path.Combine(f.RepoRoot, "fanout", "referee", "codebook.md");
        System.IO.File.WriteAllText(codebook,
            System.IO.File.ReadAllText(codebook).Replace("Exercises R1, R2.", "No rules named here."));
        Assert.Contains("codebook.example-exercises", Rules(f));
    }

    [Fact]
    public void A_worked_example_naming_a_rule_the_codebook_lacks_fails()
    {
        using var f = new MapFixture();
        var codebook = System.IO.Path.Combine(f.RepoRoot, "fanout", "referee", "codebook.md");
        System.IO.File.WriteAllText(codebook,
            System.IO.File.ReadAllText(codebook).Replace("Exercises R1, R2.", "Exercises R1, R7."));
        Assert.Contains("codebook.unknown-rule", Rules(f));
    }

    // ---- informational, never a verdict ----

    [Fact]
    public void A_schema_value_no_row_uses_is_reported_without_failing()
    {
        using var f = new MapFixture();
        var report = Validator.Validate(f.RepoRoot, f.SkillFolder);
        Assert.Contains(report.Findings,
            x => x.RuleId == "info.unused-enum-value" && x.Level == FindingLevel.Info);
        Assert.True(report.Passed);
    }
}
