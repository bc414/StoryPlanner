using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner.Views;

public partial class PlotPointCollectionViewer : UserControl
{
    // 1. Register the Dependency Property
    public static readonly DependencyProperty IsDragReorderEnabledProperty =
        DependencyProperty.Register(
            nameof(IsDragReorderEnabled), 
            typeof(bool), 
            typeof(PlotPointCollectionViewer), 
            new PropertyMetadata(false)); // Default is false (safe for the other 5 windows)

    // 2. The Wrapper Property
    public bool IsDragReorderEnabled
    {
        get => (bool)GetValue(IsDragReorderEnabledProperty);
        set => SetValue(IsDragReorderEnabledProperty, value);
    }
    
    // 1. Register the Dependency Property
    public static readonly DependencyProperty ShowReorderButtonsProperty =
        DependencyProperty.Register(
            nameof(ShowReorderButtons), 
            typeof(bool), 
            typeof(PlotPointCollectionViewer), 
            new PropertyMetadata(false)); // Default is false (safe for the other 5 windows)

    // 2. The Wrapper Property
    public bool ShowReorderButtons
    {
        get => (bool)GetValue(ShowReorderButtonsProperty);
        set => SetValue(ShowReorderButtonsProperty, value);
    }

    public PlotPointCollectionViewer()
    {
        InitializeComponent();
    }
}