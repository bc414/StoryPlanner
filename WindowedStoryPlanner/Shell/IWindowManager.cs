using StoryPlanner.Core;

namespace WindowedStoryPlanner;

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
    void OpenSourceMaterialPartWindow(SourceMaterialPartViewModel part);
    void OpenConversationReaderWindow(ConversationViewModel conversation);
}
