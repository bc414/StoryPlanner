using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Interaction logic for NoteTrackSectionView.xaml
    /// </summary>
    public partial class NoteTrackSectionView : UserControl
    {
        public NoteTrackSectionView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        // Section VMs are destroyed and recreated on every Uninitialize/Initialize
        // cycle, so the NoteCreated subscription must follow the DataContext —
        // a constructor-time subscription would pin dead VMs.
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is NoteTrackSectionViewModel oldVm)
                oldVm.NoteCreated -= OnNoteCreated;
            if (e.NewValue is NoteTrackSectionViewModel newVm)
                newVm.NoteCreated += OnNoteCreated;
        }

        // Focus the freshly inserted note's content box. Targeted by name:
        // NoteView's visual tree puts the WorldDate and FlagReason TextBoxes
        // before the content one, and collapsed elements still count, so a
        // find-first-TextBox walk would focus the wrong box on every track.
        private void OnNoteCreated(NoteViewModel note)
        {
            // Container generation is async; Loaded priority runs after layout.
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                if (NotesList.ItemContainerGenerator.ContainerFromItem(note) is not ListBoxItem item)
                    return;
                FindDescendant<TextBox>(item, tb => tb.Name == "ContentBox")?.Focus();
            }));
        }

        // TextBox consumes MouseLeftButtonDown so it never reaches the ListBoxItem.
        // PreviewMouseLeftButtonDown tunnels down before TextBox can swallow it,
        // letting us explicitly select the item.
        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item)
                item.IsSelected = true;
        }

        // All note hotkeys are intercepted here via PreviewKeyDown (tunneling),
        // which fires before any child TextBox or ListBox can handle the event.
        //
        // Hotkey map:
        //   Alt+Up    — move note up (reorder within section)
        //   Alt+Down  — move note down (reorder within section)
        //   Page Up   — toggle Confirmed ↔ Unset  (selection follows to destination section)
        //   Page Down — toggle Flagged ↔ Unset    (selection follows to destination section)
        private void NoteTrackSectionView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is not NoteTrackSectionViewModel vm) return;

            bool alt = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Alt);

            if (alt && e.Key == Key.Up)
            {
                vm.MoveNoteUpCommand.Execute(null);
                e.Handled = true;
            }
            else if (alt && e.Key == Key.Down)
            {
                vm.MoveNoteDownCommand.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.PageUp)
            {
                vm.ToggleConfirmedCommand.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.PageDown)
            {
                vm.ToggleFlaggedCommand.Execute(null);
                e.Handled = true;
            }
        }

        private static T? FindDescendant<T>(DependencyObject parent, Predicate<T> match)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typed && match(typed)) return typed;
                if (FindDescendant(child, match) is T found) return found;
            }
            return null;
        }
    }
}
