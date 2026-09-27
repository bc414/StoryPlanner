using System.Text;
using StoryPlanner.BatchFiles;
using StoryPlanner.Core;
using StoryPlanner.VoiceAttribution;

namespace StoryPlanner.V1Itemizer;

/// <summary>
/// The own-voice cut over the v1-archive and lineage corpora: one item per locus — a plot point
/// with its links' notes folded in, a subject, or a chapter — carrying only the notes the
/// attribution run credits to the author's own voice, as the <c>v1-archive-mining</c> skill's
/// label table reads them. The attribution is computed in the itemizer's own run from the archive,
/// lineage.db and the dated v1 snapshots; the sidecar <c>attribution.csv</c> is a derived artifact
/// and is never an input. <see cref="Note"/> projects an attribution row so that the selection and
/// the grouping are tested without a lineage database.
/// </summary>
public static class OwnVoice
{
    public const string LocatorNotation =
        "`pp-<id>`, `subject-<id>` or `chapter-<id>`: the PlotPoints, Subjects or Chapters row id in the v1 archive .storyplan the config names; a plot point's item carries its own notes and the notes of every link on it";

    public const string Narrowing =
        "the archive notes the attribution run credits to the author's own voice — every note whose matched source is his own prompt, whose text the dated snapshots show in the plan before the model's response, or whose match is a phrase, a single lifted sentence or nothing at all — except the flagged notes, which are never carried; a note that is a whole or framed paste of a model's text is left out";

    /// <summary>One attributed archive note, projected out of the attribution run.</summary>
    public sealed record Note(
        int Id,
        string Content,
        string OwnerTypeName,
        string OwnerName,
        string Story,
        string Chapter,
        int ChapterOrder,
        int PlotPointId,
        int OrderInChapter,
        int LinkId,
        int SubjectId,
        string State,
        string Label,
        string Role);

    /// <summary>
    /// Runs the attribution over the corpora with the VoiceAttribution tool's defaults — the
    /// thresholds the evidence set of 2026-09-02 was produced under — and projects every note,
    /// leaving out the notes of any story whose name contains <paramref name="excludeStory"/>.
    /// </summary>
    public static IReadOnlyList<Note> Attribute(string archivePath, string lineagePath, string? snapshotsDir, string? excludeStory, Action<string> log)
    {
        var rows = Attribution.RunWithDefaults(archivePath, lineagePath, snapshotsDir, log);
        return rows
            .Where(r => excludeStory is null || !r.Story.Contains(excludeStory, StringComparison.OrdinalIgnoreCase))
            .Select(Of)
            .ToList();
    }

    public static Note Of(Row r) => new(
        r.Note.Id, r.Note.Content, r.OwnerTypeName, r.OwnerName, r.Story, r.Chapter, r.ChapterOrder,
        r.PlotPointId, r.OrderInChapter, r.LinkId, r.SubjectId, PlanReader.StateName(r.Note.State), r.Label, r.Role);

    /// <summary>
    /// Whether the attribution credits this note to the author's own voice. Role is already
    /// <c>brian</c> where his own prompt matched and where PlanFirst or an echo flipped it, so the
    /// rule is that role or a label that is not a paste: a phrase, a single lifted sentence
    /// (the author's design in borrowed phrasing), no captured source, or too few words to measure.
    /// A flagged note is never carried, whatever its label.
    /// </summary>
    public static bool IsOwnVoice(Note n) =>
        n.State != "flagged"
        && (n.Role == "brian"
            || n.Label is VoiceLabel.None or VoiceLabel.Short or VoiceLabel.Phrase or VoiceLabel.Fragment);

    /// <summary>
    /// One item per locus holding at least one own-voice note. Plot points first, in chapter then
    /// position order, then subjects by id, then chapters by their order index; a locus's notes by
    /// their own id, a plot point's own notes before its links'.
    /// </summary>
    public static IReadOnlyList<ItemizerOutput.Item> Cut(IReadOnlyList<Note> notes)
    {
        var kept = notes.Where(IsOwnVoice).ToList();
        var items = new List<ItemizerOutput.Item>();

        foreach (var g in kept.Where(n => n.OwnerTypeName is "PlotPoint" or "Link" && n.PlotPointId != 0)
                     .GroupBy(n => n.PlotPointId)
                     .OrderBy(g => g.Min(n => n.ChapterOrder) == 0 ? int.MaxValue : g.Min(n => n.ChapterOrder))
                     .ThenBy(g => g.Min(n => n.OrderInChapter))
                     .ThenBy(g => g.Key))
        {
            var own = g.Where(n => n.OwnerTypeName == "PlotPoint").OrderBy(n => n.Id).ToList();
            var links = g.Where(n => n.OwnerTypeName == "Link").OrderBy(n => n.LinkId).ThenBy(n => n.Id).ToList();
            var title = own.FirstOrDefault()?.OwnerName ?? PlotPointTitle(links);
            var body = new StringBuilder()
                .Append("# v1 plot point: ").Append(title).Append('\n')
                .Append("chapter: ").Append(Place(g.First())).Append('\n')
                .Append("notes here in the author's own voice: ").Append(g.Count()).Append("\n\n");
            Section(body, "The plot point's own notes", own, n => "");
            Section(body, "The notes on its links", links, n => n.OwnerName);
            items.Add(new ItemizerOutput.Item($"pp-{g.Key}", body.ToString().TrimEnd('\n') + "\n", $"pp-{g.Key}",
                $"{Place(g.First())}: {title}, {g.Count()} notes"));
        }

        foreach (var g in kept.Where(n => n.OwnerTypeName == "Subject").GroupBy(n => n.SubjectId).OrderBy(g => g.Key))
        {
            var body = new StringBuilder()
                .Append("# v1 archive subject: ").Append(g.First().OwnerName).Append('\n')
                .Append("notes here in the author's own voice: ").Append(g.Count()).Append("\n\n");
            Section(body, "The subject's notes", g.OrderBy(n => n.Id).ToList(), n => "");
            items.Add(new ItemizerOutput.Item($"subject-{g.Key}", body.ToString().TrimEnd('\n') + "\n", $"subject-{g.Key}",
                $"{g.First().OwnerName}, {g.Count()} notes"));
        }

        foreach (var g in kept.Where(n => n.OwnerTypeName == "Chapter").GroupBy(n => n.OwnerName)
                     .OrderBy(g => g.Min(n => n.ChapterOrder)).ThenBy(g => g.Key, StringComparer.Ordinal))
        {
            var body = new StringBuilder()
                .Append("# v1 chapter: ").Append(g.Key).Append('\n')
                .Append("chapter: ").Append(Place(g.First())).Append('\n')
                .Append("notes here in the author's own voice: ").Append(g.Count()).Append("\n\n");
            Section(body, "The chapter's notes", g.OrderBy(n => n.Id).ToList(), n => "");
            items.Add(new ItemizerOutput.Item($"chapter-{ChapterSlug(g.Key, g.First().ChapterOrder)}",
                body.ToString().TrimEnd('\n') + "\n", $"chapter-{g.First().ChapterOrder}",
                $"{Place(g.First())}, {g.Count()} notes"));
        }

        return items;
    }

    static string PlotPointTitle(IReadOnlyList<Note> links) =>
        links.Count == 0 ? "(untitled)" : links[0].OwnerName.Split(" × ")[0];

    static string Place(Note n) =>
        n.Chapter.Length == 0 ? "unplaced" : n.Story.Length == 0 ? n.Chapter : $"{n.Chapter} ({n.Story})";

    /// <summary>A chapter's item id: its order index and a slug of its title, since a chapter note has no chapter row id here.</summary>
    static string ChapterSlug(string ownerName, int order)
    {
        var slug = new string(ownerName.ToLowerInvariant().Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray());
        while (slug.Contains("--")) slug = slug.Replace("--", "-");
        slug = slug.Trim('-');
        return slug.Length == 0 ? order.ToString() : $"{order}-{slug}";
    }

    static void Section(StringBuilder body, string heading, IReadOnlyList<Note> notes, Func<Note, string> where)
    {
        if (notes.Count == 0) return;
        body.Append("## ").Append(heading).Append("\n\n");
        foreach (var n in notes)
        {
            body.Append("### note ").Append(n.Id);
            var w = where(n);
            if (w.Length > 0) body.Append(" — ").Append(w);
            if (n.Label == VoiceLabel.Fragment) body.Append(" (carries a borrowed phrasing)");
            body.Append("\n\n").Append(Text(n.Content)).Append("\n\n");
        }
    }

    static string Text(string s)
    {
        var t = s.Replace("\r\n", "\n").Replace('\r', '\n').Trim();
        return t.Length == 0 ? "(no text)" : t;
    }
}
