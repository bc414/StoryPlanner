using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public class StoryService : IStoryService
{
    private AppDbContext? _context;

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
        //return fileService.GetContextForAI(false);
        return fileService.GetOptimizedContextForAI();
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
        // (This handles cases where the UI updated a property but EF didn't notice)
        _context.ChangeTracker.DetectChanges();
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}