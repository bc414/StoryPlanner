using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.Mcp;

/// <summary>
/// Owns the two .storyplan files. Read-only at the SQLite connection level (Mode=ReadOnly),
/// eager-loaded at startup, invalidated via PRAGMA data_version (the main file's mtime does
/// NOT track writes in WAL mode) plus a file length/mtime check to catch whole-file swaps.
/// </summary>
public sealed class StoryPlanSources : IDisposable
{
    private readonly object _gate = new();
    private readonly string _workingPath;
    private readonly string _archivePath;

    private SqliteConnection? _workingSentinel;
    private SqliteConnection? _archiveSentinel;
    private long _workingDataVersion = -1;
    private long _archiveDataVersion = -1;
    private (long Length, DateTime MTimeUtc) _workingFileStamp;
    private (long Length, DateTime MTimeUtc) _archiveFileStamp;

    private PlanCache? _working;
    private PlanCache? _archive;

    public StoryPlanSources(string workingPath, string archivePath)
    {
        _workingPath = workingPath;
        _archivePath = archivePath;
    }

    public void LoadAll()
    {
        Get(Corpus.Working);
        Get(Corpus.Archive);
    }

    public PlanCache Get(Corpus corpus)
    {
        lock (_gate)
        {
            return corpus == Corpus.Working
                ? EnsureFresh(Corpus.Working, _workingPath, ref _workingSentinel, ref _workingDataVersion, ref _workingFileStamp, ref _working)
                : EnsureFresh(Corpus.Archive, _archivePath, ref _archiveSentinel, ref _archiveDataVersion, ref _archiveFileStamp, ref _archive);
        }
    }

    private static PlanCache EnsureFresh(
        Corpus corpus,
        string path,
        ref SqliteConnection? sentinel,
        ref long dataVersion,
        ref (long Length, DateTime MTimeUtc) fileStamp,
        ref PlanCache? cache)
    {
        var fi = new FileInfo(path);
        if (!fi.Exists)
            throw new FileNotFoundException($".storyplan file not found: {path}");

        var stamp = (fi.Length, fi.LastWriteTimeUtc);

        // A replaced file (new inode under the same path) is invisible to the old sentinel
        // connection — reopen the sentinel when length/mtime of the main file changes.
        if (sentinel is null || stamp != fileStamp)
        {
            sentinel?.Dispose();
            sentinel = OpenReadOnly(path);
            sentinel.Open();
            fileStamp = stamp;
            cache = null; // force reload
        }

        // data_version increments on this connection whenever ANOTHER connection commits.
        var v = ReadDataVersion(sentinel);
        if (cache is null || v != dataVersion)
        {
            cache = Load(path, corpus, fi.Length);
            dataVersion = ReadDataVersion(sentinel);
        }

        return cache;
    }

    private static SqliteConnection OpenReadOnly(string path) =>
        new(new SqliteConnectionStringBuilder
        {
            DataSource = path,
            Mode = SqliteOpenMode.ReadOnly,
            Pooling = false
        }.ToString());

    private static long ReadDataVersion(SqliteConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "PRAGMA data_version;";
        return Convert.ToInt64(cmd.ExecuteScalar());
    }

    private static PlanCache Load(string path, Corpus corpus, long sizeBytes)
    {
        var connString = new SqliteConnectionStringBuilder
        {
            DataSource = path,
            Mode = SqliteOpenMode.ReadOnly,
            Pooling = false
        }.ToString();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;

        // Read-only connection: even a bug that reached SaveChanges would fail at the
        // SQLite layer. We never call Database.Migrate() — StoryService.OpenProjectAsync
        // does, and silently upgrading Brian's files is exactly what this server must not do.
        using var ctx = new AppDbContext(options);
        return PlanCache.Build(ctx, corpus, path, sizeBytes);
    }

    public void Dispose()
    {
        _workingSentinel?.Dispose();
        _archiveSentinel?.Dispose();
    }
}
