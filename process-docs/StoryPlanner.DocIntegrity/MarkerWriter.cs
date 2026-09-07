using System.Text;

namespace StoryPlanner.ProcessMap;

/// <summary>
/// Replaces only what lies between <c>&lt;!-- generated:name --&gt;</c> and
/// <c>&lt;!-- /generated --&gt;</c>. Everything outside a marker pair — every row, every
/// sentence of prose — is copied through byte for byte.
/// </summary>
public static class MarkerWriter
{
    public static string Write(string markdown, IReadOnlyDictionary<string, string> sections)
    {
        var newline = markdown.Contains("\r\n") ? "\r\n" : "\n";
        var lines = markdown.Replace("\r\n", "\n").Split('\n');
        var sb = new StringBuilder();
        var written = new HashSet<string>(StringComparer.Ordinal);

        for (var i = 0; i < lines.Length; i++)
        {
            var trimmed = lines[i].Trim();
            sb.Append(lines[i]).Append(newline);

            if (!trimmed.StartsWith(MapTables.GeneratedOpenPrefix)) continue;

            var name = trimmed[MapTables.GeneratedOpenPrefix.Length..].Replace("-->", "").Trim();
            var close = i + 1;
            while (close < lines.Length && lines[close].Trim() != MapTables.GeneratedClose) close++;
            if (close >= lines.Length)
                throw new MapFormatException(
                    $"line {i + 1}: generated section '{name}' is never closed by {MapTables.GeneratedClose}.");

            if (!sections.TryGetValue(name, out var body))
                throw new MapFormatException(
                    $"line {i + 1}: nothing generates a section named '{name}'.");

            foreach (var l in body.TrimEnd('\n').Split('\n'))
                sb.Append(l).Append(newline);
            sb.Append(lines[close]).Append(newline);
            written.Add(name);
            i = close;
        }

        var missing = sections.Keys.Where(k => !written.Contains(k)).OrderBy(k => k, StringComparer.Ordinal).ToList();
        if (missing.Count > 0)
            throw new MapFormatException(
                $"no marker pair for generated section(s): {string.Join(", ", missing)}.");

        // Split on '\n' yields a trailing empty element for a file ending in a newline; the loop
        // has already re-appended one, so drop the duplicate.
        var text = sb.ToString();
        if (markdown.EndsWith('\n') && text.EndsWith(newline + newline))
            text = text[..^newline.Length];
        else if (!markdown.EndsWith('\n') && text.EndsWith(newline))
            text = text[..^newline.Length];
        return text;
    }
}
