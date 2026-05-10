using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteTrackViewModel : ObservableObject
{
    private readonly int _ownerId;
    private readonly OwnerType _ownerType;
    private readonly NoteTrackDefinition _definition;
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _viewModelRegistry;
    private readonly IContentFactory _editorCoordinator;

    public ObservableCollection<NoteTrackSectionViewModel> Sections { get; } = new();

    public int DisplayOrder => _definition.DisplayOrder;
    public string DisplayQuestion => _definition.DisplayQuestion;
    public string TrackName => _definition.TrackName;
    public string Explanation => _definition.Explanation;
    public CognitiveMode CognitiveMode => _definition.CognitiveMode;
    public NoteTrackDefinition Definition => _definition;

    [ObservableProperty]
    private int? _assignedFunctionKey;

    public int OwnerId       => _ownerId;
    public OwnerType OwnerType => _ownerType;

    public NoteTrackViewModel(
        NoteTrackDefinition definition,
        int ownerId,
        OwnerType ownerType,
        IViewModelRegistry registry,
        IStoryService storyService,
        IContentFactory editorCoordinator)
    {
        _definition        = definition;
        _ownerId           = ownerId;
        _ownerType         = ownerType;
        _storyService      = storyService;
        _viewModelRegistry = registry;
        _editorCoordinator = editorCoordinator;
    }

    public void Initialize()
    {
        if (Sections.Count > 0) return;

        Sections.Add(new NoteTrackSectionViewModel(
            _ownerId, _ownerType, _definition, NoteState.Confirmed,
            _viewModelRegistry, _storyService));

        Sections.Add(new NoteTrackSectionViewModel(
            _ownerId, _ownerType, _definition, NoteState.Unset,
            _viewModelRegistry, _storyService));

        Sections.Add(new NoteTrackSectionViewModel(
            _ownerId, _ownerType, _definition, NoteState.Flagged,
            _viewModelRegistry, _storyService));
    }

    public void Uninitialize()
    {
        foreach (var section in Sections)
            section.Dispose();
        Sections.Clear();
    }

    [RelayCommand]
    public async Task CreateNewNote()
    {
        var unsetSection = Sections.FirstOrDefault(s => s.TargetState == NoteState.Unset);

        int maxOrder = unsetSection?.SectionNotes
            .Cast<NoteViewModel>()
            .Select(n => n.SortOrder)
            .DefaultIfEmpty(0)
            .Max() ?? 0;

        int? trackId = _definition.Id == UnassignedTrack.Definition.Id ? null : _definition.Id;

        await _editorCoordinator.CreateNoteAsync(_ownerId, _ownerType, trackId, maxOrder + 10);
    }
}
