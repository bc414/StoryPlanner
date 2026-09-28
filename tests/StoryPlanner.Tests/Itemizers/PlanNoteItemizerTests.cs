using System.Linq;
using StoryPlanner.BatchFiles;
using StoryPlanner.Core;
using StoryPlanner.PlanNoteItemizer;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The working-plan note itemizer over the synthetic plan (fixture tier). The flagged notes are
/// the load-bearing case: an item is read by a model, so a flagged note's content must never
/// reach one, as a selected note or as a note beside it. Asserted structurally, by the envelope
/// text only a leaked note would carry and by the absence of its id, never by the secret alone.
/// </summary>
public class PlanNoteItemizerTests
{
    static PlanNotes.Selection All => new("all", []);
    static PlanNotes.Context None => new("none", []);

    [Fact]
    public void Flagged_notes_never_reach_an_item_as_a_selection_or_as_context()
    {
        using var fixture = SyntheticPlan.Create();
        var plan = PlanNotes.Load(fixture.Path);
        Assert.Equal(2, plan.FlaggedLeftOut);

        var selected = PlanNotes.Select(plan, All);
        var noteItems = PlanNotes.NoteItems(plan, selected, new PlanNotes.Context("owner-notes", []));
        var ownerItems = PlanNotes.OwnerItems(plan, selected);
        var subjectItems = PlanNotes.SubjectItems(plan, selected);
        foreach (var item in noteItems.Concat(ownerItems).Concat(subjectItems))
        {
            Assert.DoesNotContain(SyntheticPlan.FlaggedContentEnvelope, item.Body);
            // No heading for either flagged note, in either unit's form ("## note 2", "### note 2 — Backstory").
            Assert.DoesNotMatch($@"\bnote {SyntheticPlan.FlaggedNoteId}\b", item.Body);
            Assert.DoesNotMatch($@"\bnote {SyntheticPlan.FlaggedNoteId2}\b", item.Body);
        }
        Assert.DoesNotContain(noteItems, i => i.Id == $"note-{SyntheticPlan.FlaggedNoteId}");
        Assert.Contains("except the flagged notes", PlanNotes.Narrowing(All, new PlanNotes.Context("owner-notes", [])));
    }

    [Fact]
    public void A_note_item_says_its_owner_track_mode_question_and_date_and_carries_the_named_tracks_beside_it()
    {
        using var fixture = SyntheticPlan.Create();
        var plan = PlanNotes.Load(fixture.Path);
        var selected = PlanNotes.Select(plan, new PlanNotes.Selection("tracks", ["backstory"]));
        Assert.Equal([SyntheticPlan.VisibleNoteId, SyntheticPlan.UnparseableDateNoteId], selected.Select(n => n.Id).ToArray());

        var items = PlanNotes.NoteItems(plan, selected, new PlanNotes.Context("owner-tracks", ["Backstory"]));
        var first = items[0];
        Assert.Equal($"note-{SyntheticPlan.VisibleNoteId}", first.Id);
        Assert.Contains("owner: Character \"Testcharacter\"", first.Body);
        Assert.Contains("track: Backstory — History: written by an in-universe historian reporting facts\n", first.Body);
        Assert.Contains("display question: What is this character's history?", first.Body);
        Assert.Contains("world date: 993", first.Body);
        Assert.Contains($"### note {SyntheticPlan.UnparseableDateNoteId} — Backstory", first.Body);
        // A legacy date that does not convert is carried as its text, never guessed.
        Assert.Contains("world date: unconverted: sometime after the war", items[1].Body);
    }

    [Fact]
    public void Every_types_mode_is_carried_as_its_persona_without_label_or_layer()
    {
        foreach (var type in Enum.GetValues<TrackType>().Where(t => t != TrackType.Unset))
        {
            var persona = PlanNotes.Persona(type);
            Assert.StartsWith("written by ", persona);
            Assert.DoesNotContain("Layer", persona);
            Assert.EndsWith(persona, type.GetCognitiveMode());
        }
    }

    [Fact]
    public void A_theme_tag_selects_the_note_and_is_carried_with_its_proposition()
    {
        using var fixture = SyntheticPlan.Create();
        fixture.ExternalWrite(ctx =>
        {
            ctx.Themes.Add(new Theme { Id = 1, Name = "Strong to be Merciful", Proposition = "Strength is the prerequisite for mercy." });
            ctx.Notes.Find(SyntheticPlan.LinkNoteId)!.ThemeId = 1;
        });
        var plan = PlanNotes.Load(fixture.Path);
        var item = Assert.Single(PlanNotes.NoteItems(plan, PlanNotes.Select(plan, new PlanNotes.Selection("theme-tagged", [])), None));
        Assert.Equal($"note-{SyntheticPlan.LinkNoteId}", item.Id);
        Assert.Contains("theme: Strong to be Merciful — Strength is the prerequisite for mercy.", item.Body);
        // A scene link's owner resolves through its plot point and subject, polymorphically.
        Assert.Contains("owner: scene link \"Testscene\" × \"Testcharacter\" ((Unassigned), chapter 1 \"Testchapter\")", item.Body);
    }

    [Fact]
    public void The_owner_unit_holds_one_item_per_owner_of_every_kind()
    {
        using var fixture = SyntheticPlan.Create();
        var plan = PlanNotes.Load(fixture.Path);
        var items = PlanNotes.OwnerItems(plan, PlanNotes.Select(plan, All));
        Assert.Equal(
            [$"subject-{SyntheticPlan.SubjectId}", $"pp-{SyntheticPlan.PlotPointId}", $"chapter-{SyntheticPlan.ChapterId}", $"link-{SyntheticPlan.LinkId}"],
            items.Select(i => i.Id).ToArray());
        Assert.Contains("track: none (unassigned)", items[1].Body);
        Assert.Empty(ItemizerOutput.Problems(items));
    }

    [Fact]
    public void A_subject_item_holds_its_own_and_its_scene_links_notes_under_every_track_it_can_hold()
    {
        using var fixture = SyntheticPlan.Create();
        fixture.ExternalWrite(ctx => ctx.NoteTrackDefinitions.Add(new NoteTrackDefinition
        {
            Id = 50, SubjectDefinitionId = SyntheticPlan.CharacterDefId, OwnerType = OwnerType.Subject,
            TrackName = "Characterization", DisplayQuestion = "Who is this character?", TrackType = TrackType.Characterization,
        }));
        var plan = PlanNotes.Load(fixture.Path);
        var item = Assert.Single(PlanNotes.SubjectItems(plan, PlanNotes.Select(plan, All)));

        // The subject with no notes has no item; plot-point and chapter notes never reach one.
        Assert.Equal($"subject-{SyntheticPlan.SubjectId}", item.Id);
        Assert.Equal("Testcharacter, 2 subject-wide notes, 1 scene-link notes", item.Description);
        Assert.DoesNotMatch($@"\bnote {SyntheticPlan.PlotPointNoteId}\b", item.Body);
        Assert.DoesNotMatch($@"\bnote {SyntheticPlan.ChapterNoteId}\b", item.Body);

        var body = item.Body;
        var wide = body.IndexOf("## Subject-wide notes", StringComparison.Ordinal);
        var links = body.IndexOf("## Scene-link notes", StringComparison.Ordinal);
        Assert.True(wide >= 0 && links > wide);
        // Every track the subject can hold at its owner is shown, an empty one with its question.
        Assert.Contains("### track: Backstory — History: written by an in-universe historian reporting facts\ndisplay question: What is this character's history?", body);
        Assert.Contains("### track: Characterization — Characterization: written by a psychologist asserting the truth of what makes a character who they are\ndisplay question: Who is this character?\n\n(no notes)", body);
        Assert.InRange(body.IndexOf($"#### note {SyntheticPlan.VisibleNoteId}\n", StringComparison.Ordinal), wide, links);
        Assert.Contains("world date: 993", body);
        // The link track sits under the scene-link notes with the scene named, not the subject track.
        var linkNote = body.IndexOf($"#### note {SyntheticPlan.LinkNoteId}\n", StringComparison.Ordinal);
        Assert.True(linkNote > body.IndexOf("### track: Revelation — WorldInference", links, StringComparison.Ordinal));
        Assert.Contains($"#### note {SyntheticPlan.LinkNoteId}\nscene: \"Testscene\" ((Unassigned), chapter 1 \"Testchapter\")", body);

        Assert.Contains("a subject holding none of the notes taken has no item", PlanNotes.Narrowing(All, None, "subject"));
        Assert.Empty(ItemizerOutput.Problems([item]));
    }

    [Fact]
    public void A_subject_items_untracked_notes_are_carried_as_unassigned()
    {
        using var fixture = SyntheticPlan.Create();
        fixture.ExternalWrite(ctx => ctx.Notes.Add(new Note
        {
            Id = 60, OwnerId = SyntheticPlan.LinkId, OwnerType = OwnerType.PlotPointSubjectLink,
            NoteState = NoteState.Unset, Content = "An untracked link note.", SortOrder = 2,
        }));
        var plan = PlanNotes.Load(fixture.Path);
        var body = Assert.Single(PlanNotes.SubjectItems(plan, PlanNotes.Select(plan, All))).Body;
        var links = body.IndexOf("## Scene-link notes", StringComparison.Ordinal);
        Assert.True(body.IndexOf("### track: none (unassigned)\n\n#### note 60\nscene:", StringComparison.Ordinal) > links);
    }
}
