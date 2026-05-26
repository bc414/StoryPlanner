using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using StoryPlanner.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WindowedStoryPlanner.ViewModels;

public partial class SubjectGroupViewModel : ObservableObject
{
    public string GroupLabel          { get; }
    public int    DisplayOrder        { get; }
    public int    SubjectDefinitionId { get; }

    private readonly ObservableCollection<SubjectViewModel> _allSubjects;
    private readonly AppSettings                            _appSettings;

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
            vm.PropertyChanged += OnSubjectPropertyChanged;

        _allSubjects.CollectionChanged += OnSourceCollectionChanged;
        _appSettings.PropertyChanged   += OnAppSettingsPropertyChanged;
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

    // ── Change listeners ──────────────────────────────────────────────────

    private void OnAppSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppSettings.IsArchiveMode))
            InvalidateSubjects();
    }

    private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
            foreach (SubjectViewModel vm in e.OldItems)
                vm.PropertyChanged -= OnSubjectPropertyChanged;

        if (e.NewItems is not null)
            foreach (SubjectViewModel vm in e.NewItems)
                vm.PropertyChanged += OnSubjectPropertyChanged;

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