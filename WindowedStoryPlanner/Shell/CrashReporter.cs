using System;
using System.IO;
using System.Text;
using System.Windows;

namespace WindowedStoryPlanner;

/// <summary>
/// The application's last line of defence. WPF's default for an unhandled exception on the UI
/// thread is to tear the process down, which in this app means losing whatever was typed but not
/// yet saved — and every note edit is typed straight into a live POCO. An unchecked cast in one
/// window is not a good enough reason to lose an afternoon of authoring.
///
/// So: report, write a log next to the app, and keep running wherever the CLR permits it. This
/// deliberately does NOT try to save the project on the way down. A crash means the in-memory
/// state is of unknown validity, writes to a .storyplan are Brian's call, and a well-meant
/// emergency save is exactly how a recoverable session becomes a corrupted file.
/// </summary>
public static class CrashReporter
{
    private static readonly object _gate = new();
    private static string _lastSignature = "";
    private static int _repeatCount;

    /// <summary>Where the log goes — beside the executable, so it travels with a published copy.</summary>
    public static string LogPath => Path.Combine(AppContext.BaseDirectory, "crash-log.txt");

    /// <summary>
    /// Reports a failure the user should know about. <paramref name="recovered"/> is false only
    /// when the CLR is going to terminate regardless, which changes what the dialog can promise.
    /// </summary>
    public static void Report(Exception ex, string origin, bool recovered)
    {
        // Callable from any thread — faulted background saves report from the thread pool.
        var dispatcher = Application.Current?.Dispatcher;
        if (dispatcher is not null && !dispatcher.CheckAccess())
        {
            dispatcher.BeginInvoke(() => Report(ex, origin, recovered));
            return;
        }

        lock (_gate)
        {
            TryLog(ex, origin);   // always logged, however often it repeats

            // A failure that recurs on every render or keystroke would otherwise trap the user in
            // a dialog loop with no way to reach File > Save. Suppress the REPEAT, not the app's
            // whole session: a different failure later still gets its dialog.
            var signature = $"{origin}|{ex.GetType().FullName}|{ex.Message}";
            _repeatCount = signature == _lastSignature ? _repeatCount + 1 : 0;
            _lastSignature = signature;
            if (_repeatCount >= 3) return;

            var sb = new StringBuilder();
            sb.AppendLine(recovered
                ? "Something went wrong, but the app is still running."
                : "Something went wrong and the app has to close.");
            sb.AppendLine();
            sb.AppendLine($"{ex.GetType().Name}: {ex.Message}");
            sb.AppendLine();
            sb.AppendLine(recovered
                ? "Your project is still open. Save your work — the action that failed did not complete, so check whatever you were editing."
                : "Any edit made since the last save may be lost.");
            sb.AppendLine();
            sb.AppendLine($"Details written to:\n{LogPath}");

            try
            {
                MessageBox.Show(sb.ToString(),
                    recovered ? "StoryPlanner — recovered from an error" : "StoryPlanner — fatal error",
                    MessageBoxButton.OK,
                    recovered ? MessageBoxImage.Warning : MessageBoxImage.Error);
            }
            catch
            {
                // No UI available (shutting down, or the failure was in the dialog stack).
                // The log is already written; nothing further is useful here.
            }
        }
    }

    private static void TryLog(Exception ex, string origin)
    {
        try
        {
            File.AppendAllText(LogPath,
                $"""

                ===== {DateTime.Now:yyyy-MM-dd HH:mm:ss} [{origin}] =====
                {ex}

                """);
        }
        catch
        {
            // A logger that throws while reporting a crash helps nobody.
        }
    }
}
