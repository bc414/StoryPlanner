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

/// <summary>A derived era (the gap between two consecutive pivots) with its collapse state.
/// Eras are never stored — this row exists only for the duration of a rebuild.</summary>
public partial class EraRow : ObservableObject
{
    private readonly TimelineViewModel _owner;

    public EraRow(TimelineViewModel owner, Era era, double fromYear, double toYear)
    {
        _owner = owner;
        Label = era.Label;
        Key = $"{era.StartYear?.ToString() ?? "min"}..{era.EndYear?.ToString() ?? "max"}";
        FromYear = fromYear;
        ToYear = toYear;
    }

    public string Label { get; }
    /// <summary>Stable across rebuilds: derived from the era's pivot bounds, not the clipped range.</summary>
    public string Key { get; }
    public double FromYear { get; }
    public double ToYear { get; }
    public string Span => $"{(int)(ToYear - FromYear)} years";
    public bool IsCollapsed => _owner.IsEraCollapsed(Key);
    public void NotifyCollapsedChanged() => OnPropertyChanged(nameof(IsCollapsed));
}
