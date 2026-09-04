using Microsoft.Data.Sqlite;

namespace StoryPlanner.CodeSessions;

public sealed record SessionRow(
    string SessionId,
    string ProjectDir,
    string Kind,
    string? ParentSessionId,
    string? Title,
    string? Slug,
    string FirstTimestamp,
    string LastTimestamp,
    int RecordCount,
    long TotalChars,
    int SubagentCount,
    int MalformedLines,
    long SourceBytes,
    string SourceMtimeUtc);

public sealed record SessionStamp(string ProjectDir, long SourceBytes, string SourceMtimeUtc);

/// <summary>
/// Creates and writes codesessions.db — the sealed-but-greppable Claude Code transcript
/// archive. Deliberately NOT served by the MCP server: its audience is future Claude Code
/// sessions, which query it directly with sqlite3 (recipes: .claude/skills/code-sessions).
///
/// The write unit is ONE SESSION (ReplaceSession), never a table — modeled on
/// SourceTextDb.Replace's per-Work scope, one level narrower. That is the property that makes
/// the ingest progressive: a session file that Claude Code's retention cleanup has deleted is
/// simply never named in a run again, so its rows persist untouched. There is no delete path
/// at all.
/// </summary>
public static class CodeSessionDb
{
    public static SqliteConnection OpenWrite(string path)
    {
        var conn = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = path,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Pooling = false
        }.ToString());
        conn.Open();

        Execute(conn, """
            CREATE TABLE IF NOT EXISTS Sessions (
                Id               INTEGER PRIMARY KEY AUTOINCREMENT,
                SessionId        TEXT NOT NULL,
                ProjectDir       TEXT NOT NULL,
                Kind             TEXT NOT NULL,
                ParentSessionId  TEXT,
                Title            TEXT,
                Slug             TEXT,
                FirstTimestamp   TEXT NOT NULL,
                LastTimestamp    TEXT NOT NULL,
                RecordCount      INTEGER NOT NULL,
                TotalChars       INTEGER NOT NULL,
                SubagentCount    INTEGER NOT NULL,
                MalformedLines   INTEGER NOT NULL DEFAULT 0,
                SourceBytes      INTEGER NOT NULL,
                SourceMtimeUtc   TEXT NOT NULL,
                FirstIngestedUtc TEXT NOT NULL,
                LastSeenUtc      TEXT NOT NULL
            );
            """);
        Execute(conn, "CREATE UNIQUE INDEX IF NOT EXISTS IX_Sessions_SessionId ON Sessions (SessionId);");

        Execute(conn, """
            CREATE TABLE IF NOT EXISTS Records (
                Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                SessionId  TEXT NOT NULL,
                Uuid       TEXT NOT NULL,
                ParentUuid TEXT,
                Seq        INTEGER NOT NULL,
                Timestamp  TEXT NOT NULL,
                Role       TEXT NOT NULL,
                Body       TEXT NOT NULL,
                BodyChars  INTEGER NOT NULL
            );
            """);
        Execute(conn, "CREATE UNIQUE INDEX IF NOT EXISTS IX_Records_Identity ON Records (SessionId, Uuid);");
        Execute(conn, "CREATE INDEX IF NOT EXISTS IX_Records_Session ON Records (SessionId, Seq);");

        AddColumnIfMissing(conn, "Sessions", "ExtractVersion", "INTEGER NOT NULL DEFAULT 1");

        return conn;
    }

    /// <summary>
    /// The extraction policy a session's rows were produced under. 1 = pre-2026-09-04, when
    /// every tool result was elided; 2 = human-authored results kept. A row can only be raised
    /// by re-extracting its transcript, so sessions that have aged off disk stay at 1 forever —
    /// the archive disclosing the limits of its own coverage, the same posture as LastSeenUtc.
    /// </summary>
    public const int CurrentExtractVersion = 2;

    /// <summary>
    /// OpenWrite is CREATE TABLE IF NOT EXISTS, so an existing database is never reshaped by it.
    /// Additive columns need this explicit, idempotent step.
    /// </summary>
    private static void AddColumnIfMissing(SqliteConnection conn, string table, string column, string decl)
    {
        using (var check = conn.CreateCommand())
        {
            check.CommandText = $"PRAGMA table_info({table})";
            using var r = check.ExecuteReader();
            while (r.Read())
                if (r.GetString(1).Equals(column, StringComparison.OrdinalIgnoreCase))
                    return;
        }
        Execute(conn, $"ALTER TABLE {table} ADD COLUMN {column} {decl};");
    }

    /// <summary>Ingest stamps for change classification, keyed by SessionId.</summary>
    public static Dictionary<string, SessionStamp> LoadStamps(SqliteConnection conn)
    {
        var stamps = new Dictionary<string, SessionStamp>(StringComparer.OrdinalIgnoreCase);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT SessionId, ProjectDir, SourceBytes, SourceMtimeUtc FROM Sessions";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            stamps[r.GetString(0)] = new SessionStamp(r.GetString(1), r.GetInt64(2), r.GetString(3));
        return stamps;
    }

    /// <summary>
    /// Replaces exactly one session's rows in one transaction. FirstIngestedUtc survives a
    /// replace (it records when the session first entered the archive, not the latest refresh).
    /// </summary>
    public static void ReplaceSession(SqliteConnection conn, SessionRow session, IReadOnlyList<ExtractedRecord> records)
    {
        using var tx = conn.BeginTransaction();
        var now = DateTime.UtcNow.ToString("o");

        string firstIngested = now;
        using (var sel = conn.CreateCommand())
        {
            sel.Transaction = tx;
            sel.CommandText = "SELECT FirstIngestedUtc FROM Sessions WHERE SessionId = $sid";
            sel.Parameters.AddWithValue("$sid", session.SessionId);
            if (sel.ExecuteScalar() is string existing && existing.Length > 0)
                firstIngested = existing;
        }

        foreach (var sql in (string[])["DELETE FROM Records WHERE SessionId = $sid;", "DELETE FROM Sessions WHERE SessionId = $sid;"])
        {
            using var del = conn.CreateCommand();
            del.Transaction = tx;
            del.CommandText = sql;
            del.Parameters.AddWithValue("$sid", session.SessionId);
            del.ExecuteNonQuery();
        }

        using (var ins = conn.CreateCommand())
        {
            ins.Transaction = tx;
            ins.CommandText = """
                INSERT INTO Sessions
                    (SessionId, ProjectDir, Kind, ParentSessionId, Title, Slug,
                     FirstTimestamp, LastTimestamp, RecordCount, TotalChars, SubagentCount,
                     MalformedLines, SourceBytes, SourceMtimeUtc, FirstIngestedUtc, LastSeenUtc,
                     ExtractVersion)
                VALUES ($sid, $proj, $kind, $parent, $title, $slug,
                        $first, $last, $count, $chars, $subs, $malformed, $bytes, $mtime, $ingested, $seen,
                        $version);
                """;
            ins.Parameters.AddWithValue("$sid", session.SessionId);
            ins.Parameters.AddWithValue("$proj", session.ProjectDir);
            ins.Parameters.AddWithValue("$kind", session.Kind);
            ins.Parameters.AddWithValue("$parent", (object?)session.ParentSessionId ?? DBNull.Value);
            ins.Parameters.AddWithValue("$title", (object?)session.Title ?? DBNull.Value);
            ins.Parameters.AddWithValue("$slug", (object?)session.Slug ?? DBNull.Value);
            ins.Parameters.AddWithValue("$first", session.FirstTimestamp);
            ins.Parameters.AddWithValue("$last", session.LastTimestamp);
            ins.Parameters.AddWithValue("$count", session.RecordCount);
            ins.Parameters.AddWithValue("$chars", session.TotalChars);
            ins.Parameters.AddWithValue("$subs", session.SubagentCount);
            ins.Parameters.AddWithValue("$malformed", session.MalformedLines);
            ins.Parameters.AddWithValue("$bytes", session.SourceBytes);
            ins.Parameters.AddWithValue("$mtime", session.SourceMtimeUtc);
            ins.Parameters.AddWithValue("$ingested", firstIngested);
            ins.Parameters.AddWithValue("$seen", now);
            ins.Parameters.AddWithValue("$version", CurrentExtractVersion);
            ins.ExecuteNonQuery();
        }

        using (var ins = conn.CreateCommand())
        {
            ins.Transaction = tx;
            ins.CommandText = """
                INSERT INTO Records (SessionId, Uuid, ParentUuid, Seq, Timestamp, Role, Body, BodyChars)
                VALUES ($sid, $uuid, $parent, $seq, $ts, $role, $body, $chars);
                """;
            var pSid = ins.Parameters.Add("$sid", SqliteType.Text);
            var pUuid = ins.Parameters.Add("$uuid", SqliteType.Text);
            var pParent = ins.Parameters.Add("$parent", SqliteType.Text);
            var pSeq = ins.Parameters.Add("$seq", SqliteType.Integer);
            var pTs = ins.Parameters.Add("$ts", SqliteType.Text);
            var pRole = ins.Parameters.Add("$role", SqliteType.Text);
            var pBody = ins.Parameters.Add("$body", SqliteType.Text);
            var pChars = ins.Parameters.Add("$chars", SqliteType.Integer);

            var seq = 0;
            foreach (var rec in records)
            {
                seq++;
                pSid.Value = session.SessionId;
                pUuid.Value = rec.Uuid;
                pParent.Value = (object?)rec.ParentUuid ?? DBNull.Value;
                pSeq.Value = seq;
                pTs.Value = rec.Timestamp;
                pRole.Value = rec.Role;
                pBody.Value = rec.Body;
                pChars.Value = rec.Body.Length;
                ins.ExecuteNonQuery();
            }
        }

        tx.Commit();
    }

    public static void DeleteSessions(SqliteConnection conn, IReadOnlyList<string> sessionIds)
    {
        if (sessionIds.Count == 0) return;
        using var tx = conn.BeginTransaction();
        foreach (var sql in (string[])["DELETE FROM Records WHERE SessionId = $sid;", "DELETE FROM Sessions WHERE SessionId = $sid;"])
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = sql;
            var p = cmd.Parameters.Add("$sid", SqliteType.Text);
            foreach (var id in sessionIds)
            {
                p.Value = id;
                cmd.ExecuteNonQuery();
            }
        }
        tx.Commit();
    }

    /// <summary>An unchanged session still gets its LastSeenUtc advanced — proof the file existed this run.</summary>
    public static void TouchSeen(SqliteConnection conn, string sessionId)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Sessions SET LastSeenUtc = $now WHERE SessionId = $sid";
        cmd.Parameters.AddWithValue("$now", DateTime.UtcNow.ToString("o"));
        cmd.Parameters.AddWithValue("$sid", sessionId);
        cmd.ExecuteNonQuery();
    }

    private static void Execute(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
