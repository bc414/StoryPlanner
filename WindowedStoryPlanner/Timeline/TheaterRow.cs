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

/// <summary>A theater in the side panel, carrying its canvas collapse state.</summary>
public partial class TheaterRow : ObservableObject
{
    private readonly TimelineViewModel _owner;
    public Theater Theater { get; }

    public TheaterRow(TimelineViewModel owner, Theater theater)
    {
        _owner = owner;
        Theater = theater;
    }

    public string Name
    {
        get => Theater.Name;
        set { Theater.Name = value; OnPropertyChanged(); }
    }

    public bool IsCollapsed => _owner.IsTheaterCollapsed(Theater.Id);
    public int TheaterId => Theater.Id;
    public void NotifyCollapsedChanged() => OnPropertyChanged(nameof(IsCollapsed));
}
