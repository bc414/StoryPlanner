using System.Windows;
using System.Windows.Controls;

namespace WindowedStoryPlanner.Views
{
    public partial class NoteTrackView : UserControl
    {
        public NoteTrackView()
        {
            InitializeComponent();
        }

        // Cancels automatic scroll-to-selection that fires when SelectedItem
        // changes on any ListBox inside this track. Must be handled here (inside
        // the ScrollViewer) rather than on the ScrollViewer itself, because the
        // ScrollViewer acts on the event in its internal override before a XAML
        // handler on the ScrollViewer element would fire.
        private void SectionsItemsControl_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}
