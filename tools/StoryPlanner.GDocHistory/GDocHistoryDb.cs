using Microsoft.Data.Sqlite;

namespace StoryPlanner.GDocHistory;

public sealed record GDocSnapshot(string Date, string Body, int FileBytes, string Source);
public sealed record GDocDiffEntry(string Date, string FromDate, string Body, int LinesAdded, int LinesRemoved, int DeltaBytes);

public static class GDocHistoryDb
{
    public static SqliteConnection OpenWrite(string path)
    {
        var conn = new SqliteConnection($"Data Source={path}");
        conn.Open();

        Execute(conn, """
            CREATE TABLE IF NOT EXISTS GDocSnapshots (
                Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                Date      TEXT NOT NULL,
                Body      TEXT NOT NULL,
                BodyChars INTEGER NOT NULL,
                FileBytes INTEGER NOT NULL,
                Source    TEXT NOT NULL
            );
            CREATE UNIQUE INDEX IF NOT EXISTS IX_GDocSnapshots_Date ON GDocSnapshots (Date);

            CREATE TABLE IF NOT EXISTS GDocDiffs (
                Id           INTEGER PRIMARY KEY AUTOINCREMENT,
                Date         TEXT NOT NULL,
                FromDate     TEXT NOT NULL,
                Body         TEXT NOT NULL,
                BodyChars    INTEGER NOT NULL,
                LinesAdded   INTEGER NOT NULL,
                LinesRemoved INTEGER NOT NULL,
                DeltaBytes   INTEGER NOT NULL
            );
            CREATE UNIQUE INDEX IF NOT EXISTS IX_GDocDiffs_Date ON GDocDiffs (Date);
            """);

        EnsureIngestRunsTable(conn);
        return conn;
    }

    public static int ReplaceGDocHistory(SqliteConnection conn,
        IReadOnlyList<GDocSnapshot> snapshots,
        IReadOnlyList<GDocDiffEntry> diffs)
    {
        using var tx = conn.BeginTransaction();

        ExecuteTx(conn, tx, "DELETE FROM GDocSnapshots;");
        ExecuteTx(conn, tx, "DELETE FROM GDocDiffs;");

        using (var cmd = conn.CreateCommand())
        {
            cmd.Transaction = tx;
            cmd.CommandText = """
                INSERT INTO GDocSnapshots (Date, Body, BodyChars, FileBytes, Source)
                VALUES ($date, $body, $bodyChars, $fileBytes, $source)
                """;
            var pDate = cmd.Parameters.Add("$date", SqliteType.Text);
            var pBody = cmd.Parameters.Add("$body", SqliteType.Text);
            var pBodyChars = cmd.Parameters.Add("$bodyChars", SqliteType.Integer);
            var pFileBytes = cmd.Parameters.Add("$fileBytes", SqliteType.Integer);
            var pSource = cmd.Parameters.Add("$source", SqliteType.Text);

            foreach (var s in snapshots)
            {
                pDate.Value = s.Date;
                pBody.Value = s.Body;
                pBodyChars.Value = s.Body.Length;
                pFileBytes.Value = s.FileBytes;
                pSource.Value = s.Source;
                cmd.ExecuteNonQuery();
            }
        }

        using (var cmd = conn.CreateCommand())
        {
            cmd.Transaction = tx;
            cmd.CommandText = """
                INSERT INTO GDocDiffs (Date, FromDate, Body, BodyChars, LinesAdded, LinesRemoved, DeltaBytes)
                VALUES ($date, $fromDate, $body, $bodyChars, $linesAdded, $linesRemoved, $deltaBytes)
                """;
            var pDate = cmd.Parameters.Add("$date", SqliteType.Text);
            var pFromDate = cmd.Parameters.Add("$fromDate", SqliteType.Text);
            var pBody = cmd.Parameters.Add("$body", SqliteType.Text);
            var pBodyChars = cmd.Parameters.Add("$bodyChars", SqliteType.Integer);
            var pLinesAdded = cmd.Parameters.Add("$linesAdded", SqliteType.Integer);
            var pLinesRemoved = cmd.Parameters.Add("$linesRemoved", SqliteType.Integer);
            var pDeltaBytes = cmd.Parameters.Add("$deltaBytes", SqliteType.Integer);

            foreach (var d in diffs)
            {
                pDate.Value = d.Date;
                pFromDate.Value = d.FromDate;
                pBody.Value = d.Body;
                pBodyChars.Value = d.Body.Length;
                pLinesAdded.Value = d.LinesAdded;
                pLinesRemoved.Value = d.LinesRemoved;
                pDeltaBytes.Value = d.DeltaBytes;
                cmd.ExecuteNonQuery();
            }
        }

        tx.Commit();
        return snapshots.Count + diffs.Count;
    }

    public static void RecordIngestRun(SqliteConnection conn, int rows)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO IngestRuns (Source, RunUtc, Rows) VALUES ('gdoc', $runUtc, $rows)";
        cmd.Parameters.AddWithValue("$runUtc", DateTime.UtcNow.ToString("O"));
        cmd.Parameters.AddWithValue("$rows", rows);
        cmd.ExecuteNonQuery();
    }

    private static void EnsureIngestRunsTable(SqliteConnection conn)
    {
        Execute(conn, """
            CREATE TABLE IF NOT EXISTS IngestRuns (
                Id     INTEGER PRIMARY KEY AUTOINCREMENT,
                Source TEXT NOT NULL,
                RunUtc TEXT NOT NULL,
                Rows   INTEGER NOT NULL
            );
            """);
    }

    private static void Execute(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    private static void ExecuteTx(SqliteConnection conn, SqliteTransaction tx, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
