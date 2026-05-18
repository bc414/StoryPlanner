using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner.ViewModels;

public class ContentFactory : IContentFactory
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;
    private readonly IWindowManager _windowManager;

    public ContentFactory(
        IStoryService storyService,
        IViewModelRegistry registry,
        IWindowManager windowManager)
    {
        _storyService  = storyService;
        _registry      = registry;
        _windowManager = windowManager;
    }

    public async Task<NoteViewModel> CreateNoteAsync(int ownerId, OwnerType ownerType, int? noteTrackDefinitionId, int sortOrder)
    {
        var newNote = new Note
        {
            OwnerId = ownerId,
            OwnerType = ownerType,
            NoteTrackDefinitionId = noteTrackDefinitionId,
            NoteState = NoteState.Unset,
            SortOrder = sortOrder,
            LastModified = DateTime.UtcNow
        };

        _storyService.Notes.Add(newNote);
        await _storyService.SaveAsync();

        var vm = new NoteViewModel(newNote, _storyService, _registry.AllThemeViewModels);
        _registry.AllNoteViewModels.Add(vm);
        return vm;
    }

    public async Task<PlotPointViewModel> CreatePlotPointAsync(int? chapterId, int orderInChapter)
    {
        var plotPoint = new PlotPoint
        {
            Title = "New Plot Point",
            ChapterId = chapterId,
            OrderInChapter = orderInChapter
        };

        _storyService.PlotPoints.Add(plotPoint);
        await _storyService.SaveAsync();

        var vm = new PlotPointViewModel(plotPoint, _registry, _storyService, this, _windowManager);
        _registry.AllPlotPointViewModels.Add(vm);
        return vm;
    }

    public async Task<SubjectViewModel> CreateSubjectAsync(int subjectDefinitionId)
    {
        var subject = new Subject { SubjectDefinitionId = subjectDefinitionId };

        _storyService.Subjects.Add(subject);
        await _storyService.SaveAsync();

        var vm = new SubjectViewModel(subject, _registry, _storyService, this, _windowManager);
        _registry.AllSubjectViewModels.Add(vm);
        return vm;
    }

    public async Task<ChapterViewModel> CreateChapterAsync()
    {
        var chapter = new Chapter();

        _storyService.Chapters.Add(chapter);
        await _storyService.SaveAsync();

        var vm = new ChapterViewModel(chapter, _registry, _storyService, this);
        _registry.AllChapterViewModels.Add(vm);
        return vm;
    }

    public async Task CreatePlotPointSubjectLinkAsync(PlotPointViewModel plotPoint, SubjectViewModel subject)
    {
        if (_registry.AllPlotPointSubjectLinkViewModels.Any(l =>
                l.PlotPointId == plotPoint.Id && l.SubjectId == subject.Id))
            return;

        var link = new PlotPointSubjectLink
        {
            PlotPointId = plotPoint.Id,
            SubjectId = subject.Id
        };

        _storyService.PlotPointsSubjectLinks.Add(link);
        await _storyService.SaveAsync();

        var vm = new PlotPointSubjectLinkViewModel(link, _registry, _storyService, this);
        _registry.AllPlotPointSubjectLinkViewModels.Add(vm);
        _registry.RaiseLinksInvalidated();  // ← new link exists
    }
}
