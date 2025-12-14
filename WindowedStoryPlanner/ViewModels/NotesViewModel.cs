using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class NotesViewModel : ObservableObject
{
    
    // The collection we are managing (passed in from the parent)
    public ObservableCollection<Note> TargetCollection { get; }

    public NotesViewModel(ObservableCollection<Note> targetCollection)
    {
        TargetCollection = targetCollection;
    }

    [RelayCommand]
    public void DeleteNote(Note note)
    {
        //This command originates from the note objects themselves and is picked up here, the container
        if (note == null) return;

        // 1. UI Update
        if (TargetCollection.Contains(note))
        {
            TargetCollection.Remove(note);
        }
    }

    [RelayCommand]
    public void AddNote()
    {
        //This command originates from the container because the new note doesn't exist yet
        var newNote = new Note
        {
            Content = "New Note"
        };

        // Adding to the ObservableCollection updates the UI and EF Core tracking automatically
        TargetCollection.Add(newNote);
    }
}