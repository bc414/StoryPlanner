using System.Net;
using System.Text.RegularExpressions;

namespace StoryPlanner.SourceTexts;

/// <summary>
/// Fimfiction EPUB chapter XHTML -> Markdown.
///
/// Deliberately NOT StoryPlanner.Core's shared HtmlToMarkdown: that one serves the Conversation
/// Reader's Claude/Gemini exports, handles no &lt;span style&gt; and strips no unknown tags, and
/// changing it to suit fiction markup would risk the reader it already serves. Fimfiction's
/// generator emits a small, closed tag set (measured across all 157 chapters of both fics:
/// p, i, b, u, br, span, hr, div, blockquote, h1, h2, sup, a, img) and encodes emphasis as
/// inline styles rather than semantic tags, so the span styles are mapped explicitly.
///
/// Italics are the load-bearing case, not a nicety: this author sets internal monologue in
/// italics throughout ("&lt;i&gt;The Queen's Island&lt;/i&gt;, she corrected in her mind"), and the
/// plain-text download collapses it into narration.
/// </summary>
public static class FicHtml
{
    /// <summary>
    /// A section marker inside a chapter: a horizontal rule, a centred bold-italic line, another
    /// rule. Verified against both fics — it appears in exactly three chapters, and it is what
    /// distinguishes a real section head from an inline bold-italic line such as the signature on
    /// a letter ("Flurry Heart"), which a text-shaped heuristic wrongly promotes to a heading.
    /// </summary>
    private static readonly Regex SectionMarker = new(
        """<hr\s*/?>\s*<div[^>]*class="bbcode-center"[^>]*>\s*<p[^>]*>\s*<span style="[^"]*font-weight:bold[^"]*font-style:italic[^"]*">(.*?)</span>\s*</p>\s*</div>\s*<hr\s*/?>""",
        RegexOptions.Singleline | RegexOptions.IgnoreCase);

    private static readonly Regex H1 = new(@"<h1[^>]*>(.*?)</h1>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    private static readonly Regex BodyTag = new(@"<body[^>]*>(.*?)</body>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

    public sealed record Section(string Title, string Markdown);

    /// <summary>The chapter's &lt;h1&gt;, decoded. Empty when absent.</summary>
    public static string ChapterTitle(string xhtml)
    {
        var m = H1.Match(xhtml);
        return m.Success ? WebUtility.HtmlDecode(StripTags(m.Groups[1].Value)).Trim() : "";
    }

    /// <summary>
    /// Splits a chapter into its marked sections. Returns an empty list when the chapter carries
    /// no section markers — the overwhelmingly common case, and the caller's signal to treat the
    /// chapter as one unit rather than to go looking for a softer delimiter.
    /// </summary>
    public static IReadOnlyList<Section> SplitSections(string xhtml)
    {
        var body = ExtractBody(xhtml);
        var matches = SectionMarker.Matches(body);
        if (matches.Count == 0) return [];

        var sections = new List<Section>();
        for (var i = 0; i < matches.Count; i++)
        {
            var start = matches[i].Index + matches[i].Length;
            var end = i + 1 < matches.Count ? matches[i + 1].Index : body.Length;
            var title = WebUtility.HtmlDecode(StripTags(matches[i].Groups[1].Value)).Trim();
            sections.Add(new Section(title, ToMarkdown(body[start..end])));
        }
        return sections;
    }

    /// <summary>The whole chapter as Markdown, &lt;h1&gt; removed (the title is stored separately).</summary>
    public static string ChapterMarkdown(string xhtml) => ToMarkdown(H1.Replace(ExtractBody(xhtml), ""));

    private static string ExtractBody(string xhtml)
    {
        var m = BodyTag.Match(xhtml);
        return m.Success ? m.Groups[1].Value : xhtml;
    }

    public static string ToMarkdown(string html)
    {
        if (string.IsNullOrEmpty(html)) return "";
        var s = html;

        // Inline styles first: Fimfiction encodes emphasis as <span style>, so these must become
        // markers before the generic tag strip below removes the spans and their styling with them.
        s = Regex.Replace(s, """<span style="([^"]*)">(.*?)</span>""", m =>
        {
            var style = m.Groups[1].Value;
            var inner = m.Groups[2].Value;
            var bold = style.Contains("font-weight:bold", StringComparison.OrdinalIgnoreCase);
            var italic = style.Contains("font-style:italic", StringComparison.OrdinalIgnoreCase);
            var underline = style.Contains("text-decoration:underline", StringComparison.OrdinalIgnoreCase);
            var strike = style.Contains("line-through", StringComparison.OrdinalIgnoreCase);
            return Wrap(inner, bold, italic, underline, strike);
        }, RegexOptions.Singleline | RegexOptions.IgnoreCase);

        s = Regex.Replace(s, @"(?s)<h2[^>]*>(.*?)</h2>", "\n## $1\n", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"(?s)<blockquote[^>]*>(.*?)</blockquote>",
            m => "\n> " + Regex.Replace(m.Groups[1].Value.Trim(), @"\n", "\n> ") + "\n", RegexOptions.IgnoreCase);

        s = Regex.Replace(s, @"(?s)<b[^>]*>(.*?)</b>", "**$1**", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"(?s)<strong[^>]*>(.*?)</strong>", "**$1**", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"(?s)<i[^>]*>(.*?)</i>", "*$1*", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"(?s)<em[^>]*>(.*?)</em>", "*$1*", RegexOptions.IgnoreCase);
        // Markdown has no underline; _..._ keeps it distinct from * (italic) and ** (bold)
        // rather than silently flattening a third emphasis into one of the other two.
        s = Regex.Replace(s, @"(?s)<u[^>]*>(.*?)</u>", "_$1_", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"(?s)<sup[^>]*>(.*?)</sup>", "^$1^", RegexOptions.IgnoreCase);

        s = Regex.Replace(s, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"<hr\s*/?>", "\n\n---\n\n", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"(?s)<p[^>]*>(.*?)</p>", "$1\n\n", RegexOptions.IgnoreCase);

        s = StripTags(s);
        s = WebUtility.HtmlDecode(s);

        // Collapse the blank-line runs the replacements above leave behind, and normalise the
        // trailing whitespace EPUB generators sprinkle at line ends.
        s = s.Replace("\r\n", "\n").Replace('\r', '\n');
        s = Regex.Replace(s, @"[ \t]+\n", "\n");
        s = Regex.Replace(s, @"\n{3,}", "\n\n");
        return s.Trim();
    }

    private static string Wrap(string inner, bool bold, bool italic, bool underline, bool strike)
    {
        if (underline) inner = $"_{inner}_";
        if (strike) inner = $"~~{inner}~~";
        if (bold && italic) return $"***{inner}***";
        if (bold) return $"**{inner}**";
        if (italic) return $"*{inner}*";
        return inner;
    }

    private static string StripTags(string s) => Regex.Replace(s, "<[^>]+>", "");
}
