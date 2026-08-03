using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner;

/// <summary>
/// Type → Subject picker. All filtering/search/reset behavior lives in the shared
/// <see cref="ScopedPickerController{TScope,TItem}"/>; this class only adapts it to XAML
/// (property names, combo events, clear-button visibility).
/// </summary>
public partial class SubjectPickerControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // ── Raised when the user confirms a subject selection ─────────────────
    public event Action<SubjectViewModel>? SubjectSelected;

    private readonly ScopedPickerController<SubjectDefinitionViewModel, SubjectViewModel> _picker;

    public SubjectPickerControl()
    {
        _picker = new ScopedPickerController<SubjectDefinitionViewModel, SubjectViewModel>(
            allItems:       () => Registry?.AllSubjectViewModels ?? Enumerable.Empty<SubjectViewModel>(),
            belongsToScope: (s, def) => s.SubjectDefinitionId == def.Id,
            searchableText: s => s.Name,
            comboOrder:     items => items.OrderBy(s => s.Name),
            searchOrder:    items => items.OrderBy(s => s.Name),
            scopedHint:     def => $"Search {def.SubjectType} subjects by name",
            unscopedHint:   "Search subjects by name");
        _picker.StateChanged += OnPickerStateChanged;

        InitializeComponent();
    }

    private void OnPickerStateChanged()
    {
        Notify(nameof(FilteredSubjects));
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
        {
            ctrl.Notify(nameof(SubjectDefinitions));
            ctrl._picker.Refresh();
        }
    }

    // ── Whether any combo dropdown is currently open ───────────────────────
    // Used by the host window to suppress light-dismiss while dropdowns are open.
    public bool IsAnyComboDropDownOpen =>
        TypeCombo.IsDropDownOpen || SubjectCombo.IsDropDownOpen;

    // ── XAML-facing projections of the shared controller ──────────────────

    public IEnumerable<SubjectDefinitionViewModel> SubjectDefinitions =>
        Registry?.AllSubjectDefinitionViewModels
                 .OrderBy(d => d.DisplayOrder)
        ?? Enumerable.Empty<SubjectDefinitionViewModel>();

    public IReadOnlyList<SubjectViewModel> FilteredSubjects => _picker.FilteredItems;
    public IReadOnlyList<SubjectViewModel> SearchResults => _picker.SearchResults;
    public bool HasSearchText => _picker.HasSearchText;
    public string SearchScopeHint => _picker.SearchScopeHint;

    public string SearchText
    {
        get => _picker.SearchText;
        set => _picker.SearchText = value;
    }

    // ── Combo SelectionChanged handlers (no two-way binding) ──────────────

    private void TypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _picker.SelectedScope = TypeCombo.SelectedItem as SubjectDefinitionViewModel;
        SubjectCombo.SelectedItem = null;   // clear any stale subject selection
        ClearTypeButton.Visibility = _picker.SelectedScope is null ? Visibility.Collapsed : Visibility.Visible;
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
        _picker.Reset();
        TypeCombo.SelectedItem  = null;
        SubjectCombo.SelectedItem = null;
        ClearTypeButton.Visibility = Visibility.Collapsed;

        SubjectSelected?.Invoke(subject);
    }
}
