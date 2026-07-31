using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WindowedStoryPlanner;

/// <summary>
/// Tab ViewModel for the SourceMaterial Library editor.
/// Follows the same pattern as ThemeLibraryViewModel: collections live in
/// IViewModelRegistry, this class only handles commands and search filtering.
/// </summary>
public partial class SourceMaterialLibraryViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;
    private readonly IWindowManager _windowManager;
    private readonly IContentDeleter _contentDeleter;

    // Registry-owned collection — exposed as passthrough for XAML binding
    public ObservableCollection<SourceMaterialViewModel> SourceMaterials => _registry.AllSourceMaterialViewModels;

    public ICollectionView FilteredSourceMaterials { get; }

    [ObservableProperty]
    private string _searchText = string.Empty;

    // ── Coverage grid: the Parts of the selected Work ──────────────────────────
    // Negative-space view (CLAUDE.md): the seeded, pre-enumerated Part set is what makes an
    // uncited/unreviewed row meaningful rather than an accretion artifact. Never sorted by
    // likely yield — seeded OrderIndex order only, per the "retrieval, not suggestion" rule.

    public ObservableCollection<SourceMaterialPartViewModel> SourceMaterialParts => _registry.AllSourceMaterialPartViewModels;

    public ICollectionView FilteredParts { get; }

    [ObservableProperty]
    private SourceMaterialViewModel? _selectedSourceMaterial;

    [ObservableProperty]
    private bool _showOnlyUntouched;

    public SourceMaterialLibraryViewModel(
        IStoryService storyService, IViewModelRegistry registry, IWindowManager windowManager, IContentDeleter contentDeleter)
    {
        _storyService  = storyService;
        _registry      = registry;
        _windowManager = windowManager;
        _contentDeleter = contentDeleter;

        var view = new ListCollectionView(SourceMaterials) { Filter = FilterSourceMaterial };
        FilteredSourceMaterials = view;

        var partsView = new ListCollectionView(SourceMaterialParts) { Filter = FilterPart };
        partsView.SortDescriptions.Add(new SortDescription(nameof(SourceMaterialPartViewModel.OrderIndex), ListSortDirection.Ascending));
        FilteredParts = partsView;
    }

    private bool FilterSourceMaterial(object obj)
    {
        if (string.IsNullOrWhiteSpace(SearchText)) return true;
        return obj is SourceMaterialViewModel sm &&
               sm.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase);
    }

    private bool FilterPart(object obj)
    {
        if (obj is not SourceMaterialPartViewModel p) return false;
        if (SelectedSourceMaterial is null || p.SourceMaterialId != SelectedSourceMaterial.Id) return false;
        return !ShowOnlyUntouched || p.IsUntouched;
    }

    partial void OnSearchTextChanged(string value) => FilteredSourceMaterials.Refresh();
    partial void OnShowOnlyUntouchedChanged(bool value) => FilteredParts.Refresh();

    partial void OnSelectedSourceMaterialChanged(SourceMaterialViewModel? value)
    {
        RefreshCoverage();
        FilteredParts.Refresh();
    }

    /// <summary>
    /// Called by ProjectLoader after it repopulates the registry. Resets the Work selection —
    /// this VM is a long-lived singleton, so a stale selection from a previously open file
    /// would otherwise survive a project switch.
    /// </summary>
    public void Reload() => SelectedSourceMaterial = null;

    /// <summary>
    /// Re-reads NoteCount/IsUntouched for every Part from IStoryService. NoteCount is
    /// deliberately not live-reactive (see SourceMaterialPartViewModel) — call this after
    /// citing/uncciting a Part elsewhere (e.g. from a note editor) to bring the grid back in
    /// sync. Also called automatically on Work selection and project load.
    /// </summary>
    [RelayCommand]
    private void RefreshCoverage()
    {
        foreach (var p in SourceMaterialParts)
            p.RefreshNoteCount();
        FilteredParts.Refresh();
    }

    [RelayCommand]
    private async Task AddSourceMaterial()
    {
        var model = new SourceMaterial
        {
            Name = "New Source Material",
            Description = string.Empty,
            OrderIndex = SourceMaterials.Count
        };
        _storyService.SourceMaterials.Add(model);
        await _storyService.SaveAsync();
        SourceMaterials.Add(new SourceMaterialViewModel(model, _storyService));
    }

    [RelayCommand]
    private async Task DeleteSourceMaterial(SourceMaterialViewModel vm)
    {
        if (!await _contentDeleter.TryDeleteSourceMaterialAsync(vm))
            MessageBox.Show(
                "Cannot delete a source that still has Parts or citations.",
                "Delete Failed",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }

    [RelayCommand]
    private async Task SaveAll()
    {
        await _storyService.SaveAsync();
    }

    [RelayCommand]
    private void OpenSourceMaterialWindow(SourceMaterialViewModel vm)
    {
        _windowManager.OpenSourceMaterialWindow(vm);
    }

    [RelayCommand]
    private void OpenSourceMaterialPartWindow(SourceMaterialPartViewModel part)
    {
        _windowManager.OpenSourceMaterialPartWindow(part);
    }

    /// <summary>The note-side counterpart of this tab's Part-side coverage grid: notes on a
    /// source-supporting track that carry no citation.</summary>
    [RelayCommand]
    private void OpenNotesWithoutCitation() =>
        _windowManager.OpenMissingFieldWindow(MissingNoteField.SourceMaterial);

    [RelayCommand]
    private async Task AddPart()
    {
        if (SelectedSourceMaterial is not { } work) return;

        var model = new SourceMaterialPart
        {
            SourceMaterialId = work.Id,
            Code = "New Part",
            Description = string.Empty,
            OrderIndex = SourceMaterialParts.Count(p => p.SourceMaterialId == work.Id),
            ReviewState = SourcePartReviewState.NotReviewed
        };
        _storyService.SourceMaterialParts.Add(model);
        await _storyService.SaveAsync();
        SourceMaterialParts.Add(new SourceMaterialPartViewModel(model, _storyService));
        FilteredParts.Refresh();
    }

    [RelayCommand]
    private async Task DeletePart(SourceMaterialPartViewModel part)
    {
        if (!await _contentDeleter.TryDeleteSourceMaterialPartAsync(part))
            MessageBox.Show(
                "Cannot delete a Part that is still cited by a note.",
                "Delete Failed",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }
}
