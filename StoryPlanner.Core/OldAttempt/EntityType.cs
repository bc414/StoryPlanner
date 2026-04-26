using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public enum EntityType
    {
        Character,
        WorldLaw,        //Discovered,  Abstract, No participants
        ConstructedIdea, //Constructed, Abstract, No participants
        Technology,      //Constructed, Concrete, No participants
        Organization,    //Constructed, Both,    Has participants
        StoryThread,
        Theme,
        Chapter,
        PlotPoint,
        CharacterLink,
        WorldPhysicsLink,
        AbstractSystemLink,
        OrganizationLink,
        TechologyLink,
        StoryThreadLink,
        ThemeLink,
    }
}
