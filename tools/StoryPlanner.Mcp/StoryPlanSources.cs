using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.Mcp;

public enum Corpus
{
    Working,
    Archive
}

/// <summary>
/// An immutable in-memory snapshot of one .storyplan file, with id lookups precomputed.
/// </summary>
public sealed class PlanCache
{
    public required string FilePath { get; init; }
    public required Corpus Corpus { get; init; }
    public required DateTime LoadedAtUtc { get; init; }
    public required long FileSizeBytes { get; init; }

    public required IReadOnlyList<Note> Notes { get; init; }
    public required IReadOnlyList<Subject> Subjects { get; init; }
    public required IReadOnlyList<SubjectDefinition> SubjectDefinitions { get; init; }
    public required IReadOnlyList<PlotPoint> PlotPoints { get; init; }
    public required IReadOnlyList<Chapter> Chapters { get; init; }
    public required IReadOnlyList<Story> Stories { get; init; }
    public required IReadOnlyList<Theater> Theaters { get; init; }
    public required IReadOnlyList<Pivot> Pivots { get; init; }
    public required IReadOnlyList<PlotPointSubjectLink> Links { get; init; }
    public required IReadOnlyList<NoteTrackDefinition> Tracks { get; init; }
    public required IReadOnlyList<Theme> Themes { get; init; }
    public required IReadOnlyList<SourceMaterial> SourceMaterials { get; init; }
    public required IReadOnlyList<SourceMaterialPart> SourceMaterialParts { get; init; }
    public required IReadOnlyList<NoteSourceReference> SourceReferences { get; init; }
    public required IReadOnlyList<Conversation> Conversations { get; init; }
    public required IReadOnlyList<ConversationBlock> Blocks { get; init; }
    public required IReadOnlyList<WorkPhase> WorkPhases { get; init; }
    public required IReadOnlyList<NarrativePropertyDefinition> NarrativeProperties { get; init; }
    public required IReadOnlyList<NarrativePropertyValueDefinition> NarrativePropertyValueDefs { get; init; }
    public required IReadOnlyList<NarrativePropertyValue> NarrativePropertyValues { get; init; }

    public required IReadOnlyDictionary<int, Subject> SubjectById { get; init; }
    public required IReadOnlyDictionary<int, SubjectDefinition> SubjectDefById { get; init; }
    public required IReadOnlyDictionary<int, PlotPoint> PlotPointById { get; init; }
    public required IReadOnlyDictionary<int, Chapter> ChapterById { get; init; }
    public required IReadOnlyDictionary<int, Story> StoryById { get; init; }
    /// <summary>Key 0 is the "(Unplaced)" sentinel — never a real Theater row.</summary>
    public required IReadOnlyDictionary<int, Theater> TheaterById { get; init; }

    /// <summary>Chapters grouped by StoryId. Key 0 is the "(Unassigned)" sentinel — never a real Story row.</summary>
    public required IReadOnlyDictionary<int, List<Chapter>> ChaptersByStory { get; init; }
    public required IReadOnlyDictionary<int, PlotPointSubjectLink> LinkById { get; init; }
    public required IReadOnlyDictionary<int, NoteTrackDefinition> TrackById { get; init; }
    public required IReadOnlyDictionary<int, Theme> ThemeById { get; init; }
    public required IReadOnlyDictionary<int, SourceMaterial> SourceMaterialById { get; init; }
    public required IReadOnlyDictionary<int, SourceMaterialPart> SourceMaterialPartById { get; init; }
    public required IReadOnlyDictionary<int, Conversation> ConversationById { get; init; }
    public required IReadOnlyDictionary<int, ConversationBlock> BlockById { get; init; }

    /// <summary>Notes grouped by (OwnerType, OwnerId) — the polymorphic ownership join.</summary>
    public required IReadOnlyDictionary<(OwnerType, int), List<Note>> NotesByOwner { get; init; }

    /// <summary>SourceMaterialParts grouped by their parent Work's Id.</summary>
    public required IReadOnlyDictionary<int, List<SourceMaterialPart>> SourceMaterialPartsByWork { get; init; }

    /// <summary>NoteSourceReferences grouped by NoteId — a note may cite several Parts for one claim.</summary>
    public required IReadOnlyDictionary<int, List<NoteSourceReference>> SourceReferencesByNote { get; init; }

    /// <summary>Links grouped by SubjectId and by PlotPointId.</summary>
    public required IReadOnlyDictionary<int, List<PlotPointSubjectLink>> LinksBySubject { get; init; }
    public required IReadOnlyDictionary<int, List<PlotPointSubjectLink>> LinksByPlotPoint { get; init; }

    public required IReadOnlyDictionary<int, WorkPhase> WorkPhaseById { get; init; }
    public required IReadOnlyDictionary<int, NarrativePropertyDefinition> NarrativePropertyById { get; init; }
    public required IReadOnlyDictionary<int, NarrativePropertyValueDefinition> NarrativePropertyValueDefById { get; init; }

    /// <summary>Allowed values grouped by their owning property, in row order.</summary>
    public required IReadOnlyDictionary<int, List<NarrativePropertyValueDefinition>> ValueDefsByProperty { get; init; }

    /// <summary>
    /// Assigned narrative property values grouped by (OwnerType, OwnerId).
    ///
    /// NarrativePropertyValue has NO OwnerType column of its own — it is resolved by tracing
    /// ValueDefinitionId -> NarrativePropertyDefinitionId -> OwnerType, mirroring
    /// ContentDeleter.RemoveOwnedNarrativePropertyValues and PlanIntegrity. Do not "simplify" this
    /// to a plain OwnerId key: subject 7 and chapter 7 would collide silently.
    /// </summary>
    public required IReadOnlyDictionary<(OwnerType, int), List<NarrativePropertyValue>> NarrativePropertyValuesByOwner { get; init; }

    /// <summary>
    /// Authored subject-to-subject edges and the definitions that type them. Unlike the narrative
    /// property values these need no owner-type trace: both endpoints are Subjects and
    /// RelationDefinitionId resolves both types.
    /// </summary>
    public required IReadOnlyList<SubjectRelationDefinition> SubjectRelationDefinitions { get; init; }
    public required IReadOnlyList<SubjectRelation> SubjectRelations { get; init; }
    public required IReadOnlyDictionary<int, SubjectRelationDefinition> SubjectRelationDefById { get; init; }

    /// <summary>Edges grouped by the subject holding them, in SortOrder.</summary>
    public required IReadOnlyDictionary<int, List<SubjectRelation>> SubjectRelationsBySubject { get; init; }
}

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

        var notes = ctx.Notes.ToList();
        var subjects = ctx.Subjects.ToList();
        var subjectDefs = ctx.SubjectDefinitions.ToList();
        var plotPoints = ctx.PlotPoints.ToList();
        var chapters = ctx.Chapters.ToList();
        var stories = ctx.Stories.ToList();
        var theaters = ctx.Theaters.ToList();
        var pivots = ctx.Pivots.ToList();
        var links = ctx.PlotPointSubjectLinks.ToList();
        var tracks = ctx.NoteTrackDefinitions.ToList();
        var themes = ctx.Themes.ToList();
        var sourceMaterials = ctx.SourceMaterials.ToList();
        var sourceMaterialParts = ctx.SourceMaterialParts.ToList();
        var sourceReferences = ctx.NoteSourceReferences.ToList();
        var conversations = ctx.Conversations.ToList();
        var blocks = ctx.ConversationBlocks.ToList();
        var workPhases = ctx.WorkPhases.OrderBy(p => p.DisplayOrder).ToList();
        var properties = ctx.NarrativePropertyDefinitions.ToList();
        var valueDefs = ctx.NarrativePropertyValueDefinitions.ToList();
        var propertyValues = ctx.NarrativePropertyValues.ToList();
        var relationDefs = ctx.SubjectRelationDefinitions.OrderBy(r => r.DisplayOrder).ThenBy(r => r.Id).ToList();
        var relations = ctx.SubjectRelations.ToList();

        // Resolve each assignment's owner type through the definition chain — the value row has no
        // OwnerType of its own. An assignment whose value definition is missing is dropped rather
        // than guessed; PlanIntegrity reports it as narrativevalue.definition_missing.
        var ownerTypeByValueDefId = valueDefs
            .Join(properties, vd => vd.NarrativePropertyDefinitionId, pd => pd.Id, (vd, pd) => new { vd.Id, pd.OwnerType })
            .ToDictionary(x => x.Id, x => x.OwnerType);

        return new PlanCache
        {
            FilePath = path,
            Corpus = corpus,
            LoadedAtUtc = DateTime.UtcNow,
            FileSizeBytes = sizeBytes,
            Notes = notes,
            Subjects = subjects,
            SubjectDefinitions = subjectDefs,
            PlotPoints = plotPoints,
            Chapters = chapters,
            Stories = stories,
            Theaters = theaters,
            Pivots = pivots,
            Links = links,
            Tracks = tracks,
            Themes = themes,
            SourceMaterials = sourceMaterials,
            SourceMaterialParts = sourceMaterialParts,
            SourceReferences = sourceReferences,
            Conversations = conversations,
            Blocks = blocks,
            SubjectById = subjects.ToDictionary(s => s.Id),
            SubjectDefById = subjectDefs.ToDictionary(d => d.Id),
            PlotPointById = plotPoints.ToDictionary(p => p.Id),
            ChapterById = chapters.ToDictionary(c => c.Id),
            StoryById = stories.ToDictionary(s => s.Id),
            TheaterById = theaters.ToDictionary(t => t.Id),
            ChaptersByStory = chapters.GroupBy(c => c.StoryId).ToDictionary(g => g.Key, g => g.ToList()),
            LinkById = links.ToDictionary(l => l.Id),
            TrackById = tracks.ToDictionary(t => t.Id),
            ThemeById = themes.ToDictionary(t => t.Id),
            SourceMaterialById = sourceMaterials.ToDictionary(s => s.Id),
            SourceMaterialPartById = sourceMaterialParts.ToDictionary(p => p.Id),
            ConversationById = conversations.ToDictionary(c => c.Id),
            BlockById = blocks.ToDictionary(b => b.Id),
            NotesByOwner = notes
                .GroupBy(n => (n.OwnerType, n.OwnerId))
                .ToDictionary(g => g.Key, g => g.OrderBy(n => n.SortOrder).ThenBy(n => n.Id).ToList()),
            LinksBySubject = links
                .GroupBy(l => l.SubjectId)
                .ToDictionary(g => g.Key, g => g.ToList()),
            LinksByPlotPoint = links
                .GroupBy(l => l.PlotPointId)
                .ToDictionary(g => g.Key, g => g.ToList()),
            SourceMaterialPartsByWork = sourceMaterialParts
                .GroupBy(p => p.SourceMaterialId)
                .ToDictionary(g => g.Key, g => g.OrderBy(p => p.OrderIndex).ToList()),
            SourceReferencesByNote = sourceReferences
                .GroupBy(r => r.NoteId)
                .ToDictionary(g => g.Key, g => g.OrderBy(r => r.SortOrder).ToList()),
            WorkPhases = workPhases,
            NarrativeProperties = properties,
            NarrativePropertyValueDefs = valueDefs,
            NarrativePropertyValues = propertyValues,
            WorkPhaseById = workPhases.ToDictionary(p => p.Id),
            NarrativePropertyById = properties.ToDictionary(p => p.Id),
            NarrativePropertyValueDefById = valueDefs.ToDictionary(v => v.Id),
            ValueDefsByProperty = valueDefs
                .GroupBy(v => v.NarrativePropertyDefinitionId)
                .ToDictionary(g => g.Key, g => g.OrderBy(v => v.Id).ToList()),
            NarrativePropertyValuesByOwner = propertyValues
                .Where(v => ownerTypeByValueDefId.ContainsKey(v.ValueDefinitionId))
                .GroupBy(v => (ownerTypeByValueDefId[v.ValueDefinitionId], v.OwnerId))
                .ToDictionary(g => g.Key, g => g.ToList()),
            SubjectRelationDefinitions = relationDefs,
            SubjectRelations = relations,
            SubjectRelationDefById = relationDefs.ToDictionary(r => r.Id),
            SubjectRelationsBySubject = relations
                .GroupBy(r => r.SubjectId)
                .ToDictionary(g => g.Key, g => g.OrderBy(r => r.SortOrder).ThenBy(r => r.Id).ToList())
        };
    }

    public void Dispose()
    {
        _workingSentinel?.Dispose();
        _archiveSentinel?.Dispose();
    }
}
