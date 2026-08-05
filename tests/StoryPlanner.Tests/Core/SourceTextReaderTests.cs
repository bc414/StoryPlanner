using StoryPlanner.SourceTexts;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure-tier tests for the source-text readers. No .storyplan, no files — every reader takes
/// text in and returns units, which is why they are shaped that way.
/// </summary>
public class SourceTextReaderTests
{
    // ── FicHtml: italics and section detection ──────────────────────────────────

    private const string ChapterWithSections = """
        <html><head><title>T</title></head><body>
            <h1>The Wind that Fanned the Flames, I</h1>
        <hr/><div class="bbcode-center" style="text-align:center"><p><span style="font-weight:bold;font-style:italic">The Queen's Scientist</span></p></div><hr/>
        <p>Doctor Silphia shuddered as a wave crashed against the railing.</p>
        <p><i>The Queen&#8217;s Island</i>, she corrected in her mind.</p>
        <p>She read the letter once more.</p>
        <p><span style="font-weight:bold;font-style:italic">Flurry Heart</span></p>
        <p>Albert left the letter on the chair.</p>
        <hr/><div class="bbcode-center" style="text-align:center"><p><span style="font-weight:bold;font-style:italic">Father</span></p></div><hr/>
        <p>Eudes scrubbed at his good eye with a wing.</p>
        </body></html>
        """;

    [Fact]
    public void Italics_survive_the_conversion()
    {
        // The whole reason this pipeline reads EPUB rather than the .txt export: the author sets
        // internal monologue in italics, and the plain-text download collapses it into narration.
        var md = FicHtml.ChapterMarkdown(ChapterWithSections);

        Assert.Contains("*The Queen’s Island*", md);
        Assert.DoesNotContain("<i>", md);
        Assert.DoesNotContain("&#8217;", md); // entities decoded, not passed through
    }

    [Fact]
    public void An_inline_bold_italic_line_is_not_a_section_head()
    {
        // "Flurry Heart" is the signature on a letter inside the Husband section of the real
        // chapter. A text-shaped heuristic (short standalone line, no terminal punctuation)
        // promotes it to a heading and invents a section; the <hr/>-wrapped marker does not.
        // If this fails, section splitting has drifted back to guessing from line shape.
        var sections = FicHtml.SplitSections(ChapterWithSections);

        Assert.Equal(2, sections.Count);
        Assert.Equal("The Queen's Scientist", sections[0].Title);
        Assert.Equal("Father", sections[1].Title);
        Assert.Contains("Flurry Heart", sections[0].Markdown); // kept as content, not promoted
    }

    [Fact]
    public void A_chapter_with_no_markers_reports_no_sections()
    {
        // Empty is the caller's signal to treat the chapter as one unit — not a prompt to go
        // looking for a softer delimiter.
        Assert.Empty(FicHtml.SplitSections("<html><body><h1>Part One</h1><p>Prose.</p></body></html>"));
    }

    [Fact]
    public void Chapter_title_comes_from_the_h1()
    {
        Assert.Equal("The Wind that Fanned the Flames, I", FicHtml.ChapterTitle(ChapterWithSections));
    }

    [Fact]
    public void Section_codes_are_stable_and_readable()
    {
        Assert.Equal("ch121-queens-scientist", FimfictionEpubReader.SectionCode("ch121", "The Queen's Scientist"));
        Assert.Equal("ch122-storm-kings-right-hoof", FimfictionEpubReader.SectionCode("ch122", "The Storm King's Right Hoof"));
    }

    // ── EPUB -> Part mapping: the refusal that guards against off-by-one ────────

    [Fact]
    public void Chapter_count_mismatch_refuses_to_map_rather_than_zipping()
    {
        // Zipping a short list onto a long one silently misdates every chapter after the gap,
        // and a citation pointing at the wrong chapter is worse than no text at all.
        var report = new IngestReport();
        var chapters = new List<FimfictionEpubReader.EpubChapter>
        {
            new("chapter-1.html", "Part One", "<html><body><p>a</p></body></html>"),
            new("chapter-2.html", "Part Two", "<html><body><p>b</p></body></html>")
        };
        var parts = new List<(string, string)> { ("ch1", "Departure"), ("ch2", "Boat"), ("ch3", "Frontier") };

        var units = FimfictionEpubReader.ToUnits("P&K", chapters, parts, new HashSet<string>(), "x.epub", report);

        Assert.Empty(units);
        Assert.True(report.HasErrors);
    }

    [Fact]
    public void Matching_counts_map_in_reading_order()
    {
        var report = new IngestReport();
        var chapters = new List<FimfictionEpubReader.EpubChapter>
        {
            new("chapter-1.html", "Part One", "<html><body><p>alpha</p></body></html>"),
            new("chapter-2.html", "Part Two", "<html><body><p>beta</p></body></html>")
        };
        var parts = new List<(string, string)> { ("ch1", "Departure"), ("ch2", "Boat") };

        var units = FimfictionEpubReader.ToUnits("P&K", chapters, parts, new HashSet<string>(), "x.epub", report);

        Assert.False(report.HasErrors);
        Assert.Equal(["ch1", "ch2"], units.Select(u => u.PartCode));
        Assert.Contains("alpha", units[0].Body);
        Assert.Contains("beta", units[1].Body);
    }

    [Fact]
    public void A_split_chapter_becomes_one_unit_per_section()
    {
        var report = new IngestReport();
        var chapters = new List<FimfictionEpubReader.EpubChapter>
        {
            new("chapter-1.html", "The Wind that Fanned the Flames, I", ChapterWithSections)
        };
        var parts = new List<(string, string)> { ("ch121", "The Wind that Fanned the Flames, I") };

        var units = FimfictionEpubReader.ToUnits(
            "P&K", chapters, parts, new HashSet<string> { "ch121" }, "x.epub", report);

        Assert.Equal(["ch121-queens-scientist", "ch121-father"], units.Select(u => u.PartCode));
        // The parent code carries no unit of its own — its text lives under the section codes.
        Assert.DoesNotContain("ch121", units.Select(u => u.PartCode));
    }

    // ── FiM transcripts ─────────────────────────────────────────────────────────

    private const string TranscriptJson = """
        {
          "27": { "season": 2, "number_in_season": 1, "title": "The Return of Harmony Part 1",
                  "transcript_url": "https://example/1",
                  "transcript": [ {"character":"Narrator","line":"Once upon a time."} ] },
          "28": { "season": 2, "number_in_season": 2, "title": "The Return of Harmony Part 2",
                  "transcript_url": "https://example/2",
                  "transcript": [ {"character":"Discord","line":"Chaos!"} ] },
          "29": { "season": 2, "number_in_season": 3, "title": "Lesson Zero",
                  "transcript": [ {"character":"Twilight","line":"A checklist."} ] }
        }
        """;

    [Fact]
    public void Two_parters_merge_only_where_the_config_says_so()
    {
        var report = new IngestReport();
        var units = FimTranscriptJsonReader.Read(
            "FiM", TranscriptJson, new Dictionary<string, string> { ["S2E02"] = "S2E01" }, "src", report);

        // Both halves land on the merged Part, and both stay addressable — the second keeps its
        // own code as a UnitKey rather than being concatenated into an anonymous blob.
        var merged = units.Where(u => u.PartCode == "S2E01").OrderBy(u => u.OrderIndex).ToList();
        Assert.Equal(2, merged.Count);
        Assert.Equal("", merged[0].UnitKey);
        Assert.Equal("S2E02", merged[1].UnitKey);
        Assert.Contains("Discord: Chaos!", merged[1].Body);

        // Everything not named in the map keeps its own code.
        Assert.Single(units, u => u.PartCode == "S2E03");
    }

    [Fact]
    public void Transcript_lines_render_as_speaker_and_line()
    {
        var units = FimTranscriptJsonReader.Read(
            "FiM", TranscriptJson, new Dictionary<string, string>(), "src", new IngestReport());

        Assert.Contains("Narrator: Once upon a time.", units.First(u => u.PartCode == "S2E01").Body);
    }

    // ── EaW localisation ────────────────────────────────────────────────────────

    private const string LocYml = """
        l_english:
        #FOCUS LOCALISATION
        EQS_Crystal_Fair:0 "Crystal Fair"
        EQS_Crystal_Fair_desc:0 "An ancient tradition of the §YCrystal Ponies§!."
        Crystal_Fair_desc:0 "[Root.GetCrystalFairFocusDesc]"
        invite_YAK_to_war_tt:0 "Allows calling the yaks."
        """;

    [Fact]
    public void Each_localisation_key_is_its_own_unit()
    {
        // One unit per key rather than one blob per country is what makes a citation precise:
        // "EQS_Crystal_Fair_desc" names a single focus description.
        var units = Hoi4LocalisationReader.Read("EaW", "EQS", LocYml, includeTooltips: false, "f.yml");

        Assert.Equal(["EQS_Crystal_Fair", "EQS_Crystal_Fair_desc", "Crystal_Fair_desc"],
            units.Select(u => u.UnitKey));
        Assert.All(units, u => Assert.Equal("EQS", u.PartCode));
    }

    [Fact]
    public void Colour_codes_are_stripped_but_scripting_variables_are_kept_verbatim()
    {
        var units = Hoi4LocalisationReader.Read("EaW", "EQS", LocYml, includeTooltips: false, "f.yml");

        Assert.Equal("An ancient tradition of the Crystal Ponies.",
            units.Single(u => u.UnitKey == "EQS_Crystal_Fair_desc").Body);
        // Unresolvable outside the game — substituting a guess would put words in the mod's mouth.
        Assert.Equal("[Root.GetCrystalFairFocusDesc]",
            units.Single(u => u.UnitKey == "Crystal_Fair_desc").Body);
    }

    [Fact]
    public void Tooltip_keys_are_excluded_unless_asked_for()
    {
        Assert.DoesNotContain(
            Hoi4LocalisationReader.Read("EaW", "EQS", LocYml, includeTooltips: false, "f.yml"),
            u => u.UnitKey.EndsWith("_tt"));
        Assert.Contains(
            Hoi4LocalisationReader.Read("EaW", "EQS", LocYml, includeTooltips: true, "f.yml"),
            u => u.UnitKey == "invite_YAK_to_war_tt");
    }

    [Fact]
    public void Country_tag_comes_from_the_filename()
    {
        Assert.Equal("GRI", Hoi4LocalisationReader.TagFromFileName("country_GRI_l_english.yml"));
        Assert.Equal("BAL", Hoi4LocalisationReader.TagFromFileName("country_BAL_protectorate_l_english.yml"));
        Assert.Null(Hoi4LocalisationReader.TagFromFileName("focus_generic_l_english.yml"));
    }
}
