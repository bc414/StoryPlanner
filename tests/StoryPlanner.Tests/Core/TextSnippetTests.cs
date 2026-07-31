using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure tier — TextSnippet.Around is a plain string function, no .storyplan needed.
/// </summary>
public class TextSnippetTests
{
    [Fact]
    public void Match_at_the_very_start_has_no_leading_ellipsis()
    {
        var text = "Chrysalis leads the changeling hive from beneath Skyfall.";

        var snippet = TextSnippet.Around(text, 0, "Chrysalis".Length, contextChars: 20);

        Assert.False(snippet.StartsWith('…'));
        Assert.StartsWith("Chrysalis", snippet);
    }

    [Fact]
    public void Match_at_the_very_end_has_no_trailing_ellipsis()
    {
        var text = "The hive is ruled by Chrysalis";
        var matchIndex = text.IndexOf("Chrysalis", StringComparison.Ordinal);

        var snippet = TextSnippet.Around(text, matchIndex, "Chrysalis".Length, contextChars: 20);

        Assert.False(snippet.EndsWith('…'));
        Assert.EndsWith("Chrysalis", snippet);
    }

    [Fact]
    public void Match_in_the_middle_gets_both_ellipses()
    {
        var text = new string('a', 100) + "NEEDLE" + new string('b', 100);
        var matchIndex = text.IndexOf("NEEDLE", StringComparison.Ordinal);

        var snippet = TextSnippet.Around(text, matchIndex, "NEEDLE".Length, contextChars: 20);

        Assert.StartsWith("…", snippet);
        Assert.EndsWith("…", snippet);
        Assert.Contains("NEEDLE", snippet);
    }

    [Fact]
    public void Whitespace_runs_collapse_to_a_single_space()
    {
        var text = "line one\r\n\r\n   line   two with NEEDLE inside";
        var matchIndex = text.IndexOf("NEEDLE", StringComparison.Ordinal);

        var snippet = TextSnippet.Around(text, matchIndex, "NEEDLE".Length, contextChars: 60);

        Assert.DoesNotContain("  ", snippet);
        Assert.DoesNotContain("\n", snippet);
        Assert.DoesNotContain("\r", snippet);
    }

    [Fact]
    public void Text_shorter_than_the_window_is_returned_whole_with_no_ellipses()
    {
        var text = "short NEEDLE text";

        var snippet = TextSnippet.Around(text, text.IndexOf("NEEDLE", StringComparison.Ordinal), "NEEDLE".Length, contextChars: 1000);

        Assert.Equal("short NEEDLE text", snippet);
    }

    [Fact]
    public void Empty_text_returns_empty()
    {
        Assert.Equal(string.Empty, TextSnippet.Around("", 0, 0));
    }

    [Fact]
    public void Context_chars_clamps_the_window_size()
    {
        var text = new string('a', 500) + "NEEDLE" + new string('b', 500);
        var matchIndex = text.IndexOf("NEEDLE", StringComparison.Ordinal);

        var narrow = TextSnippet.Around(text, matchIndex, "NEEDLE".Length, contextChars: 10);
        var wide = TextSnippet.Around(text, matchIndex, "NEEDLE".Length, contextChars: 400);

        Assert.True(narrow.Length < wide.Length);
    }
}
