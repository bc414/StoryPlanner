using StoryPlanner.Core;
using System.Linq;

namespace WindowedStoryPlanner;

/// <summary>
/// Populates IViewModelRegistry from IStoryService after a project is opened or created.
/// This is the only class permitted to construct leaf ViewModels directly.
/// </summary>
public class ProjectLoader
{
    private readonly IStoryService              _storyService;
    private readonly IViewModelRegistry         _registry;
    private readonly IContentFactory            _factory;
    private readonly IWindowManager             _windowManager;
    private readonly AppSettings                 _appSettings;
    private readonly ExportService              _exportService;
    private readonly DefinitionsEditorViewModel _definitions;
    private readonly SubjectLibraryViewModel    _subjectLibrary;
    private readonly ThemeLibraryViewModel      _themeLibrary;
    private readonly SourceMaterialLibraryViewModel _sourceMaterialLibrary;
    private readonly ConversationLibraryViewModel   _conversationLibrary;
    private readonly ExportViewModel            _export;
    private readonly GlobalSearchViewModel      _globalSearch;

    public ProjectLoader(
        IStoryService               storyService,
        IViewModelRegistry          registry,
        IContentFactory             factory,
        IWindowManager              windowManager,
        AppSettings                 appSettings,
        ExportService               exportService,
        DefinitionsEditorViewModel  definitions,
        SubjectLibraryViewModel     subjectLibrary,
        ThemeLibraryViewModel       themeLibrary,
        SourceMaterialLibraryViewModel sourceMaterialLibrary,
        ConversationLibraryViewModel   conversationLibrary,
        ExportViewModel             export,
        GlobalSearchViewModel       globalSearch)
    {
        _storyService          = storyService;
        _registry              = registry;
        _factory               = factory;
        _windowManager         = windowManager;
        _appSettings           = appSettings;
        _exportService         = exportService;
        _definitions           = definitions;
        _subjectLibrary        = subjectLibrary;
        _themeLibrary          = themeLibrary;
        _sourceMaterialLibrary = sourceMaterialLibrary;
        _conversationLibrary   = conversationLibrary;
        _export                = export;
        _globalSearch          = globalSearch;
    }

    public void Load()
    {
        // Dispose the previous file's element VMs before dropping them: their constructors
        // subscribed to app-lifetime registry events, so Clear() alone would leave every old
        // VM live and re-scanning the NEW file's notes on each mutation — the cross-project
        // leak class. Reverse of the construction order below, harmless on first load.
        foreach (var vm in _registry.AllSubjectViewModels) vm.Dispose();
        foreach (var vm in _registry.AllPlotPointViewModels) vm.Dispose();
        foreach (var vm in _registry.AllPlotPointSubjectLinkViewModels) vm.Dispose();
        foreach (var vm in _registry.AllChapterViewModels) vm.Dispose();

        _registry.Clear();

        // --- Definitions first — subjects depend on AllSubjectDefinitionViewModels ---
        foreach (var m in _storyService.SubjectDefinitions.OrderBy(s => s.DisplayOrder))
            _registry.AllSubjectDefinitionViewModels.Add(
                new SubjectDefinitionViewModel(m));

        foreach (var m in _storyService.NoteTrackDefinitions)
            _registry.AllNoteTrackDefinitionViewModels.Add(
                new NoteTrackDefinitionViewModel(m, _registry.AllSubjectDefinitionViewModels));

        // Narrative property definitions belong in THIS block, not after the narrative elements.
        // Every SubjectViewModel / PlotPointViewModel / ChapterViewModel /
        // PlotPointSubjectLinkViewModel constructed below runs InitializeCollections ->
        // RebuildNoteTracks -> new NarrativePropertyViewModel(...), which resolves its allowed
        // values against these collections. Populating them afterwards left every property with an
        // empty value list; it went unnoticed for a year only because the tables held no rows.
        _registry.AllNarrativePropertyValues = _storyService.NarrativePropertyValues;

        foreach (var value in _storyService.NarrativePropertyValueDefinitions)
            _registry.AllNarrativePropertyValueDefinitions.Add(
                new NarrativePropertyValueViewModel(value));

        // Work phases before property definitions — the gating-phase combo resolves against them
        // at construction. Same dependency shape as note tracks needing subject definitions above,
        // and stories needing to precede chapters below.
        foreach (var m in _storyService.WorkPhases.OrderBy(p => p.DisplayOrder))
            _registry.AllWorkPhaseViewModels.Add(new WorkPhaseViewModel(m));

        // Boards before property definitions, for the same reason as work phases: the board combo
        // on a property row resolves against this collection at construction.
        foreach (var m in _storyService.PropertyBoards.OrderBy(b => b.DisplayOrder))
            _registry.AllPropertyBoardViewModels.Add(
                new PropertyBoardViewModel(m, _registry.AllSubjectDefinitionViewModels));

        foreach (var m in _storyService.SubjectRelationDefinitions.OrderBy(r => r.DisplayOrder))
            _registry.AllSubjectRelationDefinitionViewModels.Add(
                new SubjectRelationDefinitionViewModel(m, _registry.AllSubjectDefinitionViewModels));

        foreach (var m in _storyService.NarrativePropertyDefinitions.OrderBy(p => p.DisplayOrder))
            _registry.AllNarrativePropertyDefinitionViewModels.Add(
                new NarrativePropertyDefinitionViewModel(
                    m, _registry.AllSubjectDefinitionViewModels, _registry.AllWorkPhaseViewModels,
                    _registry.AllPropertyBoardViewModels));

        foreach (var m in _storyService.NarrativePropertyValueDefinitions)
            _registry.AllNarrativePropertyValueDefinitionViewModels.Add(
                new NarrativePropertyValueDefinitionViewModel(
                    m, _registry.AllNarrativePropertyDefinitionViewModels));

        // Sync UI-derived state on tab VMs that depend on definitions
        _definitions.Reload();
        _subjectLibrary.Reload();

        // --- Themes ---
        foreach (var m in _storyService.Themes)
            _registry.AllThemeViewModels.Add(new ThemeViewModel(m));

        _themeLibrary.Reload();

        // --- Source Materials --- Works before Parts before Notes (Notes resolve citations
        // against both shared collections at construction time).
        foreach (var m in _storyService.SourceMaterials)
            _registry.AllSourceMaterialViewModels.Add(new SourceMaterialViewModel(m));

        foreach (var m in _storyService.SourceMaterialParts)
            _registry.AllSourceMaterialPartViewModels.Add(new SourceMaterialPartViewModel(m, _storyService));

        _sourceMaterialLibrary.Reload();
        _conversationLibrary.Reload();

        // --- Narrative elements ---
        foreach (var subject in _storyService.Subjects)
            _registry.AllSubjectViewModels.Add(
                new SubjectViewModel(subject, _registry, _storyService, _factory, _windowManager, _appSettings, _exportService));

        foreach (var plotPoint in _storyService.PlotPoints)
            _registry.AllPlotPointViewModels.Add(
                new PlotPointViewModel(plotPoint, _registry, _storyService, _factory, _windowManager, _appSettings, _exportService));

        foreach (var link in _storyService.PlotPointsSubjectLinks)
            _registry.AllPlotPointSubjectLinkViewModels.Add(
                new PlotPointSubjectLinkViewModel(link, _registry, _storyService, _factory, _appSettings, _exportService));

        // Stories before chapters — ChapterViewModel.FullNumber looks up its story's
        // OrderIndex, and the Chapters tab's story filter/combo needs AllStoryViewModels
        // populated before any ChapterViewModel construction can rely on it.
        foreach (var story in _storyService.Stories)
            _registry.AllStoryViewModels.Add(new StoryViewModel(story, _registry));

        foreach (var chapter in _storyService.Chapters)
            _registry.AllChapterViewModels.Add(
                new ChapterViewModel(chapter, _registry, _storyService, _factory, _appSettings, _exportService));

        foreach (var note in _storyService.Notes)
            _registry.AllNoteViewModels.Add(
                new NoteViewModel(note, _storyService, _registry.AllThemeViewModels,
                    _registry.AllSourceMaterialViewModels, _registry.AllSourceMaterialPartViewModels));

        // Build ConversationViewModels and attach their block VMs — the one construction
        // recipe lives in ConversationViewModel.BuildAll, shared with the library's rebuild.
        foreach (var convVm in ConversationViewModel.BuildAll(_storyService, _conversationLibrary.RefreshDashboard))
            _registry.AllConversationViewModels.Add(convVm);

        // Signal that bulk loading is complete. NarrativeElementViewModels use
        // this to defer their initial note-count calculation until all notes exist.
        _registry.RaiseStoryLoaded();

        // Bulk-adding conversations doesn't notify the library's computed dashboard
        // stats (they aren't collection-bound), so push one refresh now that all
        // blocks and their states are in place — otherwise the dashboard, and the
        // overall progress bar in particular, stays at zero until the first edit.
        _conversationLibrary.RefreshDashboard();

        _export.Reload();
        _globalSearch.Reload();
    }
}
