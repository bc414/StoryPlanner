using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner;

/// <summary>
/// Story → Chapter → PlotPoint picker. All filtering/search/reset behavior lives in the shared
/// <see cref="ScopedPickerController{TScope,TItem}"/>; this class only adapts it to XAML
/// (property names, combo events, clear-button visibility). Story is the controller's *outer*
/// scope: it narrows the chapter combo, the plot point combo, and the search, and each level is
/// independently clearable, so Story alone ("anywhere in TLTT") is as usable as Story + Chapter.
/// </summary>
public partial class PlotPointPickerControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // ── Raised when the user confirms a plot point selection ──────────────
    public event Action<PlotPointViewModel>? PlotPointSelected;

    private readonly ScopedPickerController<ChapterViewModel, PlotPointViewModel> _picker;

    public PlotPointPickerControl()
    {
        _picker = new ScopedPickerController<ChapterViewModel, PlotPointViewModel>(
            allItems:       () => Registry?.AllPlotPointViewModels ?? Enumerable.Empty<PlotPointViewModel>(),
            belongsToScope: (p, ch) => p.ChapterId == ch.Id,
            searchableText: p => p.Title,
            comboOrder:     items => NarrativeOrder.PlotPoints(Registry, items),
            searchOrder:    items => items.OrderBy(p => p.Title),
            scopedHint:     ch => $"Search {ch.FullNumberAndTitle}'s plot points by title",
            unscopedHint:   "Search plot points by title");
        _picker.StateChanged += OnPickerStateChanged;

        InitializeComponent();
    }

    private void OnPickerStateChanged()
    {
        Notify(nameof(FilteredPlotPoints));
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
            typeof(PlotPointPickerControl),
            new PropertyMetadata(null, OnRegistryChanged));

    public IViewModelRegistry? Registry
    {
        get => (IViewModelRegistry?)GetValue(RegistryProperty);
        set => SetValue(RegistryProperty, value);
    }

    private static void OnRegistryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PlotPointPickerControl ctrl)
        {
            ctrl.Notify(nameof(Stories));
            ctrl.Notify(nameof(Chapters));
            ctrl._picker.Refresh();
        }
    }

    // ── Whether any combo dropdown is currently open ───────────────────────
    // Used by the host window to suppress light-dismiss while dropdowns are open.
    public bool IsAnyComboDropDownOpen =>
        StoryCombo.IsDropDownOpen || ChapterCombo.IsDropDownOpen || PlotPointCombo.IsDropDownOpen;

    // ── XAML-facing projections of the shared controller ──────────────────

    /// <summary>Real stories plus the "(Unassigned)" sentinel — both are legal
    /// <c>Chapter.StoryId</c> values, so both must be pickable. No "(All Stories)" row: the
    /// ✕ button is how you get back to every story.</summary>
    public IEnumerable<StoryOption> Stories =>
        Registry is null
            ? Enumerable.Empty<StoryOption>()
            : StoryOption.BuildTargetList(Registry.AllStoryViewModels);

    /// <summary>The chapters of the selected story, or every chapter when no story is picked —
    /// hence story order first, or two stories' chapter 1s sit next to each other.</summary>
    public IEnumerable<ChapterViewModel> Chapters =>
        Registry is null
            ? Enumerable.Empty<ChapterViewModel>()
            : NarrativeOrder.Chapters(
                Registry,
                Registry.AllChapterViewModels.Where(c => _selectedStory is null || c.StoryId == _selectedStory.Id));

    public IReadOnlyList<PlotPointViewModel> FilteredPlotPoints => _picker.FilteredItems;
    public IReadOnlyList<PlotPointViewModel> SearchResults => _picker.SearchResults;
    public bool HasSearchText => _picker.HasSearchText;
    public string SearchScopeHint => _picker.SearchScopeHint;

    public string SearchText
    {
        get => _picker.SearchText;
        set => _picker.SearchText = value;
    }

    // ── Combo SelectionChanged handlers (no two-way binding) ──────────────

    private StoryOption? _selectedStory;

    private void StoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedStory = StoryCombo.SelectedItem as StoryOption;

        // A plot point is in a story by way of its chapter — there is no StoryId on it. The
        // chapter ids are snapshotted here rather than resolved per item so the filter stays a
        // cheap set lookup; it is retaken every time the story selection changes.
        if (_selectedStory is null)
        {
            _picker.SetOuterScope(null, null);
        }
        else
        {
            var chapterIds = Registry?.AllChapterViewModels
                                      .Where(c => c.StoryId == _selectedStory.Id)
                                      .Select(c => c.Id)
                                      .ToHashSet() ?? [];
            _picker.SetOuterScope(
                p => p.ChapterId is { } id && chapterIds.Contains(id),
                $"Search {_selectedStory.Label}'s plot points by title");
        }

        // The chapter list is now a different set — drop any stale selection from the old story.
        Notify(nameof(Chapters));
        ChapterCombo.SelectedItem = null;   // cascades through ChapterCombo_SelectionChanged
        ClearStoryButton.Visibility = _selectedStory is null ? Visibility.Collapsed : Visibility.Visible;
    }

    private void ClearStoryButton_Click(object sender, RoutedEventArgs e)
    {
        StoryCombo.SelectedItem = null; // cascades through StoryCombo_SelectionChanged
    }

    private void ChapterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _picker.SelectedScope = ChapterCombo.SelectedItem as ChapterViewModel;
        PlotPointCombo.SelectedItem = null;   // clear any stale plot point selection
        ClearChapterButton.Visibility = _picker.SelectedScope is null ? Visibility.Collapsed : Visibility.Visible;
    }

    private void ClearChapterButton_Click(object sender, RoutedEventArgs e)
    {
        ChapterCombo.SelectedItem = null; // cascades through ChapterCombo_SelectionChanged
    }

    private void PlotPointCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PlotPointCombo.SelectedItem is PlotPointViewModel plotPoint)
            CommitSelection(plotPoint);
    }

    private void SearchResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox lb && lb.SelectedItem is PlotPointViewModel plotPoint)
        {
            lb.SelectedItem = null;
            CommitSelection(plotPoint);
        }
    }

    // ── Commit ────────────────────────────────────────────────────────────

    private void CommitSelection(PlotPointViewModel plotPoint)
    {
        // Reset control state so it is clean when next opened
        _picker.Reset();
        _selectedStory               = null;
        StoryCombo.SelectedItem      = null;
        ChapterCombo.SelectedItem    = null;
        PlotPointCombo.SelectedItem  = null;
        ClearStoryButton.Visibility   = Visibility.Collapsed;
        ClearChapterButton.Visibility = Visibility.Collapsed;
        Notify(nameof(Chapters));

        PlotPointSelected?.Invoke(plotPoint);
    }
}
