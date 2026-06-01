using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
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
                services.AddSingleton<ThemeLibraryViewModel>();
                services.AddSingleton<FloatingPlotPointsViewModel>();

                services.AddSingleton<ProjectLoader>();
                services.AddSingleton<ViewModelLocator>();

                services.AddSingleton<AppSettings>();

                // Windows
                services.AddSingleton<MainWindow>();
                services.AddSingleton<Func<EditorMode, NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow>>(sp =>
                    (mode, element, initialLink) => new CommonWindow(
                        sp.GetRequiredService<IViewModelRegistry>(),
                        sp.GetRequiredService<IContentFactory>(),
                        sp.GetRequiredService<IStoryService>(),
                        sp.GetRequiredService<AppSettings>(),
                        mode,
                        element,
                        initialLink));
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        AppHost.Services.GetRequiredService<MainWindow>().Show();

        if (e.Args.Length > 0 && File.Exists(e.Args[0]))
        {
            var path = e.Args[0];
            var fileManager = AppHost.Services.GetRequiredService<FileManagerViewModel>();
            await fileManager.OpenProjectFromPath(path);

            // Navigate to Subjects tab (index 3)
            var locator = AppHost.Services.GetRequiredService<ViewModelLocator>();
            locator.SelectedTabIndex = 3;

            // Set archive mode if filename contains "archive"
            if (Path.GetFileNameWithoutExtension(path).Contains("archive", StringComparison.OrdinalIgnoreCase))
            {
                var settings = AppHost.Services.GetRequiredService<AppSettings>();
                settings.IsArchiveMode = true;
            }
        }

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