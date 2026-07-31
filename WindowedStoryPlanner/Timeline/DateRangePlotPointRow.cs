using System.Windows.Input;

namespace WindowedStoryPlanner;

/// <summary>
/// One plot point in the date-range window's second region. A display record, not a live view
/// model — the region is rebuilt wholesale when the range changes (see DateRangeNotesViewModel).
/// Plot points hold no notes of their own here; this is the syuzhet register shown beside the
/// fabula one, never mixed into it.
/// </summary>
public class DateRangePlotPointRow
{
    public required string Label { get; init; }        // "3.12.4 The Duel"
    public required string DateText { get; init; }     // "1007-03-15", at whatever precision exists
    public required string ChapterText { get; init; }  // story-qualified, or "(unplaced)"
    public required string TheaterText { get; init; }
    public required ICommand OpenCommand { get; init; }
}
