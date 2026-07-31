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
            InitializeComponent();
            Closed += (_, _) => (DataContext as SourceMaterialPartDetailViewModel)?.Dispose();
        }
    }
}
