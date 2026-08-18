using StoryPlanner.Lineage;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure-tier tests for the NotebookLM saved-DOM parser. Fragments here are small synthetic
/// copies of the real capture's structure (Angular output with stable class markers) — the
/// 7MB real capture is exercised by the ingest's dry run, not committed as a fixture.
/// </summary>
public class NlmCaptureParserTests
{
    private static string Pair(string userHtml, string modelHtml) =>
        $"""
        <div class="chat-message-pair ng-star-inserted">
          <chat-message class="individual-message"><div class="from-user-container">
            <div class="mat-mdc-card-content from-user-message-inner-content message-content">{userHtml}</div>
          </div></chat-message>
          <chat-message class="individual-message"><div class="to-user-container">
            <div class="mat-mdc-card-content message-content to-user-message-inner-content">{modelHtml}</div>
            <div class="message-actions"><button _ngcontent-x=""><mat-icon _ngcontent-x="">thumb_up</mat-icon></button>
            <button _ngcontent-x="">Copy</button><button _ngcontent-x="">Save to note</button></div>
          </div></chat-message>
        </div>
        """;

    private static string Page(string body, string title = "Perspective Analysis - Gemini Notebook") =>
        $"<html><head><title>{title}</title><style>.x{{color:red}}</style></head><body>{body}</body></html>";

    [Fact]
    public void Pairs_become_user_and_model_turns_in_order()
    {
        var html = Page(
            Pair("Break down the perspective in Silver", "It uses third-person limited.") +
            Pair("What about Eve?", "Her viewpoint arrives in chapter 12."));

        var capture = NlmCaptureParser.Parse(html);

        Assert.Equal(4, capture.Turns.Count);
        Assert.Equal(["user", "model", "user", "model"], capture.Turns.Select(t => t.Role));
        Assert.Equal([1, 2, 3, 4], capture.Turns.Select(t => t.TurnIndex));
        Assert.Contains("Break down the perspective", capture.Turns[0].Body);
        Assert.Contains("chapter 12", capture.Turns[3].Body);
    }

    [Fact]
    public void Italics_survive_as_markers_and_ui_chrome_text_does_not()
    {
        // Italics are load-bearing for the author's craft analysis — never strip them silently.
        var html = Page(Pair(
            "Explain FID",
            """Free indirect discourse blends <i _ngcontent-x="">her thoughts</i> into <b _ngcontent-x="">narration</b>."""));

        var capture = NlmCaptureParser.Parse(html);
        var model = capture.Turns[1].Body;

        Assert.Contains("*her thoughts*", model);
        Assert.Contains("**narration**", model);
        // Buttons and icon ligature names are chrome, not content.
        Assert.DoesNotContain("thumb_up", model);
        Assert.DoesNotContain("Save to note", model);
        Assert.DoesNotContain("Copy", model);
    }

    [Fact]
    public void A_studio_note_yields_title_and_relative_age_with_an_empty_body()
    {
        var html = Page(
            """
            <artifact-library-note _ngcontent-x="" class="ng-star-inserted">
              <mat-icon _ngcontent-x="">sticky_note_2</mat-icon>
              <span>Literary Blueprints for The Lioness of Tall Tale</span>
              <span>179d ago</span>
              <mat-icon _ngcontent-x="">more_vert</mat-icon>
            </artifact-library-note>
            """);

        var capture = NlmCaptureParser.Parse(html);

        var note = Assert.Single(capture.Notes);
        Assert.Equal("Literary Blueprints for The Lioness of Tall Tale", note.Title);
        Assert.Equal("179d", note.RelativeAge);
        // The panel renders previews — an empty body is the capture's truth, not a defect.
        Assert.Equal("", note.Body);
    }

    [Fact]
    public void The_notebook_title_drops_the_platform_suffix()
    {
        Assert.Equal("Perspective Analysis",
            NlmCaptureParser.Parse(Page("", title: "Perspective Analysis - Gemini Notebook")).Title);
        Assert.Equal("Perspective Analysis",
            NlmCaptureParser.Parse(Page("", title: "Perspective Analysis - NotebookLM")).Title);
    }

    [Fact]
    public void Utf8_punctuation_survives_the_extraction()
    {
        // The 2026-08-13 extraction had CP1252 mojibake ("â€™"); the parser reads decoded
        // strings and must pass typographic characters through untouched.
        var html = Page(Pair("Silver’s perspective — why?", "Because it heightens the reader’s empathy…"));

        var capture = NlmCaptureParser.Parse(html);

        Assert.Contains("Silver’s perspective — why?", capture.Turns[0].Body);
        Assert.Contains("reader’s empathy…", capture.Turns[1].Body);
        Assert.DoesNotContain("â€™", capture.Turns[1].Body);
    }

    [Fact]
    public void List_items_and_entities_render_mechanically()
    {
        var html = Page(Pair(
            "List the modes",
            """<ul _ngcontent-x=""><li _ngcontent-x="">Omniscient &amp; distant</li><li _ngcontent-x="">Limited &lt;deep&gt;</li></ul>"""));

        var model = NlmCaptureParser.Parse(html).Turns[1].Body;

        Assert.Contains("- Omniscient & distant", model);
        Assert.Contains("- Limited <deep>", model);
    }

    [Fact]
    public void An_empty_page_parses_to_nothing_rather_than_throwing()
    {
        var capture = NlmCaptureParser.Parse(Page(""));

        Assert.Empty(capture.Turns);
        Assert.Empty(capture.Notes);
    }
}
