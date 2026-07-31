using System.Collections.Generic;

namespace StoryPlanner.Core;

/// <summary>
/// The collections EntitySearch.Run searches over. A plain record so callers can build it
/// from IStoryService collections at query time (never cached — see StoryService.LoadDataAsync,
/// which reassigns those collections on every project load) or from plain lists in tests.
/// </summary>
public record SearchInput(
    IReadOnlyList<Subject> Subjects,
    IReadOnlyList<PlotPoint> PlotPoints,
    IReadOnlyList<Chapter> Chapters,
    IReadOnlyList<Theme> Themes,
    IReadOnlyList<SourceMaterial> SourceMaterials,
    IReadOnlyList<Note> Notes);
