using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WindowedStoryPlanner;

/// <summary>
/// Backing view model for the POV-characters manager window: a dedicated picker+list for
/// Subject.IsPovCharacter — the pool PlotPointViewModel.FocalCharacterChoices draws from —
/// rather than an inline checkbox on every subject widget. A real file's Character subjects run
/// in the hundreds, so the flag needed a place of its own instead of adding noise to every
/// subject's editor.
///
/// LIVE, mirroring TaggedNotesViewModelBase's subscribe/reseed shape but over
/// AllSubjectViewModels instead of AllNoteViewModels: a subject joins or leaves the list the
/// moment IsPovCharacter changes, from any source.
/// </summary>
public partial class PovCharactersViewModel : ObservableObject, IDisposable
{
    private readonly IViewModelRegistry _registry;
    public IViewModelRegistry Registry => _registry;

    private readonly ObservableCollection<SubjectViewModel> _povCharacters = new();
    public ReadOnlyObservableCollection<SubjectViewModel> PovCharacters { get; }

    // What we actually hooked. Dispose and Reseed both work off this rather than off the
    // registry, which by then may no longer contain the subjects we subscribed to.
    private readonly HashSet<SubjectViewModel> _subscribed = new();

    public PovCharactersViewModel(IViewModelRegistry registry)
    {
        _registry = registry;
        PovCharacters = new ReadOnlyObservableCollection<SubjectViewModel>(_povCharacters);

        registry.AllSubjectViewModels.CollectionChanged += OnAllSubjectsCollectionChanged;
        Reseed();
    }

    /// <summary>Sets IsPovCharacter — the subject picker's SubjectSelected handler calls this.
    /// Idempotent: picking an already-flagged subject is a no-op (SubjectViewModel's setter
    /// short-circuits on no change).</summary>
    [RelayCommand]
    private void Add(SubjectViewModel? subject)
    {
        if (subject is not null) subject.IsPovCharacter = true;
    }

    /// <summary>Clears IsPovCharacter — does not touch any PlotPoint.FocalCharacterId already
    /// pointing at this subject; those are a separate, deliberately-authorial assignment.</summary>
    [RelayCommand]
    private void Remove(SubjectViewModel? subject)
    {
        if (subject is not null) subject.IsPovCharacter = false;
    }

    private void Reseed()
    {
        foreach (var s in _subscribed) s.PropertyChanged -= OnSubjectPropertyChanged;
        _subscribed.Clear();
        _povCharacters.Clear();

        foreach (var s in _registry.AllSubjectViewModels)
        {
            Subscribe(s);
            if (s.IsPovCharacter) _povCharacters.Add(s);
        }
    }

    private void Subscribe(SubjectViewModel subject)
    {
        if (_subscribed.Add(subject)) subject.PropertyChanged += OnSubjectPropertyChanged;
    }

    private void OnAllSubjectsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Reset carries neither NewItems nor OldItems. ViewModelRegistry.Clear() raises it on
        // every project load, so without this an open window would keep the previous file's
        // rows and then append the new file's — two corpora blended in one list.
        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            Reseed();
            return;
        }

        if (e.NewItems is not null)
            foreach (SubjectViewModel s in e.NewItems)
            {
                Subscribe(s);
                if (s.IsPovCharacter) _povCharacters.Add(s);
            }

        if (e.OldItems is not null)
            foreach (SubjectViewModel s in e.OldItems)
            {
                if (_subscribed.Remove(s)) s.PropertyChanged -= OnSubjectPropertyChanged;
                _povCharacters.Remove(s);
            }
    }

    private void OnSubjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not (nameof(SubjectViewModel.IsPovCharacter) or null or "")) return;
        var subject = (SubjectViewModel)sender!;

        bool shouldBeIn = subject.IsPovCharacter;
        bool isIn = _povCharacters.Contains(subject);
        if (shouldBeIn && !isIn) _povCharacters.Add(subject);
        else if (!shouldBeIn && isIn) _povCharacters.Remove(subject);
    }

    public void Dispose()
    {
        _registry.AllSubjectViewModels.CollectionChanged -= OnAllSubjectsCollectionChanged;
        foreach (var s in _subscribed) s.PropertyChanged -= OnSubjectPropertyChanged;
        _subscribed.Clear();
    }
}
