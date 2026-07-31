using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// Reports which entities have no value on each configured narrative property.
///
/// This is a retrieval surface, not an attention metric. It answers "where is a value missing"
/// against a requirement Brian authored — he writes the property, he sets its gating phase — the
/// same category as PlanIntegrity's violation codes. It does NOT rank owners, score them, or
/// suggest which value any of them should get; deriving an axis position from note text or names
/// is exactly the categorization that stays authorial. Groups keep definition order, rows keep
/// library order.
///
/// Recomputed when the tab is shown (see PropertyGapsView's IsVisibleChanged) and on project load,
/// rather than by subscribing: the report derives from four collections at once — definitions,
/// allowed values, assignments, owners — and the service ones are reassigned on load, so
/// subscription would be four sets of bookkeeping that a later fifth input would silently outgrow.
/// The Refresh button remains for recomputing without leaving the tab.
/// </summary>
public partial class PropertyGapsViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;
    private readonly IWindowManager _windowManager;

    public ObservableCollection<PropertyGapGroup> Groups { get; } = new();

    [ObservableProperty]
    private string _summary = "No project loaded.";

    public PropertyGapsViewModel(
        IStoryService storyService,
        IViewModelRegistry registry,
        IWindowManager windowManager)
    {
        _storyService  = storyService;
        _registry      = registry;
        _windowManager = windowManager;

        _registry.StoryLoaded += Rebuild;
    }

    [RelayCommand]
    public void Rebuild()
    {
        Groups.Clear();

        var definitions = _storyService.NarrativePropertyDefinitions
            .OrderBy(d => d.DisplayOrder)
            .ThenBy(d => d.Name)
            .ToList();

        if (definitions.Count == 0)
        {
            Summary = "No narrative properties defined. Add them in the Definitions tab, "
                    + "or run the seed-narrative-properties DataOps op.";
            return;
        }

        var valueDefsByProperty = _storyService.NarrativePropertyValueDefinitions
            .GroupBy(v => v.NarrativePropertyDefinitionId)
            .ToDictionary(g => g.Key, g => g.Select(v => v.Id).ToHashSet());

        var phasesById = _storyService.WorkPhases.ToDictionary(p => p.Id);
        var subjectTypeById = _storyService.SubjectDefinitions.ToDictionary(s => s.Id, s => s.SubjectType);

        foreach (var def in definitions)
        {
            var validValueIds = valueDefsByProperty.TryGetValue(def.Id, out var ids) ? ids : new HashSet<int>();

            // (OwnerId, resolved property) is the key, never OwnerId alone: NarrativePropertyValue
            // has no OwnerType column, so a bare OwnerId match would let subject 7 satisfy the gap
            // on chapter 7. Scoping to this property's value ids resolves it — the same trace
            // ContentIntegrity and PlanIntegrity use.
            var assignedOwnerIds = _storyService.NarrativePropertyValues
                .Where(v => validValueIds.Contains(v.ValueDefinitionId))
                .Select(v => v.OwnerId)
                .ToHashSet();

            var owners = OwnersFor(def);

            var gaps = owners
                .Where(o => !assignedOwnerIds.Contains(o.OwnerId))
                .ToList();

            Groups.Add(new PropertyGapGroup
            {
                PropertyDefinitionId = def.Id,
                PropertyName = def.Name,
                Scope = ScopeLabel(def, subjectTypeById),
                GatingPhase = def.GatingWorkPhaseId is int pid && phasesById.TryGetValue(pid, out var phase)
                    ? phase.Name
                    : "—",
                TotalOwners = owners.Count,
                Gaps = gaps
            });
        }

        var totalGaps = Groups.Sum(g => g.Gaps.Count);
        Summary = $"{definitions.Count} propert{(definitions.Count == 1 ? "y" : "ies")}, "
                + $"{totalGaps} unset value{(totalGaps == 1 ? "" : "s")}.";
    }

    /// <summary>
    /// Mirrors the four propertyFactory closures exactly, including their asymmetry: Subject and
    /// PlotPointSubjectLink definitions are scoped by SubjectDefinitionId; PlotPoint and Chapter
    /// definitions apply to every owner of that type and ignore it. Diverging here would invent
    /// gaps the editor never shows.
    /// </summary>
    private List<PropertyGapRow> OwnersFor(NarrativePropertyDefinition def) =>
        def.OwnerType switch
        {
            OwnerType.Subject => _registry.AllSubjectViewModels
                .Where(s => s.SubjectDefinitionId == def.SubjectDefinitionId)
                .OrderBy(s => s.Name)
                .Select(s => Row(s.Id, OwnerType.Subject))
                .ToList(),

            OwnerType.PlotPointSubjectLink => _registry.AllPlotPointSubjectLinkViewModels
                .Where(l => _registry.AllSubjectViewModels
                    .FirstOrDefault(s => s.Id == l.SubjectId)?.SubjectDefinitionId == def.SubjectDefinitionId)
                .Select(l => Row(l.Id, OwnerType.PlotPointSubjectLink))
                .OrderBy(r => r.OwnerLabel)
                .ToList(),

            OwnerType.PlotPoint => _registry.AllPlotPointViewModels
                .Select(p => Row(p.Id, OwnerType.PlotPoint))
                .OrderBy(r => r.OwnerLabel)
                .ToList(),

            OwnerType.Chapter => _registry.AllChapterViewModels
                .Select(c => Row(c.Id, OwnerType.Chapter))
                .OrderBy(r => r.OwnerLabel)
                .ToList(),

            _ => new List<PropertyGapRow>()
        };

    private PropertyGapRow Row(int ownerId, OwnerType ownerType) => new()
    {
        OwnerId = ownerId,
        OwnerType = ownerType,
        OwnerLabel = OwnerBreadcrumbResolver.Resolve(ownerId, ownerType, _registry)
    };

    private static string ScopeLabel(NarrativePropertyDefinition def, IReadOnlyDictionary<int, string> subjectTypeById) =>
        def.OwnerType switch
        {
            OwnerType.Subject =>
                subjectTypeById.TryGetValue(def.SubjectDefinitionId, out var t) ? $"{t} subjects" : "Subjects",
            OwnerType.PlotPointSubjectLink =>
                subjectTypeById.TryGetValue(def.SubjectDefinitionId, out var lt) ? $"{lt} scene links" : "Scene links",
            OwnerType.PlotPoint => "Plot points",
            OwnerType.Chapter   => "Chapters",
            _                   => def.OwnerType.ToString()
        };

    /// <summary>
    /// Opens the owner so the value can be assigned there. The typed IWindowManager methods are
    /// deliberate — CommonWindow's primary element differs per mode, and pairing them wrongly used
    /// to be a compile-clean cast and a hard process kill.
    /// </summary>
    [RelayCommand]
    private void OpenOwner(PropertyGapRow? row)
    {
        if (row is null) return;

        switch (row.OwnerType)
        {
            case OwnerType.Subject:
                var subject = _registry.AllSubjectViewModels.FirstOrDefault(s => s.Id == row.OwnerId);
                if (subject is not null) _windowManager.OpenSubjectWindow(subject);
                break;

            case OwnerType.PlotPoint:
                var plotPoint = _registry.AllPlotPointViewModels.FirstOrDefault(p => p.Id == row.OwnerId);
                if (plotPoint is not null) _windowManager.OpenPlotPointWindow(plotPoint);
                break;

            case OwnerType.Chapter:
                var chapter = _registry.AllChapterViewModels.FirstOrDefault(c => c.Id == row.OwnerId);
                if (chapter is not null) _windowManager.OpenChapterWindow(chapter);
                break;

            case OwnerType.PlotPointSubjectLink:
                // A link is edited through its subject's window in Linking mode, with the link
                // preselected — there is no standalone link window.
                var link = _registry.AllPlotPointSubjectLinkViewModels.FirstOrDefault(l => l.Id == row.OwnerId);
                var linkSubject = link is null
                    ? null
                    : _registry.AllSubjectViewModels.FirstOrDefault(s => s.Id == link.SubjectId);
                if (linkSubject is not null)
                    _windowManager.OpenSubjectWindow(linkSubject, EditorMode.Linking, link);
                break;
        }
    }
}
