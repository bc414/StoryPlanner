using System.Text;

namespace StoryPlanner.AgentRunner;

/// <summary>
/// One unit of a Markdown document, as the supersession audit's item. The unit rule is
/// mechanical and lives here — not in any agent's protocol — because enumerating the items
/// is instrument work done once at design time, never judgment done at runtime by the
/// party whose judgment the audit exists to check.
/// </summary>
public sealed record DocUnit(int Number, string Section, string Text)
{
    public string Id => $"unit-{Number:000}";
    /// <summary>The first line of the unit, trimmed of list markers, for the manifest.</summary>
    public string FirstLine
    {
        get
        {
            var line = Text.Split('\n')[0].Trim();
            var m = System.Text.RegularExpressions.Regex.Match(line, @"^([-*]|\d+\.)\s+(.*)$");
            return m.Success ? m.Groups[2].Value : line;
        }
    }
}

public static class UnitSplitter
{
    /// <summary>
    /// The unit rule: a paragraph is a run of non-blank lines ended by a blank line or a
    /// heading; every list item (including nested) is its own unit; a fenced code block
    /// belongs to the unit before it; a table is one unit per body row (header and
    /// separator rows excluded); headings are not units; the leading frontmatter block is
    /// one unit. Units are numbered from 1 in document order.
    /// </summary>
    public static IReadOnlyList<DocUnit> Split(string markdown)
    {
        var lines = markdown.Replace("\r\n", "\n").Split('\n');
        var units = new List<DocUnit>();
        var section = "(start)";
        var current = new StringBuilder();
        var currentSection = section;
        var i = 0;

        void Flush()
        {
            if (current.Length == 0) return;
            units.Add(new DocUnit(units.Count + 1, currentSection, current.ToString().TrimEnd('\n')));
            current.Clear();
        }

        void Begin(string text)
        {
            Flush();
            currentSection = section;
            current.Append(text).Append('\n');
        }

        // Frontmatter: a leading "---" block is one unit.
        if (lines.Length > 0 && lines[0].Trim() == "---")
        {
            var fm = new StringBuilder(lines[0]).Append('\n');
            i = 1;
            while (i < lines.Length && lines[i].Trim() != "---") { fm.Append(lines[i]).Append('\n'); i++; }
            if (i < lines.Length) { fm.Append(lines[i]).Append('\n'); i++; }
            currentSection = "(frontmatter)";
            current.Append(fm);
            Flush();
        }

        var inTable = false;
        var tableRowsSeen = 0;
        for (; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmed = line.Trim();

            if (trimmed.StartsWith("```"))
            {
                // A fence opens: attach the whole block to the unit in progress — or, when a
                // blank line already closed it, re-open the unit just emitted; the block belongs
                // to the unit before it regardless of the blank line in between.
                if (current.Length == 0 && units.Count > 0)
                {
                    var last = units[^1];
                    units.RemoveAt(units.Count - 1);
                    currentSection = last.Section;
                    current.Append(last.Text).Append('\n').Append('\n');
                }
                if (current.Length == 0) Begin(line); else current.Append(line).Append('\n');
                i++;
                while (i < lines.Length)
                {
                    current.Append(lines[i]).Append('\n');
                    if (lines[i].Trim().StartsWith("```")) break;
                    i++;
                }
                continue;
            }

            if (trimmed.Length == 0)
            {
                Flush();
                inTable = false;
                tableRowsSeen = 0;
                continue;
            }

            if (trimmed.StartsWith('#'))
            {
                Flush();
                section = trimmed;
                inTable = false;
                tableRowsSeen = 0;
                continue;
            }

            if (trimmed.StartsWith('|'))
            {
                // Table: header row and separator row are structure; every body row is a unit.
                if (!inTable) { Flush(); inTable = true; tableRowsSeen = 0; }
                tableRowsSeen++;
                if (tableRowsSeen <= 2)
                {
                    var isSeparator = tableRowsSeen == 2 && trimmed.Replace("|", "").Replace("-", "").Replace(":", "").Trim().Length == 0;
                    if (tableRowsSeen == 1 || isSeparator) continue;
                }
                Begin(line);
                Flush();
                continue;
            }
            inTable = false;

            if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^\s*([-*]|\d+\.)\s+"))
            {
                Begin(line);
                continue;
            }

            // Continuation or paragraph start.
            if (current.Length == 0) Begin(line); else current.Append(line).Append('\n');
        }
        Flush();
        return units;
    }

    /// <summary>The item file the agent receives for one unit: locus, id, then the text verbatim.</summary>
    public static string RenderItem(DocUnit u) =>
        $"Unit: {u.Id}\nSection: {u.Section}\n\n{u.Text}\n";

    /// <summary>A manifest table, one row per unit, for the human and for job generation.</summary>
    public static string RenderManifest(string sourceName, string sourceSha, IReadOnlyList<DocUnit> units)
    {
        var sb = new StringBuilder();
        sb.Append("# Units of ").Append(sourceName).Append("\n\n");
        sb.Append("Source sha256 ").Append(sourceSha).Append("; ").Append(units.Count).Append(" units.\n\n");
        sb.Append("| Unit | Section | First line |\n|---|---|---|\n");
        foreach (var u in units)
            sb.Append("| ").Append(u.Id).Append(" | ").Append(u.Section.Replace("|", "\\|")).Append(" | ")
              .Append(Truncate(u.FirstLine.Replace("|", "\\|"), 90)).Append(" |\n");
        return sb.ToString();
    }

    private static string Truncate(string s, int max) => s.Length <= max ? s : s[..max] + "…";
}
