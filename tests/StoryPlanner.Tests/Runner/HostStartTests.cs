using System.Diagnostics;
using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// How <c>start</c> launches the host: through the shell, hidden, redirecting nothing, so the
/// host inherits none of the caller's standard handles and a caller reading its output to the
/// end returns once <c>start</c> exits. Tier: pure (no process is started).
/// </summary>
public class HostStartTests
{
    [Fact]
    public void The_host_is_started_through_the_shell_and_inherits_no_standard_handles()
    {
        var psi = HostStart.StartInfo(@"C:\runner\StoryPlanner.AgentRunner.exe", @"C:\repo");

        Assert.True(psi.UseShellExecute);
        Assert.False(psi.RedirectStandardInput);
        Assert.False(psi.RedirectStandardOutput);
        Assert.False(psi.RedirectStandardError);
        Assert.Equal(ProcessWindowStyle.Hidden, psi.WindowStyle);
    }

    [Fact]
    public void The_host_runs_the_host_verb_from_the_callers_directory()
    {
        var psi = HostStart.StartInfo(@"C:\runner\StoryPlanner.AgentRunner.exe", @"C:\repo");

        Assert.Equal(@"C:\runner\StoryPlanner.AgentRunner.exe", psi.FileName);
        Assert.Equal(["host"], psi.ArgumentList);
        Assert.Equal(@"C:\repo", psi.WorkingDirectory);
    }
}
