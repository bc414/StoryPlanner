using System;
using System.Text.RegularExpressions;

namespace StoryPlanner.Core;

/// <summary>
/// Extracts a context window around a match for display. Sibling of the MCP server's
/// Query.Snippet (tools/StoryPlanner.Mcp/Query.cs) — deliberately not shared with it: that
/// class lives in a different project the app has no reference to, and refactoring to share
/// it would force a republish + /mcp reconnect in every live session for no functional gain.
/// </summary>
public static class TextSnippet
{
    public static string Around(string text, int matchIndex, int matchLength, int contextChars = 160)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        int start = Math.Max(0, matchIndex - contextChars / 2);
        int len = Math.Min(text.Length - start, matchLength + contextChars);
        var s = text.Substring(start, len);
        s = Regex.Replace(s, @"\s+", " ").Trim();

        var prefix = start > 0 ? "…" : "";
        var suffix = start + len < text.Length ? "…" : "";
        return $"{prefix}{s}{suffix}";
    }
}
