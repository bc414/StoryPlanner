using System.Windows;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(ViewModelLocator locator)
    {
        InitializeComponent();
        DataContext = locator;
    }
}