using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace StoryPlanner.GDocHistory;

public static class GDocDiffer
{
    public sealed record DiffResult(string FormattedDiff, int LinesAdded, int LinesRemoved);

    private const int ContextLines = 3;

    public static DiffResult ComputeDiff(string oldText, string newText, string fromDate, string toDate,
        int oldBytes = 0, int newBytes = 0)
    {
        var diff = InlineDiffBuilder.Diff(oldText, newText);

        int added = 0, removed = 0;
        foreach (var line in diff.Lines)
        {
            if (line.Type == ChangeType.Inserted) added++;
            else if (line.Type == ChangeType.Deleted) removed++;
        }

        if (added == 0 && removed == 0)
        {
            var header = $"# Changes: {toDate} (from {fromDate})";
            var sizeNote = oldBytes > 0 ? $"\n0 lines changed / {newBytes:N0} bytes (was {oldBytes:N0})" : "\n0 lines changed";
            return new DiffResult($"{header}{sizeNote}\n\n(no changes)", 0, 0);
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"# Changes: {toDate} (from {fromDate})");
        if (oldBytes > 0)
            sb.AppendLine($"+{added} lines / -{removed} lines / {newBytes:N0} bytes (was {oldBytes:N0})");
        else
            sb.AppendLine($"+{added} lines / -{removed} lines");

        var lines = diff.Lines;
        var headingTracker = new HeadingTracker();
        var sections = IdentifySections(lines, headingTracker);

        foreach (var section in sections)
            RenderSection(sb, section, lines, headingTracker);

        return new DiffResult(sb.ToString().TrimEnd(), added, removed);
    }

    private static List<(int Start, int End, string Heading, int NearLine)> IdentifySections(
        IReadOnlyList<DiffPiece> lines, HeadingTracker headingTracker)
    {
        var sections = new List<(int Start, int End, string Heading, int NearLine)>();
        int? sectionStart = null;
        string sectionHeading = "";
        int sectionNearLine = 0;
        int originalLineNumber = 0;

        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];

            if (line.Type != ChangeType.Deleted)
                originalLineNumber++;

            if (line.Type == ChangeType.Unchanged)
                headingTracker.Consider(line.Text, originalLineNumber);

            bool isChanged = line.Type is ChangeType.Inserted or ChangeType.Deleted or ChangeType.Modified;

            if (isChanged && sectionStart == null)
            {
                sectionStart = i;
                sectionHeading = headingTracker.CurrentHeading;
                sectionNearLine = originalLineNumber;
            }

            if (sectionStart != null)
            {
                int gapFromLastChange = 0;
                for (int j = i + 1; j < lines.Count && j <= i + ContextLines * 2 + 1; j++)
                {
                    if (lines[j].Type is ChangeType.Inserted or ChangeType.Deleted or ChangeType.Modified)
                    {
                        gapFromLastChange = -1;
                        break;
                    }
                    gapFromLastChange++;
                }

                bool isLastLine = i == lines.Count - 1;
                bool nextIsUnchangedGap = !isChanged && gapFromLastChange > ContextLines * 2;
                bool noMoreChanges = gapFromLastChange >= 0 && i + gapFromLastChange >= lines.Count - 1;

                if (isLastLine || nextIsUnchangedGap || (noMoreChanges && !isChanged))
                {
                    sections.Add((sectionStart.Value, i, sectionHeading, sectionNearLine));
                    sectionStart = null;
                }
            }
        }

        if (sectionStart != null)
            sections.Add((sectionStart.Value, lines.Count - 1, sectionHeading, sectionNearLine));

        return sections;
    }

    private static void RenderSection(System.Text.StringBuilder sb,
        (int Start, int End, string Heading, int NearLine) section,
        IReadOnlyList<DiffPiece> lines, HeadingTracker headingTracker)
    {
        int contextBefore = Math.Max(0, section.Start - ContextLines);
        int contextAfter = Math.Min(lines.Count - 1, section.End + ContextLines);

        sb.AppendLine();
        sb.AppendLine($"--- under: {section.Heading}, near line {section.NearLine} ---");

        for (int i = contextBefore; i <= contextAfter; i++)
        {
            var line = lines[i];
            var prefix = line.Type switch
            {
                ChangeType.Inserted => "+ ",
                ChangeType.Deleted => "- ",
                _ => "  "
            };
            sb.AppendLine($"{prefix}{line.Text}");
        }
    }

    private sealed class HeadingTracker
    {
        public string CurrentHeading { get; private set; } = "(document top)";
        private string _prevLine = "";

        public void Consider(string lineText, int lineNumber)
        {
            var trimmed = (lineText ?? "").Trim();
            if (trimmed.Length == 0) { _prevLine = ""; return; }
            if (_prevLine.Length == 0 && IsHeading(trimmed))
                CurrentHeading = trimmed.TrimEnd(':');
            _prevLine = trimmed;
        }

        private static bool IsHeading(string trimmed)
        {
            if (trimmed.Length > 60) return false;
            if (trimmed.StartsWith('"')) return false;
            if (trimmed.StartsWith("- ") || trimmed.StartsWith("* ")) return false;

            if (trimmed.StartsWith('#')) return true;
            if (trimmed.StartsWith("Arc ", StringComparison.Ordinal) && trimmed.Contains(':')) return true;
            if (trimmed.EndsWith("Backstory", StringComparison.Ordinal)) return true;
            if (trimmed.EndsWith("Development", StringComparison.Ordinal)) return true;

            if (trimmed.Length <= 30 && char.IsUpper(trimmed[0])
                && !trimmed.Contains('.') && !trimmed.Contains(',')
                && !trimmed.Contains(';') && !trimmed.Contains('!') && !trimmed.Contains('?')
                && trimmed.Split(' ').Length <= 4)
                return true;

            return false;
        }
    }
}
