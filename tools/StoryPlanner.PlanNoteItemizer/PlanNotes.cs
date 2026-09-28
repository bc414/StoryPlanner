using System.Text;
using Microsoft.Data.Sqlite;
using StoryPlanner.BatchFiles;
using StoryPlanner.Core;

namespace StoryPlanner.PlanNoteItemizer;

/// <summary>
/// The rows of a working-plan .storyplan the note cut needs, and the cut itself, kept apart from
/// the CLI so it is tested directly. Owners are polymorphic (OwnerId + OwnerType) and resolved
/// here by hand; an owner that no longer resolves is named by its type and id rather than dropped.
/// Flagged notes (NoteState 1) are removed at load, so no selection and no context can reach them.
/// </summary>
public static class PlanNotes
{
    public sealed record PlanNote(int Id, string Content, int OwnerType, int OwnerId, int SortOrder, int? TrackId, int? ThemeId, string DateText);
    public sealed record Track(int Id, string Name, int TrackType, string DisplayQuestion);
    public sealed record Theme(int Id, string Name, string Proposition);
    public sealed record Subject(int Id, string Name, string SubjectType);
    public sealed record PlotPoint(int Id, string Title, int? ChapterId);
    public sealed record Chapter(int Id, string Title, int OrderIndex, int StoryId);
    public sealed record Link(int Id, int PlotPointId, int SubjectId);

    public sealed record Plan(
        IReadOnlyList<PlanNote> Notes,
        int FlaggedLeftOut,
        IReadOnlyDictionary<int, Track> Tracks,
        IReadOnlyDictionary<int, Theme> Themes,
        IReadOnlyDictionary<int, Subject> Subjects,
        IReadOnlyDictionary<int, PlotPoint> PlotPoints,
        IReadOnlyDictionary<int, Chapter> Chapters,
        IReadOnlyDictionary<int, string> Stories,
        IReadOnlyDictionary<int, Link> Links);

    public const int OwnerSubject = 0, OwnerPlotPoint = 1, OwnerChapter = 2, OwnerLink = 3;
    public const int Flagged = 1;

    /// <summary>Which notes the batch holds: every note, the tracked ones, the theme-tagged ones, or those in the named tracks.</summary>
    public sealed record Selection(string Kind, IReadOnlyList<string> Tracks)
    {
        public static readonly string[] Kinds = ["all", "tracked", "theme-tagged", "tracks"];
    }

    /// <summary>What sits beside a note: nothing, the owner's other notes, or the owner's notes in the named tracks.</summary>
    public sealed record Context(string Kind, IReadOnlyList<string> Tracks)
    {
        public static readonly string[] Kinds = ["none", "owner-notes", "owner-tracks"];
    }

    public static Plan Load(string path)
    {
        var cs = new SqliteConnectionStringBuilder { DataSource = path, Mode = SqliteOpenMode.ReadOnly }.ToString();
        using var conn = new SqliteConnection(cs);
        conn.Open();
        var all = Query(conn, """
            select Id, Content, OwnerType, OwnerId, SortOrder, NoteTrackDefinitionId, ThemeId, NoteState, WorldDate,
                   WorldDateStartYear, WorldDateStartMonth, WorldDateStartDay, WorldDateEndYear, WorldDateEndMonth, WorldDateEndDay
            from Notes
            """, r => (State: r.GetInt32(7), Note: new PlanNote(r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetInt32(3), r.GetInt32(4),
                r.IsDBNull(5) ? null : r.GetInt32(5), r.IsDBNull(6) ? null : r.GetInt32(6),
                DateText(new Note
                {
                    WorldDate = r.GetString(8),
                    WorldDateStartYear = Int(r, 9), WorldDateStartMonth = Int(r, 10), WorldDateStartDay = Int(r, 11),
                    WorldDateEndYear = Int(r, 12), WorldDateEndMonth = Int(r, 13), WorldDateEndDay = Int(r, 14),
                }))));
        return new Plan(
            all.Where(n => n.State != Flagged).Select(n => n.Note).ToList(),
            all.Count(n => n.State == Flagged),
            Query(conn, "select Id, TrackName, TrackType, DisplayQuestion from NoteTrackDefinitions",
                r => new Track(r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetString(3))).ToDictionary(t => t.Id),
            Query(conn, "select Id, Name, Proposition from Themes", r => new Theme(r.GetInt32(0), r.GetString(1), r.GetString(2))).ToDictionary(t => t.Id),
            Query(conn, "select s.Id, s.Name, coalesce(d.SubjectType, '') from Subjects s left join SubjectDefinitions d on d.Id = s.SubjectDefinitionId",
                r => new Subject(r.GetInt32(0), r.GetString(1), r.GetString(2))).ToDictionary(s => s.Id),
            Query(conn, "select Id, Title, ChapterId from PlotPoints", r => new PlotPoint(r.GetInt32(0), r.GetString(1), r.IsDBNull(2) ? null : r.GetInt32(2))).ToDictionary(p => p.Id),
            Query(conn, "select Id, Title, OrderIndex, StoryId from Chapters", r => new Chapter(r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetInt32(3))).ToDictionary(c => c.Id),
            Query(conn, "select Id, Title from Stories", r => (r.GetInt32(0), r.GetString(1))).ToDictionary(t => t.Item1, t => t.Item2),
            Query(conn, "select Id, PlotPointId, SubjectId from PlotPointSubjectLinks", r => new Link(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2))).ToDictionary(l => l.Id));
    }

    /// <summary>The note's world date in the app's notation; the legacy text when it does not convert; empty when undated.</summary>
    public static string DateText(Note n)
    {
        var date = n.EffectiveWorldDate();
        if (date is not null) return date.ToString()!;
        return n.HasAnyWorldDate() ? $"unconverted: {n.WorldDate.Trim()}" : "";
    }

    public static IReadOnlyList<PlanNote> Select(Plan plan, Selection selection)
    {
        var names = new HashSet<string>(selection.Tracks, StringComparer.OrdinalIgnoreCase);
        return plan.Notes.Where(n => selection.Kind switch
            {
                "all" => true,
                "tracked" => n.TrackId is not null,
                "theme-tagged" => n.ThemeId is not null,
                "tracks" => n.TrackId is int t && plan.Tracks.TryGetValue(t, out var tr) && names.Contains(tr.Name),
                _ => throw new ArgumentException($"unknown selection '{selection.Kind}'"),
            })
            .OrderBy(n => n.Id)
            .ToList();
    }

    public const string NoteLocatorNotation = "`note-<id>`: the Notes row id in the working-plan .storyplan the config names";
    public const string OwnerLocatorNotation =
        "`<owner>-<id>`: the owner of the notes in the working-plan .storyplan the config names, `subject`, `pp` (plot point), `chapter` or `link` (scene link) with its row id";

    /// <summary>One item per selected note, with the context the config asks for beside it.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> NoteItems(Plan plan, IReadOnlyList<PlanNote> selected, Context context)
    {
        var byOwner = plan.Notes.ToLookup(n => (n.OwnerType, n.OwnerId));
        var contextTracks = new HashSet<string>(context.Tracks, StringComparer.OrdinalIgnoreCase);
        var items = new List<ItemizerOutput.Item>();
        foreach (var n in selected)
        {
            var body = new StringBuilder()
                .Append("# working-plan note ").Append(n.Id).Append('\n')
                .Append("owner: ").Append(Owner(plan, n.OwnerType, n.OwnerId)).Append('\n');
            Describe(plan, n, body);
            body.Append('\n').Append(Content(n)).Append("\n\n");

            var beside = context.Kind switch
            {
                "none" => new List<PlanNote>(),
                "owner-notes" => byOwner[(n.OwnerType, n.OwnerId)].Where(o => o.Id != n.Id).ToList(),
                "owner-tracks" => byOwner[(n.OwnerType, n.OwnerId)]
                    .Where(o => o.Id != n.Id && o.TrackId is int t && plan.Tracks.TryGetValue(t, out var tr) && contextTracks.Contains(tr.Name)).ToList(),
                _ => throw new ArgumentException($"unknown context '{context.Kind}'"),
            };
            if (context.Kind != "none")
            {
                body.Append(context.Kind == "owner-notes"
                    ? "## The owner's other notes\n\n"
                    : $"## The owner's notes in {string.Join(", ", context.Tracks)}\n\n");
                if (beside.Count == 0) body.Append("(none)\n\n");
                foreach (var o in beside.OrderBy(o => TrackName(plan, o.TrackId)).ThenBy(o => o.SortOrder).ThenBy(o => o.Id))
                {
                    body.Append("### note ").Append(o.Id).Append(" — ").Append(TrackName(plan, o.TrackId));
                    if (o.DateText.Length > 0) body.Append(" (world date ").Append(o.DateText).Append(')');
                    body.Append("\n\n").Append(Content(o)).Append("\n\n");
                }
            }
            items.Add(new ItemizerOutput.Item($"note-{n.Id}", body.ToString().TrimEnd('\n') + "\n", $"note-{n.Id}",
                $"{OwnerShort(plan, n.OwnerType, n.OwnerId)}, {TrackName(plan, n.TrackId)}"));
        }
        return items;
    }

    /// <summary>One item per owner holding at least one selected note, the notes by track then order.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> OwnerItems(Plan plan, IReadOnlyList<PlanNote> selected)
    {
        var items = new List<ItemizerOutput.Item>();
        foreach (var g in selected.GroupBy(n => (n.OwnerType, n.OwnerId)).OrderBy(g => g.Key.OwnerType).ThenBy(g => g.Key.OwnerId))
        {
            var id = $"{OwnerSlug(g.Key.OwnerType)}-{g.Key.OwnerId}";
            var body = new StringBuilder()
                .Append("# working-plan ").Append(Owner(plan, g.Key.OwnerType, g.Key.OwnerId)).Append("\n\n");
            foreach (var n in g.OrderBy(n => TrackName(plan, n.TrackId)).ThenBy(n => n.SortOrder).ThenBy(n => n.Id))
            {
                body.Append("## note ").Append(n.Id).Append('\n');
                Describe(plan, n, body);
                body.Append('\n').Append(Content(n)).Append("\n\n");
            }
            items.Add(new ItemizerOutput.Item(id, body.ToString().TrimEnd('\n') + "\n", id,
                $"{OwnerShort(plan, g.Key.OwnerType, g.Key.OwnerId)}, {g.Count()} notes"));
        }
        return items;
    }

    /// <summary>The narrowing line: what the selection takes, and that flagged notes are never carried.</summary>
    public static string Narrowing(Selection selection, Context context)
    {
        var what = selection.Kind switch
        {
            "all" => "every note",
            "tracked" => "every note assigned to a track",
            "theme-tagged" => "every note carrying a theme tag",
            "tracks" => $"every note in a track named {string.Join(", ", selection.Tracks)}",
            _ => selection.Kind,
        };
        var beside = context.Kind == "none" ? "" : " and as context";
        return $"{what}, except the flagged notes, which are never carried, as an item{beside}";
    }

    static void Describe(Plan plan, PlanNote n, StringBuilder body)
    {
        if (n.TrackId is int t && plan.Tracks.TryGetValue(t, out var track))
        {
            var type = (TrackType)track.TrackType;
            body.Append("track: ").Append(track.Name).Append(" — ").Append(type).Append(": ").Append(Persona(type)).Append('\n');
            body.Append("display question: ").Append(track.DisplayQuestion.Trim().Length == 0 ? "(none)" : track.DisplayQuestion.Trim()).Append('\n');
        }
        else body.Append("track: none (unassigned)\n");
        if (n.ThemeId is int th)
            body.Append("theme: ").Append(plan.Themes.TryGetValue(th, out var theme) ? $"{theme.Name} — {theme.Proposition.Trim()}" : $"theme {th}").Append('\n');
        if (n.DateText.Length > 0) body.Append("world date: ").Append(n.DateText).Append('\n');
    }

    /// <summary>
    /// The mode a type declares, as the persona alone: "written by an in-universe historian
    /// reporting facts". The app's string opens with a label and, on six types, a layer number
    /// ("History Notes (Layer 2) - "), a reference the item never explains, so the item leaves it
    /// out (2026-09-27).
    /// </summary>
    public static string Persona(TrackType type)
    {
        var mode = type.GetCognitiveMode();
        var dash = mode.IndexOf(" - ", StringComparison.Ordinal);
        return dash < 0 ? mode : mode[(dash + 3)..];
    }

    static string Content(PlanNote n) => n.Content.Replace("\r\n", "\n").Trim() is { Length: > 0 } c ? c : "(empty)";

    static string TrackName(Plan plan, int? trackId) =>
        trackId is int t && plan.Tracks.TryGetValue(t, out var track) ? track.Name : "unassigned";

    static string OwnerSlug(int ownerType) => ownerType switch
    {
        OwnerSubject => "subject", OwnerPlotPoint => "pp", OwnerChapter => "chapter", OwnerLink => "link", _ => $"owner{ownerType}",
    };

    static string StoryName(Plan plan, int storyId) => storyId == 0 ? "(Unassigned)" : plan.Stories.GetValueOrDefault(storyId, $"story {storyId}");

    static string PlotPointPlace(Plan plan, PlotPoint p) =>
        p.ChapterId is int c && plan.Chapters.TryGetValue(c, out var ch)
            ? $"{StoryName(plan, ch.StoryId)}, chapter {ch.OrderIndex} \"{ch.Title}\""
            : "unplaced";

    /// <summary>The owner in full, for the item's head.</summary>
    public static string Owner(Plan plan, int ownerType, int ownerId) => ownerType switch
    {
        OwnerSubject => plan.Subjects.TryGetValue(ownerId, out var s) ? $"{s.SubjectType} \"{s.Name}\"" : $"subject {ownerId} (not found)",
        OwnerPlotPoint => plan.PlotPoints.TryGetValue(ownerId, out var p) ? $"plot point \"{p.Title}\" ({PlotPointPlace(plan, p)})" : $"plot point {ownerId} (not found)",
        OwnerChapter => plan.Chapters.TryGetValue(ownerId, out var c) ? $"chapter \"{c.Title}\" ({StoryName(plan, c.StoryId)})" : $"chapter {ownerId} (not found)",
        OwnerLink => plan.Links.TryGetValue(ownerId, out var l)
            ? $"scene link \"{(plan.PlotPoints.TryGetValue(l.PlotPointId, out var lp) ? lp.Title : $"plot point {l.PlotPointId}")}\" × " +
              $"\"{(plan.Subjects.TryGetValue(l.SubjectId, out var ls) ? ls.Name : $"subject {l.SubjectId}")}\"" +
              (plan.PlotPoints.TryGetValue(l.PlotPointId, out var lpp) ? $" ({PlotPointPlace(plan, lpp)})" : "")
            : $"scene link {ownerId} (not found)",
        _ => $"owner type {ownerType} id {ownerId}",
    };

    static string OwnerShort(Plan plan, int ownerType, int ownerId) => ownerType switch
    {
        OwnerSubject => plan.Subjects.TryGetValue(ownerId, out var s) ? s.Name : $"subject {ownerId}",
        OwnerPlotPoint => plan.PlotPoints.TryGetValue(ownerId, out var p) ? $"plot point {p.Title}" : $"plot point {ownerId}",
        OwnerChapter => plan.Chapters.TryGetValue(ownerId, out var c) ? $"chapter {c.Title}" : $"chapter {ownerId}",
        OwnerLink => plan.Links.TryGetValue(ownerId, out var l) && plan.PlotPoints.TryGetValue(l.PlotPointId, out var lp) && plan.Subjects.TryGetValue(l.SubjectId, out var ls)
            ? $"{lp.Title} × {ls.Name}" : $"link {ownerId}",
        _ => $"owner {ownerType}:{ownerId}",
    };

    static int? Int(SqliteDataReader r, int i) => r.IsDBNull(i) ? null : r.GetInt32(i);

    static List<T> Query<T>(SqliteConnection conn, string sql, Func<SqliteDataReader, T> map)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        using var r = cmd.ExecuteReader();
        var list = new List<T>();
        while (r.Read()) list.Add(map(r));
        return list;
    }
}
