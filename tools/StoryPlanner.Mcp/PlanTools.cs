using StoryPlanner.Core;
using System.ComponentModel;
using ModelContextProtocol.Server;

namespace StoryPlanner.Mcp;

/// <summary>Tools over the WORKING plan (v2). Flagged notes are walled — see FlaggedTools.</summary>
[McpServerToolType]
public sealed class PlanTools(StoryPlanSources sources)
{
    private PlanCache C => sources.Get(Corpus.Working);

    [McpServerTool(Name = "search_plan")]
    [Description("Regex search over the WORKING plan (v2): non-flagged note content, subject names/descriptions, plot point titles, chapter titles, theme names/propositions. Returns ids + owner + track + state + match snippet. Flagged notes and track definitions are excluded (see list_open_questions / get_track_definitions). Full .NET regex — alternation like \"Coltbert|crossbow|981\" works.")]
    public string SearchPlan(
        [Description("Regular expression (.NET syntax). Case-insensitive unless caseSensitive=true.")] string pattern,
        [Description("Match case-sensitively (default false).")] bool caseSensitive = false,
        [Description("Match whole words only (default false).")] bool wholeWord = false,
        [Description("Characters of context around each match snippet (default 150, max 2000).")] int contextChars = 150,
        [Description("Maximum hits returned (default 40, max 250). Totals are always reported.")] int limit = 40)
        => Engine.Search(C, pattern, caseSensitive, wholeWord, contextChars, limit);

    [McpServerTool(Name = "get_notes_plan")]
    [Description("Fetch full note content by note ids from the WORKING plan (v2). Each note carries owner, track, state, WorldDate (raw + parsed), theme, and last-modified. Flagged ids return a stub pointing at get_open_questions.")]
    public string GetNotesPlan(
        [Description("Note ids (from search hits or fetch results).")] int[] ids)
        => Engine.GetNotes(C, ids);

    [McpServerTool(Name = "get_subjects_plan")]
    [Description("Fetch subjects by id from the WORKING plan (v2): record, note tallies (flagged disclosed as counts), scene-link edge list in chapter order, and (unless includeNotes=false) all non-flagged notes grouped by track.")]
    public string GetSubjectsPlan(
        [Description("Subject ids (from search hits or list_subjects).")] int[] ids,
        [Description("Include full note content (default true). False returns record + edges + tallies only — useful for pure graph hops.")] bool includeNotes = true)
        => Engine.GetSubjects(C, ids, includeNotes);

    [McpServerTool(Name = "get_plot_points_plan")]
    [Description("Fetch plot points by id from the WORKING plan (v2): title, chapter position, linked subjects (edge list with link ids), and (unless includeNotes=false) all non-flagged notes grouped by track.")]
    public string GetPlotPointsPlan(
        [Description("Plot point ids.")] int[] ids,
        [Description("Include full note content (default true).")] bool includeNotes = true)
        => Engine.GetPlotPoints(C, ids, includeNotes);

    [McpServerTool(Name = "get_chapters_plan")]
    [Description("Fetch chapters by id from the WORKING plan (v2): chapter notes and the ordered plot point list with note/link tallies. Pass an EMPTY ids array for the inventory of all chapters, grouped under story headings (see list_stories/get_stories for the story layer itself).")]
    public string GetChaptersPlan(
        [Description("Chapter ids. Empty array → one-line inventory of every chapter.")] int[] ids,
        [Description("Include full chapter-note content (default true).")] bool includeNotes = true)
        => Engine.GetChapters(C, ids, includeNotes);

    [McpServerTool(Name = "get_links_plan")]
    [Description("Fetch plot-point×subject links by id from the WORKING plan (v2) — the 'what this scene does to this subject' layer. Returns endpoints and (unless includeNotes=false) the link's non-flagged notes grouped by track.")]
    public string GetLinksPlan(
        [Description("Link ids (from subject/plot point fetch edge lists).")] int[] ids,
        [Description("Include full note content (default true).")] bool includeNotes = true)
        => Engine.GetLinks(C, ids, includeNotes);

    [McpServerTool(Name = "get_theme_notes_plan")]
    [Description("All non-flagged notes tagged with a theme in the WORKING plan (v2) — the evidence cross-cut for one of the story's thematic propositions.")]
    public string GetThemeNotesPlan(
        [Description("Theme id or exact theme name (case-insensitive).")] string theme)
        => Engine.GetThemeNotes(C, theme);

    [McpServerTool(Name = "get_notes_in_date_range_plan")]
    [Description("Non-flagged notes from the WORKING plan (v2) whose world date intersects [fromYear, toYear], both years inclusive, sorted chronologically. Dates are read from the structured columns, falling back to the legacy free-text value converted mechanically (\"993\", \"870-928\", \"300 BLB\"); unconvertible values are counted, never guessed. A start-only date on a CONDITION track (SupportsWorldDateEnd) means \"in force, end TBD\" and so matches every later range; the same value on an event track does not. Omit both bounds for the full story-world chronology.")]
    public string GetNotesInDateRangePlan(
        [Description("Start year (inclusive). Negative = before year 0. Omit for open start.")] int? fromYear = null,
        [Description("End year (inclusive). Omit for open end.")] int? toYear = null)
        => Engine.GetNotesInDateRange(C, fromYear, toYear);

    [McpServerTool(Name = "count_notes_plan")]
    [Description("Group and count WORKING plan (v2) notes by up to 3 dimensions: state, track, trackType, ownerType, subject, subjectType, chapter, story, theme, source, hasWorldDate, theater, dateShape, worldDateYear. \"story\" resolves Chapter/PlotPoint/Link-owned notes through to their story (\"(Unassigned)\" if the chapter has none); Subject notes report \"(no story)\". \"theater\" is the timeline column a note renders in, via its owning subject's authorial placement (\"(Unplaced)\" = not placed, a legal state). \"dateShape\" is how the note draws on the timeline — event at year/month/day precision vs condition by span length vs open-ended vs undated. \"source\" is every cited Work/Part joined comma-separated (a note may cite several Parts for one claim) — \"(no source)\" if uncited. Counts include flagged notes (counts are numbers, not content). Compose absence/intersection questions from counts — e.g. groupBy [\"subject\",\"trackType\"] then look for zeros.")]
    public string CountNotesPlan(
        [Description("Dimensions to group by, e.g. [\"track\",\"state\"]. Default [\"state\"].")] string[]? groupBy = null)
        => Engine.CountNotes(C, groupBy ?? []);
}
