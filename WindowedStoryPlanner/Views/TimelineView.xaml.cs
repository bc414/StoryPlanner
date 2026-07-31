using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views;

public partial class TimelineView : UserControl
{
    // Hover popup lifetime. The popup must survive the gap between leaving the mark and
    // entering the popup itself, or it would be impossible to reach — hence the short grace
    // timer rather than closing on MouseLeave directly.
    private readonly DispatcherTimer _closeTimer = new() { Interval = TimeSpan.FromMilliseconds(260) };
    private bool _pointerInPopup;

    // Pinned-card drag state.
    private PinnedCard? _draggingCard;
    private Point _dragOrigin;
    private double _dragStartX, _dragStartY;

    public TimelineView()
    {
        InitializeComponent();
        _closeTimer.Tick += (_, _) =>
        {
            _closeTimer.Stop();
            if (!_pointerInPopup) HoverPopup.IsOpen = false;
        };
        DataContextChanged += (_, _) =>
        {
            if (DataContext is TimelineViewModel vm)
                vm.PropertyChanged += OnVmPropertyChanged;
        };
    }

    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // After a rebuild the VM proposes a vertical position (the dense zone by default) —
        // jump there so the default view isn't the near-empty ancient tail.
        if (e.PropertyName == nameof(TimelineViewModel.ScrollToY) && sender is TimelineViewModel vm)
            CanvasScroller.ScrollToVerticalOffset(vm.ScrollToY);
    }

    // ── Hover popup ─────────────────────────────────────────────────────────────

    /// <summary>Opens the rich card for whichever mark the cursor entered. The card content
    /// lives on the item, so a condition bar shows its one note in full and a cell shows every
    /// note it holds — which is what the bar's rotated label and the cell's "5 events" cannot.</summary>
    private static CardContent? CardOf(object? dataContext) => dataContext switch
    {
        ConditionBarItem c => c.Card,
        EventGlyphItem g => g.Card,
        EventMarkerItem m => m.Card,
        _ => null
    };

    private void Mark_MouseEnter(object sender, MouseEventArgs e)
    {
        if (sender is not FrameworkElement fe) return;
        if (CardOf(fe.DataContext) is not { Notes.Count: > 0 } card) return;

        _closeTimer.Stop();
        HoverPopupBorder.Tag = card;
        HoverPopup.IsOpen = false; // reposition to the new mark
        HoverPopup.IsOpen = true;
    }

    /// <summary>
    /// Clicking a mark promotes its ephemeral hover card into a persistent, draggable one —
    /// the same result as the popup's pin button, without having to travel to it. The card is
    /// placed where the popup already is, so the transition reads as freezing rather than
    /// spawning.
    /// </summary>
    private void Mark_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement fe || DataContext is not TimelineViewModel vm) return;
        if (CardOf(fe.DataContext) is not { Notes.Count: > 0 } card) return;

        double? x = null, y = null;
        if (HoverPopup.IsOpen)
        {
            try
            {
                var onScreen = HoverPopupBorder.PointToScreen(new Point(0, 0));
                var inLayer = PinnedLayer.PointFromScreen(onScreen);
                x = inLayer.X;
                y = inLayer.Y;
            }
            catch (InvalidOperationException) { /* fall through to the cursor position */ }
        }
        if (x is null)
        {
            var p = e.GetPosition(PinnedLayer);
            x = p.X + 14;
            y = p.Y + 10;
        }

        vm.PinCardAt(card, x, y);
        _pointerInPopup = false;
        HoverPopup.IsOpen = false;

        // The side panel still tracks the selection — unchanged behaviour, kept until the
        // panel's future is decided.
        vm.SelectItemCommand.Execute(fe.DataContext);
        e.Handled = true;
    }

    private void Mark_MouseLeave(object sender, MouseEventArgs e)
    {
        _pointerInPopup = false;
        _closeTimer.Start();
    }

    private void HoverPopup_MouseEnter(object sender, MouseEventArgs e)
    {
        _pointerInPopup = true;
        _closeTimer.Stop();
    }

    private void HoverPopup_MouseLeave(object sender, MouseEventArgs e)
    {
        _pointerInPopup = false;
        _closeTimer.Start();
    }

    /// <summary>
    /// Pins the hovered card exactly where the popup is standing. The popup lives in its own
    /// HWND, so its position has to travel through screen coordinates to reach the pinned
    /// layer's coordinate space; the popup then closes, and the pinned card takes its place
    /// with no visible jump.
    /// </summary>
    private void PinPopup_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not TimelineViewModel vm) return;
        if (HoverPopupBorder.Tag is not CardContent card) return;

        double? x = null, y = null;
        try
        {
            var onScreen = HoverPopupBorder.PointToScreen(new Point(0, 0));
            var inLayer = PinnedLayer.PointFromScreen(onScreen);
            x = inLayer.X;
            y = inLayer.Y;
        }
        catch (InvalidOperationException)
        {
            // No visual root yet — fall back to the cascade position.
        }

        vm.PinCardAt(card, x, y);
        _pointerInPopup = false;
        HoverPopup.IsOpen = false;
    }

    // ── Drag-to-date ────────────────────────────────────────────────────────────
    // Two sources — a triage row (undated) and a card's knob (re-dating) — one drop target.
    // The drop only PROPOSES: it pre-fills the confirm popup and writes nothing by itself.

    private Point _dateDragStart;
    private object? _dateDragPayload;

    private void TriageKnob_DragStart(object sender, MouseButtonEventArgs e) => BeginDateDrag(sender, e);
    private void CardKnob_DragStart(object sender, MouseButtonEventArgs e) => BeginDateDrag(sender, e);

    private void BeginDateDrag(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement fe) return;
        _dateDragStart = e.GetPosition(null);
        _dateDragPayload = fe.DataContext;
    }

    private void TriageKnob_DragMove(object sender, MouseEventArgs e) => ContinueDateDrag(sender, e);
    private void CardKnob_DragMove(object sender, MouseEventArgs e) => ContinueDateDrag(sender, e);

    private void ContinueDateDrag(object sender, MouseEventArgs e)
    {
        if (_dateDragPayload is null || e.LeftButton != MouseButtonState.Pressed) return;
        var now = e.GetPosition(null);
        if (Math.Abs(now.X - _dateDragStart.X) < SystemParameters.MinimumHorizontalDragDistance &&
            Math.Abs(now.Y - _dateDragStart.Y) < SystemParameters.MinimumVerticalDragDistance)
            return;

        var payload = _dateDragPayload;
        _dateDragPayload = null;
        _pointerInPopup = false;
        HoverPopup.IsOpen = false; // the hover card would otherwise sit under the drag
        if (sender is DependencyObject src)
            DragDrop.DoDragDrop(src, new DataObject(typeof(object), payload), DragDropEffects.Move);
        if (DataContext is TimelineViewModel vm) vm.EndDragPreview();
    }

    private void Canvas_DragOver(object sender, DragEventArgs e)
    {
        if (DataContext is not TimelineViewModel vm) return;
        e.Effects = DragDropEffects.Move;
        e.Handled = true;
        var body = e.GetPosition(BodyItems);
        var overlay = e.GetPosition(GhostLayer);
        vm.UpdateDragPreview(body.X, body.Y, overlay.X, overlay.Y);
    }

    private void Canvas_DragLeave(object sender, DragEventArgs e)
    {
        if (DataContext is TimelineViewModel vm) vm.EndDragPreview();
    }

    private void Canvas_Drop(object sender, DragEventArgs e)
    {
        if (DataContext is not TimelineViewModel vm) return;
        vm.EndDragPreview();

        var payload = e.Data.GetData(typeof(object));
        var body = e.GetPosition(BodyItems);
        if (vm.YearAtPixel(body.Y) is not { } year) return;
        var theaterId = vm.TheaterIdAtX(body.X);
        var at = e.GetPosition(this);

        switch (payload)
        {
            case TriageRow row:
                vm.BeginDateAssignment(
                    row.Note is null ? null : NoteVmFor(row), row.PlotPoint, row,
                    row.Display, row.IsCondition, year, theaterId, at.X, at.Y);
                break;
            case NoteCard card when card.Editable is { } editable:
                vm.BeginDateAssignment(
                    editable, null, null,
                    card.Subject, editable.SupportsWorldDateEnd, year, theaterId, at.X, at.Y);
                break;
        }
        e.Handled = true;
    }

    /// <summary>Triage rows hold the note model; the confirm popup needs its view model so the
    /// write goes through the one validated setter.</summary>
    private NoteViewModel? NoteVmFor(TriageRow row)
    {
        if (row.Note is null || DataContext is not TimelineViewModel vm) return null;
        return vm.NoteViewModelFor(row.Note.Id);
    }

    // ── Pinned card dragging ────────────────────────────────────────────────────

    private void PinnedCard_DragStart(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement fe || fe.DataContext is not PinnedCard card) return;
        _draggingCard = card;
        _dragOrigin = e.GetPosition(PinnedLayer);
        _dragStartX = card.X;
        _dragStartY = card.Y;
        fe.CaptureMouse();
        e.Handled = true;
    }

    private void PinnedCard_Drag(object sender, MouseEventArgs e)
    {
        if (_draggingCard is null || e.LeftButton != MouseButtonState.Pressed) return;
        var now = e.GetPosition(PinnedLayer);
        _draggingCard.X = Math.Max(0, _dragStartX + (now.X - _dragOrigin.X));
        _draggingCard.Y = Math.Max(0, _dragStartY + (now.Y - _dragOrigin.Y));
    }

    private void PinnedCard_DragEnd(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement fe) fe.ReleaseMouseCapture();
        _draggingCard = null;
    }

    /// <summary>Compact date box on a card: the setter already validated and wrote, so this
    /// just persists and re-places the mark.</summary>
    private void CardDate_LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: NoteCard { Editable: not null } card })
            card.SaveCommand?.Execute(null);
    }

    // ── Scrolling ───────────────────────────────────────────────────────────────

    private void CanvasScroller_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control || DataContext is not TimelineViewModel vm)
            return;
        if (e.Delta > 0) vm.ZoomInCommand.Execute(null);
        else vm.ZoomOutCommand.Execute(null);
        e.Handled = true;
    }

    private void CanvasScroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        // The body is the single scroll authority; the pinned panes follow it on one axis each.
        HeaderScroller.ScrollToHorizontalOffset(e.HorizontalOffset);
        GutterScroller.ScrollToVerticalOffset(e.VerticalOffset);

        // A popup anchored to a mark would drift away from it while scrolling.
        if (HoverPopup.IsOpen && !_pointerInPopup) HoverPopup.IsOpen = false;

        // Keep the VM's idea of the viewport current so zoom/mode jumps preserve the year
        // the user is looking at instead of resetting to the dense-zone default.
        if (DataContext is TimelineViewModel vm)
            vm.UpdateViewport(e.VerticalOffset + e.ViewportHeight / 2, e.ViewportHeight);
    }
}
