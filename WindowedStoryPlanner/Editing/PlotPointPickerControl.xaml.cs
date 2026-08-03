using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner;

/// <summary>
/// Chapter → PlotPoint picker. All filtering/search/reset behavior lives in the shared
/// <see cref="ScopedPickerController{TScope,TItem}"/>; this class only adapts it to XAML
/// (property names, combo events, clear-button visibility).
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
            comboOrder:     items => items.OrderBy(p => p.OrderInChapter),
            searchOrder:    items => items.OrderBy(p => p.Title),
            scopedHint:     ch => $"Search {ch.FullTitle}'s plot points by title",
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
            ctrl.Notify(nameof(Chapters));
            ctrl._picker.Refresh();
        }
    }

    // ── Whether any combo dropdown is currently open ───────────────────────
    // Used by the host window to suppress light-dismiss while dropdowns are open.
    public bool IsAnyComboDropDownOpen =>
        ChapterCombo.IsDropDownOpen || PlotPointCombo.IsDropDownOpen;

    // ── XAML-facing projections of the shared controller ──────────────────

    public IEnumerable<ChapterViewModel> Chapters =>
        Registry?.AllChapterViewModels
                 .OrderBy(c => c.OrderIndex)
        ?? Enumerable.Empty<ChapterViewModel>();

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
        ChapterCombo.SelectedItem    = null;
        PlotPointCombo.SelectedItem  = null;
        ClearChapterButton.Visibility = Visibility.Collapsed;

        PlotPointSelected?.Invoke(plotPoint);
    }
}
