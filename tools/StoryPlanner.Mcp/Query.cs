using System.Text;
using System.Text.RegularExpressions;
using StoryPlanner.Core.Models;

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
                    ? $"CH#{ch.OrderIndex} \"{ch.Title}\""
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
            ? $" (CH#{ch.OrderIndex} \"{ch.Title}\")"
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

    // ── WorldDate: raw free text plus a mechanical parse (never guessed) ────────

    private static readonly Regex WorldDateRx =
        new(@"^\s*(-?\d+)\s*-\s*(-?\d+)\s*$|^\s*(-?\d+)\s*$", RegexOptions.Compiled);

    public static (int Start, int End, bool Parsed) ParseWorldDate(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return (0, 0, false);
        var m = WorldDateRx.Match(raw);
        if (!m.Success) return (0, 0, false);
        if (m.Groups[3].Success)
        {
            var y = int.Parse(m.Groups[3].Value);
            return (y, y, true);
        }
        return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), true);
    }

    public static string WorldDateLabel(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return "";
        var (start, end, parsed) = ParseWorldDate(raw);
        return parsed
            ? (start == end ? $"wd:{raw.Trim()}" : $"wd:{raw.Trim()} [{start}..{end}]")
            : $"wd:\"{raw.Trim()}\" (unparsed)";
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
