using System;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using StoryPlanner.Core;
using StoryPlanner.VoiceAttribution;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// What voice attribution added for the v2 working plan (2026-09-28): the Conversations corpus as
/// a voice source, the plan's own dated backups as role-brian span sources, the LastModified
/// check, and per-file state names. The matcher is pinned in <see cref="VoiceMatchTests"/>; these
/// pin the readers over a real <see cref="SyntheticPlan"/> file and the date rule itself.
/// </summary>
public class VoiceAttributionV2Tests
{
    private const string Sentence = "the lioness waits beside the ruined gate until the ninth bell has rung over the valley";
    private const string ClaudeOnly = "a structural observation only the assistant ever wrote in any transcript anywhere at all";
    private static readonly DateOnly Aug5 = new(2026, 8, 5);

    private static VoiceSource Model(DateOnly date) => new("block:9", "conv-claude", "model", date);

    // ---------- the LastModified check ----------

    [Fact]
    public void A_model_source_dated_after_the_notes_last_edit_is_an_echo()
    {
        Assert.Equal(Attribution.AfterEdit, Attribution.DateCheckOf(Model(Aug5.AddDays(1)), Aug5, null));
    }

    [Fact]
    public void The_same_day_is_never_decided()
    {
        Assert.Equal("", Attribution.DateCheckOf(Model(Aug5), Aug5, null));
        Assert.Equal("", Attribution.DateCheckOf(Model(Aug5), Aug5, new ConversationReader.Span(Aug5, Aug5)));
    }

    [Fact]
    public void An_edit_inside_a_multi_day_conversation_is_flagged_not_decided()
    {
        var span = new ConversationReader.Span(Aug5.AddDays(-3), Aug5.AddDays(4));
        Assert.Equal(Attribution.Ambiguous, Attribution.DateCheckOf(Model(span.From), Aug5, span));
    }

    [Fact]
    public void An_edit_after_the_conversation_ended_leaves_it_a_possible_source()
    {
        var span = new ConversationReader.Span(Aug5.AddDays(-10), Aug5.AddDays(-2));
        Assert.Equal("", Attribution.DateCheckOf(Model(span.From), Aug5, span));
    }

    [Fact]
    public void A_conversation_with_no_recorded_end_cannot_be_ruled_out()
    {
        var span = new ConversationReader.Span(Aug5.AddDays(-10), null);
        Assert.Equal(Attribution.SpanUnknown, Attribution.DateCheckOf(Model(span.From), Aug5, span));
    }

    [Fact]
    public void A_brian_source_is_never_checked()
    {
        var prompt = new VoiceSource("block:8", "conv-claude", "brian", Aug5.AddDays(3));
        Assert.Equal("", Attribution.DateCheckOf(prompt, Aug5, null));
    }

    // ---------- conversations as a voice source ----------

    private static void SeedConversation(SyntheticPlan plan, DateTime? updated)
    {
        plan.ExternalWrite(ctx =>
        {
            ctx.Conversations.Add(new Conversation
            {
                Id = 1, Title = "chat", Platform = "Claude", BlockCount = 3,
                ConversationDate = new DateTime(2026, 8, 1, 10, 0, 0), SourceUpdatedAt = updated,
                ArcSummary = "frozen arc summary text that must never count as plan text or a voice",
            });
            ctx.ConversationBlocks.AddRange(
                new ConversationBlock { Id = 10, ConversationId = 1, BlockNumber = 1, Speaker = "user", RawContent = "Here is my idea: " + Sentence },
                new ConversationBlock { Id = 11, ConversationId = 1, BlockNumber = 2, Speaker = "assistant", RawContent = "You said " + Sentence + ". " + ClaudeOnly },
                new ConversationBlock { Id = 12, ConversationId = 1, BlockNumber = 3, Speaker = "user", RawContent = "ok", Summary = "my navigation note" });
        });
    }

    [Fact]
    public void Blocks_index_by_speaker_and_a_prompt_beats_the_reply_that_repeats_it()
    {
        using var plan = SyntheticPlan.Create();
        SeedConversation(plan, new DateTime(2026, 8, 9));
        var index = new VoiceIndex(6);

        var spans = ConversationReader.Load(plan.Path, index, _ => { });

        var mine = index.Match(Sentence);
        Assert.Equal("block:10", mine.Origin!.Id);
        Assert.Equal("brian", mine.Origin.Role);
        var theirs = index.Match(ClaudeOnly);
        Assert.Equal("block:11", theirs.Origin!.Id);
        Assert.Equal("model", theirs.Origin.Role);
        Assert.Equal("conv-claude", theirs.Origin.Layer);
        Assert.Equal(new ConversationReader.Span(new DateOnly(2026, 8, 1), new DateOnly(2026, 8, 9)), spans["block:11"]);
    }

    [Fact]
    public void A_conversation_with_no_updated_at_has_an_open_span()
    {
        using var plan = SyntheticPlan.Create();
        SeedConversation(plan, null);
        var spans = ConversationReader.Load(plan.Path, new VoiceIndex(6), _ => { });
        Assert.Null(spans["block:11"].To);
    }

    [Fact]
    public void A_model_blocks_prompt_is_the_user_block_before_it()
    {
        using var plan = SyntheticPlan.Create();
        SeedConversation(plan, null);
        var lineage = Path.Combine(Path.GetDirectoryName(plan.Path)!, "empty-lineage.db");
        using (var c = new SqliteConnection($"Data Source={lineage}")) { c.Open(); }
        using var ctx = new SourceContext(lineage, plan.Path);

        var (text, prompt) = ctx.Fetch("block:11", "model");

        Assert.Contains(ClaudeOnly, text);
        Assert.StartsWith("Here is my idea", prompt);
    }

    // ---------- the plan's backups as sources ----------

    [Fact]
    public void A_v2_backup_is_a_brian_source_dated_by_its_filename_and_its_transcripts_are_not_plan_text()
    {
        using var plan = SyntheticPlan.Create();
        SeedConversation(plan, null);
        var backups = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(plan.Path)!, "Backups")).FullName;
        var bak = Path.Combine(backups, "TLTT v2.2026-07-31_16-10-22.bak");
        using (var c = new SqliteConnection($"Data Source={plan.Path}"))
        {
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"VACUUM INTO '{bak}'";
            cmd.ExecuteNonQuery();
        }
        SqliteConnection.ClearAllPools();
        var index = new VoiceIndex(6);

        var loaded = SnapshotReader.LoadAsSources(Path.Combine(backups, "TLTT v2.2*.bak"), "v2-plan", index, _ => { });

        Assert.Equal(new DateOnly(2026, 7, 31), Assert.Single(loaded).Date);
        var note = index.Match("A perfectly ordinary retrievable note containing " + SyntheticPlan.VisibleSecret);
        Assert.Equal("v2-plan:TLTT v2.2026-07-31_16-10-22.bak", note.Origin!.Id);
        Assert.Equal("brian", note.Origin.Role);
        Assert.Null(index.Match(ClaudeOnly).Origin);
        Assert.Null(index.Match("frozen arc summary text that must never count as plan text").Origin);
    }

    [Fact]
    public void Earlier_plan_text_beats_a_later_model_quote_span_by_span()
    {
        // A v1 sentence carried into an edited v2 note: the note as a whole is in no snapshot,
        // but the carried sentence keeps the snapshot's date against a later Claude quote.
        var index = new VoiceIndex(6);
        index.Add(new VoiceSource("block:9", "conv-claude", "model", new DateOnly(2026, 5, 1)), "As your notes say, " + Sentence + ". " + ClaudeOnly);
        index.Add(new VoiceSource("v1-plan:TheLionessOfTallTale 2026-01-10.db", "v1-plan", "brian", new DateOnly(2026, 1, 10)), new[] { "unrelated cell", Sentence });

        var m = index.Match("New framing written in v2 only. " + Sentence + ". " + ClaudeOnly);

        var carried = m.Spans.Single(s => s.Source.Layer == "v1-plan");
        Assert.Equal("brian", carried.Source.Role);
        Assert.Contains(m.Spans, s => s.Source.Id == "block:9");
    }

    // ---------- state names per file ----------

    [Fact]
    public void State_names_follow_the_file_confirmed_in_v2_closed_in_the_archive()
    {
        using var plan = SyntheticPlan.Create();
        var v2 = new PlanReader(plan.Path);
        Assert.False(v2.IsArchive);
        Assert.Equal("confirmed", v2.StateLabel(2));
        Assert.Equal("unset", v2.StateLabel(0));
        Assert.Equal("flagged", v2.StateLabel(1));

        var archive = Path.Combine(Path.GetDirectoryName(plan.Path)!, "TLTT v1 Archive.storyplan");
        using (var c = new SqliteConnection($"Data Source={plan.Path}"))
        {
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"VACUUM INTO '{archive}'";
            cmd.ExecuteNonQuery();
        }
        SqliteConnection.ClearAllPools();
        var v1 = new PlanReader(archive);
        Assert.True(v1.IsArchive);
        Assert.Equal("closed", v1.StateLabel(2));
    }

    [Fact]
    public void Notes_carry_their_last_modified_stamp()
    {
        using var plan = SyntheticPlan.Create();
        Assert.All(new PlanReader(plan.Path).Notes, n => Assert.NotNull(n.LastModified));
    }
}
