using System.Text;

namespace StoryPlanner.ProcessMap;

/// <summary>Raised when the markdown cannot be parsed. The rule is flag, never guess.</summary>
public sealed class MapFormatException(string message) : Exception(message);

public sealed record MarkdownTable(
    string Section,
    IReadOnlyList<string> Headers,
    IReadOnlyList<MarkdownRow> Rows,
    int HeaderLine);

public sealed record MarkdownRow(IReadOnlyList<string> Cells, int Line);

/// <summary>
/// The markdown table reader. Follows the runner's unit rule in spirit — a table's header and
/// separator rows are structure, every body row is a unit — but splits cells, which
/// <c>UnitSplitter</c> deliberately does not (see this project's README for why it is not
/// referenced).
///
/// Fenced code blocks and the contents of generated sections are skipped: a rendered diagram
/// or a rendered consumers table must never be read back as source.
/// </summary>
public static class MapTables
{
    public const string GeneratedOpenPrefix = "<!-- generated:";
    public const string GeneratedClose = "<!-- /generated -->";

    public static IReadOnlyList<MarkdownTable> ReadAll(string markdown)
    {
        var lines = markdown.Replace("\r\n", "\n").Split('\n');
        var tables = new List<MarkdownTable>();
        var section = "";
        var inFence = false;
        var inGenerated = false;

        for (var i = 0; i < lines.Length; i++)
        {
            var trimmed = lines[i].Trim();

            if (trimmed.StartsWith("```"))
            {
                inFence = !inFence;
                continue;
            }
            if (inFence) continue;

            if (trimmed.StartsWith(GeneratedOpenPrefix)) { inGenerated = true; continue; }
            if (trimmed == GeneratedClose) { inGenerated = false; continue; }
            if (inGenerated) continue;

            if (trimmed.StartsWith('#'))
            {
                section = trimmed.TrimStart('#').Trim();
                continue;
            }

            if (!trimmed.StartsWith('|')) continue;

            // A table starts here: header, separator, then body rows to the first non-pipe line.
            var headerLine = i + 1;
            var headers = SplitCells(trimmed, headerLine);
            if (i + 1 >= lines.Length || !IsSeparator(lines[i + 1]))
                throw new MapFormatException(
                    $"line {headerLine}: a table header is not followed by a separator row. " +
                    "Refusing to guess where the body begins.");

            var rows = new List<MarkdownRow>();
            var j = i + 2;
            for (; j < lines.Length; j++)
            {
                var body = lines[j].Trim();
                if (!body.StartsWith('|')) break;
                var cells = SplitCells(body, j + 1);
                if (cells.Count != headers.Count)
                    throw new MapFormatException(
                        $"line {j + 1}: row has {cells.Count} cells, the header has {headers.Count}. " +
                        "Refusing to guess which column is missing.");
                rows.Add(new MarkdownRow(cells, j + 1));
            }

            tables.Add(new MarkdownTable(section, headers, rows, headerLine));
            i = j - 1;
        }

        if (inFence) throw new MapFormatException("an unterminated fenced code block.");
        if (inGenerated) throw new MapFormatException(
            "a generated section was opened and never closed by " + GeneratedClose + ".");

        return tables;
    }

    static bool IsSeparator(string line)
    {
        var t = line.Trim();
        if (!t.StartsWith('|')) return false;
        foreach (var c in t)
            if (c is not ('|' or '-' or ':' or ' ')) return false;
        return t.Contains('-');
    }

    /// <summary>
    /// Splits a pipe row into trimmed cells. <c>\|</c> is an escaped pipe and becomes a literal
    /// <c>|</c> in the cell — the one escape the runner's manifest writer emits.
    /// </summary>
    public static IReadOnlyList<string> SplitCells(string line, int lineNumber)
    {
        var t = line.Trim();
        if (!t.StartsWith('|') || t.Length < 2 || !t.EndsWith('|'))
            throw new MapFormatException(
                $"line {lineNumber}: a table row must open and close with '|'.");

        var cells = new List<string>();
        var cell = new StringBuilder();
        for (var i = 1; i < t.Length; i++)
        {
            var c = t[i];
            if (c == '\\' && i + 1 < t.Length && t[i + 1] == '|')
            {
                cell.Append('|');
                i++;
                continue;
            }
            if (c == '|')
            {
                cells.Add(cell.ToString().Trim());
                cell.Clear();
                continue;
            }
            cell.Append(c);
        }
        if (cell.ToString().Trim().Length > 0)
            throw new MapFormatException($"line {lineNumber}: trailing text after the last '|'.");
        return cells;
    }
}
