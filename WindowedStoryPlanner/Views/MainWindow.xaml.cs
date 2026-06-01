using System.Windows;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views;

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