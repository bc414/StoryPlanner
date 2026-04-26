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
    }
}
