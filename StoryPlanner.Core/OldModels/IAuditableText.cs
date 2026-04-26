using System;

namespace StoryPlanner.Core.Models
{
    public interface IAuditableText
    {
        DateTime LastModified { get; set; }
    }
}
