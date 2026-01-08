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
            .Include(c => c.Notes)
            .Include(c => c.PlotPoints).ThenInclude(p => p.ThreadAssignments).ThenInclude(pt => pt.StoryThread)
            .Include(c => c.PlotPoints).ThenInclude(p => p.CharacterAppearances).ThenInclude(pc => pc.Character)
            .Include(c => c.PlotPoints).ThenInclude(p => p.ThemeAssignments).ThenInclude(pt => pt.Theme)
            .Include(c => c.PlotPoints).ThenInclude(p => p.CodexReferences).ThenInclude(pt => pt.CodexEntry)
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
            StoryThreads = threads.Select(t => new 
            {
                t.Name, 
                t.Description, 
                Scope = t.ThreadScope.ToString()
                // Excluded: Icon
            }),

            // D. The Lore
            CodexEntries = lore.Select(l => new
            {
                l.Title,
                l.Type,
                l.Description
            }),

            // E. The Timeline (The most important part)
            Chapters = chapters.Select(c => new
            {
                Chapter = $"Chapter {c.OrderIndex}: {c.Title}",
                PlotPoints = c.PlotPoints.OrderBy(p => p.OrderInChapter).Select(p => new
                {
                    Title = p.Title,
                    
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
                    Themes = p.ThemeAssignments.Select(x => x.Theme?.Name).Where(n => n != null).ToList(),
                    CodexEntries = p.CodexReferences.Select(x => x.CodexEntry?.Title).Where(n => n != null).ToList(),
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

    public string GetOptimizedContextForAI()
    {
        // Helper function to project notes safely to save tokens
        // Using "N" as the key for notes lists to be ultra-compact
        Func<IEnumerable<Note>, IEnumerable<string>?> projectNotes = (notes) =>
            notes != null && notes.Any()
                ? notes.Select(n => n.Content)
                : null;

        // 1. Load data (No changes to EF logic)
        var chapters = _context.Chapters
            .Include(c => c.Notes)
            .Include(c => c.PlotPoints).ThenInclude(p => p.CharacterAppearances).ThenInclude(ca => ca.Character)
            .Include(c => c.PlotPoints).ThenInclude(p => p.ThemeAssignments).ThenInclude(ta => ta.Theme)
            .Include(c => c.PlotPoints).ThenInclude(p => p.ThreadAssignments).ThenInclude(st => st.StoryThread)
            .Include(c => c.PlotPoints).ThenInclude(p => p.CodexReferences).ThenInclude(cr => cr.CodexEntry)
            .OrderBy(c => c.OrderIndex)
            .ToList();

        // 2. Project to a clean structure (DTO)
        // ABBREVIATION KEY:
        // Ch = Chapter, Sc = Scenes, Ti = Title, Syn = Synopsis, N = Notes
        // C = Characters, n = Name, r = Role, d = DevelopmentNote
        // Thm = Themes, c = Commentary
        // Thr = Threads, tr = Trajectory, imp = Impact
        // Ref = CodexReferences

        var narrativePayload = chapters.Select(c => new
        {
            Ch = c.Title,
            N = projectNotes(c.Notes),

            Sc = c.PlotPoints.OrderBy(p => p.OrderInChapter).Select(p => new
            {
                Ti = p.Title,
                Syn = string.IsNullOrWhiteSpace(p.Synopsis) ? null : p.Synopsis,

                // Characters -> C
                C = p.CharacterAppearances
                    .Where(ca => ca.Character != null)
                    .Select(ca => new
                    {
                        n = ca.Character.Name,
                        r = ca.Role == 0 ? null : ca.Role.ToString(),
                        d = string.IsNullOrWhiteSpace(ca.DevelopmentNote) ? null : ca.DevelopmentNote
                    }),

                // Themes -> Thm (Polymorphic: String or Object)
                Thm = p.ThemeAssignments
                    .Where(ta => ta.Theme != null)
                    .Select(ta =>
                        string.IsNullOrWhiteSpace(ta.Commentary)
                        ? (object)ta.Theme.Name
                        : new { n = ta.Theme.Name, c = ta.Commentary }
                    ),

                // Threads -> Thr
                Thr = p.ThreadAssignments
                    .Where(th => th.StoryThread != null)
                    .Select(th => new
                    {
                        n = th.StoryThread.Name,
                        traj = th.ThreadTrajectory, // Shortened from ThreadTrajectory
                        imp = string.IsNullOrWhiteSpace(th.ImpactDescription) ? null : th.ImpactDescription
                    }),

                // Codex -> Ref
                Ref = p.CodexReferences
                    .Where(cr => cr.CodexEntry != null)
                    .Select(cr => new
                    {
                        n = cr.CodexEntry.Title,
                        c = cr.Commentary
                    })
            })
        });

        // =========================================================
        // PART 2: THE WORLD CONTEXT
        // =========================================================

        // We keep Context keys slightly more descriptive (Desc, Arch) 
        // because this is the "System Prompt" portion where clarity is king.
        var worldPayload = new
        {
            Threads = _context.Threads.AsNoTracking().Select(t => new
            {
                t.Name,
                Desc = t.Description, // Shortened from Description
                N = projectNotes(t.Notes)
            }),

            Themes = _context.Themes.AsNoTracking().Select(t => new
            {
                t.Name,
                Desc = t.Description,
                N = projectNotes(t.Notes)
            }),

            Chars = _context.Characters.AsNoTracking().Select(c => new
            {
                c.Name,
                Arch = c.Archetype, // Shortened from Archetype
                Desc = c.Description,
                N = projectNotes(c.Notes)
            }),

            Codex = _context.CodexEntries.AsNoTracking().Select(c => new
            {
                Title = c.Title,
                Content = c.Description,
                N = projectNotes(c.Notes)
            })
        };

        // =========================================================
        // PART 3: COMBINE AND SERIALIZE
        // =========================================================

        var finalPayload = new
        {
            // SELF-DOCUMENTING LEGEND:
            // This ensures the LLM knows exactly what the short keys mean.
            // It provides a schema definition within the data itself.
            Legend = "KEYS: Ch=Chapter, Sc=Scenes, Ti=Title, Syn=Synopsis, N=Notes. " +
                 "ENTITIES: C=Characters(n=Name,r=Role,d=DevNote), Thm=Themes(n=Name,c=Commentary), " +
                 "Thr=Threads(traj=Trajectory,imp=Impact), Ref=Codex.",

            Context = worldPayload,
            Story = narrativePayload
        };

        return JsonSerializer.Serialize(finalPayload, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            // Optional: Ensure non-ASCII characters (if any) don't get escaped to \uXXXX, saving tokens
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}