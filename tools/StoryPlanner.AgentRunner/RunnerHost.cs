using System.Text.Json;

namespace StoryPlanner.AgentRunner;

/// <summary>
/// The host's settings: the port and bind address of the page, the launch folder outside the
/// repo, the global ceilings, the idle limit after which a silent call is killed, and
/// optionally where the process map is. Read from <c>configs/host.json</c> when present;
/// every field has a default. <c>token</c> and <c>bind</c> exist for the LAN follow-up and are
/// inert on localhost. Nothing here is a batch's: a batch's settings are its definition.
/// </summary>
public sealed record HostConfig(
    int Port = 5190,
    string Bind = "127.0.0.1",
    string? Token = null,
    string? LaunchDir = null,
    int MaxParallel = 4,
    int UtilizationCap = 80,
    int IdleMinutes = 10,
    string? MapPath = null)
{
    public string Url => $"http://{Bind}:{Port}";

    /// <summary>The skill folders the map is looked for under, in order, until the router swap retires the second.</summary>
    public static readonly string[] MapCandidates =
        [".claude/skills/v3-buildout/map.md", ".claude/skills/v3-buildout-2/map.md"];

    /// <summary>
    /// Where the process map is: <c>mapPath</c> from host.json (absolute, or relative to the
    /// repo root), else the first of <see cref="MapCandidates"/> that exists under the repo root
    /// above the working directory. Null when none exists.
    /// </summary>
    public static string? ResolveMapPath(string? mapPath, string workingDir)
    {
        var repoRoot = Batch.FindRepoRoot(Path.Combine(workingDir, "x")) ?? workingDir;
        if (!string.IsNullOrWhiteSpace(mapPath))
        {
            var explicitPath = Path.IsPathRooted(mapPath) ? mapPath : Path.Combine(repoRoot, mapPath);
            return File.Exists(explicitPath) ? Path.GetFullPath(explicitPath) : null;
        }
        foreach (var candidate in MapCandidates)
        {
            var p = Path.Combine(repoRoot, candidate.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(p)) return p;
        }
        return null;
    }

    private static readonly JsonSerializerOptions Json = new() { PropertyNameCaseInsensitive = true, ReadCommentHandling = JsonCommentHandling.Skip };

    public static HostConfig Load(string? path)
    {
        var cfg = path is not null && File.Exists(path)
            ? JsonSerializer.Deserialize<HostConfig>(File.ReadAllText(path), Json) ?? new HostConfig()
            : new HostConfig();
        return cfg with { LaunchDir = cfg.LaunchDir ?? DefaultLaunchDir() };
    }

    /// <summary>The sibling folder <c>StoryPlanner-fanout</c> beside the repository the exe sits in, when none is configured.</summary>
    public static string DefaultLaunchDir()
    {
        var repo = Batch.FindRepoRoot(Path.Combine(AppContext.BaseDirectory, "x")) ?? Batch.FindRepoRoot(Path.Combine(Directory.GetCurrentDirectory(), "x"));
        var parent = repo is null ? Directory.GetCurrentDirectory() : Path.GetDirectoryName(repo) ?? repo;
        return Path.Combine(parent, "StoryPlanner-fanout");
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

public sealed record ExecuteResult(bool Ok, string? BatchId, string Message);

/// <summary>The <c>--at</c> forms: <c>HH:mm</c> (the next such clock time), an ISO instant, or <c>reset</c> (the cached window reset plus a minute).</summary>
public static class Schedule
{
    public static (DateTimeOffset? at, string? error) ParseAt(string spec, DateTimeOffset now, Utilization? cached)
    {
        spec = spec.Trim();
        if (spec.Equals("reset", StringComparison.OrdinalIgnoreCase))
        {
            if (cached is null) return (null, "--at reset: no cached utilization in ~/.claude.json — run a Claude Code session so the cache exists, or give a clock time");
            if (cached.ResetsAt <= now) return (null, $"--at reset: the cached reset ({cached.ResetsAt.ToLocalTime():HH:mm}) is already past — the window has reset; execute without --at");
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
/// global parallel ceiling, one utilization cap and one idle limit (the launch gate every
/// batch acquires through), keeps the live executions, reads every batch beneath its working
/// directory from disk, and writes its log beside the exe. Harness control only: nothing here
/// can change what a call is.
/// </summary>
public sealed class RunnerHost : ILaunchGate, IDisposable
{
    private readonly object _lock = new();
    private readonly Dictionary<string, BatchRunner> _live = new(StringComparer.OrdinalIgnoreCase);
    private readonly IChildLauncher _launcher;
    private readonly string _harnessVersion;
    private readonly CancellationTokenSource _cts = new();
    private readonly string _logPath;
    private readonly Action<string>? _echo;
    private readonly Func<Utilization?> _utilization;
    private readonly Timer _scheduler;
    private int _inFlight;

    public HostConfig Config { get; }
    /// <summary>Wherever the host was started: the page lists the batches whose definitions it finds beneath it.</summary>
    public string WorkingDir { get; }
    public string LaunchDir { get; }
    public int MaxParallel { get; private set; }
    public int UtilizationCap { get; private set; }
    public int IdleMinutes { get; private set; }
    public TimeSpan IdleLimit => TimeSpan.FromMinutes(Math.Max(1, IdleMinutes));
    public int InFlight => _inFlight;
    public DateTimeOffset StartedUtc { get; } = DateTimeOffset.UtcNow;
    public bool ShuttingDown { get; private set; }

    public event Action? Changed;
    public event Action<string, string, int>? StreamAdvanced;

    /// <param name="utilization">
    /// Where the cap reads the usage figure; the cache in <c>~/.claude.json</c> by default. Tests
    /// pass their own so the launch gate never depends on the developer's live subscription
    /// window.
    /// </param>
    public RunnerHost(HostConfig config, IChildLauncher launcher, string harnessVersion, string? workingDir = null, Action<string>? echo = null, Func<Utilization?>? utilization = null, string? logPath = null)
    {
        Config = config;
        WorkingDir = Path.GetFullPath(workingDir ?? Directory.GetCurrentDirectory());
        LaunchDir = Path.GetFullPath(config.LaunchDir ?? HostConfig.DefaultLaunchDir());
        MaxParallel = Math.Max(1, config.MaxParallel);
        UtilizationCap = Math.Clamp(config.UtilizationCap, 1, 100);
        IdleMinutes = Math.Max(1, config.IdleMinutes);
        _launcher = launcher;
        _harnessVersion = harnessVersion;
        _echo = echo;
        _utilization = utilization ?? ReadCachedUtilization;
        _logPath = logPath ?? Path.Combine(AppContext.BaseDirectory, "host-log.txt");
        _scheduler = new Timer(_ => StartDue(), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
    }

    // --- log: how batches were driven (lifecycle, executions, knob changes); the tool's own, no artifact reads it ---

    public void Log(string message)
    {
        var line = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss} {message}";
        lock (_lock) { try { File.AppendAllText(_logPath, line + "\n"); } catch (IOException) { } }
        _echo?.Invoke(line);
    }

    // --- batches: live layered over disk ---

    public IReadOnlyList<BatchSnapshot> Batches()
    {
        Dictionary<string, BatchRunner> live;
        lock (_lock) live = new(_live, StringComparer.OrdinalIgnoreCase);
        var byId = new Dictionary<string, BatchSnapshot>(StringComparer.OrdinalIgnoreCase);
        foreach (var definition in BatchCatalog.Definitions(WorkingDir))
        {
            var key = Path.GetFullPath(definition);
            byId[key] = BatchCatalog.Build(definition, WorkingDir, live.GetValueOrDefault(key));
        }
        foreach (var (key, runner) in live)
            if (!byId.ContainsKey(key)) byId[key] = BatchCatalog.Build(key, WorkingDir, runner);
        return byId.Values
            .OrderByDescending(b => b.Live)
            .ThenByDescending(b => b.LastActivityUtc ?? "")
            .ThenBy(b => b.Id, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>The definition path a batch id names: relative to the working directory, or absolute.</summary>
    public string DefinitionPathOf(string batchId)
    {
        var dir = Path.IsPathRooted(batchId) ? batchId : Path.Combine(WorkingDir, batchId.Replace('/', Path.DirectorySeparatorChar));
        return Path.GetFullPath(Path.Combine(dir, "definition.md"));
    }

    public BatchSnapshot? Batch(string batchId)
    {
        var definition = DefinitionPathOf(batchId);
        BatchRunner? live;
        lock (_lock) _live.TryGetValue(definition, out live);
        if (live is null && !File.Exists(definition)) return null;
        return BatchCatalog.Build(definition, WorkingDir, live);
    }

    public BatchRunner? Live(string batchId) { lock (_lock) return _live.GetValueOrDefault(DefinitionPathOf(batchId)); }

    // --- execute ---

    public ExecuteResult Execute(string definitionPath, string? item, DateTimeOffset? notBefore = null)
    {
        if (ShuttingDown) return new ExecuteResult(false, null, "host is shutting down");
        definitionPath = Path.GetFullPath(definitionPath);
        var id = AgentRunner.Batch.IdFor(Path.GetDirectoryName(definitionPath)!, WorkingDir);

        lock (_lock)
        {
            if (_live.TryGetValue(definitionPath, out var existing) && !existing.Completed)
                return new ExecuteResult(false, id, existing.Started
                    ? $"{id} is already executing (execute again when it completes)"
                    : $"{id} is already scheduled for {existing.NotBefore!.Value.ToLocalTime():yyyy-MM-dd HH:mm} (unschedule it first)");
        }

        var (runner, error) = BatchRunner.Create(definitionPath, WorkingDir, item, LaunchDir, _launcher, this, Log, _harnessVersion);
        if (runner is null) return new ExecuteResult(false, id, error!);

        runner.Changed += () => Changed?.Invoke();
        runner.StreamAdvanced += (i, call) => StreamAdvanced?.Invoke(id, i, call);
        var summary = runner.Summary();
        if (notBefore is { } at && at > DateTimeOffset.UtcNow)
        {
            runner.NotBefore = at;
            lock (_lock) _live[definitionPath] = runner;
            Log($"[{id}] scheduled for {at.ToLocalTime():yyyy-MM-dd HH:mm}: {summary}");
            Changed?.Invoke();
            return new ExecuteResult(true, id, $"{id}: {summary}; scheduled for {at.ToLocalTime():yyyy-MM-dd HH:mm}");
        }

        lock (_lock) _live[definitionPath] = runner;
        Log($"[{id}] execution {runner.Execution}: {summary}");
        Start(runner);
        return new ExecuteResult(true, id, $"{id}: execution {runner.Execution}, {summary}");
    }

    private void Start(BatchRunner runner)
    {
        _ = Task.Run(async () =>
        {
            try { await runner.RunAsync(_cts.Token); }
            catch (Exception ex) { Log($"[{runner.Id}] execution faulted: {ex}"); }
            Changed?.Invoke();
        });
        Changed?.Invoke();
    }

    /// <summary>The scheduler's tick: start every scheduled execution whose time has come.</summary>
    public void StartDue()
    {
        if (ShuttingDown) return;
        List<BatchRunner> due;
        lock (_lock) due = _live.Values.Where(r => r is { Started: false, NotBefore: not null } && r.NotBefore <= DateTimeOffset.UtcNow).ToList();
        foreach (var r in due)
        {
            Log($"[{r.Id}] scheduled time reached — starting");
            Start(r);
        }
    }

    /// <summary>Removes a scheduled execution that has not started. No call, no entry; the batch folder is untouched.</summary>
    public bool Unschedule(string batchId)
    {
        var key = DefinitionPathOf(batchId);
        lock (_lock)
        {
            if (!_live.TryGetValue(key, out var r) || r.Started || r.NotBefore is null) return false;
            _live.Remove(key);
        }
        Log($"[{batchId}] unscheduled");
        Changed?.Invoke();
        return true;
    }

    // --- harness control ---

    public bool Pause(string id) => With(id, r => r.Pause());
    public bool Resume(string id) => With(id, r => r.Resume());
    public bool Stop(string id) => With(id, r => r.StopAfterInFlight());
    public bool Cancel(string id, string item) { var r = Live(id); return r is not null && r.Cancel(item); }

    private bool With(string id, Action<BatchRunner> act)
    {
        var r = Live(id);
        if (r is null || r.Completed || !r.Started) return false;   // a scheduled execution has only "unschedule"
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

    public void SetIdleMinutes(int minutes)
    {
        lock (_lock) IdleMinutes = Math.Max(1, minutes);
        Log($"host idleMinutes → {IdleMinutes}");
        Changed?.Invoke();
    }

    /// <summary>Stop: every live execution finishes its in-flight calls and launches nothing further; now: their children are killed. Then the host exits.</summary>
    public async Task ShutdownAsync(bool now)
    {
        ShuttingDown = true;
        Log(now ? "shutdown NOW requested — killing children" : "shutdown requested — finishing in-flight calls");
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

    public bool TryAcquire(BatchRunner batch)
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

    public void Release(BatchRunner batch) { lock (_lock) _inFlight = Math.Max(0, _inFlight - 1); }

    public string? HoldReason(BatchRunner batch)
    {
        if (batch.Paused) return "paused";
        if (batch.StopRequested) return "stopping after in-flight";
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
    public Utilization? ReadUtilization() => _utilization();

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
