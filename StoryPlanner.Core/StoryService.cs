using System.Collections.ObjectModel;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Markdig;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

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
    public ObservableCollection<Theme> Themes { get; private set; } = new();

    public ObservableCollection<SourceMaterial> SourceMaterials { get; private set; } = new();
    public ObservableCollection<SourceMaterialPart> SourceMaterialParts { get; private set; } = new();
    public ObservableCollection<NoteSourceReference> NoteSourceReferences { get; private set; } = new();

    public ObservableCollection<GeminiEntry> GeminiEntries { get; private set; } = new();
    public ObservableCollection<Idea> Ideas { get; private set; } = new();

    public ObservableCollection<Conversation> Conversations { get; private set; } = new();
    public ObservableCollection<ConversationBlock> ConversationBlocks { get; private set; } = new();
    public ObservableCollection<ConversationSubjectCoverage> ConversationSubjectCoverages { get; private set; } = new();
    public ObservableCollection<ConversationSubjectCoverageTrack> ConversationSubjectCoverageTracks { get; private set; } = new();

    public string CurrentFilePath { get; private set; } = string.Empty;
    public bool IsProjectLoaded { get; private set; } = false;

    public StoryService()
    {
        
    }

    public async Task StoreGeminiEntriesAsync(string file)
    {
        if (_context == null) return;
        
        using Stream stream = File.OpenRead(file);
        using StreamReader reader = new StreamReader(stream);
            
        // Read file to string
        string jsonContent = await reader.ReadToEndAsync();

        // Deserialize with case-insensitive options just to be safe
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var entries = JsonSerializer.Deserialize<List<GeminiJsonReader>>(jsonContent, options);

        var answers = GeminiEntry.FromJson(entries);
        _context.GeminiEntries.AddRange(answers);
        await SaveAsync();
    }

    public string GetFullProjectJson()
    {
        if (_context == null) throw new InvalidOperationException("Project not loaded");
        var fileService = new StoryFileService(_context);
        return fileService.ExportFullDatabase();
    }

    public string GetAiContextJson(bool includeVerbatim)
    {
        return string.Empty;
    }

    public string GetMarkdown()
    {
        if (_context == null) throw new InvalidOperationException("Project not loaded");
        var fileService = new StoryFileService(_context);
        //return fileService.GetMarkdownContextForAI();
        return string.Empty;
    }

    // --- 1. NEW PROJECT ---
    public async Task CreateProjectAsync(string filePath)
    {
        if (IsProjectLoaded) return;

        CurrentFilePath = filePath;
        
        CreateSafetyBackup(filePath);
        
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
        // open that actually has something pending.
        if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            CreateSafetyBackup(filePath);

        // Ensure schema is compatible
        await _context.Database.MigrateAsync();

        await LoadDataAsync();
    }
    
    /// <summary>
    /// Copies <paramref name="originalPath"/> into a sibling "Backups" folder with a timestamped
    /// name, keeping the 10 most recent. Static and public so <c>StoryPlanner.DataOps</c> can
    /// reuse the exact same safety procedure before running a one-time operation, rather than
    /// re-deriving it.
    /// </summary>
    public static void CreateSafetyBackup(string originalPath)
    {
        try 
        {
            // Example: "MyStory.db" -> "MyStory.2023-10-27_14-30-00.bak"
            string directory = Path.GetDirectoryName(originalPath) ?? string.Empty;
            string fileName = Path.GetFileNameWithoutExtension(originalPath);
            string extension = Path.GetExtension(originalPath); // .db
        
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string backupName = $"{fileName}.{timestamp}.bak";
            string backupPath = Path.Combine(directory, "Backups", backupName);

            // Ensure "Backups" folder exists
            Directory.CreateDirectory(Path.Combine(directory, "Backups"));

            // Perform the Copy
            File.Copy(originalPath, backupPath, overwrite: true);
        
            // Optional: Clean up old backups (keep last 10)
            CleanUpOldBackups(Path.Combine(directory, "Backups"));
        }
        catch (Exception ex)
        {
            // Don't stop the app, just log it. 
            // In a real app, you might show a warning "Backup Failed".
            System.Diagnostics.Debug.WriteLine($"Backup failed: {ex.Message}");
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
        await _context.NoteTrackDefinitions.LoadAsync();
        await _context.NarrativePropertyValueDefinitions.LoadAsync();
        await _context.NarrativePropertyDefinitions.LoadAsync();
        await _context.SubjectDefinitions.LoadAsync();
        await _context.NarrativePropertyValues.LoadAsync();

        await _context.Themes.LoadAsync();   // ← was missing

        await _context.SourceMaterials.OrderBy(s => s.OrderIndex).LoadAsync();
        await _context.SourceMaterialParts.OrderBy(p => p.OrderIndex).LoadAsync();
        await _context.NoteSourceReferences.LoadAsync();
        await _context.GeminiEntries.LoadAsync();
        await _context.Ideas.LoadAsync();

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

        SourceMaterials       = _context.SourceMaterials.Local.ToObservableCollection();
        SourceMaterialParts   = _context.SourceMaterialParts.Local.ToObservableCollection();
        NoteSourceReferences  = _context.NoteSourceReferences.Local.ToObservableCollection();
        GeminiEntries   = _context.GeminiEntries.Local.ToObservableCollection();
        Ideas           = _context.Ideas.Local.ToObservableCollection();

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

        string markdownContext = GetMarkdown();
        File.WriteAllText(CurrentFilePath + ".md", markdownContext);
        string docTitle = Path.GetFileNameWithoutExtension(CurrentFilePath) + " - Story Bible";
        //await SyncToGoogleDocsAsync(markdownContext, docTitle);
        // --- Log specific metrics to CSV ---
        try
        {
            string csvFilePath = CurrentFilePath + "_stats.csv";
            bool isNewFile = !File.Exists(csvFilePath);

            using (StreamWriter sw = new StreamWriter(csvFilePath, append: true))
            {
                if (isNewFile)
                {
                    await sw.WriteLineAsync("Timestamp,CharCount,WordCount,NotesToAnalyze,NotesIncorporated");
                }

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                // 1. Character & Word Counts
                int charCount = markdownContext.Length;
                int wordCount = markdownContext.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

                // 2. Note Metrics via EF Core's Local Tracker
                //int notesToAnalyze = _context.Set<Note>().Local.Count(n => n.NeedsFurtherAnalysis);
                //int notesIncorporated = _context.Set<Note>().Local.Count(n => n.IsIncorporated);

                // Append the entry
                //await sw.WriteLineAsync($"{timestamp},{charCount},{wordCount},{notesToAnalyze},{notesIncorporated}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to write stats to CSV: {ex.Message}");
        }
    }
    /*
    public NotePropertyStats GetNoteStatsByCondition(string statName, Func<Note, bool> condition)
    {
        TODO: rework using new NoteState
    }
    */
    public async Task ImportConversationsAsync(string contentPath, string metaPath)
    {
        if (_context == null) return;
        var importer = new ConversationImporter(_context, Subjects);
        await importer.ImportAsync(contentPath, metaPath);
        await SaveAsync();

        // Reload conversation collections so in-memory state reflects new rows
        await _context.Conversations.LoadAsync();
        await _context.ConversationBlocks.LoadAsync();
        await _context.ConversationSubjectCoverages.LoadAsync();
        await _context.ConversationSubjectCoverageTracks.LoadAsync();
    }

    public async Task ImportConversationsFolderAsync(string folderPath)
    {
        if (_context == null) return;
        var importer = new ConversationImporter(_context, Subjects);
        await importer.ImportFolderAsync(folderPath);
        await SaveAsync();

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

    public async Task PurgeUnassignedNotesAsync()
    {
        if (_context == null) return;

        //TODO: don't purge, but have the unassigned notes visible
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
        if (note is not null)
            Notes.Remove(note);
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