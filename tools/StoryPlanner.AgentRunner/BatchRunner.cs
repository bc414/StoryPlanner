namespace StoryPlanner.AgentRunner;

/// <summary>
/// Decides whether one more child may launch right now, across every batch the host runs:
/// the global parallel ceiling and the utilization cap live behind it. A batch acquires a
/// slot before each launch and releases it when the child exits.
/// </summary>
public interface ILaunchGate
{
    bool TryAcquire(BatchRunner run);
    void Release(BatchRunner run);
    /// <summary>Why a launch is being held, for the page; null when nothing holds it.</summary>
    string? HoldReason(BatchRunner run);
}

/// <summary>No global constraint — the batch's own ceiling is the only one. The CLI's serverless paths and tests use it.</summary>
public sealed class OpenGate : ILaunchGate
{
    public bool TryAcquire(BatchRunner run) => true;
    public void Release(BatchRunner run) { }
    public string? HoldReason(BatchRunner run) => null;
}

public sealed record RunningAttempt(string JobId, int Attempt, DateTimeOffset StartUtc, string StreamPath, IChildHandle? Handle);

/// <summary>
/// One batch: one run folder, its jobs, its ledger. The loop that used to be Program.cs,
/// now hostable — it raises events instead of printing, takes its launcher and its launch
/// gate from outside, and accepts harness commands (pause, resume, stop after in-flight,
/// cancel a job) while running. It never changes what a job is: model, protocol, inputs and
/// instructions come from the job file and nothing here can alter them.
/// The ledger is the queue and this class is its only writer for the run.
/// </summary>
public sealed class BatchRunner
{
    private readonly object _lock = new();
    private readonly List<LedgerRow> _ledger;
    private readonly Dictionary<string, RunningAttempt> _running = new();
    private readonly HashSet<string> _cancelled = new(StringComparer.Ordinal);
    private readonly IChildLauncher _launcher;
    private readonly ILaunchGate _gate;
    private readonly Action<string> _log;
    private readonly string? _mcpConfigPath;
    private readonly string _harnessVersion;
    private int _launched;

    public string RunId { get; }
    public string RunDir { get; }
    public string LedgerPath { get; }
    public JobFile JobFile { get; }
    public IReadOnlyList<ResolvedJob> Jobs { get; }
    /// <summary>The <c>--job</c> filter this batch was enqueued with; a filtered enqueue is a pilot.</summary>
    public string? JobFilter { get; }
    public bool Paused { get; private set; }
    public bool StopRequested { get; private set; }
    /// <summary>The run's own ceiling (from the job file, adjustable live); the host's is the other one.</summary>
    public int MaxParallel { get; private set; }
    public bool Completed { get; private set; }
    /// <summary>Set by the host for a scheduled enqueue: the loop is not started before this instant.</summary>
    public DateTimeOffset? NotBefore { get; set; }
    /// <summary>True once <see cref="RunAsync"/> has been entered; a scheduled batch is live but not started.</summary>
    public bool Started { get; private set; }
    public int InFlight { get { lock (_lock) return _running.Count; } }
    public int Launched => _launched;

    public event Action? Changed;
    public event Action<string, int>? StreamAdvanced;

    public BatchRunner(string runId, string runDir, JobFile jobFile, IReadOnlyList<ResolvedJob> jobs, string? jobFilter,
        IChildLauncher launcher, ILaunchGate gate, Action<string> log, string harnessVersion, string? mcpConfigPath)
    {
        RunId = runId;
        RunDir = runDir;
        JobFile = jobFile;
        Jobs = jobs;
        JobFilter = jobFilter;
        MaxParallel = jobFile.MaxParallel;
        _launcher = launcher;
        _gate = gate;
        _log = log;
        _harnessVersion = harnessVersion;
        _mcpConfigPath = mcpConfigPath;
        LedgerPath = Path.Combine(runDir, "ledger.jsonl");
        _ledger = new List<LedgerRow>(File.Exists(LedgerPath) ? RunnerPlan.ParseLedger(File.ReadLines(LedgerPath)) : []);
    }

    /// <summary>
    /// Everything the loop needs that is not the loop: parse, resolve, the launch-folder
    /// invariants, the MCP config. Returns the error text instead of a runner when the job
    /// file is unusable, so the CLI and the host report the same message.
    /// </summary>
    public static (BatchRunner? runner, string? error) Create(string jobFilePath, string runId, string? jobFilter,
        IChildLauncher launcher, ILaunchGate gate, Action<string> log, string harnessVersion)
    {
        jobFilePath = Path.GetFullPath(jobFilePath);
        if (!File.Exists(jobFilePath)) return (null, $"Job file not found: {jobFilePath}");
        var runDir = Path.GetDirectoryName(jobFilePath)!;

        JobFile jobFile;
        IReadOnlyList<ResolvedJob> jobs;
        try
        {
            jobFile = RunnerPlan.ParseJobFile(File.ReadAllText(jobFilePath));
            jobs = RunnerPlan.Resolve(jobFile, runDir);
        }
        catch (Exception ex) when (ex is System.Text.Json.JsonException or InvalidOperationException)
        {
            return (null, $"Job file unusable: {ex.Message}");
        }

        var launchError = CheckLaunchDir(jobFile.LaunchDir, jobFilePath);
        if (launchError is not null) return (null, launchError);

        string? mcpConfigPath = null;
        if (!string.IsNullOrWhiteSpace(jobFile.McpConfig))
        {
            mcpConfigPath = Path.GetFullPath(jobFile.McpConfig, runDir);
            if (!File.Exists(mcpConfigPath)) return (null, $"mcpConfig not found: {mcpConfigPath}");
        }

        if (jobFilter is not null)
        {
            jobs = jobs.Where(j => j.Id == jobFilter).ToList();
            if (jobs.Count == 0) return (null, $"--job \"{jobFilter}\" is not in the job file.");
        }

        return (new BatchRunner(runId, runDir, jobFile, jobs, jobFilter, launcher, gate, log, harnessVersion, mcpConfigPath), null);
    }

    /// <summary>The launch-folder invariants: outside the repo, and carrying no instruction stack of its own.</summary>
    public static string? CheckLaunchDir(string launchDir, string jobFilePath)
    {
        if (string.IsNullOrWhiteSpace(launchDir) || !Directory.Exists(launchDir))
            return $"launchDir does not exist: {launchDir}";
        launchDir = Path.GetFullPath(launchDir);
        var repoRoot = FindRepoRoot(jobFilePath);
        if (repoRoot is not null && IsSameOrUnder(launchDir, repoRoot))
            return $"launchDir must be OUTSIDE the repo ({repoRoot}) — that is the whole point.";
        foreach (var forbidden in new[] { "CLAUDE.md", ".claude", ".mcp.json" })
            if (File.Exists(Path.Combine(launchDir, forbidden)) || Directory.Exists(Path.Combine(launchDir, forbidden)))
                return $"launchDir contains {forbidden}; it must carry no instruction stack of its own.";
        return null;
    }

    public static string? FindRepoRoot(string fromPath)
    {
        var d = new DirectoryInfo(Path.GetDirectoryName(fromPath)!);
        while (d != null)
        {
            if (Directory.Exists(Path.Combine(d.FullName, ".git"))) return d.FullName;
            d = d.Parent;
        }
        return null;
    }

    // Segment-aware: "…\StoryPlanner-fanout" is NOT under "…\StoryPlanner", though it starts with it.
    public static bool IsSameOrUnder(string path, string root)
    {
        var p = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
        var r = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
        return p.Equals(r, StringComparison.OrdinalIgnoreCase)
            || p.StartsWith(r + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            || p.StartsWith(r + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
    }

    // --- harness commands: how the batch runs, never what a job is ---

    public void Pause() { lock (_lock) Paused = true; _log($"[{RunId}] paused"); Changed?.Invoke(); }
    public void Resume() { lock (_lock) Paused = false; _log($"[{RunId}] resumed"); Changed?.Invoke(); }
    public void StopAfterInFlight() { lock (_lock) StopRequested = true; _log($"[{RunId}] stop requested — finishing in-flight jobs"); Changed?.Invoke(); }
    public void SetMaxParallel(int n) { lock (_lock) MaxParallel = Math.Max(1, n); _log($"[{RunId}] maxParallel → {MaxParallel}"); Changed?.Invoke(); }

    public bool Cancel(string jobId)
    {
        RunningAttempt? r;
        lock (_lock)
        {
            if (!_running.TryGetValue(jobId, out r)) return false;
            _cancelled.Add(jobId);
        }
        _log($"[{RunId}] cancelling {jobId}" + (r.Handle is { } h ? $" (PID {h.Pid})" : ""));
        r.Handle?.Kill();
        Changed?.Invoke();
        return true;
    }

    // --- state for snapshots ---

    public IReadOnlyList<LedgerRow> LedgerSnapshot() { lock (_lock) return _ledger.ToList(); }
    public IReadOnlyList<RunningAttempt> RunningSnapshot() { lock (_lock) return _running.Values.ToList(); }
    public JobState StateOf(ResolvedJob job) { lock (_lock) return RunnerPlan.StateOf(job, _ledger, JobFile.MaxAttempts); }
    public string? HoldReason() => _gate.HoldReason(this);

    // --- the loop ---

    public async Task RunAsync(CancellationToken ct)
    {
        Started = true;
        var tasks = new List<Task>();
        try
        {
            while (!ct.IsCancellationRequested)
            {
                ResolvedJob? job = null;
                bool anyPending;
                lock (_lock)
                {
                    var available = Jobs.Where(j => !_running.ContainsKey(j.Id)).ToList();
                    var next = RunnerPlan.NextPending(available, _ledger, JobFile.MaxAttempts);
                    anyPending = next is not null;
                    if (next is not null && !Paused && !StopRequested && _running.Count < MaxParallel) job = next;
                }

                if (job is null)
                {
                    var running = InFlight;
                    if (running == 0 && (!anyPending || StopRequested)) break;   // done, or stopped with nothing in flight
                    // Paused, stop-requested with children running, at the run's own ceiling, or
                    // everything pending is in flight: wait for a child or a tick, then look again.
                    try { await Task.WhenAny(tasks.Count > 0 ? Task.WhenAny(tasks) : Task.Delay(-1, ct), Task.Delay(500, ct)); }
                    catch (OperationCanceledException) { break; }
                    tasks.RemoveAll(t => t.IsCompleted);
                    continue;
                }

                if (!_gate.TryAcquire(this))
                {
                    try { await Task.Delay(500, ct); } catch (OperationCanceledException) { break; }
                    continue;
                }

                var attempt = AttemptsOf(job) + 1;
                var theJob = job;
                lock (_lock) _running[theJob.Id] = new RunningAttempt(theJob.Id, attempt, DateTimeOffset.UtcNow, Path.Combine(RunDir, "attempts", theJob.Id, $"attempt-{attempt}", "stream.jsonl"), null);
                Interlocked.Increment(ref _launched);
                Changed?.Invoke();
                tasks.Add(Task.Run(async () =>
                {
                    try { await RunOne(theJob, attempt, ct); }
                    finally
                    {
                        lock (_lock) { _running.Remove(theJob.Id); _cancelled.Remove(theJob.Id); }
                        _gate.Release(this);
                        Changed?.Invoke();
                    }
                }, CancellationToken.None));
            }
            await Task.WhenAll(tasks.Select(t => t.ContinueWith(_ => { })));
        }
        finally
        {
            Completed = true;
            _log($"[{RunId}] {(ct.IsCancellationRequested ? "stopped" : StopRequested ? "stopped after in-flight" : "complete")} — launched {_launched} this run");
            Changed?.Invoke();
        }
    }

    public void KillAll()
    {
        List<RunningAttempt> running;
        lock (_lock) { running = _running.Values.ToList(); foreach (var r in running) _cancelled.Add(r.JobId); }
        foreach (var r in running) r.Handle?.Kill();
    }

    private int AttemptsOf(ResolvedJob job) { lock (_lock) return RunnerPlan.AttemptsOf(job, _ledger); }

    private async Task RunOne(ResolvedJob job, int attempt, CancellationToken ct)
    {
        var attemptDir = Path.Combine(RunDir, "attempts", job.Id, $"attempt-{attempt}");
        Directory.CreateDirectory(attemptDir);
        var streamPath = Path.Combine(attemptDir, "stream.jsonl");
        var mode = JobFilter is null ? null : "pilot";

        ComposedPrompt prompt;
        try
        {
            prompt = RunnerPlan.ComposePrompt(job, File.ReadAllText);
        }
        catch (IOException ex)
        {
            _log($"[{RunId}] {job.Id}: cannot compose prompt — {ex.Message}");
            Record(new LedgerRow(job.Id, attempt, job.Model, _harnessVersion, "", [], [],
                DateTimeOffset.UtcNow.ToString("o"), DateTimeOffset.UtcNow.ToString("o"), -1, "", false, null, null, null, "cannot compose prompt", mode));
            return;
        }
        await File.WriteAllTextAsync(Path.Combine(attemptDir, "prompt.md"), prompt.Text, ct);

        // --restricted confines file tools to the working directories, and --add-dir of a path
        // that does not exist yet is silently dropped — so the output directory must exist first.
        var outputDir = Path.GetDirectoryName(job.OutputPath);
        if (!string.IsNullOrEmpty(outputDir)) Directory.CreateDirectory(outputDir);

        _log($"[{RunId}] {job.Id}: attempt {attempt}/{JobFile.MaxAttempts}, model {job.Model}, prompt {prompt.Text.Length:N0} chars (sha256 {prompt.Sha256[..12]}…), timeout {job.TimeoutMinutes} min");

        var start = DateTimeOffset.UtcNow;
        var request = new ChildRequest(job, prompt.Text, streamPath, Path.GetFullPath(JobFile.LaunchDir), _mcpConfigPath, TimeSpan.FromMinutes(job.TimeoutMinutes));
        var exitCode = await _launcher.LaunchAsync(request,
            handle => { lock (_lock) if (_running.TryGetValue(job.Id, out var r)) _running[job.Id] = r with { Handle = handle }; Changed?.Invoke(); },
            () => StreamAdvanced?.Invoke(job.Id, attempt),
            ct);
        var end = DateTimeOffset.UtcNow;

        bool cancelled;
        lock (_lock) cancelled = _cancelled.Contains(job.Id);
        if (cancelled && exitCode != 0) exitCode = -4;

        var outputExists = File.Exists(job.OutputPath);
        var outputCheck = exitCode == -3 ? "timed out"
            : exitCode == -4 ? "cancelled"
            : outputExists ? RunnerPlan.CheckOutput(await File.ReadAllTextAsync(job.OutputPath, ct), job.RequireOnce)
            : "no output file";
        var summary = File.Exists(streamPath)
            ? RunnerPlan.ParseResultSummary(await File.ReadAllTextAsync(streamPath, ct))
            : new ResultSummary(null, null, null, null);

        var row = new LedgerRow(job.Id, attempt, job.Model, _harnessVersion, prompt.Sha256, prompt.ProtocolShas, prompt.InputShas,
            start.ToString("o"), end.ToString("o"), exitCode, streamPath, outputExists, summary.CostUsd, summary.Turns, summary.SessionId, outputCheck, mode);
        var state = Record(row);
        _log($"[{RunId}] {job.Id}: exit {exitCode}, output {(outputExists ? "present" : "MISSING")}, check: {outputCheck}, {(end - start).TotalSeconds:F0}s" +
             (summary.CostUsd is { } cost ? $", ${cost:F3}" : "") + (summary.Turns is { } turns ? $", {turns} turn(s)" : "") +
             $" → {state}" + (state == JobState.Failed ? " — not retried" : ""));
    }

    private JobState Record(LedgerRow row)
    {
        lock (_lock)
        {
            File.AppendAllText(LedgerPath, RunnerPlan.SerializeLedgerRow(row) + "\n");
            _ledger.Add(row);
            var job = Jobs.First(j => j.Id == row.JobId);
            return RunnerPlan.StateOf(job, _ledger, JobFile.MaxAttempts);
        }
    }
}
