using System.Diagnostics;
using System.Net.Http.Json;
using StoryPlanner.AgentRunner;

// The agent runner: `claude -p` calls with explicit context, no transcript, run from a folder
// OUTSIDE the repo, one call per item of a batch, recorded per call in the batch's calls file.
// A persistent HOST owns the page's port and runs any number of batches under one global
// parallel ceiling, one utilization cap and one idle limit; the CLI hands it a batch and returns.
//
//   AgentRunner.exe start                                     start the host if none answers, open the page
//   AgentRunner.exe stop [--now]                              stop the host after in-flight calls (--now: kill them)
//   AgentRunner.exe dry-run-batch <definition.md> [--item X]  compose every call in memory, write nothing (serverless)
//   AgentRunner.exe execute-batch <definition.md> [--item X] [--at HH:mm|ISO|reset]
//                                                             one call per item without a result; --item names the pilot
//   AgentRunner.exe tally-batch <definition.md> [--flag field=value]... [--group-by column]
//                                                             write tally.md once (serverless)
//   AgentRunner.exe host                                      run the host in this process (what start spawns)
//
// Every batch verb takes the path of a batch's definition and resolves the rest beside it; the
// runner holds no root and knows no study. Harness control (pause, stop, cancel, ceilings, the
// idle limit) is the page and its JSON routes; nothing anywhere changes what a call is.

var configPath = Path.Combine(AppContext.BaseDirectory, "configs", "host.json");
if (!File.Exists(configPath)) configPath = Path.Combine(AppContext.BaseDirectory, "..", "configs", "host.json");
var config = HostConfig.Load(configPath);

if (args.Length == 0) return Usage();
var verb = args[0];
var rest = args.Skip(1).ToList();
string? Option(string name)
{
    var i = rest.IndexOf(name);
    if (i < 0 || i + 1 >= rest.Count) return null;
    var value = rest[i + 1];
    rest.RemoveRange(i, 2);
    return value;
}
List<string> Options(string name)
{
    var values = new List<string>();
    while (Option(name) is { } v) values.Add(v);
    return values;
}

switch (verb)
{
    case "start":
    {
        var url = await EnsureHost(config);
        if (url is null) return 1;
        OpenBrowser(url);
        Console.WriteLine($"host: {url}");
        return 0;
    }
    case "stop":
        return await StopHost(config, rest.Contains("--now"));
    case "host":
        return await RunHost(config);
    case "dry-run-batch":
    {
        var item = Option("--item");
        if (rest.Count != 1) return Usage("dry-run-batch takes one argument: a batch's definition.md");
        return DryRun(Path.GetFullPath(rest[0]), item, config);
    }
    case "execute-batch":
    {
        var item = Option("--item");
        var atSpec = Option("--at");
        if (rest.Count != 1) return Usage("execute-batch takes one argument: a batch's definition.md");
        DateTimeOffset? notBefore = null;
        if (atSpec is not null)
        {
            var (at, atError) = Schedule.ParseAt(atSpec, DateTimeOffset.UtcNow, RunnerHost.ReadCachedUtilization());
            if (atError is not null) { Console.Error.WriteLine(atError); return 2; }
            notBefore = at;
        }
        var url = await EnsureHost(config);
        if (url is null) return 1;
        using var http = new HttpClient { BaseAddress = new Uri(url) };
        var resp = await http.PostAsJsonAsync("/api/batches", new ExecuteRequest(Path.GetFullPath(rest[0]), item, notBefore));
        var result = await resp.Content.ReadFromJsonAsync<ExecuteResult>();
        Console.WriteLine(result?.Message ?? $"host answered {(int)resp.StatusCode}");
        if (result?.Ok == true) Console.WriteLine($"watch: {url}/batches/{result.BatchId}");
        return result?.Ok == true ? 0 : 1;
    }
    case "tally-batch":
    {
        var flags = Options("--flag").Select(f =>
        {
            var eq = f.IndexOf('=');
            return eq <= 0 ? null : new Tally.Flag(f[..eq], f[(eq + 1)..]);
        }).ToList();
        if (flags.Any(f => f is null)) return Usage("--flag takes field=value");
        var groupBy = Option("--group-by");
        if (rest.Count != 1) return Usage("tally-batch takes one argument: a batch's definition.md");
        var (batch, error) = Batch.Load(Path.GetFullPath(rest[0]), Directory.GetCurrentDirectory());
        if (batch is null) { Console.Error.WriteLine(error); return 2; }
        var (ok, message) = Tally.Write(batch, flags!, groupBy);
        Console.WriteLine(message);
        return ok ? 0 : 1;
    }
    default:
        return Usage($"Unknown verb '{verb}'.");
}

// --- verbs ---

static async Task<int> RunHost(HostConfig config)
{
    var harness = await ReadHarnessVersion();
    var host = new RunnerHost(config, new ProcessChildLauncher(msg => Console.Error.WriteLine("  " + msg)), harness, echo: Console.WriteLine);
    host.Log($"host starting on {config.Url} (harness {harness}; working dir {host.WorkingDir}; launch dir {host.LaunchDir}; ceiling {host.MaxParallel}; cap {host.UtilizationCap}%; idle {host.IdleMinutes} min)");
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
    // The host's working directory is the CLI's: the page lists the batches beneath it.
    var psi = new ProcessStartInfo(exe) { UseShellExecute = false, CreateNoWindow = true, WorkingDirectory = Directory.GetCurrentDirectory() };
    psi.ArgumentList.Add("host");
    try { Process.Start(psi); }
    catch (Exception ex) { Console.Error.WriteLine($"cannot start host: {ex.Message}"); return null; }
    for (var i = 0; i < 60; i++)
    {
        await Task.Delay(250);
        if (await Ping(config.Url)) { Console.WriteLine($"host started: {config.Url}"); return config.Url; }
    }
    Console.Error.WriteLine($"host did not answer on {config.Url} within 15 s — see host-log.txt beside the exe");
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
    Console.Write(now ? "stopping now" : "stopping after in-flight calls");
    for (var i = 0; i < 240; i++)
    {
        await Task.Delay(500);
        if (!await Ping(config.Url)) { Console.WriteLine(" — stopped"); return 0; }
        if (i % 10 == 9) Console.Write('.');
    }
    Console.WriteLine(" — still running after 2 min (calls in flight?); use `stop --now`");
    return 1;
}

static void OpenBrowser(string url)
{
    try { Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); }
    catch (Exception ex) { Console.Error.WriteLine($"open {url} yourself — {ex.Message}"); }
}

/// <summary>Serverless: read the batch, check the launch folder, compose every pending call in memory, print, write nothing.</summary>
static int DryRun(string definitionPath, string? item, HostConfig config)
{
    var launchDir = Path.GetFullPath(config.LaunchDir ?? HostConfig.DefaultLaunchDir());
    var (runner, error) = BatchRunner.Create(definitionPath, Directory.GetCurrentDirectory(), item, launchDir,
        new NoLauncher(), new OpenGate(), Console.WriteLine, "dry-run");
    if (runner is null) { Console.Error.WriteLine(error); return 2; }
    var batch = runner.Batch;

    Console.WriteLine("agent runner (DRY RUN)");
    Console.WriteLine($"  batch      : {batch.Id}");
    Console.WriteLine($"  definition : {batch.Definition.Path} ({batch.Definition.Hash[..12]}…)");
    Console.WriteLine($"  directions : {batch.Definition.DirectionsPath} ({batch.Directions.BodyHash[..12]}…), {batch.Directions.Body.Length:N0} chars");
    Console.WriteLine($"  model      : {batch.Model}{(batch.Effort is null ? "" : ", effort " + batch.Effort)}; tools [{string.Join(", ", batch.Definition.Tools)}]; mcp {(batch.Definition.McpPath is null ? "no" : batch.Definition.McpPath)}");
    Console.WriteLine($"  launchDir  : {launchDir}");
    Console.WriteLine($"  schema     : {batch.SchemaJson}");
    Console.WriteLine($"  execution  : {runner.Execution} — {runner.Summary()}");
    Console.WriteLine();
    foreach (var i in batch.Items)
    {
        var state = runner.HasSucceeded(i) ? "answered" : "pending";
        var plan = batch.Compose(i);
        Console.WriteLine($"  [{state,-8}] {i}: item {plan.ItemText.Length:N0} chars ({plan.ItemHash[..12]}…), prompt {plan.PromptHash[..12]}…");
    }
    Console.WriteLine();
    Console.WriteLine("DRY RUN — nothing called, nothing written.");
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

static int Usage(string? message = null)
{
    if (message is not null) Console.Error.WriteLine(message);
    Console.Error.WriteLine("""
        Usage:
          AgentRunner.exe start
          AgentRunner.exe stop [--now]
          AgentRunner.exe dry-run-batch <definition.md> [--item ID]
          AgentRunner.exe execute-batch <definition.md> [--item ID] [--at HH:mm|ISO|reset]
          AgentRunner.exe tally-batch   <definition.md> [--flag field=value]... [--group-by item|locator|description]
        """);
    return 2;
}

/// <summary>For the serverless dry run: a launcher that must never be called.</summary>
sealed class NoLauncher : IChildLauncher
{
    public Task<int> LaunchAsync(ChildRequest request, Action<IChildHandle> track, Action onStreamAdvanced, CancellationToken ct) =>
        throw new InvalidOperationException("dry run launches nothing");
}
