using Microsoft.Data.Sqlite;

namespace StoryPlanner.Mcp;

/// <summary>
/// Read access to the Gemini-era corpus — the founding-era web-app conversations (Sep 2025 –
/// Jun 2026) and the curated weekly story-development reports built from them. A FIFTH corpus,
/// joined to nothing: provenance, not ground truth.
///
/// Same residency model as <see cref="SourceTextStore"/>: only a manifest (identity, labels,
/// char counts) is cached; prompt/response text and report bodies are streamed per query and
/// dropped. Invalidation is a plain (length, mtime) check.
/// </summary>
public sealed class GeminiCorpusStore
{
    public sealed record EntryManifest(
        int Id, string EntryId, string ThreadId, int ThreadPos, int ThreadSize,
        string Date, string Subject, string? Subtopic,
        string TopicLabel, string ThreadSummary, string Intent,
        string? Gem, string Title, string Type, bool IsPlanPaste,
        int PromptChars, int ResponseChars);

    public sealed record ReportManifest(int Id, string Slug, string Title, string Kind, int BodyChars);

    private readonly object _gate = new();
    private readonly string? _path;
    private IReadOnlyList<EntryManifest>? _entries;
    private IReadOnlyList<ReportManifest>? _reports;
    private (long Length, DateTime MTimeUtc) _stamp;

    public GeminiCorpusStore(string? path) => _path = path;

    public bool IsConfigured => !string.IsNullOrWhiteSpace(_path) && File.Exists(_path);

    public string NotConfiguredMessage => string.IsNullOrWhiteSpace(_path)
        ? "No Gemini corpus configured — set STORYPLAN_GEMINI_CORPUS to the gemini.db path. " +
          "The founding-era conversations are unavailable; all other tools work unchanged."
        : $"Gemini corpus not found at \"{_path}\" — run the StoryPlanner.GeminiCorpus ingest to build it.";

    // ── Manifest ──────────────────────────────────────────────────────────────

    public IReadOnlyList<EntryManifest> Entries() { EnsureManifest(); return _entries!; }
    public IReadOnlyList<ReportManifest> Reports() { EnsureManifest(); return _reports!; }

    private void EnsureManifest()
    {
        lock (_gate)
        {
            var info = new FileInfo(_path!);
            var stamp = (info.Length, info.LastWriteTimeUtc);
            if (_entries is not null && _reports is not null && stamp == _stamp) return;

            var entries = new List<EntryManifest>();
            var reports = new List<ReportManifest>();

            using var conn = OpenReadOnly();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT Id, EntryId, ThreadId, ThreadPos, ThreadSize, Date, Subject, Subtopic, " +
                    "TopicLabel, ThreadSummary, Intent, Gem, Title, Type, IsPlanPaste, PromptChars, ResponseChars " +
                    "FROM Entries ORDER BY Date, Id";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    entries.Add(new EntryManifest(
                        r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetInt32(3), r.GetInt32(4),
                        r.GetString(5), r.GetString(6),
                        r.IsDBNull(7) ? null : r.GetString(7),
                        r.GetString(8), r.GetString(9), r.GetString(10),
                        r.IsDBNull(11) ? null : r.GetString(11),
                        r.GetString(12), r.GetString(13), r.GetInt32(14) != 0,
                        r.GetInt32(15), r.GetInt32(16)));
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Slug, Title, Kind, BodyChars FROM Reports ORDER BY Slug";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    reports.Add(new ReportManifest(
                        r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetInt32(4)));
            }

            _entries = entries;
            _reports = reports;
            _stamp = stamp;
        }
    }

    // ── Streaming (for search) ────────────────────────────────────────────────

    public IEnumerable<(EntryManifest Entry, string Prompt, string Response)> StreamEntries(string? subtopic)
    {
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
        {
            var entry = new EntryManifest(
                r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetInt32(3), r.GetInt32(4),
                r.GetString(5), r.GetString(6),
                r.IsDBNull(7) ? null : r.GetString(7),
                r.GetString(8), r.GetString(9), r.GetString(10),
                r.IsDBNull(11) ? null : r.GetString(11),
                r.GetString(12), r.GetString(13), r.GetInt32(14) != 0,
                r.GetInt32(15), r.GetInt32(16));
            yield return (entry, r.GetString(17), r.GetString(18));
        }
    }

    public IEnumerable<(ReportManifest Report, string Body)> StreamReports()
    {
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Slug, Title, Kind, BodyChars, Body FROM Reports ORDER BY Slug";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            var report = new ReportManifest(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetInt32(4));
            yield return (report, r.GetString(5));
        }
    }

    // ── Windowed fetch ────────────────────────────────────────────────────────

    public (EntryManifest Entry, string Prompt, string Response)? FetchEntry(int id)
    {
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT Id, EntryId, ThreadId, ThreadPos, ThreadSize, Date, Subject, Subtopic, " +
            "TopicLabel, ThreadSummary, Intent, Gem, Title, Type, IsPlanPaste, PromptChars, ResponseChars, " +
            "Prompt, Response FROM Entries WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        var entry = new EntryManifest(
            r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetInt32(3), r.GetInt32(4),
            r.GetString(5), r.GetString(6),
            r.IsDBNull(7) ? null : r.GetString(7),
            r.GetString(8), r.GetString(9), r.GetString(10),
            r.IsDBNull(11) ? null : r.GetString(11),
            r.GetString(12), r.GetString(13), r.GetInt32(14) != 0,
            r.GetInt32(15), r.GetInt32(16));
        return (entry, r.GetString(17), r.GetString(18));
    }

    public (ReportManifest Report, string Body)? FetchReport(int id)
    {
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Slug, Title, Kind, BodyChars, Body FROM Reports WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (new ReportManifest(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetInt32(4)),
            r.GetString(5));
    }

    public (ReportManifest Report, string Body)? FetchReportBySlug(string slug)
    {
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Slug, Title, Kind, BodyChars, Body FROM Reports WHERE Slug = $slug COLLATE NOCASE";
        cmd.Parameters.AddWithValue("$slug", slug);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (new ReportManifest(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetInt32(4)),
            r.GetString(5));
    }

    // ── Connection ────────────────────────────────────────────────────────────

    private SqliteConnection OpenReadOnly()
    {
        var conn = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = _path,
            Mode = SqliteOpenMode.ReadOnly,
            Pooling = false
        }.ToString());
        conn.Open();
        return conn;
    }
}
