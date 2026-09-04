using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure-tier tests for the batch loop with a launcher that starts no process: the run's own
/// parallel ceiling, the launch gate, pause/resume, stop-after-in-flight, cancel, the ledger
/// rows written, the pilot mark, and re-launch skipping succeeded jobs. Tier: pure (temp folders).
/// </summary>
public class BatchRunnerTests
{
    private static BatchRunner Make(TempRun t, FakeLauncher launcher, ILaunchGate? gate = null, string? jobFilter = null)
    {
        var (runner, error) = BatchRunner.Create(t.JobFilePath, "work/run-1", jobFilter, launcher, gate ?? new OpenGate(), _ => { }, "test");
        Assert.Null(error);
        return runner!;
    }

    [Fact]
    public async Task Runs_every_job_writes_ledger_rows_and_honours_the_runs_own_ceiling()
    {
        using var t = new TempRun();
        t.WriteJobs(5, maxParallel: 2);
        var launcher = new FakeLauncher { Delay = TimeSpan.FromMilliseconds(60) };
        var runner = Make(t, launcher);

        await runner.RunAsync(CancellationToken.None);

        Assert.True(runner.Completed);
        Assert.Equal(5, launcher.Launched);
        Assert.Equal(2, launcher.MaxConcurrent);
        var rows = RunnerPlan.ParseLedger(File.ReadLines(runner.LedgerPath));
        Assert.Equal(5, rows.Count);
        Assert.All(rows, r => { Assert.True(r.Succeeded); Assert.Equal("ok", r.OutputCheck); Assert.Null(r.Mode); Assert.Equal(0.01, r.CostUsd); });
        Assert.All(runner.Jobs, j => Assert.Equal(JobState.Succeeded, runner.StateOf(j)));
    }

    [Fact]
    public async Task A_failed_output_check_is_a_failed_attempt_and_a_relaunch_skips_succeeded_jobs()
    {
        using var t = new TempRun();
        t.WriteJobs(3);
        var launcher = new FakeLauncher { WriteOutput = r => r.Job.Id != "job-02" };
        await Make(t, launcher).RunAsync(CancellationToken.None);

        var rows = RunnerPlan.ParseLedger(File.ReadLines(t.RunDir + "/ledger.jsonl"));
        Assert.Equal("no output file", rows.Single(r => r.JobId == "job-02").OutputCheck);

        // Second enqueue on the same folder: only the failed job is pending — and at maxAttempts 1 it is FAILED, so nothing launches.
        var second = new FakeLauncher();
        var runner = Make(t, second);
        Assert.Equal(JobState.Failed, runner.StateOf(runner.Jobs[1]));
        await runner.RunAsync(CancellationToken.None);
        Assert.Equal(0, second.Launched);
    }

    [Fact]
    public async Task Pause_holds_new_launches_and_resume_continues()
    {
        using var t = new TempRun();
        t.WriteJobs(3);
        var launcher = new FakeLauncher { Hold = new SemaphoreSlim(0) };
        var runner = Make(t, launcher);
        var run = runner.RunAsync(CancellationToken.None);

        await Wait.Until(() => runner.InFlight == 1, what: "first launch");
        runner.Pause();
        launcher.Hold.Release();                       // first child finishes
        await Wait.Until(() => runner.InFlight == 0, what: "first child done");
        await Task.Delay(300);
        Assert.Equal(1, launcher.Launched);            // paused: nothing else launched
        Assert.False(runner.Completed);

        runner.Resume();
        await Wait.Until(() => launcher.Launched == 2, what: "second launch after resume");
        launcher.Hold.Release(); launcher.Hold.Release();
        await run;
        Assert.Equal(3, launcher.Launched);
    }

    [Fact]
    public async Task Stop_after_in_flight_finishes_running_children_and_leaves_the_rest_pending()
    {
        using var t = new TempRun();
        t.WriteJobs(4);
        var launcher = new FakeLauncher { Hold = new SemaphoreSlim(0) };
        var runner = Make(t, launcher);
        var run = runner.RunAsync(CancellationToken.None);

        await Wait.Until(() => runner.InFlight == 1);
        runner.StopAfterInFlight();
        launcher.Hold.Release();
        await run;

        Assert.Equal(1, launcher.Launched);
        Assert.Single(RunnerPlan.ParseLedger(File.ReadLines(runner.LedgerPath)));
        Assert.Equal(JobState.Pending, runner.StateOf(runner.Jobs[1]));   // no row written — still re-launchable
    }

    [Fact]
    public async Task Cancel_kills_the_child_and_records_a_cancelled_attempt()
    {
        using var t = new TempRun();
        t.WriteJobs(1);
        var launcher = new FakeLauncher { Hold = new SemaphoreSlim(0) };
        var runner = Make(t, launcher);
        var run = runner.RunAsync(CancellationToken.None);

        await Wait.Until(() => runner.RunningSnapshot().Any(r => r.Handle is not null));
        Assert.True(runner.Cancel("job-01"));
        await run;

        var row = Assert.Single(RunnerPlan.ParseLedger(File.ReadLines(runner.LedgerPath)));
        Assert.Equal(-4, row.ExitCode);
        Assert.Equal("cancelled", row.OutputCheck);
        Assert.False(row.Succeeded);
    }

    [Fact]
    public async Task The_launch_gate_holds_every_launch_until_it_opens()
    {
        using var t = new TempRun();
        t.WriteJobs(2, maxParallel: 2);
        var launcher = new FakeLauncher();
        var gate = new ToggleGate();
        var runner = Make(t, launcher, gate);
        var run = runner.RunAsync(CancellationToken.None);

        await Task.Delay(300);
        Assert.Equal(0, launcher.Launched);
        gate.Open = true;
        await run;
        Assert.Equal(2, launcher.Launched);
        Assert.Equal(2, gate.Acquired);
        Assert.Equal(2, gate.Released);
    }

    [Fact]
    public async Task A_job_filter_runs_one_job_and_marks_its_row_as_the_pilot()
    {
        using var t = new TempRun();
        t.WriteJobs(3);
        var launcher = new FakeLauncher();
        var runner = Make(t, launcher, jobFilter: "job-02");
        Assert.Single(runner.Jobs);

        await runner.RunAsync(CancellationToken.None);

        var row = Assert.Single(RunnerPlan.ParseLedger(File.ReadLines(runner.LedgerPath)));
        Assert.Equal("job-02", row.JobId);
        Assert.Equal("pilot", row.Mode);
    }

    [Fact]
    public void Create_refuses_a_launch_folder_inside_the_repo_or_carrying_an_instruction_stack()
    {
        using var t = new TempRun();
        t.WriteJobs(1);
        File.WriteAllText(Path.Combine(t.LaunchDir, "CLAUDE.md"), "x");
        var (runner, error) = BatchRunner.Create(t.JobFilePath, "w/r", null, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Null(runner);
        Assert.Contains("CLAUDE.md", error);
    }

    private sealed class ToggleGate : ILaunchGate
    {
        public volatile bool Open;
        public int Acquired, Released;
        public bool TryAcquire(BatchRunner run) { if (!Open) return false; Interlocked.Increment(ref Acquired); return true; }
        public void Release(BatchRunner run) => Interlocked.Increment(ref Released);
        public string? HoldReason(BatchRunner run) => Open ? null : "closed";
    }
}
