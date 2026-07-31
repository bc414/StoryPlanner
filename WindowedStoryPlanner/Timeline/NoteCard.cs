using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowedStoryPlanner;

/// <summary>One note (or plot point) rendered in full: subject, provenance, complete content,
/// and — for a flagged note — its flag reason, which often carries more substance than the body
/// it qualifies. The app shows Brian everything; the flagged wall governs export and the MCP
/// server, not this surface.</summary>
public sealed class NoteCard
{
    public string Subject { get; init; } = "";
    public string Meta { get; init; } = "";
    public string Content { get; init; } = "";
    public string FlagReason { get; init; } = "";
    public bool IsFlagged { get; init; }
    public bool IsPlotPoint { get; init; }
    public string Fill { get; init; } = "#888888";

    /// <summary>The live view model behind this note, so a card can edit the date in place
    /// through exactly the same validated setter the note view uses. Null for plot points.</summary>
    public NoteViewModel? Editable { get; init; }

    /// <summary>Invoked after an in-card date edit: saves and re-places the mark.</summary>
    public ICommand? SaveCommand { get; init; }

    public bool CanEditDate => Editable is not null;
    public bool HasContent => Content.Trim().Length > 0;
    public bool HasFlagReason => FlagReason.Trim().Length > 0;
}
