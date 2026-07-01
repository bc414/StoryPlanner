using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels;

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

    // Registry-owned collection — exposed as passthrough for XAML binding
    public ObservableCollection<SourceMaterialViewModel> SourceMaterials => _registry.AllSourceMaterialViewModels;

    public ICollectionView FilteredSourceMaterials { get; }

    [ObservableProperty]
    private string _searchText = string.Empty;

    public SourceMaterialLibraryViewModel(IStoryService storyService, IViewModelRegistry registry, IWindowManager windowManager)
    {
        _storyService  = storyService;
        _registry      = registry;
        _windowManager = windowManager;

        var view = new ListCollectionView(SourceMaterials) { Filter = FilterSourceMaterial };
        FilteredSourceMaterials = view;
    }

    private bool FilterSourceMaterial(object obj)
    {
        if (string.IsNullOrWhiteSpace(SearchText)) return true;
        return obj is SourceMaterialViewModel sm &&
               sm.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase);
    }

    partial void OnSearchTextChanged(string value) => FilteredSourceMaterials.Refresh();

    /// <summary>
    /// Called by ProjectLoader after it repopulates the registry.
    /// </summary>
    public void Reload() { /* registry already repopulated by ProjectLoader */ }

    [RelayCommand]
    private async Task AddSourceMaterial()
    {
        var model = new SourceMaterial { Name = "New Source Material", Description = string.Empty };
        _storyService.SourceMaterials.Add(model);
        await _storyService.SaveAsync();
        SourceMaterials.Add(new SourceMaterialViewModel(model, _storyService));
    }

    [RelayCommand]
    private async Task DeleteSourceMaterial(SourceMaterialViewModel vm)
    {
        _storyService.SourceMaterials.Remove(vm.Model);
        SourceMaterials.Remove(vm);
        await _storyService.SaveAsync();
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
}
