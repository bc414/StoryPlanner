using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowedStoryPlanner.Views;

public partial class NoteViewer : UserControl
{
    // The Data Source
    public static readonly DependencyProperty NotesSourceProperty =
        DependencyProperty.Register("NotesSource", typeof(IEnumerable), typeof(NoteViewer));

    public IEnumerable NotesSource
    {
        get { return (IEnumerable)GetValue(NotesSourceProperty); }
        set { SetValue(NotesSourceProperty, value); }
    }

    // Optional: Command to add a new note
    public static readonly DependencyProperty AddNoteCommandProperty =
        DependencyProperty.Register("AddNoteCommand", typeof(ICommand), typeof(NoteViewer));

    public ICommand AddNoteCommand
    {
        get { return (ICommand)GetValue(AddNoteCommandProperty); }
        set { SetValue(AddNoteCommandProperty, value); }
    }

    public NoteViewer()
    {
        InitializeComponent();
        
        // This is important for Gong DragDrop later!
        // It allows the ListBox to define its own DataContext root for the UserControl bindings
        this.DataContext = this; 
    }
}