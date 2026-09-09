using StoryPlanner.AgentRunner;
using StoryPlanner.BatchFiles;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The runner's own check (decisions.md, "the smoke test is the runner's own check and leaves
/// the record for the runner's tests"): one batch of one item in a temporary folder, through
/// the real CLI from a launch folder outside any repository, proving the round trip — the
/// directions as the system prompt, the item on stdin, the JSON Schema enforced, the result
/// rendered, the calls file written. It spends a call, so it runs only when
/// <c>STORYPLAN_RUNNER_SMOKE</c> is set; otherwise it returns after saying so in the test
/// output (xunit 2 cannot skip at runtime), which is why its name says what it proves.
/// </summary>
public class SmokeTest(Xunit.Abstractions.ITestOutputHelper output)
{
    [Fact]
    public async Task One_item_round_trips_through_the_real_cli_when_STORYPLAN_RUNNER_SMOKE_is_set()
    {
        if (Environment.GetEnvironmentVariable("STORYPLAN_RUNNER_SMOKE") is null)
        {
            output.WriteLine("not run: set STORYPLAN_RUNNER_SMOKE=1 to run the smoke test through the real CLI");
            return;
        }
        using var t = new TempBatch(batch: "01-smoke");
        t.WriteItems(1, model: "sonnet", effort: "low");
        File.WriteAllText(Path.Combine(t.BatchDir, "items", "item-01.md"), "The sky was blue and the note said so. It shows a.\n");

        var log = new List<string>();
        var (runner, error) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, "item-01", t.LaunchDir,
            new ProcessChildLauncher(log.Add), new OpenGate { IdleLimit = TimeSpan.FromMinutes(5) }, log.Add, "smoke");
        Assert.Null(error);
        await runner!.RunAsync(CancellationToken.None);

        var calls = CallsFile.Read(Path.Combine(t.BatchDir, "calls.md"));
        var call = Assert.Single(calls.Entries);
        Assert.True(call.Succeeded, string.Join("\n", log) + "\n" + call.Check);
        Assert.True(call.Pilot);
        var result = File.ReadAllText(Path.Combine(t.BatchDir, "results", "item-01.md"));
        var (answer, problems) = ResultFile.Parse(runner.Batch.Directions, result);
        Assert.Empty(problems);
        Assert.Contains(answer["class"]!.ToString(), new[] { "a", "b", "cannot-place" });
        var stream = File.ReadAllText(Path.Combine(t.BatchDir, "attempts", "item-01", "call-1", "stream.jsonl"));
        Assert.Contains("\"structured_output\"", stream);
        Assert.DoesNotContain("mcp_servers\":[{", stream);
    }
}
