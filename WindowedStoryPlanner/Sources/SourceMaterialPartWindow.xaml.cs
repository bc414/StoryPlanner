using System.Windows;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Interaction logic for SourceMaterialPartWindow.xaml
    /// </summary>
    public partial class SourceMaterialPartWindow : Window
    {
        public SourceMaterialPartWindow()
        {
            // Disposal of the DataContext is WindowManager.ShowSingleton's job.
            InitializeComponent();
        }
    }
}
