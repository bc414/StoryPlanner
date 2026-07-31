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

/// <summary>One undated (or unusably-dated) item awaiting a date. Assignment is explicit and
/// confirmable: type the notation, press Assign — there is no undo in this app, so nothing is
/// ever written by a gesture alone.</summary>
public partial class TriageRow : ObservableObject
{
    private readonly TimelineViewModel _owner;
    public Note? Note { get; }
    public PlotPoint? PlotPoint { get; }

    public TriageRow(TimelineViewModel owner, Note? note, PlotPoint? plotPoint)
    {
        _owner = owner;
        Note = note;
        PlotPoint = plotPoint;
    }

    public string Display { get; init; } = "";
    public string Preview { get; init; } = "";
    public bool IsCondition { get; init; }
    public bool IsPlotPoint => PlotPoint is not null;

    public string DateHint => IsCondition
        ? "854..914 · 1007.. · ..1007"
        : "1007 · 1007-03 · 1007-03-15";

    [ObservableProperty] private string _dateText = "";
    [ObservableProperty] private string _error = "";

    public IEnumerable<Theater> TheaterChoices => _owner.TheaterChoices;

    public Theater? SelectedTheater
    {
        get => PlotPoint is { } pp ? TheaterChoices.FirstOrDefault(t => t.Id == pp.TheaterId) : null;
        set { if (PlotPoint is { } pp && value is not null) pp.TheaterId = value.Id; }
    }

    [RelayCommand]
    private async Task Assign()
    {
        if (!WorldDate.TryParse(DateText, out var date, out var error) || date is null)
        {
            Error = error.Length > 0 ? error : "Give a date first.";
            return;
        }
        if (IsPlotPoint || !IsCondition)
        {
            if (date.Value.End is not null || DateText.Contains(".."))
            {
                Error = IsPlotPoint
                    ? "A plot point is one scene — a span means it is holding more than one. Give the moment it happens; duration belongs on a subject condition track."
                    : "This is an event track — give a single date, or move the note to the condition track.";
                return;
            }
        }

        if (Note is { } note)
        {
            note.SetWorldDate(date);
            note.WorldDate = string.Empty;
        }
        else if (PlotPoint is { } pp && date.Value.Start is { } at)
        {
            pp.SetFabulaDate(at);
        }
        Error = "";
        await _owner.PersistAndRebuild();
    }
}
