using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.ResultsQuery;

/// <summary>What a view produced: the canonical query, a note when the batch is not the snapshot, and one table with a caption; a cites view's rows are the tokens.</summary>
public sealed record ViewResult(Query Query, string? SnapshotNote, string Caption, IReadOnlyList<string> Columns, IReadOnlyList<IReadOnlyList<string>> Rows)
{
    /// <summary>The plain text a verb prints: the canonical string, the note, the caption, the table.</summary>
    public string Render()
    {
        var sb = new StringBuilder();
        sb.Append(Query.Print()).Append('\n');
        if (SnapshotNote is not null) sb.Append("note: ").Append(SnapshotNote).Append('\n');
        sb.Append(Caption).Append('\n');
        if (Columns.Count > 0) sb.Append(string.Join(" | ", Columns)).Append('\n');
        foreach (var r in Rows) sb.Append(string.Join(" | ", r)).Append('\n');
        return sb.ToString();
    }
}

/// <summary>
/// The views: a query resolved against a batch, the field's lines filtered, scoped and sampled
/// the same way for every view, then one table. Counts count lines; nothing here merges two
/// values or ranks anything beyond a count.
/// </summary>
public static class Views
{
    public static (ViewResult? Result, string? Error) Run(LoadedBatch batch, Query query)
    {
        var (q, error) = query.Resolve(batch);
        if (q is null) return (null, error);
        var field = batch.Field(q.Field)!;
        var note = q.Answered == batch.Answered ? null
            : $"this query was run over {q.Answered} answered results; the batch now has {batch.Answered}";
        var lines = Select(batch, q, field);
        var caption = $"{lines.Count} line(s) of {q.Field}" + (q.Filters.Count > 0 ? " matching" : "") + (q.Story is not null ? $" in {q.Story}" : q.Item is not null ? $" in {q.Item}" : "") + (q.Sample is not null ? $", sample {q.Sample} seed {q.Seed}" : "");
        ViewResult R(string cap, IReadOnlyList<string> cols, IEnumerable<IReadOnlyList<string>> rows) => new(q, note, cap, cols, rows.ToList());
        var top = q.Top ?? 50;
        switch (q.View)
        {
            case "list":
                return (R(caption, ["item", .. field.Columns.Select(c => c.Id)], lines.Select(l => (IReadOnlyList<string>)[l.Item, .. field.Columns.Select(c => l.Part(c.Position))])), null);
            case "cites":
            {
                var items = lines.Select(l => l.Item).Distinct().ToList();
                return (R($"{items.Count} item(s) over {caption}", ["cites"], items.Select(i => (IReadOnlyList<string>)[$"{batch.BatchId}/{i}"])), null);
            }
            case "terms":
            {
                var col = field.Find(q.Col!)!;
                var n = q.N ?? 1;
                var exclude = q.Exclude is null ? null : new Regex(q.Exclude, RegexOptions.IgnoreCase);
                var counts = new Dictionary<string, int>(StringComparer.Ordinal);
                var linesWith = new Dictionary<string, int>(StringComparer.Ordinal);
                foreach (var l in lines)
                {
                    var value = l.Part(col.Position);
                    IEnumerable<string> grams = q.Position is { } p ? (Terms.At(value, p) is { } t ? [t] : []) : Terms.Grams(value, n);
                    var seen = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var g in grams)
                    {
                        if (exclude is not null && exclude.IsMatch(g)) continue;
                        counts[g] = counts.GetValueOrDefault(g) + 1;
                        if (seen.Add(g)) linesWith[g] = linesWith.GetValueOrDefault(g) + 1;
                    }
                }
                var what = q.Position is { } pp ? $"word at position {pp}" : n == 1 ? "words" : $"{n}-word runs";
                var rows = counts.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key, StringComparer.Ordinal).Take(top)
                    .Select(kv => (IReadOnlyList<string>)[kv.Key, kv.Value.ToString(CultureInfo.InvariantCulture), linesWith[kv.Key].ToString(CultureInfo.InvariantCulture)]);
                return (R($"{what} of {col.Id} over {caption}; {counts.Count} distinct, top {Math.Min(top, counts.Count)}", ["term", "count", "lines"], rows), null);
            }
            case "pairs":
            {
                var col = field.Find(q.Col!)!;
                var exclude = q.Exclude is null ? null : new Regex(q.Exclude, RegexOptions.IgnoreCase);
                var counts = new Dictionary<string, int>(StringComparer.Ordinal);
                foreach (var l in lines)
                {
                    var t = Terms.Tokens(l.Part(col.Position)).Where(w => exclude is null || !exclude.IsMatch(w)).Distinct().OrderBy(w => w, StringComparer.Ordinal).ToList();
                    for (var i = 0; i < t.Count; i++)
                        for (var j = i + 1; j < t.Count; j++)
                        {
                            var k = t[i] + "+" + t[j];
                            counts[k] = counts.GetValueOrDefault(k) + 1;
                        }
                }
                var rows = counts.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key, StringComparer.Ordinal).Take(top)
                    .Select(kv => (IReadOnlyList<string>)[kv.Key, kv.Value.ToString(CultureInfo.InvariantCulture)]);
                return (R($"pairs of words within one value of {col.Id} over {caption}; {counts.Count} distinct, top {Math.Min(top, counts.Count)}", ["pair", "lines"], rows), null);
            }
            case "by-story":
            {
                var per = lines.GroupBy(l => l.Story).ToDictionary(g => g.Key, g => (Lines: g.Count(), Items: g.Select(l => l.Item).Distinct().Count()), StringComparer.Ordinal);
                var rows = batch.Stories.Where(s => per.ContainsKey(s.Story) || q.Filters.Count == 0)
                    .Select(s => (IReadOnlyList<string>)[s.Story, per.GetValueOrDefault(s.Story).Lines.ToString(CultureInfo.InvariantCulture), per.GetValueOrDefault(s.Story).Items.ToString(CultureInfo.InvariantCulture), $"{s.Answered}/{s.Items}"]);
                return (R($"{caption}, by story; {per.Count} of {batch.Stories.Count} stories", ["story", "lines", "items with a line", "answered/items"], rows), null);
            }
            case "sort":
            {
                var col = field.Find(q.Col!)!;
                var rows = lines.OrderBy(l => l.Part(col.Position), StringComparer.OrdinalIgnoreCase).ThenBy(l => l.Item, StringComparer.Ordinal)
                    .Select(l => (IReadOnlyList<string>)[l.Part(col.Position), l.Item]);
                return (R($"{col.Id} of {caption}, sorted", [col.Id, "item"], rows), null);
            }
            case "health":
            {
                var rows = new List<IReadOnlyList<string>>();
                rows.Add(["items", batch.Items.ToString(CultureInfo.InvariantCulture)]);
                rows.Add(["answered", batch.Answered.ToString(CultureInfo.InvariantCulture)]);
                rows.Add(["malformed", batch.Malformed.Count.ToString(CultureInfo.InvariantCulture)]);
                rows.Add(["missing", batch.Missing.Count.ToString(CultureInfo.InvariantCulture)]);
                rows.Add(["stories", batch.Stories.Count.ToString(CultureInfo.InvariantCulture)]);
                rows.Add([$"lines of {q.Field}", lines.Count.ToString(CultureInfo.InvariantCulture)]);
                var perItem = lines.GroupBy(l => l.Item).Select(g => g.Count()).OrderBy(x => x).ToList();
                if (perItem.Count > 0) rows.Add([$"lines per item (min/median/max)", $"{perItem[0]}/{perItem[perItem.Count / 2]}/{perItem[^1]}"]);
                var arity = batch.Arity.Where(a => a.Field == q.Field).ToList();
                rows.Add(["arity problems", arity.Count.ToString(CultureInfo.InvariantCulture)]);
                foreach (var a in arity.Take(top)) rows.Add([$"  {a.Item} line {a.Ordinal}", $"{a.Parts} parts, {a.Declared} declared"]);
                foreach (var c in field.Columns)
                {
                    var none = lines.Count(l => Terms.IsNone(l.Part(c.Position)));
                    var hedged = lines.Count(l => Terms.IsHedgedNone(l.Part(c.Position)));
                    var empty = lines.Count(l => l.Part(c.Position).Length == 0);
                    rows.Add([$"{c.Id} ({c.Position}): {c.Label}", $"none {none}, hedged none {hedged}, empty {empty}"]);
                }
                rows.Add(["stopwords", string.Join(" ", Terms.Stopwords)]);
                rows.Add(["normalization", "lowercase; 's dropped; non-alphanumerics to spaces; stopwords dropped; nothing stemmed or merged"]);
                return (R($"health of {q.Field}" + (q.Filters.Count > 0 || q.Story is not null || q.Item is not null ? $" over {caption}" : ""), ["what", "value"], rows), null);
            }
            default:
                return (null, $"view '{q.View}' is not implemented");
        }
    }

    /// <summary>The field's lines after the filters, the scope and the sample, in result order; the sample is a seeded shuffle's first n, so the same seed gives the same lines.</summary>
    static IReadOnlyList<ResultLine> Select(LoadedBatch batch, Query q, PartField field)
    {
        var res = q.Filters.Select(f => (field.Find(f.Column)!.Position, new Regex(f.Pattern, RegexOptions.IgnoreCase))).ToList();
        IEnumerable<ResultLine> lines = batch.Lines.Where(l => l.Field == field.Key);
        if (q.Story is not null) lines = lines.Where(l => l.Story == q.Story);
        if (q.Item is not null) lines = lines.Where(l => l.Item == q.Item);
        lines = lines.Where(l => res.All(r => r.Item2.IsMatch(l.Part(r.Position))));
        var list = lines.ToList();
        if (q.Sample is { } n && n < list.Count)
        {
            var rng = new Random(q.Seed ?? 0);
            var order = Enumerable.Range(0, list.Count).ToArray();
            for (var i = order.Length - 1; i > 0; i--) { var j = rng.Next(i + 1); (order[i], order[j]) = (order[j], order[i]); }
            var picked = order.Take(n).OrderBy(i => i).Select(i => list[i]).ToList();
            return picked;
        }
        return list;
    }
}
