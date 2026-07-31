using System.Text.RegularExpressions;

namespace StoryPlanner.Core;

/// <summary>
/// The NNN_{slug} identity a conversation carries in <see cref="Conversation.SourceFilePrefix"/>.
///
/// Shared by both halves of the pipeline on purpose: <see cref="ConversationContentExporter"/>
/// names its content files with it, and <see cref="ConversationImporter.ImportScannedAsync"/>
/// assigns the same prefix when importing straight from a scan. That's what lets a conversation
/// imported raw today be exported for a summary pass later and pair back up with the DB record it
/// came from, instead of arriving as a duplicate.
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
