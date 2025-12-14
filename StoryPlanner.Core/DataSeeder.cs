using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Chapters.AnyAsync()) return;

        // 1. Get the Data
        var data = DesignDataFactory.CreateWorld();

        // 2. Dump it into EF Core
        // EF is smart enough to handle the graph if we add the root lists.
        // Important: We reset IDs to 0 if your DB auto-increments, 
        // OR allow identity insert. For SQLite, adding with IDs usually works fine.
    
        context.SourceMaterials.AddRange(data.Sources);
        context.Locations.AddRange(data.Locations);
        context.Characters.AddRange(data.Characters);
        context.Themes.AddRange(data.Themes);
        context.Threads.AddRange(data.Threads);
        context.Notes.AddRange(data.Notes);
        context.Chapters.AddRange(data.Chapters);

        await context.SaveChangesAsync();
    }
    public static async Task SeedOldAsync(AppDbContext context)
    {
        // 1. Check if database is already seeded
        if (await context.Chapters.AnyAsync()) return;

        // =========================================================
        // 2. CREATE DEFINITIONS (The World Bible)
        // =========================================================

        // --- SOURCES ---
        var srcCanon = new SourceMaterial { Name = "MLP: Friendship is Magic", Abbreviation = "FIM", ColorHex = "#d63384" }; // Pink
        var srcEaw = new SourceMaterial { Name = "Equestria at War (Mod)", Abbreviation = "EaW", ColorHex = "#fd7e14" }; // Orange
        var srcHoi4 = new SourceMaterial { Name = "Hearts of Iron 4", Abbreviation = "HOI4", ColorHex = "#6f42c1" }; // Purple
        var srcOriginal = new SourceMaterial { Name = "Original Fiction", Abbreviation = "OC", ColorHex = "#20c997" }; // Teal

        context.SourceMaterials.AddRange(srcCanon, srcEaw, srcHoi4, srcOriginal);
        await context.SaveChangesAsync();

        // --- LOCATIONS ---
        var locCrystalCity = new Location { Name = "The Crystal City", Region = "Crystal Empire", Description = "The capital, shielded by the Crystal Heart." };
        var locBadlands = new Location { Name = "The Badlands", Region = "Changeling Lands", Description = "Arid border region, staging ground for the invasion." };
        var locCanterlot = new Location { Name = "Canterlot", Region = "Equestria", Description = "The mountain capital of Princess Celestia." };

        context.Locations.AddRange(locCrystalCity, locBadlands, locCanterlot);
        await context.SaveChangesAsync();

        // --- CHARACTERS ---
        var charSombra = new Character { Name = "King Sombra", Archetype = "The Reformer Tyrant", Description = "Returned not as a monster, but as a hard-liner industrialist king." };
        var charTwilight = new Character { Name = "Twilight Sparkle", Archetype = "The Reluctant General", Description = "Thrust into command of the Northern Front." };
        var charChrysalis = new Character { Name = "Queen Chrysalis", Archetype = "The Warlord", Description = "Seeking resources to feed the hive." };

        context.Characters.AddRange(charSombra, charTwilight, charChrysalis);
        await context.SaveChangesAsync();

        // --- THEMES ---
        var themeWar = new Theme { Name = "Industrialized Magic", Description = "The horror of combining spells with machine guns.", ColorHex = "#dc3545" }; // Red
        var themeRedemption = new Theme { Name = "Cost of Redemption", Description = "Can Sombra actually be good, or is he just necessary?", ColorHex = "#0d6efd" }; // Blue

        context.Themes.AddRange(themeWar, themeRedemption);
        await context.SaveChangesAsync();

        // --- THREADS ---
        var threadInvasion = new StoryThread { Name = "The Changeling Offensive", Description = "The main military conflict.", ColorHex = "#198754" }; // Green
        var threadPolitics = new StoryThread { Name = "Crystal Politics", Description = "Internal strife between Sombra and the Pony representatives.", ColorHex = "#ffc107" }; // Yellow

        context.Threads.AddRange(threadInvasion, threadPolitics);
        await context.SaveChangesAsync();

        // =========================================================
        // 3. CREATE LORE NOTES (The "Why")
        // =========================================================

        var notes = new List<Note>
        {
            // Sombra's Notes
            new Note { Character = charSombra, SourceMaterial = srcCanon, Content = "Was originally a shadow monster who enslaved ponies.", IsStrictRule = true },
            new Note { Character = charSombra, SourceMaterial = srcEaw, Content = "In this timeline, he creates the 'Crystal Wehrmacht' to defend against Changelings.", IsStrictRule = false },
            
            // Chrysalis Notes
            new Note { Character = charChrysalis, SourceMaterial = srcCanon, Content = "Feeds on love.", IsStrictRule = true },
            new Note { Character = charChrysalis, SourceMaterial = srcEaw, Content = "Has tanks. Lots of tanks.", IsStrictRule = true },

            // World Rules (Codex)
            new Note { CodexEntry = new CodexEntry { Title = "Magical Exhaustion", Type = "Magic System", Description = "Rules of magic cost" }, 
                       SourceMaterial = srcOriginal, Content = "Unicorns cannot shield against artillery for more than 2 hours.", IsStrictRule = true }
        };

        context.Notes.AddRange(notes);
        await context.SaveChangesAsync();

        // =========================================================
        // 4. CREATE THE STORY (The "What")
        // =========================================================

        // --- CHAPTER 1 ---
        var chap1 = new Chapter { Title = "Chapter 1: The Northern Wind", OrderIndex = 1, Description = "The calm before the storm." };
        
        var pp1 = new PlotPoint 
        { 
            Title = "Sombra's Return", 
            Description = "Sombra walks into the council room. Everyone is terrified, but he lays down a map of the invasion plans.",
            Chapter = chap1,
            Location = locCrystalCity,
            OrderInChapter = 1,
            Intensity = 3,
            VerbatimText = "\"I did not come back to enslave you. I came back because they are coming.\"",
            Status = DraftStatus.Drafted
        };

        // Link Actors & Threads to Plot Point 1
        pp1.CharacterAppearances.Add(new PlotPointCharacter { Character = charSombra });
        pp1.CharacterAppearances.Add(new PlotPointCharacter { Character = charTwilight });
        pp1.ThreadAssignments.Add(new PlotPointThread { StoryThread = threadPolitics });
        pp1.ThemeAssignments.Add(new PlotPointTheme { Theme = themeRedemption });
        
        var pp11 = new PlotPoint 
        { 
            Title = "Sombra's Return 2", 
            Description = "This is a weird story.",
            Chapter = chap1,
            Location = locCrystalCity,
            OrderInChapter = 1,
            Intensity = 3,
            VerbatimText = "Okay.",
            Status = DraftStatus.Drafted
        };

        // Link Actors & Threads to Plot Point 1
        pp11.CharacterAppearances.Add(new PlotPointCharacter { Character = charSombra });
        pp11.CharacterAppearances.Add(new PlotPointCharacter { Character = charTwilight });
        pp11.ThreadAssignments.Add(new PlotPointThread { StoryThread = threadPolitics });
        pp11.ThemeAssignments.Add(new PlotPointTheme { Theme = themeRedemption });

        // --- CHAPTER 2 ---
        var chap2 = new Chapter { Title = "Chapter 2: Blitzkrieg", OrderIndex = 2, Description = "The war begins." };

        var pp2 = new PlotPoint 
        { 
            Title = "The Border Skirmish", 
            Description = "Chrysalis's tanks cross the border at the Badlands. The local garrison is overrun.",
            Chapter = chap2,
            Location = locBadlands,
            OrderInChapter = 1,
            Intensity = 9,
            Status = DraftStatus.Idea
        };
        
        // Link Actors & Threads to Plot Point 2
        pp2.CharacterAppearances.Add(new PlotPointCharacter { Character = charChrysalis });
        pp2.ThreadAssignments.Add(new PlotPointThread { StoryThread = threadInvasion });
        pp2.ThemeAssignments.Add(new PlotPointTheme { Theme = themeWar });

        // Add to Context
        context.Chapters.AddRange(chap1, chap2);
        context.PlotPoints.AddRange(pp1, pp2, pp11);

        await context.SaveChangesAsync();
    }
}