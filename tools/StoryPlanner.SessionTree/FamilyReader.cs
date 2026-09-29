using System.Globalization;
using System.Text.Json;
using Microsoft.Data.Sqlite;

namespace StoryPlanner.SessionTree;

public sealed record SessionRecord(string Uuid, string? ParentUuid, int Seq, string Timestamp, string Role, string Body)
{
    /// <summary>
    /// What a fork preserves when it copies history: everything but the UUIDs, which it
    /// re-issues. The same record in two forks therefore shares this triple and nothing else.
    /// </summary>
    public (string Timestamp, string Role, string Body) Key => (Timestamp, Role, Body);
}

public sealed record SessionInfo(
    string SessionId,
    string ProjectDir,
    string? Title,
    string FirstTimestamp,
    string LastTimestamp,
    int ExtractVersion,
    string SourceMtimeUtc,
    IReadOnlyList<SessionRecord> Records);

public sealed record SubagentInfo(
    string SessionId,
    string ParentSessionId,
    string FirstTimestamp,
    string LastTimestamp,
    IReadOnlyList<SessionRecord> Records)
{
    /// <summary>The id a hand-back names: the session stem without its "agent-" prefix.</summary>
    public string AgentId => SessionId.StartsWith("agent-", StringComparison.Ordinal) ? SessionId[6..] : SessionId;
}

/// <summary>Every main session that shares one first record, with their subagents.</summary>
public sealed record Family(IReadOnlyList<SessionInfo> Members, IReadOnlyList<SubagentInfo> Subagents, string LastIngestUtc)
{
    public SessionRecord Root => Members[0].Records[0];
}

/// <summary>What the disk says about a family the db holds — the archive is only as current as its last ingest.</summary>
public sealed record DiskStatus(IReadOnlyDictionary<string, string> MemberStatus, IReadOnlyList<string> UningestedForks);

/// <summary>
/// Reads a fork family out of codesessions.db. A fork is a new session file whose history is a
/// copy of its parent's, so every member of a family has the same first record — the same
/// (Timestamp, Role, Body), under a different Uuid. That first record is the family's identity:
/// any member names it, and forks of forks are found with no walk.
/// </summary>
public static class FamilyReader
{
    public const string DefaultDb = @"C:\Users\Brian\Desktop\TLTT CodeSessions.db";

    public static string DefaultProjectsRoot =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude", "projects");

    /// <summary>Read-only: an ingest may be writing the archive while this reads it.</summary>
    public static SqliteConnection Open(string path)
    {
        var conn = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = path,
            Mode = SqliteOpenMode.ReadOnly,
            Pooling = false
        }.ToString());
        conn.Open();
        return conn;
    }

    /// <summary>The main sessions whose id starts with the given text, case-insensitively.</summary>
    public static IReadOnlyList<string> Resolve(SqliteConnection conn, string idOrPrefix) =>
        Query(conn, """
            SELECT SessionId FROM Sessions
            WHERE Kind = 'main' AND substr(lower(SessionId), 1, length($p)) = lower($p)
            ORDER BY SessionId
            """, cmd => cmd.Parameters.AddWithValue("$p", idOrPrefix), r => r.GetString(0));

    public static Family Load(SqliteConnection conn, string sessionId)
    {
        var root = Query(conn, "SELECT Timestamp, Role, Body FROM Records WHERE SessionId = $sid AND Seq = 1",
                cmd => cmd.Parameters.AddWithValue("$sid", sessionId),
                r => (Timestamp: r.GetString(0), Role: r.GetString(1), Body: r.GetString(2)))
            .SingleOrDefault();
        if (root.Body is null) throw new InvalidOperationException($"Session {sessionId} has no records.");

        var members = Query(conn, """
                SELECT s.SessionId, s.ProjectDir, s.Title, s.FirstTimestamp, s.LastTimestamp, s.ExtractVersion, s.SourceMtimeUtc
                FROM Sessions s JOIN Records r ON r.SessionId = s.SessionId AND r.Seq = 1
                WHERE s.Kind = 'main' AND r.Timestamp = $ts AND r.Role = $role AND r.Body = $body
                ORDER BY s.FirstTimestamp, s.SessionId
                """,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("$ts", root.Timestamp);
                    cmd.Parameters.AddWithValue("$role", root.Role);
                    cmd.Parameters.AddWithValue("$body", root.Body);
                },
                r => new SessionInfo(r.GetString(0), r.GetString(1), r.IsDBNull(2) ? null : r.GetString(2),
                    r.GetString(3), r.GetString(4), r.GetInt32(5), r.GetString(6), []))
            .Select(m => m with { Records = Records(conn, m.SessionId) })
            .ToList();

        var subagents = new List<SubagentInfo>();
        foreach (var m in members)
            subagents.AddRange(Query(conn, """
                    SELECT SessionId, ParentSessionId, FirstTimestamp, LastTimestamp FROM Sessions
                    WHERE Kind = 'subagent' AND ParentSessionId = $sid
                    ORDER BY FirstTimestamp, SessionId
                    """,
                    cmd => cmd.Parameters.AddWithValue("$sid", m.SessionId),
                    r => new SubagentInfo(r.GetString(0), r.GetString(1), r.GetString(2), r.GetString(3), []))
                .Select(s => s with { Records = Records(conn, s.SessionId) }));

        var lastIngest = Query(conn, "SELECT MAX(LastSeenUtc) FROM Sessions", _ => { },
            r => r.IsDBNull(0) ? "" : r.GetString(0)).Single();

        return new Family(members, subagents, lastIngest);
    }

    /// <summary>
    /// Two things the db cannot say about itself. A member whose transcript holds dialogue newer
    /// than the archive's has turns missing here — a changed file alone is not evidence of that,
    /// because Claude Code appends non-dialogue records (session stats) to a transcript long after
    /// its last turn. And a fork made after the last ingest run is not a member at all — it is
    /// found by opening only the transcripts written since that run and comparing their first
    /// record's timestamp with the family's.
    /// </summary>
    public static DiskStatus CheckDisk(Family family, string projectsRoot)
    {
        var status = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var m in family.Members)
        {
            var path = Path.Combine(projectsRoot, m.ProjectDir, m.SessionId + ".jsonl");
            if (!File.Exists(path)) status[m.SessionId] = "archive only (aged off disk)";
            else if (File.GetLastWriteTimeUtc(path).ToString("o") == m.SourceMtimeUtc) status[m.SessionId] = "on disk, ingested";
            else if (DialogueTimestamps(path).LastOrDefault() is { } last && string.CompareOrdinal(last, m.LastTimestamp) > 0)
                status[m.SessionId] = $"{NewerDialogue} on disk, to {last}";
            else status[m.SessionId] = "changed since ingest, no newer dialogue";
        }

        var uningested = new List<string>();
        var memberIds = family.Members.Select(m => m.SessionId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var since = DateTime.TryParse(family.LastIngestUtc, CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var t) ? t : DateTime.MinValue;
        foreach (var dir in family.Members.Select(m => Path.Combine(projectsRoot, m.ProjectDir)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (!Directory.Exists(dir)) continue;
            foreach (var file in Directory.GetFiles(dir, "*.jsonl", SearchOption.TopDirectoryOnly))
            {
                var stem = Path.GetFileNameWithoutExtension(file);
                if (memberIds.Contains(stem) || File.GetLastWriteTimeUtc(file) <= since) continue;
                if (DialogueTimestamps(file).FirstOrDefault() == family.Root.Timestamp) uningested.Add(stem);
            }
        }
        uningested.Sort(StringComparer.Ordinal);
        return new DiskStatus(status, uningested);
    }

    /// <summary>The opening of a member status that means the archive is missing turns.</summary>
    public const string NewerDialogue = "NEWER DIALOGUE";

    /// <summary>The timestamps of a transcript's user and assistant records, in file order, read lazily.</summary>
    private static IEnumerable<string> DialogueTimestamps(string path)
    {
        FileStream stream;
        try
        {
            stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        }
        catch (IOException)
        {
            yield break; // locked or vanished between listing and opening: nothing this run can see
        }

        using var reader = new StreamReader(stream);
        while (reader.ReadLine() is { } line)
        {
            if (line.Length == 0) continue;
            string? ts = null;
            try
            {
                using var doc = JsonDocument.Parse(line);
                var r = doc.RootElement;
                if (r.ValueKind == JsonValueKind.Object &&
                    r.TryGetProperty("type", out var type) && type.GetString() is "user" or "assistant" &&
                    r.TryGetProperty("timestamp", out var t) && t.ValueKind == JsonValueKind.String)
                    ts = t.GetString();
            }
            catch (JsonException)
            {
                // A live session appends in place; a torn line is not this reader's concern.
            }
            if (ts is not null) yield return ts;
        }
    }

    private static List<SessionRecord> Records(SqliteConnection conn, string sessionId) =>
        Query(conn, """
            SELECT Uuid, ParentUuid, Seq, Timestamp, Role, Body FROM Records
            WHERE SessionId = $sid ORDER BY Seq
            """,
            cmd => cmd.Parameters.AddWithValue("$sid", sessionId),
            r => new SessionRecord(r.GetString(0), r.IsDBNull(1) ? null : r.GetString(1), r.GetInt32(2),
                r.GetString(3), r.GetString(4), r.GetString(5)));

    private static List<T> Query<T>(SqliteConnection conn, string sql, Action<SqliteCommand> bind, Func<SqliteDataReader, T> map)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        bind(cmd);
        using var r = cmd.ExecuteReader();
        var list = new List<T>();
        while (r.Read()) list.Add(map(r));
        return list;
    }
}
