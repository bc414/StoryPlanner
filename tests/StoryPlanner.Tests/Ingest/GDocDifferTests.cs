using StoryPlanner.GDocHistory;
using Xunit;

namespace StoryPlanner.Tests;

public class GDocDifferTests
{
    [Fact]
    public void Single_added_line_produces_one_addition()
    {
        var old = "Line one\nLine two\nLine three";
        var @new = "Line one\nLine two\nNew line\nLine three";
        var result = GDocDiffer.ComputeDiff(old, @new, "2025-04-18", "2025-04-28");
        Assert.Equal(1, result.LinesAdded);
        Assert.Equal(0, result.LinesRemoved);
        Assert.Contains("New line", result.FormattedDiff);
    }

    [Fact]
    public void Replaced_line_counts_as_one_add_and_one_remove()
    {
        var old = "Line one\nOld line\nLine three";
        var @new = "Line one\nReplaced line\nLine three";
        var result = GDocDiffer.ComputeDiff(old, @new, "2025-04-18", "2025-04-28");
        Assert.Equal(1, result.LinesAdded);
        Assert.Equal(1, result.LinesRemoved);
        Assert.Contains("Replaced line", result.FormattedDiff);
        Assert.Contains("Old line", result.FormattedDiff);
    }

    [Fact]
    public void Context_lines_bracket_changes()
    {
        var old = "A\nB\nC\nD\nE\nF\nG";
        var @new = "A\nB\nC\nX\nE\nF\nG";
        var result = GDocDiffer.ComputeDiff(old, @new, "2025-04-18", "2025-04-28");
        Assert.Contains("C", result.FormattedDiff);
        Assert.Contains("E", result.FormattedDiff);
    }

    [Fact]
    public void Identical_texts_produce_zero_changes()
    {
        var text = "No changes here\nSecond line";
        var result = GDocDiffer.ComputeDiff(text, text, "2025-04-18", "2025-04-28");
        Assert.Equal(0, result.LinesAdded);
        Assert.Equal(0, result.LinesRemoved);
        Assert.Contains("(no changes)", result.FormattedDiff);
    }

    [Fact]
    public void Search_term_in_added_line_is_findable()
    {
        var old = "The kingdom had no military";
        var @new = "The kingdom had no military\nGriffonia had modern rifles for 50 years";
        var result = GDocDiffer.ComputeDiff(old, @new, "2025-04-18", "2025-04-28");
        Assert.Contains("Griffonia", result.FormattedDiff);
    }

    [Fact]
    public void Header_includes_both_dates()
    {
        var result = GDocDiffer.ComputeDiff("a", "b", "2025-04-18", "2025-04-28");
        Assert.Contains("2025-04-28", result.FormattedDiff);
        Assert.Contains("2025-04-18", result.FormattedDiff);
    }

    [Fact]
    public void Header_includes_byte_sizes_when_provided()
    {
        var result = GDocDiffer.ComputeDiff("a", "ab", "2025-04-18", "2025-04-28",
            oldBytes: 22407, newBytes: 32594);
        Assert.Contains("32,594", result.FormattedDiff);
        Assert.Contains("22,407", result.FormattedDiff);
    }

    [Fact]
    public void Added_lines_are_prefixed_with_plus()
    {
        var old = "Before";
        var @new = "Before\nAdded line";
        var result = GDocDiffer.ComputeDiff(old, @new, "2025-04-18", "2025-04-28");
        Assert.Contains("+ Added line", result.FormattedDiff);
    }

    [Fact]
    public void Removed_lines_are_prefixed_with_minus()
    {
        var old = "Keep\nRemove me\nKeep too";
        var @new = "Keep\nKeep too";
        var result = GDocDiffer.ComputeDiff(old, @new, "2025-04-18", "2025-04-28");
        Assert.Contains("- Remove me", result.FormattedDiff);
    }

    [Fact]
    public void Heading_tracking_reports_nearest_heading()
    {
        var old = "Intro\n\nOCs\nMali description\nComet description";
        var @new = "Intro\n\nOCs\nMali description\nNew OC added\nComet description";
        var result = GDocDiffer.ComputeDiff(old, @new, "2025-04-18", "2025-04-28");
        Assert.Contains("under: OCs", result.FormattedDiff);
    }

    [Fact]
    public void Document_top_heading_when_change_is_before_any_heading()
    {
        var old = "In the disastrous opening of the Great War, Celestia abandons the defense of Tall Tale.";
        var @new = "In the disastrous opening of the Great War, Celestia abandons the defense of Tall Tale.\nCan Applejack save the city?";
        var result = GDocDiffer.ComputeDiff(old, @new, "2025-04-18", "2025-04-28");
        Assert.Contains("(document top)", result.FormattedDiff);
    }

    [Fact]
    public void Multiple_change_sections_produce_separate_blocks()
    {
        var old = "A\nB\nC\nD\nE\nF\nG\nH\nI\nJ\nK\nL\nM\nN\nO\nP";
        var @new = "A\nX\nC\nD\nE\nF\nG\nH\nI\nJ\nK\nL\nM\nY\nO\nP";
        var result = GDocDiffer.ComputeDiff(old, @new, "2025-04-18", "2025-04-28");
        Assert.Equal(2, result.LinesAdded);
        Assert.Equal(2, result.LinesRemoved);
        var sectionCount = result.FormattedDiff.Split("--- under:").Length - 1;
        Assert.True(sectionCount >= 2, $"Expected at least 2 sections, got {sectionCount}");
    }
}
