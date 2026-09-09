using System.Text.Json;
using System.Text.Json.Nodes;

namespace StoryPlanner.AgentRunner;

/// <summary>One line of <c>stream.jsonl</c> as the page shows it: what kind of event, a one-line reading, and the raw line.</summary>
public sealed record StreamEvent(string Kind, string Text, string Raw);

/// <summary>What the result event of a call carries: the totals, the session, the reply text, and the structured answer the CLI validated against the batch's JSON Schema.</summary>
public sealed record ResultSummary(double? CostUsd, int? Turns, string? SessionId, string? ResultText, JsonObject? StructuredOutput, bool IsError)
{
    public static readonly ResultSummary Empty = new(null, null, null, null, null, false);
}

/// <summary>
/// Reads the child's <c>stream-json</c> events into something a person can follow: the
/// agent's text, each tool call with a short account of its input, each tool result's size,
/// the init event's tool list, the thinking-token counter, the harness's rate-limit reading,
/// and the final result with cost. Never throws on a partial or foreign line — it comes back
/// as <c>raw</c>. The same reader serves a live tail and a finished attempt; there is one code
/// path.
///
/// The <c>system</c> type carries subtypes (harness 2.1.258: <c>init</c> once,
/// <c>thinking_tokens</c> per thinking step, <c>api_retry</c> per 429), so it is read by
/// subtype; the 2026-09-05 audit run showed every one of them as "init" until it was. The
/// <c>stream_event</c> type is the partial-message delta the call asks for so the answer is
/// never written in silence; a run of them is one <c>writing</c> line. The kind label is the
/// page's; the text never repeats it.
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
                    return ParseSystem(root, line);
                case "rate_limit_event":
                    return ParseRateLimit(root, line);
                case "stream_event":
                    return ParseStreamEvent(root, line);
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
                    return new StreamEvent("done", $"{(cost is { } cc ? $"${cc:F3}" : "cost ?")}, {turns?.ToString() ?? "?"} turn(s). {reply}".TrimEnd(), line);
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

    private static StreamEvent ParseSystem(JsonElement root, string line)
    {
        var subtype = root.TryGetProperty("subtype", out var st) ? st.GetString() ?? "" : "";
        switch (subtype)
        {
            case "init":
            {
                var tools = root.TryGetProperty("tools", out var ts) && ts.ValueKind == JsonValueKind.Array
                    ? string.Join(", ", ts.EnumerateArray().Select(e => e.GetString())) : "";
                var model = root.TryGetProperty("model", out var m) ? m.GetString() : null;
                var mcp = root.TryGetProperty("mcp_servers", out var ms) && ms.ValueKind == JsonValueKind.Array ? ms.GetArrayLength() : 0;
                return new StreamEvent("init", $"model {model}; tools [{tools}]; mcp servers {mcp}", line);
            }
            case "thinking_tokens":
            {
                // estimated_tokens is the running total; the delta is the step. One event per step,
                // collapsed to the latest total by ReadTail so a long think is one line, not fifty.
                var total = root.TryGetProperty("estimated_tokens", out var e) && e.ValueKind == JsonValueKind.Number ? e.GetInt64() : (long?)null;
                return new StreamEvent("thinking", total is { } tt ? $"~{tt:N0} tokens so far (estimated)" : "(estimate missing)", line);
            }
            case "":
                return new StreamEvent("system", Truncate(line, 200), line);
            default:
            {
                // Any other subtype: named, with its fields raw, so a new harness event is seen
                // for what it is rather than mistaken for something the reader knows.
                var fields = root.EnumerateObject()
                    .Where(p => p.Name is not ("type" or "subtype" or "session_id" or "uuid"))
                    .Select(p => $"{p.Name}={p.Value.GetRawText()}");
                return new StreamEvent("system", Truncate($"{subtype}: {string.Join(" ", fields)}", 200), line);
            }
        }
    }

    /// <summary>
    /// A partial-message line (<c>--include-partial-messages</c>): the API's own stream events,
    /// one per delta while the model writes. They exist so the idle limit sees the answer being
    /// written; the page shows a run of them as one <c>writing</c> line. The delta's kind
    /// (thinking, text, the answer's JSON) is the text; the message envelope lines carry theirs.
    /// </summary>
    private static StreamEvent ParseStreamEvent(JsonElement root, string line)
    {
        if (!root.TryGetProperty("event", out var ev) || ev.ValueKind != JsonValueKind.Object)
            return new StreamEvent("writing", "", line);
        var type = ev.TryGetProperty("type", out var t) ? t.GetString() ?? "" : "";
        if (type == "content_block_delta" && ev.TryGetProperty("delta", out var delta) && delta.ValueKind == JsonValueKind.Object)
        {
            var kind = delta.TryGetProperty("type", out var dt) ? dt.GetString() ?? "" : "";
            return new StreamEvent("writing", kind switch
            {
                "thinking_delta" => "thinking",
                "text_delta" => "text",
                "input_json_delta" => "the answer",
                "signature_delta" => "",
                _ => kind,
            }, line);
        }
        return new StreamEvent("writing", "", line);
    }

    /// <summary>
    /// The harness's own reading of the subscription windows at that moment — the one live
    /// figure the usage bar cannot get, which reads only what Claude Code last cached.
    /// </summary>
    private static StreamEvent ParseRateLimit(JsonElement root, string line)
    {
        if (!root.TryGetProperty("rate_limit_info", out var info) || info.ValueKind != JsonValueKind.Object)
            return new StreamEvent("usage", Truncate(line, 200), line);
        var parts = new List<string>();
        if (info.TryGetProperty("unifiedWindows", out var windows) && windows.ValueKind == JsonValueKind.Object)
        {
            if (Percent(windows, "five_hour") is { } five) parts.Add($"five-hour {five}%");
            if (Percent(windows, "seven_day") is { } seven) parts.Add($"seven-day {seven}%");
        }
        var status = info.TryGetProperty("status", out var s) && s.ValueKind == JsonValueKind.String ? s.GetString() : null;
        if (status is not null && status != "allowed") parts.Add(status);
        return new StreamEvent("usage", parts.Count > 0 ? string.Join(", ", parts) : Truncate(line, 200), line);
    }

    private static int? Percent(JsonElement windows, string name)
    {
        if (!windows.TryGetProperty(name, out var w) || w.ValueKind != JsonValueKind.Object) return null;
        if (!w.TryGetProperty("utilization", out var u) || u.ValueKind != JsonValueKind.Number) return null;
        return (int)Math.Round(u.GetDouble() * 100);
    }

    /// <summary>
    /// The last <paramref name="maxEvents"/> events of a stream file, consecutive thinking
    /// steps and consecutive writing deltas each collapsed to one line first; empty when the
    /// file does not exist yet.
    /// </summary>
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
        var events = Collapse(lines.Select(Parse));
        return events.Skip(Math.Max(0, events.Count - maxEvents)).ToList();
    }

    /// <summary>
    /// A run of consecutive <c>thinking</c> events becomes the last one, which carries the
    /// running total, with the step count appended. A run of consecutive <c>writing</c> events
    /// (the partial-message deltas, a thousand for one answer) becomes one line naming what was
    /// written and how many deltas it took. Every other event passes through.
    /// </summary>
    public static IReadOnlyList<StreamEvent> Collapse(IEnumerable<StreamEvent> events)
    {
        var result = new List<StreamEvent>();
        StreamEvent? pendingThinking = null;
        var steps = 0;
        StreamEvent? pendingWriting = null;
        var deltas = 0;
        var written = new List<string>();
        void FlushThinking()
        {
            if (pendingThinking is null) return;
            result.Add(steps > 1 ? pendingThinking with { Text = $"{pendingThinking.Text}, {steps} steps" } : pendingThinking);
            pendingThinking = null;
            steps = 0;
        }
        void FlushWriting()
        {
            if (pendingWriting is null) return;
            var what = written.Count > 0 ? string.Join(", ", written) : "(message envelope)";
            result.Add(pendingWriting with { Text = $"{what} — {deltas:N0} delta(s)" });
            pendingWriting = null;
            deltas = 0;
            written.Clear();
        }
        foreach (var e in events)
        {
            if (e.Kind == "thinking") { FlushWriting(); pendingThinking = e; steps++; continue; }
            if (e.Kind == "writing")
            {
                FlushThinking();
                pendingWriting = e;
                deltas++;
                if (e.Text.Length > 0 && !written.Contains(e.Text)) written.Add(e.Text);
                continue;
            }
            FlushThinking();
            FlushWriting();
            result.Add(e);
        }
        FlushThinking();
        FlushWriting();
        return result;
    }

    /// <summary>
    /// Cost, turn count, session id, the reply and the structured answer from the child's
    /// output: a <c>stream-json</c> file (one event per line — the live form), a <c>json</c>
    /// array of events, or a single result object. The <c>result</c> event carries the totals
    /// and, under <c>--json-schema</c>, the answer as <c>structured_output</c>; when it is
    /// absent but the reply text parses as a JSON object, that object is taken. Nulls when
    /// anything is missing — never throws.
    /// </summary>
    public static ResultSummary ParseResult(string text)
    {
        JsonElement? result = null;
        try
        {
            using var doc = JsonDocument.Parse(text);
            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in doc.RootElement.EnumerateArray())
                    if (IsResultEvent(el)) result = el.Clone();
            }
            else if (doc.RootElement.ValueKind == JsonValueKind.Object)
            {
                result = doc.RootElement.Clone();
            }
        }
        catch (JsonException)
        {
            foreach (var line in text.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                try
                {
                    using var doc = JsonDocument.Parse(line);
                    if (IsResultEvent(doc.RootElement)) result = doc.RootElement.Clone();
                }
                catch (JsonException) { /* a partial or non-JSON line — skip it */ }
            }
        }
        if (result is null) return ResultSummary.Empty;
        var r = result.Value;
        double? cost = r.TryGetProperty("total_cost_usd", out var c) && c.ValueKind == JsonValueKind.Number ? c.GetDouble() : null;
        int? turns = r.TryGetProperty("num_turns", out var n) && n.ValueKind == JsonValueKind.Number ? n.GetInt32() : null;
        string? session = r.TryGetProperty("session_id", out var s) && s.ValueKind == JsonValueKind.String ? s.GetString() : null;
        string? reply = r.TryGetProperty("result", out var rt) && rt.ValueKind == JsonValueKind.String ? rt.GetString() : null;
        var isError = r.TryGetProperty("is_error", out var ie) && ie.ValueKind == JsonValueKind.True;
        JsonObject? structured = null;
        if (r.TryGetProperty("structured_output", out var so) && so.ValueKind == JsonValueKind.Object)
            structured = JsonNode.Parse(so.GetRawText()) as JsonObject;
        else if (reply is not null)
        {
            try { structured = JsonNode.Parse(reply) as JsonObject; } catch (JsonException) { }
        }
        return new ResultSummary(cost, turns, session, reply, structured, isError);
    }

    private static bool IsResultEvent(JsonElement el) =>
        el.ValueKind == JsonValueKind.Object && el.TryGetProperty("type", out var t) && t.GetString() == "result";

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
