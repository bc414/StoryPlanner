using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

public class Note
{
    public int Id { get; set; }

    public int OwnerId { get; set; }
    public int NoteTrackDefinitionId { get; set; }

    public DateTime LastModified { get; set; }

    
    public string Content { get; set; } = string.Empty;

    public NoteState NoteState { get; set; }
    public string FlagReason { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public string WorldDate { get; set; } = string.Empty; //year or year range allowed
}

public enum NoteState
{
    Unset, //capatured but not reviewed, provisional, not to be used as a axiom for other worldbuilding yet
    Flagged, //reviewed and found to require new research. FlagReason should be populated
    Confirmed //Stable within current structural context; safe for downstream design work to depend on (but can still change if a better idea emerges)
}