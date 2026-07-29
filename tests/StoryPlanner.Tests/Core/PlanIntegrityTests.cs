using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// PlanIntegrity is the id-based extraction of the referential-integrity assumptions
/// ContentDeleter's guards encode ad hoc against view models — see the testing skill's
/// "Known gap" note. These tests are its first direct coverage: seed a fixture with rows
/// that deliberately violate the invariants no foreign key enforces, and confirm Check
/// reports them rather than silently returning plausible-looking wrong data.
/// </summary>
public class PlanIntegrityTests
{
    [Fact]
    public void Check_passes_clean_on_an_unmodified_synthetic_plan()
    {
        using var plan = SyntheticPlan.Create();
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Empty(violations);
    }

    [Fact]
    public void Check_reports_a_note_whose_owner_does_not_exist()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.Notes.Add(new Note
        {
            Id = 999, OwnerId = 424242, OwnerType = OwnerType.Subject,
            NoteState = NoteState.Unset, Content = "orphaned", SortOrder = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "note.owner_missing" && v.Detail.Contains("note:999"));
    }

    [Fact]
    public void Check_reports_a_link_whose_plot_point_or_subject_is_dangling()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.PlotPointSubjectLinks.Add(new PlotPointSubjectLink
        {
            Id = 999, PlotPointId = 424242, SubjectId = 424243
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "link.plotpoint_missing" && v.Detail.Contains("link:999"));
        Assert.Contains(violations, v => v.Rule == "link.subject_missing" && v.Detail.Contains("link:999"));
    }

    [Fact]
    public void Check_reports_a_plot_point_pointing_at_a_missing_chapter()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.PlotPoints.Add(new PlotPoint
        {
            Id = 999, Title = "Dangling", ChapterId = 424242, OrderInChapter = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "plotpoint.chapter_missing" && v.Detail.Contains("plotpoint:999"));
    }

    [Fact]
    public void ComputeNoteChecksum_is_stable_across_reads_and_changes_when_content_changes()
    {
        using var plan = SyntheticPlan.Create();
        using var ctx1 = OpenContext(plan.Path);
        using var ctx2 = OpenContext(plan.Path);

        Assert.Equal(PlanIntegrity.ComputeNoteChecksum(ctx1), PlanIntegrity.ComputeNoteChecksum(ctx2));

        var before = PlanIntegrity.ComputeNoteChecksum(ctx1);
        plan.ExternalWrite(ctx =>
        {
            var note = ctx.Notes.First(n => n.Id == SyntheticPlan.VisibleNoteId);
            note.Content += " changed";
        });
        using var ctx3 = OpenContext(plan.Path);
        var after = PlanIntegrity.ComputeNoteChecksum(ctx3);

        Assert.NotEqual(before, after);
    }

    [Fact]
    public void CompareRowCounts_flags_only_tables_outside_the_allowed_set()
    {
        var before = new Dictionary<string, long> { ["Chapters"] = 1, ["Notes"] = 5 };
        var afterGrew = new Dictionary<string, long> { ["Chapters"] = 2, ["Notes"] = 5 };

        var violations = PlanIntegrity.CompareRowCounts(before, afterGrew, allowedToChange: new HashSet<string> { "Chapters" });
        Assert.Empty(violations);

        var violationsUnexpected = PlanIntegrity.CompareRowCounts(before, afterGrew, allowedToChange: new HashSet<string>());
        Assert.Single(violationsUnexpected);
        Assert.Equal("rowcount.changed", violationsUnexpected[0].Rule);
    }

    private static AppDbContext OpenContext(string path) =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={path}").Options);
}
