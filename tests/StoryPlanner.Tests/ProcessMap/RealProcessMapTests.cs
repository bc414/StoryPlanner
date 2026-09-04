using System.IO;
using StoryPlanner.ProcessMap;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The one test that reads the real skill folder. Once it is un-skipped, <c>dotnet test</c>
/// pins the method's topology the way <c>PlanIntegrity</c> pins the data's: a row edited into
/// an unreachable file, an ungated write, or a companion nobody routes to fails the build.
/// </summary>
public class RealProcessMapTests
{
    [Fact(Skip = "Expected to fail until the gap review lands (2026-09-04). The rows are an " +
                 "unvalidated draft: governed-by cells still address sections, several files " +
                 "have no consumer, and M.4 writes f.hyp ungated. Un-skip in the router-swap " +
                 "commit of methodology revision 2.")]
    public void The_real_process_map_validates()
    {
        var repoRoot = FindRepoRoot();
        var skillFolder = Path.Combine(repoRoot, ".claude", "skills", "v3-buildout");
        var report = Validator.Validate(repoRoot, skillFolder);

        Assert.True(report.Passed, string.Join("\n", report.Findings
            .Where(f => f.Level == FindingLevel.Failure)
            .Select(f => $"{f.RuleId} {f.RowId} {f.Message}")));
    }

    /// <summary>
    /// The real map parses even while it fails validation. Structure and semantics are separate
    /// failures: a draft may be wrong without being unreadable, and this test is what keeps the
    /// gap list legible while the rows are in flux.
    /// </summary>
    [Fact]
    public void The_real_process_map_parses_even_while_its_rows_are_a_draft()
    {
        var mapPath = Path.Combine(FindRepoRoot(), ".claude", "skills", "v3-buildout", "process-map.md");
        Assert.True(File.Exists(mapPath), mapPath);

        var doc = MapReader.Read(File.ReadAllText(mapPath));
        Assert.NotEmpty(doc.Roots);
        Assert.NotEmpty(doc.Files);
        Assert.NotEmpty(doc.Processes);
        Assert.NotEmpty(doc.Edges);
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
