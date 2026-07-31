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

public sealed class EventMarkerItem : TimelineItem
{
    public string Label { get; init; } = "";
    public string Fill { get; init; } = "#888888";
    public string Border { get; init; } = "Transparent"; // plot points carry their story's color here
    public bool IsFlagged { get; init; }
    public bool IsPlotPoint { get; init; }
    public object? Payload { get; init; }
    public CardContent Card { get; init; } = new();
}
