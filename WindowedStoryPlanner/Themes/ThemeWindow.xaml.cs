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
            InitializeComponent();
            Closed += (_, _) => (DataContext as ThemeDetailViewModel)?.Dispose();
        }
    }
}
