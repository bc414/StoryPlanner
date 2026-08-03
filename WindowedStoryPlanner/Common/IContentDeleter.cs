using System.Threading.Tasks;

namespace WindowedStoryPlanner;

public interface IContentDeleter
{
    /// <summary>
    /// Deletes a note unconditionally from the service and registry.
    /// </summary>
    Task DeleteNoteAsync(NoteViewModel note);

    /// <summary>
    /// Deletes a PlotPointSubjectLink if it has no notes.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteLinkAsync(PlotPointSubjectLinkViewModel link);

    /// <summary>
    /// Deletes a Subject if it has no notes and no remaining links.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteSubjectAsync(SubjectViewModel subject);

    /// <summary>
    /// Deletes a PlotPoint if it has no notes and no remaining links.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeletePlotPointAsync(PlotPointViewModel plotPoint);

    /// <summary>
    /// Deletes a Chapter if it has no notes.
    /// Its plot points are orphaned (ChapterId set to null) — not deleted.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteChapterAsync(ChapterViewModel chapter);

    /// <summary>
    /// Deletes a Story. Story is container-only (owns no notes), so this always succeeds —
    /// its chapters are orphaned to the "(Unassigned)" sentinel (StoryId = 0), never cascaded
    /// or refused. The bool return matches the other TryDelete*Async guards' shape.
    /// </summary>
    Task<bool> TryDeleteStoryAsync(StoryViewModel story);

    /// <summary>
    /// Deletes a Theme if no note is tagged with it.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteThemeAsync(ThemeViewModel theme);

    /// <summary>
    /// Deletes a SubjectDefinition if nothing depends on it: no subjects typed by it, no
    /// note-track or narrative-property definitions scoped to it.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteSubjectDefinitionAsync(SubjectDefinitionViewModel definition);

    /// <summary>
    /// Deletes a NoteTrackDefinition if no note carries its id.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteNoteTrackDefinitionAsync(NoteTrackDefinitionViewModel definition);

    /// <summary>
    /// Deletes a Theater, orphaning its subjects and plot points back to the "(Unplaced)"
    /// sentinel (TheaterId = 0). Never refuses — same shape as TryDeleteStoryAsync.
    /// </summary>
    Task DeleteTheaterAsync(StoryPlanner.Core.Theater theater);

    /// <summary>
    /// Deletes a Pivot unconditionally — eras are derived as the gaps between pivots,
    /// never stored, so removing one orphans nothing.
    /// </summary>
    Task DeletePivotAsync(StoryPlanner.Core.Pivot pivot);

    /// <summary>
    /// Deletes a SourceMaterial (Work) if it has no Parts and no direct citations.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteSourceMaterialAsync(SourceMaterialViewModel work);

    /// <summary>
    /// Deletes a SourceMaterialPart if no note still cites it.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteSourceMaterialPartAsync(SourceMaterialPartViewModel part);

    /// <summary>
    /// Deletes a WorkPhase if no narrative property definition gates on it.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteWorkPhaseAsync(WorkPhaseViewModel phase);

    /// <summary>
    /// Deletes a NarrativePropertyDefinition, and its allowed values with it, if no owner has
    /// assigned any of those values. Returns false and takes no action otherwise.
    /// </summary>
    Task<bool> TryDeleteNarrativePropertyDefinitionAsync(NarrativePropertyDefinitionViewModel property);

    /// <summary>
    /// Deletes a NarrativePropertyValueDefinition if no owner has it assigned.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteNarrativePropertyValueDefinitionAsync(NarrativePropertyValueDefinitionViewModel value);
}
