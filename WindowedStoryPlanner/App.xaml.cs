using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoryPlanner.Core;
using WindowedStoryPlanner.ViewModels;
using WindowedStoryPlanner.Views;

namespace WindowedStoryPlanner;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    // The "Brain" of the application that holds all services
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                // --- ADD THIS BLOCK ---
                // This registers the AppDbContext so the "StoryService" can find it.
                // We use "Data Source=StoryPlanner.db" to create a local file.
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite("Data Source=StoryPlanner.db");
                });
                
                // 2. REGISTER SINGLETON SERVICE
                // This is your "Central Store". Both MainWindow and ChapterWindow
                // will talk to this ONE instance.
                services.AddSingleton<IStoryService, StoryService>();

                // 3. REGISTER WINDOWS
                // We register MainWindow so DI can inject the Service into it automatically.
                services.AddSingleton<MainWindow>();
                
                // (Optional) Register other windows if they have complex dependencies,
                // otherwise you can just 'new' them up manually.
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        // 4. Show Window
        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);
        
        // Register a global handler for the KeyDown event on ANY Window
        EventManager.RegisterClassHandler(typeof(Window), 
            Window.KeyDownEvent, 
            new KeyEventHandler(OnGlobalKeyDown));
    }
    
    private void OnGlobalKeyDown(object sender, KeyEventArgs e)
    {
        // Check for Ctrl + S
        if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        {
            // Access the Singleton Instance of MainViewModel
            var vm = MainViewModel.Instance;

            // Execute the command if it exists and can execute
            if (vm != null && vm.SaveChangesCommand.CanExecute(null))
            {
                vm.SaveChangesCommand.Execute(null);
                e.Handled = true; // Prevent other controls from reacting
            }
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        // Gracefully shut down the host (saves logs, closes connections)
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}