using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views
{
    /// <summary>
    /// Interaction logic for CommonWindow.xaml
    /// </summary>
    public partial class CommonWindow : Window
    {
        public CommonWindow()
        {
            InitializeComponent();

            // Override mouse-wheel on the root scroller so that wheel events
            // originating inside nested collections (ListBox, ItemsControl, etc.)
            // always propagate to the window-level ScrollViewer instead of being
            // consumed by the inner control.
            AddHandler(
                UIElement.PreviewMouseWheelEvent,
                new MouseWheelEventHandler(OnPreviewMouseWheel),
                handledEventsToo: true);
        }

        // ── Lifecycle ────────────────────────────────────────────────────────

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (DataContext is OwnerViewModel vm)
            {
                vm.OnWindowOpened();
                vm.RegisterTrackHotkeys(this);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            (DataContext as OwnerViewModel)?.OnWindowClosed();
            base.OnClosed(e);
        }

        // ── Scroll forwarding ────────────────────────────────────────────────

        /// <summary>
        /// Intercepts all mouse-wheel events — including those already marked
        /// Handled by inner ItemsControls / ListBoxes — and re-dispatches them
        /// to the root ScrollViewer so the window always scrolls.
        /// </summary>
        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (RootScroller is null) return;

            // Only forward if the event did NOT originate on the root scroller
            // itself (avoids an infinite loop).
            if (e.OriginalSource is DependencyObject source &&
                IsNestedUnderInnerScroller(source))
            {
                return; // let inner scrollers handle their own content
            }

            e.Handled = true;
            var args = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,
                Source = sender
            };
            RootScroller.RaiseEvent(args);
        }

        /// <summary>
        /// Returns true only when <paramref name="source"/> is inside a
        /// ScrollViewer that is NOT the root window scroller — i.e. the
        /// inner control owns its own scrollable region that should keep
        /// scroll focus.  For note tracks we intentionally have NO inner
        /// ScrollViewer, so this returns false and the wheel always reaches
        /// RootScroller.
        /// </summary>
        private bool IsNestedUnderInnerScroller(DependencyObject source)
        {
            var current = source;
            while (current is not null)
            {
                if (current is ScrollViewer sv && !ReferenceEquals(sv, RootScroller))
                    return true;
                if (ReferenceEquals(current, this))
                    break;
                current = VisualTreeHelper.GetParent(current);
            }
            return false;
        }
    }
}
