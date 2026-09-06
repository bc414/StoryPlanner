using System.Text.RegularExpressions;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace StoryPlanner.AgentRunner;

/// <summary>Markdown to markup for the process-map page and a run's <c>run.md</c>. Same Markdig the Core project renders with.</summary>
public static class MarkdownView
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    private static readonly Regex MermaidBlock = new(
        "<pre><code class=\"language-mermaid\">(?<src>.*?)</code></pre>",
        RegexOptions.Singleline | RegexOptions.Compiled);

    public static MarkupString Render(string? markdown) =>
        new(string.IsNullOrWhiteSpace(markdown) ? "" : Markdig.Markdown.ToHtml(markdown, Pipeline));

    /// <summary>
    /// The same rendering with each mermaid fence folded into a <c>details</c> block: Markdig
    /// draws no diagrams, and whether the page loads mermaid.js or links the file for them is
    /// Brian's ruling (engineering handoff 2026-09-05, runner item 4). Until then the source is
    /// present and out of the way, and the tables read as tables.
    /// </summary>
    public static MarkupString RenderWithFoldedDiagrams(string? markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown)) return new("");
        var html = Markdig.Markdown.ToHtml(markdown, Pipeline);
        html = MermaidBlock.Replace(html, m =>
            "<details class=\"diagram\"><summary>diagram — mermaid source, drawn where the file is opened in a mermaid-aware viewer</summary>"
            + $"<pre><code class=\"language-mermaid\">{m.Groups["src"].Value}</code></pre></details>");
        return new(html);
    }
}
