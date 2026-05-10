using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// One Expander section in the Subject Library — subjects of a single SubjectDefinition.
/// </summary>
public class SubjectGroupViewModel
{
    public string GroupLabel        { get; }
    public int    DisplayOrder      { get; }
    public int    SubjectDefinitionId { get; }  // stored directly — avoids string lookup in AddSubject
    public ICollectionView Subjects { get; }

    public SubjectGroupViewModel(
        SubjectDefinition definition,
        ObservableCollection<SubjectViewModel> allSubjects)
    {
        GroupLabel          = definition.SubjectType;
        DisplayOrder        = definition.DisplayOrder;
        SubjectDefinitionId = definition.Id;

        var cvs = new CollectionViewSource { Source = allSubjects };
        Subjects = cvs.View;
        Subjects.Filter = obj =>
            obj is SubjectViewModel s && s.SubjectDefinitionId == definition.Id;
        Subjects.SortDescriptions.Add(
            new SortDescription(nameof(SubjectViewModel.Name), ListSortDirection.Ascending));
    }
}