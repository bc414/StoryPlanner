using System.Text.Json;

namespace StoryPlanner.AgentRunner;

/// <summary>
/// The host's settings: the port and bind address of the page, the global ceilings, and
/// where the fanout tree is. Read from <c>configs/host.json</c> when present; every field
/// has a default. <c>token</c> and <c>bind</c> exist for the LAN follow-up and are inert on
/// localhost.
/// </summary>
public sealed record HostConfig(
    int Port = 5190,
    string Bind = "127.0.0.1",
    string? Token = null,
    string? FanoutRoot = null,
    int MaxParallel = 4,
    int UtilizationCap = 80)
{
    public string Url => $"http://{Bind}:{Port}";

    private static readonly JsonSerializerOptions Json = new() { PropertyNameCaseInsensitive = true, ReadCommentHandling = JsonCommentHandling.Skip };

    public static HostConfig Load(string? path)
    {
        var cfg = path is not null && File.Exists(path)
            ? JsonSerializer.Deserialize<HostConfig>(File.ReadAllText(path), Json) ?? new HostConfig()
            : new HostConfig();
        return cfg with { FanoutRoot = cfg.FanoutRoot ?? FindFanoutRoot() };
    }

    /// <summary>The repo's <c>fanout/</c>, found by walking up from the exe (publish/ or bin/) to the folder holding <c>.git</c>, then from the cwd.</summary>
    public static string FindFanoutRoot()
    {
        foreach (var start in new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
        {
            var d = new DirectoryInfo(start);
            while (d != null)
            {
                if (Directory.Exists(Path.Combine(d.FullName, ".git"))) return Path.Combine(d.FullName, "fanout");
                d = d.Parent;
            }
        }
        return Path.Combine(Directory.GetCurrentDirectory(), "fanout");
    }
}

/// <summary>
/// What Claude Code last cached about the subscription's usage: the five-hour window (the
/// one the cap gates on), the seven-day window, when the cache was fetched, and a lock
/// reason if the account is locked. All of it is the cache's word, not a live query.
/// </summary>
public sealed record Utilization(
    int Percent,
    DateTimeOffset ResetsAt,
    DateTimeOffset ReadAtUtc,
    int? SevenDayPercent = null,
    DateTimeOffset? SevenDayResetsAt = null,
    string? LockedReason = null)
{
    /// <summary>Older than an hour is not to be trusted either way — a session must have run for the cache to refresh.</summary>
    public bool Stale => DateTimeOffset.UtcNow - ReadAtUtc > TimeSpan.FromHours(1);
    public TimeSpan Age => DateTimeOffset.UtcNow - ReadAtUtc;
    public bool ResetPassed => ResetsAt <= DateTimeOffset.UtcNow;
    public TimeSpan UntilReset => ResetsAt - DateTimeOffset.UtcNow;
}

public sealed record EnqueueResult(bool Ok, string? RunId, string Message);

/// <summary>The <c>--at</c> forms: <c>HH:mm</c> (the next such clock time), an ISO instant, or <c>reset</c> (the cached window reset plus a minute).</summary>
public static class Schedule
{
    public static (DateTimeOffset? at, string? error) ParseAt(string spec, DateTimeOffset now, Utilization? cached)
    {
        spec = spec.Trim();
        if (spec.Equals("reset", StringComparison.OrdinalIgnoreCase))
        {
            if (cached is null) return (null, "--at reset: no cached utilization in ~/.claude.json — run a Claude Code session so the cache exists, or give a clock time");
            if (cached.ResetsAt <= now) return (null, $"--at reset: the cached reset ({cached.ResetsAt.ToLocalTime():HH:mm}) is already past — the window has reset; enqueue without --at");
            return (cached.ResetsAt.AddMinutes(1), null);
        }
        if (TimeOnly.TryParseExact(spec, "HH:mm", out var clock))
        {
            var local = now.ToLocalTime();
            var candidate = new DateTimeOffset(local.Date.Add(clock.ToTimeSpan()), local.Offset);
            if (candidate <= now) candidate = candidate.AddDays(1);
            return (candidate, null);
        }
        if (DateTimeOffset.TryParse(spec, null, System.Globalization.DateTimeStyles.AssumeLocal, out var instant))
        {
            if (instant <= now) return (null, $"--at {spec} is in the past");
            return (instant, null);
        }
        return (null, $"--at {spec}: give HH:mm, an ISO date-time, or reset");
    }
}

/// <summary>
/// The persistent host: owns the page's port, runs any number of batches at once under one
/// global parallel ceiling and one utilization cap (the launch gate every batch acquires
/// through), keeps the live batches, reads the history from disk, and writes the host log.
/// Harness control only: nothing here can change what a job is.
/// </summary>
public sealed class RunnerHost : ILaunchGate, IDisposable
{
    private readonly object _lock = new();
    private readonly Dictionary<string, BatchRunner> _live = new(StringComparer.Ordinal);
    private readonly IChildLauncher _launcher;
    private readonly string _harnessVersion;
    private readonly CancellationTokenSource _cts = new();
    private readonly string _logPath;
    private readonly Action<string>? _echo;
    private readonly Timer _scheduler;
    private int _inFlight;

    public HostConfig Config { get; }
    public string FanoutRoot { get; }
    public int MaxParallel { get; private set; }
    public int UtilizationCap { get; private set; }
    public int InFlight => _inFlight;
    public DateTimeOffset StartedUtc { get; } = DateTimeOffset.UtcNow;
    public bool ShuttingDown { get; private set; }

    public event Action? Changed;
    public event Action<string, string, int>? StreamAdvanced;

    public RunnerHost(HostConfig config, IChildLauncher launcher, string harnessVersion, Action<string>? echo = null)
    {
        Config = config;
        FanoutRoot = Path.GetFullPath(config.FanoutRoot ?? HostConfig.FindFanoutRoot());
        MaxParallel = Math.Max(1, config.MaxParallel);
        UtilizationCap = Math.Clamp(config.UtilizationCap, 1, 100);
        _launcher = launcher;
        _harnessVersion = harnessVersion;
        _echo = echo;
        Directory.CreateDirectory(FanoutRoot);
        _logPath = Path.Combine(FanoutRoot, "host-log.txt");
        _scheduler = new Timer(_ => StartDue(), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
    }

    // --- log: the provenance of how batches were driven (lifecycle, enqueues, knob changes) ---

    public void Log(string message)
    {
        var line = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss} {message}";
        lock (_lock) { try { File.AppendAllText(_logPath, line + "\n"); } catch (IOException) { } }
        _echo?.Invoke(line);
    }

    // --- runs: live layered over disk ---

    public IReadOnlyList<RunSnapshot> Runs()
    {
        Dictionary<string, BatchRunner> live;
        lock (_lock) live = new(_live);
        var byId = new Dictionary<string, RunSnapshot>(StringComparer.Ordinal);
        foreach (var dir in RunCatalog.RunDirs(FanoutRoot))
        {
            var id = RunCatalog.RunIdFor(dir, FanoutRoot);
            byId[id] = RunCatalog.Build(dir, FanoutRoot, live.GetValueOrDefault(id));
        }
        foreach (var (id, runner) in live)
            if (!byId.ContainsKey(id)) byId[id] = RunCatalog.Build(runner.RunDir, FanoutRoot, runner);
        return byId.Values
            .OrderByDescending(r => r.Live)
            .ThenByDescending(r => r.LastActivityUtc ?? "")
            .ToList();
    }

    public RunSnapshot? Run(string runId)
    {
        BatchRunner? live;
        lock (_lock) _live.TryGetValue(runId, out live);
        var dir = live?.RunDir ?? Path.Combine(FanoutRoot, runId.Replace('/', Path.DirectorySeparatorChar));
        if (live is null && !File.Exists(Path.Combine(dir, "ledger.jsonl")) && !File.Exists(Path.Combine(dir, "jobs.json"))) return null;
        return RunCatalog.Build(dir, FanoutRoot, live);
    }

    public BatchRunner? Live(string runId) { lock (_lock) return _live.GetValueOrDefault(runId); }

    // --- enqueue ---

    public EnqueueResult Enqueue(string jobFilePath, string? jobFilter, DateTimeOffset? notBefore = null)
    {
        if (ShuttingDown) return new EnqueueResult(false, null, "host is shutting down");
        jobFilePath = Path.GetFullPath(jobFilePath);
        var runDir = Path.GetDirectoryName(jobFilePath)!;
        if (!BatchRunner.IsSameOrUnder(runDir, FanoutRoot))
            return new EnqueueResult(false, null, $"run folder must be under {FanoutRoot} (the fanout tree is the record)");
        var runId = RunCatalog.RunIdFor(runDir, FanoutRoot);

        lock (_lock)
        {
            if (_live.TryGetValue(runId, out var existing) && !existing.Completed)
                return new EnqueueResult(false, runId, existing.Started
                    ? $"{runId} is already running (enqueue again when it completes)"
                    : $"{runId} is already scheduled for {existing.NotBefore!.Value.ToLocalTime():yyyy-MM-dd HH:mm} (unschedule it first)");
        }

        var (runner, error) = BatchRunner.Create(jobFilePath, runId, jobFilter, _launcher, this, Log, _harnessVersion);
        if (runner is null) return new EnqueueResult(false, runId, error!);

        runner.Changed += () => Changed?.Invoke();
        runner.StreamAdvanced += (job, attempt) => StreamAdvanced?.Invoke(runId, job, attempt);
        var pilot = jobFilter is null ? "" : $" (pilot: --job {jobFilter})";
        if (notBefore is { } at && at > DateTimeOffset.UtcNow)
        {
            runner.NotBefore = at;
            lock (_lock) _live[runId] = runner;
            Log($"[{runId}] scheduled for {at.ToLocalTime():yyyy-MM-dd HH:mm}{pilot}: {runner.Jobs.Count} job(s), run ceiling {runner.MaxParallel}");
            Changed?.Invoke();
            return new EnqueueResult(true, runId, $"{runId}: {runner.Jobs.Count} job(s) scheduled for {at.ToLocalTime():yyyy-MM-dd HH:mm}");
        }

        lock (_lock) _live[runId] = runner;
        Log($"[{runId}] enqueued{pilot}: {runner.Jobs.Count} job(s), run ceiling {runner.MaxParallel}");
        Start(runner);
        return new EnqueueResult(true, runId, $"{runId}: {runner.Jobs.Count} job(s) enqueued");
    }

    private void Start(BatchRunner runner)
    {
        _ = Task.Run(async () =>
        {
            try { await runner.RunAsync(_cts.Token); }
            catch (Exception ex) { Log($"[{runner.RunId}] batch faulted: {ex}"); }
            Changed?.Invoke();
        });
        Changed?.Invoke();
    }

    /// <summary>The scheduler's tick: start every scheduled batch whose time has come.</summary>
    public void StartDue()
    {
        if (ShuttingDown) return;
        List<BatchRunner> due;
        lock (_lock) due = _live.Values.Where(r => r is { Started: false, NotBefore: not null } && r.NotBefore <= DateTimeOffset.UtcNow).ToList();
        foreach (var r in due)
        {
            Log($"[{r.RunId}] scheduled time reached — starting");
            Start(r);
        }
    }

    /// <summary>Removes a scheduled batch that has not started. No ledger row; the run folder is untouched.</summary>
    public bool Unschedule(string runId)
    {
        lock (_lock)
        {
            if (!_live.TryGetValue(runId, out var r) || r.Started || r.NotBefore is null) return false;
            _live.Remove(runId);
        }
        Log($"[{runId}] unscheduled");
        Changed?.Invoke();
        return true;
    }

    // --- harness control ---

    public bool Pause(string id) => With(id, r => r.Pause());
    public bool Resume(string id) => With(id, r => r.Resume());
    public bool Stop(string id) => With(id, r => r.StopAfterInFlight());
    public bool SetRunMaxParallel(string id, int n) => With(id, r => r.SetMaxParallel(n));
    public bool Cancel(string id, string jobId) { var r = Live(id); return r is not null && r.Cancel(jobId); }

    private bool With(string id, Action<BatchRunner> act)
    {
        var r = Live(id);
        if (r is null || r.Completed || !r.Started) return false;   // a scheduled batch has only "unschedule"
        act(r);
        return true;
    }

    public void SetMaxParallel(int n)
    {
        lock (_lock) MaxParallel = Math.Max(1, n);
        Log($"host maxParallel → {MaxParallel}");
        Changed?.Invoke();
    }

    public void SetUtilizationCap(int percent)
    {
        lock (_lock) UtilizationCap = Math.Clamp(percent, 1, 100);
        Log($"host utilizationCap → {UtilizationCap}%");
        Changed?.Invoke();
    }

    /// <summary>Stop: every live batch finishes its in-flight jobs and launches nothing further; now: their children are killed. Then the host exits.</summary>
    public async Task ShutdownAsync(bool now)
    {
        ShuttingDown = true;
        Log(now ? "shutdown NOW requested — killing children" : "shutdown requested — finishing in-flight jobs");
        List<BatchRunner> live;
        lock (_lock)
        {
            foreach (var s in _live.Where(kv => !kv.Value.Started).Select(kv => kv.Key).ToList()) { _live.Remove(s); Log($"[{s}] unscheduled by shutdown"); }
            live = _live.Values.Where(r => !r.Completed).ToList();
        }
        foreach (var r in live) { r.StopAfterInFlight(); if (now) r.KillAll(); }
        Changed?.Invoke();
        while (live.Any(r => !r.Completed)) await Task.Delay(250);
        _cts.Cancel();
        Log("host stopped");
    }

    // --- the launch gate: global ceiling and cap, across every batch ---

    public bool TryAcquire(BatchRunner run)
    {
        lock (_lock)
        {
            if (ShuttingDown) return false;
            if (_inFlight >= MaxParallel) return false;
            if (CapExceeded()) return false;
            _inFlight++;
            return true;
        }
    }

    public void Release(BatchRunner run) { lock (_lock) _inFlight = Math.Max(0, _inFlight - 1); }

    public string? HoldReason(BatchRunner run)
    {
        if (run.Paused) return "paused";
        if (run.StopRequested) return "stopping after in-flight";
        if (run.InFlight >= run.MaxParallel) return $"at the run's ceiling ({run.MaxParallel})";
        lock (_lock)
        {
            if (ShuttingDown) return "host shutting down";
            if (_inFlight >= MaxParallel) return $"at the host ceiling ({MaxParallel} in flight)";
            var u = ReadUtilization();
            if (u is not null && u.Percent >= UtilizationCap && u.ResetsAt > DateTimeOffset.UtcNow)
                return $"{(u.Stale ? "stale " : "")}utilization {u.Percent}% ≥ cap {UtilizationCap}% — waiting for the reset at {u.ResetsAt.ToLocalTime():HH:mm}";
        }
        return null;
    }

    public void Dispose()
    {
        _scheduler.Dispose();
        _cts.Dispose();
    }

    private bool CapExceeded()
    {
        var u = ReadUtilization();
        return u is not null && u.Percent >= UtilizationCap && u.ResetsAt > DateTimeOffset.UtcNow;
    }

    /// <summary>What Claude Code last cached in <c>~/.claude.json</c> — not a live query; the file's mtime says how stale.</summary>
    public Utilization? ReadUtilization() => ReadCachedUtilization();

    public static Utilization? ReadCachedUtilization()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude.json");
        if (!File.Exists(path)) return null;
        try
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (!doc.RootElement.TryGetProperty("cachedUsageUtilization", out var cache)) return null;
            if (!cache.TryGetProperty("utilization", out var u)) return null;
            if (!u.TryGetProperty("five_hour", out var fiveHour) || fiveHour.ValueKind != JsonValueKind.Object) return null;
            var percent = fiveHour.GetProperty("utilization").GetInt32();
            var resetsAt = DateTimeOffset.Parse(fiveHour.GetProperty("resets_at").GetString()!);
            var fetchedAt = cache.TryGetProperty("fetchedAtMs", out var f) && f.ValueKind == JsonValueKind.Number
                ? DateTimeOffset.FromUnixTimeMilliseconds(f.GetInt64())
                : File.GetLastWriteTimeUtc(path);
            int? sevenPercent = null; DateTimeOffset? sevenResets = null;
            if (u.TryGetProperty("seven_day", out var seven) && seven.ValueKind == JsonValueKind.Object)
            {
                sevenPercent = seven.GetProperty("utilization").GetInt32();
                if (seven.TryGetProperty("resets_at", out var sr) && sr.ValueKind == JsonValueKind.String) sevenResets = DateTimeOffset.Parse(sr.GetString()!);
            }
            var locked = fiveHour.TryGetProperty("locked_reason", out var lr) && lr.ValueKind == JsonValueKind.String ? lr.GetString() : null;
            return new Utilization(percent, resetsAt, fetchedAt, sevenPercent, sevenResets, locked);
        }
        catch
        {
            return null;
        }
    }
}
