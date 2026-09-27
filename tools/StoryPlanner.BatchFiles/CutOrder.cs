using System.Security.Cryptography;
using System.Text;

namespace StoryPlanner.BatchFiles;

/// <summary>
/// The cut's order (d-2026-09-27-5): the order a batch calls its items in, ascending by the
/// SHA-256 hash of each item's slug taken over its UTF-8 bytes. The hash depends on the slug
/// alone, so every batch cut the same way calls its items in the same order, whatever the study,
/// the directions version or the execution; the index keeps its own order for reading. The runner
/// orders its calls by it and DocIntegrity derives the items a batch answered out of it, through
/// this one function.
/// </summary>
public static class CutOrder
{
    /// <summary>The SHA-256 of the slug's UTF-8 bytes, lowercase hex; no newline normalisation, a slug having none.</summary>
    public static string KeyOf(string slug)
        => Convert.ToHexStringLower(SHA256.HashData(new UTF8Encoding(false).GetBytes(slug)));

    /// <summary>The items in the cut's order. Hex of equal length compares as the bytes do, so an ordinal sort of the keys is the byte order.</summary>
    public static IReadOnlyList<string> Of(IEnumerable<string> items)
        => items.Select(i => (Item: i, Key: KeyOf(i)))
            .OrderBy(x => x.Key, StringComparer.Ordinal)
            .ThenBy(x => x.Item, StringComparer.Ordinal)
            .Select(x => x.Item)
            .ToList();

    /// <summary>
    /// The answered items that sit after the first unanswered item in the cut's order: those
    /// called out of turn, by a queue jump, a shuffle, or while a failed call waited. A batch
    /// whose every item has answered has none.
    /// </summary>
    public static IReadOnlyList<string> AnsweredOutOfOrder(IEnumerable<string> items, IReadOnlySet<string> answered)
    {
        var order = Of(items);
        var firstGap = -1;
        for (var i = 0; i < order.Count; i++)
            if (!answered.Contains(order[i])) { firstGap = i; break; }
        if (firstGap < 0) return [];
        return order.Skip(firstGap + 1).Where(answered.Contains).ToList();
    }
}
