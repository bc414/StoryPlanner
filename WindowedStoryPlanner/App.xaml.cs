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
using WindowedStoryPlanner;

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
        InstallGlobalExceptionHandlers();

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
                services.AddSingleton<StoryLibraryViewModel>();
                services.AddSingleton<ChapterLibraryViewModel>();
                services.AddSingleton<ThemeLibraryViewModel>();
                services.AddSingleton<SourceMaterialLibraryViewModel>();
                services.AddSingleton<ConversationLibraryViewModel>();
                services.AddSingleton<FloatingPlotPointsViewModel>();
                services.AddSingleton<TimelineViewModel>();
                services.AddSingleton<GlobalSearchViewModel>();
                services.AddSingleton<ProgressViewModel>();
                services.AddSingleton<PropertyGapsViewModel>();

                services.AddSingleton<ExportService>();
                services.AddSingleton<ExportViewModel>();

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

    /// <summary>
    /// Three doors an exception can leave by, all of which default to killing the process or —
    /// worse — saying nothing at all. Wired in the constructor so they are live before any
    /// window, service, or project load can throw.
    /// </summary>
    private void InstallGlobalExceptionHandlers()
    {
        // 1. The UI thread: commands, event handlers, bindings, `async void` handlers. Recoverable
        //    — the dispatcher keeps pumping once the exception is marked handled, so a bad click
        //    costs you the click, not the session.
        DispatcherUnhandledException += (_, args) =>
        {
            args.Handled = true;
            CrashReporter.Report(args.Exception, "UI thread", recovered: true);
        };

        // 2. Faulted tasks nobody awaited — the app is full of `_ = SaveAsync()`. Without this the
        //    exception surfaces only when the GC finalizes the Task, and .NET does not terminate
        //    for it, so a FAILED SAVE IS SILENT: the user believes their work is on disk when it
        //    is not. This is the data-loss path that has no visible symptom.
        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            args.SetObserved();
            CrashReporter.Report(args.Exception, "background task", recovered: true);
        };

        // 3. Anything else — a non-UI thread. The CLR is going to terminate whatever we do here;
        //    all that is left is to say so and leave a log behind.
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is Exception ex)
                CrashReporter.Report(ex, "background thread", recovered: false);
        };
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

            // Navigate to Subjects tab (index 4 — a "Stories" tab was inserted at index 1,
            // between File Management and Chapters, shifting every tab after it by one)
            var locator = AppHost.Services.GetRequiredService<ViewModelLocator>();
            locator.SelectedTabIndex = 4;

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