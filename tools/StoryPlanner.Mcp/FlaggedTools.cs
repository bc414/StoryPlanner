using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using ModelContextProtocol.Server;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Mcp;

/// <summary>
/// The flagged family — the only door to flagged notes (open questions). Deliberately
/// bifurcated from the ordinary tools: flagged notes are not stable enough to be a source
/// of truth, so their content and FlagReason text never appear in ordinary results.
/// FlagReason is itself a lore-adjacent corpus (the author drafts into it) and is fully
/// regex-searchable here.
/// </summary>
[McpServerToolType]
public sealed class FlaggedTools(StoryPlanSources sources)
{
    private static Corpus ParseCorpus(string corpus) =>
        corpus.Equals("archive", StringComparison.OrdinalIgnoreCase) ? Corpus.Archive : Corpus.Working;

    [McpServerTool(Name = "list_open_questions")]
    [Description("Compact index of flagged notes (open questions): id, owner, track, flag reason (truncated), content preview. These are NOT settled lore — each is a claim plus an unresolved obligation. Filter by subject, track, and/or a regex over content+reason. Full text via get_open_questions.")]
    public string ListOpenQuestions(
        [Description("\"working\" (v2, default) or \"archive\" (v1 — note: only 11 of 88 archive flags have a recorded reason).")] string corpus = "working",
        [Description("Filter to one subject by id or exact name (case-insensitive). Matches subject-owned notes and notes on that subject's scene links.")] string? subject = null,
        [Description("Filter to one track by exact track name (case-insensitive). Working plan only — archive notes are untracked.")] string? track = null,
        [Description("Regex filter over note content AND flag reason (.NET syntax, case-insensitive).")] string? pattern = null,
        [Description("Maximum rows (default 250).")] int limit = 250)
    {
        var c = sources.Get(ParseCorpus(corpus));
        limit = Math.Clamp(limit, 1, 500);

        Regex? rx = null;
        if (!string.IsNullOrWhiteSpace(pattern))
        {
            try { rx = Query.BuildRegex(pattern, caseSensitive: false, wholeWord: false); }
            catch (ArgumentException ex) { return $"Invalid regex: {ex.Message}"; }
        }

        // Resolve subject filter (id or exact name) to a subject id.
        int? subjectId = null;
        if (!string.IsNullOrWhiteSpace(subject))
        {
            if (int.TryParse(subject, out var sid) && c.SubjectById.ContainsKey(sid)) subjectId = sid;
            else
            {
                var match = c.Subjects.FirstOrDefault(s => string.Equals(s.Name, subject, StringComparison.OrdinalIgnoreCase));
                if (match is null)
                    return $"Subject \"{subject}\" not found in {Query.CorpusName(c.Corpus)} (exact name or id required — try list_subjects or search).";
                subjectId = match.Id;
            }
        }

        var all = c.Notes.Where(n => n.NoteState == NoteState.Flagged).ToList();
        IEnumerable<Note> filtered = all;

        if (subjectId is int sidv)
        {
            var linkIds = c.LinksBySubject.TryGetValue(sidv, out var ll)
                ? ll.Select(l => l.Id).ToHashSet()
                : [];
            filtered = filtered.Where(n =>
                (n.OwnerType == OwnerType.Subject && n.OwnerId == sidv) ||
                (n.OwnerType == OwnerType.PlotPointSubjectLink && linkIds.Contains(n.OwnerId)));
        }

        if (!string.IsNullOrWhiteSpace(track))
            filtered = filtered.Where(n => string.Equals(Query.TrackName(c, n), track, StringComparison.OrdinalIgnoreCase));

        if (rx is not null)
        {
            try { filtered = filtered.Where(n => rx.IsMatch(n.Content) || rx.IsMatch(n.FlagReason)).ToList(); }
            catch (RegexMatchTimeoutException) { return "Regex timed out (2s) — simplify the pattern."; }
        }

        var rows = filtered
            .OrderBy(n => Query.OwnerLabel(c, n.OwnerType, n.OwnerId), StringComparer.OrdinalIgnoreCase)
            .ThenBy(n => Query.TrackName(c, n))
            .ThenBy(n => n.Id)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine($"# open questions {Query.CorpusName(c.Corpus)} — {all.Count} flagged total, {rows.Count} after filters" +
                      (rows.Count > limit ? $", showing first {limit}" : "") +
                      ". R=flag reason, C=content preview. Full text: get_open_questions(ids).");
        var shown = 0;
        foreach (var n in rows)
        {
            if (shown++ >= limit) break;
            var r = n.FlagReason.Trim().Length == 0 ? "(no reason recorded)" : Query.Truncate(Query.OneLine(n.FlagReason), 200);
            var content = n.Content.Trim().Length == 0 ? "(empty — pure question)" : Query.Truncate(Query.OneLine(n.Content), 100);
            sb.AppendLine($"{Query.OwnerLabel(c, n.OwnerType, n.OwnerId)} · {Query.TrackName(c, n)} (q:{n.Id}) R:\"{r}\" C:\"{content}\"");
        }
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "get_open_questions")]
    [Description("Full detail for flagged notes by id: complete flag reason and complete content, with owner, track, WorldDate, and theme. Treat both as UNSTABLE — a flagged note is an open case (research directive, undecided design fork, placement doubt, or possibly-superseded claim), never settled lore.")]
    public string GetOpenQuestions(
        [Description("Note ids of flagged notes (from list_open_questions, or a count disclosure).")] int[] ids,
        [Description("\"working\" (v2, default) or \"archive\" (v1).")] string corpus = "working")
    {
        var c = sources.Get(ParseCorpus(corpus));
        var noteById = c.Notes.ToDictionary(n => n.Id);
        var sb = new StringBuilder();
        int found = 0, notFlagged = 0, missing = 0;
        var body = new StringBuilder();

        foreach (var id in ids.Distinct())
        {
            if (!noteById.TryGetValue(id, out var n))
            {
                missing++;
                body.AppendLine($"## q:{id} — not found in {Query.CorpusName(c.Corpus)}");
                continue;
            }
            if (n.NoteState != NoteState.Flagged)
            {
                notFlagged++;
                body.AppendLine($"## q:{id} — not flagged (state: {Query.StateLabel(c.Corpus, n.NoteState)}) — use get_notes_{(c.Corpus == Corpus.Working ? "plan" : "archive")}");
                continue;
            }
            found++;
            body.AppendLine($"## {Query.OwnerLabel(c, n.OwnerType, n.OwnerId)} · {Query.TrackLabel(c, n)} — FLAGGED (unstable) " +
                            $"(q:{n.Id}, {Query.OwnerRef(n.OwnerType, n.OwnerId)})");
            var meta = new List<string>();
            var wd = Query.WorldDateLabel(n.WorldDate);
            if (wd.Length > 0) meta.Add(wd);
            if (n.ThemeId is int tid)
                meta.Add(c.ThemeById.TryGetValue(tid, out var th) ? $"theme:{th.Name}" : $"theme:{tid}?");
            meta.Add($"modified:{n.LastModified:yyyy-MM-dd}");
            body.AppendLine(string.Join(" | ", meta));
            body.AppendLine("FLAG REASON:");
            body.AppendLine(n.FlagReason.Trim().Length == 0 ? "(no reason recorded)" : n.FlagReason.TrimEnd());
            body.AppendLine("CONTENT:");
            body.AppendLine(n.Content.Trim().Length == 0 ? "(empty — pure question)" : n.Content.TrimEnd());
            body.AppendLine();
        }

        sb.AppendLine($"# get_open_questions {Query.CorpusName(c.Corpus)} — {found} returned, {notFlagged} not flagged, {missing} not found");
        sb.Append(body);
        return Query.Cap(sb);
    }
}
