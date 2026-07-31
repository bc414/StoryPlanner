using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WindowedStoryPlanner;

/// <summary>
/// For the app's many "mutate then save, don't block the UI" call sites.
///
/// The bare <c>_ = SaveAsync();</c> form discards the Task, and with it any exception it carries.
/// The failure then surfaces only if and when the GC finalizes the faulted Task — .NET does not
/// terminate for that, so in practice <b>a save that fails does so silently</b>. The user keeps
/// editing a project they believe is on disk. That is the worst failure mode this app has: not a
/// crash, which is loud, but a quiet one that is only discovered after closing the window.
///
/// This makes the discard explicit and observed, so a failed write is reported the moment it
/// happens rather than at an arbitrary GC later.
/// </summary>
public static class FireAndForgetExtensions
{
    public static void FireAndForget(this Task task, [CallerMemberName] string caller = "")
    {
        if (task.IsCompletedSuccessfully) return;   // the overwhelmingly common case, no allocation

        _ = Observe(task, caller);

        static async Task Observe(Task task, string caller)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                CrashReporter.Report(ex, $"{caller} (background save)", recovered: true);
            }
        }
    }
}
