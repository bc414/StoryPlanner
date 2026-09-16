using System.Globalization;
using System.Text;

namespace StoryPlanner.BatchFiles;

/// <summary>One filter of a query: the column, by whatever name the query gave, and the regex matched against its part, case-insensitive.</summary>
public sealed record Filter(string Column, string Pattern);

/// <summary>
/// One question put to a batch's results and its one canonical string (d-2026-09-15-2, -3),
/// parsed and printed by the same code so that two sessions asking the same thing write the
/// same string, a lead can carry it, and the checker can hold it:
/// <code>rq1 batch=&lt;study/batch&gt; answered=&lt;n&gt; field=&lt;key&gt; [where &lt;col&gt;~&lt;regex&gt; ...] [story=&lt;slug&gt;] [item=&lt;id&gt;] [sample=&lt;n&gt; seed=&lt;s&gt;] view=&lt;verb&gt; [col=..] [n=..] [position=..] [exclude=..] [top=..] [group=..]</code>
/// A value with a space, a quote or a backslash is double-quoted, <c>\"</c> and <c>\\</c>
/// escaped. <c>answered</c> is the snapshot the query was run over; a run over a batch with
/// another count says so. The views: <c>list</c>, <c>cites</c>, <c>terms</c>, <c>pairs</c>,
/// <c>by-story</c>, <c>sort</c>, <c>health</c>. Resolving a query against a batch, which needs
/// the batch's results, lives with the tool that runs it, <c>tools/StoryPlanner.ResultsQuery</c>.
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

    /// <summary>Parses a string; the error names the first thing wrong. Filters are kept in the order given; the tool's resolve puts them in column order.</summary>
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
