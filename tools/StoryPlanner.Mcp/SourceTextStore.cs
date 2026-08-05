using Microsoft.Data.Sqlite;

namespace StoryPlanner.Mcp;

/// <summary>
/// Read access to sources.db — the published source material a citation points at (episode
/// transcripts, fic chapters, game flavour text). A FOURTH corpus: it is joined to the plan only
/// through (Work Name, Part Code), the same pair a citation renders as "FiM·S3E01", and only when
/// a caller explicitly asks for it.
///
/// Unlike the .storyplan caches, bodies are NEVER held in memory. Only a manifest (identity,
/// labels, char counts) is cached; text is streamed per query and dropped. That is what keeps a
/// ~50 MB corpus from becoming ~100 MB of resident UTF-16 for the whole session, and it is why
/// this file carries an index while a .storyplan deliberately does not: the .storyplan's premise
/// is that nothing queries it after load, which is exactly what is not true here.
///
/// Invalidation is a plain (length, mtime) check rather than the .storyplan's PRAGMA
/// data_version: sources.db is not in WAL mode and is only ever rewritten wholesale by the
/// offline ingest tool, so its mtime does advance on write.
/// </summary>
public sealed class SourceTextStore
{
    public sealed record Unit(
        int Id, string WorkName, string PartCode, string UnitKey,
        string UnitLabel, string Kind, int OrderIndex, int CharCount);

    private readonly object _gate = new();
    private readonly string? _path;
    private IReadOnlyList<Unit>? _manifest;
    private (long Length, DateTime MTimeUtc) _stamp;

    public SourceTextStore(string? path) => _path = path;

    /// <summary>False when STORYPLAN_SOURCE_TEXTS is unset or the file is missing — the tools
    /// then say so rather than the server failing to start. The corpus is optional.</summary>
    public bool IsConfigured => !string.IsNullOrWhiteSpace(_path) && File.Exists(_path);

    public string NotConfiguredMessage => string.IsNullOrWhiteSpace(_path)
        ? "No source-text corpus configured — set STORYPLAN_SOURCE_TEXTS to a sources.db path. " +
          "The plan's citations still resolve without it; only the cited text is unavailable."
        : $"Source-text corpus not found at \"{_path}\" — run the StoryPlanner.SourceTexts ingest to build it.";

    public IReadOnlyList<Unit> Manifest()
    {
        lock (_gate)
        {
            var info = new FileInfo(_path!);
            var stamp = (info.Length, info.LastWriteTimeUtc);
            if (_manifest is not null && stamp == _stamp) return _manifest;

            var units = new List<Unit>();
            using var conn = OpenReadOnly();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "SELECT Id, WorkName, PartCode, UnitKey, UnitLabel, Kind, OrderIndex, LENGTH(Body) " +
                "FROM SourceTexts ORDER BY WorkName, PartCode, OrderIndex, Id";
            using var r = cmd.ExecuteReader();
            while (r.Read())
                units.Add(new Unit(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3),
                    r.GetString(4), r.GetString(5), r.GetInt32(6), r.GetInt32(7)));

            _manifest = units;
            _stamp = stamp;
            return units;
        }
    }

    /// <summary>
    /// Streams (unit, body) pairs matching the filters, in document order. Bodies are yielded one
    /// at a time and never accumulated, so an unfiltered sweep costs a read, not a residency.
    /// </summary>
    public IEnumerable<(Unit Unit, string Body)> Stream(string? work, string? partCode, string? kind)
    {
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();

        var where = new List<string>();
        if (!string.IsNullOrWhiteSpace(work)) { where.Add("WorkName = $w COLLATE NOCASE"); cmd.Parameters.AddWithValue("$w", work); }
        if (!string.IsNullOrWhiteSpace(partCode)) { where.Add("PartCode = $p COLLATE NOCASE"); cmd.Parameters.AddWithValue("$p", partCode); }
        if (!string.IsNullOrWhiteSpace(kind)) { where.Add("Kind = $k COLLATE NOCASE"); cmd.Parameters.AddWithValue("$k", kind); }

        cmd.CommandText =
            "SELECT Id, WorkName, PartCode, UnitKey, UnitLabel, Kind, OrderIndex, LENGTH(Body), Body FROM SourceTexts " +
            (where.Count > 0 ? "WHERE " + string.Join(" AND ", where) + " " : "") +
            "ORDER BY WorkName, OrderIndex, Id";

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            var unit = new Unit(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3),
                r.GetString(4), r.GetString(5), r.GetInt32(6), r.GetInt32(7));
            yield return (unit, r.GetString(8));
        }
    }

    /// <summary>A window of one unit's body. Returns null when the id is unknown.</summary>
    public (Unit Unit, string Body, string SourceRef)? Fetch(int id, int offset, int length)
    {
        using var conn = OpenReadOnly();
        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT Id, WorkName, PartCode, UnitKey, UnitLabel, Kind, OrderIndex, LENGTH(Body), " +
            "SUBSTR(Body, $off, $len), SourceRef FROM SourceTexts WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.Parameters.AddWithValue("$off", offset + 1); // SQLite SUBSTR is 1-based
        cmd.Parameters.AddWithValue("$len", length);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        var unit = new Unit(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3),
            r.GetString(4), r.GetString(5), r.GetInt32(6), r.GetInt32(7));
        return (unit, r.GetString(8), r.GetString(9));
    }

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
