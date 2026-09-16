using System.Text.RegularExpressions;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.ResultsQuery;

/// <summary>
/// A query resolved against a loaded batch. The form, <see cref="Query"/>, lives in BatchFiles
/// so the checker holds a lead's query line without reading results (d-2026-09-15-3); resolving
/// needs the batch's fields and columns, and lives here.
/// </summary>
public static class QueryResolution
{
    static readonly Regex Slug = new(@"^[a-z0-9-]+$", RegexOptions.Compiled);

    /// <summary>
    /// Checks the query against a batch and returns it in canonical form: the field known,
    /// each filter's column and any col resolved to the column's short id, filters in column
    /// order, defaults left implicit. The error names the first thing wrong.
    /// </summary>
    public static (Query? Query, string? Error) Resolve(this Query query, LoadedBatch batch)
    {
        var field = batch.Field(query.Field);
        if (field is null) return (null, $"field '{query.Field}' is not a bar-part field of this batch; the fields are {string.Join(", ", batch.Fields.Select(f => f.Key))}");
        var resolved = new List<(Column Column, string Pattern)>();
        foreach (var f in query.Filters)
        {
            var c = field.Find(f.Column);
            if (c is null) return (null, $"column '{f.Column}' is not a column of {field.Key}; the columns are {string.Join(", ", field.Columns.Select(x => $"{x.Id} ({x.Position})"))}");
            if (!ValidRegex(f.Pattern, out var re)) return (null, $"'{f.Pattern}' is not a regex: {re}");
            resolved.Add((c, f.Pattern));
        }
        if (resolved.Select(r => r.Column.Position).Distinct().Count() != resolved.Count) return (null, "one filter per column");
        Column? col = null;
        if (query.Col is not null)
        {
            col = field.Find(query.Col);
            if (col is null) return (null, $"col '{query.Col}' is not a column of {field.Key}");
        }
        if (query.Exclude is not null && !ValidRegex(query.Exclude, out var ex)) return (null, $"exclude '{query.Exclude}' is not a regex: {ex}");
        if (query.Story is not null && !batch.Stories.Any(s => s.Story == query.Story)) return (null, $"story '{query.Story}' is not in the index");
        if (query.Item is not null && !Slug.IsMatch(query.Item)) return (null, $"item '{query.Item}' is not a slug");
        if (query.Group is not null && query.Group == LoadedBatch.DefaultGroupSeparator) return (null, $"group is only given when it is not '{LoadedBatch.DefaultGroupSeparator}'");
        var q = query with
        {
            Batch = batch.BatchId,
            Field = field.Key,
            Filters = resolved.OrderBy(r => r.Column.Position).Select(r => new Filter(r.Column.Id, r.Pattern)).ToList(),
            Col = col?.Id,
            N = query.N == 1 ? null : query.N,
            Seed = query.Sample is null ? null : query.Seed ?? 0,
        };
        return (q, null);
    }

    static bool ValidRegex(string pattern, out string error)
    {
        try { _ = new Regex(pattern); error = ""; return true; }
        catch (ArgumentException e) { error = e.Message; return false; }
    }
}
