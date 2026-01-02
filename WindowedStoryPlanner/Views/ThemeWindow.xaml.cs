using System.Windows;

namespace WindowedStoryPlanner.Views;

public partial class ThemeWindow : Window
{
    public ThemeWindow()
    {
        InitializeComponent();

        // 1. Apply the limit BEFORE the window loads.
        // Using WorkArea is better than PrimaryScreenHeight as it accounts for the Taskbar.
        this.MaxHeight = SystemParameters.WorkArea.Height * 0.9;

        // 2. Wait until the window is effectively on screen and sized.
        this.ContentRendered += (s, e) =>
        {
            // Switch to Manual mode. This "freezes" the window at its current 
            // calculated size (which was clamped by MaxHeight).
            this.SizeToContent = SizeToContent.Manual;

            // Now remove the limit so the user can resize it larger if they want.
            this.MaxHeight = double.PositiveInfinity;
        };
    }
}