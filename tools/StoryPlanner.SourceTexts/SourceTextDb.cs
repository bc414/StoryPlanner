using Microsoft.Data.Sqlite;

namespace StoryPlanner.SourceTexts;

/// <summary>
/// Creates and writes sources.db — one table, no EF, no migrations, no AppDbContext.
///
/// Deliberately a separate file from the .storyplan: the WPF app eager-loads its whole database
/// at startup and has no use for source text, and every VACUUM INTO safety backup would carry
/// tens of megabytes of published prose for nothing. Keeping it out also means none of the
/// settled .storyplan architecture (no FKs, no indexes, load-everything) is touched.
///
/// The one index here is not a contradiction of that rule: the .storyplan's premise is that
/// nothing queries it after load, which is exactly what is NOT true of this file — it is queried
/// per call precisely so its bodies never have to be resident.
/// </summary>
public static class SourceTextDb
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
            CREATE TABLE IF NOT EXISTS SourceTexts (
                Id           INTEGER PRIMARY KEY AUTOINCREMENT,
                WorkName     TEXT NOT NULL,
                PartCode     TEXT NOT NULL,
                UnitKey      TEXT NOT NULL,
                UnitLabel    TEXT NOT NULL,
                Kind         TEXT NOT NULL,
                OrderIndex   INTEGER NOT NULL,
                Body         TEXT NOT NULL,
                SourceRef    TEXT NOT NULL,
                RetrievedUtc TEXT NOT NULL
            );
            """);
        Execute(conn, """
            CREATE UNIQUE INDEX IF NOT EXISTS IX_SourceTexts_Identity
                ON SourceTexts (WorkName, PartCode, UnitKey);
            """);
        return conn;
    }

    /// <summary>
    /// Replaces every unit belonging to the named works, inside one transaction.
    ///
    /// Replace rather than merge: a re-ingest follows a re-download, and a chapter that vanished
    /// from the source (renamed, merged, withdrawn) must vanish here too rather than linger as a
    /// row no source can account for. Works not named are left untouched, so one fic can be
    /// refreshed without touching the others.
    /// </summary>
    public static int Replace(SqliteConnection conn, IEnumerable<string> workNames, IReadOnlyList<SourceTextUnit> units)
    {
        using var tx = conn.BeginTransaction();

        foreach (var work in workNames)
        {
            using var del = conn.CreateCommand();
            del.Transaction = tx;
            del.CommandText = "DELETE FROM SourceTexts WHERE WorkName = $w";
            del.Parameters.AddWithValue("$w", work);
            del.ExecuteNonQuery();
        }

        var now = DateTime.UtcNow.ToString("o");
        var written = 0;
        using (var ins = conn.CreateCommand())
        {
            ins.Transaction = tx;
            ins.CommandText = """
                INSERT INTO SourceTexts
                    (WorkName, PartCode, UnitKey, UnitLabel, Kind, OrderIndex, Body, SourceRef, RetrievedUtc)
                VALUES ($w, $p, $u, $l, $k, $o, $b, $s, $r);
                """;
            var pw = ins.Parameters.Add("$w", SqliteType.Text);
            var pp = ins.Parameters.Add("$p", SqliteType.Text);
            var pu = ins.Parameters.Add("$u", SqliteType.Text);
            var pl = ins.Parameters.Add("$l", SqliteType.Text);
            var pk = ins.Parameters.Add("$k", SqliteType.Text);
            var po = ins.Parameters.Add("$o", SqliteType.Integer);
            var pb = ins.Parameters.Add("$b", SqliteType.Text);
            var ps = ins.Parameters.Add("$s", SqliteType.Text);
            var pr = ins.Parameters.Add("$r", SqliteType.Text);

            foreach (var u in units)
            {
                pw.Value = u.WorkName;
                pp.Value = u.PartCode;
                pu.Value = u.UnitKey;
                pl.Value = u.UnitLabel;
                pk.Value = u.Kind;
                po.Value = u.OrderIndex;
                pb.Value = u.Body;
                ps.Value = u.SourceRef;
                pr.Value = now;
                ins.ExecuteNonQuery();
                written++;
            }
        }

        tx.Commit();
        return written;
    }

    private static void Execute(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
