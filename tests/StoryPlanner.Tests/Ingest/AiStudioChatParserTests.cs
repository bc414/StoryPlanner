using StoryPlanner.Lineage;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure-tier tests for the AI Studio Drive-export parser — strings in, records out; no files.
/// The rules under test are the retired ConversationSplitter parser's, carried into the
/// lineage ingest with two deliberate changes: roles verbatim, drops reported by name.
/// </summary>
public class AiStudioChatParserTests
{
    private static string Chat(params string[] chunks) =>
        $$"""
        {
          "runSettings": { "model": "models/gemini-test" },
          "systemInstruction": { "text": "You are a senior developmental editor." },
          "chunkedPrompt": { "chunks": [ {{string.Join(",", chunks)}} ], "pendingInputs": [] }
        }
        """;

    private static string Text(string role, string text, string? createTime = "2026-02-20T10:00:00Z", bool thought = false) =>
        $$"""{ "role": "{{role}}", "text": "{{text}}", "tokenCount": 5{{(createTime is not null ? $", \"createTime\": \"{createTime}\"" : "")}}{{(thought ? ", \"isThought\": true" : "")}} }""";

    [Fact]
    public void Thinking_chunks_are_stripped_and_turn_indices_stay_contiguous()
    {
        var json = Chat(
            Text("user", "First question"),
            Text("model", "Considering the nuances of the ask", thought: true),
            Text("model", "First answer"),
            Text("user", "Second question"),
            Text("model", "Second answer"));

        var chat = AiStudioChatParser.Parse(json, "Test Chat", out var failure);

        Assert.Null(failure);
        Assert.NotNull(chat);
        Assert.Equal(4, chat.Turns.Count);
        Assert.Equal([1, 2, 3, 4], chat.Turns.Select(t => t.TurnIndex));
        Assert.DoesNotContain(chat.Turns, t => t.Body.Contains("Considering the nuances"));
    }

    [Fact]
    public void A_drive_document_chunk_becomes_a_placeholder_turn_not_content()
    {
        var json = Chat(
            """{ "role": "user", "driveDocument": { "id": "1GbJ_doc" }, "createTime": "2026-02-20T09:00:00Z" }""",
            Text("user", "Analyze the attached plan"),
            Text("model", "Analysis follows"),
            Text("user", "Thanks"));

        var chat = AiStudioChatParser.Parse(json, "Doc Chat", out _);

        Assert.NotNull(chat);
        var placeholder = chat.Turns[0];
        Assert.True(placeholder.IsPlaceholder);
        Assert.Equal("[Attached document: 1GbJ_doc]", placeholder.Body);
        // The document chunk still anchors the conversation date — it is the first chunk.
        Assert.Equal("2026-02-20T09:00:00Z", chat.Date);
    }

    [Fact]
    public void Title_is_underscores_to_spaces_titlecase_and_chatkey_is_the_filename_verbatim()
    {
        var json = Chat(Text("user", "a"), Text("model", "b"), Text("user", "c"));

        var chat = AiStudioChatParser.Parse(json, "Note_Organizer_Part_0", out _);

        Assert.NotNull(chat);
        Assert.Equal("Note Organizer Part 0", chat.Title);
        Assert.Equal("Note_Organizer_Part_0", chat.ChatKey);
    }

    [Fact]
    public void Roles_are_stored_verbatim_user_and_model()
    {
        var json = Chat(Text("user", "a"), Text("model", "b"), Text("user", "c"));

        var chat = AiStudioChatParser.Parse(json, "Roles", out _);

        Assert.NotNull(chat);
        // Schema truth: no "assistant" mapping — the export says "model".
        Assert.Equal(["user", "model", "user"], chat.Turns.Select(t => t.Role));
    }

    [Fact]
    public void A_chat_with_two_or_fewer_surviving_turns_is_dropped_and_named()
    {
        var json = Chat(Text("user", "lone question"), Text("model", "lone answer"));

        var chat = AiStudioChatParser.Parse(json, "Tiny Chat", out var failure);

        Assert.Null(chat);
        Assert.NotNull(failure);
        Assert.Equal("Tiny Chat", failure.FileName);
        Assert.StartsWith("dropped:", failure.Reason);
    }

    [Fact]
    public void A_non_json_candidate_fails_with_a_reason_never_silently()
    {
        var chat = AiStudioChatParser.Parse("This is a markdown file, not a chat.", "stray.notes", out var failure);

        Assert.Null(chat);
        Assert.NotNull(failure);
        Assert.Contains("not JSON", failure.Reason);
    }

    [Fact]
    public void A_json_file_without_chunkedPrompt_is_not_an_ai_studio_chat()
    {
        var chat = AiStudioChatParser.Parse("""{ "some": "other json" }""", "config.data", out var failure);

        Assert.Null(chat);
        Assert.Contains("chunkedPrompt", failure!.Reason);
    }

    [Fact]
    public void Turns_missing_createTime_are_counted_for_the_dry_run()
    {
        var json = Chat(
            Text("user", "dated"),
            Text("model", "undated one", createTime: null),
            Text("user", "undated two", createTime: null));

        var chat = AiStudioChatParser.Parse(json, "Partial Dates", out _);

        Assert.NotNull(chat);
        Assert.Equal(2, chat.TurnsMissingCreateTime);
        Assert.Equal("2026-02-20T10:00:00Z", chat.Date); // from the first dated chunk
    }

    [Fact]
    public void Model_and_system_instruction_are_carried()
    {
        var json = Chat(Text("user", "a"), Text("model", "b"), Text("user", "c"));

        var chat = AiStudioChatParser.Parse(json, "Meta", out _);

        Assert.NotNull(chat);
        Assert.Equal("models/gemini-test", chat.Model);
        Assert.Equal("You are a senior developmental editor.", chat.SystemInstruction);
    }
}
