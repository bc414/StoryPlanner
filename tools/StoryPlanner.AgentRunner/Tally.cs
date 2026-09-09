using System.Globalization;
using System.Text;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.AgentRunner;

/// <summary>
/// The <c>tally-batch</c> verb (decisions.md, "The runner's verbs"): reads the definition, the
/// directions' What to produce and Classes, the index, the calls and the result files as
/// rendered, by the same parser that holds them to the declaration, and writes
/// <c>tally.md</c> once: counts per enum field, the items whose value is in a set named on the
/// command line, the malformed and the missing, free-text fields listed and never counted,
/// and a grouping by any column of the index. The model's JSON is never read back.
/// </summary>
public static class Tally
{
    public sealed record Flag(string Field, string Value);

    public static string Build(Batch batch, IReadOnlyList<Flag> flags, string? groupBy)
    {
        var d = batch.Directions;
        var calls = CallsFile.Read(batch.Definition.CallsPath);
        var enumFields = d.Output.Where(o => o.Kind == OutputKind.Enum).Select(o => o.Key).ToList();
        var freeFields = d.Output.Where(o => o.Kind != OutputKind.Enum).Select(o => $"{o.Key} ({KindWord(o.Kind)})").ToList();

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

        var sb = new StringBuilder();
        sb.Append("# ").Append(batch.Name).Append(" — tally\n\n");
        sb.Append(KeyedLines.RenderLine("definition", batch.Definition.Hash)).Append('\n');
        sb.Append(KeyedLines.RenderLine("directions", $"{Path.GetFileNameWithoutExtension(batch.Definition.DirectionsPath!)}@{d.BodyHash}")).Append('\n');
        sb.Append(KeyedLines.RenderLine("items", batch.Items.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        sb.Append(KeyedLines.RenderLine("answered", answered.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        sb.Append(KeyedLines.RenderLine("malformed", malformed.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        sb.Append(KeyedLines.RenderLine("missing", missing.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        sb.Append(KeyedLines.RenderLine("calls", calls.Entries.Count.ToString(CultureInfo.InvariantCulture))).Append('\n');
        var cost = calls.Entries.Sum(c => c.Cost ?? 0);
        sb.Append(KeyedLines.RenderLine("cost", cost.ToString("F4", CultureInfo.InvariantCulture))).Append('\n');

        foreach (var field in enumFields)
        {
            sb.Append("\n## ").Append(field).Append("\n\n| value | count |\n|---|---|\n");
            var counts = d.Classes.ToDictionary(c => c.Label, _ => 0, StringComparer.Ordinal);
            foreach (var a in answered.Values)
            {
                var v = a[field]?.ToString() ?? "";
                counts[v] = counts.GetValueOrDefault(v) + 1;
            }
            foreach (var (value, count) in counts) sb.Append("| ").Append(value).Append(" | ").Append(count).Append(" |\n");
        }

        if (flags.Count > 0)
        {
            sb.Append("\n## Flagged\n\n| item | field | value |\n|---|---|---|\n");
            var order = batch.Items.ToList();
            foreach (var kv in answered.OrderBy(kv => order.IndexOf(kv.Key)))
                foreach (var flag in flags)
                    if ((kv.Value[flag.Field]?.ToString() ?? "") == flag.Value)
                        sb.Append("| ").Append(kv.Key).Append(" | ").Append(flag.Field).Append(" | ").Append(flag.Value).Append(" |\n");
        }

        sb.Append("\n## Malformed\n\n");
        if (malformed.Count == 0) sb.Append("none\n");
        else
        {
            sb.Append("| item | problem |\n|---|---|\n");
            foreach (var (item, problem) in malformed) sb.Append("| ").Append(item).Append(" | ").Append(problem.Replace("|", "\\|")).Append(" |\n");
        }

        sb.Append("\n## Missing\n\n");
        if (missing.Count == 0) sb.Append("none\n");
        else foreach (var item in missing) sb.Append("- ").Append(item).Append('\n');

        sb.Append("\n## Not counted\n\n");
        if (freeFields.Count == 0) sb.Append("none\n");
        else foreach (var f in freeFields) sb.Append("- ").Append(f).Append('\n');

        if (groupBy is not null)
        {
            sb.Append("\n## By ").Append(groupBy).Append("\n\n");
            if (!IndexFile.Columns.Contains(groupBy)) sb.Append($"'{groupBy}' is not a column of the index (item, locator, description)\n");
            else
            {
                sb.Append("| ").Append(groupBy);
                foreach (var field in enumFields) foreach (var c in d.Classes) sb.Append(" | ").Append(field).Append('=').Append(c.Label);
                sb.Append(" |\n|---|");
                foreach (var _ in enumFields.SelectMany(_ => d.Classes)) sb.Append("---|");
                sb.Append('\n');
                var groups = batch.Index.Rows.GroupBy(r => groupBy switch { "item" => r.Item, "locator" => r.Locator, _ => r.Description }, StringComparer.Ordinal);
                foreach (var g in groups)
                {
                    sb.Append("| ").Append(g.Key.Replace("|", "\\|"));
                    foreach (var field in enumFields)
                        foreach (var c in d.Classes)
                            sb.Append(" | ").Append(g.Count(r => answered.TryGetValue(r.Item, out var a) && (a[field]?.ToString() ?? "") == c.Label));
                    sb.Append(" |\n");
                }
            }
        }
        return sb.ToString();
    }

    static string KindWord(OutputKind k) => k switch { OutputKind.Line => "line", OutputKind.Block => "block", OutputKind.ListOfLine => "list of line", _ => "enum" };

    /// <summary>Writes the tally once; a written tally is frozen and a second run is refused.</summary>
    public static (bool Ok, string Message) Write(Batch batch, IReadOnlyList<Flag> flags, string? groupBy)
    {
        var path = batch.Definition.TallyPath;
        if (File.Exists(path)) return (false, $"{path} exists; a tally is written once and never edited");
        File.WriteAllText(path, Build(batch, flags, groupBy), new UTF8Encoding(false));
        return (true, $"wrote {path}");
    }
}
