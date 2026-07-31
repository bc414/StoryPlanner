using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using StoryPlanner.Core.Timeline;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowedStoryPlanner.ViewModels;

// ── Canvas items ──────────────────────────────────────────────────────────────
// Plain positioned data — the view is an ItemsControl over a Canvas; all layout math
// (zoom, lane packing, aggregation, collapse) happens here, in pixel space.

public abstract class TimelineItem
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Width { get; init; }
    public double Height { get; init; }
    public string Tooltip { get; init; } = "";
}

/// <summary>The pinned column header — lives in its own strip that scrolls horizontally with
/// the canvas but never vertically, so the theater a mark belongs to is always readable.</summary>
public sealed class TheaterHeaderItem : TimelineItem
{
    public string Name { get; init; } = "";
    public bool IsBandShaded { get; init; } // alternating neutral banding — never a hue
    public bool IsCollapsed { get; init; }
    public int TheaterId { get; init; }
    public string CountLabel { get; init; } = "";
    /// <summary>First three characters — the collapsed header is only ~26px wide, and
    /// "Equ/Aqu/Cha/Sky/Her/Sta/Tzi/Zeb/Cry/Ole" stay mutually distinguishable. Full name in the tooltip.</summary>
    public string ShortName { get; init; } = "";
}

/// <summary>The column's full-height background band in the scrolling body — the header's
/// counterpart, carrying only the banding and separator.</summary>
public sealed class TheaterColumnItem : TimelineItem
{
    public bool IsBandShaded { get; init; }
}

/// <summary>A year number in the pinned left gutter: scrolls vertically with the body but never
/// horizontally, so the date stays readable however far right you scan.</summary>
public sealed class YearLabelItem : TimelineItem
{
    public string Label { get; init; } = "";
    public bool IsPivot { get; init; }
}

/// <summary>A collapsed theater's density ribbon: one tick per populated year, opacity by how
/// many items sit there. Keeps absence visible and gives peripheral awareness of where a
/// hidden column is busy, at ~24px instead of ~150.</summary>
public sealed class DensityTickItem : TimelineItem
{
    public double Opacity { get; init; }
}

public sealed class ConditionBarItem : TimelineItem
{
    public string Label { get; init; } = "";
    public string Fill { get; init; } = "#888888";
    public bool IsFlagged { get; init; }
    public bool IsOpenEnded { get; init; }        // end TBD — faded tail
    public bool IsLabelVisible { get; init; }     // only when the bar is tall enough to carry text
    public object? Payload { get; init; }
    /// <summary>Full note for the hover popup — the answer to the rotated-label problem: the
    /// bar carries a glyph, the popup carries the reading.</summary>
    public CardContent Card { get; init; } = new();
}

/// <summary>Year-view register for conditions: past the strip zoom threshold a condition's bar
/// would tower far beyond the viewport and its extent stops being readable, so each year band
/// instead carries a named strip of what is IN FORCE that year.</summary>
public sealed class ConditionStripItem : TimelineItem
{
    public string Label { get; init; } = "";
}

public sealed class EventMarkerItem : TimelineItem
{
    public string Label { get; init; } = "";
    public string Fill { get; init; } = "#888888";
    public string Border { get; init; } = "Transparent"; // plot points carry their story's color here
    public bool IsFlagged { get; init; }
    public bool IsPlotPoint { get; init; }
    public object? Payload { get; init; }
    public CardContent Card { get; init; } = new();
}

/// <summary>Month-precision events: the marker sits at the month's midpoint (a position claim),
/// this thin bar spans the whole month (the certainty claim).</summary>
public sealed class EventWhiskerItem : TimelineItem
{
    public string Fill { get; init; } = "#888888";
}

public sealed class CompositionSegment
{
    public string Fill { get; init; } = "#888888";
    public double Width { get; init; }
}

/// <summary>
/// A (theater, year) CELL — the timeline's primary object while year is the working precision.
/// Everything in it genuinely shares a year, so no ordering is known or implied: this is an
/// unordered set, and the glyph is the honest rendering of that, not a degraded marker. The
/// composition bar shows the subject-type mix; clicking opens the full list.
/// </summary>
public sealed class EventGlyphItem : TimelineItem
{
    public string Label { get; init; } = "";   // "7 events" / a single name
    public string YearLabel { get; init; } = "";
    public IReadOnlyList<CompositionSegment> Composition { get; init; } = [];
    public IReadOnlyList<CellEntry> Entries { get; init; } = [];
    public bool HasFlagged { get; init; }
    /// <summary>Every item in the cell, in full — the answer to "5 events" telling you nothing.</summary>
    public CardContent Card { get; init; } = new();
}

/// <summary>One note (or plot point) rendered in full: subject, provenance, complete content,
/// and — for a flagged note — its flag reason, which often carries more substance than the body
/// it qualifies. The app shows Brian everything; the flagged wall governs export and the MCP
/// server, not this surface.</summary>
public sealed class NoteCard
{
    public string Subject { get; init; } = "";
    public string Meta { get; init; } = "";
    public string Content { get; init; } = "";
    public string FlagReason { get; init; } = "";
    public bool IsFlagged { get; init; }
    public bool IsPlotPoint { get; init; }
    public string Fill { get; init; } = "#888888";

    /// <summary>The live view model behind this note, so a card can edit the date in place
    /// through exactly the same validated setter the note view uses. Null for plot points.</summary>
    public NoteViewModel? Editable { get; init; }

    /// <summary>Invoked after an in-card date edit: saves and re-places the mark.</summary>
    public ICommand? SaveCommand { get; init; }

    public bool CanEditDate => Editable is not null;
    public bool HasContent => Content.Trim().Length > 0;
    public bool HasFlagReason => FlagReason.Trim().Length > 0;
}

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

/// <summary>What a hover popup — or a pinned card — displays. One note for a condition bar,
/// every note in the cell for an event cell.</summary>
public sealed class CardContent
{
    public string Title { get; init; } = "";
    public string Subtitle { get; init; } = "";
    public IReadOnlyList<NoteCard> Notes { get; init; } = [];

    /// <summary>
    /// False when the card holds a single note the title already names — a condition bar or a
    /// placed marker — so the body is just the prose. True for a cell, where each note needs
    /// its own subject line to be told apart. Nothing should be stated twice on one card.
    /// </summary>
    public bool ShowNoteHeaders { get; init; } = true;

    public bool HasSubtitle => Subtitle.Trim().Length > 0;
}

/// <summary>A popup the author pinned: detached from its mark, freely draggable, and surviving
/// rebuilds so two notes from different theaters and years can sit side by side and be compared
/// — the read the side panel's single-selection model cannot serve.</summary>
public partial class PinnedCard : ObservableObject
{
    public CardContent Content { get; init; } = new();
    [ObservableProperty] private double _x;
    [ObservableProperty] private double _y;
}

/// <summary>One item inside a cell — rendered as a row when the cell is expanded in the panel.</summary>
public sealed class CellEntry
{
    public string Label { get; init; } = "";
    public string Detail { get; init; } = "";
    public string Fill { get; init; } = "#888888";
    public string Body { get; init; } = "";
    public bool IsFlagged { get; init; }
    public bool IsPlotPoint { get; init; }
}

/// <summary>The summary band a collapsed era draws in one theater's column.</summary>
public sealed class EraBandItem : TimelineItem
{
    public string Label { get; init; } = "";
    public bool IsFullWidth { get; init; }
}

public sealed class PivotLineItem : TimelineItem
{
    public string Label { get; init; } = "";
}

public sealed class YearTickItem : TimelineItem
{
    public string Label { get; init; } = "";
}

// ── The tab ───────────────────────────────────────────────────────────────────

/// <summary>
/// Master timeline: y = world time (linear, oldest at top), x = theaters. Two registers on one
/// canvas — fabula (subject history notes: bars for conditions, cells for events) and syuzhet
/// (plot points). Retrieval only: shows what is here, never ranks or marks anything missing.
///
/// SNAPSHOT ON BUILD, deliberately not live: this VM reads the model collections directly at
/// Refresh time and does NOT subscribe to per-note changes (deliberate divergence from
/// TaggedNotesViewModelBase, which is a live view). Edits elsewhere appear on the next Refresh.
/// </summary>
public partial class TimelineViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;

    // Layout constants (pixels). Marker/glyph height is FIXED at every zoom — an event asserts
    // position, not extent; only condition bars scale with the year axis.
    private const double HeaderH = 34;
    private const double GutterW = 52;
    private const double CondLaneW = 24;
    // Sized for the CELL glyph ("1007  5 events" + composition bar), which is what the canvas
    // actually draws while every date is year-precision — not for long subject names.
    private const double EventLaneW = 118;
    private const double MarkerH = 14;
    private const double GlyphH = 18;
    private const double MinTheaterW = 120;
    private const double CollapsedTheaterW = 26;
    private const double ColumnPad = 10;
    private const double CollapsedEraH = 30;
    private const double YearGlyphZoomThreshold = 18;

    /// <summary>At and past this zoom the view flips from survey mode (condition bars whose
    /// height is extent) to year mode (per-year "in force" strips).</summary>
    public const double StripZoomThresholdPpy = 240;
    private const double SurveyPpy = 12;
    private const double YearViewPpy = 280;

    public bool IsYearViewMode => PixelsPerYear >= StripZoomThresholdPpy;

    // Session state — not persisted (nothing persists UI prefs in this app yet).
    private readonly HashSet<int> _collapsedTheaters = [];
    private readonly HashSet<string> _collapsedEras = [];

    public TimelineViewModel(IStoryService storyService, IViewModelRegistry registry)
    {
        _storyService = storyService;
        _registry = registry;
        _registry.StoryLoaded += () => { Rebuild(); RebuildSidePanels(); };
    }

    /// <summary>The scrolling body.</summary>
    public ObservableCollection<TimelineItem> Items { get; } = new();
    /// <summary>Pinned top strip — horizontally synced to the body, never scrolls vertically.</summary>
    public ObservableCollection<TimelineItem> HeaderItems { get; } = new();
    /// <summary>Pinned left gutter — vertically synced to the body, never scrolls horizontally.</summary>
    public ObservableCollection<TimelineItem> GutterItems { get; } = new();

    [ObservableProperty] private double _canvasWidth;
    [ObservableProperty] private double _canvasHeight;
    public double HeaderHeight => HeaderH;
    public double GutterWidth => GutterW;
    [ObservableProperty] private double _pixelsPerYear = 12;
    [ObservableProperty] private string _statusLine = "No data yet — open a project and press Refresh.";

    [ObservableProperty] private string _selectedTitle = "";
    [ObservableProperty] private string _selectedBody = "";

    /// <summary>Rows of the currently-expanded cell. Empty unless a cell is selected.</summary>
    public ObservableCollection<CellEntry> SelectedCellEntries { get; } = new();
    [ObservableProperty] private string _selectedCellHeader = "";
    public bool HasSelectedCell => SelectedCellEntries.Count > 0;

    [ObservableProperty] private double _scrollToY;

    [RelayCommand]
    private void SelectItem(TimelineItem? item)
    {
        if (item is null) return;
        SelectedCellEntries.Clear();

        switch (item)
        {
            case EventGlyphItem cell:
                SelectedCellHeader = $"{cell.YearLabel} — {cell.Entries.Count} item(s), no order known within the year";
                foreach (var entry in cell.Entries) SelectedCellEntries.Add(entry);
                SelectedTitle = $"{cell.YearLabel} · {cell.Label}";
                SelectedBody = "";
                break;
            case ConditionBarItem c:
                SelectedTitle = c.Label;
                SelectedBody = c.Tooltip;
                SelectedCellHeader = "";
                break;
            case EventMarkerItem m:
                SelectedTitle = m.Label;
                SelectedBody = m.Tooltip;
                SelectedCellHeader = "";
                break;
            default:
                SelectedTitle = "";
                SelectedBody = item.Tooltip;
                SelectedCellHeader = "";
                break;
        }
        OnPropertyChanged(nameof(HasSelectedCell));
    }

    /// <summary>Selects one row inside an expanded cell without collapsing the cell list.</summary>
    [RelayCommand]
    private void SelectCellEntry(CellEntry? entry)
    {
        if (entry is null) return;
        SelectedTitle = entry.Label;
        SelectedBody = entry.Body.Length > 0 ? entry.Body : entry.Detail;
    }

    // ── Pinned cards ────────────────────────────────────────────────────────────
    // Deliberately NOT cleared by Rebuild: a pinned card is detached from its mark on purpose,
    // so zooming, collapsing, or refreshing never destroys a comparison in progress.

    public ObservableCollection<PinnedCard> PinnedCards { get; } = new();
    public bool HasPinnedCards => PinnedCards.Count > 0;

    /// <summary>
    /// Pins a card at an explicit position — the view passes the hover popup's own location, so
    /// pinning reads as the card freezing exactly where it already is rather than jumping to a
    /// corner. Falls back to a cascade only when no position is supplied.
    /// </summary>
    public void PinCardAt(CardContent? content, double? x = null, double? y = null)
    {
        if (content is null) return;
        // Clicking the same mark twice shouldn't stack identical cards on top of each other.
        if (PinnedCards.Any(p => ReferenceEquals(p.Content, content))) return;
        var cascade = PinnedCards.Count % 8 * 26;
        PinnedCards.Add(new PinnedCard
        {
            Content = content,
            X = Math.Max(0, x ?? 120 + cascade),
            Y = Math.Max(0, y ?? 90 + cascade)
        });
        OnPropertyChanged(nameof(HasPinnedCards));
    }

    [RelayCommand]
    private void ClosePinnedCard(PinnedCard? card)
    {
        if (card is null) return;
        PinnedCards.Remove(card);
        OnPropertyChanged(nameof(HasPinnedCards));
    }

    [RelayCommand]
    private void CloseAllPinnedCards()
    {
        PinnedCards.Clear();
        OnPropertyChanged(nameof(HasPinnedCards));
    }

    [RelayCommand]
    private void ZoomIn() { PixelsPerYear = Math.Min(4000, PixelsPerYear * 1.5); Rebuild(); }

    [RelayCommand]
    private void ZoomOut() { PixelsPerYear = Math.Max(0.5, PixelsPerYear / 1.5); Rebuild(); }

    [RelayCommand]
    private void ZoomSurvey() { PixelsPerYear = SurveyPpy; Rebuild(); }

    [RelayCommand]
    private void ZoomYearView() { PixelsPerYear = YearViewPpy; Rebuild(); }

    [RelayCommand]
    private void Refresh() { Rebuild(); RebuildSidePanels(); }

    [RelayCommand]
    private void ToggleTheaterCollapse(object? theaterId)
    {
        if (theaterId is not int id && !int.TryParse(theaterId?.ToString(), out id)) return;
        if (!_collapsedTheaters.Remove(id)) _collapsedTheaters.Add(id);
        Rebuild();
        foreach (var row in TheaterRows) row.NotifyCollapsedChanged();
    }

    [RelayCommand]
    private void ExpandAllTheaters()
    {
        _collapsedTheaters.Clear();
        Rebuild();
        foreach (var row in TheaterRows) row.NotifyCollapsedChanged();
    }

    public bool IsTheaterCollapsed(int id) => _collapsedTheaters.Contains(id);

    [RelayCommand]
    private void ToggleEraCollapse(EraRow? row)
    {
        if (row is null) return;
        if (!_collapsedEras.Remove(row.Key)) _collapsedEras.Add(row.Key);
        row.NotifyCollapsedChanged();
        Rebuild();
    }

    public bool IsEraCollapsed(string key) => _collapsedEras.Contains(key);

    // ── Drag-to-date ────────────────────────────────────────────────────────────

    [ObservableProperty] private DateAssignment? _pendingAssignment;
    public bool HasPendingAssignment => PendingAssignment is not null;
    partial void OnPendingAssignmentChanged(DateAssignment? value) => OnPropertyChanged(nameof(HasPendingAssignment));

    private readonly List<(double X, double Width, int TheaterId)> _columnRanges = [];

    // Live drag-over ghost: what the date WOULD be if released here. Read-only feedback —
    // the gesture still writes nothing until Confirm.
    [ObservableProperty] private bool _isDragPreviewVisible;
    [ObservableProperty] private string _dragPreviewLabel = "";
    [ObservableProperty] private double _dragPreviewX;
    [ObservableProperty] private double _dragPreviewY;
    [ObservableProperty] private double _dragPreviewLineY;

    /// <param name="bodyX">position within the scrolling body (for year/theater)</param>
    /// <param name="overlayX">position within the overlay (for placing the ghost)</param>
    public void UpdateDragPreview(double bodyX, double bodyY, double overlayX, double overlayY)
    {
        if (YearAtPixel(bodyY) is not { } year) { IsDragPreviewVisible = false; return; }
        var point = PointAtFractionalYear(year);
        var theaterId = TheaterIdAtX(bodyX);
        var theaterName = theaterId == 0
            ? UnplacedTheater.Definition.Name
            : _storyService.Theaters.FirstOrDefault(t => t.Id == theaterId)?.Name ?? "(Unplaced)";

        // Show the notation that will actually be written — no prettier form that could differ.
        DragPreviewLabel = $"{point} · {theaterName}";
        DragPreviewX = overlayX + 16;
        DragPreviewY = overlayY + 14;

        // Snap the rule to where the drop truly lands, so the preview cannot promise a position
        // the write wouldn't honour.
        var snapDelta = PixelForPoint(point) is { } snapped ? snapped - bodyY : 0;
        DragPreviewLineY = overlayY + snapDelta;
        IsDragPreviewVisible = true;
    }

    public void EndDragPreview() => IsDragPreviewVisible = false;

    /// <summary>The live view model for a note id, so every date write goes through the same
    /// validated setter regardless of which surface started it.</summary>
    public NoteViewModel? NoteViewModelFor(int noteId) =>
        _registry.AllNoteViewModels.FirstOrDefault(n => n.Id == noteId);

    /// <summary>Persists an in-card date edit and re-places the mark on the canvas.</summary>
    [RelayCommand]
    private async Task PersistDateEdit() => await PersistAndRebuildCanvasOnly();

    /// <summary>Below this zoom a month is under ~20px and cannot be aimed at, so a drop can
    /// only honestly claim a year.</summary>
    private const double MonthPickPpy = StripZoomThresholdPpy;   // 240 → a month is 20px
    /// <summary>Below this, a day is under ~3px. Above it, dropping can name a day.</summary>
    private const double DayPickPpy = 1100;                      // → a day is ~3px

    /// <summary>
    /// The date a drop at this fractional year would claim, at the finest precision the current
    /// zoom can actually aim at. This is how within-year order gets decided: at year view you
    /// drag a note out of its cell to a position inside the year, and it lands with a month.
    /// Precision follows what the zoom can resolve — never finer than you could have aimed.
    /// </summary>
    public WorldDatePoint PointAtFractionalYear(double fractionalYear)
    {
        var year = (int)Math.Floor(fractionalYear);
        var frac = fractionalYear - year;
        if (PixelsPerYear < MonthPickPpy) return new WorldDatePoint(year);

        // Epsilon absorbs the precision lost subtracting a ~1000-magnitude year from the
        // fraction; without it a value that IS exactly a boundary floors to the slot below,
        // so a point would not survive a round trip through its own fraction.
        const double eps = 1e-6;
        var month = Math.Clamp((int)Math.Floor(frac * 12 + eps) + 1, 1, 12);
        if (PixelsPerYear < DayPickPpy) return new WorldDatePoint(year, month);

        // Same uniform 12x31 grid WorldDatePoint uses for its own fractions.
        var withinMonth = frac - (month - 1) / 12.0;
        var day = Math.Clamp((int)Math.Floor(withinMonth * 372 + eps) + 1, 1, 31);
        return new WorldDatePoint(year, month, day);
    }

    /// <summary>Where that point actually sits on the axis — so the ghost line snaps to the
    /// position the drop will really take, rather than floating between two months.</summary>
    public double? PixelForPoint(WorldDatePoint point) => _lastAxis?.YOf(point.EarliestFraction);

    /// <summary>Inverts the axis: which fractional year sits at this pixel offset in the body.</summary>
    public double? YearAtPixel(double pixelY)
    {
        if (_lastAxis is not { } axis) return null;
        double lo = axis.MinYear, hi = axis.MaxYear;
        for (var i = 0; i < 48 && hi - lo > 0.001; i++)
        {
            var mid = (lo + hi) / 2;
            if (axis.YOf(mid) < pixelY) lo = mid; else hi = mid;
        }
        return (lo + hi) / 2;
    }

    /// <summary>Which theater column contains this x. Falls back to "(Unplaced)".</summary>
    public int TheaterIdAtX(double x)
    {
        foreach (var (cx, w, id) in _columnRanges)
            if (x >= cx && x < cx + w) return id;
        return 0;
    }

    /// <summary>
    /// Starts a proposed assignment from a drop. Pre-fills a year at the precision the corpus
    /// actually works in — year — and, for a condition, an open-ended interval, since a drop
    /// establishes a start and nothing about an end.
    /// </summary>
    public void BeginDateAssignment(NoteViewModel? note, PlotPoint? plotPoint, TriageRow? triageRow,
        string display, bool isCondition, double year, int theaterId, double px, double py)
    {
        // Pre-fill at whatever precision the zoom could aim at: a year in survey, a month in
        // year view, a day when zoomed past it. This is the point of dropping — deciding
        // where inside 1007 something sits, visually, rather than typing it.
        var point = PointAtFractionalYear(year);
        var proposed = isCondition ? $"{point}.." : point.ToString();
        var theaterName = theaterId == 0
            ? UnplacedTheater.Definition.Name
            : _storyService.Theaters.FirstOrDefault(t => t.Id == theaterId)?.Name ?? "(Unplaced)";

        var assignment = new DateAssignment(this, note, plotPoint, triageRow, display, isCondition,
            proposed, $"dropped at {point} · {theaterName}")
        {
            X = px,
            Y = py
        };
        if (plotPoint is not null)
            assignment.SelectedTheater = TheaterChoices.FirstOrDefault(t => t.Id == theaterId);
        PendingAssignment = assignment;
    }

    // ── Viewport tracking ───────────────────────────────────────────────────────
    private double _lastPpy = double.NaN;
    private YearAxis? _lastAxis;
    private double _centerFrac = double.NaN;
    private double _viewportH = 800;

    public void UpdateViewport(double verticalCenterPx, double viewportHeight)
    {
        _viewportH = viewportHeight;
        if (_lastAxis is { } axis && _lastPpy > 0)
        {
            // Invert the axis approximately: find the year whose YOf is nearest the center.
            var lo = axis.MinYear; var hi = axis.MaxYear;
            for (var i = 0; i < 40 && hi - lo > 0.01; i++)
            {
                var mid = (lo + hi) / 2;
                if (axis.YOf(mid) < verticalCenterPx) lo = mid; else hi = mid;
            }
            _centerFrac = (lo + hi) / 2;
        }
    }

    // ── Snapshot build ──────────────────────────────────────────────────────────

    private sealed record CondEntry(double TopFrac, double? BottomFrac, string Label, string Fill,
        bool Flagged, string Tooltip, object Payload, NoteCard Card, string Detail);
    private sealed record EventEntry(WorldDatePoint At, string Label, string Fill, string Border,
        bool Flagged, bool IsPlotPoint, string Detail, string Body, object Payload, NoteCard Card);

    public void Rebuild()
    {
        Items.Clear();
        HeaderItems.Clear();
        GutterItems.Clear();
        _columnRanges.Clear();
        if (!_storyService.IsProjectLoaded) return;

        // Live note view models, so a card can edit its date through the same validated setter
        // the note view uses rather than a second write path.
        var noteVmById = new Dictionary<int, NoteViewModel>();
        foreach (var nvm in _registry.AllNoteViewModels) noteVmById[nvm.Id] = nvm;

        var trackById = _storyService.NoteTrackDefinitions.ToDictionary(t => t.Id);
        var subjectById = _storyService.Subjects.ToDictionary(s => s.Id);
        var subjectTypeByDefId = _storyService.SubjectDefinitions.ToDictionary(d => d.Id, d => d.SubjectType);
        var chapterById = _storyService.Chapters.ToDictionary(c => c.Id);
        var storyById = _storyService.Stories.ToDictionary(s => s.Id);

        var theaters = _storyService.Theaters.OrderBy(t => t.OrderIndex).ToList();
        theaters.Add(UnplacedTheater.Definition);

        var condByTheater = theaters.ToDictionary(t => t.Id, _ => new List<CondEntry>());
        var eventsByTheater = theaters.ToDictionary(t => t.Id, _ => new List<EventEntry>());
        int shown = 0, undatedSkipped = 0;

        foreach (var note in _storyService.Notes)
        {
            if (note.NoteTrackDefinitionId is not int trackId ||
                !trackById.TryGetValue(trackId, out var track) || !track.SupportsWorldDate)
                continue;
            if (EffectiveDate(note) is not { } date) { undatedSkipped++; continue; }
            if (note.OwnerType != OwnerType.Subject || !subjectById.TryGetValue(note.OwnerId, out var subject))
                continue;

            var theaterId = condByTheater.ContainsKey(subject.TheaterId) ? subject.TheaterId : 0;
            var typeName = subjectTypeByDefId.GetValueOrDefault(subject.SubjectDefinitionId, "?");
            var fill = SubjectTypeFill(typeName);
            var flagged = note.NoteState == NoteState.Flagged;
            // Two shapes of the same provenance: the full one for a card whose title is the
            // subject (the date belongs in the subtitle), and a track-only one for cells, whose
            // title already carries the year.
            var detail = $"{track.TrackName} · {DateLabel(note, track)}{(flagged ? " · ⚑ flagged" : "")}";
            var trackOnly = $"{track.TrackName}{(flagged ? " · ⚑ flagged" : "")}";
            var card = new NoteCard
            {
                Subject = subject.Name, Meta = trackOnly, Content = note.Content,
                FlagReason = note.FlagReason, IsFlagged = flagged, Fill = fill,
                Editable = noteVmById.GetValueOrDefault(note.Id),
                SaveCommand = PersistDateEditCommand
            };

            if (track.SupportsWorldDateEnd)
            {
                if (date.Start is not { } start) continue; // start TBD — no honest top edge; triage carries it
                condByTheater[theaterId].Add(new CondEntry(
                    start.EarliestFraction, date.End?.LatestFraction,
                    subject.Name, fill, flagged,
                    $"{subject.Name} · {detail}", note, card, detail));
            }
            else if (date.Start is { } at)
            {
                eventsByTheater[theaterId].Add(new EventEntry(
                    at, subject.Name, fill, "Transparent", flagged, false,
                    detail, note.Content, note, card));
            }
            shown++;
        }

        foreach (var pp in _storyService.PlotPoints)
        {
            if (pp.GetFabulaDate() is not { } at) continue;
            var theaterId = eventsByTheater.ContainsKey(pp.TheaterId) ? pp.TheaterId : 0;
            var story = pp.ChapterId is int chId && chapterById.TryGetValue(chId, out var ch) &&
                        storyById.TryGetValue(ch.StoryId, out var s) ? s : null;
            var chapterLabel = pp.ChapterId is int cid && chapterById.TryGetValue(cid, out var ch2)
                ? $"{(story?.Abbreviation is { Length: > 0 } ab ? ab : story?.Title ?? "(Unassigned)")} CH#{ch2.OrderIndex}"
                : "(unplaced)";
            var ppMeta = $"PLOT POINT · {chapterLabel} · {at}";
            eventsByTheater[theaterId].Add(new EventEntry(
                at, pp.Title, PlotPointFill,
                story?.ColorHex is { Length: > 0 } hex ? hex : "#666666",
                false, true, ppMeta, "", pp,
                new NoteCard
                {
                    Subject = pp.Title, Meta = ppMeta, Content = "", IsPlotPoint = true, Fill = PlotPointFill
                }));
            shown++;
        }

        // ── Vertical range and axis ──
        var allFracs = condByTheater.Values.SelectMany(l => l)
            .SelectMany(c => new[] { c.TopFrac, c.BottomFrac ?? c.TopFrac })
            .Concat(eventsByTheater.Values.SelectMany(l => l).Select(e => e.At.EarliestFraction))
            .ToList();
        if (allFracs.Count == 0)
        {
            StatusLine = "No dated notes or plot points yet.";
            CanvasWidth = 400; CanvasHeight = 200;
            return;
        }
        var minFrac = Math.Floor(allFracs.Min()) - 1;
        var maxFrac = Math.Ceiling(allFracs.Max()) + 1;
        var ppy = PixelsPerYear;

        var collapsedRanges = BuildEraRows(minFrac, maxFrac)
            .Where(r => _collapsedEras.Contains(r.Key))
            .Select(r => (r.FromYear, r.ToYear))
            .ToList();

        // Body coordinates start at the origin: the year gutter and the theater headers are
        // separate pinned panes now, not reserved space inside the scrolling canvas.
        var axis = YearAxis.Build(minFrac, maxFrac, ppy, collapsedRanges, CollapsedEraH, top: 0);
        double YOf(double frac) => axis.YOf(frac);
        var yearViewMode = ppy >= StripZoomThresholdPpy;

        double x = 0;
        var headerItems = new List<TimelineItem>();
        var bodyItems = new List<TimelineItem>();
        var band = false;
        var collapsedCount = 0;

        foreach (var theater in theaters)
        {
            var conds = condByTheater[theater.Id];
            var evts = eventsByTheater[theater.Id];

            // ── Collapsed column: a narrow strip carrying a density ribbon. Absence stays
            //    visible (the column persists) and busy years still read at a glance.
            if (_collapsedTheaters.Contains(theater.Id))
            {
                collapsedCount++;
                headerItems.Add(new TheaterHeaderItem
                {
                    Name = theater.Name, X = x, Y = 0,
                    Width = CollapsedTheaterW, Height = HeaderH,
                    IsBandShaded = band, IsCollapsed = true, TheaterId = theater.Id,
                    CountLabel = (conds.Count + evts.Count).ToString(),
                    ShortName = theater.Name.Length <= 3 ? theater.Name : theater.Name[..3],
                    Tooltip = $"{theater.Name} — collapsed. {conds.Count} conditions, {evts.Count} events. Click to expand."
                });
                bodyItems.Add(new TheaterColumnItem
                {
                    X = x, Y = 0, Width = CollapsedTheaterW, Height = axis.Height, IsBandShaded = band
                });
                _columnRanges.Add((x, CollapsedTheaterW, theater.Id));
                band = !band;

                var perYear = evts.GroupBy(e => e.At.Year).ToDictionary(g => g.Key, g => g.Count());
                foreach (var c in conds)
                    for (var yr = (int)c.TopFrac; yr < (c.BottomFrac ?? maxFrac); yr++)
                        perYear[yr] = perYear.GetValueOrDefault(yr) + 1;

                var maxCount = perYear.Count > 0 ? perYear.Values.Max() : 1;
                foreach (var (year, count) in perYear)
                {
                    if (axis.IsCollapsedAt(year)) continue;
                    bodyItems.Add(new DensityTickItem
                    {
                        X = x + 4, Y = YOf(year), Width = CollapsedTheaterW - 8, Height = 2,
                        Opacity = 0.25 + 0.75 * (count / (double)maxCount),
                        Tooltip = $"{theater.Name} {FormatYear(year)} — {count} item(s)"
                    });
                }
                x += CollapsedTheaterW;
                continue;
            }

            // ── Conditions: bars in survey mode, per-year in-force strips in year mode ──
            var packConds = yearViewMode ? new List<CondEntry>() : conds;
            var (condLanes, condLaneCount) = LanePacker.Pack(
                packConds.Select(c => YOf(c.TopFrac)).ToList(),
                packConds.Select(c => YOf(c.BottomFrac ?? maxFrac)).ToList());

            // ── Events: group into (theater, year) CELLS. Year precision never unbundles —
            //    zoom cannot reveal resolution that isn't in the data. Finer-precision events
            //    place individually once the zoom actually resolves within-year positions.
            var fineEvents = new List<EventEntry>();
            var cellGroups = new Dictionary<int, List<EventEntry>>();
            foreach (var e in evts)
            {
                if (axis.IsCollapsedAt(e.At.Year)) continue; // folded into the era band
                if (e.At.Month is not null && ppy >= YearGlyphZoomThreshold) fineEvents.Add(e);
                else
                {
                    if (!cellGroups.TryGetValue(e.At.Year, out var list))
                        cellGroups[e.At.Year] = list = [];
                    list.Add(e);
                }
            }

            var eventBoxes = new List<(double Top, double Bottom, object Item)>();
            foreach (var e in fineEvents)
            {
                var centerY = YOf((e.At.EarliestFraction + e.At.LatestFraction) / 2);
                var whiskerTop = YOf(e.At.EarliestFraction);
                var whiskerBottom = YOf(e.At.LatestFraction);
                var hasWhisker = e.At.Day is null;
                var top = Math.Min(centerY - MarkerH / 2, whiskerTop);
                var bottom = Math.Max(centerY + MarkerH / 2, hasWhisker ? whiskerBottom : centerY);
                eventBoxes.Add((top, bottom, (object)(e, centerY, whiskerTop, whiskerBottom, hasWhisker)));
            }
            foreach (var (year, group) in cellGroups)
            {
                var top = YOf(new WorldDatePoint(year).EarliestFraction);
                eventBoxes.Add((top, top + GlyphH, (object)(year, group)));
            }
            var (eventLanes, eventLaneCount) = LanePacker.Pack(
                eventBoxes.Select(b => b.Top).ToList(), eventBoxes.Select(b => b.Bottom).ToList());

            var width = Math.Max(MinTheaterW,
                condLaneCount * CondLaneW + eventLaneCount * EventLaneW + 3 * ColumnPad);

            headerItems.Add(new TheaterHeaderItem
            {
                Name = theater.Name, X = x, Y = 0, Width = width, Height = HeaderH,
                IsBandShaded = band, IsCollapsed = false, TheaterId = theater.Id,
                CountLabel = "", Tooltip = $"{theater.Description}\n\nClick the name to collapse this column."
            });
            bodyItems.Add(new TheaterColumnItem
            {
                X = x, Y = 0, Width = width, Height = axis.Height, IsBandShaded = band
            });
            _columnRanges.Add((x, width, theater.Id));
            band = !band;

            if (!yearViewMode)
            {
                for (var i = 0; i < conds.Count; i++)
                {
                    var c = conds[i];
                    var top = YOf(c.TopFrac);
                    var bottom = YOf(c.BottomFrac ?? maxFrac);
                    var h = Math.Max(bottom - top, 1);
                    bodyItems.Add(new ConditionBarItem
                    {
                        X = x + ColumnPad + condLanes[i] * CondLaneW,
                        Y = top, Width = CondLaneW - 6, Height = h,
                        Label = c.Label, Fill = c.Fill, IsFlagged = c.Flagged,
                        IsOpenEnded = c.BottomFrac is null,
                        IsLabelVisible = h >= 52,
                        Tooltip = c.Tooltip, Payload = c.Payload,
                        // Title names the subject, subtitle carries track + extent, body is the
                        // prose alone — the name is stated once.
                        Card = new CardContent
                        {
                            Title = c.Label, Subtitle = c.Detail, Notes = [c.Card], ShowNoteHeaders = false
                        }
                    });
                }
            }
            else if (conds.Count > 0)
            {
                for (var year = (int)minFrac; year < maxFrac; year++)
                {
                    if (axis.IsCollapsedAt(year)) continue;
                    var inForce = conds.Where(c =>
                        c.TopFrac < year + 1 && (c.BottomFrac ?? maxFrac) > year).ToList();
                    if (inForce.Count == 0) continue;
                    bodyItems.Add(new ConditionStripItem
                    {
                        X = x + ColumnPad, Y = YOf(year) + 16,
                        Width = width - 2 * ColumnPad, Height = 18,
                        Label = "∥ " + string.Join(" · ", inForce.Select(c => c.Label)),
                        Tooltip = $"In force during {FormatYear(year)}:\n" + string.Join("\n",
                            inForce.Select(c => $"  {c.Label}{(c.Flagged ? " ⚑" : "")}"))
                    });
                }
            }

            var eventX0 = x + ColumnPad + condLaneCount * CondLaneW + ColumnPad;
            for (var i = 0; i < eventBoxes.Count; i++)
            {
                var laneX = eventX0 + eventLanes[i] * EventLaneW;
                switch (eventBoxes[i].Item)
                {
                    case (EventEntry e, double centerY, double wTop, double wBottom, bool hasWhisker):
                        if (hasWhisker)
                            bodyItems.Add(new EventWhiskerItem
                            {
                                X = laneX + 4, Y = wTop, Width = 3, Height = Math.Max(wBottom - wTop, 2),
                                Fill = e.Fill, Tooltip = $"{e.Label} · {e.Detail}"
                            });
                        bodyItems.Add(new EventMarkerItem
                        {
                            X = laneX, Y = centerY - MarkerH / 2, Width = EventLaneW - 8, Height = MarkerH,
                            Label = e.Label, Fill = e.Fill, Border = e.Border, IsFlagged = e.Flagged,
                            IsPlotPoint = e.IsPlotPoint,
                            Tooltip = $"{e.Label} · {e.Detail}", Payload = e.Payload,
                            Card = new CardContent
                            {
                                Title = e.Label, Subtitle = e.Detail, Notes = [e.Card], ShowNoteHeaders = false
                            }
                        });
                        break;
                    case (int year, List<EventEntry> group):
                        bodyItems.Add(BuildCell(year, group, laneX, eventBoxes[i].Top, theater.Name));
                        break;
                }
            }

            x += width;
        }

        // ── Collapsed era bands (full width), year ticks, pivots ──
        foreach (var (fromYear, toYear, pixelTop, pixelHeight) in axis.CollapsedBands())
        {
            var inBand = eventsByTheater.Values.SelectMany(l => l)
                .Count(e => e.At.Year >= fromYear && e.At.Year < toYear);
            bodyItems.Add(new EraBandItem
            {
                X = 0, Y = pixelTop, Width = x, Height = pixelHeight, IsFullWidth = true,
                Label = $"⟨ {FormatYear((int)fromYear)} – {FormatYear((int)toYear)} collapsed · {inBand} events not shown ⟩",
                Tooltip = "Collapsed range — expand it in the Eras panel to place these on the axis."
            });
            GutterItems.Add(new YearLabelItem
            {
                X = 0, Y = pixelTop + pixelHeight / 2 - 7, Width = GutterW - 6, Height = 14,
                Label = "⟨⟩", Tooltip = $"{FormatYear((int)fromYear)} – {FormatYear((int)toYear)} collapsed"
            });
        }

        // Rules stay in the scrolling body (they must span every column); their year LABELS go
        // to the pinned gutter, so the date is readable however far right you have scanned.
        var step = new[] { 1, 2, 5, 10, 25, 50, 100, 250, 500 }.FirstOrDefault(s => s * ppy >= 40, 1000);
        for (var year = (int)(Math.Ceiling(minFrac / step) * step); year <= maxFrac; year += step)
        {
            if (axis.IsCollapsedAt(year)) continue;
            bodyItems.Add(new YearTickItem { X = 0, Y = YOf(year), Width = x, Height = 1, Label = "" });
            GutterItems.Add(new YearLabelItem
            {
                X = 0, Y = YOf(year) - 7, Width = GutterW - 6, Height = 14, Label = FormatYear(year)
            });
        }

        foreach (var pivot in _storyService.Pivots.OrderBy(p => p.Year))
        {
            if (axis.IsCollapsedAt(pivot.Year)) continue;
            bodyItems.Add(new PivotLineItem
            {
                X = 0, Y = YOf(pivot.Year), Width = x, Height = 2,
                Label = pivot.Name, Tooltip = pivot.Description
            });
            GutterItems.Add(new YearLabelItem
            {
                X = 0, Y = YOf(pivot.Year) - 7, Width = GutterW - 6, Height = 14,
                Label = FormatYear(pivot.Year), IsPivot = true, Tooltip = pivot.Name
            });
        }

        foreach (var item in headerItems) HeaderItems.Add(item);
        foreach (var item in bodyItems) Items.Add(item);

        CanvasWidth = x + 20;
        CanvasHeight = axis.Height + 30;

        var targetY = double.IsNaN(_centerFrac)
            ? YOf(Math.Max(minFrac, 850)) - 60
            : YOf(Math.Clamp(_centerFrac, minFrac, maxFrac)) - _viewportH / 2;
        _lastAxis = axis;
        _lastPpy = ppy;
        ScrollToY = Math.Max(0, targetY);

        OnPropertyChanged(nameof(IsYearViewMode));
        var collapsedNote = collapsedCount > 0 ? $" · {collapsedCount} collapsed" : "";
        var eraNote = _collapsedEras.Count > 0 ? $" · {_collapsedEras.Count} era(s) collapsed" : "";
        StatusLine = $"{shown} dated items · {theaters.Count} theaters{collapsedNote} · " +
                     $"{FormatYear((int)minFrac + 1)}..{FormatYear((int)maxFrac - 1)}{eraNote} · {ppy:0.#} px/year · " +
                     (yearViewMode ? "year view (in-force strips)" : "survey (extent bars)") +
                     (undatedSkipped > 0 ? $" · {undatedSkipped} undated on dated tracks (see Triage)" : "");
    }

    /// <summary>Builds one (theater, year) cell: count, subject-type composition bar, and the
    /// unordered entry list the detail panel expands.</summary>
    private static EventGlyphItem BuildCell(int year, List<EventEntry> group, double x, double y,
        string theaterName)
    {
        var barWidth = EventLaneW - 20;
        var byFill = group.GroupBy(e => e.Fill).Select(g => (Fill: g.Key, Count: g.Count())).ToList();
        var segments = byFill
            .Select(f => new CompositionSegment { Fill = f.Fill, Width = barWidth * f.Count / group.Count })
            .ToList();

        var entries = group
            .OrderBy(e => e.IsPlotPoint).ThenBy(e => e.Label, StringComparer.OrdinalIgnoreCase)
            .Select(e => new CellEntry
            {
                Label = e.Label, Detail = e.Detail, Fill = e.Fill, Body = e.Body,
                IsFlagged = e.Flagged, IsPlotPoint = e.IsPlotPoint
            })
            .ToList();

        return new EventGlyphItem
        {
            X = x, Y = y, Width = EventLaneW - 8, Height = GlyphH,
            Label = group.Count == 1 ? group[0].Label : $"{group.Count} events",
            YearLabel = FormatYear(year),
            Composition = segments,
            Entries = entries,
            HasFlagged = group.Any(e => e.Flagged),
            // The year and theater live in the title, so each note's line carries only its
            // subject and track — the date is never repeated per note.
            Card = new CardContent
            {
                Title = $"{FormatYear(year)} · {theaterName}",
                Subtitle = group.Count == 1 ? "" : $"{group.Count} items — no order known within the year",
                ShowNoteHeaders = true,
                Notes = group
                    .OrderBy(e => e.IsPlotPoint).ThenBy(e => e.Label, StringComparer.OrdinalIgnoreCase)
                    .Select(e => e.Card).ToList()
            }
        };
    }

    private static string FormatYear(int year) => year < 0 ? $"{-year} BLB" : year.ToString();

    private static WorldDate? EffectiveDate(Note n)
    {
        try { if (n.GetWorldDate() is { } d) return d; }
        catch (ArgumentException) { return null; }
        var outcome = WorldDateLegacy.TryConvert(n.WorldDate, out var legacy);
        return outcome is WorldDateLegacy.Outcome.Point or WorldDateLegacy.Outcome.Range ? legacy : null;
    }

    private static string DateLabel(Note n, NoteTrackDefinition track)
    {
        try { if (n.GetWorldDate() is { } d) return d.ToNotation(d.End is not null || track.SupportsWorldDateEnd); }
        catch (ArgumentException) { }
        return n.WorldDate;
    }

    private const string PlotPointFill = "#C53B3B";
    private static string SubjectTypeFill(string subjectType) => subjectType switch
    {
        "Character" => "#3B6EC5",
        "Bond" => "#C55BA8",
        "Organization" => "#3F9D53",
        "Civilizational System" => "#7E57C2",
        "Technology" => "#E08A2E",
        "World Law" => "#1FA0A8",
        _ => "#888888"
    };

    // ── Side panels ─────────────────────────────────────────────────────────────

    public ObservableCollection<TriageRow> TriageRows { get; } = new();
    public ObservableCollection<PlacementRow> SubjectPlacementRows { get; } = new();
    public ObservableCollection<TheaterRow> TheaterRows { get; } = new();
    public ObservableCollection<EraRow> EraRows { get; } = new();
    public ObservableCollection<Pivot> PivotRows { get; } = new();

    /// <summary>Derived eras clipped to the data's range — N pivots give N+1 eras, so there is
    /// never a gap or an overlap to reconcile.</summary>
    private List<EraRow> BuildEraRows(double minFrac, double maxFrac)
    {
        var rows = new List<EraRow>();
        foreach (var era in Eras.FromPivots(_storyService.Pivots))
        {
            var from = era.StartYear ?? minFrac;
            var to = era.EndYear ?? maxFrac;
            if (to <= minFrac || from >= maxFrac) continue;
            // Key comes from the era's own pivot bounds, never the clipped range — the canvas
            // and the side panel clip to slightly different ranges, and a key that moved with
            // the clip would make a collapse toggle fail to match between them.
            rows.Add(new EraRow(this, era, Math.Max(from, minFrac), Math.Min(to, maxFrac)));
        }
        return rows;
    }

    public void RebuildSidePanels()
    {
        TriageRows.Clear();
        SubjectPlacementRows.Clear();
        TheaterRows.Clear();
        EraRows.Clear();
        PivotRows.Clear();
        if (!_storyService.IsProjectLoaded) return;

        foreach (var t in _storyService.Theaters.OrderBy(t => t.OrderIndex))
            TheaterRows.Add(new TheaterRow(this, t));
        foreach (var p in _storyService.Pivots.OrderBy(p => p.Year)) PivotRows.Add(p);

        var trackById = _storyService.NoteTrackDefinitions.ToDictionary(t => t.Id);
        var subjectById = _storyService.Subjects.ToDictionary(s => s.Id);
        var chapterById = _storyService.Chapters.ToDictionary(c => c.Id);

        var dated = _storyService.Notes
            .Where(n => n.NoteTrackDefinitionId is int tid && trackById.TryGetValue(tid, out var t)
                        && t.SupportsWorldDate && EffectiveDate(n) is not null)
            .Select(n => EffectiveDate(n)!.Value.Start?.Year)
            .Where(y => y is not null).Select(y => y!.Value).ToList();
        var minYear = dated.Count > 0 ? dated.Min() - 1 : 0;
        var maxYear = dated.Count > 0 ? dated.Max() + 1 : 1;
        foreach (var row in BuildEraRows(minYear, maxYear)) EraRows.Add(row);

        foreach (var note in _storyService.Notes)
        {
            if (note.NoteTrackDefinitionId is not int tid || !trackById.TryGetValue(tid, out var track) ||
                !track.SupportsWorldDate || EffectiveDate(note) is not null)
                continue;
            var owner = note.OwnerType == OwnerType.Subject && subjectById.TryGetValue(note.OwnerId, out var s)
                ? s.Name : $"{note.OwnerType}:{note.OwnerId}";
            TriageRows.Add(new TriageRow(this, note, null)
            {
                Display = $"{owner} · {track.TrackName}" +
                          (string.IsNullOrWhiteSpace(note.WorldDate) ? "" : $" · was: \"{note.WorldDate}\""),
                Preview = note.Content,
                IsCondition = track.SupportsWorldDateEnd
            });
        }
        foreach (var pp in _storyService.PlotPoints)
        {
            if (pp.FabulaYear is not null) continue;
            var chapter = pp.ChapterId is int cid && chapterById.TryGetValue(cid, out var ch)
                ? $"CH#{ch.OrderIndex} \"{ch.Title}\"" : "(unplaced)";
            TriageRows.Add(new TriageRow(this, null, pp)
            {
                Display = $"PP · {chapter} · {pp.Title}", Preview = "", IsCondition = false
            });
        }

        var subjectTypeByDefId = _storyService.SubjectDefinitions.ToDictionary(d => d.Id, d => d.SubjectType);
        foreach (var subject in _storyService.Subjects.OrderBy(s => s.TheaterId).ThenBy(s => s.Name))
            SubjectPlacementRows.Add(new PlacementRow(this, subject,
                subjectTypeByDefId.GetValueOrDefault(subject.SubjectDefinitionId, "?")));
    }

    public IEnumerable<Theater> TheaterChoices =>
        new[] { UnplacedTheater.Definition }.Concat(_storyService.Theaters.OrderBy(t => t.OrderIndex));

    internal async Task PersistAndRebuild()
    {
        await _storyService.SaveAsync();
        Rebuild();
        RebuildSidePanels();
        OnPropertyChanged(nameof(TheaterChoices));
    }

    /// <summary>
    /// Save + canvas rebuild WITHOUT reconstructing the side panels. Used by placement picks:
    /// rebuilding ~700 row VMs on every combo selection caused visible lag, and the re-sort
    /// yanked the just-edited row out from under the cursor.
    /// </summary>
    internal async Task PersistAndRebuildCanvasOnly()
    {
        await _storyService.SaveAsync();
        Rebuild();
    }

    // ── Theater management ──────────────────────────────────────────────────────

    [RelayCommand]
    private async Task AddTheater()
    {
        var theater = new Theater
        {
            Name = "New Theater",
            OrderIndex = (_storyService.Theaters.Count > 0 ? _storyService.Theaters.Max(t => t.OrderIndex) : 0) + 1
        };
        _storyService.Theaters.Add(theater);
        await PersistAndRebuild();
    }

    [RelayCommand]
    private async Task DeleteTheater(TheaterRow? row)
    {
        if (row is null) return;
        // Orphan members back to "(Unplaced)" (sentinel 0) — never refuse, never cascade.
        foreach (var s in _storyService.Subjects.Where(s => s.TheaterId == row.Theater.Id)) s.TheaterId = 0;
        foreach (var p in _storyService.PlotPoints.Where(p => p.TheaterId == row.Theater.Id)) p.TheaterId = 0;
        _storyService.Theaters.Remove(row.Theater);
        await PersistAndRebuild();
    }

    [RelayCommand]
    private async Task MoveTheaterUp(TheaterRow? row) => await SwapTheaterOrder(row, -1);

    [RelayCommand]
    private async Task MoveTheaterDown(TheaterRow? row) => await SwapTheaterOrder(row, +1);

    private async Task SwapTheaterOrder(TheaterRow? row, int direction)
    {
        if (row is null) return;
        var ordered = _storyService.Theaters.OrderBy(t => t.OrderIndex).ToList();
        var index = ordered.IndexOf(row.Theater);
        var other = index + direction;
        if (index < 0 || other < 0 || other >= ordered.Count) return;
        (row.Theater.OrderIndex, ordered[other].OrderIndex) = (ordered[other].OrderIndex, row.Theater.OrderIndex);
        await PersistAndRebuild();
    }

    [RelayCommand]
    private async Task SaveEdits() => await PersistAndRebuild();

    // ── Pivot management ────────────────────────────────────────────────────────

    [ObservableProperty] private string _newPivotYearText = "";
    [ObservableProperty] private string _newPivotName = "";

    [RelayCommand]
    private async Task AddPivot()
    {
        if (!int.TryParse(NewPivotYearText.Replace("BLB", "-").Trim(), out var year)) return;
        _storyService.Pivots.Add(new Pivot { Year = year, Name = NewPivotName });
        NewPivotYearText = "";
        NewPivotName = "";
        await PersistAndRebuild();
    }

    [RelayCommand]
    private async Task DeletePivot(Pivot pivot)
    {
        _storyService.Pivots.Remove(pivot);
        await PersistAndRebuild();
    }
}

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
            _ = _owner.PersistAndRebuildCanvasOnly();
        }
    }
}
