using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.DataOps;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Runs DataOpEnvelope + SeedSourceMaterial against a small dedicated file: one Work with two
/// Parts, one Work with no Parts (the "cite the whole Work" case, e.g. Hearts of Iron IV), and
/// one track to enable. Proves the upsert-by-Name/(Work,Code) idempotency and that ReviewState
/// set on first creation survives a re-run (a re-seed must never silently un-review a Part).
/// </summary>
public class SeedSourceMaterialOpTests
{
    private const int DefId = 1;
    private const int TrackId = 10;

    private const string Config = """
        {
          "sourceMaterials": [
            {
              "name": "MLP:FiM", "description": "The canon series.", "partNoun": "Episode", "orderIndex": 1,
              "parts": [
                { "code": "S3E01", "name": "The Crystal Empire Part 1", "orderIndex": 1 },
                { "code": "Dragonshy", "name": "", "orderIndex": 2 }
              ]
            },
            {
              "name": "Hearts of Iron IV", "description": "Base game.", "partNoun": "", "orderIndex": 2,
              "parts": []
            }
          ],
          "enableSourceMaterialOnTracks": [10]
        }
        """;

    [Fact]
    public async Task Apply_seeds_works_parts_and_enables_the_configured_track()
    {
        var (path, dir) = await BuildFile();
        try
        {
            var exitCode = await DataOpEnvelope.RunAsync(new SeedSourceMaterial(), path, ParseConfig(Config), apply: true);
            Assert.Equal(0, exitCode);

            using var verify = OpenContext(path);

            var fim = await verify.SourceMaterials.SingleAsync(w => w.Name == "MLP:FiM");
            Assert.Equal("Episode", fim.PartNoun);
            Assert.Equal(1, fim.OrderIndex);

            var crystalEmpire = await verify.SourceMaterialParts.SingleAsync(p => p.Code == "S3E01");
            Assert.Equal(fim.Id, crystalEmpire.SourceMaterialId);
            Assert.Equal("The Crystal Empire Part 1", crystalEmpire.Name);
            Assert.Equal(SourcePartReviewState.NotReviewed, crystalEmpire.ReviewState);

            var hoi4 = await verify.SourceMaterials.SingleAsync(w => w.Name == "Hearts of Iron IV");
            Assert.Equal("", hoi4.PartNoun);
            Assert.False(await verify.SourceMaterialParts.AnyAsync(p => p.SourceMaterialId == hoi4.Id));

            var track = await verify.NoteTrackDefinitions.SingleAsync(t => t.Id == TrackId);
            Assert.True(track.SupportsSourceMaterial);
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task A_second_apply_does_not_duplicate_rows_or_reset_review_state()
    {
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedSourceMaterial(), path, ParseConfig(Config), apply: true));

            // Simulate Brian reviewing a Part between runs — the re-seed must not clobber this.
            using (var mutate = OpenContext(path))
            {
                var part = await mutate.SourceMaterialParts.SingleAsync(p => p.Code == "Dragonshy");
                part.ReviewState = SourcePartReviewState.Reviewed;
                await mutate.SaveChangesAsync();
            }

            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedSourceMaterial(), path, ParseConfig(Config), apply: true));

            using var verify = OpenContext(path);
            Assert.Equal(2, await verify.SourceMaterials.CountAsync());
            Assert.Equal(2, await verify.SourceMaterialParts.CountAsync());

            var dragonshy = await verify.SourceMaterialParts.SingleAsync(p => p.Code == "Dragonshy");
            Assert.Equal(SourcePartReviewState.Reviewed, dragonshy.ReviewState);
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task Dry_run_persists_nothing()
    {
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedSourceMaterial(), path, ParseConfig(Config), apply: false));

            using var verify = OpenContext(path);
            Assert.False(await verify.SourceMaterials.AnyAsync());
            Assert.False(await verify.SourceMaterialParts.AnyAsync());
            Assert.False((await verify.NoteTrackDefinitions.SingleAsync(t => t.Id == TrackId)).SupportsSourceMaterial);
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task Is_tolerant_of_a_configured_track_id_that_does_not_exist()
    {
        // v1-archive tolerance, matching ConvertWorldDates: a track id absent from this file
        // must not fail the op — only Notes' polymorphic owners get that treatment elsewhere;
        // this op's track lookup is a plain Where(), which already skips silently.
        const string configWithMissingTrack = """
            { "sourceMaterials": [], "enableSourceMaterialOnTracks": [10, 424242] }
            """;
        var (path, dir) = await BuildFile();
        try
        {
            var exitCode = await DataOpEnvelope.RunAsync(new SeedSourceMaterial(), path, ParseConfig(configWithMissingTrack), apply: true);
            Assert.Equal(0, exitCode);

            using var verify = OpenContext(path);
            Assert.True((await verify.NoteTrackDefinitions.SingleAsync(t => t.Id == TrackId)).SupportsSourceMaterial);
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
        var dir = Directory.CreateTempSubdirectory("seed-source-material-tests-");
        var file = Path.Combine(dir.FullName, "sources.storyplan");

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={file}").Options;
        using var ctx = new AppDbContext(options);
        await ctx.Database.MigrateAsync();

        ctx.SubjectDefinitions.Add(new SubjectDefinition { Id = DefId, SubjectType = "Character", DisplayOrder = 0 });
        ctx.NoteTrackDefinitions.Add(new NoteTrackDefinition
        {
            Id = TrackId, SubjectDefinitionId = DefId, OwnerType = OwnerType.Subject,
            TrackName = "Source Material References", DisplayQuestion = "What canon informs this?",
            TrackType = TrackType.Canon, ExpansionModeDisplayOrder = 1, LinkingModeDisplayOrder = 1,
            GardenerModeDisplayOrder = 1, AuditModeDisplayOrder = 1, SceneDesignModeDisplayOrder = 1
        });

        await ctx.SaveChangesAsync();
        return (file, dir.FullName);
    }
}
