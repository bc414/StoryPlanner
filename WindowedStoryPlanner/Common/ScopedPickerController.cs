using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WindowedStoryPlanner;

/// <summary>
/// The shared logic of the scope-combo + item-combo + scoped-search picker shape
/// (SubjectPickerControl's Type → Subject, PlotPointPickerControl's Chapter → PlotPoint).
/// Extracted 2026-08-02 after the scoped-search enhancement had to be hand-ported across
/// three near-identical controls: the *behavior* now lives once, and the controls are thin
/// adapters that forward their XAML-facing property names here. A future picker (Theme,
/// WorkPhase, …) instantiates this rather than becoming the fourth copy.
///
/// The controls keep ownership of anything genuinely visual (combo dropdown state, clear-button
/// visibility, popup light-dismiss) — this class never touches WPF types.
/// </summary>
public sealed class ScopedPickerController<TScope, TItem>
    where TScope : class
    where TItem : class
{
    private readonly Func<IEnumerable<TItem>> _allItems;
    private readonly Func<TItem, TScope, bool> _belongsToScope;
    private readonly Func<TItem, string> _searchableText;
    private readonly Func<IEnumerable<TItem>, IEnumerable<TItem>> _comboOrder;
    private readonly Func<IEnumerable<TItem>, IEnumerable<TItem>> _searchOrder;
    private readonly Func<TScope, string> _scopedHint;
    private readonly string _unscopedHint;

    /// <summary>Raised whenever FilteredItems / SearchResults / SearchScopeHint / SearchText
    /// change — the owning control forwards this to its own INotifyPropertyChanged names.</summary>
    public event Action? StateChanged;

    public ScopedPickerController(
        Func<IEnumerable<TItem>> allItems,
        Func<TItem, TScope, bool> belongsToScope,
        Func<TItem, string> searchableText,
        Func<IEnumerable<TItem>, IEnumerable<TItem>> comboOrder,
        Func<IEnumerable<TItem>, IEnumerable<TItem>> searchOrder,
        Func<TScope, string> scopedHint,
        string unscopedHint)
    {
        _allItems       = allItems;
        _belongsToScope = belongsToScope;
        _searchableText = searchableText;
        _comboOrder     = comboOrder;
        _searchOrder    = searchOrder;
        _scopedHint     = scopedHint;
        _unscopedHint   = unscopedHint;
    }

    // ── Scope ─────────────────────────────────────────────────────────────

    private TScope? _selectedScope;
    public TScope? SelectedScope
    {
        get => _selectedScope;
        set
        {
            _selectedScope = value;
            RebuildFilteredItems();
            RebuildSearchResults();
            StateChanged?.Invoke();
        }
    }

    /// <summary>Tooltip reflecting whatever the search is currently scoped to — a scope picked
    /// in the combo restrains the search box to that scope's items.</summary>
    public string SearchScopeHint =>
        _selectedScope is not null ? _scopedHint(_selectedScope) : _unscopedHint;

    // ── Filtered items (item combo ItemsSource) ───────────────────────────

    private List<TItem> _filteredItems = [];
    public IReadOnlyList<TItem> FilteredItems => _filteredItems;

    private void RebuildFilteredItems()
    {
        var items = _allItems();
        if (_selectedScope is not null)
            items = items.Where(i => _belongsToScope(i, _selectedScope));
        _filteredItems = _comboOrder(items).ToList();
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
            RebuildSearchResults();
            StateChanged?.Invoke();
        }
    }

    public bool HasSearchText => !string.IsNullOrWhiteSpace(_searchText);

    private List<TItem> _searchResults = [];
    public IReadOnlyList<TItem> SearchResults => _searchResults;

    private void RebuildSearchResults()
    {
        if (!HasSearchText)
        {
            _searchResults = [];
            return;
        }

        var lower = _searchText.Trim().ToLowerInvariant();

        // A scope picked in the combo restrains the search to just that scope's items —
        // searching outside it no longer makes sense once one is already chosen.
        var scope = _selectedScope is null
            ? _allItems()
            : _allItems().Where(i => _belongsToScope(i, _selectedScope));

        _searchResults = _searchOrder(
                scope.Where(i => _searchableText(i).ToLowerInvariant().Contains(lower)))
            .ToList();
    }

    // ── Lifecycle ─────────────────────────────────────────────────────────

    /// <summary>Recomputes everything — the owning control calls this when its Registry DP is set.</summary>
    public void Refresh()
    {
        RebuildFilteredItems();
        RebuildSearchResults();
        StateChanged?.Invoke();
    }

    /// <summary>Resets scope and search so the picker is clean when next opened —
    /// called by the control after a selection is committed.</summary>
    public void Reset()
    {
        _selectedScope = null;
        _searchText = string.Empty;
        RebuildFilteredItems();
        RebuildSearchResults();
        StateChanged?.Invoke();
    }
}
