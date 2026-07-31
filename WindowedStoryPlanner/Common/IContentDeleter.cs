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
    /// Deletes a SourceMaterial (Work) if it has no Parts and no direct citations.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteSourceMaterialAsync(SourceMaterialViewModel work);

    /// <summary>
    /// Deletes a SourceMaterialPart if no note still cites it.
    /// Returns false and takes no action if the precondition is not met.
    /// </summary>
    Task<bool> TryDeleteSourceMaterialPartAsync(SourceMaterialPartViewModel part);
}
