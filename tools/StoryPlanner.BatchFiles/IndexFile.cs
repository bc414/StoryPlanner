using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.BatchFiles;

public sealed record IndexRow(string Item, string Locator, string Description, int Line);

public sealed record IndexProblem(string Part, string Message, int Line);

/// <summary>
/// A batch's index (index-schema): the title, a head of keyed lines, and one table row per
/// item in the order the itemizer produced them. Read by the runner for the items to call and
/// their order, and written by every itemizer through <see cref="Render"/>.
/// </summary>
public sealed class IndexFile
{
    public static readonly string[] HeadKeys = ["itemizer", "corpus", "locator notation", "source hash"];
    public static readonly string[] Columns = ["item", "locator", "description"];
    static readonly Regex Slug = new(@"^[a-z0-9-]+$", RegexOptions.Compiled);
    static readonly Regex Sha = new(@"^[0-9a-f]{64}$", RegexOptions.Compiled);

    public string? Title { get; }
    public KeyedBlock Head { get; }
    public IReadOnlyList<IndexRow> Rows { get; }
    public IReadOnlyList<IndexProblem> Problems { get; }

    public string? Itemizer => Head.Value("itemizer");
    public string? Corpus => Head.Value("corpus");
    public string? LocatorNotation => Head.Value("locator notation");
    public string? SourceHash => Head.Value("source hash");

    IndexFile(string? title, KeyedBlock head, IReadOnlyList<IndexRow> rows, IReadOnlyList<IndexProblem> problems)
    { Title = title; Head = head; Rows = rows; Problems = problems; }

    public static IndexFile Read(string path) => Parse(File.ReadAllText(path));

    public static IndexFile Parse(string text)
    {
        var lines = Hashing.NormalizeNewlines(text).Split('\n');
        var problems = new List<IndexProblem>();
        string? title = null;
        var i = 0;
        while (i < lines.Length && lines[i].Trim().Length == 0) i++;
        if (i < lines.Length && lines[i].StartsWith("# ", StringComparison.Ordinal)) { title = lines[i][2..].Trim(); i++; }

        var headLines = new List<string>();
        var headStart = i;
        while (i < lines.Length && !lines[i].TrimStart().StartsWith('|')) { headLines.Add(lines[i]); i++; }
        var head = KeyedLines.Read(headLines, headStart + 1);
        foreach (var s in head.Stray) problems.Add(new IndexProblem("head", $"line {s.Line}: a line outside the head's keyed lines and the table", s.Line));
        var keys = head.Fields.Select(f => f.Key).ToList();
        foreach (var k in keys.Where(k => !HeadKeys.Contains(k))) problems.Add(new IndexProblem("head", $"head key '{k}' is unknown", head.Field(k)!.Line));
        foreach (var k in HeadKeys.Take(3).Where(k => !keys.Contains(k))) problems.Add(new IndexProblem("head", $"head key '{k}' is missing", headStart + 1));
        var order = keys.Where(HeadKeys.Contains).Select(k => Array.IndexOf(HeadKeys, k)).ToList();
        if (order.Zip(order.Skip(1)).Any(p => p.Second <= p.First)) problems.Add(new IndexProblem("head", "the head keys are in the order " + string.Join(", ", HeadKeys), headStart + 1));
        foreach (var f in head.Fields.Where(f => f.Value.Length == 0)) problems.Add(new IndexProblem("head", $"line {f.Line}: '{f.Key}' has no value", f.Line));
        if (head.Value("source hash") is { } sh && !Sha.IsMatch(sh)) problems.Add(new IndexProblem("head", "source hash is not a SHA-256", head.Field("source hash")!.Line));

        var rows = new List<IndexRow>();
        if (i >= lines.Length) problems.Add(new IndexProblem("table", "no table", lines.Length));
        else
        {
            var header = SplitCells(lines[i]);
            if (!header.Select(h => h.ToLowerInvariant()).SequenceEqual(Columns))
                problems.Add(new IndexProblem("table", $"line {i + 1}: the columns are item | locator | description; found [{string.Join(" | ", header)}]", i + 1));
            i++;
            if (i >= lines.Length || !IsSeparator(lines[i])) problems.Add(new IndexProblem("table", $"line {i + 1}: the header is followed by a separator row", i + 1));
            else i++;
            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (; i < lines.Length; i++)
            {
                var l = lines[i];
                if (l.Trim().Length == 0) continue;
                if (!l.TrimStart().StartsWith('|')) { problems.Add(new IndexProblem("table", $"line {i + 1}: a line outside the head and the table", i + 1)); continue; }
                var cells = SplitCells(l);
                if (cells.Count != Columns.Length) { problems.Add(new IndexProblem("table", $"line {i + 1}: {cells.Count} cells, the header has {Columns.Length}", i + 1)); continue; }
                var row = new IndexRow(cells[0], cells[1], cells[2], i + 1);
                if (!Slug.IsMatch(row.Item)) problems.Add(new IndexProblem("item", $"line {i + 1}: item '{row.Item}' is not a lowercase slug", i + 1));
                else if (!seen.Add(row.Item)) problems.Add(new IndexProblem("item", $"line {i + 1}: item '{row.Item}' repeats", i + 1));
                if (row.Locator.Length == 0) problems.Add(new IndexProblem("locator", $"line {i + 1}: the locator is empty", i + 1));
                rows.Add(row);
            }
            if (rows.Count == 0 && !problems.Any(p => p.Part == "table")) problems.Add(new IndexProblem("table", "the table is empty", i));
        }
        return new IndexFile(title, head, rows, problems);
    }

    static bool IsSeparator(string line)
    {
        var t = line.Trim();
        return t.StartsWith('|') && t.All(c => c is '|' or '-' or ':' or ' ') && t.Contains('-');
    }

    public static IReadOnlyList<string> SplitCells(string line)
    {
        var t = line.Trim();
        if (t.StartsWith('|')) t = t[1..];
        if (t.EndsWith('|')) t = t[..^1];
        var cells = new List<string>();
        var sb = new StringBuilder();
        for (var i = 0; i < t.Length; i++)
        {
            if (t[i] == '\\' && i + 1 < t.Length && t[i + 1] == '|') { sb.Append('|'); i++; continue; }
            if (t[i] == '|') { cells.Add(sb.ToString().Trim()); sb.Clear(); continue; }
            sb.Append(t[i]);
        }
        cells.Add(sb.ToString().Trim());
        return cells;
    }

    static string Cell(string s) => s.Replace("\r", "").Replace("\n", " ").Replace("|", "\\|");

    /// <summary>The index an itemizer writes: title, head, table; <paramref name="sourceHash"/> only when the corpus is one document.</summary>
    public static string Render(string batch, string itemizer, string corpus, string locatorNotation, string? sourceHash, IEnumerable<(string Item, string Locator, string Description)> rows)
    {
        var sb = new StringBuilder();
        sb.Append("# ").Append(batch).Append(" — index\n\n");
        sb.Append(KeyedLines.RenderLine("itemizer", itemizer)).Append('\n');
        sb.Append(KeyedLines.RenderLine("corpus", corpus)).Append('\n');
        sb.Append(KeyedLines.RenderLine("locator notation", locatorNotation)).Append('\n');
        if (sourceHash is not null) sb.Append(KeyedLines.RenderLine("source hash", sourceHash)).Append('\n');
        sb.Append("\n| item | locator | description |\n|---|---|---|\n");
        foreach (var r in rows) sb.Append("| ").Append(Cell(r.Item)).Append(" | ").Append(Cell(r.Locator)).Append(" | ").Append(Cell(r.Description)).Append(" |\n");
        return sb.ToString();
    }
}
