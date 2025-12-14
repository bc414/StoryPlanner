namespace StoryPlanner.Core.Models;

// 1. A simple container to hold your entire object graph
public record StoryWorldData(
    List<SourceMaterial> Sources,
    List<Location> Locations,
    List<Character> Characters,
    List<Theme> Themes,
    List<StoryThread> Threads,
    List<Note> Notes,
    List<Chapter> Chapters
);

public static class DesignDataFactory
{
    public static Note SampleNote
    {
        get
        {
            return CreateWorld().Notes.First();
        }
    }
    public static StoryWorldData CreateWorld()
    {
        // --- SOURCES ---
        var srcCanon = new SourceMaterial { SourceMaterialId = 1, Name = "MLP: Friendship is Magic", Abbreviation = "FIM", ColorHex = "#d63384" };
        var srcEaw = new SourceMaterial { SourceMaterialId = 2, Name = "Equestria at War (Mod)", Abbreviation = "EaW", ColorHex = "#fd7e14" };
        var srcHoi4 = new SourceMaterial { SourceMaterialId = 3, Name = "Hearts of Iron 4", Abbreviation = "HOI4", ColorHex = "#6f42c1" };
        var srcOriginal = new SourceMaterial { SourceMaterialId = 4, Name = "Original Fiction", Abbreviation = "OC", ColorHex = "#20c997" };

        var sources = new List<SourceMaterial> { srcCanon, srcEaw, srcHoi4, srcOriginal };

        // --- LOCATIONS ---
        var locCrystalCity = new Location { Id = 1, Name = "The Crystal City", Region = "Crystal Empire", Description = "The capital, shielded by the Crystal Heart." };
        var locBadlands = new Location { Id = 2, Name = "The Badlands", Region = "Changeling Lands", Description = "Arid border region." };
        var locCanterlot = new Location { Id = 3, Name = "Canterlot", Region = "Equestria", Description = "The mountain capital." };

        var locations = new List<Location> { locCrystalCity, locBadlands, locCanterlot };

        // --- CHARACTERS ---
        var charSombra = new Character { Id = 1, Name = "King Sombra", Archetype = "The Reformer Tyrant", Description = "Returned not as a monster." };
        var charTwilight = new Character { Id = 2, Name = "Twilight Sparkle", Archetype = "The Reluctant General", Description = "Thrust into command." };
        var charChrysalis = new Character { Id = 3, Name = "Queen Chrysalis", Archetype = "The Warlord", Description = "Seeking resources." };

        var characters = new List<Character> { charSombra, charTwilight, charChrysalis };

        // --- THEMES ---
        var themeWar = new Theme { Id = 1, Name = "Industrialized Magic", ColorHex = "#dc3545" };
        var themeRedemption = new Theme { Id = 2, Name = "Cost of Redemption", ColorHex = "#0d6efd" };

        var themes = new List<Theme> { themeWar, themeRedemption };

        // --- THREADS ---
        var threadInvasion = new StoryThread { Id = 1, Name = "The Changeling Offensive", ColorHex = "#198754" };
        var threadPolitics = new StoryThread { Id = 2, Name = "Crystal Politics", ColorHex = "#ffc107" };

        var threads = new List<StoryThread> { threadInvasion, threadPolitics };

        // --- NOTES ---
        var notes = new List<Note>
        {
            new Note { Id = 1, Character = charSombra, SourceMaterial = srcCanon, Content = "Was originally a shadow monster.", IsStrictRule = true },
            new Note { Id = 2, Character = charSombra, SourceMaterial = srcEaw, Content = "Creates the 'Crystal Wehrmacht'.", IsStrictRule = false },
            new Note { Id = 3, Character = charChrysalis, SourceMaterial = srcCanon, Content = "Feeds on love.", IsStrictRule = true },
            new Note { Id = 4, CodexEntry = new CodexEntry { Id = 1, Title = "Magical Exhaustion" }, SourceMaterial = srcOriginal, Content = "Unicorns cannot shield > 2 hours.", IsStrictRule = true }
        };

        // --- CHAPTERS & PLOT POINTS ---
        var chap1 = new Chapter { Id = 1, Title = "Chapter 1: The Northern Wind", OrderIndex = 1, Description = "The calm before the storm." };
        
        var pp1 = new PlotPoint 
        { 
            Id = 1,
            Title = "Sombra's Return", 
            Description = "Sombra walks into the council room.",
            Chapter = chap1,
            Location = locCrystalCity,
            OrderInChapter = 1,
            Intensity = 3,
            VerbatimText = "\"I did not come back to enslave you.\"",
            Status = DraftStatus.Drafted
        };
        // Manual Linking for Object Graph
        pp1.CharacterAppearances.Add(new PlotPointCharacter { Character = charSombra });
        pp1.CharacterAppearances.Add(new PlotPointCharacter { Character = charTwilight });
        pp1.ThreadAssignments.Add(new PlotPointThread { StoryThread = threadPolitics });
        pp1.ThemeAssignments.Add(new PlotPointTheme { Theme = themeRedemption });
        
        chap1.PlotPoints.Add(pp1);

        var chap2 = new Chapter { Id = 2, Title = "Chapter 2: Blitzkrieg", OrderIndex = 2 };
        // ... (Add your other plot points here similar to above) ...

        var chapters = new List<Chapter> { chap1, chap2 };

        return new StoryWorldData(sources, locations, characters, themes, threads, notes, chapters);
    }

}