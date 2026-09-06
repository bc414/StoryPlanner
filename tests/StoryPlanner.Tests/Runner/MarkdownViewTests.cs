using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The process-map page's rendering: Markdig's diagram extension turns a mermaid fence into
/// <c>&lt;pre class="mermaid"&gt;</c>, and the page folds that into a <c>details</c> block so
/// the consumers table and the validation report read first (the drawing itself awaits a
/// ruling). Tier: pure.
/// </summary>
public class MarkdownViewTests
{
    const string Map = "## The whole graph\n\n```mermaid\nflowchart TD\n  a --> b\n```\n\n## Consumers\n\n| artifact | read by |\n|---|---|\n| jobs | round-run |\n";

    [Fact]
    public void A_mermaid_fence_is_folded_into_a_details_block_and_the_tables_stay_tables()
    {
        var html = MarkdownView.RenderWithFoldedDiagrams(Map).Value;
        Assert.Contains("<details class=\"diagram\">", html);
        Assert.Contains("flowchart TD", html);
        Assert.Contains("a --> b", html);                    // the source inside the fold, as the diagram extension wrote it (raw, not escaped)
        Assert.DoesNotContain("<pre class=\"mermaid\">", html);
        Assert.Contains("<table>", html);
        Assert.Contains("<h2 id=\"consumers\">Consumers</h2>", html);

        // The plain renderer (run.md) leaves the fence as the diagram extension wrote it.
        Assert.Contains("<pre class=\"mermaid\">", MarkdownView.Render(Map).Value);
        Assert.Equal("", MarkdownView.RenderWithFoldedDiagrams("  ").Value);
    }
}
