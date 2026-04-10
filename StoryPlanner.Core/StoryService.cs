using System.Collections.ObjectModel;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Markdig;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public class StoryService : IStoryService
{
    private AppDbContext? _context;
    private string _googleDocId = "1rr3SASDp85y2sYzkXVvdgkOLFp18-6LbiyPFAoKrSgw";

    // --- The In-Memory Data Graph ---
    public ObservableCollection<Chapter> Chapters { get; private set; } = new();
    public ObservableCollection<StoryThread> StoryThreads { get; private set; } = new();
    public ObservableCollection<Character> Characters { get; private set; } = new();
    public ObservableCollection<Theme> Themes { get; private set; } = new();
    
    // --- NEW ENTITIES ---
    public ObservableCollection<Location> Locations { get; private set; } = new();
    public ObservableCollection<CodexEntry> CodexEntries { get; private set; } = new();
    public ObservableCollection<SourceMaterial> SourceMaterials { get; private set; } = new();

    // All plot points (to be filtered later)
    public ObservableCollection<PlotPoint> PlotPoints { get; private set; } = new();

    public ObservableCollection<GeminiEntry> GeminiEntries { get; private set; } = new();
    public ObservableCollection<Idea> Ideas { get; private set; } = new();

    public ObservableCollection<Note> UnassignedNotes { get; private set; } = new();

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

    public async Task RestoreProjectFromJsonAsync(string json)
    {
        if (_context == null) throw new InvalidOperationException("Project not loaded");
        var fileService = new StoryFileService(_context);
        
        await fileService.ImportFullDatabaseAsync(json);
        
        // CRITICAL: Reload the data into the ObservableCollections
        // so the UI updates immediately
        await LoadDataAsync();
    }

    public string GetAiContextJson(bool includeVerbatim)
    {
        if (_context == null) throw new InvalidOperationException("Project not loaded");
        var fileService = new StoryFileService(_context);
        //string a = fileService.GetContextForAI(false);
        string b = fileService.GetOptimizedContextForAINew();
        //string c = fileService.GetSuperOptimizedContextForAI();

        //string c = fileService.GetMarkdownContextForAI();

        return b;
    }

    public string GetMarkdown()
    {
        if (_context == null) throw new InvalidOperationException("Project not loaded");
        var fileService = new StoryFileService(_context);
        return fileService.GetMarkdownContextForAI();
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
        await DataSeeder.SeedAsync(_context);

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

        // Ensure schema is compatible
        await _context.Database.MigrateAsync();

        await LoadDataAsync();
    }
    
    private void CreateSafetyBackup(string originalPath)
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

    private void CleanUpOldBackups(string backupFolder)
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
        // ---------------------------------------------------------------------------
        // STEP 1: LOAD ALL NOTES (The Atoms)
        // ---------------------------------------------------------------------------
        // We load every single note in the database.
        // Because Notes are polymorphic (they can belong to Themes, Characters, etc.),
        // loading them first ensures that when we load the "Containers" later, 
        // their .Notes collections will instantly populate via Fix-Up.
        await _context.Set<Note>()
            .LoadAsync();

        var unassignedList = _context.Set<Note>().Local.Where(n =>
            !n.CharacterId.HasValue &&
            !n.ThemeId.HasValue &&
            !n.LocationId.HasValue &&
            !n.CodexEntryId.HasValue &&
            !n.ChapterId.HasValue &&
            !n.StoryThreadId.HasValue).ToList();

        UnassignedNotes = new ObservableCollection<Note>(unassignedList);

        // 3. Wire up the auto-sync to the Database Context
        UnassignedNotes.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null)
                foreach (Note n in e.NewItems) _context.Notes.Add(n);

            if (e.OldItems != null)
                foreach (Note n in e.OldItems) _context.Notes.Remove(n);
        };

        // ---------------------------------------------------------------------------
        // STEP 2: LOAD ALL PLOT POINTS (The Narrative Units)
        // ---------------------------------------------------------------------------
        // We load EVERY PlotPoint (both Chapter-bound and Floating).
        // We include every possible connection here. 
        await _context.PlotPoints
            // 1. Geography
            .Include(p => p.Location) 
            // 2. Threads (Junction Table + The Thread itself)
            .Include(p => p.ThreadAssignments).ThenInclude(t => t.StoryThread)
            // 3. Themes (Junction Table + The Theme itself)
            .Include(p => p.ThemeAssignments).ThenInclude(t => t.Theme)
            // 4. Characters (Junction Table + The Character itself)
            .Include(p => p.CharacterAppearances).ThenInclude(c => c.Character)
            .Include(p => p.CodexReferences).ThenInclude(c => c.CodexEntry)
            .Include(p => p.Chapter)
            .AsSplitQuery() // Essential to prevent Cartesian Explosion
            .LoadAsync();

        // ---------------------------------------------------------------------------
        // STEP 3: LOAD THE CONTAINERS (The Buckets)
        // ---------------------------------------------------------------------------
        // We don't need .Include() here! 
        // Why? Because the Children (Notes and PlotPoints) are already in memory.
        // EF Core will see "Chapter 1" and "PlotPoint 5 (ChapterId=1)" and connect them.

        await _context.Chapters.OrderBy(c => c.OrderIndex).LoadAsync();
        await _context.Threads.LoadAsync();
        await _context.Characters.LoadAsync();
        await _context.Themes.LoadAsync();
        await _context.Locations.LoadAsync();
        await _context.CodexEntries.LoadAsync();
        await _context.SourceMaterials.LoadAsync();
        await _context.GeminiEntries.LoadAsync();
        await _context.Ideas.LoadAsync();

        // ---------------------------------------------------------------------------
        // STEP 4: BIND TO UI
        // ---------------------------------------------------------------------------
        Chapters = _context.Chapters.Local.ToObservableCollection();
        StoryThreads = _context.Threads.Local.ToObservableCollection();
        Characters = _context.Characters.Local.ToObservableCollection();
        Themes = _context.Themes.Local.ToObservableCollection();
        Locations = _context.Locations.Local.ToObservableCollection();
        CodexEntries = _context.CodexEntries.Local.ToObservableCollection();
        SourceMaterials = _context.SourceMaterials.Local.ToObservableCollection();
        PlotPoints = _context.PlotPoints.Local.ToObservableCollection();
        GeminiEntries = _context.GeminiEntries.Local.ToObservableCollection();
        Ideas  = _context.Ideas.Local.ToObservableCollection();

        IsProjectLoaded = true;
    }

    public async Task SaveAsync()
    {
        if (_context == null) throw new InvalidOperationException("Not initialized");
        // FORCE EF Core to look at all objects in memory and detect changes
        _context.ChangeTracker.DetectChanges();
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
                int notesToAnalyze = _context.Set<Note>().Local.Count(n => n.NeedsFurtherAnalysis);
                int notesIncorporated = _context.Set<Note>().Local.Count(n => n.IsIncorporated);

                // Append the entry
                await sw.WriteLineAsync($"{timestamp},{charCount},{wordCount},{notesToAnalyze},{notesIncorporated}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to write stats to CSV: {ex.Message}");
        }
    }
    
    private async Task SyncToGoogleDocsAsync(string markdownText, string documentTitle)
    {
        try
        {
            // 1. Authenticate (Assumes a Service Account for seamless background saving)
            // You will need to generate a service-account.json from Google Cloud Console
            GoogleCredential credential;
            using (var stream = new FileStream("service-account.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(DriveService.Scope.DriveFile);
            }

            // 2. Create Drive Service
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "StoryPlanner"
            });

            // 3. Convert Markdown to HTML for Native Google Doc Formatting
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            string htmlContent = Markdown.ToHtml(markdownText, pipeline);

            // 4. Prepare File Stream
            using var byteArray = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlContent));

            if (string.IsNullOrEmpty(_googleDocId))
            {
                // CREATE NEW DOCUMENT
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = documentTitle,
                    MimeType = "application/vnd.google-apps.document" // Triggers conversion to Native Google Doc
                };

                var request = service.Files.Create(fileMetadata, byteArray, "text/html");
                request.Fields = "id, webViewLink";
                
                var progress = await request.UploadAsync();
                if (progress.Status == UploadStatus.Completed)
                {
                    _googleDocId = request.ResponseBody.Id;
                    System.Diagnostics.Debug.WriteLine($"Created Google Doc: {request.ResponseBody.WebViewLink}");
                }
            }
            else
            {
                // UPDATE EXISTING DOCUMENT (Overwrites content with the new save)
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = documentTitle 
                };

                var request = service.Files.Update(fileMetadata, _googleDocId, byteArray, "text/html");
                var progress = await request.UploadAsync();
                
                if (progress.Status == UploadStatus.Completed)
                {
                    System.Diagnostics.Debug.WriteLine($"Updated existing Google Doc: {_googleDocId}");
                }
            }
        }
        catch (Exception ex)
        {
            // Fail silently so a network error doesn't crash your local save
            System.Diagnostics.Debug.WriteLine($"Google Drive Sync Failed: {ex.Message}");
        }
    }
    
    // Inside StoryService.cs

    public NotePropertyStats GetNoteStatsByCondition(string statName, Func<Note, bool> condition)
    {
        // Return an empty/zeroed stats object if context isn't loaded
        if (_context == null) return new NotePropertyStats { StatName = statName };

        // Apply the generic condition to the local cache
        var filteredNotes = _context.Set<Note>().Local
            .Where(condition)
            .ToList();

        return new NotePropertyStats
        {
            StatName = statName,
            Total = filteredNotes.Count,
        
            // Tally based on the foreign keys
            Characters = filteredNotes.Count(n => n.CharacterId.HasValue),
            Themes = filteredNotes.Count(n => n.ThemeId.HasValue),
            Codex = filteredNotes.Count(n => n.CodexEntryId.HasValue),
            Chapters = filteredNotes.Count(n => n.ChapterId.HasValue),
            Threads = filteredNotes.Count(n => n.StoryThreadId.HasValue),
        
            // Catch notes without a parent
            Unassigned = filteredNotes.Count(n => 
                !n.CharacterId.HasValue && 
                !n.ThemeId.HasValue && 
                !n.LocationId.HasValue && 
                !n.CodexEntryId.HasValue && 
                !n.ChapterId.HasValue && 
                !n.StoryThreadId.HasValue)
        };
    }

    public void DeleteNote(Note note)
    {
        if (_context != null)
        {
            _context.Notes.Remove(note);
        }
    }


    public async Task PurgeUnassignedNotesAsync()
    {
        if (_context == null) return;

        var unassignedList = _context.Set<Note>().Local.Where(n =>
            !n.CharacterId.HasValue &&
            !n.ThemeId.HasValue &&
            !n.LocationId.HasValue &&
            !n.CodexEntryId.HasValue &&
            !n.ChapterId.HasValue &&
            !n.StoryThreadId.HasValue).ToList();

        _context.Notes.RemoveRange(unassignedList);

        if (UnassignedNotes != null)
        {
            UnassignedNotes.Clear();
        }

        await SaveAsync();
    }

    public IEnumerable<IAuditableText> GetAllAuditableTexts()
    {
        if (_context == null) yield break;

        foreach (var note in _context.Set<Note>().Local) yield return note;
        
        foreach (var plotPoint in _context.Set<PlotPoint>().Local)
        {
            yield return plotPoint;
            foreach (var thread in plotPoint.ThreadAssignments) yield return thread;
            foreach (var theme in plotPoint.ThemeAssignments) yield return theme;
            foreach (var charApp in plotPoint.CharacterAppearances) yield return charApp;
            foreach (var codex in plotPoint.CodexReferences) yield return codex;
        }
    }


    public void Dispose()
    {
        _context?.Dispose();
    }
}