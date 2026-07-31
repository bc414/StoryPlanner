using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Fixture-tier tests for <see cref="ConversationImporter"/>'s merge semantics.
///
/// These guard three things a regression would break silently: that summaries are OPTIONAL (a
/// content file with no meta partner imports rather than being skipped), that a content-only
/// re-import never erases summaries or read-state a previous pass established, and that a meta
/// file's subjectsCovered array — present in every meta file authored before 2026-07-31 — writes
/// no coverage rows. That last one is the reason the AI-suggested-track cut is enforceable rather
/// than merely done: reconnecting the path fails a test instead of quietly refilling the tables.
/// </summary>
public class ConversationImporterTests
{
    private const string Uuid = "11111111-2222-3333-4444-555555555555";

    private static AppDbContext OpenContext(SyntheticPlan plan)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={plan.Path}")
            .Options;
        return new AppDbContext(options);
    }

    private static ConversationImportSource Source(string uuid, params (int Number, string Speaker, string Text)[] blocks) =>
        new(Platform: "Claude",
            Title: "A design conversation",
            ConversationDate: "2026-07-01T10:00:00Z",
            SourceUuid: uuid,
            SourceUpdatedAt: "2026-07-01T12:00:00Z",
            Blocks: blocks
                .Select(b => new ConversationImportBlock(b.Number, b.Speaker, b.Text, IsCompaction: false))
                .ToList());

    // ── Summaries are optional ────────────────────────────────────────────────

    [Fact]
    public async Task Import_without_meta_creates_the_conversation_with_empty_summaries()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);

        var result = await new ConversationImporter(ctx).ImportAsync(
            Source(Uuid, (1, "user", "What if the nursery is self-protection?"),
                         (2, "assistant", "Then the reveal lands two chapters early.")),
            "001_a-design-conversation",
            meta: null);

        Assert.Equal(1, result.Created);
        Assert.Equal(1, result.WithoutSummaries);

        var conv = Assert.Single(await ctx.Conversations.ToListAsync());
        Assert.Equal("", conv.ArcSummary);
        Assert.Equal(2, conv.BlockCount);

        var blocks = await ctx.ConversationBlocks.OrderBy(b => b.BlockNumber).ToListAsync();
        Assert.Equal(2, blocks.Count);
        Assert.All(blocks, b => Assert.Equal("", b.Summary));
        Assert.All(blocks, b => Assert.Equal(BlockState.Unread, b.BlockState));
        // Raw content is the point of a summary-less import — it must be fully present.
        Assert.Contains("nursery", blocks[0].RawContent);
    }

    [Fact]
    public async Task Folder_import_takes_a_content_file_with_no_meta_partner()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);

        var folder = Directory.CreateTempSubdirectory("storyplan-import-");
        try
        {
            await File.WriteAllTextAsync(Path.Combine(folder.FullName, "007_bare_content.json"), """
                {
                  "platform": "Claude",
                  "title": "Bare conversation",
                  "conversationDate": "2026-07-02T09:00:00Z",
                  "sourceUuid": "bare-uuid",
                  "sourceUpdatedAt": "2026-07-02T09:30:00Z",
                  "blocks": [
                    { "blockNumber": 1, "speaker": "user", "rawContent": "First turn.", "isCompaction": false }
                  ]
                }
                """);

            var result = await new ConversationImporter(ctx).ImportFolderAsync(folder.FullName);

            Assert.Equal(1, result.Created);
            Assert.Equal(1, result.WithoutSummaries);
            var conv = Assert.Single(await ctx.Conversations.ToListAsync());
            Assert.Equal("Bare conversation", conv.Title);
            Assert.Equal("007_bare", conv.SourceFilePrefix);
        }
        finally
        {
            folder.Delete(recursive: true);
        }
    }

    // ── Meta enriches, never destroys ─────────────────────────────────────────

    [Fact]
    public async Task Meta_pass_over_a_bare_conversation_adds_summaries_and_keeps_read_state()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);
        var importer = new ConversationImporter(ctx);

        var source = Source(Uuid, (1, "user", "First turn."), (2, "assistant", "Second turn."));
        await importer.ImportAsync(source, "001_a-design-conversation", meta: null);

        // Triage block 1 before the summaries arrive — the reader's state must outlive re-import.
        var block1 = await ctx.ConversationBlocks.SingleAsync(b => b.BlockNumber == 1);
        block1.BlockState = BlockState.Done;
        await ctx.SaveChangesAsync();

        var meta = new ConversationMeta
        {
            ArcSummary = "Establishes the immortality rules.",
            Blocks =
            {
                new ConversationMetaBlock { BlockNumber = 1, Summary = "User sets the rules." },
                new ConversationMetaBlock { BlockNumber = 2, Summary = "Assistant draws implications." }
            }
        };

        var result = await importer.ImportAsync(source, "001_a-design-conversation", meta);

        Assert.Equal(1, result.Updated);
        Assert.Equal(0, result.WithoutSummaries);

        var conv = Assert.Single(await ctx.Conversations.ToListAsync());
        Assert.Equal("Establishes the immortality rules.", conv.ArcSummary);

        var blocks = await ctx.ConversationBlocks.OrderBy(b => b.BlockNumber).ToListAsync();
        Assert.Equal("User sets the rules.", blocks[0].Summary);
        Assert.Equal(BlockState.Done, blocks[0].BlockState);   // survived the re-import
        Assert.Equal(BlockState.Unread, blocks[1].BlockState);
    }

    [Fact]
    public async Task Content_only_reimport_preserves_existing_summaries_and_adds_the_new_turns()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);
        var importer = new ConversationImporter(ctx);

        var meta = new ConversationMeta
        {
            ArcSummary = "The arc, as summarized earlier.",
            Blocks = { new ConversationMetaBlock { BlockNumber = 1, Summary = "Block one, summarized." } }
        };
        await importer.ImportAsync(Source(Uuid, (1, "user", "First turn.")), "001_conv", meta);

        // The conversation is reopened and re-scanned; no fresh meta pass has been run.
        await importer.ImportAsync(
            Source(Uuid, (1, "user", "First turn."), (2, "assistant", "A newly added turn.")),
            "001_conv",
            meta: null);

        var conv = Assert.Single(await ctx.Conversations.ToListAsync());
        Assert.Equal("The arc, as summarized earlier.", conv.ArcSummary);
        Assert.Equal(2, conv.BlockCount);

        var blocks = await ctx.ConversationBlocks.OrderBy(b => b.BlockNumber).ToListAsync();
        Assert.Equal("Block one, summarized.", blocks[0].Summary);  // not blanked
        Assert.Equal("", blocks[1].Summary);                        // genuinely new, never summarized
    }

    // ── The suggestion path stays cut ─────────────────────────────────────────

    [Fact]
    public async Task Meta_file_carrying_subjectsCovered_writes_no_coverage_rows()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);

        var folder = Directory.CreateTempSubdirectory("storyplan-import-");
        try
        {
            await File.WriteAllTextAsync(Path.Combine(folder.FullName, "001_legacy_content.json"), """
                {
                  "platform": "Claude",
                  "title": "Legacy pair",
                  "conversationDate": "2026-07-03T09:00:00Z",
                  "sourceUuid": "legacy-uuid",
                  "sourceUpdatedAt": "2026-07-03T09:30:00Z",
                  "blocks": [
                    { "blockNumber": 1, "speaker": "user", "rawContent": "A turn.", "isCompaction": false }
                  ]
                }
                """);

            // Shaped exactly like the meta files authored before the 2026-07-31 cut: the
            // subjectsCovered array is still there and must be inert, not merely unused.
            await File.WriteAllTextAsync(Path.Combine(folder.FullName, "001_legacy_meta.json"), $$"""
                {
                  "arcSummary": "An arc.",
                  "subjectsCovered": [
                    { "subjectId": {{SyntheticPlan.SubjectId}}, "subjectName": "Testcharacter",
                      "noteTrackDefinitionIds": [{{SyntheticPlan.BackstoryTrackId}}] }
                  ],
                  "blocks": [ { "blockNumber": 1, "summary": "A summary.", "hasDecisions": true } ]
                }
                """);

            await new ConversationImporter(ctx).ImportFolderAsync(folder.FullName);

            // The summaries land...
            var conv = Assert.Single(await ctx.Conversations.ToListAsync());
            Assert.Equal("An arc.", conv.ArcSummary);
            Assert.Equal("A summary.", (await ctx.ConversationBlocks.SingleAsync()).Summary);

            // ...and nothing proposes structure.
            Assert.Empty(await ctx.ConversationSubjectCoverages.ToListAsync());
            Assert.Empty(await ctx.ConversationSubjectCoverageTracks.ToListAsync());
        }
        finally
        {
            folder.Delete(recursive: true);
        }
    }

    // ── Direct import from a scan ─────────────────────────────────────────────

    [Fact]
    public async Task Scanned_import_numbers_new_conversations_above_the_highest_existing_prefix()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);

        ctx.Conversations.Add(new Conversation
        {
            Title = "Already here", Platform = "Claude", ConversationDate = new DateTime(2026, 6, 1),
            BlockCount = 3, SourceFilePrefix = "042_already-here", SourceUuid = "existing-uuid"
        });
        await ctx.SaveChangesAsync();

        var items = new List<ConversationSyncItem>
        {
            new() { Export = Export("uuid-a", "First new one"),  Classification = ConversationSyncClassification.New },
            new() { Export = Export("uuid-b", "Second new one"), Classification = ConversationSyncClassification.New }
        };

        var result = await new ConversationImporter(ctx).ImportScannedAsync(items);

        Assert.Equal(2, result.Created);
        Assert.Equal(2, result.WithoutSummaries);

        var prefixes = await ctx.Conversations
            .Where(c => c.SourceUuid != "existing-uuid")
            .Select(c => c.SourceFilePrefix)
            .OrderBy(p => p)
            .ToListAsync();

        // Continues above 042 and does not collide within the batch.
        Assert.Equal(new[] { "043_first-new-one", "044_second-new-one" }, prefixes);
    }

    [Fact]
    public async Task Scanned_import_of_a_reopened_row_updates_in_place_and_keeps_its_prefix()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);

        ctx.Conversations.Add(new Conversation
        {
            Title = "Older title", Platform = "Claude", ConversationDate = new DateTime(2026, 6, 1),
            BlockCount = 1, SourceFilePrefix = "005_older-title", SourceUuid = "uuid-reopened",
            ArcSummary = "Summarized in an earlier pass."
        });
        await ctx.SaveChangesAsync();

        var item = new ConversationSyncItem
        {
            Export = Export("uuid-reopened", "Newer title", blockCount: 3),
            Classification = ConversationSyncClassification.Reopened,
            ExistingSourceFilePrefix = "005_older-title"
        };

        var result = await new ConversationImporter(ctx).ImportScannedAsync([item]);

        Assert.Equal(0, result.Created);
        Assert.Equal(1, result.Updated);

        var conv = Assert.Single(await ctx.Conversations.ToListAsync());
        Assert.Equal("005_older-title", conv.SourceFilePrefix);
        Assert.Equal("Newer title", conv.Title);
        Assert.Equal(3, conv.BlockCount);
        Assert.Equal("Summarized in an earlier pass.", conv.ArcSummary); // a raw re-import never blanks it
    }

    private static ParsedClaudeConversation Export(string uuid, string title, int blockCount = 2) =>
        new()
        {
            Uuid = uuid,
            Title = title,
            ConversationDate = "2026-07-05T08:00:00Z",
            UpdatedAt = "2026-07-05T09:00:00Z",
            Blocks = Enumerable.Range(1, blockCount)
                .Select(i => new ParsedClaudeBlock
                {
                    BlockNumber = i,
                    Speaker = i % 2 == 1 ? "user" : "assistant",
                    RawContent = $"Turn {i}."
                })
                .ToList()
        };
}
