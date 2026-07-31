using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The gap nothing else covers: upgrading an EXISTING, POPULATED file — not a fresh
/// SyntheticPlan built straight to head. This is exactly the risk profile of the AddStories
/// migration running against Brian's real .storyplan files, which already hold thousands of
/// rows at the previous migration head.
/// </summary>
public class StoryMigrationTests
{
    private const string PreStoriesMigration = "20260716032821_ConversationSourceUuidAndIgnoreList";

    [Fact]
    public async Task Upgrading_a_populated_pre_stories_file_preserves_every_row_and_defaults_StoryId_to_zero()
    {
        var dir = Directory.CreateTempSubdirectory("storyplan-migration-tests-");
        var file = Path.Combine(dir.FullName, "pre-stories.storyplan");
        try
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={file}")
                .Options;

            // 1. Build the file at the migration immediately BEFORE AddStories — this is the
            //    schema shape Brian's real files are at until this feature ships.
            using (var ctx = new AppDbContext(options))
            {
                var migrator = ctx.GetInfrastructure().GetRequiredService<IMigrator>();
                await migrator.MigrateAsync(PreStoriesMigration);

                // Schema-changed tables must be seeded via raw SQL matching the OLD schema. EF
                // always generates INSERTs from the CURRENT compiled model — which already
                // declares Chapters.StoryId (AddStories) and the MasterTimeline columns
                // (Notes.WorldDate*, PlotPoints.TheaterId/Fabula*, Subjects.TheaterId) — and
                // none of those columns exist in the physical table at this migration. Tables
                // untouched by both migrations still go through the normal tracked DbSet.
                await ctx.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Chapters (Id, OrderIndex, Title) VALUES (1, 1, 'Chapter One'), (2, 2, 'Chapter Two')");
                await ctx.Database.ExecuteSqlRawAsync(
                    "INSERT INTO PlotPoints (Id, Title, ChapterId, OrderInChapter) VALUES (1, 'A scene', 1, 1)");
                await ctx.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Subjects (Id, Name, Description, Abbreviation, ColorHex, SubjectDefinitionId) " +
                    "VALUES (1, 'A subject', '', '', '', 1)");
                await ctx.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Notes (Id, OwnerId, OwnerType, LastModified, Content, NoteState, FlagReason, SortOrder, WorldDate) " +
                    "VALUES (1, 1, 2, '2026-01-01', 'Pre-migration content', 0, '', 1, '870-928')");

                ctx.SubjectDefinitions.Add(new SubjectDefinition { Id = 1, SubjectType = "Character", DisplayOrder = 0 });
                await ctx.SaveChangesAsync();
            }

            // 2. Upgrade to head — this is what StoryService.OpenProjectAsync does on next open.
            using (var ctx = new AppDbContext(options))
            {
                await ctx.Database.MigrateAsync();
            }

            // 3. Every row survives, and every chapter defaults to StoryId 0 ("(Unassigned)").
            using (var verify = new AppDbContext(options))
            {
                Assert.Equal(2, await verify.Chapters.CountAsync());
                Assert.Equal(1, await verify.PlotPoints.CountAsync());
                Assert.Equal(1, await verify.Notes.CountAsync());
                Assert.Empty(await verify.Stories.ToListAsync());

                foreach (var chapter in await verify.Chapters.ToListAsync())
                    Assert.Equal(0, chapter.StoryId);

                var chapterOne = await verify.Chapters.SingleAsync(c => c.Id == 1);
                Assert.Equal("Chapter One", chapterOne.Title);
                Assert.Equal(1, chapterOne.OrderIndex);

                var note = await verify.Notes.SingleAsync();
                Assert.Equal("Pre-migration content", note.Content);

                // MasterTimeline columns arrive empty/sentinel: legacy free-text WorldDate is
                // preserved untouched (conversion is the DataOps op's job, never the migration's),
                // structured dates are null, and theater ids default to the "(Unplaced)" 0.
                Assert.Equal("870-928", note.WorldDate);
                Assert.Null(note.WorldDateStartYear);
                Assert.Null(note.WorldDateEndYear);
                Assert.Equal(0, (await verify.PlotPoints.SingleAsync()).TheaterId);
                Assert.Null((await verify.PlotPoints.SingleAsync()).FabulaYear);
                Assert.Equal(0, (await verify.Subjects.SingleAsync()).TheaterId);

                // FocalCharacter columns (2026-07-31): a plot point's POV defaults to
                // undesignated (null, not a sentinel — there is no "(No POV)" row), and a
                // subject defaults to not being a POV candidate.
                Assert.Null((await verify.PlotPoints.SingleAsync()).FocalCharacterId);
                Assert.False((await verify.Subjects.SingleAsync()).IsPovCharacter);
            }
        }
        finally
        {
            try { Directory.Delete(dir.FullName, recursive: true); } catch (IOException) { }
        }
    }
}
