using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core;

public class StoryFileService
{
    private readonly AppDbContext _context;

    public StoryFileService(AppDbContext context)
    {
        _context = context;
    }

    // =========================================================================
    // 1. FULL DATABASE BACKUP (JSON)
    // =========================================================================

    public string ExportFullDatabase()
    {
        // A. Load ALL relationships to ensure deep serialization
        var data = new StoryProjectData
        {
            Title = "Story Backup",
            ExportDate = DateTime.Now,
            
            // 1. Core Narrative
            Chapters = _context.Chapters
                .Include(c => c.Notes)
                .Include(c => c.PlotPoints)
                    .ThenInclude(pp => pp.ThreadAssignments)
                .Include(c => c.PlotPoints)
                    .ThenInclude(pp => pp.CharacterAppearances)
                .Include(c => c.PlotPoints)
                    .ThenInclude(pp => pp.ThemeAssignments)
                .Include(c => c.PlotPoints)
                    .ThenInclude(pp => pp.CodexReferences)
                .Include(c => c.PlotPoints) // Include child notes if PlotPoints have them (Not in your model currently, but good practice)
                .AsNoTracking()
                .ToList(),

            // 2. World Entities (Include Notes for all)
            Characters = _context.Characters.Include(c => c.Notes).AsNoTracking().ToList(),
            Locations = _context.Locations.Include(l => l.Notes).Include(l => l.PlotPoints).AsNoTracking().ToList(),
            Themes = _context.Themes.Include(t => t.Notes).AsNoTracking().ToList(),
            StoryThreads = _context.Threads.Include(t => t.Notes).AsNoTracking().ToList(),
            CodexEntries = _context.CodexEntries.Include(c => c.Notes).AsNoTracking().ToList(),
            
            // 3. Meta
            SourceMaterials = _context.SourceMaterials.Include(s => s.Notes).AsNoTracking().ToList(),
            GeminiEntries = _context.GeminiEntries.AsNoTracking().ToList(),
            Ideas = _context.Ideas.AsNoTracking().ToList()
        };

        // B. Serialize with Preserved References (Handles Circular Links safely)
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve, // Critical for Entity Framework graphs
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        return JsonSerializer.Serialize(data, options);
    }

    public async Task ImportFullDatabaseAsync(string jsonContent)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var data = JsonSerializer.Deserialize<StoryProjectData>(jsonContent, options);
        if (data == null) throw new Exception("Failed to deserialize story file.");

        // A. WIPE EXISTING DATABASE (Clear the slate)
        // We delete in reverse dependency order where possible to avoid FK conflicts, 
        // though EF Core usually handles simple cascades.
        _context.PlotPointCharacters.RemoveRange(_context.PlotPointCharacters);
        _context.PlotPointThemes.RemoveRange(_context.PlotPointThemes);
        _context.PlotPointThreads.RemoveRange(_context.PlotPointThreads);
        _context.PlotPointCodexEntries.RemoveRange(_context.PlotPointCodexEntries);
        
        _context.Notes.RemoveRange(_context.Notes); // Delete all notes first
        
        _context.PlotPoints.RemoveRange(_context.PlotPoints);
        _context.Chapters.RemoveRange(_context.Chapters);
        
        _context.Characters.RemoveRange(_context.Characters);
        _context.Threads.RemoveRange(_context.Threads);
        _context.Themes.RemoveRange(_context.Themes);
        _context.Locations.RemoveRange(_context.Locations);
        _context.CodexEntries.RemoveRange(_context.CodexEntries);
        _context.SourceMaterials.RemoveRange(_context.SourceMaterials);
        _context.GeminiEntries.RemoveRange(_context.GeminiEntries);
        _context.Ideas.RemoveRange(_context.Ideas);

        await _context.SaveChangesAsync();

        // B. RESTORE DATA
        // EF Core allows inserting entities with explicit IDs.
        
        if (data.SourceMaterials.Any()) _context.SourceMaterials.AddRange(data.SourceMaterials);
        if (data.GeminiEntries.Any()) _context.GeminiEntries.AddRange(data.GeminiEntries);
        if(data.Ideas.Any()) _context.Ideas.AddRange(data.Ideas);
        
        // Insert Reference Entities
        if (data.Characters.Any()) _context.Characters.AddRange(data.Characters);
        if (data.Themes.Any()) _context.Themes.AddRange(data.Themes);
        if (data.StoryThreads.Any()) _context.Threads.AddRange(data.StoryThreads);
        if (data.Locations.Any()) _context.Locations.AddRange(data.Locations);
        if (data.CodexEntries.Any()) _context.CodexEntries.AddRange(data.CodexEntries);

        // Insert Hierarchy
        if (data.Chapters.Any()) _context.Chapters.AddRange(data.Chapters);

        // Note: PlotPoints are usually restored automatically because they are in the data.Chapters[i].PlotPoints collection.
        // EF Core graph fix-up will recreate the connections.

        await _context.SaveChangesAsync();
    }

    // =========================================================================
    // 2. AI CONTEXT EXPORT (Optimized for Tokens)
    // =========================================================================

    public string GetContextForAI(bool includeVerbatimText)
    {
        // 1. Fetch raw data (No Tracking for speed)
        var chapters = _context.Chapters
            .Include(c => c.PlotPoints).ThenInclude(p => p.ThreadAssignments).ThenInclude(pt => pt.StoryThread)
            .Include(c => c.PlotPoints).ThenInclude(p => p.CharacterAppearances).ThenInclude(pc => pc.Character)
            .Include(c => c.PlotPoints).ThenInclude(p => p.ThemeAssignments).ThenInclude(pt => pt.Theme)
            .AsNoTracking().ToList();

        var characters = _context.Characters.AsNoTracking().ToList();
        var threads = _context.Threads.AsNoTracking().ToList();
        var themes = _context.Themes.AsNoTracking().ToList();
        var lore = _context.CodexEntries.AsNoTracking().ToList();
        var locations = _context.Locations.AsNoTracking().ToList();

        // 2. Project into a Clean DTO (stripping Colors, IDs, Icons, etc.)
        var aiContext = new
        {
            Context = "Story Bible",
            
            // A. The Cast
            Characters = characters.Select(c => new 
            {
                c.Name,
                c.Archetype, 
                c.Description, 
                c.Inspiration // Often helpful for AI to know "Han Solo type"
            }),

            // B. The Themes
            Themes = themes.Select(t => new 
            {
                t.Name, 
                t.Description 
                // Excluded: ColorHex, Abbreviation
            }),

            // C. The Threads (Plots)
            PlotLines = threads.Select(t => new 
            {
                t.Name, 
                t.Description, 
                Scope = t.ThreadScope.ToString()
                // Excluded: Icon
            }),

            // D. The Lore
            Glossary = lore.Select(l => new
            {
                l.Title,
                l.Type,
                l.Description
            }),

            // E. The Timeline (The most important part)
            Timeline = chapters.Select(c => new
            {
                Chapter = $"Chapter {c.OrderIndex}: {c.Title}",
                Scenes = c.PlotPoints.OrderBy(p => p.OrderInChapter).Select(p => new
                {
                    Title = p.Title,
                    Context = $"Where: {p.Location?.Name ?? "Unknown"}. When: {p.WorldDate ?? "Unspecified"}",
                    
                    // The core 5Ws
                    Role = p.CoreDriver.ToString(),
                    Synopsis = p.Synopsis,
                    Input_Stakes = p.Stakes,
                    Output_Outcome = p.Outcome,

                    // Conditional Verbatim
                    DraftText = includeVerbatimText ? p.VerbatimText : null,

                    // Flat lists of names (Saves tokens compared to full objects)
                    Characters = p.CharacterAppearances.Select(x => x.Character?.Name).Where(n => n != null).ToList(),
                    Threads = p.ThreadAssignments.Select(x => x.StoryThread?.Name).Where(n => n != null).ToList(),
                    Themes = p.ThemeAssignments.Select(x => x.Theme?.Name).Where(n => n != null).ToList()
                })
            })
        };

        // 3. Serialize cleanly
        return JsonSerializer.Serialize(aiContext, new JsonSerializerOptions 
        { 
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Skips 'DraftText' if null
        });
    }
}