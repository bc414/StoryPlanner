using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.ChapterItemizer;

/// <summary>
/// The item computation of the chapter itemizer, kept out of the CLI so it is tested directly.
/// A story file in the converter's format — a `# slug` title line, then one `## Chapter N — title`
/// heading per chapter — is cut into one item per heading. The body is the story's slug, the
/// chapter's heading and the chapter's text verbatim; the id is `<slug>-ch<NN>`, the ordinal
/// zero-padded to the width the story needs; the locator is `<slug>#<N>`, the Nth chapter heading
/// of the story's file. A file with no chapter heading is a source fault and is refused, never
/// cut into zero items.
/// </summary>
public static class Chapters
{
    public sealed record Item(string Id, string Body, string Locator, string Description);
    public sealed record Story(string Slug, string Text);

    static readonly Regex Heading = new(@"^## Chapter (?<n>\d+) — (?<title>.*)$", RegexOptions.Compiled);

    /// <summary>What a locator addresses, in words, for the index head.</summary>
    public const string LocatorNotation =
        "`<slug>#<N>`: the story's file `<slug>.md` in the source folder, and N the ordinal of the chapter's `## Chapter` heading in it";

    /// <summary>The narrowing line for the index head: the excluded stories by name, or none when the config excludes nothing.</summary>
    public static string? Narrowing(IReadOnlyCollection<string> excluded)
        => excluded.Count == 0 ? null : $"every story file in the source folder except {string.Join(", ", excluded)}, the config's exclude list";

    /// <summary>Every story not excluded, in ordinal order of slug, cut into its chapters.</summary>
    public static IReadOnlyList<Item> CutAll(IEnumerable<Story> stories, IReadOnlySet<string> exclude)
        => stories.Where(s => !exclude.Contains(s.Slug))
            .OrderBy(s => s.Slug, StringComparer.Ordinal)
            .SelectMany(Cut)
            .ToList();

    /// <summary>One story cut into one item per `## Chapter` heading.</summary>
    public static IReadOnlyList<Item> Cut(Story story)
    {
        var lines = story.Text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        var headings = new List<(int Line, string Title)>();
        for (var i = 0; i < lines.Length; i++)
        {
            var m = Heading.Match(lines[i]);
            if (m.Success) headings.Add((i, m.Groups["title"].Value));
        }
        if (headings.Count == 0)
            throw new InvalidOperationException($"{story.Slug}: no `## Chapter` heading; not a converter-format story file");

        var total = headings.Count;
        var width = Math.Max(2, total.ToString().Length);
        var items = new List<Item>(total);
        for (var k = 0; k < total; k++)
        {
            var n = k + 1;
            var start = headings[k].Line + 1;
            var end = k + 1 < total ? headings[k + 1].Line : lines.Length;
            var text = TrimBlankEdges(lines, start, end);
            var body = new StringBuilder()
                .Append("# ").Append(story.Slug).Append('\n')
                .Append(lines[headings[k].Line]).Append("\n\n")
                .Append(text).Append('\n')
                .ToString();
            items.Add(new Item(
                $"{story.Slug}-ch{n.ToString().PadLeft(width, '0')}",
                body,
                $"{story.Slug}#{n}",
                $"{story.Slug}, chapter {n} of {total}: {headings[k].Title}"));
        }
        return items;
    }

    static string TrimBlankEdges(string[] lines, int start, int end)
    {
        while (start < end && lines[start].Trim().Length == 0) start++;
        while (end > start && lines[end - 1].Trim().Length == 0) end--;
        return string.Join('\n', lines, start, end - start);
    }
}
