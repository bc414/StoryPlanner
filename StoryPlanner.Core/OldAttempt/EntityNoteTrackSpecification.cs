using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public class EntityNoteTrackSpecification
    {
        public EntityType EntityType { get; set; }
        public NoteTrack NoteTrack { get; set; }
        public string Description { get; set; } = string.Empty;


        public static Dictionary<EntityType, List<EntityNoteTrackSpecification>> GetEntityNoteTrackSpecifications()
        {
            Dictionary<EntityType, List<EntityNoteTrackSpecification>> specs = new Dictionary<EntityType, List<EntityNoteTrackSpecification>>();

            

            List<EntityNoteTrackSpecification> characterSpecs = new List<EntityNoteTrackSpecification>
            {
                new EntityNoteTrackSpecification
                {
                    EntityType = EntityType.Character,
                    NoteTrack = NoteTrack.PsychologicalArchitecture,
                    Description = "The character's core values, what made them, and how they can interpret or misinterpret the truth (epistemic limits)"
                },
                new EntityNoteTrackSpecification
                {
                    EntityType = EntityType.Character,
                    NoteTrack = NoteTrack.HistoricalRecord,
                    Description = "Objective backstory of the character"
                }
            };
            AddUniversalTracks(characterSpecs, EntityType.Character);
            AddTracksForSubjectEntities(characterSpecs, EntityType.Character);
            specs[EntityType.Character] = characterSpecs;

            List<EntityNoteTrackSpecification> characterLinkSpecs = new List<EntityNoteTrackSpecification>();
            AddUniversalTracks(characterLinkSpecs, EntityType.CharacterLink);
            AddRevelation(characterLinkSpecs, EntityType.CharacterLink);
            characterLinkSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.CharacterLink,
                NoteTrack = NoteTrack.CharacterDevelopment,
                Description = "How the character's psychology changes due to the plot point"
            });
            specs[EntityType.CharacterLink] = characterLinkSpecs;

            List<EntityNoteTrackSpecification> worldMechanicSpecs = new List<EntityNoteTrackSpecification>();
            AddUniversalTracks(worldMechanicSpecs, EntityType.Character);
            AddRevelation(worldMechanicSpecs, EntityType.WorldLaw)

            List<EntityNoteTrackSpecification> themeSpecs = new List<EntityNoteTrackSpecification>();
            AddUniversalTracks(themeSpecs, EntityType.Theme);
            themeSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.Theme,
                NoteTrack = NoteTrack.DeliveryPlan,
                Description = "The overarching plan for how this subject will be presented to the reader across the project (narrative macro architecture), dictates information control"
            });
            themeSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.Theme,
                NoteTrack = NoteTrack.SocialCommentary,
                Description = "How the theme applies to the real world"
            });
            specs[EntityType.Theme] = themeSpecs;

            List<EntityNoteTrackSpecification> themeLinkSpecs = new List<EntityNoteTrackSpecification>();
            AddUniversalTracks(themeLinkSpecs, EntityType.ThemeLink);
            themeLinkSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.ThemeLink,
                NoteTrack = NoteTrack.PropositionEvidence,
                Description = "Evidence or examples showing the theme's proposition. Informs the scene design by articulating what should be shown to the reader such that they logically arrive at the proposition."
            });
            //Needs an enum for whether the evidence is an argument or counterargument

            List<EntityNoteTrackSpecification> storyThreadSpecs = new List<EntityNoteTrackSpecification>();
            AddUniversalTracks(storyThreadSpecs, EntityType.StoryThread);
            storyThreadSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.StoryThread,
                NoteTrack = NoteTrack.DeliveryPlan,
                Description = "What the purpose of the goal is, the plan to plant seeds across chapters"
            });

            List<EntityNoteTrackSpecification> storyThreadLinkSpecs = new List<EntityNoteTrackSpecification>();
            AddUniversalTracks(storyThreadLinkSpecs, EntityType.StoryThreadLink);
            storyThreadLinkSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.StoryThread,
                NoteTrack = NoteTrack.GoalMovement,
                Description = "How the goal is affected by the plot point"
            });
            //Needs a goal trajectory enum

            List<EntityNoteTrackSpecification> plotPointSpecs = new List<EntityNoteTrackSpecification>();
            AddUniversalTracks(plotPointSpecs, EntityType.PlotPoint);
            plotPointSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.PlotPoint,
                NoteTrack = NoteTrack.Stakes,
                Description = "Why this scene must exist for the timeline"
            });
            plotPointSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.PlotPoint,
                NoteTrack = NoteTrack.Outcome,
                Description = "What changed in the timeline"
            });
            plotPointSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.PlotPoint,
                NoteTrack = NoteTrack.Synopsis,
                Description = "What happens objectively as experienced by the POV character"
            });
            plotPointSpecs.Add(new EntityNoteTrackSpecification
            {
                EntityType = EntityType.PlotPoint,
                NoteTrack = NoteTrack.ReaderState,
                Description = "What the design intends for the reader to feel"
            }); //should this be difference?

            return specs;
        }

        public static void AddUniversalTracks(List<EntityNoteTrackSpecification> specs, EntityType entityType)
        {
            specs.Add(new EntityNoteTrackSpecification
            {
                EntityType = entityType,
                NoteTrack = NoteTrack.Unset,
                Description = "Uncategorized notes"
            });
            specs.Add(new EntityNoteTrackSpecification
            {
                EntityType = entityType,
                NoteTrack = NoteTrack.MetaComment,
                Description = "Notes about the story design and planning process, not story design itself"
            });
        }

        public static void AddTracksForSubjectEntities(List<EntityNoteTrackSpecification> specs, EntityType entityType)
        {
            specs.Add(new EntityNoteTrackSpecification
            {
                EntityType = entityType,
                NoteTrack = NoteTrack.DeliveryPlan,
                Description = "The overarching plan for how this subject will be presented to the reader across the project (narrative macro architecture), dictates information control"
            });
            specs.Add(new EntityNoteTrackSpecification
            {
                EntityType = entityType,
                NoteTrack = NoteTrack.RealWorldParallels,
                Description = "Real world inspiration and parallels to the subject"
            });
        }

        public static void AddRevelation(List<EntityNoteTrackSpecification> specs, EntityType entityType)
        {
            specs.Add(new EntityNoteTrackSpecification
            {
                EntityType = entityType,
                NoteTrack = NoteTrack.Revelation,
                Description = "What mechanics or backstory of the subject is revealed or utilized, independent of POV character or reader perception (zero focalization claim). Informs but does not specify scene design."
            });
            specs.Add(new EntityNoteTrackSpecification
            {
                EntityType = entityType,
                NoteTrack = NoteTrack.ReaderAccumulation,
                Description = ""
            })
        }
    }
}
