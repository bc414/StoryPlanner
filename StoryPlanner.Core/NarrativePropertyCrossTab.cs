namespace StoryPlanner.Core;

/// <summary>
/// Groups owners into the cells of a two-property cross-tab — who sits at which pair of values.
///
/// Retrieval only, and the omissions are the design. There is no count ordering, no percentage,
/// no "coverage" figure, and no marking of empty cells: an empty intersection is a fact about the
/// world Brian built, exactly like an untouched source-material Part, and a cell holding six
/// subjects is not thereby more important than one holding one. Axis order is the value
/// definitions' own row order, which
/// <see cref="NarrativePropertyValueDefinition"/> documents as the authored spectrum — never
/// alphabetical and never by population.
///
/// Shared by the WPF grids and any future consumer so the grouping cannot drift; Pure-tier
/// testable, no .storyplan required.
/// </summary>
public static class NarrativePropertyCrossTab
{
    /// <summary>The unset band's synthetic id. Negative so it can never collide with a real
    /// <see cref="NarrativePropertyValueDefinition"/> id, and never persisted anywhere — there is
    /// deliberately no "(none)" value row in the database, and this does not become one.</summary>
    public const int UnsetBandId = -1;

    public const string UnsetBandLabel = "(unset)";

    /// <summary>One position on an axis: a real allowed value, or the synthetic unset band.</summary>
    public readonly record struct Band(int ValueDefinitionId, string Label, string ColorHex)
    {
        public bool IsUnset => ValueDefinitionId == UnsetBandId;
    }

    public sealed record Cell(int RowIndex, int ColumnIndex, IReadOnlyList<int> OwnerIds);

    public sealed record Result(
        IReadOnlyList<Band> Rows,
        IReadOnlyList<Band> Columns,
        IReadOnlyList<Cell> Cells)
    {
        /// <summary>Owners placed anywhere in this grid. With the unset band off this is LESS than
        /// the owner count passed in, legitimately — see the remarks on Build.</summary>
        public int PlacedOwnerCount => Cells.Sum(c => c.OwnerIds.Count);

        public Cell CellAt(int row, int column) =>
            Cells.First(c => c.RowIndex == row && c.ColumnIndex == column);
    }

    /// <summary>
    /// Builds the grid. <paramref name="rowValues"/> and <paramref name="columnValues"/> are the two
    /// properties' allowed values, already in row order. <paramref name="ownerValueIds"/> maps an
    /// owner to the value definition ids it holds — an owner absent from it, or holding nothing for
    /// one of these two properties, is unset on that axis.
    ///
    /// <para><b><paramref name="includeUnsetBand"/> changes the population, not just the layout.</b>
    /// True: "(unset)" is appended as an ordinary last band on each axis and unset owners land in
    /// it. False: an owner unset on EITHER axis is absent from this grid entirely. That is correct
    /// for a board whose properties are all meant to be assigned, and it means grid populations
    /// legitimately differ from each other and from the owner count. Callers must not report the
    /// difference as missing data.</para>
    /// </summary>
    public static Result Build(
        IReadOnlyList<NarrativePropertyValueDefinition> rowValues,
        IReadOnlyList<NarrativePropertyValueDefinition> columnValues,
        IReadOnlyDictionary<int, IReadOnlySet<int>> ownerValueIds,
        IEnumerable<int> ownerIds,
        bool includeUnsetBand)
    {
        var rows = BuildBands(rowValues, includeUnsetBand);
        var columns = BuildBands(columnValues, includeUnsetBand);

        var buckets = new Dictionary<(int Row, int Column), List<int>>();

        foreach (var ownerId in ownerIds)
        {
            var held = ownerValueIds.TryGetValue(ownerId, out var v) ? v : EmptySet;

            var rowIndex = IndexOf(rows, held);
            var columnIndex = IndexOf(columns, held);

            // -1 means unset on that axis with no band to hold it. Dropping the owner is the
            // documented behaviour, not a silent failure.
            if (rowIndex < 0 || columnIndex < 0) continue;

            if (!buckets.TryGetValue((rowIndex, columnIndex), out var bucket))
                buckets[(rowIndex, columnIndex)] = bucket = new List<int>();
            bucket.Add(ownerId);
        }

        // Every cell is materialized, including empty ones: a grid with holes in it would make an
        // empty intersection indistinguishable from a rendering bug.
        var cells = new List<Cell>(rows.Count * columns.Count);
        for (var r = 0; r < rows.Count; r++)
        for (var c = 0; c < columns.Count; c++)
            cells.Add(new Cell(r, c,
                buckets.TryGetValue((r, c), out var owners)
                    ? owners
                    : Array.Empty<int>()));

        return new Result(rows, columns, cells);
    }

    /// <summary>
    /// Convenience for the caller that has raw <see cref="NarrativePropertyValue"/> rows: folds them
    /// into the owner→value-ids map Build wants. The caller is responsible for having already
    /// scoped the rows to one owner type — these rows carry no OwnerType, so subject 7 and chapter
    /// 7 collide if that scoping is skipped.
    /// </summary>
    public static IReadOnlyDictionary<int, IReadOnlySet<int>> MapAssignments(
        IEnumerable<NarrativePropertyValue> values) =>
        values
            .GroupBy(v => v.OwnerId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlySet<int>)g.Select(v => v.ValueDefinitionId).ToHashSet());

    private static readonly IReadOnlySet<int> EmptySet = new HashSet<int>();

    private static List<Band> BuildBands(
        IReadOnlyList<NarrativePropertyValueDefinition> values, bool includeUnsetBand)
    {
        var bands = values
            .Select(v => new Band(v.Id, v.ValueName, v.ColorHex))
            .ToList();

        // Last, always — an axis reads as its authored spectrum with "not on it" at the end,
        // rather than the spectrum being interrupted.
        if (includeUnsetBand)
            bands.Add(new Band(UnsetBandId, UnsetBandLabel, string.Empty));

        return bands;
    }

    private static int IndexOf(List<Band> bands, IReadOnlySet<int> held)
    {
        for (var i = 0; i < bands.Count; i++)
            if (!bands[i].IsUnset && held.Contains(bands[i].ValueDefinitionId))
                return i;

        var unsetIndex = bands.FindIndex(b => b.IsUnset);
        return unsetIndex;   // -1 when there is no unset band, which drops the owner
    }
}
