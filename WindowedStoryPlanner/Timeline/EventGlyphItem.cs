using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowedStoryPlanner;

/// <summary>
/// A (theater, year) CELL — the timeline's primary object while year is the working precision.
/// Everything in it genuinely shares a year, so no ordering is known or implied: this is an
/// unordered set, and the glyph is the honest rendering of that, not a degraded marker. The
/// composition bar shows the subject-type mix; clicking opens the full list.
/// </summary>
public sealed class EventGlyphItem : TimelineItem
{
    public string Label { get; init; } = "";   // "7 events" / a single name
    public string YearLabel { get; init; } = "";
    public IReadOnlyList<CompositionSegment> Composition { get; init; } = [];
    public IReadOnlyList<CellEntry> Entries { get; init; } = [];
    public bool HasFlagged { get; init; }
    /// <summary>Every item in the cell, in full — the answer to "5 events" telling you nothing.</summary>
    public CardContent Card { get; init; } = new();
}
