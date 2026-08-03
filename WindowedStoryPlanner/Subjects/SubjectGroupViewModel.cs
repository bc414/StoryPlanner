using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using StoryPlanner.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WindowedStoryPlanner;

public partial class SubjectGroupViewModel : ObservableObject, IDisposable
{
    public string GroupLabel          { get; }
    public int    DisplayOrder        { get; }
    public int    SubjectDefinitionId { get; }

    private readonly ObservableCollection<SubjectViewModel> _allSubjects;
    private readonly AppSettings                            _appSettings;

    // What we actually subscribed to — Dispose and the Reset case work off this set, never off
    // the live collection (which no longer holds those items by then). Same discipline as
    // TaggedNotesViewModelBase.
    private readonly HashSet<SubjectViewModel> _subscribed = new();

    /// <summary>
    /// Subjects filtered to this group and sorted by the current mode:
    ///   normal       → alphabetical by Name
    ///   archive mode → descending UnconfirmedNoteCount (most outstanding work first)
    /// Recomputed whenever the source collection, archive mode, a subject's
    /// UnconfirmedNoteCount, or a subject's SubjectDefinitionId changes.
    /// </summary>
    public IReadOnlyList<SubjectViewModel> Subjects => BuildSubjects();

    public SubjectGroupViewModel(
        SubjectDefinition                      definition,
        ObservableCollection<SubjectViewModel> allSubjects,
        AppSettings                            appSettings)
    {
        GroupLabel          = definition.SubjectType;
        DisplayOrder        = definition.DisplayOrder;
        SubjectDefinitionId = definition.Id;
        _allSubjects        = allSubjects;
        _appSettings        = appSettings;

        // FIX 1: subscribe to subjects that already exist at construction time
        foreach (var vm in _allSubjects)
            Subscribe(vm);

        _allSubjects.CollectionChanged += OnSourceCollectionChanged;
        _appSettings.PropertyChanged   += OnAppSettingsPropertyChanged;
    }

    /// <summary>
    /// SubjectLibraryViewModel calls this on every group it replaces (RebuildGroups runs on each
    /// definition change and project load). Without it every discarded group stayed subscribed to
    /// the app-lifetime collection and re-filtered itself forever.
    /// </summary>
    public void Dispose()
    {
        foreach (var vm in _subscribed)
            vm.PropertyChanged -= OnSubjectPropertyChanged;
        _subscribed.Clear();

        _allSubjects.CollectionChanged -= OnSourceCollectionChanged;
        _appSettings.PropertyChanged   -= OnAppSettingsPropertyChanged;
    }

    // ── Sorting / filtering ───────────────────────────────────────────────

    private IReadOnlyList<SubjectViewModel> BuildSubjects()
    {
        var filtered = _allSubjects.Where(s => s.SubjectDefinitionId == SubjectDefinitionId);

        return (_appSettings.IsArchiveMode
            ? filtered.OrderBy(s => s.UnconfirmedNoteCount).ThenBy(s => s.Name)
            : filtered.OrderBy(s => s.Name))
            .ToList();
    }

    private void InvalidateSubjects() => OnPropertyChanged(nameof(Subjects));

    private void Subscribe(SubjectViewModel vm)
    {
        if (_subscribed.Add(vm))
            vm.PropertyChanged += OnSubjectPropertyChanged;
    }

    private void Unsubscribe(SubjectViewModel vm)
    {
        if (_subscribed.Remove(vm))
            vm.PropertyChanged -= OnSubjectPropertyChanged;
    }

    // ── Change listeners ──────────────────────────────────────────────────

    private void OnAppSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppSettings.IsArchiveMode))
            InvalidateSubjects();
    }

    private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            // Registry Clear() raises Reset with neither OldItems nor NewItems — re-seed
            // from the subscribed set, then from whatever the collection now holds.
            foreach (var vm in _subscribed.ToList())
                Unsubscribe(vm);
            foreach (var vm in _allSubjects)
                Subscribe(vm);
            InvalidateSubjects();
            return;
        }

        if (e.OldItems is not null)
            foreach (SubjectViewModel vm in e.OldItems)
                Unsubscribe(vm);

        if (e.NewItems is not null)
            foreach (SubjectViewModel vm in e.NewItems)
                Subscribe(vm);

        InvalidateSubjects();
    }

    private void OnSubjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            // FIX 2: subject moved to a different definition → re-filter this group
            case nameof(SubjectViewModel.SubjectDefinitionId):
            // Re-sort whenever the archive-mode sort key changes
            case nameof(SubjectViewModel.UnconfirmedNoteCount):
            // Re-sort in normal mode
            case nameof(SubjectViewModel.Name):
                InvalidateSubjects();
                break;
        }
    }
}
