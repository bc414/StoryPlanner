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

/// <summary>What a hover popup — or a pinned card — displays. One note for a condition bar,
/// every note in the cell for an event cell.</summary>
public sealed class CardContent
{
    public string Title { get; init; } = "";
    public string Subtitle { get; init; } = "";
    public IReadOnlyList<NoteCard> Notes { get; init; } = [];

    /// <summary>
    /// False when the card holds a single note the title already names — a condition bar or a
    /// placed marker — so the body is just the prose. True for a cell, where each note needs
    /// its own subject line to be told apart. Nothing should be stated twice on one card.
    /// </summary>
    public bool ShowNoteHeaders { get; init; } = true;

    public bool HasSubtitle => Subtitle.Trim().Length > 0;
}
