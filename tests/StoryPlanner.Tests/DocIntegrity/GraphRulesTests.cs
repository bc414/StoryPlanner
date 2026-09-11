using System.Linq;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// What the tables derive rather than state. Consumers are never authored, so this is the only
/// place they exist — a hand-kept consumers column is the stale mirror the schema exists to
/// stop repeating.
/// </summary>
public class GraphRulesTests
{
    [Fact]
    public void Writers_and_readers_are_derived_from_writes_and_reads()
    {
        using var f = new MapFixture();
        var traffic = GraphRules.Traffic(f.Doc).ToDictionary(t => t.ArtifactId);
        // candidates is a generated view: compose-candidates writes it, promote reads it.
        Assert.Equal(["compose-candidates"], traffic["candidates"].Writers);
        Assert.Equal(["promote"], traffic["candidates"].Readers);
        Assert.Empty(traffic["directions"].Writers);
    }

    [Fact]
    public void An_artifact_named_as_an_instrument_counts_as_read()
    {
        using var f = MapFixture.With(MapFixture.SurfacingFile,
            "| assess-referee-items | agent | | directions items |", "| assess-referee-items | agent | items | directions |");
        var items = GraphRules.Traffic(f.Doc).Single(t => t.ArtifactId == "items");
        Assert.Equal(["assess-referee-items"], items.InstrumentOf);
        Assert.True(items.IsRead);
        Assert.Contains(("assemble-referee-batch", "assess-referee-items", "items"), GraphRules.DataEdges(f.Doc));
    }

    [Fact]
    public void A_data_edge_exists_where_one_row_writes_what_another_reads()
    {
        using var f = new MapFixture();
        var edges = GraphRules.DataEdges(f.Doc);
        Assert.Contains(("assemble-referee-batch", "assess-referee-items", "items"), edges);
        Assert.Contains(("compose-candidates", "promote", "candidates"), edges);
        Assert.DoesNotContain(edges, e => e.From == e.To);
    }

    [Fact]
    public void The_terminus_is_the_activity_that_enables_nothing()
    {
        using var f = new MapFixture();
        Assert.Equal(["changing-the-planner-for-v3"], GraphRules.Termini(f.Doc));
    }

    [Fact]
    public void Enabled_by_is_the_reverse_of_the_router_column()
    {
        using var f = new MapFixture();
        Assert.Equal(["surfacing-candidates"], GraphRules.EnabledBy(f.Doc, "promoting-refereed-candidates"));
        Assert.Empty(GraphRules.EnabledBy(f.Doc, "surfacing-candidates"));
    }

    [Fact]
    public void The_reference_enables_graph_has_no_cycle()
    {
        using var f = new MapFixture();
        Assert.Empty(GraphRules.EnablesCycles(f.Doc));
    }

    [Fact]
    public void A_cycle_is_reported_once_with_its_members()
    {
        using var f = MapFixture.With(MapFixture.SkillFile,
            "| changing-the-planner-for-v3 | |", "| changing-the-planner-for-v3 | surfacing-candidates |");
        var cycle = Assert.Single(GraphRules.EnablesCycles(f.Doc));
        Assert.Equal(3, cycle.Count);
        Assert.Contains("promoting-refereed-candidates", cycle);
    }

    [Fact]
    public void An_enables_edge_is_backed_by_the_artifacts_that_flow_along_it()
    {
        using var f = new MapFixture();
        Assert.Equal(["candidates"], GraphRules.Backing(f.Doc, "surfacing-candidates", "promoting-refereed-candidates"));
        Assert.Empty(GraphRules.Backing(f.Doc, "surfacing-candidates", "changing-the-planner-for-v3"));
    }

    [Fact]
    public void An_hitl_writer_gates_the_path_from_candidates()
    {
        using var f = new MapFixture();
        Assert.Empty(GraphRules.UngatedPaths(f.Doc, "candidates", WellKnown.HypothesisArtifacts));
    }

    [Fact]
    public void A_session_writer_reached_from_candidates_is_reported_with_its_route()
    {
        using var f = MapFixture.With(MapFixture.PromotingFile, "| promote | hitl |", "| promote | session |");
        var paths = GraphRules.UngatedPaths(f.Doc, "candidates", WellKnown.HypothesisArtifacts);
        // promote is the sole reader of candidates and the sole hypothesis writer: one path of one.
        Assert.Equal(1, paths.Count);
        Assert.Contains(paths, p => p.Nodes.SequenceEqual(["promote"]));
        Assert.All(paths, p => Assert.Equal("promote", p.Nodes[^1]));
    }

    [Fact]
    public void An_hitl_process_only_after_the_write_does_not_gate_it()
    {
        // A review that reads what promote wrote is detection, not prevention.
        using var f = MapFixture.With(MapFixture.PromotingFile,
            "| promote | hitl | git | candidates findings hypothesis-record hypothesis-status question-list | hypothesis-record hypothesis-status question-list | specified | Brian decides each candidate |",
            "| promote | session | git | candidates findings hypothesis-record hypothesis-status question-list | hypothesis-record hypothesis-status question-list | specified | The session decides |\n" +
            "| review | hitl | git | hypothesis-record | question-list | specified | Brian reviews the diff afterwards |");
        Assert.NotEmpty(GraphRules.UngatedPaths(f.Doc, "candidates", WellKnown.HypothesisArtifacts));
    }

    [Fact]
    public void A_cycle_in_the_data_graph_does_not_hang_the_search()
    {
        // promote reads and writes hypothesis-record: a self-cycle the search must not hang on.
        using var f = MapFixture.With(MapFixture.PromotingFile, "| promote | hitl |", "| promote | agent |");
        Assert.NotEmpty(GraphRules.UngatedPaths(f.Doc, "candidates", WellKnown.HypothesisArtifacts));
    }
}
