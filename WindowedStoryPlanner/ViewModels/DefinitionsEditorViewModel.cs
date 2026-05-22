using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Tab ViewModel for the Definitions editor.
/// Follows the same pattern as ChapterLibraryViewModel:
/// collections live in IViewModelRegistry, this class only handles commands
/// and owns UI-derived state (AvailableSubjectTypes).
/// </summary>
public partial class DefinitionsEditorViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;

    // Registry-owned collections — exposed as passthroughs for XAML binding
    public ObservableCollection<SubjectDefinitionViewModel> SubjectDefinitions
        => _registry.AllSubjectDefinitionViewModels;

    public ObservableCollection<NoteTrackDefinitionViewModel> NoteTrackDefinitions
        => _registry.AllNoteTrackDefinitionViewModels;

    // UI-only derived state — not model data, lives here not in registry
    public ObservableCollection<string> AvailableSubjectTypes { get; } = new();

    public IReadOnlyList<OwnerType> OwnerTypes { get; } = Enum.GetValues<OwnerType>();
    public IReadOnlyList<TrackType> TrackTypes { get; } = Enum.GetValues<TrackType>();
    public IReadOnlyList<int?> FunctionKeyOptions { get; } =
        new int?[] { null }.Concat(Enumerable.Range(1, 12).Cast<int?>()).ToList();

    public DefinitionsEditorViewModel(IStoryService storyService, IViewModelRegistry registry)
    {
        _storyService = storyService;
        _registry     = registry;
    }

    /// <summary>
    /// Refreshes AvailableSubjectTypes from the registry after ProjectLoader repopulates it.
    /// The registry collections are already cleared and repopulated by ProjectLoader directly —
    /// this only needs to sync the derived UI list.
    /// </summary>
    public void Reload() => RefreshAvailableSubjectTypes();

    private void RefreshAvailableSubjectTypes()
    {
        AvailableSubjectTypes.Clear();
        foreach (var subjectType in SubjectDefinitions.Select(s => s.SubjectType))
            AvailableSubjectTypes.Add(subjectType);
    }

    [RelayCommand]
    private async Task AddSubjectDefinition()
    {
        int nextOrder = SubjectDefinitions.Count > 0
            ? SubjectDefinitions.Max(s => s.DisplayOrder) + 1
            : 0;
        var model = new SubjectDefinition { SubjectType = "NewType", DisplayOrder = nextOrder };
        _storyService.SubjectDefinitions.Add(model);
        await _storyService.SaveAsync();
        SubjectDefinitions.Add(new SubjectDefinitionViewModel(model, _storyService));
        RefreshAvailableSubjectTypes();
    }

    [RelayCommand]
    private async Task DeleteSubjectDefinition(SubjectDefinitionViewModel vm)
    {
        _storyService.SubjectDefinitions.Remove(vm.Model);
        SubjectDefinitions.Remove(vm);
        await _storyService.SaveAsync();
        RefreshAvailableSubjectTypes();
    }

    [RelayCommand]
    private async Task MoveSubjectDefinitionUp(SubjectDefinitionViewModel vm)
    {
        int index = SubjectDefinitions.IndexOf(vm);
        if (index <= 0) return;
        var other = SubjectDefinitions[index - 1];
        (vm.DisplayOrder, other.DisplayOrder) = (other.DisplayOrder, vm.DisplayOrder);
        SubjectDefinitions.Move(index, index - 1);
        await _storyService.SaveAsync();
    }

    [RelayCommand]
    private async Task MoveSubjectDefinitionDown(SubjectDefinitionViewModel vm)
    {
        int index = SubjectDefinitions.IndexOf(vm);
        if (index < 0 || index >= SubjectDefinitions.Count - 1) return;
        var other = SubjectDefinitions[index + 1];
        (vm.DisplayOrder, other.DisplayOrder) = (other.DisplayOrder, vm.DisplayOrder);
        SubjectDefinitions.Move(index, index + 1);
        await _storyService.SaveAsync();
    }

    [RelayCommand]
    private async Task SaveAll()
    {
        RefreshAvailableSubjectTypes();
        await _storyService.SaveAsync();
    }

    [RelayCommand]
    private async Task AddNoteTrackDefinition()
    {
        var model = new NoteTrackDefinition { TrackName = "New Track", DisplayOrder = 0 };
        _storyService.NoteTrackDefinitions.Add(model);
        await _storyService.SaveAsync();
        NoteTrackDefinitions.Add(new NoteTrackDefinitionViewModel(model, _storyService, SubjectDefinitions));
    }

    [RelayCommand]
    private async Task DeleteNoteTrackDefinition(NoteTrackDefinitionViewModel vm)
    {
        _storyService.NoteTrackDefinitions.Remove(vm.Model);
        NoteTrackDefinitions.Remove(vm);
        await _storyService.SaveAsync();
    }
}
