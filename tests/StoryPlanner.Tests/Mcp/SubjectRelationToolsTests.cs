using StoryPlanner.Core;
using StoryPlanner.Mcp;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The subject-relation MCP surface. Beyond "it renders", three things matter: a configured but
/// undrawn edge must report as (none) rather than falling silent, the tree must terminate on a
/// cycle instead of hanging the server, and a subject with no edges must be reported as an
/// ordinary state rather than an error.
/// </summary>
public class SubjectRelationToolsTests
{
    private const int RelationId = 1;

    [Fact]
    public void list_subject_relations_reports_none_defined_rather_than_erroring()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new ReferenceTools(plan.Sources);

        Assert.Contains("none defined", tools.ListSubjectRelations());
    }

    [Fact]
    public void list_subject_relations_reports_the_endpoints_the_kind_and_the_authored_count()
    {
        using var plan = SeededPlan();
        var tools = new ReferenceTools(plan.Sources);

        var result = tools.ListSubjectRelations();

        Assert.Contains("Ancestor", result);
        Assert.Contains($"(relation id:{RelationId})", result);
        Assert.Contains("Character → Character", result);
        Assert.Contains("1 authored", result);
        Assert.Contains("single-valued", result);
        Assert.Contains("forms a hierarchy", result);
        Assert.Contains("read backwards: Succeeded by", result);
    }

    [Fact]
    public void get_subjects_renders_a_drawn_edge_and_an_undrawn_one_explicitly()
    {
        using var plan = SeededPlan();
        var tools = new PlanTools(plan.Sources);

        // EmptySubject is Testcharacter's ancestor, so Testcharacter holds the edge.
        var holder = tools.GetSubjectsPlan([SyntheticPlan.SubjectId], includeNotes: false);
        Assert.Contains("relations: Ancestor=Lonelysubject", holder);

        // The other end has no outgoing edge. It must say so — omission would be
        // indistinguishable from the relation not applying to this subject type at all.
        var root = tools.GetSubjectsPlan([SyntheticPlan.EmptySubjectId], includeNotes: false);
        Assert.Contains("relations: Ancestor=(none)", root);
    }

    [Fact]
    public void get_subject_tree_walks_the_stored_direction_and_carries_property_values()
    {
        using var plan = SeededPlan();
        var tools = new ReferenceTools(plan.Sources);

        var result = tools.GetSubjectTree(SyntheticPlan.SubjectId, RelationId);

        Assert.Contains("Testcharacter", result);
        Assert.Contains("Lonelysubject", result);
        Assert.Contains("\"Ancestor\"", result);
        // The card's whole point: a value visible on each node of the line.
        Assert.Contains("properties: Boundary Axis=Universalism", result);
    }

    [Fact]
    public void get_subject_tree_inverted_renders_descendants_and_labels_itself_so()
    {
        using var plan = SeededPlan();
        var tools = new ReferenceTools(plan.Sources);

        var result = tools.GetSubjectTree(SyntheticPlan.EmptySubjectId, RelationId, inverted: true);

        Assert.Contains("\"Succeeded by\"", result);
        Assert.Contains("Testcharacter", result);
    }

    [Fact]
    public void get_subject_tree_reports_an_edgeless_subject_as_ordinary_not_as_an_error()
    {
        using var plan = SeededPlan();
        var tools = new ReferenceTools(plan.Sources);

        // Walking up from the root: nothing above it, which is the normal state for most subjects.
        var result = tools.GetSubjectTree(SyntheticPlan.EmptySubjectId, RelationId);

        Assert.Contains("Lonelysubject", result);
        Assert.Contains("nothing above this subject", result);
    }

    [Fact]
    public void get_subject_tree_terminates_on_a_cycle_instead_of_hanging()
    {
        // Rows can arrive from DataOps or hand-written SQL without passing the app's picker guard.
        // A naive recursion would spin forever inside the server process.
        using var plan = SeededPlan(alsoCloseTheLoop: true);
        var tools = new ReferenceTools(plan.Sources);

        var result = tools.GetSubjectTree(SyntheticPlan.SubjectId, RelationId);

        Assert.Contains("already above on this line; walk stopped", result);
    }

    [Fact]
    public void get_subject_tree_rejects_an_unknown_subject_and_an_unknown_relation()
    {
        using var plan = SeededPlan();
        var tools = new ReferenceTools(plan.Sources);

        Assert.Contains("not found", tools.GetSubjectTree(9999, RelationId));
        Assert.Contains("not found", tools.GetSubjectTree(SyntheticPlan.SubjectId, 9999));
    }

    /// <summary>
    /// Testcharacter --Ancestor--> Lonelysubject, plus a Boundary Axis value on each so the tree
    /// has something to render per node.
    /// </summary>
    private static SyntheticPlan SeededPlan(bool alsoCloseTheLoop = false)
    {
        var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectRelationDefinitions.Add(new SubjectRelationDefinition
            {
                Id = RelationId,
                SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                TargetSubjectDefinitionId = SyntheticPlan.CharacterDefId,
                Name = "Ancestor",
                InverseName = "Succeeded by",
                DisplayOrder = 1,
                IsSingle = true,
                FormsHierarchy = true
            });
            ctx.SubjectRelations.Add(new SubjectRelation
            {
                Id = 1, RelationDefinitionId = RelationId,
                SubjectId = SyntheticPlan.SubjectId,
                TargetSubjectId = SyntheticPlan.EmptySubjectId
            });

            if (alsoCloseTheLoop)
                ctx.SubjectRelations.Add(new SubjectRelation
                {
                    Id = 2, RelationDefinitionId = RelationId,
                    SubjectId = SyntheticPlan.EmptySubjectId,
                    TargetSubjectId = SyntheticPlan.SubjectId
                });

            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Boundary Axis", DisplayOrder = 1
            });
            ctx.NarrativePropertyValueDefinitions.Add(new NarrativePropertyValueDefinition
            {
                Id = 1, NarrativePropertyDefinitionId = 1, ValueName = "Universalism", ColorHex = "#27AE60"
            });
            ctx.NarrativePropertyValues.Add(new NarrativePropertyValue
            {
                Id = 1, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 1
            });
        });
        plan.Sources.LoadAll();
        return plan;
    }
}
