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

/// <summary>Year-view register for conditions: past the strip zoom threshold a condition's bar
/// would tower far beyond the viewport and its extent stops being readable, so each year band
/// instead carries a named strip of what is IN FORCE that year.</summary>
public sealed class ConditionStripItem : TimelineItem
{
    public string Label { get; init; } = "";
}
