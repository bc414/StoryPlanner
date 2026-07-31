using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WindowedStoryPlanner;

/// <summary>
/// Base for cross-cutting "show me every note tagged with X" view models (Theme, SourceMaterial,
/// SourceMaterialPart, world-date range). Unlike NoteTrackSectionViewModel, this filters across
/// every owner/track in the project on some property of the note rather than on
/// (OwnerId, OwnerType). Subclasses supply the criterion; the view is CrossCutNoteListView.
///
/// LIVE, not snapshot (deliberate divergence from TimelineViewModel): it subscribes to every
/// NoteViewModel so a note joins or leaves the list the moment its tag changes.
/// </summary>
public abstract class TaggedNotesViewModelBase : ObservableObject, IDisposable
{
    protected readonly IViewModelRegistry _registry;
    protected readonly IWindowManager _windowManager;

    private readonly ObservableCollection<NoteViewModel> _notes = new();
    public ReadOnlyObservableCollection<NoteViewModel> Notes { get; }

    // What we actually hooked. Dispose and Reset both work off this rather than off the
    // registry, which by then may no longer contain the notes we subscribed to.
    private readonly HashSet<NoteViewModel> _subscribed = new();

    protected TaggedNotesViewModelBase(IViewModelRegistry registry, IWindowManager windowManager)
    {
        _registry = registry;
        _windowManager = windowManager;
        Notes = new ReadOnlyObservableCollection<NoteViewModel>(_notes);

        registry.AllNoteViewModels.CollectionChanged += OnAllNotesCollectionChanged;
        Reseed();
    }

    /// <summary>
    /// Opens the entity a row's note actually lives on — the way out of a cross-cut list and back
    /// into the note's own context, where its siblings on the same track are. Bound to the
    /// breadcrumb, which is already the thing naming where the note is.
    /// </summary>
    public IRelayCommand<NoteViewModel> OpenOwnerCommand => _openOwnerCommand ??=
        new RelayCommand<NoteViewModel>(note =>
        {
            if (note is not null) OwnerNavigator.Open(note, _registry, _windowManager);
        });

    private IRelayCommand<NoteViewModel>? _openOwnerCommand;

    /// <summary>Whether this note currently carries the tag this view model is filtering on.</summary>
    protected abstract bool Matches(NoteViewModel note);

    /// <summary>
    /// Whether a change to this NoteViewModel property can flip membership. A null or empty name
    /// is WPF's "everything changed" and must be treated as a yes.
    /// </summary>
    protected abstract bool AffectsMembership(string? propertyName);

    /// <summary>
    /// What CrossCutNoteListView binds. Defaults to <see cref="Notes"/>; a subclass whose surface
    /// has an inherent order (chronology) overrides it with a sorted view over the same collection.
    /// </summary>
    public virtual IEnumerable NotesSource => Notes;

    public string Breadcrumb(NoteViewModel note) =>
        $"{OwnerBreadcrumbResolver.Resolve(note.OwnerId, note.OwnerType, _registry)} · " +
        $"{note.NoteTrackDefinition?.TrackName ?? "Unassigned"}";

    /// <summary>
    /// Re-test every note. Needed when the criterion itself changes — the date range is the first
    /// one the user can edit while the view is open.
    /// </summary>
    protected void Reevaluate()
    {
        foreach (var n in _subscribed)
        {
            bool shouldBeIn = Matches(n);
            bool isIn = _notes.Contains(n);
            if (shouldBeIn && !isIn) _notes.Add(n);
            else if (!shouldBeIn && isIn) _notes.Remove(n);
        }
    }

    private void Reseed()
    {
        foreach (var n in _subscribed) n.PropertyChanged -= OnNotePropertyChanged;
        _subscribed.Clear();
        _notes.Clear();

        foreach (var n in _registry.AllNoteViewModels)
        {
            Subscribe(n);
            if (Matches(n)) _notes.Add(n);
        }
    }

    private void Subscribe(NoteViewModel note)
    {
        if (_subscribed.Add(note)) note.PropertyChanged += OnNotePropertyChanged;
    }

    private void OnAllNotesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Reset carries neither NewItems nor OldItems. ViewModelRegistry.Clear() raises it on
        // every project load, so without this an open cross-cut window would keep the previous
        // file's rows and then append the new file's — two corpora blended in one list.
        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            Reseed();
            return;
        }

        if (e.NewItems is not null)
            foreach (NoteViewModel n in e.NewItems)
            {
                Subscribe(n);
                if (Matches(n)) _notes.Add(n);
            }

        if (e.OldItems is not null)
            foreach (NoteViewModel n in e.OldItems)
            {
                if (_subscribed.Remove(n)) n.PropertyChanged -= OnNotePropertyChanged;
                _notes.Remove(n);
            }
    }

    private void OnNotePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!AffectsMembership(e.PropertyName)) return;
        var note = (NoteViewModel)sender!;

        bool shouldBeIn = Matches(note);
        bool isIn = _notes.Contains(note);

        if (shouldBeIn && !isIn) _notes.Add(note);
        else if (!shouldBeIn && isIn) _notes.Remove(note);
    }

    public virtual void Dispose()
    {
        _registry.AllNoteViewModels.CollectionChanged -= OnAllNotesCollectionChanged;
        foreach (var n in _subscribed) n.PropertyChanged -= OnNotePropertyChanged;
        _subscribed.Clear();
    }
}
