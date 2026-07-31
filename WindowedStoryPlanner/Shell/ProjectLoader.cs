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
        _registry.Clear();

        // --- Definitions first — subjects depend on AllSubjectDefinitionViewModels ---
        foreach (var m in _storyService.SubjectDefinitions.OrderBy(s => s.DisplayOrder))
            _registry.AllSubjectDefinitionViewModels.Add(
                new SubjectDefinitionViewModel(m, _storyService));

        foreach (var m in _storyService.NoteTrackDefinitions)
            _registry.AllNoteTrackDefinitionViewModels.Add(
                new NoteTrackDefinitionViewModel(m, _storyService, _registry.AllSubjectDefinitionViewModels));

        // Sync UI-derived state on tab VMs that depend on definitions
        _definitions.Reload();
        _subjectLibrary.Reload();

        // --- Themes ---
        foreach (var m in _storyService.Themes)
            _registry.AllThemeViewModels.Add(new ThemeViewModel(m, _storyService));

        _themeLibrary.Reload();

        // --- Source Materials --- Works before Parts before Notes (Notes resolve citations
        // against both shared collections at construction time).
        foreach (var m in _storyService.SourceMaterials)
            _registry.AllSourceMaterialViewModels.Add(new SourceMaterialViewModel(m, _storyService));

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

        // Build ConversationViewModels and attach their block VMs
        var blocksByConv = _storyService.ConversationBlocks
            .GroupBy(b => b.ConversationId)
            .ToDictionary(g => g.Key, g => g.OrderBy(b => b.BlockNumber).ToList());

        foreach (var conv in _storyService.Conversations)
        {
            var convVm = new ConversationViewModel(conv, _windowManager, _factory, _registry);
            if (blocksByConv.TryGetValue(conv.Id, out var blocks))
                foreach (var block in blocks)
                {
                    var blockVm = new ConversationBlockViewModel(block, _storyService) { ParentConversation = convVm };
                    blockVm.Initialize();
                    convVm.Blocks.Add(blockVm);
                }
            convVm.BuildSubjectCoverages(
                _storyService.ConversationSubjectCoverages,
                _storyService.ConversationSubjectCoverageTracks,
                _registry.AllSubjectViewModels,
                _registry.AllNoteTrackDefinitionViewModels,
                _storyService);
            convVm.OnStatsRefreshed = _conversationLibrary.RefreshDashboard;
            _registry.AllConversationViewModels.Add(convVm);
        }

        _registry.AllNarrativePropertyValues = _storyService.NarrativePropertyValues;

        foreach (var value in _storyService.NarrativePropertyValueDefinitions)
            _registry.AllNarrativePropertyValueDefinitions.Add(
                new NarrativePropertyValueViewModel(value));

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
