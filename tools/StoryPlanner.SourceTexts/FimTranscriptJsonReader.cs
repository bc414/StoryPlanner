using System.Text;
using System.Text.Json;

namespace StoryPlanner.SourceTexts;

/// <summary>
/// Reads the MLP:FiM transcript corpus (one JSON object keyed by overall episode number, each
/// entry carrying season/number_in_season/title/writers/airdate/transcript_url and a
/// [{character, line}] transcript) into one unit per episode, coded "S{season}E{nn}".
///
/// The corpus and the plan's Part set do not line up one-to-one, and that is expected: the plan
/// merges the two-part episodes into a single Part ("S2E01 The Return of Harmony" with no S2E02),
/// while the corpus lists both halves, and the corpus has no entries for the movie, the specials,
/// or the clip-show shorts. Merges are taken from the config's mergeInto map — written down once
/// as a decision — and never inferred from titles; anything unmatched is reported, never dropped
/// silently.
/// </summary>
public static class FimTranscriptJsonReader
{
    public static IReadOnlyList<SourceTextUnit> Read(
        string workName,
        string json,
        IReadOnlyDictionary<string, string> mergeInto,
        string sourceRef,
        IngestReport report)
    {
        using var doc = JsonDocument.Parse(json);

        // Overall episode number is the object key, so ordering by it keeps airdate order.
        var entries = doc.RootElement.EnumerateObject()
            .Select(p => (Key: p.Name, Value: p.Value))
            .OrderBy(e => int.TryParse(e.Key, out var n) ? n : int.MaxValue)
            .ToList();

        var byCode = new Dictionary<string, List<SourceTextUnit>>();
        foreach (var (key, value) in entries)
        {
            if (!value.TryGetProperty("season", out var seasonEl) ||
                !value.TryGetProperty("number_in_season", out var numEl))
            {
                report.Note($"{workName}: entry \"{key}\" has no season/number_in_season — skipped.");
                continue;
            }

            var code = $"S{seasonEl.GetInt32()}E{numEl.GetInt32():00}";
            var target = mergeInto.TryGetValue(code, out var merged) ? merged : code;

            var title = Str(value, "title");
            var body = RenderTranscript(value);
            if (body.Length == 0)
            {
                report.Note($"{workName}: {code} \"{title}\" has an empty transcript — skipped.");
                continue;
            }

            if (!byCode.TryGetValue(target, out var list)) byCode[target] = list = [];
            list.Add(new SourceTextUnit
            {
                WorkName = workName,
                PartCode = target,
                // A merged two-parter keeps both halves addressable rather than concatenating
                // them into one anonymous blob.
                UnitKey = target == code ? "" : code,
                UnitLabel = title,
                Kind = SourceTextKind.Transcript,
                OrderIndex = list.Count,
                Body = body,
                SourceRef = Str(value, "transcript_url") is { Length: > 0 } url ? url : sourceRef
            });
            if (target != code) report.Note($"{workName}: {code} \"{title}\" merged into Part {target}.");
        }

        return byCode.Values.SelectMany(v => v).ToList();
    }

    private static string RenderTranscript(JsonElement entry)
    {
        if (!entry.TryGetProperty("transcript", out var lines) || lines.ValueKind != JsonValueKind.Array)
            return "";

        var sb = new StringBuilder();
        foreach (var line in lines.EnumerateArray())
        {
            var character = Str(line, "character");
            var text = Str(line, "line");
            if (text.Length == 0) continue;
            sb.AppendLine(character.Length > 0 ? $"{character}: {text}" : text);
        }
        return sb.ToString().TrimEnd();
    }

    private static string Str(JsonElement el, string prop) =>
        el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
}
