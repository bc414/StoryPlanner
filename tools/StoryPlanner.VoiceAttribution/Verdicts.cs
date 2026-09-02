using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.VoiceAttribution;

/// <summary>
/// Reads Brian's verdicts back out of a calibration sheet and crosses them with the CURRENT
/// run's labels and roles — the agreement measure for the instrument. A verdict is the text on
/// the line after the entry's header that starts with "verdict" (either "verdict: xxx" or, as
/// happened on the first sheet, "verdict xxx:"); its first word is normalised to one of
/// brian / model / mixed / phrase / paratext / ok / other.
/// </summary>
public static class Verdicts
{
    public sealed record Verdict(int NoteId, string SheetGroup, string Raw, string Class);

    private static readonly Regex Header = new(@"^### (?<group>[a-z\-]+) (?<n>\d+) · note (?<id>\d+)", RegexOptions.Multiline);
    private static readonly Regex Line = new(@"^verdict:?\s*(?<text>.*?)\s*:?\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

    public static List<Verdict> Parse(string sheetText)
    {
        var list = new List<Verdict>();
        var headers = Header.Matches(sheetText);
        for (int i = 0; i < headers.Count; i++)
        {
            int start = headers[i].Index;
            int end = i + 1 < headers.Count ? headers[i + 1].Index : sheetText.Length;
            var block = sheetText[start..end];
            var v = Line.Match(block);
            var raw = v.Success ? v.Groups["text"].Value.Trim() : "";
            list.Add(new Verdict(int.Parse(headers[i].Groups["id"].Value), headers[i].Groups["group"].Value, raw, Classify(raw)));
        }
        return list;
    }

    public static string Classify(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return "ok";
        var t = raw.Trim().ToLowerInvariant();
        var first = Regex.Match(t, @"^[a-z\?\-]+").Value;
        bool Has(params string[] needles) => needles.Any(n => t.Contains(n));
        if (first is "ok" or "yes" or "agree") return "ok";
        if (first == "?") return "unsure";
        if (Has("paratext")) return "paratext";
        if (first is "brian" or "me" or "mine" || Has("my prompt", "reads like my voice", "reads as me", "whole note reads as me", "note reads as me", "no ai involved")) return "brian";
        if (first is "mixed" or "mix" || Has("mixed", "a mix")) return "mixed";
        if (first is "phrase" or "boilerplate" || Has("boilerplate")) return "phrase";
        if (first is "model" or "ai" or "gemini" || Has("reads as ai", "from a model", "is accurately ai", "the whole thing reads as ai")) return "model";
        return "other";
    }

    /// <summary>Tool (label × role) rows against verdict classes, for the notes the sheet covers.</summary>
    public static string Matrix(List<Verdict> verdicts, Dictionary<int, Row> rows, string title)
    {
        var classes = new[] { "ok", "brian", "model", "mixed", "phrase", "paratext", "unsure", "other" };
        var sb = new StringBuilder();
        sb.AppendLine($"## agreement — {title} ({verdicts.Count} verdicts)");
        sb.AppendLine($"  {"tool label/role",-26}{"n",5} " + string.Join(" ", classes.Select(c => c.PadLeft(9))));
        var groups = verdicts.Where(v => rows.ContainsKey(v.NoteId))
            .GroupBy(v => $"{rows[v.NoteId].Label}/{(rows[v.NoteId].Role.Length > 0 ? rows[v.NoteId].Role : "—")}")
            .OrderBy(g => g.Key);
        foreach (var g in groups)
        {
            var counts = g.GroupBy(v => v.Class).ToDictionary(x => x.Key, x => x.Count());
            sb.AppendLine($"  {g.Key,-26}{g.Count(),5} " + string.Join(" ", classes.Select(c => counts.GetValueOrDefault(c).ToString().PadLeft(9))));
        }
        var missing = verdicts.Count(v => !rows.ContainsKey(v.NoteId));
        if (missing > 0) sb.AppendLine($"  ({missing} verdicts on notes not in this run)");
        sb.AppendLine("  reading: a 'brian' verdict on a model-role row, or a 'model' verdict on a brian/none row, is a disagreement; 'ok' is agreement; 'mixed' agrees with framed-paste; 'phrase' agrees with phrase.");
        sb.AppendLine("  verdicts classed 'other':");
        foreach (var v in verdicts.Where(v => v.Class == "other")) sb.AppendLine($"    note {v.NoteId}: {v.Raw}");
        return sb.ToString();
    }
}
