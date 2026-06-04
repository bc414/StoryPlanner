using System.Text;

namespace StoryPlanner.Core;

public static class SubjectsMarkdownExporter
{
    public static string Build(IEnumerable<SubjectExportData> subjects)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Subjects");
        sb.AppendLine();

        var groups = subjects
            .GroupBy(s => s.SubjectType)
            .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase);

        foreach (var group in groups)
        {
            sb.AppendLine($"## {group.Key}");
            sb.AppendLine();

            foreach (var s in group.OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase))
            {
                sb.AppendLine($"### {s.Name}");
                sb.AppendLine();

                /*sb.AppendLine("| Field | Value |");
                sb.AppendLine("|---|---|");
                sb.AppendLine($"| Abbreviation | {s.Abbreviation} |");
                sb.AppendLine($"| Color | {s.ColorHex} |");
                sb.AppendLine();

                if (!string.IsNullOrWhiteSpace(s.Description))
                {
                    sb.AppendLine(s.Description);
                    sb.AppendLine();
                }*/

                sb.AppendLine("---");
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}

public record SubjectExportData(
    string Name,
    string SubjectType,
    string Description,
    string Abbreviation,
    string ColorHex);
