using Microsoft.Data.Sqlite;

namespace StoryPlanner.GeminiCorpus;

public static class GeminiCorpusDb
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
            CREATE TABLE IF NOT EXISTS Entries (
                Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                EntryId       TEXT NOT NULL,
                ThreadId      TEXT NOT NULL,
                ThreadPos     INTEGER NOT NULL,
                ThreadSize    INTEGER NOT NULL,
                Date          TEXT NOT NULL,
                LocalTime     TEXT NOT NULL,
                Subject       TEXT NOT NULL,
                Subtopic      TEXT,
                TopicLabel    TEXT NOT NULL,
                ThreadSummary TEXT NOT NULL,
                Intent        TEXT NOT NULL,
                Gem           TEXT,
                Title         TEXT NOT NULL,
                Prompt        TEXT NOT NULL,
                Response      TEXT NOT NULL,
                Type          TEXT NOT NULL,
                IsPlanPaste   INTEGER NOT NULL DEFAULT 0,
                PromptChars   INTEGER NOT NULL,
                ResponseChars INTEGER NOT NULL
            );
            """);
        Execute(conn, "CREATE UNIQUE INDEX IF NOT EXISTS IX_Entries_EntryId ON Entries (EntryId);");
        Execute(conn, "CREATE INDEX IF NOT EXISTS IX_Entries_ThreadId ON Entries (ThreadId);");

        Execute(conn, """
            CREATE TABLE IF NOT EXISTS Reports (
                Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                Slug      TEXT NOT NULL,
                Title     TEXT NOT NULL,
                Kind      TEXT NOT NULL,
                Body      TEXT NOT NULL,
                BodyChars INTEGER NOT NULL
            );
            """);
        Execute(conn, "CREATE UNIQUE INDEX IF NOT EXISTS IX_Reports_Slug ON Reports (Slug);");

        return conn;
    }

    public static int ReplaceEntries(SqliteConnection conn, IReadOnlyList<GeminiEntry> entries)
    {
        using var tx = conn.BeginTransaction();

        using (var del = conn.CreateCommand())
        {
            del.Transaction = tx;
            del.CommandText = "DELETE FROM Entries;";
            del.ExecuteNonQuery();
        }

        var written = 0;
        using (var ins = conn.CreateCommand())
        {
            ins.Transaction = tx;
            ins.CommandText = """
                INSERT INTO Entries
                    (EntryId, ThreadId, ThreadPos, ThreadSize, Date, LocalTime,
                     Subject, Subtopic, TopicLabel, ThreadSummary, Intent, Gem,
                     Title, Prompt, Response, Type, IsPlanPaste, PromptChars, ResponseChars)
                VALUES ($entryId, $threadId, $threadPos, $threadSize, $date, $localTime,
                        $subject, $subtopic, $topicLabel, $threadSummary, $intent, $gem,
                        $title, $prompt, $response, $type, $isPlanPaste, $promptChars, $responseChars);
                """;
            var pEntryId = ins.Parameters.Add("$entryId", SqliteType.Text);
            var pThreadId = ins.Parameters.Add("$threadId", SqliteType.Text);
            var pThreadPos = ins.Parameters.Add("$threadPos", SqliteType.Integer);
            var pThreadSize = ins.Parameters.Add("$threadSize", SqliteType.Integer);
            var pDate = ins.Parameters.Add("$date", SqliteType.Text);
            var pLocalTime = ins.Parameters.Add("$localTime", SqliteType.Text);
            var pSubject = ins.Parameters.Add("$subject", SqliteType.Text);
            var pSubtopic = ins.Parameters.Add("$subtopic", SqliteType.Text);
            var pTopicLabel = ins.Parameters.Add("$topicLabel", SqliteType.Text);
            var pThreadSummary = ins.Parameters.Add("$threadSummary", SqliteType.Text);
            var pIntent = ins.Parameters.Add("$intent", SqliteType.Text);
            var pGem = ins.Parameters.Add("$gem", SqliteType.Text);
            var pTitle = ins.Parameters.Add("$title", SqliteType.Text);
            var pPrompt = ins.Parameters.Add("$prompt", SqliteType.Text);
            var pResponse = ins.Parameters.Add("$response", SqliteType.Text);
            var pType = ins.Parameters.Add("$type", SqliteType.Text);
            var pIsPlanPaste = ins.Parameters.Add("$isPlanPaste", SqliteType.Integer);
            var pPromptChars = ins.Parameters.Add("$promptChars", SqliteType.Integer);
            var pResponseChars = ins.Parameters.Add("$responseChars", SqliteType.Integer);

            foreach (var e in entries)
            {
                pEntryId.Value = e.EntryId;
                pThreadId.Value = e.ThreadId;
                pThreadPos.Value = e.ThreadPos;
                pThreadSize.Value = e.ThreadSize;
                pDate.Value = e.Date;
                pLocalTime.Value = e.LocalTime;
                pSubject.Value = e.Subject;
                pSubtopic.Value = (object?)e.Subtopic ?? DBNull.Value;
                pTopicLabel.Value = e.TopicLabel;
                pThreadSummary.Value = e.ThreadSummary;
                pIntent.Value = e.Intent;
                pGem.Value = (object?)e.Gem ?? DBNull.Value;
                pTitle.Value = e.Title;
                pPrompt.Value = e.Prompt;
                pResponse.Value = e.Response;
                pType.Value = e.Type;
                pIsPlanPaste.Value = e.IsPlanPaste ? 1 : 0;
                pPromptChars.Value = e.PromptChars;
                pResponseChars.Value = e.ResponseChars;
                ins.ExecuteNonQuery();
                written++;
            }
        }

        tx.Commit();
        return written;
    }

    public static int ReplaceReports(SqliteConnection conn, IReadOnlyList<GeminiReport> reports)
    {
        using var tx = conn.BeginTransaction();

        using (var del = conn.CreateCommand())
        {
            del.Transaction = tx;
            del.CommandText = "DELETE FROM Reports;";
            del.ExecuteNonQuery();
        }

        var written = 0;
        using (var ins = conn.CreateCommand())
        {
            ins.Transaction = tx;
            ins.CommandText = """
                INSERT INTO Reports (Slug, Title, Kind, Body, BodyChars)
                VALUES ($slug, $title, $kind, $body, $bodyChars);
                """;
            var pSlug = ins.Parameters.Add("$slug", SqliteType.Text);
            var pTitle = ins.Parameters.Add("$title", SqliteType.Text);
            var pKind = ins.Parameters.Add("$kind", SqliteType.Text);
            var pBody = ins.Parameters.Add("$body", SqliteType.Text);
            var pBodyChars = ins.Parameters.Add("$bodyChars", SqliteType.Integer);

            foreach (var r in reports)
            {
                pSlug.Value = r.Slug;
                pTitle.Value = r.Title;
                pKind.Value = r.Kind;
                pBody.Value = r.Body;
                pBodyChars.Value = r.Body.Length;
                ins.ExecuteNonQuery();
                written++;
            }
        }

        tx.Commit();
        return written;
    }

    /// <summary>
    /// Appends this run to lineage.db's shared disclosure ledger (created here if this ingest
    /// reaches the file first — same DDL as StoryPlanner.Lineage's LineageDb, copied not shared,
    /// like the rest of the sidecar-tool boilerplate). list_lineage reads the latest row per
    /// source to distinguish "never ingested" from "ingested, zero rows".
    /// </summary>
    public static void RecordIngestRun(SqliteConnection conn, int rows)
    {
        Execute(conn, """
            CREATE TABLE IF NOT EXISTS IngestRuns (
                Id     INTEGER PRIMARY KEY AUTOINCREMENT,
                Source TEXT NOT NULL,
                RunUtc TEXT NOT NULL,
                Rows   INTEGER NOT NULL
            );
            """);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO IngestRuns (Source, RunUtc, Rows) VALUES ('gemini', $t, $r);";
        cmd.Parameters.AddWithValue("$t", DateTime.UtcNow.ToString("o"));
        cmd.Parameters.AddWithValue("$r", rows);
        cmd.ExecuteNonQuery();
    }

    private static void Execute(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
