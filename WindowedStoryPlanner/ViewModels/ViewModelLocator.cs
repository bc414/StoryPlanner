using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// DataContext for MainWindow. Pure property bag — no logic, no ObservableObject needed.
/// Each property is a tab ViewModel resolved from DI.
/// </summary>
public partial class ViewModelLocator : ObservableObject
{
    public FileManagerViewModel              FileManager        { get; }
    public ChapterLibraryViewModel           Chapters           { get; }
    public DefinitionsEditorViewModel        Definitions        { get; }
    public SubjectLibraryViewModel           Subjects           { get; }
    public ThemeLibraryViewModel             Themes             { get; }
    public SourceMaterialLibraryViewModel    SourceMaterials    { get; }
    public ConversationLibraryViewModel      ConversationLibrary { get; }
    public FloatingPlotPointsViewModel       FloatingPlotPoints { get; }
    public ExportViewModel                   Export             { get; }

    [ObservableProperty]
    private int _selectedTabIndex;

    public ViewModelLocator(
        FileManagerViewModel           fileManager,
        ChapterLibraryViewModel        chapters,
        DefinitionsEditorViewModel     definitions,
        SubjectLibraryViewModel        subjects,
        ThemeLibraryViewModel          themes,
        SourceMaterialLibraryViewModel sourceMaterials,
        ConversationLibraryViewModel   conversationLibrary,
        FloatingPlotPointsViewModel    floatingPlotPoints,
        ExportViewModel                export)
    {
        FileManager         = fileManager;
        Chapters            = chapters;
        Definitions         = definitions;
        Subjects            = subjects;
        Themes              = themes;
        SourceMaterials     = sourceMaterials;
        ConversationLibrary = conversationLibrary;
        FloatingPlotPoints  = floatingPlotPoints;
        Export              = export;
    }
}
