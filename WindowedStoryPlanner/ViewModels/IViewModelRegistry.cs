using StoryPlanner.Core.Models;
using System;
using System.Collections.ObjectModel;

namespace WindowedStoryPlanner.ViewModels;

public interface IViewModelRegistry
{
    ObservableCollection<SubjectViewModel> AllSubjectViewModels { get; }
    ObservableCollection<PlotPointViewModel> AllPlotPointViewModels { get; }
    ObservableCollection<PlotPointSubjectLinkViewModel> AllPlotPointSubjectLinkViewModels { get; }
    ObservableCollection<ChapterViewModel> AllChapterViewModels { get; }
    ObservableCollection<NoteViewModel> AllNoteViewModels { get; }
    ObservableCollection<NarrativePropertyValue> AllNarrativePropertyValues { get; set; }
    ObservableCollection<NarrativePropertyValueViewModel> AllNarrativePropertyValueDefinitions { get; }

    // Promoted from DefinitionsEditorViewModel — registry owns these like every other VM collection
    ObservableCollection<SubjectDefinitionViewModel> AllSubjectDefinitionViewModels { get; }
    ObservableCollection<NoteTrackDefinitionViewModel> AllNoteTrackDefinitionViewModels { get; }

    event Action<int> NoteViewModelMutated;
    void RaiseNoteMutated(int noteId);
    void Clear();
    event Action LinksInvalidated;
    void RaiseLinksInvalidated();
}
