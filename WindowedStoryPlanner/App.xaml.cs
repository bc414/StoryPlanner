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
            .ConfigureServices((_, services) =>
            {
                // Infrastructure
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("Data Source=StoryPlanner.db"));

                // Core
                services.AddSingleton<IStoryService, StoryService>();

                // ViewModel services
                services.AddSingleton<IViewModelRegistry, ViewModelRegistry>();
                services.AddSingleton<IContentFactory, ContentFactory>();
                services.AddSingleton<IContentDeleter, ContentDeleter>();
                services.AddSingleton<IWindowManager, WindowManager>();

                // Tab ViewModels
                services.AddSingleton<DefinitionsEditorViewModel>();
                services.AddSingleton<SubjectLibraryViewModel>();
                services.AddSingleton<FileManagerViewModel>();
                services.AddSingleton<ChapterLibraryViewModel>();

                services.AddSingleton<ProjectLoader>();
                services.AddSingleton<ViewModelLocator>();

                // Windows
                services.AddSingleton<MainWindow>();
                services.AddSingleton<Func<NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow>>(sp =>
                    (element, initialLink) => new CommonWindow(
                        sp.GetRequiredService<IViewModelRegistry>(),
                        sp.GetRequiredService<IContentFactory>(),
                        sp.GetRequiredService<IStoryService>(),
                        element,
                        initialLink));
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        AppHost.Services.GetRequiredService<MainWindow>().Show();

        base.OnStartup(e);

        EventManager.RegisterClassHandler(
            typeof(Window),
            Window.KeyDownEvent,
            new KeyEventHandler(OnGlobalKeyDown));
    }

    private void OnGlobalKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        {
            var vm = AppHost!.Services.GetRequiredService<FileManagerViewModel>();
            if (vm.SaveChangesCommand.CanExecute(null))
            {
                vm.SaveChangesCommand.Execute(null);
                e.Handled = true;
            }
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}