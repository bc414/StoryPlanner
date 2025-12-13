using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class CharacterViewModel : ObservableObject
{
    private readonly Character _character;
    
    public NotesViewModel NotesViewModel { get; }

    public CharacterViewModel(Character character)
    {
        _character = character;

        NotesViewModel = new NotesViewModel(character.Notes);
    }

    // --- Properties Wrapper ---
    // We wrap these to trigger PropertyChanged notifications for the UI

    public string Name
    {
        get => _character.Name;
        set => SetProperty(_character.Name, value, _character, (u, n) => u.Name = n);
    }

    public string Inspiration
    {
        get => _character.Inspiration;
        set => SetProperty(_character.Inspiration, value, _character, (u, n) => u.Inspiration = n);
    }

    public string Archetype
    {
        get => _character.Archetype;
        set => SetProperty(_character.Archetype, value, _character, (u, n) => u.Archetype = n);
    }

    public string Description
    {
        get => _character.Description;
        set => SetProperty(_character.Description, value, _character, (u, n) => u.Description = n);
    }

    // Direct access to the ObservableCollection for the NoteViewer
    public ObservableCollection<Note> Notes => _character.Notes;

    // --- Commands ---

    
}