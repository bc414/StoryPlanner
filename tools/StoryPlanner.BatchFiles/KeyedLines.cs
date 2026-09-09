using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.BatchFiles;

/// <summary>One keyed line as read: its key, the value on the line, the continuation lines beneath it, and where it was.</summary>
public sealed record KeyedField(string Key, string Value, IReadOnlyList<string> Continuation, int Line)
{
    /// <summary>The value on the line and every continuation line that is not a list item, joined by newlines and trimmed.</summary>
    public string BlockText
    {
        get
        {
            var parts = new List<string>();
            if (Value.Length > 0) parts.Add(Value);
            parts.AddRange(Continuation.Where(c => !IsListItem(c)).Select(c => c.Length >= 2 ? c[2..] : c.TrimStart()));
            return string.Join('\n', parts).Trim();
        }
    }

    /// <summary>The continuation lines that are list items, without their markers.</summary>
    public IReadOnlyList<string> ListItems
        => Continuation.Where(IsListItem).Select(c => c.TrimStart()[2..].TrimEnd()).ToList();

    public bool HasContinuation => Continuation.Count > 0;

    static bool IsListItem(string continuation) => continuation.TrimStart().StartsWith("- ", StringComparison.Ordinal);
}

/// <summary>A line inside a keyed block that is neither a keyed line nor a continuation of one.</summary>
public sealed record StrayLine(string Text, int Line);

public sealed record KeyedBlock(IReadOnlyList<KeyedField> Fields, IReadOnlyList<StrayLine> Stray)
{
    public KeyedField? Field(string key) => Fields.FirstOrDefault(f => f.Key == key);
    public string? Value(string key) => Field(key)?.Value;
}

/// <summary>
/// The one line grammar of the artifacts' fields, read and written here and nowhere else:
/// a keyed line is <c>- key: value</c> at the left margin, one space after the colon; a line
/// indented two spaces continues the value above it, and a continuation that begins with
/// <c>- </c> is one item of a list; a blank line followed by a continuation continues it too.
/// The same mapping renders a result from the model's JSON and reads it back.
/// </summary>
public static class KeyedLines
{
    static readonly Regex Keyed = new(@"^- (?<key>[a-z][a-z0-9 ]*?): ?(?<value>.*)$", RegexOptions.Compiled);

    public static bool IsKeyedLine(string line) => Keyed.IsMatch(line);

    /// <summary>Reads a block of lines; <paramref name="firstLineNumber"/> is the 1-based number of <c>lines[0]</c>.</summary>
    public static KeyedBlock Read(IReadOnlyList<string> lines, int firstLineNumber = 1)
    {
        var fields = new List<KeyedField>();
        var stray = new List<StrayLine>();
        string? key = null, value = null;
        var line = 0;
        var cont = new List<string>();

        void Flush()
        {
            if (key is not null) fields.Add(new KeyedField(key, value!, cont, line));
            key = null;
            cont = [];
        }

        for (var i = 0; i < lines.Count; i++)
        {
            var raw = lines[i].TrimEnd('\r');
            var n = firstLineNumber + i;
            if (raw.Trim().Length == 0) continue;
            var m = Keyed.Match(raw);
            if (m.Success)
            {
                Flush();
                key = m.Groups["key"].Value;
                value = m.Groups["value"].Value.Trim();
                line = n;
                continue;
            }
            if (raw.StartsWith("  ", StringComparison.Ordinal) && key is not null) { cont.Add(raw); continue; }
            stray.Add(new StrayLine(raw, n));
        }
        Flush();
        return new KeyedBlock(fields, stray);
    }

    /// <summary>A one-value line.</summary>
    public static string RenderLine(string key, string value) => $"- {key}: {value}";

    /// <summary>A block: the first line on the keyed line, the rest as two-space continuations.</summary>
    public static string RenderBlock(string key, string text)
    {
        var parts = Hashing.NormalizeNewlines(text).Split('\n');
        var sb = new StringBuilder();
        sb.Append("- ").Append(key).Append(':');
        if (parts.Length > 0 && parts[0].Length > 0) sb.Append(' ').Append(parts[0]);
        for (var i = 1; i < parts.Length; i++) sb.Append('\n').Append("  ").Append(parts[i]);
        return sb.ToString();
    }

    /// <summary>A list: the key alone on its line, one <c>  - item</c> per item beneath it.</summary>
    public static string RenderList(string key, IEnumerable<string> items)
    {
        var sb = new StringBuilder();
        sb.Append("- ").Append(key).Append(':');
        foreach (var item in items) sb.Append("\n  - ").Append(item.Replace("\r", "").Replace("\n", " "));
        return sb.ToString();
    }
}
