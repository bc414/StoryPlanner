using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.Lineage;

public sealed record NlmTurn(int TurnIndex, string Role, string Body);

/// <summary>
/// A studio-panel note as it appears in a saved capture: title + relative age ("179d ago",
/// relative to the capture date). Bodies are NOT in the saved DOM — the panel renders preview
/// cards only — so Body is empty until a capture that opened the note exists. The ingest
/// discloses that as a count, never fills it in.
/// </summary>
public sealed record NlmNote(int NoteIndex, string Title, string RelativeAge, string Body);

public sealed record NlmCapture(string Title, IReadOnlyList<NlmTurn> Turns, IReadOnlyList<NlmNote> Notes);

/// <summary>A parsed capture joined with its authored config row — the unit LineageDb writes.</summary>
public sealed record NlmNotebook(
    string Slug,
    string Title,
    string? AuthoredDate,
    string CaptureFile,
    string CapturedUtc,
    IReadOnlyList<NlmTurn> Turns,
    IReadOnlyList<NlmNote> Notes);

/// <summary>
/// Parses a NotebookLM notebook saved as a complete webpage (Ctrl+S on notebooklm.google.com
/// with the chat panel scrolled fully to the top — the history is server-side lazy-loaded, so
/// an under-scrolled save silently lacks the older turns; the per-notebook turn count this
/// parser reports exists to make that visible).
///
/// The saved DOM is Angular output with stable class markers, which is why this is a
/// dependency-free scanner rather than an HTML-parser package: each exchange is a
/// "chat-message-pair" containing a "from-user-…" (the author's prompt) and a "to-user-…"
/// (the model response); studio notes are &lt;artifact-library-note&gt; preview cards.
///
/// Italics are load-bearing in this corpus (the author's craft analysis leans on them), so
/// &lt;i&gt;/&lt;em&gt; convert to *…* and &lt;b&gt;/&lt;strong&gt; to **…** rather than being
/// stripped. Input must be read as UTF-8 — the 2026-08-13 extraction's mojibake ("â€™") came
/// from a CP1252 read, not from the capture itself.
/// </summary>
public static partial class NlmCaptureParser
{
    private const string PairMarker = "class=\"chat-message-pair";
    private const string FromUserMarker = "from-user-message-inner-content";
    private const string ToUserMarker = "to-user-container";

    [GeneratedRegex(@"<title>(.*?)</title>", RegexOptions.Singleline)]
    private static partial Regex TitleTag();

    [GeneratedRegex(@"<artifact-library-note\b.*?</artifact-library-note>", RegexOptions.Singleline)]
    private static partial Regex NoteElement();

    [GeneratedRegex(@"\b(\d+\s*(?:mo|[smhdwy]))\s+ago\b")]
    private static partial Regex RelativeAge();

    // Containers whose text is UI chrome, never content — removed wholesale before tag-stripping.
    [GeneratedRegex(@"<(style|script|svg|mat-icon|button|chat-actions)\b.*?</\1>", RegexOptions.Singleline)]
    private static partial Regex ChromeElement();

    [GeneratedRegex(@"<(/?)i(?=[\s>/])[^>]*>|<(/?)em(?=[\s>/])[^>]*>")]
    private static partial Regex ItalicTag();

    [GeneratedRegex(@"<(/?)b(?=[\s>/])[^>]*>|<(/?)strong(?=[\s>/])[^>]*>")]
    private static partial Regex BoldTag();

    [GeneratedRegex(@"<li(?=[\s>/])[^>]*>")]
    private static partial Regex ListItemOpen();

    [GeneratedRegex(@"<br[^>]*>|</(p|div|li|tr|ul|ol|h[1-6]|paragraph-element-view|labs-tailwind-structural-element-view-v2|table)>")]
    private static partial Regex BlockBoundary();

    [GeneratedRegex(@"<[^>]+>")]
    private static partial Regex AnyTag();

    public static NlmCapture Parse(string html)
    {
        var title = "";
        var titleMatch = TitleTag().Match(html);
        if (titleMatch.Success)
        {
            title = WebUtility.HtmlDecode(titleMatch.Groups[1].Value).Trim();
            foreach (var suffix in new[] { " - Gemini Notebook", " - NotebookLM" })
                if (title.EndsWith(suffix, StringComparison.Ordinal))
                    title = title[..^suffix.Length].Trim();
        }

        return new NlmCapture(title, ParseTurns(html), ParseNotes(html));
    }

    private static List<NlmTurn> ParseTurns(string html)
    {
        var turns = new List<NlmTurn>();

        // Segment on the pair marker; each segment runs to the start of the next pair.
        var starts = new List<int>();
        for (var i = html.IndexOf(PairMarker, StringComparison.Ordinal);
             i >= 0;
             i = html.IndexOf(PairMarker, i + 1, StringComparison.Ordinal))
            starts.Add(i);

        for (var p = 0; p < starts.Count; p++)
        {
            var segStart = starts[p];
            var segEnd = p + 1 < starts.Count ? starts[p + 1] : html.Length;
            var segment = html[segStart..segEnd];

            var fromIdx = segment.IndexOf(FromUserMarker, StringComparison.Ordinal);
            var toIdx = segment.IndexOf(ToUserMarker, StringComparison.Ordinal);

            if (fromIdx >= 0)
            {
                var end = toIdx > fromIdx ? toIdx : segment.Length;
                var body = HtmlToText(segment[fromIdx..end]);
                if (body.Length > 0)
                    turns.Add(new NlmTurn(turns.Count + 1, "user", body));
            }

            if (toIdx >= 0)
            {
                var body = HtmlToText(segment[toIdx..]);
                if (body.Length > 0)
                    turns.Add(new NlmTurn(turns.Count + 1, "model", body));
            }
        }

        return turns;
    }

    private static List<NlmNote> ParseNotes(string html)
    {
        var notes = new List<NlmNote>();
        foreach (Match m in NoteElement().Matches(html))
        {
            var text = HtmlToText(m.Value);
            var age = "";
            var ageMatch = RelativeAge().Match(text);
            if (ageMatch.Success)
            {
                age = ageMatch.Groups[1].Value; // "179d" — rendered as "(179d before capture)"
                text = (text[..ageMatch.Index] + text[(ageMatch.Index + ageMatch.Length)..]).Trim();
            }
            // Preview cards carry the title only; anything beyond it would be a future
            // full-note capture's body. Today that remainder is empty.
            var title = SplitTitle(text, out var body);
            notes.Add(new NlmNote(notes.Count + 1, title, age, body));
        }
        return notes;

        static string SplitTitle(string text, out string body)
        {
            var firstBreak = text.IndexOf('\n');
            if (firstBreak < 0) { body = ""; return text.Trim(); }
            body = text[(firstBreak + 1)..].Trim();
            return text[..firstBreak].Trim();
        }
    }

    /// <summary>
    /// Mechanical DOM-text extraction: chrome containers removed wholesale, italics/bold kept
    /// as markdown markers, block boundaries become newlines, everything else stripped.
    /// </summary>
    public static string HtmlToText(string html)
    {
        var s = ChromeElement().Replace(html, " ");
        s = ItalicTag().Replace(s, "*");
        s = BoldTag().Replace(s, "**");
        s = ListItemOpen().Replace(s, "\n- ");
        s = BlockBoundary().Replace(s, "\n");
        s = AnyTag().Replace(s, " ");
        s = WebUtility.HtmlDecode(s);

        var sb = new StringBuilder(s.Length);
        var emptyRun = 0;
        foreach (var rawLine in s.Split('\n'))
        {
            var line = Regex.Replace(rawLine, @"[ \t\u00A0]+", " ").Trim();
            if (line.Length == 0)
            {
                emptyRun++;
                continue;
            }
            if (sb.Length > 0)
                sb.Append(emptyRun > 0 ? "\n\n" : "\n");
            emptyRun = 0;
            sb.Append(line);
        }
        return sb.ToString().Trim();
    }
}
