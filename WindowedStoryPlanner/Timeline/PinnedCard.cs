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

/// <summary>A popup the author pinned: detached from its mark, freely draggable, and surviving
/// rebuilds so two notes from different theaters and years can sit side by side and be compared
/// — the read the side panel's single-selection model cannot serve.</summary>
public partial class PinnedCard : ObservableObject
{
    public CardContent Content { get; init; } = new();
    [ObservableProperty] private double _x;
    [ObservableProperty] private double _y;
}
