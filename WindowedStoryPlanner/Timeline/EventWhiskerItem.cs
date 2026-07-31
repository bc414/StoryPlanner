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

/// <summary>Month-precision events: the marker sits at the month's midpoint (a position claim),
/// this thin bar spans the whole month (the certainty claim).</summary>
public sealed class EventWhiskerItem : TimelineItem
{
    public string Fill { get; init; } = "#888888";
}
