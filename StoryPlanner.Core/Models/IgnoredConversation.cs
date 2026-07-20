namespace StoryPlanner.Core.Models;

/// <summary>
/// A Claude conversation the user has explicitly marked as not story-related (programming
/// questions, general chat, etc.). Keeps it out of the "New" bucket on every future scan so
/// repeat scans don't keep re-surfacing off-topic conversations for export to Cowork.
/// </summary>
public class IgnoredConversation
{
    public int    Id         { get; set; }
    public string SourceUuid { get; set; } = string.Empty;
    public string Title      { get; set; } = string.Empty; // snapshot, for display only
}
