using System.Windows;

namespace WindowedStoryPlanner;

/// <summary>
/// Interaction logic for MissingFieldWindow.xaml. Disposal of the DataContext is
/// WindowManager.ShowSingleton's job.
/// </summary>
public partial class MissingFieldWindow : Window
{
    public MissingFieldWindow()
    {
        InitializeComponent();
    }
}
