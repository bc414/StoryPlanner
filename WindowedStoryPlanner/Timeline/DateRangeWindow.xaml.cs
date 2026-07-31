using System.Windows;

namespace WindowedStoryPlanner;

/// <summary>
/// Interaction logic for DateRangeWindow.xaml. Disposal of the DataContext is
/// WindowManager.ShowSingleton's job.
/// </summary>
public partial class DateRangeWindow : Window
{
    public DateRangeWindow()
    {
        InitializeComponent();
    }
}
