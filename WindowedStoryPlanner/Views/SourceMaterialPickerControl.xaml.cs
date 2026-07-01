using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views;

public partial class SourceMaterialPickerControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // ── AvailableSourceMaterials DP ─────────────────────────────────────────

    public static readonly DependencyProperty AvailableSourceMaterialsProperty =
        DependencyProperty.Register(
            nameof(AvailableSourceMaterials),
            typeof(IEnumerable<SourceMaterialViewModel>),
            typeof(SourceMaterialPickerControl),
            new PropertyMetadata(null, OnAvailableSourceMaterialsChanged));

    public IEnumerable<SourceMaterialViewModel>? AvailableSourceMaterials
    {
        get => (IEnumerable<SourceMaterialViewModel>?)GetValue(AvailableSourceMaterialsProperty);
        set => SetValue(AvailableSourceMaterialsProperty, value);
    }

    private static void OnAvailableSourceMaterialsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SourceMaterialPickerControl ctrl)
            ctrl.RebuildSearchResults();
    }

    // ── SelectedSourceMaterial DP ────────────────────────────────────────────

    public static readonly DependencyProperty SelectedSourceMaterialProperty =
        DependencyProperty.Register(
            nameof(SelectedSourceMaterial),
            typeof(SourceMaterialViewModel),
            typeof(SourceMaterialPickerControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedSourceMaterialChanged));

    public SourceMaterialViewModel? SelectedSourceMaterial
    {
        get => (SourceMaterialViewModel?)GetValue(SelectedSourceMaterialProperty);
        set => SetValue(SelectedSourceMaterialProperty, value);
    }

    private static void OnSelectedSourceMaterialChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SourceMaterialPickerControl ctrl)
            ctrl.Notify(nameof(SelectedSourceMaterial));
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

    private List<SourceMaterialViewModel> _searchResults = [];
    public IReadOnlyList<SourceMaterialViewModel> SearchResults => _searchResults;

    private void RebuildSearchResults()
    {
        if (AvailableSourceMaterials is null || !HasSearchText)
        {
            _searchResults = [];
            Notify(nameof(SearchResults));
            return;
        }

        var lower = _searchText.Trim().ToLowerInvariant();
        _searchResults = AvailableSourceMaterials
            .Where(s => s.Name.ToLowerInvariant().Contains(lower))
            .OrderBy(s => s.Name)
            .ToList();

        Notify(nameof(SearchResults));
    }

    // ── Selection handlers ───────────────────────────────────────────────────

    private void SearchResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox lb && lb.SelectedItem is SourceMaterialViewModel sourceMaterial)
        {
            lb.SelectedItem = null;
            SearchText = string.Empty;
            SelectedSourceMaterial = sourceMaterial;
        }
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedSourceMaterial = null;
    }

    // ── Constructor ───────────────────────────────────────────────────────

    public SourceMaterialPickerControl()
    {
        InitializeComponent();
    }
}
