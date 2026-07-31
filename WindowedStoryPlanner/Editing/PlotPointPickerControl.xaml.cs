using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner;

public partial class PlotPointPickerControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // ── Raised when the user confirms a plot point selection ──────────────
    public event Action<PlotPointViewModel>? PlotPointSelected;

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
            ctrl.OnRegistryChanged();
    }

    private void OnRegistryChanged()
    {
        Notify(nameof(Chapters));
        RebuildFilteredPlotPoints();
        RebuildSearchResults();
    }

    // ── Whether any combo dropdown is currently open ───────────────────────
    // Used by the host window to suppress light-dismiss while dropdowns are open.
    public bool IsAnyComboDropDownOpen =>
        ChapterCombo.IsDropDownOpen || PlotPointCombo.IsDropDownOpen;

    /// <summary>Tooltip reflecting whatever RebuildSearchResults is currently scoped to — a
    /// Chapter picked in the combo restrains the search box to that chapter's plot points (see
    /// RebuildSearchResults), matching SourceMaterialPickerControl's Work-scoped search.</summary>
    public string SearchScopeHint =>
        _selectedChapter is not null
            ? $"Search {_selectedChapter.FullTitle}'s plot points by title"
            : "Search plot points by title";

    // ── Chapters (chapter combo ItemsSource) ──────────────────────────────

    public IEnumerable<ChapterViewModel> Chapters =>
        Registry?.AllChapterViewModels
                 .OrderBy(c => c.OrderIndex)
        ?? Enumerable.Empty<ChapterViewModel>();

    // ── Filtered plot points (plot point combo ItemsSource) ───────────────

    private ChapterViewModel? _selectedChapter;

    private List<PlotPointViewModel> _filteredPlotPoints = [];
    public IReadOnlyList<PlotPointViewModel> FilteredPlotPoints => _filteredPlotPoints;

    private void RebuildFilteredPlotPoints()
    {
        _filteredPlotPoints = Registry is null
            ? []
            : _selectedChapter is null
                ? Registry.AllPlotPointViewModels.OrderBy(p => p.OrderInChapter).ToList()
                : Registry.AllPlotPointViewModels
                           .Where(p => p.ChapterId == _selectedChapter.Id)
                           .OrderBy(p => p.OrderInChapter)
                           .ToList();

        Notify(nameof(FilteredPlotPoints));
    }

    // ── Search ────────────────────────────────────────────────────────────

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;
            _searchText = value;
            Notify(nameof(SearchText));
            Notify(nameof(HasSearchText));
            RebuildSearchResults();
        }
    }

    public bool HasSearchText => !string.IsNullOrWhiteSpace(_searchText);

    private List<PlotPointViewModel> _searchResults = [];
    public IReadOnlyList<PlotPointViewModel> SearchResults => _searchResults;

    private void RebuildSearchResults()
    {
        if (Registry is null || !HasSearchText)
        {
            _searchResults = [];
            Notify(nameof(SearchResults));
            return;
        }

        var lower = _searchText.Trim().ToLowerInvariant();

        // A Chapter picked in the combo restrains the search to just that chapter's plot
        // points — searching other chapters no longer makes sense once one is already chosen
        // (same enhancement as SourceMaterialPickerControl's Work-scoped Part search).
        var scope = _selectedChapter is null
            ? Registry.AllPlotPointViewModels
            : Registry.AllPlotPointViewModels.Where(p => p.ChapterId == _selectedChapter.Id);

        _searchResults = scope
            .Where(p => p.Title.ToLowerInvariant().Contains(lower))
            .OrderBy(p => p.Title)
            .ToList();

        Notify(nameof(SearchResults));
    }

    // ── Combo SelectionChanged handlers (no two-way binding) ──────────────

    private void ChapterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedChapter = ChapterCombo.SelectedItem as ChapterViewModel;
        RebuildFilteredPlotPoints();
        PlotPointCombo.SelectedItem = null;
        ClearChapterButton.Visibility = _selectedChapter is null ? Visibility.Collapsed : Visibility.Visible;
        Notify(nameof(SearchScopeHint));
        RebuildSearchResults();
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
        _selectedChapter = null;
        RebuildFilteredPlotPoints();
        ChapterCombo.SelectedItem    = null;
        PlotPointCombo.SelectedItem  = null;
        ClearChapterButton.Visibility = Visibility.Collapsed;
        Notify(nameof(SearchScopeHint));
        SearchText = string.Empty;

        PlotPointSelected?.Invoke(plotPoint);
    }

    // ── Constructor ───────────────────────────────────────────────────────

    public PlotPointPickerControl()
    {
        InitializeComponent();
    }
}