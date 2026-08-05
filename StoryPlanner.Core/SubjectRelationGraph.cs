namespace StoryPlanner.Core;

/// <summary>
/// Walks <see cref="SubjectRelation"/> edges of ONE relation definition. Retrieval only: it
/// reports which authored edges exist and where they lead, never how important a node is, how
/// long a chain "should" be, or which edge a subject ought to have.
///
/// Shared by the WPF layer and the MCP server so the two can never disagree about what a chain
/// is — the same role <see cref="WorldDateRange"/> plays for dates. Everything here takes plain
/// collections, so it is Pure-tier testable with no .storyplan.
///
/// <para><b>Every walk is cycle-safe, and that is not defensive coding.</b> The schema has no
/// constraints, a non-hierarchy relation may legitimately be cyclic (a symmetric "Rival of"),
/// and rows can arrive from DataOps or hand-written SQL that never passed the picker's guard. A
/// naive recursion would hang the UI thread on data that is merely unusual, so revisits stop the
/// walk and are reported rather than hidden.</para>
/// </summary>
public static class SubjectRelationGraph
{
    /// <summary>One node of a walk: the subject, its depth from the root, and whether the walk
    /// stopped here because this subject was already visited on this walk.</summary>
    public readonly record struct Node(int SubjectId, int Depth, bool StopsOnCycle);

    /// <summary>
    /// Depth-first expansion from <paramref name="rootSubjectId"/>, in render order.
    ///
    /// <paramref name="inverted"/> false follows the stored direction (SubjectId → TargetSubjectId):
    /// with an "Ancestor" edge that walks UP a lineage, and on a single-valued relation it
    /// degenerates to a chain. True follows edges backwards (TargetSubjectId → SubjectId): the same
    /// "Ancestor" edge then renders the whole DESCENDANT tree, which is what
    /// <see cref="SubjectRelationDefinition.InverseName"/> labels.
    ///
    /// The root is always the first node, at depth 0, even when it has no edges at all.
    /// </summary>
    public static IReadOnlyList<Node> Walk(
        IEnumerable<SubjectRelation> relations,
        int relationDefinitionId,
        int rootSubjectId,
        bool inverted)
    {
        var edges = EdgesByOrigin(relations, relationDefinitionId, inverted);
        var result = new List<Node>();
        // The path, not a global seen-set: a diamond (two branches reconverging on one subject)
        // is legal and should render under both parents. Only a subject repeating on its OWN
        // ancestry line is a cycle.
        var onPath = new HashSet<int>();
        Visit(rootSubjectId, 0);
        return result;

        void Visit(int subjectId, int depth)
        {
            if (!onPath.Add(subjectId))
            {
                result.Add(new Node(subjectId, depth, StopsOnCycle: true));
                return;
            }

            result.Add(new Node(subjectId, depth, StopsOnCycle: false));

            if (edges.TryGetValue(subjectId, out var next))
                foreach (var target in next)
                    Visit(target, depth + 1);

            onPath.Remove(subjectId);
        }
    }

    /// <summary>
    /// The line from <paramref name="subjectId"/> up to its root, ROOT FIRST, following the stored
    /// direction. Only meaningful on a single-valued relation — with several outgoing edges the
    /// first by SortOrder is taken, because "the chain" has no other definition. Stops on a cycle,
    /// so the result is always finite and always starts with the highest ancestor reached.
    /// </summary>
    public static IReadOnlyList<int> Chain(
        IEnumerable<SubjectRelation> relations,
        int relationDefinitionId,
        int subjectId)
    {
        var edges = EdgesByOrigin(relations, relationDefinitionId, inverted: false);
        var seen = new HashSet<int>();
        var upward = new List<int>();

        var current = subjectId;
        while (seen.Add(current))
        {
            upward.Add(current);
            if (!edges.TryGetValue(current, out var next) || next.Count == 0) break;
            current = next[0];
        }

        upward.Reverse();
        return upward;
    }

    /// <summary>Subjects pointing AT this one on this relation — the inverse edge, one level.</summary>
    public static IReadOnlyList<int> Children(
        IEnumerable<SubjectRelation> relations,
        int relationDefinitionId,
        int subjectId) =>
        relations
            .Where(r => r.RelationDefinitionId == relationDefinitionId && r.TargetSubjectId == subjectId)
            .OrderBy(r => r.SortOrder).ThenBy(r => r.Id)
            .Select(r => r.SubjectId)
            .Distinct()
            .ToList();

    /// <summary>
    /// Every subject beneath this one, transitively, excluding the subject itself. This is what the
    /// relation picker subtracts from its candidate list on a hierarchy relation, so a cycle can
    /// never be authored in the first place.
    /// </summary>
    public static IReadOnlySet<int> Descendants(
        IEnumerable<SubjectRelation> relations,
        int relationDefinitionId,
        int subjectId)
    {
        var found = new HashSet<int>();
        foreach (var node in Walk(relations, relationDefinitionId, subjectId, inverted: true))
            if (node.SubjectId != subjectId)
                found.Add(node.SubjectId);
        return found;
    }

    /// <summary>
    /// Subjects of this type holding no outgoing edge on this relation — the tops of the lines.
    /// A subject with no edges at all is a root, which is correct: it is a one-node tree, not an
    /// omission. <paramref name="subjectIds"/> is the caller's already-scoped candidate set.
    /// </summary>
    public static IReadOnlyList<int> Roots(
        IEnumerable<SubjectRelation> relations,
        int relationDefinitionId,
        IEnumerable<int> subjectIds)
    {
        var hasOutgoing = relations
            .Where(r => r.RelationDefinitionId == relationDefinitionId)
            .Select(r => r.SubjectId)
            .ToHashSet();

        return subjectIds.Where(id => !hasOutgoing.Contains(id)).ToList();
    }

    /// <summary>
    /// Would pointing <paramref name="subjectId"/> at <paramref name="candidateTargetId"/> close a
    /// loop? True for self-reference, and true when the candidate already sits beneath the subject.
    /// The picker's guard; <c>PlanIntegrity</c> is the after-the-fact auditor for rows that never
    /// passed through it.
    /// </summary>
    public static bool WouldCreateCycle(
        IEnumerable<SubjectRelation> relations,
        int relationDefinitionId,
        int subjectId,
        int candidateTargetId) =>
        subjectId == candidateTargetId
        || Descendants(relations, relationDefinitionId, subjectId).Contains(candidateTargetId);

    /// <summary>
    /// Every subject that sits on a cycle of this relation. Used by PlanIntegrity; reported per
    /// subject rather than per loop so the violation names something Brian can open and fix.
    /// </summary>
    public static IReadOnlySet<int> SubjectsOnCycles(
        IEnumerable<SubjectRelation> relations,
        int relationDefinitionId)
    {
        var edges = EdgesByOrigin(relations, relationDefinitionId, inverted: false);

        // Depth-first with an explicit path, following EVERY outgoing edge rather than the first:
        // a hierarchy relation is usually single-valued, but IsSingle is a separate flag and a
        // multi-parent hierarchy can hide a loop down its second edge.
        var onCycle = new HashSet<int>();
        var finished = new HashSet<int>();
        var path = new List<int>();
        var onPath = new HashSet<int>();

        foreach (var start in edges.Keys)
            Visit(start);

        return onCycle;

        void Visit(int node)
        {
            if (onPath.Contains(node))
            {
                // A back edge. Everything from where this node sits on the current path up to the
                // top of it forms the loop; nodes below that merely feed into it and are clean.
                for (var i = path.LastIndexOf(node); i < path.Count; i++)
                    onCycle.Add(path[i]);
                return;
            }

            // Already fully explored: any loop through it was found during its own visit.
            if (!finished.Add(node)) return;

            path.Add(node);
            onPath.Add(node);

            if (edges.TryGetValue(node, out var next))
                foreach (var target in next)
                    Visit(target);

            onPath.Remove(node);
            path.RemoveAt(path.Count - 1);
        }
    }

    private static Dictionary<int, List<int>> EdgesByOrigin(
        IEnumerable<SubjectRelation> relations,
        int relationDefinitionId,
        bool inverted) =>
        relations
            .Where(r => r.RelationDefinitionId == relationDefinitionId)
            .OrderBy(r => r.SortOrder).ThenBy(r => r.Id)
            .GroupBy(r => inverted ? r.TargetSubjectId : r.SubjectId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => inverted ? r.SubjectId : r.TargetSubjectId).Distinct().ToList());
}
