using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.DataOps;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// seed-timeline-defaults idempotency, with the 2026-08-02 prose rule: theater and pivot names
/// and descriptions are Brian's framing of his fictional world, so they are stamped only at row
/// creation — a re-run updates ordering (structure) and can never clobber an in-app rewrite.
/// </summary>
public class SeedTimelineDefaultsOpTests
{
    private const string Config = """
        {
          "theaters": [
            { "name": "Heartland", "orderIndex": 1, "description": "Seeded description" },
            { "name": "Frontier",  "orderIndex": 2 }
          ],
          "pivots": [
            { "year": 0,    "name": "The Banishment", "description": "Seeded pivot description" },
            { "year": 1007, "name": "The Return" }
          ]
        }
        """;

    [Fact]
    public async Task Apply_creates_rows_with_prose_then_a_rerun_updates_order_only()
    {
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedTimelineDefaults(), path, ParseConfig(Config), apply: true));

            await using (var app = OpenContext(path))
            {
                var heartland = await app.Theaters.SingleAsync(t => t.Name == "Heartland");
                Assert.Equal("Seeded description", heartland.Description);
                heartland.Description = "Brian's rewritten description";

                var banishment = await app.Pivots.SingleAsync(p => p.Year == 0);
                Assert.Equal("The Banishment", banishment.Name);
                banishment.Name = "The Banishment of the Sisters";

                await app.SaveChangesAsync();
            }

            // Re-run with the ORIGINAL config: rows must not duplicate, prose must survive.
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedTimelineDefaults(), path, ParseConfig(Config), apply: true));

            using var verify = OpenContext(path);
            Assert.Equal(2, await verify.Theaters.CountAsync());
            Assert.Equal(2, await verify.Pivots.CountAsync());
            Assert.Equal("Brian's rewritten description",
                (await verify.Theaters.SingleAsync(t => t.Name == "Heartland")).Description);
            Assert.Equal("The Banishment of the Sisters",
                (await verify.Pivots.SingleAsync(p => p.Year == 0)).Name);
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
        var dir = Directory.CreateTempSubdirectory("seed-timeline-defaults-tests-");
        var file = Path.Combine(dir.FullName, "timeline.storyplan");

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={file}").Options;
        using var ctx = new AppDbContext(options);
        await ctx.Database.MigrateAsync();
        await ctx.SaveChangesAsync();
        return (file, dir.FullName);
    }
}
