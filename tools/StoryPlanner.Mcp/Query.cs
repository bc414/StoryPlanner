using System.Text;
using System.Text.RegularExpressions;
using StoryPlanner.Core;

namespace StoryPlanner.Mcp;

/// <summary>
/// Schema-truth helpers: per-file state labels, polymorphic owner resolution,
/// mechanical WorldDate parsing, regex construction, snippet extraction.
/// These report what is — they never rank, filter by relevance, or interpret.
/// </summary>
public static class Query
{
    public const int MaxOutputChars = 60_000;

    // ── State labels (per-file semantics — never emit raw enum values) ──────────

    public static string StateLabel(Corpus corpus, NoteState state) => corpus switch
    {
        Corpus.Working => state switch
        {
            NoteState.Unset => "unset",
            NoteState.Flagged => "flagged",
            NoteState.Confirmed => "confirmed",
            _ => $"state{(int)state}"
        },
        // Archive semantics: "closed" = reviewed, no longer needs attention. Whether the
        // content was migrated to v2 or deliberately superseded was NOT recorded.
        _ => state switch
        {
            NoteState.Unset => "open",
            NoteState.Flagged => "flagged",
            NoteState.Confirmed => "closed(disposition-not-recorded)",
            _ => $"state{(int)state}"
        }
    };

    public static string CorpusName(Corpus corpus) =>
        corpus == Corpus.Working ? "working-plan(v2)" : "archive(v1)";

    // ── Owner resolution (no FKs in the schema — this join is done here, once) ──

    /// <summary>
    /// "{Abbreviation or Title} CH#{n}" — e.g. "TLTT CH#12" or "(Unassigned) CH#5". StoryId = 0
    /// is the permanent "(Unassigned)" sentinel (see UnassignedStory), not a missing reference.
    /// </summary>
    public static string ChapterLabel(PlanCache c, Chapter ch) =>
        $"{StoryLabel(c, ch.StoryId)} CH#{ch.OrderIndex}";

    public static string StoryLabel(PlanCache c, int storyId)
    {
        if (storyId == 0) return "(Unassigned)";
        if (!c.StoryById.TryGetValue(storyId, out var s)) return $"story:{storyId}(missing)";
        return string.IsNullOrEmpty(s.Abbreviation) ? s.Title : s.Abbreviation;
    }

    /// <summary>
    /// Name-led label for an owner — the id is available separately via <see cref="OwnerRef"/>
    /// for graph navigation, kept out of this string so callers can lead output with the name
    /// and demote the id to a trailing parenthetical instead of the id leading every line.
    /// </summary>
    public static string OwnerLabel(PlanCache c, OwnerType type, int ownerId)
    {
        switch (type)
        {
            case OwnerType.Subject:
                return c.SubjectById.TryGetValue(ownerId, out var s)
                    ? $"{s.Name}"
                    : $"subject:{ownerId}(missing)";
            case OwnerType.PlotPoint:
                if (!c.PlotPointById.TryGetValue(ownerId, out var pp))
                    return $"plotpoint:{ownerId}(missing)";
                return $"PP \"{pp.Title}\"{ChapterSuffix(c, pp)}";
            case OwnerType.Chapter:
                return c.ChapterById.TryGetValue(ownerId, out var ch)
                    ? $"{ChapterLabel(c, ch)} \"{ch.Title}\""
                    : $"chapter:{ownerId}(missing)";
            case OwnerType.PlotPointSubjectLink:
                if (!c.LinkById.TryGetValue(ownerId, out var link))
                    return $"link:{ownerId}(missing)";
                var subjName = c.SubjectById.TryGetValue(link.SubjectId, out var ls) ? ls.Name : $"subject:{link.SubjectId}?";
                var ppTitle = c.PlotPointById.TryGetValue(link.PlotPointId, out var lp) ? lp.Title : $"plotpoint:{link.PlotPointId}?";
                return $"LINK \"{ppTitle}\" x {subjName}";
            default:
                return $"{type}:{ownerId}";
        }
    }

    /// <summary>The callable id for the owner entity — distinct from the note/edge's own id,
    /// this is what a follow-up fetch call uses. Always placed at the end of a composed line.</summary>
    public static string OwnerRef(OwnerType type, int ownerId) => type switch
    {
        OwnerType.Subject => $"subject:{ownerId}",
        OwnerType.PlotPoint => $"plotpoint:{ownerId}",
        OwnerType.Chapter => $"chapter:{ownerId}",
        OwnerType.PlotPointSubjectLink => $"link:{ownerId}",
        _ => $"owner:{(int)type}:{ownerId}"
    };

    private static string ChapterSuffix(PlanCache c, PlotPoint pp)
    {
        if (pp.ChapterId is null) return " (unplaced)";
        return c.ChapterById.TryGetValue(pp.ChapterId.Value, out var ch)
            ? $" ({ChapterLabel(c, ch)} \"{ch.Title}\")"
            : $" (chapter:{pp.ChapterId} missing)";
    }

    public static string TrackName(PlanCache c, Note n) =>
        n.NoteTrackDefinitionId is int id && c.TrackById.TryGetValue(id, out var t)
            ? t.TrackName
            : "(untracked)";

    public static string TrackLabel(PlanCache c, Note n)
    {
        if (n.NoteTrackDefinitionId is int id && c.TrackById.TryGetValue(id, out var t))
            return $"{t.TrackName} [{t.TrackType}]";
        return "(untracked)";
    }

    // ── WorldDate: structured columns first, legacy free text as fallback ───────
    // The structured columns (StoryPlanner.Core.WorldDate) are the source of truth once the
    // convert-world-dates DataOps op has run on a file; until then notes still carry the
    // legacy free-text string, which is converted on read here — mechanically, never guessed
    // (WorldDateLegacy). Both paths share Core's one implementation.

    /// <summary>The note's date regardless of conversion state; null = undated or unconvertible.</summary>
    public static WorldDate? EffectiveWorldDate(Note n)
    {
        try
        {
            var structured = n.GetWorldDate();
            if (structured is not null) return structured;
        }
        catch (ArgumentException)
        {
            return null; // malformed columns — report as unparsed, never guess
        }
        var outcome = WorldDateLegacy.TryConvert(n.WorldDate, out var legacy);
        return outcome is WorldDateLegacy.Outcome.Point or WorldDateLegacy.Outcome.Range ? legacy : null;
    }

    /// <summary>True when the note carries ANY date signal — structured or legacy text,
    /// including unconvertible legacy text ("?" is a date-shaped claim, just not a usable one).</summary>
    public static bool HasAnyWorldDate(Note n) =>
        n.WorldDateStartYear is not null || n.WorldDateEndYear is not null ||
        !string.IsNullOrWhiteSpace(n.WorldDate);

    public static string WorldDateLabel(Note n)
    {
        if (!HasAnyWorldDate(n)) return "";
        var date = EffectiveWorldDate(n);
        return date is { } d
            ? $"wd:{d.ToNotation(d.End is not null)}"
            : $"wd:\"{n.WorldDate.Trim()}\" (unparsed)";
    }

    // ── Source material: a note may cite several Parts for one claim ────────────
    // (e.g. "the Wonderbolts were useless in a crisis" citing four episodes) — see
    // NoteSourceReference's doc comment. All are rendered, comma-joined, never just the first.

    public static string SourceLabel(PlanCache c, Note n)
    {
        if (!c.SourceReferencesByNote.TryGetValue(n.Id, out var refs) || refs.Count == 0) return "";
        var citations = refs
            .Select(r => FormatSourceCitation(c, r))
            .Where(s => s.Length > 0);
        return $"source:{string.Join(",", citations)}";
    }

    private static string FormatSourceCitation(PlanCache c, NoteSourceReference r)
    {
        if (!c.SourceMaterialById.TryGetValue(r.SourceMaterialId, out var work)) return "";
        if (r.SourceMaterialPartId is int partId && c.SourceMaterialPartById.TryGetValue(partId, out var part))
            return $"{work.Name}·{part.Code}";
        return work.Name;
    }

    // ── Regex + snippets ────────────────────────────────────────────────────────

    public static Regex BuildRegex(string pattern, bool caseSensitive, bool wholeWord)
    {
        if (wholeWord) pattern = $@"\b(?:{pattern})\b";
        var opts = RegexOptions.None;
        if (!caseSensitive) opts |= RegexOptions.IgnoreCase;
        return new Regex(pattern, opts, TimeSpan.FromSeconds(2));
    }

    public static string Snippet(string text, Match m, int contextChars)
    {
        int start = Math.Max(0, m.Index - contextChars / 2);
        int len = Math.Min(text.Length - start, m.Length + contextChars);
        var s = text.Substring(start, len);
        s = Regex.Replace(s, @"\s+", " ").Trim();
        var prefix = start > 0 ? "…" : "";
        var suffix = start + len < text.Length ? "…" : "";
        return $"{prefix}{s}{suffix}";
    }

    public static string Truncate(string s, int max) =>
        s.Length <= max ? s : s[..max] + "…";

    public static string OneLine(string s) =>
        Regex.Replace(s, @"\s+", " ").Trim();

    /// <summary>Caps tool output at MaxOutputChars with an explicit truncation notice.</summary>
    public static string Cap(StringBuilder sb)
    {
        if (sb.Length <= MaxOutputChars) return sb.ToString();
        var s = sb.ToString(0, MaxOutputChars);
        return s + "\n\n[OUTPUT TRUNCATED at " + MaxOutputChars +
               " chars — narrow the request (fewer ids, tighter filters, smaller limit).]";
    }
}
