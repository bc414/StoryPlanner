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
    public ObservableCollection<Note> Notes { get; private set; } = new();
    public ObservableCollection<SubjectDefinition> SubjectDefinitions { get; private set; } = new();
    public ObservableCollection<NoteTrackDefinition> NoteTrackDefinitions { get; private set; } = new();
    public ObservableCollection<NarrativePropertyDefinition> NarrativePropertyDefinitions { get; private set; } = new();
    public ObservableCollection<NarrativePropertyValueDefinition> NarrativePropertyValueDefinitions { get; private set; } = new();
    public ObservableCollection<NarrativePropertyValue> NarrativePropertyValues { get; private set; } = new();

    public ObservableCollection<SourceMaterial> SourceMaterials { get; private set; } = new();

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

    public string GetAiContextJson(bool includeVerbatim)
    {
        return string.Empty;
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

        await _context.Set<Note>()
            .LoadAsync();

        await _context.PlotPoints
            .LoadAsync();

        await _context.Chapters.OrderBy(c => c.OrderIndex).LoadAsync();

        // Definitions — load leaves first so EF relationship fixup wires nav properties
        await _context.NoteTrackDefinitions.LoadAsync();
        await _context.NarrativePropertyValueDefinitions.LoadAsync();
        await _context.NarrativePropertyDefinitions.LoadAsync();   // nav → NarrativePropertyValueDefinitions
        await _context.SubjectDefinitions.LoadAsync();             // nav → NoteTrackDefinitions, NarrativePropertyDefinitions
        await _context.NarrativePropertyValues.LoadAsync();        // FK refs only, no nav props

        await _context.SourceMaterials.LoadAsync();
        await _context.GeminiEntries.LoadAsync();
        await _context.Ideas.LoadAsync();

        // ---------------------------------------------------------------------------
        // STEP 4: BIND TO UI
        // ---------------------------------------------------------------------------
        Chapters = _context.Chapters.Local.ToObservableCollection();
        PlotPoints = _context.PlotPoints.Local.ToObservableCollection();

        SubjectDefinitions           = _context.SubjectDefinitions.Local.ToObservableCollection();
        NoteTrackDefinitions         = _context.NoteTrackDefinitions.Local.ToObservableCollection();
        NarrativePropertyDefinitions = _context.NarrativePropertyDefinitions.Local.ToObservableCollection();
        NarrativePropertyValueDefinitions = _context.NarrativePropertyValueDefinitions.Local.ToObservableCollection();
        NarrativePropertyValues      = _context.NarrativePropertyValues.Local.ToObservableCollection();

        SourceMaterials = _context.SourceMaterials.Local.ToObservableCollection();
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
                !n.CodexEntryId.HasValue && 
                !n.ChapterId.HasValue && 
                !n.StoryThreadId.HasValue)
        };
    }

    public void DeleteNote(Note note)
    {
        if (_context != null)
        {
            // 1. Handle EF Core Tracking
            if (note.Id == 0)
            {
                // The note is new and unsaved. Just detach it from EF tracking.
                _context.Entry(note).State = EntityState.Detached;
            }
            else
            {
                // The note exists in the database. Mark it for deletion.
                _context.Notes.Remove(note);
            }

            // 2. Clean up the UI Collection
            // Ensure the note is removed from your unassigned list so the UI updates
            // (Since your CollectionChanged event only handles NewItems, not OldItems)
            if (UnassignedNotes != null && UnassignedNotes.Contains(note))
            {
                UnassignedNotes.Remove(note);
            }
        }
    }


    public async Task PurgeUnassignedNotesAsync()
    {
        if (_context == null) return;

        var unassignedList = _context.Set<Note>().Local.Where(n =>
            !n.CharacterId.HasValue &&
            !n.ThemeId.HasValue &&
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