using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Grouping subjects into the cells of a two-property cross-tab. Pure — no .storyplan.
///
/// The load-bearing case is <c>includeUnsetBand: false</c>, which changes the POPULATION and not
/// just the layout: an owner unset on either axis leaves the grid entirely, so a grid's total is
/// legitimately smaller than the owner count. That is the political-axes board's configuration,
/// and a caller that reported the shortfall as missing data would be wrong.
/// </summary>
public class NarrativePropertyCrossTabTests
{
    // Political Power (property 2) and Social Contract (property 4), with the live value ids.
    private const int Distributed = 3, Concentrated = 4;
    private const int Unconditional = 7, Transactional = 8, Stratified = 13;

    private static NarrativePropertyValueDefinition Value(int id, string name, string color = "") =>
        new() { Id = id, ValueName = name, ColorHex = color };

    private static readonly List<NarrativePropertyValueDefinition> PoliticalPower =
    [
        Value(Distributed, "Distributed", "#27AE60"),
        Value(Concentrated, "Concentrated", "#8E44AD"),
    ];

    private static readonly List<NarrativePropertyValueDefinition> SocialContract =
    [
        Value(Unconditional, "Unconditional"),
        Value(Transactional, "Transactional"),
        Value(Stratified, "Stratified"),
    ];

    private static IReadOnlyDictionary<int, IReadOnlySet<int>> Assign(
        params (int OwnerId, int[] ValueIds)[] rows) =>
        rows.ToDictionary(r => r.OwnerId, r => (IReadOnlySet<int>)r.ValueIds.ToHashSet());

    // ── Shape ───────────────────────────────────────────────────────────────────

    [Fact]
    public void A_two_by_three_grid_has_two_rows_three_columns_and_six_cells()
    {
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract, Assign(), [], includeUnsetBand: false);

        Assert.Equal(2, result.Rows.Count);
        Assert.Equal(3, result.Columns.Count);
        Assert.Equal(6, result.Cells.Count);
    }

    [Fact]
    public void Empty_cells_are_materialized_rather_than_omitted()
    {
        // A grid with holes would make an empty intersection indistinguishable from a bug.
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract,
            Assign((1, [Distributed, Unconditional])), [1], includeUnsetBand: false);

        Assert.Equal(6, result.Cells.Count);
        Assert.Empty(result.CellAt(1, 2).OwnerIds);
    }

    [Fact]
    public void Axis_order_follows_row_order_not_value_name()
    {
        // "Distributed" sorts before "Concentrated" alphabetically; the authored spectrum is the
        // other way round, and the spectrum wins.
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract, Assign(), [], includeUnsetBand: false);

        Assert.Equal(["Distributed", "Concentrated"], result.Rows.Select(r => r.Label));
        Assert.Equal(["Unconditional", "Transactional", "Stratified"], result.Columns.Select(c => c.Label));
    }

    [Fact]
    public void A_band_carries_its_value_colour_through()
    {
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract, Assign(), [], includeUnsetBand: false);

        Assert.Equal("#27AE60", result.Rows[0].ColorHex);
        // Empty is passed through untouched — never substituted with a generated palette entry.
        Assert.Equal(string.Empty, result.Columns[0].ColorHex);
    }

    // ── Placement ───────────────────────────────────────────────────────────────

    [Fact]
    public void Owners_land_in_the_cell_of_the_two_values_they_hold()
    {
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract,
            Assign(
                (129, [Distributed, Unconditional]),      // Pioneer Equestria
                (55,  [Distributed, Unconditional]),      // Equestrian Republic
                (97,  [Concentrated, Transactional])),    // Feudal Herzland
            [129, 55, 97], includeUnsetBand: false);

        Assert.Equal([129, 55], result.CellAt(0, 0).OwnerIds);
        Assert.Equal([97], result.CellAt(1, 1).OwnerIds);
        Assert.Equal(3, result.PlacedOwnerCount);
    }

    [Fact]
    public void Values_of_other_properties_are_ignored_rather_than_confusing_the_placement()
    {
        // A board holds five properties; a grid uses two of them. The other three are on the
        // owner's value set and must not affect which cell it lands in.
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract,
            Assign((129, [1, 2, Distributed, Unconditional, 9, 5])),
            [129], includeUnsetBand: false);

        Assert.Equal([129], result.CellAt(0, 0).OwnerIds);
    }

    // ── The unset band, both ways ───────────────────────────────────────────────

    [Fact]
    public void With_the_band_on_it_is_appended_last_on_each_axis()
    {
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract, Assign(), [], includeUnsetBand: true);

        Assert.Equal(3, result.Rows.Count);
        Assert.Equal(4, result.Columns.Count);
        Assert.True(result.Rows[^1].IsUnset);
        Assert.True(result.Columns[^1].IsUnset);
        Assert.Equal(NarrativePropertyCrossTab.UnsetBandLabel, result.Rows[^1].Label);
        // Never a real value definition id — nothing may write this back as an answer.
        Assert.Equal(NarrativePropertyCrossTab.UnsetBandId, result.Rows[^1].ValueDefinitionId);
    }

    [Fact]
    public void With_the_band_on_partially_and_fully_unset_owners_are_placed()
    {
        // Wingbardy (99) holds Social Contract but not Political Power; Zumidia (220) holds neither.
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract,
            Assign((99, [Transactional]), (220, [])),
            [99, 220], includeUnsetBand: true);

        Assert.Equal([99], result.CellAt(2, 1).OwnerIds);    // unset row, Transactional column
        Assert.Equal([220], result.CellAt(2, 3).OwnerIds);   // unset row, unset column
        Assert.Equal(2, result.PlacedOwnerCount);
    }

    [Fact]
    public void With_the_band_off_an_owner_unset_on_either_axis_leaves_the_grid_entirely()
    {
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract,
            Assign(
                (129, [Distributed, Unconditional]),
                (99,  [Transactional]),                  // Political Power unset
                (220, [])),                              // both unset
            [129, 99, 220], includeUnsetBand: false);

        // Three owners in, one placed — and the shortfall is the configuration working, not a gap.
        Assert.Equal(1, result.PlacedOwnerCount);
        Assert.Equal([129], result.CellAt(0, 0).OwnerIds);
        Assert.DoesNotContain(result.Rows, b => b.IsUnset);
        Assert.DoesNotContain(result.Columns, b => b.IsUnset);
    }

    [Fact]
    public void An_owner_with_no_assignment_row_at_all_behaves_as_unset_not_as_an_error()
    {
        var result = NarrativePropertyCrossTab.Build(
            PoliticalPower, SocialContract, Assign(), [220], includeUnsetBand: true);

        Assert.Equal([220], result.CellAt(2, 3).OwnerIds);
    }

    // ── MapAssignments ──────────────────────────────────────────────────────────

    [Fact]
    public void MapAssignments_folds_rows_into_one_set_per_owner()
    {
        List<NarrativePropertyValue> values =
        [
            new() { Id = 1, OwnerId = 129, ValueDefinitionId = Distributed },
            new() { Id = 2, OwnerId = 129, ValueDefinitionId = Unconditional },
            new() { Id = 3, OwnerId = 97,  ValueDefinitionId = Concentrated },
        ];

        var map = NarrativePropertyCrossTab.MapAssignments(values);

        Assert.Equal([Distributed, Unconditional], map[129].Order());
        Assert.Equal([Concentrated], map[97]);
        Assert.False(map.ContainsKey(220));
    }
}
