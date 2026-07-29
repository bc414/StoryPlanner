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
                sb.AppendLine($"subject:{s.Id} {s.Name} — {vis} notes{(flg > 0 ? $" (+{flg} flagged)" : "")}, {links} scene links");
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
            sb.AppendLine($"theme:{t.Id} \"{t.Name}\" — {tagged} tagged notes{(flagged > 0 ? $" (+{flagged} flagged)" : "")}");
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
}
