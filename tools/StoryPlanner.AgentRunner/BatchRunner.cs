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
    /// <summary>Take a slot past the ceiling and the cap, for the queue jump; released like any other, and counted from then on. False only when the host is shutting down.</summary>
    bool TryForce(BatchRunner batch);
    void Release(BatchRunner batch);
    /// <summary>Why a launch is being held, for the page; null when nothing holds it.</summary>
    string? HoldReason(BatchRunner batch);
    /// <summary>The idle limit every call runs under, the host's setting.</summary>
    TimeSpan IdleLimit { get; }
    /// <summary>What a finished call's stream said about the subscription windows: the live figure the cap gates on, and a refusal the gate holds on.</summary>
    void Observe(RateLimitReading reading, string batchId, string item);
}

/// <summary>No global constraint and a long idle limit. The CLI's serverless paths and tests use it.</summary>
public sealed class OpenGate : ILaunchGate
{
    public bool TryAcquire(BatchRunner batch) => true;
    public bool TryForce(BatchRunner batch) => true;
    public void Release(BatchRunner batch) { }
    public string? HoldReason(BatchRunner batch) => null;
    public TimeSpan IdleLimit { get; init; } = TimeSpan.FromMinutes(10);
    public void Observe(RateLimitReading reading, string batchId, string item) { }
}

public sealed record RunningCall(string Item, int Call, DateTimeOffset StartUtc, string StreamPath, IChildHandle? Handle);

/// <summary>
/// One execution of a batch (decisions.md, "executing a batch is one call per item still
/// without a result"): one call for every item of the index that has no successful call yet.
/// The call's number is the execution's. Nothing here changes what a call is, and no
/// execution is a special shape: the method's pilot is an execution paused after its first
/// launches. The loop takes its launcher and its gate from outside,
/// walks the index in its order or, under random order, in one shuffle of it drawn at
/// construction, and accepts harness commands (pause, resume, stop after in-flight, cancel
/// an item, the queue jump) while it runs. The calls file is the batch's state and this
/// class is its only writer. A launch the API refused at the subscription limit before any
/// work (d-2026-09-15-4) is not a call: nothing is recorded, the gate is told and holds
/// every launch until the window resets, and the item stays pending in this execution, so
/// the loop calls it again once the hold lifts.
/// </summary>
public sealed class BatchRunner
{
    private readonly object _lock = new();
    private readonly List<CallEntry> _calls;
    private readonly Dictionary<string, RunningCall> _running = new(StringComparer.Ordinal);
    private readonly HashSet<string> _cancelled = new(StringComparer.Ordinal);
    private readonly List<Task> _forced = new();
    private CancellationToken _ct;
    private readonly IChildLauncher _launcher;
    private readonly ILaunchGate _gate;
    private readonly Action<string> _log;
    private readonly string _harnessVersion;
    private readonly string _launchDir;
    private int _launched;
    private int _refused;

    public Batch Batch { get; }
    public string Id => Batch.Id;
    /// <summary>The number every call of this execution carries: one more than the last execution's.</summary>
    public int Execution { get; }
    /// <summary>Whether this execution walks the index in a shuffle rather than its order — an execution's setting; no call is different for it.</summary>
    public bool RandomOrder { get; }
    /// <summary>The sequence the next item is picked from: the index's order, or one shuffle of it drawn at construction under <see cref="RandomOrder"/>.</summary>
    public IReadOnlyList<string> Order { get; }
    public bool Paused { get; private set; }
    public bool StopRequested { get; private set; }
    public bool Completed { get; private set; }
    public DateTimeOffset? NotBefore { get; set; }
    public bool Started { get; private set; }
    public int InFlight { get { lock (_lock) return _running.Count; } }
    public int Launched => _launched;
    /// <summary>Launches the API refused at the subscription limit before any work — turned away, not calls, not recorded.</summary>
    public int Refused => _refused;

    public event Action? Changed;
    public event Action<string, int>? StreamAdvanced;

    public BatchRunner(Batch batch, IChildLauncher launcher, ILaunchGate gate, Action<string> log, string harnessVersion, string launchDir, bool randomOrder = false)
    {
        Batch = batch;
        _launcher = launcher;
        _gate = gate;
        _log = log;
        _harnessVersion = harnessVersion;
        _launchDir = launchDir;
        RandomOrder = randomOrder;
        var order = batch.Items.ToArray();
        if (randomOrder) Random.Shared.Shuffle(order);
        Order = order;
        var calls = CallsFile.Read(batch.Definition.CallsPath);
        _calls = new List<CallEntry>(calls.Entries);
        Execution = calls.Executions + 1;
    }

    /// <summary>
    /// Everything the loop needs that is not the loop: the batch, its items on disk, the
    /// launch folder's invariants. Returns the error text instead of a runner when the batch
    /// is unusable, so the CLI and the host report the same message.
    /// </summary>
    public static (BatchRunner? Runner, string? Error) Create(string definitionPath, string workingDir, string launchDir,
        IChildLauncher launcher, ILaunchGate gate, Action<string> log, string harnessVersion, bool randomOrder = false)
    {
        var (batch, error) = Batch.Load(definitionPath, workingDir);
        if (batch is null) return (null, error);
        var missing = batch.MissingItems();
        if (missing.Count > 0) return (null, $"{missing.Count} item(s) have no body under items/ (first: {missing[0]}); the itemizer or collator regenerates them");
        var launchError = Batch.CheckLaunchDir(launchDir, definitionPath);
        if (launchError is not null) return (null, launchError);
        return (new BatchRunner(batch, launcher, gate, log, harnessVersion, Path.GetFullPath(launchDir), randomOrder), null);
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

    // --- the queue jump ---

    /// <summary>Why <see cref="CallNow"/> would refuse the item; null when it is callable now. Called under the lock.</summary>
    private string? WhyNotCallable(string item)
    {
        if (!Started || Completed) return "not executing";
        if (StopRequested) return "stop requested — nothing more is launched";
        if (!Batch.Items.Contains(item)) return $"{item} is not in the index";
        if (_running.ContainsKey(item)) return $"{item} is already running";
        if (_calls.Any(c => c.Item == item && c.Succeeded)) return $"{item} has answered";
        if (_calls.Any(c => c.Item == item && c.Call == Execution)) return $"{item} was already called in execution {Execution}; the next execute-batch calls it again";
        return null;
    }

    /// <summary>Whether the queue jump would take the item right now: pending in this execution and not yet called by it.</summary>
    public bool CanCallNow(string item) { lock (_lock) return WhyNotCallable(item) is null; }

    /// <summary>
    /// The queue jump (Brian, 2026-09-15: "It's just a queue jump", "It should go past the
    /// gate"): one item pending in this execution, called now, past the host's ceiling and
    /// cap, taking a slot the gate counts from then on. Harness control: the call is composed,
    /// recorded and checked exactly as the loop's would be, under this execution's number;
    /// an item already called by this execution is refused, so one call per item per
    /// execution holds. Refused once stop is requested; allowed under pause,
    /// which holds the loop and not the hand. The jump goes to the log and never to the calls
    /// file, like the order of calls. Returns why it was refused, or null.
    /// </summary>
    public string? CallNow(string item)
    {
        lock (_lock)
        {
            if (WhyNotCallable(item) is { } why) return why;
            _running[item] = Starting(item);
        }
        if (!_gate.TryForce(this))
        {
            lock (_lock) _running.Remove(item);
            return "the host is shutting down";
        }
        _log($"[{Id}] {item}: called now, past the gate");
        var task = Launch(item, _ct);
        lock (_lock) _forced.Add(task);
        return null;
    }

    // --- state for snapshots ---

    public IReadOnlyList<CallEntry> CallsSnapshot() { lock (_lock) return _calls.ToList(); }
    public IReadOnlyList<RunningCall> RunningSnapshot() { lock (_lock) return _running.Values.ToList(); }
    public bool HasSucceeded(string item) { lock (_lock) return _calls.Any(c => c.Item == item && c.Succeeded); }
    public string? HoldReason() => _gate.HoldReason(this);

    /// <summary>The items this execution will call: those of the index without a successful call.</summary>
    public IReadOnlyList<string> Pending()
    {
        lock (_lock)
            return Batch.Items.Where(i => !_calls.Any(c => c.Item == i && c.Succeeded)).ToList();
    }

    /// <summary>What an execution will do, from the calls file: the items it will call and the ones it skips as answered.</summary>
    public string Summary()
    {
        var pending = Pending().Count;
        return $"{Batch.Items.Count} item(s) — {pending} to call, {Batch.Items.Count - pending} skipped as answered" + (RandomOrder ? ", random order" : "");
    }

    // --- the loop ---

    public async Task RunAsync(CancellationToken ct)
    {
        Started = true;
        _ct = ct;
        var tasks = new List<Task>();
        try
        {
            while (!ct.IsCancellationRequested)
            {
                string? item = null;
                bool anyPending;
                lock (_lock)
                {
                    var next = Order.FirstOrDefault(i => !_running.ContainsKey(i) && !_calls.Any(c => c.Item == i && c.Succeeded) && !_calls.Any(c => c.Item == i && c.Call == Execution));
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
                lock (_lock)
                {
                    // A queue jump may have taken this item between the pick and here.
                    if (_running.ContainsKey(theItem) || _calls.Any(c => c.Item == theItem && c.Call == Execution)) { _gate.Release(this); continue; }
                    _running[theItem] = Starting(theItem);
                }
                tasks.Add(Launch(theItem, ct));
            }
            List<Task> forced;
            lock (_lock) forced = _forced.ToList();
            await Task.WhenAll(tasks.Concat(forced).Select(t => t.ContinueWith(_ => { })));
        }
        finally
        {
            Completed = true;
            _log($"[{Id}] {(ct.IsCancellationRequested ? "stopped" : StopRequested ? "stopped after in-flight" : "complete")} — {_launched - _refused} call(s) this execution" + (_refused > 0 ? $", {_refused} launch(es) refused at the limit and not recorded" : ""));
            WriteTallyIfComplete();
            Changed?.Invoke();
        }
    }

    private RunningCall Starting(string item) =>
        new(item, Execution, DateTimeOffset.UtcNow, Path.Combine(Batch.Definition.AttemptsDir, item, $"call-{Execution}", "stream.jsonl"), null);

    /// <summary>One child on its own task: the item's call, then its running entry dropped and its slot released. The caller has put the item in the running set under the lock.</summary>
    private Task Launch(string item, CancellationToken ct)
    {
        Interlocked.Increment(ref _launched);
        Changed?.Invoke();
        return Task.Run(async () =>
        {
            try { await RunOne(item, ct); }
            finally
            {
                lock (_lock) { _running.Remove(item); _cancelled.Remove(item); }
                _gate.Release(this);
                Changed?.Invoke();
            }
        }, CancellationToken.None);
    }

    /// <summary>
    /// The host writes the tally when the last item has a successful call and never again
    /// (decisions.md, "the host writes the tally at completion"): no session step to forget,
    /// and every batch's tally has the same shape. An execution that left items unanswered
    /// writes none.
    /// </summary>
    private void WriteTallyIfComplete()
    {
        try
        {
            if (File.Exists(Batch.Definition.TallyPath)) return;
            if (!Batch.Items.All(HasSucceeded)) return;
            var (ok, message) = Tally.Write(Batch);
            _log($"[{Id}] {(ok ? "tally written — " + message : message)}");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or FormatException)
        {
            _log($"[{Id}] tally not written: {ex.Message}; run tally-batch on the definition");
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

        var streamText = File.Exists(streamPath) ? await File.ReadAllTextAsync(streamPath, ct) : null;
        var summary = streamText is null ? ResultSummary.Empty : StreamEvents.ParseResult(streamText);

        // The harness reports the subscription windows on every call; the host reads the live
        // figure from it. A launch refused at the limit before any work is not a call: the gate
        // holds until the reset, nothing is recorded, and the loop calls the item again after.
        var rateLimit = streamText is null ? null : StreamEvents.ReadRateLimit(streamText);
        if (rateLimit is not null) _gate.Observe(rateLimit, Id, item);
        if (rateLimit is { Rejected: true } && summary.StructuredOutput is null && !cancelled)
        {
            Interlocked.Increment(ref _refused);
            var until = rateLimit.RejectedResetsAt ?? rateLimit.FiveHourResetsAt;
            _log($"[{Id}] {item}: launch refused at the {rateLimit.RejectedWindow ?? "subscription"} limit — not a call, nothing recorded; the host holds until {until.ToLocalTime():HH:mm}, then the loop calls it again");
            return;
        }

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
            start.ToString("o"), end.ToString("o"), exitCode, check, summary.CostUsd, summary.Turns, summary.SessionId);
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
