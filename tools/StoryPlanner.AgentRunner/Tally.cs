using System.Globalization;
using System.Text;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.AgentRunner;

/// <summary>
/// The tally (decisions.md, "The runner's verbs", changed by "verifying-a-corpus is assemble,
/// assess and write-findings; the host writes the tally at completion"): reads the definition,
/// the directions' What to produce and Classes, the index, the calls and the result files as
/// rendered, by the same parser that holds them to the declaration, and writes <c>tally.md</c>
/// once, its sections fixed by the directions: counts per enum field, the malformed and the
/// missing, free-text fields listed and never counted. The host writes it when the last item
/// has a successful call; <c>tally-batch</c> writes it only when it is absent. A grouping by
/// an index column is a view, printed and never written. The model's JSON is never read back.
/// </summary>
public static class Tally
{
    sealed record Reading(Dictionary<string, System.Text.Json.Nodes.JsonObject> Answered, List<(string Item, string Problem)> Malformed, List<string> Missing, CallsFile Calls);

    static Reading Read(Batch batch)
    {
        var d = batch.Directions;
        var calls = CallsFile.Read(batch.Definition.CallsPath);
        var answered = new Dictionary<string, System.Text.Json.Nodes.JsonObject>(StringComparer.Ordinal);
        var malformed = new List<(string Item, string Problem)>();
        var missing = new List<string>();
        foreach (var item in batch.Items)
        {
            var path = batch.ResultPath(item);
            if (!File.Exists(path) || !calls.HasSucceeded(item)) { missing.Add(item); continue; }
            var (answer, problems) = ResultFile.Parse(d, File.ReadAllText(path));
            if (problems.Count > 0) { malformed.Add((item, problems[0])); continue; }
            answered[item] = answer;
        }
        return new Reading(answered, malformed, missing, calls);
    }

    /// <summary>The tally's text: the fixed sections and nothing else.</summary>
    public static string Build(Batch batch)
    {
        var d = batch.Directions;
        var r = Read(batch);
        var enumFields = d.Output.Where(o => o.Kind == OutputKind.Enum).Select(o => o.Key).ToList();
        var freeFields = d.Output.Where(o => o.Kind != OutputKind.Enum).Select(o => $"{o.Key} ({KindWord(o.Kind)})").ToList();

        var sb = new StringBuilder();
        sb.Append("# ").Append(batch.Name).Append(" — tally\n\n");
        sb.Append(KeyedLines.RenderLine("definition", batch.Definition.Hash)).Append('\n');
        sb.Append(KeyedLines.RenderLine("directions", $"{Path.GetFileNameWithoutExtension(batch.Definition.DirectionsPath!)}@{d.BodyHash}")).Append('\n');
        sb.Append(KeyedLines.RenderLine("items", batch.Items.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        sb.Append(KeyedLines.RenderLine("answered", r.Answered.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        sb.Append(KeyedLines.RenderLine("malformed", r.Malformed.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        sb.Append(KeyedLines.RenderLine("missing", r.Missing.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        sb.Append(KeyedLines.RenderLine("calls", r.Calls.Entries.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        var cost = r.Calls.Entries.Sum(c => c.Cost ?? 0);
        sb.Append(KeyedLines.RenderLine("cost", cost.ToString("F4", CultureInfo.InvariantCulture))).Append('\n');

        foreach (var field in enumFields)
        {
            sb.Append("\n## ").Append(field).Append("\n\n| value | count |\n|---|---|\n");
            var counts = d.Classes.ToDictionary(c => c.Label, _ => 0, StringComparer.Ordinal);
            foreach (var a in r.Answered.Values)
            {
                var v = a[field]?.ToString() ?? "";
                counts[v] = counts.GetValueOrDefault(v) + 1;
            }
            foreach (var (value, count) in counts) sb.Append("| ").Append(value).Append(" | ").Append(count).Append(" |\n");
        }

        sb.Append("\n## Malformed\n\n");
        if (r.Malformed.Count == 0) sb.Append("none\n");
        else
        {
            sb.Append("| item | problem |\n|---|---|\n");
            foreach (var (item, problem) in r.Malformed) sb.Append("| ").Append(item).Append(" | ").Append(problem.Replace("|", "\\|")).Append(" |\n");
        }

        sb.Append("\n## Missing\n\n");
        if (r.Missing.Count == 0) sb.Append("none\n");
        else foreach (var item in r.Missing) sb.Append("- ").Append(item).Append('\n');

        sb.Append("\n## Not counted\n\n");
        if (freeFields.Count == 0) sb.Append("none\n");
        else foreach (var f in freeFields) sb.Append("- ").Append(f).Append('\n');
        return sb.ToString();
    }

    /// <summary>
    /// A view for the analysis and the review, never written: a cross-tab of every enum field's
    /// classes by one column of the index (item, locator or description), one row per distinct
    /// value in index order.
    /// </summary>
    public static string GroupBy(Batch batch, string column)
    {
        if (!IndexFile.Columns.Contains(column)) return $"'{column}' is not a column of the index ({string.Join(", ", IndexFile.Columns)})\n";
        var d = batch.Directions;
        var r = Read(batch);
        var enumFields = d.Output.Where(o => o.Kind == OutputKind.Enum).Select(o => o.Key).ToList();
        var sb = new StringBuilder();
        sb.Append("## By ").Append(column).Append("\n\n| ").Append(column);
        foreach (var field in enumFields) foreach (var c in d.Classes) sb.Append(" | ").Append(field).Append('=').Append(c.Label);
        sb.Append(" |\n|---|");
        foreach (var _ in enumFields.SelectMany(_ => d.Classes)) sb.Append("---|");
        sb.Append('\n');
        var groups = batch.Index.Rows.GroupBy(row => column switch { "item" => row.Item, "locator" => row.Locator, _ => row.Description }, StringComparer.Ordinal);
        foreach (var g in groups)
        {
            sb.Append("| ").Append(g.Key.Replace("|", "\\|"));
            foreach (var field in enumFields)
                foreach (var c in d.Classes)
                    sb.Append(" | ").Append(g.Count(row => r.Answered.TryGetValue(row.Item, out var a) && (a[field]?.ToString() ?? "") == c.Label));
            sb.Append(" |\n");
        }
        return sb.ToString();
    }

    static string KindWord(OutputKind k) => k switch { OutputKind.Line => "line", OutputKind.Block => "block", OutputKind.ListOfLine => "list of line", _ => "enum" };

    /// <summary>Writes the tally once; a written tally is frozen and a second write is refused.</summary>
    public static (bool Ok, string Message) Write(Batch batch)
    {
        var path = batch.Definition.TallyPath;
        if (File.Exists(path)) return (false, $"{path} exists; a tally is written once and never edited");
        File.WriteAllText(path, Build(batch), new UTF8Encoding(false));
        return (true, $"wrote {path}");
    }
}
