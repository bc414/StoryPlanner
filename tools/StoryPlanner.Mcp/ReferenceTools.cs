using System.ComponentModel;
using System.Text;
using ModelContextProtocol.Server;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Mcp;

/// <summary>
/// Reference tools — the live replacements for the four files Brian used to paste by hand
/// (TLTT v2-definitions.md, v2-subjects.md, v2-themes.md, and ad-hoc stats). Plain tools
/// rather than MCP resources so they work identically in Claude Code and Claude Desktop.
/// </summary>
[McpServerToolType]
public sealed class ReferenceTools(StoryPlanSources sources)
{
    [McpServerTool(Name = "get_track_definitions")]
    [Description("The 106 note-track definitions from the working plan (v2) — pass 3 of retrieval: what each track's notes MEAN. Grouped by subject type and owner scope; each carries its TrackType, the authorial cognitive mode, DisplayQuestion (the exact question its notes answer), and Usage/Audit directives where recorded. Filter by track names and/or subject type. Track definitions are final — safe to read once per session and reuse.")]
    public string GetTrackDefinitions(
        [Description("Exact track names to return (case-insensitive), e.g. [\"Backstory\",\"Reader Opinion Plan\"]. Omit for all 106.")] string[]? trackNames = null,
        [Description("Filter to one subject type, e.g. \"Character\". Omit for all.")] string? subjectType = null)
    {
        var c = sources.Get(Corpus.Working);
        var sb = new StringBuilder();

        IEnumerable<NoteTrackDefinition> tracks = c.Tracks;
        if (trackNames is { Length: > 0 })
        {
            var wanted = new HashSet<string>(trackNames, StringComparer.OrdinalIgnoreCase);
            tracks = tracks.Where(t => wanted.Contains(t.TrackName));
        }
        if (!string.IsNullOrWhiteSpace(subjectType))
        {
            var defIds = c.SubjectDefinitions
                .Where(d => string.Equals(d.SubjectType, subjectType, StringComparison.OrdinalIgnoreCase))
                .Select(d => d.Id)
                .ToHashSet();
            tracks = tracks.Where(t => defIds.Contains(t.SubjectDefinitionId) || t.OwnerType is OwnerType.PlotPoint or OwnerType.Chapter);
        }

        var list = tracks.ToList();
        sb.AppendLine($"# track definitions — {list.Count} of {c.Tracks.Count} (working plan)");
        sb.AppendLine();

        // Cognitive modes for the track types present — the authorial persona per type.
        sb.AppendLine("## Track types (cognitive modes)");
        foreach (var tt in list.Select(t => t.TrackType).Distinct().OrderBy(t => (int)t))
            sb.AppendLine($"- {tt}: {tt.GetCognitiveMode()}");
        sb.AppendLine();

        foreach (var group in list
                     .GroupBy(t => c.SubjectDefById.TryGetValue(t.SubjectDefinitionId, out var d) ? d.SubjectType : "(project-wide)")
                     .OrderBy(g => g.Key))
        {
            foreach (var scope in group.GroupBy(t => t.OwnerType).OrderBy(g => (int)g.Key))
            {
                var scopeName = scope.Key switch
                {
                    OwnerType.Subject => "subject-wide tracks",
                    OwnerType.PlotPoint => "plot point tracks",
                    OwnerType.Chapter => "chapter tracks",
                    OwnerType.PlotPointSubjectLink => "scene-link tracks (per plot-point×subject)",
                    _ => scope.Key.ToString()
                };
                sb.AppendLine($"## {group.Key} — {scopeName}");
                foreach (var t in scope.OrderBy(t => t.ExpansionModeDisplayOrder))
                {
                    sb.AppendLine($"### {t.TrackName} (track id:{t.Id}) [{t.TrackType}]");
                    if (t.DisplayQuestion.Length > 0) sb.AppendLine($"Q: {t.DisplayQuestion}");
                    if (t.UsageDirective.Length > 0) sb.AppendLine($"Usage: {t.UsageDirective}");
                    if (t.AuditDirective.Length > 0) sb.AppendLine($"Audit: {t.AuditDirective}");
                    var flags = new List<string>();
                    if (t.IsSingleton) flags.Add("singleton");
                    if (t.SupportsWorldDate) flags.Add("worldDate");
                    if (t.SupportsTheme) flags.Add("theme");
                    if (t.SupportsSourceMaterial) flags.Add("sourceMaterial");
                    if (flags.Count > 0) sb.AppendLine($"supports: {string.Join(", ", flags)}");
                }
                sb.AppendLine();
            }
        }
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "list_subjects")]
    [Description("Inventory of subjects: id, name, type, retrievable note count, flagged count, scene-link count. The name→id map for fetches. In the archive, subject 'types' are v1 triage labels (\"First Pass, subject notes only\", \"Deferred…\"), not categories.")]
    public string ListSubjects(
        [Description("\"working\" (v2, default) or \"archive\" (v1).")] string corpus = "working",
        [Description("Filter to one subject type (working plan) or triage label (archive), case-insensitive. Omit for all.")] string? subjectType = null)
    {
        var c = sources.Get(corpus.Equals("archive", StringComparison.OrdinalIgnoreCase) ? Corpus.Archive : Corpus.Working);
        var sb = new StringBuilder();

        var groups = c.Subjects
            .GroupBy(s => c.SubjectDefById.TryGetValue(s.SubjectDefinitionId, out var d) ? d.SubjectType : "?")
            .Where(g => subjectType is null || string.Equals(g.Key, subjectType, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(g => g.Count())
            .ToList();

        sb.AppendLine($"# subjects {Query.CorpusName(c.Corpus)} — {groups.Sum(g => g.Count())} across {groups.Count} types");
        foreach (var g in groups)
        {
            sb.AppendLine($"## {g.Key} ({g.Count()})");
            foreach (var s in g.OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase))
            {
                var own = c.NotesByOwner.TryGetValue((OwnerType.Subject, s.Id), out var list) ? list : [];
                var vis = own.Count(n => n.NoteState != NoteState.Flagged);
                var flg = own.Count - vis;
                var links = c.LinksBySubject.TryGetValue(s.Id, out var ll) ? ll.Count : 0;
                sb.AppendLine($"{s.Name} — {vis} notes{(flg > 0 ? $" (+{flg} flagged)" : "")}, {links} scene links (subject:{s.Id})");
            }
        }
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "list_themes")]
    [Description("The story's thematic propositions: id, name, full proposition text, and how many notes carry each theme tag.")]
    public string ListThemes(
        [Description("\"working\" (v2, default) or \"archive\" (v1).")] string corpus = "working")
    {
        var c = sources.Get(corpus.Equals("archive", StringComparison.OrdinalIgnoreCase) ? Corpus.Archive : Corpus.Working);
        var sb = new StringBuilder();
        sb.AppendLine($"# themes {Query.CorpusName(c.Corpus)} — {c.Themes.Count}");
        foreach (var t in c.Themes.OrderBy(t => t.Id))
        {
            var tagged = c.Notes.Count(n => n.ThemeId == t.Id && n.NoteState != NoteState.Flagged);
            var flagged = c.Notes.Count(n => n.ThemeId == t.Id && n.NoteState == NoteState.Flagged);
            sb.AppendLine($"\"{t.Name}\" — {tagged} tagged notes{(flagged > 0 ? $" (+{flagged} flagged)" : "")} (theme:{t.Id})");
            sb.AppendLine($"  proposition: {t.Proposition}");
        }
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "get_stats")]
    [Description("Population statistics for one or both corpora: file info, subjects by type, notes by state, tracked/untracked, plot point placement, links, chapters, tracks defined vs used, themes, WorldDate/theme tagging coverage, and conversation-block triage tallies. The live version of the numbers — never stale.")]
    public string GetStats(
        [Description("\"working\", \"archive\", or \"both\" (default).")] string corpus = "both")
    {
        var sb = new StringBuilder();
        if (!corpus.Equals("archive", StringComparison.OrdinalIgnoreCase))
            AppendStats(sb, sources.Get(Corpus.Working));
        if (!corpus.Equals("working", StringComparison.OrdinalIgnoreCase))
            AppendStats(sb, sources.Get(Corpus.Archive));
        return Query.Cap(sb);
    }

    private static void AppendStats(StringBuilder sb, PlanCache c)
    {
        sb.AppendLine($"# {Query.CorpusName(c.Corpus)} — {c.FilePath} ({c.FileSizeBytes / 1024.0 / 1024.0:F1} MB), snapshot loaded {c.LoadedAtUtc:yyyy-MM-dd HH:mm:ss}Z");

        var byType = c.Subjects
            .GroupBy(s => c.SubjectDefById.TryGetValue(s.SubjectDefinitionId, out var d) ? d.SubjectType : "?")
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Key} {g.Count()}");
        sb.AppendLine($"subjects: {c.Subjects.Count} ({string.Join(", ", byType)})");

        var byState = c.Notes.GroupBy(n => Query.StateLabel(c.Corpus, n.NoteState))
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Count()} {g.Key}");
        var untracked = c.Notes.Count(n => n.NoteTrackDefinitionId is null);
        sb.AppendLine($"notes: {c.Notes.Count} ({string.Join(", ", byState)}); untracked: {untracked}");

        var unplaced = c.PlotPoints.Count(p => p.ChapterId is null);
        sb.AppendLine($"plot points: {c.PlotPoints.Count} ({unplaced} unplaced) | chapters: {c.Chapters.Count} | scene links: {c.Links.Count}");

        if (c.Stories.Count > 0)
        {
            var perStory = c.Stories.OrderBy(s => s.OrderIndex).Select(s =>
            {
                var chs = c.ChaptersByStory.TryGetValue(s.Id, out var l) ? l : [];
                var pps = chs.Sum(ch => c.PlotPoints.Count(p => p.ChapterId == ch.Id));
                return $"{s.Title} {chs.Count}ch/{pps}pp";
            });
            var unassignedChapters = c.Chapters.Count(ch => ch.StoryId == 0);
            sb.AppendLine($"stories: {c.Stories.Count} ({string.Join(", ", perStory)})" +
                          (unassignedChapters > 0 ? $" | Unassigned: {unassignedChapters} chapters" : ""));
        }

        if (c.Tracks.Count > 0)
        {
            var usedTrackIds = c.Notes.Where(n => n.NoteTrackDefinitionId is int).Select(n => n.NoteTrackDefinitionId!.Value).ToHashSet();
            var unused = c.Tracks.Count(t => !usedTrackIds.Contains(t.Id));
            sb.AppendLine($"track definitions: {c.Tracks.Count} ({unused} with zero notes)");
        }
        else sb.AppendLine("track definitions: 0 (this file predates the track system)");

        var withDate = c.Notes.Count(n => !string.IsNullOrWhiteSpace(n.WorldDate));
        var withTheme = c.Notes.Count(n => n.ThemeId is not null);
        sb.AppendLine($"themes: {c.Themes.Count} | notes with worldDate: {withDate} | with theme tag: {withTheme}");

        if (c.Conversations.Count > 0)
        {
            var blockStates = c.Blocks.GroupBy(b => b.BlockState)
                .OrderBy(g => (int)g.Key)
                .Select(g => $"{g.Count()} {g.Key.ToString().ToLowerInvariant()}");
            sb.AppendLine($"conversations: {c.Conversations.Count} ({c.Blocks.Count} blocks: {string.Join(", ", blockStates)})");
        }
        sb.AppendLine();
    }

    [McpServerTool(Name = "list_stories")]
    [Description("Inventory of stories: title, abbreviation, reading order, chapter and plot-point counts, plus any chapters still \"(Unassigned)\" (StoryId 0 — a legal, permanent state, not an error). Stories are never joined across corpora: a story of the same name in the working plan and the archive are unrelated rows with no shared id — never cross-reference them by name.")]
    public string ListStories(
        [Description("\"working\" (v2, default) or \"archive\" (v1).")] string corpus = "working")
    {
        var c = sources.Get(corpus.Equals("archive", StringComparison.OrdinalIgnoreCase) ? Corpus.Archive : Corpus.Working);
        var sb = new StringBuilder();
        sb.AppendLine($"# stories in {Query.CorpusName(c.Corpus)} — {c.Stories.Count}");
        foreach (var s in c.Stories.OrderBy(s => s.OrderIndex))
        {
            var chapters = c.ChaptersByStory.TryGetValue(s.Id, out var chs) ? chs : [];
            var pps = chapters.Sum(ch => c.PlotPoints.Count(p => p.ChapterId == ch.Id));
            var abbr = string.IsNullOrEmpty(s.Abbreviation) ? "" : $" [{s.Abbreviation}]";
            sb.AppendLine($"\"{s.Title}\"{abbr} — {chapters.Count} chapters, {pps} plot points (story:{s.Id})");
        }
        var unassigned = c.Chapters.Count(ch => ch.StoryId == 0);
        if (unassigned > 0)
            sb.AppendLine($"(Unassigned) — {unassigned} chapters");
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "get_stories")]
    [Description("Fetch stories by id: title, abbreviation, and the ordered chapter list (with plot-point/note tallies). Pass an EMPTY ids array for the inventory (same as list_stories).")]
    public string GetStories(
        [Description("Story ids. Empty array → inventory of every story.")] int[] ids,
        [Description("\"working\" (v2, default) or \"archive\" (v1).")] string corpus = "working")
    {
        var c = sources.Get(corpus.Equals("archive", StringComparison.OrdinalIgnoreCase) ? Corpus.Archive : Corpus.Working);
        if (ids.Length == 0) return ListStories(corpus);

        var sb = new StringBuilder();
        foreach (var id in ids.Distinct())
        {
            if (!c.StoryById.TryGetValue(id, out var s))
            {
                sb.AppendLine($"## story:{id} — not found in {Query.CorpusName(c.Corpus)}");
                continue;
            }
            var abbr = string.IsNullOrEmpty(s.Abbreviation) ? "" : $" [{s.Abbreviation}]";
            sb.AppendLine($"## \"{s.Title}\"{abbr} (story:{s.Id})");

            var chapters = c.ChaptersByStory.TryGetValue(s.Id, out var chs)
                ? chs.OrderBy(ch => ch.OrderIndex).ToList()
                : new List<Chapter>();
            sb.AppendLine($"chapters ({chapters.Count}):");
            foreach (var ch in chapters)
            {
                var pps = c.PlotPoints.Count(p => p.ChapterId == ch.Id);
                var own = c.NotesByOwner.TryGetValue((OwnerType.Chapter, ch.Id), out var list) ? list : [];
                var vis = own.Count(n => n.NoteState != NoteState.Flagged);
                var flg = own.Count - vis;
                sb.AppendLine($"  CH#{ch.OrderIndex} \"{ch.Title}\" — {pps} plot points, {vis} chapter notes{(flg > 0 ? $" (+{flg} flagged)" : "")} (chapter:{ch.Id})");
            }
            sb.AppendLine();
        }
        return Query.Cap(sb);
    }
}
