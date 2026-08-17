using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner;

/// <summary>
/// The four snap-to-half buttons shared by CommonWindow and ConversationReaderWindow.
/// No dependency properties: unlike the other shared controls in this folder, the thing this
/// one acts on is the hosting <see cref="Window"/>, not a view model — so it resolves the host
/// at click time and needs nothing bound to it. This is window chrome, so it stays code-behind
/// rather than acquiring a view model.
/// </summary>
public partial class WindowSnapButtons : UserControl
{
    public WindowSnapButtons() => InitializeComponent();

    private void SnapLeftHalf_Click(object sender, RoutedEventArgs e) => Snap(WindowSnap.LeftHalf);

    private void SnapTopHalf_Click(object sender, RoutedEventArgs e) => Snap(WindowSnap.TopHalf);

    private void SnapBottomHalf_Click(object sender, RoutedEventArgs e) => Snap(WindowSnap.BottomHalf);

    private void SnapRightHalf_Click(object sender, RoutedEventArgs e) => Snap(WindowSnap.RightHalf);

    private void Snap(Action<Window> snap)
    {
        if (Window.GetWindow(this) is { } window) snap(window);
    }
}
