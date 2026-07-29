using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure tier — string and DTO transforms, no .storyplan and no DbContext.
/// These are cheap to test and easy to break silently during refactors.
/// </summary>
public class PureTransformTests
{
    // ── HtmlToMarkdown (global namespace, regex-based) ───────────────────────

    [Fact]
    public void HtmlToMarkdown_handles_null_and_empty_without_throwing()
    {
        Assert.Equal(string.Empty, HtmlToMarkdown.Convert(""));
        Assert.Equal(string.Empty, HtmlToMarkdown.Convert(null!));
    }

    [Fact]
    public void HtmlToMarkdown_decodes_entities()
    {
        var result = HtmlToMarkdown.Convert("<p>Celestia&#39;s &quot;reforms&quot;</p>");

        Assert.Contains("Celestia's", result);
        Assert.Contains("\"reforms\"", result);
        Assert.DoesNotContain("&#39;", result);
        Assert.DoesNotContain("&quot;", result);
    }

    [Theory]
    [InlineData("<h1>Title</h1>", "# Title")]
    [InlineData("<h2>Sub</h2>", "## Sub")]
    [InlineData("<h3>Deep</h3>", "### Deep")]
    public void HtmlToMarkdown_converts_headers(string html, string expected)
    {
        Assert.Contains(expected, HtmlToMarkdown.Convert(html));
    }

    [Fact]
    public void HtmlToMarkdown_converts_emphasis_and_strips_tags()
    {
        var result = HtmlToMarkdown.Convert("<p><strong>bold</strong> and <em>italic</em></p>");

        Assert.Contains("**bold**", result);
        Assert.Contains("*italic*", result);
        Assert.DoesNotContain("<strong>", result);
        Assert.DoesNotContain("<em>", result);
    }

    // ── Markdown exporters (take flattened DTOs, no service dependency) ──────

    [Fact]
    public void SubjectsExporter_groups_by_type_and_emits_ids()
    {
        var markdown = SubjectsMarkdownExporter.Build([
            new SubjectExportData(1, "Applejack", "Character", "", "AJ", "#f90"),
            new SubjectExportData(2, "Spell Matrices", "Technology", "", "", ""),
            new SubjectExportData(3, "Celestia", "Character", "", "", "")
        ]);

        Assert.Contains("# Subjects", markdown);
        Assert.Contains("## Character", markdown);
        Assert.Contains("## Technology", markdown);
        Assert.Contains("### Applejack (id: 1)", markdown);

        // Alphabetical within a group: Applejack before Celestia.
        Assert.True(markdown.IndexOf("Applejack", StringComparison.Ordinal)
                  < markdown.IndexOf("Celestia", StringComparison.Ordinal));
    }

    [Fact]
    public void SubjectsExporter_emits_names_only_by_design()
    {
        // Description/abbreviation output is deliberately commented out in the exporter —
        // this file feeds subject-name matching, not content. Locking the behavior in so a
        // future "restore the fields" change is a conscious decision, not an accident.
        var markdown = SubjectsMarkdownExporter.Build([
            new SubjectExportData(1, "Applejack", "Character", "A farm pony", "AJ", "#f90")
        ]);

        Assert.DoesNotContain("A farm pony", markdown);
        Assert.DoesNotContain("#f90", markdown);
    }

    [Fact]
    public void ThemesExporter_emits_name_and_proposition()
    {
        var markdown = ThemesMarkdownExporter.Build([
            new ThemeExportData("Strong to be Merciful", "Strength is the prerequisite for mercy.")
        ]);

        Assert.Contains("# Themes", markdown);
        Assert.Contains("## Strong to be Merciful", markdown);
        Assert.Contains("Strength is the prerequisite for mercy.", markdown);
    }

    [Fact]
    public void ThemesExporter_omits_an_empty_proposition_without_emitting_a_blank_line_run()
    {
        var markdown = ThemesMarkdownExporter.Build([new ThemeExportData("Unwritten", "")]);

        Assert.Contains("## Unwritten", markdown);
        Assert.DoesNotContain("\n\n\n\n", markdown);
    }

    // ── Cognitive modes: the LLM-facing semantics of each track type ─────────

    [Fact]
    public void Every_real_track_type_has_a_cognitive_mode()
    {
        foreach (var type in Enum.GetValues<TrackType>().Where(t => t != TrackType.Unset))
        {
            var mode = type.GetCognitiveMode();

            Assert.False(string.IsNullOrWhiteSpace(mode));
            Assert.NotEqual("Unset Notes", mode);
        }
    }

    [Fact]
    public void The_unassigned_track_uses_id_zero_which_EF_never_generates()
    {
        // UnassignedTrack.Definition stands in for "no track" in UI ordering; id 0 is safe
        // precisely because EF's autoincrement starts at 1.
        Assert.Equal(0, UnassignedTrack.Definition.Id);
        Assert.Equal("Unassigned", UnassignedTrack.Definition.TrackName);
        Assert.Equal(int.MaxValue, UnassignedTrack.Definition.ExpansionModeDisplayOrder);
    }

    // ── ConversationMarkdownRenderer ──────────────────────────────────────────

    [Fact]
    public void Render_turns_a_single_newline_into_a_hard_break()
    {
        // ClaudeExportParser joins attachment placeholders and message parts with a single
        // AppendLine() (a markdown soft break). Without UseSoftlineBreakAsHardlineBreak(),
        // Markdig collapses that to a space and the lines run together.
        var html = ConversationMarkdownRenderer.Render(
            "[Attached file: A.md]\n[Attached file: B.md]", "Claude", "user");

        Assert.Contains("<br", html);
    }

    [Fact]
    public void Render_treats_CRLF_the_same_as_LF()
    {
        // Confirmed data has mixed line endings (AppendLine emits \r\n; export text keeps
        // whatever the source used). Guards against a future "just normalize EOLs" change
        // masking a real difference between the two.
        var lf = ConversationMarkdownRenderer.Render("line one\nline two", "Claude", "assistant");
        var crlf = ConversationMarkdownRenderer.Render("line one\r\nline two", "Claude", "assistant");

        Assert.Contains("<br", lf);
        Assert.Contains("<br", crlf);
    }

    [Fact]
    public void Render_still_separates_paragraphs_on_a_blank_line()
    {
        var html = ConversationMarkdownRenderer.Render("first paragraph\n\nsecond paragraph", "Claude", "assistant");

        Assert.Equal(2, html.Split("<p>").Length - 1);
    }

    [Theory]
    [InlineData("user", "user")]
    [InlineData("assistant", "assistant")]
    [InlineData("some-unexpected-sender", "assistant")]
    public void Render_maps_speaker_to_a_body_role_class(string speaker, string expectedClass)
    {
        var html = ConversationMarkdownRenderer.Render("hello", "Claude", speaker);

        Assert.Contains($"<body class='{expectedClass}'>", html);
    }
}
