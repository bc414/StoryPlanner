using System.Text;

namespace StoryPlanner.ResultsQuery;

/// <summary>
/// The one normalization the term views apply, fixed and printed under health so any count
/// can be reproduced by hand: lowercase; a possessive <c>'s</c> dropped; every character that
/// is not a letter or digit becomes a space; the words split on spaces; the stopwords dropped.
/// Nothing is stemmed and nothing is merged.
/// </summary>
public static class Terms
{
    public static readonly string[] Stopwords =
        ["a", "an", "the", "of", "in", "to", "as", "and", "or", "with", "for", "by", "on", "at", "that", "is", "into", "from", "its", "it", "their", "her", "his", "s"];

    public static IReadOnlyList<string> Tokens(string value)
    {
        var sb = new StringBuilder(value.Length);
        var lower = value.ToLowerInvariant().Replace("'s", "").Replace("’s", "");
        foreach (var ch in lower) sb.Append(char.IsLetterOrDigit(ch) ? ch : ' ');
        return sb.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(w => !Stopwords.Contains(w)).ToList();
    }

    /// <summary>The n-grams of a value: its tokens for n=1, adjacent pairs joined by a space for n=2.</summary>
    public static IEnumerable<string> Grams(string value, int n)
    {
        var t = Tokens(value);
        if (n <= 1) return t;
        return Enumerable.Range(0, Math.Max(0, t.Count - n + 1)).Select(i => string.Join(' ', t.Skip(i).Take(n)));
    }

    /// <summary>The token at a 1-based position, or null when the value has fewer.</summary>
    public static string? At(string value, int position)
    {
        var t = Tokens(value);
        return position >= 1 && position <= t.Count ? t[position - 1] : null;
    }

    /// <summary>Whether a value is the none token, exactly: <c>none</c> after normalization and nothing else.</summary>
    public static bool IsNone(string value) => Tokens(value) is ["none"];

    /// <summary>Whether a value opens with none and goes on: <c>none, purely tonal</c>.</summary>
    public static bool IsHedgedNone(string value) => Tokens(value) is ["none", _, ..];
}
