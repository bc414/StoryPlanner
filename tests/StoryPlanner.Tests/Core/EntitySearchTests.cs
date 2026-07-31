using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure tier — EntitySearch.Run takes plain in-memory collections, no .storyplan needed.
/// </summary>
public class EntitySearchTests
{
    private static SearchInput Input(
        Subject[]? subjects = null,
        PlotPoint[]? plotPoints = null,
        Chapter[]? chapters = null,
        Theme[]? themes = null,
        SourceMaterial[]? sourceMaterials = null,
        Note[]? notes = null)
        => new(
            subjects ?? [],
            plotPoints ?? [],
            chapters ?? [],
            themes ?? [],
            sourceMaterials ?? [],
            notes ?? []);

    [Fact]
    public void Matches_subject_name_description_and_abbreviation()
    {
        var byName = Input(subjects: [new Subject { Id = 1, Name = "Chrysalis", Description = "", Abbreviation = "" }]);
        var byDescription = Input(subjects: [new Subject { Id = 2, Name = "Someone", Description = "leads the changeling hive", Abbreviation = "" }]);
        var byAbbreviation = Input(subjects: [new Subject { Id = 3, Name = "Someone Else", Description = "", Abbreviation = "CHG" }]);

        Assert.Single(EntitySearch.Run(byName, "chrysalis"));
        Assert.Single(EntitySearch.Run(byDescription, "hive"));
        Assert.Single(EntitySearch.Run(byAbbreviation, "chg"));
    }

    [Fact]
    public void Subject_name_wins_over_description_when_both_match()
    {
        var input = Input(subjects: [new Subject { Id = 1, Name = "Hive Queen", Description = "Hive Queen's true form", Abbreviation = "" }]);

        var hits = EntitySearch.Run(input, "hive queen");

        var hit = Assert.Single(hits);
        Assert.Equal("Name", hit.MatchedField);
    }

    [Fact]
    public void Matches_plot_point_title()
    {
        var input = Input(plotPoints: [new PlotPoint { Id = 1, Title = "The hive gambit" }]);

        var hit = Assert.Single(EntitySearch.Run(input, "gambit"));
        Assert.Equal(SearchHitKind.PlotPoint, hit.Kind);
        Assert.Equal("Title", hit.MatchedField);
    }

    [Fact]
    public void Matches_chapter_title()
    {
        var input = Input(chapters: [new Chapter { Id = 1, Title = "Hive", OrderIndex = 12 }]);

        var hit = Assert.Single(EntitySearch.Run(input, "hive"));
        Assert.Equal(SearchHitKind.Chapter, hit.Kind);
    }

    [Fact]
    public void Matches_theme_name_and_proposition()
    {
        var byName = Input(themes: [new Theme { Id = 1, Name = "Strong to be Merciful", Proposition = "" }]);
        var byProposition = Input(themes: [new Theme { Id = 2, Name = "Unrelated", Proposition = "Strength is the prerequisite for mercy." }]);

        Assert.Single(EntitySearch.Run(byName, "merciful"));
        Assert.Single(EntitySearch.Run(byProposition, "prerequisite"));
    }

    [Fact]
    public void Matches_source_material_name_and_description()
    {
        var byName = Input(sourceMaterials: [new SourceMaterial { Id = 1, Name = "Equestria at War", Description = "" }]);
        var byDescription = Input(sourceMaterials: [new SourceMaterial { Id = 2, Name = "Unrelated", Description = "the HoI4 mod this borrows from" }]);

        Assert.Single(EntitySearch.Run(byName, "equestria"));
        Assert.Single(EntitySearch.Run(byDescription, "hoi4"));
    }

    [Fact]
    public void Matches_note_content()
    {
        // Nothing else in the app searches note content — this is the capability B2 adds.
        var input = Input(notes: [new Note { Id = 1, Content = "...changeling physiology depends on love as fuel...", NoteState = NoteState.Unset }]);

        var hit = Assert.Single(EntitySearch.Run(input, "physiology"));
        Assert.Equal(SearchHitKind.Note, hit.Kind);
        Assert.Equal("Content", hit.MatchedField);
        Assert.Contains("physiology", hit.Snippet);
    }

    // ── Flagged notes are searched in full — a deliberate divergence, pinned here so it
    //    reads as intent rather than a regression against NoteExportRenderer / FlaggedWallTests.

    [Fact]
    public void Flagged_notes_are_returned_with_their_content()
    {
        var input = Input(notes: [new Note
        {
            Id = 1,
            Content = "the hive queen's succession is unresolved",
            NoteState = NoteState.Flagged,
            FlagReason = "need to decide before chapter 30"
        }]);

        var hit = Assert.Single(EntitySearch.Run(input, "succession"));
        Assert.Equal("Content", hit.MatchedField);
        Assert.Contains("succession", hit.Snippet);
    }

    [Fact]
    public void Flag_reason_is_searched()
    {
        var input = Input(notes: [new Note
        {
            Id = 1,
            Content = "unrelated content",
            NoteState = NoteState.Flagged,
            FlagReason = "conflicts with the Skyfall timeline established in ch12"
        }]);

        var hit = Assert.Single(EntitySearch.Run(input, "skyfall"));
        Assert.Equal("FlagReason", hit.MatchedField);
    }

    [Fact]
    public void Matching_is_case_insensitive()
    {
        var input = Input(subjects: [new Subject { Id = 1, Name = "Chrysalis" }]);

        Assert.Single(EntitySearch.Run(input, "CHRYSALIS"));
        Assert.Single(EntitySearch.Run(input, "ChRySaLiS"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("a")]
    public void Blank_or_single_character_queries_return_nothing(string query)
    {
        var input = Input(subjects: [new Subject { Id = 1, Name = "Applejack" }]);

        Assert.Empty(EntitySearch.Run(input, query));
    }

    [Fact]
    public void Kind_filter_restricts_to_one_kind()
    {
        var input = Input(
            subjects: [new Subject { Id = 1, Name = "Hive Queen" }],
            plotPoints: [new PlotPoint { Id = 1, Title = "Hive Queen's gambit" }]);

        var hits = EntitySearch.Run(input, "hive", SearchHitKind.PlotPoint);

        var hit = Assert.Single(hits);
        Assert.Equal(SearchHitKind.PlotPoint, hit.Kind);
    }

    [Fact]
    public void Results_are_grouped_by_kind_in_a_fixed_order()
    {
        var input = Input(
            notes: [new Note { Id = 1, Content = "matches the query" }],
            subjects: [new Subject { Id = 1, Name = "matches the query" }],
            plotPoints: [new PlotPoint { Id = 1, Title = "matches the query" }],
            chapters: [new Chapter { Id = 1, Title = "matches the query" }],
            themes: [new Theme { Id = 1, Name = "matches the query" }],
            sourceMaterials: [new SourceMaterial { Id = 1, Name = "matches the query" }]);

        var hits = EntitySearch.Run(input, "matches the query");

        Assert.Equal(
            [SearchHitKind.Subject, SearchHitKind.PlotPoint, SearchHitKind.Chapter,
             SearchHitKind.Theme, SearchHitKind.SourceMaterial, SearchHitKind.Note],
            hits.Select(h => h.Kind));
    }

    [Fact]
    public void Subjects_are_ordered_alphabetically_by_name_within_the_kind()
    {
        var input = Input(subjects: [
            new Subject { Id = 1, Name = "Zebra Enclave" },
            new Subject { Id = 2, Name = "Applejack Enclave" },
        ]);

        var hits = EntitySearch.Run(input, "enclave");

        Assert.Equal([2, 1], hits.Select(h => h.Id));
    }

    [Fact]
    public void Chapters_are_ordered_by_order_index_within_the_kind()
    {
        var input = Input(chapters: [
            new Chapter { Id = 1, Title = "Reveal - Second", OrderIndex = 5 },
            new Chapter { Id = 2, Title = "Reveal - First", OrderIndex = 1 },
        ]);

        var hits = EntitySearch.Run(input, "reveal");

        Assert.Equal([2, 1], hits.Select(h => h.Id));
    }
}
