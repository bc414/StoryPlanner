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
        // Processes are in router order, then table order: promoting's rows precede refereeing's.
        Assert.Equal(["promote", "referee-append"], traffic["candidates"].Writers);
        Assert.Equal(["promote", "referee-run", "referee-append"], traffic["candidates"].Readers);
        Assert.Empty(traffic["directions"].Writers);
    }

    [Fact]
    public void An_artifact_named_as_an_instrument_counts_as_read()
    {
        using var f = MapFixture.With(MapFixture.RefereeingFile,
            "| referee-judge | agent | | directions items |", "| referee-judge | agent | items | directions |");
        var items = GraphRules.Traffic(f.Doc).Single(t => t.ArtifactId == "items");
        Assert.Equal(["referee-judge"], items.InstrumentOf);
        Assert.True(items.IsRead);
        Assert.Contains(("referee-run", "referee-judge", "items"), GraphRules.DataEdges(f.Doc));
    }

    [Fact]
    public void A_data_edge_exists_where_one_row_writes_what_another_reads()
    {
        using var f = new MapFixture();
        var edges = GraphRules.DataEdges(f.Doc);
        Assert.Contains(("referee-run", "referee-judge", "items"), edges);
        Assert.Contains(("referee-append", "promote", "candidates"), edges);
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
        Assert.Equal(["refereeing-a-candidate"], GraphRules.EnabledBy(f.Doc, "promoting-checked-candidates"));
        Assert.Empty(GraphRules.EnabledBy(f.Doc, "refereeing-a-candidate"));
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
            "| changing-the-planner-for-v3 | |", "| changing-the-planner-for-v3 | refereeing-a-candidate |");
        var cycle = Assert.Single(GraphRules.EnablesCycles(f.Doc));
        Assert.Equal(3, cycle.Count);
        Assert.Contains("promoting-checked-candidates", cycle);
    }

    [Fact]
    public void An_enables_edge_is_backed_by_the_artifacts_that_flow_along_it()
    {
        using var f = new MapFixture();
        Assert.Equal(["candidates"], GraphRules.Backing(f.Doc, "refereeing-a-candidate", "promoting-checked-candidates"));
        Assert.Empty(GraphRules.Backing(f.Doc, "refereeing-a-candidate", "changing-the-planner-for-v3"));
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
        // Every reader of candidates reaches the write; promote itself is a path of one.
        Assert.Equal(3, paths.Count);
        Assert.Contains(paths, p => p.Nodes.SequenceEqual(["promote"]));
        Assert.All(paths, p => Assert.Equal("promote", p.Nodes[^1]));
    }

    [Fact]
    public void An_hitl_process_only_after_the_write_does_not_gate_it()
    {
        // A review that reads what promote wrote is detection, not prevention.
        using var f = MapFixture.With(MapFixture.PromotingFile,
            "| promote | hitl | git | candidates hypothesis-record hypothesis-status question-list verification-artifact | hypothesis-record hypothesis-status candidates question-list verification-artifact | specified | Brian decides each candidate |",
            "| promote | session | git | candidates hypothesis-record hypothesis-status question-list verification-artifact | hypothesis-record hypothesis-status candidates question-list verification-artifact | specified | The session decides |\n" +
            "| review | hitl | git | hypothesis-record | question-list | specified | Brian reviews the diff afterwards |");
        Assert.NotEmpty(GraphRules.UngatedPaths(f.Doc, "candidates", WellKnown.HypothesisArtifacts));
    }

    [Fact]
    public void A_cycle_in_the_data_graph_does_not_hang_the_search()
    {
        // referee-append writes candidates, which referee-run reads: a cycle by construction.
        using var f = MapFixture.With(MapFixture.PromotingFile, "| promote | hitl |", "| promote | agent |");
        Assert.NotEmpty(GraphRules.UngatedPaths(f.Doc, "candidates", WellKnown.HypothesisArtifacts));
    }
}
