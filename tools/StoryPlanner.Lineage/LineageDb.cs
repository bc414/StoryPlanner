using Microsoft.Data.Sqlite;

namespace StoryPlanner.Lineage;

/// <summary>
/// Creates and writes the AI Studio and NotebookLM tables of lineage.db — the single physical
/// database behind the MCP server's LINEAGE corpus. The Gemini web-app layer's tables
/// (Entries/Reports) are owned and written by tools/StoryPlanner.GeminiCorpus, pointed at the
/// same file; each ingest replaces only its own tables, so the three sources can be re-ingested
/// independently.
///
/// Same shape as GeminiCorpusDb / SourceTextDb: no EF, no migrations, hand-written DDL run
/// idempotently on every open. The identity indexes exist because this file IS queried per call
/// (the .storyplan "no indexes" rule rests on nothing querying it after load — untrue here).
///
/// IngestRuns is the shared disclosure ledger: every ingest (including GeminiCorpus) appends a
/// row per --apply run, so list_lineage can distinguish "never ingested" from "ingested, zero
/// rows" — the one thing a single-file corpus cannot otherwise say.
/// </summary>
public static class LineageDb
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
            CREATE TABLE IF NOT EXISTS AiStudioChats (
                Id                INTEGER PRIMARY KEY AUTOINCREMENT,
                ChatKey           TEXT NOT NULL,
                Title             TEXT NOT NULL,
                Date              TEXT NOT NULL,
                Model             TEXT,
                SystemInstruction TEXT NOT NULL,
                SystemChars       INTEGER NOT NULL,
                TurnCount         INTEGER NOT NULL,
                TotalChars        INTEGER NOT NULL
            );
            """);
        Execute(conn, "CREATE UNIQUE INDEX IF NOT EXISTS IX_AiStudioChats_ChatKey ON AiStudioChats (ChatKey);");

        Execute(conn, """
            CREATE TABLE IF NOT EXISTS AiStudioTurns (
                Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                ChatKey       TEXT NOT NULL,
                TurnIndex     INTEGER NOT NULL,
                Role          TEXT NOT NULL,
                CreateTime    TEXT,
                IsPlaceholder INTEGER NOT NULL DEFAULT 0,
                Body          TEXT NOT NULL,
                BodyChars     INTEGER NOT NULL
            );
            """);
        Execute(conn, "CREATE UNIQUE INDEX IF NOT EXISTS IX_AiStudioTurns_Identity ON AiStudioTurns (ChatKey, TurnIndex);");
        Execute(conn, "CREATE INDEX IF NOT EXISTS IX_AiStudioTurns_ChatKey ON AiStudioTurns (ChatKey);");

        Execute(conn, """
            CREATE TABLE IF NOT EXISTS NlmNotebooks (
                Id           INTEGER PRIMARY KEY AUTOINCREMENT,
                Slug         TEXT NOT NULL,
                Title        TEXT NOT NULL,
                AuthoredDate TEXT,
                CaptureFile  TEXT NOT NULL,
                CapturedUtc  TEXT NOT NULL,
                TurnCount    INTEGER NOT NULL,
                NoteCount    INTEGER NOT NULL,
                TotalChars   INTEGER NOT NULL
            );
            """);
        Execute(conn, "CREATE UNIQUE INDEX IF NOT EXISTS IX_NlmNotebooks_Slug ON NlmNotebooks (Slug);");

        Execute(conn, """
            CREATE TABLE IF NOT EXISTS NlmTurns (
                Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                Slug      TEXT NOT NULL,
                TurnIndex INTEGER NOT NULL,
                Role      TEXT NOT NULL,
                Body      TEXT NOT NULL,
                BodyChars INTEGER NOT NULL
            );
            """);
        Execute(conn, "CREATE UNIQUE INDEX IF NOT EXISTS IX_NlmTurns_Identity ON NlmTurns (Slug, TurnIndex);");

        Execute(conn, """
            CREATE TABLE IF NOT EXISTS NlmNotes (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                Slug        TEXT NOT NULL,
                NoteIndex   INTEGER NOT NULL,
                Title       TEXT NOT NULL,
                RelativeAge TEXT NOT NULL,
                Body        TEXT NOT NULL,
                BodyChars   INTEGER NOT NULL
            );
            """);
        Execute(conn, "CREATE UNIQUE INDEX IF NOT EXISTS IX_NlmNotes_Identity ON NlmNotes (Slug, NoteIndex);");

        EnsureIngestRunsTable(conn);

        return conn;
    }

    /// <summary>Replaces the whole AI Studio layer (static corpus — wholesale, like gemini).</summary>
    public static int ReplaceAiStudio(SqliteConnection conn, IReadOnlyList<AiStudioChat> chats)
    {
        using var tx = conn.BeginTransaction();
        ExecuteTx(conn, tx, "DELETE FROM AiStudioChats;");
        ExecuteTx(conn, tx, "DELETE FROM AiStudioTurns;");

        var written = 0;
        using (var insChat = conn.CreateCommand())
        using (var insTurn = conn.CreateCommand())
        {
            insChat.Transaction = tx;
            insChat.CommandText = """
                INSERT INTO AiStudioChats (ChatKey, Title, Date, Model, SystemInstruction, SystemChars, TurnCount, TotalChars)
                VALUES ($key, $title, $date, $model, $sys, $sysChars, $turns, $chars);
                """;
            var cKey = insChat.Parameters.Add("$key", SqliteType.Text);
            var cTitle = insChat.Parameters.Add("$title", SqliteType.Text);
            var cDate = insChat.Parameters.Add("$date", SqliteType.Text);
            var cModel = insChat.Parameters.Add("$model", SqliteType.Text);
            var cSys = insChat.Parameters.Add("$sys", SqliteType.Text);
            var cSysChars = insChat.Parameters.Add("$sysChars", SqliteType.Integer);
            var cTurns = insChat.Parameters.Add("$turns", SqliteType.Integer);
            var cChars = insChat.Parameters.Add("$chars", SqliteType.Integer);

            insTurn.Transaction = tx;
            insTurn.CommandText = """
                INSERT INTO AiStudioTurns (ChatKey, TurnIndex, Role, CreateTime, IsPlaceholder, Body, BodyChars)
                VALUES ($key, $idx, $role, $time, $ph, $body, $chars);
                """;
            var tKey = insTurn.Parameters.Add("$key", SqliteType.Text);
            var tIdx = insTurn.Parameters.Add("$idx", SqliteType.Integer);
            var tRole = insTurn.Parameters.Add("$role", SqliteType.Text);
            var tTime = insTurn.Parameters.Add("$time", SqliteType.Text);
            var tPh = insTurn.Parameters.Add("$ph", SqliteType.Integer);
            var tBody = insTurn.Parameters.Add("$body", SqliteType.Text);
            var tChars = insTurn.Parameters.Add("$chars", SqliteType.Integer);

            foreach (var chat in chats)
            {
                cKey.Value = chat.ChatKey;
                cTitle.Value = chat.Title;
                cDate.Value = chat.Date;
                cModel.Value = (object?)chat.Model ?? DBNull.Value;
                cSys.Value = chat.SystemInstruction;
                cSysChars.Value = chat.SystemInstruction.Length;
                cTurns.Value = chat.Turns.Count;
                cChars.Value = chat.Turns.Sum(t => t.Body.Length);
                insChat.ExecuteNonQuery();

                foreach (var turn in chat.Turns)
                {
                    tKey.Value = chat.ChatKey;
                    tIdx.Value = turn.TurnIndex;
                    tRole.Value = turn.Role;
                    tTime.Value = (object?)turn.CreateTime ?? DBNull.Value;
                    tPh.Value = turn.IsPlaceholder ? 1 : 0;
                    tBody.Value = turn.Body;
                    tChars.Value = turn.Body.Length;
                    insTurn.ExecuteNonQuery();
                    written++;
                }
            }
        }

        tx.Commit();
        return written;
    }

    /// <summary>Replaces the whole NotebookLM layer (captures are re-parsed wholesale each run).</summary>
    public static int ReplaceNotebookLm(SqliteConnection conn, IReadOnlyList<NlmNotebook> notebooks)
    {
        using var tx = conn.BeginTransaction();
        ExecuteTx(conn, tx, "DELETE FROM NlmNotebooks;");
        ExecuteTx(conn, tx, "DELETE FROM NlmTurns;");
        ExecuteTx(conn, tx, "DELETE FROM NlmNotes;");

        var written = 0;
        using (var insNb = conn.CreateCommand())
        using (var insTurn = conn.CreateCommand())
        using (var insNote = conn.CreateCommand())
        {
            insNb.Transaction = tx;
            insNb.CommandText = """
                INSERT INTO NlmNotebooks (Slug, Title, AuthoredDate, CaptureFile, CapturedUtc, TurnCount, NoteCount, TotalChars)
                VALUES ($slug, $title, $date, $file, $captured, $turns, $notes, $chars);
                """;
            var nSlug = insNb.Parameters.Add("$slug", SqliteType.Text);
            var nTitle = insNb.Parameters.Add("$title", SqliteType.Text);
            var nDate = insNb.Parameters.Add("$date", SqliteType.Text);
            var nFile = insNb.Parameters.Add("$file", SqliteType.Text);
            var nCaptured = insNb.Parameters.Add("$captured", SqliteType.Text);
            var nTurns = insNb.Parameters.Add("$turns", SqliteType.Integer);
            var nNotes = insNb.Parameters.Add("$notes", SqliteType.Integer);
            var nChars = insNb.Parameters.Add("$chars", SqliteType.Integer);

            insTurn.Transaction = tx;
            insTurn.CommandText = """
                INSERT INTO NlmTurns (Slug, TurnIndex, Role, Body, BodyChars)
                VALUES ($slug, $idx, $role, $body, $chars);
                """;
            var tSlug = insTurn.Parameters.Add("$slug", SqliteType.Text);
            var tIdx = insTurn.Parameters.Add("$idx", SqliteType.Integer);
            var tRole = insTurn.Parameters.Add("$role", SqliteType.Text);
            var tBody = insTurn.Parameters.Add("$body", SqliteType.Text);
            var tChars = insTurn.Parameters.Add("$chars", SqliteType.Integer);

            insNote.Transaction = tx;
            insNote.CommandText = """
                INSERT INTO NlmNotes (Slug, NoteIndex, Title, RelativeAge, Body, BodyChars)
                VALUES ($slug, $idx, $title, $age, $body, $chars);
                """;
            var oSlug = insNote.Parameters.Add("$slug", SqliteType.Text);
            var oIdx = insNote.Parameters.Add("$idx", SqliteType.Integer);
            var oTitle = insNote.Parameters.Add("$title", SqliteType.Text);
            var oAge = insNote.Parameters.Add("$age", SqliteType.Text);
            var oBody = insNote.Parameters.Add("$body", SqliteType.Text);
            var oChars = insNote.Parameters.Add("$chars", SqliteType.Integer);

            foreach (var nb in notebooks)
            {
                nSlug.Value = nb.Slug;
                nTitle.Value = nb.Title;
                nDate.Value = (object?)nb.AuthoredDate ?? DBNull.Value;
                nFile.Value = nb.CaptureFile;
                nCaptured.Value = nb.CapturedUtc;
                nTurns.Value = nb.Turns.Count;
                nNotes.Value = nb.Notes.Count;
                nChars.Value = nb.Turns.Sum(t => t.Body.Length) + nb.Notes.Sum(o => o.Body.Length);
                insNb.ExecuteNonQuery();

                foreach (var turn in nb.Turns)
                {
                    tSlug.Value = nb.Slug;
                    tIdx.Value = turn.TurnIndex;
                    tRole.Value = turn.Role;
                    tBody.Value = turn.Body;
                    tChars.Value = turn.Body.Length;
                    insTurn.ExecuteNonQuery();
                    written++;
                }

                foreach (var note in nb.Notes)
                {
                    oSlug.Value = nb.Slug;
                    oIdx.Value = note.NoteIndex;
                    oTitle.Value = note.Title;
                    oAge.Value = note.RelativeAge;
                    oBody.Value = note.Body;
                    oChars.Value = note.Body.Length;
                    insNote.ExecuteNonQuery();
                    written++;
                }
            }
        }

        tx.Commit();
        return written;
    }

    /// <summary>Appends one row to the shared disclosure ledger. Every --apply run records one.</summary>
    public static void RecordIngestRun(SqliteConnection conn, string source, int rows)
    {
        EnsureIngestRunsTable(conn);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO IngestRuns (Source, RunUtc, Rows) VALUES ($s, $t, $r);";
        cmd.Parameters.AddWithValue("$s", source);
        cmd.Parameters.AddWithValue("$t", DateTime.UtcNow.ToString("o"));
        cmd.Parameters.AddWithValue("$r", rows);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Also called by the GeminiCorpus ingest (via its own copy of this DDL) — the table must be
    /// creatable by whichever ingest reaches the file first.
    /// </summary>
    public static void EnsureIngestRunsTable(SqliteConnection conn)
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
