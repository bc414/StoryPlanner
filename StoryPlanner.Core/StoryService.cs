using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace StoryPlanner.Core;

/// <summary>
/// Represents data loading
/// </summary>
public class StoryService : IStoryService
{
    private AppDbContext? _context;

    // --- The In-Memory Data Graph ---

    public ObservableCollection<Subject> Subjects { get; private set; } = new();
    public ObservableCollection<PlotPoint> PlotPoints { get; private set; } = new();

    public ObservableCollection<PlotPointSubjectLink> PlotPointsSubjectLinks { get; private set; } = new();

    public ObservableCollection<Chapter> Chapters { get; private set; } = new();
    public ObservableCollection<Story> Stories { get; private set; } = new();
    public ObservableCollection<Theater> Theaters { get; private set; } = new();
    public ObservableCollection<Pivot> Pivots { get; private set; } = new();
    public ObservableCollection<Note> Notes { get; private set; } = new();
    public ObservableCollection<SubjectDefinition> SubjectDefinitions { get; private set; } = new();
    public ObservableCollection<NoteTrackDefinition> NoteTrackDefinitions { get; private set; } = new();
    public ObservableCollection<NarrativePropertyDefinition> NarrativePropertyDefinitions { get; private set; } = new();
    public ObservableCollection<NarrativePropertyValueDefinition> NarrativePropertyValueDefinitions { get; private set; } = new();
    public ObservableCollection<NarrativePropertyValue> NarrativePropertyValues { get; private set; } = new();
    public ObservableCollection<PropertyBoard> PropertyBoards { get; private set; } = new();
    public ObservableCollection<SubjectRelationDefinition> SubjectRelationDefinitions { get; private set; } = new();
    public ObservableCollection<SubjectRelation> SubjectRelations { get; private set; } = new();
    public ObservableCollection<WorkPhase> WorkPhases { get; private set; } = new();
    public ObservableCollection<Theme> Themes { get; private set; } = new();

    public ObservableCollection<SourceMaterial> SourceMaterials { get; private set; } = new();
    public ObservableCollection<SourceMaterialPart> SourceMaterialParts { get; private set; } = new();
    public ObservableCollection<NoteSourceReference> NoteSourceReferences { get; private set; } = new();

    public ObservableCollection<UiSetting> UiSettings { get; private set; } = new();

    public ObservableCollection<Conversation> Conversations { get; private set; } = new();
    public ObservableCollection<ConversationBlock> ConversationBlocks { get; private set; } = new();
    public ObservableCollection<ConversationSubjectCoverage> ConversationSubjectCoverages { get; private set; } = new();
    public ObservableCollection<ConversationSubjectCoverageTrack> ConversationSubjectCoverageTracks { get; private set; } = new();

    public string CurrentFilePath { get; private set; } = string.Empty;
    public bool IsProjectLoaded { get; private set; } = false;

    public StoryService()
    {
        
    }

    // --- 1. NEW PROJECT ---
    public async Task CreateProjectAsync(string filePath)
    {
        if (IsProjectLoaded) return;

        CurrentFilePath = filePath;

        // About to wipe any existing file at this path — refuse if it can't be snapshotted first.
        if (File.Exists(filePath) && !CreateSafetyBackup(filePath))
            throw new InvalidOperationException(
                $"\"{filePath}\" already exists and could not be backed up before being overwritten.");

        // Configure for the new file
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={filePath}")
            .EnableSensitiveDataLogging()
            .Options;

        _context = new AppDbContext(options);

        // 1. WIPE THE FILE (If overwriting)
        // We strictly delete the file so we start from a clean slate.
        await _context.Database.EnsureDeletedAsync();

        // 2. CREATE VIA MIGRATIONS (Fix is here!)
        // Do NOT use EnsureCreatedAsync(). Use MigrateAsync() so the 
        // __EFMigrationsHistory table is created correctly.
        await _context.Database.MigrateAsync();

        // Seed Defaults
        //Get rid of this when done testing and ready to actually plan my story
        //await DataSeeder.SeedAsync(_context);

        // Load it up
        await LoadDataAsync();
    }

    // --- 2. OPEN PROJECT ---
    public async Task OpenProjectAsync(string filePath)
    {
        if (IsProjectLoaded) return;
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found", filePath);

        CurrentFilePath = filePath;

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={filePath}")
            .EnableSensitiveDataLogging()
            .Options;

        _context = new AppDbContext(options);

        // Back up before any schema upgrade — this is the one path that silently rewrote
        // Brian's real file with no safety net (CreateSafetyBackup was only ever called from
        // CreateProjectAsync, which then deletes the file anyway). Only costs a copy on the
        // open that actually has something pending — and if the backup cannot be taken, the
        // migration does not run: a failed safety net plus an in-place schema rewrite is the
        // exact combination this guard exists to prevent.
        if ((await _context.Database.GetPendingMigrationsAsync()).Any() && !CreateSafetyBackup(filePath))
            throw new InvalidOperationException(
                $"A schema upgrade is pending for \"{filePath}\" but the safety backup could not be " +
                "written (see debug output). Fix the backup problem and reopen — migrating without " +
                "a backup rewrites the file in place with no way back.");

        // Ensure schema is compatible
        await _context.Database.MigrateAsync();

        await LoadDataAsync();
    }
    
    /// <summary>
    /// Snapshots <paramref name="originalPath"/> into a sibling "Backups" folder with a timestamped
    /// name, keeping the 10 most recent. Static and public so <c>StoryPlanner.DataOps</c> can
    /// reuse the exact same safety procedure before running a one-time operation, rather than
    /// re-deriving it.
    ///
    /// Uses SQLite's <c>VACUUM INTO</c> rather than <c>File.Copy</c>: the files are WAL-mode, so a
    /// bare main-file copy silently misses every transaction still sitting in the <c>-wal</c>
    /// sidecar (the documented 2026-07-30 stale-copy trap). <c>VACUUM INTO</c> reads through the
    /// WAL and produces a consistent, self-contained snapshot even while another process has the
    /// file open. Returns false on failure — callers that are about to do something irreversible
    /// (schema migration, a DataOps write) must refuse rather than proceed unprotected.
    /// </summary>
    public static bool CreateSafetyBackup(string originalPath)
    {
        try
        {
            // Example: "MyStory.storyplan" -> "Backups/MyStory.2026-08-02_14-30-00.bak"
            string directory = Path.GetDirectoryName(originalPath) ?? string.Empty;
            string fileName = Path.GetFileNameWithoutExtension(originalPath);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string backupFolder = Path.Combine(directory, "Backups");
            string backupPath = Path.Combine(backupFolder, $"{fileName}.{timestamp}.bak");

            Directory.CreateDirectory(backupFolder);

            // VACUUM INTO refuses to overwrite; the timestamped name makes collisions
            // effectively impossible, but clear a leftover from a same-second retry anyway.
            if (File.Exists(backupPath)) File.Delete(backupPath);

            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(
                       $"Data Source={originalPath};Mode=ReadOnly"))
            {
                connection.Open();
                using var command = connection.CreateCommand();
                // VACUUM INTO does not accept parameters — escape the literal path instead.
                command.CommandText = $"VACUUM INTO '{backupPath.Replace("'", "''")}'";
                command.ExecuteNonQuery();
            }

            CleanUpOldBackups(backupFolder);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Backup failed: {ex.Message}");
            return false;
        }
    }

    private static void CleanUpOldBackups(string backupFolder)
    {
        try
        {
            var dir = new DirectoryInfo(backupFolder);
            var files = dir.GetFiles("*.bak")
                .OrderByDescending(f => f.CreationTime) // Newest first
                .Skip(10) // Keep top 10
                .ToList();

            foreach (var file in files)
            {
                file.Delete();
            }
        }
        catch { /* Ignore cleanup errors */ }
    }

    public async Task LoadDataAsync()
    {
        if (_context == null) return;

        await _context.Notes.LoadAsync();
        await _context.Subjects.LoadAsync();
        await _context.PlotPoints.LoadAsync();
        await _context.PlotPointSubjectLinks.LoadAsync();

        // Stories load first so the chapter query below can order by (story, chapter) below.
        await _context.Stories.OrderBy(s => s.OrderIndex).LoadAsync();

        await _context.Theaters.OrderBy(t => t.OrderIndex).LoadAsync();
        await _context.Pivots.OrderBy(p => p.Year).LoadAsync();

        // Chapters sort by (story reading order, chapter order). No navigation property exists
        // (settled architecture), so the join is written explicitly; StoryId = 0 (the
        // "(Unassigned)" sentinel, see UnassignedStory) never matches a real Story row and
        // sorts last via the null-coalesced MaxValue.
        await (from c in _context.Chapters
               join s in _context.Stories on c.StoryId equals s.Id into storyGroup
               from s in storyGroup.DefaultIfEmpty()
               orderby s == null ? int.MaxValue : s.OrderIndex, c.OrderIndex
               select c)
            .LoadAsync();

        // Definitions — load leaves first so EF relationship fixup wires nav properties
        await _context.WorkPhases.LoadAsync();
        await _context.NoteTrackDefinitions.LoadAsync();
        await _context.PropertyBoards.LoadAsync();
        await _context.NarrativePropertyValueDefinitions.LoadAsync();
        await _context.NarrativePropertyDefinitions.LoadAsync();
        await _context.SubjectDefinitions.LoadAsync();
        await _context.NarrativePropertyValues.LoadAsync();
        await _context.SubjectRelationDefinitions.LoadAsync();
        await _context.SubjectRelations.LoadAsync();

        await _context.Themes.LoadAsync();   // ← was missing

        await _context.SourceMaterials.OrderBy(s => s.OrderIndex).LoadAsync();
        await _context.SourceMaterialParts.OrderBy(p => p.OrderIndex).LoadAsync();
        await _context.NoteSourceReferences.LoadAsync();
        await _context.UiSettings.LoadAsync();

        await _context.Conversations.OrderBy(c => c.ConversationDate).LoadAsync();
        await _context.ConversationBlocks.OrderBy(b => b.ConversationId).ThenBy(b => b.BlockNumber).LoadAsync();
        await _context.ConversationSubjectCoverages.LoadAsync();
        await _context.ConversationSubjectCoverageTracks.LoadAsync();

        // STEP 4: BIND TO UI
        Notes                  = _context.Notes.Local.ToObservableCollection();
        Subjects               = _context.Subjects.Local.ToObservableCollection();
        Stories                = _context.Stories.Local.ToObservableCollection();
        Theaters               = _context.Theaters.Local.ToObservableCollection();
        Pivots                 = _context.Pivots.Local.ToObservableCollection();
        Chapters               = _context.Chapters.Local.ToObservableCollection();
        PlotPoints             = _context.PlotPoints.Local.ToObservableCollection();
        PlotPointsSubjectLinks = _context.PlotPointSubjectLinks.Local.ToObservableCollection();
        Themes                 = _context.Themes.Local.ToObservableCollection();

        SubjectDefinitions                = _context.SubjectDefinitions.Local.ToObservableCollection();
        NoteTrackDefinitions              = _context.NoteTrackDefinitions.Local.ToObservableCollection();
        NarrativePropertyDefinitions      = _context.NarrativePropertyDefinitions.Local.ToObservableCollection();
        NarrativePropertyValueDefinitions = _context.NarrativePropertyValueDefinitions.Local.ToObservableCollection();
        NarrativePropertyValues           = _context.NarrativePropertyValues.Local.ToObservableCollection();
        PropertyBoards                    = _context.PropertyBoards.Local.ToObservableCollection();
        SubjectRelationDefinitions        = _context.SubjectRelationDefinitions.Local.ToObservableCollection();
        SubjectRelations                  = _context.SubjectRelations.Local.ToObservableCollection();
        // .Local is change-tracker order, NOT the LoadAsync ordering — every consumer sorts by
        // DisplayOrder at the point of use.
        WorkPhases                        = _context.WorkPhases.Local.ToObservableCollection();

        SourceMaterials       = _context.SourceMaterials.Local.ToObservableCollection();
        SourceMaterialParts   = _context.SourceMaterialParts.Local.ToObservableCollection();
        NoteSourceReferences  = _context.NoteSourceReferences.Local.ToObservableCollection();
        UiSettings            = _context.UiSettings.Local.ToObservableCollection();

        Conversations                 = _context.Conversations.Local.ToObservableCollection();
        ConversationBlocks            = _context.ConversationBlocks.Local.ToObservableCollection();
        ConversationSubjectCoverages  = _context.ConversationSubjectCoverages.Local.ToObservableCollection();
        ConversationSubjectCoverageTracks = _context.ConversationSubjectCoverageTracks.Local.ToObservableCollection();

        IsProjectLoaded = true;
    }

    public async Task SaveAsync()
    {
        if (_context == null) throw new InvalidOperationException("Not initialized");
        await _context.SaveChangesAsync();
    }
    public async Task<ConversationImportResult> ImportConversationsAsync(string contentPath, string? metaPath)
    {
        if (_context == null) return ConversationImportResult.Empty;
        var importer = new ConversationImporter(_context);
        var result = await importer.ImportFileAsync(contentPath, metaPath);
        await SaveAsync();
        await ReloadConversationsAsync();
        return result;
    }

    public async Task<ConversationImportResult> ImportConversationsFolderAsync(string folderPath)
    {
        if (_context == null) return ConversationImportResult.Empty;
        var importer = new ConversationImporter(_context);
        var result = await importer.ImportFolderAsync(folderPath);
        await SaveAsync();
        await ReloadConversationsAsync();
        return result;
    }

    public async Task<ConversationImportResult> ImportScannedConversationsAsync(IReadOnlyList<ConversationSyncItem> items)
    {
        if (_context == null) return ConversationImportResult.Empty;
        var importer = new ConversationImporter(_context);
        var result = await importer.ImportScannedAsync(items);
        await SaveAsync();
        await ReloadConversationsAsync();
        return result;
    }

    // Reload conversation collections so in-memory state reflects new rows. The coverage sets are
    // reloaded too: nothing writes them any more (the AI-suggested routing was cut 2026-07-31) but
    // the existing rows stay live so DeleteConversationAsync's cascade keeps working.
    private async Task ReloadConversationsAsync()
    {
        if (_context == null) return;
        await _context.Conversations.LoadAsync();
        await _context.ConversationBlocks.LoadAsync();
        await _context.ConversationSubjectCoverages.LoadAsync();
        await _context.ConversationSubjectCoverageTracks.LoadAsync();
    }

    // --- Conversation scan/export (Claude export vs. DB ground-truth) ---

    public async Task<List<ConversationSyncItem>> ScanClaudeExportAsync(string claudeExportPath)
    {
        if (_context == null) return new List<ConversationSyncItem>();

        // The export can be well over 100MB, so parse off the UI thread.
        var parsed  = await Task.Run(() => ClaudeExportParser.Parse(claudeExportPath));
        var ignored = await _context.IgnoredConversations.ToListAsync();

        return ConversationSyncScanner.Scan(parsed, Conversations, ignored);
    }

    public async Task<List<ConversationContentExporter.ExportedFile>> ExportConversationContentAsync(
        IReadOnlyList<ConversationSyncItem> selectedItems, string outputFolder)
    {
        if (_context == null) return new List<ConversationContentExporter.ExportedFile>();
        return await Task.Run(() => ConversationContentExporter.Export(selectedItems, outputFolder, Conversations));
    }

    public async Task BackfillConversationUuidAsync(int conversationId, string uuid)
    {
        if (_context == null) return;
        var conversation = Conversations.FirstOrDefault(c => c.Id == conversationId);
        if (conversation is null) return;

        conversation.SourceUuid = uuid;
        await SaveAsync();
    }

    public async Task IgnoreConversationAsync(string uuid, string title)
    {
        if (_context == null) return;
        if (await _context.IgnoredConversations.AnyAsync(i => i.SourceUuid == uuid)) return;

        _context.IgnoredConversations.Add(new IgnoredConversation { SourceUuid = uuid, Title = title });
        await SaveAsync();
    }

    public async Task UnignoreConversationAsync(string uuid)
    {
        if (_context == null) return;
        var entry = await _context.IgnoredConversations.FirstOrDefaultAsync(i => i.SourceUuid == uuid);
        if (entry is null) return;

        _context.IgnoredConversations.Remove(entry);
        await SaveAsync();
    }

    public async Task<ConversationSyncItem> RescanOneAsync(ParsedClaudeConversation export)
    {
        if (_context == null) return new ConversationSyncItem { Export = export, Classification = ConversationSyncClassification.New };

        var ignored = await _context.IgnoredConversations.ToListAsync();
        return ConversationSyncScanner.Scan([export], Conversations, ignored).Single();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public NoteTrackDefinition? GetNoteTrackDefinition(int id)
    => _context?.Set<NoteTrackDefinition>().Find(id);

    public void DeleteNote(int noteId)
    {
        var note = Notes.FirstOrDefault(n => n.Id == noteId);
        if (note is null) return;

        // Citations are note-owned; without this cascade a deleted cited note leaves dangling
        // NoteSourceReference rows that silently corrupt the source-material coverage data.
        foreach (var reference in NoteSourceReferences.Where(r => r.NoteId == noteId).ToList())
            NoteSourceReferences.Remove(reference);

        Notes.Remove(note);
    }

    public void DeleteLink(int linkId)
    {
        var link = PlotPointsSubjectLinks.FirstOrDefault(l => l.Id == linkId);
        if (link is null) return;

        RemoveOwnedNarrativePropertyValues(linkId, OwnerType.PlotPointSubjectLink);
        PlotPointsSubjectLinks.Remove(link);
    }

    public void RemoveOwnedNarrativePropertyValues(int ownerId, OwnerType ownerType)
    {
        // NarrativePropertyValue has no OwnerType column — ownership resolves only by tracing
        // ValueDefinitionId → NarrativePropertyDefinitionId → OwnerType. Without the trace,
        // subject 7 and chapter 7 collide silently.
        var validValueDefinitionIds = NarrativePropertyValueDefinitions
            .Where(vd => NarrativePropertyDefinitions
                .Any(pd => pd.Id == vd.NarrativePropertyDefinitionId && pd.OwnerType == ownerType))
            .Select(vd => vd.Id)
            .ToHashSet();

        foreach (var value in NarrativePropertyValues
                     .Where(v => v.OwnerId == ownerId && validValueDefinitionIds.Contains(v.ValueDefinitionId))
                     .ToList())
            NarrativePropertyValues.Remove(value);
    }

    public void RemoveSubjectRelations(int subjectId)
    {
        // BOTH ends. Dropping only the outgoing edges would leave every subject that pointed AT
        // this one holding a target id that no longer resolves — and since subject ids are
        // reused by SQLite's rowid allocation, a later subject could silently inherit the edge.
        foreach (var relation in SubjectRelations
                     .Where(r => r.SubjectId == subjectId || r.TargetSubjectId == subjectId)
                     .ToList())
            SubjectRelations.Remove(relation);
    }

    public async Task DeleteConversationAsync(Conversation conversation)
    {
        var blocks = ConversationBlocks.Where(b => b.ConversationId == conversation.Id).ToList();
        foreach (var block in blocks)
            ConversationBlocks.Remove(block);

        var coverages = ConversationSubjectCoverages.Where(c => c.ConversationId == conversation.Id).ToList();
        foreach (var coverage in coverages)
        {
            var tracks = ConversationSubjectCoverageTracks
                .Where(t => t.ConversationSubjectCoverageId == coverage.Id).ToList();
            foreach (var track in tracks)
                ConversationSubjectCoverageTracks.Remove(track);

            ConversationSubjectCoverages.Remove(coverage);
        }

        Conversations.Remove(conversation);
        await SaveAsync();
    }
}