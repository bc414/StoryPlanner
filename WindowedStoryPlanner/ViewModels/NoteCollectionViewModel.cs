using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core.Models;
using System.Linq;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteCollectionViewModel : ObservableObject, IDropTarget
{
    public ObservableCollection<Note> NoteCollection { get; set; }
    public ObservableCollection<SourceMaterial> AvailableSources
    {
        get
        {
            return MainViewModel.Instance.SourceMaterials;
        }
    }
    
    [ObservableProperty]
    private bool _isNoteReorderMode;

    [ObservableProperty]
    private bool _showCommands;

    public NoteCollectionViewModel(ObservableCollection<Note> sourceCollection)
    {
        NoteCollection = sourceCollection;
    }

    [RelayCommand]
    private void AddNote()
    {
        var newNote = new Note
        {
            Content = "New Note",
            SortOrder = NoteCollection.Count
        };
        NoteCollection.Add(newNote);
    }

    // --- Drag & Drop Implementation ---

    public void DragOver(IDropInfo dropInfo)
    {
        // 1. Identify the dragged data
        if (dropInfo.Data is not Note sourceNote) return;

        // 2. CHECK TARGET: Is it the Trash Zone?
        // We check the Name of the visual element that the mouse is hovering over
        var targetElement = dropInfo.VisualTarget as FrameworkElement;

        if (targetElement != null && targetElement.Name == "TrashZone")
        {
            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            return; // Allow drop
        }

        // 3. CHECK TARGET: Is it the List (Reordering)?
        if (dropInfo.TargetItem is Note)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Move;
        }
    }

    public void Drop(IDropInfo dropInfo)
    {
        var sourceNote = dropInfo.Data as Note;
        if (sourceNote == null) return;

        var targetElement = dropInfo.VisualTarget as FrameworkElement;

        // --- CASE A: DELETE (Dropped on Trash) ---
        if (targetElement != null && targetElement.Name == "TrashZone")
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete this note?\n\n\"{sourceNote.Content}\"",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                NoteCollection.Remove(sourceNote);
                UpdateSortOrders();
            }
            return;
        }

        // --- CASE B: REORDER (Dropped on List) ---
        GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.Drop(dropInfo);
        UpdateSortOrders();
    }

    private void UpdateSortOrders()
    {
        for (int i = 0; i < NoteCollection.Count; i++)
        {
            NoteCollection[i].SortOrder = i;
        }
    }

    [RelayCommand]
    private void DeleteNote(Note noteToDelete)
    {
        if (noteToDelete == null) return;
        NoteCollection.Remove(noteToDelete);
        UpdateSortOrders();
    }
}