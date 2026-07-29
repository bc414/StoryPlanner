using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure tier — no .storyplan needed. ConversationSyncScanner.Scan is a pure classification
/// function, and it is the piece of the import pipeline most able to do quiet damage: a wrong
/// classification either loses an author's block-triage state (by treating a reopened
/// conversation as New) or silently merges two unrelated conversations.
/// </summary>
public class ConversationSyncScannerTests
{
    // ParsedClaudeConversation carries dates as ISO 8601 *strings* (straight off the export);
    // the DB's Conversation carries DateTime. The scanner reconciles the two.
    private static ParsedClaudeConversation Export(
        string uuid, string title, DateTime date, int blocks, DateTime? updated = null) => new()
    {
        Uuid = uuid,
        Title = title,
        ConversationDate = date.ToString("O"),
        UpdatedAt = (updated ?? date).ToString("O"),
        Blocks = Enumerable.Range(1, blocks)
            .Select(i => new ParsedClaudeBlock
            {
                BlockNumber = i,
                Speaker = i % 2 == 1 ? "user" : "assistant",
                RawContent = $"block {i}"
            })
            .ToList()
    };

    private static Conversation Db(
        int id, string title, DateTime date, int blockCount,
        string uuid = "", string platform = "Claude") => new()
    {
        Id = id,
        Title = title,
        ConversationDate = date,
        BlockCount = blockCount,
        SourceUuid = uuid,
        Platform = platform
    };

    private static readonly DateTime Day = new(2026, 5, 11, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void An_unknown_conversation_is_New()
    {
        var result = ConversationSyncScanner.Scan(
            [Export("uuid-a", "Brand new", Day, 10)], [], []);

        var item = Assert.Single(result);
        Assert.Equal(ConversationSyncClassification.New, item.Classification);
        Assert.Null(item.MatchedConversationId);
    }

    [Fact]
    public void A_uuid_match_with_the_same_block_count_is_Unchanged()
    {
        var result = ConversationSyncScanner.Scan(
            [Export("uuid-a", "Known", Day, 10)],
            [Db(7, "Known", Day, 10, uuid: "uuid-a")],
            []);

        var item = Assert.Single(result);
        Assert.Equal(ConversationSyncClassification.Unchanged, item.Classification);
        Assert.Equal(7, item.MatchedConversationId);
        Assert.Equal(0, item.BlockCountDelta);
    }

    [Fact]
    public void A_uuid_match_with_more_blocks_is_Reopened_and_reports_the_delta()
    {
        var result = ConversationSyncScanner.Scan(
            [Export("uuid-a", "Known", Day, 14)],
            [Db(7, "Known", Day, 10, uuid: "uuid-a")],
            []);

        var item = Assert.Single(result);
        Assert.Equal(ConversationSyncClassification.Reopened, item.Classification);
        Assert.Equal(7, item.MatchedConversationId);
        Assert.Equal(4, item.BlockCountDelta);
    }

    [Fact]
    public void Uuid_matching_is_case_insensitive()
    {
        var result = ConversationSyncScanner.Scan(
            [Export("UUID-A", "Known", Day, 10)],
            [Db(7, "Known", Day, 10, uuid: "uuid-a")],
            []);

        Assert.NotEqual(ConversationSyncClassification.New, Assert.Single(result).Classification);
    }

    [Fact]
    public void An_ignored_uuid_is_Ignored_and_never_resurfaces_as_New()
    {
        var result = ConversationSyncScanner.Scan(
            [Export("uuid-a", "Off topic", Day, 10)],
            [],
            [new IgnoredConversation { SourceUuid = "uuid-a", Title = "Off topic" }]);

        Assert.Equal(ConversationSyncClassification.Ignored, Assert.Single(result).Classification);
    }

    [Fact]
    public void A_pre_uuid_record_matched_by_heuristic_only_ever_reaches_NeedsConfirmation()
    {
        // The pre-uuid backlog: same title, same day, export has >= the DB's blocks.
        var result = ConversationSyncScanner.Scan(
            [Export("uuid-a", "Note categorization bootstrapping", Day, 285)],
            [Db(47, "Note categorization bootstrapping", Day, 280)],
            []);

        var item = Assert.Single(result);
        Assert.Equal(ConversationSyncClassification.NeedsConfirmation, item.Classification);
        Assert.Equal(47, item.ProposedMatchConversationId);
        Assert.Null(item.MatchedConversationId);   // never auto-committed
    }

    [Fact]
    public void Heuristic_matching_never_claims_a_Gemini_record()
    {
        // A Claude export must not heuristically absorb a same-day Gemini conversation —
        // Gemini records are also un-uuid'd, so platform is the only discriminator.
        var result = ConversationSyncScanner.Scan(
            [Export("uuid-a", "Flowing Current", Day, 90)],
            [Db(5, "Flowing Current", Day, 87, platform: "Gemini")],
            []);

        Assert.Equal(ConversationSyncClassification.New, Assert.Single(result).Classification);
    }

    [Fact]
    public void Heuristic_matching_does_not_claim_a_record_with_more_blocks_than_the_export()
    {
        // Fewer blocks in the export than the DB means it isn't the same conversation grown.
        var result = ConversationSyncScanner.Scan(
            [Export("uuid-a", "Known", Day, 5)],
            [Db(7, "Known", Day, 50)],
            []);

        Assert.Equal(ConversationSyncClassification.New, Assert.Single(result).Classification);
    }

    [Fact]
    public void Two_exports_cannot_both_claim_the_same_db_candidate()
    {
        var result = ConversationSyncScanner.Scan(
            [
                Export("uuid-a", "Shared title", Day, 30),
                Export("uuid-b", "Shared title", Day, 31)
            ],
            [Db(7, "Shared title", Day, 20)],
            []);

        var proposed = result.Count(r => r.ProposedMatchConversationId == 7);
        Assert.Equal(1, proposed);   // the candidate is claimed exactly once
    }

    [Fact]
    public void Scan_is_read_only_and_returns_one_row_per_export()
    {
        var db = new List<Conversation> { Db(7, "Known", Day, 10, uuid: "uuid-a") };
        var ignored = new List<IgnoredConversation>();

        var result = ConversationSyncScanner.Scan(
            [Export("uuid-a", "Known", Day, 10), Export("uuid-b", "Other", Day, 3)],
            db, ignored);

        Assert.Equal(2, result.Count);
        Assert.Single(db);        // untouched
        Assert.Empty(ignored);    // untouched
    }
}
