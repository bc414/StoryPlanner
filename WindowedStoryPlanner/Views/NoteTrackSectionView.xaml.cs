using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views
{
    /// <summary>
    /// Interaction logic for NoteTrackSectionView.xaml
    /// </summary>
    public partial class NoteTrackSectionView : UserControl
    {
        public NoteTrackSectionView()
        {
            InitializeComponent();
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
        //   F1–F12    — move to track (handled at CommonWindow level)
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

        // After a state toggle the note has moved to a sibling section on the same
        // NoteTrackViewModel. Walk up to find NoteTrackView, then locate whichever
        // section now owns the note and set SelectedNote on it so the ListBox
        // selection follows and CommonWindow._selectedNote stays current.
        private void SelectNoteInSiblingSection(NoteViewModel note)
        {
            var trackView = FindAncestor<NoteTrackView>(this);
            if (trackView?.DataContext is not NoteTrackViewModel trackVm) return;

            foreach (var section in trackVm.Sections)
            {
                if (section.TargetState == note.NoteState)
                {
                    section.SelectedNote = note;
                    return;
                }
            }
        }

        private static T? FindAncestor<T>(DependencyObject child) where T : DependencyObject
        {
            var current = VisualTreeHelper.GetParent(child);
            while (current is not null)
            {
                if (current is T match) return match;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}
