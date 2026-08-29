using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

// Usage:
//   dotnet run --project tools/StoryPlanner.AnalysisRunner -- [options]
//
//   --utilization-cap N   Pause when five_hour utilization >= N% (default: 80)
//   --dry-run             Show the queue and current utilization, then exit
//
// The runner reads .claude/skills/analyze-story/populations.md for the next
// unchecked story (order: Subset B → C → A, Tier 3 skipped), invokes
// `claude -p` with the analyze-story skill, checks utilization after each
// story, and sleeps until the window resets when over the cap.

var cliArgs = Environment.GetCommandLineArgs().Skip(1).ToArray();

var utilizationCap = 80;
var dryRun = false;

for (int i = 0; i < cliArgs.Length; i++)
{
    switch (cliArgs[i])
    {
        case "--utilization-cap" when i + 1 < cliArgs.Length:
            utilizationCap = int.Parse(cliArgs[++i]);
            break;
        case "--dry-run":
            dryRun = true;
            break;
    }
}

Process? activeClaudeProcess = null;
var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    Console.WriteLine("\n  Ctrl+C received — shutting down...");
    cts.Cancel();
    if (activeClaudeProcess is { HasExited: false } p)
    {
        Console.WriteLine($"  Killing claude process (PID {p.Id})...");
        try { p.Kill(entireProcessTree: true); } catch { }
    }
};

var repoRoot = FindRepoRoot(Environment.CurrentDirectory)
    ?? throw new InvalidOperationException("Not inside the StoryPlanner repo.");
var populationsPath = Path.Combine(repoRoot, ".claude", "skills", "analyze-story", "populations.md");
var claudeJsonPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude.json");

Console.WriteLine($"Analysis Runner");
Console.WriteLine($"  repo:               {repoRoot}");
Console.WriteLine($"  utilization cap:    {utilizationCap}%");
Console.WriteLine($"  dry run:            {dryRun}");

var utilSnapshot = ReadUtilization(claudeJsonPath);
if (utilSnapshot != null)
    Console.WriteLine($"  current utilization: {utilSnapshot.Value.percent}% five-hour, resets {utilSnapshot.Value.resetsAt.ToLocalTime():HH:mm}");

Console.WriteLine();

if (dryRun)
{
    Console.WriteLine("Queue (B → C → A, Tier 3 skipped):");
    var allStories = FindAllStories(populationsPath);
    for (var i = 0; i < allStories.Count; i++)
        Console.WriteLine($"  [{i + 1}] {allStories[i]}");
    Console.WriteLine($"\n{allStories.Count} stories queued.");
    return;
}

var totalCompleted = 0;
var totalFailed = 0;

while (!cts.Token.IsCancellationRequested)
{
    var utilization = ReadUtilization(claudeJsonPath);
    if (utilization != null && utilization.Value.percent >= utilizationCap)
    {
        var resetAt = utilization.Value.resetsAt;
        var waitTime = resetAt - DateTimeOffset.UtcNow;
        if (waitTime > TimeSpan.Zero)
        {
            Console.WriteLine();
            Console.WriteLine($"  Utilization at {utilization.Value.percent}% (cap: {utilizationCap}%)");
            Console.WriteLine($"  Window resets at {resetAt.ToLocalTime():HH:mm} local ({waitTime.TotalMinutes:F0} min)");
            Console.WriteLine($"  Sleeping... (Ctrl+C to stop)");

            try { await Task.Delay(waitTime + TimeSpan.FromMinutes(1), cts.Token); }
            catch (OperationCanceledException) { break; }

            Console.WriteLine($"  Awake. Resuming.");
            continue;
        }
    }

    var story = FindNextStory(populationsPath);
    if (story == null)
    {
        Console.WriteLine();
        Console.WriteLine($"All stories complete. {totalCompleted} succeeded, {totalFailed} failed.");
        break;
    }

    Console.WriteLine($"[{totalCompleted + totalFailed + 1}] {story}");

    var success = await RunAnalysis(story, repoRoot, p => activeClaudeProcess = p, cts.Token);
    activeClaudeProcess = null;

    if (cts.Token.IsCancellationRequested)
        break;

    if (success)
    {
        totalCompleted++;
        Console.WriteLine($"  Done. ({totalCompleted} completed, {totalFailed} failed)");
    }
    else
    {
        totalFailed++;
        Console.WriteLine($"  FAILED — moving on. ({totalCompleted} completed, {totalFailed} failed)");
    }

    var postUtil = ReadUtilization(claudeJsonPath);
    if (postUtil != null)
        Console.WriteLine($"  Utilization: {postUtil.Value.percent}% five-hour, resets {postUtil.Value.resetsAt.ToLocalTime():HH:mm}");
}

if (cts.Token.IsCancellationRequested)
    Console.WriteLine($"\nStopped. {totalCompleted} completed, {totalFailed} failed.");

// --- Helpers ---

static string? FindRepoRoot(string dir)
{
    var d = new DirectoryInfo(dir);
    while (d != null)
    {
        if (Directory.Exists(Path.Combine(d.FullName, ".claude")))
            return d.FullName;
        d = d.Parent;
    }
    return null;
}

static List<string> FindAllStories(string populationsPath)
{
    var lines = File.ReadAllLines(populationsPath);
    var sectionStories = new Dictionary<string, List<string>>();
    var currentSection = "";

    foreach (var line in lines)
    {
        if (line.StartsWith("### Subset") || line.StartsWith("## Tier 3"))
            currentSection = line;

        if (!line.StartsWith("- [ ] "))
            continue;

        if (currentSection.Contains("Tier 3") || currentSection.Contains("Extra long"))
            continue;

        var match = Regex.Match(line, @"^- \[ \] (\S+)");
        if (!match.Success) continue;

        if (!sectionStories.ContainsKey(currentSection))
            sectionStories[currentSection] = [];
        sectionStories[currentSection].Add(match.Groups[1].Value);
    }

    return sectionStories
        .OrderBy(kv => kv.Key.Contains("Subset B") ? 0 : kv.Key.Contains("Subset C") ? 1 : 2)
        .SelectMany(kv => kv.Value)
        .ToList();
}

static string? FindNextStory(string populationsPath)
{
    var lines = File.ReadAllLines(populationsPath);
    var sectionStories = new Dictionary<string, List<string>>();
    var currentSection = "";

    foreach (var line in lines)
    {
        if (line.StartsWith("### Subset") || line.StartsWith("## Tier 3"))
            currentSection = line;

        if (!line.StartsWith("- [ ] "))
            continue;

        if (currentSection.Contains("Tier 3") || currentSection.Contains("Extra long"))
            continue;

        var match = Regex.Match(line, @"^- \[ \] (\S+)");
        if (!match.Success) continue;

        if (!sectionStories.ContainsKey(currentSection))
            sectionStories[currentSection] = [];
        sectionStories[currentSection].Add(match.Groups[1].Value);
    }

    foreach (var key in sectionStories.Keys
                 .OrderBy(k => k.Contains("Subset B") ? 0 : k.Contains("Subset C") ? 1 : 2))
    {
        if (sectionStories[key].Count > 0)
            return sectionStories[key][0];
    }

    return null;
}

static (int percent, DateTimeOffset resetsAt)? ReadUtilization(string claudeJsonPath)
{
    if (!File.Exists(claudeJsonPath))
        return null;

    try
    {
        var json = File.ReadAllText(claudeJsonPath);
        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("cachedUsageUtilization", out var cache))
            return null;
        if (!cache.TryGetProperty("utilization", out var util))
            return null;
        if (!util.TryGetProperty("five_hour", out var fiveHour))
            return null;

        var percent = fiveHour.GetProperty("utilization").GetInt32();
        var resetsAtStr = fiveHour.GetProperty("resets_at").GetString();
        var resetsAt = DateTimeOffset.Parse(resetsAtStr!);

        return (percent, resetsAt);
    }
    catch
    {
        return null;
    }
}

static async Task<bool> RunAnalysis(
    string storySlug, string repoRoot, Action<Process> trackProcess, CancellationToken ct)
{
    var psi = new ProcessStartInfo
    {
        FileName = "claude",
        ArgumentList =
        {
            "-p",
            $"/analyze-story {storySlug}",
            "--permission-mode", "auto"
        },
        WorkingDirectory = repoRoot,
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true
    };

    try
    {
        using var process = Process.Start(psi);
        if (process == null)
            return false;

        trackProcess(process);

        var stdoutTask = Task.Run(async () =>
        {
            while (await process.StandardOutput.ReadLineAsync() is { } line)
                Console.WriteLine($"  | {line}");
        });

        var stderrTask = Task.Run(async () =>
        {
            while (await process.StandardError.ReadLineAsync() is { } line)
                Console.Error.WriteLine($"  ! {line}");
        });

        await process.WaitForExitAsync(ct);
        await Task.WhenAll(stdoutTask, stderrTask);

        return process.ExitCode == 0;
    }
    catch (OperationCanceledException)
    {
        return false;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"  Failed to start claude: {ex.Message}");
        return false;
    }
}
