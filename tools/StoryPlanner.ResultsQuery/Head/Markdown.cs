using Markdig;
using Microsoft.AspNetCore.Components;

namespace StoryPlanner.ResultsQuery;

/// <summary>
/// Markdown to markup for the item panel, on the same Markdig the runner's page and Core render
/// with. Line breaks stay where the file has them, so an item's head lines do not run together;
/// raw HTML is off, so a note's own angle brackets show as text rather than becoming markup.
/// </summary>
public static class MarkdownView
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions().UseSoftlineBreakAsHardlineBreak().DisableHtml().Build();

    public static MarkupString Render(string? markdown) =>
        new(string.IsNullOrWhiteSpace(markdown) ? "" : Markdig.Markdown.ToHtml(markdown, Pipeline));
}
