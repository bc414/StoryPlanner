using System.Text.RegularExpressions;

namespace StoryPlanner.ProcessMap;

/// <summary>
/// A citation of a place in a document: <c>path</c>, optionally <c>§ heading</c>, optionally
/// <c>¶ n</c> naming the nth top-level ordered-list item under that heading.
///
/// Grammar is strict by ruling (2026-09-04): nothing is normalised away. A bare trailing
/// integer — <c>§ Constitutional rules 5</c> — is a syntax error, not an item pointer, because
/// silently stripping it is how a pointer to rule 5 becomes a pointer to all nine without
/// anyone deciding to widen it. The sanctioned form is <c>§ Constitutional rules ¶ 5</c>.
///
/// Used by <c>Roots.source</c>, which is a citation. <c>governed-by</c> is NOT a locus: it is a
/// reading assignment and a precedence declaration, so it is a bare path and a § in it is an
/// error (see <see cref="Validator"/>).
/// </summary>
public sealed record Locus(string Path, string? Heading, int? Item)
{
    public const char SectionSign = '§';
    public const char ItemSign = '¶';

    public static bool TryParse(string cell, out Locus? locus, out string? error)
    {
        locus = null;
        error = null;
        var text = cell.Trim();
        if (text.Length == 0) { error = "empty"; return false; }

        string path = text, rest = "";
        var section = text.IndexOf(SectionSign);
        if (section >= 0)
        {
            path = text[..section].Trim();
            rest = text[(section + 1)..].Trim();
        }

        if (!TryValidatePath(path, out error)) return false;

        string? heading = null;
        int? item = null;
        if (section >= 0)
        {
            if (rest.Length == 0) { error = $"'{SectionSign}' with no heading after it"; return false; }
            var itemAt = rest.IndexOf(ItemSign);
            if (itemAt >= 0)
            {
                heading = rest[..itemAt].Trim();
                var n = rest[(itemAt + 1)..].Trim();
                if (!int.TryParse(n, out var parsed) || parsed < 1)
                {
                    error = $"'{ItemSign} {n}' is not a positive item number";
                    return false;
                }
                item = parsed;
            }
            else heading = rest;

            if (heading.Length == 0) { error = "empty heading"; return false; }
            if (Regex.IsMatch(heading, @"\s\d+$"))
            {
                error = $"heading '{heading}' ends in a bare number. An ordered-list item is " +
                        $"addressed as '{ItemSign} n'; a trailing integer is never normalised away";
                return false;
            }
        }

        locus = new Locus(path, heading, item);
        return true;
    }

    /// <summary>
    /// Paths are repo-relative, one form only (ruling 2026-09-04). No search order means no
    /// ambiguity: a bare <c>SKILL.md</c> that could mean either of two skills is rejected here
    /// rather than resolved by a precedence nobody chose.
    /// </summary>
    public static bool TryValidatePath(string path, out string? error)
    {
        error = null;
        if (path.Length == 0) { error = "empty path"; return false; }
        if (path.Contains(' '))
        {
            error = $"'{path}' is not a path (it contains a space)";
            return false;
        }
        if (path.Contains('(') || path.Contains(')'))
        {
            error = $"'{path}' carries a parenthetical. A cell states a path or nothing";
            return false;
        }
        if (Path_IsRooted(path))
        {
            error = $"'{path}' is absolute or drive-qualified; paths are repo-relative";
            return false;
        }
        if (!path.Contains('/'))
        {
            error = $"'{path}' has no directory. Paths are repo-relative, so a bare file name " +
                    "cannot be resolved without a search order, and there is none";
            return false;
        }
        return true;
    }

    static bool Path_IsRooted(string p)
        => p.StartsWith('/') || p.StartsWith('\\') || (p.Length > 1 && p[1] == ':');

    public string Display()
    {
        var s = Path;
        if (Heading is not null) s += $" {SectionSign} {Heading}";
        if (Item is not null) s += $" {ItemSign} {Item}";
        return s;
    }
}

/// <summary>The headings and ordered lists of one markdown file, for resolving a locus.</summary>
public sealed class MarkdownOutline
{
    public sealed record Heading(string Text, int Level, int Line);

    readonly string[] _lines;
    public IReadOnlyList<Heading> Headings { get; }

    public MarkdownOutline(string markdown)
    {
        _lines = markdown.Replace("\r\n", "\n").Split('\n');
        var headings = new List<Heading>();
        var inFence = false;
        for (var i = 0; i < _lines.Length; i++)
        {
            var t = _lines[i].Trim();
            if (t.StartsWith("```")) { inFence = !inFence; continue; }
            if (inFence || !t.StartsWith('#')) continue;
            var level = t.TakeWhile(c => c == '#').Count();
            headings.Add(new Heading(t[level..].Trim(), level, i + 1));
        }
        Headings = headings;
    }

    /// <summary>
    /// Exactly-one-match semantics: zero matches and two matches are both failures. A heading
    /// that appears twice cannot be cited without saying which one.
    /// </summary>
    public IReadOnlyList<Heading> Find(string text)
        => Headings.Where(h => string.Equals(h.Text, text, StringComparison.Ordinal)).ToList();

    /// <summary>
    /// Top-level ordered-list items under a heading, up to the next heading of the same or a
    /// higher level. "Top-level" means indented less than four spaces; a nested list is not
    /// addressable, because its numbering restarts.
    /// </summary>
    public int CountOrderedItems(Heading heading)
    {
        var count = 0;
        var inFence = false;
        for (var i = heading.Line; i < _lines.Length; i++)
        {
            var raw = _lines[i];
            var t = raw.Trim();
            if (t.StartsWith("```")) { inFence = !inFence; continue; }
            if (inFence) continue;
            if (t.StartsWith('#'))
            {
                var level = t.TakeWhile(c => c == '#').Count();
                if (level <= heading.Level) break;
                continue;
            }
            var indent = raw.Length - raw.TrimStart(' ').Length;
            if (indent >= 4) continue;
            if (Regex.IsMatch(t, @"^\d+\.\s")) count++;
        }
        return count;
    }
}
