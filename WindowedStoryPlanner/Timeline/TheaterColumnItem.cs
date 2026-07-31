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

/// <summary>The column's full-height background band in the scrolling body — the header's
/// counterpart, carrying only the banding and separator.</summary>
public sealed class TheaterColumnItem : TimelineItem
{
    public bool IsBandShaded { get; init; }
}
