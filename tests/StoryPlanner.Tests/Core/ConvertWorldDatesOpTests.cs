using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using StoryPlanner.DataOps;
using StoryPlanner.DataOps.Ops;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Runs DataOpEnvelope + ConvertWorldDates against a dedicated file that mirrors the live v2
/// shape at tractable scale: one History track whose notes carry every legacy WorldDate format
/// class found in the real data (point, range, negative range, '?'-suffixed, "N BLB", bare "?",
/// and the inverted range "954-914"). Also proves the envelope's note-content checksum passes:
/// the op touches WorldDate fields and track ids, never Content/FlagReason/NoteState.
/// </summary>
public class ConvertWorldDatesOpTests
{
    private const int DefId = 1;
    private const int SubjectId = 1;
    private const int SourceTrackId = 10;
    private const int LaterTrackId = 11; // sits after the source in display order — must be bumped

    private const string Config = """
        { "trackSplits": [ {
            "sourceTrackId": 10,
            "event":     { "trackName": "Backstory", "displayQuestion": "What happened, and when?" },
            "condition": { "trackName": "Life Phases", "displayQuestion": "Over what periods, from when to when?" }
        } ] }
        """;

    [Fact]
    public async Task Apply_converts_splits_and_refiles_by_notation()
    {
        var (path, dir) = await BuildFile();
        try
        {
            var exitCode = await DataOpEnvelope.RunAsync(new ConvertWorldDates(), path, ParseConfig(Config), apply: true);
            Assert.Equal(0, exitCode); // includes the envelope's note-content checksum guard

            using var verify = OpenContext(path);

            // Track split: the source row is the event track, a new condition twin exists.
            var source = await verify.NoteTrackDefinitions.SingleAsync(t => t.Id == SourceTrackId);
            Assert.True(source.SupportsWorldDate);
            Assert.False(source.SupportsWorldDateEnd);
            Assert.Equal("What happened, and when?", source.DisplayQuestion);

            var condition = await verify.NoteTrackDefinitions.SingleAsync(t => t.TrackName == "Life Phases");
            Assert.True(condition.SupportsWorldDate);
            Assert.True(condition.SupportsWorldDateEnd);
            Assert.Equal(source.SubjectDefinitionId, condition.SubjectDefinitionId);
            Assert.Equal(source.OwnerType, condition.OwnerType);
            Assert.Equal(TrackType.History, condition.TrackType);

            // The condition twin slots directly after its event twin; later tracks bumped past it.
            Assert.Equal(source.ExpansionModeDisplayOrder + 1, condition.ExpansionModeDisplayOrder);
            var later = await verify.NoteTrackDefinitions.SingleAsync(t => t.Id == LaterTrackId);
            Assert.True(later.ExpansionModeDisplayOrder > condition.ExpansionModeDisplayOrder);

            var notes = await verify.Notes.ToListAsync();
            Note N(int id) => notes.Single(n => n.Id == id);

            // Points converted at year precision, stayed on the event track, legacy blanked.
            Assert.Equal(993, N(1).WorldDateStartYear);
            Assert.Null(N(1).WorldDateStartMonth);
            Assert.Null(N(1).WorldDateEndYear);
            Assert.Equal(SourceTrackId, N(1).NoteTrackDefinitionId);
            Assert.Equal("", N(1).WorldDate);

            Assert.Equal(998, N(4).WorldDateStartYear);   // "998?" — suffix dropped
            Assert.Equal(-300, N(5).WorldDateStartYear);  // "300 BLB" — normalised

            // Ranges converted to intervals AND re-filed to the condition track.
            Assert.Equal(870, N(2).WorldDateStartYear);
            Assert.Equal(928, N(2).WorldDateEndYear);
            Assert.Equal(condition.Id, N(2).NoteTrackDefinitionId);
            Assert.Equal("", N(2).WorldDate);

            Assert.Equal(-100, N(3).WorldDateStartYear);  // "-100-0"
            Assert.Equal(0, N(3).WorldDateEndYear);
            Assert.Equal(condition.Id, N(3).NoteTrackDefinitionId);

            // Unconvertibles: structured stays null, legacy string LEFT IN PLACE (triage), no move.
            Assert.Null(N(6).WorldDateStartYear);
            Assert.Equal("?", N(6).WorldDate);
            Assert.Equal(SourceTrackId, N(6).NoteTrackDefinitionId);

            Assert.Null(N(7).WorldDateStartYear);
            Assert.Equal("954-914", N(7).WorldDate);      // inverted — flag, never guess

            // Undated note untouched.
            Assert.Null(N(8).WorldDateStartYear);
            Assert.Equal("", N(8).WorldDate);
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task A_second_apply_is_a_no_op()
    {
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new ConvertWorldDates(), path, ParseConfig(Config), apply: true));
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new ConvertWorldDates(), path, ParseConfig(Config), apply: true));

            using var verify = OpenContext(path);
            Assert.Equal(1, await verify.NoteTrackDefinitions.CountAsync(t => t.TrackName == "Life Phases"));
            var later = await verify.NoteTrackDefinitions.SingleAsync(t => t.Id == LaterTrackId);
            // Second run finds the twin already present → no second display-order bump.
            Assert.Equal(3, later.ExpansionModeDisplayOrder);
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task Dry_run_persists_nothing()
    {
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new ConvertWorldDates(), path, ParseConfig(Config), apply: false));

            using var verify = OpenContext(path);
            Assert.Equal("993", (await verify.Notes.SingleAsync(n => n.Id == 1)).WorldDate);
            Assert.Null((await verify.Notes.SingleAsync(n => n.Id == 1)).WorldDateStartYear);
            Assert.False(await verify.NoteTrackDefinitions.AnyAsync(t => t.TrackName == "Life Phases"));
        }
        finally { TryDelete(dir); }
    }

    private static JsonElement ParseConfig(string json) => JsonDocument.Parse(json).RootElement;

    private static AppDbContext OpenContext(string path) =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={path}").Options);

    private static void TryDelete(string dir)
    {
        try { Directory.Delete(dir, recursive: true); } catch (IOException) { }
    }

    private static async Task<(string Path, string Dir)> BuildFile()
    {
        var dir = Directory.CreateTempSubdirectory("convert-world-dates-tests-");
        var file = Path.Combine(dir.FullName, "dates.storyplan");

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={file}").Options;
        using var ctx = new AppDbContext(options);
        await ctx.Database.MigrateAsync();

        ctx.SubjectDefinitions.Add(new SubjectDefinition { Id = DefId, SubjectType = "Character", DisplayOrder = 0 });
        ctx.Subjects.Add(new Subject { Id = SubjectId, Name = "Testcharacter", SubjectDefinitionId = DefId });
        ctx.NoteTrackDefinitions.AddRange(
            new NoteTrackDefinition
            {
                Id = SourceTrackId, SubjectDefinitionId = DefId, OwnerType = OwnerType.Subject,
                TrackName = "Backstory", DisplayQuestion = "What is this character's history?",
                TrackType = TrackType.History, SupportsWorldDate = true, ExpansionModeDisplayOrder = 1,
                LinkingModeDisplayOrder = 1, GardenerModeDisplayOrder = 1, AuditModeDisplayOrder = 1,
                SceneDesignModeDisplayOrder = 1
            },
            new NoteTrackDefinition
            {
                Id = LaterTrackId, SubjectDefinitionId = DefId, OwnerType = OwnerType.Subject,
                TrackName = "Psychology", DisplayQuestion = "Who are they?",
                TrackType = TrackType.Characterization, ExpansionModeDisplayOrder = 2,
                LinkingModeDisplayOrder = 2, GardenerModeDisplayOrder = 2, AuditModeDisplayOrder = 2,
                SceneDesignModeDisplayOrder = 2
            });

        Note MakeNote(int id, string worldDate, int sortOrder) => new()
        {
            Id = id, OwnerId = SubjectId, OwnerType = OwnerType.Subject,
            NoteTrackDefinitionId = SourceTrackId, NoteState = NoteState.Unset,
            Content = $"Note {id}", WorldDate = worldDate, SortOrder = sortOrder
        };

        ctx.Notes.AddRange(
            MakeNote(1, "993", 1),
            MakeNote(2, "870-928", 2),
            MakeNote(3, "-100-0", 3),
            MakeNote(4, "998?", 4),
            MakeNote(5, "300 BLB", 5),
            MakeNote(6, "?", 6),
            MakeNote(7, "954-914", 7),
            MakeNote(8, "", 8));

        await ctx.SaveChangesAsync();
        return (file, dir.FullName);
    }
}
