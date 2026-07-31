using System.Text;
using System.Text.RegularExpressions;
using StoryPlanner.Core;

namespace StoryPlanner.Mcp;

/// <summary>
/// The shared per-corpus core behind the *_plan and *_archive tool families.
/// Flagged wall (hard, with count disclosure): ordinary results never contain flagged
/// note content or FlagReason text — the same semantics as NoteExportRenderer's export
/// exclusion. Flagged existence is disclosed as counts; content is retrievable only
/// through the flagged tool family (list_open_questions / get_open_questions).
/// </summary>
internal static class Engine
{
    // ── search ──────────────────────────────────────────────────────────────────

    public static string Search(PlanCache c, string pattern, bool caseSensitive, bool wholeWord,
        int contextChars, int limit)
    {
        Regex rx;
        try { rx = Query.BuildRegex(pattern, caseSensitive, wholeWord); }
        catch (ArgumentException ex) { return $"Invalid regex: {ex.Message}"; }

        contextChars = Math.Clamp(contextChars, 20, 2000);
        limit = Math.Clamp(limit, 1, 250);

        var lines = new List<string>();
        int noteHits = 0, subjectHits = 0, ppHits = 0, chapterHits = 0, themeHits = 0, walled = 0;

        try
        {
            foreach (var n in c.Notes)
            {
                if (n.NoteState == NoteState.Flagged)
                {
                    // Wall: match for the count only — no snippet, no content.
                    if (rx.IsMatch(n.Content) || rx.IsMatch(n.FlagReason)) walled++;
                    continue;
                }
                var m = rx.Match(n.Content);
                if (!m.Success) continue;
                noteHits++;
                if (lines.Count < limit)
                    // Name-led, id trailing in a parenthetical; the unbounded snippet goes on
                    // its own indented line so a truncation never eats the id off the end.
                    lines.Add($"{Query.OwnerLabel(c, n.OwnerType, n.OwnerId)} · {Query.TrackName(c, n)} · " +
                              $"{Query.StateLabel(c.Corpus, n.NoteState)} (note:{n.Id})\n  \"{Query.Snippet(n.Content, m, contextChars)}\"");
            }

            foreach (var s in c.Subjects)
            {
                var mName = rx.Match(s.Name);
                var mDesc = rx.Match(s.Description);
                if (!mName.Success && !mDesc.Success) continue;
                subjectHits++;
                if (lines.Count < limit)
                {
                    var type = c.SubjectDefById.TryGetValue(s.SubjectDefinitionId, out var d) ? d.SubjectType : "?";
                    var where = mName.Success
                        ? $"name: \"{s.Name}\""
                        : $"description: \"{Query.Snippet(s.Description, mDesc, contextChars)}\"";
                    lines.Add($"{s.Name} [{type}] (subject:{s.Id}) — {where}");
                }
            }

            foreach (var p in c.PlotPoints)
            {
                var m = rx.Match(p.Title);
                if (!m.Success) continue;
                ppHits++;
                if (lines.Count < limit)
                    lines.Add($"{Query.OwnerLabel(c, OwnerType.PlotPoint, p.Id)} (plotpoint:{p.Id})");
            }

            foreach (var ch in c.Chapters)
            {
                var m = rx.Match(ch.Title);
                if (!m.Success) continue;
                chapterHits++;
                if (lines.Count < limit)
                    lines.Add($"{Query.ChapterLabel(c, ch)} \"{ch.Title}\" (chapter:{ch.Id})");
            }

            foreach (var t in c.Themes)
            {
                var mName = rx.Match(t.Name);
                var mProp = rx.Match(t.Proposition);
                if (!mName.Success && !mProp.Success) continue;
                themeHits++;
                if (lines.Count < limit)
                {
                    var where = mName.Success ? $"name: \"{t.Name}\""
                        : $"proposition: \"{Query.Snippet(t.Proposition, mProp, contextChars)}\"";
                    lines.Add($"{t.Name} (theme:{t.Id}) — {where}");
                }
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return "Regex timed out (2s) — simplify the pattern.";
        }

        var total = noteHits + subjectHits + ppHits + chapterHits + themeHits;
        var sb = new StringBuilder();
        sb.AppendLine($"# search {Query.CorpusName(c.Corpus)} /{pattern}/ — {total} matches " +
                      $"(notes {noteHits}, subjects {subjectHits}, plot points {ppHits}, chapters {chapterHits}, themes {themeHits})" +
                      (total > lines.Count ? $". Showing first {lines.Count}." : "."));
        if (walled > 0)
            sb.AppendLine($"# walled: {walled} flagged-note matches (content or flag reason) — list_open_questions(pattern: \"{pattern}\", corpus: \"{(c.Corpus == Corpus.Working ? "working" : "archive")}\")");
        if (total == 0 && walled == 0)
            sb.AppendLine("(no matches)");
        foreach (var l in lines) sb.AppendLine(l);
        return Query.Cap(sb);
    }

    // ── fetch: notes ────────────────────────────────────────────────────────────

    public static string GetNotes(PlanCache c, int[] ids)
    {
        var sb = new StringBuilder();
        int found = 0, missing = 0, flagged = 0;
        var body = new StringBuilder();
        var noteById = c.Notes.ToDictionary(n => n.Id); // small; built per call is fine at this scale

        foreach (var id in ids.Distinct())
        {
            if (!noteById.TryGetValue(id, out var n))
            {
                missing++;
                body.AppendLine($"## note:{id} — not found in {Query.CorpusName(c.Corpus)}");
                continue;
            }
            if (n.NoteState == NoteState.Flagged)
            {
                flagged++;
                body.AppendLine($"{Query.OwnerLabel(c, n.OwnerType, n.OwnerId)} · {Query.TrackName(c, n)} — FLAGGED (walled) (note:{id}): use get_open_questions(ids: [{id}])");
                continue;
            }
            found++;
            AppendNoteBlock(body, c, n);
        }

        sb.AppendLine($"# get_notes {Query.CorpusName(c.Corpus)} — {found} returned, {flagged} flagged (walled), {missing} not found");
        sb.Append(body);
        return Query.Cap(sb);
    }

    private static void AppendNoteBlock(StringBuilder sb, PlanCache c, Note n)
    {
        sb.AppendLine($"## {Query.OwnerLabel(c, n.OwnerType, n.OwnerId)} · {Query.TrackLabel(c, n)} · " +
                      $"{Query.StateLabel(c.Corpus, n.NoteState)} (note:{n.Id}, {Query.OwnerRef(n.OwnerType, n.OwnerId)})");
        var meta = new List<string>();
        var wd = Query.WorldDateLabel(n);
        if (wd.Length > 0) meta.Add(wd);
        if (n.ThemeId is int tid)
            meta.Add(c.ThemeById.TryGetValue(tid, out var th) ? $"theme:{th.Name}" : $"theme:{tid}?");
        var src = Query.SourceLabel(c, n);
        if (src.Length > 0) meta.Add(src);
        meta.Add($"modified:{n.LastModified:yyyy-MM-dd}");
        sb.AppendLine(string.Join(" | ", meta));
        sb.AppendLine(n.Content.Length == 0 ? "(empty content)" : n.Content.TrimEnd());
        sb.AppendLine();
    }

    // ── flagged tally (the count-disclosure half of the wall) ───────────────────

    private static string FlaggedTally(PlanCache c, IEnumerable<Note> scopeNotes, string scopeHint)
    {
        var flagged = scopeNotes.Where(n => n.NoteState == NoteState.Flagged).ToList();
        if (flagged.Count == 0) return "";
        var byTrack = flagged.GroupBy(n => Query.TrackName(c, n))
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Key} {g.Count()}");
        return $" + {flagged.Count} flagged (walled: {string.Join(", ", byTrack)} — list_open_questions({scopeHint}))";
    }

    // ── fetch: subjects (edges embedded: scene links) ───────────────────────────

    public static string GetSubjects(PlanCache c, int[] ids, bool includeNotes)
    {
        var sb = new StringBuilder();
        var corpusArg = c.Corpus == Corpus.Working ? "working" : "archive";
        foreach (var id in ids.Distinct())
        {
            if (!c.SubjectById.TryGetValue(id, out var s))
            {
                sb.AppendLine($"## subject:{id} — not found in {Query.CorpusName(c.Corpus)}");
                continue;
            }
            var type = c.SubjectDefById.TryGetValue(s.SubjectDefinitionId, out var d) ? d.SubjectType : "?";
            sb.AppendLine($"## {s.Name} [{type}] (subject:{s.Id})");
            if (s.Abbreviation.Length > 0) sb.AppendLine($"abbreviation: {s.Abbreviation}");
            if (s.Description.Length > 0) sb.AppendLine($"description: {s.Description}");

            var own = c.NotesByOwner.TryGetValue((OwnerType.Subject, s.Id), out var list) ? list : [];
            var visible = own.Where(n => n.NoteState != NoteState.Flagged).ToList();
            var states = visible.GroupBy(n => Query.StateLabel(c.Corpus, n.NoteState))
                .Select(g => $"{g.Count()} {g.Key}");
            sb.AppendLine($"notes: {visible.Count} retrievable ({string.Join(", ", states)})" +
                          FlaggedTally(c, own, $"subject: \"{s.Name}\", corpus: \"{corpusArg}\""));

            // Edges: scene links (chapter order, then order in chapter)
            var links = c.LinksBySubject.TryGetValue(s.Id, out var ll) ? ll : [];
            if (links.Count > 0)
            {
                sb.AppendLine($"scenes ({links.Count} links):");
                foreach (var l in links
                    .OrderBy(l => OrderKey(c, l.PlotPointId).chapterOrder)
                    .ThenBy(l => OrderKey(c, l.PlotPointId).orderInChapter))
                {
                    var linkNotes = c.NotesByOwner.TryGetValue((OwnerType.PlotPointSubjectLink, l.Id), out var lnl) ? lnl : [];
                    var vis = linkNotes.Count(n => n.NoteState != NoteState.Flagged);
                    var flg = linkNotes.Count - vis;
                    sb.AppendLine($"  {Query.OwnerLabel(c, OwnerType.PlotPoint, l.PlotPointId)}" +
                                  $" — {vis} link notes{(flg > 0 ? $", +{flg} flagged" : "")} (link:{l.Id})");
                }
            }
            else sb.AppendLine("scenes: none (no plot-point links)");

            if (includeNotes && visible.Count > 0)
                AppendNotesGroupedByTrack(sb, c, visible);
            sb.AppendLine();
        }
        return Query.Cap(sb);
    }

    private static (int chapterOrder, int orderInChapter) OrderKey(PlanCache c, int plotPointId)
    {
        if (!c.PlotPointById.TryGetValue(plotPointId, out var pp)) return (int.MaxValue, int.MaxValue);
        if (pp.ChapterId is int chId && c.ChapterById.TryGetValue(chId, out var ch))
            return (ch.OrderIndex, pp.OrderInChapter);
        return (int.MaxValue - 1, pp.OrderInChapter); // unplaced last (before missing)
    }

    private static void AppendNotesGroupedByTrack(StringBuilder sb, PlanCache c, List<Note> visible)
    {
        foreach (var g in visible.GroupBy(n => Query.TrackLabel(c, n)))
        {
            sb.AppendLine($"### {g.Key} — {g.Count()} notes");
            foreach (var n in g)
            {
                var meta = new List<string> { Query.StateLabel(c.Corpus, n.NoteState) };
                var wd = Query.WorldDateLabel(n);
                if (wd.Length > 0) meta.Add(wd);
                if (n.ThemeId is int tid)
                    meta.Add(c.ThemeById.TryGetValue(tid, out var th) ? $"theme:{th.Name}" : $"theme:{tid}?");
                var src = Query.SourceLabel(c, n);
                if (src.Length > 0) meta.Add(src);
                meta.Add($"(note:{n.Id})");
                sb.AppendLine($"--- {string.Join(" | ", meta)}");
                sb.AppendLine(n.Content.Length == 0 ? "(empty content)" : n.Content.TrimEnd());
            }
        }
    }

    // ── fetch: plot points (edges embedded: chapter + subject links) ────────────

    public static string GetPlotPoints(PlanCache c, int[] ids, bool includeNotes)
    {
        var sb = new StringBuilder();
        var corpusArg = c.Corpus == Corpus.Working ? "working" : "archive";
        foreach (var id in ids.Distinct())
        {
            if (!c.PlotPointById.TryGetValue(id, out var pp))
            {
                sb.AppendLine($"## plotpoint:{id} — not found in {Query.CorpusName(c.Corpus)}");
                continue;
            }
            sb.AppendLine($"## \"{pp.Title}\" (plotpoint:{pp.Id})");
            if (pp.ChapterId is int chId && c.ChapterById.TryGetValue(chId, out var ch))
                sb.AppendLine($"chapter: {Query.ChapterLabel(c, ch)} \"{ch.Title}\" (position {pp.OrderInChapter}) (chapter:{ch.Id})");
            else
                sb.AppendLine("chapter: (unplaced)");

            var own = c.NotesByOwner.TryGetValue((OwnerType.PlotPoint, pp.Id), out var list) ? list : [];
            var visible = own.Where(n => n.NoteState != NoteState.Flagged).ToList();
            sb.AppendLine($"notes: {visible.Count} retrievable" +
                          FlaggedTally(c, own, $"corpus: \"{corpusArg}\""));

            var links = c.LinksByPlotPoint.TryGetValue(pp.Id, out var ll) ? ll : [];
            if (links.Count > 0)
            {
                sb.AppendLine($"linked subjects ({links.Count}):");
                foreach (var l in links)
                {
                    var subjLabel = c.SubjectById.TryGetValue(l.SubjectId, out var s)
                        ? $"{s.Name}" +
                          (c.SubjectDefById.TryGetValue(s.SubjectDefinitionId, out var d) ? $" [{d.SubjectType}]" : "") +
                          $" (subject:{s.Id})"
                        : $"subject:{l.SubjectId}(missing)";
                    var linkNotes = c.NotesByOwner.TryGetValue((OwnerType.PlotPointSubjectLink, l.Id), out var lnl) ? lnl : [];
                    var vis = linkNotes.Count(n => n.NoteState != NoteState.Flagged);
                    var flg = linkNotes.Count - vis;
                    sb.AppendLine($"  {subjLabel} — {vis} link notes{(flg > 0 ? $", +{flg} flagged" : "")} (link:{l.Id})");
                }
            }
            else sb.AppendLine("linked subjects: none");

            if (includeNotes && visible.Count > 0)
                AppendNotesGroupedByTrack(sb, c, visible);
            sb.AppendLine();
        }
        return Query.Cap(sb);
    }

    // ── fetch: chapters (empty ids → inventory of all chapters) ─────────────────

    public static string GetChapters(PlanCache c, int[] ids, bool includeNotes)
    {
        var sb = new StringBuilder();
        if (ids.Length == 0)
        {
            sb.AppendLine($"# chapters in {Query.CorpusName(c.Corpus)} — {c.Chapters.Count} total (ids omitted → inventory)");

            // Grouped under story headings, in story reading order; "(Unassigned)" (StoryId 0,
            // never a real Story row) sorts last regardless of where 0 would otherwise land.
            var groups = c.Chapters.GroupBy(ch => ch.StoryId)
                .OrderBy(g => g.Key == 0
                    ? int.MaxValue
                    : (c.StoryById.TryGetValue(g.Key, out var st) ? st.OrderIndex : int.MaxValue - 1));

            foreach (var group in groups)
            {
                sb.AppendLine($"## {Query.StoryLabel(c, group.Key)}" + (group.Key != 0 ? $" (story:{group.Key})" : ""));
                foreach (var ch in group.OrderBy(x => x.OrderIndex))
                {
                    var pps = c.PlotPoints.Count(p => p.ChapterId == ch.Id);
                    var own = c.NotesByOwner.TryGetValue((OwnerType.Chapter, ch.Id), out var list) ? list : [];
                    var vis = own.Count(n => n.NoteState != NoteState.Flagged);
                    var flg = own.Count - vis;
                    sb.AppendLine($"CH#{ch.OrderIndex} \"{ch.Title}\" — {pps} plot points, {vis} chapter notes{(flg > 0 ? $" (+{flg} flagged)" : "")} (chapter:{ch.Id})");
                }
            }
            return Query.Cap(sb);
        }

        var corpusArg = c.Corpus == Corpus.Working ? "working" : "archive";
        foreach (var id in ids.Distinct())
        {
            if (!c.ChapterById.TryGetValue(id, out var ch))
            {
                sb.AppendLine($"## chapter:{id} — not found in {Query.CorpusName(c.Corpus)}");
                continue;
            }
            sb.AppendLine($"## {Query.ChapterLabel(c, ch)} \"{ch.Title}\" (chapter:{ch.Id})");

            var own = c.NotesByOwner.TryGetValue((OwnerType.Chapter, ch.Id), out var list) ? list : [];
            var visible = own.Where(n => n.NoteState != NoteState.Flagged).ToList();
            sb.AppendLine($"chapter notes: {visible.Count} retrievable" +
                          FlaggedTally(c, own, $"corpus: \"{corpusArg}\""));

            var pps = c.PlotPoints.Where(p => p.ChapterId == ch.Id).OrderBy(p => p.OrderInChapter).ToList();
            sb.AppendLine($"plot points ({pps.Count}):");
            foreach (var pp in pps)
            {
                var ppNotes = c.NotesByOwner.TryGetValue((OwnerType.PlotPoint, pp.Id), out var pl) ? pl : [];
                var links = c.LinksByPlotPoint.TryGetValue(pp.Id, out var ll) ? ll.Count : 0;
                var vis = ppNotes.Count(n => n.NoteState != NoteState.Flagged);
                var flg = ppNotes.Count - vis;
                sb.AppendLine($"  \"{pp.Title}\" (pos {pp.OrderInChapter}) — {vis} notes{(flg > 0 ? $" (+{flg} flagged)" : "")}, {links} links (plotpoint:{pp.Id})");
            }

            if (includeNotes && visible.Count > 0)
                AppendNotesGroupedByTrack(sb, c, visible);
            sb.AppendLine();
        }
        return Query.Cap(sb);
    }

    // ── fetch: links ────────────────────────────────────────────────────────────

    public static string GetLinks(PlanCache c, int[] ids, bool includeNotes)
    {
        var sb = new StringBuilder();
        var corpusArg = c.Corpus == Corpus.Working ? "working" : "archive";
        foreach (var id in ids.Distinct())
        {
            if (!c.LinkById.TryGetValue(id, out var l))
            {
                sb.AppendLine($"## link:{id} — not found in {Query.CorpusName(c.Corpus)}");
                continue;
            }
            var subjLabel = c.SubjectById.TryGetValue(l.SubjectId, out var s) ? $"{s.Name} (subject:{s.Id})" : $"subject:{l.SubjectId}(missing)";
            sb.AppendLine($"## {Query.OwnerLabel(c, OwnerType.PlotPoint, l.PlotPointId)} x {subjLabel} (link:{l.Id}, plotpoint:{l.PlotPointId})");

            var own = c.NotesByOwner.TryGetValue((OwnerType.PlotPointSubjectLink, l.Id), out var list) ? list : [];
            var visible = own.Where(n => n.NoteState != NoteState.Flagged).ToList();
            sb.AppendLine($"link notes: {visible.Count} retrievable" +
                          FlaggedTally(c, own, $"corpus: \"{corpusArg}\""));

            if (includeNotes && visible.Count > 0)
                AppendNotesGroupedByTrack(sb, c, visible);
            sb.AppendLine();
        }
        return Query.Cap(sb);
    }

    // ── theme notes ─────────────────────────────────────────────────────────────

    public static string GetThemeNotes(PlanCache c, string theme)
    {
        Theme? t = null;
        if (int.TryParse(theme, out var tid))
            c.ThemeById.TryGetValue(tid, out t);
        t ??= c.Themes.FirstOrDefault(x => string.Equals(x.Name, theme, StringComparison.OrdinalIgnoreCase));
        if (t is null)
            return $"Theme \"{theme}\" not found in {Query.CorpusName(c.Corpus)}. Known: " +
                   string.Join(", ", c.Themes.Select(x => $"{x.Id}:{x.Name}"));

        var tagged = c.Notes.Where(n => n.ThemeId == t.Id).ToList();
        var visible = tagged.Where(n => n.NoteState != NoteState.Flagged).ToList();

        var sb = new StringBuilder();
        sb.AppendLine($"# \"{t.Name}\" (theme:{t.Id}) in {Query.CorpusName(c.Corpus)}");
        sb.AppendLine($"proposition: {t.Proposition}");
        sb.AppendLine($"tagged notes: {visible.Count} retrievable" + FlaggedTally(c, tagged, $"corpus: \"{(c.Corpus == Corpus.Working ? "working" : "archive")}\""));
        sb.AppendLine();
        foreach (var n in visible.OrderBy(n => n.OwnerType).ThenBy(n => n.OwnerId).ThenBy(n => n.SortOrder))
            AppendNoteBlock(sb, c, n);
        return Query.Cap(sb);
    }

    // ── chronology ──────────────────────────────────────────────────────────────

    public static string GetNotesInDateRange(PlanCache c, int? fromYear, int? toYear)
    {
        var dated = c.Notes.Where(Query.HasAnyWorldDate).ToList();
        var parsedNotes = new List<(Note n, double earliest, double latest)>();
        int unparseable = 0;
        foreach (var n in dated)
        {
            if (Query.EffectiveWorldDate(n) is { } d)
                parsedNotes.Add((n,
                    d.EarliestFraction ?? double.NegativeInfinity,   // start TBD ("..1007")
                    d.End is not null ? d.LatestFraction!.Value
                        : IsConditionTrack(c, n) ? double.PositiveInfinity // in force, end TBD
                        : d.LatestFraction ?? double.PositiveInfinity));
            else unparseable++;
        }

        double lo = fromYear ?? double.NegativeInfinity;
        double hi = toYear is int ty ? ty + 1.0 : double.PositiveInfinity; // inclusive year → exclusive edge
        var inRange = parsedNotes
            .Where(x => x.latest >= lo && x.earliest < hi)
            .OrderBy(x => x.earliest).ThenBy(x => x.latest).ThenBy(x => x.n.Id)
            .ToList();
        var visible = inRange.Where(x => x.n.NoteState != NoteState.Flagged).ToList();
        var flaggedInRange = inRange.Count - visible.Count;

        var sb = new StringBuilder();
        var rangeLabel = $"{(fromYear?.ToString() ?? "-inf")}..{(toYear?.ToString() ?? "+inf")}";
        sb.AppendLine($"# chronology {Query.CorpusName(c.Corpus)} [{rangeLabel}] — {visible.Count} notes" +
                      (flaggedInRange > 0 ? $" (+{flaggedInRange} flagged, walled)" : "") +
                      $"; dated notes total: {dated.Count}, unparseable WorldDate values: {unparseable}");
        sb.AppendLine("# sorted chronologically (structured world dates; legacy free-text values converted mechanically, never guessed)");
        sb.AppendLine();
        foreach (var (n, _, _) in visible)
            AppendNoteBlock(sb, c, n);
        return Query.Cap(sb);
    }

    /// <summary>A start-only date on a condition track means "in force, end TBD" — for range
    /// intersection it extends to +inf, unlike the same stored value on an event track.</summary>
    private static bool IsConditionTrack(PlanCache c, Note n) =>
        n.NoteTrackDefinitionId is int id && c.TrackById.TryGetValue(id, out var t) && t.SupportsWorldDateEnd;

    // ── generic count/group ─────────────────────────────────────────────────────

    private static readonly string[] ValidDims =
        ["state", "track", "trackType", "ownerType", "subject", "subjectType", "chapter", "story", "theme", "source", "hasWorldDate", "theater", "dateShape", "worldDateYear"];

    public static string CountNotes(PlanCache c, string[] groupBy)
    {
        if (groupBy.Length == 0) groupBy = ["state"];
        if (groupBy.Length > 3) return "groupBy supports at most 3 dimensions.";
        foreach (var d in groupBy)
            if (!ValidDims.Contains(d))
                return $"Unknown dimension \"{d}\". Valid: {string.Join(", ", ValidDims)}";

        string Dim(Note n, string dim) => dim switch
        {
            "state" => Query.StateLabel(c.Corpus, n.NoteState),
            "track" => Query.TrackName(c, n),
            "trackType" => n.NoteTrackDefinitionId is int id && c.TrackById.TryGetValue(id, out var t)
                ? t.TrackType.ToString() : "(untracked)",
            "ownerType" => n.OwnerType.ToString(),
            "subject" => SubjectDim(c, n),
            "subjectType" => SubjectTypeDim(c, n),
            "chapter" => ChapterDim(c, n),
            "story" => StoryDim(c, n),
            "theme" => n.ThemeId is int tid
                ? (c.ThemeById.TryGetValue(tid, out var th) ? th.Name : $"theme:{tid}?")
                : "(no theme)",
            // Multi-valued (a note may cite several Parts) — composite label, not multi-membership,
            // consistent with every other dimension here producing one group key per note.
            "source" => Query.SourceLabel(c, n) is { Length: > 0 } label ? label["source:".Length..] : "(no source)",
            "hasWorldDate" => Query.HasAnyWorldDate(n) ? "yes" : "no",
            "theater" => TheaterDim(c, n),
            "dateShape" => DateShapeDim(c, n),
            "worldDateYear" => WorldDateYearDim(c, n),
            _ => "?"
        };

        var rows = c.Notes
            .GroupBy(n => string.Join(" | ", groupBy.Select(d => Dim(n, d))))
            .Select(g => (Key: g.Key, Count: g.Count()))
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Key)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine($"# count_notes {Query.CorpusName(c.Corpus)} by {string.Join(",", groupBy)} — {rows.Count} rows, {c.Notes.Count} notes total");
        sb.AppendLine("# counts include flagged notes (counts are numbers, not content; use the state dimension to see them)");
        var shown = 0;
        foreach (var (key, count) in rows)
        {
            if (shown++ >= 500) { sb.AppendLine($"[... {rows.Count - 500} more rows — add dimensions or filter]"); break; }
            sb.AppendLine($"{key} | {count}");
        }
        return Query.Cap(sb);
    }

    // "subject" dim: subject-owned notes → subject name; link-owned notes → the linked
    // subject's name (mechanical join); plot-point/chapter-owned → "(not subject-owned)".
    private static string SubjectDim(PlanCache c, Note n)
    {
        int? subjectId = n.OwnerType switch
        {
            OwnerType.Subject => n.OwnerId,
            OwnerType.PlotPointSubjectLink => c.LinkById.TryGetValue(n.OwnerId, out var l) ? l.SubjectId : null,
            _ => null
        };
        if (subjectId is null) return "(not subject-owned)";
        return c.SubjectById.TryGetValue(subjectId.Value, out var s) ? s.Name : $"subject:{subjectId}?";
    }

    // "worldDateYear" dim: the note's START year (events and conditions alike). Crossed with
    // "theater" this reports COLLISION DENSITY — how many items share one (column, year) cell,
    // which is what the canvas must draw as a group while year remains the working precision.
    private static string WorldDateYearDim(PlanCache c, Note n)
    {
        var date = Query.EffectiveWorldDate(n);
        if (date is null) return Query.HasAnyWorldDate(n) ? "(unparsed)" : "(undated)";
        return date.Value.Start?.Year.ToString() ?? "(start TBD)";
    }

    // "theater" dim: the timeline column a note renders in — its owning subject's placement.
    // Only subject-owned notes have one (link-owned notes resolve through their subject);
    // TheaterId 0 is "(Unplaced)", a legal authorial state, not a missing reference.
    private static string TheaterDim(PlanCache c, Note n)
    {
        int? subjectId = n.OwnerType switch
        {
            OwnerType.Subject => n.OwnerId,
            OwnerType.PlotPointSubjectLink => c.LinkById.TryGetValue(n.OwnerId, out var l) ? l.SubjectId : null,
            _ => null
        };
        if (subjectId is null) return "(not subject-owned)";
        if (!c.SubjectById.TryGetValue(subjectId.Value, out var s)) return $"subject:{subjectId}?";
        if (s.TheaterId == 0) return "(Unplaced)";
        return c.TheaterById.TryGetValue(s.TheaterId, out var t) ? t.Name : $"theater:{s.TheaterId}?";
    }

    // "dateShape" dim: how a dated note renders on the timeline — the event/condition split
    // comes from the TRACK, and the precision from the value. Answers "what will the canvas
    // actually draw", which is what governs layout work.
    private static string DateShapeDim(PlanCache c, Note n)
    {
        var date = Query.EffectiveWorldDate(n);
        if (date is null) return Query.HasAnyWorldDate(n) ? "unparsed (triage)" : "undated";
        var isCondition = n.NoteTrackDefinitionId is int id
                          && c.TrackById.TryGetValue(id, out var t) && t.SupportsWorldDateEnd;
        var d = date.Value;
        if (!isCondition)
            return d.Start?.Month is null ? "event (year precision → glyph)"
                 : d.Start?.Day is null ? "event (month precision)" : "event (day precision)";
        if (d.End is null) return "condition (open-ended)";
        var extent = (d.End.Value.Year - (d.Start?.Year ?? d.End.Value.Year));
        return extent >= 100 ? "condition (span ≥100y)"
             : extent >= 10 ? "condition (span 10-99y)" : "condition (span <10y)";
    }

    private static string SubjectTypeDim(PlanCache c, Note n)
    {
        int? subjectId = n.OwnerType switch
        {
            OwnerType.Subject => n.OwnerId,
            OwnerType.PlotPointSubjectLink => c.LinkById.TryGetValue(n.OwnerId, out var l) ? l.SubjectId : null,
            _ => null
        };
        if (subjectId is null) return "(not subject-owned)";
        if (!c.SubjectById.TryGetValue(subjectId.Value, out var s)) return $"subject:{subjectId}?";
        return c.SubjectDefById.TryGetValue(s.SubjectDefinitionId, out var d) ? d.SubjectType : "?";
    }

    // "chapter" dim: plot-point-owned → its chapter; link-owned → the link's plot point's
    // chapter; chapter-owned → that chapter; subject-owned → "(no chapter)".
    private static string ChapterDim(PlanCache c, Note n)
    {
        int? ppId = n.OwnerType switch
        {
            OwnerType.PlotPoint => n.OwnerId,
            OwnerType.PlotPointSubjectLink => c.LinkById.TryGetValue(n.OwnerId, out var l) ? l.PlotPointId : null,
            _ => null
        };
        if (n.OwnerType == OwnerType.Chapter)
            return c.ChapterById.TryGetValue(n.OwnerId, out var chOwn) ? $"CH#{chOwn.OrderIndex} {chOwn.Title}" : $"chapter:{n.OwnerId}?";
        if (ppId is null) return "(no chapter)";
        if (!c.PlotPointById.TryGetValue(ppId.Value, out var pp)) return $"plotpoint:{ppId}?";
        if (pp.ChapterId is null) return "(unplaced plot point)";
        return c.ChapterById.TryGetValue(pp.ChapterId.Value, out var ch) ? $"{Query.ChapterLabel(c, ch)} {ch.Title}" : $"chapter:{pp.ChapterId}?";
    }

    // "story" dim: Chapter-owned -> its story; PlotPoint-owned -> its chapter's story;
    // Link-owned -> its plot point's chapter's story; Subject-owned -> "(no story)".
    private static string StoryDim(PlanCache c, Note n)
    {
        int? chapterId = n.OwnerType switch
        {
            OwnerType.Chapter => n.OwnerId,
            OwnerType.PlotPoint => c.PlotPointById.TryGetValue(n.OwnerId, out var pp) ? pp.ChapterId : null,
            OwnerType.PlotPointSubjectLink => c.LinkById.TryGetValue(n.OwnerId, out var l)
                && c.PlotPointById.TryGetValue(l.PlotPointId, out var lpp) ? lpp.ChapterId : null,
            _ => null
        };
        if (chapterId is null) return "(no story)";
        return c.ChapterById.TryGetValue(chapterId.Value, out var ch) ? Query.StoryLabel(c, ch.StoryId) : "(no story)";
    }
}
