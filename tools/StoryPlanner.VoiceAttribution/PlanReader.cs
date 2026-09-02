using Microsoft.Data.Sqlite;

namespace StoryPlanner.VoiceAttribution;

/// <summary>
/// The rows of a <c>.storyplan</c> this tool needs, read through raw SQL with
/// <c>Mode=ReadOnly</c> — never <c>StoryService.OpenProjectAsync</c>, which migrates the schema
/// in place. Works on the v1 archive and the v2 working plan alike (same v2 schema; tracks and
/// states are carried through but not interpreted here).
/// </summary>
public sealed class PlanReader
{
    public sealed record Note(int Id, string Content, int OwnerType, int OwnerId, int State);
    public sealed record Subject(int Id, string Name, string SubjectType);
    public sealed record Chapter(int Id, string Title, int OrderIndex, int StoryId);
    public sealed record PlotPoint(int Id, string Title, int? ChapterId, int OrderInChapter);
    public sealed record Link(int Id, int PlotPointId, int SubjectId);

    public IReadOnlyList<Note> Notes { get; }
    public IReadOnlyDictionary<int, Subject> Subjects { get; }
    public IReadOnlyDictionary<int, Chapter> Chapters { get; }
    public IReadOnlyDictionary<int, PlotPoint> PlotPoints { get; }
    public IReadOnlyDictionary<int, Link> Links { get; }
    public IReadOnlyDictionary<int, string> Stories { get; }

    public const int OwnerSubject = 0, OwnerPlotPoint = 1, OwnerChapter = 2, OwnerLink = 3;

    public PlanReader(string path)
    {
        var cs = new SqliteConnectionStringBuilder { DataSource = path, Mode = SqliteOpenMode.ReadOnly }.ToString();
        using var conn = new SqliteConnection(cs);
        conn.Open();

        Notes = Query(conn, "select Id, Content, OwnerType, OwnerId, NoteState from Notes order by Id",
            r => new Note(r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetInt32(3), r.GetInt32(4)));
        Subjects = Query(conn, "select s.Id, s.Name, d.SubjectType from Subjects s left join SubjectDefinitions d on d.Id = s.SubjectDefinitionId",
            r => new Subject(r.GetInt32(0), r.GetString(1), r.IsDBNull(2) ? "" : r.GetString(2))).ToDictionary(s => s.Id);
        Chapters = Query(conn, "select Id, Title, OrderIndex, StoryId from Chapters",
            r => new Chapter(r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetInt32(3))).ToDictionary(c => c.Id);
        PlotPoints = Query(conn, "select Id, Title, ChapterId, OrderInChapter from PlotPoints",
            r => new PlotPoint(r.GetInt32(0), r.GetString(1), r.IsDBNull(2) ? null : r.GetInt32(2), r.GetInt32(3))).ToDictionary(p => p.Id);
        Links = Query(conn, "select Id, PlotPointId, SubjectId from PlotPointSubjectLinks",
            r => new Link(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2))).ToDictionary(l => l.Id);
        Stories = Query(conn, "select Id, Title from Stories", r => (r.GetInt32(0), r.GetString(1))).ToDictionary(t => t.Item1, t => t.Item2);
    }

    private static List<T> Query<T>(SqliteConnection conn, string sql, Func<SqliteDataReader, T> map)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        using var r = cmd.ExecuteReader();
        var list = new List<T>();
        while (r.Read()) list.Add(map(r));
        return list;
    }

    public string StoryName(int storyId) => storyId == 0 ? "(Unassigned)" : Stories.GetValueOrDefault(storyId, $"story:{storyId}");

    /// <summary>Chapter of a note's owner: the chapter itself, a plot point's chapter, or a link's plot point's chapter. Null for subjects.</summary>
    public Chapter? ChapterOf(Note n) => n.OwnerType switch
    {
        OwnerChapter => Chapters.GetValueOrDefault(n.OwnerId),
        OwnerPlotPoint => PlotPoints.TryGetValue(n.OwnerId, out var pp) && pp.ChapterId is int c ? Chapters.GetValueOrDefault(c) : null,
        OwnerLink => Links.TryGetValue(n.OwnerId, out var l) && PlotPoints.TryGetValue(l.PlotPointId, out var lp) && lp.ChapterId is int lc ? Chapters.GetValueOrDefault(lc) : null,
        _ => null,
    };

    public string OwnerTypeName(int ownerType) => ownerType switch
    {
        OwnerSubject => "Subject", OwnerPlotPoint => "PlotPoint", OwnerChapter => "Chapter", OwnerLink => "Link", _ => ownerType.ToString(),
    };

    public string OwnerName(Note n) => n.OwnerType switch
    {
        OwnerSubject => Subjects.TryGetValue(n.OwnerId, out var s) ? s.Name : $"subject:{n.OwnerId}",
        OwnerPlotPoint => PlotPoints.TryGetValue(n.OwnerId, out var p) ? p.Title : $"pp:{n.OwnerId}",
        OwnerChapter => Chapters.TryGetValue(n.OwnerId, out var c) ? c.Title : $"chapter:{n.OwnerId}",
        OwnerLink => Links.TryGetValue(n.OwnerId, out var l)
            ? $"{(PlotPoints.TryGetValue(l.PlotPointId, out var lp) ? lp.Title : $"pp:{l.PlotPointId}")} × {(Subjects.TryGetValue(l.SubjectId, out var ls) ? ls.Name : $"subject:{l.SubjectId}")}"
            : $"link:{n.OwnerId}",
        _ => "",
    };

    public static string StateName(int state) => state switch { 0 => "open", 1 => "flagged", 2 => "closed", _ => state.ToString() };
}
