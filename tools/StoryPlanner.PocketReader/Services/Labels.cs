using StoryPlanner.Core;

namespace StoryPlanner.PocketReader;

/// <summary>
/// Display strings resolved through PlanCache lookups. Corpus-aware where the data semantics
/// differ: the v1 archive's note states are open / flagged / closed, never "confirmed".
/// </summary>
public static class Labels
{
    public static string CorpusSlug(Corpus c) => c == Corpus.Working ? "plan" : "archive";

    public static Corpus? ParseCorpus(string slug) => slug switch
    {
        "plan" => Corpus.Working,
        "archive" => Corpus.Archive,
        _ => null
    };

    public static string CorpusName(Corpus c) => c == Corpus.Working ? "Working plan" : "v1 archive";

    public static string NoteStateLabel(Corpus corpus, NoteState state) => (corpus, state) switch
    {
        (_, NoteState.Flagged) => "flagged",
        (Corpus.Working, NoteState.Confirmed) => "confirmed",
        (Corpus.Archive, NoteState.Confirmed) => "closed",
        (Corpus.Archive, _) => "open",
        _ => "unset"
    };

    public static string StoryLabel(PlanCache c, int storyId)
    {
        if (storyId == 0 || !c.StoryById.TryGetValue(storyId, out var s)) return "(Unassigned)";
        return string.IsNullOrWhiteSpace(s.Abbreviation) ? s.Title : s.Abbreviation;
    }

    /// <summary>"TLTT CH#12", story-qualified, the same shape the MCP server emits.</summary>
    public static string ChapterLabel(PlanCache c, int chapterId) =>
        c.ChapterById.TryGetValue(chapterId, out var ch)
            ? $"{StoryLabel(c, ch.StoryId)} CH#{ch.OrderIndex}"
            : $"chapter {chapterId}";

    public static string ChapterTitle(PlanCache c, int chapterId) =>
        c.ChapterById.TryGetValue(chapterId, out var ch) ? ch.Title : "";

    public static string SubjectName(PlanCache c, int subjectId) =>
        c.SubjectById.TryGetValue(subjectId, out var s) ? s.Name : $"subject {subjectId}";

    public static string SubjectType(PlanCache c, Subject s) =>
        c.SubjectDefById.TryGetValue(s.SubjectDefinitionId, out var d) ? d.SubjectType : "";

    public static string PlotPointTitle(PlanCache c, int plotPointId) =>
        c.PlotPointById.TryGetValue(plotPointId, out var p) ? p.Title : $"scene {plotPointId}";

    public static string TheaterName(PlanCache c, int theaterId) =>
        theaterId == 0 ? "(Unplaced)" : c.TheaterById.TryGetValue(theaterId, out var t) ? t.Name : $"theater {theaterId}";

    public static NoteTrackDefinition Track(PlanCache c, int? trackId) =>
        trackId is int id && c.TrackById.TryGetValue(id, out var t) ? t : UnassignedTrack.Definition;

    public static string WorldDateText(PlanCache c, Note n)
    {
        var d = n.EffectiveWorldDate();
        if (d is null) return string.IsNullOrWhiteSpace(n.WorldDate) ? "" : $"{n.WorldDate} (unconverted)";
        var track = Track(c, n.NoteTrackDefinitionId);
        return d.Value.ToNotation(track.SupportsWorldDateEnd);
    }

    public static string FabulaDate(PlotPoint p) => p.GetFabulaDate()?.ToString() ?? "";

    public static string Citations(PlanCache c, Note n)
    {
        if (!c.SourceReferencesByNote.TryGetValue(n.Id, out var refs) || refs.Count == 0) return "";
        return string.Join("; ", refs.Select(r =>
        {
            var work = c.SourceMaterialById.TryGetValue(r.SourceMaterialId, out var w) ? w.Name : $"work {r.SourceMaterialId}";
            if (r.SourceMaterialPartId is int pid && c.SourceMaterialPartById.TryGetValue(pid, out var part))
                return $"{work} · {part.Code} {part.Name}".TrimEnd();
            return work;
        }));
    }

    /// <summary>Where a note lives, as one line, and the route that opens it (null when none).</summary>
    public static (string Text, string? Href) Owner(PlanCache c, Note n)
    {
        var slug = CorpusSlug(c.Corpus);
        switch (n.OwnerType)
        {
            case OwnerType.Subject:
                return (SubjectName(c, n.OwnerId), $"subject/{slug}/{n.OwnerId}");
            case OwnerType.PlotPoint:
                return (PlotPointTitle(c, n.OwnerId), $"plotpoint/{slug}/{n.OwnerId}");
            case OwnerType.Chapter:
                return ($"{ChapterLabel(c, n.OwnerId)} {ChapterTitle(c, n.OwnerId)}".Trim(), null);
            case OwnerType.PlotPointSubjectLink:
                if (c.LinkById.TryGetValue(n.OwnerId, out var l))
                    return ($"{PlotPointTitle(c, l.PlotPointId)} · {SubjectName(c, l.SubjectId)}", $"link/{slug}/{l.Id}");
                return ($"link {n.OwnerId}", null);
            default:
                return ($"{n.OwnerType} {n.OwnerId}", null);
        }
    }

    public static string ItemHref(ItemRef r)
    {
        var slug = CorpusSlug(r.Corpus);
        return r.Kind switch
        {
            RandomUnit.Subject => $"subject/{slug}/{r.Id}",
            RandomUnit.PlotPoint => $"plotpoint/{slug}/{r.Id}",
            RandomUnit.Link => $"link/{slug}/{r.Id}",
            _ => $"note/{slug}/{r.Id}"
        };
    }

    public static string UnitName(RandomUnit u) => u switch
    {
        RandomUnit.Note => "note",
        RandomUnit.Subject => "subject",
        RandomUnit.PlotPoint => "scene",
        RandomUnit.Link => "scene link",
        _ => u.ToString()
    };
}
