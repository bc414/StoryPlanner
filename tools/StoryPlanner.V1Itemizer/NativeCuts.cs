using System.Text;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.V1Itemizer;

/// <summary>
/// The item computation over a native-schema v1 snapshot, kept out of the CLI so it is tested
/// directly. Items follow chapter order, then position in the chapter; unplaced plot points last,
/// by id. A plot point's text is carried verbatim, field by field, and every link it has is listed
/// under its kind with the linked entity's name, its set values and its payload, an empty payload
/// written as such: a link with no text still records who or what the plot point names.
/// </summary>
public static class NativeCuts
{
    public const string PlotPointLocatorNotation =
        "`pp-<id>`: the PlotPoints row id in the native-schema v1 snapshot the config names";
    public const string ThemeCommentaryLocatorNotation =
        "`pp-<plot point id>/theme-<theme id>`: the PlotPointThemes row keyed by the two ids in the native-schema v1 snapshot the config names";

    const string NoText = "(no text)";
    static readonly string[] KindOrder = [NativeV1.Character, NativeV1.Theme, NativeV1.Thread, NativeV1.Codex];

    /// <summary>One item per plot point, except those in <paramref name="excludeChapters"/> and, unless <paramref name="includeUnplaced"/>, those in no chapter.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> PlotPoints(NativeV1.Plan plan, IReadOnlySet<int> excludeChapters, bool includeUnplaced)
    {
        var chapters = plan.Chapters.ToDictionary(c => c.Id);
        var linksByPp = plan.Links.ToLookup(l => l.PlotPointId);
        return Ordered(plan, chapters)
            .Where(p => p.ChapterId is int c ? !excludeChapters.Contains(c) : includeUnplaced)
            .Select(p =>
            {
                var body = new StringBuilder()
                    .Append("# v1 plot point: ").Append(p.Title).Append('\n')
                    .Append("chapter: ").Append(ChapterLabel(p, chapters)).Append("\n\n");
                Field(body, "Synopsis", p.Synopsis);
                FieldIfAny(body, "Outcome", p.Outcome);
                FieldIfAny(body, "Stakes", p.Stakes);
                var links = linksByPp[p.Id]
                    .OrderBy(l => Array.IndexOf(KindOrder, l.Kind)).ThenBy(l => l.Order).ThenBy(l => l.EntityId)
                    .ToList();
                body.Append("## Links\n\n");
                if (links.Count == 0) body.Append("(none)\n\n");
                foreach (var l in links)
                {
                    var entity = plan.Entities.GetValueOrDefault((l.Kind, l.EntityId));
                    body.Append("### ").Append(entity?.Kind ?? l.Kind).Append(": ").Append(entity?.Name ?? $"missing {l.Kind.ToLowerInvariant()} {l.EntityId}");
                    if (l.Qualifiers.Count > 0) body.Append(" (").Append(string.Join("; ", l.Qualifiers)).Append(')');
                    body.Append("\n\n").Append(Text(l.Text)).Append("\n\n");
                }
                return new ItemizerOutput.Item($"pp-{p.Id}", body.ToString().TrimEnd('\n') + "\n", $"pp-{p.Id}",
                    $"{ChapterLabel(p, chapters)}: {p.Title}");
            })
            .ToList();
    }

    /// <summary>One item per theme link whose commentary is not blank, with the theme's description and the plot point's synopsis beside it.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> ThemeCommentaries(NativeV1.Plan plan)
    {
        var chapters = plan.Chapters.ToDictionary(c => c.Id);
        var themeLinks = plan.Links.Where(l => l.Kind == NativeV1.Theme && l.Text.Trim().Length > 0).ToLookup(l => l.PlotPointId);
        var items = new List<ItemizerOutput.Item>();
        foreach (var p in Ordered(plan, chapters))
        foreach (var l in themeLinks[p.Id].OrderBy(l => l.EntityId))
        {
            var theme = plan.Entities.GetValueOrDefault((NativeV1.Theme, l.EntityId));
            var themeName = theme?.Name ?? $"missing theme {l.EntityId}";
            var body = new StringBuilder()
                .Append("# v1 theme commentary: ").Append(p.Title).Append(" × ").Append(themeName).Append('\n')
                .Append("chapter: ").Append(ChapterLabel(p, chapters)).Append("\n\n");
            body.Append("theme: ").Append(themeName).Append("\n\n");
            FieldIfAny(body, "Theme description", theme?.Description ?? "");
            Field(body, "Plot point synopsis", p.Synopsis);
            body.Append("## Commentary");
            if (l.Qualifiers.Count > 0) body.Append(" (").Append(string.Join("; ", l.Qualifiers)).Append(')');
            body.Append("\n\n").Append(l.Text.Trim()).Append('\n');
            items.Add(new ItemizerOutput.Item($"pp-{p.Id}-theme-{l.EntityId}", body.ToString(), $"pp-{p.Id}/theme-{l.EntityId}",
                $"{ChapterLabel(p, chapters)}: {p.Title} × {themeName}"));
        }
        return items;
    }

    /// <summary>The narrowing line of the plot-point cut, or null when it takes every plot point.</summary>
    public static string? PlotPointNarrowing(NativeV1.Plan plan, IReadOnlySet<int> excludeChapters, bool includeUnplaced)
    {
        var parts = new List<string>();
        var named = plan.Chapters.Where(c => excludeChapters.Contains(c.Id)).OrderBy(c => c.OrderIndex).Select(c => $"{c.OrderIndex} {c.Title}").ToList();
        if (named.Count > 0) parts.Add($"except those in the chapters {string.Join(", ", named)}, the config's excludeChapters");
        if (!includeUnplaced) parts.Add("except those in no chapter");
        return parts.Count == 0 ? null : "every plot point " + string.Join("; and ", parts);
    }

    public const string ThemeCommentaryNarrowing = "the plot point × theme links whose commentary is not blank";

    static IEnumerable<NativeV1.PlotPoint> Ordered(NativeV1.Plan plan, IReadOnlyDictionary<int, NativeV1.Chapter> chapters) =>
        plan.PlotPoints
            .OrderBy(p => p.ChapterId is int c && chapters.TryGetValue(c, out var ch) ? ch.OrderIndex : int.MaxValue)
            .ThenBy(p => p.OrderInChapter)
            .ThenBy(p => p.Id);

    static string ChapterLabel(NativeV1.PlotPoint p, IReadOnlyDictionary<int, NativeV1.Chapter> chapters) =>
        p.ChapterId is int c && chapters.TryGetValue(c, out var ch)
            ? $"chapter {ch.OrderIndex} {ch.Title}, position {p.OrderInChapter}"
            : "unplaced";

    static void Field(StringBuilder body, string heading, string text) =>
        body.Append("## ").Append(heading).Append("\n\n").Append(Text(text)).Append("\n\n");

    /// <summary>A section only when its field holds text: most plot points leave outcome and stakes blank, and a blank field is not the item's content.</summary>
    static void FieldIfAny(StringBuilder body, string heading, string text)
    {
        if (text.Trim().Length > 0) Field(body, heading, text);
    }

    static string Text(string s)
    {
        var t = s.Replace("\r\n", "\n").Replace('\r', '\n').Trim();
        return t.Length == 0 ? NoText : t;
    }
}
