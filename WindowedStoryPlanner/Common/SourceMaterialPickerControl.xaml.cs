using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WindowedStoryPlanner;

/// <summary>
/// Multi-chip source-material citation picker. Chips are the note's existing
/// NoteSourceReferences (rendered from Note.SourceReferences); the "+" opens a popup offering a
/// combined Work/Part search plus an inline quick-add form. Follows WorldDatePickerControl's
/// shape — a Note DP pointing at the whole NoteViewModel, so the control can host outside
/// NoteView too — rather than a plain value DP, because add/remove/create are all mutations
/// that belong on the view model, not on this control.
/// </summary>
public partial class SourceMaterialPickerControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // ── Note DP ──────────────────────────────────────────────────────────────

    public static readonly DependencyProperty NoteProperty = DependencyProperty.Register(
        nameof(Note), typeof(NoteViewModel), typeof(SourceMaterialPickerControl), new PropertyMetadata(null));

    public NoteViewModel? Note
    {
        get => (NoteViewModel?)GetValue(NoteProperty);
        set => SetValue(NoteProperty, value);
    }

    private NoteViewModel? Vm => Note ?? DataContext as NoteViewModel;

    // ── Search results ───────────────────────────────────────────────────────

    private sealed class SearchResult
    {
        public required SourceMaterialViewModel Work { get; init; }
        public SourceMaterialPartViewModel? Part { get; init; }
        public required string Label { get; init; }
    }

    private List<SearchResult> _results = [];
    public bool HasResults => _results.Count > 0;

    /// <summary>Tooltip reflecting whatever RebuildResults is currently scoped to — a Work
    /// picked in the combo restrains the search box to that Work's Parts (see RebuildResults),
    /// so the hint has to say so rather than always claiming "Works and Parts".</summary>
    public string SearchScopeHint =>
        PickWorkCombo?.SelectedItem is SourceMaterialViewModel work
            ? $"Search {work.Name}'s Parts by name/code"
            : "Search Works and Parts by name/code";

    // ── Cascading Work → Part combos ─────────────────────────────────────────
    // The same picker shape as SubjectPickerControl's Type → Subject pair (plain, non-editable
    // combos; a selection commits immediately; the control resets itself afterward) adapted for
    // the one place the hierarchy differs: a Part is optional here. Rather than requiring one
    // before anything can commit, the Part combo's own item list always carries a synthetic
    // "cite the Work itself" entry alongside the Work's real Parts, so selecting *either* kind
    // of entry in the second combo commits — same commit-on-second-selection shape, just with
    // a null-Part item folded into the list instead of a separate escape hatch.

    private sealed class PartComboItem
    {
        public SourceMaterialPartViewModel? Part { get; init; }
        public required string Label { get; init; }
    }

    private void PickWorkCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PickWorkCombo.SelectedItem is not SourceMaterialViewModel work || Vm is null)
        {
            PickPartCombo.ItemsSource = null;
            PickPartCombo.IsEnabled = false;
            ClearWorkButton.Visibility = Visibility.Collapsed;
            Notify(nameof(SearchScopeHint));
            RebuildResults();
            return;
        }

        var items = new List<PartComboItem> { new() { Part = null, Label = "(cite the work itself)" } };
        items.AddRange(Vm.AvailableSourceMaterialParts
            .Where(p => p.SourceMaterialId == work.Id)
            .OrderBy(p => p.OrderIndex)
            .Select(p => new PartComboItem { Part = p, Label = p.DisplayLabel }));

        PickPartCombo.ItemsSource = items;
        PickPartCombo.SelectedItem = null;
        PickPartCombo.IsEnabled = true;

        // Picking a Work here also restrains the free-text search below to that Work's Parts
        // (see RebuildResults) — searching a specific fanfic's ~120 chapters by name is a lot
        // faster than either scrolling the Part combo or re-searching the whole 465-part corpus.
        ClearWorkButton.Visibility = Visibility.Visible;
        Notify(nameof(SearchScopeHint));
        RebuildResults();
    }

    private void ClearWorkButton_Click(object sender, RoutedEventArgs e)
    {
        PickWorkCombo.SelectedItem = null; // cascades through PickWorkCombo_SelectionChanged
    }

    private void PickPartCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PickPartCombo.SelectedItem is not PartComboItem item ||
            PickWorkCombo.SelectedItem is not SourceMaterialViewModel work || Vm is null)
            return;

        Vm.AddSourceReference(work, item.Part);
        OpenToggle.IsChecked = false;
    }

    // ── Popup lifecycle ──────────────────────────────────────────────────────

    private void Popup_Opened(object sender, System.EventArgs e)
    {
        SearchBox.Text = string.Empty;
        AddNewForm.Visibility = Visibility.Collapsed;
        WorkCombo.SelectedItem = null;
        WorkCombo.Text = string.Empty;
        PartCodeBox.Text = string.Empty;
        PartNameBox.Text = string.Empty;
        PickWorkCombo.SelectedItem = null;
        PickPartCombo.ItemsSource = null;
        PickPartCombo.IsEnabled = false;
        ClearWorkButton.Visibility = Visibility.Collapsed;
        Notify(nameof(SearchScopeHint));
        RebuildResults();
        SearchBox.Focus();
        HookWindowWheel();
    }

    private void Popup_Closed(object sender, System.EventArgs e) => UnhookWindowWheel();

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => RebuildResults();

    private void RebuildResults()
    {
        var query = SearchBox.Text.Trim();
        var scopeWork = PickWorkCombo.SelectedItem as SourceMaterialViewModel;

        if (Vm is null || query.Length == 0)
        {
            _results = [];
        }
        else
        {
            var lower = query.ToLowerInvariant();

            var partsInScope = scopeWork is null
                ? Vm.AvailableSourceMaterialParts
                : Vm.AvailableSourceMaterialParts.Where(p => p.SourceMaterialId == scopeWork.Id);

            var partHits = partsInScope
                .Where(p => p.Code.ToLowerInvariant().Contains(lower) || p.Name.ToLowerInvariant().Contains(lower))
                .OrderBy(p => p.OrderIndex)
                .Select(p =>
                {
                    var work = scopeWork ?? Vm.AvailableSourceMaterials.FirstOrDefault(w => w.Id == p.SourceMaterialId);
                    return work is null
                        ? null
                        : new SearchResult { Work = work, Part = p, Label = scopeWork is null ? $"{work.Name} · {p.DisplayLabel}" : p.DisplayLabel };
                })
                .OfType<SearchResult>();

            // A Work picked in the combo restrains the search to just its Parts — searching
            // other Works no longer makes sense once one is already chosen.
            if (scopeWork is not null)
            {
                _results = partHits.ToList();
            }
            else
            {
                var workHits = Vm.AvailableSourceMaterials
                    .Where(w => w.Name.ToLowerInvariant().Contains(lower))
                    .OrderBy(w => w.Name)
                    .Select(w => new SearchResult { Work = w, Part = null, Label = w.Name });

                _results = workHits.Concat(partHits).ToList();
            }
        }

        ResultsList.ItemsSource = _results;
        Notify(nameof(HasResults));

        // Pre-fill the quick-add Part code with whatever was typed — the common quick-add case
        // is "this episode isn't in the seeded list yet", where the search text already IS the
        // code the author wants. Only while the quick-add form is CLOSED, though: once it's open
        // the author is editing that box directly, and echoing the search text into it would
        // overwrite what they typed on every keystroke elsewhere.
        if (AddNewForm.Visibility != Visibility.Visible)
            PartCodeBox.Text = query;
    }

    private void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ResultsList.SelectedItem is not SearchResult result || Vm is null) return;
        ResultsList.SelectedItem = null;
        Vm.AddSourceReference(result.Work, result.Part);
        OpenToggle.IsChecked = false;
    }

    private void RemoveChip_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: NoteSourceReferenceViewModel reference } || Vm is null) return;
        Vm.RemoveSourceReference(reference);
    }

    // ── Quick add ────────────────────────────────────────────────────────────

    private void ShowAddNewButton_Click(object sender, RoutedEventArgs e)
    {
        AddNewForm.Visibility = AddNewForm.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        if (AddNewForm.Visibility == Visibility.Visible && WorkCombo.Text.Length == 0 && SearchBox.Text.Length > 0)
        {
            // If the search text matches an existing Work by name, default to it rather than
            // proposing a duplicate Work.
            var existing = Vm?.AvailableSourceMaterials
                .FirstOrDefault(w => string.Equals(w.Name, SearchBox.Text.Trim(), System.StringComparison.OrdinalIgnoreCase));
            if (existing is not null) WorkCombo.SelectedItem = existing;
        }
    }

    private void CancelAddNew_Click(object sender, RoutedEventArgs e)
    {
        AddNewForm.Visibility = Visibility.Collapsed;
    }

    // async void is the standard shape for a WPF event handler; the awaits matter because
    // creating a Work/Part must complete its save before the new row's id is read (see
    // NoteViewModel.CreateSourceMaterialAsync). The button is disabled across the await so a
    // double-click can't create the same Work twice.
    private async void ConfirmAddNew_Click(object sender, RoutedEventArgs e)
    {
        if (Vm is null) return;
        if (sender is not Button confirmButton) return;

        confirmButton.IsEnabled = false;
        try
        {
            SourceMaterialViewModel? work = WorkCombo.SelectedItem as SourceMaterialViewModel;
            if (work is null)
            {
                var typed = WorkCombo.Text.Trim();
                if (typed.Length == 0) return; // a citation needs at least a Work

                work = Vm.AvailableSourceMaterials
                    .FirstOrDefault(w => string.Equals(w.Name, typed, StringComparison.OrdinalIgnoreCase));
                work ??= await Vm.CreateSourceMaterialAsync(typed);
            }

            var code = PartCodeBox.Text.Trim();
            SourceMaterialPartViewModel? part = null;
            if (code.Length > 0)
            {
                // Reuse an existing Part under this Work with the same code rather than duplicating it.
                part = Vm.AvailableSourceMaterialParts
                    .FirstOrDefault(p => p.SourceMaterialId == work.Id &&
                                          string.Equals(p.Code, code, StringComparison.OrdinalIgnoreCase));
                part ??= await Vm.CreateSourceMaterialPartAsync(work, code, PartNameBox.Text.Trim());
            }

            Vm.AddSourceReference(work, part);
            OpenToggle.IsChecked = false;
        }
        finally
        {
            confirmButton.IsEnabled = true;
        }
    }

    private static ScrollViewer? FindScrollViewer(DependencyObject? node)
    {
        if (node is null) return null;
        if (node is ScrollViewer sv) return sv;
        var count = VisualTreeHelper.GetChildrenCount(node);
        for (var i = 0; i < count; i++)
        {
            if (FindScrollViewer(VisualTreeHelper.GetChild(node, i)) is { } found) return found;
        }
        return null;
    }

    // ── Wheel forwarding for everything inside this popup ────────────────────
    // Windows delivers WM_MOUSEWHEEL to the FOCUSED window, not the window under the cursor. A
    // Popup — this control's, and each ComboBox dropdown's — is a non-activating HWND that never
    // takes focus, so every wheel notch arrives at the hosting Window and is routed within ITS
    // element tree, a tree that contains no popup content at all. Nothing inside the popup can
    // receive MouseWheel by construction: that's why a handler on the results ListBox, one on
    // the dropdown's ScrollViewer, and one on the ComboBox itself all failed to fire.
    //
    // So listen where the event demonstrably does arrive — the Window — and forward it to
    // whichever surface should scroll: an open dropdown first (it visually covers everything
    // else), otherwise the results list when the pointer is over it. handledEventsToo:true
    // because the main window's own ScrollViewer may already have marked it handled. Hooked only
    // while our popup is open.

    private Window? _wheelHost;

    private void HookWindowWheel()
    {
        _wheelHost = Window.GetWindow(this);
        _wheelHost?.AddHandler(Mouse.PreviewMouseWheelEvent,
            new MouseWheelEventHandler(OnWindowPreviewMouseWheel), handledEventsToo: true);
    }

    private void UnhookWindowWheel()
    {
        _wheelHost?.RemoveHandler(Mouse.PreviewMouseWheelEvent,
            new MouseWheelEventHandler(OnWindowPreviewMouseWheel));
        _wheelHost = null;
    }

    private void OnWindowPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var target = OpenDropdownScrollViewer()
                     ?? (ResultsList.IsMouseOver ? FindScrollViewer(ResultsList) : null);
        if (target is null) return;

        ScrollByWheelNotches(target, e.Delta);
        e.Handled = true;
    }

    /// <summary>
    /// Scrolls one wheel notch's worth, in the ScrollViewer's OWN unit. Deliberately not
    /// ScrollToVerticalOffset(VerticalOffset - Delta): an ItemsControl scrolls logically by
    /// default (CanContentScroll=true), so VerticalOffset counts ITEMS, not pixels — feeding it
    /// the raw delta of 120 per notch jumps 120 items at a time. LineUp/LineDown move exactly
    /// one unit in whichever mode the ScrollViewer is actually in (one item when logical, ~one
    /// text line when pixel-based), which is what the OS wheel setting is expressed in too.
    /// </summary>
    private static void ScrollByWheelNotches(ScrollViewer sv, int delta)
    {
        if (delta == 0) return;

        var notches = Math.Abs(delta) / (double)Mouse.MouseWheelDeltaForOneLine;
        var linesPerNotch = SystemParameters.WheelScrollLines; // OS setting; -1 means "a page"
        var scrollUp = delta > 0;

        if (linesPerNotch < 0)
        {
            for (var i = 0; i < Math.Max(1, (int)Math.Round(notches)); i++)
            {
                if (scrollUp) sv.PageUp(); else sv.PageDown();
            }
            return;
        }

        var lines = Math.Max(1, (int)Math.Round(notches * linesPerNotch));
        for (var i = 0; i < lines; i++)
        {
            if (scrollUp) sv.LineUp(); else sv.LineDown();
        }
    }

    private ScrollViewer? OpenDropdownScrollViewer()
    {
        foreach (var combo in new[] { PickWorkCombo, PickPartCombo, WorkCombo })
        {
            if (!combo.IsDropDownOpen) continue;
            if (combo.Template?.FindName("PART_Popup", combo) is not Popup dropdown) continue;
            if (FindScrollViewer(dropdown.Child) is { } sv) return sv;
        }
        return null;
    }

    // ── Constructor ───────────────────────────────────────────────────────

    public SourceMaterialPickerControl()
    {
        InitializeComponent();
    }
}
