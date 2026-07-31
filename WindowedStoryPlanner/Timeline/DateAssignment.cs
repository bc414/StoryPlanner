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

/// <summary>
/// A date the author has proposed by dragging something onto the canvas but has NOT yet
/// committed. The gesture only ever pre-fills; the write happens on Confirm. This app has no
/// undo, so no drag may silently change data.
/// </summary>
public partial class DateAssignment : ObservableObject
{
    private readonly TimelineViewModel _owner;
    private readonly NoteViewModel? _note;
    private readonly PlotPoint? _plotPoint;
    private readonly TriageRow? _triageRow;

    public DateAssignment(TimelineViewModel owner, NoteViewModel? note, PlotPoint? plotPoint,
        TriageRow? triageRow, string display, bool isCondition, string proposed, string context)
    {
        _owner = owner;
        _note = note;
        _plotPoint = plotPoint;
        _triageRow = triageRow;
        Display = display;
        IsCondition = isCondition;
        Context = context;
        _dateText = proposed;
    }

    public string Display { get; }
    public string Context { get; }
    public bool IsCondition { get; }
    public bool IsPlotPoint => _plotPoint is not null;
    public NoteViewModel? Note => _note;

    public string Hint => IsCondition
        ? "854..914 · 1007.. (end TBD) · ..1007"
        : "1007 · 1007-03 · 1007-03-15";

    [ObservableProperty] private string _dateText = "";
    [ObservableProperty] private string _error = "";
    [ObservableProperty] private double _x;
    [ObservableProperty] private double _y;

    public IEnumerable<Theater> TheaterChoices => _owner.TheaterChoices;

    [ObservableProperty] private Theater? _selectedTheater;

    [RelayCommand]
    private async Task Confirm()
    {
        if (!WorldDate.TryParse(DateText, out var date, out var error) || date is null)
        {
            Error = error.Length > 0 ? error : "Give a date first.";
            return;
        }
        if (!IsCondition && (date.Value.End is not null || DateText.Contains("..")))
        {
            Error = IsPlotPoint
                ? "A plot point is one scene — a span means it is holding more than one. Give the moment it happens."
                : "This is an event track — give a single date, or move the note to the condition track.";
            return;
        }

        if (_note is { } noteVm)
        {
            noteVm.WorldDate = DateText;
            if (noteVm.HasWorldDateError) { Error = noteVm.WorldDateError; return; }
        }
        else if (_plotPoint is { } pp && date.Value.Start is { } at)
        {
            pp.SetFabulaDate(at);
            if (SelectedTheater is { } t) pp.TheaterId = t.Id;
        }

        _owner.PendingAssignment = null;
        if (_triageRow is not null) await _owner.PersistAndRebuild();  // triage list shrinks
        else await _owner.PersistAndRebuildCanvasOnly();
    }

    [RelayCommand]
    private void Cancel() => _owner.PendingAssignment = null;
}
