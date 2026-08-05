namespace StoryPlanner.SourceTexts;

/// <summary>
/// Accumulates what the ingest matched, what it could not, and what it decided — printed in full
/// on a dry run so the chapter-to-Part mapping is eyeballed before anything is written. The
/// mapping is the one place a silent off-by-one could hide, so it is reported per pair rather
/// than summarised as a count.
/// </summary>
public sealed class IngestReport
{
    private readonly List<string> _notes = [];
    private readonly List<string> _errors = [];
    private readonly List<(string Work, string Code, string PartName, string SourceTitle, bool Agrees)> _mappings = [];
    private readonly List<(string Work, IReadOnlyList<string> Source, IReadOnlyList<string> Plan)> _mismatches = [];

    public bool HasErrors => _errors.Count > 0;

    public void Note(string message) => _notes.Add(message);
    public void Error(string message) => _errors.Add(message);

    public void Mapping(string work, string code, string partName, string sourceTitle, bool agrees) =>
        _mappings.Add((work, code, partName, sourceTitle, agrees));

    public void ListMismatch(string work, IEnumerable<string> source, IEnumerable<string> plan) =>
        _mismatches.Add((work, source.ToList(), plan.ToList()));

    public void PrintMappings()
    {
        foreach (var group in _mappings.GroupBy(m => m.Work))
        {
            var disagreeing = group.Where(m => !m.Agrees).ToList();
            Console.WriteLine();
            Console.WriteLine($"  {group.Key}: {group.Count()} chapter(s) mapped by reading order, " +
                              $"{group.Count() - disagreeing.Count} with agreeing titles.");
            // Agreeing titles corroborate the order mapping and need no review; the disagreements
            // are the whole point of printing this.
            foreach (var m in disagreeing)
                Console.WriteLine($"    {m.Code,-24} plan:\"{m.PartName}\"  <-  epub:\"{m.SourceTitle}\"");
            if (disagreeing.Count == 0) Console.WriteLine("    (every title agrees)");
        }
    }

    public void PrintNotes()
    {
        if (_notes.Count == 0) return;
        Console.WriteLine();
        Console.WriteLine($"  notes ({_notes.Count}):");
        foreach (var n in _notes) Console.WriteLine($"    {n}");
    }

    public void PrintProblems()
    {
        foreach (var (work, source, plan) in _mismatches)
        {
            Console.WriteLine();
            Console.WriteLine($"  {work}: source has {source.Count}, plan has {plan.Count}");
            for (var i = 0; i < Math.Max(source.Count, plan.Count); i++)
            {
                var s = i < source.Count ? source[i] : "—";
                var p = i < plan.Count ? plan[i] : "—";
                Console.WriteLine($"    {i,4}  {Trim(s),-52}  {Trim(p)}");
            }
        }
        if (_errors.Count == 0) return;
        Console.WriteLine();
        Console.WriteLine($"  ERRORS ({_errors.Count}):");
        foreach (var e in _errors) Console.WriteLine($"    {e}");
    }

    private static string Trim(string s) => s.Length <= 50 ? s : s[..49] + "…";
}
