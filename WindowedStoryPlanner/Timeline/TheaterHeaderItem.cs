using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowedStoryPlanner;

/// <summary>The pinned column header — lives in its own strip that scrolls horizontally with
/// the canvas but never vertically, so the theater a mark belongs to is always readable.</summary>
public sealed class TheaterHeaderItem : TimelineItem
{
    public string Name { get; init; } = "";
    public bool IsBandShaded { get; init; } // alternating neutral banding — never a hue
    public bool IsCollapsed { get; init; }
    public int TheaterId { get; init; }
    public string CountLabel { get; init; } = "";
    /// <summary>First three characters — the collapsed header is only ~26px wide, and
    /// "Equ/Aqu/Cha/Sky/Her/Sta/Tzi/Zeb/Cry/Ole" stay mutually distinguishable. Full name in the tooltip.</summary>
    public string ShortName { get; init; } = "";
}
