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
            var m = new MainViewModel(mockService);
            m.UpdateState();
            return m;
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

    public static ChapterViewModel ChapterViewModel
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return new ChapterViewModel(data.Chapters.First());
        }
    }

    public static StoryThreadViewModel StoryThreadViewModel
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return new StoryThreadViewModel(data.StoryThreads.First());
        }
    }

    public static CodexEntryViewModel CodexEntryViewModel
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return new CodexEntryViewModel(data.CodexEntries.First());
        }
    }

    public static ThemeViewModel ThemeViewModel
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return new ThemeViewModel(data.Themes.First());
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

    public static PlotPointViewModelOld PlotPointViewModel
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return new PlotPointViewModel(data.PlotPoints.First());
        }
    }

    public static PlotPointCollectionViewModel PlotPointCollectionViewModel
    {
        get
        {
            List<PlotPoint> plotPoints = DesignDataFactory.CreateWorld().PlotPoints;
            ObservableCollection<PlotPointViewModelOld> vms = new ObservableCollection<PlotPointViewModelOld>();
            foreach(var plotPoint in plotPoints)
            {
                vms.Add(new PlotPointViewModel(plotPoint));
            }
            var pp = new PlotPointCollectionViewModel();
            pp.ViewModelCollection = vms;
            return pp;
        }
    }

    public static PlotPointCharacter PlotPointCharacter
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return data.PlotPoints.First().CharacterAppearances.First();
        }
    }

    public static PlotPointCodexEntry PlotPointCodexEntry
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return data.PlotPoints.First(p => p.Id == 2).CodexReferences.First();
        }
    }

    public static PlotPointTheme PlotPointTheme
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return data.PlotPoints.First(p => p.Id == 6).ThemeAssignments.First();
        }
    }

    public static PlotPointStoryThread PlotPointThread
    {
        get
        {
            var data = DesignDataFactory.CreateWorld();
            return data.PlotPoints.First(p => p.Id == 6).ThreadAssignments.First();
        }
    }

    public static Idea Idea
    {
        get
        {
            return new Idea()
            {
                Text = "Sample Text",
                Id = 1,
                State = IdeaState.Written,
            };
        }
    }
}