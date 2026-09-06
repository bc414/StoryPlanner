namespace StoryPlanner.ProcessMap;

/// <summary>
/// The headings of one markdown file, for resolving a <c>format</c> cell to exactly one
/// heading in <c>artifacts.md</c> and for checking an activity file's shape. Fenced code and
/// generated sections are skipped: a heading inside a rendered section is never source.
/// </summary>
public sealed class MarkdownOutline
{
    public sealed record Heading(string Text, int Level, int Line);

    public IReadOnlyList<Heading> Headings { get; }

    public MarkdownOutline(string markdown)
    {
        var lines = markdown.Replace("\r\n", "\n").Split('\n');
        var headings = new List<Heading>();
        var inFence = false;
        var inGenerated = false;
        for (var i = 0; i < lines.Length; i++)
        {
            var t = lines[i].Trim();
            if (t.StartsWith("```")) { inFence = !inFence; continue; }
            if (inFence) continue;
            if (t.StartsWith(MapTables.GeneratedOpenPrefix)) { inGenerated = true; continue; }
            if (t == MapTables.GeneratedClose) { inGenerated = false; continue; }
            if (inGenerated || !t.StartsWith('#')) continue;
            var level = t.TakeWhile(c => c == '#').Count();
            headings.Add(new Heading(t[level..].Trim(), level, i + 1));
        }
        Headings = headings;
    }

    /// <summary>
    /// Exactly-one-match semantics: zero matches and two matches are both failures. A heading
    /// that appears twice cannot be cited without saying which one.
    /// </summary>
    public IReadOnlyList<Heading> Find(string text)
        => Headings.Where(h => string.Equals(h.Text, text, StringComparison.Ordinal)).ToList();
}
