using StoryPlanner.Core;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner;

public class ContentDeleter : IContentDeleter
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;

    public ContentDeleter(IStoryService storyService, IViewModelRegistry registry)
    {
        _storyService = storyService;
        _registry = registry;
    }

    public async Task DeleteNoteAsync(NoteViewModel note)
    {
        // NoteSourceReference rows are note-owned — no orphan risk, cascade is correct here
        // (contrast SourceMaterial/SourceMaterialPart below, which refuse instead).
        var ownedReferences = _storyService.NoteSourceReferences.Where(r => r.NoteId == note.Id).ToList();
        foreach (var r in ownedReferences)
            _storyService.NoteSourceReferences.Remove(r);

        _storyService.Notes.Remove(note.Note);
        _registry.AllNoteViewModels.Remove(note);
        await _storyService.SaveAsync();
    }

    public async Task<bool> TryDeleteLinkAsync(PlotPointSubjectLinkViewModel link)
    {
        bool hasNotes = _storyService.Notes
            .Any(n => n.OwnerId == link.Id && n.OwnerType == OwnerType.PlotPointSubjectLink);

        if (hasNotes) return false;

        RemoveOwnedNarrativePropertyValues(link.Id, OwnerType.PlotPointSubjectLink);

        _storyService.PlotPointsSubjectLinks.Remove(link.Link);
        _registry.AllPlotPointSubjectLinkViewModels.Remove(link);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteSubjectAsync(SubjectViewModel subject)
    {
        bool hasNotes = _storyService.Notes
            .Any(n => n.OwnerId == subject.Id && n.OwnerType == OwnerType.Subject);


        bool hasLinks = _storyService.PlotPointsSubjectLinks
            .Any(l => l.SubjectId == subject.Id);

        if (hasNotes || hasLinks) return false;

        RemoveOwnedNarrativePropertyValues(subject.Id, OwnerType.Subject);

        _storyService.Subjects.Remove(subject.Subject);
        _registry.AllSubjectViewModels.Remove(subject);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeletePlotPointAsync(PlotPointViewModel plotPoint)
    {
        bool hasNotes = _storyService.Notes
            .Any(n => n.OwnerId == plotPoint.Id && n.OwnerType == OwnerType.PlotPoint);


        bool hasLinks = _storyService.PlotPointsSubjectLinks
            .Any(l => l.PlotPointId == plotPoint.Id);

        if (hasNotes || hasLinks) return false;

        RemoveOwnedNarrativePropertyValues(plotPoint.Id, OwnerType.PlotPoint);

        _storyService.PlotPoints.Remove(plotPoint.PlotPoint);
        _registry.AllPlotPointViewModels.Remove(plotPoint);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteChapterAsync(ChapterViewModel chapter)
    {
        bool hasNotes = _storyService.Notes
    .Any(n => n.OwnerId == chapter.Id && n.OwnerType == OwnerType.Chapter);

        if (hasNotes) return false;

        // Orphan plot points — set ChapterId to null, do not delete
        var ownedPlotPoints = _storyService.PlotPoints
            .Where(pp => pp.ChapterId == chapter.Id)
            .ToList();
        foreach (var pp in ownedPlotPoints)
            pp.ChapterId = null;

        RemoveOwnedNarrativePropertyValues(chapter.Id, OwnerType.Chapter);

        _storyService.Chapters.Remove(chapter.Chapter);
        _registry.AllChapterViewModels.Remove(chapter);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteStoryAsync(StoryViewModel story)
    {
        // Orphan children — set StoryId back to the "(Unassigned)" sentinel, do not delete.
        // Story has no notes to guard on, so this never refuses.
        var ownedChapters = _storyService.Chapters
            .Where(ch => ch.StoryId == story.Id)
            .ToList();
        foreach (var ch in ownedChapters)
            ch.StoryId = 0;

        _storyService.Stories.Remove(story.Story);
        _registry.AllStoryViewModels.Remove(story);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteSourceMaterialAsync(SourceMaterialViewModel work)
    {
        if (ContentIntegrity.SourceMaterialHasDependents(_storyService, work.Id)) return false;

        _storyService.SourceMaterials.Remove(work.Model);
        _registry.AllSourceMaterialViewModels.Remove(work);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteSourceMaterialPartAsync(SourceMaterialPartViewModel part)
    {
        if (ContentIntegrity.SourceMaterialPartHasReferences(_storyService, part.Id)) return false;

        _storyService.SourceMaterialParts.Remove(part.Model);
        _registry.AllSourceMaterialPartViewModels.Remove(part);
        await _storyService.SaveAsync();
        return true;
    }

    // --- Helpers ---

    private void RemoveOwnedNarrativePropertyValues(int ownerId, OwnerType ownerType)
    {
        // Resolve which ValueDefinitionIds are valid for this owner type,
        // by tracing: ValueDefinition → PropertyDefinition → OwnerType
        var validValueDefinitionIds = _storyService.NarrativePropertyValueDefinitions
            .Where(vd => _storyService.NarrativePropertyDefinitions
                .Any(pd => pd.Id == vd.NarrativePropertyDefinitionId
                        && pd.OwnerType == ownerType))
            .Select(vd => vd.Id)
            .ToHashSet();

        var owned = _storyService.NarrativePropertyValues
            .Where(p => p.OwnerId == ownerId
                     && validValueDefinitionIds.Contains(p.ValueDefinitionId))
            .ToList();

        foreach (var prop in owned)
            _storyService.NarrativePropertyValues.Remove(prop);
    }
}
