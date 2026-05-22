using StoryPlanner.Core;
using System.Linq;

namespace WindowedStoryPlanner.ViewModels;

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
    private readonly DefinitionsEditorViewModel _definitions;
    private readonly SubjectLibraryViewModel    _subjectLibrary;
    private readonly ThemeLibraryViewModel      _themeLibrary;

    public ProjectLoader(
        IStoryService               storyService,
        IViewModelRegistry          registry,
        IContentFactory             factory,
        IWindowManager              windowManager,
        AppSettings                 appSettings,
        DefinitionsEditorViewModel  definitions,
        SubjectLibraryViewModel     subjectLibrary,
        ThemeLibraryViewModel       themeLibrary)
    {
        _storyService   = storyService;
        _registry       = registry;
        _factory        = factory;
        _windowManager  = windowManager;
        _appSettings    = appSettings;
        _definitions    = definitions;
        _subjectLibrary = subjectLibrary;
        _themeLibrary   = themeLibrary;
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

        // --- Narrative elements ---
        foreach (var subject in _storyService.Subjects)
            _registry.AllSubjectViewModels.Add(
                new SubjectViewModel(subject, _registry, _storyService, _factory, _windowManager, _appSettings));

        foreach (var plotPoint in _storyService.PlotPoints)
            _registry.AllPlotPointViewModels.Add(
                new PlotPointViewModel(plotPoint, _registry, _storyService, _factory, _windowManager, _appSettings));

        foreach (var link in _storyService.PlotPointsSubjectLinks)
            _registry.AllPlotPointSubjectLinkViewModels.Add(
                new PlotPointSubjectLinkViewModel(link, _registry, _storyService, _factory, _appSettings));

        foreach (var chapter in _storyService.Chapters)
            _registry.AllChapterViewModels.Add(
                new ChapterViewModel(chapter, _registry, _storyService, _factory, _appSettings));

        foreach (var note in _storyService.Notes)
            _registry.AllNoteViewModels.Add(
                new NoteViewModel(note, _storyService, _registry.AllThemeViewModels));

        _registry.AllNarrativePropertyValues = _storyService.NarrativePropertyValues;

        foreach (var value in _storyService.NarrativePropertyValueDefinitions)
            _registry.AllNarrativePropertyValueDefinitions.Add(
                new NarrativePropertyValueViewModel(value));
    }
}
