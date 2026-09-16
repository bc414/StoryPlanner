using StoryPlanner.AgentRunner;
using StoryPlanner.BatchFiles;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure-tier tests for the batch loop with a launcher that starts no process: one call per
/// item without a result, the result rendered from the structured answer, the calls file
/// appended with its head, a later execution calling only what failed, random order, the
/// launch gate, pause/resume, stop-after-in-flight, cancel, the queue jump, and the launch
/// folder's invariants. Tier: pure (temp folders).
/// </summary>
public class BatchRunnerTests
{
    private static BatchRunner Make(TempBatch t, FakeLauncher launcher, ILaunchGate? gate = null, bool random = false)
    {
        var (runner, error) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, t.LaunchDir, launcher, gate ?? new OpenGate(), _ => { }, "test", random);
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
        Assert.All(calls.Entries, c => { Assert.True(c.Succeeded); Assert.Equal(1, c.Call); Assert.Equal("high", c.Effort); Assert.Equal(0.01, c.Cost); Assert.Equal(64, c.DirectionsHash.Length); });
        Assert.Equal("- class: a\n- why: 1\n", File.ReadAllText(Path.Combine(t.BatchDir, "results", "item-02.md")));
        Assert.True(File.Exists(Path.Combine(t.BatchDir, "attempts", "item-01", "call-1", "system-prompt.md")));
        Assert.Equal(runner.Batch.Directions.Body, File.ReadAllText(Path.Combine(t.BatchDir, "attempts", "item-01", "call-1", "system-prompt.md")));

        // The call: the item's text on stdin, the directions as a system-prompt file, the schema, no tools, no transcript.
        var req = launcher.Requests.Single(r => r.Item == "item-01");
        Assert.Equal("The note 1.\n", req.Stdin);
        Assert.Contains("--system-prompt-file", req.Args);
        Assert.Contains("--json-schema", req.Args);
        Assert.Contains("--no-session-persistence", req.Args);
        Assert.Contains("--include-partial-messages", req.Args);     // the answer is never written in silence (idle limit)
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
    public async Task Random_order_walks_one_shuffle_of_the_index_and_still_calls_every_item_once()
    {
        using var t = new TempBatch();
        t.WriteItems(20);
        var launcher = new FakeLauncher { Delay = TimeSpan.FromMilliseconds(5) };
        var runner = Make(t, launcher, random: true);
        Assert.True(runner.RandomOrder);
        Assert.Equal("20 item(s) — 20 to call, 0 skipped as answered, random order", runner.Summary());
        Assert.Equal(runner.Batch.Items.Order(), runner.Order.Order());   // a permutation of the index …
        Assert.NotEqual(runner.Batch.Items, runner.Order);                 // … and not its order (20! to 1 against)

        await runner.RunAsync(CancellationToken.None);
        Assert.Equal(20, launcher.Launched);
        Assert.Equal(20, launcher.Order.Distinct().Count());
        var calls = CallsFile.Read(Path.Combine(t.BatchDir, "calls.md"));
        Assert.Equal(20, calls.Entries.Count);
        Assert.All(calls.Entries, c => Assert.True(c.Succeeded));

        // Without the flag the sequence is the index's.
        Assert.Equal(runner.Batch.Items, Make(t, new FakeLauncher()).Order);
    }

    [Fact]
    public async Task Under_a_ceiling_of_one_the_calls_follow_the_execution_order()
    {
        using var t = new TempBatch();
        t.WriteItems(3);
        var launcher = new FakeLauncher { Delay = TimeSpan.FromMilliseconds(10) };
        var runner = Make(t, launcher, new CeilingGate(1), random: true);
        await runner.RunAsync(CancellationToken.None);
        Assert.Equal(runner.Order, launcher.Order.ToList());
    }

    [Fact]
    public async Task A_queue_jump_calls_a_pending_item_now_past_the_gate_and_refuses_what_is_not_pending()
    {
        using var t = new TempBatch();
        t.WriteItems(4);
        var launcher = new FakeLauncher { Hold = new SemaphoreSlim(0), AnswerFor = r => r.Item == "item-01" ? null : """{"class":"a","why":"1"}""" };
        var gate = new CeilingGate(1);
        var runner = Make(t, launcher, gate);
        Assert.Equal("not executing", runner.CallNow("item-03"));               // the loop has not started
        var run = runner.RunAsync(CancellationToken.None);
        await Wait.Until(() => runner.InFlight == 1, what: "item-01 holds the one slot");

        Assert.True(runner.CanCallNow("item-03"));
        Assert.Null(runner.CallNow("item-03"));                                  // past the ceiling of one
        await Wait.Until(() => launcher.Launched == 2, what: "the jump launched");
        Assert.Equal(2, runner.InFlight);
        Assert.Equal(2, gate.InFlight);                                          // the slot is counted from then on
        Assert.Contains("already running", runner.CallNow("item-03"));
        Assert.Contains("not in the index", runner.CallNow("item-09"));

        launcher.Hold.Release(); launcher.Hold.Release();                        // item-01 fails, item-03 answers
        await Wait.Until(() => runner.HasSucceeded("item-03") && runner.CallsSnapshot().Any(c => c.Item == "item-01"), what: "both recorded");
        Assert.Contains("has answered", runner.CallNow("item-03"));
        Assert.Contains("already called in execution 1", runner.CallNow("item-01"));

        launcher.Hold.Release(); launcher.Hold.Release();                        // the loop finishes item-02 and item-04
        await run;
        Assert.Equal(4, launcher.Launched);
        var calls = CallsFile.Read(Path.Combine(t.BatchDir, "calls.md"));
        Assert.Equal(4, calls.Entries.Count);
        Assert.All(calls.Entries, c => Assert.Equal(1, c.Call));
        Assert.Equal("ok", calls.Entries.Single(c => c.Item == "item-03").Check);
        Assert.Equal(["item-01"], runner.Pending());
        Assert.Equal("not executing", runner.CallNow("item-01"));               // completed
    }

    [Fact]
    public async Task A_queue_jump_is_allowed_while_paused_and_refused_once_stop_is_requested()
    {
        using var t = new TempBatch();
        t.WriteItems(3);
        var launcher = new FakeLauncher { Hold = new SemaphoreSlim(0) };
        var runner = Make(t, launcher, new CeilingGate(1));
        var run = runner.RunAsync(CancellationToken.None);
        await Wait.Until(() => runner.InFlight == 1);

        runner.Pause();
        Assert.Null(runner.CallNow("item-02"));                                  // pause holds the loop, not the hand
        await Wait.Until(() => launcher.Launched == 2, what: "the jump under pause");
        runner.StopAfterInFlight();
        Assert.Contains("stop requested", runner.CallNow("item-03"));

        launcher.Hold.Release(); launcher.Hold.Release();
        await run;
        Assert.Equal(2, launcher.Launched);
        Assert.Equal(["item-03"], runner.Pending());
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
        var (runner, error) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, t.LaunchDir, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Null(runner);
        Assert.Contains("CLAUDE.md", error);
        File.Delete(Path.Combine(t.LaunchDir, "CLAUDE.md"));

        var inside = Path.Combine(t.WorkingDir, "launch-inside");
        Directory.CreateDirectory(inside);
        var (_, insideError) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, inside, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Contains("OUTSIDE", insideError);

        File.Delete(Path.Combine(t.BatchDir, "items", "item-02.md"));
        var (_, missing) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, t.LaunchDir, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Contains("item-02", missing);
    }

    [Fact]
    public void A_batch_composes_each_call_from_the_directions_body_and_the_item_text()
    {
        using var t = new TempBatch();
        t.WriteItems(1, kind: "full");
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
        Assert.Equal("", list[list.IndexOf("--tools") + 1]);
        Assert.DoesNotContain("--allowed-tools", list);
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

    /// <summary>
    /// d-2026-09-15-4: a launch the API refuses at the subscription limit is not a call. Nothing
    /// is recorded, the gate is told and holds, and the item is called again by the same
    /// execution once the gate reopens — the overnight run rides through the wall on its own.
    /// The 2026-09-15 failure this replaces: 1,665 refused launches recorded as failed calls.
    /// </summary>
    [Fact]
    public async Task A_launch_refused_at_the_limit_is_not_recorded_the_gate_holds_and_the_same_execution_calls_the_item_again()
    {
        using var t = new TempBatch();
        t.WriteItems(3);
        var refusals = 0;
        var launcher = new FakeLauncher { Delay = TimeSpan.FromMilliseconds(10), FiveHour = 0.48, RefuseFor = r => r.Item == "item-01" && Interlocked.Increment(ref refusals) == 1 };
        var gate = new CeilingGate(1);
        gate.OnObserve = reading => { if (reading.Rejected) gate.Open = false; };
        var log = new List<string>();
        var (runner, error) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, t.LaunchDir, launcher, gate, log.Add, "test");
        Assert.Null(error);
        var run = runner!.RunAsync(CancellationToken.None);

        await Wait.Until(() => !gate.Open, what: "the gate told of the refusal");
        await Task.Delay(300);
        Assert.False(runner.Completed);
        Assert.Equal(1, runner.Refused);
        Assert.Equal(1, launcher.Launched);                                      // held: nothing more launched
        Assert.False(File.Exists(Path.Combine(t.BatchDir, "calls.md")));        // the refusal is not a call
        Assert.Equal(["item-01", "item-02", "item-03"], runner.Pending());
        Assert.Contains(log, l => l.Contains("refused") && l.Contains("nothing recorded"));
        Assert.True(gate.Readings.Last().Rejected);

        gate.Open = true;                                                        // the reset passed
        await run;
        Assert.Equal(4, launcher.Launched);
        Assert.Equal(["item-01", "item-01", "item-02", "item-03"], launcher.Order);
        var calls = CallsFile.Read(Path.Combine(t.BatchDir, "calls.md"));
        Assert.Equal(3, calls.Entries.Count);
        Assert.All(calls.Entries, c => { Assert.True(c.Succeeded); Assert.Equal(1, c.Call); });
        Assert.True(runner.Completed);
        Assert.Contains(log, l => l.Contains("3 call(s) this execution, 1 launch(es) refused"));

        // Every ordinary call reported the five-hour figure to the gate as well.
        Assert.Equal(48, gate.Readings.Last().FiveHourPercent);
        Assert.False(gate.Readings.Last().Rejected);
        Assert.Equal(4, gate.Readings.Count);
    }

    private sealed class CeilingGate(int ceiling) : ILaunchGate
    {
        public volatile bool Open = true;
        public Action<RateLimitReading>? OnObserve;
        public readonly List<RateLimitReading> Readings = [];
        private int _inFlight;
        public int InFlight { get { lock (this) return _inFlight; } }
        public void Observe(RateLimitReading reading, string batchId, string item) { lock (Readings) Readings.Add(reading); OnObserve?.Invoke(reading); }
        public bool TryAcquire(BatchRunner batch)
        {
            if (!Open) return false;
            lock (this) { if (_inFlight >= ceiling) return false; _inFlight++; return true; }
        }
        public bool TryForce(BatchRunner batch) { lock (this) { _inFlight++; return true; } }
        public void Release(BatchRunner batch) { lock (this) _inFlight--; }
        public string? HoldReason(BatchRunner batch) => Open ? null : "closed";
        public TimeSpan IdleLimit => TimeSpan.FromMinutes(1);
    }
}
