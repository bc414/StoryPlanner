using System.Text;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// The one rendering of a <see cref="ValidationReport"/> as text: findings grouped by level in
/// the order failure, info, vacuous, one line each, then the verdict. The CLI prints it and the
/// write hook hands it back to the session, so both say the same thing about the same rows.
/// </summary>
public static class ReportText
{
    public static string Format(ValidationReport report)
    {
        var sb = new StringBuilder();
        foreach (var group in report.Findings.GroupBy(f => f.Level).OrderBy(g => (int)g.Key))
        {
            sb.AppendLine();
            sb.AppendLine($"== {group.Key.ToString().ToUpperInvariant()} ({group.Count()}) ==");
            foreach (var f in group)
                sb.AppendLine($"{f.CheckId,-32} {f.RowId,-36} {f.Message}");
        }
        sb.AppendLine();
        sb.AppendLine(report.Passed
            ? $"check: passed, {report.Findings.Count} note(s)."
            : $"check: {report.Failures} failure(s).");
        return sb.ToString();
    }

    /// <summary>Failures only, for a message that has to be read in one glance.</summary>
    public static string FormatFailures(ValidationReport report)
    {
        var sb = new StringBuilder();
        foreach (var f in report.Findings.Where(f => f.Level == FindingLevel.Failure))
            sb.AppendLine($"  {f.CheckId}  {f.RowId}  {f.Message}");
        return sb.ToString();
    }
}
