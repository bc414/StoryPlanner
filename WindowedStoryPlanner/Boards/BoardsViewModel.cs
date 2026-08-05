using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// The Boards tab: a board selector, and two independent views over the selected board — its
/// pairwise grids, and a subject tree.
///
/// <para><b>These are two separate features that happen to share a scope.</b> The grids do not
/// show relations and the tree does not show cells; there is no overlay between them. What they
/// share is the board — which properties are under comparison — and therefore the card control
/// that renders a subject's values, so a pattern learned in one reads identically in the other.</para>
///
/// <para><b>Retrieval, not suggestion.</b> Cells are populated by authored assignments and the tree
/// by authored edges. Nothing here ranks a cell, scores a lineage, computes coverage, or proposes
/// a value or an edge. An empty cell is a fact about the world Brian built, exactly like an
/// untouched source-material Part.</para>
///
/// Recomputed on tab-show and on project load rather than by subscription, for the reason
/// PropertyGapsViewModel documents: the report derives from several collections at once and the
/// service ones are reassigned on load, so subscribing would be several sets of bookkeeping that a
/// later input would silently outgrow.
/// </summary>
public partial class BoardsViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;
    private readonly IWindowManager _windowManager;

    private bool _suppressRebuild;

    public BoardsViewModel(
        IStoryService storyService,
        IViewModelRegistry registry,
        IWindowManager windowManager)
    {
        _storyService  = storyService;
        _registry      = registry;
        _windowManager = windowManager;

        _registry.StoryLoaded += Rebuild;
    }

    // ── Board selection ───────────────────────────────────────────────────────

    public ObservableCollection<PropertyBoardViewModel> Boards => _registry.AllPropertyBoardViewModels;

    [ObservableProperty]
    private PropertyBoardViewModel? _selectedBoard;

    partial void OnSelectedBoardChanged(PropertyBoardViewModel? value)
    {
        if (_suppressRebuild) return;
        RebuildGrids();
        RebuildRelationChoices();
    }

    [ObservableProperty]
    private string _summary = "No project loaded.";

    // ── Grids ─────────────────────────────────────────────────────────────────

    public ObservableCollection<PropertyGridViewModel> Grids { get; } = new();

    // ── Matches ───────────────────────────────────────────────────────────────

    /// <summary>Groups of 2+ subjects on identical coordinates, largest first.</summary>
    public ObservableCollection<MatchGroupViewModel> SharedMatchGroups { get; } = new();

    /// <summary>Subjects alone on their coordinates, in authored value order.</summary>
    public ObservableCollection<MatchGroupViewModel> AloneMatchGroups { get; } = new();

    /// <summary>Subjects excluded from grouping because a board property is unset.</summary>
    public ObservableCollection<PartiallyAssignedViewModel> PartiallyAssigned { get; } = new();

    [ObservableProperty]
    private string _matchSummary = string.Empty;

    // ── Tree ──────────────────────────────────────────────────────────────────

    public ObservableCollection<SubjectRelationDefinitionViewModel> RelationChoices { get; } = new();

    [ObservableProperty]
    private SubjectRelationDefinitionViewModel? _selectedRelation;

    partial void OnSelectedRelationChanged(SubjectRelationDefinitionViewModel? value)
    {
        if (_suppressRebuild) return;
        RebuildRootChoices();
        RebuildTree();
    }

    public ObservableCollection<SubjectViewModel> RootChoices { get; } = new();

    [ObservableProperty]
    private SubjectViewModel? _selectedRoot;

    partial void OnSelectedRootChanged(SubjectViewModel? value)
    {
        if (!_suppressRebuild) RebuildTree();
    }

    /// <summary>
    /// False walks the stored direction (subject → target, labelled by the relation's Name); true
    /// walks it backwards (labelled by InverseName). With an "Ancestor" edge, forward from the
    /// youngest system climbs its line and inverted from the oldest renders the whole descendant
    /// tree — the same authored edges read two ways, never a second stored edge.
    /// </summary>
    [ObservableProperty]
    private bool _walkInverted = true;

    partial void OnWalkInvertedChanged(bool value)
    {
        if (!_suppressRebuild) RebuildTree();
    }

    public string DirectionLabel => SelectedRelation is null
        ? string.Empty
        : WalkInverted
            ? (string.IsNullOrWhiteSpace(SelectedRelation.InverseName)
                ? $"inverse of \"{SelectedRelation.Name}\""
                : SelectedRelation.InverseName)
            : SelectedRelation.Name;

    public ObservableCollection<SubjectTreeNodeViewModel> TreeNodes { get; } = new();

    [ObservableProperty]
    private string _treeSummary = string.Empty;

    // ── Rebuild ───────────────────────────────────────────────────────────────

    [RelayCommand]
    public void Rebuild()
    {
        _suppressRebuild = true;
        try
        {
            // Boards is the registry's own collection, already repopulated by ProjectLoader; only
            // the selection needs re-establishing, by id, since the VM instances are new.
            var previousBoardId = SelectedBoard?.Id;
            SelectedBoard = Boards.FirstOrDefault(b => b.Id == previousBoardId) ?? Boards.FirstOrDefault();
        }
        finally
        {
            _suppressRebuild = false;
        }

        RebuildGrids();
        RebuildRelationChoices();
    }

    private void RebuildGrids()
    {
        Grids.Clear();
        SharedMatchGroups.Clear();
        AloneMatchGroups.Clear();
        PartiallyAssigned.Clear();
        MatchSummary = string.Empty;

        if (SelectedBoard is null)
        {
            Summary = Boards.Count == 0
                ? "No boards defined. Add one in Definitions → Boards & Relations, then put properties on it with the Board column."
                : "Select a board.";
            return;
        }

        var properties = BoardProperties(SelectedBoard);

        if (properties.Count < 2)
        {
            Summary = $"\"{SelectedBoard.Name}\" has {properties.Count} propert"
                    + $"{(properties.Count == 1 ? "y" : "ies")} — a grid needs two. "
                    + "Assign more on the Narrative Properties tab.";
            return;
        }

        var subjects = BoardSubjects(SelectedBoard);
        var valueDefsByProperty = ValueDefsByProperty(properties);
        var held = HeldValueIdsBySubject(valueDefsByProperty);

        // Cards are built once per subject and shared across all C(n,2) grids: the card is
        // position-independent by design, so ten grids over 37 subjects need 37 cards, not 370.
        var cardsBySubjectId = subjects.ToDictionary(
            s => s.Id,
            s => SubjectCardViewModel.Build(
                s, properties, valueDefsByProperty,
                held.TryGetValue(s.Id, out var h) ? h : new HashSet<int>(),
                OpenSubject));

        for (var i = 0; i < properties.Count; i++)
        for (var j = i + 1; j < properties.Count; j++)
            Grids.Add(BuildGrid(
                properties[i], properties[j], valueDefsByProperty, held, subjects, cardsBySubjectId,
                SelectedBoard.IncludeUnsetBand));

        var pairCount = Grids.Count;
        Summary = $"{properties.Count} properties · {pairCount} grid{(pairCount == 1 ? "" : "s")} · "
                + $"{subjects.Count} subjects"
                + (SelectedBoard.IncludeUnsetBand ? " · unset shown as a band" : " · unset omitted");

        // Same inputs, same cards — a card is position-independent, so the 37 built above serve
        // every grid AND every match group. Do not build a second set.
        RebuildMatches(properties, valueDefsByProperty, held, subjects, cardsBySubjectId);
    }

    /// <summary>
    /// Groups subjects holding identical values on EVERY board property. Distinct from the grids,
    /// which cross two properties at a time and so mix subjects that agree on those two and differ
    /// elsewhere — the full-tuple collisions are only visible here.
    ///
    /// A subject unset on any property is not grouped and is listed separately: "these agree on all
    /// five" cannot be said when some are unknown.
    /// </summary>
    private void RebuildMatches(
        IReadOnlyList<NarrativePropertyDefinition> properties,
        IReadOnlyDictionary<int, List<NarrativePropertyValueDefinition>> valueDefsByProperty,
        IReadOnlyDictionary<int, IReadOnlySet<int>> held,
        IReadOnlyList<SubjectViewModel> subjects,
        IReadOnlyDictionary<int, SubjectCardViewModel> cardsBySubjectId)
    {
        var result = NarrativePropertyMatchGroups.Build(
            properties, valueDefsByProperty, held, subjects.Select(s => s.Id));

        // The group's tuple is not projected anywhere: every member card already renders it, so a
        // group-level copy would be the same five values a seventh time.
        MatchGroupViewModel ToViewModel(NarrativePropertyMatchGroups.Group group) => new()
        {
            Cards = group.OwnerIds.Select(id => cardsBySubjectId[id]).ToList()
        };

        foreach (var group in result.Shared) SharedMatchGroups.Add(ToViewModel(group));
        foreach (var group in result.Alone) AloneMatchGroups.Add(ToViewModel(group));

        foreach (var partial in result.PartiallyAssigned)
            PartiallyAssigned.Add(new PartiallyAssignedViewModel
            {
                Card = cardsBySubjectId[partial.OwnerId],
                UnsetCount = partial.UnsetCount
            });

        var sharedCount = result.SharedOwnerCount;
        MatchSummary =
            $"{SharedMatchGroups.Count} shared position{(SharedMatchGroups.Count == 1 ? "" : "s")} "
            + $"covering {sharedCount} subject{(sharedCount == 1 ? "" : "s")} · "
            + $"{AloneMatchGroups.Count} alone"
            + (PartiallyAssigned.Count > 0
                ? $" · {PartiallyAssigned.Count} not fully assigned, not grouped"
                : string.Empty);
    }

    private PropertyGridViewModel BuildGrid(
        NarrativePropertyDefinition rowProperty,
        NarrativePropertyDefinition columnProperty,
        IReadOnlyDictionary<int, List<NarrativePropertyValueDefinition>> valueDefsByProperty,
        IReadOnlyDictionary<int, IReadOnlySet<int>> held,
        IReadOnlyList<SubjectViewModel> subjects,
        IReadOnlyDictionary<int, SubjectCardViewModel> cardsBySubjectId,
        bool includeUnsetBand)
    {
        var result = NarrativePropertyCrossTab.Build(
            valueDefsByProperty.TryGetValue(rowProperty.Id, out var rv) ? rv : [],
            valueDefsByProperty.TryGetValue(columnProperty.Id, out var cv) ? cv : [],
            held,
            subjects.Select(s => s.Id),
            includeUnsetBand);

        var grid = new PropertyGridViewModel
        {
            RowPropertyName = rowProperty.Name,
            ColumnPropertyName = columnProperty.Name,
            PlacedCount = result.PlacedOwnerCount,
            OmittedCount = subjects.Count - result.PlacedOwnerCount,
            ColumnCount = result.Columns.Count + 1
        };

        // Header row: corner, then one header per column band.
        grid.Cells.Add(new PropertyGridCell { Kind = PropertyGridCellKind.Corner });
        foreach (var column in result.Columns)
            grid.Cells.Add(new PropertyGridCell
            {
                Kind = PropertyGridCellKind.ColumnHeader,
                Label = column.Label,
                ColorHex = column.ColorHex
            });

        // Body rows: row header, then the cells.
        for (var r = 0; r < result.Rows.Count; r++)
        {
            grid.Cells.Add(new PropertyGridCell
            {
                Kind = PropertyGridCellKind.RowHeader,
                Label = result.Rows[r].Label,
                ColorHex = result.Rows[r].ColorHex
            });

            for (var c = 0; c < result.Columns.Count; c++)
                grid.Cells.Add(new PropertyGridCell
                {
                    Kind = PropertyGridCellKind.Body,
                    Cards = result.CellAt(r, c).OwnerIds
                        .Select(id => cardsBySubjectId[id])
                        .ToList()
                });
        }

        return grid;
    }

    private void RebuildRelationChoices()
    {
        _suppressRebuild = true;
        try
        {
            var previousId = SelectedRelation?.Id;
            RelationChoices.Clear();

            if (SelectedBoard is not null)
                foreach (var definition in _registry.AllSubjectRelationDefinitionViewModels
                             .Where(d => d.SubjectDefinitionId == SelectedBoard.SubjectDefinitionId)
                             .OrderBy(d => d.DisplayOrder).ThenBy(d => d.Id))
                    RelationChoices.Add(definition);

            SelectedRelation = RelationChoices.FirstOrDefault(d => d.Id == previousId)
                            ?? RelationChoices.FirstOrDefault();
        }
        finally
        {
            _suppressRebuild = false;
        }

        RebuildRootChoices();
        RebuildTree();
    }

    private void RebuildRootChoices()
    {
        _suppressRebuild = true;
        try
        {
            var previousId = SelectedRoot?.Id;
            RootChoices.Clear();

            if (SelectedRelation is not null)
                foreach (var subject in _registry.AllSubjectViewModels
                             .Where(s => s.SubjectDefinitionId == SelectedRelation.SubjectDefinitionId)
                             .OrderBy(s => s.Name, System.StringComparer.CurrentCultureIgnoreCase))
                    RootChoices.Add(subject);

            SelectedRoot = RootChoices.FirstOrDefault(s => s.Id == previousId);
        }
        finally
        {
            _suppressRebuild = false;
        }
    }

    private void RebuildTree()
    {
        TreeNodes.Clear();
        OnPropertyChanged(nameof(DirectionLabel));

        if (SelectedBoard is null || SelectedRelation is null || SelectedRoot is null)
        {
            TreeSummary = SelectedRelation is null
                ? "No relations defined for this subject type. Add one in Definitions → Boards & Relations."
                : "Pick a subject to render from.";
            return;
        }

        var properties = BoardProperties(SelectedBoard);
        var valueDefsByProperty = ValueDefsByProperty(properties);
        var held = HeldValueIdsBySubject(valueDefsByProperty);

        var walk = SubjectRelationGraph.Walk(
            _storyService.SubjectRelations, SelectedRelation.Id, SelectedRoot.Id, WalkInverted);

        foreach (var node in walk)
        {
            var subject = _registry.AllSubjectViewModels.FirstOrDefault(s => s.Id == node.SubjectId);
            if (subject is null) continue;

            TreeNodes.Add(new SubjectTreeNodeViewModel
            {
                Card = SubjectCardViewModel.Build(
                    subject, properties, valueDefsByProperty,
                    held.TryGetValue(subject.Id, out var h) ? h : new HashSet<int>(),
                    OpenSubject),
                Depth = node.Depth,
                StopsOnCycle = node.StopsOnCycle
            });
        }

        var reached = TreeNodes.Count(n => !n.StopsOnCycle);
        TreeSummary = reached <= 1
            ? $"\"{SelectedRoot.Name}\" has nothing beneath it on this relation."
            : $"{reached} subjects, {TreeNodes.Max(n => n.Depth)} deep.";
    }

    // ── Shared lookups ────────────────────────────────────────────────────────

    /// <summary>
    /// The board's member properties, in DisplayOrder. Filtered to OwnerType.Subject — boards are
    /// subject-scoped, and a property of another owner type on a board is a data error
    /// PlanIntegrity reports rather than something to render.
    /// </summary>
    private List<NarrativePropertyDefinition> BoardProperties(PropertyBoardViewModel board) =>
        _storyService.NarrativePropertyDefinitions
            .Where(p => p.PropertyBoardId == board.Id && p.OwnerType == OwnerType.Subject)
            .OrderBy(p => p.DisplayOrder).ThenBy(p => p.Id)
            .ToList();

    private List<SubjectViewModel> BoardSubjects(PropertyBoardViewModel board) =>
        _registry.AllSubjectViewModels
            .Where(s => s.SubjectDefinitionId == board.SubjectDefinitionId)
            .OrderBy(s => s.Name, System.StringComparer.CurrentCultureIgnoreCase)
            .ToList();

    /// <summary>Allowed values per property, in row order — the authored spectrum.</summary>
    private Dictionary<int, List<NarrativePropertyValueDefinition>> ValueDefsByProperty(
        IReadOnlyList<NarrativePropertyDefinition> properties)
    {
        var ids = properties.Select(p => p.Id).ToHashSet();
        return _storyService.NarrativePropertyValueDefinitions
            .Where(v => ids.Contains(v.NarrativePropertyDefinitionId))
            .OrderBy(v => v.Id)
            .GroupBy(v => v.NarrativePropertyDefinitionId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    /// <summary>
    /// Assignments folded to owner → value ids, scoped to THIS board's value definitions.
    /// NarrativePropertyValue has no OwnerType column, so a bare OwnerId match would let chapter 7
    /// satisfy subject 7 — restricting to these value ids is the same trace ContentIntegrity,
    /// PlanIntegrity and the entity editor all use.
    /// </summary>
    private IReadOnlyDictionary<int, IReadOnlySet<int>> HeldValueIdsBySubject(
        IReadOnlyDictionary<int, List<NarrativePropertyValueDefinition>> valueDefsByProperty)
    {
        var boardValueIds = valueDefsByProperty.Values.SelectMany(v => v).Select(v => v.Id).ToHashSet();

        return NarrativePropertyCrossTab.MapAssignments(
            _storyService.NarrativePropertyValues.Where(v => boardValueIds.Contains(v.ValueDefinitionId)));
    }

    private void OpenSubject(SubjectViewModel subject) => _windowManager.OpenSubjectWindow(subject);
}
