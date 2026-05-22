using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public class SubjectGroupViewModel
{
    public string GroupLabel        { get; }
    public int    DisplayOrder      { get; }
    public int    SubjectDefinitionId { get; }
    public ICollectionView Subjects { get; }

    public SubjectGroupViewModel(
        SubjectDefinition definition,
        ObservableCollection<SubjectViewModel> allSubjects)
    {
        GroupLabel          = definition.SubjectType;
        DisplayOrder        = definition.DisplayOrder;
        SubjectDefinitionId = definition.Id;

        var cvs = new CollectionViewSource { Source = allSubjects };
        cvs.IsLiveFilteringRequested = true;
        cvs.LiveFilteringProperties.Add(nameof(SubjectViewModel.SubjectDefinitionId));
        
        Subjects = cvs.View;
        Subjects.Filter = obj =>
            obj is SubjectViewModel s && s.SubjectDefinitionId == definition.Id;
        Subjects.SortDescriptions.Add(
            new SortDescription(nameof(SubjectViewModel.Name), ListSortDirection.Ascending));
    }
}