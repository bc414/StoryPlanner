using System.Collections.Concurrent;
using System.Text.Json;
using StoryPlanner.AgentRunner;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.Tests;

/// <summary>
/// A child launcher that starts no process: it writes a one-line stream, then a result event
/// carrying the structured answer the request's JSON Schema asks for (unless told not to),
/// holds for as long as the test says, and can be "killed". Lets the batch loop's queue,
/// ceilings, pause, stop and cancel semantics be tested in the pure tier.
/// </summary>
public sealed class FakeLauncher : IChildLauncher
{
    private sealed class Handle(TaskCompletionSource<int> killed) : IChildHandle
    {
        public int Pid => 4242;
        public void Kill() => killed.TrySetResult(137);
    }

    private int _current;
    public int MaxConcurrent;
    public int Launched;
    public readonly ConcurrentQueue<string> Order = new();
    public readonly ConcurrentQueue<ChildRequest> Requests = new();
    /// <summary>When set, a launch waits here (one release per launch) instead of the fixed delay.</summary>
    public SemaphoreSlim? Hold;
    public TimeSpan Delay = TimeSpan.FromMilliseconds(30);
    public Func<ChildRequest, int> ExitFor = _ => 0;
    /// <summary>The structured answer the fake result event carries; null for no structured output.</summary>
    public Func<ChildRequest, string?> AnswerFor = _ => """{"class":"a","why":"1"}""";

    public async Task<int> LaunchAsync(ChildRequest request, Action<IChildHandle> track, Action onStreamAdvanced, CancellationToken ct)
    {
        var now = Interlocked.Increment(ref _current);
        Interlocked.Increment(ref Launched);
        Order.Enqueue(request.Item);
        Requests.Enqueue(request);
        lock (this) MaxConcurrent = Math.Max(MaxConcurrent, now);
        var killed = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        track(new Handle(killed));
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(request.StreamPath)!);
            await File.WriteAllTextAsync(request.StreamPath,
                """{"type":"system","subtype":"init","tools":[],"mcp_servers":[],"model":"fake"}""" + "\n", ct);
            onStreamAdvanced();

            var wait = Hold is not null ? Hold.WaitAsync(ct) : Task.Delay(Delay, ct);
            var done = await Task.WhenAny(wait, killed.Task);
            if (done == killed.Task) return 137;
            await wait;

            var answer = AnswerFor(request);
            var result = answer is null
                ? """{"type":"result","total_cost_usd":0.01,"num_turns":1,"session_id":"s","result":"done"}"""
                : "{\"type\":\"result\",\"total_cost_usd\":0.01,\"num_turns\":1,\"session_id\":\"s\",\"result\":\"done\",\"structured_output\":" + answer + "}";
            await File.AppendAllTextAsync(request.StreamPath, result + "\n", ct);
            onStreamAdvanced();
            return ExitFor(request);
        }
        finally
        {
            Interlocked.Decrement(ref _current);
        }
    }
}

/// <summary>A throwaway study with one batch under the temp dir, and a launch folder beside it, written in the batch files' shapes.</summary>
public sealed class TempBatch : IDisposable
{
    public string Root { get; }
    public string WorkingDir { get; }
    public string StudyDir { get; }
    public string BatchDir { get; }
    public string LaunchDir { get; }
    public string DefinitionPath => Path.Combine(BatchDir, "definition.md");
    public string Id => Batch.IdFor(BatchDir, WorkingDir);

    public const string Directions = """
        ---
        questions: v1-archive/q
        ---

        ## What you are given

        One note.

        ## Classes

        - a: shows a
        - b: shows b
        - cannot-place: the criteria do not decide it

        ## Criteria

        1. A rule.

        ## What to produce

        - class: enum
        - why: line, the criterion

        """;

    public TempBatch(string study = "verification-of-v1-archive-test", string batch = "01-full")
    {
        Root = Path.Combine(Path.GetTempPath(), "sp-batch-" + Guid.NewGuid().ToString("N"));
        WorkingDir = Path.Combine(Root, "repo");
        StudyDir = Path.Combine(WorkingDir, "docs", "v3-framework", "studies", study);
        BatchDir = Path.Combine(StudyDir, "batches", batch);
        LaunchDir = Path.Combine(Root, "launch");
        Directory.CreateDirectory(Path.Combine(WorkingDir, ".git"));
        Directory.CreateDirectory(Path.Combine(BatchDir, "items"));
        Directory.CreateDirectory(LaunchDir);
        File.WriteAllText(Path.Combine(StudyDir, "directions-1.md"), Directions);
    }

    /// <summary>Writes the definition, the index and the item bodies for N items.</summary>
    public string WriteItems(int count, string? kind = "sample", string model = "sonnet", string? effort = null, string? extra = null)
    {
        var def = $"# {Path.GetFileName(BatchDir)} — definition\n\n- directions: ../../directions-1.md\n" + (kind is null ? "" : $"- kind: {kind}\n") + $"- model: {model}\n" + (effort is null ? "" : $"- effort: {effort}\n") + (extra ?? "");
        File.WriteAllText(DefinitionPath, def);
        File.WriteAllText(Path.Combine(BatchDir, "index.md"),
            IndexFile.Render(Path.GetFileName(BatchDir), "tools/StoryPlanner.TestItemizer, 1", "v1-archive", "a note id",
                null, Enumerable.Range(1, count).Select(i => ($"item-{i:00}", $"note-{i}", $"note {i}"))));
        foreach (var i in Enumerable.Range(1, count))
            File.WriteAllText(Path.Combine(BatchDir, "items", $"item-{i:00}.md"), $"The note {i}.\n");
        return DefinitionPath;
    }

    public void Dispose()
    {
        try { Directory.Delete(Root, recursive: true); } catch { }
    }
}

public static class Wait
{
    public static async Task Until(Func<bool> condition, int timeoutMs = 5000, string? what = null)
    {
        var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
        while (!condition())
        {
            if (DateTime.UtcNow > deadline) throw new TimeoutException("waited for: " + (what ?? "condition"));
            await Task.Delay(20);
        }
    }
}
