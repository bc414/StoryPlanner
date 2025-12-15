using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteCollectionViewModel : ObservableObject
{
    // 1. The Source of Truth (EF Core tracks this)
    private readonly ObservableCollection<Note> _sourceCollection;

    // 2. The UI Collection (The View binds to this)
    public ObservableCollection<NoteViewModel> ViewCollection { get; } = new();

    public NoteCollectionViewModel(ObservableCollection<Note> sourceCollection)
    {
        _sourceCollection = sourceCollection;

        // 3. Hydrate the View from the Source on load
        foreach (var note in _sourceCollection)
        {
            ViewCollection.Add(CreateWrapper(note));
        }
    }
    
    // Helper to keep logic consistent
    private NoteViewModel CreateWrapper(Note note)
    {
        // Pass a "Remove Action" so the item can delete itself
        return new NoteViewModel(note);
    }

    [RelayCommand]
    private void AddNote()
    {
        // A. Create Data
        var newNote = new Note { Content = "New Note" };

        // B. Update EF Core (This marks it as 'Added' in the DB context)
        _sourceCollection.Add(newNote);

        // C. Update UI
        ViewCollection.Add(CreateWrapper(newNote));
    }

    private void RemoveNote(NoteViewModel vm)
    {
        // A. Remove from UI
        ViewCollection.Remove(vm);

        // B. Remove from EF Core (This marks it as 'Deleted' in the DB context)
        // We access the underlying model inside the VM
        _sourceCollection.Remove(vm.Model); 
    }
}