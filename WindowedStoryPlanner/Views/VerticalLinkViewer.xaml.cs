using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner.Views;

public partial class VerticalLinkViewer : UserControl
{
    public VerticalLinkViewer()
    {
        InitializeComponent();
    }
    
    // 1. Register the Dependency Property
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            nameof(ItemsSource),            // Property Name
            typeof(IEnumerable),            // Property Type (IEnumerable works for List, ObservableCollection, etc.)
            typeof(VerticalLinkViewer),     // Owner Type (This Class)
            new PropertyMetadata(null));    // Default Value

    // 2. The Wrapper Property (what you see in XAML)
    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
}