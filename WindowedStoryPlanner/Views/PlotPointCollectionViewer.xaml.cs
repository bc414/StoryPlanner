using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner.Views;

public partial class PlotPointCollectionViewer : UserControl
{
    public static readonly DependencyProperty ShowReorderButtonsProperty =
        DependencyProperty.Register("ShowReorderButtons", typeof(bool), typeof(PlotPointCollectionViewer), new PropertyMetadata(false));

    public bool ShowReorderButtons
    {
        get { return (bool)GetValue(ShowReorderButtonsProperty); }
        set { SetValue(ShowReorderButtonsProperty, value); }
    }

    public PlotPointCollectionViewer()
    {
        InitializeComponent();
    }
}