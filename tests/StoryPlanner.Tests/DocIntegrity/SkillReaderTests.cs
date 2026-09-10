using System.Linq;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The multi-file reader: the router in SKILL.md, the Artifacts table, and one Processes
/// table per activity file named by a router row. Where it refuses, it refuses by rule id.
/// </summary>
public class SkillReaderTests
{
    [Fact]
    public void The_three_tables_are_read_into_typed_rows()
    {
        using var f = new MapFixture();
        var doc = f.Doc;

        Assert.Equal(3, doc.Activities.Count);
        Assert.Equal(4, doc.Processes.Count);
        Assert.Equal(12, doc.Artifacts.Count);

        var judge = doc.Processes.Single(p => p.Id == "assess-referee-items");
        Assert.Equal("refereeing-candidates", judge.Activity);
        Assert.Equal("agent", judge.Mode);
        Assert.Empty(judge.Instruments);
        Assert.Equal(["directions", "items"], judge.Reads);
        Assert.Equal(["results"], judge.Writes);
        Assert.Equal(MapFixture.RefereeingFile, judge.File);

        var promoting = doc.Activities.Single(a => a.Id == "promoting-refereed-candidates");
        Assert.Equal(["changing-the-planner-for-v3"], promoting.Enables);
        Assert.Empty(doc.Activities.Single(a => a.Id == "changing-the-planner-for-v3").Enables);
    }

    [Fact]
    public void A_router_row_with_no_activity_file_yields_no_processes_rather_than_refusing()
    {
        using var f = MapFixture.Without(MapFixture.PromotingFile);
        var doc = f.Doc;
        Assert.Equal(3, doc.Activities.Count);
        Assert.Empty(doc.ProcessesOf("promoting-refereed-candidates"));
    }

    [Fact]
    public void A_markdown_file_that_is_neither_a_companion_nor_an_activity_is_listed_as_an_orphan()
    {
        using var f = MapFixture.WithExtra("stray.md", "# stray\n");
        Assert.Equal(["stray.md"], f.Doc.OrphanActivityFiles);
    }

    [Fact]
    public void A_table_with_an_unrecognised_signature_is_refused_by_rule_id()
    {
        using var f = MapFixture.With(MapFixture.PromotingFile,
            "Promotes what Brian did not decide.",
            "Promotes what Brian did not decide.\n\n| surprise | column |\n|---|---|\n| one | two |\n");
        var ex = Assert.Throws<MapFormatException>(() => f.Doc);
        Assert.Equal(SkillReader.UnknownSignature, ex.CheckId);
    }

    [Fact]
    public void A_malformed_row_is_refused_with_the_file_named()
    {
        using var f = MapFixture.With(MapFixture.PromotingFile,
            "| Brian decides each candidate |",
            "| Brian decides each candidate | extra |");
        var ex = Assert.Throws<MapFormatException>(() => f.Doc);
        Assert.Equal(MapFormatException.Unparseable, ex.CheckId);
        Assert.Contains(MapFixture.PromotingFile, ex.Message);
    }

    [Fact]
    public void A_missing_router_is_refused_by_its_own_rule_id()
    {
        using var f = MapFixture.Without(MapFixture.SkillFile);
        var ex = Assert.Throws<MapFormatException>(() => f.Doc);
        Assert.Equal("skill.missing", ex.CheckId);
    }

    [Fact]
    public void A_SKILL_md_without_an_artifacts_table_is_refused_by_its_own_rule_id()
    {
        using var f = MapFixture.With(MapFixture.SkillFile, MapFixture.ArtifactsSection, "");
        var ex = Assert.Throws<MapFormatException>(() => f.Doc);
        Assert.Equal("artifacts.missing", ex.CheckId);
    }

    [Fact]
    public void A_second_artifacts_table_in_SKILL_md_is_refused()
    {
        using var f = MapFixture.With(MapFixture.SkillFile, "## Companions", MapFixture.ArtifactsSection + "## Companions");
        var ex = Assert.Throws<MapFormatException>(() => f.Doc);
        Assert.Equal(SkillReader.UnknownSignature, ex.CheckId);
    }

    [Fact]
    public void A_schema_file_no_row_names_is_listed_as_an_orphan()
    {
        using var f = new MapFixture();
        f.WriteSchema("stray-schema", "# stray-schema\n");
        Assert.Equal(["stray-schema.md"], f.Doc.OrphanSchemaFiles);
    }
}
