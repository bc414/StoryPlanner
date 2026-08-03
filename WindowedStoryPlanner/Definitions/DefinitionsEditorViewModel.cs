using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WindowedStoryPlanner;

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
    private readonly IContentDeleter _deleter;

    // Registry-owned collections — exposed as passthroughs for XAML binding
    public ObservableCollection<SubjectDefinitionViewModel> SubjectDefinitions
        => _registry.AllSubjectDefinitionViewModels;

    public ObservableCollection<NoteTrackDefinitionViewModel> NoteTrackDefinitions
        => _registry.AllNoteTrackDefinitionViewModels;

    public ObservableCollection<WorkPhaseViewModel> WorkPhases
        => _registry.AllWorkPhaseViewModels;

    public ObservableCollection<NarrativePropertyDefinitionViewModel> NarrativePropertyDefinitions
        => _registry.AllNarrativePropertyDefinitionViewModels;

    public ObservableCollection<NarrativePropertyValueDefinitionViewModel> NarrativePropertyValueDefinitions
        => _registry.AllNarrativePropertyValueDefinitionViewModels;

    /// <summary>Status line for the narrative-property grids — a refused delete is otherwise silent.</summary>
    [ObservableProperty]
    private string _narrativePropertyStatus = string.Empty;

    /// <summary>Status line for the subject/track definition grids — same role as
    /// <see cref="NarrativePropertyStatus"/> on the other tab.</summary>
    [ObservableProperty]
    private string _definitionStatus = string.Empty;

    // UI-only derived state — not model data, lives here not in registry
    public ObservableCollection<string> AvailableSubjectTypes { get; } = new();

    public IReadOnlyList<OwnerType> OwnerTypes { get; } = Enum.GetValues<OwnerType>();
    public IReadOnlyList<TrackType> TrackTypes { get; } = Enum.GetValues<TrackType>();

    public DefinitionsEditorViewModel(IStoryService storyService, IViewModelRegistry registry, IContentDeleter deleter)
    {
        _storyService = storyService;
        _registry     = registry;
        _deleter      = deleter;
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
        SubjectDefinitions.Add(new SubjectDefinitionViewModel(model));
        RefreshAvailableSubjectTypes();
    }

    [RelayCommand]
    private async Task DeleteSubjectDefinition(SubjectDefinitionViewModel vm)
    {
        if (await _deleter.TryDeleteSubjectDefinitionAsync(vm))
        {
            DefinitionStatus = string.Empty;
            RefreshAvailableSubjectTypes();
        }
        else
        {
            DefinitionStatus = $"Cannot delete \"{vm.SubjectType}\" — subjects, tracks, or narrative " +
                               "properties still use it. Reassign or delete those first.";
        }
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
        NoteTrackDefinitions.Add(new NoteTrackDefinitionViewModel(model, SubjectDefinitions));
    }

    [RelayCommand]
    private async Task DeleteNoteTrackDefinition(NoteTrackDefinitionViewModel vm)
    {
        DefinitionStatus = await _deleter.TryDeleteNoteTrackDefinitionAsync(vm)
            ? string.Empty
            : $"Cannot delete \"{vm.TrackName}\" — notes still carry this track. Move or delete them first.";
    }

    // ── Work phases ───────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task AddWorkPhase()
    {
        int nextOrder = WorkPhases.Count > 0 ? WorkPhases.Max(p => p.DisplayOrder) + 1 : 1;
        var model = new WorkPhase { Name = "New Phase", DisplayOrder = nextOrder };
        _storyService.WorkPhases.Add(model);
        await _storyService.SaveAsync();
        WorkPhases.Add(new WorkPhaseViewModel(model));
    }

    [RelayCommand]
    private async Task DeleteWorkPhase(WorkPhaseViewModel vm)
    {
        NarrativePropertyStatus = await _deleter.TryDeleteWorkPhaseAsync(vm)
            ? string.Empty
            : $"Cannot delete \"{vm.Name}\" — a narrative property gates on it. Clear that first.";
    }

    // ── Narrative property definitions ────────────────────────────────────────

    [RelayCommand]
    private async Task AddNarrativePropertyDefinition()
    {
        int nextOrder = NarrativePropertyDefinitions.Count > 0
            ? NarrativePropertyDefinitions.Max(p => p.DisplayOrder) + 1
            : 1;
        var model = new NarrativePropertyDefinition
        {
            Name = "New Property",
            OwnerType = OwnerType.Subject,
            SubjectDefinitionId = SubjectDefinitions.FirstOrDefault()?.Id ?? 0,
            DisplayOrder = nextOrder
            // Question / Explanation stay empty — the same rule the seed op follows. An empty
            // field is visibly unfinished; a placeholder reads as decided.
        };
        _storyService.NarrativePropertyDefinitions.Add(model);
        await _storyService.SaveAsync();
        NarrativePropertyDefinitions.Add(new NarrativePropertyDefinitionViewModel(
            model, SubjectDefinitions, WorkPhases));
    }

    [RelayCommand]
    private async Task DeleteNarrativePropertyDefinition(NarrativePropertyDefinitionViewModel vm)
    {
        NarrativePropertyStatus = await _deleter.TryDeleteNarrativePropertyDefinitionAsync(vm)
            ? string.Empty
            : $"Cannot delete \"{vm.Name}\" — entities have values assigned on it. Clear those first.";
    }

    // ── Allowed values ────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task AddNarrativePropertyValueDefinition(NarrativePropertyDefinitionViewModel property)
    {
        if (property is null) return;

        var model = new NarrativePropertyValueDefinition
        {
            NarrativePropertyDefinitionId = property.Id,
            ValueName = "New Value"
        };
        _storyService.NarrativePropertyValueDefinitions.Add(model);
        await _storyService.SaveAsync();

        NarrativePropertyValueDefinitions.Add(new NarrativePropertyValueDefinitionViewModel(
            model, NarrativePropertyDefinitions));
        // The entity editor's picker binds the other projection — keep both in step or a new value
        // will not appear in any dropdown until the project is reopened.
        _registry.AllNarrativePropertyValueDefinitions.Add(new NarrativePropertyValueViewModel(model));
    }

    [RelayCommand]
    private async Task DeleteNarrativePropertyValueDefinition(NarrativePropertyValueDefinitionViewModel vm)
    {
        NarrativePropertyStatus = await _deleter.TryDeleteNarrativePropertyValueDefinitionAsync(vm)
            ? string.Empty
            : $"Cannot delete \"{vm.ValueName}\" — entities have it assigned. Clear those first.";
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
                Id:                          t.Id,
                TrackName:                   t.TrackName,
                TrackType:                   t.TrackType.ToString(),
                OwnerType:                   t.OwnerType.ToString(),
                SelectedSubjectType:         t.SelectedSubjectType,
                IsSingleton:                 t.IsSingleton,
                SupportsWorldDate:           t.SupportsWorldDate,
                SupportsTheme:               t.SupportsTheme,
                SupportsSourceMaterial:      t.SupportsSourceMaterial,
                CanEditInAuditMode:          t.CanEditInAuditMode,
                DisplayQuestion:             t.DisplayQuestion ?? string.Empty,
                UsageDirective:              t.UsageDirective  ?? string.Empty,
                AuditDirective:              t.AuditDirective  ?? string.Empty,
                ExpansionModeDisplayOrder:   t.ExpansionModeDisplayOrder,
                LinkingModeDisplayOrder:     t.LinkingModeDisplayOrder,
                GardenerModeDisplayOrder:    t.GardenerModeDisplayOrder,
                AuditModeDisplayOrder:       t.AuditModeDisplayOrder,
                SceneDesignModeDisplayOrder: t.SceneDesignModeDisplayOrder,
                HiddenInExpansionMode:       t.HiddenInExpansionMode,
                HiddenInLinkingMode:         t.HiddenInLinkingMode,
                HiddenInGardenerMode:        t.HiddenInGardenerMode,
                HiddenInAuditMode:           t.HiddenInAuditMode,
                HiddenInSceneDesignMode:     t.HiddenInSceneDesignMode));

        var propertyData = NarrativePropertyDefinitions
            .Select(p => new NarrativePropertyExportData(
                Id:                  p.Id,
                Name:                p.Name,
                OwnerType:           p.OwnerType.ToString(),
                SelectedSubjectType: p.OwnerType is OwnerType.PlotPoint or OwnerType.Chapter
                                         ? string.Empty          // these ignore SubjectDefinitionId
                                         : p.SelectedSubjectType,
                DisplayOrder:        p.DisplayOrder,
                Question:            p.Question,
                Explanation:         p.Explanation,
                GatingWorkPhase:     p.SelectedWorkPhase?.Name ?? string.Empty,
                Values:              NarrativePropertyValueDefinitions
                                         .Where(v => v.NarrativePropertyDefinitionId == p.Id)
                                         .Select(v => new NarrativePropertyValueExportData(v.ValueName, v.Description))
                                         .ToList()));

        string markdown = DefinitionsMarkdownExporter.Build(subjectData, trackData, propertyData);
        File.WriteAllText(outputPath, markdown);

        MessageBox.Show($"Exported to:\n{outputPath}", "Export Complete",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    // ── Canonical TrackType display order (shared by all mode auto-numbering) ─

    private static readonly IReadOnlyList<TrackType> CanonicalTrackTypeOrder = new[]
    {
        TrackType.Ontology,
        TrackType.Civilization,
        TrackType.History,
        TrackType.Characterization,
        TrackType.PageDesign,
        TrackType.WorldInference,
        TrackType.ThematicEvidence,
        TrackType.NarrativeArchitecture,
        TrackType.Canon,
        TrackType.Analogies,
        TrackType.Allegories,
        TrackType.NotesToSelf,
        TrackType.Unset,
    };

    /// <summary>
    /// Within each independent section (subject type / owner type grouping),
    /// assigns sequential display-order values 0, 1, 2 … according to
    /// <paramref name="trackTypeOrder"/>. Tracks whose <see cref="TrackType"/>
    /// does not appear in the list are placed at the end in their original
    /// relative order.
    /// </summary>
    private async Task AssignSequentialDisplayOrders(
        TrackType[]                                trackTypeOrder,
        Func<NoteTrackDefinitionViewModel, int>    getOrder,
        Action<NoteTrackDefinitionViewModel, int>  setOrder)
    {
        var groups = NoteTrackDefinitions
            .GroupBy(t => (SectionOrder(t), SectionKey(t), SubGroupOrder(t)));

        foreach (var group in groups)
        {
            var ordered = group
                .OrderBy(t =>
                {
                    int idx = Array.IndexOf(trackTypeOrder, t.TrackType);
                    return idx < 0 ? int.MaxValue : idx;
                })
                .ThenBy(getOrder)
                .ToList();

            for (int i = 0; i < ordered.Count; i++)
                setOrder(ordered[i], i);
        }

        await _storyService.SaveAsync();
    }

    [RelayCommand]
    private Task AutoNumberExpansionModeDisplayOrders() =>
        AssignSequentialDisplayOrders(
            trackTypeOrder: new[]
            {
                TrackType.Canon,
                TrackType.Analogies,
                TrackType.Ontology,
                TrackType.Civilization,
                TrackType.History,
                TrackType.Characterization,
                TrackType.ThematicEvidence,
                TrackType.Allegories,
                TrackType.NotesToSelf,
                TrackType.NarrativeArchitecture,
                TrackType.PageDesign,
                TrackType.WorldInference,
                TrackType.Unset,
            },
            getOrder: t => t.ExpansionModeDisplayOrder,
            setOrder: (t, v) => t.ExpansionModeDisplayOrder = v);

    [RelayCommand]
    private Task AutoNumberLinkingModeDisplayOrders() =>
        AssignSequentialDisplayOrders(
            trackTypeOrder: new[]
            {
                TrackType.Ontology,
                TrackType.Civilization,
                TrackType.History,
                TrackType.Characterization,
                TrackType.PageDesign,
                TrackType.WorldInference,
                TrackType.NarrativeArchitecture,
                TrackType.ThematicEvidence,
                TrackType.Allegories,
                TrackType.Canon,
                TrackType.Analogies,
                TrackType.NotesToSelf,
                TrackType.Unset,
            },
            getOrder: t => t.LinkingModeDisplayOrder,
            setOrder: (t, v) => t.LinkingModeDisplayOrder = v);

    [RelayCommand]
    private Task AutoNumberGardenerModeDisplayOrders() =>
        AssignSequentialDisplayOrders(
            trackTypeOrder: new[]
            {
                TrackType.Ontology,
                TrackType.Civilization,
                TrackType.History,
                TrackType.Characterization,
                TrackType.PageDesign,
                TrackType.WorldInference,
                TrackType.ThematicEvidence,
                TrackType.NarrativeArchitecture,
                TrackType.Canon,
                TrackType.Analogies,
                TrackType.Allegories,
                TrackType.NotesToSelf,
                TrackType.Unset,
            },
            getOrder: t => t.GardenerModeDisplayOrder,
            setOrder: (t, v) => t.GardenerModeDisplayOrder = v);

    [RelayCommand]
    private Task AutoNumberAuditModeDisplayOrders() =>
        AssignSequentialDisplayOrders(
            trackTypeOrder: new[]
            {
                TrackType.NarrativeArchitecture,
                TrackType.Ontology,
                TrackType.Civilization,
                TrackType.History,
                TrackType.Characterization,
                TrackType.PageDesign,
                TrackType.WorldInference,
                TrackType.ThematicEvidence,
                TrackType.Canon,
                TrackType.Analogies,
                TrackType.Allegories,
                TrackType.NotesToSelf,
                TrackType.Unset,
            },
            getOrder: t => t.AuditModeDisplayOrder,
            setOrder: (t, v) => t.AuditModeDisplayOrder = v);

    [RelayCommand]
    private Task AutoNumberSceneDesignModeDisplayOrders() =>
        AssignSequentialDisplayOrders(
            trackTypeOrder: new[]
            {
                TrackType.Ontology,
                TrackType.Civilization,
                TrackType.History,
                TrackType.Characterization,
                TrackType.PageDesign,
                TrackType.WorldInference,
                TrackType.ThematicEvidence,
                TrackType.NarrativeArchitecture,
                TrackType.Canon,
                TrackType.Analogies,
                TrackType.Allegories,
                TrackType.NotesToSelf,
                TrackType.Unset,
            },
            getOrder: t => t.SceneDesignModeDisplayOrder,
            setOrder: (t, v) => t.SceneDesignModeDisplayOrder = v);
}
