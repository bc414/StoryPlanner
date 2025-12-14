using System.Windows;
using StoryPlanner.Core;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IStoryService _storyStateService;

    // The 'AppHost' will automatically pass this service in!
    public MainWindow(IStoryService storyStateService)
    {
        InitializeComponent();
        _storyStateService = storyStateService;
        
        // --- BINDING THE VIEWMODEL ---
        // We manually create the ViewModel using the injected service
        // and assign it to the DataContext. 
        // This allows the XAML bindings (like {Binding Characters}) to work.
        DataContext = new MainViewModel(_storyStateService);
    }
}