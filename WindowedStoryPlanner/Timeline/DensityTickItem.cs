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

/// <summary>A collapsed theater's density ribbon: one tick per populated year, opacity by how
/// many items sit there. Keeps absence visible and gives peripheral awareness of where a
/// hidden column is busy, at ~24px instead of ~150.</summary>
public sealed class DensityTickItem : TimelineItem
{
    public double Opacity { get; init; }
}
