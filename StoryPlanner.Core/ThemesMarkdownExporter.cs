using System.Text;

namespace StoryPlanner.Core;

public static class ThemesMarkdownExporter
{
    public static string Build(IEnumerable<ThemeExportData> themes)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Themes");
        sb.AppendLine();

        foreach (var t in themes.OrderBy(t => t.Name, StringComparer.OrdinalIgnoreCase))
        {
            sb.AppendLine($"## {t.Name}");
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(t.Proposition))
            {
                sb.AppendLine(t.Proposition);
                sb.AppendLine();
            }

            sb.AppendLine("---");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}

public record ThemeExportData(string Name, string Proposition);
