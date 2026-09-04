using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using StoryPlanner.AgentRunner;

// The agent runner: `claude -p` children with explicit context, no transcript, run from a
// folder OUTSIDE the repo, recorded per attempt in a ledger. Since 2026-09-03 (late) it is a
// persistent HOST: one process owns the page's port and runs any number of batches under one
// global parallel ceiling and one utilization cap; the CLI enqueues to it and returns.
//
//   AgentRunner.exe                           start the host if none answers, open the page
//   AgentRunner.exe <run>/jobs.json           enqueue the run (starts the host if needed)
//   AgentRunner.exe <run>/jobs.json --job ID  enqueue one job — the pilot
//   AgentRunner.exe <run>/jobs.json --at X    schedule: X = HH:mm (next such time), an ISO date-time, or reset
//   AgentRunner.exe <run>/jobs.json --dry-run compose every prompt, launch nothing (serverless)
//   AgentRunner.exe split <doc.md> <items>    cut a Markdown document into unit items (serverless)
//   AgentRunner.exe stop [--now]              stop the host after in-flight jobs (--now: kill them)
//   AgentRunner.exe host                      run the host in this process (what the CLI spawns)
//
// The run folder is the job file's folder; the host writes ledger.jsonl and attempts/ there.
// Harness control (pause, stop, cancel, ceilings) is the page and its JSON routes; nothing
// anywhere changes what a job is. Replaces AnalysisRunner (deleted 2026-09-03).

var config = HostConfig.Load(Path.Combine(AppContext.BaseDirectory, "configs", "host.json"))
    ?? new HostConfig();
if (!File.Exists(Path.Combine(AppContext.BaseDirectory, "configs", "host.json")))
    config = HostConfig.Load(Path.Combine(AppContext.BaseDirectory, "..", "configs", "host.json"));

if (args.Length > 0 && args[0] == "split") return RunSplit(args.Skip(1).ToArray());
if (args.Length > 0 && args[0] == "host") return await RunHost(config);
if (args.Length > 0 && args[0] == "stop") return await StopHost(config, args.Contains("--now"));

var positional = args.Where(a => !a.StartsWith("--")).ToList();
var dryRun = args.Contains("--dry-run");
var jobArgIdx = Array.IndexOf(args, "--job");
string? onlyJob = jobArgIdx >= 0 && jobArgIdx + 1 < args.Length ? args[jobArgIdx + 1] : null;
if (onlyJob is not null) positional.Remove(onlyJob);
var atArgIdx = Array.IndexOf(args, "--at");
string? atSpec = atArgIdx >= 0 && atArgIdx + 1 < args.Length ? args[atArgIdx + 1] : null;
if (atSpec is not null) positional.Remove(atSpec);
DateTimeOffset? notBefore = null;
if (atSpec is not null)
{
    var (at, atError) = Schedule.ParseAt(atSpec, DateTimeOffset.UtcNow, RunnerHost.ReadCachedUtilization());
    if (atError is not null) { Console.Error.WriteLine(atError); return 2; }
    notBefore = at;
}

if (positional.Count == 0)
{
    var url = await EnsureHost(config);
    if (url is null) return 1;
    OpenBrowser(url);
    Console.WriteLine($"host: {url}");
    return 0;
}
if (positional.Count != 1)
{
    Console.Error.WriteLine("Usage: AgentRunner.exe [<run>/jobs.json [--dry-run] [--job ID] [--at HH:mm|ISO|reset]] | split <doc.md> <items-dir> | stop [--now] | host");
    return 2;
}

var jobFilePath = Path.GetFullPath(positional[0]);
if (dryRun) return DryRun(jobFilePath, onlyJob);

{
    var url = await EnsureHost(config);
    if (url is null) return 1;
    using var http = new HttpClient { BaseAddress = new Uri(url) };
    var resp = await http.PostAsJsonAsync("/api/runs", new EnqueueRequest(jobFilePath, onlyJob, notBefore));
    var result = await resp.Content.ReadFromJsonAsync<EnqueueResult>();
    Console.WriteLine(result?.Message ?? $"host answered {(int)resp.StatusCode}");
    if (result?.Ok == true) Console.WriteLine($"watch: {url}/runs/{result.RunId}");
    return result?.Ok == true ? 0 : 1;
}

// --- verbs ---

static async Task<int> RunHost(HostConfig config)
{
    var harness = await ReadHarnessVersion();
    var host = new RunnerHost(config, new ProcessChildLauncher(msg => Console.Error.WriteLine("  " + msg)), harness, Console.WriteLine);
    host.Log($"host starting on {config.Url} (harness {harness}; fanout {host.FanoutRoot}; ceiling {host.MaxParallel}; cap {host.UtilizationCap}%)");
    var app = RunnerApi.BuildApp(host, config.Url);
    Console.CancelKeyPress += (_, e) => { e.Cancel = true; _ = host.ShutdownAsync(now: true).ContinueWith(_ => app.StopAsync()); };
    try
    {
        await app.RunAsync();
    }
    catch (Exception ex)
    {
        host.Log($"host failed: {ex.Message}");
        return 1;
    }
    return 0;
}

static async Task<string?> EnsureHost(HostConfig config)
{
    if (await Ping(config.Url)) return config.Url;
    var exe = Environment.ProcessPath;
    if (exe is null) { Console.Error.WriteLine("cannot locate own executable to start the host"); return null; }
    var psi = new ProcessStartInfo(exe) { UseShellExecute = false, CreateNoWindow = true, WorkingDirectory = AppContext.BaseDirectory };
    psi.ArgumentList.Add("host");
    try { Process.Start(psi); }
    catch (Exception ex) { Console.Error.WriteLine($"cannot start host: {ex.Message}"); return null; }
    for (var i = 0; i < 60; i++)
    {
        await Task.Delay(250);
        if (await Ping(config.Url)) { Console.WriteLine($"host started: {config.Url}"); return config.Url; }
    }
    Console.Error.WriteLine($"host did not answer on {config.Url} within 15 s — see fanout/host-log.txt");
    return null;
}

static async Task<bool> Ping(string url)
{
    try
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromMilliseconds(800) };
        var r = await http.GetAsync(url + "/api/ping");
        return r.IsSuccessStatusCode;
    }
    catch { return false; }
}

static async Task<int> StopHost(HostConfig config, bool now)
{
    if (!await Ping(config.Url)) { Console.WriteLine("no host is running"); return 0; }
    using var http = new HttpClient { BaseAddress = new Uri(config.Url) };
    await http.PostAsync($"/api/host/shutdown?now={(now ? "true" : "false")}", null);
    Console.Write(now ? "stopping now" : "stopping after in-flight jobs");
    for (var i = 0; i < 240; i++)
    {
        await Task.Delay(500);
        if (!await Ping(config.Url)) { Console.WriteLine(" — stopped"); return 0; }
        if (i % 10 == 9) Console.Write('.');
    }
    Console.WriteLine(" — still running after 2 min (children in flight?); use `stop --now`");
    return 1;
}

static void OpenBrowser(string url)
{
    try { Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); }
    catch (Exception ex) { Console.Error.WriteLine($"open {url} yourself — {ex.Message}"); }
}

/// <summary>Serverless: parse, resolve, check the launch folder, compose every pending prompt, print, launch nothing.</summary>
static int DryRun(string jobFilePath, string? onlyJob)
{
    var runDir = Path.GetDirectoryName(jobFilePath)!;
    var (runner, error) = BatchRunner.Create(jobFilePath, RunCatalog.RunIdFor(runDir, HostConfig.FindFanoutRoot()), onlyJob,
        new NoLauncher(), new OpenGate(), Console.WriteLine, "dry-run");
    if (runner is null) { Console.Error.WriteLine(error); return 2; }

    Console.WriteLine("agent runner (DRY RUN)");
    Console.WriteLine($"  run       : {runner.RunDir}");
    Console.WriteLine($"  launchDir : {Path.GetFullPath(runner.JobFile.LaunchDir)}");
    Console.WriteLine($"  ledger    : {runner.LedgerPath} ({runner.LedgerSnapshot().Count} attempt(s) recorded)");
    Console.WriteLine($"  run ceilings: maxAttempts {runner.JobFile.MaxAttempts}; timeout {runner.JobFile.TimeoutMinutes} min; maxParallel {runner.MaxParallel}; utilizationCap {runner.JobFile.UtilizationCap}%");
    Console.WriteLine();
    foreach (var j in runner.Jobs)
        Console.WriteLine($"  [{runner.StateOf(j),-9}] {j.Id}  model={j.Model} mcp={(j.Mcp ? "yes" : "no")}  item: {j.Item}");
    Console.WriteLine();
    foreach (var j in runner.Jobs.Where(j => runner.StateOf(j) == JobState.Pending))
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

/// <summary>For the serverless dry run: a launcher that must never be called.</summary>
sealed class NoLauncher : IChildLauncher
{
    public Task<int> LaunchAsync(ChildRequest request, Action<IChildHandle> track, Action onStreamAdvanced, CancellationToken ct) =>
        throw new InvalidOperationException("dry run launches nothing");
}
