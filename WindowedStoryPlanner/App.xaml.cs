using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoryPlanner;
using StoryPlanner.Services;
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
                // 2. REGISTER SINGLETON SERVICE
                // This is your "Central Store". Both MainWindow and ChapterWindow
                // will talk to this ONE instance.
                services.AddSingleton<StoryService>();

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
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        // Gracefully shut down the host (saves logs, closes connections)
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}