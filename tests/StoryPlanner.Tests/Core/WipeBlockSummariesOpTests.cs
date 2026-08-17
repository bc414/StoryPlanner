using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.DataOps;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Runs DataOpEnvelope + WipeBlockSummaries against a dedicated two-conversation file — the
/// shared SyntheticPlan baseline seeds no conversation rows at all.
///
/// What these tests are really guarding is the asymmetry: the op must destroy exactly one column
/// and nothing else. Block summaries are machine-written text on their way out; RawContent,
/// triage state and ArcSummary are not, and there is no undo.
/// </summary>
public class WipeBlockSummariesOpTests
{
    private const string ValidConfig = """{ "confirm": "wipe-all-conversation-block-summaries" }""";

    [Fact]
    public async Task Apply_clears_every_summary_and_leaves_the_rest_of_the_block_alone()
    {
        var (path, dir) = await BuildTwoConversationFile();
        try
        {
            var exitCode = await DataOpEnvelope.RunAsync(
                new WipeBlockSummaries(), path, ParseConfig(ValidConfig), apply: true);
            Assert.Equal(0, exitCode);

            using var verify = OpenContext(path);
            var blocks = await verify.ConversationBlocks.OrderBy(b => b.Id).ToListAsync();

            Assert.All(blocks, b => Assert.Equal("", b.Summary));

            // The irreplaceable half of the table is byte-identical.
            Assert.Equal(["user", "assistant", "user", "assistant"], blocks.Select(b => b.Speaker));
            Assert.Equal(["Turn one.", "Turn two.", "Turn three.", "Turn four."],
                         blocks.Select(b => b.RawContent));
            Assert.Equal([BlockState.Done, BlockState.Flagged, BlockState.Unread, BlockState.Skipped],
                         blocks.Select(b => b.BlockState));

            // ArcSummary is explicitly out of scope — it stays as frozen historical text.
            var convs = await verify.Conversations.OrderBy(c => c.Id).ToListAsync();
            Assert.Equal(["The arc, as summarized earlier.", ""], convs.Select(c => c.ArcSummary));
        }
        finally
        {
            TryDelete(dir);
        }
    }

    [Fact]
    public async Task Dry_run_reports_zero_violations_but_persists_nothing()
    {
        var (path, dir) = await BuildTwoConversationFile();
        try
        {
            var exitCode = await DataOpEnvelope.RunAsync(
                new WipeBlockSummaries(), path, ParseConfig(ValidConfig), apply: false);
            Assert.Equal(0, exitCode);

            using var verify = OpenContext(path);
            var summaries = await verify.ConversationBlocks.OrderBy(b => b.Id)
                                        .Select(b => b.Summary).ToListAsync();
            Assert.Equal(["Block one, summarized.", "Block two, summarized.", "", "Block four, summarized."],
                         summaries);
        }
        finally
        {
            TryDelete(dir);
        }
    }

    [Fact]
    public async Task A_second_apply_is_a_no_op()
    {
        var (path, dir) = await BuildTwoConversationFile();
        try
        {
            var first = await DataOpEnvelope.RunAsync(
                new WipeBlockSummaries(), path, ParseConfig(ValidConfig), apply: true);
            Assert.Equal(0, first);

            var second = await DataOpEnvelope.RunAsync(
                new WipeBlockSummaries(), path, ParseConfig(ValidConfig), apply: true);
            Assert.Equal(0, second);

            using var verify = OpenContext(path);
            Assert.Equal(4, await verify.ConversationBlocks.CountAsync());
            Assert.All(await verify.ConversationBlocks.ToListAsync(), b => Assert.Equal("", b.Summary));
        }
        finally
        {
            TryDelete(dir);
        }
    }

    [Fact]
    public async Task A_config_without_the_confirmation_token_is_refused_and_writes_nothing()
    {
        var (path, dir) = await BuildTwoConversationFile();
        try
        {
            var exitCode = await DataOpEnvelope.RunAsync(
                new WipeBlockSummaries(), path, ParseConfig("""{ "confirm": "yes" }"""), apply: true);
            Assert.Equal(1, exitCode);

            using var verify = OpenContext(path);
            Assert.Equal("Block one, summarized.",
                         await verify.ConversationBlocks.OrderBy(b => b.Id).Select(b => b.Summary).FirstAsync());
        }
        finally
        {
            TryDelete(dir);
        }
    }

    private static JsonElement ParseConfig(string json) => JsonDocument.Parse(json).RootElement;

    private static AppDbContext OpenContext(string path) =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={path}").Options);

    private static void TryDelete(string dir)
    {
        try { Directory.Delete(dir, recursive: true); } catch (IOException) { }
    }

    /// <summary>
    /// Two conversations, four blocks. One block starts with an empty summary and one
    /// conversation with an empty ArcSummary, so "already empty" is exercised on both.
    /// </summary>
    private static async Task<(string Path, string Dir)> BuildTwoConversationFile()
    {
        var dir = Directory.CreateTempSubdirectory("wipe-block-summaries-tests-");
        var file = Path.Combine(dir.FullName, "conversations.storyplan");

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={file}").Options;
        using var ctx = new AppDbContext(options);
        await ctx.Database.MigrateAsync();

        ctx.Conversations.AddRange(
            new Conversation { Id = 1, Title = "First", Platform = "Claude", BlockCount = 2, ArcSummary = "The arc, as summarized earlier." },
            new Conversation { Id = 2, Title = "Second", Platform = "Gemini", BlockCount = 2, ArcSummary = "" });
        ctx.ConversationBlocks.AddRange(
            new ConversationBlock { Id = 1, ConversationId = 1, BlockNumber = 1, Speaker = "user",      RawContent = "Turn one.",   Summary = "Block one, summarized.",  BlockState = BlockState.Done },
            new ConversationBlock { Id = 2, ConversationId = 1, BlockNumber = 2, Speaker = "assistant", RawContent = "Turn two.",   Summary = "Block two, summarized.",  BlockState = BlockState.Flagged },
            new ConversationBlock { Id = 3, ConversationId = 2, BlockNumber = 1, Speaker = "user",      RawContent = "Turn three.", Summary = "",                        BlockState = BlockState.Unread },
            new ConversationBlock { Id = 4, ConversationId = 2, BlockNumber = 2, Speaker = "assistant", RawContent = "Turn four.",  Summary = "Block four, summarized.", BlockState = BlockState.Skipped });
        await ctx.SaveChangesAsync();

        return (file, dir.FullName);
    }
}
