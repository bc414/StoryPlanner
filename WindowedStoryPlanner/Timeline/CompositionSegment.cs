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

public sealed class CompositionSegment
{
    public string Fill { get; init; } = "#888888";
    public double Width { get; init; }
}
