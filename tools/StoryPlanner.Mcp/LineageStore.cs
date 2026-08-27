using Microsoft.Data.Sqlite;

namespace StoryPlanner.Mcp;

/// <summary>
/// Read access to the LINEAGE corpus — one physical database (lineage.db) holding four
/// source layers as parallel shelves: the pre-AI Google Doc revision history (written by
/// tools/StoryPlanner.GDocHistory), the Gemini web-app conversations and their curated reports
/// (written by tools/StoryPlanner.GeminiCorpus), the never-imported AI Studio chats, and the
/// NotebookLM captures (both written by tools/StoryPlanner.Lineage). One corpus, four source
/// layers, still joined to nothing — provenance, not ground truth.
///
/// Same residency model as <see cref="SourceTextStore"/>: only manifests (identity, labels,
/// char counts) are cached; bodies are streamed per query and dropped. Invalidation is a plain
/// (length, mtime) check — this file is only ever rewritten wholesale by its ingests.
///
/// A missing table is a legal state, not an error: each ingest creates only its own tables, so
/// a lineage.db that gemini has written but the aistudio/notebooklm ingest has not yet touched
/// simply reports those layers as never ingested (the IngestRuns ledger is what distinguishes
/// "never ingested" from "ingested, zero rows").
/// </summary>
public sealed class LineageStore(string? path)
{
    public sealed record GeminiEntryManifest(
        int Id, string EntryId, string ThreadId, int ThreadPos, int ThreadSize,
        string Date, string Subject, string? Subtopic,
        string TopicLabel, string ThreadSummary, string Intent,
        string? Gem, string Title, string Type, bool IsPlanPaste,
        int PromptChars, int ResponseChars);

    public sealed record ReportManifest(int Id, string Slug, string Title, string Kind, int BodyChars);

    public sealed record AiChatManifest(
        int Id, string ChatKey, string Title, string Date, string? Model,
        int SystemChars, int TurnCount, int TotalChars);

    public sealed record AiTurn(int TurnIndex, string Role, string? CreateTime, bool IsPlaceholder, string Body);

    public sealed record NlmNotebookManifest(
        int Id, string Slug, string Title, string? AuthoredDate, string CaptureFile,
        string CapturedUtc, int TurnCount, int NoteCount, int TotalChars);

    public sealed record NlmNoteManifest(int Id, string Slug, int NoteIndex, string Title, string RelativeAge, int BodyChars);

    public sealed record IngestRun(string Source, string RunUtc, long Rows);

    public sealed record GDocDiffManifest(int Id, string Date, string FromDate, int BodyChars, int LinesAdded, int LinesRemoved, int DeltaBytes);
    public sealed record GDocSnapshotManifest(int Id, string Date, int BodyChars, int FileBytes, string Source);

    private readonly object _gate = new();
    private IReadOnlyList<GeminiEntryManifest>? _geminiEntries;
    private IReadOnlyList<ReportManifest>? _reports;
    private IReadOnlyList<AiChatManifest>? _aiChats;
    private IReadOnlyList<NlmNotebookManifest>? _nlmNotebooks;
    private IReadOnlyList<NlmNoteManifest>? _nlmNotes;
    private IReadOnlyList<GDocDiffManifest>? _gdocDiffs;
    private IReadOnlyList<GDocSnapshotManifest>? _gdocSnapshots;
    private IReadOnlyDictionary<string, IngestRun>? _latestRuns;
    private bool _hasGemini, _hasAiStudio, _hasNlm, _hasGDoc;
    private (long Length, DateTime MTimeUtc) _stamp;

    public bool IsConfigured => !string.IsNullOrWhiteSpace(path) && File.Exists(path);

    public string NotConfiguredMessage => string.IsNullOrWhiteSpace(path)
        ? "No lineage corpus configured — set STORYPLAN_LINEAGE to the lineage.db path. " +
          "The founding-era chats (Gemini web app, AI Studio, NotebookLM) are unavailable; " +
          "all other tools work unchanged."
        : $"Lineage corpus not found at \"{path}\" — run the StoryPlanner.GeminiCorpus and " +
          "StoryPlanner.Lineage ingests to build it.";

    // ── Manifest ──────────────────────────────────────────────────────────────

    public IReadOnlyList<GeminiEntryManifest> GeminiEntries() { EnsureManifest(); return _geminiEntries!; }
    public IReadOnlyList<ReportManifest> Reports() { EnsureManifest(); return _reports!; }
    public IReadOnlyList<AiChatManifest> AiChats() { EnsureManifest(); return _aiChats!; }
    public IReadOnlyList<NlmNotebookManifest> NlmNotebooks() { EnsureManifest(); return _nlmNotebooks!; }
    public IReadOnlyList<NlmNoteManifest> NlmNotes() { EnsureManifest(); return _nlmNotes!; }
    public IReadOnlyList<GDocDiffManifest> GDocDiffs() { EnsureManifest(); return _gdocDiffs!; }
    public IReadOnlyList<GDocSnapshotManifest> GDocSnapshots() { EnsureManifest(); return _gdocSnapshots!; }

    /// <summary>Latest ingest run per source ("gemini" / "aistudio" / "notebooklm" / "gdoc"), if any.</summary>
    public IReadOnlyDictionary<string, IngestRun> LatestIngestRuns() { EnsureManifest(); return _latestRuns!; }

    private void EnsureManifest()
    {
        lock (_gate)
        {
            var info = new FileInfo(path!);
            var stamp = (info.Length, info.LastWriteTimeUtc);
            if (_geminiEntries is not null && stamp == _stamp) return;

            using var conn = OpenReadOnly();

            _hasGemini = TableExists(conn, "Entries");
            _hasAiStudio = TableExists(conn, "AiStudioChats");
            _hasNlm = TableExists(conn, "NlmNotebooks");
            _hasGDoc = TableExists(conn, "GDocDiffs");

            var entries = new List<GeminiEntryManifest>();
            if (_hasGemini)
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "SELECT Id, EntryId, ThreadId, ThreadPos, ThreadSize, Date, Subject, Subtopic, " +
                    "TopicLabel, ThreadSummary, Intent, Gem, Title, Type, IsPlanPaste, PromptChars, ResponseChars " +
                    "FROM Entries ORDER BY Date, Id";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    entries.Add(ReadGeminiManifest(r));
            }

            var reports = new List<ReportManifest>();
            if (TableExists(conn, "Reports"))
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Id, Slug, Title, Kind, BodyChars FROM Reports ORDER BY Slug";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    reports.Add(new ReportManifest(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetInt32(4)));
            }

            var aiChats = new List<AiChatManifest>();
            if (_hasAiStudio)
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "SELECT Id, ChatKey, Title, Date, Model, SystemChars, TurnCount, TotalChars " +
                    "FROM AiStudioChats ORDER BY Date, Id";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    aiChats.Add(new AiChatManifest(
                        r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3),
                        r.IsDBNull(4) ? null : r.GetString(4),
                        r.GetInt32(5), r.GetInt32(6), r.GetInt32(7)));
            }

            var notebooks = new List<NlmNotebookManifest>();
            var notes = new List<NlmNoteManifest>();
            if (_hasNlm)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT Id, Slug, Title, AuthoredDate, CaptureFile, CapturedUtc, TurnCount, NoteCount, TotalChars " +
                        "FROM NlmNotebooks ORDER BY Slug";
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                        notebooks.Add(new NlmNotebookManifest(
                            r.GetInt32(0), r.GetString(1), r.GetString(2),
                            r.IsDBNull(3) ? null : r.GetString(3),
                            r.GetString(4), r.GetString(5), r.GetInt32(6), r.GetInt32(7), r.GetInt32(8)));
                }
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Slug, NoteIndex, Title, RelativeAge, BodyChars FROM NlmNotes ORDER BY Slug, NoteIndex";
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                        notes.Add(new NlmNoteManifest(
                            r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetString(3), r.GetString(4), r.GetInt32(5)));
                }
            }

            var gdocDiffs = new List<GDocDiffManifest>();
            var gdocSnapshots = new List<GDocSnapshotManifest>();
            if (_hasGDoc)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Date, FromDate, BodyChars, LinesAdded, LinesRemoved, DeltaBytes FROM GDocDiffs ORDER BY Date";
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                        gdocDiffs.Add(new GDocDiffManifest(
                            r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetInt32(3),
                            r.GetInt32(4), r.GetInt32(5), r.GetInt32(6)));
                }
                if (TableExists(conn, "GDocSnapshots"))
                {
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT Id, Date, BodyChars, FileBytes, Source FROM GDocSnapshots ORDER BY Date";
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                        gdocSnapshots.Add(new GDocSnapshotManifest(
                            r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetInt32(3), r.GetString(4)));
                }
            }

            var runs = new Dictionary<string, IngestRun>(StringComparer.OrdinalIgnoreCase);
            if (TableExists(conn, "IngestRuns"))
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Source, RunUtc, Rows FROM IngestRuns ORDER BY RunUtc";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    runs[r.GetString(0)] = new IngestRun(r.GetString(0), r.GetString(1), r.GetInt64(2));
            }
            // A pre-ledger gemini.db still counts as ingested if its tables hold rows.
            if (_hasGemini && entries.Count > 0 && !runs.ContainsKey("gemini"))
                runs["gemini"] = new IngestRun("gemini", "(before the IngestRuns ledger)", entries.Count);

            _geminiEntries = entries;
            _reports = reports;
            _aiChats = aiChats;
            _nlmNotebooks = notebooks;
            _nlmNotes = notes;
            _gdocDiffs = gdocDiffs;
            _gdocSnapshots = gdocSnapshots;
            _latestRuns = runs;
            _stamp = stamp;
        }
    }

    private static bool TableExists(SqliteConnection conn, string name)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = $n";
        cmd.Parameters.AddWithValue("$n", name);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    // ── Streaming (for search) ────────────────────────────────────────────────

    public IEnumerable<(GeminiEntryManifest Entry, string Prompt, string Response)> StreamGeminiEntries(string? subtopic)
    {
        EnsureManifest();
        if (!_hasGemini) yield break;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        var where = subtopic is not null ? "WHERE Subtopic = $sub COLLATE NOCASE " : "";
        cmd.CommandText =
            "SELECT Id, EntryId, ThreadId, ThreadPos, ThreadSize, Date, Subject, Subtopic, " +
            "TopicLabel, ThreadSummary, Intent, Gem, Title, Type, IsPlanPaste, PromptChars, ResponseChars, " +
            "Prompt, Response FROM Entries " + where + "ORDER BY Date, Id";
        if (subtopic is not null)
            cmd.Parameters.AddWithValue("$sub", subtopic);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            yield return (ReadGeminiManifest(r), r.GetString(17), r.GetString(18));
    }

    public IEnumerable<(ReportManifest Report, string Body)> StreamReports()
    {
        EnsureManifest();
        if (_reports!.Count == 0) yield break;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Slug, Title, Kind, BodyChars, Body FROM Reports ORDER BY Slug";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            yield return (new ReportManifest(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetInt32(4)),
                r.GetString(5));
    }

    public IEnumerable<(AiChatManifest Chat, AiTurn Turn)> StreamAiTurns()
    {
        EnsureManifest();
        if (!_hasAiStudio) yield break;

        var chatsByKey = _aiChats!.ToDictionary(c => c.ChatKey, StringComparer.Ordinal);
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT t.ChatKey, t.TurnIndex, t.Role, t.CreateTime, t.IsPlaceholder, t.Body " +
            "FROM AiStudioTurns t JOIN AiStudioChats c ON c.ChatKey = t.ChatKey " +
            "ORDER BY c.Date, c.Id, t.TurnIndex";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            if (!chatsByKey.TryGetValue(r.GetString(0), out var chat)) continue;
            yield return (chat, new AiTurn(
                r.GetInt32(1), r.GetString(2),
                r.IsDBNull(3) ? null : r.GetString(3),
                r.GetInt32(4) != 0, r.GetString(5)));
        }
    }

    public IEnumerable<(AiChatManifest Chat, string SystemInstruction)> StreamAiSystemInstructions()
    {
        EnsureManifest();
        if (!_hasAiStudio) yield break;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT Id, ChatKey, Title, Date, Model, SystemChars, TurnCount, TotalChars, SystemInstruction " +
            "FROM AiStudioChats ORDER BY Date, Id";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            yield return (new AiChatManifest(
                    r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3),
                    r.IsDBNull(4) ? null : r.GetString(4),
                    r.GetInt32(5), r.GetInt32(6), r.GetInt32(7)),
                r.GetString(8));
    }

    public IEnumerable<(NlmNotebookManifest Notebook, int TurnIndex, string Role, string Body)> StreamNlmTurns()
    {
        EnsureManifest();
        if (!_hasNlm) yield break;

        var bySlug = _nlmNotebooks!.ToDictionary(n => n.Slug, StringComparer.OrdinalIgnoreCase);
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Slug, TurnIndex, Role, Body FROM NlmTurns ORDER BY Slug, TurnIndex";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            if (!bySlug.TryGetValue(r.GetString(0), out var nb)) continue;
            yield return (nb, r.GetInt32(1), r.GetString(2), r.GetString(3));
        }
    }

    public IEnumerable<(NlmNoteManifest Note, string Body)> StreamNlmNotes()
    {
        EnsureManifest();
        if (!_hasNlm) yield break;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Slug, NoteIndex, Title, RelativeAge, BodyChars, Body FROM NlmNotes ORDER BY Slug, NoteIndex";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            yield return (new NlmNoteManifest(
                    r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetString(3), r.GetString(4), r.GetInt32(5)),
                r.GetString(6));
    }

    public IEnumerable<(GDocDiffManifest Diff, string Body)> StreamGDocDiffs()
    {
        EnsureManifest();
        if (!_hasGDoc) yield break;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Date, FromDate, BodyChars, LinesAdded, LinesRemoved, DeltaBytes, Body FROM GDocDiffs ORDER BY Date";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            yield return (new GDocDiffManifest(
                    r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetInt32(3),
                    r.GetInt32(4), r.GetInt32(5), r.GetInt32(6)),
                r.GetString(7));
    }

    public IEnumerable<(GDocSnapshotManifest Snapshot, string Body)> StreamGDocSnapshots()
    {
        EnsureManifest();
        if (!_hasGDoc) yield break;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Date, BodyChars, FileBytes, Source, Body FROM GDocSnapshots ORDER BY Date";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            yield return (new GDocSnapshotManifest(
                    r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetInt32(3), r.GetString(4)),
                r.GetString(5));
    }

    // ── Windowed fetch ────────────────────────────────────────────────────────

    public (GeminiEntryManifest Entry, string Prompt, string Response)? FetchGeminiEntry(int id)
    {
        EnsureManifest();
        if (!_hasGemini) return null;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT Id, EntryId, ThreadId, ThreadPos, ThreadSize, Date, Subject, Subtopic, " +
            "TopicLabel, ThreadSummary, Intent, Gem, Title, Type, IsPlanPaste, PromptChars, ResponseChars, " +
            "Prompt, Response FROM Entries WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (ReadGeminiManifest(r), r.GetString(17), r.GetString(18));
    }

    public (ReportManifest Report, string Body)? FetchReport(int id) =>
        FetchReportWhere("Id = $v", id);

    public (ReportManifest Report, string Body)? FetchReportBySlug(string slug) =>
        FetchReportWhere("Slug = $v COLLATE NOCASE", slug);

    private (ReportManifest Report, string Body)? FetchReportWhere(string where, object value)
    {
        EnsureManifest();
        if (_reports!.Count == 0) return null;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT Id, Slug, Title, Kind, BodyChars, Body FROM Reports WHERE {where}";
        cmd.Parameters.AddWithValue("$v", value);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (new ReportManifest(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetInt32(4)),
            r.GetString(5));
    }

    public (AiChatManifest Chat, IReadOnlyList<AiTurn> Turns)? FetchAiChat(int id)
    {
        EnsureManifest();
        var chat = _aiChats!.FirstOrDefault(c => c.Id == id);
        if (chat is null) return null;

        var turns = new List<AiTurn>();
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT TurnIndex, Role, CreateTime, IsPlaceholder, Body FROM AiStudioTurns " +
            "WHERE ChatKey = $key ORDER BY TurnIndex";
        cmd.Parameters.AddWithValue("$key", chat.ChatKey);
        using var r = cmd.ExecuteReader();
        while (r.Read())
            turns.Add(new AiTurn(
                r.GetInt32(0), r.GetString(1),
                r.IsDBNull(2) ? null : r.GetString(2),
                r.GetInt32(3) != 0, r.GetString(4)));
        return (chat, turns);
    }

    public (AiChatManifest Chat, string SystemInstruction)? FetchAiSystem(int id)
    {
        EnsureManifest();
        var chat = _aiChats!.FirstOrDefault(c => c.Id == id);
        if (chat is null) return null;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT SystemInstruction FROM AiStudioChats WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        return cmd.ExecuteScalar() is string s ? (chat, s) : null;
    }

    public (NlmNotebookManifest Notebook, IReadOnlyList<(int TurnIndex, string Role, string Body)> Turns)? FetchNlmNotebook(int? id, string? slug)
    {
        EnsureManifest();
        var nb = id is not null
            ? _nlmNotebooks!.FirstOrDefault(n => n.Id == id)
            : _nlmNotebooks!.FirstOrDefault(n => n.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        if (nb is null) return null;

        var turns = new List<(int, string, string)>();
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT TurnIndex, Role, Body FROM NlmTurns WHERE Slug = $slug ORDER BY TurnIndex";
        cmd.Parameters.AddWithValue("$slug", nb.Slug);
        using var r = cmd.ExecuteReader();
        while (r.Read())
            turns.Add((r.GetInt32(0), r.GetString(1), r.GetString(2)));
        return (nb, turns);
    }

    public (NlmNoteManifest Note, string Body)? FetchNlmNote(int id)
    {
        EnsureManifest();
        if (!_hasNlm) return null;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Slug, NoteIndex, Title, RelativeAge, BodyChars, Body FROM NlmNotes WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (new NlmNoteManifest(
                r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetString(3), r.GetString(4), r.GetInt32(5)),
            r.GetString(6));
    }

    public (GDocDiffManifest Diff, string Body)? FetchGDocDiff(int id)
    {
        EnsureManifest();
        if (!_hasGDoc) return null;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Date, FromDate, BodyChars, LinesAdded, LinesRemoved, DeltaBytes, Body FROM GDocDiffs WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (new GDocDiffManifest(
                r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetInt32(3),
                r.GetInt32(4), r.GetInt32(5), r.GetInt32(6)),
            r.GetString(7));
    }

    public (GDocSnapshotManifest Snapshot, string Body)? FetchGDocSnapshot(int id)
    {
        EnsureManifest();
        if (!_hasGDoc) return null;

        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Date, BodyChars, FileBytes, Source, Body FROM GDocSnapshots WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (new GDocSnapshotManifest(
                r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetInt32(3), r.GetString(4)),
            r.GetString(5));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static GeminiEntryManifest ReadGeminiManifest(SqliteDataReader r) => new(
        r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetInt32(3), r.GetInt32(4),
        r.GetString(5), r.GetString(6),
        r.IsDBNull(7) ? null : r.GetString(7),
        r.GetString(8), r.GetString(9), r.GetString(10),
        r.IsDBNull(11) ? null : r.GetString(11),
        r.GetString(12), r.GetString(13), r.GetInt32(14) != 0,
        r.GetInt32(15), r.GetInt32(16));

    private SqliteConnection OpenReadOnly()
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
}
