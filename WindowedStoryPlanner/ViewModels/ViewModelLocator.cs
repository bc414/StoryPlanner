using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// DataContext for MainWindow. Pure property bag — no logic, no ObservableObject needed.
/// Each property is a tab ViewModel resolved from DI.
/// </summary>
public class ViewModelLocator
{
    public FileManagerViewModel       FileManager  { get; }
    public ChapterLibraryViewModel    Chapters     { get; }
    public DefinitionsEditorViewModel Definitions  { get; }
    public SubjectLibraryViewModel    Subjects     { get; }
    public ThemeLibraryViewModel      Themes       { get; }

    public ViewModelLocator(
        FileManagerViewModel       fileManager,
        ChapterLibraryViewModel    chapters,
        DefinitionsEditorViewModel definitions,
        SubjectLibraryViewModel    subjects,
        ThemeLibraryViewModel      themes)
    {
        FileManager = fileManager;
        Chapters    = chapters;
        Definitions = definitions;
        Subjects    = subjects;
        Themes      = themes;
    }
}
