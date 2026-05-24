using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using WindowedStoryPlanner.Views;

namespace WindowedStoryPlanner.ViewModels;

public class WindowManager : IWindowManager
{
    private readonly Func<EditorMode, NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow> _commonWindowFactory;
    private readonly Dictionary<object, Window> _singletonWindows = new();

    public WindowManager(
        Func<EditorMode, NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow> commonWindowFactory)
    {
        _commonWindowFactory = commonWindowFactory;
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
}
