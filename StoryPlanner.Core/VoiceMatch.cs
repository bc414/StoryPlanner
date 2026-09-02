using System.Text;

namespace StoryPlanner.Core;

/// <summary>
/// Mechanical provenance for note text: does this text exist in a dated corpus of prior
/// sources, and which source contains it earliest? Pure — no I/O, no opinion about registers
/// or adoption. The unit of comparison is a k-word shingle (a sliding window of k consecutive
/// words after <see cref="VoiceText.Tokenize"/> normalisation); a note's coverage is the share
/// of its shingles found anywhere in the index, and its origin is the single earliest-dated
/// source explaining the most of them. Built for WU1.4 (v1 archive vs lineage); the same
/// engine is what a future copy-paste-detection DataOp (implementation-candidates D19) would
/// wrap with a write path.
/// </summary>
public static class VoiceText
{
    public readonly record struct Token(string Text, int Start, int End);

    /// <summary>
    /// Lowercase alphanumeric runs with their offsets into the original string. Everything
    /// else — punctuation, apostrophes, markdown glyphs, whitespace — is a separator, so
    /// "you're" is two tokens and "**The Speech:**" is two. Both sides of a comparison go
    /// through this, which is what makes the comparison indifferent to formatting.
    /// </summary>
    public static List<Token> Tokenize(string text)
    {
        var tokens = new List<Token>();
        if (string.IsNullOrEmpty(text)) return tokens;
        int i = 0, n = text.Length;
        var sb = new StringBuilder();
        while (i < n)
        {
            while (i < n && !char.IsLetterOrDigit(text[i])) i++;
            if (i >= n) break;
            int start = i;
            sb.Clear();
            while (i < n && char.IsLetterOrDigit(text[i])) { sb.Append(char.ToLowerInvariant(text[i])); i++; }
            tokens.Add(new Token(sb.ToString(), start, i));
        }
        return tokens;
    }

    /// <summary>Normalised text: tokens joined by single spaces. Used for containment checks.</summary>
    public static string Normalize(string text)
    {
        var tokens = Tokenize(text);
        var sb = new StringBuilder();
        for (int i = 0; i < tokens.Count; i++)
        {
            if (i > 0) sb.Append(' ');
            sb.Append(tokens[i].Text);
        }
        return sb.ToString();
    }

    /// <summary>64-bit FNV-1a over the k tokens starting at <paramref name="at"/>, with a separator byte between tokens.</summary>
    public static ulong ShingleHash(List<Token> tokens, int at, int k)
    {
        const ulong prime = 1099511628211UL;
        ulong h = 14695981039346656037UL;
        for (int t = at; t < at + k; t++)
        {
            var s = tokens[t].Text;
            for (int c = 0; c < s.Length; c++) { h ^= s[c]; h *= prime; }
            h ^= 0x1F; h *= prime;
        }
        return h;
    }

    public static IEnumerable<ulong> Shingles(List<Token> tokens, int k)
    {
        for (int i = 0; i + k <= tokens.Count; i++) yield return ShingleHash(tokens, i, k);
    }
}

/// <summary>One dated body of text the index knows about — a Gemini response, a prompt, an AI Studio turn, a doc snapshot.</summary>
public sealed record VoiceSource(string Id, string Layer, string Role, DateOnly? Date);

public sealed record VoiceVote(VoiceSource Source, int Shingles);

/// <summary>A maximal run of consecutive note words covered by matched shingles, with the source the earliest-wins rule credits for its first shingle.</summary>
public sealed record MatchedRun(int StartWord, int Words, VoiceSource Source);

/// <summary>A maximal run of consecutive note words no shingle covers. Position: leading (before the first match), interior, or trailing.</summary>
public sealed record UncoveredRun(int StartWord, int Words, string Position);

/// <summary>A run of consecutive note words credited to ONE source, as character offsets [Start, End) into the original note text.</summary>
public sealed record SourceSpan(int Start, int End, int Words, VoiceSource Source);

/// <summary>
/// Result of matching one note against a <see cref="VoiceIndex"/>. Two coverage figures:
/// <c>Coverage</c> is matched shingles ÷ shingles considered (null when none could be — note
/// shorter than k words, or all-ubiquitous); <c>TokenCoverage</c> is covered words ÷ words,
/// which is the better verbatim-ness measure because a three-word cut costs three words, not k
/// windows. <c>Origin</c> is the earliest-dated source holding the plurality of matched
/// shingles; <c>OriginShare</c> its share of them; <c>Tie</c> true when a source of a different
/// Role ties it on both votes and date. <c>MatchedPassage</c> / <c>OriginPassage</c> /
/// <c>UncoveredText</c> are verbatim from the note: the longest covered run overall, the longest
/// credited to the origin, and the longest uncovered run.
/// </summary>
public sealed record VoiceMatchResult(
    int Words,
    int Shingles,
    int Considered,
    int Matched,
    double? Coverage,
    double TokenCoverage,
    VoiceSource? Origin,
    double OriginShare,
    bool Tie,
    string MatchedPassage,
    string OriginPassage,
    string UncoveredText,
    IReadOnlyList<MatchedRun> MatchedRuns,
    IReadOnlyList<UncoveredRun> UncoveredRuns,
    IReadOnlyList<SourceSpan> Spans,
    IReadOnlyList<VoiceVote> Votes)
{
    public int LongestRunWords => MatchedRuns.Count == 0 ? 0 : MatchedRuns.Max(r => r.Words);
    public int LongestUncoveredWords => UncoveredRuns.Count == 0 ? 0 : UncoveredRuns.Max(r => r.Words);

    public static VoiceMatchResult Empty(int words, int shingles, int considered, double? coverage) =>
        new(words, shingles, considered, 0, coverage, 0, null, 0, false, "", "", "", Array.Empty<MatchedRun>(), Array.Empty<UncoveredRun>(), Array.Empty<SourceSpan>(), Array.Empty<VoiceVote>());
}

/// <summary>
/// Thresholds for <see cref="VoiceLabel.Of"/>. Provisional until the calibration gate; every
/// value is a CLI argument in the tool. <c>R</c>: a matched run at least this many words is a
/// lift, not shared phrasing. <c>G</c>: an uncovered run at least this many words is the
/// author's writing, not an edit. <c>VerbatimCoverage</c>: token coverage at or above which a
/// gap-free paste is verbatim. <c>PasteScale</c>: a matched run at least this long makes the note
/// a paste rather than a note with a lift (default 2R).
/// </summary>
public sealed record LabelThresholds(int R = 8, int G = 6, double VerbatimCoverage = 0.90, int? PasteScale = null)
{
    public int PasteWords => PasteScale ?? 2 * R;
}

/// <summary>
/// The shape of a note's relationship to its sources — deliberately about structure, not
/// about who wrote it (that is the origin's Role, reported beside the label):
/// <c>verbatim</c> the note is a paste; <c>edited-paste</c> a paste with pieces cut or reworded
/// and nothing authored at sentence scale; <c>framed-paste</c> a paste with a sentence or more
/// of the author's around or inside it; <c>fragment</c> a sentence-scale lift inside a note
/// that is otherwise not a paste; <c>phrase</c> shared phrasing only, never attribution;
/// <c>none</c> no captured source; <c>short</c> fewer than k words, unmeasurable.
/// </summary>
public static class VoiceLabel
{
    public const string Verbatim = "verbatim", EditedPaste = "edited-paste", FramedPaste = "framed-paste",
        Fragment = "fragment", Phrase = "phrase", None = "none", Short = "short";

    public static readonly string[] All = { Verbatim, EditedPaste, FramedPaste, Fragment, Phrase, None, Short };

    public static string Of(VoiceMatchResult m, LabelThresholds t)
    {
        if (m.Shingles == 0 || m.Coverage is null) return Short;
        if (m.Matched == 0) return None;
        int longest = m.LongestRunWords;
        bool lift = longest >= t.R;
        bool paste = longest >= t.PasteWords || (lift && m.TokenCoverage >= 0.5);
        bool authored = m.LongestUncoveredWords >= t.G;
        if (paste)
        {
            if (authored) return FramedPaste;
            // A pure deletion leaves every remaining word covered (token coverage 1.0) but
            // breaks the shingles across the seam; so verbatim needs BOTH coverages high.
            return m.TokenCoverage >= t.VerbatimCoverage && m.Coverage >= t.VerbatimCoverage ? Verbatim : EditedPaste;
        }
        return lift ? Fragment : Phrase;
    }
}

/// <summary>
/// Shingle index over dated sources. Per shingle it keeps the earliest-dated source that
/// contains it and the number of distinct sources that do — the latter drives the
/// ubiquitous-shingle filter at match time ("the lioness of tall tale" is in everything and
/// proves nothing). Sources with a null date sort after every dated source; among sources of
/// the same date the one added first wins, which is how a Gemini prompt (added before its
/// response) beats the response that repeats it.
/// </summary>
public sealed class VoiceIndex
{
    private struct Entry { public int SourceIdx; public int SourceCount; }

    private readonly Dictionary<ulong, Entry> _map = new();
    private readonly List<VoiceSource> _sources = new();

    public int K { get; }
    public int SourceCount => _sources.Count;
    public int ShingleCount => _map.Count;

    public VoiceIndex(int k = 6)
    {
        if (k < 2) throw new ArgumentOutOfRangeException(nameof(k));
        K = k;
    }

    public void Add(VoiceSource source, string text)
    {
        var tokens = VoiceText.Tokenize(text);
        if (tokens.Count < K) return;
        int idx = _sources.Count;
        _sources.Add(source);
        var seen = new HashSet<ulong>();
        foreach (var h in VoiceText.Shingles(tokens, K))
        {
            if (!seen.Add(h)) continue;
            if (_map.TryGetValue(h, out var e))
            {
                e.SourceCount++;
                if (Earlier(source, _sources[e.SourceIdx])) e.SourceIdx = idx;
                _map[h] = e;
            }
            else _map[h] = new Entry { SourceIdx = idx, SourceCount = 1 };
        }
    }

    private static bool Earlier(VoiceSource a, VoiceSource b)
    {
        if (a.Date is null) return false;
        if (b.Date is null) return true;
        return a.Date.Value < b.Date.Value; // equal dates: the earlier-added source keeps the shingle
    }

    /// <param name="maxSources">Shingles found in more than this many distinct sources are ignored; 0 = no filter.</param>
    public VoiceMatchResult Match(string noteText, int maxSources = 0)
    {
        var tokens = VoiceText.Tokenize(noteText);
        int total = Math.Max(0, tokens.Count - K + 1);
        if (total == 0) return VoiceMatchResult.Empty(tokens.Count, 0, 0, null);

        int considered = 0, matched = 0;
        var votes = new Dictionary<int, int>();
        var hitSource = new int[total];
        Array.Fill(hitSource, -1);
        for (int i = 0; i < total; i++)
        {
            var h = VoiceText.ShingleHash(tokens, i, K);
            if (_map.TryGetValue(h, out var e))
            {
                if (maxSources > 0 && e.SourceCount > maxSources) continue;
                considered++; matched++; hitSource[i] = e.SourceIdx;
                votes[e.SourceIdx] = votes.GetValueOrDefault(e.SourceIdx) + 1;
            }
            else considered++;
        }

        double? coverage = considered == 0 ? null : (double)matched / considered;
        if (matched == 0) return VoiceMatchResult.Empty(tokens.Count, total, considered, coverage);

        var ranked = votes
            .Select(kv => new VoiceVote(_sources[kv.Key], kv.Value))
            .OrderByDescending(v => v.Shingles)
            .ThenBy(v => v.Source.Date ?? DateOnly.MaxValue)
            .ThenBy(v => v.Source.Id, StringComparer.Ordinal)
            .ToList();
        var origin = ranked[0];
        bool tie = ranked.Skip(1).Any(v =>
            v.Shingles == origin.Shingles && v.Source.Date == origin.Source.Date && v.Source.Role != origin.Source.Role);
        var originSource = origin.Source;
        int originIdx = _sources.IndexOf(originSource);

        // Token mask: word w is covered if any matched shingle spans it. Credit each covered
        // word to the source of the first matched shingle that covers it.
        var wordSource = new int[tokens.Count];
        Array.Fill(wordSource, -1);
        for (int i = 0; i < total; i++)
            if (hitSource[i] >= 0)
                for (int w = i; w < i + K; w++)
                    if (wordSource[w] < 0) wordSource[w] = hitSource[i];
        int covered = wordSource.Count(x => x >= 0);

        var matchedRuns = new List<MatchedRun>();
        var uncoveredRuns = new List<UncoveredRun>();
        int runStart = 0;
        for (int w = 1; w <= tokens.Count; w++)
        {
            bool boundary = w == tokens.Count || (wordSource[w] >= 0) != (wordSource[runStart] >= 0);
            if (!boundary) continue;
            int len = w - runStart;
            if (wordSource[runStart] >= 0) matchedRuns.Add(new MatchedRun(runStart, len, _sources[wordSource[runStart]]));
            else uncoveredRuns.Add(new UncoveredRun(runStart, len, runStart == 0 ? "leading" : w == tokens.Count ? "trailing" : "interior"));
            runStart = w;
        }

        string Slice(int startWord, int words) =>
            noteText.Substring(tokens[startWord].Start, tokens[startWord + words - 1].End - tokens[startWord].Start);
        string Longest(IEnumerable<(int Start, int Words)> runs)
        {
            var best = runs.OrderByDescending(r => r.Words).FirstOrDefault();
            return best.Words == 0 ? "" : Slice(best.Start, best.Words);
        }
        var passage = Longest(matchedRuns.Select(r => (r.StartWord, r.Words)));
        // Origin passage: contiguous words credited to the origin specifically. A covered run can
        // straddle two sources (a note stitched from two responses), so it is recomputed on the
        // per-word credit rather than taken from the coverage runs.
        var originRuns = new List<(int Start, int Words)>();
        for (int w = 0, rs = -1; w <= tokens.Count; w++)
        {
            bool on = w < tokens.Count && wordSource[w] == originIdx;
            if (on && rs < 0) rs = w;
            if (!on && rs >= 0) { originRuns.Add((rs, w - rs)); rs = -1; }
        }
        var originPassage = Longest(originRuns);
        var uncoveredText = Longest(uncoveredRuns.Select(r => (r.StartWord, r.Words)));

        // Source spans: every covered run split at source boundaries, with character offsets
        // into the original note — the per-word credit rendered as text spans, for marking AI
        // text inside a note (and the shape a future copy-paste-detection op would persist).
        var spans = new List<SourceSpan>();
        for (int w = 0, rs = -1; w <= tokens.Count; w++)
        {
            bool boundary = w == tokens.Count || rs < 0 || wordSource[w] != wordSource[rs];
            if (!boundary) continue;
            if (rs >= 0 && wordSource[rs] >= 0)
                spans.Add(new SourceSpan(tokens[rs].Start, tokens[w - 1].End, w - rs, _sources[wordSource[rs]]));
            rs = w;
        }

        return new VoiceMatchResult(tokens.Count, total, considered, matched, coverage, (double)covered / tokens.Count,
            originSource, (double)origin.Shingles / matched, tie, passage, originPassage, uncoveredText,
            matchedRuns, uncoveredRuns, spans, ranked);
    }

    /// <summary>The most widely shared shingles — for eyeballing what a ubiquity cutoff would drop. Returns (source count, sample source id).</summary>
    public IReadOnlyList<(int SourceCount, ulong Hash)> MostUbiquitous(int take) =>
        _map.OrderByDescending(kv => kv.Value.SourceCount).Take(take).Select(kv => (kv.Value.SourceCount, kv.Key)).ToList();
}

/// <summary>
/// Dated full-text snapshots of the plan itself (v1 database backups, pre-AI doc snapshots),
/// answering "when did this note's text first exist in the plan?" A note is contained in a
/// snapshot when every one of its shingles is present there; a note shorter than k words falls
/// back to normalised-substring search. Schema-agnostic on purpose: callers feed it every text
/// column of every table, because the v1 schema drifted weekly and note ids never joined.
/// </summary>
public sealed class PlanSnapshotIndex
{
    public sealed record Snapshot(string Name, DateOnly Date);

    private readonly List<(Snapshot Snap, HashSet<ulong> Shingles, string Text)> _snaps = new();
    public int K { get; }

    public PlanSnapshotIndex(int k = 6) { K = k; }

    public IReadOnlyList<Snapshot> Snapshots => _snaps.Select(s => s.Snap).OrderBy(s => s.Date).ToList();

    public void Add(Snapshot snapshot, IEnumerable<string> texts)
    {
        var set = new HashSet<ulong>();
        var sb = new StringBuilder();
        foreach (var t in texts)
        {
            if (string.IsNullOrWhiteSpace(t)) continue;
            var tokens = VoiceText.Tokenize(t);
            foreach (var h in VoiceText.Shingles(tokens, K)) set.Add(h);
            sb.Append(' ').Append(VoiceText.Normalize(t)).Append(' ');
        }
        _snaps.Add((snapshot, set, sb.ToString()));
    }

    public enum Verdict { Found, Absent, Ambiguous }

    /// <param name="minWordsForShort">Notes with fewer tokens than this are reported Ambiguous rather than searched — one or two words prove nothing.</param>
    public (Verdict Verdict, Snapshot? First) FirstAppearance(string noteText, int minWordsForShort = 4)
    {
        var tokens = VoiceText.Tokenize(noteText);
        if (tokens.Count < minWordsForShort) return (Verdict.Ambiguous, null);
        Snapshot? first = null;
        if (tokens.Count >= K)
        {
            var shingles = VoiceText.Shingles(tokens, K).ToList();
            foreach (var (snap, set, _) in _snaps)
            {
                if (first is not null && snap.Date >= first.Date) continue;
                bool all = true;
                foreach (var h in shingles) if (!set.Contains(h)) { all = false; break; }
                if (all) first = snap;
            }
        }
        else
        {
            var needle = " " + VoiceText.Normalize(noteText) + " ";
            foreach (var (snap, _, text) in _snaps)
            {
                if (first is not null && snap.Date >= first.Date) continue;
                if (text.Contains(needle, StringComparison.Ordinal)) first = snap;
            }
        }
        return first is null ? (Verdict.Absent, null) : (Verdict.Found, first);
    }
}
