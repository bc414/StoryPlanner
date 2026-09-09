using StoryPlanner.AgentRunner;
using StoryPlanner.BatchFiles;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure-tier tests for the batch loop with a launcher that starts no process: one call per
/// item without a result, the result rendered from the structured answer, the calls file
/// appended with its head, the pilot, a later execution calling only what failed, the launch
/// gate, pause/resume, stop-after-in-flight, cancel, and the launch folder's invariants.
/// Tier: pure (temp folders).
/// </summary>
public class BatchRunnerTests
{
    private static BatchRunner Make(TempBatch t, FakeLauncher launcher, ILaunchGate? gate = null, string? item = null)
    {
        var (runner, error) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, item, t.LaunchDir, launcher, gate ?? new OpenGate(), _ => { }, "test");
        Assert.Null(error);
        return runner!;
    }

    [Fact]
    public async Task Executes_every_item_renders_each_result_and_records_each_call()
    {
        using var t = new TempBatch();
        t.WriteItems(3, effort: "high");
        var launcher = new FakeLauncher { Delay = TimeSpan.FromMilliseconds(40) };
        var runner = Make(t, launcher);
        Assert.Equal(1, runner.Execution);
        Assert.Equal("3 item(s) — 3 to call, 0 skipped as answered", runner.Summary());

        await runner.RunAsync(CancellationToken.None);

        Assert.True(runner.Completed);
        Assert.Equal(3, launcher.Launched);
        var calls = CallsFile.Read(Path.Combine(t.BatchDir, "calls.md"));
        Assert.Equal(DefinitionFile.Read(t.DefinitionPath).Hash, calls.DefinitionHash);
        Assert.Equal(3, calls.Entries.Count);
        Assert.All(calls.Entries, c => { Assert.True(c.Succeeded); Assert.Equal(1, c.Call); Assert.Equal("high", c.Effort); Assert.False(c.Pilot); Assert.Equal(0.01, c.Cost); Assert.Equal(64, c.DirectionsHash.Length); });
        Assert.Equal("- class: a\n- why: 1\n", File.ReadAllText(Path.Combine(t.BatchDir, "results", "item-02.md")));
        Assert.True(File.Exists(Path.Combine(t.BatchDir, "attempts", "item-01", "call-1", "system-prompt.md")));
        Assert.Equal(runner.Batch.Directions.Body, File.ReadAllText(Path.Combine(t.BatchDir, "attempts", "item-01", "call-1", "system-prompt.md")));

        // The call: the item's text on stdin, the directions as a system-prompt file, the schema, no tools, no transcript.
        var req = launcher.Requests.Single(r => r.Item == "item-01");
        Assert.Equal("The note 1.\n", req.Stdin);
        Assert.Contains("--system-prompt-file", req.Args);
        Assert.Contains("--json-schema", req.Args);
        Assert.Contains("--no-session-persistence", req.Args);
        Assert.Contains("--restricted", req.Args);
        Assert.Equal("", req.Args[req.Args.ToList().IndexOf("--tools") + 1]);
        Assert.Equal("high", req.Args[req.Args.ToList().IndexOf("--effort") + 1]);
        Assert.Equal(t.LaunchDir, req.LaunchDir);
    }

    [Fact]
    public async Task A_call_with_no_structured_output_fails_and_a_later_execution_calls_only_that_item()
    {
        using var t = new TempBatch();
        t.WriteItems(3);
        var launcher = new FakeLauncher { AnswerFor = r => r.Item == "item-02" ? null : """{"class":"b","why":"1"}""" };
        await Make(t, launcher).RunAsync(CancellationToken.None);

        var calls = CallsFile.Read(Path.Combine(t.BatchDir, "calls.md"));
        Assert.Equal("no structured output", calls.Entries.Single(c => c.Item == "item-02").Check);
        Assert.False(File.Exists(Path.Combine(t.BatchDir, "results", "item-02.md")));

        var second = new FakeLauncher();
        var runner = Make(t, second);
        Assert.Equal(2, runner.Execution);
        Assert.Equal("3 item(s) — 1 to call, 2 skipped as answered", runner.Summary());
        await runner.RunAsync(CancellationToken.None);
        Assert.Equal(1, second.Launched);
        Assert.Equal("item-02", second.Order.Single());
        var after = CallsFile.Read(Path.Combine(t.BatchDir, "calls.md"));
        Assert.Equal(4, after.Entries.Count);
        Assert.Equal(2, after.Entries.Last().Call);
        Assert.True(after.HasSucceeded("item-02"));
    }

    [Fact]
    public async Task An_answer_outside_the_classes_is_recorded_malformed_and_a_nonzero_exit_fails()
    {
        using var t = new TempBatch();
        t.WriteItems(2);
        var launcher = new FakeLauncher
        {
            AnswerFor = r => r.Item == "item-01" ? """{"class":"z","why":"1"}""" : """{"class":"a","why":"1"}""",
            ExitFor = r => r.Item == "item-02" ? 1 : 0,
        };
        await Make(t, launcher).RunAsync(CancellationToken.None);
        var calls = CallsFile.Read(Path.Combine(t.BatchDir, "calls.md"));
        Assert.StartsWith("malformed:", calls.Entries.Single(c => c.Item == "item-01").Check);
        Assert.Equal("exit 1", calls.Entries.Single(c => c.Item == "item-02").Check);
        Assert.All(calls.Entries, c => Assert.False(c.Succeeded));
    }

    [Fact]
    public async Task Naming_an_item_calls_it_alone_and_marks_the_call_as_the_pilot()
    {
        using var t = new TempBatch();
        t.WriteItems(3);
        var launcher = new FakeLauncher();
        var runner = Make(t, launcher, item: "item-02");
        Assert.Equal("1 item(s) — 1 to call, 0 skipped as answered (pilot)", runner.Summary());
        await runner.RunAsync(CancellationToken.None);
        var call = Assert.Single(CallsFile.Read(Path.Combine(t.BatchDir, "calls.md")).Entries);
        Assert.Equal("item-02", call.Item);
        Assert.True(call.Pilot);
        Assert.Equal(1, launcher.Launched);
    }

    [Fact]
    public async Task Pause_holds_new_launches_and_resume_continues()
    {
        using var t = new TempBatch();
        t.WriteItems(3);
        var launcher = new FakeLauncher { Hold = new SemaphoreSlim(0) };
        var gate = new CeilingGate(1);
        var runner = Make(t, launcher, gate);
        var run = runner.RunAsync(CancellationToken.None);

        await Wait.Until(() => runner.InFlight == 1, what: "first launch");
        runner.Pause();
        launcher.Hold.Release();
        await Wait.Until(() => runner.InFlight == 0, what: "first child done");
        await Task.Delay(300);
        Assert.Equal(1, launcher.Launched);
        Assert.False(runner.Completed);

        runner.Resume();
        await Wait.Until(() => launcher.Launched == 2, what: "second launch after resume");
        launcher.Hold.Release(); launcher.Hold.Release();
        await run;
        Assert.Equal(3, launcher.Launched);
    }

    [Fact]
    public async Task Stop_after_in_flight_finishes_running_calls_and_leaves_the_rest_pending()
    {
        using var t = new TempBatch();
        t.WriteItems(4);
        var launcher = new FakeLauncher { Hold = new SemaphoreSlim(0) };
        var runner = Make(t, launcher, new CeilingGate(1));
        var run = runner.RunAsync(CancellationToken.None);

        await Wait.Until(() => runner.InFlight == 1);
        runner.StopAfterInFlight();
        launcher.Hold.Release();
        await run;

        Assert.Equal(1, launcher.Launched);
        Assert.Single(CallsFile.Read(Path.Combine(t.BatchDir, "calls.md")).Entries);
        Assert.Equal(3, runner.Pending().Count);
    }

    [Fact]
    public async Task Cancel_kills_the_child_and_records_a_cancelled_call()
    {
        using var t = new TempBatch();
        t.WriteItems(1);
        var launcher = new FakeLauncher { Hold = new SemaphoreSlim(0) };
        var runner = Make(t, launcher);
        var run = runner.RunAsync(CancellationToken.None);

        await Wait.Until(() => runner.RunningSnapshot().Any(r => r.Handle is not null));
        Assert.True(runner.Cancel("item-01"));
        await run;

        var call = Assert.Single(CallsFile.Read(Path.Combine(t.BatchDir, "calls.md")).Entries);
        Assert.Equal(-4, call.Exit);
        Assert.Equal("cancelled", call.Check);
        Assert.False(call.Succeeded);
    }

    [Fact]
    public async Task The_launch_gate_holds_every_launch_until_it_opens_and_bounds_the_calls_in_flight()
    {
        using var t = new TempBatch();
        t.WriteItems(4);
        var launcher = new FakeLauncher { Delay = TimeSpan.FromMilliseconds(60) };
        var gate = new CeilingGate(2) { Open = false };
        var runner = Make(t, launcher, gate);
        var run = runner.RunAsync(CancellationToken.None);

        await Task.Delay(300);
        Assert.Equal(0, launcher.Launched);
        gate.Open = true;
        await run;
        Assert.Equal(4, launcher.Launched);
        Assert.Equal(2, launcher.MaxConcurrent);
    }

    [Fact]
    public void Create_refuses_a_launch_folder_inside_the_repo_or_carrying_an_instruction_stack_and_a_batch_missing_bodies()
    {
        using var t = new TempBatch();
        t.WriteItems(2);
        File.WriteAllText(Path.Combine(t.LaunchDir, "CLAUDE.md"), "x");
        var (runner, error) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, null, t.LaunchDir, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Null(runner);
        Assert.Contains("CLAUDE.md", error);
        File.Delete(Path.Combine(t.LaunchDir, "CLAUDE.md"));

        var inside = Path.Combine(t.WorkingDir, "launch-inside");
        Directory.CreateDirectory(inside);
        var (_, insideError) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, null, inside, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Contains("OUTSIDE", insideError);

        var (_, unknown) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, "item-09", t.LaunchDir, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Contains("item-09", unknown);

        File.Delete(Path.Combine(t.BatchDir, "items", "item-02.md"));
        var (_, missing) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, null, t.LaunchDir, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Contains("item-02", missing);
    }

    [Fact]
    public void A_batch_composes_each_call_from_the_directions_body_and_the_item_text()
    {
        using var t = new TempBatch();
        t.WriteItems(1, kind: "full", extra: "- tools:\n  - Read\n");
        var (batch, error) = Batch.Load(t.DefinitionPath, t.WorkingDir);
        Assert.Null(error);
        var plan = batch!.Compose("item-01");
        Assert.Equal(batch.Directions.Body, plan.SystemPrompt);
        Assert.Equal("The note 1.\n", plan.ItemText);
        Assert.Equal(Hashing.Sha256Hex("The note 1.\n"), plan.ItemHash);
        Assert.Equal(64, plan.PromptHash.Length);
        Assert.Contains("\"enum\":[\"a\",\"b\",\"cannot-place\"]", plan.SchemaJson);
        Assert.Equal("docs/v3-framework/studies/verification-of-v1-archive-test/batches/01-full", batch.Id);

        var args = batch.BuildArgs("C:/x/system-prompt.md");
        var list = args.ToList();
        Assert.Equal("sonnet", list[list.IndexOf("--model") + 1]);
        Assert.Equal("Read", list[list.IndexOf("--tools") + 1]);
        Assert.Contains("--allowed-tools", list);
        Assert.DoesNotContain("--mcp-config", list);
        Assert.DoesNotContain("--add-dir", list);
        Assert.Contains("--strict-mcp-config", list);
    }

    [Fact]
    public void Load_names_what_is_missing()
    {
        using var t = new TempBatch();
        Assert.Contains("No definition", Batch.Load(t.DefinitionPath, t.WorkingDir).Error);
        t.WriteItems(1);
        File.Delete(Path.Combine(t.BatchDir, "index.md"));
        Assert.Contains("No index", Batch.Load(t.DefinitionPath, t.WorkingDir).Error);
        t.WriteItems(1);
        File.WriteAllText(t.DefinitionPath, "# 01-full — definition\n\n- directions: ../../directions-9.md\n- model: sonnet\n");
        Assert.Contains("directions line resolves to no file", Batch.Load(t.DefinitionPath, t.WorkingDir).Error);
    }

    private sealed class CeilingGate(int ceiling) : ILaunchGate
    {
        public volatile bool Open = true;
        private int _inFlight;
        public bool TryAcquire(BatchRunner batch)
        {
            if (!Open) return false;
            lock (this) { if (_inFlight >= ceiling) return false; _inFlight++; return true; }
        }
        public void Release(BatchRunner batch) { lock (this) _inFlight--; }
        public string? HoldReason(BatchRunner batch) => Open ? null : "closed";
        public TimeSpan IdleLimit => TimeSpan.FromMinutes(1);
    }
}
