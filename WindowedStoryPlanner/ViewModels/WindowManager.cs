using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using WindowedStoryPlanner.Views;

namespace WindowedStoryPlanner.ViewModels;

public class WindowManager : IWindowManager
{
    // No IContentFactory — the delegate captures it from the service provider at
    // registration time, so WindowManager has no direct knowledge of it at all.
    private readonly Func<NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow> _commonWindowFactory;
    private readonly Dictionary<object, Window> _singletonWindows = new();

    public WindowManager(
        Func<NarrativeElementViewModel, PlotPointSubjectLinkViewModel?, CommonWindow> commonWindowFactory)
    {
        _commonWindowFactory = commonWindowFactory;
    }

    /// <summary>
    /// Opens a new CommonWindow. Always creates a new instance —
    /// CommonWindow is intentionally multi-instance.
    /// </summary>
    public void OpenCommonWindow(
        NarrativeElementViewModel element,
        PlotPointSubjectLinkViewModel? initialLink = null)
    {
        _commonWindowFactory(element, initialLink).Show();
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
}
