using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace WindowedStoryPlanner.ViewModels
{
    public interface IPlotPointListViewModel
    {
        ICollectionView PlotPoints { get; }
        PlotPointViewModel? SelectedPlotPoint { get; set; }
        ICommand MovePlotPointUpCommand { get; }
        ICommand MovePlotPointDownCommand { get; }
        ICommand AddPlotPointCommand { get; }
    }
}
