using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using StoryPlanner.BatchFiles;
using StoryPlanner.V1Itemizer;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The v1 itemizer. The native cuts are fixture-tested against a temp database in the native v1
/// schema: that schema is frozen with the v1 planner, no production writer for it exists any more,
/// so the fixture carries the columns of the 2026-04-18 snapshot that the reader selects, and a
/// renamed column fails here rather than in a batch. The archive's subject cut runs over the
/// synthetic .storyplan, whose schema the archive shares.
/// </summary>
public class V1ItemizerTests : IDisposable
{
    readonly string _dir = Directory.CreateTempSubdirectory("v1-itemizer-").FullName;
    readonly string _snapshot;

    public V1ItemizerTests()
    {
        _snapshot = Path.Combine(_dir, "TheLionessOfTallTale 2026-04-18.db");
        using var conn = new SqliteConnection($"Data Source={_snapshot};Pooling=False");
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            create table Chapters (Id integer primary key, Title text not null, Summary text not null, OrderIndex integer not null, Description text not null default '');
            create table PlotPoints (Id integer primary key, Title text not null, Synopsis text not null, VerbatimText text null, Status integer not null,
                TensionPhase integer not null, WorldDate text null, ChapterId integer null, OrderInChapter integer not null, LocationId integer null,
                Outcome text not null default '', Stakes text not null default '');
            create table Characters (Id integer primary key, Name text not null, Inspiration text not null, Archetype text not null default '', Description text not null default '');
            create table Themes (Id integer primary key, Name text not null, ColorHex text not null default '', Description text not null default '', Abbreviation text not null default '');
            create table Threads (Id integer primary key, Name text not null, Description text not null, Icon text not null, ThreadScope integer not null default 0);
            create table CodexEntries (Id integer primary key, Title text not null, Category integer not null, Description text not null default '', Type text not null default '');
            create table PlotPointCharacters (PlotPointId integer not null, CharacterId integer not null, DevelopmentImpact integer not null, DevelopmentNote text null, Role integer not null, LogicalOrder integer not null default 0);
            create table PlotPointThemes (PlotPointId integer not null, ThemeId integer not null, Commentary text null, Prominence integer not null);
            create table PlotPointThreads (PlotPointId integer not null, ThreadId integer not null, ImpactDescription text not null, IsPrimary integer not null, StoryThreadId integer not null, ThreadTrajectory integer not null, SortOrder integer not null default 0);
            create table PlotPointCodexEntries (PlotPointId integer not null, CodexEntryId integer not null, UsageType integer not null, Commentary text not null, LogicalOrder integer not null default 0);

            insert into Chapters values (1, 'Passion', '', 13, ''), (2, 'Blog Posts', '', 33, '');
            insert into PlotPoints (Id, Title, Synopsis, Status, TensionPhase, ChapterId, OrderInChapter, Outcome, Stakes) values
                (10, 'Second scene', 'Second synopsis.', 0, 0, 1, 2, '', ''),
                (11, 'First scene', 'First synopsis.' || char(13) || char(10) || 'Its second line.', 0, 0, 1, 1, 'She wins.', ''),
                (12, 'A blog post', 'Paratext.', 0, 0, 2, 1, '', ''),
                (13, 'Loose idea', 'Unplaced.', 0, 0, null, 0, '', '');
            insert into Characters (Id, Name, Inspiration) values (1, 'Fleur', ''), (2, 'Applejack', '');
            insert into Themes (Id, Name, Description) values (1, 'Honesty vs Poseurs', 'Poseurs perform virtue.'), (2, 'Silent Theme', '');
            insert into Threads (Id, Name, Description, Icon) values (5, 'The Romance', '', '');
            insert into CodexEntries (Id, Title, Category) values (7, 'Star Spade', 3);
            insert into PlotPointCharacters values (11, 1, 0, 'Written from her side.', 3, 1), (11, 2, 0, null, 0, 0);
            insert into PlotPointThemes values (11, 1, 'The scene tests the poseur.', 3), (11, 2, '   ', 0), (10, 2, null, 0);
            insert into PlotPointThreads values (11, 5, '', 1, 5, 2, 0);
            insert into PlotPointCodexEntries values (11, 7, 2, 'Explained here.', 0);
            """;
        cmd.ExecuteNonQuery();
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void A_plot_point_carries_its_fields_verbatim_and_every_link_under_its_kind_even_without_text()
    {
        var plan = NativeV1.Load(_snapshot);
        var items = NativeCuts.PlotPoints(plan, new HashSet<int>(), includeUnplaced: true);

        Assert.Equal(["pp-11", "pp-10", "pp-12", "pp-13"], items.Select(i => i.Id).ToArray());
        var first = items[0];
        Assert.Equal("pp-11", first.Locator);
        Assert.Equal("chapter 13 Passion, position 1: First scene", first.Description);
        Assert.Contains("## Synopsis\n\nFirst synopsis.\nIts second line.\n", first.Body);
        Assert.Contains("## Outcome\n\nShe wins.", first.Body);
        Assert.DoesNotContain("## Stakes", first.Body); // blank, so not a section
        // Characters in their logical order, the set enum value by name, a link with no text still listed.
        var applejack = first.Body.IndexOf("### Character: Applejack\n\n(no text)", StringComparison.Ordinal);
        var fleur = first.Body.IndexOf("### Character: Fleur (role: PointOfView)\n\nWritten from her side.", StringComparison.Ordinal);
        Assert.True(applejack >= 0 && fleur > applejack);
        Assert.Contains("### Theme: Honesty vs Poseurs (prominence: Demonstration)", first.Body);
        Assert.Contains("### Thread: The Romance (primary; trajectory: Positive)", first.Body);
        Assert.Contains("### Codex entry (Technology): Star Spade (usage: Definition)\n\nExplained here.", first.Body);
        Assert.Contains("## Links\n\n(none)", items.Single(i => i.Id == "pp-12").Body);
        Assert.Equal("unplaced: Loose idea", items[^1].Description);
    }

    [Fact]
    public void Excluded_chapters_and_unplaced_plot_points_leave_the_cut_and_the_narrowing_names_them()
    {
        var plan = NativeV1.Load(_snapshot);
        var exclude = new HashSet<int> { 2 };
        var items = NativeCuts.PlotPoints(plan, exclude, includeUnplaced: false);
        Assert.Equal(["pp-11", "pp-10"], items.Select(i => i.Id).ToArray());
        Assert.Equal("every plot point except those in the chapters 33 Blog Posts, the config's excludeChapters; and except those in no chapter",
            NativeCuts.PlotPointNarrowing(plan, exclude, includeUnplaced: false));
        Assert.Null(NativeCuts.PlotPointNarrowing(plan, new HashSet<int>(), includeUnplaced: true));
    }

    [Fact]
    public void Only_theme_links_with_commentary_are_cut_each_with_the_theme_and_the_synopsis_beside_it()
    {
        var items = NativeCuts.ThemeCommentaries(NativeV1.Load(_snapshot));
        var only = Assert.Single(items);
        Assert.Equal("pp-11-theme-1", only.Id);
        Assert.Equal("pp-11/theme-1", only.Locator);
        Assert.Contains("theme: Honesty vs Poseurs", only.Body);
        Assert.Contains("## Theme description\n\nPoseurs perform virtue.", only.Body);
        Assert.Contains("## Plot point synopsis\n\nFirst synopsis.", only.Body);
        Assert.Contains("## Commentary (prominence: Demonstration)\n\nThe scene tests the poseur.", only.Body);
    }

    [Fact]
    public void A_subject_item_carries_its_triage_label_and_its_own_notes_and_a_subject_with_none_is_left_out()
    {
        using var plan = SyntheticPlan.Create(archiveSemantics: true);
        var (subjects, notes) = ArchiveSubjects.Load(plan.Path);
        var items = ArchiveSubjects.Cut(subjects, notes);

        var only = Assert.Single(items);
        Assert.Equal($"subject-{SyntheticPlan.SubjectId}", only.Id);
        Assert.Contains("triage label: Character", only.Body);
        Assert.Contains($"### note {SyntheticPlan.VisibleNoteId}\n\n", only.Body);
        Assert.Contains($"### note {SyntheticPlan.UnparseableDateNoteId}\n\n", only.Body);
        // A link's note or a plot point's note is not a subject's note.
        Assert.DoesNotContain($"### note {SyntheticPlan.LinkNoteId}\n", only.Body);
        Assert.DoesNotContain($"### note {SyntheticPlan.PlotPointNoteId}\n", only.Body);
        Assert.Empty(ItemizerOutput.Problems(items));
    }
}
