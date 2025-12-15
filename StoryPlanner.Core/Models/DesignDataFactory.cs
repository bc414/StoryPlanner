using System.Collections.ObjectModel;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core.Models;

public record StoryWorldData(
    List<SourceMaterial> Sources,
    List<Location> Locations,
    List<Character> Characters,
    List<Theme> Themes,
    List<StoryThread> Threads,
    List<Note> Notes,
    List<Chapter> Chapters,
    List<PlotPoint> PlotPoints
);

public static class DesignDataFactory
{
    public static StoryWorldData CreateWorld()
    {
        // ==============================================================================
        // 1. SOURCES & LOCATIONS
        // ==============================================================================
        var sources = new List<SourceMaterial>
        {
            new() { Id = 1, Name = "Canon FIM", Abbreviation = "FIM", ColorHex = "#D63384" },
            new() { Id = 2, Name = "Equestria at War", Abbreviation = "EaW", ColorHex = "#FD7E14" },
            new() { Id = 3, Name = "Original Fiction", Abbreviation = "OC", ColorHex = "#20C997" }
        };

        var locations = new List<Location>
        {
            new() { Id = 1, Name = "Tall Tale", Region = "North Equestria", Description = "Industrial hub, home of Star Energy. The 'Stalingrad' of the war." },
            new() { Id = 2, Name = "Appleloosa", Region = "Southwest", Description = "Oil fields and Buffalo territory. Site of Rockfeller's sabotage." },
            new() { Id = 3, Name = "Tzinacatl Jungle", Region = "Southeast", Description = "Home of the 12 Tribes. Source of rubber and ancient irrigation magic." },
            new() { Id = 4, Name = "Canterlot Castle", Region = "Central", Description = "The Throne Room. Site of Pagala's execution and Celestia's abdication." },
            new() { Id = 5, Name = "Bluebell River", Region = "Central Plains", Description = "The site of the decisive armored spearhead." },
            new() { Id = 6, Name = "Cloudbury", Region = "Griffonian Republic", Description = "Capital of the GR. Site of the 'Voice' conference." },
            new() { Id = 7, Name = "Vesalipolis", Region = "Changeling Lands", Description = "The final siege." }
        };

        // ==============================================================================
        // 2. CHARACTERS
        // ==============================================================================
        var characters = new List<Character>
        {
            new() { Id = 1, Name = "Applejack", Description = "The Lioness. From Farmer to General to President. Represents practical honesty and the 'Red/Pink' synthesis." },
            new() { Id = 2, Name = "Twilight Sparkle", Description = "The Architect. Traumatized by her own power ('The Monster'). Rejects Celestia's mold to find her own truth." },
            new() { Id = 3, Name = "Trimmel", Description = "Changeling Field Marshal. A meritocrat serving a tyrant. Eventually joins SECEF." },
            new() { Id = 4, Name = "Queen Chrysalis", Description = "The Posed Queen. Represents industrialized predation and ego." },
            new() { Id = 5, Name = "Princess Celestia", Description = "The Benevolent Stagnation. Her 'Walled Garden' policy caused the war. Represents the failure of absolute monarchy." },
            new() { Id = 6, Name = "Henri Gourard", Description = "Aquileian revolutionary and griffon tank commander. Brings 'Republican' ideology to Tall Tale." },
            new() { Id = 7, Name = "Mali (Tempest Wind)", Description = "Thestral volunteer tank commander. Bridges the gap between Pony/Thestral/Changeling." },
            new() { Id = 8, Name = "Comet Shine", Description = "CEO of Star Energy. Proves that capitalism can serve the public good if guided by harmony." },
            new() { Id = 9, Name = "Fleur Bloom", Description = "Scientist. Revealer of Earth Pony Magic. Anti-Noble revolutionary." },
            new() { Id = 10, Name = "Actia Pagala", Description = "Fanatical Changeling commander. Represents pure hate. Killed by AJ." }
        };

        // ==============================================================================
        // 3. STORY THREADS
        // ==============================================================================
        var threads = new List<StoryThread>
        {
            // Global / Serial Threads
            new() { Id = 1, Name = "The Great War", ThreadScope = ThreadScope.Overarching, Description = "The military conflict to stop the Changeling Hegemony." },
            new() { Id = 2, Name = "The Republic", ThreadScope = ThreadScope.Overarching, Description = "The political shift from Celestia's Stagnation to a Harmonic Republic." },
            new() { Id = 3, Name = "TwiJack Romance", ThreadScope = ThreadScope.Overarching, Description = "From friends to partners. Grounding the 'Monster' with the 'Lioness'." },
            new() { Id = 4, Name = "The Unified Theory", ThreadScope = ThreadScope.Overarching, Description = "Fleur & Twilight's research: Earth Pony Magic, Crystal Tech, and the liberalization of magic." },
            
            // Immediate / Episodic Threads
            new() { Id = 5, Name = "Defense of Tall Tale", ThreadScope = ThreadScope.Immediate, Description = "Holding the line against Synovial's green waves." },
            new() { Id = 6, Name = "The Oil Crisis", ThreadScope = ThreadScope.Immediate, Description = "Dealing with Rockfeller and the Buffalo sabotage." },
            new() { Id = 7, Name = "The White Peace", ThreadScope = ThreadScope.Immediate, Description = "The confrontation with Celestia after liberation." },
            new() { Id = 8, Name = "Griffonian Expedition", ThreadScope = ThreadScope.Overarching, Description = "SECEF intervention in Aquileia." }
        };

        // ==============================================================================
        // 4. THEMES
        // ==============================================================================
        var themes = new List<Theme>
        {
            new() { Id = 1, Name = "Red vs. Pink Love", Description = "Ambition/Passion (Red) vs. Connection/Gentleness (Pink). AJ represents the synthesis." },
            new() { Id = 2, Name = "Authenticity vs. Posing", Description = "Being true to oneself (AJ/Trimmel) vs. performing a role (Chrysalis/Early Twilight)." },
            new() { Id = 3, Name = "Burden of Command", Description = "The weight of sending others to die. Contrast between Celestia (Run away) and AJ (Shoulder it)." },
            new() { Id = 4, Name = "Democratization of Magic", Description = "Technology (Rifles, Crystals) allowing commoners to stand against elites." }
        };

        // ==============================================================================
        // 5. CHAPTERS (The "Arc" Structure)
        // ==============================================================================
        var chapters = new List<Chapter>
        {
            // Arc 1: The Defense (Tall Tale)
            new() { Id = 1, Title = "Abandonment", Summary = "Luna orders the retreat. AJ refuses and stays with the radio." },
            new() { Id = 2, Title = "Digging In", Summary = "Mobilizing the city. Star Energy reveals the tanks." },
            new() { Id = 3, Title = "The Monster", Summary = "2nd Battle. Twilight arrives and vaporizes the spearhead. 'You're my monster'." },
            
            // Arc 2: Command & Industry
            new() { Id = 4, Title = "Mobilization", Summary = "AJ takes command. Manehattan mobilizes (Rarity). Pinkie at the guns." },
            new() { Id = 5, Title = "Black Gold", Summary = "Journey to Appleloosa. Investigating Rockfeller." },
            new() { Id = 6, Title = "Sabotage", Summary = "The Buffalo reveal. Rockfeller arrested. Earth Pony Magic proven." },
            new() { Id = 7, Title = "Kindness", Summary = "Fluttershy and the prisoners. 'Love Rations'." },

            // Arc 3: The Counter-Attack
            new() { Id = 8, Title = "Encirclement", Summary = "3rd Battle of Tall Tale. Synovial's green wave. The Backhand Blow." },
            new() { Id = 9, Title = "Spearhead", Summary = "The Bluebell River push. Crossing the mountains." },
            new() { Id = 10, Title = "Honor", Summary = "Battle of Ponytown. Trimmel surrenders to the Lioness." },

            // Arc 4: Revolution
            new() { Id = 11, Title = "Judgment", Summary = "Liberation of Canterlot. Pagala executed. Celestia's return." },
            new() { Id = 12, Title = "Stagnation", Summary = "The White Peace. Celestia refuses to invade Changeling Lands. The break." },
            new() { Id = 13, Title = "Expedition", Summary = "SECEF forms. Landing in Aquileia. Trimmel joins AJ." },
            new() { Id = 14, Title = "The Republic", Summary = "The Referendum. Celestia abdicates. AJ elected President." },

            // Arc 5: Victory
            new() { Id = 15, Title = "Liberation", Summary = "Invading the Changeling Lands. Vraks surrenders." },
            new() { Id = 16, Title = "Harmony", Summary = "End of the war. Wedding at Sweet Apple Acres." }
        };

        // ==============================================================================
        // 6. PLOT POINTS
        // ==============================================================================
        var points = new List<PlotPoint>();

        // --- CHAPTER 1: ABANDONMENT ---
        var pp1 = new PlotPoint
        {
            Id = 1,
            ChapterId = 1,
            Title = "The Retreat Order",
            LocationId = 1,
            WorldDate = "1011.12.05",
            Stakes = "Survival of Tall Tale's civilian population.",
            Synopsis = "Luna arrives and orders a general retreat, citing the 'nightmares' of the soldiers. Applejack refuses to leave her home and the Star Energy factories.",
            Outcome = "The Regular Army leaves. AJ stays with the local militia. She keeps the radio.",
            CoreDriver = CoreDriver.Character,
            TensionPhase = TensionPhase.Situation,
            ConflictType = ConflictType.Societal,
            Presentation = Presentation.RealTime,
            Status = DraftStatus.Outlined
        };
        pp1.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp1, ThreadId = 5, IsPrimary = true, ThreadTrajectory = GoalTrajectory.Stagnant, ImpactDescription = "Defense weakened, but resolve hardened." });
        pp1.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp1, ThreadId = 2, IsPrimary = false, ThreadTrajectory = GoalTrajectory.Positive, ImpactDescription = "First defiance of Royal authority." });
        pp1.CharacterAppearances.Add(new PlotPointCharacter { PlotPoint = pp1, CharacterId = 1, Role = CharacterRole.Protagonist, DevelopmentImpact = CharacterDevImpact.PivotalShift, DevelopmentNote = "Becomes the General." });
        points.Add(pp1);

        // --- CHAPTER 3: THE MONSTER ---
        var pp2 = new PlotPoint
        {
            Id = 2,
            ChapterId = 3,
            Title = "Red Magic",
            LocationId = 1,
            WorldDate = "1012.01.15",
            Stakes = "AJ's life. The integrity of the defensive line.",
            Synopsis = "AJ is pinned down by tanks. Twilight arrives, disobeying Celestia, and uses a laser to vaporize the fuel trucks and changelings.",
            Outcome = "Tanks destroyed. Twilight traumatized by her lethality.",
            CoreDriver = CoreDriver.Action,
            TensionPhase = TensionPhase.LocalClimax,
            ConflictType = ConflictType.Internal,
            Presentation = Presentation.RealTime,
            Status = DraftStatus.Drafted
        };
        pp2.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp2, ThreadId = 3, IsPrimary = true, ThreadTrajectory = GoalTrajectory.Positive, ImpactDescription = "Shared trauma. 'You are my monster.' Bonding." });
        pp2.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp2, ThreadId = 5, IsPrimary = false, ThreadTrajectory = GoalTrajectory.Triumph, ImpactDescription = "Tall Tale survives the second battle." });
        pp2.CharacterAppearances.Add(new PlotPointCharacter { PlotPoint = pp2, CharacterId = 2, Role = CharacterRole.Protagonist, DevelopmentImpact = CharacterDevImpact.Transformation, DevelopmentNote = "Loss of Innocence." });
        points.Add(pp2);

        // --- CHAPTER 6: SABOTAGE ---
        var pp3 = new PlotPoint
        {
            Id = 3,
            ChapterId = 6,
            Title = "The Shovel Reveal",
            LocationId = 2,
            WorldDate = "1012.03.10",
            Stakes = "Oil supply for the tanks.",
            Synopsis = "Fleur Bloom explains that mechanization doesn't dampen Earth Pony magic; it amplifies it. Rockfeller was suppressing this to keep control.",
            Outcome = "Rockfeller arrested. Buffalo given stewardship of drills. Oil secured.",
            CoreDriver = CoreDriver.World,
            TensionPhase = TensionPhase.Resolution,
            ConflictType = ConflictType.Societal,
            Presentation = Presentation.RealTime,
            Status = DraftStatus.Planned
        };
        pp3.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp3, ThreadId = 6, IsPrimary = true, ThreadTrajectory = GoalTrajectory.Triumph, ImpactDescription = "Oil crisis solved. Star Energy model validated." });
        pp3.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp3, ThreadId = 4, IsPrimary = false, ThreadTrajectory = GoalTrajectory.Triumph, ImpactDescription = "Theory of Magic confirmed." });
        points.Add(pp3);

        // --- CHAPTER 10: HONOR ---
        var pp4 = new PlotPoint
        {
            Id = 4,
            ChapterId = 10,
            Title = "Trimmel's Surrender",
            LocationId = 5,
            WorldDate = "1012.06.20",
            Stakes = "The encirclement of the Changeling Army.",
            Synopsis = "Trimmel realizes he has been outmaneuvered by AJ's 'Backhand Blow'. He surrenders to AJ, acknowledging her as a peer ('The Lioness').",
            Outcome = "Trimmel captured. Treated with dignity.",
            CoreDriver = CoreDriver.Character,
            TensionPhase = TensionPhase.LocalClimax,
            ConflictType = ConflictType.Interpersonal,
            Presentation = Presentation.RealTime,
            Status = DraftStatus.Idea
        };
        pp4.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp4, ThreadId = 1, IsPrimary = true, ThreadTrajectory = GoalTrajectory.Triumph, ImpactDescription = "Enemy's best general removed from the board." });
        pp4.CharacterAppearances.Add(new PlotPointCharacter { PlotPoint = pp4, CharacterId = 3, Role = CharacterRole.Antagonist, DevelopmentImpact = CharacterDevImpact.PivotalShift, DevelopmentNote = "Respects the enemy. Breaks with Chrysalis." });
        points.Add(pp4);

        // --- CHAPTER 11: JUDGMENT ---
        var pp5 = new PlotPoint
        {
            Id = 5,
            ChapterId = 11,
            Title = "The Entrenching Tool",
            LocationId = 4,
            WorldDate = "1012.08.01",
            Stakes = "Liberation of Canterlot.",
            Synopsis = "In the throne room, Pagala uses hostages. AJ shoots Pagala with a magical rifle (or shovel?), ending her terror.",
            Outcome = "Pagala dead. Canterlot liberated. AJ traumatized but resolute.",
            CoreDriver = CoreDriver.Action,
            TensionPhase = TensionPhase.LocalClimax,
            ConflictType = ConflictType.Interpersonal,
            Presentation = Presentation.RealTime,
            Status = DraftStatus.Idea
        };
        pp5.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp5, ThreadId = 1, IsPrimary = true, ThreadTrajectory = GoalTrajectory.Triumph, ImpactDescription = "Occupation of Equestria ends." });
        pp5.CharacterAppearances.Add(new PlotPointCharacter { PlotPoint = pp5, CharacterId = 1, Role = CharacterRole.Protagonist, DevelopmentImpact = CharacterDevImpact.Transformation, DevelopmentNote = "Becomes the Judge/Executioner." });
        points.Add(pp5);

        // --- CHAPTER 12: STAGNATION ---
        var pp6 = new PlotPoint
        {
            Id = 6,
            ChapterId = 12,
            Title = "The White Peace",
            LocationId = 4,
            WorldDate = "1012.09.01",
            Stakes = "The future of the war.",
            Synopsis = "Celestia declares the war over and refuses to invade the Changeling Lands. She wants to return to the 'Walled Garden'. AJ refuses to stop.",
            Outcome = "Political rupture. AJ defies the crown openly.",
            CoreDriver = CoreDriver.Character,
            TensionPhase = TensionPhase.Situation,
            ConflictType = ConflictType.Societal,
            Presentation = Presentation.RealTime,
            Status = DraftStatus.Outlined
        };
        pp6.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp6, ThreadId = 2, IsPrimary = true, ThreadTrajectory = GoalTrajectory.Triumph, ImpactDescription = "The Republic becomes inevitable." });
        pp6.ThreadAssignments.Add(new PlotPointThread { PlotPoint = pp6, ThreadId = 1, IsPrimary = false, ThreadTrajectory = GoalTrajectory.Negative, ImpactDescription = "War effort stalled by politics." });
        pp6.ThemeAssignments.Add(new PlotPointTheme { PlotPoint = pp6, ThemeId = 3, Prominence = ThemeProminence.CentralConflict, Commentary = "Burden of Command vs. Stagnation." });
        points.Add(pp6);

        return new StoryWorldData(sources, locations, characters, themes, threads, new List<Note>(), chapters, points);
    }
}