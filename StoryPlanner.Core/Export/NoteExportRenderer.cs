using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryPlanner.Core;

namespace StoryPlanner.Core;

public static class NoteExportRenderer
{
    public static string Build(ExportResult result, ExportConfiguration config, IStoryService storyService)
    {
        var sb = new StringBuilder();

        // ---- Lookups ----
        var subjectById     = storyService.Subjects.ToDictionary(s => s.Id);
        var plotPointById   = storyService.PlotPoints.ToDictionary(p => p.Id);
        var chapterById     = storyService.Chapters.ToDictionary(c => c.Id);
        var storyById       = storyService.Stories.ToDictionary(s => s.Id);
        var themeById       = storyService.Themes.ToDictionary(t => t.Id);
        var subjectDefById  = storyService.SubjectDefinitions.ToDictionary(sd => sd.Id);

        // Source material citations, pre-formatted per note (NoteId -> "Work · Part" joined by
        // ", " for multi-cite notes) so the metadata renderer stays a single dictionary lookup,
        // matching themeById's shape. A note can cite several Parts for one claim (see
        // NoteSourceReference's doc comment) — all are rendered, not just the first.
        var sourceMaterialById     = storyService.SourceMaterials.ToDictionary(w => w.Id);
        var sourceMaterialPartById = storyService.SourceMaterialParts.ToDictionary(p => p.Id);
        var sourceCitationByNoteId = storyService.NoteSourceReferences
            .GroupBy(r => r.NoteId)
            .ToDictionary(
                g => g.Key,
                g => string.Join(", ", g
                    .OrderBy(r => r.SortOrder)
                    .Select(r => FormatCitation(r, sourceMaterialById, sourceMaterialPartById))
                    .Where(s => s.Length > 0)));

        // StoryId 0 is the "(Unassigned)" sentinel — never a real Story row.
        int StoryOrderOf(Chapter ch) => ch.StoryId == 0 || !storyById.TryGetValue(ch.StoryId, out var s)
            ? int.MaxValue : s.OrderIndex;
        string StoryLabelOf(Chapter ch) => ch.StoryId == 0
            ? "(Unassigned)"
            : (storyById.TryGetValue(ch.StoryId, out var s) ? s.Title : $"story:{ch.StoryId}?");

        // Map (PlotPointId, SubjectId) → PlotPointSubjectLink.Id for note lookups
        var linkIdByPair = storyService.PlotPointsSubjectLinks
            .ToDictionary(l => (l.PlotPointId, l.SubjectId), l => l.Id);

        // Filtered notes (exclude Flagged only; null NoteTrackDefinitionId = Unassigned, kept for optional rendering)
        var notesByOwner = storyService.Notes
            .Where(n => n.NoteState != NoteState.Flagged)
            .GroupBy(n => (n.OwnerId, n.OwnerType))
            .ToDictionary(g => g.Key, g => g.ToList());

        // Track def lookups
        var subjectTrackDefs = storyService.NoteTrackDefinitions
            .Where(ntd => ntd.OwnerType == OwnerType.Subject)
            .GroupBy(ntd => ntd.SubjectDefinitionId)
            .ToDictionary(g => g.Key, g => g.OrderBy(ntd => ntd.ExpansionModeDisplayOrder).ToList());

        var linkTrackDefs = storyService.NoteTrackDefinitions
            .Where(ntd => ntd.OwnerType == OwnerType.PlotPointSubjectLink)
            .GroupBy(ntd => ntd.SubjectDefinitionId)
            .ToDictionary(g => g.Key, g => g.OrderBy(ntd => ntd.ExpansionModeDisplayOrder).ToList());

        var plotPointTrackDefs = storyService.NoteTrackDefinitions
            .Where(ntd => ntd.OwnerType == OwnerType.PlotPoint)
            .OrderBy(ntd => ntd.ExpansionModeDisplayOrder)
            .ToList();

        // SubjectDefinition IDs present in the export content
        var subDefIdsInPart1 = result.FullSubjectIds
            .Where(id => subjectById.ContainsKey(id))
            .Select(id => subjectById[id].SubjectDefinitionId)
            .ToHashSet();

        var subDefIdsInLinks = result.ActiveLinks
            .Select(l => l.SubjectId)
            .Where(id => subjectById.ContainsKey(id))
            .Select(id => subjectById[id].SubjectDefinitionId)
            .ToHashSet();

        bool hasAnyPlotPoints = result.FullPlotPointIds.Count > 0 || result.ThinPlotPointIds.Count > 0;

        // ---- PREAMBLE ----
        sb.AppendLine("# Preamble");
        sb.AppendLine();
        BuildPreamble(sb, config, storyService, subjectDefById,
            subDefIdsInPart1, subDefIdsInLinks, hasAnyPlotPoints,
            subjectTrackDefs, linkTrackDefs, plotPointTrackDefs);

        // ---- PART 1: SUBJECT PROFILES ----
        if (result.FullSubjectIds.Count > 0)
        {
            sb.AppendLine("---");
            sb.AppendLine();
            sb.AppendLine("# Part 1: Subject Profiles");
            sb.AppendLine();

            var subjectGroups = result.FullSubjectIds
                .Where(id => subjectById.ContainsKey(id))
                .Select(id => subjectById[id])
                .Where(s => subjectDefById.ContainsKey(s.SubjectDefinitionId))
                .GroupBy(s => s.SubjectDefinitionId)
                .OrderBy(g => subjectDefById[g.Key].DisplayOrder);

            foreach (var group in subjectGroups)
            {
                sb.AppendLine($"## {subjectDefById[group.Key].SubjectType}");
                sb.AppendLine();

                foreach (var subject in group.OrderBy(s => s.Name))
                {
                    sb.AppendLine($"### {subject.Name}");
                    sb.AppendLine();

                    var tracks = subjectTrackDefs.TryGetValue(subject.SubjectDefinitionId, out var td)
                        ? td.Where(t => config.IncludedTrackTypes.Contains(t.TrackType)).ToList()
                        : new List<NoteTrackDefinition>();

                    var ownerNotes = notesByOwner.TryGetValue((subject.Id, OwnerType.Subject), out var sn)
                        ? sn : new List<Note>();

                    RenderTrackSections(sb, tracks, ownerNotes, themeById, sourceCitationByNoteId, "####",
                        config.IncludedTrackTypes.Contains(TrackType.Unset));
                }
            }
        }

        // ---- PART 2: SCENE CONTENT ----
        var allPpIds = result.FullPlotPointIds.Union(result.ThinPlotPointIds).ToHashSet();
        if (allPpIds.Count > 0)
        {
            sb.AppendLine("---");
            sb.AppendLine();
            sb.AppendLine("# Part 2: Scene Content");
            sb.AppendLine();

            var allPps = allPpIds
                .Where(id => plotPointById.ContainsKey(id))
                .Select(id => plotPointById[id])
                .ToList();

            // PlotPoints without a chapter
            var noChapter = allPps.Where(p => p.ChapterId == null)
                .OrderBy(p => p.OrderInChapter).ToList();

            if (noChapter.Count > 0)
            {
                sb.AppendLine("## (No Chapter)");
                sb.AppendLine();
                foreach (var pp in noChapter)
                    RenderPlotPoint(sb, pp, result, config, notesByOwner, plotPointTrackDefs,
                        linkTrackDefs, subjectById, subjectDefById, themeById, sourceCitationByNoteId, linkIdByPair);
            }

            // PlotPoints grouped by chapter, ordered by (story reading order, chapter order)
            var chapterGroups = allPps
                .Where(p => p.ChapterId.HasValue && chapterById.ContainsKey(p.ChapterId.Value))
                .GroupBy(p => p.ChapterId!.Value)
                .OrderBy(g => StoryOrderOf(chapterById[g.Key]))
                .ThenBy(g => chapterById[g.Key].OrderIndex);

            foreach (var group in chapterGroups)
            {
                var chapter = chapterById[group.Key];
                sb.AppendLine($"## {StoryLabelOf(chapter)} — Chapter {chapter.OrderIndex}: {chapter.Title}");
                sb.AppendLine();
                foreach (var pp in group.OrderBy(p => p.OrderInChapter))
                    RenderPlotPoint(sb, pp, result, config, notesByOwner, plotPointTrackDefs,
                        linkTrackDefs, subjectById, subjectDefById, themeById, sourceCitationByNoteId, linkIdByPair);
            }
        }

        return sb.ToString();
    }

    // ---- Preamble ----

    private static void BuildPreamble(
        StringBuilder sb,
        ExportConfiguration config,
        IStoryService storyService,
        Dictionary<int, SubjectDefinition> subjectDefById,
        HashSet<int> subDefIdsInPart1,
        HashSet<int> subDefIdsInLinks,
        bool hasAnyPlotPoints,
        Dictionary<int, List<NoteTrackDefinition>> subjectTrackDefs,
        Dictionary<int, List<NoteTrackDefinition>> linkTrackDefs,
        List<NoteTrackDefinition> plotPointTrackDefs)
    {
        // Track Type Glossary
        sb.AppendLine("## Track Type Glossary");
        sb.AppendLine();
        foreach (var tt in Enum.GetValues<TrackType>()
                     .Where(t => t != TrackType.Unset && config.IncludedTrackTypes.Contains(t)))
        {
            sb.AppendLine($"**{tt}** — {tt.GetCognitiveMode()}");
        }
        sb.AppendLine();

        // Themes (all project themes)
        sb.AppendLine("## Themes");
        sb.AppendLine();
        foreach (var theme in storyService.Themes.OrderBy(t => t.Name))
        {
            sb.AppendLine($"### {theme.Name}");
            sb.AppendLine(theme.Proposition);
            sb.AppendLine();
        }

        // Subject Roster (all project subjects, name + type only)
        sb.AppendLine("## Subject Roster");
        sb.AppendLine();
        var rosterGroups = storyService.Subjects
            .Where(s => subjectDefById.ContainsKey(s.SubjectDefinitionId))
            .GroupBy(s => s.SubjectDefinitionId)
            .OrderBy(g => subjectDefById[g.Key].DisplayOrder);
        foreach (var group in rosterGroups)
        {
            sb.AppendLine($"**{subjectDefById[group.Key].SubjectType}**");
            foreach (var subject in group.OrderBy(s => s.Name))
                sb.AppendLine($"- {subject.Name}");
            sb.AppendLine();
        }

        // Track Definitions (filtered to subject types in content + included track types)
        sb.AppendLine("## Track Definitions");
        sb.AppendLine();

        // Subjects present in Part 1 — their project-wide tracks
        var subDefsForPart1 = subDefIdsInPart1
            .Where(id => subjectDefById.ContainsKey(id))
            .Select(id => subjectDefById[id])
            .OrderBy(sd => sd.DisplayOrder);

        foreach (var subDef in subDefsForPart1)
        {
            var tracks = subjectTrackDefs.TryGetValue(subDef.Id, out var td)
                ? td.Where(t => config.IncludedTrackTypes.Contains(t.TrackType)).ToList()
                : new List<NoteTrackDefinition>();
            if (tracks.Count == 0) continue;

            sb.AppendLine($"### {subDef.SubjectType} — Project-Wide Tracks");
            sb.AppendLine();
            foreach (var track in tracks)
            {
                sb.AppendLine($"**{track.TrackName}** — {track.TrackType.GetCognitiveMode()}");
                sb.AppendLine($"*{track.DisplayQuestion}*");
                sb.AppendLine();
            }
        }

        // Subjects present via links — their link tracks
        var subDefsForLinks = subDefIdsInLinks
            .Where(id => subjectDefById.ContainsKey(id))
            .Select(id => subjectDefById[id])
            .OrderBy(sd => sd.DisplayOrder);

        foreach (var subDef in subDefsForLinks)
        {
            var tracks = linkTrackDefs.TryGetValue(subDef.Id, out var td)
                ? td.Where(t => config.IncludedTrackTypes.Contains(t.TrackType)).ToList()
                : new List<NoteTrackDefinition>();
            if (tracks.Count == 0) continue;

            sb.AppendLine($"### {subDef.SubjectType} — Link Tracks");
            sb.AppendLine();
            foreach (var track in tracks)
            {
                sb.AppendLine($"**{track.TrackName}** — {track.TrackType.GetCognitiveMode()}");
                sb.AppendLine($"*{track.DisplayQuestion}*");
                sb.AppendLine();
            }
        }

        // Plot Point tracks
        if (hasAnyPlotPoints)
        {
            var ppTracks = plotPointTrackDefs.Where(t => config.IncludedTrackTypes.Contains(t.TrackType)).ToList();
            if (ppTracks.Count > 0)
            {
                sb.AppendLine("### Plot Point Tracks");
                sb.AppendLine();
                foreach (var track in ppTracks)
                {
                    sb.AppendLine($"**{track.TrackName}** — {track.TrackType.GetCognitiveMode()}");
                    sb.AppendLine($"*{track.DisplayQuestion}*");
                    sb.AppendLine();
                }
            }
        }
    }

    // ---- PlotPoint rendering ----

    private static void RenderPlotPoint(
        StringBuilder sb,
        PlotPoint pp,
        ExportResult result,
        ExportConfiguration config,
        Dictionary<(int, OwnerType), List<Note>> notesByOwner,
        List<NoteTrackDefinition> plotPointTrackDefs,
        Dictionary<int, List<NoteTrackDefinition>> linkTrackDefs,
        Dictionary<int, Subject> subjectById,
        Dictionary<int, SubjectDefinition> subjectDefById,
        Dictionary<int, Theme> themeById,
        Dictionary<int, string> sourceCitationByNoteId,
        Dictionary<(int, int), int> linkIdByPair)
    {
        sb.AppendLine($"### {pp.Title}");
        sb.AppendLine();

        if (pp.FocalCharacterId is int focalId)
        {
            var focalName = subjectById.TryGetValue(focalId, out var focalSubject) ? focalSubject.Name : $"subject:{focalId}?";
            sb.AppendLine($"*POV: {focalName}*");
            sb.AppendLine();
        }

        // Own tracks — full entries only
        if (result.FullPlotPointIds.Contains(pp.Id))
        {
            var ppNotes = notesByOwner.TryGetValue((pp.Id, OwnerType.PlotPoint), out var pn)
                ? pn : new List<Note>();
            var ppTracks = plotPointTrackDefs.Where(t => config.IncludedTrackTypes.Contains(t.TrackType)).ToList();
            RenderTrackSections(sb, ppTracks, ppNotes, themeById, sourceCitationByNoteId, "####",
                config.IncludedTrackTypes.Contains(TrackType.Unset));
        }

        // Link sub-sections
        var linkedSubjects = result.ActiveLinks
            .Where(l => l.PlotPointId == pp.Id)
            .Select(l => l.SubjectId)
            .Where(id => subjectById.ContainsKey(id))
            .Select(id => subjectById[id])
            .Where(s => subjectDefById.ContainsKey(s.SubjectDefinitionId))
            .OrderBy(s => subjectDefById[s.SubjectDefinitionId].DisplayOrder)
            .ThenBy(s => s.Name)
            .ToList();

        foreach (var subject in linkedSubjects)
        {
            sb.AppendLine($"#### {subject.Name}");
            sb.AppendLine();

            if (!linkIdByPair.TryGetValue((pp.Id, subject.Id), out var linkId)) continue;

            var linkNotes = notesByOwner.TryGetValue((linkId, OwnerType.PlotPointSubjectLink), out var ln)
                ? ln : new List<Note>();

            var tracks = linkTrackDefs.TryGetValue(subject.SubjectDefinitionId, out var td)
                ? td.Where(t => config.IncludedTrackTypes.Contains(t.TrackType)).ToList()
                : new List<NoteTrackDefinition>();

            RenderTrackSections(sb, tracks, linkNotes, themeById, sourceCitationByNoteId, "#####",
                config.IncludedTrackTypes.Contains(TrackType.Unset));
        }
    }

    // ---- Track section rendering ----

    private static void RenderTrackSections(
        StringBuilder sb,
        List<NoteTrackDefinition> tracks,
        List<Note> allNotes,
        Dictionary<int, Theme> themeById,
        Dictionary<int, string> sourceCitationByNoteId,
        string headingLevel,
        bool includeUnassigned = false)
    {
        foreach (var track in tracks)
        {
            var trackNotes = allNotes
                .Where(n => n.NoteTrackDefinitionId == track.Id)
                .OrderBy(n => n.SortOrder)
                .ToList();

            if (trackNotes.Count == 0) continue;

            sb.AppendLine($"{headingLevel} {track.TrackName}");
            sb.AppendLine($"*{track.TrackType.GetCognitiveMode()} — {track.DisplayQuestion}*");
            sb.AppendLine();

            foreach (var note in trackNotes)
            {
                RenderNoteAsListItem(sb, note, track, themeById, sourceCitationByNoteId);
                sb.AppendLine();
            }
        }

        if (includeUnassigned)
        {
            var unassigned = allNotes
                .Where(n => n.NoteTrackDefinitionId == null)
                .OrderBy(n => n.SortOrder)
                .ToList();

            if (unassigned.Count > 0)
            {
                sb.AppendLine($"{headingLevel} {UnassignedTrack.Definition.TrackName}");
                sb.AppendLine($"*{UnassignedTrack.Definition.DisplayQuestion}*");
                sb.AppendLine();

                foreach (var note in unassigned)
                {
                    RenderNoteAsListItem(sb, note, UnassignedTrack.Definition, themeById, sourceCitationByNoteId);
                    sb.AppendLine();
                }
            }
        }
    }

    private static void RenderNoteAsListItem(
        StringBuilder sb,
        Note note,
        NoteTrackDefinition track,
        Dictionary<int, Theme> themeById,
        Dictionary<int, string> sourceCitationByNoteId)
    {
        var lines = note.Content.Split('\n');
        sb.Append("- ");
        sb.AppendLine(lines[0].TrimEnd('\r'));
        for (int i = 1; i < lines.Length; i++)
            sb.AppendLine("  " + lines[i].TrimEnd('\r'));
        AppendNoteMetadata(sb, note, track, themeById, sourceCitationByNoteId);
    }

    // ---- Note metadata ----

    private static void AppendNoteMetadata(
        StringBuilder sb,
        Note note,
        NoteTrackDefinition track,
        Dictionary<int, Theme> themeById,
        Dictionary<int, string> sourceCitationByNoteId)
    {
        if (track.SupportsTheme)
        {
            if (note.ThemeId.HasValue && themeById.TryGetValue(note.ThemeId.Value, out var theme))
                sb.AppendLine($"  *Theme: {theme.Name}*");
            else if (note.NoteState == NoteState.Confirmed)
                sb.AppendLine("  *Theme: (no theme assigned)*");
            else
                sb.AppendLine("  *(theme not yet assigned)*");
        }

        if (track.SupportsWorldDate)
        {
            if (!string.IsNullOrWhiteSpace(note.WorldDate))
                sb.AppendLine($"  *Date: {note.WorldDate}*");
            else if (note.NoteState == NoteState.Confirmed)
                sb.AppendLine("  *Date: (no date)*");
            else
                sb.AppendLine("  *(date not yet assigned)*");
        }

        if (track.SupportsSourceMaterial)
        {
            if (sourceCitationByNoteId.TryGetValue(note.Id, out var citation) && citation.Length > 0)
                sb.AppendLine($"  *Source: {citation}*");
            else if (note.NoteState == NoteState.Confirmed)
                sb.AppendLine("  *Source: (none cited)*");
            else
                sb.AppendLine("  *(source not yet cited)*");
        }
    }

    private static string FormatCitation(
        NoteSourceReference reference,
        Dictionary<int, SourceMaterial> workById,
        Dictionary<int, SourceMaterialPart> partById)
    {
        if (!workById.TryGetValue(reference.SourceMaterialId, out var work)) return "";

        if (reference.SourceMaterialPartId is int partId && partById.TryGetValue(partId, out var part))
        {
            var partLabel = string.IsNullOrWhiteSpace(part.Name) ? part.Code : $"{part.Code} — {part.Name}";
            return $"{work.Name} · {partLabel}";
        }

        return work.Name;
    }
}
