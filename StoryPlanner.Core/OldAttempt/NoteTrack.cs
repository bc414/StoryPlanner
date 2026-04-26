using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public enum NoteTrack
    {
        //Grouped by assertion type
        Unset = 0,

        //Story Content - makes claims about the in-universe world
        HistoricalRecord = 1,
        WorldMechanics = 2,
        SystemMechanics,
        TechnologicalMechanics,
        PsychologicalArchitecture,
        BindingLogic,
        CharacterDevelopment, //How they change

        //Narrative Architecture - makes decisions about information control
        DeliveryPlan, //It goes on Character, Theme, Codex Entries and Story Threads. These are notes about how to reveal and utilize throughout the story
        Revelation, //For connections with backstory and mechanics
        PropositionEvidence, //For themes
        GoalMovement, //For story threads
        Stakes,
        ReaderState,//todo: change to something more intuitive
        Synopsis,
        Outcome,

        //Project Management
        MetaComment, //aka meta observation, it is a note about the story planning process
        RevisionDirective,
        RealWorldParallels, //todo: I don't like this, need something more friendly
        SocialCommentary,
        Obligation, //entity-to-entity connections are design obligations. A plot point must eventually be made. Plot-point connection entities are design deliverables.

        //Prose - language the reader encounters
        ProseFragment,
        VerbatimText,
    }

    public enum AssertionType
    {
        Unset,
        StoryContent, //make less generic
        NarrativeArchitecture,
        Prose,
        ProjectManagement
    }

    public static class NoteExtensions
    {
        public static AssertionType GetAssertionType(this NoteTrack noteTrack)
        {
            return noteTrack switch
            {
                NoteTrack.ProseFragment => AssertionType.Prose,
                _ => AssertionType.Unset
            };
        }
    }
}
