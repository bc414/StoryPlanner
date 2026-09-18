using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using StoryPlanner.Core;
using StoryPlanner.GeminiCorpus;
using StoryPlanner.Lineage;
using StoryPlanner.TurnItemizer;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The turn itemizer. The exchange and decision cuts are pure; the readers are fixture-tested
/// against a temp lineage.db written by the production ingests' own Db classes and against the
/// synthetic plan's conversation tables, so the SQL is held to the schemas the ingests write.
/// </summary>
public class TurnItemizerTests : IDisposable
{
    readonly string _dir = Directory.CreateTempSubdirectory("turn-itemizer-").FullName;

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        Directory.Delete(_dir, recursive: true);
    }

    static Turns.Record U(string id, string text, bool placeholder = false) => new(id, Turns.User, text, placeholder);
    static Turns.Record M(string id, string text) => new(id, Turns.Model, text);

    [Fact]
    public void Consecutive_records_of_one_role_are_one_turn_and_each_user_turn_after_a_model_turn_is_an_item()
    {
        var c = new Turns.Conversation("AI Studio (lineage)", "chat 1 \"Test\"", "2026-02-20", [
            U("aistudio:1#t1", "Opening prompt."),
            M("aistudio:1#t2", "First answer."),
            U("aistudio:1#t3", "[Attached document: abc]", placeholder: true),
            U("aistudio:1#t4", "Read the attached plan again."),
            M("aistudio:1#t5", "Second answer."),
            U("aistudio:1#t6", "[Attached document: def]", placeholder: true),
        ]);
        Assert.Equal(5, Turns.Runs(c.Records).Count);

        var items = Turns.UserTurns([c]);
        // The opening prompt follows no model turn; the last user turn is attached documents only.
        var only = Assert.Single(items);
        Assert.Equal("aistudio-1-t3", only.Id);
        Assert.Equal("aistudio:1#t2 → aistudio:1#t3 + aistudio:1#t4", only.Locator);
        Assert.Contains("## The model turn before it\n\nFirst answer.", only.Body);
        Assert.Contains("[Attached document: abc] (an attached document, never captured)\n\nRead the attached plan again.", only.Body);
    }

    [Fact]
    public void A_model_turn_ends_with_a_question_when_its_last_paragraph_holds_one()
    {
        Assert.True(Turns.EndsWithQuestion("Analysis.\n\nWhich of these fits?\n"));
        Assert.True(Turns.EndsWithQuestion("Analysis.\n\n1. Does she know?\n2. Does he?"));
        Assert.False(Turns.EndsWithQuestion("Was it planned? Yes.\n\nThat settles it."));

        var c = new Turns.Conversation("conversations (Claude)", "conversation 1 \"T\"", "2026-07-01", [
            U("block:1", "Ask."),
            M("block:2", "Here is the answer.\n\nShall I go on?"),
            U("block:3", "Yes."),
            M("block:4", "Done. Anything else?"),
        ]);
        var items = Turns.QuestionEndings([c]);
        Assert.Equal(["block-2", "block-4"], items.Select(i => i.Id).ToArray());
        Assert.Contains("## The next user turn\n\nYes.", items[0].Body);
        Assert.Equal("block:4 → (none)", items[1].Locator);
        Assert.Contains("(none: the conversation ends here)", items[1].Body);
    }

    [Fact]
    public void A_decision_carries_the_assistant_turn_since_the_last_user_turn_with_tool_noise_dropped()
    {
        var s = new SessionDecisions.Session("s1", "Test session", "2026-09-17");
        var records = new List<SessionDecisions.Record>
        {
            new(1, "s1", "u1", "user", "Build the thing."),
            new(2, "s1", "u2", "assistant", "Reading the code first."),
            new(3, "s1", "u3", "assistant", "[tool_use: Read — Program.cs]"),
            new(4, "s1", "u4", "user", "[tool result elided — 400 chars]"),
            new(5, "s1", "u5", "assistant", "Two options; A is recommended."),
            new(6, "s1", "u6", "assistant", "[tool_use: AskUserQuestion]"),
            new(7, "s1", "u7", "user", "[AskUserQuestion — 1 answered]\n\nQ: Which?\nChose: B"),
            new(8, "s1", "u8", "assistant", "Going with B."),
            new(9, "s1", "u9", "user", "[Request interrupted by user]"),
            new(10, "s1", "u10", "user", "Actually, A after all."),
            new(11, "s1", "u11", "assistant", "Noted."),
            new(12, "s1", "u12", "user", "<command-name>/model</command-name>"),
            new(13, "s1", "u13", "user", "Carry on."),
        };

        var items = SessionDecisions.Cut([s], records, new HashSet<string> { SessionDecisions.Answers, SessionDecisions.Prompts });
        Assert.Equal(["cs-7", "cs-10"], items.Select(i => i.Id).ToArray());
        var answer = items[0];
        Assert.Equal("s1#u7", answer.Locator);
        Assert.Contains("Reading the code first.\n\nTwo options; A is recommended.\n\n[tool_use: AskUserQuestion]", answer.Body);
        Assert.DoesNotContain("tool_use: Read", answer.Body);
        Assert.DoesNotContain("tool result elided", answer.Body);
        // The interrupt leaves the lead-in standing for what is typed after it; the command clears it,
        // so "Carry on." follows no assistant text and is not an item.
        Assert.Contains("## The assistant's turn before it\n\nGoing with B.", items[1].Body);

        var answersOnly = SessionDecisions.Cut([s], records, new HashSet<string> { SessionDecisions.Answers });
        Assert.Equal(["cs-7"], answersOnly.Select(i => i.Id).ToArray());
    }

    [Fact]
    public void The_lineage_readers_take_the_dialogue_the_ingests_wrote_and_leave_activity_records_out()
    {
        var db = Path.Combine(_dir, "lineage.db");
        using (var conn = GeminiCorpusDb.OpenWrite(db))
        {
            GeminiCorpusDb.ReplaceEntries(conn, [
                Entry("e2", "T1", 2, "Second prompt?", "Second response.", "qa"),
                Entry("e1", "T1", 1, "First prompt.", "First response.", "qa"),
                Entry("e3", "T1", 3, "", "# Gemini Canvas titled x", "activity"),
            ]);
        }
        using (var conn = LineageDb.OpenWrite(db))
        {
            LineageDb.ReplaceAiStudio(conn, [
                new AiStudioChat("Chat A", "Chat A", "2026-02-20T10:00:00Z", "models/test", "", [
                    new AiStudioTurn(1, "user", null, false, "Hello."),
                    new AiStudioTurn(2, "model", null, false, "Hi."),
                ], 0)
            ]);
            LineageDb.ReplaceNotebookLm(conn, [
                new NlmNotebook("nb", "Notebook", null, "nb.htm", "2026-08-13T00:00:00Z",
                    [new NlmTurn(1, "user", "Q."), new NlmTurn(2, "model", "A.")], [])
            ]);
        }

        var gemini = Assert.Single(Sources.ReadGemini(db));
        Assert.Equal(Turns.User, gemini.Records[0].Role);
        Assert.Equal("First prompt.", gemini.Records[0].Text);
        Assert.Equal(4, gemini.Records.Count);
        Assert.EndsWith(" response", gemini.Records[3].Id);
        var geminiItem = Assert.Single(Turns.UserTurns([gemini]));
        Assert.Contains("Second prompt?", geminiItem.Body);

        var aiStudio = Assert.Single(Sources.ReadAiStudio(db));
        Assert.StartsWith("aistudio:", aiStudio.Records[0].Id);
        Assert.Equal([Turns.User, Turns.Model], aiStudio.Records.Select(r => r.Role).ToArray());
        var notebook = Assert.Single(Sources.ReadNotebookLm(db));
        Assert.Equal("undated", notebook.Date);
    }

    [Fact]
    public void The_conversations_reader_leaves_compaction_blocks_out()
    {
        using var fixture = SyntheticPlan.Create();
        fixture.ExternalWrite(ctx =>
        {
            ctx.Conversations.Add(new Conversation { Id = 1, Title = "Test talk", ConversationDate = new DateTime(2026, 7, 1), Platform = "Claude", BlockCount = 3 });
            ctx.ConversationBlocks.AddRange(
                new ConversationBlock { Id = 1, ConversationId = 1, BlockNumber = 1, Speaker = "assistant", RawContent = "An answer." },
                new ConversationBlock { Id = 2, ConversationId = 1, BlockNumber = 2, Speaker = "user", RawContent = "Summary of earlier context.", IsCompaction = true },
                new ConversationBlock { Id = 3, ConversationId = 1, BlockNumber = 3, Speaker = "user", RawContent = "A reply." });
        });
        var c = Assert.Single(Sources.ReadConversations(fixture.Path));
        Assert.Equal("conversations (Claude)", c.Layer);
        Assert.Equal(["block:1", "block:3"], c.Records.Select(r => r.Id).ToArray());
        Assert.Equal("block:1 → block:3", Assert.Single(Turns.UserTurns([c])).Locator);
    }

    static GeminiEntry Entry(string id, string thread, int pos, string prompt, string response, string type) => new(
        EntryId: id, ThreadId: thread, ThreadPos: pos, ThreadSize: 3, Date: "2025-11-02", LocalTime: "2025-11-02 20:15",
        Subject: "creative-writing", Subtopic: null, TopicLabel: "", ThreadSummary: "", Intent: "", Gem: null, Title: "",
        Prompt: prompt, Response: response, Type: type, IsPlanPaste: false, PromptChars: prompt.Length, ResponseChars: response.Length);
}
