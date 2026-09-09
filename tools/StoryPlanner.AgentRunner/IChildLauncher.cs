using System.Diagnostics;
using System.Text;

namespace StoryPlanner.AgentRunner;

/// <summary>A running child, as much of it as the batch loop needs: an id to show and a way to kill it.</summary>
public interface IChildHandle
{
    int Pid { get; }
    void Kill();
}

/// <summary>One call to make: the arguments, the item text for stdin, where the stream goes, where to launch from, and how long a silent stream is tolerated.</summary>
public sealed record ChildRequest(
    string Item,
    IReadOnlyList<string> Args,
    string Stdin,
    string StreamPath,
    string LaunchDir,
    TimeSpan IdleLimit);

/// <summary>
/// Launches one <c>claude -p</c> child and returns its exit code. Behind an interface so the
/// batch loop's queue, ceilings, pause and cancel semantics are testable with a fake that
/// never starts a process. Exit codes the launcher itself assigns: -1 could not start,
/// -2 cancelled by the token, -3 killed for an idle stream.
/// </summary>
public interface IChildLauncher
{
    Task<int> LaunchAsync(ChildRequest request, Action<IChildHandle> track, Action onStreamAdvanced, CancellationToken ct);
}

/// <summary>
/// The real launcher: the child's working directory is the launch folder outside the repo,
/// the item's text is its whole stdin, its stdout (one JSON event per line) is teed to
/// <c>stream.jsonl</c> as it arrives, its stderr goes to the log, and a child whose stream
/// has been silent for the idle limit has its whole process tree killed. There is no
/// absolute time limit (decisions.md, "the idle limit is the host's").
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
        foreach (var a in request.Args) psi.ArgumentList.Add(a);

        Process? process = null;
        try
        {
            process = Process.Start(psi);
            if (process is null) return -1;
            track(new Handle(process));

            await process.StandardInput.WriteAsync(request.Stdin);
            process.StandardInput.Close();

            var lastLine = DateTimeOffset.UtcNow;
            var stdoutTask = Task.Run(async () =>
            {
                await using var stream = new FileStream(request.StreamPath, FileMode.Create, FileAccess.Write, FileShare.Read);
                await using var writer = new StreamWriter(stream, new UTF8Encoding(false));
                while (await process.StandardOutput.ReadLineAsync() is { } line)
                {
                    await writer.WriteLineAsync(line);
                    await writer.FlushAsync();
                    lastLine = DateTimeOffset.UtcNow;
                    onStreamAdvanced();
                }
            });
            var stderrTask = Task.Run(async () =>
            {
                while (await process.StandardError.ReadLineAsync() is { } line)
                    log($"! [{request.Item}] {line}");
            });

            var idle = false;
            while (!process.HasExited)
            {
                try { await Task.Delay(1000, ct); }
                catch (OperationCanceledException) { break; }
                if (DateTimeOffset.UtcNow - lastLine > request.IdleLimit) { idle = true; break; }
            }
            if (!process.HasExited)
            {
                try { process.Kill(entireProcessTree: true); } catch { }
                try { await stdoutTask; } catch { }
                if (idle)
                {
                    log($"{request.Item}: no output for {request.IdleLimit.TotalMinutes:F0} min — process tree killed");
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
            log($"Failed to start claude for {request.Item}: {ex.Message}");
            return -1;
        }
        finally
        {
            process?.Dispose();
        }
    }
}
