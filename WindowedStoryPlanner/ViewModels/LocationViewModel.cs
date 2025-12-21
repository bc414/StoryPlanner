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
        NoteCollectionViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(NoteCollectionViewModel.IsNoteReorderMode))
            {
                OnPropertyChanged(nameof(IsLinkingMode));
            }
        };

        // 2. Define Sorter (Chronological)
        Comparison<PlotPoint> standardSorter = (a, b) =>
        {
            int aChapter = a.Chapter?.OrderIndex ?? int.MaxValue;
            int bChapter = b.Chapter?.OrderIndex ?? int.MaxValue;

            if (aChapter != bChapter) return aChapter.CompareTo(bChapter);
            return a.OrderInChapter.CompareTo(b.OrderInChapter);
        };

        // 3. Initialize View-Only Collection
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel();
        PlotPointCollectionViewModel.SetAndSortItems(_location.PlotPoints, standardSorter);

        // 4. Live Updates
        _location.PlotPoints.CollectionChanged += (s, e) => 
        {
            PlotPointCollectionViewModel.SetAndSortItems(_location.PlotPoints, standardSorter);
        };
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