using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core
{
    public interface INoteable
    {
        int Id { get; }
        OwnerType OwnerType { get; }
    }
}
