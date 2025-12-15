using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteCollectionViewModel : ObservableObject, IDropTarget
{
    // 1. The Source of Truth (EF Core tracks this) and the view model (Note is an ObservableObject)
    public ObservableCollection<Note> NoteCollection;

    public NoteCollectionViewModel(ObservableCollection<Note> sourceCollection)
    {
        NoteCollection = sourceCollection;
    }

    [RelayCommand]
    private void AddNote()
    {
        // A. Create Data
        var newNote = new Note { Content = "New Note" };

        // B. Update EF Core (This marks it as 'Added' in the DB context)
        NoteCollection.Add(newNote);
    }

    [RelayCommand]
    private void RemoveNote(Note note)
    {
        //Comes via relative source
        // A. Remove from UI
        NoteCollection.Remove(note);
    }

    public void DragOver(IDropInfo dropInfo)
    {
        
    }

    public void Drop(IDropInfo dropInfo)
    {
        //reorder
    }
}