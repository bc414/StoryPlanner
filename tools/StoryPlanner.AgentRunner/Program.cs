using System.Diagnostics;
using System.Text;
using System.Text.Json;
using StoryPlanner.AgentRunner;

// Launches autonomous-agent jobs — classifiers, investigators, auditors, referees — as
// `claude -p` children, up to maxParallel at once, from a launch directory OUTSIDE the repo
// with every input passed explicitly and no transcript persisted. Replaces AnalysisRunner
// (deleted 2026-09-03), whose infinite retry of a failing job from the repo root left 9,245
// transcripts in the StoryPlanner project history on 2026-08-27.
//
//   dotnet run --project tools/StoryPlanner.AgentRunner -- <run>/jobs.json [--dry-run] [--job ID]
//   dotnet run --project tools/StoryPlanner.AgentRunner -- split <document.md> <items-dir>
//
// The folder holding the job file is the RUN FOLDER: relative paths in the job file resolve
// against it, and the runner writes there — ledger.jsonl (the queue: one row per attempt) and
// attempts/<id>/attempt-<n>/prompt.md and stream.jsonl (the inside of the session, teed live
// as the child emits it). A job is pending until an attempt succeeds (exit 0, output present,
// required markers each present once) or maxAttempts is reached; a job at maxAttempts is
// FAILED and never relaunched. An attempt past timeoutMinutes is killed and recorded as
// failed. Ctrl+C kills every child and exits; there is no other control surface — a head,
// when one is wanted, hosts this loop rather than steering it through files.

if (args.Length > 0 && args[0] == "split")
    return RunSplit(args.Skip(1).ToArray());

var positional = args.Where(a => !a.StartsWith("--")).ToList();
var dryRun = args.Contains("--dry-run");
var jobArgIdx = Array.IndexOf(args, "--job");
string? onlyJob = jobArgIdx >= 0 && jobArgIdx + 1 < args.Length ? args[jobArgIdx + 1] : null;
if (onlyJob is not null) positional.Remove(onlyJob);

if (positional.Count != 1)
{
    Console.Error.WriteLine("Usage: dotnet run -- <run>/jobs.json [--dry-run] [--job ID]  |  split <document.md> <items-dir>");
    return 2;
}

var jobFilePath = Path.GetFullPath(positional[0]);
if (!File.Exists(jobFilePath))
{
    Console.Error.WriteLine($"Job file not found: {jobFilePath}");
    return 2;
}
var runDir = Path.GetDirectoryName(jobFilePath)!;

JobFile jobFile;
IReadOnlyList<ResolvedJob> jobs;
try
{
    jobFile = RunnerPlan.ParseJobFile(File.ReadAllText(jobFilePath));
    jobs = RunnerPlan.Resolve(jobFile, runDir);
}
catch (Exception ex) when (ex is JsonException or InvalidOperationException)
{
    Console.Error.WriteLine($"Job file unusable: {ex.Message}");
    return 2;
}

if (string.IsNullOrWhiteSpace(jobFile.LaunchDir) || !Directory.Exists(jobFile.LaunchDir))
{
    Console.Error.WriteLine($"launchDir does not exist: {jobFile.LaunchDir}");
    return 2;
}
var launchDir = Path.GetFullPath(jobFile.LaunchDir);
var repoRoot = FindRepoRoot(jobFilePath);
if (repoRoot is not null && IsSameOrUnder(launchDir, repoRoot))
{
    Console.Error.WriteLine($"launchDir must be OUTSIDE the repo ({repoRoot}) — that is the whole point.");
    return 2;
}
foreach (var forbidden in new[] { "CLAUDE.md", ".claude", ".mcp.json" })
{
    if (File.Exists(Path.Combine(launchDir, forbidden)) || Directory.Exists(Path.Combine(launchDir, forbidden)))
    {
        Console.Error.WriteLine($"launchDir contains {forbidden}; it must carry no instruction stack of its own.");
        return 2;
    }
}

string? mcpConfigPath = null;
if (!string.IsNullOrWhiteSpace(jobFile.McpConfig))
{
    mcpConfigPath = Path.GetFullPath(jobFile.McpConfig, runDir);
    if (!File.Exists(mcpConfigPath))
    {
        Console.Error.WriteLine($"mcpConfig not found: {mcpConfigPath}");
        return 2;
    }
}

if (onlyJob is not null)
{
    jobs = jobs.Where(j => j.Id == onlyJob).ToList();
    if (jobs.Count == 0)
    {
        Console.Error.WriteLine($"--job \"{onlyJob}\" is not in the job file.");
        return 2;
    }
}

var ledgerPath = Path.Combine(runDir, "ledger.jsonl");
var ledgerLock = new object();
var ledger = new List<LedgerRow>(File.Exists(ledgerPath) ? RunnerPlan.ParseLedger(File.ReadLines(ledgerPath)) : []);
var claudeJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude.json");

Console.WriteLine($"agent runner ({(dryRun ? "DRY RUN" : "LIVE")})");
Console.WriteLine($"  run       : {runDir}");
Console.WriteLine($"  launchDir : {launchDir}");
Console.WriteLine($"  ledger    : {ledgerPath} ({ledger.Count} attempt(s) recorded)");
Console.WriteLine($"  cap       : {jobFile.UtilizationCap}% five-hour; maxAttempts {jobFile.MaxAttempts}; timeout {jobFile.TimeoutMinutes} min; maxParallel {jobFile.MaxParallel}");
var util = ReadUtilization(claudeJsonPath);
if (util is not null)
    Console.WriteLine($"  utilization (cached by Claude Code, may be stale): {util.Value.percent}% five-hour, resets {util.Value.resetsAt.ToLocalTime():HH:mm}");
Console.WriteLine();

foreach (var j in jobs)
{
    var state = RunnerPlan.StateOf(j, ledger, jobFile.MaxAttempts);
    var attempts = RunnerPlan.AttemptsOf(j, ledger);
    Console.WriteLine($"  [{state,-9}] {j.Id}  model={j.Model} mcp={(j.Mcp ? "yes" : "no")} attempts={attempts}/{jobFile.MaxAttempts}  item: {j.Item}");
}
Console.WriteLine();

if (dryRun)
{
    // Compose every pending prompt so a missing input or a bad path fails here, not mid-batch.
    foreach (var j in jobs.Where(j => RunnerPlan.StateOf(j, ledger, jobFile.MaxAttempts) == JobState.Pending))
    {
        try
        {
            var p = RunnerPlan.ComposePrompt(j, File.ReadAllText);
            Console.WriteLine($"  {j.Id}: prompt {p.Text.Length:N0} chars, {p.ProtocolShas.Count} protocol(s), {p.InputShas.Count} input(s), {j.RequireOnce.Count} required marker(s)");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"  {j.Id}: CANNOT COMPOSE — {ex.Message}");
        }
    }
    Console.WriteLine("DRY RUN — nothing launched.");
    return 0;
}

var harnessVersion = await ReadHarnessVersion();
Console.WriteLine($"  harness: {harnessVersion}");
Console.WriteLine();

var running = new Dictionary<string, Process?>();
var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    Console.WriteLine("\n  Ctrl+C received — shutting down...");
    cts.Cancel();
    lock (ledgerLock)
    {
        foreach (var (id, p) in running)
        {
            if (p is null || p.HasExited) continue;
            Console.WriteLine($"  Killing claude process tree for {id} (PID {p.Id})...");
            try { p.Kill(entireProcessTree: true); } catch { }
        }
    }
};

var launched = 0;
var tasks = new List<Task>();
var semaphore = new SemaphoreSlim(jobFile.MaxParallel);

while (!cts.Token.IsCancellationRequested)
{
    // The cap gates every launch, not only an idle runner: over the cap with jobs in flight,
    // wait for them; over the cap with nothing in flight, sleep to the reset.
    var utilization = ReadUtilization(claudeJsonPath);
    if (utilization is not null && utilization.Value.percent >= jobFile.UtilizationCap)
    {
        var wait = utilization.Value.resetsAt - DateTimeOffset.UtcNow;
        if (wait > TimeSpan.Zero)
        {
            if (running.Count > 0)
            {
                try { await Task.WhenAny(Task.WhenAny(tasks), Task.Delay(TimeSpan.FromSeconds(10), cts.Token)); }
                catch (OperationCanceledException) { break; }
                tasks.RemoveAll(t => t.IsCompleted);
                continue;
            }
            Console.WriteLine($"  Utilization {utilization.Value.percent}% >= cap {jobFile.UtilizationCap}%; sleeping until " +
                              $"{utilization.Value.resetsAt.ToLocalTime():HH:mm} ({wait.TotalMinutes:F0} min). Ctrl+C to stop.");
            try { await Task.Delay(wait + TimeSpan.FromMinutes(1), cts.Token); }
            catch (OperationCanceledException) { break; }
            continue;
        }
    }

    ResolvedJob? job;
    lock (ledgerLock)
    {
        var available = jobs.Where(j => !running.ContainsKey(j.Id)).ToList();
        job = RunnerPlan.NextPending(available, ledger, jobFile.MaxAttempts);
    }

    if (job is null)
    {
        if (running.Count == 0)
        {
            Console.WriteLine($"No pending jobs. Launched {launched} this run.");
            break;
        }
        // Everything pending is in flight: wait for one to finish, then look again.
        try { await Task.WhenAny(Task.WhenAny(tasks), Task.Delay(TimeSpan.FromSeconds(2), cts.Token)); }
        catch (OperationCanceledException) { break; }
        tasks.RemoveAll(t => t.IsCompleted);
        continue;
    }

    try { await semaphore.WaitAsync(cts.Token); }
    catch (OperationCanceledException) { break; }

    var attempt = RunnerPlan.AttemptsOf(job, ledger) + 1;
    lock (ledgerLock) running[job.Id] = null;
    launched++;
    var theJob = job;
    tasks.Add(Task.Run(async () =>
    {
        try { await RunOne(theJob, attempt); }
        finally
        {
            lock (ledgerLock) running.Remove(theJob.Id);
            semaphore.Release();
        }
    }));
}

await Task.WhenAll(tasks.Select(t => t.ContinueWith(_ => { })));
if (cts.Token.IsCancellationRequested)
    Console.WriteLine($"Stopped. Launched {launched} this run.");
return 0;

async Task RunOne(ResolvedJob job, int attempt)
{
    var attemptDir = Path.Combine(runDir, "attempts", job.Id, $"attempt-{attempt}");
    Directory.CreateDirectory(attemptDir);
    var streamPath = Path.Combine(attemptDir, "stream.jsonl");

    ComposedPrompt prompt;
    try
    {
        prompt = RunnerPlan.ComposePrompt(job, File.ReadAllText);
    }
    catch (IOException ex)
    {
        Console.Error.WriteLine($"  {job.Id}: cannot compose prompt — {ex.Message}");
        Record(new LedgerRow(job.Id, attempt, job.Model, harnessVersion, "", [], [],
            DateTimeOffset.UtcNow.ToString("o"), DateTimeOffset.UtcNow.ToString("o"), -1, "", false, null, null, null, "cannot compose prompt"));
        return;
    }
    await File.WriteAllTextAsync(Path.Combine(attemptDir, "prompt.md"), prompt.Text);

    // --restricted confines file tools to the working directories, and --add-dir of a path
    // that does not exist yet is silently dropped — so the output directory must exist first.
    var outputDir = Path.GetDirectoryName(job.OutputPath);
    if (!string.IsNullOrEmpty(outputDir)) Directory.CreateDirectory(outputDir);

    Console.WriteLine($"[{job.Id}] attempt {attempt}/{jobFile.MaxAttempts}, model {job.Model}, prompt {prompt.Text.Length:N0} chars (sha256 {prompt.Sha256[..12]}…), timeout {job.TimeoutMinutes} min");

    var start = DateTimeOffset.UtcNow;
    var exitCode = await RunClaude(job, prompt.Text, streamPath, TimeSpan.FromMinutes(job.TimeoutMinutes), p =>
    {
        lock (ledgerLock) running[job.Id] = p;
    });
    var end = DateTimeOffset.UtcNow;
    var outputExists = File.Exists(job.OutputPath);
    var outputCheck = exitCode == -3 ? "timed out"
        : outputExists ? RunnerPlan.CheckOutput(await File.ReadAllTextAsync(job.OutputPath), job.RequireOnce)
        : "no output file";
    var summary = File.Exists(streamPath)
        ? RunnerPlan.ParseResultSummary(await File.ReadAllTextAsync(streamPath))
        : new ResultSummary(null, null, null, null);

    var row = new LedgerRow(job.Id, attempt, job.Model, harnessVersion, prompt.Sha256, prompt.ProtocolShas, prompt.InputShas,
        start.ToString("o"), end.ToString("o"), exitCode, streamPath, outputExists, summary.CostUsd, summary.Turns, summary.SessionId, outputCheck);
    var state = Record(row);
    Console.WriteLine($"  [{job.Id}] exit {exitCode}, output {(outputExists ? "present" : "MISSING")}, check: {outputCheck}, {(end - start).TotalSeconds:F0}s" +
                      (summary.CostUsd is { } cost ? $", ${cost:F3}" : "") +
                      (summary.Turns is { } turns ? $", {turns} turn(s)" : "") +
                      $" → {state}" + (state == JobState.Failed ? " — not retried" : ""));
}

JobState Record(LedgerRow row)
{
    lock (ledgerLock)
    {
        File.AppendAllText(ledgerPath, RunnerPlan.SerializeLedgerRow(row) + "\n");
        ledger.Add(row);
        var job = jobs.First(j => j.Id == row.JobId);
        return RunnerPlan.StateOf(job, ledger, jobFile.MaxAttempts);
    }
}

// Exit codes the runner itself assigns: -1 could not start, -2 cancelled by Ctrl+C, -3 timed out.
async Task<int> RunClaude(ResolvedJob job, string promptText, string streamPath, TimeSpan timeout, Action<Process> track)
{
    var psi = new ProcessStartInfo
    {
        FileName = "claude",
        WorkingDirectory = launchDir,
        UseShellExecute = false,
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        StandardInputEncoding = new UTF8Encoding(false),
        StandardOutputEncoding = Encoding.UTF8,
    };
    foreach (var a in RunnerPlan.BuildArgs(job, mcpConfigPath)) psi.ArgumentList.Add(a);

    Process? process = null;
    try
    {
        process = Process.Start(psi);
        if (process is null) return -1;
        track(process);

        // The prompt is the whole stdin — no positional prompt, so the hashed document is
        // exactly what the agent received (and Windows argument-length limits never apply).
        await process.StandardInput.WriteAsync(promptText);
        process.StandardInput.Close();

        // Tee stdout to stream.jsonl line by line as it arrives: the inside of the session, live.
        var stdoutTask = Task.Run(async () =>
        {
            await using var stream = new FileStream(streamPath, FileMode.Create, FileAccess.Write, FileShare.Read);
            await using var writer = new StreamWriter(stream, new UTF8Encoding(false));
            while (await process.StandardOutput.ReadLineAsync() is { } line)
            {
                await writer.WriteLineAsync(line);
                await writer.FlushAsync();
            }
        });
        var stderrTask = Task.Run(async () =>
        {
            while (await process.StandardError.ReadLineAsync() is { } line)
                Console.Error.WriteLine($"  ! [{job.Id}] {line}");
        });

        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, timeoutCts.Token);
        try
        {
            await process.WaitForExitAsync(linked.Token);
        }
        catch (OperationCanceledException)
        {
            try { process.Kill(entireProcessTree: true); } catch { }
            if (timeoutCts.IsCancellationRequested && !cts.Token.IsCancellationRequested)
            {
                Console.Error.WriteLine($"  {job.Id}: timed out after {timeout.TotalMinutes:F0} min — process tree killed");
                try { await stdoutTask; } catch { }
                return -3;
            }
            try { await stdoutTask; } catch { }
            return -2;
        }
        await stdoutTask;
        await stderrTask;
        return process.ExitCode;
    }
    catch (Exception ex) when (ex is not OperationCanceledException)
    {
        Console.Error.WriteLine($"  Failed to start claude: {ex.Message}");
        return -1;
    }
    finally
    {
        process?.Dispose();
    }
}

// --- split verb ---

static int RunSplit(string[] a)
{
    if (a.Length != 2)
    {
        Console.Error.WriteLine("Usage: split <document.md> <items-dir>");
        return 2;
    }
    var doc = Path.GetFullPath(a[0]);
    var outDir = Path.GetFullPath(a[1]);
    if (!File.Exists(doc))
    {
        Console.Error.WriteLine($"Document not found: {doc}");
        return 2;
    }
    if (Directory.Exists(outDir) && Directory.EnumerateFileSystemEntries(outDir).Any())
    {
        Console.Error.WriteLine($"Items directory is not empty: {outDir} — a split is done once per run; start a new run folder to split again.");
        return 2;
    }
    Directory.CreateDirectory(outDir);
    var text = File.ReadAllText(doc);
    var units = UnitSplitter.Split(text);
    foreach (var u in units)
        File.WriteAllText(Path.Combine(outDir, u.Id + ".md"), UnitSplitter.RenderItem(u), new UTF8Encoding(false));
    var manifest = UnitSplitter.RenderManifest(Path.GetFileName(doc), RunnerPlan.Sha256Hex(text), units);
    File.WriteAllText(Path.Combine(outDir, "manifest.md"), manifest, new UTF8Encoding(false));
    Console.WriteLine($"{units.Count} units from {doc} → {outDir} (manifest.md beside them)");
    foreach (var g in units.GroupBy(u => u.Section))
        Console.WriteLine($"  {g.Count(),3}  {g.Key}");
    return 0;
}

// --- Helpers ---

static string? FindRepoRoot(string fromPath)
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
static bool IsSameOrUnder(string path, string root)
{
    var p = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
    var r = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
    return p.Equals(r, StringComparison.OrdinalIgnoreCase)
        || p.StartsWith(r + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
        || p.StartsWith(r + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
}

static async Task<string> ReadHarnessVersion()
{
    try
    {
        var psi = new ProcessStartInfo("claude") { UseShellExecute = false, RedirectStandardOutput = true };
        psi.ArgumentList.Add("--version");
        using var p = Process.Start(psi);
        if (p is null) return "unknown";
        var text = (await p.StandardOutput.ReadToEndAsync()).Trim();
        await p.WaitForExitAsync();
        return string.IsNullOrEmpty(text) ? "unknown" : text;
    }
    catch
    {
        return "unknown";
    }
}

// Reads the figure Claude Code itself last cached in ~/.claude.json — not a live query, so it
// can be stale in either direction. The cap is a courtesy; maxAttempts and the timeout are
// the real guards.
static (int percent, DateTimeOffset resetsAt)? ReadUtilization(string claudeJsonPath)
{
    if (!File.Exists(claudeJsonPath)) return null;
    try
    {
        using var doc = JsonDocument.Parse(File.ReadAllText(claudeJsonPath));
        if (!doc.RootElement.TryGetProperty("cachedUsageUtilization", out var cache)) return null;
        if (!cache.TryGetProperty("utilization", out var u)) return null;
        if (!u.TryGetProperty("five_hour", out var fiveHour)) return null;
        var percent = fiveHour.GetProperty("utilization").GetInt32();
        var resetsAt = DateTimeOffset.Parse(fiveHour.GetProperty("resets_at").GetString()!);
        return (percent, resetsAt);
    }
    catch
    {
        return null;
    }
}
