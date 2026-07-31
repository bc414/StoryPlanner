using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WindowedStoryPlanner;

/// <summary>
/// Base for cross-cutting "show me every note tagged with X" view models
/// (Theme, SourceMaterial). Unlike NoteTrackSectionViewModel, this filters
/// across every owner/track in the project on a tag property (e.g.
/// SelectedTheme/SelectedSourceMaterial) rather than on (OwnerId, OwnerType).
/// </summary>
public abstract class TaggedNotesViewModelBase : ObservableObject, IDisposable
{
    protected readonly IViewModelRegistry _registry;

    private readonly ObservableCollection<NoteViewModel> _notes = new();
    public ReadOnlyObservableCollection<NoteViewModel> Notes { get; }

    protected TaggedNotesViewModelBase(IViewModelRegistry registry)
    {
        _registry = registry;
        Notes = new ReadOnlyObservableCollection<NoteViewModel>(_notes);

        foreach (var n in registry.AllNoteViewModels.Where(Matches))
            _notes.Add(n);

        registry.AllNoteViewModels.CollectionChanged += OnAllNotesCollectionChanged;
        foreach (var n in registry.AllNoteViewModels)
            n.PropertyChanged += OnNotePropertyChanged;
    }

    /// <summary>Whether this note currently carries the tag this view model is filtering on.</summary>
    protected abstract bool Matches(NoteViewModel note);

    /// <summary>The NoteViewModel property whose change should re-evaluate membership.</summary>
    protected abstract string TagPropertyName { get; }

    public string Breadcrumb(NoteViewModel note) =>
        OwnerBreadcrumbResolver.Resolve(note.OwnerId, note.OwnerType, _registry);

    private void OnAllNotesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is not null)
            foreach (NoteViewModel n in e.NewItems)
            {
                n.PropertyChanged += OnNotePropertyChanged;
                if (Matches(n)) _notes.Add(n);
            }

        if (e.OldItems is not null)
            foreach (NoteViewModel n in e.OldItems)
            {
                n.PropertyChanged -= OnNotePropertyChanged;
                _notes.Remove(n);
            }
    }

    private void OnNotePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != TagPropertyName) return;
        var note = (NoteViewModel)sender!;

        bool shouldBeIn = Matches(note);
        bool isIn = _notes.Contains(note);

        if (shouldBeIn && !isIn) _notes.Add(note);
        else if (!shouldBeIn && isIn) _notes.Remove(note);
    }

    public void Dispose()
    {
        _registry.AllNoteViewModels.CollectionChanged -= OnAllNotesCollectionChanged;
        foreach (var n in _registry.AllNoteViewModels)
            n.PropertyChanged -= OnNotePropertyChanged;
    }
}
