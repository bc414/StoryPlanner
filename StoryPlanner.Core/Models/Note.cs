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

    /// <summary>
    /// LEGACY free-text date, kept only until the convert-world-dates DataOps op has run on
    /// every real file; the op moves its content into the structured columns below and blanks
    /// it. New code never writes this. Drop the column in a later migration once both real
    /// files are converted.
    /// </summary>
    public string WorldDate { get; set; } = string.Empty;

    // Structured world date (see StoryPlanner.Core.WorldDate). All-null = undated. Whether
    // this is an event (start only) or a condition (start..end) is the TRACK's shape
    // (NoteTrackDefinition.SupportsWorldDateEnd), never stored per note. Year is the precision
    // floor; month/day are progressive pinning, null = to be determined.
    public int? WorldDateStartYear { get; set; }
    public int? WorldDateStartMonth { get; set; }
    public int? WorldDateStartDay { get; set; }
    public int? WorldDateEndYear { get; set; }
    public int? WorldDateEndMonth { get; set; }
    public int? WorldDateEndDay { get; set; }

    public int? ThemeId { get; set; }
    public int? SourceMaterialId { get; set; }
}

public enum NoteState
{
    Unset,     // captured but not reviewed; provisional
    Flagged,   // reviewed and found to require new research; FlagReason should be populated
    Confirmed  // stable within current structural context; safe for downstream design work
}