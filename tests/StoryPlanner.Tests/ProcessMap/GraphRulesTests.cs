using System.Linq;
using StoryPlanner.ProcessMap;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// What the map derives rather than states. Consumers are never authored, so this is the only
/// place they exist — a hand-kept consumers column is the stale mirror the map exists to stop
/// repeating.
/// </summary>
public class GraphRulesTests
{
    static ProcessMapDocument Doc(string? find = null, string? replace = null)
        => MapReader.Read(find is null
            ? MapFixture.ValidMap
            : MapFixture.ValidMap.Replace(find, replace!));

    [Fact]
    public void Consumers_are_derived_from_inputs_and_producers_from_outputs()
    {
        var traffic = GraphRules.Traffic(Doc()).ToDictionary(t => t.FileId);
        Assert.Equal(["P.1"], traffic["f.b"].Producers);
        Assert.Equal(["P.2"], traffic["f.b"].Consumers);
    }

    [Fact]
    public void A_file_no_process_reads_has_an_empty_consumer_list_rather_than_being_omitted()
    {
        var doc = Doc(
            "| f.hyp | docs/hyp.md | committed | docs/proc.md |",
            "| f.hyp | docs/hyp.md | committed | docs/proc.md |\n| f.orphan | docs/orphan.md | committed | docs/proc.md |");
        var orphan = GraphRules.Traffic(doc).Single(t => t.FileId == "f.orphan");
        Assert.Empty(orphan.Producers);
        Assert.Empty(orphan.Consumers);
    }

    [Fact]
    public void A_data_edge_exists_where_one_row_writes_what_another_reads()
    {
        var edges = GraphRules.DataEdges(Doc());
        Assert.Contains(("P.1", "P.2", "f.b"), edges);
        Assert.DoesNotContain(edges, e => e.From == e.To);
    }

    [Fact]
    public void The_union_graph_carries_both_control_and_data_edges()
    {
        // P.3 writes f.hyp which P.0 reads; no control edge says so.
        var graph = GraphRules.UnionGraph(Doc());
        Assert.Contains("P.0", graph["P.3"]);
        Assert.DoesNotContain(Doc().Edges, e => e.From == "P.3" && e.To == "P.0");
    }

    [Fact]
    public void A_brian_actor_before_the_write_gates_the_path()
        => Assert.Empty(GraphRules.UngatedPaths(Doc(), "f.cand", "f.hyp"));

    [Fact]
    public void A_path_with_no_brian_actor_before_the_write_is_reported_with_its_route()
    {
        var doc = Doc("| P.3 | M | sop | Promote | brian |", "| P.3 | M | sop | Promote | hitl:fable |");
        var path = Assert.Single(GraphRules.UngatedPaths(doc, "f.cand", "f.hyp"));
        Assert.Equal(["P.3"], path.Nodes);
    }

    [Fact]
    public void A_brian_row_only_after_the_write_does_not_gate_it()
    {
        // P.3 writes f.hyp itself; a Brian review downstream is detection, not prevention.
        var doc = Doc(
            "| P.3 | M | sop | Promote | brian | f.cand | f.hyp | C1 | docs/proc.md | exists |",
            "| P.3 | M | sop | Promote | hitl:fable | f.cand | f.hyp | C1 | docs/proc.md | exists |\n" +
            "| P.4 | M | sop | Review the diff | brian | f.hyp | f.hyp | C1 | docs/proc.md | exists |");
        Assert.NotEmpty(GraphRules.UngatedPaths(doc, "f.cand", "f.hyp"));
    }

    [Fact]
    public void A_cycle_in_the_graph_does_not_hang_the_search()
    {
        // The fixture's P.0 → P.1 → P.2 → P.3 → (f.hyp) → P.0 is a cycle by construction.
        var doc = Doc("| P.3 | M | sop | Promote | brian |", "| P.3 | M | sop | Promote | script |");
        Assert.NotEmpty(GraphRules.UngatedPaths(doc, "f.cand", "f.hyp"));
    }

    [Fact]
    public void Fan_in_counts_the_rows_sharing_one_governing_document()
    {
        var fanIn = GraphRules.GovernorFanIn(Doc());
        var (file, rows) = Assert.Single(fanIn);
        Assert.Equal("docs/proc.md", file);
        Assert.Equal(4, rows.Count);
    }
}
