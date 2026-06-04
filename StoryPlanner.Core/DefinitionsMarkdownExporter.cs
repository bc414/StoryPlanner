using System.Text;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public static class DefinitionsMarkdownExporter
{
    // ── Section key helpers ────────────────────────────────────────────────────

    /// <summary>
    /// Returns the ## heading key for a track.
    /// Subject and PlotPointSubjectLink tracks share a key derived from their
    /// resolved subject type so they collapse into the same section.
    /// </summary>
    private static string SectionKey(NoteTrackDefinitionExportData t) =>
        t.OwnerType switch
        {
            nameof(OwnerType.Subject)              => string.IsNullOrWhiteSpace(t.SelectedSubjectType)
                                                          ? "Unassigned Subject"
                                                          : t.SelectedSubjectType,
            nameof(OwnerType.PlotPointSubjectLink) => string.IsNullOrWhiteSpace(t.SelectedSubjectType)
                                                          ? "Unassigned Subject"
                                                          : t.SelectedSubjectType,
            nameof(OwnerType.PlotPoint)            => "Plot Point",
            nameof(OwnerType.Chapter)              => "Chapter",
            _                                      => t.OwnerType
        };

    /// <summary>
    /// Returns the ### sub-group label within a section.
    /// </summary>
    private static string SubGroupLabel(NoteTrackDefinitionExportData t) =>
        t.OwnerType switch
        {
            nameof(OwnerType.PlotPointSubjectLink) => "Link Tracks",
            nameof(OwnerType.Chapter) => "Chapter Tracks",
            nameof(OwnerType.PlotPoint) => "Plot Point Tracks",
            _                                      => "Project-Wide Tracks"
        };

    /// <summary>
    /// Sort order for ## sections: subject types first (alphabetical),
    /// then Plot Point, then Chapter.
    /// </summary>
    private static int SectionOrder(string sectionKey) =>
        sectionKey switch
        {
            "Plot Point" => int.MaxValue - 1,
            "Chapter"    => int.MaxValue,
            _            => 0   // subject-type sections sort alphabetically amongst themselves
        };

    // ── Public entry point ─────────────────────────────────────────────────────

    /// <summary>
    /// Builds a markdown document from the resolved subject and note-track definitions.
    /// Tracks are grouped by entity section (## Subject Type / Plot Point / Chapter),
    /// then by ownership kind (### Project-Wide Track / Link Track).
    /// </summary>
    public static string Build(
        IEnumerable<(string SubjectType, int DisplayOrder)> subjectDefinitions,
        IEnumerable<NoteTrackDefinitionExportData> noteTrackDefinitions)
    {
        var sb = new StringBuilder();

        // ── Track Types ────────────────────────────────────────────────────────
        sb.AppendLine("# Track Types");
        sb.AppendLine();
        sb.AppendLine("Track types define the cognitive mode and authorial perspective of a note track.");
        sb.AppendLine();

        foreach (var trackType in Enum.GetValues<TrackType>().Where(t => t != TrackType.Unset))
        {
            sb.AppendLine($"- **{trackType}**: {trackType.GetCognitiveMode()}");
        }

        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();

        // ── Subject Definitions ────────────────────────────────────────────────
        sb.AppendLine("# Subject Definitions");
        sb.AppendLine();
        sb.AppendLine("Subject types define the categories of entities (characters, locations, etc.) "
                    + "that can own notes and appear in the story planner.");
        sb.AppendLine();

        foreach (var s in subjectDefinitions.OrderBy(s => s.DisplayOrder))
            sb.AppendLine($"- **{s.SubjectType}**");

        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();

        // ── Note Track Definitions ─────────────────────────────────────────────
        sb.AppendLine("# Note Track Definitions");
        sb.AppendLine();
        sb.AppendLine("Each note track definition describes one category of notes that can be "
                    + "attached to subjects. The text below are helpers for an author to "
                    + "write and audit notes of each type.");
        sb.AppendLine();

        var tracks = noteTrackDefinitions.ToList();

        var sections = tracks
            .GroupBy(SectionKey)
            .OrderBy(g => SectionOrder(g.Key))
            .ThenBy(g => g.Key, StringComparer.OrdinalIgnoreCase);

        foreach (var section in sections)
        {
            sb.AppendLine($"## {section.Key}");
            sb.AppendLine();

            // "Project-Wide Track" before "Link Track"
            var subGroups = section
                .GroupBy(SubGroupLabel)
                .OrderBy(g => g.Key == "Project-Wide Track" ? 0 : 1);

            foreach (var subGroup in subGroups)
            {
                sb.AppendLine($"### {subGroup.Key}");
                sb.AppendLine();

                foreach (var t in subGroup.OrderBy(t => t.ExpansionModeDisplayOrder))
                {
                    sb.AppendLine($"#### {t.TrackName}");
                    sb.AppendLine();

                    sb.AppendLine($"Track Type: {t.TrackType}");
                    sb.AppendLine();

                    if (!string.IsNullOrWhiteSpace(t.DisplayQuestion))
                    {
                        sb.AppendLine("**Display Question**");
                        sb.AppendLine();
                        sb.AppendLine(t.DisplayQuestion);
                        sb.AppendLine();
                    }

                    if (!string.IsNullOrWhiteSpace(t.UsageDirective))
                    {
                        sb.AppendLine("**Usage Directive**");
                        sb.AppendLine();
                        sb.AppendLine(t.UsageDirective);
                        sb.AppendLine();
                    }

                    if (!string.IsNullOrWhiteSpace(t.AuditDirective))
                    {
                        sb.AppendLine("**Audit Directive**");
                        sb.AppendLine();
                        sb.AppendLine(t.AuditDirective);
                        sb.AppendLine();
                    }

                    sb.AppendLine("---");
                    sb.AppendLine();
                }
            }
        }

        return sb.ToString();
    }

    private static string BoolLabel(bool value) => value ? "Yes" : "No";
}

/// <summary>
/// Plain data record passed from the ViewModel layer to the exporter.
/// Carries the already-resolved SelectedSubjectType so the Core layer
/// stays free of ViewModel dependencies.
/// </summary>
public record NoteTrackDefinitionExportData(
    string TrackName,
    string TrackType,
    string OwnerType,
    string SelectedSubjectType,
    bool IsSingleton,
    bool SupportsWorldDate,
    bool SupportsTheme,
    bool CanEditInAuditMode,
    string DisplayQuestion,
    string UsageDirective,
    string AuditDirective,
    int ExpansionModeDisplayOrder,
    int LinkingModeDisplayOrder,
    int GardenerModeDisplayOrder,
    int AuditModeDisplayOrder,
    int SceneDesignModeDisplayOrder);