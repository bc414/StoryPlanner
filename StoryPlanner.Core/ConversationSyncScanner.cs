using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public enum ConversationSyncClassification
{
    /// <summary>No DB match by uuid and no heuristic match. Most of these are off-topic and
    /// won't be exported — that's expected; the user hand-picks what actually goes to Cowork.</summary>
    New,

    /// <summary>Certain match (DB record already carries this uuid) and the export has more
    /// blocks / a newer updated_at than what's stored. No ambiguity.</summary>
    Reopened,

    /// <summary>Certain match (uuid) with the same block count / updated_at. Nothing to do.</summary>
    Unchanged,

    /// <summary>A *guessed* match to an older DB record that has no stored uuid, based on the
    /// date + block-count heuristic (see FindHeuristicMatch). Must be confirmed by the user before
    /// it's treated as the same conversation — this is a one-time reconciliation state for the
    /// pre-uuid backlog. The user can also skip the guess entirely and manually map any New or
    /// NeedsConfirmation row to a specific DB conversation.</summary>
    NeedsConfirmation,

    /// <summary>The export uuid is in the ignore list (marked "not story" previously). Hidden by
    /// default in the scan preview so it stops resurfacing as New on every scan.</summary>
    Ignored
}

/// <summary>One row of a scan result: an exported Claude conversation plus how it relates to
/// what's already in the DB. No writes happen while building this — it's purely advisory for the
/// Scan Preview UI, which is where the user decides what actually gets exported for Cowork.</summary>
public class ConversationSyncItem
{
    public required ParsedClaudeConversation Export { get; init; }
    public ConversationSyncClassification Classification { get; set; }

    /// <summary>Set when Classification is Reopened or Unchanged — the certain DB match.</summary>
    public int? MatchedConversationId { get; set; }

    /// <summary>Set when Classification is NeedsConfirmation — the guessed DB match awaiting
    /// the user's confirm/reject.</summary>
    public int?   ProposedMatchConversationId { get; set; }
    public string ProposedMatchTitle          { get; set; } = string.Empty;

    /// <summary>The matched (or proposed) DB record's current block count; 0 when there's no match at all.</summary>
    public int DbBlockCount { get; set; }

    /// <summary>The matched (or proposed) DB record's existing NNN_{slug} prefix, reused on export
    /// so a reopened conversation's content file keeps the same name across cycles.</summary>
    public string ExistingSourceFilePrefix { get; set; } = string.Empty;

    public int ExportBlockCount  => Export.Blocks.Count;
    public int BlockCountDelta   => ExportBlockCount - DbBlockCount;
}

/// <summary>
/// Compares a freshly parsed Claude export against the StoryPlanner DB's existing conversations
/// (the "ground truth") and classifies each exported conversation as New / Reopened / Unchanged /
/// NeedsConfirmation / Ignored. Read-only — callers act on the result (confirm a match, ignore a
/// conversation, export selected content files) via separate IStoryService calls.
/// </summary>
public static class ConversationSyncScanner
{
    public static List<ConversationSyncItem> Scan(
        IEnumerable<ParsedClaudeConversation> exported,
        IEnumerable<Conversation> dbConversations,
        IEnumerable<IgnoredConversation> ignored)
    {
        var ignoredUuids = ignored.Select(i => i.SourceUuid).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var dbList = dbConversations.ToList();
        var dbByUuid = dbList
            .Where(c => !string.IsNullOrEmpty(c.SourceUuid))
            .ToDictionary(c => c.SourceUuid, StringComparer.OrdinalIgnoreCase);

        // Un-uuid'd Claude DB records are the only candidates for heuristic matching. Platform is
        // filtered here — without it, a Claude export conversation can heuristically match a
        // same-day Gemini conversation (also un-uuid'd, since Gemini was never given a Claude uuid),
        // which is never correct. Track which candidates have already been proposed so two export
        // conversations don't both claim the same record.
        var unmatchedDbCandidates = dbList
            .Where(c => string.IsNullOrEmpty(c.SourceUuid) && c.Platform == "Claude")
            .ToList();
        var claimedCandidateIds = new HashSet<int>();

        var results = new List<ConversationSyncItem>();

        foreach (var conv in exported)
        {
            if (ignoredUuids.Contains(conv.Uuid))
            {
                results.Add(new ConversationSyncItem { Export = conv, Classification = ConversationSyncClassification.Ignored });
                continue;
            }

            if (!string.IsNullOrEmpty(conv.Uuid) && dbByUuid.TryGetValue(conv.Uuid, out var certain))
            {
                bool changed = conv.Blocks.Count > certain.BlockCount ||
                               IsNewer(conv.UpdatedAt, certain.SourceUpdatedAt);

                results.Add(new ConversationSyncItem
                {
                    Export                   = conv,
                    Classification            = changed ? ConversationSyncClassification.Reopened : ConversationSyncClassification.Unchanged,
                    MatchedConversationId     = certain.Id,
                    DbBlockCount              = certain.BlockCount,
                    ExistingSourceFilePrefix  = certain.SourceFilePrefix
                });
                continue;
            }

            var candidate = FindHeuristicMatch(conv, unmatchedDbCandidates, claimedCandidateIds);
            if (candidate is not null)
            {
                claimedCandidateIds.Add(candidate.Id);
                results.Add(new ConversationSyncItem
                {
                    Export                       = conv,
                    Classification                = ConversationSyncClassification.NeedsConfirmation,
                    ProposedMatchConversationId  = candidate.Id,
                    ProposedMatchTitle           = candidate.Title,
                    DbBlockCount                  = candidate.BlockCount,
                    ExistingSourceFilePrefix      = candidate.SourceFilePrefix
                });
                continue;
            }

            results.Add(new ConversationSyncItem { Export = conv, Classification = ConversationSyncClassification.New });
        }

        return results;
    }

    /// <summary>
    /// Date + block-count heuristic for reconciling the pre-uuid backlog. Requires the export's
    /// block count to be at least the DB record's — a reopened conversation only ever gains
    /// blocks, so if the export has *fewer* blocks it's a different conversation, not a match.
    ///
    /// Title is NOT required to match: conversation titles are editable in the library, so a
    /// conversation renamed after import would otherwise never auto-propose a match and would
    /// silently fall through to New on every scan. An exact title match is still preferred when
    /// one exists among the date-window candidates (highest confidence, and disambiguates
    /// same-day conversations) but isn't required — every proposal is NeedsConfirmation either
    /// way, so the user reviews and confirms/rejects it by hand regardless of how it was found.
    ///
    /// The date check tolerates a 1-day drift rather than requiring an exact calendar-day match.
    /// Conversations imported before this pipeline existed were stored with ConversationDate
    /// converted to local time (a plain DateTime.TryParse on a "Z"-suffixed UTC string implicitly
    /// does that); this scanner parses the export's date preserving UTC. For a conversation near
    /// local midnight those two representations can land on different calendar days, so an exact
    /// match would silently miss it — a 1-day window safely absorbs any timezone offset.
    /// </summary>
    private static Conversation? FindHeuristicMatch(
        ParsedClaudeConversation conv,
        List<Conversation> candidates,
        HashSet<int> claimedCandidateIds)
    {
        DateTime? exportDate = ParseDate(conv.ConversationDate);
        if (!exportDate.HasValue) return null;

        var dateWindowCandidates = candidates
            .Where(c => !claimedCandidateIds.Contains(c.Id) &&
                        Math.Abs((c.ConversationDate.Date - exportDate.Value.Date).TotalDays) <= 1 &&
                        conv.Blocks.Count >= c.BlockCount)
            .ToList();

        if (dateWindowCandidates.Count == 0) return null;

        string normalizedTitle = NormalizeTitle(conv.Title);
        return dateWindowCandidates.FirstOrDefault(c => NormalizeTitle(c.Title) == normalizedTitle)
               ?? dateWindowCandidates[0];
    }

    private static string NormalizeTitle(string title) =>
        title.Trim().ToLowerInvariant();

    private static bool IsNewer(string exportUpdatedAtIso, DateTime? storedUpdatedAt)
    {
        if (!storedUpdatedAt.HasValue) return false;
        var exportUpdatedAt = ParseDate(exportUpdatedAtIso);
        return exportUpdatedAt.HasValue && exportUpdatedAt.Value > storedUpdatedAt.Value;
    }

    private static DateTime? ParseDate(string iso) =>
        DateTime.TryParse(iso, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt) ? dt : null;
}
