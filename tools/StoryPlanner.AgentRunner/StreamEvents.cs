using System.Text.Json;

namespace StoryPlanner.AgentRunner;

/// <summary>One line of <c>stream.jsonl</c> as the page shows it: what kind of event, a one-line reading, and the raw line.</summary>
public sealed record StreamEvent(string Kind, string Text, string Raw);

/// <summary>
/// Reads the child's <c>stream-json</c> events into something a person can follow: the
/// agent's text, each tool call with a short account of its input, each tool result's size,
/// the init event's tool list, and the final result with cost. Never throws on a partial or
/// foreign line — it comes back as <c>raw</c>. The same reader serves a live tail and a
/// finished attempt; there is one code path.
/// </summary>
public static class StreamEvents
{
    public static StreamEvent Parse(string line)
    {
        line = line.TrimEnd('\r');
        if (string.IsNullOrWhiteSpace(line)) return new StreamEvent("raw", "", line);
        try
        {
            using var doc = JsonDocument.Parse(line);
            var root = doc.RootElement;
            var type = root.TryGetProperty("type", out var t) ? t.GetString() ?? "" : "";
            switch (type)
            {
                case "system":
                {
                    var tools = root.TryGetProperty("tools", out var ts) && ts.ValueKind == JsonValueKind.Array
                        ? string.Join(", ", ts.EnumerateArray().Select(e => e.GetString())) : "";
                    var model = root.TryGetProperty("model", out var m) ? m.GetString() : null;
                    var mcp = root.TryGetProperty("mcp_servers", out var ms) && ms.ValueKind == JsonValueKind.Array ? ms.GetArrayLength() : 0;
                    return new StreamEvent("init", $"init — model {model}; tools [{tools}]; mcp servers {mcp}", line);
                }
                case "assistant":
                {
                    var parts = new List<string>();
                    if (root.TryGetProperty("message", out var msg) && msg.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var block in content.EnumerateArray())
                        {
                            var bt = block.TryGetProperty("type", out var bty) ? bty.GetString() : null;
                            if (bt == "text" && block.TryGetProperty("text", out var tx))
                                parts.Add(Truncate(tx.GetString() ?? "", 400));
                            else if (bt == "tool_use")
                            {
                                var name = block.TryGetProperty("name", out var n) ? n.GetString() : "tool";
                                var input = block.TryGetProperty("input", out var inp) ? SummarizeInput(inp) : "";
                                return new StreamEvent("tool", $"{name} {input}".TrimEnd(), line);
                            }
                        }
                    }
                    return new StreamEvent("text", parts.Count > 0 ? string.Join(" ", parts) : "(assistant)", line);
                }
                case "user":
                {
                    var size = 0;
                    if (root.TryGetProperty("message", out var msg) && msg.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.Array)
                        foreach (var block in content.EnumerateArray())
                            if (block.TryGetProperty("content", out var c))
                                size += c.ValueKind == JsonValueKind.String ? c.GetString()!.Length : c.GetRawText().Length;
                    return new StreamEvent("result", $"tool result — {size:N0} chars", line);
                }
                case "result":
                {
                    var cost = root.TryGetProperty("total_cost_usd", out var c) && c.ValueKind == JsonValueKind.Number ? c.GetDouble() : (double?)null;
                    var turns = root.TryGetProperty("num_turns", out var n) && n.ValueKind == JsonValueKind.Number ? n.GetInt32() : (int?)null;
                    var reply = root.TryGetProperty("result", out var r) && r.ValueKind == JsonValueKind.String ? Truncate(r.GetString() ?? "", 300) : "";
                    return new StreamEvent("done", $"done — {(cost is { } cc ? $"${cc:F3}" : "cost ?")}, {turns?.ToString() ?? "?"} turn(s). {reply}".TrimEnd(), line);
                }
                default:
                    return new StreamEvent("raw", Truncate(line, 200), line);
            }
        }
        catch (JsonException)
        {
            return new StreamEvent("raw", Truncate(line, 200), line);
        }
    }

    /// <summary>The last <paramref name="maxEvents"/> events of a stream file; empty when it does not exist yet.</summary>
    public static IReadOnlyList<StreamEvent> ReadTail(string path, int maxEvents)
    {
        if (!File.Exists(path)) return [];
        List<string> lines;
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
        using (var reader = new StreamReader(fs))
        {
            lines = [];
            while (reader.ReadLine() is { } l) lines.Add(l);
        }
        return lines.Skip(Math.Max(0, lines.Count - maxEvents)).Select(Parse).ToList();
    }

    private static string SummarizeInput(JsonElement input)
    {
        if (input.ValueKind != JsonValueKind.Object) return Truncate(input.GetRawText(), 120);
        // The two things a classifier does: write one file, read one file.
        if (input.TryGetProperty("file_path", out var fp) && fp.ValueKind == JsonValueKind.String)
        {
            var size = input.TryGetProperty("content", out var ct) && ct.ValueKind == JsonValueKind.String ? $" ({ct.GetString()!.Length:N0} chars)" : "";
            return Path.GetFileName(fp.GetString()!) + size;
        }
        return Truncate(input.GetRawText(), 120);
    }

    private static string Truncate(string s, int max) => s.Length <= max ? s : s[..max] + "…";
}
