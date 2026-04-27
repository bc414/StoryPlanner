using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WindowedStoryPlanner.ViewModels
{
    public interface IViewModelRegistry
    {
        ObservableCollection<SubjectViewModel> AllSubjectViewModels { get; }
        ObservableCollection<PlotPointViewModel> AllPlotPointViewModels { get; }
        ObservableCollection<PlotPointSubjectLinkViewModel> AllPlotPointSubjectLinkViewModels { get; }
        ObservableCollection<ChapterViewModel> AllChapterViewModels { get; }
        ObservableCollection<NoteViewModel> AllNoteViewModels { get; }
        ObservableCollection<NarrativePropertyValue> AllNarrativePropertyValues { get; }
        ObservableCollection<NarrativePropertyValueViewModel> AllNarrativePropertyValueDefinitions { get; }

        /// <summary>
        /// Raised when a NoteViewModel's filter-relevant properties (OwnerId,
        /// NoteTrackDefinitionId, NoteState, SortOrder) have been mutated.
        /// The int argument is the affected Note.Id.
        /// All active NoteTrackSectionViewModels should refresh their views in response.
        /// </summary>
        event Action<int> NoteViewModelMutated;

        /// <summary>
        /// Broadcasts NoteViewModelMutated to all current subscribers.
        /// Call this from NoteTrackSectionViewModel after completing a drop mutation.
        /// </summary>
        void RaiseNoteMutated(int noteId);
    }
}
