using StoryPlanner.Core;
using System.ComponentModel;
using ModelContextProtocol.Server;

namespace StoryPlanner.Mcp;

/// <summary>
/// Tools over the ARCHIVE (v1). Same shapes as the *_plan family, different semantics:
/// v1 predates the track system (all notes are untracked), has a much richer scene graph
/// (1,125 plot-point×subject links), and its note states mean: open / flagged /
/// closed(disposition-not-recorded) — "closed" is "reviewed, no longer needs attention",
/// NOT "migrated to v2"; some closed content was deliberately superseded instead.
/// </summary>
[McpServerToolType]
public sealed class ArchiveTools(StoryPlanSources sources)
{
    private PlanCache C => sources.Get(Corpus.Archive);

    [McpServerTool(Name = "search_archive")]
    [Description("Regex search over the ARCHIVE (v1): non-flagged note content, subject names/descriptions, plot point titles, chapter titles, theme names/propositions. v1 is the older capture-era dataset — organized differently from v2 on purpose (no tracks; its subject 'types' are triage labels, not categories). Every hit carries the archive state label: open, or closed(disposition-not-recorded).")]
    public string SearchArchive(
        [Description("Regular expression (.NET syntax). Case-insensitive unless caseSensitive=true.")] string pattern,
        [Description("Match case-sensitively (default false).")] bool caseSensitive = false,
        [Description("Match whole words only (default false).")] bool wholeWord = false,
        [Description("Characters of context around each match snippet (default 150, max 2000).")] int contextChars = 150,
        [Description("Maximum hits returned (default 40, max 250). Totals are always reported.")] int limit = 40)
        => Engine.Search(C, pattern, caseSensitive, wholeWord, contextChars, limit);

    [McpServerTool(Name = "get_notes_archive")]
    [Description("Fetch full note content by note ids from the ARCHIVE (v1). 'closed(disposition-not-recorded)' means reviewed and done-with — migrated to v2 OR deliberately superseded; the data does not record which, so never treat closed content as current truth.")]
    public string GetNotesArchive(
        [Description("Note ids (archive id space — unrelated to working-plan ids).")] int[] ids)
        => Engine.GetNotes(C, ids);

    [McpServerTool(Name = "get_subjects_archive")]
    [Description("Fetch ARCHIVE (v1) subjects by id: record, note tallies, scene-link edge list in chapter order (v1 holds the rich scene graph), and (unless includeNotes=false) all non-flagged notes. v1 subject ids do NOT correspond to v2 subject ids.")]
    public string GetSubjectsArchive(
        [Description("Archive subject ids.")] int[] ids,
        [Description("Include full note content (default true).")] bool includeNotes = true)
        => Engine.GetSubjects(C, ids, includeNotes);

    [McpServerTool(Name = "get_plot_points_archive")]
    [Description("Fetch ARCHIVE (v1) plot points by id: title, chapter position, linked subjects (v1 has 1,125 links across 342 plot points), and (unless includeNotes=false) the plot point's non-flagged notes.")]
    public string GetPlotPointsArchive(
        [Description("Archive plot point ids.")] int[] ids,
        [Description("Include full note content (default true).")] bool includeNotes = true)
        => Engine.GetPlotPoints(C, ids, includeNotes);

    [McpServerTool(Name = "get_chapters_archive")]
    [Description("Fetch ARCHIVE (v1) chapters by id: chapter notes and the ordered plot point list. Pass an EMPTY ids array for the inventory of all 34 v1 chapters, grouped under story headings (see list_stories/get_stories — the archive's 3 stories are unrelated rows from v2's, never cross-referenced by name).")]
    public string GetChaptersArchive(
        [Description("Archive chapter ids. Empty array → one-line inventory of every chapter.")] int[] ids,
        [Description("Include full chapter-note content (default true).")] bool includeNotes = true)
        => Engine.GetChapters(C, ids, includeNotes);

    [McpServerTool(Name = "get_links_archive")]
    [Description("Fetch ARCHIVE (v1) plot-point×subject links by id — the scene-level payload layer ('what this scene does to this subject'). This graph exists mainly in v1; it has not yet been migrated to v2.")]
    public string GetLinksArchive(
        [Description("Archive link ids (from subject/plot point fetch edge lists).")] int[] ids,
        [Description("Include full note content (default true).")] bool includeNotes = true)
        => Engine.GetLinks(C, ids, includeNotes);

    [McpServerTool(Name = "get_theme_notes_archive")]
    [Description("All non-flagged ARCHIVE (v1) notes tagged with a theme (theme tagging is sparse in v1).")]
    public string GetThemeNotesArchive(
        [Description("Theme id or exact theme name (case-insensitive).")] string theme)
        => Engine.GetThemeNotes(C, theme);

    [McpServerTool(Name = "get_notes_in_date_range_archive")]
    [Description("Non-flagged ARCHIVE (v1) notes whose world date intersects [fromYear, toYear], both years inclusive, sorted chronologically. v1 has no tracks, so every date reads as an event; legacy free-text values are converted mechanically and unconvertible ones are counted, never guessed. Omit both bounds for the full archive chronology.")]
    public string GetNotesInDateRangeArchive(
        [Description("Start year (inclusive). Negative = before year 0. Omit for open start.")] int? fromYear = null,
        [Description("End year (inclusive). Omit for open end.")] int? toYear = null)
        => Engine.GetNotesInDateRange(C, fromYear, toYear);

    [McpServerTool(Name = "count_notes_archive")]
    [Description("Group and count ARCHIVE (v1) notes by up to 3 dimensions: state, track, trackType, ownerType, subject, subjectType, chapter, story, theme, source, hasWorldDate. Note: in v1 every note is untracked (the track system postdates v1), 'subjectType' returns v1's triage labels (e.g. \"First Pass, subject notes only\") not real categories, and 'source' will read \"(no source)\" throughout (Source Material is a v2-era concept, same as tracks). See TIMELINE-REFACTOR-BACKLOG.md 1c for the pre-existing theater/dateShape/worldDateYear description gap (separate from this addition).")]
    public string CountNotesArchive(
        [Description("Dimensions to group by, e.g. [\"subject\",\"state\"]. Default [\"state\"].")] string[]? groupBy = null)
        => Engine.CountNotes(C, groupBy ?? []);
}
