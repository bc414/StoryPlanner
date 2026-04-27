using StoryPlanner.Core.Models;
using System.Threading.Tasks;

namespace WindowedStoryPlanner.ViewModels
{
    public interface IEditorCoordinator
    {
        void OpenEditorWindow(OwnerViewModel viewModel);

        /// <summary>
        /// Creates a Note model + NoteViewModel atomically and registers both
        /// in IStoryService and AllNoteViewModels. Returns the new ViewModel.
        /// </summary>
        Task<NoteViewModel> CreateNoteAsync(int ownerId, int noteTrackDefinitionId, int sortOrder);

        /// <summary>
        /// Creates a PlotPoint model + ViewModel atomically and registers both
        /// in IStoryService and AllPlotPointViewModels. Returns the new ViewModel.
        /// </summary>
        Task<PlotPointViewModel> CreatePlotPointAsync(int? chapterId, int orderInChapter);

        /// <summary>
        /// Creates a PlotPointSubjectLink model + ViewModel atomically and registers
        /// both in IStoryService and AllPlotPointSubjectLinkViewModels.
        /// No-ops if the link already exists.
        /// </summary>
        Task CreatePlotPointSubjectLinkAsync(PlotPointViewModel plotPoint, SubjectViewModel subject);
    }
}
