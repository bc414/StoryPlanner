using System.Threading.Tasks;

namespace WindowedStoryPlanner.ViewModels;

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
}
