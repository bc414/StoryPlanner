using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class LocationViewModel : EntityViewModel
{
    private readonly Location _location;
    public Location Location => _location;

    public LocationViewModel(Location location)
    {
        _location = location;

        // Initialize the notes collection generic to the entity
        NoteCollectionViewModel = new NoteCollectionViewModel(location.Notes);
    }

    // --- Properties Wrapper ---

    public string Name
    {
        get => _location.Name;
        set => SetProperty(_location.Name, value, _location, (u, n) => u.Name = n);
    }

    public string Region
    {
        get => _location.Region;
        set => SetProperty(_location.Region, value, _location, (u, n) => u.Region = n);
    }

    public string Description
    {
        get => _location.Description;
        set => SetProperty(_location.Description, value, _location, (u, n) => u.Description = n);
    }
}