using System.Collections.Generic;
using System.Linq;
using StoryPlanner.Core;

namespace StoryPlanner.Core;

public class ExportConfiguration
{
    public List<(int Id, OwnerType OwnerType)> Anchors { get; set; } = new();
    public int Scope { get; set; }

    /// <summary>Restricts ChapterFrom/ChapterTo to one story — without it, "chapters 1–5" would
    /// match five chapters in every story now that OrderIndex is per-story, not global.</summary>
    public int? StoryId { get; set; }
    public int? ChapterFrom { get; set; }
    public int? ChapterTo { get; set; }
    public HashSet<TrackType> IncludedTrackTypes { get; set; }
        = new(System.Enum.GetValues<TrackType>().Where(t => t != TrackType.Unset));
}
