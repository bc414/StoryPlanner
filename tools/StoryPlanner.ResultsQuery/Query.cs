using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.ResultsQuery;

/// <summary>One filter: the column, by whatever name the query gave, and the regex matched against its part, case-insensitive.</summary>
public sealed record Filter(string Column, string Pattern);

/// <summary>
/// One query over a batch and its one canonical string, parsed and printed by the same code so
/// that two sessions asking the same thing write the same string, and a lead can carry it:
/// <code>rq1 batch=&lt;study/batch&gt; answered=&lt;n&gt; field=&lt;key&gt; [where &lt;col&gt;~&lt;regex&gt; ...] [story=&lt;slug&gt;] [item=&lt;id&gt;] [sample=&lt;n&gt; seed=&lt;s&gt;] view=&lt;verb&gt; [col=..] [n=..] [position=..] [exclude=..] [top=..] [group=..]</code>
/// A value with a space, a quote or a backslash is double-quoted, <c>\"</c> and <c>\\</c>
/// escaped. <c>answered</c> is the snapshot the query was run over; a run over a batch with
/// another count says so. The views: <c>list</c>, <c>cites</c>, <c>terms</c>, <c>pairs</c>,
/// <c>by-story</c>, <c>sort</c>, <c>health</c>.
/// </summary>
public sealed record Query(
    string Batch,
    int Answered,
    string Field,
    IReadOnlyList<Filter> Filters,
    string? Story,
    string? Item,
    int? Sample,
    int? Seed,
    string View,
    string? Col,
    int? N,
    int? Position,
    string? Exclude,
    int? Top,
    string? Group)
{
    public const string Version = "rq1";
    public static readonly string[] Views = ["list", "cites", "terms", "pairs", "by-story", "sort", "health"];
    /// <summary>The views that take a column: the terms and pairs of it, or the values of it sorted.</summary>
    public static readonly string[] ColumnViews = ["terms", "pairs", "sort"];
    static readonly string[] OptionOrder = ["col", "n", "position", "exclude", "top", "group"];
    static readonly Regex Slug = new(@"^[a-z0-9-]+$", RegexOptions.Compiled);

    /// <summary>The canonical string.</summary>
    public string Print()
    {
        var sb = new StringBuilder();
        sb.Append(Version).Append(" batch=").Append(Quote(Batch)).Append(" answered=").Append(Answered.ToString(CultureInfo.InvariantCulture));
        sb.Append(" field=").Append(Quote(Field));
        foreach (var f in Filters) sb.Append(" where ").Append(Quote(f.Column)).Append('~').Append(Quote(f.Pattern));
        if (Story is not null) sb.Append(" story=").Append(Quote(Story));
        if (Item is not null) sb.Append(" item=").Append(Quote(Item));
        if (Sample is { } s) { sb.Append(" sample=").Append(s); sb.Append(" seed=").Append(Seed ?? 0); }
        sb.Append(" view=").Append(View);
        if (Col is not null) sb.Append(" col=").Append(Quote(Col));
        if (N is { } n) sb.Append(" n=").Append(n);
        if (Position is { } p) sb.Append(" position=").Append(p);
        if (Exclude is not null) sb.Append(" exclude=").Append(Quote(Exclude));
        if (Top is { } t) sb.Append(" top=").Append(t);
        if (Group is not null) sb.Append(" group=").Append(Quote(Group));
        return sb.ToString();
    }

    public override string ToString() => Print();

    /// <summary>Parses a string; the error names the first thing wrong. Filters are kept in the order given; <see cref="Resolve"/> puts them in column order.</summary>
    public static (Query? Query, string? Error) Parse(string text)
    {
        var tokens = Tokenize(text, out var tokenError);
        if (tokenError is not null) return (null, tokenError);
        if (tokens.Count == 0 || tokens[0] != Version) return (null, $"a query begins with {Version}");
        string? batch = null, field = null, story = null, item = null, view = null, col = null, exclude = null, group = null;
        int? answered = null, sample = null, seed = null, n = null, position = null, top = null;
        var filters = new List<Filter>();
        for (var i = 1; i < tokens.Count; i++)
        {
            var t = tokens[i];
            if (t == "where")
            {
                if (i + 1 >= tokens.Count) return (null, "where is followed by <column>~<regex>");
                var w = tokens[++i];
                var tilde = w.IndexOf('~');
                if (tilde <= 0) return (null, $"'{w}' is not <column>~<regex>");
                filters.Add(new Filter(w[..tilde], w[(tilde + 1)..]));
                continue;
            }
            var eq = t.IndexOf('=');
            if (eq <= 0) return (null, $"'{t}' is not key=value");
            var key = t[..eq];
            var value = t[(eq + 1)..];
            int? Int() => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var x) ? x : null;
            switch (key)
            {
                case "batch": batch = value; break;
                case "answered": answered = Int(); if (answered is null) return (null, "answered is a number"); break;
                case "field": field = value; break;
                case "story": story = value; break;
                case "item": item = value; break;
                case "sample": sample = Int(); if (sample is null or < 1) return (null, "sample is a number of at least 1"); break;
                case "seed": seed = Int(); if (seed is null) return (null, "seed is a number"); break;
                case "view": view = value; break;
                case "col": col = value; break;
                case "n": n = Int(); if (n is null or < 1 or > 3) return (null, "n is 1, 2 or 3"); break;
                case "position": position = Int(); if (position is null or < 1) return (null, "position is a number of at least 1"); break;
                case "exclude": exclude = value; break;
                case "top": top = Int(); if (top is null or < 1) return (null, "top is a number of at least 1"); break;
                case "group": group = value; break;
                default: return (null, $"'{key}' is not a key of a query");
            }
        }
        if (batch is null) return (null, "batch=<study/batch> is missing");
        if (answered is null) return (null, "answered=<n> is missing");
        if (field is null) return (null, "field=<key> is missing");
        if (view is null) return (null, "view=<verb> is missing");
        if (!Views.Contains(view)) return (null, $"view '{view}' is not one of {string.Join(", ", Views)}");
        if (ColumnViews.Contains(view) && col is null) return (null, $"view {view} needs col=<column>");
        if (story is not null && item is not null) return (null, "story and item are not both given");
        if (sample is not null && seed is null) seed = 0;
        if (seed is not null && sample is null) return (null, "seed goes with sample");
        return (new Query(batch, answered.Value, field, filters, story, item, sample, seed, view, col, n, position, exclude, top, group), null);
    }

    /// <summary>
    /// Checks the query against a batch and returns it in canonical form: the field known,
    /// each filter's column and any col resolved to the column's short id, filters in column
    /// order, defaults left implicit. The error names the first thing wrong.
    /// </summary>
    public (Query? Query, string? Error) Resolve(LoadedBatch batch)
    {
        var field = batch.Field(Field);
        if (field is null) return (null, $"field '{Field}' is not a bar-part field of this batch; the fields are {string.Join(", ", batch.Fields.Select(f => f.Key))}");
        var resolved = new List<(Column Column, string Pattern)>();
        foreach (var f in Filters)
        {
            var c = field.Find(f.Column);
            if (c is null) return (null, $"column '{f.Column}' is not a column of {field.Key}; the columns are {string.Join(", ", field.Columns.Select(x => $"{x.Id} ({x.Position})"))}");
            if (!ValidRegex(f.Pattern, out var re)) return (null, $"'{f.Pattern}' is not a regex: {re}");
            resolved.Add((c, f.Pattern));
        }
        if (resolved.Select(r => r.Column.Position).Distinct().Count() != resolved.Count) return (null, "one filter per column");
        Column? col = null;
        if (Col is not null)
        {
            col = field.Find(Col);
            if (col is null) return (null, $"col '{Col}' is not a column of {field.Key}");
        }
        if (Exclude is not null && !ValidRegex(Exclude, out var ex)) return (null, $"exclude '{Exclude}' is not a regex: {ex}");
        if (Story is not null && !batch.Stories.Any(s => s.Story == Story)) return (null, $"story '{Story}' is not in the index");
        if (Item is not null && !Slug.IsMatch(Item)) return (null, $"item '{Item}' is not a slug");
        if (Group is not null && Group == LoadedBatch.DefaultGroupSeparator) return (null, $"group is only given when it is not '{LoadedBatch.DefaultGroupSeparator}'");
        var q = this with
        {
            Batch = batch.BatchId,
            Field = field.Key,
            Filters = resolved.OrderBy(r => r.Column.Position).Select(r => new Filter(r.Column.Id, r.Pattern)).ToList(),
            Col = col?.Id,
            N = N == 1 ? null : N,
            Seed = Sample is null ? null : Seed ?? 0,
        };
        return (q, null);
    }

    static bool ValidRegex(string pattern, out string error)
    {
        try { _ = new Regex(pattern); error = ""; return true; }
        catch (ArgumentException e) { error = e.Message; return false; }
    }

    public static string Quote(string value)
    {
        if (value.Length > 0 && !value.Any(c => char.IsWhiteSpace(c) || c == '"' || c == '\\')) return value;
        return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
    }

    /// <summary>Splits on whitespace, a double-quoted run one token with its escapes undone; a quote may open mid-token (<c>where col~"a b"</c>).</summary>
    public static IReadOnlyList<string> Tokenize(string text, out string? error)
    {
        error = null;
        var tokens = new List<string>();
        var sb = new StringBuilder();
        var inQuote = false;
        var any = false;
        for (var i = 0; i < text.Length; i++)
        {
            var ch = text[i];
            if (inQuote)
            {
                if (ch == '\\' && i + 1 < text.Length) { sb.Append(text[++i]); continue; }
                if (ch == '"') { inQuote = false; continue; }
                sb.Append(ch);
                continue;
            }
            if (ch == '"') { inQuote = true; any = true; continue; }
            if (char.IsWhiteSpace(ch)) { if (any) { tokens.Add(sb.ToString()); sb.Clear(); any = false; } continue; }
            sb.Append(ch); any = true;
        }
        if (inQuote) { error = "a quote is not closed"; return tokens; }
        if (any) tokens.Add(sb.ToString());
        return tokens;
    }
}
