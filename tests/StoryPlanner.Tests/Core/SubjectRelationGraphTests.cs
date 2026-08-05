using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Walking authored subject-to-subject edges. Shared by the app's tree view and the MCP server's
/// get_subject_tree, so a drift between them shows up here first. Pure — no .storyplan, no
/// DbContext.
///
/// The cycle cases are the reason this type exists rather than a recursive lambda at each call
/// site: the schema has no constraints, a non-hierarchy relation may legitimately loop, and rows
/// can arrive from DataOps or hand-written SQL that never passed the picker's guard. A walk that
/// hangs on merely-unusual data would take the UI thread with it.
/// </summary>
public class SubjectRelationGraphTests
{
    private const int Ancestor = 1;
    private const int Rival = 2;

    /// <summary>Edge in the stored direction: subject → target.</summary>
    private static SubjectRelation Edge(int id, int subjectId, int targetId, int definitionId = Ancestor, int sortOrder = 0) =>
        new() { Id = id, RelationDefinitionId = definitionId, SubjectId = subjectId, TargetSubjectId = targetId, SortOrder = sortOrder };

    /// <summary>The real Aquileia line, youngest first: 250 → 226 → 54 → 225 → 103.</summary>
    private static List<SubjectRelation> Aquileia() =>
    [
        Edge(1, 250, 226),
        Edge(2, 226, 54),
        Edge(3, 54, 225),
        Edge(4, 225, 103),
    ];

    // ── Direction ───────────────────────────────────────────────────────────────

    [Fact]
    public void Forward_from_the_youngest_walks_up_the_whole_line()
    {
        var walk = SubjectRelationGraph.Walk(Aquileia(), Ancestor, 250, inverted: false);

        Assert.Equal([250, 226, 54, 225, 103], walk.Select(n => n.SubjectId));
        Assert.Equal([0, 1, 2, 3, 4], walk.Select(n => n.Depth));
        Assert.All(walk, n => Assert.False(n.StopsOnCycle));
    }

    [Fact]
    public void Inverted_from_the_root_renders_the_descendant_tree()
    {
        var walk = SubjectRelationGraph.Walk(Aquileia(), Ancestor, 103, inverted: true);

        Assert.Equal([103, 225, 54, 226, 250], walk.Select(n => n.SubjectId));
        Assert.Equal([0, 1, 2, 3, 4], walk.Select(n => n.Depth));
    }

    [Fact]
    public void The_root_is_always_present_even_with_no_edges_at_all()
    {
        var walk = SubjectRelationGraph.Walk([], Ancestor, 220, inverted: false);

        var node = Assert.Single(walk);
        Assert.Equal(220, node.SubjectId);
        Assert.Equal(0, node.Depth);
    }

    [Fact]
    public void A_walk_ignores_edges_belonging_to_another_relation()
    {
        List<SubjectRelation> relations = [Edge(1, 250, 226), Edge(2, 250, 99, definitionId: Rival)];

        Assert.Equal([250, 226], SubjectRelationGraph.Walk(relations, Ancestor, 250, inverted: false)
            .Select(n => n.SubjectId));
    }

    // ── Branching: one ancestor, several successors ─────────────────────────────

    [Fact]
    public void A_subject_may_have_several_children_and_all_of_them_render()
    {
        // The real shape from note:1630 — Grover III (123) is ancestor to both the chronological
        // successor Grover IV (124) and, ideologically, the Griffonian Republic (53).
        List<SubjectRelation> relations = [Edge(1, 124, 123), Edge(2, 53, 123, sortOrder: 1)];

        var walk = SubjectRelationGraph.Walk(relations, Ancestor, 123, inverted: true);

        Assert.Equal([123, 124, 53], walk.Select(n => n.SubjectId));
        Assert.Equal([0, 1, 1], walk.Select(n => n.Depth));
    }

    [Fact]
    public void Children_reports_only_the_direct_inverse_edge()
    {
        Assert.Equal([54], SubjectRelationGraph.Children(Aquileia(), Ancestor, 225));
        Assert.Empty(SubjectRelationGraph.Children(Aquileia(), Ancestor, 250));
    }

    [Fact]
    public void A_diamond_renders_under_both_parents_rather_than_being_deduplicated()
    {
        // 4 descends from both 2 and 3, which both descend from 1. Not a cycle — 4 legitimately
        // appears twice, once under each parent.
        List<SubjectRelation> relations = [Edge(1, 2, 1), Edge(2, 3, 1), Edge(3, 4, 2), Edge(4, 4, 3)];

        var walk = SubjectRelationGraph.Walk(relations, Ancestor, 1, inverted: true);

        Assert.Equal(2, walk.Count(n => n.SubjectId == 4));
        Assert.All(walk, n => Assert.False(n.StopsOnCycle));
    }

    // ── Cycles ──────────────────────────────────────────────────────────────────

    [Fact]
    public void A_self_reference_terminates_and_is_marked()
    {
        var walk = SubjectRelationGraph.Walk([Edge(1, 7, 7)], Ancestor, 7, inverted: false);

        Assert.Equal([7, 7], walk.Select(n => n.SubjectId));
        Assert.False(walk[0].StopsOnCycle);
        Assert.True(walk[1].StopsOnCycle);
    }

    [Fact]
    public void A_two_node_cycle_terminates_and_is_marked()
    {
        List<SubjectRelation> relations = [Edge(1, 1, 2), Edge(2, 2, 1)];

        var walk = SubjectRelationGraph.Walk(relations, Ancestor, 1, inverted: false);

        Assert.Equal([1, 2, 1], walk.Select(n => n.SubjectId));
        Assert.True(walk[^1].StopsOnCycle);
    }

    [Fact]
    public void A_three_node_cycle_terminates_and_is_marked()
    {
        List<SubjectRelation> relations = [Edge(1, 1, 2), Edge(2, 2, 3), Edge(3, 3, 1)];

        var walk = SubjectRelationGraph.Walk(relations, Ancestor, 1, inverted: false);

        Assert.Equal([1, 2, 3, 1], walk.Select(n => n.SubjectId));
        Assert.True(walk[^1].StopsOnCycle);
    }

    [Fact]
    public void Chain_stops_on_a_cycle_instead_of_looping()
    {
        List<SubjectRelation> relations = [Edge(1, 1, 2), Edge(2, 2, 3), Edge(3, 3, 1)];

        // Finite, and each subject appears once — the ordering is root-first over what was reached.
        Assert.Equal([3, 2, 1], SubjectRelationGraph.Chain(relations, Ancestor, 1));
    }

    [Fact]
    public void SubjectsOnCycles_names_every_member_of_the_loop_and_nothing_upstream()
    {
        // 4 → 1 → 2 → 3 → 1: the loop is {1,2,3}; 4 merely feeds into it.
        List<SubjectRelation> relations = [Edge(1, 1, 2), Edge(2, 2, 3), Edge(3, 3, 1), Edge(4, 4, 1)];

        Assert.Equal([1, 2, 3], SubjectRelationGraph.SubjectsOnCycles(relations, Ancestor).Order());
    }

    [Fact]
    public void SubjectsOnCycles_is_empty_for_an_acyclic_line()
    {
        Assert.Empty(SubjectRelationGraph.SubjectsOnCycles(Aquileia(), Ancestor));
    }

    // ── Chain, descendants, roots, and the picker's guard ───────────────────────

    [Fact]
    public void Chain_reads_root_first()
    {
        Assert.Equal([103, 225, 54, 226, 250], SubjectRelationGraph.Chain(Aquileia(), Ancestor, 250));
        Assert.Equal([103], SubjectRelationGraph.Chain(Aquileia(), Ancestor, 103));
    }

    [Fact]
    public void Descendants_are_transitive_and_exclude_the_subject_itself()
    {
        var descendants = SubjectRelationGraph.Descendants(Aquileia(), Ancestor, 103);

        Assert.Equal([54, 225, 226, 250], descendants.Order());
        Assert.DoesNotContain(103, descendants);
    }

    [Fact]
    public void Roots_are_the_subjects_with_no_outgoing_edge()
    {
        int[] all = [103, 225, 54, 226, 250, 220];

        // 220 has no edges at all and is a root — a one-node tree, not an omission.
        Assert.Equal([103, 220], SubjectRelationGraph.Roots(Aquileia(), Ancestor, all).Order());
    }

    [Fact]
    public void WouldCreateCycle_catches_self_reference_and_any_descendant()
    {
        var relations = Aquileia();

        Assert.True(SubjectRelationGraph.WouldCreateCycle(relations, Ancestor, 103, 103));
        Assert.True(SubjectRelationGraph.WouldCreateCycle(relations, Ancestor, 103, 250));
        Assert.True(SubjectRelationGraph.WouldCreateCycle(relations, Ancestor, 225, 54));
    }

    [Fact]
    public void WouldCreateCycle_permits_an_unrelated_target()
    {
        // Feudal Aquileia (103) is the top of the line; giving it an ancestor outside the line is
        // legal, and so is a second subject joining the line under an existing member.
        Assert.False(SubjectRelationGraph.WouldCreateCycle(Aquileia(), Ancestor, 103, 97));
        Assert.False(SubjectRelationGraph.WouldCreateCycle(Aquileia(), Ancestor, 97, 225));
    }
}
