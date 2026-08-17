using System.Text.RegularExpressions;

namespace StoryPlanner.Core;

/// <summary>
/// The NNN_{slug} identity a conversation carries in <see cref="Conversation.SourceFilePrefix"/>.
///
/// <see cref="ConversationImporter.ImportScannedAsync"/> assigns it when importing straight from a
/// scan, and it is the fallback match key when a conversation has no sourceUuid — which is what
/// lets a legacy NNN_{slug}_content.json folder still pair back up with the record it produced,
/// instead of arriving as a duplicate. It outlived the file export it was named for (retired
/// 2026-08-11) because that matching job is entirely its own.
/// </summary>
public static class ConversationPrefix
{
    /// <summary>The next free NNN above the highest already claimed by any DB conversation.</summary>
    public static int NextIndex(IEnumerable<Conversation> existing) =>
        existing.Select(c => LeadingIndex(c.SourceFilePrefix)).DefaultIfEmpty(0).Max() + 1;

    public static string Build(int index, string title) => $"{index:D3}_{Slugify(title)}";

    public static int LeadingIndex(string prefix)
    {
        if (string.IsNullOrEmpty(prefix)) return 0;
        var digits = new string(prefix.TakeWhile(char.IsDigit).ToArray());
        return int.TryParse(digits, out var n) ? n : 0;
    }

    public static string Slugify(string title)
    {
        string lower   = title.ToLowerInvariant();
        string ascii   = Regex.Replace(lower, @"[^a-z0-9\s-]", " ");
        string hyphens = Regex.Replace(ascii.Trim(), @"\s+", "-");
        string clean   = Regex.Replace(hyphens, @"-+", "-").Trim('-');
        return clean.Length > 60 ? clean[..60].TrimEnd('-') : clean;
    }
}
