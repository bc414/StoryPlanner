using System.Collections.Generic;
using StoryPlanner.ProcessMap;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Rendering and the marker contract. The generated sections are never hand-edited, so the
/// things worth pinning are that a re-render changes nothing, that each section carries what
/// the tables derive, and that everything outside a marker pair is copied through untouched.
/// </summary>
public class RenderTests
{
    [Fact]
    public void Level_one_draws_every_activity_and_its_enables_edges()
    {
        using var f = new MapFixture();
        var level1 = MermaidRenderer.Level1(f.Doc, forced: false);
        Assert.Contains("changingtheplannerforv3[[\"changing-the-planner-for-v3\"]]:::terminus", level1);
        Assert.Contains("refereeingacandidate[\"refereeing-a-candidate\"]:::activity", level1);
        Assert.Contains("refereeingacandidate --> promotingcheckedcandidates", level1);
        Assert.Contains("promotingcheckedcandidates --> changingtheplannerforv3", level1);
    }

    [Fact]
    public void An_activity_section_draws_its_processes_by_mode_and_the_artifacts_they_touch()
    {
        using var f = new MapFixture();
        var section = MermaidRenderer.Activity(f.Doc, "refereeing-a-candidate", forced: false);
        Assert.Contains("refereerun[\"referee-run<br/>session\"]:::session", section);
        Assert.Contains("refereejudge([\"referee-judge<br/>agent\"]):::agent", section);
        Assert.Contains("candidates[/\"candidates\"/]:::artifact", section);
        Assert.Contains("codebook --> refereerun", section);
        Assert.Contains("refereeappend --> candidates", section);
        Assert.DoesNotContain("promote", section);
    }

    [Fact]
    public void An_hitl_process_is_a_hexagon()
    {
        using var f = new MapFixture();
        Assert.Contains("promote{{\"promote<br/>hitl\"}}:::hitl", MermaidRenderer.Activity(f.Doc, "promoting-checked-candidates", forced: false));
    }

    [Fact]
    public void An_instrument_read_is_a_dashed_edge()
    {
        using var f = MapFixture.With(MapFixture.RefereeingFile,
            "| referee-judge | agent | | codebook items |", "| referee-judge | agent | items | codebook |");
        Assert.Contains("items -.-> refereejudge", MermaidRenderer.Activity(f.Doc, "refereeing-a-candidate", forced: false));
    }

    [Fact]
    public void An_activity_section_carries_what_the_tables_derive_for_it()
    {
        using var f = new MapFixture();
        var section = MermaidRenderer.Activity(f.Doc, "refereeing-a-candidate", forced: false);
        Assert.Contains("- **inputs**: calibration-record codebook instances", section);
        Assert.Contains("- **outputs**: candidates items results", section);
        Assert.Contains("- **instruments**: runner", section);
        Assert.Contains("- **enabled by**: —", section);
        Assert.Contains("- **enables**: promoting-checked-candidates", section);
        Assert.Contains("- **enabled by**: refereeing-a-candidate",
            MermaidRenderer.Activity(f.Doc, "promoting-checked-candidates", forced: false));
    }

    [Fact]
    public void The_map_holds_the_whole_graph_the_consumers_and_the_verdict()
    {
        using var f = new MapFixture();
        var map = MermaidRenderer.Map(f.Doc, f.Report, forced: false);
        Assert.Contains("subgraph refereeingacandidate[\"refereeing-a-candidate\"]", map);
        Assert.DoesNotContain("subgraph changingtheplannerforv3", map);
        Assert.Contains("| candidates | promote referee-append | promote referee-run referee-append | — |", map);
        Assert.Contains("Last run: **passed**", map);
        Assert.Contains("<!-- generated:graph -->", map);
        Assert.Contains("<!-- generated:consumers -->", map);
        Assert.Contains("<!-- generated:validation -->", map);
    }

    [Fact]
    public void The_same_rows_render_byte_identical_text()
    {
        using var f = new MapFixture();
        var doc = f.Doc;
        Assert.Equal(MermaidRenderer.Level1(doc, false), MermaidRenderer.Level1(doc, false));
        Assert.Equal(MermaidRenderer.Activity(doc, "promoting-checked-candidates", false),
            MermaidRenderer.Activity(doc, "promoting-checked-candidates", false));
        Assert.Equal(MermaidRenderer.Map(doc, f.Report, false), MermaidRenderer.Map(doc, f.Report, false));
    }

    [Fact]
    public void Forcing_stamps_every_section_unvalidated()
    {
        using var f = new MapFixture();
        var doc = f.Doc;
        Assert.Contains("UNVALIDATED", MermaidRenderer.Level1(doc, forced: true));
        Assert.Contains("UNVALIDATED", MermaidRenderer.Activity(doc, "refereeing-a-candidate", forced: true));
        Assert.Contains("UNVALIDATED", MermaidRenderer.Map(doc, f.Report, forced: true));
    }

    [Fact]
    public void Mermaid_node_ids_drop_the_hyphens_the_row_ids_carry()
        => Assert.Equal("refereerun", MermaidRenderer.NodeId("referee-run"));

    [Fact]
    public void Ids_that_merge_once_hyphens_are_dropped_are_refused_before_drawing()
    {
        using var f = MapFixture.With(MapFixture.ArtifactsFile,
            "| results | fanout/<instance>/<run>/results/ | frozen | | The agents' outputs |",
            "| results | fanout/<instance>/<run>/results/ | frozen | | The agents' outputs |\n| refereerun | docs/x.md | frozen | | Collides with referee-run |");
        Assert.Throws<MapFormatException>(() => MermaidRenderer.CheckNodeIds(f.Doc));
    }

    // ---- the marker contract ----

    static readonly Dictionary<string, string> Level1Body = new() { [MermaidRenderer.Level1Section] = "body of level-1\n" };

    [Fact]
    public void Writing_replaces_only_what_lies_between_the_markers()
    {
        var updated = MarkerWriter.Write(MapFixture.Skill, Level1Body);
        Assert.Contains("body of level-1", updated);
        Assert.Contains("| changing-the-planner-for-v3 | | The terminus", updated);
        Assert.Equal(
            MapFixture.Skill.Split("<!-- generated:level-1 -->")[0],
            updated.Split("<!-- generated:level-1 -->")[0]);
        Assert.Equal(
            MapFixture.Skill.Split("<!-- /generated -->")[1],
            updated.Split("<!-- /generated -->")[1]);
    }

    [Fact]
    public void A_second_write_over_the_first_is_idempotent()
    {
        var once = MarkerWriter.Write(MapFixture.Skill, Level1Body);
        Assert.Equal(once, MarkerWriter.Write(once, Level1Body));
    }

    [Fact]
    public void A_section_with_no_marker_pair_is_refused_rather_than_appended()
        => Assert.Throws<MapFormatException>(() =>
            MarkerWriter.Write(MapFixture.Skill, new Dictionary<string, string> { ["nowhere"] = "x" }));

    [Fact]
    public void An_unclosed_marker_is_refused()
        => Assert.Throws<MapFormatException>(() => MarkerWriter.Write(
            "<!-- generated:level-1 -->\n", Level1Body));
}
