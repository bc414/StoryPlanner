using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;

namespace WindowedStoryPlanner.ViewModels;

public partial class EntityViewModel : ObservableObject, IDropTarget
{
    public NoteCollectionViewModel NoteCollectionViewModel { get; set; }
    
    [RelayCommand]
    public void OpenWindow()
    {
        MainViewModel.Instance.OpenEditorWindow(this);
    }
    
    public virtual void DragOver(IDropInfo dropInfo)
    {
        throw new NotImplementedException();
    }

    public virtual void Drop(IDropInfo dropInfo)
    {
        throw new NotImplementedException();
    }
}