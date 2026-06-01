using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json.Serialization;
using System;

namespace StoryPlanner.Core.Models;

public class Note
{
    public int Id { get; set; }

    public int OwnerId { get; set; }
    public OwnerType OwnerType { get; set; }

    public int? NoteTrackDefinitionId { get; set; }

    public DateTime LastModified { get; set; }

    public string Content { get; set; } = string.Empty;

    public NoteState NoteState { get; set; }
    public string FlagReason { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public string WorldDate { get; set; } = string.Empty; // year or year range allowed
    public int? ThemeId { get; set; }
}

public enum NoteState
{
    Unset,     // captured but not reviewed; provisional
    Flagged,   // reviewed and found to require new research; FlagReason should be populated
    Confirmed  // stable within current structural context; safe for downstream design work
}