using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// Display wrapper for one StoryPlanner.Core.SearchHit. Resolves nothing itself —
/// GlobalSearchViewModel does the registry lookups and passes in the already-resolved
/// display strings, per CLAUDE.md's "models are row vessels, view models do relationship
/// work" rule. Immutable: a new list is built on every RebuildResults() rather than mutating
/// these in place.
/// </summary>
public class SearchResultViewModel
{
    public SearchHitKind Kind { get; }
    public int Id { get; }
    public string MatchedField { get; }
    public string Snippet { get; }
    public string TypeLabel { get; }

    /// <summary>Entity name/title for most kinds; the owner breadcrumb ("Subject — Chrysalis")
    /// for a Note, since a note has no title of its own.</summary>
    public string Title { get; }

    /// <summary>Second display line — null except for Note, where it's the track name.</summary>
    public string? Subtitle { get; }

    /// <summary>NoteViewModel.StateLabel glyph ("✓"/"⚑"/"–"); empty for non-note kinds.</summary>
    public string StateGlyph { get; }

    public bool IsFlagged { get; }

    public SearchResultViewModel(
        SearchHit hit,
        string typeLabel,
        string title,
        string? subtitle,
        string stateGlyph,
        bool isFlagged)
    {
        Kind = hit.Kind;
        Id = hit.Id;
        MatchedField = hit.MatchedField;
        Snippet = hit.Snippet;
        TypeLabel = typeLabel;
        Title = title;
        Subtitle = subtitle;
        StateGlyph = stateGlyph;
        IsFlagged = isFlagged;
    }
}
