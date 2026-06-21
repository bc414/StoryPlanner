using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Export;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class ExportViewModel : ObservableObject
{
    private readonly IStoryService       _storyService;
    private readonly IViewModelRegistry  _registry;
    private readonly AppSettings         _appSettings;
    private readonly ExportService       _exportService;

    // ---- Anchor picker ----
    [ObservableProperty] private string       _searchQuery           = string.Empty;
    [ObservableProperty] private OwnerType?   _searchOwnerTypeFilter;   // null = all types
    [ObservableProperty] private AnchorCandidateItem? _selectedSearchResult;

    public ObservableCollection<AnchorCandidateItem> SearchResults { get; } = new();
    public ObservableCollection<AnchorCandidateItem> Anchors       { get; } = new();

    // ---- Search type filter helpers ----
    public static IReadOnlyList<string> TypeFilterOptions { get; } =
        new[] { "All", "Subject", "Plot Point", "Chapter" };

    [ObservableProperty]
    private string _selectedTypeFilter = "All";

    partial void OnSelectedTypeFilterChanged(string value)
    {
        SearchOwnerTypeFilter = value switch
        {
            "Subject"    => (OwnerType?)OwnerType.Subject,
            "Plot Point" => (OwnerType?)OwnerType.PlotPoint,
            "Chapter"    => (OwnerType?)OwnerType.Chapter,
            _            => null
        };
    }

    // ---- Export options ----
    [ObservableProperty] private int  _scope;

    // String-backed chapter range (TextBox-friendly; empty = no bound)
    [ObservableProperty] private string _chapterFromText = string.Empty;
    [ObservableProperty] private string _chapterToText   = string.Empty;

    private int? ChapterFrom =>
        int.TryParse(ChapterFromText, out var v) ? v : (int?)null;

    private int? ChapterTo =>
        int.TryParse(ChapterToText, out var v) ? v : (int?)null;

    public ObservableCollection<TrackTypeFilterItem> TrackTypeFilters { get; } = new();

    public ExportViewModel(IStoryService storyService, IViewModelRegistry registry, AppSettings appSettings, ExportService exportService)
    {
        _storyService  = storyService;
        _registry      = registry;
        _appSettings   = appSettings;
        _exportService = exportService;
        InitializeTrackTypeFilters();
        _appSettings.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppSettings.IsArchiveMode))
                UpdateUnassignedDefault();
        };
    }

    private void InitializeTrackTypeFilters()
    {
        TrackTypeFilters.Clear();
        foreach (var tt in Enum.GetValues<TrackType>().Where(t => t != TrackType.Unset))
            TrackTypeFilters.Add(new TrackTypeFilterItem(tt, isIncluded: true));
        TrackTypeFilters.Add(new TrackTypeFilterItem(TrackType.Unset, isIncluded: _appSettings.IsArchiveMode));
    }

    private void UpdateUnassignedDefault()
    {
        var item = TrackTypeFilters.FirstOrDefault(f => f.TrackType == TrackType.Unset);
        if (item != null) item.IsIncluded = _appSettings.IsArchiveMode;
    }

    // Called by ProjectLoader after a project is loaded
    public void Reload() => RebuildSearchResults();

    // ---- Search ----

    partial void OnSearchQueryChanged(string value)           => RebuildSearchResults();
    partial void OnSearchOwnerTypeFilterChanged(OwnerType? value) => RebuildSearchResults();

    private void RebuildSearchResults()
    {
        SearchResults.Clear();
        if (!_storyService.IsProjectLoaded) return;

        var query  = SearchQuery.Trim();
        var filter = SearchOwnerTypeFilter;

        if (filter is null or OwnerType.Subject)
        {
            foreach (var s in _storyService.Subjects
                         .Where(s => Matches(s.Name, query))
                         .OrderBy(s => s.Name))
                SearchResults.Add(new AnchorCandidateItem(s.Id, OwnerType.Subject, s.Name));
        }

        if (filter is null or OwnerType.PlotPoint)
        {
            foreach (var pp in _storyService.PlotPoints
                         .Where(p => Matches(p.Title, query))
                         .OrderBy(p => p.Title))
                SearchResults.Add(new AnchorCandidateItem(pp.Id, OwnerType.PlotPoint, pp.Title));
        }

        if (filter is null or OwnerType.Chapter)
        {
            foreach (var c in _storyService.Chapters
                         .Where(c => Matches(c.Title, query))
                         .OrderBy(c => c.OrderIndex))
                SearchResults.Add(new AnchorCandidateItem(c.Id, OwnerType.Chapter, c.Title));
        }
    }

    private static bool Matches(string name, string query)
        => string.IsNullOrWhiteSpace(query)
           || name.Contains(query, StringComparison.OrdinalIgnoreCase);

    // ---- Commands ----

    [RelayCommand]
    private void AddAnchor()
    {
        if (SelectedSearchResult is not { } item) return;
        if (Anchors.Any(a => a.Id == item.Id && a.OwnerType == item.OwnerType)) return;
        Anchors.Add(item);
    }

    [RelayCommand]
    private void RemoveAnchor(AnchorCandidateItem item) => Anchors.Remove(item);

    [RelayCommand]
    private void SelectAllTrackTypes()
    {
        foreach (var f in TrackTypeFilters) f.IsIncluded = true;
    }

    [RelayCommand]
    private void SelectNoneTrackTypes()
    {
        foreach (var f in TrackTypeFilters) f.IsIncluded = false;
    }

    [RelayCommand]
    private void Export()
    {
        if (!_storyService.IsProjectLoaded)
        {
            MessageBox.Show("No project is open.", "Export", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (Anchors.Count == 0)
        {
            MessageBox.Show("Add at least one anchor before exporting.", "Export",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var config = new ExportConfiguration
        {
            Anchors            = Anchors.Select(a => (a.Id, a.OwnerType)).ToList(),
            Scope              = Scope,
            ChapterFrom        = ChapterFrom,
            ChapterTo          = ChapterTo,
            IncludedTrackTypes = TrackTypeFilters
                .Where(f => f.IsIncluded)
                .Select(f => f.TrackType)
                .ToHashSet()
        };

        var markdown = _exportService.BuildMarkdown(config);
        var path     = _exportService.WriteToFile(markdown, "", BuildFileName());
        MessageBox.Show($"Exported to:\n{path}", "Export Complete",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    [RelayCommand]
    private void ExportFull()
    {
        if (!_storyService.IsProjectLoaded)
        {
            MessageBox.Show("No project is open.", "Export", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        var path = _exportService.ExportAll();
        MessageBox.Show($"Full export saved to:\n{path}", "Full Export Complete",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    [RelayCommand]
    private void ExportIndividual()
    {
        if (!_storyService.IsProjectLoaded)
        {
            MessageBox.Show("No project is open.", "Export", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        var (s, pp) = _exportService.ExportAllIndividual();
        var dir     = Path.Combine(_exportService.GetExportsDirectory(), "Individual");
        MessageBox.Show($"Exported {s} subject files and {pp} plot point files to:\n{dir}",
            "Individual Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private string BuildFileName()
    {
        var names  = string.Join(",", Anchors.Select(a => ExportService.SanitizeFileName(a.DisplayName)));
        var suffix = $"-scope{Scope}.md";
        var raw    = names + suffix;
        const int max = 80;
        return raw.Length <= max ? raw : raw[..(max - suffix.Length)] + suffix;
    }
}

public class AnchorCandidateItem
{
    public int       Id          { get; }
    public OwnerType OwnerType   { get; }
    public string    DisplayName { get; }

    public string TypeLabel => OwnerType switch
    {
        OwnerType.Subject            => "Subject",
        OwnerType.PlotPoint          => "Plot Point",
        OwnerType.Chapter            => "Chapter",
        _                            => OwnerType.ToString()
    };

    public AnchorCandidateItem(int id, OwnerType ownerType, string displayName)
    {
        Id          = id;
        OwnerType   = ownerType;
        DisplayName = displayName;
    }
}

public partial class TrackTypeFilterItem : ObservableObject
{
    public TrackType TrackType { get; }
    public string    Label     => TrackType == TrackType.Unset ? "Unassigned" : TrackType.ToString();

    [ObservableProperty]
    private bool _isIncluded;

    public TrackTypeFilterItem(TrackType trackType, bool isIncluded)
    {
        TrackType   = trackType;
        _isIncluded = isIncluded;
    }
}
