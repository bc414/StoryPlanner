using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner;

public class WindowManager : IWindowManager
{
    private readonly Func<EditorMode, NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow> _commonWindowFactory;
    private readonly IViewModelRegistry _registry;
    private readonly IStoryService _storyService;
    private readonly Dictionary<object, Window> _singletonWindows = new();

    /// <summary>Keys for the windows there is only ever one of, regardless of what they show.</summary>
    private static readonly object DateRangeWindowKey = new();
    private static readonly object MissingFieldWindowKey = new();
    private static readonly object PovCharactersWindowKey = new();

    public WindowManager(
        Func<EditorMode, NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow> commonWindowFactory,
        IViewModelRegistry registry,
        IStoryService storyService)
    {
        _commonWindowFactory = commonWindowFactory;
        _registry = registry;
        _storyService = storyService;
    }

    // CommonWindow is intentionally multi-instance — these always create a new one.

    public void OpenSubjectWindow(
        SubjectViewModel subject,
        EditorMode mode = EditorMode.Expansion,
        PlotPointSubjectLinkViewModel? initialLink = null)
    {
        if (mode is not (EditorMode.Expansion or EditorMode.Linking))
            throw new ArgumentOutOfRangeException(nameof(mode), mode,
                "A subject's editor opens in Expansion or Linking; Gardener takes a plot point.");

        _commonWindowFactory(mode, subject, initialLink).Show();
    }

    public void OpenPlotPointWindow(PlotPointViewModel plotPoint) =>
        _commonWindowFactory(EditorMode.Gardener, plotPoint, null).Show();

    /// <summary>
    /// Who owns a singleton window's DataContext — which decides whether closing the window
    /// disposes it. Stated at every call site because it is NOT inferable from the type:
    /// ChapterViewModel and ThemeDetailViewModel are both IDisposable, and disposing the first
    /// one is a bug. Before 2026-08-06 this was an <c>as IDisposable</c> type test, so a view
    /// model's lifetime silently depended on whether it happened to implement an interface.
    /// </summary>
    private enum ContextLifetime
    {
        /// <summary>Built by the factory below for this window alone, and disposed when the
        /// window closes. Every cross-cut view model is this: they subscribe to registry events
        /// that must be released on close, which is why disposal lives here rather than in each
        /// window's code-behind.</summary>
        OwnedByWindow,

        /// <summary>A registry element or DI singleton that outlives any window showing it, so
        /// this class never disposes it. Their teardown belongs to whoever created them —
        /// ProjectLoader for registry elements, the container for DI singletons — and a window
        /// that needs per-open setup uses the refcounted
        /// <see cref="NarrativeElementViewModel.OnWindowOpened"/> pair instead.</summary>
        OutlivesWindow
    }

    /// <summary>
    /// Shows the one window registered under <paramref name="key"/>, creating it if it isn't open.
    /// </summary>
    private Window ShowSingleton(object key, Func<Window> create, ContextLifetime lifetime)
    {
        if (_singletonWindows.TryGetValue(key, out var existing) && existing.IsLoaded)
        {
            if (existing.WindowState == WindowState.Minimized)
                existing.WindowState = WindowState.Normal;
            existing.Activate();
            return existing;
        }

        var window = create();
        _singletonWindows[key] = window;
        window.Closed += (_, _) =>
        {
            Unregister(window);
            if (lifetime == ContextLifetime.OwnedByWindow)
                (window.DataContext as IDisposable)?.Dispose();
        };
        window.Show();
        return window;
    }

    /// <summary>
    /// Closes every window opened over the current project, main window excluded. ProjectLoader
    /// calls this BEFORE it disposes the outgoing file's element view models, so each window runs
    /// its ordinary Closed teardown while its DataContext is still valid. Without it a file switch
    /// left editors and cross-cuts bound to view models ProjectLoader had just disposed, and left
    /// this class's singleton dictionary keyed on them — reopening one of those chapters would
    /// then activate a window showing the previous file.
    /// </summary>
    public void CloseAllProjectWindows()
    {
        // ToList first: closing mutates Application.Current.Windows mid-enumeration.
        foreach (var window in Application.Current.Windows
                                          .OfType<Window>()
                                          .Where(w => w != Application.Current.MainWindow)
                                          .ToList())
            window.Close();

        _singletonWindows.Clear();   // each Closed handler unregisters itself; this is the backstop
    }

    /// <summary>
    /// Drops every key pointing at this window — by value, not by the key it was created under.
    /// A retargeted ChapterWindow is re-keyed, and removing the original key on close would evict
    /// whichever window has since claimed it, leaving that one open but unregistered.
    /// </summary>
    private void Unregister(Window window)
    {
        foreach (var key in _singletonWindows.Where(kv => kv.Value == window).Select(kv => kv.Key).ToList())
            _singletonWindows.Remove(key);
    }

    /// <summary>
    /// Opens a ChapterWindow for the given chapter — singleton per chapter. The chapter VM belongs
    /// to the registry and its window must not dispose it: <see cref="NarrativeElementViewModel"/>
    /// subscribes to the registry's note events in its constructor and nothing re-subscribes, so a
    /// dispose on close permanently stopped that chapter's note counts (the Chapters tab card)
    /// updating for the rest of the session. Per-open track setup is the OnWindowOpened refcount,
    /// which ChapterWindow already drives.
    /// </summary>
    public void OpenChapterWindow(ChapterViewModel chapter) =>
        ShowSingleton(chapter,
            () => new ChapterWindow(this, _registry) { DataContext = chapter },
            ContextLifetime.OutlivesWindow);

    /// <summary>
    /// Re-points an open ChapterWindow at a different chapter, keeping the window itself (and its
    /// size and position) — the window's own Story → Chapter picker. Retarget rather than
    /// close-and-reopen: <see cref="NarrativeElementViewModel.Dispose"/> unsubscribes an element
    /// from the app-lifetime registry events for good, and ShowSingleton disposes on close, so
    /// hopping between chapters that way would quietly deaden every chapter passed through.
    /// </summary>
    public void RetargetChapterWindow(ChapterWindow window, ChapterViewModel target)
    {
        if (ReferenceEquals(window.DataContext, target)) return;

        // The per-chapter singleton wins: if the target already has a window, go there and leave
        // this one showing what it was, rather than ending up with two windows on one chapter.
        if (_singletonWindows.TryGetValue(target, out var existing) && existing.IsLoaded && existing != window)
        {
            if (existing.WindowState == WindowState.Minimized)
                existing.WindowState = WindowState.Normal;
            existing.Activate();
            return;
        }

        // Balances the OnWindowOpened that ChapterWindow's Loaded did — that refcount is what
        // builds and tears down the note tracks. The outgoing VM stays alive in the registry.
        (window.DataContext as NarrativeElementViewModel)?.OnWindowClosed();

        Unregister(window);
        window.DataContext = target;
        _singletonWindows[target] = window;
        target.OnWindowOpened();
    }

    /// <summary>
    /// Opens the Floating Plot Points window — application-wide singleton.
    /// The VM is passed in to avoid a circular DI dependency, and being a DI singleton it is
    /// handed back out after this window closes, so this window does not own it.
    /// </summary>
    public void OpenFloatingPlotPointsWindow(FloatingPlotPointsViewModel vm) =>
        ShowSingleton(vm, () => new FloatingPlotPointsWindow { DataContext = vm },
            ContextLifetime.OutlivesWindow);

    /// <summary>
    /// Opens a ThemeWindow for the given theme — singleton per theme.
    /// </summary>
    public void OpenThemeWindow(ThemeViewModel theme) =>
        ShowSingleton(theme,
            () => new ThemeWindow { DataContext = new ThemeDetailViewModel(theme, _registry, this) },
            ContextLifetime.OwnedByWindow);

    /// <summary>
    /// Opens a SourceMaterialWindow for the given source material — singleton per source material.
    /// </summary>
    public void OpenSourceMaterialWindow(SourceMaterialViewModel sourceMaterial) =>
        ShowSingleton(sourceMaterial, () => new SourceMaterialWindow
        {
            DataContext = new SourceMaterialDetailViewModel(sourceMaterial, _registry, this)
        }, ContextLifetime.OwnedByWindow);

    /// <summary>
    /// Opens a SourceMaterialPartWindow for the given Part — singleton per Part. The drill-down
    /// from the coverage grid's "N notes" cell, complementing OpenSourceMaterialWindow (which
    /// shows citations of the whole Work).
    /// </summary>
    public void OpenSourceMaterialPartWindow(SourceMaterialPartViewModel part) =>
        ShowSingleton(part, () => new SourceMaterialPartWindow
        {
            DataContext = new SourceMaterialPartDetailViewModel(part, _registry, this)
        }, ContextLifetime.OwnedByWindow);

    /// <summary>
    /// Opens the world-date range window — application-wide singleton, unlike the per-entity
    /// cross-cuts above, because the criterion is typed into the window rather than picked from
    /// a library row.
    /// </summary>
    public void OpenDateRangeWindow() =>
        ShowSingleton(DateRangeWindowKey, () => new DateRangeWindow
        {
            DataContext = new DateRangeNotesViewModel(_registry, _storyService, this)
        }, ContextLifetime.OwnedByWindow);

    /// <summary>
    /// Opens the empty-field cross-cut — application-wide singleton, like the date range. Reached
    /// from both the Themes and the Sources tab, so an already-open window switches to the field
    /// the caller asked for rather than a second one appearing.
    /// </summary>
    public void OpenMissingFieldWindow(MissingNoteField field)
    {
        var window = ShowSingleton(MissingFieldWindowKey, () => new MissingFieldWindow
        {
            DataContext = new MissingFieldNotesViewModel(_registry, this, field)
        }, ContextLifetime.OwnedByWindow);

        if (window.DataContext is MissingFieldNotesViewModel vm)
            vm.SelectedField = field;   // no-op on a freshly created one
    }

    /// <summary>
    /// Opens a ConversationReaderWindow for the given conversation — singleton per conversation.
    /// The conversation VM is a registry element doing double duty as library card and reader
    /// DataContext, so it outlives the reader window; it is not IDisposable today, which is
    /// exactly why the old type test made this look safe when it was only accidentally so.
    /// </summary>
    public void OpenConversationReaderWindow(ConversationViewModel conversation) =>
        ShowSingleton(conversation, () => new ConversationReaderWindow { DataContext = conversation },
            ContextLifetime.OutlivesWindow);

    /// <summary>
    /// Opens the POV-characters manager — application-wide singleton, like the date range and
    /// missing-field windows. The dedicated picker+list, not an inline checkbox on every subject
    /// widget: a real file's Character subjects run in the hundreds, so the flag needed a place
    /// of its own.
    /// </summary>
    public void OpenPovCharactersWindow() =>
        ShowSingleton(PovCharactersWindowKey, () => new PovCharactersWindow
        {
            DataContext = new PovCharactersViewModel(_registry)
        }, ContextLifetime.OwnedByWindow);
}
