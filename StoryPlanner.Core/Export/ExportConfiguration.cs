using System.Collections.Generic;
using System.Linq;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core.Export;

public class ExportConfiguration
{
    public List<(int Id, OwnerType OwnerType)> Anchors { get; set; } = new();
    public int Scope { get; set; }
    public int? ChapterFrom { get; set; }
    public int? ChapterTo { get; set; }
    public HashSet<TrackType> IncludedTrackTypes { get; set; }
        = new(System.Enum.GetValues<TrackType>().Where(t => t != TrackType.Unset));
}
