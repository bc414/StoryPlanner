using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.DataOps;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Runs DataOpEnvelope + SeedNarrativeProperties against a small dedicated file: two work phases,
/// one two-pole axis on a subject type the file has, and one on a subject type it does not.
///
/// The two assertions that matter most are about what the op REFUSES to do — write a
/// NarrativePropertyValue (assignment is authorial), and overwrite prose authored in the app
/// between runs. Both are invariants a well-meaning future edit would break silently.
/// </summary>
public class SeedNarrativePropertiesOpTests
{
    private const int CivDefId = 1;
    private const int CharacterDefId = 2;

    private const string Config = """
        {
          "workPhases": [
            { "name": "Expansion", "displayOrder": 1, "requiresZeroFlaggedNotes": true },
            { "name": "Audit",     "displayOrder": 2, "requiresZeroFlaggedNotes": true, "requiresZeroUnsetNotes": true }
          ],
          "properties": [
            {
              "subjectType": "Civilizational System", "ownerType": "Subject",
              "name": "Boundary Axis", "displayOrder": 3, "gatingWorkPhase": "Audit",
              "values": [ { "valueName": "Universalism" }, { "valueName": "Tribalism" } ]
            },
            {
              "subjectType": "World Law", "ownerType": "Subject",
              "name": "Never Seeded Here", "displayOrder": 1, "gatingWorkPhase": null,
              "values": [ { "valueName": "Irrelevant" } ]
            }
          ]
        }
        """;

    [Fact]
    public async Task Apply_seeds_phases_properties_and_values_and_resolves_the_gate_by_name()
    {
        var (path, dir) = await BuildFile();
        try
        {
            var exitCode = await DataOpEnvelope.RunAsync(new SeedNarrativeProperties(), path, ParseConfig(Config), apply: true);
            Assert.Equal(0, exitCode);

            using var verify = OpenContext(path);

            var audit = await verify.WorkPhases.SingleAsync(p => p.Name == "Audit");
            Assert.Equal(2, audit.DisplayOrder);
            Assert.True(audit.RequiresZeroFlaggedNotes);
            Assert.True(audit.RequiresZeroUnsetNotes);

            var expansion = await verify.WorkPhases.SingleAsync(p => p.Name == "Expansion");
            Assert.True(expansion.RequiresZeroFlaggedNotes);
            Assert.False(expansion.RequiresZeroUnsetNotes);   // omitted in config = false, not inherited

            var boundary = await verify.NarrativePropertyDefinitions.SingleAsync(p => p.Name == "Boundary Axis");
            Assert.Equal(CivDefId, boundary.SubjectDefinitionId);
            Assert.Equal(OwnerType.Subject, boundary.OwnerType);
            Assert.Equal(3, boundary.DisplayOrder);
            // Gate resolved by NAME — the config must survive ids differing between files.
            Assert.Equal(audit.Id, boundary.GatingWorkPhaseId);

            var poles = await verify.NarrativePropertyValueDefinitions
                .Where(v => v.NarrativePropertyDefinitionId == boundary.Id)
                .Select(v => v.ValueName).ToListAsync();
            Assert.Equal(2, poles.Count);
            Assert.Contains("Universalism", poles);
            Assert.Contains("Tribalism", poles);
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task Never_writes_a_narrative_property_value()
    {
        // Assigning a subject a position on an axis is categorization — Brian's alone. The op
        // seeds the vocabulary and stops. ExtraChecks omits NarrativePropertyValues from
        // allowedToChange, so a future edit that assigned one would also fail the envelope.
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedNarrativeProperties(), path, ParseConfig(Config), apply: true));

            using var verify = OpenContext(path);
            Assert.False(await verify.NarrativePropertyValues.AnyAsync());
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task A_second_apply_does_not_duplicate_rows_or_overwrite_prose_authored_in_the_app()
    {
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedNarrativeProperties(), path, ParseConfig(Config), apply: true));

            // Simulate Brian authoring the framing in the Definitions tab between runs. The config
            // has no field that could express any of this, so a re-seed must leave it alone.
            using (var mutate = OpenContext(path))
            {
                var p = await mutate.NarrativePropertyDefinitions.SingleAsync(x => x.Name == "Boundary Axis");
                p.Question = "Who counts as one of us?";
                p.Explanation = "Tribalism is a survival adaptation, not a moral failing.";
                var v = await mutate.NarrativePropertyValueDefinitions.SingleAsync(x => x.ValueName == "Tribalism");
                v.Description = "Protecting the in-group by excluding the out-group.";
                await mutate.SaveChangesAsync();
            }

            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedNarrativeProperties(), path, ParseConfig(Config), apply: true));

            using var verify = OpenContext(path);
            Assert.Equal(2, await verify.WorkPhases.CountAsync());
            Assert.Equal(1, await verify.NarrativePropertyDefinitions.CountAsync());
            Assert.Equal(2, await verify.NarrativePropertyValueDefinitions.CountAsync());

            var property = await verify.NarrativePropertyDefinitions.SingleAsync();
            Assert.Equal("Who counts as one of us?", property.Question);
            Assert.Equal("Tribalism is a survival adaptation, not a moral failing.", property.Explanation);
            Assert.Equal("Protecting the in-group by excluding the out-group.",
                (await verify.NarrativePropertyValueDefinitions.SingleAsync(x => x.ValueName == "Tribalism")).Description);
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task Skips_a_subject_type_the_file_does_not_have_rather_than_inventing_one()
    {
        // The v1-archive case: its SubjectDefinitions hold triage labels, not categories, so every
        // property is skipped and the op is a no-op that still brings the file to migration head.
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedNarrativeProperties(), path, ParseConfig(Config), apply: true));

            using var verify = OpenContext(path);
            Assert.Equal(2, await verify.SubjectDefinitions.CountAsync());   // "World Law" was not created
            Assert.False(await verify.NarrativePropertyDefinitions.AnyAsync(p => p.Name == "Never Seeded Here"));
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task Seeds_no_prose_on_first_run()
    {
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedNarrativeProperties(), path, ParseConfig(Config), apply: true));

            using var verify = OpenContext(path);
            var property = await verify.NarrativePropertyDefinitions.SingleAsync();
            Assert.Equal("", property.Question);
            Assert.Equal("", property.Explanation);
            Assert.All(await verify.NarrativePropertyValueDefinitions.ToListAsync(),
                v => Assert.Equal("", v.Description));
            Assert.All(await verify.WorkPhases.ToListAsync(), p => Assert.Equal("", p.Description));
        }
        finally { TryDelete(dir); }
    }

    [Fact]
    public async Task Dry_run_persists_nothing()
    {
        var (path, dir) = await BuildFile();
        try
        {
            Assert.Equal(0, await DataOpEnvelope.RunAsync(new SeedNarrativeProperties(), path, ParseConfig(Config), apply: false));

            using var verify = OpenContext(path);
            Assert.False(await verify.WorkPhases.AnyAsync());
            Assert.False(await verify.NarrativePropertyDefinitions.AnyAsync());
            Assert.False(await verify.NarrativePropertyValueDefinitions.AnyAsync());
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
        var dir = Directory.CreateTempSubdirectory("seed-narrative-properties-tests-");
        var file = Path.Combine(dir.FullName, "properties.storyplan");

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={file}").Options;
        using var ctx = new AppDbContext(options);
        await ctx.Database.MigrateAsync();

        ctx.SubjectDefinitions.AddRange(
            new SubjectDefinition { Id = CivDefId, SubjectType = "Civilizational System", DisplayOrder = 0 },
            new SubjectDefinition { Id = CharacterDefId, SubjectType = "Character", DisplayOrder = 1 });

        await ctx.SaveChangesAsync();
        return (file, dir.FullName);
    }
}
