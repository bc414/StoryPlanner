using Markdig;
using Microsoft.AspNetCore.Components;

namespace StoryPlanner.AgentRunner;

/// <summary>Markdown to markup for the protocol page and a run's <c>run.md</c>. Same Markdig the Core project renders with.</summary>
public static class MarkdownView
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
    public static MarkupString Render(string? markdown) =>
        new(string.IsNullOrWhiteSpace(markdown) ? "" : Markdig.Markdown.ToHtml(markdown, Pipeline));
}
