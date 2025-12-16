using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace StoryPlanner.Core.Models;

public record StoryWorldData(
    List<SourceMaterial> Sources,
    List<Location> Locations,
    List<Character> Characters,
    List<Theme> Themes,
    List<StoryThread> StoryThreads,
    List<Note> Notes,
    List<Chapter> Chapters,
    List<PlotPoint> PlotPoints,
    List<CodexEntry> CodexEntries
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
            new() { Id = 1, Name = "The Great War The Great War", ThreadScope = ThreadScope.Overarching, Description = "The military conflict to stop the Changeling Hegemony." },
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
            new() { Id = 1, Name = "Red vs. Pink Love", Abbreviation = "PR", Description = "Ambition/Passion (Red) vs. Connection/Gentleness (Pink). AJ represents the synthesis." },
            new() { Id = 2, Name = "Authenticity vs. Posing", Abbreviation = "PA",Description = "Being true to oneself (AJ/Trimmel) vs. performing a role (Chrysalis/Early Twilight)." },
            new() { Id = 3, Name = "Burden of Command", Abbreviation = "CO", Description = "The weight of sending others to die. Contrast between Celestia (Run away) and AJ (Shoulder it)." },
            new() { Id = 4, Name = "Democratization of Magic", Abbreviation = "DM", Description = "Technology (Rifles, Crystals) allowing commoners to stand against elites." }
        };

        // ==============================================================================
        // 5. CHAPTERS (The "Arc" Structure)
        // ==============================================================================
        var chapters = new List<Chapter>
        {
            // Arc 1: The Defense (Tall Tale)
            new() { Id = 1, Title = "Abandonment", Summary = "Luna orders the retreat. AJ refuses and stays with the radio.", OrderIndex = 1 },
            new() { Id = 2, Title = "Digging In", Summary = "Mobilizing the city. Star Energy reveals the tanks.", OrderIndex = 2 },
            new() { Id = 3, Title = "The Monster", Summary = "2nd Battle. Twilight arrives and vaporizes the spearhead. 'You're my monster'.", OrderIndex = 3 },
            
            // Arc 2: Command & Industry
            new() { Id = 4, Title = "Mobilization", Summary = "AJ takes command. Manehattan mobilizes (Rarity). Pinkie at the guns.", OrderIndex = 4 },
            new() { Id = 5, Title = "Black Gold", Summary = "Journey to Appleloosa. Investigating Rockfeller.", OrderIndex = 5 },
            new() { Id = 6, Title = "Sabotage", Summary = "The Buffalo reveal. Rockfeller arrested. Earth Pony Magic proven.", OrderIndex = 6 },
            new() { Id = 7, Title = "Kindness", Summary = "Fluttershy and the prisoners. 'Love Rations'.", OrderIndex = 7 },

            // Arc 3: The Counter-Attack
            new() { Id = 8, Title = "Encirclement", Summary = "3rd Battle of Tall Tale. Synovial's green wave. The Backhand Blow.", OrderIndex = 8 },
            new() { Id = 9, Title = "Spearhead", Summary = "The Bluebell River push. Crossing the mountains.", OrderIndex = 9 },
            new() { Id = 10, Title = "Honor", Summary = "Battle of Ponytown. Trimmel surrenders to the Lioness.", OrderIndex = 10 },

            // Arc 4: Revolution
            new() { Id = 11, Title = "Judgment", Summary = "Liberation of Canterlot. Pagala executed. Celestia's return.", OrderIndex = 11 },
            new() { Id = 12, Title = "Stagnation", Summary = "The White Peace. Celestia refuses to invade Changeling Lands. The break.", OrderIndex = 12 },
            new() { Id = 13, Title = "Expedition", Summary = "SECEF forms. Landing in Aquileia. Trimmel joins AJ.", OrderIndex = 13 },
            new() { Id = 14, Title = "The Republic", Summary = "The Referendum. Celestia abdicates. AJ elected President.", OrderIndex = 14 },

            // Arc 5: Victory
            new() { Id = 15, Title = "Liberation", Summary = "Invading the Changeling Lands. Vraks surrenders.", OrderIndex = 15 },
            new() { Id = 16, Title = "Harmony", Summary = "End of the war. Wedding at Sweet Apple Acres.", OrderIndex = 16 }
        };

        // ==============================================================================
        // 8. CODEX ENTRIES (Moved up so PlotPoints can reference them)
        // ==============================================================================
        var codexEntries = new List<CodexEntry>
        {
            // ENTRY 1: TECH
            new()
            {
                Id = 1,
                Title = "Mk. IV 'Sun-Piercer' Mag-Rifle",
                Type = "Weaponry",
                Description = "The standard issue rifle of the Republican Army. It uses a crystallized mana clip to accelerate a depleted uranium shard. Requires no unicorn magic to operate, democratizing lethality.",
                Category = CodexCategory.Technology
            },
            // ENTRY 2: MAGIC THEORY
            new()
            {
                Id = 2,
                Title = "Metabolic Arcano-Synthesis",
                Type = "Magic Theory",
                Description = "Fleur Bloom's thesis stating that Earth Pony magic is not passive, but a metabolization of calories into kinetic force. This revolutionized logistics, requiring the army to issue 'High-Cal' rations rather than standard hay.",
                Category = CodexCategory.MagicSystem
            },
            // ENTRY 3: ORGANIZATION
            new()
            {
                Id = 3,
                Title = "SECEF",
                Type = "Organization",
                Description = "The South Equestrian Continental Expeditionary Force. Formed after the break with Canterlot, this organization integrates Thestral Night-Fighters, Changeling Defectors, and Pony Armor under a unified, meritocratic command structure.",
                Category = CodexCategory.Organization
            },
            // ENTRY 4: ARTIFACT
            new()
            {
                Id = 4,
                Title = "The Crystal Matrix",
                Type = "Infrastructure",
                Description = "A defensive grid originally designed by the Crystal Empire. It creates a dimensional anchor that prevents teleportation. Essential for stopping Changeling infiltrators from bypassing trench lines.",
                Category = CodexCategory.MagicSystem
            }
        };

        // ==============================================================================
        // 6. PLOT POINTS
        // ==============================================================================
        var points = new List<PlotPoint>();

        // --- CHAPTER 1: ABANDONMENT ---
        var pp1 = new PlotPoint
        {
            Id = 1,
            Chapter = chapters.First(c => c.Id == 1),
            ChapterId = 1, // <--- EXPLICIT ID
            Title = "The Retreat Order",
            Location = locations.First(l => l.Id == 1),
            LocationId = 1, // <--- EXPLICIT ID
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
        // Link by Object AND Id to prevent "ThreadId=0" duplicates
        pp1.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp1,
            PlotPointId = 1, // <--- EXPLICIT ID
            StoryThread = threads.First(t => t.Id == 5),
            ThreadId = 5,    // <--- EXPLICIT ID
            ThreadTrajectory = GoalTrajectory.Stagnant,
            ImpactDescription = "Defense weakened, but resolve hardened.",
            IsPrimary = true
        });
        pp1.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp1,
            PlotPointId = 1,
            StoryThread = threads.First(t => t.Id == 2),
            ThreadId = 2,    // <--- EXPLICIT ID
            ThreadTrajectory = GoalTrajectory.Positive,
            ImpactDescription = "First defiance of Royal authority."
        });
        pp1.CharacterAppearances.Add(new PlotPointCharacter
        {
            PlotPoint = pp1,
            PlotPointId = 1,
            Character = characters.First(c => c.Id == 1),
            CharacterId = 1, // <--- EXPLICIT ID
            Role = CharacterRole.Protagonist,
            DevelopmentImpact = CharacterDevImpact.PivotalShift,
            DevelopmentNote = "Becomes the General."
        });
        points.Add(pp1);

        // --- CHAPTER 3: THE MONSTER ---
        var pp2 = new PlotPoint
        {
            Id = 2,
            Chapter = chapters.First(c => c.Id == 3),
            ChapterId = 3,
            Title = "Red Magic",
            Location = locations.First(l => l.Id == 1),
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
        pp2.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp2,
            PlotPointId = 2,
            StoryThread = threads.First(t => t.Id == 3),
            ThreadId = 3,
            ThreadTrajectory = GoalTrajectory.Positive,
            ImpactDescription = "Shared trauma. 'You are my monster.' Bonding.",
            IsPrimary = true
        });
        pp2.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp2,
            PlotPointId = 2,
            StoryThread = threads.First(t => t.Id == 5),
            ThreadId = 5,
            ThreadTrajectory = GoalTrajectory.Triumph,
            ImpactDescription = "Tall Tale survives the second battle."
        });
        pp2.CharacterAppearances.Add(new PlotPointCharacter
        {
            PlotPoint = pp2,
            PlotPointId = 2,
            Character = characters.First(c => c.Id == 2),
            CharacterId = 2,
            Role = CharacterRole.Protagonist,
            DevelopmentImpact = CharacterDevImpact.Transformation,
            DevelopmentNote = "Loss of Innocence."
        });
        points.Add(pp2);

        // --- CHAPTER 6: SABOTAGE ---
        var pp3 = new PlotPoint
        {
            Id = 3,
            Chapter = chapters.First(c => c.Id == 6),
            ChapterId = 6,
            Title = "The Shovel Reveal",
            Location = locations.First(l => l.Id == 2),
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
        pp3.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp3,
            PlotPointId = 3,
            StoryThread = threads.First(t => t.Id == 6),
            ThreadId = 6,
            ThreadTrajectory = GoalTrajectory.Triumph,
            ImpactDescription = "Oil crisis solved. Star Energy model validated.",
            IsPrimary = true
        });
        pp3.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp3,
            PlotPointId = 3,
            StoryThread = threads.First(t => t.Id == 4),
            ThreadId = 4,
            ThreadTrajectory = GoalTrajectory.Triumph,
            ImpactDescription = "Theory of Magic confirmed."
        });
        points.Add(pp3);

        // --- CHAPTER 10: HONOR ---
        var pp4 = new PlotPoint
        {
            Id = 4,
            Chapter = chapters.First(c => c.Id == 10),
            ChapterId = 10,
            Title = "Trimmel's Surrender",
            Location = locations.First(l => l.Id == 5),
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
        pp4.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp4,
            PlotPointId = 4,
            StoryThread = threads.First(t => t.Id == 1),
            ThreadId = 1,
            ThreadTrajectory = GoalTrajectory.Triumph,
            ImpactDescription = "Enemy's best general removed from the board.",
            IsPrimary = true
        });
        pp4.CharacterAppearances.Add(new PlotPointCharacter
        {
            PlotPoint = pp4,
            PlotPointId = 4,
            Character = characters.First(c => c.Id == 3),
            CharacterId = 3,
            Role = CharacterRole.Antagonist,
            DevelopmentImpact = CharacterDevImpact.PivotalShift,
            DevelopmentNote = "Respects the enemy. Breaks with Chrysalis."
        });
        points.Add(pp4);

        // --- CHAPTER 11: JUDGMENT ---
        var pp5 = new PlotPoint
        {
            Id = 5,
            Chapter = chapters.First(c => c.Id == 11),
            ChapterId = 11,
            Title = "The Entrenching Tool",
            Location = locations.First(l => l.Id == 4),
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
        pp5.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp5,
            PlotPointId = 5,
            StoryThread = threads.First(t => t.Id == 1),
            ThreadId = 1,
            ThreadTrajectory = GoalTrajectory.Triumph,
            ImpactDescription = "Occupation of Equestria ends.",
            IsPrimary = true
        });
        pp5.CharacterAppearances.Add(new PlotPointCharacter
        {
            PlotPoint = pp5,
            PlotPointId = 5,
            Character = characters.First(c => c.Id == 1),
            CharacterId = 1,
            Role = CharacterRole.Protagonist,
            DevelopmentImpact = CharacterDevImpact.Transformation,
            DevelopmentNote = "Becomes the Judge/Executioner."
        });
        points.Add(pp5);

        // --- CHAPTER 12: STAGNATION ---
        var pp6 = new PlotPoint
        {
            Id = 6,
            Chapter = chapters.First(c => c.Id == 12),
            ChapterId = 12,
            Title = "The White Peace",
            Location = locations.First(l => l.Id == 4),
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
        pp6.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp6,
            PlotPointId = 6,
            StoryThread = threads.First(t => t.Id == 2),
            ThreadId = 2,
            ThreadTrajectory = GoalTrajectory.Triumph,
            ImpactDescription = "The Republic becomes inevitable.",
            IsPrimary = true
        });
        pp6.ThreadAssignments.Add(new PlotPointThread
        {
            PlotPoint = pp6,
            PlotPointId = 6,
            StoryThread = threads.First(t => t.Id == 1),
            ThreadId = 1,
            ThreadTrajectory = GoalTrajectory.Negative,
            ImpactDescription = "War effort stalled by politics."
        });
        pp6.ThemeAssignments.Add(new PlotPointTheme
        {
            PlotPoint = pp6,
            PlotPointId = 6,
            Theme = themes.First(t => t.Id == 3),
            ThemeId = 3,
            Prominence = ThemeProminence.CentralConflict,
            Commentary = "Burden of Command vs. Stagnation."
        });
        points.Add(pp6);

        // ==============================================================================
        // 7. NOTES (The Atomic Truths)
        // ==============================================================================
        var notes = new List<Note>
        {
            // --- WORLD RULES (Strict) ---
            new()
            {
                Id = 1,
                Content = "Earth Pony magic is metabolic; it burns calories to create kinetic force or shielding. Soldiers need 6,000+ kcal/day.",
                IsStrictRule = true,
                Theme = themes.First(t => t.Id == 4)
            },
            new()
            {
                Id = 2,
                Content = "Crystal Matrix shields are opaque to teleportation. You cannot jump into a shielded city.",
                IsStrictRule = true,
                Theme = themes.First(t => t.Id == 4)
            },
            new()
            {
                Id = 3,
                Content = "The 'Green' (Changeling Magic) feeds on despair. Panic in the ranks literally strengthens the enemy's armor.",
                IsStrictRule = true,
                Theme = themes.First(t => t.Id == 1)
            },

            // --- CHARACTER INSIGHTS (Soft) ---
            new()
            {
                Id = 4,
                Content = "Applejack refuses to wear the official General's uniform. She wears her modified foreman's jacket to maintain connection with the workers.",
                IsStrictRule = false,
                Character = characters.First(c => c.Id == 1)
            },
            new()
            {
                Id = 5,
                Content = "Twilight hates the smell of ozone because it reminds her of the vaporization of the Changeling spearhead.",
                IsStrictRule = false,
                Character = characters.First(c => c.Id == 2)
            },
            new()
            {
                Id = 6,
                Content = "Trimmel respects competence above species. He will salute a pony who outsmarts him.",
                IsStrictRule = false,
                Character = characters.First(c => c.Id == 3)
            },
            new()
            {
                Id = 7,
                Content = "Celestia genuinely believes she is protecting ponies by keeping them weak. It is a maternal condescension.",
                IsStrictRule = false,
                Character = characters.First(c => c.Id == 5)
            },

            // --- LOCATION SPECIFIC ---
            new()
            {
                Id = 8,
                Content = "The smog over Tall Tale is so thick that pegasi cannot fly safely without respirators.",
                IsStrictRule = true,
                SourceMaterial = sources.First(s => s.Id == 3) // Original Fiction
            },
            new()
            {
                Id = 9,
                Content = "The Appleloosa oil fields are rigged with explosives. If the line breaks, they blow the wells.",
                IsStrictRule = true,
                SourceMaterial = sources.First(s => s.Id == 3)
            }
        };

        // ==============================================================================
        // 9. CODEX LINKS (Connecting Lore to Plot)
        // ==============================================================================

        // LINK 1: The Rifle is used in the Execution (PP5)
        pp5.CodexReferences.Add(new PlotPointCodexEntry
        {
            PlotPoint = pp5,
            CodexEntry = codexEntries.First(c => c.Id == 1), // Mag-Rifle
            LogicalOrder = 1,
            Commentary = "The specific weapon used to kill Pagala."
        });

        // LINK 2: Magic Theory is proven during the Oil/Shovel Reveal (PP3)
        pp3.CodexReferences.Add(new PlotPointCodexEntry
        {
            PlotPoint = pp3,
            CodexEntry = codexEntries.First(c => c.Id == 2), // Metabolic Theory
            LogicalOrder = 1,
            Commentary = "Fleur explains the caloric needs here."
        });

        // LINK 3: SECEF is hinted at during Trimmel's surrender (PP4)
        pp4.CodexReferences.Add(new PlotPointCodexEntry
        {
            PlotPoint = pp4,
            CodexEntry = codexEntries.First(c => c.Id == 3), // SECEF
            LogicalOrder = 2,
            Commentary = "The seed of the organization forms when AJ accepts Trimmel."
        });

        // LINK 4: SECEF is formally established during The White Peace (PP6)
        pp6.CodexReferences.Add(new PlotPointCodexEntry
        {
            PlotPoint = pp6,
            CodexEntry = codexEntries.First(c => c.Id == 3), // SECEF
            LogicalOrder = 1,
            Commentary = "Official formation of the faction."
        });

        // LINK 5: Crystal Matrix prevents escape in The Monster (PP2)
        pp2.CodexReferences.Add(new PlotPointCodexEntry
        {
            PlotPoint = pp2,
            CodexEntry = codexEntries.First(c => c.Id == 4), // Matrix
            LogicalOrder = 3,
            Commentary = "Why the Changelings couldn't just teleport away from the laser."
        });

        // Add entries to the final return object
        return new StoryWorldData(sources, locations, characters, themes, threads, notes, chapters, points, codexEntries);
    }
}