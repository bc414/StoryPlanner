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
        // StoryService.DeleteNote cascades the note's NoteSourceReference rows — citations are
        // note-owned, so the cascade is correct there (contrast SourceMaterial/Part below, which
        // refuse instead). Keeping the cascade in Core means no caller can bypass it.
        _storyService.DeleteNote(note.Id);
        _registry.AllNoteViewModels.Remove(note);
        await _storyService.SaveAsync();
    }

    public async Task<bool> TryDeleteLinkAsync(PlotPointSubjectLinkViewModel link)
    {
        if (ContentIntegrity.HasNotes(_storyService, link.Id, OwnerType.PlotPointSubjectLink)) return false;

        // StoryService.DeleteLink removes the link's owned NarrativePropertyValue rows with it.
        _storyService.DeleteLink(link.Id);
        _registry.AllPlotPointSubjectLinkViewModels.Remove(link);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteSubjectAsync(SubjectViewModel subject)
    {
        bool hasNotes = ContentIntegrity.HasNotes(_storyService, subject.Id, OwnerType.Subject);

        bool hasLinks = _storyService.PlotPointsSubjectLinks
            .Any(l => l.SubjectId == subject.Id);

        // A subject designated as someone's POV is a peer reference, not a container
        // relationship — refuse rather than nulling PlotPoint.FocalCharacterId out from under it.
        bool isFocalCharacter = _storyService.PlotPoints
            .Any(p => p.FocalCharacterId == subject.Id);

        if (hasNotes || hasLinks || isFocalCharacter) return false;

        _storyService.RemoveOwnedNarrativePropertyValues(subject.Id, OwnerType.Subject);

        _storyService.Subjects.Remove(subject.Subject);
        _registry.AllSubjectViewModels.Remove(subject);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeletePlotPointAsync(PlotPointViewModel plotPoint)
    {
        bool hasNotes = ContentIntegrity.HasNotes(_storyService, plotPoint.Id, OwnerType.PlotPoint);

        bool hasLinks = _storyService.PlotPointsSubjectLinks
            .Any(l => l.PlotPointId == plotPoint.Id);

        if (hasNotes || hasLinks) return false;

        _storyService.RemoveOwnedNarrativePropertyValues(plotPoint.Id, OwnerType.PlotPoint);

        _storyService.PlotPoints.Remove(plotPoint.PlotPoint);
        _registry.AllPlotPointViewModels.Remove(plotPoint);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteChapterAsync(ChapterViewModel chapter)
    {
        if (ContentIntegrity.HasNotes(_storyService, chapter.Id, OwnerType.Chapter)) return false;

        // Orphan plot points — set ChapterId to null, do not delete
        var ownedPlotPoints = _storyService.PlotPoints
            .Where(pp => pp.ChapterId == chapter.Id)
            .ToList();
        foreach (var pp in ownedPlotPoints)
            pp.ChapterId = null;

        _storyService.RemoveOwnedNarrativePropertyValues(chapter.Id, OwnerType.Chapter);

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

    public async Task<bool> TryDeleteThemeAsync(ThemeViewModel theme)
    {
        // Note.ThemeId is a raw id — deleting a tagged theme would silently erase the tagging
        // work (the notes would start appearing in "notes without theme" as if never tagged).
        if (ContentIntegrity.ThemeHasNotes(_storyService, theme.Id)) return false;

        _storyService.Themes.Remove(theme.Model);
        _registry.AllThemeViewModels.Remove(theme);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteSubjectDefinitionAsync(SubjectDefinitionViewModel definition)
    {
        // Type Object row — subjects, note tracks, and narrative properties are all scoped by it.
        if (ContentIntegrity.SubjectDefinitionHasDependents(_storyService, definition.Id)) return false;

        _storyService.SubjectDefinitions.Remove(definition.Model);
        _registry.AllSubjectDefinitionViewModels.Remove(definition);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteNoteTrackDefinitionAsync(NoteTrackDefinitionViewModel definition)
    {
        // A note keeps its NoteTrackDefinitionId when the track row vanishes: the categorization
        // is lost by reference, and a condition-track note's date semantics silently flip
        // (event-vs-condition lives on the track). Refuse while any note carries the id.
        if (ContentIntegrity.NoteTrackDefinitionHasNotes(_storyService, definition.Id)) return false;

        _storyService.NoteTrackDefinitions.Remove(definition.Model);
        _registry.AllNoteTrackDefinitionViewModels.Remove(definition);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task DeleteTheaterAsync(Theater theater)
    {
        // Orphan members back to "(Unplaced)" (sentinel 0) — never refuse, never cascade.
        // Same shape as TryDeleteStoryAsync's StoryId sentinel.
        foreach (var s in _storyService.Subjects.Where(s => s.TheaterId == theater.Id)) s.TheaterId = 0;
        foreach (var p in _storyService.PlotPoints.Where(p => p.TheaterId == theater.Id)) p.TheaterId = 0;
        _storyService.Theaters.Remove(theater);
        await _storyService.SaveAsync();
    }

    public async Task DeletePivotAsync(Pivot pivot)
    {
        // Eras are derived as the gaps between pivots, never stored — removing a pivot
        // orphans nothing, so this is unconditional.
        _storyService.Pivots.Remove(pivot);
        await _storyService.SaveAsync();
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

    public async Task<bool> TryDeleteWorkPhaseAsync(WorkPhaseViewModel phase)
    {
        if (ContentIntegrity.WorkPhaseHasDependents(_storyService, phase.Id)) return false;

        _storyService.WorkPhases.Remove(phase.Model);
        _registry.AllWorkPhaseViewModels.Remove(phase);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteNarrativePropertyDefinitionAsync(NarrativePropertyDefinitionViewModel property)
    {
        if (ContentIntegrity.NarrativePropertyDefinitionHasDependents(_storyService, property.Id)) return false;

        // Its allowed values go with it — they are meaningless without the property and are
        // provably unassigned, since the guard above just established that. This is the one place
        // a cascade is correct rather than a refusal: nothing authored is being discarded.
        var ownedValues = _storyService.NarrativePropertyValueDefinitions
            .Where(v => v.NarrativePropertyDefinitionId == property.Id)
            .ToList();

        foreach (var value in ownedValues)
        {
            var vm = _registry.AllNarrativePropertyValueDefinitionViewModels.FirstOrDefault(x => x.Id == value.Id);
            if (vm is not null) _registry.AllNarrativePropertyValueDefinitionViewModels.Remove(vm);
            _storyService.NarrativePropertyValueDefinitions.Remove(value);
        }

        // The entity-editor picker binds NarrativePropertyValueViewModels from this parallel
        // collection, so it has to be pruned too or a deleted value keeps appearing in dropdowns.
        foreach (var stale in _registry.AllNarrativePropertyValueDefinitions
                     .Where(v => ownedValues.Any(o => o.Id == v.Id)).ToList())
            _registry.AllNarrativePropertyValueDefinitions.Remove(stale);

        _storyService.NarrativePropertyDefinitions.Remove(property.Model);
        _registry.AllNarrativePropertyDefinitionViewModels.Remove(property);
        await _storyService.SaveAsync();
        return true;
    }

    public async Task<bool> TryDeleteNarrativePropertyValueDefinitionAsync(NarrativePropertyValueDefinitionViewModel value)
    {
        if (ContentIntegrity.NarrativePropertyValueDefinitionHasAssignments(_storyService, value.Id)) return false;

        var stale = _registry.AllNarrativePropertyValueDefinitions.FirstOrDefault(v => v.Id == value.Id);
        if (stale is not null) _registry.AllNarrativePropertyValueDefinitions.Remove(stale);

        _storyService.NarrativePropertyValueDefinitions.Remove(value.Model);
        _registry.AllNarrativePropertyValueDefinitionViewModels.Remove(value);
        await _storyService.SaveAsync();
        return true;
    }
}
