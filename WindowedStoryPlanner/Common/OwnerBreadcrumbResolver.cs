using System.Linq;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// Resolves a Note's (OwnerId, OwnerType) to a human-readable "where does this
/// note actually live" label, for use in cross-cutting views (Theme/SourceMaterial
/// detail windows) where notes from many different owners are shown together.
/// </summary>
public static class OwnerBreadcrumbResolver
{
    public static string Resolve(int ownerId, OwnerType ownerType, IViewModelRegistry registry)
    {
        switch (ownerType)
        {
            case OwnerType.Chapter:
                var chapter = registry.AllChapterViewModels.FirstOrDefault(c => c.Id == ownerId);
                return chapter is not null ? $"Chapter {chapter.OrderIndex}. {chapter.Title}" : $"Chapter #{ownerId}";

            case OwnerType.Subject:
                var subject = registry.AllSubjectViewModels.FirstOrDefault(s => s.Id == ownerId);
                return subject is not null ? $"Subject — {subject.Name}" : $"Subject #{ownerId}";

            case OwnerType.PlotPoint:
                var plotPoint = registry.AllPlotPointViewModels.FirstOrDefault(p => p.Id == ownerId);
                return plotPoint is not null ? $"PlotPoint {plotPoint.FullOrder}{plotPoint.Title}" : $"PlotPoint #{ownerId}";

            case OwnerType.PlotPointSubjectLink:
                var link = registry.AllPlotPointSubjectLinkViewModels.FirstOrDefault(l => l.Id == ownerId);
                return link is not null
                    ? $"Link — {link.PlotPointDisplayText} ↔ {link.SubjectName}"
                    : $"Link #{ownerId}";

            default:
                return $"{ownerType} #{ownerId}";
        }
    }
}
