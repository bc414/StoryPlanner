using System.Diagnostics;

namespace StoryPlanner.AgentRunner;

/// <summary>
/// How <c>start</c> launches the host. Through the shell, hidden, so the host gets a console of
/// its own and inherits none of the caller's standard handles: started with
/// <c>UseShellExecute = false</c> it held the caller's stdout open for its whole life, and a
/// caller that reads to the end of that stream — a Claude Code tool call — never returned
/// (2026-09-27). The host's record is <c>host-log.txt</c>, so its console is not needed.
/// </summary>
public static class HostStart
{
    public static ProcessStartInfo StartInfo(string exe, string workingDir)
    {
        // The host's working directory is the CLI's: the page lists the batches beneath it.
        var psi = new ProcessStartInfo(exe) { UseShellExecute = true, WindowStyle = ProcessWindowStyle.Hidden, WorkingDirectory = workingDir };
        psi.ArgumentList.Add("host");
        return psi;
    }
}
