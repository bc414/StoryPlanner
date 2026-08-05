using System.Text.RegularExpressions;

namespace StoryPlanner.SourceTexts;

/// <summary>
/// Reads Equestria at War's HoI4 localisation YAML into one unit per key.
///
/// One unit per key, not one blob per country, because that is what makes a citation precise:
/// "EQS_Crystal_Fair_desc" names a single focus description, whereas a flattened country file
/// names everything Equestria says. It is also what the Work/Part model already implies — a Part
/// is one unit of a mining pass, and a focus description is one such unit.
///
/// Scripting variables ([Root.GetName], [CRY.Capital.GetName]) are kept verbatim: they cannot be
/// resolved outside the game, and substituting a guess would put words in the mod's mouth. Only
/// the §-colour codes are stripped, being presentation with no reading value.
/// </summary>
public static class Hoi4LocalisationReader
{
    // key:0 "value" — the version number after the colon is optional in practice.
    private static readonly Regex Entry = new(
        """^\s*([A-Za-z0-9_.\-]+):\d*\s+"(.*)"\s*$""", RegexOptions.Compiled);

    private static readonly Regex ColourCode = new("§.", RegexOptions.Compiled);
    private static readonly Regex CountryFile = new(
        @"^country_([A-Za-z]{3})(?:_.*)?_l_english\.yml$", RegexOptions.IgnoreCase);

    /// <summary>The 3-letter country tag a localisation filename belongs to, or null.</summary>
    public static string? TagFromFileName(string fileName)
    {
        var m = CountryFile.Match(fileName);
        return m.Success ? m.Groups[1].Value.ToUpperInvariant() : null;
    }

    public static IReadOnlyList<SourceTextUnit> Read(
        string workName,
        string partCode,
        string yml,
        bool includeTooltips,
        string sourceRef)
    {
        var units = new List<SourceTextUnit>();
        var order = 0;
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var raw in yml.Split('\n'))
        {
            var line = raw.TrimEnd('\r');
            if (line.Length == 0) continue;
            var trimmed = line.TrimStart();
            if (trimmed.StartsWith('#') || trimmed.StartsWith("l_", StringComparison.OrdinalIgnoreCase)) continue;

            var m = Entry.Match(line);
            if (!m.Success) continue;

            var key = m.Groups[1].Value;
            // Tooltips are mechanical effect text ("§Y[CRY.GetName]§! will be less likely to seek
            // independence") rather than flavour; whether they belong is Brian's call, not ours.
            if (!includeTooltips && key.EndsWith("_tt", StringComparison.OrdinalIgnoreCase)) continue;

            var value = Clean(m.Groups[2].Value);
            if (value.Length == 0) continue;
            if (!seen.Add(key)) continue; // a later duplicate key is a mod override; first wins

            units.Add(new SourceTextUnit
            {
                WorkName = workName,
                PartCode = partCode,
                UnitKey = key,
                Kind = SourceTextKind.Flavor,
                OrderIndex = order++,
                Body = value,
                SourceRef = sourceRef
            });
        }
        return units;
    }

    private static string Clean(string value)
    {
        value = ColourCode.Replace(value, "");
        value = value.Replace("\\n", "\n").Replace("\\\"", "\"");
        return value.Trim();
    }
}
