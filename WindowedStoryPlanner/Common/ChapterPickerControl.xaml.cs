using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner;

/// <summary>
/// Story → Chapter picker — the first two levels of <see cref="PlotPointPickerControl"/>, stopping
/// at the chapter. All filtering/search/reset behavior lives in the shared
/// <see cref="ScopedPickerController{TScope,TItem}"/>; this class only adapts it to XAML
/// (property names, combo events, clear-button visibility). Built 2026-08-06 for ChapterWindow's
/// "change chapter" flyout, which is why it commits on selection like the other two rather than
/// exposing a bindable SelectedChapter.
/// </summary>
public partial class ChapterPickerControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // ── Raised when the user confirms a chapter selection ─────────────────
    public event Action<ChapterViewModel>? ChapterSelected;

    private readonly ScopedPickerController<StoryOption, ChapterViewModel> _picker;

    public ChapterPickerControl()
    {
        _picker = new ScopedPickerController<StoryOption, ChapterViewModel>(
            allItems:       () => Registry?.AllChapterViewModels ?? Enumerable.Empty<ChapterViewModel>(),
            belongsToScope: (c, story) => c.StoryId == story.Id,
            searchableText: c => c.Title,
            comboOrder:     items => NarrativeOrder.Chapters(Registry, items),
            searchOrder:    items => NarrativeOrder.Chapters(Registry, items),
            scopedHint:     story => $"Search {story.Label}'s chapters by title",
            unscopedHint:   "Search chapters by title");
        _picker.StateChanged += OnPickerStateChanged;

        InitializeComponent();
    }

    private void OnPickerStateChanged()
    {
        Notify(nameof(FilteredChapters));
        Notify(nameof(SearchResults));
        Notify(nameof(SearchText));
        Notify(nameof(HasSearchText));
        Notify(nameof(SearchScopeHint));
    }

    // ── Registry DP ───────────────────────────────────────────────────────

    public static readonly DependencyProperty RegistryProperty =
        DependencyProperty.Register(
            nameof(Registry),
            typeof(IViewModelRegistry),
            typeof(ChapterPickerControl),
            new PropertyMetadata(null, OnRegistryChanged));

    public IViewModelRegistry? Registry
    {
        get => (IViewModelRegistry?)GetValue(RegistryProperty);
        set => SetValue(RegistryProperty, value);
    }

    private static void OnRegistryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ChapterPickerControl ctrl)
        {
            ctrl.Notify(nameof(Stories));
            ctrl._picker.Refresh();
        }
    }

    // ── XAML-facing projections of the shared controller ──────────────────

    /// <summary>Real stories plus the "(Unassigned)" sentinel — both are legal
    /// <c>Chapter.StoryId</c> values, so both must be pickable. No "(All Stories)" row: the
    /// ✕ button is how you get back to every story.</summary>
    public IEnumerable<StoryOption> Stories =>
        Registry is null
            ? Enumerable.Empty<StoryOption>()
            : StoryOption.BuildTargetList(Registry.AllStoryViewModels);

    public IReadOnlyList<ChapterViewModel> FilteredChapters => _picker.FilteredItems;
    public IReadOnlyList<ChapterViewModel> SearchResults => _picker.SearchResults;
    public bool HasSearchText => _picker.HasSearchText;
    public string SearchScopeHint => _picker.SearchScopeHint;

    public string SearchText
    {
        get => _picker.SearchText;
        set => _picker.SearchText = value;
    }

    // ── Combo SelectionChanged handlers (no two-way binding) ──────────────

    private void StoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _picker.SelectedScope = StoryCombo.SelectedItem as StoryOption;
        ChapterCombo.SelectedItem = null;   // clear any stale chapter selection
        ClearStoryButton.Visibility = _picker.SelectedScope is null ? Visibility.Collapsed : Visibility.Visible;
    }

    private void ClearStoryButton_Click(object sender, RoutedEventArgs e)
    {
        StoryCombo.SelectedItem = null; // cascades through StoryCombo_SelectionChanged
    }

    private void ChapterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ChapterCombo.SelectedItem is ChapterViewModel chapter)
            CommitSelection(chapter);
    }

    private void SearchResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox lb && lb.SelectedItem is ChapterViewModel chapter)
        {
            lb.SelectedItem = null;
            CommitSelection(chapter);
        }
    }

    // ── Commit ────────────────────────────────────────────────────────────

    private void CommitSelection(ChapterViewModel chapter)
    {
        // Reset control state so it is clean when next opened
        _picker.Reset();
        StoryCombo.SelectedItem   = null;
        ChapterCombo.SelectedItem = null;
        ClearStoryButton.Visibility = Visibility.Collapsed;

        ChapterSelected?.Invoke(chapter);
    }
}
