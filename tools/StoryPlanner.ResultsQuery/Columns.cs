using StoryPlanner.BatchFiles;

namespace StoryPlanner.ResultsQuery;

/// <summary>
/// One column of a bar-part field: its 1-based position, the label the directions gave the
/// part (the text up to its first comma), and the short id a query string uses for it. The
/// short id is the label's first word that is not a function word, or <c>c&lt;k&gt;</c> when
/// there is none or another column of the same field has the same one; a query also accepts
/// the position and the full label.
/// </summary>
public sealed record Column(int Position, string Label, string Id);

/// <summary>
/// A <c>list of line</c> field whose What to produce text declares parts separated by a bar:
/// the field's key and its columns in order. The directions convention this tool reads
/// (exploration-of-technique-mechanism-goal-co-occurrence, directions-2): the text after the
/// first colon is the parts, ` | ` between them.
/// </summary>
public sealed record PartField(string Key, IReadOnlyList<Column> Columns)
{
    public const string Separator = " | ";
    /// <summary>A result line's parts are separated by a bar with any spacing around it: readers write <c>a | b</c> and <c>a|b</c> alike, and the directions say only "a bar".</summary>
    static readonly System.Text.RegularExpressions.Regex Bar = new(@"\s*\|\s*", System.Text.RegularExpressions.RegexOptions.Compiled);
    static readonly string[] FunctionWords = ["what", "the", "a", "an", "it", "is", "of", "to", "in", "as", "and", "its"];

    /// <summary>The bar-part fields a directions file declares, in declared order; a list of line field whose text has no bar is not one.</summary>
    public static IReadOnlyList<PartField> Of(DirectionsFile directions)
        => directions.Output.Where(f => f.Kind == OutputKind.ListOfLine && f.Text.Contains(Separator, StringComparison.Ordinal))
            .Select(f => new PartField(f.Key, ColumnsOf(f.Text))).ToList();

    /// <summary>A result line split into its parts on the bar, each trimmed.</summary>
    public static IReadOnlyList<string> SplitParts(string line) => Bar.Split(line.Trim()).Select(p => p.Trim()).ToList();

    /// <summary>The columns a field's text declares.</summary>
    public static IReadOnlyList<Column> ColumnsOf(string fieldText)
    {
        var colon = fieldText.IndexOf(':');
        var parts = (colon < 0 ? fieldText : fieldText[(colon + 1)..]).Split(Separator);
        var labels = parts.Select(p =>
        {
            var t = p.Trim();
            var comma = t.IndexOf(',');
            var semi = t.IndexOf(';');
            var cut = new[] { comma, semi }.Where(x => x >= 0).DefaultIfEmpty(-1).Min();
            return (cut < 0 ? t : t[..cut]).Trim();
        }).ToList();
        var ids = labels.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.ToLowerInvariant().Trim('\'', '"', '(', ')'))
            .FirstOrDefault(w => !FunctionWords.Contains(w) && w.All(char.IsLetterOrDigit))).ToList();
        var columns = new List<Column>();
        for (var i = 0; i < labels.Count; i++)
        {
            var id = ids[i];
            if (id is null || ids.Count(x => x == id) > 1) id = $"c{i + 1}";
            columns.Add(new Column(i + 1, labels[i], id));
        }
        return columns;
    }

    /// <summary>The column a query names: by short id, by position, or by full label; null when none matches.</summary>
    public Column? Find(string name)
    {
        var n = name.Trim();
        if (int.TryParse(n, out var k)) return Columns.FirstOrDefault(c => c.Position == k);
        return Columns.FirstOrDefault(c => string.Equals(c.Id, n, StringComparison.OrdinalIgnoreCase))
            ?? Columns.FirstOrDefault(c => string.Equals(c.Label, n, StringComparison.OrdinalIgnoreCase));
    }
}
