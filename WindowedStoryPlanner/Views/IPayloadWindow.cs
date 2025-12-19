using System.Windows.Input;

namespace WindowedStoryPlanner.Views;

public interface IPayloadWindow
{
    // The specific property we need to inject
    ICommand UnlinkCommand { get; set; }
    
    // We also include DataContext here so we don't have to cast to FrameworkElement
    object DataContext { get; set; }
}