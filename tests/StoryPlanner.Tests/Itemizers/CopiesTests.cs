using System.Linq;
using StoryPlanner.TurnItemizer;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The turn itemizer's lineage cuts beside the v1 archive. The attribution itself is
/// VoiceAttribution's; these tests hold what the cuts add over projected notes: how an attribution
/// source is keyed to a dialogue record, what counts as a copy, that the prompt cut keeps the
/// reader blind to the copy, and how a conversation is cut into parts.
/// </summary>
public class CopiesTests
{
    static Copies.Taken T(int id, string origin, string label, string role, bool planFirst = false, bool echo = false, string content = "note text")
        => new(id, content, "PlotPoint \"The Duel\"", label, role, planFirst, echo, origin);

    static Turns.Conversation Gemini(params (string Id, string Role, string Text)[] records) =>
        new("Gemini web (lineage)", "thread th_abc", "2026-02-20", records.Select(r => new Turns.Record(r.Id, r.Role, r.Text)).ToList());

    [Fact]
    public void A_gemini_source_is_keyed_to_its_prompt_or_its_response_by_the_sources_own_role()
    {
        Assert.Equal("gemini:12 response", Copies.RecordId("gemini:12", "model"));
        Assert.Equal("gemini:12 prompt", Copies.RecordId("gemini:12", "brian"));
        Assert.Equal("aistudio:62#t4", Copies.RecordId("aistudio:62#t4", "model"));
        Assert.Equal("nlm:2#t15", Copies.RecordId("nlm:2#t15", "brian"));
    }

    [Theory]
    [InlineData("verbatim", "model", false, true)]
    [InlineData("framed-paste", "model", false, true)]
    [InlineData("fragment", "model", false, true)]
    [InlineData("verbatim", "brian", true, false)]
    [InlineData("verbatim", "brian", false, false)]
    public void A_copy_is_a_model_paste_or_lift_and_never_text_the_plan_held_first(string label, string role, bool planFirst, bool copy)
    {
        Assert.Equal(copy, Copies.IsCopy(T(1, "gemini:12 response", label, role, planFirst)));
    }

    [Fact]
    public void The_prompt_cut_carries_the_user_turn_alone_and_puts_the_copy_in_the_index_row()
    {
        var conv = Gemini(("gemini:12 prompt", Turns.User, "Analyze Minette."), ("gemini:12 response", Turns.Model, "Minette is a strategist."),
                          ("gemini:13 prompt", Turns.User, "And Reni?"), ("gemini:13 response", Turns.Model, "Reni is loyal."));
        var taken = new[]
        {
            T(1, "gemini:12 response", "verbatim", "model"),
            T(2, "gemini:12 response", "fragment", "model"),
            T(3, "gemini:13 response", "verbatim", "brian", planFirst: true),
        }.ToLookup(t => t.OriginId);

        var items = Copies.PromptsByCopy(new[] { conv }, taken);

        Assert.Equal(new[] { "gemini-12-prompt", "gemini-13-prompt" }, items.Select(i => i.Id));
        Assert.Contains("Analyze Minette.", items[0].Body);
        Assert.DoesNotContain("strategist", items[0].Body);
        Assert.DoesNotContain("copied", items[0].Body);
        Assert.EndsWith("copied: 1 pasted, 1 lifted into archive notes", items[0].Description);
        Assert.EndsWith("not copied", items[1].Description);
    }

    [Fact]
    public void The_thread_cut_lists_each_note_under_the_record_it_came_from_with_its_relation()
    {
        var conv = Gemini(("gemini:12 prompt", Turns.User, "Analyze Minette."), ("gemini:12 response", Turns.Model, "Minette is a strategist."));
        var taken = new[]
        {
            T(1, "gemini:12 response", "framed-paste", "model", content: "Minette plans ahead."),
            T(2, "gemini:12 prompt", "verbatim", "brian", content: "Analyze Minette."),
        }.ToLookup(t => t.OriginId);

        var item = Assert.Single(Copies.ThreadsWithNotes(new[] { conv }, taken, 200_000));

        Assert.Equal("gemini-th-abc", item.Id);
        Assert.Equal("gemini:12 prompt … gemini:12 response", item.Locator);
        Assert.Contains("archive notes traced to this stretch: 2", item.Body);
        Assert.Contains("#### note 2, on PlotPoint \"The Duel\" — the author's own words in this record", item.Body);
        Assert.Contains("#### note 1, on PlotPoint \"The Duel\" — pasted from this reply inside the author's own framing", item.Body);
        Assert.True(item.Body.IndexOf("Minette is a strategist.") < item.Body.IndexOf("Minette plans ahead."));
    }

    [Fact]
    public void A_long_conversation_is_cut_into_parts_only_before_a_user_record()
    {
        var text = new string('x', 60);
        var conv = new Turns.Conversation("AI Studio (lineage)", "chat 62 \"Long\"", "2026-01-10",
            Enumerable.Range(0, 6).Select(i => new Turns.Record($"aistudio:62#t{i}", i % 2 == 0 ? Turns.User : Turns.Model, text)).ToList());

        var items = Copies.ThreadsWithNotes(new[] { conv }, Enumerable.Empty<Copies.Taken>().ToLookup(t => t.OriginId), 250);

        Assert.Equal(new[] { "aistudio-62-p1", "aistudio-62-p2", "aistudio-62-p3" }, items.Select(i => i.Id));
        Assert.All(items, i => Assert.Matches(@"^aistudio:62#t\d*[02468] … aistudio:62#t\d*[13579]$", i.Locator));
        Assert.Contains("part: 2 of 3", items[1].Body);
    }
}
