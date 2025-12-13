using Microsoft.EntityFrameworkCore;
using StoryPlanner.Models;
using Thread = System.Threading.Thread;

namespace StoryPlanner.Services;

public class StoryStateService : IDisposable
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private AppDbContext? _context;

    // --- The In-Memory Data Graph ---
    // The UI binds directly to these Lists
    public List<Chapter> Chapters { get; private set; } = new();
    public List<StoryThread> Threads { get; private set; } = new();
    public List<Character> Characters { get; private set; } = new();
    public List<Theme> Themes { get; private set; } = new();
    public List<PlotPoint> FloatingPlotPoints { get; private set; } = new();

    public bool IsInitialized { get; private set; } = false;
    public event Action? OnChange; // To trigger UI updates

    public StoryStateService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    // 1. Load EVERYTHING into memory
    public async Task LoadProjectAsync()
    {
        // Discard old context if reloading
        _context?.Dispose();
        _context = await _factory.CreateDbContextAsync();

        // Eagerly Load the entire graph
        // Note: We use AsSplitQuery() because loading this many joins in one query 
        // can cause a "Cartesian Explosion" in SQL.
        
        Chapters = await _context.Chapters
            .Include(c => c.PlotPoints)
                .ThenInclude(p => p.ThreadAssignments)
            .Include(c => c.PlotPoints)
                .ThenInclude(p => p.CharacterAppearances)
            .Include(c => c.PlotPoints)
                .ThenInclude(p => p.Prerequisites) // Dependency Logic
            .OrderBy(c => c.OrderIndex)
            .AsSplitQuery() 
            .ToListAsync();

        Threads = await _context.Threads.ToListAsync();
        Characters = await _context.Characters.Include(c => c.Notes).ToListAsync();
        Themes = await _context.Themes.Include(t => t.Notes).ToListAsync();
        
        // Load points that aren't in any chapter
        FloatingPlotPoints = await _context.PlotPoints
            .Where(p => p.ChapterId == null)
            .Include(p => p.ThreadAssignments)
            .ToListAsync();

        IsInitialized = true;
        NotifyStateChanged();
    }

    public Chapter GetChapter(int chapterID)
    {
        return Chapters.Where(c => c.OrderIndex == chapterID).FirstOrDefault();
    }

    // 1.5 Allow components to add items to the tracking context
    public void AddPlotPoint(PlotPoint plotPoint)
    {
        if (_context == null)
        {
            throw new InvalidOperationException("Cannot add PlotPoint: StoryStateService is not initialized. Call LoadProjectAsync() first.");
        }
        _context.PlotPoints.Add(plotPoint);
    }

    // 2. The Magic Save Button
    public async Task SaveAsync()
    {
        if (_context == null)
        {
            throw new InvalidOperationException("Cannot save: StoryStateService is not initialized. Call LoadProjectAsync() first.");
        }
        
        // EF Core already knows what changed. 
        // We just commit it to the SQLite file.
        await _context.SaveChangesAsync();
    }

    // 3. Helper to notify Blazor to re-render
    private void NotifyStateChanged() => OnChange?.Invoke();

    public void Dispose()
    {
        _context?.Dispose();
    }
}