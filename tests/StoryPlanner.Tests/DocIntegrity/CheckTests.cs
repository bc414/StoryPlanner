using System.IO;
using System.Linq;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The <c>check</c> verb's scoping, pure tier: one path in, everything governed at or under it
/// out, following the fixture's artifacts table. Assertions are on rule ids and on which
/// folders and files were checked, never on message prose.
/// </summary>
public class CheckTests
{
    [Fact]
    public void A_file_in_a_skill_folder_checks_the_folder_s_shape()
    {
        using var f = MapFixture.With(MapFixture.SkillFile,
            "| candidates | docs/v3-framework/<study>/candidates.md |",
            "| items | docs/v3-framework/<study>/candidates.md |");
        var result = Check.Run(f.RepoRoot, Path.Combine(f.SkillFolder, MapFixture.SkillFile));
        Assert.False(result.Report.Passed);
        Assert.Contains("id.duplicate", result.Report.Findings.Select(x => x.CheckId));
        Assert.Equal([Path.GetFullPath(f.SkillFolder)], result.SkillFolders);
        Assert.Empty(result.GovernedFiles);
    }

    [Fact]
    public void A_governed_file_checks_its_class_s_schema_and_is_named_by_its_repo_path()
    {
        using var f = new MapFixture().WithStateTree();
        var path = f.TreePath("docs", "v3-framework", "hypotheses", "032-other.md");
        var result = Check.Run(f.RepoRoot, path);
        var mismatch = Assert.Single(result.Report.Findings, x => x.CheckId == "hypothesis.status.mismatch");
        Assert.Equal("docs/v3-framework/hypotheses/032-other.md", mismatch.RowId);
        Assert.Empty(result.SkillFolders);
        Assert.Equal([path], result.GovernedFiles);
    }

    [Fact]
    public void A_file_governed_by_nothing_passes_with_no_findings()
    {
        using var f = new MapFixture();
        Directory.CreateDirectory(f.TreePath("docs"));
        File.WriteAllText(f.TreePath("docs", "note.md"), "# note\n");
        var result = Check.Run(f.RepoRoot, f.TreePath("docs", "note.md"));
        Assert.True(result.Report.Passed);
        Assert.Empty(result.Report.Findings);
        Assert.Empty(result.SkillFolders);
        Assert.Empty(result.GovernedFiles);
    }

    [Fact]
    public void The_repository_root_checks_every_skill_folder_and_every_governed_file()
    {
        using var f = new MapFixture().WithStateTree();
        var result = Check.Run(f.RepoRoot, f.RepoRoot);
        Assert.Equal([Path.GetFullPath(f.SkillFolder)], result.SkillFolders);
        var files = result.GovernedFiles.Select(p => Path.GetRelativePath(f.RepoRoot, p).Replace('\\', '/')).ToList();
        Assert.Contains("docs/v3-framework/hypotheses/031-dt-classes.md", files);
        Assert.Contains("docs/v3-framework/hypotheses/032-other.md", files);
        Assert.Contains("docs/v3-framework/studies.md", files);
        Assert.Equal(files.Count, files.Distinct().Count());
        Assert.False(result.Report.Passed);
        Assert.Contains("hypothesis.status.mismatch", result.Report.Findings.Select(x => x.CheckId));
    }

    [Fact]
    public void A_narrower_folder_bounds_the_check()
    {
        using var f = new MapFixture().WithStateTree();

        var docs = Check.Run(f.RepoRoot, f.TreePath("docs"));
        Assert.Empty(docs.SkillFolders);
        Assert.NotEmpty(docs.GovernedFiles);
        Assert.All(docs.GovernedFiles, p => Assert.StartsWith(f.TreePath("docs"), p));

        var skill = Check.Run(f.RepoRoot, f.SkillFolder);
        Assert.Equal([Path.GetFullPath(f.SkillFolder)], skill.SkillFolders);
        Assert.Empty(skill.GovernedFiles);
        Assert.True(skill.Report.Passed);
    }

    [Fact]
    public void A_class_with_a_checker_but_no_row_in_a_checked_folder_is_reported_not_failed()
    {
        using var f = new MapFixture();
        var result = Check.Run(f.RepoRoot, f.SkillFolder);
        Assert.True(result.Report.Passed);
        var noRow = result.Report.Findings.Where(x => x.CheckId == "check.no-row").ToList();
        Assert.All(noRow, x => Assert.Equal(FindingLevel.Info, x.Level));
        Assert.Contains(noRow, x => x.RowId == WellKnown.HypothesisIndex);
        Assert.Contains(noRow, x => x.RowId == WellKnown.LeadsArtifact);
        Assert.DoesNotContain(noRow, x => x.RowId == WellKnown.Studies);
    }

    [Fact]
    public void A_governed_skill_folder_is_located_from_itself_and_from_a_file_inside_it()
    {
        using var f = new MapFixture();
        Assert.Equal(Path.GetFullPath(f.SkillFolder), GovernedSkill.Locate(f.SkillFolder));
        Assert.Equal(Path.GetFullPath(f.SkillFolder), GovernedSkill.Locate(Path.Combine(f.SkillFolder, MapFixture.SkillFile)));
        Assert.Equal(Path.GetFullPath(f.RepoRoot), GovernedSkill.RepoRootOf(f.SkillFolder));
    }
}
