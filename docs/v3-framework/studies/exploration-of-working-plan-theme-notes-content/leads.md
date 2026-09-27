# exploration-of-working-plan-theme-notes-content — leads

## Leads

### exploration-of-working-plan-theme-notes-content/kinds-of-claim-across-the-theme-tagged-notes
- lead: The readers cut the 105 theme-tagged notes into 281 claims, a median of 3 per note and 8 at the
  most. 149 claims over 78 notes are design commitments — something the author is settling as so. 85
  claims over 65 notes are readings — what some given fact means. 30 claims over 27 notes are
  restatements of the theme's own proposition or of a note beside them. 17 claims over 16 notes carry a
  name of the reader's own (general claim, thesis, analogy, maxim, illustration, stated assertion).
  Design commitments outnumber readings by three to two, and 78 of the 105 notes hold at least one.
- seen in: theme-tagged notes across the working plan, on subjects, on scene links and on one chapter
- query:
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims view=health
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims where kind~"^design commitment$" view=cites
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims where kind~^reading$ view=cites
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims where kind~^restatement$ view=cites
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims where kind~"^(?!(design commitment|reading|restatement)$)" view=cites
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims view=terms col=kind top=14

### exploration-of-working-plan-theme-notes-content/claims-answering-a-question-their-own-track-does-not-ask
- lead: Asked whether each claim answers the question its note's own track asks, the readers said yes for
  64 claims over 45 notes, no for 71 claims over 44 notes, and partly for 146 claims over 88 notes. The
  71 that answer something else are dominated by world questions rather than thematic ones: how a spell
  works in the world, where a technology comes from, how many basic rules the world has, whether an
  effect can drive a spell alone, how the republic's magical output changes over time, what a culture
  suppresses in sex and why, who runs the salons and what they do for their clients, what had to happen
  for the revolutions to succeed and which institutions did it, what happens when the Patriotten reach
  the corporate seat of power, which faction's outlook the ruthless form comes from.
- seen in: theme-tagged notes on technologies, organizations, civilizational systems and world laws
- query:
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims where fits~^yes view=cites
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims where fits~^no view=cites
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims where fits~^partly view=cites
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims where kind~"^design commitment$" where fits~^no sample=14 seed=3 view=list

### exploration-of-working-plan-theme-notes-content/how-the-notes-stand-to-the-theme-they-carry
- lead: On the whole-note question of how each note stands to the theme it is tagged with: 41 supply
  evidence for the proposition without arguing it, 20 argue the proposition, 18 name the theme or its
  territory without arguing it, 9 do not touch the theme at all, 3 were read as holding no text of their
  own, and 14 gave an answer of another shape (evidence for one half of the proposition only, evidence
  in compressed form, touching the theme by implication, voicing the ethical half, thin evidence). So a
  fifth of the theme-tagged notes argue their theme, two fifths supply evidence for it without arguing
  it, and a tenth do not touch it.
- seen in: theme-tagged notes across the working plan
- cites:
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-1036
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-2478
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-2553
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-2

### exploration-of-working-plan-theme-notes-content/notes-that-do-not-touch-the-theme-they-are-tagged-with
- lead: 9 theme-tagged notes were read as not touching their theme at all: the tag is on the note and the
  note says nothing about the proposition. One, on Applejack's Characterization track, describes her
  starting attitude to industry and is tagged with a strength-and-mercy proposition; another states how
  many basic rules the world has; others state a piece of world lore or a character condition. The tag
  in these cases is the only thing joining the note to the theme.
- seen in: theme-tagged notes on characters, world laws and civilizational systems
- cites:
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-1078
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-1126
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-1694
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-1715
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-1724
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-1852
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-1864
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-2
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-2149

### exploration-of-working-plan-theme-notes-content/notes-that-restate-their-theme-or-a-neighbour
- lead: 30 claims over 27 notes restate what the theme's own proposition, or a note beside them, already
  says and add nothing. In two the note's whole text is a near-restatement of the proposition on the
  line above it in the item — one reads "The family and material conditions you grew up in determines
  the civilization you build, not personal virtue or vice" against a proposition reading "material
  conditions drive morality, not moral preaching or being taught or told"; the other states that having
  enemies motivates but falls short against conscience-driven solidarity, against a proposition that
  conscience and genuine cooperation are more powerful than extraction.
- seen in: theme-tagged notes on characters, scene links and world laws
- query:
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims where kind~^restatement$ view=cites
- cites:
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-2539
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-2469

### exploration-of-working-plan-theme-notes-content/which-tracks-the-theme-tag-sits-in
- lead: Read against the index's description column, the 105 theme-tagged notes sit in nine tracks:
  Themes 41 and Theme Plan 41 between them carry four fifths, then Scene Theme Evidence 10, Scene Thesis
  4, Reader Prior Belief Clash 4, Scene Thematic Evidence 2, and one each in Theme Evidence, Gap Meaning
  and Characterization. Eight of the nine are ThematicEvidence tracks; Characterization is not, and the
  one note in it was read as not touching its theme.
- seen in: the track names in the index's description column
- query:
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims view=health
- cites:
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-2

### exploration-of-working-plan-theme-notes-content/an-empty-note-carrying-a-theme-tag
- lead: One theme-tagged note has no text at all. The tag, the track and the owner are recorded and the
  note itself is blank.
- seen in: one theme-tagged note in the working plan
- cites:
  - exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes/note-398

### exploration-of-working-plan-theme-notes-content/the-questions-the-claims-answer
- lead: The readers were asked to write, for each claim, the question it answers in one clause of their
  own words. Over 281 claims the column holds 794 distinct words: "what" opens 207 of the lines, "how"
  41, "why" 32, "who" 19, "when" 13, "where" 10. The recurring subjects are technology (21 lines),
  system (20), world (19), story (15), power (10), republic (10) and change (9); "theme" appears in only
  12 of the 281. So even in the plan's theme tracks, the question a claim answers is mostly a question
  about the world and its mechanisms rather than about a thematic proposition.
- seen in: theme-tagged notes across the working plan
- query:
  - rq1 batch=exploration-of-working-plan-theme-notes-content/01-theme-tagged-notes answered=105 field=claims view=terms col=question top=26

## Proposed questions

- Where a theme-tagged note answers a question about the world's mechanisms rather than about its theme,
  does the plan hold a subject-level track whose display question it answers instead?
- Of the working plan's theme-tagged notes that argue their proposition, how many sit in a scene-link
  track and how many on a subject, and is the argument the same kind in each?
- Which of the plan's twelve themes have theme-tagged notes that argue their proposition, and which have
  only notes that name or supply evidence for it?
- Does the working plan hold design commitments about the world that exist only inside a theme-tagged
  note, so that removing the theme tracks would lose them?
- How many of the working plan's theme-tagged notes restate the proposition of the theme they are tagged
  with, and is the restatement in the same words?
