namespace StoryPlanner.Core;

/// <summary>
/// One match from EntitySearch.Run. Deliberately thin — Id + Kind is enough for a caller
/// with access to the live collections (or view-model registry) to resolve everything else
/// (owner, track, breadcrumb). This type does no relationship work; see CLAUDE.md's
/// "models are row vessels, view models do relationship work" rule.
/// </summary>
public record SearchHit(SearchHitKind Kind, int Id, string MatchedField, string Snippet);
