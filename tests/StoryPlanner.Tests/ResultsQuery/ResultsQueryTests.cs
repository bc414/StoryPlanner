using StoryPlanner.BatchFiles;
using StoryPlanner.ResultsQuery;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The query engine over an exploration batch's results, on inline directions and results,
/// no filesystem: the columns a field's text declares, the query string's round trip, and each
/// view's counts done by hand. Tier: Pure.
/// </summary>
public class ResultsQueryTests
{
    const string Directions = """
        ---
        questions: questions/x
        ---

        ## What you are given

        One chapter.

        ## How to read

        Read it.

        ## What to produce

        - moments: list of line, one per moment, three parts separated by a bar: technique, the way as named in any story | the experience it produces | what the moment does to what the reader knows; a part with nothing in it is the word none
        - unplaced: list of line, one per thing: what it is | what marks it | where in the chapter, in words; empty if none
        - summary: line, one line
        """;

    static readonly DirectionsFile Dir = DirectionsFile.Parse(Directions);

    static LoadedBatch Batch(params (string Item, string Locator, string? Text)[] rows)
        => LoadedBatch.Read("exploration-of-x/01-chapters", Dir, rows.Select(r => (new IndexRow(r.Item, r.Locator, "", 1), r.Text)));

    static string Result(params string[] moments)
        => "- moments:\n" + string.Concat(moments.Select(m => "  - " + m + "\n")) + "- unplaced:\n- summary: s\n";

    static LoadedBatch Sample() => Batch(
        ("a-ch01", "a#1", Result(
            "free indirect discourse | warmth | establishes Twilight's home",
            "comic dialogue | light amusement | none",
            "italicized interior monologue | intimacy | reveals Dash's jealousy")),
        ("a-ch02", "a#2", Result(
            "expository dialogue | curiosity | establishes the town's history",
            "section break | none | none, purely tonal")),
        ("b-ch01", "b#1", Result(
            "dramatic irony | comic dread | reveals what Rarity does not know",
            "dialogue with an animal | comic sympathy")),   // two parts: an arity problem, kept
        ("b-ch02", "b#2", null),                                     // no successful call
        ("c-ch01", "c#1", "- moments:\n- unplaced:\n"));              // malformed: summary missing

    [Fact]
    public void Columns_come_from_the_text_after_the_colon_with_short_ids_and_positions()
    {
        var fields = PartField.Of(Dir);
        Assert.Equal(["moments", "unplaced"], fields.Select(f => f.Key));
        var m = fields[0];
        Assert.Equal(["technique", "experience", "moment"], m.Columns.Select(c => c.Id));
        Assert.Equal(["technique", "the experience it produces", "what the moment does to what the reader knows"], m.Columns.Select(c => c.Label));
        Assert.Equal([1, 2, 3], m.Columns.Select(c => c.Position));
        var u = fields[1];
        Assert.Equal(["c1", "marks", "where"], u.Columns.Select(c => c.Id));   // "what it is" has no content word
        Assert.Equal("experience", m.Find("2")!.Id);
        Assert.Equal("experience", m.Find("the experience it produces")!.Id);
        Assert.Null(m.Find("nothing"));
    }

    [Fact]
    public void Duplicate_short_ids_fall_back_to_positions()
    {
        var cols = PartField.ColumnsOf("one per x: what marks it | what marks the end");
        Assert.Equal(["c1", "c2"], cols.Select(c => c.Id));
    }

    [Fact]
    public void Reading_splits_lines_and_counts_answered_malformed_missing_and_arity()
    {
        var b = Sample();
        Assert.Equal(5, b.Items);
        Assert.Equal(3, b.Answered);
        Assert.Equal(["b-ch02"], b.Missing);
        Assert.Single(b.Malformed);
        Assert.Equal(7, b.Lines.Count(l => l.Field == "moments"));
        Assert.Single(b.Arity);
        Assert.Equal(("b-ch01", 2, 3), (b.Arity[0].Item, b.Arity[0].Parts, b.Arity[0].Declared));
        Assert.Equal("", b.Lines.Single(l => l.Item == "b-ch01" && l.Ordinal == 2).Part(3));
        Assert.Equal([("a", 2, 2), ("b", 1, 2), ("c", 0, 1)], b.Stories);
    }

    [Fact]
    public void Query_string_round_trips_and_names_what_is_wrong()
    {
        var text = "rq1 batch=exploration-of-x/01-chapters answered=3 field=moments where technique~dialogue where experience~\"light amuse\\\\d?\" story=a sample=2 seed=7 view=terms col=moment n=2 exclude=twilight top=10";
        var (q, error) = Query.Parse(text);
        Assert.Null(error);
        Assert.Equal(text, q!.Print());
        Assert.Equal("light amuse\\d?", q.Filters[1].Pattern);

        Assert.Equal("a query begins with rq1", Query.Parse("rq2 batch=x").Error);
        Assert.Equal("view 'x' is not one of list, cites, terms, pairs, by-story, sort, health", Query.Parse("rq1 batch=b answered=1 field=f view=x").Error);
        Assert.Equal("view terms needs col=<column>", Query.Parse("rq1 batch=b answered=1 field=f view=terms").Error);
        Assert.Equal("a quote is not closed", Query.Parse("rq1 batch=\"b").Error);
        Assert.Equal("'oops' is not key=value", Query.Parse("rq1 oops").Error);
    }

    [Fact]
    public void Resolve_puts_filters_in_column_order_by_short_id_and_rejects_unknown_columns()
    {
        var b = Sample();
        var q = new Query(b.BatchId, 3, "moments", [new Filter("3", "reveal"), new Filter("technique", "dialogue")], null, null, null, null, "list", null, null, null, null, null, null);
        var (r, error) = q.Resolve(b);
        Assert.Null(error);
        Assert.Equal("rq1 batch=exploration-of-x/01-chapters answered=3 field=moments where technique~dialogue where moment~reveal view=list", r!.Print());

        Assert.Contains("is not a column of moments", new Query(b.BatchId, 3, "moments", [new Filter("kind", "x")], null, null, null, null, "list", null, null, null, null, null, null).Resolve(b).Error);
        Assert.Contains("is not a bar-part field", new Query(b.BatchId, 3, "summary", [], null, null, null, null, "list", null, null, null, null, null, null).Resolve(b).Error);
        Assert.Contains("is not a regex", new Query(b.BatchId, 3, "moments", [new Filter("1", "(")], null, null, null, null, "list", null, null, null, null, null, null).Resolve(b).Error);
    }

    static ViewResult Run(LoadedBatch b, string query)
    {
        var (q, e) = Query.Parse(query);
        Assert.Null(e);
        var (r, error) = Views.Run(b, q!);
        Assert.Null(error);
        return r!;
    }

    [Fact]
    public void List_filters_case_insensitively_and_cites_gives_distinct_items_in_index_order()
    {
        var b = Sample();
        var list = Run(b, "rq1 batch=x answered=3 field=moments where technique~DIALOGUE view=list");
        Assert.Equal(3, list.Rows.Count);
        Assert.Equal(["item", "technique", "experience", "moment"], list.Columns);
        Assert.Equal(["a-ch01", "comic dialogue", "light amusement", "none"], list.Rows[0]);
        Assert.Null(list.SnapshotNote);

        var cites = Run(b, "rq1 batch=x answered=3 field=moments where technique~dialogue view=cites");
        Assert.Equal([["exploration-of-x/01-chapters/a-ch01"], ["exploration-of-x/01-chapters/a-ch02"], ["exploration-of-x/01-chapters/b-ch01"]], cites.Rows);
    }

    [Fact]
    public void Snapshot_mismatch_is_noted_never_refused()
    {
        var r = Run(Sample(), "rq1 batch=x answered=600 field=moments view=list");
        Assert.Equal("this query was run over 600 answered results; the batch now has 3", r.SnapshotNote);
        Assert.Equal(7, r.Rows.Count);
    }

    [Fact]
    public void Terms_count_words_after_the_fixed_normalization()
    {
        var b = Sample();
        var t = Run(b, "rq1 batch=x answered=3 field=moments view=terms col=technique");
        var counts = t.Rows.ToDictionary(r => r[0], r => int.Parse(r[1]));
        Assert.Equal(3, counts["dialogue"]);
        Assert.Equal(1, counts["comic"]);          // "comic dialogue" only: "comic dread" is in the experience column
        Assert.False(counts.ContainsKey("with"));   // stopword
        Assert.False(counts.ContainsKey("an"));

        var possessive = Run(b, "rq1 batch=x answered=3 field=moments view=terms col=moment");
        var c2 = possessive.Rows.ToDictionary(r => r[0], r => int.Parse(r[1]));
        Assert.Equal(1, c2["twilight"]);            // Twilight's → twilight
        Assert.False(c2.ContainsKey("s"));
        Assert.Equal(2, c2["none"]);                // "none" and "none, purely tonal"

        var bigrams = Run(b, "rq1 batch=x answered=3 field=moments view=terms col=technique n=2");
        Assert.Equal("free indirect", bigrams.Rows.First(r => r[0].StartsWith("free")) [0]);
        Assert.Equal("1", bigrams.Rows.First(r => r[0] == "interior monologue")[1]);

        var first = Run(b, "rq1 batch=x answered=3 field=moments view=terms col=moment position=1");
        var c3 = first.Rows.ToDictionary(r => r[0], r => int.Parse(r[1]));
        Assert.Equal(2, c3["establishes"]);
        Assert.Equal(2, c3["reveals"]);
        Assert.Equal(2, c3["none"]);

        var excluded = Run(b, "rq1 batch=x answered=3 field=moments view=terms col=technique exclude=^dialogue$ top=1");
        Assert.Single(excluded.Rows);
        Assert.NotEqual("dialogue", excluded.Rows[0][0]);
    }

    [Fact]
    public void Pairs_count_word_pairs_within_one_value()
    {
        var p = Run(Sample(), "rq1 batch=x answered=3 field=moments view=pairs col=technique");
        var counts = p.Rows.ToDictionary(r => r[0], r => int.Parse(r[1]));
        Assert.Equal(1, counts["comic+dialogue"]);
        Assert.Equal(1, counts["free+indirect"]);
        Assert.False(counts.ContainsKey("dialogue+free"));
    }

    [Fact]
    public void By_story_counts_lines_and_items_and_shows_coverage()
    {
        var r = Run(Sample(), "rq1 batch=x answered=3 field=moments where technique~dialogue view=by-story");
        Assert.Equal([["a", "2", "2", "2/2"], ["b", "1", "1", "1/2"]], r.Rows);
        var all = Run(Sample(), "rq1 batch=x answered=3 field=moments view=by-story");
        Assert.Equal(3, all.Rows.Count);            // an unfiltered by-story lists every story, c with no line
        Assert.Equal(["c", "0", "0", "0/1"], all.Rows[2]);
    }

    [Fact]
    public void Sort_orders_values_case_insensitively_and_sample_is_reproducible_by_seed()
    {
        var s = Run(Sample(), "rq1 batch=x answered=3 field=moments view=sort col=technique");
        Assert.Equal("comic dialogue", s.Rows[0][0]);
        Assert.Equal("section break", s.Rows[^1][0]);

        var one = Run(Sample(), "rq1 batch=x answered=3 field=moments sample=3 seed=5 view=list");
        var two = Run(Sample(), "rq1 batch=x answered=3 field=moments sample=3 seed=5 view=list");
        var other = Run(Sample(), "rq1 batch=x answered=3 field=moments sample=3 seed=6 view=list");
        Assert.Equal(3, one.Rows.Count);
        Assert.Equal(one.Rows, two.Rows);
        Assert.NotEqual(one.Rows, other.Rows);
        Assert.Contains("sample=3 seed=5", one.Query.Print());
    }

    [Fact]
    public void Health_counts_nones_arity_and_prints_the_normalization()
    {
        var h = Run(Sample(), "rq1 batch=x answered=3 field=moments view=health");
        var rows = h.Rows.ToDictionary(r => r[0], r => r[1]);
        Assert.Equal("3", rows["answered"]);
        Assert.Equal("1", rows["missing"]);
        Assert.Equal("1", rows["malformed"]);
        Assert.Equal("7", rows["lines of moments"]);
        Assert.Equal("2/2/3", rows["lines per item (min/median/max)"]);
        Assert.Equal("1", rows["arity problems"]);
        Assert.Equal("none 1, hedged none 1, empty 1", rows["moment (3): what the moment does to what the reader knows"]);
        Assert.Equal("none 1, hedged none 0, empty 0", rows["experience (2): the experience it produces"]);
        Assert.Contains("nothing stemmed or merged", rows["normalization"]);
        Assert.Equal(string.Join(" ", Terms.Stopwords), rows["stopwords"]);
    }

    [Fact]
    public void Render_prints_the_canonical_string_first()
    {
        var r = Run(Sample(), "rq1 batch=x answered=3 field=moments where technique~irony view=cites");
        var lines = r.Render().Split('\n');
        Assert.Equal("rq1 batch=exploration-of-x/01-chapters answered=3 field=moments where technique~irony view=cites", lines[0]);
        Assert.Equal("exploration-of-x/01-chapters/b-ch01", lines[^2]);
    }

    [Fact]
    public void A_bar_with_any_spacing_separates_parts()
    {
        Assert.Equal(["a", "b", "c"], PartField.SplitParts("a|b | c"));
        Assert.Equal(["a", "b"], PartField.SplitParts("  a  |b  "));
        Assert.Equal(["a", "", "b"], PartField.SplitParts("a || b"));
        var b = Batch(("a-ch01", "a#1", Result("comic dialogue|light amusement|none")));
        Assert.Empty(b.Arity);
        Assert.Equal("light amusement", b.Lines[0].Part(2));
    }

    [Fact]
    public void Story_is_the_locator_before_the_group_separator()
    {
        Assert.Equal("a-story", LoadedBatch.StoryOf("a-story#12", "#"));
        Assert.Equal("whole", LoadedBatch.StoryOf("whole", "#"));
        Assert.Equal("x", LoadedBatch.StoryOf("x/3", "/"));
    }
}
