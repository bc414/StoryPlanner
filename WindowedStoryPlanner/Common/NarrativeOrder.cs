using System.Collections.Generic;
using System.Linq;

namespace WindowedStoryPlanner;

/// <summary>
/// Reading-order sorts for chapters and plot points, in one place because every picker whose list
/// can span more than one story needs the same answer (2026-08-06: the plot point picker's Story
/// level and the chapter picker both wanted it). There are no navigation properties, so "which
/// story is this chapter in" is a dictionary built once per sort here — a per-item
/// <c>FirstOrDefault</c> would re-scan on every keystroke in a picker's search box.
/// </summary>
public static class NarrativeOrder
{
    /// <summary>Chapters by (story reading order, chapter number). Story 0 is "(Unassigned)" and
    /// has no <c>Stories</c> row, so it falls to 0 — the same treatment the Chapters tab's sort
    /// gives it.</summary>
    public static IEnumerable<ChapterViewModel> Chapters(
        IViewModelRegistry? registry, IEnumerable<ChapterViewModel> chapters)
    {
        var storyOrder = StoryOrder(registry);
        return chapters
            .OrderBy(c => storyOrder.TryGetValue(c.StoryId, out var o) ? o : 0)
            .ThenBy(c => c.OrderIndex);
    }

    /// <summary>Plot points by (story, chapter, position in chapter). Ordering by
    /// <c>OrderInChapter</c> alone stacks every chapter's "1." together the moment the set spans
    /// more than one chapter. A plot point with no chapter sorts last.</summary>
    public static IEnumerable<PlotPointViewModel> PlotPoints(
        IViewModelRegistry? registry, IEnumerable<PlotPointViewModel> plotPoints)
    {
        var chapterKey = ChapterKeys(registry);
        return plotPoints
            .OrderBy(p => p.ChapterId is { } id && chapterKey.TryGetValue(id, out var key) ? key : Unchaptered)
            .ThenBy(p => p.OrderInChapter);
    }

    private static readonly (int Story, int Chapter) Unchaptered = (int.MaxValue, int.MaxValue);

    private static Dictionary<int, int> StoryOrder(IViewModelRegistry? registry) =>
        registry?.AllStoryViewModels.ToDictionary(s => s.Id, s => s.OrderIndex) ?? new();

    private static Dictionary<int, (int Story, int Chapter)> ChapterKeys(IViewModelRegistry? registry)
    {
        var storyOrder = StoryOrder(registry);
        return registry?.AllChapterViewModels.ToDictionary(
                   c => c.Id,
                   c => (storyOrder.TryGetValue(c.StoryId, out var o) ? o : 0, c.OrderIndex))
               ?? new();
    }
}
