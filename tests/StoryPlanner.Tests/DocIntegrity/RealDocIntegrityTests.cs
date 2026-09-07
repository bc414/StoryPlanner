using System.IO;
using System.Linq;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The tests that read the real skill folder. Once the validate test is un-skipped,
/// <c>dotnet test</c> pins the method's topology the way <c>PlanIntegrity</c> pins the data's:
/// a row edited into an unreachable artifact, an ungated hypothesis write, or a cycle in
/// <c>enables</c> fails the build.
/// </summary>
public class RealDocIntegrityTests
{
    /// <summary>
    /// Revision 2 is built beside the live skill and swapped in one commit; until then this is
    /// the folder. Un-skipped 2026-09-05 when handoff 2's step 2 landed (validate passes on the
    /// folder); repoint at v3-buildout in the router-swap commit, where the test also gains
    /// <c>claude plugin validate .claude/skills</c>.
    /// </summary>
    const string SkillFolderName = "v3-buildout-2";

    [Fact]
    public void The_real_skill_validates()
    {
        var report = Validator.Validate(RealSkillFolder());
        Assert.True(report.Passed, string.Join("\n", report.Findings
            .Where(f => f.Level == FindingLevel.Failure)
            .Select(f => $"{f.CheckId} {f.RowId} {f.Message}")));
    }

    /// <summary>
    /// The real tables parse even while they fail validation. Structure and semantics are
    /// separate failures: a draft may be wrong without being unreadable, and this test is what
    /// keeps the gap list legible while the rows are in flux.
    /// </summary>
    [Fact]
    public void The_real_skill_parses_even_while_its_rows_are_a_draft()
    {
        var doc = SkillReader.Read(RealSkillFolder());
        Assert.NotEmpty(doc.Activities);
        Assert.NotEmpty(doc.Processes);
        Assert.NotEmpty(doc.Artifacts);
    }

    static string RealSkillFolder()
    {
        var folder = Path.Combine(FindRepoRoot(), ".claude", "skills", SkillFolderName);
        Assert.True(Directory.Exists(folder), folder);
        return folder;
    }

    static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, ".git")))
            dir = dir.Parent;
        Assert.NotNull(dir);
        return dir!.FullName;
    }
}
