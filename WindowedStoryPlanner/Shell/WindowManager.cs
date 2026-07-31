using StoryPlanner.Core;
using System;
using System.Collections.Generic;
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

    public WindowManager(
        Func<EditorMode, NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow> commonWindowFactory,
        IViewModelRegistry registry,
        IStoryService storyService)
    {
        _commonWindowFactory = commonWindowFactory;
        _registry = registry;
        _storyService = storyService;
    }

    /// <summary>
    /// Opens a new CommonWindow. Always creates a new instance —
    /// CommonWindow is intentionally multi-instance.
    /// </summary>
    public void OpenCommonWindow(
        EditorMode mode,
        NarrativeElementViewModel element,
        PlotPointSubjectLinkViewModel? initialLink = null)
    {
        _commonWindowFactory(mode, element, initialLink).Show();
    }

    /// <summary>
    /// Shows the one window registered under <paramref name="key"/>, creating it if it isn't
    /// open. Disposal is handled here rather than in each window's code-behind, so a DataContext
    /// that holds subscriptions (every TaggedNotesViewModelBase does) is released on close
    /// without each new window having to remember.
    /// </summary>
    private Window ShowSingleton(object key, Func<Window> create)
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
            _singletonWindows.Remove(key);
            (window.DataContext as IDisposable)?.Dispose();
        };
        window.Show();
        return window;
    }

    /// <summary>
    /// Opens a ChapterWindow for the given chapter — singleton per chapter.
    /// </summary>
    public void OpenChapterWindow(ChapterViewModel chapter) =>
        ShowSingleton(chapter, () => new ChapterWindow { DataContext = chapter });

    /// <summary>
    /// Opens the Floating Plot Points window — application-wide singleton.
    /// The VM is passed in to avoid a circular DI dependency.
    /// </summary>
    public void OpenFloatingPlotPointsWindow(FloatingPlotPointsViewModel vm) =>
        ShowSingleton(vm, () => new FloatingPlotPointsWindow { DataContext = vm });

    /// <summary>
    /// Opens a ThemeWindow for the given theme — singleton per theme.
    /// </summary>
    public void OpenThemeWindow(ThemeViewModel theme) =>
        ShowSingleton(theme, () => new ThemeWindow { DataContext = new ThemeDetailViewModel(theme, _registry) });

    /// <summary>
    /// Opens a SourceMaterialWindow for the given source material — singleton per source material.
    /// </summary>
    public void OpenSourceMaterialWindow(SourceMaterialViewModel sourceMaterial) =>
        ShowSingleton(sourceMaterial, () => new SourceMaterialWindow
        {
            DataContext = new SourceMaterialDetailViewModel(sourceMaterial, _registry)
        });

    /// <summary>
    /// Opens a SourceMaterialPartWindow for the given Part — singleton per Part. The drill-down
    /// from the coverage grid's "N notes" cell, complementing OpenSourceMaterialWindow (which
    /// shows citations of the whole Work).
    /// </summary>
    public void OpenSourceMaterialPartWindow(SourceMaterialPartViewModel part) =>
        ShowSingleton(part, () => new SourceMaterialPartWindow
        {
            DataContext = new SourceMaterialPartDetailViewModel(part, _registry)
        });

    /// <summary>
    /// Opens the world-date range window — application-wide singleton, unlike the per-entity
    /// cross-cuts above, because the criterion is typed into the window rather than picked from
    /// a library row.
    /// </summary>
    public void OpenDateRangeWindow() =>
        ShowSingleton(DateRangeWindowKey, () => new DateRangeWindow
        {
            DataContext = new DateRangeNotesViewModel(_registry, _storyService, this)
        });

    /// <summary>
    /// Opens the empty-field cross-cut — application-wide singleton, like the date range. Reached
    /// from both the Themes and the Sources tab, so an already-open window switches to the field
    /// the caller asked for rather than a second one appearing.
    /// </summary>
    public void OpenMissingFieldWindow(MissingNoteField field)
    {
        var window = ShowSingleton(MissingFieldWindowKey, () => new MissingFieldWindow
        {
            DataContext = new MissingFieldNotesViewModel(_registry, field)
        });

        if (window.DataContext is MissingFieldNotesViewModel vm)
            vm.SelectedField = field;   // no-op on a freshly created one
    }

    /// <summary>
    /// Opens a ConversationReaderWindow for the given conversation — singleton per conversation.
    /// </summary>
    public void OpenConversationReaderWindow(ConversationViewModel conversation) =>
        ShowSingleton(conversation, () => new ConversationReaderWindow { DataContext = conversation });
}
