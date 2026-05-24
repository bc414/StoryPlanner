using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public interface IWindowManager
{
    void OpenCommonWindow(
        EditorMode mode,
        NarrativeElementViewModel element,
        PlotPointSubjectLinkViewModel? initialLink = null);

    void OpenChapterWindow(ChapterViewModel chapter);

    void OpenFloatingPlotPointsWindow(FloatingPlotPointsViewModel vm);
}
