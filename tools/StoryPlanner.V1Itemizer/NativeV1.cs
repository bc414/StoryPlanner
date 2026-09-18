using Microsoft.Data.Sqlite;

namespace StoryPlanner.V1Itemizer;

/// <summary>
/// The rows of a native-schema v1 snapshot the plot-point and theme-commentary cuts need: the
/// schema of the v1 planner before the 2026-05 conversion, with typed entities (Characters,
/// CodexEntries, Locations, Themes, Threads) and four typed link tables, each link carrying its
/// own payload text. Opened <c>immutable=1</c> and read-only: the backups are never written.
/// A link's enum values are carried as their names when set, from the v1 model's enums at the
/// last commit before the rework (f1ef2bd^); an unset value (0) is carried as nothing.
/// </summary>
public static class NativeV1
{
    public sealed record Chapter(int Id, int OrderIndex, string Title);
    public sealed record PlotPoint(int Id, string Title, string Synopsis, int? ChapterId, int OrderInChapter, string Outcome, string Stakes);
    public sealed record Entity(string Kind, int Id, string Name, string Description);
    /// <summary>One link of a plot point to an entity: its kind, the payload text, and the set enum values as "name: value" pairs.</summary>
    public sealed record Link(int PlotPointId, string Kind, int EntityId, string Text, IReadOnlyList<string> Qualifiers, int Order);

    public sealed record Plan(
        IReadOnlyList<Chapter> Chapters,
        IReadOnlyList<PlotPoint> PlotPoints,
        IReadOnlyDictionary<(string Kind, int Id), Entity> Entities,
        IReadOnlyList<Link> Links);

    public const string Character = "Character", Theme = "Theme", Thread = "Thread", Codex = "Codex entry";

    static readonly string[] CharacterRole = ["Unset", "Supporting", "Protagonist", "PointOfView", "Antagonist"];
    static readonly string[] CharacterDevImpact = ["Static", "CoreValueLearned", "CoreValueDemonstration", "CoreValueChange"];
    static readonly string[] ThemeProminence = ["Unset", "Motif", "Discussion", "Demonstration", "CentralConflict"];
    static readonly string[] CodexUsageType = ["Mentioned", "ActiveUsage", "Definition", "Subversion", "Failure"];
    static readonly string[] GoalTrajectory = ["Unset", "Stagnant", "Positive", "Negative", "Triumph", "Disaster"];
    static readonly string[] CodexCategory = ["Unset", "Nation", "MagicSystem", "Technology", "History", "Organization", "WorldRules", "SocietalDifferences", "Concept", "Backstory", "AuthorialDirectives"];

    /// <summary>"label: Name" when the value is set, nothing when it is 0; an out-of-range value is carried as its number.</summary>
    public static IEnumerable<string> Qualifier(string label, string[] names, int value)
    {
        if (value == 0) yield break;
        yield return $"{label}: {(value > 0 && value < names.Length ? names[value] : value.ToString())}";
    }

    public static Plan Load(string path)
    {
        var uri = "file:" + new Uri(Path.GetFullPath(path)).AbsolutePath + "?immutable=1";
        var cs = new SqliteConnectionStringBuilder { DataSource = uri, Mode = SqliteOpenMode.ReadOnly }.ToString();
        using var conn = new SqliteConnection(cs);
        conn.Open();

        var chapters = Query(conn, "select Id, OrderIndex, Title from Chapters",
            r => new Chapter(r.GetInt32(0), r.GetInt32(1), r.GetString(2)));
        var plotPoints = Query(conn, "select Id, Title, Synopsis, ChapterId, OrderInChapter, Outcome, Stakes from PlotPoints",
            r => new PlotPoint(r.GetInt32(0), r.GetString(1), r.GetString(2), r.IsDBNull(3) ? null : r.GetInt32(3), r.GetInt32(4), r.GetString(5), r.GetString(6)));

        var entities = new List<Entity>();
        entities.AddRange(Query(conn, "select Id, Name, Description from Characters", r => new Entity(Character, r.GetInt32(0), r.GetString(1), r.GetString(2))));
        entities.AddRange(Query(conn, "select Id, Name, Description from Themes", r => new Entity(Theme, r.GetInt32(0), r.GetString(1), r.GetString(2))));
        entities.AddRange(Query(conn, "select Id, Name, Description from Threads", r => new Entity(Thread, r.GetInt32(0), r.GetString(1), r.GetString(2))));
        entities.AddRange(Query(conn, "select Id, Title, Description, Category from CodexEntries", r =>
        {
            var category = r.GetInt32(3);
            var kind = category > 0 && category < CodexCategory.Length ? $"{Codex} ({CodexCategory[category]})" : Codex;
            return new Entity(kind, r.GetInt32(0), r.GetString(1), r.GetString(2));
        }));

        var links = new List<Link>();
        links.AddRange(Query(conn, "select PlotPointId, CharacterId, coalesce(DevelopmentNote, ''), Role, DevelopmentImpact, LogicalOrder from PlotPointCharacters",
            r => new Link(r.GetInt32(0), Character, r.GetInt32(1), r.GetString(2),
                Qualifier("role", CharacterRole, r.GetInt32(3)).Concat(Qualifier("development impact", CharacterDevImpact, r.GetInt32(4))).ToList(), r.GetInt32(5))));
        links.AddRange(Query(conn, "select PlotPointId, ThemeId, coalesce(Commentary, ''), Prominence from PlotPointThemes",
            r => new Link(r.GetInt32(0), Theme, r.GetInt32(1), r.GetString(2), Qualifier("prominence", ThemeProminence, r.GetInt32(3)).ToList(), 0)));
        links.AddRange(Query(conn, "select PlotPointId, StoryThreadId, ImpactDescription, IsPrimary, ThreadTrajectory, SortOrder from PlotPointThreads",
            r => new Link(r.GetInt32(0), Thread, r.GetInt32(1), r.GetString(2),
                (r.GetInt32(3) != 0 ? new[] { "primary" } : Array.Empty<string>()).Concat(Qualifier("trajectory", GoalTrajectory, r.GetInt32(4))).ToList(), r.GetInt32(5))));
        links.AddRange(Query(conn, "select PlotPointId, CodexEntryId, Commentary, UsageType, LogicalOrder from PlotPointCodexEntries",
            r => new Link(r.GetInt32(0), Codex, r.GetInt32(1), r.GetString(2), Qualifier("usage", CodexUsageType, r.GetInt32(3)).ToList(), r.GetInt32(4))));

        // A codex entry's kind carries its category, so links find it by (Codex, id) through a second key.
        var byKey = new Dictionary<(string, int), Entity>();
        foreach (var e in entities)
            byKey[(e.Kind.StartsWith(Codex, StringComparison.Ordinal) ? Codex : e.Kind, e.Id)] = e;
        return new Plan(chapters, plotPoints, byKey, links);
    }

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
