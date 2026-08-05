using StoryPlanner.Core;
using StoryPlanner.Mcp;
using StoryPlanner.SourceTexts;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Fixture-tier tests for the source-text corpus — a real sources.db in a temp dir beside a real
/// synthetic .storyplan, joined the way the tools join them: by (Work name, Part code), never by id.
/// </summary>
public class SourceTextToolsTests : IDisposable
{
    private readonly SyntheticPlan _plan = SyntheticPlan.Create();
    private readonly string _dbPath;

    private const int WorkId = 1;
    private const int EpisodePartId = 1;
    private const int TextlessPartId = 2;
    private const int LongPartId = 3;

    /// <summary>Longer than the 500-char minimum window, so windowing is actually exercised.</summary>
    private static readonly string LongBody = string.Join("\n",
        Enumerable.Range(0, 100).Select(i => $"Narrator: This is line {i:000} of a very long transcript."));

    public SourceTextToolsTests()
    {
        _plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial
            {
                Id = WorkId, Name = "FiM", Description = "MLP:FiM", PartNoun = "Episode", OrderIndex = 0
            });
            ctx.SourceMaterialParts.AddRange(
                new SourceMaterialPart { Id = EpisodePartId, SourceMaterialId = WorkId, Code = "S1E01", Name = "Mare in the Moon", OrderIndex = 0 },
                new SourceMaterialPart { Id = TextlessPartId, SourceMaterialId = WorkId, Code = "Movie", Name = "The Movie", OrderIndex = 1 },
                new SourceMaterialPart { Id = LongPartId, SourceMaterialId = WorkId, Code = "S1E02", Name = "Elements of Harmony", OrderIndex = 2 });
        });

        _dbPath = Path.Combine(Path.GetDirectoryName(_plan.Path)!, "sources.db");
        using var conn = SourceTextDb.OpenWrite(_dbPath);
        SourceTextDb.Replace(conn, ["FiM"], [
            new SourceTextUnit
            {
                WorkName = "FiM", PartCode = "S1E01", UnitLabel = "Mare in the Moon",
                Kind = SourceTextKind.Transcript, OrderIndex = 0,
                Body = "Narrator: Once upon a time in Equestria.\nTwilight: The Elements of Harmony.",
                SourceRef = "https://example/transcript"
            },
            new SourceTextUnit
            {
                WorkName = "FiM", PartCode = "S1E02", UnitLabel = "Elements of Harmony",
                Kind = SourceTextKind.Transcript, OrderIndex = 0,
                Body = LongBody, SourceRef = "https://example/transcript2"
            }
        ]);
    }

    private SourceTextTools Tools() => new(_plan.Sources, new SourceTextStore(_dbPath));

    [Fact]
    public void A_part_with_no_text_is_reported_as_coverage_not_as_a_defect()
    {
        var result = Tools().ListSourceTexts(work: "FiM");

        Assert.Contains("2 of 3 Part(s) have text", result);
        Assert.Contains("(no text)", result);

        // Coverage, never a ranking. Asserting on the absence of words like "recommend" is the
        // wrong shape — the header legitimately contains one, saying it is NOT a recommendation.
        // The structural claim is that Parts come back in the plan's own OrderIndex order, so a
        // textless Part keeps its place instead of being demoted to the end or promoted as a
        // suggestion of what to acquire next.
        var order = result.Split('\n')
            .Select(l => l.Trim())
            .Where(l => l.StartsWith("S1E") || l.StartsWith("Movie"))
            .Select(l => l.Split(' ')[0].TrimEnd(':'))
            .ToList();
        Assert.Equal(["S1E01", "Movie", "S1E02"], order);
    }

    [Fact]
    public void Search_returns_hits_with_the_ids_needed_to_fetch_them()
    {
        var result = Tools().SearchSourceTexts("Elements of Harmony");

        Assert.Contains("FiM·S1E01", result);
        Assert.Contains("sourcetext:", result);
        // The plan-side id, so a hit can be walked back to the Part it cites.
        Assert.Contains($"sourcepart:{EpisodePartId}", result);
        Assert.Contains("1 hit(s)", result);
    }

    [Fact]
    public void Search_scoped_to_a_missing_work_finds_nothing_rather_than_falling_back()
    {
        var result = Tools().SearchSourceTexts("Equestria", work: "P&K");

        Assert.Contains("0 hit(s)", result);
        Assert.DoesNotContain("Once upon a time", result); // no cross-work leakage
    }

    [Fact]
    public void Fetch_by_work_and_part_returns_the_body()
    {
        var result = Tools().GetSourceText(work: "FiM", part: "S1E01");

        Assert.Contains("Once upon a time in Equestria", result);
        Assert.Contains("1 unit(s) returned", result);
    }

    [Fact]
    public void Fetch_windows_long_bodies_and_names_the_offset_to_continue_from()
    {
        // A single fic chapter in the real corpus exceeds 120,000 characters — more than the whole
        // output budget — so the window is load-bearing, not a nicety.
        var first = Tools().GetSourceText(work: "FiM", part: "S1E02", offset: 0, length: 500);

        Assert.Contains("line 000", first);
        Assert.DoesNotContain("line 099", first);
        Assert.Contains("WINDOWED", first);
        Assert.Contains("offset: 500", first);

        // The named offset actually continues where the first window stopped, rather than
        // restarting or skipping — the whole point of reporting it.
        var second = Tools().GetSourceText(work: "FiM", part: "S1E02", offset: 500, length: 500);
        Assert.DoesNotContain("line 000", second);
        Assert.Contains(LongBody.Substring(500, 60), second);
    }

    [Fact]
    public void A_body_that_fits_carries_no_window_notice()
    {
        Assert.DoesNotContain("WINDOWED", Tools().GetSourceText(work: "FiM", part: "S1E01"));
    }

    [Fact]
    public void Fetch_refuses_an_ambiguous_request_rather_than_guessing()
    {
        var result = Tools().GetSourceText(work: "FiM"); // no part, no ids

        Assert.Contains("Refusing to guess", result);
    }

    [Fact]
    public void An_unconfigured_corpus_reports_itself_instead_of_failing()
    {
        // The corpus is optional: the plan's citations still resolve without it.
        var tools = new SourceTextTools(_plan.Sources, new SourceTextStore(null));

        Assert.Contains("No source-text corpus configured", tools.ListSourceTexts());
        Assert.Contains("No source-text corpus configured", tools.SearchSourceTexts("anything"));
    }

    [Fact]
    public void An_invalid_regex_is_an_error_message_not_an_exception()
    {
        Assert.Contains("Invalid regex", Tools().SearchSourceTexts("(unclosed"));
    }

    public void Dispose() => _plan.Dispose();
}
