namespace StoryPlanner.Core;

/// <summary>
/// Groups owners that hold the SAME value on EVERY property of a board — the full-tuple
/// collisions a pairwise grid cannot show, since a grid crosses two properties at a time and its
/// cells mix owners that agree on those two and differ elsewhere.
///
/// <para>Exact predicate only. This measures nothing: there is no similarity score, no
/// near-miss ("differs on one axis"), no explanation of why a group exists, and nothing is ever
/// proposed as belonging in one. Groups are ordered largest first, which is presentation over
/// authored data — the same category as the Progress tab's sortable counts — and ties break on the
/// authored value order so the ordering is deterministic rather than incidental.</para>
///
/// <para><b>An owner unset on any property is not grouped at all.</b> Two owners cannot be said to
/// match on all five when three are unknown, so they are reported separately with their unset
/// count. This mirrors a grid's behaviour when the board's unset band is off.</para>
///
/// Pure-tier: plain collections in, no DbContext.
/// </summary>
public static class NarrativePropertyMatchGroups
{
    /// <summary>
    /// Owners sharing one exact tuple. <paramref name="ValueDefinitionIds"/> is positional — one
    /// entry per board property, in board order.
    /// </summary>
    public sealed record Group(IReadOnlyList<int> ValueDefinitionIds, IReadOnlyList<int> OwnerIds);

    /// <summary>An owner left out of grouping, and how many of the board's properties it lacks.</summary>
    public sealed record PartialOwner(int OwnerId, int UnsetCount);

    public sealed record Result(
        IReadOnlyList<Group> Shared,
        IReadOnlyList<Group> Alone,
        IReadOnlyList<PartialOwner> PartiallyAssigned)
    {
        /// <summary>Owners that landed in a shared group — never the owner count passed in.</summary>
        public int SharedOwnerCount => Shared.Sum(g => g.OwnerIds.Count);

        public int GroupedOwnerCount => SharedOwnerCount + Alone.Count;
    }

    /// <summary>
    /// Builds the groups. <paramref name="properties"/> is the board's members in display order —
    /// the tuple is positional, so two owners cannot match by holding the same values against
    /// different properties. Values of properties outside the board are ignored, exactly as
    /// <see cref="NarrativePropertyCrossTab"/> ignores them.
    /// </summary>
    public static Result Build(
        IReadOnlyList<NarrativePropertyDefinition> properties,
        IReadOnlyDictionary<int, List<NarrativePropertyValueDefinition>> valueDefsByProperty,
        IReadOnlyDictionary<int, IReadOnlySet<int>> ownerValueIds,
        IEnumerable<int> ownerIds)
    {
        var partial = new List<PartialOwner>();

        // Key is the tuple of value ids; the parallel index tuple is the sort key, and is the
        // position of each held value within its own property's list — the authored spectrum
        // order, never alphabetical.
        var buckets = new Dictionary<string, (int[] ValueIds, int[] Indices, List<int> Owners)>();

        foreach (var ownerId in ownerIds)
        {
            var held = ownerValueIds.TryGetValue(ownerId, out var v) ? v : EmptySet;

            var valueIds = new int[properties.Count];
            var indices = new int[properties.Count];
            var unset = 0;

            for (var i = 0; i < properties.Count; i++)
            {
                var values = valueDefsByProperty.TryGetValue(properties[i].Id, out var vs)
                    ? vs
                    : EmptyValues;

                var index = -1;
                for (var j = 0; j < values.Count; j++)
                    if (held.Contains(values[j].Id)) { index = j; break; }

                if (index < 0) { unset++; continue; }

                valueIds[i] = values[index].Id;
                indices[i] = index;
            }

            if (unset > 0)
            {
                partial.Add(new PartialOwner(ownerId, unset));
                continue;
            }

            var key = string.Join(",", valueIds);
            if (!buckets.TryGetValue(key, out var bucket))
                buckets[key] = bucket = (valueIds, indices, new List<int>());
            bucket.Owners.Add(ownerId);
        }

        var all = buckets.Values.ToList();

        var shared = all
            .Where(b => b.Owners.Count > 1)
            .OrderByDescending(b => b.Owners.Count)
            .ThenBy(b => b.Indices, IndexTupleComparer.Instance)
            .Select(b => new Group(b.ValueIds, b.Owners))
            .ToList();

        // All size 1, so the count ordering is vacuous — the authored order is the whole ordering.
        var alone = all
            .Where(b => b.Owners.Count == 1)
            .OrderBy(b => b.Indices, IndexTupleComparer.Instance)
            .Select(b => new Group(b.ValueIds, b.Owners))
            .ToList();

        return new Result(shared, alone, partial.OrderByDescending(p => p.UnsetCount)
                                                .ThenBy(p => p.OwnerId)
                                                .ToList());
    }

    private static readonly IReadOnlySet<int> EmptySet = new HashSet<int>();
    private static readonly List<NarrativePropertyValueDefinition> EmptyValues = new();

    /// <summary>Lexicographic over the per-property value positions — the authored spectrum walk.</summary>
    private sealed class IndexTupleComparer : IComparer<int[]>
    {
        public static readonly IndexTupleComparer Instance = new();

        public int Compare(int[]? x, int[]? y)
        {
            if (x is null || y is null) return 0;

            for (var i = 0; i < x.Length && i < y.Length; i++)
                if (x[i] != y[i]) return x[i].CompareTo(y[i]);

            return x.Length.CompareTo(y.Length);
        }
    }
}
