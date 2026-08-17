using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WindowedStoryPlanner;

/// <summary>
/// Snaps a window to one half of the screen — left, right, top or bottom — so it can be parked
/// against another window while working. Uses the monitor's <em>work area</em> rather than its
/// raw bounds, so the taskbar is never covered, and resolves the monitor the window is currently
/// on rather than always the primary one.
/// </summary>
internal static class WindowSnap
{
    public static void LeftHalf(Window window)
    {
        var a = WorkAreaFor(window);
        Apply(window, a.Left, a.Top, a.Width / 2.0, a.Height);
    }

    public static void RightHalf(Window window)
    {
        var a = WorkAreaFor(window);
        Apply(window, a.Left + a.Width / 2.0, a.Top, a.Width / 2.0, a.Height);
    }

    public static void TopHalf(Window window)
    {
        var a = WorkAreaFor(window);
        Apply(window, a.Left, a.Top, a.Width, a.Height / 2.0);
    }

    public static void BottomHalf(Window window)
    {
        var a = WorkAreaFor(window);
        Apply(window, a.Left, a.Top + a.Height / 2.0, a.Width, a.Height / 2.0);
    }

    private static void Apply(Window window, double left, double top, double width, double height)
    {
        window.WindowState = WindowState.Normal;   // both hosts launch Maximized
        window.Left   = left;
        window.Top    = top;
        window.Width  = width;
        window.Height = height;
    }

    /// <summary>
    /// The work area of the monitor this window is on, in device-independent units.
    /// Falls back to <see cref="SystemParameters.WorkArea"/> (the primary monitor) whenever the
    /// window has no hwnd yet or the interop calls fail — a snap button must never throw.
    /// </summary>
    private static Rect WorkAreaFor(Window window)
    {
        var hwnd = new WindowInteropHelper(window).Handle;
        if (hwnd == IntPtr.Zero)
            return SystemParameters.WorkArea;

        var monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
        if (monitor == IntPtr.Zero)
            return SystemParameters.WorkArea;

        var info = new MONITORINFO { cbSize = Marshal.SizeOf<MONITORINFO>() };
        if (!GetMonitorInfo(monitor, ref info))
            return SystemParameters.WorkArea;

        // rcWork is physical pixels; Window.Left/Top/Width/Height are DIPs. The app is
        // system-DPI-aware (no app.manifest, no PerMonitorV2 opt-in), so one uniform factor
        // covers the whole virtual desktop.
        var transform = PresentationSource.FromVisual(window)?.CompositionTarget?.TransformFromDevice;
        if (transform is not { } m)
            return SystemParameters.WorkArea;

        var topLeft     = m.Transform(new Point(info.rcWork.left,  info.rcWork.top));
        var bottomRight = m.Transform(new Point(info.rcWork.right, info.rcWork.bottom));
        return new Rect(topLeft, bottomRight);
    }

    private const int MONITOR_DEFAULTTONEAREST = 2;

    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int flags);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int left, top, right, bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MONITORINFO
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public int dwFlags;
    }
}
