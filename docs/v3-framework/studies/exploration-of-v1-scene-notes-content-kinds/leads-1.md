# exploration-of-v1-scene-notes-content-kinds — leads

## Leads

### exploration-of-v1-scene-notes-content-kinds/how-the-passages-divide-between-the-six-kinds
- lead: Over the 379 TLTT plot points the readers cut 7,020 passages — a median of 14 per plot point, 1 at the
  least and 127 at the most. Of those, 3,220 lines over 373 plot points are a fact of the story's world; 1,117
  over 289 a character's belief or perception; 1,076 over 279 an authorial reading of what something means;
  820 over 257 scene content seen from someone's side; 409 over 166 a fragment of prose drafted as it would
  appear on the page; 291 over 164 a note to self; and 87 over 63 carry a name of the reader's own. So nearly
  every plot point holds a fact of the world, and facts of the world are almost half of all the text.
- seen in: synopses, outcomes, stakes and character, theme, thread and codex links of plot points in every chapter
- query:
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages view=health
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"fact of the world" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"belief or perception" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"authorial reading" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"scene content from a side" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"prose fragment" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"note to self" view=cites

### exploration-of-v1-scene-notes-content-kinds/almost-every-box-holds-more-than-one-kind
- lead: On the separate whole-item question of whether one box — the synopsis, the outcome, the stakes or one
  link's text — holds more than one kind of content, 356 of the 379 plot points say yes and 23 say no. So
  mixing is not the exception in the archive's free-form boxes; a box that holds one kind of content is the
  rarity.
- seen in: synopses, outcomes, stakes and link texts of plot points in every chapter
- query:
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages view=health

### exploration-of-v1-scene-notes-content-kinds/a-quarter-of-the-worlds-facts-are-carried-in-speech-or-a-document
- lead: Of the passages carrying a fact of the story's world, 2,633 state it plainly as true, 812 put it inside
  quoted speech, 103 inside a character's thought and 60 inside a letter, a document, a broadcast or a report.
  So about a quarter of the fabula written into the archive's plot points is written as in-scene delivery
  rather than as a statement of what is true. What arrives that way: a commander reporting force movements and
  the bat-pony town they are heading for; the parloirs telling workers that adulthood should look like wine,
  jazz and debate; how foals come about, given in one character's speech; an invention's use for addiction
  against another's for abundance, "put in Thorax's own words"; a character claiming he took down the nobility
  and instituted absolute meritocracy with methods inspired by another's model; a buyback plan for rifles.
- seen in: synopses above all, and theme, thread and codex links, across the chapters
- query:
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where carried~^plainly view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where carried~"inside speech" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where carried~"inside a thought" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where carried~"inside a letter|inside a document|letter or document" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where carried~"inside speech" sample=8 seed=5 view=list

### exploration-of-v1-scene-notes-content-kinds/a-belief-is-paired-with-the-truth-it-is-wrong-about-a-third-of-the-time
- lead: Of the 1,107 passages that give a character's belief and carry a verdict on the pairing, 366 over 151
  plot points have the truth that belief is wrong about in the same item, and 741 over 248 plot points do not.
  So the belief and its contradicting truth sit together in a third of the cases and apart in two thirds.
- seen in: synopses and character, theme and thread links of plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where paired~"truth present" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where paired~"truth absent" view=cites

### exploration-of-v1-scene-notes-content-kinds/the-point-of-correction-is-named-in-one-belief-in-eight
- lead: Of the 1,107 belief passages, 140 name a scene, a moment or a date at which the character is corrected
  and 967 name none. Almost all the naming happens where the truth is already in the same item: 136 of the 366
  truth-present passages name a correction, against 4 of the 741 truth-absent ones. So where the archive
  records a belief without its truth, it almost never records when the belief is corrected either.
- seen in: synopses and character, theme and thread links of plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where paired~"none named" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where paired~"truth present" view=cites

### exploration-of-v1-scene-notes-content-kinds/drafted-prose-sits-in-the-planning-boxes
- lead: 409 passages over 166 plot points are fragments of prose drafted as they would appear on the page: a
  character's counter-argument in her own words; two lines of what is stitched on a uniform; a block of quoted
  dialogue between two characters after a lesson; a line of dialogue asking for an alternative; a drafted line
  handing the ponies a tool and telling them to handle their business. They sit in the synopsis and in codex
  link text beside the reporting of what happens.
- seen in: synopses and codex links of plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"prose fragment" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"prose fragment" sample=5 seed=1 view=list

### exploration-of-v1-scene-notes-content-kinds/where-the-passages-sit
- lead: The where column names the synopsis in 4,108 of the 7,020 lines and a link in 1,403; a codex entry in
  1,000, a character link in 960, a thread link in 542, a theme link in 356. So three passages in five sit in
  the synopsis and the rest are spread over the four link kinds, with codex entries and character links
  carrying the most.
- seen in: synopses and character, theme, thread and codex links of plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages view=terms col=where top=12

### exploration-of-v1-scene-notes-content-kinds/authorial-readings-are-as-common-as-characters-beliefs
- lead: 1,076 passages over 279 plot points are authorial readings — what some fact or event means, written
  from outside the story — against 1,117 passages of a character's belief or perception over 289 plot points.
  The two are almost equally common and sit in the same boxes, and together they outnumber the 820 passages of
  scene content seen from someone's side.
- seen in: synopses and theme, character and codex links of plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"authorial reading" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"belief or perception" view=cites
  - rq1 batch=exploration-of-v1-scene-notes-content-kinds/01-tltt-plot-points answered=379 field=passages where kind~"scene content from a side" view=cites

## Proposed questions

- Of the facts of the world carried inside speech in the v1 plot points, how many are stated plainly somewhere
  else in the archive, and how many exist only as something a character says?
- Where a belief is recorded without the truth it is wrong about, does the archive hold that truth on the
  subject the belief is about?
- The point of correction is named in one belief passage in eight: for the rest, is the correction recorded as
  a later plot point, or not recorded at all?
- Do the plot points whose boxes hold one kind of content only differ from the rest by length, by chapter, or
  by how many links they carry?
- Of the 409 drafted prose fragments in the v1 plot points, how many survive in the working plan, and on which
  track?
