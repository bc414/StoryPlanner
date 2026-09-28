using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The real launcher over a stand-in child (cmd), for what a child that dies at startup leaves
/// behind: its own exit code and its stderr in the log, not an unexplained -1. The item is large
/// enough that writing it to a child which never reads its stdin breaks the pipe. Tier: pure
/// (a temp folder and a short-lived local process; Windows, as the runner is).
/// </summary>
public class ProcessChildLauncherTests : IDisposable
{
    private readonly string _dir = Directory.CreateTempSubdirectory("launcher-").FullName;

    public void Dispose() { try { Directory.Delete(_dir, recursive: true); } catch { } }

    [Fact]
    public async Task A_child_that_dies_at_startup_records_its_exit_code_and_its_stderr()
    {
        var log = new List<string>();
        var launcher = new ProcessChildLauncher(msg => { lock (log) log.Add(msg); }, fileName: "cmd");
        var request = new ChildRequest(
            Item: "note-1",
            Args: ["/c", "echo boom 1>&2 & exit /b 3"],
            Stdin: new string('x', 4 * 1024 * 1024),
            StreamPath: Path.Combine(_dir, "stream.jsonl"),
            LaunchDir: _dir,
            IdleLimit: TimeSpan.FromMinutes(1));

        var exit = await launcher.LaunchAsync(request, _ => { }, () => { }, CancellationToken.None);

        Assert.Equal(3, exit);
        lock (log)
        {
            Assert.Contains(log, l => l.StartsWith("! [note-1]") && l.Contains("boom"));
            Assert.Contains(log, l => l.StartsWith("note-1: the item could not be written to the child's stdin"));
        }
    }

    [Fact]
    public async Task A_child_that_cannot_be_started_is_minus_one_with_the_reason_logged()
    {
        var log = new List<string>();
        var launcher = new ProcessChildLauncher(msg => { lock (log) log.Add(msg); }, fileName: "no-such-program-" + Guid.NewGuid().ToString("N"));
        var request = new ChildRequest("note-2", [], "x", Path.Combine(_dir, "stream.jsonl"), _dir, TimeSpan.FromMinutes(1));

        var exit = await launcher.LaunchAsync(request, _ => { }, () => { }, CancellationToken.None);

        Assert.Equal(-1, exit);
        lock (log) Assert.Contains(log, l => l.StartsWith("note-2: the launch failed (Win32Exception"));
    }
}
