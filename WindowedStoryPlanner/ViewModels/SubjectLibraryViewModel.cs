using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Tab ViewModel for the Subject Library.
/// Follows the same pattern as ChapterLibraryViewModel:
/// all subject data lives in IViewModelRegistry; this class owns only
/// Groups, which is a UI-derived grouping of AllSubjectViewModels by definition.
/// </summary>
public partial class SubjectLibraryViewModel : ObservableObject
{
    private readonly IContentFactory    _factory;
    private readonly IContentDeleter    _deleter;
    private readonly IWindowManager     _windowManager;
    private readonly IViewModelRegistry _registry;

    public AppSettings AppSettings { get; }

    /// <summary>
    /// One entry per SubjectDefinition, ordered by DisplayOrder.
    /// UI-derived — rebuilt whenever definitions change.
    /// Lives here, not in the registry, for the same reason AvailableSubjectTypes
    /// lives on DefinitionsEditorViewModel: it is a presentation grouping, not model data.
    /// </summary>
    public ObservableCollection<SubjectGroupViewModel> Groups { get; } = new();

    public SubjectLibraryViewModel(
        IContentFactory    factory,
        IContentDeleter    deleter,
        IWindowManager     windowManager,
        IViewModelRegistry registry,
        AppSettings        appSettings)
    {
        _factory       = factory;
        _deleter       = deleter;
        _windowManager = windowManager;
        _registry      = registry;
        AppSettings    = appSettings;

        // Rebuild groups when definitions are added/removed at runtime (e.g. from Definitions tab).
        // Listen to the registry collection — not IStoryService — since the registry is
        // now the canonical source for SubjectDefinitionViewModels.
        _registry.AllSubjectDefinitionViewModels.CollectionChanged += (_, _) => RebuildGroups();

        // Do NOT call RebuildGroups() here — registry is empty at construction time.
        // ProjectLoader calls Reload() after populating the registry.
    }

    /// <summary>
    /// Rebuilds Groups from the current registry state.
    /// Called by ProjectLoader after each project load and automatically
    /// when AllSubjectDefinitionViewModels changes at runtime.
    /// </summary>
    public void Reload() => RebuildGroups();

    private void RebuildGroups()
    {
        Groups.Clear();
        foreach (var defVm in _registry.AllSubjectDefinitionViewModels.OrderBy(d => d.DisplayOrder))
            Groups.Add(new SubjectGroupViewModel(defVm.Model, _registry.AllSubjectViewModels, AppSettings));
    }

    [RelayCommand]
    private async Task AddSubject(SubjectGroupViewModel group)
    {
        // SubjectDefinitionId is stored directly on the group — no fragile string lookup needed
        var vm = await _factory.CreateSubjectAsync(group.SubjectDefinitionId);
        _windowManager.OpenCommonWindow(EditorMode.Expansion, vm);
    }

    [RelayCommand]
    private void OpenSubject(SubjectViewModel subject) =>
        _windowManager.OpenCommonWindow(EditorMode.Expansion, subject);

    [RelayCommand]
    private async Task DeleteSubject(SubjectViewModel subject)
    {
        if (!await _deleter.TryDeleteSubjectAsync(subject))
            MessageBox.Show(
                "Cannot delete a subject that still has notes or plot point links.",
                "Delete Failed",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }
}
