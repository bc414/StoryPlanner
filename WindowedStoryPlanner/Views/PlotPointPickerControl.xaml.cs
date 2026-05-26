using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views;

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
        _searchResults = Registry.AllPlotPointViewModels
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
        SearchText = string.Empty;

        PlotPointSelected?.Invoke(plotPoint);
    }

    // ── Constructor ───────────────────────────────────────────────────────

    public PlotPointPickerControl()
    {
        InitializeComponent();
    }
}