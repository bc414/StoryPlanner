# exploration-of-v1-archive-note-relationships-in-prose — leads

- items with results: 379 of 379
- written by: the Claude Code session that ran the autonomous study campaign of 2026-09-26 to 27,
  which planned this study, wrote its directions, built or chose its itemizer and read its pilot,
  and drew these leads from the batch's results; written before the rule that leads are written by
  a fresh session

## Leads

### exploration-of-v1-archive-note-relationships-in-prose/leads-1/how-many-joints-the-prose-carries
- lead: Over the 379 TLTT plot points the readers found 2,644 joints between two boxes of one item — a
  median of 8 per plot point, 1 at the least and 41 at the most. Of those, 1,883 are implicit, carried
  only by what the two boxes say, and 658 are explicit, where one box points at the other in words. So
  the prose carries roughly three implicit joints for every one that names its other end, and no plot
  point was found with none.
- seen in: plot points of every chapter, synopses and character, theme, thread and codex links alike
- query:
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations view=health
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations view=terms col=strength top=8
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where strength~^explicit view=cites

### exploration-of-v1-archive-note-relationships-in-prose/leads-1/what-kinds-of-joint-the-prose-carries
- lead: Under the loose alternations, the commonest joint is one box being an instance or an occasion of
  what another states generally: 600 lines over 273 plot points. Then one box giving the cause or reason
  of what another states, 496 lines over 221 plot points; one continuing another in time, 269 lines over
  163; one revising, narrowing, overturning, correcting or complicating another, 209 lines over 154; one
  planning the delivery on the page of what another states as true, 136 lines over 95; one presupposing
  another and not working without it, 112 lines over 100; two contradicting each other or standing in
  tension, 86 lines over 64; and one stating the reader's response to what another states, only 23 lines
  over 20. The families overlap where a reader named a joint with more than one of these words.
- seen in: plot points across all chapters
- query:
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~"instance|occasion|example|case of|particular" view=cites
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~"cause|reason|because|why|leads to|drives" view=cites
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~"continu|then|after|later|in time|sequel" view=cites
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~revis|narrow|overturn|correct|qualif|complicat view=cites
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~"deliver|on the page|stag|plans the" view=cites
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~"presuppos|depends|requires|does not work without|rests on" view=cites
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~"contradict|tension|conflict|disagree|at odds" view=cites
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~reader view=cites

### exploration-of-v1-archive-note-relationships-in-prose/leads-1/the-synopsis-is-one-end-of-most-joints
- lead: The synopsis is named as one end of the joint in 1,186 of the 2,644 lines and as the other end in
  1,039; a codex entry stands at one end in 428 lines and the other in 604, a character link 479 and 460,
  a thread link 291 and 319, a theme link 217 and 192. So the graph the prose carries is mostly a star
  around the synopsis, with a substantial minority of joints running link-to-link with the synopsis at
  neither end.
- seen in: plot points across all chapters
- query:
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations view=terms col=one top=12
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations view=terms col=other top=12

### exploration-of-v1-archive-note-relationships-in-prose/leads-1/what-an-explicit-joint-looks-like
- lead: An explicit joint is usually carried by a shared proper name or a shared coined term rather than
  by any reference: two links both calling Applejack "Chrysalis's puppet", once for the character and
  once for the doctrine thread; a synopsis and a theme link sharing "poseurs" and "warlords"; a synopsis
  and a codex entry sharing "Marshal Blueblood". Where a joint is a correction it is marked by a phrase —
  "the reality is" against what the audience thinks; where it is a cause it is marked by a stated reason
  — "Henri is not pleased" after a revelation of royalist roots.
- seen in: synopses and character, theme, thread and codex links of plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where strength~^explicit sample=8 seed=11 view=list
- cites:
  - exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points/pp-21
  - exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points/pp-41
  - exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points/pp-186
  - exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points/pp-89
  - exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points/pp-148

### exploration-of-v1-archive-note-relationships-in-prose/leads-1/boxes-that-contradict-each-other-inside-one-plot-point
- lead: 86 lines over 64 plot points join two boxes by contradiction or tension rather than agreement. One
  has the synopsis use Pinkie's cartoon physics for a gag while the theme link says she drops them;
  another has a character link give Celestia's real reason for staying out while the same link records
  what the audience thinks instead. These are joints the prose carries between boxes that were written at
  different times and never reconciled.
- seen in: synopses set against theme and character links, plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~"contradict|tension|conflict|disagree|at odds" view=cites
- cites:
  - exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points/pp-85
  - exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points/pp-89

### exploration-of-v1-archive-note-relationships-in-prose/leads-1/joints-whose-other-end-is-not-in-the-item
- lead: Beside the joints inside an item, the readers found 2,286 places where a box plainly speaks of
  something the item does not hold — a median of 6 per plot point, up to 18. They point at earlier scenes
  ("first time the princesses hear about it", "the dismissal here"), at a character's history not linked
  here (Mali's parloir work, Rarity earning her council seat), at world history a codex entry dates but
  does not carry (the Temberik and their Kurdish proverb, the period from 930 ALB), and at documents and
  exchanges from earlier parts of the story (Coltbert's Predator's Dilemma paper and the Verany–Coltbert
  newspaper exchange). The word "earlier" occurs in 607 of these lines and "elsewhere" in 289.
- seen in: synopses and character, thread, theme and codex links of plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=outward view=health
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=outward view=terms col=points top=22
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=outward sample=8 seed=5 view=list

### exploration-of-v1-archive-note-relationships-in-prose/leads-1/whether-an-items-boxes-read-as-one-design
- lead: Asked whether the item's boxes read as one design or as separate entries, the readers said one
  design for 204 of the 379 plot points and separate for 54; the remaining 121 gave an answer of another
  shape, most often that the item holds a single box — a synopsis with no outcome, stakes or links — so
  there is nothing for its boxes to join. Where the answer was one design it is usually because every box
  describes one event from a different aspect; where it was separate it is usually one link text trailing
  into a note of its own.
- seen in: plot points across all chapters
- cites:
  - exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points/pp-192
  - exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points/pp-20

### exploration-of-v1-archive-note-relationships-in-prose/leads-1/the-reader-is-rarely-one-end-of-a-joint
- lead: Only 23 lines over 20 plot points join two boxes by one stating the reader's response to what the
  other states, the rarest of the eight kinds named in the directions and a fortieth as common as the
  instance-of joint. The v1 boxes join to each other mostly through the world's causation, not through
  what the reader is to do with it.
- seen in: plot points across the chapters
- query:
  - rq1 batch=exploration-of-v1-archive-note-relationships-in-prose/01-tltt-plot-points answered=379 field=relations where kind~reader view=cites

## Proposed questions

- Where a v1 plot point's boxes contradict each other, was one written later than the other, and does the
  working plan carry one of the two?
- Of the joints the v1 prose carries implicitly, how many run between boxes the working plan now keeps in
  different tracks, and how many inside one track?
- What share of a v1 plot point's outward pointers name a scene the archive holds, against lore or history
  the archive holds only as a codex entry?
- Do the v1 plot points whose boxes read as separate entries differ from those that read as one design by
  when they were written, or by which chapter they sit in?
- Where two boxes of a v1 plot point carry the same claim in the same words, which of the working plan's
  tracks would hold each?
