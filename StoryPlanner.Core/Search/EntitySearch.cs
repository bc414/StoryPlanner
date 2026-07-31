using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryPlanner.Core;

/// <summary>
/// Cross-entity text search over a plan's in-memory collections. No ranking, no fuzzy
/// matching — plain case-insensitive substring match, one hit per entity (first matching
/// field wins, in the priority order below), fixed pass order by kind. Mirrors the settled
/// MCP server contract (dumb tools, caller supplies vocabulary) rather than inventing scoring.
///
/// Deliberately searches Note.Content and Note.FlagReason for FLAGGED notes too — unlike
/// NoteExportRenderer and the MCP server's Engine.Search, which wall flagged content from
/// anything an LLM consumes. This is the author reading their own data in their own app;
/// CLAUDE.md scopes the wall to "wherever an LLM consumes data," which this is not. See
/// EntitySearchTests for the tests that pin this as intentional, not a regression.
/// </summary>
public static class EntitySearch
{
    private const int MinQueryLength = 2;

    public static IReadOnlyList<SearchHit> Run(SearchInput input, string query, SearchHitKind? kindFilter = null)
    {
        query = query?.Trim() ?? string.Empty;
        if (query.Length < MinQueryLength) return Array.Empty<SearchHit>();

        var hits = new List<SearchHit>();

        if (kindFilter is null or SearchHitKind.Subject)
        {
            hits.AddRange(input.Subjects
                .OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase)
                .Select(s => MatchSubject(s, query))
                .OfType<SearchHit>());
        }

        if (kindFilter is null or SearchHitKind.PlotPoint)
        {
            hits.AddRange(input.PlotPoints
                .OrderBy(p => p.Title, StringComparer.OrdinalIgnoreCase)
                .Select(p => MatchField(SearchHitKind.PlotPoint, p.Id, "Title", p.Title, query))
                .OfType<SearchHit>());
        }

        if (kindFilter is null or SearchHitKind.Chapter)
        {
            hits.AddRange(input.Chapters
                .OrderBy(c => c.OrderIndex)
                .Select(c => MatchField(SearchHitKind.Chapter, c.Id, "Title", c.Title, query))
                .OfType<SearchHit>());
        }

        if (kindFilter is null or SearchHitKind.Theme)
        {
            hits.AddRange(input.Themes
                .OrderBy(t => t.Name, StringComparer.OrdinalIgnoreCase)
                .Select(t => MatchTheme(t, query))
                .OfType<SearchHit>());
        }

        if (kindFilter is null or SearchHitKind.SourceMaterial)
        {
            hits.AddRange(input.SourceMaterials
                .OrderBy(sm => sm.Name, StringComparer.OrdinalIgnoreCase)
                .Select(sm => MatchSourceMaterial(sm, query))
                .OfType<SearchHit>());
        }

        if (kindFilter is null or SearchHitKind.SourceMaterialPart)
        {
            hits.AddRange(input.SourceMaterialParts
                .OrderBy(p => p.OrderIndex)
                .Select(p => MatchSourceMaterialPart(p, query))
                .OfType<SearchHit>());
        }

        if (kindFilter is null or SearchHitKind.Note)
        {
            hits.AddRange(input.Notes
                .OrderBy(n => n.OwnerType)
                .ThenBy(n => n.OwnerId)
                .ThenBy(n => n.SortOrder)
                .Select(n => MatchNote(n, query))
                .OfType<SearchHit>());
        }

        return hits;
    }

    private static SearchHit? MatchSubject(Subject s, string query)
        => MatchField(SearchHitKind.Subject, s.Id, "Name", s.Name, query)
           ?? MatchField(SearchHitKind.Subject, s.Id, "Description", s.Description, query)
           ?? MatchField(SearchHitKind.Subject, s.Id, "Abbreviation", s.Abbreviation, query);

    private static SearchHit? MatchTheme(Theme t, string query)
        => MatchField(SearchHitKind.Theme, t.Id, "Name", t.Name, query)
           ?? MatchField(SearchHitKind.Theme, t.Id, "Proposition", t.Proposition, query);

    private static SearchHit? MatchSourceMaterial(SourceMaterial sm, string query)
        => MatchField(SearchHitKind.SourceMaterial, sm.Id, "Name", sm.Name, query)
           ?? MatchField(SearchHitKind.SourceMaterial, sm.Id, "Description", sm.Description, query);

    private static SearchHit? MatchSourceMaterialPart(SourceMaterialPart p, string query)
        => MatchField(SearchHitKind.SourceMaterialPart, p.Id, "Code", p.Code, query)
           ?? MatchField(SearchHitKind.SourceMaterialPart, p.Id, "Name", p.Name, query)
           ?? MatchField(SearchHitKind.SourceMaterialPart, p.Id, "Description", p.Description, query);

    private static SearchHit? MatchNote(Note n, string query)
        => MatchField(SearchHitKind.Note, n.Id, "Content", n.Content, query)
           ?? MatchField(SearchHitKind.Note, n.Id, "FlagReason", n.FlagReason, query);

    private static SearchHit? MatchField(SearchHitKind kind, int id, string fieldName, string? value, string query)
    {
        if (string.IsNullOrEmpty(value)) return null;
        var index = value.IndexOf(query, StringComparison.OrdinalIgnoreCase);
        if (index < 0) return null;
        var snippet = TextSnippet.Around(value, index, query.Length);
        return new SearchHit(kind, id, fieldName, snippet);
    }
}
