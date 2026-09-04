using System.Diagnostics;
using System.Text;

namespace StoryPlanner.AgentRunner;

/// <summary>A running child, as much of it as the batch loop needs: an id to show and a way to kill it.</summary>
public interface IChildHandle
{
    int Pid { get; }
    void Kill();
}

public sealed record ChildRequest(
    ResolvedJob Job,
    string PromptText,
    string StreamPath,
    string LaunchDir,
    string? McpConfigPath,
    TimeSpan Timeout);

/// <summary>
/// Launches one <c>claude -p</c> child and returns its exit code. Behind an interface so the
/// batch loop's queue, ceilings, pause and cancel semantics are testable with a fake that
/// never starts a process. Exit codes the launcher itself assigns: -1 could not start,
/// -2 cancelled by the token, -3 timed out.
/// </summary>
public interface IChildLauncher
{
    Task<int> LaunchAsync(ChildRequest request, Action<IChildHandle> track, Action onStreamAdvanced, CancellationToken ct);
}

/// <summary>
/// The real launcher: the child's working directory is the launch folder outside the repo,
/// the prompt is its whole stdin, its stdout (one JSON event per line) is teed to
/// <c>stream.jsonl</c> as it arrives, its stderr goes to the log, and a child past the
/// timeout has its whole process tree killed.
/// </summary>
public sealed class ProcessChildLauncher(Action<string> log) : IChildLauncher
{
    private sealed class Handle(Process p) : IChildHandle
    {
        public int Pid => p.Id;
        public void Kill() { try { p.Kill(entireProcessTree: true); } catch { } }
    }

    public async Task<int> LaunchAsync(ChildRequest request, Action<IChildHandle> track, Action onStreamAdvanced, CancellationToken ct)
    {
        var job = request.Job;
        var psi = new ProcessStartInfo
        {
            FileName = "claude",
            WorkingDirectory = request.LaunchDir,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardInputEncoding = new UTF8Encoding(false),
            StandardOutputEncoding = Encoding.UTF8,
        };
        foreach (var a in RunnerPlan.BuildArgs(job, request.McpConfigPath)) psi.ArgumentList.Add(a);

        Process? process = null;
        try
        {
            process = Process.Start(psi);
            if (process is null) return -1;
            track(new Handle(process));

            // The prompt is the whole stdin — no positional prompt, so the hashed document is
            // exactly what the agent received (and Windows argument-length limits never apply).
            await process.StandardInput.WriteAsync(request.PromptText);
            process.StandardInput.Close();

            var stdoutTask = Task.Run(async () =>
            {
                await using var stream = new FileStream(request.StreamPath, FileMode.Create, FileAccess.Write, FileShare.Read);
                await using var writer = new StreamWriter(stream, new UTF8Encoding(false));
                while (await process.StandardOutput.ReadLineAsync() is { } line)
                {
                    await writer.WriteLineAsync(line);
                    await writer.FlushAsync();
                    onStreamAdvanced();
                }
            });
            var stderrTask = Task.Run(async () =>
            {
                while (await process.StandardError.ReadLineAsync() is { } line)
                    log($"! [{job.Id}] {line}");
            });

            using var timeoutCts = new CancellationTokenSource(request.Timeout);
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);
            try
            {
                await process.WaitForExitAsync(linked.Token);
            }
            catch (OperationCanceledException)
            {
                try { process.Kill(entireProcessTree: true); } catch { }
                try { await stdoutTask; } catch { }
                if (timeoutCts.IsCancellationRequested && !ct.IsCancellationRequested)
                {
                    log($"{job.Id}: timed out after {request.Timeout.TotalMinutes:F0} min — process tree killed");
                    return -3;
                }
                return -2;
            }
            await stdoutTask;
            await stderrTask;
            return process.ExitCode;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            log($"Failed to start claude for {job.Id}: {ex.Message}");
            return -1;
        }
        finally
        {
            process?.Dispose();
        }
    }
}
