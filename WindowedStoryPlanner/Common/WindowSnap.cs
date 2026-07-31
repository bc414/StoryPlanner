using System.Windows;

namespace WindowedStoryPlanner;

/// <summary>
/// Snaps a window to the top or bottom half of the screen, so it can be parked against
/// another window while working. Uses <see cref="SystemParameters.WorkArea"/> rather than
/// the raw screen size, so the taskbar is never covered.
/// </summary>
internal static class WindowSnap
{
    public static void TopHalf(Window window)
    {
        var wa = SystemParameters.WorkArea;

        window.WindowState = WindowState.Normal;
        window.Left   = wa.Left;
        window.Top    = wa.Top;
        window.Width  = wa.Width;
        window.Height = wa.Height / 2.0;
    }

    public static void BottomHalf(Window window)
    {
        var wa = SystemParameters.WorkArea;

        window.WindowState = WindowState.Normal;
        window.Left   = wa.Left;
        window.Top    = wa.Top + wa.Height / 2.0;
        window.Width  = wa.Width;
        window.Height = wa.Height / 2.0;
    }
}
