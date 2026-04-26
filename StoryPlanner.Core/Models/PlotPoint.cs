using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

public class PlotPoint : INoteable
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    // --- Relationships ---
    public int? ChapterId { get; set; }
    public int OrderInChapter { get; set; }

    public OwnerType OwnerType => OwnerType.PlotPoint;

    public int GetTotalTextLength()
    {
        return GetCombinedText().Length;
    }

    public string GetCombinedText()
    {
        return string.Empty;
    }
}