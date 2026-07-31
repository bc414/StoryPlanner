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

/// <summary>The summary band a collapsed era draws in one theater's column.</summary>
public sealed class EraBandItem : TimelineItem
{
    public string Label { get; init; } = "";
    public bool IsFullWidth { get; init; }
}
