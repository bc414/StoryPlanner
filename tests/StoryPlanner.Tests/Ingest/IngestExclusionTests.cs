using StoryPlanner.CodeSessions;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure-tier tests for the authored ingest exclusion: the first HUMAN user message decides,
/// tool-result and image records (array content) are not human messages, and the predicate is
/// the config's regex list in order. Cascade to subagents is Program.cs wiring (as with
/// IngestPlan) and is not covered here.
/// </summary>
public class IngestExclusionTests
{
    private static string User(string content, string uuid = "u1") =>
        $$"""{"type":"user","uuid":"{{uuid}}","parentUuid":null,"timestamp":"2026-08-27T10:00:00Z","sessionId":"s1","message":{"role":"user","content":{{content}} } }""";

    private static string Other(string type) =>
        $$"""{"type":"{{type}}","uuid":"x","timestamp":"2026-08-27T09:59:00Z","sessionId":"s1"}""";

    private static readonly IReadOnlyList<System.Text.RegularExpressions.Regex> Rules =
        IngestExclusion.CompileRules(["^/analyze-story "]);

    [Fact]
    public void First_string_content_user_record_is_the_body()
    {
        var body = IngestExclusion.FirstUserBody([User("\"/analyze-story romance-reports\""), User("\"second\"", "u2")]);
        Assert.Equal("/analyze-story romance-reports", body);
    }

    [Fact]
    public void Slash_command_markup_is_unwrapped_to_the_typed_command()
    {
        // The real shape of a `claude -p "/analyze-story romance-reports"` first turn (2026-08-27 batch).
        var wrapped = User("\"<command-message>analyze-story</command-message>\\n<command-name>/analyze-story</command-name>\\n<command-args>romance-reports</command-args>\"");
        var body = IngestExclusion.FirstUserBody([wrapped]);
        Assert.Equal("/analyze-story romance-reports", body);
        Assert.Equal("^/analyze-story ", IngestExclusion.MatchingRule(body, Rules));
    }

    [Fact]
    public void Slash_command_without_args_unwraps_to_the_bare_command()
    {
        Assert.Equal("/mcp", IngestExclusion.UnwrapSlashCommand("<command-message>mcp</command-message>\n<command-name>/mcp</command-name>"));
        Assert.Equal("plain text stays", IngestExclusion.UnwrapSlashCommand("plain text stays"));
    }

    [Fact]
    public void Non_dialogue_records_before_it_are_ignored()
    {
        var body = IngestExclusion.FirstUserBody([Other("queue-operation"), Other("ai-title"), User("\"/analyze-story x\"")]);
        Assert.Equal("/analyze-story x", body);
    }

    [Fact]
    public void Array_content_user_records_are_tool_results_not_human_messages()
    {
        var toolResult = User("""[{"type":"tool_result","tool_use_id":"t1","content":"/analyze-story would be wrong here"}]""");
        var body = IngestExclusion.FirstUserBody([toolResult, User("\"hello\"", "u2")]);
        Assert.Equal("hello", body);
    }

    [Fact]
    public void No_human_message_means_null_and_no_match()
    {
        var body = IngestExclusion.FirstUserBody([Other("queue-operation")]);
        Assert.Null(body);
        Assert.Null(IngestExclusion.MatchingRule(body, Rules));
    }

    [Fact]
    public void Malformed_lines_are_skipped()
    {
        var body = IngestExclusion.FirstUserBody(["{not json", User("\"/analyze-story y\"")]);
        Assert.Equal("/analyze-story y", body);
    }

    [Fact]
    public void Matching_rule_is_the_pattern_text_and_anchoring_is_respected()
    {
        Assert.Equal("^/analyze-story ", IngestExclusion.MatchingRule("/analyze-story romance-reports", Rules));
        Assert.Null(IngestExclusion.MatchingRule("please run /analyze-story romance-reports", Rules));
        Assert.Null(IngestExclusion.MatchingRule("/analyze-story", Rules));
    }

    [Fact]
    public void Rules_are_tried_in_order_and_the_first_hit_wins()
    {
        var rules = IngestExclusion.CompileRules(["^/other", "^/analyze-story ", "analyze"]);
        Assert.Equal("^/analyze-story ", IngestExclusion.MatchingRule("/analyze-story z", rules));
    }
}
