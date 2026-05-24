using CommunityToolkit.Mvvm.ComponentModel;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace WindowedStoryPlanner.ViewModels;

public partial class NarrativeElementViewModel : ObservableObject, IDropTarget, IEditorModeAware
{
    protected readonly IStoryService _storyService;
    protected readonly IViewModelRegistry _viewModelRegistry;
    protected readonly IContentFactory _editorCoordinator;

    public ObservableCollection<NoteTrackViewModel> NoteTracks { get; set; } = new();
    public ObservableCollection<NarrativePropertyViewModel> NarrativeProperties { get; set; } = new();

    private int _openWindowCount = 0;

    private Func<List<NoteTrackDefinition>>? _noteTrackFactory;
    private Func<List<NarrativePropertyDefinition>>? _propertyFactory;
    private int _ownerId;
    private OwnerType _ownerType;

    public NarrativeElementViewModel(
        IViewModelRegistry viewModelRegistry,
        IStoryService storyService,
        IContentFactory editorCoordinator,
        AppSettings appSettings)
    {
        _storyService      = storyService;
        _viewModelRegistry = viewModelRegistry;
        _editorCoordinator = editorCoordinator;
        AppSettings        = appSettings;
    }

    protected void InitializeCollections(
        int ownerId,
        OwnerType ownerType,
        Func<List<NoteTrackDefinition>> noteTrackFactory,
        Func<List<NarrativePropertyDefinition>> propertyFactory)
    {
        _ownerId          = ownerId;
        _ownerType        = ownerType;
        _noteTrackFactory = noteTrackFactory;
        _propertyFactory  = propertyFactory;

        RebuildNoteTracks();
    }

    private void RebuildNoteTracks()
    {
        if (_noteTrackFactory is null || _propertyFactory is null) return;

        NoteTracks.Clear();
        NarrativeProperties.Clear();

        foreach (var ntd in _noteTrackFactory())
            NoteTracks.Add(new NoteTrackViewModel(
                ntd, _ownerId, _ownerType,
                _viewModelRegistry, _storyService, _editorCoordinator, AppSettings));

        // Always add an unassigned track at the end for notes with no track set
        NoteTracks.Add(new NoteTrackViewModel(
            UnassignedTrack.Definition, _ownerId, _ownerType,
            _viewModelRegistry, _storyService, _editorCoordinator, AppSettings));

        foreach (var npd in _propertyFactory())
            NarrativeProperties.Add(new NarrativePropertyViewModel(
                _ownerId, _ownerType, npd, _viewModelRegistry, _storyService));

        var first = NoteTracks.OrderBy(t => t.DisplayOrder).FirstOrDefault();
        if (first is not null)
            first.IsFirstTrack = true;
    }

    public void OnWindowOpened()
    {
        _openWindowCount++;
        if (_openWindowCount > 1) return;

        RebuildNoteTracks();

        foreach (var track in NoteTracks)
            track.Initialize();
    }

    public void OnWindowClosed()
    {
        _openWindowCount = Math.Max(0, _openWindowCount - 1);
        if (_openWindowCount > 0) return;

        foreach (var track in NoteTracks)
            track.Uninitialize();
    }

    // ── IEditorModeAware ──────────────────────────────────────────────────

    /// <summary>
    /// Pushes <paramref name="mode"/> to every track so they update their
    /// <see cref="NoteTrackViewModel.HeaderText"/>, <see cref="NoteTrackViewModel.IsReadOnly"/>,
    /// and <see cref="NoteTrackViewModel.CanPromoteToConfirmed"/> in one call.
    /// </summary>
    public void SetTrackDisplayMode(TrackDisplayMode mode)
    {
        foreach (var track in NoteTracks)
            track.TrackDisplayMode = mode;
    }

    // ── GongSolutions IDropTarget (PlotPoint ↔ Subject linking) ──────────

    public virtual void DragOver(IDropInfo dropInfo)
    {
        // Let NoteViewModel drags pass through to NoteTrackSectionView's drop handler
        if (dropInfo.Data is NoteViewModel)
        {
            dropInfo.Effects = DragDropEffects.None;
            return;
        }

        bool canDrop = (dropInfo.Data, this) switch
        {
            (SubjectViewModel,              PlotPointViewModel) => true,
            (PlotPointViewModel,            SubjectViewModel)   => true,
            (PlotPointSubjectLinkViewModel, PlotPointViewModel) => true,
            (PlotPointSubjectLinkViewModel, SubjectViewModel)   => true,
            _ => false
        };

        if (canDrop)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
        }
    }

    public virtual async void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is NoteViewModel) return;

        var source = dropInfo.Data;
        var target = this;

        switch (source, target)
        {
            case (SubjectViewModel s, PlotPointViewModel p):
                await _editorCoordinator.CreatePlotPointSubjectLinkAsync(p, s); break;
            case (PlotPointViewModel p, SubjectViewModel s):
                await _editorCoordinator.CreatePlotPointSubjectLinkAsync(p, s); break;
            case (PlotPointSubjectLinkViewModel l, PlotPointViewModel p):
                l.PlotPointId = p.Id;
                _viewModelRegistry.RaiseLinksInvalidated();
                break;
            case (PlotPointSubjectLinkViewModel l, SubjectViewModel s):
                l.SubjectId = s.Id;
                _viewModelRegistry.RaiseLinksInvalidated();
                break;
        }
    }

    public AppSettings AppSettings { get; }
}
