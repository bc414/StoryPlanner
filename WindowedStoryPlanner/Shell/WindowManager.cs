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
    private readonly Dictionary<object, Window> _singletonWindows = new();

    public WindowManager(
        Func<EditorMode, NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow> commonWindowFactory,
        IViewModelRegistry registry)
    {
        _commonWindowFactory = commonWindowFactory;
        _registry = registry;
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
    /// Opens a ChapterWindow for the given chapter — singleton per chapter.
    /// </summary>
    public void OpenChapterWindow(ChapterViewModel chapter)
    {
        if (_singletonWindows.TryGetValue(chapter, out var existing) && existing.IsLoaded)
        {
            if (existing.WindowState == WindowState.Minimized)
                existing.WindowState = WindowState.Normal;
            existing.Activate();
            return;
        }

        var window = new ChapterWindow { DataContext = chapter };
        _singletonWindows[chapter] = window;
        window.Closed += (_, _) => _singletonWindows.Remove(chapter);
        window.Show();
    }

    /// <summary>
    /// Opens the Floating Plot Points window — application-wide singleton.
    /// The VM is passed in to avoid a circular DI dependency.
    /// </summary>
    public void OpenFloatingPlotPointsWindow(FloatingPlotPointsViewModel vm)
    {
        if (_singletonWindows.TryGetValue(vm, out var existing) && existing.IsLoaded)
        {
            if (existing.WindowState == WindowState.Minimized)
                existing.WindowState = WindowState.Normal;
            existing.Activate();
            return;
        }

        var window = new FloatingPlotPointsWindow { DataContext = vm };
        _singletonWindows[vm] = window;
        window.Closed += (_, _) => _singletonWindows.Remove(vm);
        window.Show();
    }

    /// <summary>
    /// Opens a ThemeWindow for the given theme — singleton per theme.
    /// </summary>
    public void OpenThemeWindow(ThemeViewModel theme)
    {
        if (_singletonWindows.TryGetValue(theme, out var existing) && existing.IsLoaded)
        {
            if (existing.WindowState == WindowState.Minimized)
                existing.WindowState = WindowState.Normal;
            existing.Activate();
            return;
        }

        var window = new ThemeWindow { DataContext = new ThemeDetailViewModel(theme, _registry) };
        _singletonWindows[theme] = window;
        window.Closed += (_, _) => _singletonWindows.Remove(theme);
        window.Show();
    }

    /// <summary>
    /// Opens a SourceMaterialWindow for the given source material — singleton per source material.
    /// </summary>
    public void OpenSourceMaterialWindow(SourceMaterialViewModel sourceMaterial)
    {
        if (_singletonWindows.TryGetValue(sourceMaterial, out var existing) && existing.IsLoaded)
        {
            if (existing.WindowState == WindowState.Minimized)
                existing.WindowState = WindowState.Normal;
            existing.Activate();
            return;
        }

        var window = new SourceMaterialWindow { DataContext = new SourceMaterialDetailViewModel(sourceMaterial, _registry) };
        _singletonWindows[sourceMaterial] = window;
        window.Closed += (_, _) => _singletonWindows.Remove(sourceMaterial);
        window.Show();
    }

    /// <summary>
    /// Opens a SourceMaterialPartWindow for the given Part — singleton per Part. The drill-down
    /// from the coverage grid's "N notes" cell, complementing OpenSourceMaterialWindow (which
    /// shows citations of the whole Work).
    /// </summary>
    public void OpenSourceMaterialPartWindow(SourceMaterialPartViewModel part)
    {
        if (_singletonWindows.TryGetValue(part, out var existing) && existing.IsLoaded)
        {
            if (existing.WindowState == WindowState.Minimized)
                existing.WindowState = WindowState.Normal;
            existing.Activate();
            return;
        }

        var window = new SourceMaterialPartWindow { DataContext = new SourceMaterialPartDetailViewModel(part, _registry) };
        _singletonWindows[part] = window;
        window.Closed += (_, _) => _singletonWindows.Remove(part);
        window.Show();
    }

    /// <summary>
    /// Opens a ConversationReaderWindow for the given conversation — singleton per conversation.
    /// </summary>
    public void OpenConversationReaderWindow(ConversationViewModel conversation)
    {
        if (_singletonWindows.TryGetValue(conversation, out var existing) && existing.IsLoaded)
        {
            if (existing.WindowState == WindowState.Minimized)
                existing.WindowState = WindowState.Normal;
            existing.Activate();
            return;
        }

        var window = new ConversationReaderWindow { DataContext = conversation };
        _singletonWindows[conversation] = window;
        window.Closed += (_, _) => _singletonWindows.Remove(conversation);
        window.Show();
    }
}
