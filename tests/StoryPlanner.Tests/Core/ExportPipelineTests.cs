using StoryPlanner.Core;
using StoryPlanner.Core.Export;
using StoryPlanner.Core.Models;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// ExportResolver + NoteExportRenderer — the app's "give me scoped story context" engine,
/// and the other half of the flagged wall (NoteExportRenderer excludes Flagged notes, the
/// same rule the MCP server enforces).
///
/// Exercised through the REAL StoryService rather than a fake: the resolver reads
/// DbSet.Local-backed ObservableCollections, and a hand-rolled IStoryService would prove
/// the test's assumptions rather than the code.
/// </summary>
public class ExportPipelineTests
{
    // ── Scope expansion ─────────────────────────────────────────────────────

    [Fact]
    public async Task Scope0_returns_only_the_anchored_subject()
    {
        using var plan = SyntheticPlan.Create();
        using var svc = await plan.OpenStoryServiceAsync();

        var config = new ExportConfiguration
        {
            Anchors = [(SyntheticPlan.SubjectId, OwnerType.Subject)],
            Scope = 0
        };
        var result = ExportResolver.Resolve(config, svc);

        Assert.Contains(SyntheticPlan.SubjectId, result.FullSubjectIds);
        Assert.Empty(result.FullPlotPointIds);
        Assert.Empty(result.ThinPlotPointIds);
        // Scope 0 only activates a link when BOTH ends are anchored.
        Assert.Empty(result.ActiveLinks);
    }

    [Fact]
    public async Task Scope1_expands_a_subject_anchor_to_thin_plot_points()
    {
        using var plan = SyntheticPlan.Create();
        using var svc = await plan.OpenStoryServiceAsync();

        var config = new ExportConfiguration
        {
            Anchors = [(SyntheticPlan.SubjectId, OwnerType.Subject)],
            Scope = 1
        };
        var result = ExportResolver.Resolve(config, svc);

        Assert.Contains((SyntheticPlan.PlotPointId, SyntheticPlan.SubjectId), result.ActiveLinks);
        Assert.Contains(SyntheticPlan.PlotPointId, result.ThinPlotPointIds);
        Assert.DoesNotContain(SyntheticPlan.PlotPointId, result.FullPlotPointIds);
    }

    [Fact]
    public async Task Scope2_promotes_thin_plot_points_to_full()
    {
        using var plan = SyntheticPlan.Create();
        using var svc = await plan.OpenStoryServiceAsync();

        var config = new ExportConfiguration
        {
            Anchors = [(SyntheticPlan.SubjectId, OwnerType.Subject)],
            Scope = 2
        };
        var result = ExportResolver.Resolve(config, svc);

        Assert.Contains(SyntheticPlan.PlotPointId, result.FullPlotPointIds);
        // Step 7 dedup: full wins over thin, so the id must not appear in both.
        Assert.DoesNotContain(SyntheticPlan.PlotPointId, result.ThinPlotPointIds);
    }

    [Fact]
    public async Task A_chapter_anchor_expands_to_its_plot_points_without_consuming_scope()
    {
        using var plan = SyntheticPlan.Create();
        using var svc = await plan.OpenStoryServiceAsync();

        var config = new ExportConfiguration
        {
            Anchors = [(SyntheticPlan.ChapterId, OwnerType.Chapter)],
            Scope = 0
        };
        var result = ExportResolver.Resolve(config, svc);

        Assert.Contains(SyntheticPlan.PlotPointId, result.FullPlotPointIds);
    }

    [Fact]
    public async Task ResolveAll_includes_everything_at_full_depth()
    {
        using var plan = SyntheticPlan.Create();
        using var svc = await plan.OpenStoryServiceAsync();

        var result = ExportResolver.ResolveAll(svc);

        Assert.Equal(2, result.FullSubjectIds.Count);
        Assert.Contains(SyntheticPlan.PlotPointId, result.FullPlotPointIds);
        Assert.Contains((SyntheticPlan.PlotPointId, SyntheticPlan.SubjectId), result.ActiveLinks);
    }

    [Fact]
    public async Task A_subject_with_no_links_expands_to_nothing_at_any_scope()
    {
        using var plan = SyntheticPlan.Create();
        using var svc = await plan.OpenStoryServiceAsync();

        var config = new ExportConfiguration
        {
            Anchors = [(SyntheticPlan.EmptySubjectId, OwnerType.Subject)],
            Scope = 2
        };
        var result = ExportResolver.Resolve(config, svc);

        // 221 of 263 real v2 subjects are in this state — scope expansion is a no-op for them.
        Assert.Empty(result.ActiveLinks);
        Assert.Empty(result.FullPlotPointIds);
    }

    // ── Renderer ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Renderer_excludes_flagged_notes_the_same_as_the_MCP_wall()
    {
        using var plan = SyntheticPlan.Create();
        using var svc = await plan.OpenStoryServiceAsync();

        var config = new ExportConfiguration { Anchors = [], Scope = 0 };
        var markdown = NoteExportRenderer.Build(ExportResolver.ResolveAll(svc), config, svc);

        // NoteExportRenderer.cs:28 — the export has never shown flagged content to an LLM.
        Assert.DoesNotContain(SyntheticPlan.FlaggedContentSecret, markdown);
        Assert.DoesNotContain(SyntheticPlan.FlaggedContentEnvelope, markdown);
        Assert.DoesNotContain(SyntheticPlan.FlaggedReasonSecret, markdown);
        Assert.Contains(SyntheticPlan.VisibleSecret, markdown); // ordinary notes still render
    }

    [Fact]
    public async Task Renderer_emits_track_names_and_subject_structure()
    {
        using var plan = SyntheticPlan.Create();
        using var svc = await plan.OpenStoryServiceAsync();

        var config = new ExportConfiguration { Anchors = [], Scope = 0 };
        var markdown = NoteExportRenderer.Build(ExportResolver.ResolveAll(svc), config, svc);

        Assert.Contains("Testcharacter", markdown);
        Assert.Contains("Backstory", markdown);
        Assert.Contains("Character", markdown); // the subject type grouping
    }

    [Fact]
    public async Task Renderer_honours_the_track_type_filter()
    {
        using var plan = SyntheticPlan.Create();
        using var svc = await plan.OpenStoryServiceAsync();

        var config = new ExportConfiguration
        {
            Anchors = [],
            Scope = 0,
            IncludedTrackTypes = [TrackType.WorldInference]   // excludes History (Backstory)
        };
        var markdown = NoteExportRenderer.Build(ExportResolver.ResolveAll(svc), config, svc);

        Assert.DoesNotContain(SyntheticPlan.VisibleSecret, markdown);
    }
}
