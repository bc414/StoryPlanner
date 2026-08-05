using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WindowedStoryPlanner;

/// <summary>
/// One pairwise grid on a board — two properties crossed, subjects in the cells. A board of n
/// properties produces C(n,2) of these.
///
/// A flat cell list including the header row and column, laid out by a UniformGrid bound to
/// <see cref="ColumnCount"/>. Nothing here ranks, scores, or highlights: an empty cell is a fact
/// about the world, and the counts on the headers are a tally, not a measure.
/// </summary>
public partial class PropertyGridViewModel : ObservableObject
{
    public required string RowPropertyName { get; init; }
    public required string ColumnPropertyName { get; init; }

    /// <summary>Header for the collapsed expander, e.g. "Political Power × Social Contract".</summary>
    public string Header => $"{RowPropertyName} × {ColumnPropertyName}";

    /// <summary>
    /// Subjects placed in this grid. With the board's unset band off this is legitimately fewer
    /// than the board's subject count — a subject unset on either axis is absent from this grid,
    /// which is the configuration working and not a gap. The label says so rather than leaving a
    /// bare number that looks like a discrepancy.
    /// </summary>
    public required int PlacedCount { get; init; }

    public required int OmittedCount { get; init; }

    public string CountLabel => OmittedCount == 0
        ? $"{PlacedCount} subjects"
        : $"{PlacedCount} subjects · {OmittedCount} unset on an axis, not shown";

    /// <summary>Total columns INCLUDING the leading header column.</summary>
    public required int ColumnCount { get; init; }

    /// <summary>Row-major, header row first, header column first within each row.</summary>
    public ObservableCollection<PropertyGridCell> Cells { get; } = new();
}

/// <summary>
/// One position in the flat grid: a corner, an axis header, or a populated cell. A single
/// collection with a discriminator rather than three, because a UniformGrid lays out one
/// sequence and the alternative is three parallel collections that can disagree about size.
/// </summary>
public sealed class PropertyGridCell
{
    public required PropertyGridCellKind Kind { get; init; }

    /// <summary>Header text for a header cell; empty otherwise.</summary>
    public string Label { get; init; } = string.Empty;

    /// <summary>Header fill, taken from the value's own colour so an axis reads in the same
    /// palette as the chips inside the cells.</summary>
    public string ColorHex { get; init; } = string.Empty;

    public System.Windows.Media.Brush HeaderFill => ChipInk.FillBrush(ColorHex);
    public System.Windows.Media.Brush HeaderInk => ChipInk.InkBrush(ColorHex);

    public IReadOnlyList<SubjectCardViewModel> Cards { get; init; } = [];

    public bool IsCorner => Kind == PropertyGridCellKind.Corner;
    public bool IsHeader => Kind is PropertyGridCellKind.RowHeader or PropertyGridCellKind.ColumnHeader;
    public bool IsBody => Kind == PropertyGridCellKind.Body;
}

public enum PropertyGridCellKind
{
    Corner,
    ColumnHeader,
    RowHeader,
    Body
}
