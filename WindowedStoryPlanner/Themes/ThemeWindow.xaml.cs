using System.Windows;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Interaction logic for ThemeWindow.xaml
    /// </summary>
    public partial class ThemeWindow : Window
    {
        public ThemeWindow()
        {
            // Disposal of the DataContext is WindowManager.ShowSingleton's job.
            InitializeComponent();
        }
    }
}
