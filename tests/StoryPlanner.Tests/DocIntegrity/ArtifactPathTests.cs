using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The artifact <c>path</c> cell: one repo-relative pattern, never prose. Nothing is
/// normalised away, and a cell naming two patterns is a syntax error rather than a choice
/// the tool makes for the author.
/// </summary>
public class ArtifactPathTests
{
    const string Studies = "docs/v3-framework/studies";

    static ArtifactPath Parse(string cell)
    {
        Assert.True(ArtifactPath.TryParse(cell, out var path, out var error), error);
        return path!;
    }

    static string Error(string cell)
    {
        Assert.False(ArtifactPath.TryParse(cell, out _, out var error));
        return error!;
    }

    [Fact]
    public void A_bare_pattern_is_the_whole_file()
    {
        var p = Parse($"{Studies}/<study>/candidates.md");
        Assert.Equal($"{Studies}/<study>/candidates.md", p.Pattern);
        Assert.Null(p.Heading);
        Assert.False(p.Frontmatter);
        Assert.False(p.OutsideRepo);
    }

    [Fact]
    public void A_section_sign_names_the_section_of_a_shared_file()
    {
        var p = Parse("docs/v3-framework/hypotheses/NNN-slug.md § Record");
        Assert.Equal("docs/v3-framework/hypotheses/NNN-slug.md", p.Pattern);
        Assert.Equal("Record", p.Heading);
    }

    [Fact]
    public void The_word_frontmatter_names_the_frontmatter_of_a_shared_file()
    {
        var p = Parse("docs/v3-framework/hypotheses/NNN-slug.md frontmatter");
        Assert.Equal("docs/v3-framework/hypotheses/NNN-slug.md", p.Pattern);
        Assert.True(p.Frontmatter);
    }

    [Fact]
    public void Outside_the_repo_is_the_one_non_path_value()
        => Assert.True(Parse("outside the repo").OutsideRepo);

    [Fact]
    public void Two_patterns_joined_by_or_are_a_syntax_error()
        => Assert.Contains("one pattern", Error($"{Studies}/<study>/directions-N.md, or docs/v3-framework/referee/directions-N.md"));

    [Fact]
    public void A_pattern_and_its_tests_is_a_syntax_error()
        => Assert.Contains("one pattern", Error("tools/StoryPlanner.<Name>/ and its tests"));

    [Fact]
    public void A_parenthetical_is_a_syntax_error()
        => Assert.Contains("parenthetical", Error("docs/x.md(note)"));

    [Fact]
    public void An_absolute_path_is_a_syntax_error()
        => Assert.Contains("repo-relative", Error("C:/docs/x.md"));

    [Fact]
    public void A_bare_file_name_is_a_syntax_error_because_there_is_no_search_order()
        => Assert.Contains("no directory", Error("x.md"));

    [Fact]
    public void An_unclosed_placeholder_is_a_syntax_error()
        => Assert.Contains("unclosed", Error($"{Studies}/<study/x.md"));

    [Fact]
    public void A_section_sign_with_nothing_after_it_is_a_syntax_error()
        => Assert.Contains("no heading", Error("docs/x.md §"));

    [Fact]
    public void Study_scope_is_carried_by_the_study_or_batch_placeholder()
    {
        Assert.True(Parse($"{Studies}/<study>/candidates.md").IsStudyScoped);
        Assert.True(Parse($"{Studies}/<study>/batches/<batch>/items/").IsStudyScoped);
        Assert.False(Parse("docs/v3-framework/questions/<corpus>.md").IsStudyScoped);
        Assert.False(Parse("outside the repo").IsStudyScoped);
    }

    [Fact]
    public void A_series_is_numbered_by_N_or_dated_and_a_batch_or_a_hypothesis_id_is_not()
    {
        Assert.True(Parse("docs/v3-framework/methodology-revision-N.md").IsSeries);
        Assert.True(Parse($"{Studies}/<study>/calibration-<date>.md").IsSeries);
        Assert.True(Parse($"{Studies}/<study>/directions-N.md").IsSeries);
        Assert.False(Parse($"{Studies}/<study>/batches/<batch>/results/").IsSeries);
        Assert.False(Parse($"{Studies}/<study>/batches/<batch>/definition.md").IsSeries);
        Assert.False(Parse("docs/v3-framework/hypotheses/NNN-slug.md").IsSeries);
        Assert.False(Parse("tools/StoryPlanner.<Name>/").IsSeries);
        Assert.False(Parse("outside the repo").IsSeries);
    }

    [Fact]
    public void The_regex_binds_the_study_and_leaves_the_rest_as_wildcards()
    {
        var r = Parse($"{Studies}/<study>/directions-N.md").ToRegex("verification-of-x-y");
        Assert.Matches(r, $"{Studies}/verification-of-x-y/directions-2.md");
        Assert.DoesNotMatch(r, $"{Studies}/verification-of-x-y/directions-a.md");
        Assert.DoesNotMatch(r, $"{Studies}/other/directions-1.md");
    }

    [Fact]
    public void A_batch_is_two_digits_a_hyphen_and_a_slug()
    {
        var r = Parse($"{Studies}/<study>/batches/<batch>/definition.md").ToRegex();
        Assert.Matches(r, $"{Studies}/verification-of-x-y/batches/01-sample/definition.md");
        Assert.Matches(r, $"{Studies}/verification-of-x-y/batches/12-full-opus/definition.md");
        Assert.DoesNotMatch(r, $"{Studies}/verification-of-x-y/batches/1-sample/definition.md");
        Assert.DoesNotMatch(r, $"{Studies}/verification-of-x-y/batches/2026-09-20/definition.md");
        Assert.DoesNotMatch(r, $"{Studies}/verification-of-x-y/batches/01-Sample/definition.md");
    }

    [Fact]
    public void NNN_is_three_digits_and_slug_is_anything()
    {
        var r = Parse("docs/v3-framework/hypotheses/NNN-slug.md").ToRegex();
        Assert.Matches(r, "docs/v3-framework/hypotheses/031-dt-classes.md");
        Assert.DoesNotMatch(r, "docs/v3-framework/hypotheses/31-dt.md");
    }

    [Fact]
    public void Dot_star_is_any_extension_and_a_trailing_slash_is_a_directory()
    {
        Assert.Matches(Parse($"{Studies}/<study>/itemize.*").ToRegex("r"), $"{Studies}/r/itemize.py");
        var dir = Parse($"{Studies}/<study>/batches/<batch>/items/");
        Assert.True(dir.IsDirectory);
        Assert.Matches(dir.ToRegex("r"), $"{Studies}/r/batches/01-full/items");
    }

    [Fact]
    public void An_unbound_study_is_a_wildcard()
        => Assert.Matches(Parse($"{Studies}/<study>/candidates.md").ToRegex(), $"{Studies}/anything/candidates.md");

    [Fact]
    public void The_fixed_prefix_is_the_directory_to_enumerate()
    {
        var p = Parse($"{Studies}/<study>/batches/<batch>/items/");
        Assert.Equal($"{Studies}/verification-of-x-y/batches", p.FixedPrefix("verification-of-x-y"));
        Assert.Equal(Studies, p.FixedPrefix());
        Assert.Equal("docs/v3-framework", Parse("docs/v3-framework/studies.md").FixedPrefix());
    }
}
