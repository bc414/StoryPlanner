using System.Windows;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Interaction logic for SourceMaterialWindow.xaml
    /// </summary>
    public partial class SourceMaterialWindow : Window
    {
        public SourceMaterialWindow()
        {
            // Disposal of the DataContext is WindowManager.ShowSingleton's job.
            InitializeComponent();
        }
    }
}
