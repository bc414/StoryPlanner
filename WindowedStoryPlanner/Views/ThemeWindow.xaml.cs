using System.Windows;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views
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
