using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The Core-level delete cascades and the safety backup. These are the choke points every UI
/// delete path now routes through (2026-08-02): DeleteNote carries the NoteSourceReference
/// cascade, DeleteLink the NarrativePropertyValue cascade — so the invariants hold no matter
/// which view model triggered the delete.
/// </summary>
public class StoryServiceTests
{
    [Fact]
    public async Task DeleteNote_removes_the_notes_citations_with_it()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "MLP:FiM" });
            ctx.NoteSourceReferences.AddRange(
                new NoteSourceReference { Id = 1, NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 1 },
                new NoteSourceReference { Id = 2, NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 1 },
                // A different note's citation must survive.
                new NoteSourceReference { Id = 3, NoteId = SyntheticPlan.PlotPointNoteId, SourceMaterialId = 1 });
        });
        using var svc = await plan.OpenStoryServiceAsync();

        svc.DeleteNote(SyntheticPlan.VisibleNoteId);
        await svc.SaveAsync();

        Assert.DoesNotContain(svc.Notes, n => n.Id == SyntheticPlan.VisibleNoteId);
        Assert.DoesNotContain(svc.NoteSourceReferences, r => r.NoteId == SyntheticPlan.VisibleNoteId);
        Assert.Contains(svc.NoteSourceReferences, r => r.NoteId == SyntheticPlan.PlotPointNoteId);
    }

    [Fact]
    public async Task DeleteLink_removes_only_the_links_property_values_despite_the_missing_OwnerType()
    {
        // NarrativePropertyValue has no OwnerType column. The subject and the link in the
        // fixture share numeric id 1 — exactly the collision the definition-trace exists
        // to prevent. Deleting the link must keep the subject's value.
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.NarrativePropertyDefinitions.AddRange(
                new NarrativePropertyDefinition
                {
                    Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    OwnerType = OwnerType.PlotPointSubjectLink, Name = "Link-scoped"
                },
                new NarrativePropertyDefinition
                {
                    Id = 2, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    OwnerType = OwnerType.Subject, Name = "Subject-scoped"
                });
            ctx.NarrativePropertyValueDefinitions.AddRange(
                new NarrativePropertyValueDefinition { Id = 1, NarrativePropertyDefinitionId = 1, ValueName = "On link" },
                new NarrativePropertyValueDefinition { Id = 2, NarrativePropertyDefinitionId = 2, ValueName = "On subject" });
            ctx.NarrativePropertyValues.AddRange(
                new NarrativePropertyValue { Id = 1, OwnerId = SyntheticPlan.LinkId, ValueDefinitionId = 1 },
                new NarrativePropertyValue { Id = 2, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 2 });
        });
        using var svc = await plan.OpenStoryServiceAsync();

        svc.DeleteLink(SyntheticPlan.LinkId);
        await svc.SaveAsync();

        Assert.DoesNotContain(svc.PlotPointsSubjectLinks, l => l.Id == SyntheticPlan.LinkId);
        Assert.DoesNotContain(svc.NarrativePropertyValues, v => v.Id == 1);
        Assert.Contains(svc.NarrativePropertyValues, v => v.Id == 2);
    }

    [Fact]
    public void CreateSafetyBackup_produces_a_consistent_snapshot_and_reports_success()
    {
        using var plan = SyntheticPlan.Create();

        Assert.True(StoryService.CreateSafetyBackup(plan.Path));

        var backupDir = Path.Combine(Path.GetDirectoryName(plan.Path)!, "Backups");
        var backup = Directory.GetFiles(backupDir, "*.bak").Single();

        // VACUUM INTO reads through the WAL, so the snapshot must contain the seeded rows —
        // a bare File.Copy of a WAL-mode main file would miss unchecked-pointed transactions
        // (the documented 2026-07-30 stale-copy trap).
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={backup};Mode=ReadOnly").Options;
        using var ctx = new AppDbContext(options);
        Assert.Equal(2, ctx.Subjects.Count());
        Assert.True(ctx.Notes.Count() >= 7);
    }

    [Fact]
    public void CreateSafetyBackup_returns_false_when_the_file_cannot_be_read()
    {
        // Callers about to migrate or run a DataOps write must refuse on false —
        // proceeding without a net is the failure mode this return value exists for.
        Assert.False(StoryService.CreateSafetyBackup(
            Path.Combine(Path.GetTempPath(), "storyplan-tests-missing", "no-such-file.storyplan")));
    }
}
