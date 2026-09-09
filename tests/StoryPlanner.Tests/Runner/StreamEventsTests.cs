using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The stream reader turns the child's <c>stream-json</c> lines into what the page shows:
/// init, text, tool call, tool result, thinking, usage, done — and never throws on a partial
/// line. The harness-2.1.258 fixture lines are copied from the 2026-09-05 audit run's pilot
/// stream (<c>arm-A-22-the-referee</c>), the fourth shape the child's output has had. Tier: pure.
/// </summary>
public class StreamEventsTests
{
    // Verbatim from the 2026-09-05 audit's stream, now under docs/v3-framework-historical/skill-audits/2026-09-05-v3-buildout-2/ (harness 2.1.258).
    const string Init258 = """{"type":"system","subtype":"init","cwd":"C:\\Users\\Brian\\RiderProjects\\StoryPlanner-fanout","session_id":"9fccd71b-e207-49f4-9af7-9b5ab29b65b4","tools":["Write"],"mcp_servers":[],"model":"claude-sonnet-5","permissionMode":"auto","slash_commands":[],"apiKeySource":"none","claude_code_version":"2.1.258","output_style":"default","agents":["claude","claude-code-guide","Explore","general-purpose","Plan","statusline-setup"],"skills":[],"plugins":[],"capabilities":["interrupt_receipt_v1","interrupt_cancel_queued_v1","msg_lifecycle_v1"],"analytics_disabled":false,"product_feedback_disabled":false,"uuid":"94a45f7c-c5a1-4649-9385-e50ed27854d0","messaging_socket_path":"\\\\.\\pipe\\LOCAL\\cc-msg-79dafe59e3442789c258925512ede091","fast_mode_state":"off","fast_mode_disabled_reason":"extra_usage_disabled","powershell_path":"C:\\WINDOWS\\System32\\WindowsPowerShell\\v1.0\\powershell.exe"}""";
    const string Thinking258 = """{"type":"system","subtype":"thinking_tokens","estimated_tokens":50,"estimated_tokens_delta":50,"session_id":"9fccd71b-e207-49f4-9af7-9b5ab29b65b4","uuid":"aa6a6f1c-3e09-456f-8b2c-bffb711bd1d6"}""";
    const string RateLimit258 = """{"type":"rate_limit_event","rate_limit_info":{"status":"allowed","resetsAt":1788624000,"rateLimitType":"five_hour","overageStatus":"rejected","overageDisabledReason":"org_level_disabled","isUsingOverage":false,"unifiedWindows":{"five_hour":{"utilization":0.49,"resetsAt":1788624000},"seven_day":{"utilization":0.18,"resetsAt":1788789600}}},"uuid":"6b8bc7ac-c456-4663-a8ff-a23c170743d4","session_id":"9fccd71b-e207-49f4-9af7-9b5ab29b65b4"}""";
    // From a batch job of the same run: the harness retrying a 429.
    const string ApiRetry258 = """{"type":"system","subtype":"api_retry","attempt":1,"max_retries":10,"retry_delay_ms":517,"error_status":429,"error":"rate_limit","session_id":"9d75b1de-bb04-4f18-aeef-13d7f3d5ea02","uuid":"5f5210c5-4108-425e-8782-aac5fcbce616"}""";

    [Fact]
    public void Parses_each_event_kind_into_a_one_line_reading()
    {
        var init = StreamEvents.Parse("""{"type":"system","subtype":"init","tools":["Write"],"mcp_servers":[],"model":"claude-sonnet-5"}""");
        Assert.Equal("init", init.Kind);
        Assert.Contains("tools [Write]", init.Text);
        Assert.Contains("mcp servers 0", init.Text);

        var text = StreamEvents.Parse("""{"type":"assistant","message":{"content":[{"type":"text","text":"Reading the unit."}]}}""");
        Assert.Equal("text", text.Kind);
        Assert.Equal("Reading the unit.", text.Text);

        var tool = StreamEvents.Parse("""{"type":"assistant","message":{"content":[{"type":"tool_use","name":"Write","input":{"file_path":"C:/x/results/arm-A.md","content":"## unit-001\n"}}]}}""");
        Assert.Equal("tool", tool.Kind);
        Assert.Equal("Write arm-A.md (12 chars)", tool.Text);

        var result = StreamEvents.Parse("""{"type":"user","message":{"content":[{"type":"tool_result","content":"File written"}]}}""");
        Assert.Equal("result", result.Kind);
        Assert.Contains("12 chars", result.Text);

        var done = StreamEvents.Parse("""{"type":"result","total_cost_usd":0.21,"num_turns":2,"result":"Wrote the file."}""");
        Assert.Equal("done", done.Kind);
        Assert.Contains("$0.210", done.Text);
        Assert.Contains("2 turn(s)", done.Text);
        Assert.Contains("Wrote the file.", done.Text);
    }

    /// <summary>
    /// Harness 2.1.258: <c>system</c> carries subtypes, and only <c>init</c> is the init event.
    /// The page once showed 55 "init" lines for one job (2026-09-05) because the parser never
    /// looked. The kind label is the page's; the text does not repeat it ("initinit").
    /// </summary>
    [Fact]
    public void System_events_are_read_by_subtype_and_a_rate_limit_event_is_a_usage_reading()
    {
        var init = StreamEvents.Parse(Init258);
        Assert.Equal("init", init.Kind);
        Assert.Equal("model claude-sonnet-5; tools [Write]; mcp servers 0", init.Text);

        var thinking = StreamEvents.Parse(Thinking258);
        Assert.Equal("thinking", thinking.Kind);
        Assert.Contains("50 tokens", thinking.Text);
        Assert.DoesNotContain("init", thinking.Text);

        var retry = StreamEvents.Parse(ApiRetry258);
        Assert.Equal("system", retry.Kind);
        Assert.StartsWith("api_retry:", retry.Text);
        Assert.Contains("error_status=429", retry.Text);
        Assert.DoesNotContain("session_id", retry.Text);

        var usage = StreamEvents.Parse(RateLimit258);
        Assert.Equal("usage", usage.Kind);
        Assert.Equal("five-hour 49%, seven-day 18%", usage.Text);

        var done = StreamEvents.Parse("""{"type":"result","total_cost_usd":0.392869,"num_turns":2,"result":"Wrote the audit."}""");
        Assert.StartsWith("$0.393", done.Text);
    }

    [Fact]
    public void Consecutive_thinking_steps_collapse_to_one_line_carrying_the_latest_total_and_the_step_count()
    {
        var path = Path.Combine(Path.GetTempPath(), "sp-stream-" + Guid.NewGuid().ToString("N") + ".jsonl");
        try
        {
            File.WriteAllLines(path,
            [
                Init258,
                RateLimit258,
                Thinking258,
                """{"type":"system","subtype":"thinking_tokens","estimated_tokens":120,"estimated_tokens_delta":70}""",
                """{"type":"system","subtype":"thinking_tokens","estimated_tokens":6606,"estimated_tokens_delta":6486}""",
                """{"type":"assistant","message":{"content":[{"type":"text","text":"Writing."}]}}""",
                """{"type":"system","subtype":"thinking_tokens","estimated_tokens":6650,"estimated_tokens_delta":44}""",
                """{"type":"result","total_cost_usd":0.39,"num_turns":2,"result":"done"}""",
            ]);
            var tail = StreamEvents.ReadTail(path, 100);
            Assert.Equal(["init", "usage", "thinking", "text", "thinking", "done"], tail.Select(e => e.Kind));
            Assert.Equal("~6,606 tokens so far (estimated), 3 steps", tail[2].Text);
            Assert.Equal("~6,650 tokens so far (estimated)", tail[4].Text);     // a single step carries no count

            // The tail is the last N of the collapsed events, not of the raw lines.
            Assert.Equal(["thinking", "done"], StreamEvents.ReadTail(path, 2).Select(e => e.Kind));
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void The_result_event_yields_the_totals_and_the_structured_answer_in_every_shape_the_child_has_had()
    {
        const string stream = """
            [
              {"type":"system","subtype":"init","cwd":"X:/launch","tools":[],"mcp_servers":[]},
              {"type":"assistant","message":{"role":"assistant","content":[{"type":"text","text":"working"}]}},
              {"type":"result","subtype":"success","total_cost_usd":0.0674,"num_turns":2,"session_id":"55f2","result":"{\"class\":\"a\"}","structured_output":{"class":"a","why":"1"}}
            ]
            """;
        var s = StreamEvents.ParseResult(stream);
        Assert.Equal(0.0674, s.CostUsd);
        Assert.Equal(2, s.Turns);
        Assert.Equal("55f2", s.SessionId);
        Assert.Equal("a", s.StructuredOutput!["class"]!.ToString());

        // stream-json lines, the result object's type key late, no structured_output: the reply text that parses as JSON is taken.
        const string lines = "{\"type\":\"system\",\"subtype\":\"init\"}\n"
            + "{\"duration_api_ms\":79047,\"session_id\":\"ab\",\"total_cost_usd\":0.21,\"num_turns\":2,\"subtype\":\"success\",\"result\":\"{\\\"class\\\":\\\"b\\\",\\\"why\\\":\\\"2\\\"}\",\"type\":\"result\",\"is_error\":false}\n";
        var streamed = StreamEvents.ParseResult(lines);
        Assert.Equal(0.21, streamed.CostUsd);
        Assert.Equal("b", streamed.StructuredOutput!["class"]!.ToString());
        Assert.False(streamed.IsError);

        var prose = StreamEvents.ParseResult("""{"type":"result","total_cost_usd":1.5,"num_turns":1,"session_id":"s","result":"Wrote the file."}""");
        Assert.Equal(1.5, prose.CostUsd);
        Assert.Null(prose.StructuredOutput);

        var none = StreamEvents.ParseResult("not json at all");
        Assert.Null(none.CostUsd);
        Assert.Null(none.StructuredOutput);
    }

    [Fact]
    public void A_partial_or_foreign_line_comes_back_raw_and_the_tail_reads_the_last_n()
    {
        var partial = StreamEvents.Parse("""{"type":"assistant","message":{"content":[{"type":"te""");
        Assert.Equal("raw", partial.Kind);

        var path = Path.Combine(Path.GetTempPath(), "sp-stream-" + Guid.NewGuid().ToString("N") + ".jsonl");
        try
        {
            File.WriteAllLines(path, Enumerable.Range(1, 10).Select(i => "{\"type\":\"assistant\",\"message\":{\"content\":[{\"type\":\"text\",\"text\":\"line " + i + "\"}]}}"));
            var tail = StreamEvents.ReadTail(path, 3);
            Assert.Equal(["line 8", "line 9", "line 10"], tail.Select(e => e.Text));
            Assert.Empty(StreamEvents.ReadTail(path + ".missing", 3));
        }
        finally { File.Delete(path); }
    }
}
