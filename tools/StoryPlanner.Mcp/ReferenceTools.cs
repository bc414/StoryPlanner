using System.ComponentModel;
using System.Text;
using ModelContextProtocol.Server;
using StoryPlanner.Core;

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
                    if (t.SupportsWorldDate) flags.Add(t.SupportsWorldDateEnd ? "worldDate (condition: start..end)" : "worldDate (event: start only)");
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

    [McpServerTool(Name = "list_source_materials")]
    [Description("Coverage over the two-tier Source Material model (Work -> Part, e.g. MLP:FiM -> S3E01, or Equestria at War -> a playable country). Per Part: citing note count and review state (Reviewed = deliberately passed over for TLTT material, independent of citation count). This is the negative-space view — an untouched Part (NotReviewed, 0 citations) is a rewatch/reread candidate, never ranked or suggested here, just listed. The Work/Part set is pre-seeded (DataOps seed-source-material) rather than accreted from citations, so an untouched Part is a real signal, not a gap in what's been tagged.")]
    public string ListSourceMaterials(
        [Description("\"working\" (v2, default) or \"archive\" (v1).")] string corpus = "working")
    {
        var c = sources.Get(corpus.Equals("archive", StringComparison.OrdinalIgnoreCase) ? Corpus.Archive : Corpus.Working);
        var sb = new StringBuilder();

        var citedPartIds = c.SourceReferencesByNote.Values.SelectMany(refs => refs)
            .Where(r => r.SourceMaterialPartId.HasValue)
            .Select(r => r.SourceMaterialPartId!.Value)
            .ToHashSet();
        var citationCountByPart = c.SourceReferencesByNote.Values.SelectMany(refs => refs)
            .Where(r => r.SourceMaterialPartId.HasValue)
            .GroupBy(r => r.SourceMaterialPartId!.Value)
            .ToDictionary(g => g.Key, g => g.Count());
        var citationCountByWork = c.SourceReferencesByNote.Values.SelectMany(refs => refs)
            .GroupBy(r => r.SourceMaterialId)
            .ToDictionary(g => g.Key, g => g.Count());

        sb.AppendLine($"# source materials {Query.CorpusName(c.Corpus)} — {c.SourceMaterials.Count} works, {c.SourceMaterialParts.Count} parts");
        if (c.SourceMaterials.Count == 0)
        {
            sb.AppendLine("(none seeded — see the seed-source-material DataOps op)");
            return Query.Cap(sb);
        }

        foreach (var work in c.SourceMaterials.OrderBy(w => w.OrderIndex))
        {
            var workCitations = citationCountByWork.GetValueOrDefault(work.Id);
            sb.AppendLine($"## {work.Name} — {workCitations} note(s) cite the work directly (source:{work.Id})");
            if (work.Description.Length > 0) sb.AppendLine($"  {work.Description}");

            var parts = c.SourceMaterialPartsByWork.TryGetValue(work.Id, out var pl) ? pl : [];
            if (parts.Count == 0)
            {
                if (work.PartNoun.Length == 0) sb.AppendLine("  no Parts — cite the work itself");
                continue;
            }

            var partNoun = work.PartNoun.Length > 0 ? work.PartNoun : "Part";
            var untouched = parts.Count(p => p.ReviewState == SourcePartReviewState.NotReviewed && !citedPartIds.Contains(p.Id));
            sb.AppendLine($"  {parts.Count} {partNoun}s ({untouched} untouched — never reviewed AND never cited)");
            foreach (var p in parts)
            {
                var n = citationCountByPart.GetValueOrDefault(p.Id);
                var reviewed = p.ReviewState == SourcePartReviewState.Reviewed;
                var flag = n == 0 && !reviewed ? " <- untouched" : "";
                var label = p.Name.Length > 0 ? $"{p.Code} — {p.Name}" : p.Code;
                sb.AppendLine($"  {label}: {n} note(s){(reviewed ? ", reviewed" : "")} (sourcepart:{p.Id}){flag}");
            }
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

        var withDate = c.Notes.Count(Query.HasAnyWorldDate);
        var withTheme = c.Notes.Count(n => n.ThemeId is not null);
        var withSource = c.SourceReferencesByNote.Count; // notes with >=1 citation, not citation count
        sb.AppendLine($"themes: {c.Themes.Count} | notes with worldDate: {withDate} | with theme tag: {withTheme} | with source citation: {withSource}");

        if (c.SourceMaterials.Count > 0)
        {
            var citedPartIds = c.SourceReferencesByNote.Values.SelectMany(refs => refs)
                .Where(r => r.SourceMaterialPartId.HasValue)
                .Select(r => r.SourceMaterialPartId!.Value)
                .ToHashSet();
            var untouched = c.SourceMaterialParts.Count(p =>
                p.ReviewState == SourcePartReviewState.NotReviewed && !citedPartIds.Contains(p.Id));
            sb.AppendLine($"source materials: {c.SourceMaterials.Count} works, {c.SourceMaterialParts.Count} parts ({untouched} untouched)");
        }

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

    [McpServerTool(Name = "list_theaters")]
    [Description("Inventory of theaters — the master timeline's x-axis (a display coordinate ordered by narrative density, NOT a taxonomy). Per theater: subject count by type, dated-note count, plot-point count, and world-date span. TheaterId 0 is \"(Unplaced)\" — a legal, permanent state meaning the author has not placed it (or is deliberately uncertain), never an error. Theater assignment is authorial: never infer it from a subject's name.")]
    public string ListTheaters(
        [Description("\"working\" (v2, default) or \"archive\" (v1).")] string corpus = "working")
    {
        var c = sources.Get(corpus.Equals("archive", StringComparison.OrdinalIgnoreCase) ? Corpus.Archive : Corpus.Working);
        var sb = new StringBuilder();
        sb.AppendLine($"# theaters in {Query.CorpusName(c.Corpus)} — {c.Theaters.Count}");
        if (c.Theaters.Count == 0)
            sb.AppendLine("(none defined — the timeline x-axis is unconfigured in this file)");

        var subjectsByTheater = c.Subjects.GroupBy(s => s.TheaterId).ToDictionary(g => g.Key, g => g.ToList());
        var ppByTheater = c.PlotPoints.GroupBy(p => p.TheaterId).ToDictionary(g => g.Key, g => g.ToList());

        IEnumerable<(int Id, string Name, int Order)> rows = c.Theaters
            .OrderBy(t => t.OrderIndex).Select(t => (t.Id, t.Name, t.OrderIndex));
        rows = rows.Append((0, "(Unplaced)", int.MaxValue));

        foreach (var (id, name, _) in rows)
        {
            var subs = subjectsByTheater.GetValueOrDefault(id) ?? [];
            var pps = ppByTheater.GetValueOrDefault(id) ?? [];
            if (subs.Count == 0 && pps.Count == 0 && id == 0) continue;

            var subjectIds = subs.Select(s => s.Id).ToHashSet();
            var dated = c.Notes.Where(n => n.OwnerType == OwnerType.Subject
                                           && subjectIds.Contains(n.OwnerId)
                                           && Query.EffectiveWorldDate(n) is not null).ToList();

            var byType = subs
                .GroupBy(s => c.SubjectDefById.TryGetValue(s.SubjectDefinitionId, out var d) ? d.SubjectType : "?")
                .OrderByDescending(g => g.Count())
                .Select(g => $"{g.Key} {g.Count()}");

            var years = dated.Select(n => Query.EffectiveWorldDate(n)!.Value)
                .SelectMany(d => new[] { d.Start?.Year, d.End?.Year })
                .Where(y => y is not null).Select(y => y!.Value).ToList();
            var span = years.Count > 0 ? $"{years.Min()}..{years.Max()}" : "no dated notes";

            sb.AppendLine($"\"{name}\" — {subs.Count} subjects ({string.Join(", ", byType)}), " +
                          $"{dated.Count} dated notes [{span}], {pps.Count} plot points (theater:{id})");
        }

        if (c.Pivots.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine($"# pivots — {c.Pivots.Count} authored years; eras are DERIVED as the gaps between them (never stored)");
            foreach (var p in c.Pivots.OrderBy(p => p.Year))
                sb.AppendLine($"{p.Year} — {p.Name}{(p.Description.Length > 0 ? $" ({p.Description})" : "")}");
        }
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
