using System.Collections;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;

namespace WindowedStoryPlanner.ViewModels;

public partial class EntityViewModel : ObservableObject, IDropTarget
{
    public NoteCollectionViewModel NoteCollectionViewModel { get; set; }
    public PlotPointCollectionViewModel PlotPointCollectionViewModel { get; set; }

    public virtual bool IsLinkingMode => !NoteCollectionViewModel.IsNoteReorderMode;

    public EntityViewModel()
    {
        
    }
    
    [RelayCommand]
    public void OpenWindow()
    {
        MainViewModel.Instance.OpenEditorWindow(this);
    }
    
    public virtual void DragOver(IDropInfo dropInfo)
    {
        
        object source = dropInfo.Data;

        var target = this;
        
        // 2. IDENTIFY ROLES
        // We need to know which one is the PlotPoint (the container) and which is the "Other" (the item).
        PlotPointViewModel? plotPoint = source as PlotPointViewModel ?? target as PlotPointViewModel;
        EntityViewModel? otherEntity = (source == plotPoint) ? target as EntityViewModel : source as EntityViewModel;

        // 3. VALIDATE TYPE COMPATIBILITY
        bool isTypeCompatible = (source, target) switch
        {
            (CharacterViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, CharacterViewModel) => true,
            (ThemeViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, ThemeViewModel) => true,
            (StoryThreadViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, StoryThreadViewModel) => true,
            (CodexEntryViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, CodexEntryViewModel) => true,
            (ChapterViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, ChapterViewModel) => true,
            (LocationViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, LocationViewModel) => true,
            _ => false
        };

        // 4. CHECK LOGIC: "Compatible Type" AND "Not Already Linked"
        if (isTypeCompatible && plotPoint != null && otherEntity != null)
        {
            // The Performace Check: This is fast (local in-memory check)
            if (!plotPoint.IsLinkedTo(otherEntity))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move; // Allowed
            }
            else
            {
                // Explicitly deny if already linked (optional, as default is None, but good for clarity)
                dropInfo.Effects = DragDropEffects.None; 
            }
        }
        
    }

    public virtual void Drop(IDropInfo dropInfo)
    {
        var source = dropInfo.Data;
        var target = this;

        switch (source, target)
        {
            // --- 1. CHARACTER ---
            case (CharacterViewModel c, PlotPointViewModel p):
                p.LinkCharacter(c);
                break;
            case (PlotPointViewModel p, CharacterViewModel c):
                p.LinkCharacter(c);
                break;

            // --- 2. THEME ---
            case (ThemeViewModel t, PlotPointViewModel p):
                p.LinkTheme(t);
                break;
            case (PlotPointViewModel p, ThemeViewModel t):
                p.LinkTheme(t);
                break;

            // --- 3. STORY THREAD ---
            case (StoryThreadViewModel s, PlotPointViewModel p):
                p.LinkThread(s);
                break;
            case (PlotPointViewModel p, StoryThreadViewModel s):
                p.LinkThread(s);
                break;

            // --- 4. CODEX ENTRY ---
            case (CodexEntryViewModel e, PlotPointViewModel p):
                p.LinkCodexEntry(e);
                break;
            case (PlotPointViewModel p, CodexEntryViewModel e):
                p.LinkCodexEntry(e);
                break;

            // --- 5. CHAPTER ---
            case (ChapterViewModel ch, PlotPointViewModel p):
                 p.LinkChapter(ch);
                break;
            case (PlotPointViewModel p, ChapterViewModel ch):
                 p.LinkChapter(ch);
                break;
            
            // --- 6. LOCATION ---
            case (LocationViewModel l, PlotPointViewModel p):
                p.LinkLocation(l);
                break;
            case (PlotPointViewModel p, LocationViewModel l):
                p.LinkLocation(l);
                break;
        }
    }
}