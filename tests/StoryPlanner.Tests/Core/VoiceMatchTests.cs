using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The shingle matcher behind voice attribution (WU1.4) — a synthetic corpus of a few dated
/// sources, no database. What is pinned: normalisation is formatting-blind, earliest dated
/// source wins, ties across roles are reported not broken, the ubiquity filter drops shared
/// boilerplate from the denominator, the matched passage is verbatim from the note, and the
/// plan-snapshot index dates a note by the earliest snapshot that contains it.
/// </summary>
public class VoiceMatchTests
{
    private static readonly VoiceSource Model1 = new("gemini:1", "gemini", "model", new DateOnly(2026, 1, 26));
    private static readonly VoiceSource Brian1 = new("gemini:1", "gemini", "brian", new DateOnly(2026, 1, 26));
    private static readonly VoiceSource Model2 = new("gemini:2", "gemini", "model", new DateOnly(2026, 2, 3));
    private static readonly VoiceSource Undated = new("nlm:1#t3", "notebooklm", "model", null);

    private const string Speech =
        "You think you're saving them, Princess? You're just fattening them up. I saw it. I saw what happens when we fold. We don't get mercy; we get used.";

    [Fact]
    public void Tokenize_is_case_and_punctuation_blind_and_keeps_offsets()
    {
        var t = VoiceText.Tokenize("**The Speech:** \"You're\" — right.");
        Assert.Equal(new[] { "the", "speech", "you", "re", "right" }, t.Select(x => x.Text));
        Assert.Equal("The", "**The Speech:** \"You're\" — right."[t[0].Start..t[0].End]);
        Assert.Equal("You", "**The Speech:** \"You're\" — right."[t[2].Start..t[2].End]);
    }

    [Fact]
    public void Verbatim_paste_matches_its_source_whole_despite_formatting()
    {
        var idx = new VoiceIndex(6);
        idx.Add(Model1, "### The Speech\n- **\"" + Speech + "\"**");
        var m = idx.Match("The Speech: " + Speech);
        Assert.NotNull(m.Coverage);
        Assert.True(m.Coverage >= 0.85, $"coverage {m.Coverage}");
        Assert.Equal("gemini:1", m.Origin!.Id);
        Assert.Equal("model", m.Origin.Role);
        Assert.Contains("fattening them up", m.MatchedPassage);
        Assert.False(m.Tie);
    }

    [Fact]
    public void Nothing_in_the_index_means_none_not_short()
    {
        var idx = new VoiceIndex(6);
        idx.Add(Model1, Speech);
        var m = idx.Match("Luna's eyes are wide, pupils dilated. She looks physically sick tonight.");
        Assert.Equal(0, m.Matched);
        Assert.Equal(0.0, m.Coverage);
        Assert.Null(m.Origin);
    }

    [Fact]
    public void Shorter_than_k_words_has_no_coverage()
    {
        var idx = new VoiceIndex(6);
        idx.Add(Model1, Speech);
        var m = idx.Match("You cannot understand!");
        Assert.Equal(0, m.Shingles);
        Assert.Null(m.Coverage);
    }

    [Fact]
    public void Earliest_dated_source_wins_regardless_of_add_order()
    {
        var idx = new VoiceIndex(6);
        idx.Add(Model2, Speech);   // later, added first
        idx.Add(Brian1, Speech);   // earlier
        var m = idx.Match(Speech);
        Assert.Equal("brian", m.Origin!.Role);
        Assert.Equal(new DateOnly(2026, 1, 26), m.Origin.Date);
        Assert.False(m.Tie);
    }

    [Fact]
    public void Undated_source_never_beats_a_dated_one()
    {
        var idx = new VoiceIndex(6);
        idx.Add(Undated, Speech);
        idx.Add(Model2, Speech);
        Assert.Equal("gemini:2", idx.Match(Speech).Origin!.Id);
    }

    [Fact]
    public void Same_date_different_role_same_votes_is_a_tie()
    {
        var idx = new VoiceIndex(6);
        // Two distinct sources, same date, each holding a different half of the note.
        var a = new VoiceSource("aistudio:1#t1", "aistudio", "brian", new DateOnly(2026, 3, 1));
        var b = new VoiceSource("aistudio:1#t2", "aistudio", "model", new DateOnly(2026, 3, 1));
        const string half1 = "the predator is never full and the only way to save the herd";
        const string half2 = "is to injure the predator before it comes back for more food tonight";
        idx.Add(a, half1);
        idx.Add(b, half2);
        var m = idx.Match(half1 + " " + half2);
        // Votes: each half is 13 words → 8 shingles; the bridging shingles match neither.
        Assert.True(m.Tie, $"expected tie; votes {string.Join(",", m.Votes.Select(v => v.Source.Id + ":" + v.Shingles))}");
    }

    [Fact]
    public void Ubiquity_filter_drops_boilerplate_from_the_denominator()
    {
        var idx = new VoiceIndex(6);
        const string boiler = "the lioness of tall tale story plan by brian";
        for (int i = 0; i < 30; i++)
            idx.Add(new VoiceSource($"gemini:{i}", "gemini", "model", new DateOnly(2026, 1, 1).AddDays(i)), boiler + $" extra words number {i} here to vary the tail");
        var note = boiler + " and then some private observation nobody else has typed anywhere";
        var raw = idx.Match(note);
        var filtered = idx.Match(note, maxSources: 10);
        Assert.True(raw.Matched > 0);
        Assert.Equal(0, filtered.Matched);
        Assert.True(filtered.Considered < raw.Considered);
    }

    [Fact]
    public void Matched_passage_is_verbatim_from_the_note_not_the_source()
    {
        var idx = new VoiceIndex(6);
        idx.Add(Model1, Speech.ToUpperInvariant());
        var note = "Applejack's speech: " + Speech + " (my note)";
        var m = idx.Match(note);
        Assert.Contains(m.MatchedPassage, note);
        Assert.StartsWith("You think", m.MatchedPassage);
    }

    // ---- labels: the structural shape of a note's relationship to its sources ----

    private static readonly LabelThresholds T = new(R: 8, G: 6, VerbatimCoverage: 0.90);
    private const string Paste =
        "Applejack's role as General is a performative shell. She delegates the actual strategy to Star Energy and the Aquileians because she is self-aware enough to know she lacks the expertise. However because she is the Element of Honesty the troops rally around her as a figurehead.";

    private static VoiceIndex PasteIndex()
    {
        var idx = new VoiceIndex(6);
        idx.Add(Model1, Paste);
        return idx;
    }

    [Fact]
    public void Whole_paste_is_verbatim()
    {
        var m = PasteIndex().Match(Paste);
        Assert.Equal(VoiceLabel.Verbatim, VoiceLabel.Of(m, T));
        Assert.Equal(1.0, m.TokenCoverage);
    }

    [Fact]
    public void Paste_with_an_interior_phrase_cut_is_edited_not_partial()
    {
        // Cut "to Star Energy and the Aquileians" out of the middle. Every remaining word is
        // still inside some matched window (token coverage stays 1.0), but the windows across
        // the seam are gone (shingle coverage falls) — a deletion, not authored text.
        var cut = Paste.Replace(" to Star Energy and the Aquileians", "");
        var m = PasteIndex().Match(cut);
        Assert.Equal(1.0, m.TokenCoverage);
        Assert.True(m.Coverage < 0.90, $"shingle coverage {m.Coverage}");
        Assert.Equal(0, m.LongestUncoveredWords);
        Assert.Equal(VoiceLabel.EditedPaste, VoiceLabel.Of(m, T));
    }

    [Fact]
    public void Paste_with_an_authored_sentence_around_it_is_framed()
    {
        var framed = "This is the imposter-syndrome beat I want the wine scene to earn and it has to land before the tent. " + Paste;
        var m = PasteIndex().Match(framed);
        Assert.Equal(VoiceLabel.FramedPaste, VoiceLabel.Of(m, T));
        Assert.Equal("leading", m.UncoveredRuns[0].Position);
        Assert.StartsWith("This is the imposter-syndrome beat", m.UncoveredText);
    }

    [Fact]
    public void One_lifted_sentence_inside_an_authored_note_is_a_fragment()
    {
        var note = "Skyfall bought the imperial fleet rather than building fluyts, sold munitions to every side, and taxed the gate. They are a state built on Credit, Gunpowder, and Water. The Dutch parallel is the seven provinces and the eighty years war, which is why the republic never trusts a landward neighbour and keeps its powder dry at sea.";
        var idx = new VoiceIndex(6);
        idx.Add(Model1, "Verdict: the Dutch Republic is the ultimate example of a state built on Credit, Gunpowder, and Water. It is a perfect model.");
        var m = idx.Match(note);
        Assert.True(m.LongestRunWords >= 8, $"run {m.LongestRunWords}");
        Assert.True(m.TokenCoverage < 0.5);
        Assert.Equal(VoiceLabel.Fragment, VoiceLabel.Of(m, T));
    }

    [Fact]
    public void Shared_short_phrasing_is_phrase_never_attribution()
    {
        var note = "Blueblood cuts in over the radio and says he thinks Chrysalis is baiting us on purpose. She isn't a cartoon villain, she's a mastermind pretending to be one to provoke a response.";
        var idx = new VoiceIndex(6);
        idx.Add(Model1, "This is crucial for her depth. She isn't a cartoon villain who hates fun; she is an efficiency extremist.");
        var m = idx.Match(note);
        Assert.True(m.Matched > 0);
        Assert.True(m.LongestRunWords < 8, $"run {m.LongestRunWords}");
        Assert.Equal(VoiceLabel.Phrase, VoiceLabel.Of(m, T));
    }

    [Fact]
    public void Spans_are_split_per_source_with_character_offsets_into_the_note()
    {
        // A note stitched from a model response and Brian's own prompt: two spans, each
        // crediting its source, offsets slicing the original text exactly.
        const string mine = "and this is the sentence I typed into the prompt myself that day";
        var idx = new VoiceIndex(6);
        idx.Add(Model1, Speech);
        idx.Add(new VoiceSource("gemini:9", "gemini", "brian", new DateOnly(2026, 2, 1)), mine);
        var note = Speech + " " + mine;
        var m = idx.Match(note);
        Assert.Equal(2, m.Spans.Count);
        Assert.Equal("model", m.Spans[0].Source.Role);
        Assert.Equal("brian", m.Spans[1].Source.Role);
        Assert.Equal(Speech.TrimEnd('.'), note[m.Spans[0].Start..m.Spans[0].End]);
        Assert.Equal(mine, note[m.Spans[1].Start..m.Spans[1].End]);
    }

    [Fact]
    public void Prompt_beats_the_response_that_repeats_it_on_the_same_date()
    {
        var idx = new VoiceIndex(6);
        idx.Add(Brian1, "My question: " + Speech);          // prompt added first, same date
        idx.Add(Model1, "You wrote: " + Speech + " — yes."); // response repeats it
        var m = idx.Match(Speech);
        Assert.Equal("brian", m.Origin!.Role);
    }

    [Fact]
    public void Snapshot_index_dates_a_note_by_earliest_containing_snapshot()
    {
        var snaps = new PlanSnapshotIndex(6);
        var jan = new PlanSnapshotIndex.Snapshot("2026-01-10", new DateOnly(2026, 1, 10));
        var feb = new PlanSnapshotIndex.Snapshot("2026-02-02", new DateOnly(2026, 2, 2));
        snaps.Add(feb, new[] { "chapter three notes", Speech });   // later snapshot added first
        snaps.Add(jan, new[] { "chapter three notes only" });
        var (verdict, first) = snaps.FirstAppearance(Speech);
        Assert.Equal(PlanSnapshotIndex.Verdict.Found, verdict);
        Assert.Equal("2026-02-02", first!.Name);
        Assert.Equal(PlanSnapshotIndex.Verdict.Absent, snaps.FirstAppearance("text that was never in any snapshot at all").Verdict);
        Assert.Equal(PlanSnapshotIndex.Verdict.Ambiguous, snaps.FirstAppearance("---").Verdict);
    }

    [Fact]
    public void Snapshot_index_uses_substring_search_for_notes_shorter_than_k()
    {
        var snaps = new PlanSnapshotIndex(6);
        snaps.Add(new PlanSnapshotIndex.Snapshot("s", new DateOnly(2026, 1, 1)), new[] { "Luna: You cannot understand! Get out." });
        Assert.Equal(PlanSnapshotIndex.Verdict.Found, snaps.FirstAppearance("you cannot understand get out").Verdict);
        Assert.Equal(PlanSnapshotIndex.Verdict.Absent, snaps.FirstAppearance("you cannot understand me now").Verdict);
    }
}
