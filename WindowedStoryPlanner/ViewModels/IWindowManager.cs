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

    void OpenThemeWindow(ThemeViewModel theme);

    void OpenSourceMaterialWindow(SourceMaterialViewModel sourceMaterial);
    void OpenConversationReaderWindow(ConversationViewModel conversation);
}
