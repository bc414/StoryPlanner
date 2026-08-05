using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Exact-tuple grouping over a board's properties. Pure — no .storyplan.
///
/// Two rules carry the weight. An owner unset on ANY property is excluded from grouping entirely
/// rather than matched on its known values — "these two agree on all five" must not be said when
/// three are unknown. And the ordering is size-first with the authored value order as tie-break,
/// so a value renamed to sort differently alphabetically must not move a group.
/// </summary>
public class NarrativePropertyMatchGroupsTests
{
    // Two properties, poles in authored order: Economy (1,2) and Political Power (3,4).
    private const int AssetSpec = 1, Standardization = 2;
    private const int Distributed = 3, Concentrated = 4;

    private static readonly List<NarrativePropertyDefinition> Board =
    [
        new() { Id = 1, Name = "Economy", DisplayOrder = 1 },
        new() { Id = 2, Name = "Political Power", DisplayOrder = 2 },
    ];

    private static readonly Dictionary<int, List<NarrativePropertyValueDefinition>> Values = new()
    {
        [1] =
        [
            new() { Id = AssetSpec, NarrativePropertyDefinitionId = 1, ValueName = "Asset Specificity" },
            new() { Id = Standardization, NarrativePropertyDefinitionId = 1, ValueName = "Standardization" },
        ],
        [2] =
        [
            new() { Id = Distributed, NarrativePropertyDefinitionId = 2, ValueName = "Distributed" },
            new() { Id = Concentrated, NarrativePropertyDefinitionId = 2, ValueName = "Concentrated" },
        ],
    };

    private static IReadOnlyDictionary<int, IReadOnlySet<int>> Assign(
        params (int OwnerId, int[] ValueIds)[] rows) =>
        rows.ToDictionary(r => r.OwnerId, r => (IReadOnlySet<int>)r.ValueIds.ToHashSet());

    private static NarrativePropertyMatchGroups.Result Build(
        IReadOnlyDictionary<int, IReadOnlySet<int>> assignments) =>
        NarrativePropertyMatchGroups.Build(Board, Values, assignments, assignments.Keys.Order());

    // ── Grouping ────────────────────────────────────────────────────────────────

    [Fact]
    public void Owners_holding_the_same_value_on_every_property_group_together()
    {
        var result = Build(Assign(
            (1, [AssetSpec, Distributed]),
            (2, [AssetSpec, Distributed]),
            (3, [AssetSpec, Distributed])));

        var group = Assert.Single(result.Shared);
        Assert.Equal([1, 2, 3], group.OwnerIds);
        Assert.Empty(result.Alone);
    }

    [Fact]
    public void Differing_on_a_single_property_is_enough_to_split_them()
    {
        var result = Build(Assign(
            (1, [AssetSpec, Distributed]),
            (2, [AssetSpec, Concentrated])));

        Assert.Empty(result.Shared);
        Assert.Equal(2, result.Alone.Count);
    }

    [Fact]
    public void Shared_holds_only_multi_member_groups_and_Alone_only_singletons()
    {
        var result = Build(Assign(
            (1, [AssetSpec, Distributed]),
            (2, [AssetSpec, Distributed]),
            (3, [Standardization, Concentrated])));

        Assert.Equal(2, Assert.Single(result.Shared).OwnerIds.Count);
        Assert.Equal([3], Assert.Single(result.Alone).OwnerIds);
        Assert.Equal(2, result.SharedOwnerCount);
        Assert.Equal(3, result.GroupedOwnerCount);
    }

    [Fact]
    public void The_tuple_is_positional_so_the_same_ids_against_other_properties_do_not_match()
    {
        // Owner 2 holds Economy's second pole and Political Power's first; owner 1 the reverse.
        // A set-based key would see two identical two-element sets only if the ids coincided —
        // this pins that the key is per-property, not a bag of ids.
        var result = Build(Assign(
            (1, [AssetSpec, Concentrated]),
            (2, [Standardization, Distributed])));

        Assert.Empty(result.Shared);
        Assert.Equal(2, result.Alone.Count);
    }

    [Fact]
    public void A_value_belonging_to_a_property_outside_the_board_does_not_affect_the_key()
    {
        // 99 is a value of some property not on this board — as in the real file, where a subject
        // carries five axes and a board may compare two of them.
        var result = Build(Assign(
            (1, [AssetSpec, Distributed, 99]),
            (2, [AssetSpec, Distributed])));

        Assert.Equal([1, 2], Assert.Single(result.Shared).OwnerIds);
    }

    // ── Partially assigned owners ───────────────────────────────────────────────

    [Fact]
    public void An_owner_unset_on_any_property_is_excluded_and_reported_with_its_count()
    {
        var result = Build(Assign(
            (1, [AssetSpec, Distributed]),
            (2, [AssetSpec]),        // Political Power unset
            (3, [])));               // both unset

        // Owner 2 must NOT join owner 1 on the strength of its one known value.
        Assert.Equal([1], Assert.Single(result.Alone).OwnerIds);
        Assert.Empty(result.Shared);

        Assert.Collection(result.PartiallyAssigned,
            p => { Assert.Equal(3, p.OwnerId); Assert.Equal(2, p.UnsetCount); },
            p => { Assert.Equal(2, p.OwnerId); Assert.Equal(1, p.UnsetCount); });
    }

    [Fact]
    public void Two_owners_unset_on_the_same_property_are_not_grouped_with_each_other()
    {
        var result = Build(Assign(
            (1, [AssetSpec]),
            (2, [AssetSpec])));

        Assert.Empty(result.Shared);
        Assert.Empty(result.Alone);
        Assert.Equal(2, result.PartiallyAssigned.Count);
    }

    [Fact]
    public void An_owner_with_no_assignment_row_at_all_is_partially_assigned_not_an_error()
    {
        var result = NarrativePropertyMatchGroups.Build(
            Board, Values, Assign(), [7]);

        var partial = Assert.Single(result.PartiallyAssigned);
        Assert.Equal(7, partial.OwnerId);
        Assert.Equal(2, partial.UnsetCount);
    }

    // ── Ordering ────────────────────────────────────────────────────────────────

    [Fact]
    public void Shared_groups_are_ordered_largest_first()
    {
        var result = Build(Assign(
            (1, [AssetSpec, Distributed]),
            (2, [AssetSpec, Distributed]),
            (3, [Standardization, Concentrated]),
            (4, [Standardization, Concentrated]),
            (5, [Standardization, Concentrated])));

        Assert.Equal(2, result.Shared.Count);
        Assert.Equal(3, result.Shared[0].OwnerIds.Count);
        Assert.Equal(2, result.Shared[1].OwnerIds.Count);
    }

    [Fact]
    public void Ties_break_on_authored_value_order_not_on_value_name()
    {
        // Both groups have two members. "Standardization" sorts after "Asset Specificity"
        // alphabetically AND in row order, so this alone would not distinguish the rules —
        // the next test renames to prove which one is in force.
        var result = Build(Assign(
            (1, [Standardization, Distributed]),
            (2, [Standardization, Distributed]),
            (3, [AssetSpec, Distributed]),
            (4, [AssetSpec, Distributed])));

        Assert.Equal([3, 4], result.Shared[0].OwnerIds);   // Economy pole 1 comes first
        Assert.Equal([1, 2], result.Shared[1].OwnerIds);
    }

    [Fact]
    public void Renaming_a_value_to_sort_differently_alphabetically_does_not_move_a_group()
    {
        // Rename the FIRST pole to something that sorts last alphabetically. Row order is the
        // authored spectrum, so its group must still lead.
        var renamed = new Dictionary<int, List<NarrativePropertyValueDefinition>>
        {
            [1] =
            [
                new() { Id = AssetSpec, NarrativePropertyDefinitionId = 1, ValueName = "Zzz Asset Specificity" },
                new() { Id = Standardization, NarrativePropertyDefinitionId = 1, ValueName = "Aaa Standardization" },
            ],
            [2] = Values[2],
        };

        var assignments = Assign(
            (1, [Standardization, Distributed]),
            (2, [Standardization, Distributed]),
            (3, [AssetSpec, Distributed]),
            (4, [AssetSpec, Distributed]));

        var result = NarrativePropertyMatchGroups.Build(
            Board, renamed, assignments, assignments.Keys.Order());

        Assert.Equal([3, 4], result.Shared[0].OwnerIds);
    }

    [Fact]
    public void Alone_groups_are_ordered_by_authored_value_order()
    {
        var result = Build(Assign(
            (1, [Standardization, Concentrated]),
            (2, [AssetSpec, Concentrated]),
            (3, [AssetSpec, Distributed])));

        Assert.Equal([3], result.Alone[0].OwnerIds);   // pole 1, pole 1
        Assert.Equal([2], result.Alone[1].OwnerIds);   // pole 1, pole 2
        Assert.Equal([1], result.Alone[2].OwnerIds);   // pole 2, pole 2
    }

    [Fact]
    public void A_group_carries_its_tuple_positionally()
    {
        var result = Build(Assign(
            (1, [Standardization, Concentrated]),
            (2, [Standardization, Concentrated])));

        Assert.Equal([Standardization, Concentrated], Assert.Single(result.Shared).ValueDefinitionIds);
    }

    [Fact]
    public void An_empty_board_or_no_owners_yields_nothing_rather_than_throwing()
    {
        Assert.Empty(Build(Assign()).Shared);
        Assert.Empty(NarrativePropertyMatchGroups.Build([], Values, Assign((1, [])), [1]).Shared);
    }
}
