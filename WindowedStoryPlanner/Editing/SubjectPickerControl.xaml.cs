using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner;

public partial class SubjectPickerControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // ── Raised when the user confirms a subject selection ─────────────────
    public event Action<SubjectViewModel>? SubjectSelected;

    // ── Registry DP ───────────────────────────────────────────────────────

    public static readonly DependencyProperty RegistryProperty =
        DependencyProperty.Register(
            nameof(Registry),
            typeof(IViewModelRegistry),
            typeof(SubjectPickerControl),
            new PropertyMetadata(null, OnRegistryChanged));

    public IViewModelRegistry? Registry
    {
        get => (IViewModelRegistry?)GetValue(RegistryProperty);
        set => SetValue(RegistryProperty, value);
    }

    private static void OnRegistryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SubjectPickerControl ctrl)
            ctrl.OnRegistryChanged();
    }

    private void OnRegistryChanged()
    {
        Notify(nameof(SubjectDefinitions));
        RebuildFilteredSubjects();
        RebuildSearchResults();
    }

    // ── Whether any combo dropdown is currently open ───────────────────────
    // Used by the host window to suppress light-dismiss while dropdowns are open.
    public bool IsAnyComboDropDownOpen =>
        TypeCombo.IsDropDownOpen || SubjectCombo.IsDropDownOpen;

    /// <summary>Tooltip reflecting whatever RebuildSearchResults is currently scoped to — a
    /// Type picked in the combo restrains the search box to that type's subjects (see
    /// RebuildSearchResults), matching SourceMaterialPickerControl's Work-scoped search.</summary>
    public string SearchScopeHint =>
        _selectedDefinition is not null
            ? $"Search {_selectedDefinition.SubjectType} subjects by name"
            : "Search subjects by name";

    // ── Subject definitions (type combo) ──────────────────────────────────

    public IEnumerable<SubjectDefinitionViewModel> SubjectDefinitions =>
        Registry?.AllSubjectDefinitionViewModels
                 .OrderBy(d => d.DisplayOrder)
        ?? Enumerable.Empty<SubjectDefinitionViewModel>();

    // ── Filtered subjects (subject combo ItemsSource) ─────────────────────

    private SubjectDefinitionViewModel? _selectedDefinition;

    private List<SubjectViewModel> _filteredSubjects = [];
    public IReadOnlyList<SubjectViewModel> FilteredSubjects => _filteredSubjects;

    private void RebuildFilteredSubjects()
    {
        _filteredSubjects = Registry is null
            ? []
            : _selectedDefinition is null
                ? Registry.AllSubjectViewModels.OrderBy(s => s.Name).ToList()
                : Registry.AllSubjectViewModels
                           .Where(s => s.SubjectDefinitionId == _selectedDefinition.Id)
                           .OrderBy(s => s.Name)
                           .ToList();

        Notify(nameof(FilteredSubjects));
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

    private List<SubjectViewModel> _searchResults = [];
    public IReadOnlyList<SubjectViewModel> SearchResults => _searchResults;

    private void RebuildSearchResults()
    {
        if (Registry is null || !HasSearchText)
        {
            _searchResults = [];
            Notify(nameof(SearchResults));
            return;
        }

        var lower = _searchText.Trim().ToLowerInvariant();

        // A Type picked in the combo restrains the search to just that type's subjects —
        // searching other types no longer makes sense once one is already chosen (same
        // enhancement as SourceMaterialPickerControl's Work-scoped Part search).
        var scope = _selectedDefinition is null
            ? Registry.AllSubjectViewModels
            : Registry.AllSubjectViewModels.Where(s => s.SubjectDefinitionId == _selectedDefinition.Id);

        _searchResults = scope
            .Where(s => s.Name.ToLowerInvariant().Contains(lower))
            .OrderBy(s => s.Name)
            .ToList();

        Notify(nameof(SearchResults));
    }

    // ── Combo SelectionChanged handlers (no two-way binding) ──────────────

    private void TypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedDefinition = TypeCombo.SelectedItem as SubjectDefinitionViewModel;
        // Rebuild the subject list and clear any stale subject selection
        RebuildFilteredSubjects();
        SubjectCombo.SelectedItem = null;
        ClearTypeButton.Visibility = _selectedDefinition is null ? Visibility.Collapsed : Visibility.Visible;
        Notify(nameof(SearchScopeHint));
        RebuildSearchResults();
    }

    private void ClearTypeButton_Click(object sender, RoutedEventArgs e)
    {
        TypeCombo.SelectedItem = null; // cascades through TypeCombo_SelectionChanged
    }

    private void SubjectCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SubjectCombo.SelectedItem is SubjectViewModel subject)
            CommitSelection(subject);
    }

    private void SearchResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox lb && lb.SelectedItem is SubjectViewModel subject)
        {
            lb.SelectedItem = null;
            CommitSelection(subject);
        }
    }

    // ── Commit ────────────────────────────────────────────────────────────

    private void CommitSelection(SubjectViewModel subject)
    {
        // Reset control state so it is clean when next opened
        _selectedDefinition = null;
        RebuildFilteredSubjects();
        TypeCombo.SelectedItem  = null;
        SubjectCombo.SelectedItem = null;
        ClearTypeButton.Visibility = Visibility.Collapsed;
        Notify(nameof(SearchScopeHint));
        SearchText = string.Empty;

        SubjectSelected?.Invoke(subject);
    }

    // ── Constructor ───────────────────────────────────────────────────────

    public SubjectPickerControl()
    {
        InitializeComponent();
    }
}