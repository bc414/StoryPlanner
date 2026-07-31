using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core;

public class PlotPoint : INoteable
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    // --- Relationships ---
    public int? ChapterId { get; set; }
    public int OrderInChapter { get; set; }

    /// <summary>Theater the scene is set in. 0 = the permanent "(Unplaced)" sentinel, same
    /// pattern as Chapter.StoryId — a legal long-lived state, not a missing reference.</summary>
    public int TheaterId { get; set; }

    // Fabula date — when this scene happens in world time. EVENT ONLY, never an interval: a
    // plot point that wants a span is holding more than one scene (the single-responsibility
    // violation conv:21 diagnosed); duration, where it is the claim, belongs on a subject
    // condition track. A plot point thus carries two independent temporal coordinates: this
    // fabula date, and its syuzhet position (ChapterId + OrderInChapter) — their divergence is
    // flashback/non-linear telling, made visible on the timeline. All-null = undated (triage).
    public int? FabulaYear { get; set; }
    public int? FabulaMonth { get; set; }
    public int? FabulaDay { get; set; }

    public OwnerType OwnerType => OwnerType.PlotPoint;

    public int GetTotalTextLength()
    {
        return GetCombinedText().Length;
    }

    public string GetCombinedText()
    {
        return string.Empty;
    }
}