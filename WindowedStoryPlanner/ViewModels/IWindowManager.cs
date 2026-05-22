using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindowedStoryPlanner.ViewModels
{
    public interface IWindowManager
    {
        void OpenCommonWindow(
            EditorMode mode,
            NarrativeElementViewModel element,
            PlotPointSubjectLinkViewModel? initialLink = null);

        void OpenChapterWindow(ChapterViewModel chapter);
    }
}
