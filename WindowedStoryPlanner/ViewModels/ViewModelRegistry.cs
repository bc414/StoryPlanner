using System;
using System.Collections.ObjectModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Pure collection store for the lifetime of the application.
/// No factory dependencies — ProjectLoader is solely responsible for populating it.
/// </summary>
public class ViewModelRegistry : IViewModelRegistry
{
    public ObservableCollection<SubjectViewModel> AllSubjectViewModels { get; } = new();
    public ObservableCollection<PlotPointViewModel> AllPlotPointViewModels { get; } = new();
    public ObservableCollection<PlotPointSubjectLinkViewModel> AllPlotPointSubjectLinkViewModels { get; } = new();
    public ObservableCollection<ChapterViewModel> AllChapterViewModels { get; } = new();
    public ObservableCollection<NoteViewModel> AllNoteViewModels { get; } = new();
    public ObservableCollection<NarrativePropertyValue> AllNarrativePropertyValues { get; set; } = new();
    public ObservableCollection<NarrativePropertyValueViewModel> AllNarrativePropertyValueDefinitions { get; } = new();
    public ObservableCollection<SubjectDefinitionViewModel> AllSubjectDefinitionViewModels { get; } = new();
    public ObservableCollection<NoteTrackDefinitionViewModel> AllNoteTrackDefinitionViewModels { get; } = new();
    public ObservableCollection<ThemeViewModel> AllThemeViewModels { get; } = new();

    public event Action<int>? NoteViewModelMutated;
    public void RaiseNoteMutated(int noteId) => NoteViewModelMutated?.Invoke(noteId);

    public event Action<int>? NoteSelected;
    public void RaiseNoteSelected(int noteId) => NoteSelected?.Invoke(noteId);

    public event Action? LinksInvalidated;
    public void RaiseLinksInvalidated() => LinksInvalidated?.Invoke();

    public void Clear()
    {
        AllSubjectViewModels.Clear();
        AllPlotPointViewModels.Clear();
        AllPlotPointSubjectLinkViewModels.Clear();
        AllChapterViewModels.Clear();
        AllNoteViewModels.Clear();
        AllNarrativePropertyValueDefinitions.Clear();
        AllSubjectDefinitionViewModels.Clear();
        AllNoteTrackDefinitionViewModels.Clear();
        AllThemeViewModels.Clear();
        // AllNarrativePropertyValues is not cleared here — it is replaced by
        // reference in ProjectLoader since it is a direct alias to the service list.
    }
}
