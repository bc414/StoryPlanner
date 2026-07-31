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

public sealed class ConditionBarItem : TimelineItem
{
    public string Label { get; init; } = "";
    public string Fill { get; init; } = "#888888";
    public bool IsFlagged { get; init; }
    public bool IsOpenEnded { get; init; }        // end TBD — faded tail
    public bool IsLabelVisible { get; init; }     // only when the bar is tall enough to carry text
    public object? Payload { get; init; }
    /// <summary>Full note for the hover popup — the answer to the rotated-label problem: the
    /// bar carries a glyph, the popup carries the reading.</summary>
    public CardContent Card { get; init; } = new();
}
