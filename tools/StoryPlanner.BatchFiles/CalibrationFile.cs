using System.Text.RegularExpressions;

namespace StoryPlanner.BatchFiles;

/// <summary>
/// What the definition check reads of a calibration (calibration-schema): the directions
/// version and hash its title names, and whether its verdict accepts the version at that
/// hash. The rest of the file is not yet reviewed and not read here.
/// </summary>
public sealed class CalibrationFile
{
    static readonly Regex Title = new(@"^#\s*Calibration\s*—\s*directions-(?<n>\d+)@(?<hash>[0-9a-f]{6,64})\s*—\s*(?<date>\d{4}-\d{2}-\d{2}[a-z]?)\s*$", RegexOptions.Compiled);

    public int? Version { get; }
    public string? Hash { get; }
    public string? Date { get; }
    public bool Accepted { get; }
    public bool TitleParsed => Hash is not null;

    CalibrationFile(int? version, string? hash, string? date, bool accepted)
    { Version = version; Hash = hash; Date = date; Accepted = accepted; }

    public static CalibrationFile Read(string path) => Parse(File.ReadAllText(path));

    public static CalibrationFile Parse(string text)
    {
        var lines = Hashing.NormalizeNewlines(text).Split('\n');
        var first = lines.FirstOrDefault(l => l.StartsWith('#')) ?? "";
        var m = Title.Match(first);
        var verdict = Section(lines, "Verdict");
        var accepted = verdict.Contains("accepted", StringComparison.OrdinalIgnoreCase)
                       && !verdict.Contains("not accepted", StringComparison.OrdinalIgnoreCase);
        return m.Success
            ? new CalibrationFile(int.Parse(m.Groups["n"].Value), m.Groups["hash"].Value, m.Groups["date"].Value, accepted)
            : new CalibrationFile(null, null, null, accepted);
    }

    static string Section(string[] lines, string heading)
    {
        var inside = false;
        var sb = new System.Text.StringBuilder();
        foreach (var l in lines)
        {
            var t = l.Trim();
            if (t.StartsWith('#'))
            {
                if (inside) break;
                inside = t.TrimStart('#').Trim().Equals(heading, StringComparison.Ordinal);
                continue;
            }
            if (inside) sb.Append(l).Append('\n');
        }
        return sb.ToString();
    }
}
