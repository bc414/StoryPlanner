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

/// <summary>One item inside a cell — rendered as a row when the cell is expanded in the panel.</summary>
public sealed class CellEntry
{
    public string Label { get; init; } = "";
    public string Detail { get; init; } = "";
    public string Fill { get; init; } = "#888888";
    public string Body { get; init; } = "";
    public bool IsFlagged { get; init; }
    public bool IsPlotPoint { get; init; }
}
