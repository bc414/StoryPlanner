using Microsoft.Data.Sqlite;
using StoryPlanner.CodeSessions;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Fixture-tier tests for codesessions.db — a real temp SQLite file written with the
/// production Db class. The invariant that matters most: the write unit is one session, so
/// a session whose transcript has aged off disk keeps its rows through every later ingest.
/// </summary>
public class CodeSessionDbTests : IDisposable
{
    private readonly string _dir = Directory.CreateTempSubdirectory("codesessions-test").FullName;
    private readonly string _dbPath;

    public CodeSessionDbTests() => _dbPath = Path.Combine(_dir, "codesessions.db");

    private static SessionRow Row(string sessionId, string kind = "main", string? parent = null,
        long bytes = 100, string mtime = "2026-08-01T00:00:00Z") => new(
        SessionId: sessionId, ProjectDir: "Proj", Kind: kind, ParentSessionId: parent,
        Title: "A machine title", Slug: "a-slug",
        FirstTimestamp: "2026-08-01T10:00:00Z", LastTimestamp: "2026-08-01T11:00:00Z",
        RecordCount: 1, TotalChars: 10, SubagentCount: 0, MalformedLines: 0,
        SourceBytes: bytes, SourceMtimeUtc: mtime);

    private static ExtractedRecord Rec(string uuid, string body) =>
        new(uuid, null, "2026-08-01T10:00:00Z", "user", body);

    [Fact]
    public void Reingesting_one_session_replaces_only_that_sessions_rows()
    {
        using var conn = CodeSessionDb.OpenWrite(_dbPath);
        CodeSessionDb.ReplaceSession(conn, Row("session-a"), [Rec("a1", "A's original body")]);
        CodeSessionDb.ReplaceSession(conn, Row("session-b"), [Rec("b1", "B's original body")]);

        // B changes; A is untouched by B's replace.
        CodeSessionDb.ReplaceSession(conn, Row("session-b", bytes: 200, mtime: "2026-08-02T00:00:00Z"),
            [Rec("b1", "B's rewritten body"), Rec("b2", "B's new turn")]);

        Assert.Equal("A's original body", ScalarString(conn,
            "SELECT Body FROM Records WHERE SessionId = 'session-a'"));
        Assert.Equal(2, ScalarInt(conn, "SELECT COUNT(*) FROM Records WHERE SessionId = 'session-b'"));
        Assert.Equal(1, ScalarInt(conn, "SELECT COUNT(*) FROM Sessions WHERE SessionId = 'session-b'"));
    }

    [Fact]
    public void A_session_absent_from_disk_is_retained_because_nothing_ever_deletes_it()
    {
        using var conn = CodeSessionDb.OpenWrite(_dbPath);
        CodeSessionDb.ReplaceSession(conn, Row("aged-out"), [Rec("x1", "the only surviving record")]);

        // A later run sees only other sessions — the ingest touches what it finds and names
        // nothing else. There is no delete path to test the absence of; assert the rows stand
        // after unrelated writes.
        CodeSessionDb.ReplaceSession(conn, Row("still-on-disk"), [Rec("y1", "newer work")]);
        CodeSessionDb.TouchSeen(conn, "still-on-disk");

        Assert.Equal(1, ScalarInt(conn, "SELECT COUNT(*) FROM Sessions WHERE SessionId = 'aged-out'"));
        Assert.Equal("the only surviving record", ScalarString(conn,
            "SELECT Body FROM Records WHERE SessionId = 'aged-out'"));
    }

    [Fact]
    public void FirstIngestedUtc_survives_a_replace_and_LoadStamps_round_trips()
    {
        using var conn = CodeSessionDb.OpenWrite(_dbPath);
        CodeSessionDb.ReplaceSession(conn, Row("s", bytes: 100), [Rec("r1", "v1")]);
        var firstIngested = ScalarString(conn, "SELECT FirstIngestedUtc FROM Sessions WHERE SessionId = 's'");

        CodeSessionDb.ReplaceSession(conn, Row("s", bytes: 200, mtime: "2026-08-05T00:00:00Z"), [Rec("r1", "v2")]);

        Assert.Equal(firstIngested, ScalarString(conn, "SELECT FirstIngestedUtc FROM Sessions WHERE SessionId = 's'"));

        var stamps = CodeSessionDb.LoadStamps(conn);
        Assert.Equal(200, stamps["s"].SourceBytes);
        Assert.Equal("2026-08-05T00:00:00Z", stamps["s"].SourceMtimeUtc);
        Assert.Equal("Proj", stamps["s"].ProjectDir);
    }

    [Fact]
    public void A_subagent_session_carries_its_kind_and_parent()
    {
        using var conn = CodeSessionDb.OpenWrite(_dbPath);
        CodeSessionDb.ReplaceSession(conn, Row("agent-ae36de02", kind: "subagent", parent: "parent-session"),
            [Rec("s1", "subagent dialogue")]);

        Assert.Equal("subagent", ScalarString(conn, "SELECT Kind FROM Sessions WHERE SessionId = 'agent-ae36de02'"));
        Assert.Equal("parent-session", ScalarString(conn, "SELECT ParentSessionId FROM Sessions WHERE SessionId = 'agent-ae36de02'"));
    }

    private static string ScalarString(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        return (string)cmd.ExecuteScalar()!;
    }

    private static int ScalarInt(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void Dispose()
    {
        try { Directory.Delete(_dir, recursive: true); } catch (IOException) { }
    }
}
