using StoryPlanner.Core;
using StoryPlanner.Mcp;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The narrative-property MCP surface. Two things matter beyond "it renders": unset must be
/// reported explicitly rather than omitted (otherwise "not decided" and "no such property" look
/// identical), and the owner-type trace must not collide a subject with a chapter sharing an id —
/// NarrativePropertyValue has no OwnerType column, so that join is hand-written.
/// </summary>
public class NarrativePropertyToolsTests
{
    private const int PropertyId = 1;
    private const int PoleAId = 1;
    private const int PoleBId = 2;

    [Fact]
    public void list_narrative_properties_reports_none_defined_rather_than_erroring()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new ReferenceTools(plan.Sources);

        var result = tools.ListNarrativeProperties();

        Assert.Contains("none defined", result);
    }

    [Fact]
    public void list_narrative_properties_reports_values_with_assignment_counts_and_the_gating_phase()
    {
        using var plan = SeededPlan();
        var tools = new ReferenceTools(plan.Sources);

        var result = tools.ListNarrativeProperties();

        Assert.Contains("Boundary Axis", result);
        Assert.Contains($"(property id:{PropertyId})", result);
        Assert.Contains("gates at phase: Audit", result);
        Assert.Contains("Universalism", result);
        Assert.Contains("Tribalism", result);
        // One subject was assigned Universalism; Tribalism was not.
        Assert.Contains("Universalism (value id:1) — 1 assigned", result);
        Assert.Contains("Tribalism (value id:2) — 0 assigned", result);
    }

    [Fact]
    public void get_subjects_renders_an_assigned_value_and_an_unset_one_explicitly()
    {
        using var plan = SeededPlan();
        var tools = new PlanTools(plan.Sources);

        var assigned = tools.GetSubjectsPlan([SyntheticPlan.SubjectId], includeNotes: false);
        Assert.Contains("properties: Boundary Axis=Universalism", assigned);

        // The other subject has no value. It must say so, not fall silent — omission would be
        // indistinguishable from the property not applying to this type at all.
        var unassigned = tools.GetSubjectsPlan([SyntheticPlan.EmptySubjectId], includeNotes: false);
        Assert.Contains("properties: Boundary Axis=(unset)", unassigned);
    }

    [Fact]
    public void a_chapter_assignment_does_not_leak_onto_a_subject_with_the_same_id()
    {
        // SyntheticPlan's SubjectId and ChapterId are both 1. NarrativePropertyValue carries only
        // OwnerId, so an owner-type-blind join would report the chapter's value on the subject.
        Assert.Equal(SyntheticPlan.SubjectId, SyntheticPlan.ChapterId);

        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 9, SubjectDefinitionId = 0, OwnerType = OwnerType.Chapter,
                Name = "Chapter Axis", DisplayOrder = 1
            });
            ctx.NarrativePropertyValueDefinitions.Add(new NarrativePropertyValueDefinition
            {
                Id = 9, NarrativePropertyDefinitionId = 9, ValueName = "Chapter Value"
            });
            ctx.NarrativePropertyValues.Add(new NarrativePropertyValue
            {
                Id = 9, OwnerId = SyntheticPlan.ChapterId, ValueDefinitionId = 9
            });
        });
        plan.Sources.LoadAll();
        var tools = new PlanTools(plan.Sources);

        var subject = tools.GetSubjectsPlan([SyntheticPlan.SubjectId], includeNotes: false);

        Assert.DoesNotContain("Chapter Value", subject);
        Assert.DoesNotContain("Chapter Axis", subject);
    }

    private static SyntheticPlan SeededPlan()
    {
        var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.WorkPhases.Add(new WorkPhase
            {
                Id = 2, Name = "Audit", DisplayOrder = 2,
                RequiresZeroFlaggedNotes = true, RequiresZeroUnsetNotes = true
            });
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = PropertyId, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Boundary Axis",
                DisplayOrder = 1, GatingWorkPhaseId = 2
            });
            ctx.NarrativePropertyValueDefinitions.AddRange(
                new NarrativePropertyValueDefinition { Id = PoleAId, NarrativePropertyDefinitionId = PropertyId, ValueName = "Universalism" },
                new NarrativePropertyValueDefinition { Id = PoleBId, NarrativePropertyDefinitionId = PropertyId, ValueName = "Tribalism" });
            ctx.NarrativePropertyValues.Add(new NarrativePropertyValue
            {
                Id = 1, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = PoleAId
            });
        });
        plan.Sources.LoadAll();
        return plan;
    }
}
