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

/// <summary>A year number in the pinned left gutter: scrolls vertically with the body but never
/// horizontally, so the date stays readable however far right you scan.</summary>
public sealed class YearLabelItem : TimelineItem
{
    public string Label { get; init; } = "";
    public bool IsPivot { get; init; }
}
