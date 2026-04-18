using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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
            Themes = _context.Themes.Include(t => t.Notes).AsNoTracking().ToList(),
            StoryThreads = _context.StoryThreads.Include(t => t.Notes).AsNoTracking().ToList(),
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
        _context.StoryThreads.RemoveRange(_context.StoryThreads);
        _context.Themes.RemoveRange(_context.Themes);
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
        if (data.StoryThreads.Any()) _context.StoryThreads.AddRange(data.StoryThreads);
        if (data.CodexEntries.Any()) _context.CodexEntries.AddRange(data.CodexEntries);

        // Insert Hierarchy
        if (data.Chapters.Any()) _context.Chapters.AddRange(data.Chapters);

        // Note: PlotPoints are usually restored automatically because they are in the data.Chapters[i].PlotPoints collection.
        // EF Core graph fix-up will recreate the connections.

        await _context.SaveChangesAsync();
    }
    public string GetMarkdownContextForAI()
    {
        var sb = new StringBuilder();

        // =========================================================
        // 1. DATA LOADING
        // =========================================================
        var chapters = _context.Chapters
            .Include(c => c.Notes)
            .Include(c => c.PlotPoints).ThenInclude(p => p.CharacterAppearances).ThenInclude(ca => ca.Character)
            .Include(c => c.PlotPoints).ThenInclude(p => p.ThemeAssignments).ThenInclude(ta => ta.Theme)
            .Include(c => c.PlotPoints).ThenInclude(p => p.ThreadAssignments).ThenInclude(st => st.StoryThread)
            .Include(c => c.PlotPoints).ThenInclude(p => p.CodexReferences).ThenInclude(cr => cr.CodexEntry)
            .OrderBy(c => c.OrderIndex)
            .AsNoTracking()
            .ToList();

        var threads = _context.StoryThreads.Include(t => t.Notes).AsNoTracking().ToList();
        var themes = _context.Themes.Include(t => t.Notes).AsNoTracking().ToList();
        var chars = _context.Characters.Include(c => c.Notes).AsNoTracking().ToList();
        var codex = _context.CodexEntries.Include(c => c.Notes).AsNoTracking().ToList();

        // =========================================================
        // HELPER FUNCTIONS
        // =========================================================

        string Clean(string? s) => string.IsNullOrWhiteSpace(s) ? "" : s.Replace("\r\n", " ").Replace("\n", " ").Trim();

        string FormatBlock(string label, string? text, int indentLevel = 0)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";

            var normalized = text.Replace("\r\n", "\n").Trim();
            var lines = normalized.Split('\n');

            var b = new StringBuilder();
            var prefix = new string('>', indentLevel + 1) + " ";

            b.Append($"{prefix}**{label}:** {lines[0]}");

            for (int i = 1; i < lines.Length; i++)
            {
                b.AppendLine();
                if (string.IsNullOrWhiteSpace(lines[i]))
                    b.Append(prefix.Trim());
                else
                    b.Append($"{prefix}{lines[i]}");
            }

            b.AppendLine();
            return b.ToString();
        }

        // =========================================================
        // PART 1: WORLD BIBLE (DEFINITIONS)
        // =========================================================
        sb.AppendLine("# PART 1: WORLD BIBLE");

        sb.AppendLine("## CHARACTERS");
        foreach (var c in chars)
        {
            // NOTE: We keep definitions plain (### Bob) so they act as anchors.
            // If you want them clickable in Obsidian, you could use ### [[Bob]]
            // but standard Markdown usually prefers plain headers.
            sb.AppendLine($"### {c.Name}");
            if (!string.IsNullOrWhiteSpace(c.Archetype)) sb.AppendLine($"* **Archetype:** {c.Archetype}");
            if (!string.IsNullOrWhiteSpace(c.Description)) sb.Append(FormatBlock("Desc", c.Description));
            foreach (var n in c.Notes) sb.Append(FormatBlock($"Note {n.SortOrder}", n.Content));
            sb.AppendLine();
        }

        sb.AppendLine("## CODEX ENTRIES");
        foreach (var c in codex)
        {
            sb.AppendLine($"### {c.Title}");
            if (!string.IsNullOrWhiteSpace(c.Category.ToString())) sb.AppendLine($"* **Category:** {c.Category}");
            if (!string.IsNullOrWhiteSpace(c.Description)) sb.Append(FormatBlock("Desc", c.Description));
            foreach (var n in c.Notes) sb.Append(FormatBlock($"Note {n.SortOrder}", n.Content));
            sb.AppendLine();
        }

        sb.AppendLine("## THEMES");
        foreach (var t in themes)
        {
            sb.AppendLine($"### {t.Name}");
            if (!string.IsNullOrWhiteSpace(t.Description)) sb.Append(FormatBlock("Desc", t.Description));
            foreach (var n in t.Notes) sb.Append(FormatBlock($"Note {n.SortOrder}", n.Content));
            sb.AppendLine();
        }

        sb.AppendLine("## STORY THREADS");
        foreach (var t in threads)
        {
            sb.AppendLine($"### {t.Icon} {t.Name}");
            sb.AppendLine($"* **Scope:** {t.ThreadScope}");
            if (!string.IsNullOrWhiteSpace(t.Description)) sb.Append(FormatBlock("Desc", t.Description));
            foreach (var n in t.Notes) sb.Append(FormatBlock($"Note {n.SortOrder}", n.Content));
            sb.AppendLine();
        }

        // =========================================================
        // PART 2: NARRATIVE TIMELINE (REFERENCES)
        // =========================================================
        sb.AppendLine("# PART 2: NARRATIVE TIMELINE");

        foreach (var c in chapters)
        {
            sb.AppendLine($"## CHAPTER {c.OrderIndex}: {c.Title}");

            if (!string.IsNullOrWhiteSpace(c.Summary)) sb.Append(FormatBlock("Summary", c.Summary));
            foreach (var n in c.Notes) sb.Append(FormatBlock("Chapter Note", n.Content));

            sb.AppendLine();
            int sceneNum = 1;

            foreach (var p in c.PlotPoints.OrderBy(x => x.OrderInChapter))
            {
                sb.AppendLine($"### SCENE {c.OrderIndex}.{sceneNum}: {p.Title}");

                // --- A. THE NARRATIVE ---
                if (!string.IsNullOrWhiteSpace(p.Stakes)) sb.Append(FormatBlock("Stakes", p.Stakes));
                if (!string.IsNullOrWhiteSpace(p.Synopsis)) sb.Append(FormatBlock("Synopsis", p.Synopsis));
                if (!string.IsNullOrWhiteSpace(p.Outcome)) sb.Append(FormatBlock("Outcome", p.Outcome));

                sb.AppendLine();

                // --- B. THE PAYLOADS (WIKIFIED) ---

                // 1. Characters
                var activeChars = p.CharacterAppearances.Where(x => x.Character != null).ToList();
                if (activeChars.Any())
                {
                    sb.AppendLine("**Characters:**");
                    foreach (var ca in activeChars)
                    {
                        var details = new List<string>();
                        if (ca.Role != 0) details.Add(ca.Role.ToString());
                        if (ca.DevelopmentImpact != 0) details.Add($"Impact: {ca.DevelopmentImpact}");

                        // CHANGE: Wrapped Name in [[ ]]
                        var header = $"* **[[{ca.Character!.Name}]]**";
                        if (details.Any()) header += $" ({string.Join(", ", details)})";
                        sb.AppendLine(header);

                        if (!string.IsNullOrWhiteSpace(ca.DevelopmentNote))
                        {
                            sb.Append(FormatBlock("Analysis", ca.DevelopmentNote));
                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine();
                }

                // 2. Threads
                var activeThreads = p.ThreadAssignments.Where(x => x.StoryThread != null).ToList();
                if (activeThreads.Any())
                {
                    sb.AppendLine("**Threads:**");
                    foreach (var th in activeThreads)
                    {
                        // CHANGE: Wrapped Name in [[ ]]
                        var header = $"* **[[{th.StoryThread!.Name}]]**";
                        if (th.ThreadTrajectory != 0) header += $" ({th.ThreadTrajectory})";
                        sb.AppendLine(header);

                        if (!string.IsNullOrWhiteSpace(th.ImpactDescription))
                        {
                            sb.Append(FormatBlock("Update", th.ImpactDescription));
                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine();
                }

                // 3. Themes
                var activeThemes = p.ThemeAssignments.Where(x => x.Theme != null).ToList();
                if (activeThemes.Any())
                {
                    sb.AppendLine("**Themes:**");
                    foreach (var th in activeThemes)
                    {
                        // CHANGE: Wrapped Name in [[ ]]
                        var header = $"* **[[{th.Theme!.Name}]]**";
                        if (th.Prominence != 0) header += $" ({th.Prominence})";
                        sb.AppendLine(header);

                        if (!string.IsNullOrWhiteSpace(th.Commentary))
                        {
                            sb.Append(FormatBlock("Commentary", th.Commentary));
                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine();
                }

                // 4. Codex
                var activeCodex = p.CodexReferences.Where(x => x.CodexEntry != null).ToList();
                if (activeCodex.Any())
                {
                    sb.AppendLine("**Codex:**");
                    foreach (var cr in activeCodex)
                    {
                        // CHANGE: Wrapped Title in [[ ]]
                        var header = $"* **[[{cr.CodexEntry!.Title}]]**";
                        if (!string.IsNullOrWhiteSpace(cr.CodexEntry.Category.ToString()))
                            header += $" ({cr.CodexEntry.Category})";
                        sb.AppendLine(header);

                        if (!string.IsNullOrWhiteSpace(cr.Commentary))
                        {
                            sb.Append(FormatBlock("Note", cr.Commentary));
                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine();
                }

                sb.AppendLine("---");
                sceneNum++;
            }
        }

        return sb.ToString();
    }
}