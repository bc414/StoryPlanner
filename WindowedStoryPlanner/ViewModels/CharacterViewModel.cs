using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class CharacterViewModel : EntityViewModel
{
    private readonly Character _character;
    public Character Character => _character;

    public CharacterViewModel(Character character, IEditorCoordinator editorCoordinator) : base(editorCoordinator)
    {
        _character = character;

        NoteCollectionViewModel = new NoteCollectionViewModel(character.Notes);
        NoteCollectionViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(NoteCollectionViewModel.IsNoteReorderMode))
            {
                OnPropertyChanged(nameof(IsLinkingMode));
            }
        };
        
        // 1. Define the "Chapter -> Order" Comparator
        Comparison<PlotPoint> standardSorter = (a, b) =>
        {
            // Handle null chapters (put them last)
            int aChapter = a.Chapter?.OrderIndex ?? int.MaxValue;
            int bChapter = b.Chapter?.OrderIndex ?? int.MaxValue;

            if (aChapter != bChapter)
                return aChapter.CompareTo(bChapter);

            // If in same chapter, sort by order in chapter
            return a.OrderInChapter.CompareTo(b.OrderInChapter);
        };

        // 2. Extract PlotPoints from the Appearances junction table
        var relevantPlotPoints = character.Appearances.Select(x => x.PlotPoint);

        // 3. Initialize Collection (View Only - No Reorder Strategy)
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel();
        PlotPointCollectionViewModel.SetAndSortItems(relevantPlotPoints, standardSorter);

        // 4. (Optional) Listen for new links to update the list live
        character.Appearances.CollectionChanged += (s, e) => 
        {
            var updatedPoints = _character.Appearances.Select(x => x.PlotPoint);
            PlotPointCollectionViewModel.SetAndSortItems(updatedPoints, standardSorter);
        };
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
}