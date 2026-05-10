using System;
using System.Collections.Generic;
using System.Text;

namespace WindowedStoryPlanner.ViewModels
{
    public interface IWindowManager
    {
        void OpenCommonWindow(
            NarrativeElementViewModel element,
            PlotPointSubjectLinkViewModel? initialLink = null);

        void OpenChapterWindow(ChapterViewModel chapter);
    }
}
