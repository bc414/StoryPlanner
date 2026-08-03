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

/// <summary>One subject's theater placement — the author-made mapping the timeline's x-axis
/// depends on. Never derived from names; every change is an explicit pick here.</summary>
public partial class PlacementRow : ObservableObject
{
    private readonly TimelineViewModel _owner;
    public Subject Subject { get; }
    public string TypeName { get; }

    public PlacementRow(TimelineViewModel owner, Subject subject, string typeName)
    {
        _owner = owner;
        Subject = subject;
        TypeName = typeName;
    }

    public string Name => Subject.Name;
    public IEnumerable<Theater> TheaterChoices => _owner.TheaterChoices;

    public Theater? SelectedTheater
    {
        get => TheaterChoices.FirstOrDefault(t => t.Id == Subject.TheaterId) ?? UnplacedTheater.Definition;
        set
        {
            if (value is null || value.Id == Subject.TheaterId) return;
            Subject.TheaterId = value.Id;
            OnPropertyChanged();
            // This persists a TheaterId write — a discarded Task would make a failed save silent.
            _owner.PersistAndRebuildCanvasOnly().FireAndForget();
        }
    }
}
