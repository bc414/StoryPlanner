using System.Text;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.AgentRunner;

/// <summary>
/// Decides whether one more child may launch right now, across every batch the host runs:
/// the global parallel ceiling and the utilization cap live behind it. A batch acquires a
/// slot before each launch and releases it when the child exits.
/// </summary>
public interface ILaunchGate
{
    bool TryAcquire(BatchRunner batch);
    void Release(BatchRunner batch);
    /// <summary>Why a launch is being held, for the page; null when nothing holds it.</summary>
    string? HoldReason(BatchRunner batch);
    /// <summary>The idle limit every call runs under, the host's setting.</summary>
    TimeSpan IdleLimit { get; }
}

/// <summary>No global constraint and a long idle limit. The CLI's serverless paths and tests use it.</summary>
public sealed class OpenGate : ILaunchGate
{
    public bool TryAcquire(BatchRunner batch) => true;
    public void Release(BatchRunner batch) { }
    public string? HoldReason(BatchRunner batch) => null;
    public TimeSpan IdleLimit { get; init; } = TimeSpan.FromMinutes(10);
}

public sealed record RunningCall(string Item, int Call, DateTimeOffset StartUtc, string StreamPath, IChildHandle? Handle);

/// <summary>
/// One execution of a batch (decisions.md, "executing a batch is one call per item still
/// without a result"): one call for every item of the index that has no successful call yet,
/// or the one item named, which is the pilot. The call's number is the execution's. Nothing
/// here changes what a call is; the loop takes its launcher and its gate from outside and
/// accepts harness commands (pause, resume, stop after in-flight, cancel an item) while it
/// runs. The calls file is the batch's state and this class is its only writer.
/// </summary>
public sealed class BatchRunner
{
    private readonly object _lock = new();
    private readonly List<CallEntry> _calls;
    private readonly Dictionary<string, RunningCall> _running = new(StringComparer.Ordinal);
    private readonly HashSet<string> _cancelled = new(StringComparer.Ordinal);
    private readonly IChildLauncher _launcher;
    private readonly ILaunchGate _gate;
    private readonly Action<string> _log;
    private readonly string _harnessVersion;
    private readonly string _launchDir;
    private int _launched;

    public Batch Batch { get; }
    public string Id => Batch.Id;
    /// <summary>The item this execution was asked for alone; an execution naming one item is the pilot.</summary>
    public string? ItemFilter { get; }
    /// <summary>The number every call of this execution carries: one more than the last execution's.</summary>
    public int Execution { get; }
    public bool Paused { get; private set; }
    public bool StopRequested { get; private set; }
    public bool Completed { get; private set; }
    public DateTimeOffset? NotBefore { get; set; }
    public bool Started { get; private set; }
    public int InFlight { get { lock (_lock) return _running.Count; } }
    public int Launched => _launched;

    public event Action? Changed;
    public event Action<string, int>? StreamAdvanced;

    public BatchRunner(Batch batch, string? itemFilter, IChildLauncher launcher, ILaunchGate gate, Action<string> log, string harnessVersion, string launchDir)
    {
        Batch = batch;
        ItemFilter = itemFilter;
        _launcher = launcher;
        _gate = gate;
        _log = log;
        _harnessVersion = harnessVersion;
        _launchDir = launchDir;
        var calls = CallsFile.Read(batch.Definition.CallsPath);
        _calls = new List<CallEntry>(calls.Entries);
        Execution = calls.Executions + 1;
    }

    /// <summary>
    /// Everything the loop needs that is not the loop: the batch, its items on disk, the
    /// launch folder's invariants, the filter. Returns the error text instead of a runner when
    /// the batch is unusable, so the CLI and the host report the same message.
    /// </summary>
    public static (BatchRunner? Runner, string? Error) Create(string definitionPath, string workingDir, string? itemFilter, string launchDir,
        IChildLauncher launcher, ILaunchGate gate, Action<string> log, string harnessVersion)
    {
        var (batch, error) = Batch.Load(definitionPath, workingDir);
        if (batch is null) return (null, error);
        var missing = batch.MissingItems();
        if (missing.Count > 0) return (null, $"{missing.Count} item(s) have no body under items/ (first: {missing[0]}); the itemizer regenerates them");
        var launchError = Batch.CheckLaunchDir(launchDir, definitionPath);
        if (launchError is not null) return (null, launchError);
        if (itemFilter is not null && !batch.Items.Contains(itemFilter))
            return (null, $"--item \"{itemFilter}\" is not in the index.");
        return (new BatchRunner(batch, itemFilter, launcher, gate, log, harnessVersion, Path.GetFullPath(launchDir)), null);
    }

    // --- harness commands: how the batch runs, never what a call is ---

    public void Pause() { lock (_lock) Paused = true; _log($"[{Id}] paused"); Changed?.Invoke(); }
    public void Resume() { lock (_lock) Paused = false; _log($"[{Id}] resumed"); Changed?.Invoke(); }
    public void StopAfterInFlight() { lock (_lock) StopRequested = true; _log($"[{Id}] stop requested — finishing in-flight calls"); Changed?.Invoke(); }

    public bool Cancel(string item)
    {
        RunningCall? r;
        lock (_lock)
        {
            if (!_running.TryGetValue(item, out r)) return false;
            _cancelled.Add(item);
        }
        _log($"[{Id}] cancelling {item}" + (r.Handle is { } h ? $" (PID {h.Pid})" : ""));
        r.Handle?.Kill();
        Changed?.Invoke();
        return true;
    }

    // --- state for snapshots ---

    public IReadOnlyList<CallEntry> CallsSnapshot() { lock (_lock) return _calls.ToList(); }
    public IReadOnlyList<RunningCall> RunningSnapshot() { lock (_lock) return _running.Values.ToList(); }
    public bool HasSucceeded(string item) { lock (_lock) return _calls.Any(c => c.Item == item && c.Succeeded); }
    public string? HoldReason() => _gate.HoldReason(this);

    /// <summary>The items this execution will call: those in the filter, if any, without a successful call.</summary>
    public IReadOnlyList<string> Pending()
    {
        lock (_lock)
            return Batch.Items.Where(i => (ItemFilter is null || i == ItemFilter) && !_calls.Any(c => c.Item == i && c.Succeeded)).ToList();
    }

    /// <summary>What an execution will do, from the calls file: the items it will call and the ones it skips as answered.</summary>
    public string Summary()
    {
        var scope = ItemFilter is null ? Batch.Items : [ItemFilter];
        var pending = Pending().Count;
        return $"{scope.Count} item(s) — {pending} to call, {scope.Count - pending} skipped as answered" + (ItemFilter is null ? "" : " (pilot)");
    }

    // --- the loop ---

    public async Task RunAsync(CancellationToken ct)
    {
        Started = true;
        var tasks = new List<Task>();
        try
        {
            while (!ct.IsCancellationRequested)
            {
                string? item = null;
                bool anyPending;
                lock (_lock)
                {
                    var next = Batch.Items.FirstOrDefault(i => (ItemFilter is null || i == ItemFilter) && !_running.ContainsKey(i) && !_calls.Any(c => c.Item == i && c.Succeeded) && !_calls.Any(c => c.Item == i && c.Call == Execution));
                    anyPending = next is not null;
                    if (next is not null && !Paused && !StopRequested) item = next;
                }

                if (item is null)
                {
                    if (InFlight == 0 && (!anyPending || StopRequested)) break;
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

                var theItem = item;
                var streamPath = Path.Combine(Batch.Definition.AttemptsDir, theItem, $"call-{Execution}", "stream.jsonl");
                lock (_lock) _running[theItem] = new RunningCall(theItem, Execution, DateTimeOffset.UtcNow, streamPath, null);
                Interlocked.Increment(ref _launched);
                Changed?.Invoke();
                tasks.Add(Task.Run(async () =>
                {
                    try { await RunOne(theItem, ct); }
                    finally
                    {
                        lock (_lock) { _running.Remove(theItem); _cancelled.Remove(theItem); }
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
            _log($"[{Id}] {(ct.IsCancellationRequested ? "stopped" : StopRequested ? "stopped after in-flight" : "complete")} — {_launched} call(s) this execution");
            Changed?.Invoke();
        }
    }

    public void KillAll()
    {
        List<RunningCall> running;
        lock (_lock) { running = _running.Values.ToList(); foreach (var r in running) _cancelled.Add(r.Item); }
        foreach (var r in running) r.Handle?.Kill();
    }

    private async Task RunOne(string item, CancellationToken ct)
    {
        var callDir = Path.Combine(Batch.Definition.AttemptsDir, item, $"call-{Execution}");
        Directory.CreateDirectory(callDir);
        var streamPath = Path.Combine(callDir, "stream.jsonl");
        var systemPromptPath = Path.Combine(callDir, "system-prompt.md");

        var plan = Batch.Compose(item);
        await File.WriteAllTextAsync(systemPromptPath, plan.SystemPrompt, new UTF8Encoding(false), ct);
        await File.WriteAllTextAsync(Path.Combine(callDir, "item.md"), plan.ItemText, new UTF8Encoding(false), ct);

        _log($"[{Id}] {item}: call {Execution}, model {Batch.Model}{(Batch.Effort is null ? "" : " effort " + Batch.Effort)}, {plan.Characters:N0} chars (prompt {plan.PromptHash[..12]}…)");

        var start = DateTimeOffset.UtcNow;
        var request = new ChildRequest(item, Batch.BuildArgs(systemPromptPath), plan.ItemText, streamPath, _launchDir, _gate.IdleLimit);
        var exitCode = await _launcher.LaunchAsync(request,
            handle => { lock (_lock) if (_running.TryGetValue(item, out var r)) _running[item] = r with { Handle = handle }; Changed?.Invoke(); },
            () => StreamAdvanced?.Invoke(item, Execution),
            ct);
        var end = DateTimeOffset.UtcNow;

        bool cancelled;
        lock (_lock) cancelled = _cancelled.Contains(item);
        if (cancelled && exitCode != 0) exitCode = -4;

        var summary = File.Exists(streamPath)
            ? StreamEvents.ParseResult(await File.ReadAllTextAsync(streamPath, ct))
            : ResultSummary.Empty;

        string check;
        if (exitCode == -3) check = "idle";
        else if (exitCode == -4) check = "cancelled";
        else if (exitCode != 0) check = $"exit {exitCode}";
        else if (summary.StructuredOutput is null) check = "no structured output";
        else
        {
            Directory.CreateDirectory(Batch.Definition.ResultsDir);
            var rendered = Batch.Render(summary.StructuredOutput);
            await File.WriteAllTextAsync(Batch.ResultPath(item), rendered, new UTF8Encoding(false), ct);
            var (_, problems) = ResultFile.Parse(Batch.Directions, rendered);
            check = problems.Count == 0 ? CallEntry.Ok : "malformed: " + problems[0];
        }

        var entry = new CallEntry(item, Execution, Batch.Model, Batch.Effort, _harnessVersion, plan.DirectionsHash, plan.ItemHash, plan.PromptHash,
            start.ToString("o"), end.ToString("o"), exitCode, check, summary.CostUsd, summary.Turns, summary.SessionId, ItemFilter is not null);
        Record(entry);
        _log($"[{Id}] {item}: exit {exitCode}, check: {check}, {(end - start).TotalSeconds:F0}s" +
             (summary.CostUsd is { } cost ? $", ${cost:F3}" : "") + (summary.Turns is { } turns ? $", {turns} turn(s)" : ""));
    }

    private void Record(CallEntry entry)
    {
        lock (_lock)
        {
            var path = Batch.Definition.CallsPath;
            if (!File.Exists(path))
                File.WriteAllText(path, CallsFile.RenderHead(Batch.Name, Batch.Definition.Hash), new UTF8Encoding(false));
            File.AppendAllText(path, CallsFile.RenderEntry(entry), new UTF8Encoding(false));
            _calls.Add(entry);
        }
    }
}
