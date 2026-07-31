using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// Backs the "Global Search" tab (FEATURE-AUDIT.md B2) — searches entity names/titles and
/// note content/flag-reason across the whole loaded plan. Matching itself lives in
/// StoryPlanner.Core.EntitySearch (Pure-tier tested); this VM's job is purely relationship
/// work — resolving a hit's Id back to a display label and an "open" action via the existing
/// IViewModelRegistry / IWindowManager, the same way ExportViewModel's anchor picker does.
///
/// Deliberately includes flagged notes' content in full (unlike NoteExportRenderer and the
/// MCP server's Engine.Search) — this is the author reading their own data in their own app,
/// not an LLM-facing surface. See EntitySearchTests for the tests pinning that as intentional.
/// </summary>
public partial class GlobalSearchViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;
    private readonly IWindowManager _windowManager;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private SearchResultViewModel? _selectedResult;

    public ObservableCollection<SearchResultViewModel> Results { get; } = new();

    public static IReadOnlyList<string> TypeFilterOptions { get; } =
        new[] { "All", "Subject", "Plot Point", "Chapter", "Theme", "Source Material", "Note" };

    [ObservableProperty] private string _selectedTypeFilter = "All";

    private SearchHitKind? KindFilter => SelectedTypeFilter switch
    {
        "Subject"         => SearchHitKind.Subject,
        "Plot Point"      => SearchHitKind.PlotPoint,
        "Chapter"         => SearchHitKind.Chapter,
        "Theme"           => SearchHitKind.Theme,
        "Source Material" => SearchHitKind.SourceMaterial,
        "Note"            => SearchHitKind.Note,
        _                 => null
    };

    public string ResultSummary
    {
        get
        {
            if (Results.Count == 0) return "No results";

            // GroupBy preserves first-occurrence order, which matches EntitySearch.Run's
            // fixed kind pass order — no separate sort needed.
            var byType = Results
                .GroupBy(r => r.TypeLabel)
                .Select(g => $"{g.Count()} {g.Key.ToLowerInvariant()}{(g.Count() == 1 ? "" : "s")}");

            return $"{Results.Count} result{(Results.Count == 1 ? "" : "s")} — {string.Join(", ", byType)}";
        }
    }

    public bool HasResults => Results.Count > 0;

    public GlobalSearchViewModel(IStoryService storyService, IViewModelRegistry registry, IWindowManager windowManager)
    {
        _storyService = storyService;
        _registry = registry;
        _windowManager = windowManager;
    }

    partial void OnSearchTextChanged(string value) => RebuildResults();
    partial void OnSelectedTypeFilterChanged(string value) => RebuildResults();

    /// <summary>Called by ProjectLoader after a project is (re)loaded, so results and the query
    /// from a previously-open file don't linger into the newly opened one.</summary>
    public void Reload()
    {
        SearchText = string.Empty;
        RebuildResults();
    }

    private void RebuildResults()
    {
        Results.Clear();

        if (_storyService.IsProjectLoaded)
        {
            // Read collections at query time, never cache a reference — StoryService.LoadDataAsync
            // reassigns these ObservableCollection instances on every project load.
            var input = new SearchInput(
                _storyService.Subjects.ToList(),
                _storyService.PlotPoints.ToList(),
                _storyService.Chapters.ToList(),
                _storyService.Themes.ToList(),
                _storyService.SourceMaterials.ToList(),
                _storyService.SourceMaterialParts.ToList(),
                _storyService.Notes.ToList());

            foreach (var hit in EntitySearch.Run(input, SearchText, KindFilter))
                Results.Add(BuildResult(hit));
        }

        OnPropertyChanged(nameof(ResultSummary));
        OnPropertyChanged(nameof(HasResults));
    }

    private SearchResultViewModel BuildResult(SearchHit hit) => hit.Kind switch
    {
        SearchHitKind.Subject        => BuildSubjectResult(hit),
        SearchHitKind.PlotPoint      => BuildPlotPointResult(hit),
        SearchHitKind.Chapter        => BuildChapterResult(hit),
        SearchHitKind.Theme          => BuildThemeResult(hit),
        SearchHitKind.SourceMaterial => BuildSourceMaterialResult(hit),
        SearchHitKind.Note           => BuildNoteResult(hit),
        _                            => new SearchResultViewModel(hit, hit.Kind.ToString(), $"#{hit.Id}", null, "", false)
    };

    private SearchResultViewModel BuildSubjectResult(SearchHit hit)
    {
        var vm = FindSubject(hit.Id);
        return new SearchResultViewModel(hit, "Subject", vm?.Name ?? $"Subject #{hit.Id}", null, "", false);
    }

    private SearchResultViewModel BuildPlotPointResult(SearchHit hit)
    {
        var vm = FindPlotPoint(hit.Id);
        var title = vm is not null ? $"{vm.FullOrder}{vm.Title}" : $"Plot Point #{hit.Id}";
        return new SearchResultViewModel(hit, "Plot Point", title, null, "", false);
    }

    private SearchResultViewModel BuildChapterResult(SearchHit hit)
    {
        var vm = FindChapter(hit.Id);
        return new SearchResultViewModel(hit, "Chapter", vm?.FullNumberAndTitle ?? $"Chapter #{hit.Id}", null, "", false);
    }

    private SearchResultViewModel BuildThemeResult(SearchHit hit)
    {
        var vm = _registry.AllThemeViewModels.FirstOrDefault(t => t.Id == hit.Id);
        return new SearchResultViewModel(hit, "Theme", vm?.Name ?? $"Theme #{hit.Id}", null, "", false);
    }

    private SearchResultViewModel BuildSourceMaterialResult(SearchHit hit)
    {
        var vm = _registry.AllSourceMaterialViewModels.FirstOrDefault(sm => sm.Id == hit.Id);
        return new SearchResultViewModel(hit, "Source Material", vm?.Name ?? $"Source Material #{hit.Id}", null, "", false);
    }

    private SearchResultViewModel BuildNoteResult(SearchHit hit)
    {
        var note = _registry.AllNoteViewModels.FirstOrDefault(n => n.Id == hit.Id);
        if (note is null)
            return new SearchResultViewModel(hit, "Note", $"Note #{hit.Id}", null, "", false);

        var breadcrumb = OwnerBreadcrumbResolver.Resolve(note.OwnerId, note.OwnerType, _registry);
        var trackName = note.NoteTrackDefinition?.TrackName ?? "Unassigned";
        return new SearchResultViewModel(hit, "Note", breadcrumb, trackName, note.StateLabel, note.IsFlagged);
    }

    // ── Activation ───────────────────────────────────────────────────────────
    // The full navigation vocabulary already exists on IWindowManager; a search result just
    // has to resolve its Id back to a view model and hand it off, the same as every library
    // tab's own "Open" command.

    [RelayCommand]
    private void OpenSelected()
    {
        if (SelectedResult is not { } result) return;

        switch (result.Kind)
        {
            case SearchHitKind.Subject:
                if (FindSubject(result.Id) is { } subjectVm)
                    _windowManager.OpenSubjectWindow(subjectVm);
                break;

            case SearchHitKind.PlotPoint:
                if (FindPlotPoint(result.Id) is { } plotPointVm)
                    _windowManager.OpenPlotPointWindow(plotPointVm);
                break;

            case SearchHitKind.Chapter:
                if (FindChapter(result.Id) is { } chapterVm)
                    _windowManager.OpenChapterWindow(chapterVm);
                break;

            case SearchHitKind.Theme:
                if (_registry.AllThemeViewModels.FirstOrDefault(t => t.Id == result.Id) is { } themeVm)
                    _windowManager.OpenThemeWindow(themeVm);
                break;

            case SearchHitKind.SourceMaterial:
                if (_registry.AllSourceMaterialViewModels.FirstOrDefault(sm => sm.Id == result.Id) is { } sourceVm)
                    _windowManager.OpenSourceMaterialWindow(sourceVm);
                break;

            case SearchHitKind.Note:
                OpenNoteOwner(result.Id);
                break;
        }
    }

    private void OpenNoteOwner(int noteId)
    {
        var note = _registry.AllNoteViewModels.FirstOrDefault(n => n.Id == noteId);
        if (note is not null)
            OwnerNavigator.Open(note, _registry, _windowManager);
    }

    private SubjectViewModel? FindSubject(int id) => _registry.AllSubjectViewModels.FirstOrDefault(s => s.Id == id);
    private PlotPointViewModel? FindPlotPoint(int id) => _registry.AllPlotPointViewModels.FirstOrDefault(p => p.Id == id);
    private ChapterViewModel? FindChapter(int id) => _registry.AllChapterViewModels.FirstOrDefault(c => c.Id == id);
}
