using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Fixture-tier tests for <see cref="ConversationImporter"/>'s merge semantics.
///
/// The important ones here guard AUTHORED content against an import. Since 2026-08-11 a block's
/// Summary is Brian's own hand-written navigation note and BlockState is his triage; the importer
/// writes neither, ever, and there is no undo if it ever starts. A re-import must refresh the
/// transcript and nothing else.
///
/// The rest guard two deliberate inertnesses. A legacy *_meta.json's summaries write nothing (the
/// Cowork round trip that produced them is retired), and its subjectsCovered array writes no
/// coverage rows (the AI-suggested subject×track routing, cut 2026-07-31). Both cuts are
/// enforceable rather than merely done: reconnecting either path fails a test instead of quietly
/// refilling the field.
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

    // ── Blocks arrive bare; the note is Brian's to write ──────────────────────

    [Fact]
    public async Task Import_creates_the_conversation_with_empty_summaries()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);

        var result = await new ConversationImporter(ctx).ImportAsync(
            Source(Uuid, (1, "user", "What if the nursery is self-protection?"),
                         (2, "assistant", "Then the reveal lands two chapters early.")),
            "001_a-design-conversation");

        Assert.Equal(1, result.Created);

        var conv = Assert.Single(await ctx.Conversations.ToListAsync());
        Assert.Equal("", conv.ArcSummary);
        Assert.Equal(2, conv.BlockCount);

        var blocks = await ctx.ConversationBlocks.OrderBy(b => b.BlockNumber).ToListAsync();
        Assert.Equal(2, blocks.Count);
        Assert.All(blocks, b => Assert.Equal("", b.Summary));
        Assert.All(blocks, b => Assert.Equal(BlockState.Unread, b.BlockState));
        // Raw content is the whole payload of an import — it must be fully present.
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
            var conv = Assert.Single(await ctx.Conversations.ToListAsync());
            Assert.Equal("Bare conversation", conv.Title);
            Assert.Equal("007_bare", conv.SourceFilePrefix);
        }
        finally
        {
            folder.Delete(recursive: true);
        }
    }

    // ── An import never writes an authored field ──────────────────────────────

    [Fact]
    public async Task Reimport_preserves_hand_written_block_summaries_and_adds_the_new_turns()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);
        var importer = new ConversationImporter(ctx);

        await importer.ImportAsync(Source(Uuid, (1, "user", "First turn.")), "001_conv");

        // Brian reads the block and writes his own note on it, then triages it.
        var block1 = await ctx.ConversationBlocks.SingleAsync(b => b.BlockNumber == 1);
        block1.Summary    = "Where the tariff argument actually starts.";
        block1.BlockState = BlockState.Done;
        await ctx.SaveChangesAsync();

        // The conversation is reopened and re-scanned. His note and his triage must survive:
        // there is no undo, so an importer that overwrites either destroys authored work.
        await importer.ImportAsync(
            Source(Uuid, (1, "user", "First turn."), (2, "assistant", "A newly added turn.")),
            "001_conv");

        var conv = Assert.Single(await ctx.Conversations.ToListAsync());
        Assert.Equal(2, conv.BlockCount);

        var blocks = await ctx.ConversationBlocks.OrderBy(b => b.BlockNumber).ToListAsync();
        Assert.Equal("Where the tariff argument actually starts.", blocks[0].Summary);
        Assert.Equal(BlockState.Done, blocks[0].BlockState);
        Assert.Equal("", blocks[1].Summary);                  // genuinely new, never annotated
        Assert.Equal(BlockState.Unread, blocks[1].BlockState);
    }

    [Fact]
    public async Task A_legacy_meta_file_never_overwrites_a_hand_written_summary()
    {
        using var plan = SyntheticPlan.Create();
        await using var ctx = OpenContext(plan);
        var importer = new ConversationImporter(ctx);

        var folder = Directory.CreateTempSubdirectory("storyplan-import-");
        try
        {
            await File.WriteAllTextAsync(Path.Combine(folder.FullName, "001_conv_content.json"), """
                {
                  "platform": "Claude",
                  "title": "A design conversation",
                  "conversationDate": "2026-07-01T10:00:00Z",
                  "sourceUuid": "meta-clobber-uuid",
                  "sourceUpdatedAt": "2026-07-01T12:00:00Z",
                  "blocks": [
                    { "blockNumber": 1, "speaker": "user", "rawContent": "First turn.", "isCompaction": false }
                  ]
                }
                """);
            await File.WriteAllTextAsync(Path.Combine(folder.FullName, "001_conv_meta.json"), """
                {
                  "arcSummary": "An AI-written arc summary.",
                  "blocks": [ { "blockNumber": 1, "summary": "An AI-written block summary." } ]
                }
                """);

            await importer.ImportFolderAsync(folder.FullName);

            var block = await ctx.ConversationBlocks.SingleAsync();
            Assert.Equal("", block.Summary);        // the meta wrote nothing on the way in
            block.Summary = "Brian's own note.";
            await ctx.SaveChangesAsync();

            // Re-import the same folder, meta and all.
            await importer.ImportFolderAsync(folder.FullName);

            Assert.Equal("Brian's own note.", (await ctx.ConversationBlocks.SingleAsync()).Summary);
            Assert.Equal("", (await ctx.Conversations.SingleAsync()).ArcSummary);
        }
        finally
        {
            folder.Delete(recursive: true);
        }
    }

    // ── Both suggestion paths stay cut ────────────────────────────────────────

    [Fact]
    public async Task Legacy_meta_file_writes_neither_summaries_nor_coverage_rows()
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
            // subjectsCovered array is still there, and so are the summaries retired on
            // 2026-08-11. All of it must be inert, not merely unused.
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

            // The content lands...
            var conv = Assert.Single(await ctx.Conversations.ToListAsync());
            Assert.Equal("Legacy pair", conv.Title);
            Assert.Equal("A turn.", (await ctx.ConversationBlocks.SingleAsync()).RawContent);

            // ...and none of the machine-written text does.
            Assert.Equal("", conv.ArcSummary);
            Assert.Equal("", (await ctx.ConversationBlocks.SingleAsync()).Summary);
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
        // Frozen historical text from the retired Cowork pass: nothing writes ArcSummary any
        // more, and a re-import must not blank what is already there.
        Assert.Equal("Summarized in an earlier pass.", conv.ArcSummary);
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
