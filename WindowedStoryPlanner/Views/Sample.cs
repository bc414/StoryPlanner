using System.Collections.ObjectModel;
using StoryPlanner.Core.Models;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views;

public static class Sample
{
    public static MainViewModel MainViewModel
    {
        get
        {
            var mockService = new DesignTimeStoryService();
            return new MainViewModel(mockService);
        }
    }

    public static CharacterViewModel CharacterViewModel
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return new CharacterViewModel(data.Characters.First());
        }
    }

    public static ObservableCollection<CharacterViewModel> CharacterViewModelCollection
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            var collection = new ObservableCollection<CharacterViewModel>();
            
            foreach (var character in data.Characters)
            {
                collection.Add(new CharacterViewModel(character));
            }
            return collection;
        }
    }

    public static Note Note
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return data.Notes.First();
        }
    }

    public static NoteCollectionViewModel NoteCollectionViewModel => 
        new(new ObservableCollection<Note>(DesignDataFactory.CreateWorld().Notes));

    public static PlotPointViewModel PlotPointViewModel
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return new PlotPointViewModel(data.PlotPoints.First());
        }
    }
}