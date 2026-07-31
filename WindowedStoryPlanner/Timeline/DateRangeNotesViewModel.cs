using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;

namespace WindowedStoryPlanner;

/// <summary>
/// Cross-cut of the plan by world time: every note on an EVENT track whose date falls in a range,
/// in chronological order, plus the plot points dated into the same range shown separately.
///
/// Events only — condition notes are excluded. Whether a note asserts a moment or an extent is
/// the track's business (NoteTrackDefinition.SupportsWorldDateEnd), never the stored value, so
/// this filters on the track. A chronology of things that happened reads differently from a set
/// of states in force, and the timeline canvas is where the two belong together.
///
/// Retrieval only. It reports what is dated into the range and nothing else — it does not surface
/// undated notes (that is the Timeline tab's triage panel), does not compare the two registers,
/// and does not rank or characterise a range's contents.
/// </summary>
public partial class DateRangeNotesViewModel : TaggedNotesViewModelBase
{
    private readonly IStoryService _storyService;

    // Null = no range applied yet, which is what the window opens on: rendering the whole
    // chronology by default is visibly slow, and nothing here is worth that wait before the
    // author has said what they are looking for. Field initializer rather than a constructor
    // assignment because derived field initializers run BEFORE the base constructor, and the
    // base constructor seeds the list by calling Matches(), which reads this.
    private WorldDateRange? _range = null;

    private string _appliedLabel = "";

    public DateRangeNotesViewModel(
        IViewModelRegistry registry,
        IStoryService storyService,
        IWindowManager windowManager) : base(registry, windowManager)
    {
        _storyService = storyService;

        ((INotifyCollectionChanged)Notes).CollectionChanged += (_, _) => RaiseStatus();
        registry.StoryLoaded += OnStoryLoaded;

        RebuildPlotPoints();
    }

    // ── The criterion ───────────────────────────────────────────────────────────

    /// <summary>
    /// The range in the app's own world-date notation ("854..914", "1007", "854..", "..914").
    /// Empty means the whole timeline. Invalid input is kept on screen with
    /// <see cref="RangeError"/> and applied to nothing — flag, never guess.
    /// </summary>
    [ObservableProperty]
    private string _rangeText = "";

    [ObservableProperty]
    private string _rangeError = "";

    public bool HasRangeError => RangeError.Length > 0;

    partial void OnRangeErrorChanged(string value) => OnPropertyChanged(nameof(HasRangeError));

    [RelayCommand]
    private void ApplyRange()
    {
        var text = RangeText.Trim();
        if (text.Length == 0)
        {
            // Deliberately not "the whole timeline": that is the one range slow enough to be felt,
            // and it is what the Timeline canvas already is. An explicit open end ("854..") still
            // reaches as far as the author asks.
            RangeError = "";
            _range = null;
            _appliedLabel = "";
        }
        else
        {
            if (!WorldDate.TryParse(text, out var bound, out var error))
            {
                RangeError = error;
                return;
            }
            RangeError = "";
            // The parser cannot tell "1007" from "1007.." — both store a start with no end — so
            // interval-ness comes from the text, exactly as when rendering a note's date.
            _range = WorldDateRange.FromBound(bound, asInterval: text.Contains(".."));
            _appliedLabel = text;
        }

        Reevaluate();
        RebuildPlotPoints();
        RaiseStatus();
    }

    // ── Notes (live, via the base) ──────────────────────────────────────────────

    protected override bool Matches(NoteViewModel note) =>
        _range is { } range &&
        note.SupportsWorldDate && !note.SupportsWorldDateEnd &&  // event tracks only
        note.Note.EffectiveWorldDate() is { } date &&
        range.Overlaps(date, isConditionTrack: false);

    protected override bool AffectsMembership(string? propertyName) =>
        propertyName is nameof(NoteViewModel.WorldDate)
                     or nameof(NoteViewModel.SupportsWorldDate)   // a track change moves all three
                     or nameof(NoteViewModel.SupportsWorldDateEnd)
                     or null or "";

    /// <summary>
    /// Chronological — earliest, then latest, then id, the same ordering the MCP server's
    /// get_notes_in_date_range uses. ListCollectionView inserts at the sorted position, so live
    /// membership stays ordered. Safe to sort live: NoteView's date box commits on LostFocus,
    /// not per keystroke, so a row cannot move out from under the caret while it is being typed in.
    /// </summary>
    public override IEnumerable NotesSource => _sortedNotes ??= new ListCollectionView(Notes)
    {
        CustomSort = ChronologicalComparer.Instance
    };

    private ListCollectionView? _sortedNotes;

    private sealed class ChronologicalComparer : IComparer
    {
        public static readonly ChronologicalComparer Instance = new();

        public int Compare(object? x, object? y)
        {
            if (x is not NoteViewModel a || y is not NoteViewModel b) return 0;
            var (ae, al) = SpanOf(a);
            var (be, bl) = SpanOf(b);
            int c = ae.CompareTo(be);
            if (c != 0) return c;
            c = al.CompareTo(bl);
            return c != 0 ? c : a.Note.Id.CompareTo(b.Note.Id);
        }

        // Event tracks only reach this list, so the condition-track tail never applies.
        private static (double Earliest, double Latest) SpanOf(NoteViewModel n) =>
            n.Note.EffectiveWorldDate() is { } d
                ? WorldDateRange.Span(d, isConditionTrack: false)
                : (double.NegativeInfinity, double.NegativeInfinity);
    }

    // ── Plot points (snapshot, rebuilt on Apply) ────────────────────────────────
    // Deliberately not live: PlotPointViewModel exposes no Fabula* properties and raises no
    // change notification for them (dates are assigned through the Timeline's triage rows), and
    // adding observable ones purely to animate this region would be a real cost for a case that
    // does not arise while this window is open.

    public ObservableCollection<DateRangePlotPointRow> PlotPoints { get; } = new();

    private void OnStoryLoaded()
    {
        RebuildPlotPoints();
        RaiseStatus();
    }

    private void RebuildPlotPoints()
    {
        PlotPoints.Clear();
        if (_range is not { } range) return;

        // Read through the service each time — its collections are reassigned on project load.
        var theaterNames = _storyService.Theaters.ToDictionary(t => t.Id, t => t.Name);
        var dated = new List<(double At, string Order, int Id, PlotPointViewModel Vm, WorldDatePoint Date)>();

        foreach (var vm in _registry.AllPlotPointViewModels)
        {
            if (vm.PlotPoint.GetFabulaDate() is not { } at) continue;
            // Plot points are always events — a scene wanting a span is more than one scene.
            if (!range.Overlaps(WorldDate.Event(at), isConditionTrack: false)) continue;
            dated.Add((at.EarliestFraction, vm.FullOrder, vm.Id, vm, at));
        }

        foreach (var row in dated.OrderBy(d => d.At).ThenBy(d => d.Order).ThenBy(d => d.Id))
        {
            var vm = row.Vm;
            var chapter = vm.ChapterId is int cid
                ? _registry.AllChapterViewModels.FirstOrDefault(c => c.Id == cid)
                : null;

            PlotPoints.Add(new DateRangePlotPointRow
            {
                Label = $"{vm.FullOrder}{vm.Title}",
                DateText = row.Date.ToString(),
                ChapterText = chapter?.FullNumberAndTitle ?? "(unplaced)",
                TheaterText = theaterNames.TryGetValue(vm.PlotPoint.TheaterId, out var name)
                    ? name
                    : "(Unplaced)",
                OpenCommand = new RelayCommand(() =>
                    _windowManager.OpenPlotPointWindow(vm))
            });
        }
    }

    // ── Status ──────────────────────────────────────────────────────────────────
    // Counts of what was retrieved, nothing derived from them. No ratio between the two
    // registers, no comparison against what is undated.

    public bool HasRange => _range is not null;

    public string StatusLine => HasRange
        ? $"{Notes.Count} event note(s) · {PlotPoints.Count} plot point(s) in {_appliedLabel}"
        : "";

    public string EmptyNotesText => HasRange
        ? $"No event notes are dated into {_appliedLabel}."
        : "Enter a range above and press Apply.";

    public string EmptyPlotPointsText => HasRange
        ? $"No plot points are dated into {_appliedLabel}."
        : "";

    public bool HasNotes => Notes.Count > 0;
    public bool HasPlotPoints => PlotPoints.Count > 0;

    private void RaiseStatus()
    {
        OnPropertyChanged(nameof(HasRange));
        OnPropertyChanged(nameof(StatusLine));
        OnPropertyChanged(nameof(EmptyNotesText));
        OnPropertyChanged(nameof(EmptyPlotPointsText));
        OnPropertyChanged(nameof(HasNotes));
        OnPropertyChanged(nameof(HasPlotPoints));
    }

    public override void Dispose()
    {
        _registry.StoryLoaded -= OnStoryLoaded;
        base.Dispose();
    }
}
