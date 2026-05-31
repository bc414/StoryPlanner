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

    ObservableCollection<SubjectDefinitionViewModel> AllSubjectDefinitionViewModels { get; }
    ObservableCollection<NoteTrackDefinitionViewModel> AllNoteTrackDefinitionViewModels { get; }
    ObservableCollection<ThemeViewModel> AllThemeViewModels { get; }

    event Action<int> NoteViewModelMutated;
    void RaiseNoteMutated(int noteId);

    // Broadcast when any section selects a note so all other sections
    // can clear their own selection, enforcing a single selection across
    // the entire NarrativeElementFullView.
    event Action<int> NoteSelected;
    void RaiseNoteSelected(int noteId);

    /// <summary>
    /// True after <see cref="RaiseStoryLoaded"/> has been called at least once.
    /// Consumers that are created post-load can read this to skip deferral logic.
    /// </summary>
    bool IsStoryLoaded { get; }

    /// <summary>
    /// Raised once after all view models have been bulk-loaded from storage.
    /// Subscribers that are too expensive to update per-note during load
    /// should defer their initial work until this fires.
    /// </summary>
    event Action StoryLoaded;
    void RaiseStoryLoaded();

    void Clear();
    event Action LinksInvalidated;
    void RaiseLinksInvalidated();
}
