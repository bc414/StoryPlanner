using System.Linq;
using StoryPlanner.V1Itemizer;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The own-voice cut of the v1 itemizer. The attribution run itself is VoiceAttribution's and
/// tested there; these tests hold the two things this cut adds — which attributed notes count as
/// the author's own voice, read from the v1-archive-mining skill's label table, and how the kept
/// notes are grouped into loci — over projected notes, with no lineage database.
/// </summary>
public class OwnVoiceTests
{
    static OwnVoice.Note N(int id, string owner, string label, string role = "", string state = "open",
        int pp = 0, int link = 0, int subject = 0, int chapterOrder = 13, int order = 1, string ownerName = "", string content = "text")
        => new(id, content, owner, ownerName, "The Lioness of Tall Tale", chapterOrder == 0 ? "" : $"CH#{chapterOrder} Passion",
            chapterOrder, pp, order, link, subject, state, label, role);

    [Theory]
    [InlineData("none", "", true)]
    [InlineData("short", "", true)]
    [InlineData("phrase", "model", true)]
    [InlineData("fragment", "model", true)]
    [InlineData("verbatim", "brian", true)]
    [InlineData("framed-paste", "brian", true)]
    [InlineData("verbatim", "model", false)]
    [InlineData("edited-paste", "model", false)]
    [InlineData("framed-paste", "model", false)]
    public void The_label_table_decides_what_is_the_authors_own_voice(string label, string role, bool kept)
    {
        Assert.Equal(kept, OwnVoice.IsOwnVoice(N(1, "PlotPoint", label, role, pp: 5)));
    }

    [Fact]
    public void A_flagged_note_is_never_carried_whatever_its_label()
    {
        Assert.False(OwnVoice.IsOwnVoice(N(1, "PlotPoint", "none", state: "flagged", pp: 5)));
        Assert.False(OwnVoice.IsOwnVoice(N(2, "Subject", "short", role: "brian", state: "flagged", subject: 3)));
    }

    [Fact]
    public void A_plot_point_carries_its_own_notes_before_its_links_and_names_each_link()
    {
        var notes = new[]
        {
            N(20, "Link", "none", pp: 5, link: 9, ownerName: "The Duel × Fleur", content: "From her side."),
            N(10, "PlotPoint", "none", pp: 5, ownerName: "The Duel", content: "They fight at dawn."),
            N(30, "Link", "verbatim", role: "model", pp: 5, link: 9, ownerName: "The Duel × Fleur", content: "A pasted analysis."),
        };
        var item = Assert.Single(OwnVoice.Cut(notes));
        Assert.Equal("pp-5", item.Id);
        Assert.Equal("pp-5", item.Locator);
        Assert.StartsWith("# v1 plot point: The Duel\n", item.Body);
        Assert.Contains("notes here in the author's own voice: 2", item.Body);
        Assert.True(item.Body.IndexOf("They fight at dawn.") < item.Body.IndexOf("From her side."));
        Assert.Contains("### note 20 — The Duel × Fleur", item.Body);
        Assert.DoesNotContain("A pasted analysis.", item.Body);
    }

    [Fact]
    public void A_note_that_lifts_one_sentence_is_marked_as_carrying_a_borrowed_phrasing()
    {
        var item = Assert.Single(OwnVoice.Cut(new[] { N(10, "PlotPoint", "fragment", role: "model", pp: 5, ownerName: "The Duel") }));
        Assert.Contains("### note 10 (carries a borrowed phrasing)", item.Body);
    }

    [Fact]
    public void Plot_points_run_in_chapter_then_position_order_then_subjects_then_chapters()
    {
        var notes = new[]
        {
            N(1, "Subject", "none", subject: 40, chapterOrder: 0, ownerName: "Minette"),
            N(2, "PlotPoint", "none", pp: 7, chapterOrder: 20, order: 1, ownerName: "Later"),
            N(3, "PlotPoint", "none", pp: 8, chapterOrder: 3, order: 2, ownerName: "Early second"),
            N(4, "PlotPoint", "none", pp: 9, chapterOrder: 3, order: 1, ownerName: "Early first"),
            N(5, "Chapter", "none", chapterOrder: 3, ownerName: "Laughter"),
        };
        Assert.Equal(new[] { "pp-9", "pp-8", "pp-7", "subject-40", "chapter-3-laughter" }, OwnVoice.Cut(notes).Select(i => i.Id));
    }

    [Fact]
    public void A_locus_with_no_own_voice_note_has_no_item()
    {
        var notes = new[]
        {
            N(1, "PlotPoint", "verbatim", role: "model", pp: 5, ownerName: "All pasted"),
            N(2, "Subject", "none", state: "flagged", subject: 4, chapterOrder: 0, ownerName: "Only flagged"),
            N(3, "PlotPoint", "none", pp: 6, ownerName: "Kept"),
        };
        Assert.Equal(new[] { "pp-6" }, OwnVoice.Cut(notes).Select(i => i.Id));
    }
}
