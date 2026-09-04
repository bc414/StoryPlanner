using System.Text.RegularExpressions;

namespace StoryPlanner.ProcessMap;

public sealed record ScannedDiagrams(
    IReadOnlyList<string> Nodes,
    IReadOnlyList<string> Edges,
    IReadOnlyDictionary<string, string> Normalisation);

/// <summary>
/// Reads node ids and edge pairs out of every mermaid block in a markdown file, for the set
/// comparison against draft 1 (<c>git show 32b6d4b:docs/v3-framework/process-map-1-draft.md</c>).
///
/// Ids are normalised — dots, hyphens and underscores dropped, lower-cased — because draft 1
/// wrote <c>P1</c> and <c>fCand</c> by hand for what the rows call <c>P.1</c> and <c>f.cand</c>.
/// The raw → normalised mapping is reported alongside so a normalisation collision is visible
/// rather than silently merging two nodes.
/// </summary>
public static class MermaidScanner
{
    static readonly Regex NodeDecl = new(
        @"^\s*([A-Za-z][A-Za-z0-9_.-]*)\s*(\[|\(|\{|>)", RegexOptions.Compiled);

    /// <summary>
    /// A whole connector, never a hyphen or dot inside an id: an arrow must carry '&gt;', or be
    /// the bare <c>---</c> link.
    /// </summary>
    static readonly Regex Connector = new(
        @"\s*(?:-\.->|\.->|={2,3}>|-{2,3}>|-{3})\s*", RegexOptions.Compiled);

    static readonly Regex Identifier = new(
        @"(?<![A-Za-z0-9_.-])([A-Za-z][A-Za-z0-9_.-]*)", RegexOptions.Compiled);

    public static ScannedDiagrams Scan(string markdown)
    {
        var nodes = new SortedSet<string>(StringComparer.Ordinal);
        var edges = new SortedSet<string>(StringComparer.Ordinal);
        var mapping = new SortedDictionary<string, string>(StringComparer.Ordinal);

        string Note(string raw)
        {
            var n = Normalise(raw);
            mapping[raw] = n;
            return n;
        }

        foreach (var block in Blocks(markdown))
        foreach (var rawLine in block)
        {
            var line = rawLine.Trim();
            if (line.Length == 0) continue;
            if (line.StartsWith("flowchart", StringComparison.Ordinal)) continue;
            if (line.StartsWith("classDef", StringComparison.Ordinal)) continue;
            if (line.StartsWith("class ", StringComparison.Ordinal)) continue;
            if (line.StartsWith("subgraph", StringComparison.Ordinal)) continue;
            if (line.StartsWith("%%", StringComparison.Ordinal)) continue;

            var decl = NodeDecl.Match(line);
            if (decl.Success) nodes.Add(Note(decl.Groups[1].Value));

            // Split on connectors; each adjacent pair of segments is an edge. A segment may hold
            // an '&' list (mermaid's "E & V & S & I --> P6"), which is one edge per member.
            var segments = Connector.Split(StripLabels(line));
            if (segments.Length < 2) continue;
            for (var i = 0; i + 1 < segments.Length; i++)
            {
                var from = IdsIn(segments[i]);
                var to = IdsIn(segments[i + 1]);
                foreach (var a in from)
                foreach (var b in to)
                {
                    var na = Note(a);
                    var nb = Note(b);
                    nodes.Add(na);
                    nodes.Add(nb);
                    edges.Add($"{na} -> {nb}");
                }
            }
        }

        return new ScannedDiagrams(nodes.ToList(), edges.ToList(), mapping);
    }

    /// <summary>Bare identifiers left in a segment once labels are stripped.</summary>
    static IReadOnlyList<string> IdsIn(string segment)
        => Identifier.Matches(segment).Select(m => m.Groups[1].Value).ToList();

    /// <summary>
    /// Removes bracketed node text so an edge label or a node caption cannot be read as an id.
    /// </summary>
    static string StripLabels(string line)
    {
        line = Regex.Replace(line, "\"[^\"]*\"", "\"\"");
        line = Regex.Replace(line, @"\[[^\]]*\]", "[]");
        line = Regex.Replace(line, @"\{[^}]*\}", "{}");
        line = Regex.Replace(line, @"\(\([^)]*\)\)", "(())");
        line = Regex.Replace(line, @"\([^)]*\)", "()");

        // An unquoted mid-arrow label ("P3 -- exploratory --> E") would otherwise read as a node.
        line = Regex.Replace(line, @"-->\|[^|]*\|", "-->");
        line = Regex.Replace(line, @"--(?!>)[^>]*?-->", "-->");
        line = Regex.Replace(line, @"-\.[^>]*?\.->", ".->");
        line = Regex.Replace(line, @"==(?!>)[^>]*?==>", "==>");
        return line;
    }

    static IEnumerable<List<string>> Blocks(string markdown)
    {
        var lines = markdown.Replace("\r\n", "\n").Split('\n');
        List<string>? current = null;
        foreach (var line in lines)
        {
            var t = line.Trim();
            if (t.StartsWith("```"))
            {
                if (current is not null) { yield return current; current = null; }
                else if (t.Contains("mermaid", StringComparison.OrdinalIgnoreCase)) current = [];
                continue;
            }
            current?.Add(line);
        }
        if (current is not null) yield return current;
    }

    public static string Normalise(string id)
        => id.Replace(".", "").Replace("-", "").Replace("_", "").ToLowerInvariant();
}
