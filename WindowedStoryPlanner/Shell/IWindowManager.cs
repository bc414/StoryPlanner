using StoryPlanner.Core;

namespace WindowedStoryPlanner;

public interface IWindowManager
{
    /// <summary>
    /// Opens a subject's editor. CommonWindow's primary element is a SubjectViewModel in every
    /// mode this accepts — passing a plot point instead used to be a compile-clean unchecked cast
    /// and a hard process kill. Split into two typed methods so the pairing is the compiler's job.
    /// </summary>
    /// <param name="mode">Expansion or Linking. Gardener belongs to <see cref="OpenPlotPointWindow"/>.</param>
    /// <param name="initialLink">Link to preselect — the point of Linking mode.</param>
    void OpenSubjectWindow(
        SubjectViewModel subject,
        EditorMode mode = EditorMode.Expansion,
        PlotPointSubjectLinkViewModel? initialLink = null);

    /// <summary>Opens a plot point's editor (Gardener — the only mode whose primary element is one).</summary>
    void OpenPlotPointWindow(PlotPointViewModel plotPoint);

    void OpenChapterWindow(ChapterViewModel chapter);

    void OpenFloatingPlotPointsWindow(FloatingPlotPointsViewModel vm);

    void OpenThemeWindow(ThemeViewModel theme);

    void OpenSourceMaterialWindow(SourceMaterialViewModel sourceMaterial);
    void OpenSourceMaterialPartWindow(SourceMaterialPartViewModel part);
    void OpenDateRangeWindow();
    void OpenMissingFieldWindow(MissingNoteField field);
    void OpenConversationReaderWindow(ConversationViewModel conversation);
}
