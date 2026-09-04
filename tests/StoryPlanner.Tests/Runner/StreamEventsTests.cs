using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The stream reader turns the child's <c>stream-json</c> lines into what the page shows:
/// init, text, tool call, tool result, done — and never throws on a partial line. Tier: pure.
/// </summary>
public class StreamEventsTests
{
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
