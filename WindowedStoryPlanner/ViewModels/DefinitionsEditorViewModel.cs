using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
    public void Reload()
    {
        RefreshAvailableSubjectTypes();
        SortNoteTrackDefinitions();
    }

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
        var model = new NoteTrackDefinition 
        { 
            TrackName = "New Track", 
            ExpansionModeDisplayOrder = 0,
            LinkingModeDisplayOrder = 0,
            GardenerModeDisplayOrder = 0,
            AuditModeDisplayOrder = 0,
            SceneDesignModeDisplayOrder = 0
        };
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

    private void SortNoteTrackDefinitions()
    {
        var sorted = NoteTrackDefinitions
            .OrderBy(SectionOrder)
            .ThenBy(SectionKey,     StringComparer.OrdinalIgnoreCase)
            .ThenBy(SubGroupOrder)
            .ThenBy(t => t.ExpansionModeDisplayOrder)
            .ToList();

        for (int targetIndex = 0; targetIndex < sorted.Count; targetIndex++)
        {
            int currentIndex = NoteTrackDefinitions.IndexOf(sorted[targetIndex]);
            if (currentIndex != targetIndex)
                NoteTrackDefinitions.Move(currentIndex, targetIndex);
        }
    }

    // ── Sort key helpers (mirror the exporter's grouping logic) ───────────────

    private static string SectionKey(NoteTrackDefinitionViewModel t) =>
        t.OwnerType switch
        {
            OwnerType.Subject              => string.IsNullOrWhiteSpace(t.SelectedSubjectType)
                                                 ? "Unassigned Subject"
                                                 : t.SelectedSubjectType,
            OwnerType.PlotPointSubjectLink => string.IsNullOrWhiteSpace(t.SelectedSubjectType)
                                                 ? "Unassigned Subject"
                                                 : t.SelectedSubjectType,
            OwnerType.PlotPoint            => "Plot Point",
            OwnerType.Chapter              => "Chapter",
            _                              => t.OwnerType.ToString()
        };

    private static int SectionOrder(NoteTrackDefinitionViewModel t) =>
        t.OwnerType switch
        {
            OwnerType.PlotPoint => int.MaxValue - 1,
            OwnerType.Chapter   => int.MaxValue,
            _                   => 0   // subject-type sections sort alphabetically via SectionKey
        };

    private static int SubGroupOrder(NoteTrackDefinitionViewModel t) =>
        t.OwnerType == OwnerType.PlotPointSubjectLink ? 1 : 0;

    [RelayCommand]
    private void ResortNoteTrackDefinitions() => SortNoteTrackDefinitions();

    [RelayCommand]
    private void ExportDefinitionsToMarkdown()
    {
        string projectPath = _storyService.CurrentFilePath;
        string projectName = Path.GetFileNameWithoutExtension(projectPath);
        string outputPath  = Path.Combine(Path.GetDirectoryName(projectPath)!, $"{projectName}-definitions.md");

        var subjectData = SubjectDefinitions
            .Select(s => (s.SubjectType, s.DisplayOrder));

        var trackData = NoteTrackDefinitions
            .Select(t => new NoteTrackDefinitionExportData(
                TrackName:                   t.TrackName,
                TrackType:                   t.TrackType.ToString(),
                OwnerType:                   t.OwnerType.ToString(),
                SelectedSubjectType:         t.SelectedSubjectType,
                IsSingleton:                 t.IsSingleton,
                SupportsWorldDate:           t.SupportsWorldDate,
                SupportsTheme:               t.SupportsTheme,
                CanEditInAuditMode:          t.CanEditInAuditMode,
                DisplayQuestion:             t.DisplayQuestion ?? string.Empty,
                UsageDirective:              t.UsageDirective  ?? string.Empty,
                AuditDirective:              t.AuditDirective  ?? string.Empty,
                ExpansionModeDisplayOrder:   t.ExpansionModeDisplayOrder,
                LinkingModeDisplayOrder:     t.LinkingModeDisplayOrder,
                GardenerModeDisplayOrder:    t.GardenerModeDisplayOrder,
                AuditModeDisplayOrder:       t.AuditModeDisplayOrder,
                SceneDesignModeDisplayOrder: t.SceneDesignModeDisplayOrder));

        string markdown = DefinitionsMarkdownExporter.Build(subjectData, trackData);
        File.WriteAllText(outputPath, markdown);

        MessageBox.Show($"Exported to:\n{outputPath}", "Export Complete",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
