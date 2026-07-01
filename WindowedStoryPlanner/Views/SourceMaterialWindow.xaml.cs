using System.Windows;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views
{
    /// <summary>
    /// Interaction logic for SourceMaterialWindow.xaml
    /// </summary>
    public partial class SourceMaterialWindow : Window
    {
        public SourceMaterialWindow()
        {
            InitializeComponent();
            Closed += (_, _) => (DataContext as SourceMaterialDetailViewModel)?.Dispose();
        }
    }
}
