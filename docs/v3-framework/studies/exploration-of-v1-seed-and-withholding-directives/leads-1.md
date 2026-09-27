# exploration-of-v1-seed-and-withholding-directives — leads

- items with results: 379 of 379
- written by: the Claude Code session that ran the autonomous study campaign of 2026-09-26 to 27,
  which planned this study, wrote its directions, built or chose its itemizer and read its pilot,
  and drew these leads from the batch's results; written before the rule that leads are written by
  a fresh session

## Leads

### exploration-of-v1-seed-and-withholding-directives/leads-1/half-the-plot-points-instruct-the-page-and-half-do-not
- lead: Of the archive's 379 TLTT plot points, 181 hold no instruction about what the finished page does; the
  other 198 hold 527 between them, a median of 2 per instructing item and up to 12. Of the 527: 106 lines over
  69 plot points are planting — the page is to put something there for the reader to pick up or work out; 66
  lines over 44 plot points are withholding — the page is not to state something, or the focalizer is not to
  register it; and 357 lines over 167 plot points carry a name of the reader's own for some other instruction
  about the page.
- seen in: plot points of every chapter of the archive
- query:
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives view=health
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives view=terms col=kind top=14
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives where kind~^planting view=cites
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives where kind~^withholding view=cites
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives where kind~"^(?!(planting|withholding)$)" view=cites

### exploration-of-v1-seed-and-withholding-directives/leads-1/withholding-is-usually-the-focalizer-not-the-page
- lead: The 66 withholding directives are mostly not about what the page omits but about what a character in
  the scene fails to register while the reader is given it: "Celestia is to see only her little ponies armed
  with soul-draining rifles and is not to grasp the real cause of the paranoia, though the audience is to
  know it"; "Rarity is to leave believing her hopelessly out of touch, without seeing the fear behind it";
  "Applejack and Twilight are not to grasp why Fleur's and Mali's manes are messed up, so they miss the joke";
  "Applejack is not to register anything odd about the departure", marked "AJ doesn't think twice"; "the
  characters on the page hear only the surface meaning (missing home cooking) and do not register the deeper
  meaning". A smaller number are about the page itself: an account that is to omit the sexual-liberty side of
  the Manehattan parlors; a description that is to leave out the taxed urban population and show only an
  Aquileia grown from royal patronage; a real reason kept back until after the war.
- seen in: synopses and character and codex links of plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives where kind~^withholding sample=10 seed=4 view=list

### exploration-of-v1-seed-and-withholding-directives/leads-1/what-planting-directives-plant
- lead: The 106 planting directives ask the page to seed something the reader is to work out, and what they
  point at is a mix of story structure, character psychology and real-world reading: an early seed that
  monarchy is a bad system, for the later republic storyline; Applejack's impostor syndrome, to be inferred
  from her needing wine to say a hard truth; press posters readable as a side explaining away its own
  failures, "in the manner of British press on the Desert Fox"; a speech the reader is to hear as resembling a
  Hitler speech; a vision for Herzland that is to leave the audience concluding where America should go; an
  exchange about a translator's Equestrian working as a hint toward Chrysalis; a buried newspaper item about a
  griffon alias being arrested, "with the reader recognising the alias though nobody in the story does"; a
  flat delivery leaving the audience with the sense that the Age of Innocence is over.
- seen in: synopses and character and codex links of plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives where kind~^planting sample=8 seed=2 view=list

### exploration-of-v1-seed-and-withholding-directives/leads-1/a-fifth-of-directives-do-not-say-what-they-point-at
- lead: 119 of the 527 directives, over 73 plot points, answer "not given" in the column asking what fact of
  the story's world is to be inferred or kept back: the item instructs the page without naming the thing the
  instruction is about.
- seen in: plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives where points~"^not given" view=cites

### exploration-of-v1-seed-and-withholding-directives/leads-1/what-else-the-items-instruct-about-the-page
- lead: The 357 directives outside the two named kinds were given names of the readers' own, and they cluster:
  characterization (11), point of view (7), framing (7), callback (5), thematic commentary and thematic
  demonstration (7), recontextualization (3), exposition delivery (3), contrast (3), structure, sequencing,
  placement and staging (10 between them), tone of the line (2), satire (2), reveal and revelation (4). The
  kind column's commonest words after planting and withholding are framing (30), tone (30),
  characterization (29), thematic (28), dialogue (20), callback (10).
- seen in: plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives where kind~"^(?!(planting|withholding)$)" view=list
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives view=terms col=kind top=14

### exploration-of-v1-seed-and-withholding-directives/leads-1/the-directives-are-written-as-planning-shorthand-and-parenthetical-asides
- lead: The register column names planning shorthand in 184 of the 527 lines and expository prose in 175; an
  aside in 120 and a parenthetical in 108; a directive in 92 and a note in 87; a question in 41. The word
  "should" appears in 50 of the register values. So the instructions about the page sit in the same boxes as
  the reporting of what happens, mostly as clipped notes and bracketed asides rather than as a labelled layer.
- seen in: plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points answered=379 field=directives view=terms col=register top=16

### exploration-of-v1-seed-and-withholding-directives/leads-1/most-plot-points-never-speak-of-the-reader
- lead: On the separate whole-item question of whether the plot point speaks of the reader or the audience at
  all, 321 of the 379 say it does not and 58 say it does. So the seeding and withholding instructions are
  mostly addressed to the page and to the characters rather than framed in terms of the reader, even where
  what they arrange is a reader's inference.
- seen in: plot points across all chapters
- cites:
  - exploration-of-v1-seed-and-withholding-directives/01-tltt-plot-points/pp-104

## Proposed questions

- The withholding directives are mostly about what a character fails to register while the reader is given it:
  how many of them would the working plan's Character-Reader Perception Gap track hold, and how many sit
  outside it?
- Where a directive does not say what it points at, is the fact recorded elsewhere in the same plot point, or
  nowhere in the archive?
- Of the planting directives that ask the reader to read a real-world parallel — the Desert Fox press, a
  Hitler-like speech, where America should go — does the working plan hold those on its Analogies or
  Allegories tracks?
- Half the plot points hold no instruction about the page: do those differ from the instructing half by
  chapter, or by whether they record a focalizer?
- The working plan's Inference Seeds track holds no notes: which of the 106 planting directives would it take?
