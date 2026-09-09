using StoryPlanner.BatchFiles;

namespace StoryPlanner.AgentRunner;

/// <summary>One item of a batch as the page and the JSON routes show it. Same shape for a live and a finished batch.</summary>
public sealed record ItemSnapshot(
    string Item,
    string Locator,
    string State,          // Pending | Running | Succeeded | Failed
    int Calls,
    int? LastExit,
    string? LastCheck,
    double CostUsd,
    string? LastStartUtc,
    string? LastEndUtc,
    double? LastSeconds,
    string? StreamPath);

/// <summary>
/// The stages a batch folder shows evidence of (agent-runner skill § The host and its page),
/// each detected mechanically from what is on disk — never declared, never judged.
/// </summary>
public sealed record BatchStages(
    bool Defined,          // definition.md reads
    bool Itemized,         // index.md with rows, every body present
    bool Piloted,          // a call marked pilot
    bool Executed,         // every item has a successful call
    bool Tallied);         // tally.md

/// <summary>The immutable view of one batch that both the Razor components and the JSON routes consume, built from the folder with the live execution layered on.</summary>
public sealed record BatchSnapshot(
    string Id,
    string Dir,
    string Study,
    string Name,
    string? Kind,
    string? Model,
    string? Effort,
    string? Directions,
    bool Live,
    bool Completed,
    bool Paused,
    bool StopRequested,
    int InFlight,
    string? HoldReason,
    IReadOnlyList<ItemSnapshot> Items,
    int Pending,
    int Succeeded,
    int Failed,
    double CostUsd,
    string? Error,
    BatchStages Stages,
    string? LastActivityUtc,
    string? NotBeforeUtc = null,
    bool Scheduled = false);

/// <summary>
/// Reads batches from disk: every <c>definition.md</c> under a <c>batches/</c> folder beneath
/// the host's working directory is a batch, its id the folder's path relative to that
/// directory. Builds the same <see cref="BatchSnapshot"/> the host builds for a live
/// execution, so history and live are one view. The items of a batch are the index's, never
/// the live execution's filter: a pilot leaves the rest pending.
/// </summary>
public static class BatchCatalog
{
    static readonly string[] Skipped = [".git", "bin", "obj", "node_modules", "publish", ".idea", ".vs", "attempts", "items", "results"];

    /// <summary>Every definition path beneath the working directory, skipping build and history folders.</summary>
    public static IEnumerable<string> Definitions(string workingDir)
    {
        if (!Directory.Exists(workingDir)) yield break;
        var stack = new Stack<string>();
        stack.Push(workingDir);
        while (stack.Count > 0)
        {
            var dir = stack.Pop();
            IEnumerable<string> subs;
            try { subs = Directory.EnumerateDirectories(dir); } catch (IOException) { continue; } catch (UnauthorizedAccessException) { continue; }
            foreach (var sub in subs)
            {
                var name = Path.GetFileName(sub);
                if (Skipped.Contains(name) || name.StartsWith('.')) continue;
                if (Path.GetFileName(dir) == "batches" && File.Exists(Path.Combine(sub, "definition.md")))
                    yield return Path.Combine(sub, "definition.md");
                stack.Push(sub);
            }
        }
    }

    public static BatchSnapshot Load(string definitionPath, string workingDir) => Build(definitionPath, workingDir, live: null);

    /// <summary>The one builder: disk first, then the live execution's harness state and in-flight set layered on.</summary>
    public static BatchSnapshot Build(string definitionPath, string workingDir, BatchRunner? live)
    {
        definitionPath = Path.GetFullPath(definitionPath);
        var dir = Path.GetDirectoryName(definitionPath)!;
        var id = Batch.IdFor(dir, workingDir);
        var name = Path.GetFileName(dir);
        var study = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(dir)!)!);

        DefinitionFile? definition = null;
        string? error = null;
        try { definition = DefinitionFile.Read(definitionPath); }
        catch (IOException ex) { error = $"definition unreadable: {ex.Message}"; }
        if (definition is { Problems.Count: > 0 }) error = $"definition: {definition.Problems[0].Message}";

        var indexPath = definition?.IndexPath ?? Path.Combine(dir, "index.md");
        IndexFile? index = File.Exists(indexPath) ? IndexFile.Read(indexPath) : null;
        var calls = live?.CallsSnapshot() ?? CallsFile.Read(definition?.CallsPath ?? Path.Combine(dir, "calls.md")).Entries;
        var running = live?.RunningSnapshot().ToDictionary(r => r.Item, StringComparer.Ordinal) ?? new Dictionary<string, RunningCall>(StringComparer.Ordinal);

        var ids = index?.Rows.Select(r => r.Item).ToList() ?? [];
        foreach (var i in calls.Select(c => c.Item).Distinct()) if (!ids.Contains(i)) ids.Add(i);

        var items = new List<ItemSnapshot>();
        foreach (var item in ids)
        {
            var rows = calls.Where(c => c.Item == item).OrderBy(c => c.Call).ToList();
            var last = rows.LastOrDefault();
            var state = running.ContainsKey(item) ? "Running" : rows.Any(r => r.Succeeded) ? "Succeeded" : rows.Count > 0 ? "Failed" : "Pending";
            double? seconds = null;
            string? startUtc = last?.Started, endUtc = last?.Ended, streamPath = last is null ? null : Path.Combine(dir, "attempts", item, $"call-{last.Call}", "stream.jsonl");
            if (running.TryGetValue(item, out var ra))
            {
                startUtc = ra.StartUtc.ToString("o"); endUtc = null; streamPath = ra.StreamPath;
                seconds = (DateTimeOffset.UtcNow - ra.StartUtc).TotalSeconds;
            }
            else if (last is not null && DateTimeOffset.TryParse(last.Started, out var s) && DateTimeOffset.TryParse(last.Ended, out var e))
                seconds = (e - s).TotalSeconds;
            items.Add(new ItemSnapshot(item, index?.Rows.FirstOrDefault(r => r.Item == item)?.Locator ?? "", state,
                rows.Count, last?.Exit, last?.Check, rows.Sum(r => r.Cost ?? 0), startUtc, endUtc, seconds, streamPath));
        }

        var itemized = index is { Rows.Count: > 0 } && index.Rows.All(r => File.Exists(Path.Combine(dir, "items", r.Item + ".md")));
        var executed = items.Count > 0 && items.All(i => i.State == "Succeeded");
        var stages = new BatchStages(
            Defined: definition is not null && error is null,
            Itemized: itemized,
            Piloted: calls.Any(c => c.Pilot),
            Executed: executed,
            Tallied: File.Exists(Path.Combine(dir, "tally.md")));

        var lastActivity = calls.Select(c => c.Ended).Where(s => !string.IsNullOrEmpty(s)).OrderBy(s => s).LastOrDefault();

        return new BatchSnapshot(
            id, dir, study, name,
            definition?.Kind, definition?.Model, definition?.Effort,
            definition?.DirectionsPath is null ? null : Path.GetFileNameWithoutExtension(definition.DirectionsPath),
            Live: live is not null && !live.Completed,
            Completed: executed,
            Paused: live?.Paused ?? false,
            StopRequested: live?.StopRequested ?? false,
            InFlight: running.Count,
            HoldReason: live?.HoldReason(),
            Items: items,
            Pending: items.Count(i => i.State == "Pending"),
            Succeeded: items.Count(i => i.State == "Succeeded"),
            Failed: items.Count(i => i.State == "Failed"),
            CostUsd: calls.Sum(c => c.Cost ?? 0),
            Error: error,
            Stages: stages,
            LastActivityUtc: lastActivity,
            NotBeforeUtc: live?.NotBefore?.ToString("o"),
            Scheduled: live is { Started: false, NotBefore: not null });
    }
}
