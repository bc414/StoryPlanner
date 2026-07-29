using System.Linq;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core.Export;

public static class ExportResolver
{
    public static ExportResult Resolve(ExportConfiguration config, IStoryService storyService)
    {
        var result = new ExportResult();

        bool hasSubjectOrChapterAnchor = config.Anchors.Any(a =>
            a.OwnerType == OwnerType.Subject || a.OwnerType == OwnerType.Chapter);

        // Step 1: Chapter expansion (pre-scope, no scope level consumed)
        foreach (var anchor in config.Anchors.Where(a => a.OwnerType == OwnerType.Chapter))
        {
            foreach (var pp in storyService.PlotPoints.Where(p => p.ChapterId == anchor.Id))
                result.FullPlotPointIds.Add(pp.Id);
        }

        // Step 2: Explicit anchors
        foreach (var anchor in config.Anchors)
        {
            if (anchor.OwnerType == OwnerType.Subject)
                result.FullSubjectIds.Add(anchor.Id);
            else if (anchor.OwnerType == OwnerType.PlotPoint)
                result.FullPlotPointIds.Add(anchor.Id);
        }

        // Step 3: Scope 0 link bonus — include link when both Subject and PlotPoint are anchored
        foreach (var link in storyService.PlotPointsSubjectLinks)
        {
            if (result.FullPlotPointIds.Contains(link.PlotPointId) &&
                result.FullSubjectIds.Contains(link.SubjectId))
                result.ActiveLinks.Add((link.PlotPointId, link.SubjectId));
        }

        if (config.Scope >= 1)
        {
            // Step 4: Expand all links from full PlotPoints
            foreach (var link in storyService.PlotPointsSubjectLinks)
            {
                if (!result.FullPlotPointIds.Contains(link.PlotPointId)) continue;
                if (hasSubjectOrChapterAnchor && !IsInChapterRange(link.PlotPointId, config, storyService)) continue;
                result.ActiveLinks.Add((link.PlotPointId, link.SubjectId));
            }

            // Step 5: Expand from Subject anchors to thin PlotPoints
            foreach (var anchor in config.Anchors.Where(a => a.OwnerType == OwnerType.Subject))
            {
                foreach (var link in storyService.PlotPointsSubjectLinks.Where(l => l.SubjectId == anchor.Id))
                {
                    if (hasSubjectOrChapterAnchor && !IsInChapterRange(link.PlotPointId, config, storyService)) continue;
                    result.ActiveLinks.Add((link.PlotPointId, link.SubjectId));
                    if (!result.FullPlotPointIds.Contains(link.PlotPointId))
                        result.ThinPlotPointIds.Add(link.PlotPointId);
                }
            }
        }

        if (config.Scope >= 2)
        {
            // Step 6: Pull other-end entities to full depth
            // Subject anchors → linked PlotPoints promoted from thin to full
            foreach (var (plotPointId, _) in result.ActiveLinks)
                result.FullPlotPointIds.Add(plotPointId);
            // PlotPoint/Chapter anchors → linked Subjects get Part 1 profiles
            foreach (var (_, subjectId) in result.ActiveLinks)
                result.FullSubjectIds.Add(subjectId);
        }

        // Step 7: Deduplication — full wins over thin
        result.ThinPlotPointIds.ExceptWith(result.FullPlotPointIds);

        return result;
    }

    public static ExportResult ResolveAll(IStoryService storyService)
    {
        var result = new ExportResult();
        foreach (var s in storyService.Subjects)
            result.FullSubjectIds.Add(s.Id);
        foreach (var pp in storyService.PlotPoints)
            result.FullPlotPointIds.Add(pp.Id);
        foreach (var l in storyService.PlotPointsSubjectLinks)
            result.ActiveLinks.Add((l.PlotPointId, l.SubjectId));
        return result;
    }

    private static bool IsInChapterRange(int plotPointId, ExportConfiguration config, IStoryService storyService)
    {
        if (config.ChapterFrom == null && config.ChapterTo == null && config.StoryId == null)
            return true;

        var pp = storyService.PlotPoints.FirstOrDefault(p => p.Id == plotPointId);
        if (pp?.ChapterId == null) return false;

        var chapter = storyService.Chapters.FirstOrDefault(c => c.Id == pp.ChapterId.Value);
        if (chapter == null) return false;

        // ChapterFrom/ChapterTo are per-story OrderIndex values — without a StoryId they are
        // ambiguous across every story, so a range is only meaningful once scoped to one.
        if (config.StoryId.HasValue && chapter.StoryId != config.StoryId.Value) return false;
        if (config.ChapterFrom.HasValue && chapter.OrderIndex < config.ChapterFrom.Value) return false;
        if (config.ChapterTo.HasValue && chapter.OrderIndex > config.ChapterTo.Value) return false;

        return true;
    }
}
