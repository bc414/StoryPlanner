# exploration-of-v1-subject-notes-as-scene-sequences — leads

## Leads

### exploration-of-v1-subject-notes-as-scene-sequences/how-the-subject-passages-divide
- lead: Over 210 of the archive's 217 note-owning subjects the readers cut 3,718 passages — a median of 13
  per subject, 1 at the least and 137 at the most. Of those, 2,439 lines over 205 subjects are statements
  about the subject, 844 lines over 140 subjects are beats of a scene, and 436 lines over 136 subjects
  carry a name of the reader's own. So roughly two passages in three are a statement about the subject and
  one in four or five is a scene beat, and nearly two thirds of the subjects hold at least one beat. 24 of
  the 3,718 lines hold the wrong number of parts.
- seen in: subjects of every kind in the archive — characters, places, world lore, organizations and events
- query:
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages view=health
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages where kind~"statement about the subject" view=cites
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages where kind~"scene beat" view=cites
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages where kind~"^(?!.*(statement about the subject|scene beat))" view=cites

### exploration-of-v1-subject-notes-as-scene-sequences/most-scene-beats-carry-no-year
- lead: Of the 844 scene beats, only 94 over 39 subjects were named as carrying a year, and 613 over 133
  subjects as carrying none; the rest were named otherwise. The year column holds something other than
  "none" in 428 lines over 111 subjects, and what it holds is often not a year at all but a phrase — "after",
  "only", "years", a chapter, a heading — beside the dates that do appear (980, 981, 986, 978, 1007, 1002,
  1000, 1008, 995, 930, 1006, and 109 lines carrying the ALB era marker). So the archive's subject notes
  hold scene sequences whose order is legible while their placement in world time mostly is not.
- seen in: subjects across the archive
- query:
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages where kind~"scene beat with a year" view=cites
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages where kind~"scene beat without" view=cites
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages where year~"^(?!none$)" view=cites
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages view=terms col=year top=18

### exploration-of-v1-subject-notes-as-scene-sequences/a-third-of-subjects-hold-a-note-that-runs-as-an-ordered-sequence
- lead: 138 notes over 76 of the 210 subjects run as a sequence of beats in the order they happen. What
  marks a sequence is one of a few things: a numbered list with headings ("numbered list 1 to 5 with
  headings"); ages and years counting up ("958 ALB, 14 in 972, 15, 22 in 980, 50 in 1008"); connectives
  chaining consequence ("six months later", "Without a central palace", "Without new incoming slaves",
  "After the defeat at Mount Aris"); and stage words ("initially", "eventually gets sharper", "When Henri
  finally understands"). The sequences run from two beats to about five.
- seen in: notes on characters, on organizations and on historical events across the archive
- query:
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=sequences view=cites
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=sequences view=health
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=sequences sample=8 seed=6 view=list

### exploration-of-v1-subject-notes-as-scene-sequences/which-subjects-hold-the-most-scene-beats
- lead: The subjects densest in scene beats are, in order: Moriset Discret's Aquileia/Coltbert Reforms with
  50 beats over 137 passage lines, The True Actions and Motivations of Chrysalis with 36 over 55, Prince
  Blueblood 29 over 43, Minette 26 over 50, Trimmel 22 over 38, the 1st Aquileian Revolution and
  Counterrevolution of 980 22 over 47, Coltbert 21 over 53, The Predator's Dilemma 19 over 80, Rebuilding
  Ain Trotgourait 16 over 24, Réni Ducep 16 over 26. The subjects with the most ordered sequences are
  Statthalter Slave Trade with 7, then Moriset Discret's reforms and Trimmel with 6 each, Rebuilding Ain
  Trotgourait with 5, and Blueblood, Minette, The Predator's Dilemma, Grover IV's Gilded Age and Applejack
  with 4 each.
- seen in: character subjects and event-and-institution subjects of the archive
- cites:
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-248
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-612
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-42
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-44
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-11
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-271
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-28
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-223
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-291
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-45
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-275

### exploration-of-v1-subject-notes-as-scene-sequences/beat-density-by-the-reviews-triage-label
- lead: Read against the review labels in the index's description column, the share of passages that are
  scene beats differs by label: Deferred until chapter notes 9 of 17 lines, over 4 subjects; Deferred for
  Minette's Prequel 103 of 312, over 11 subjects; Deferred for Chrysalis's Prequel 39 of 133, over 10;
  Uncategorized 261 of 1,057, over 20; First Pass, subject notes only 283 of 1,370, over 121; Deferred for
  other reasons 101 of 540, over 29; Deferred for a plot point 42 of 249, over 12; Deferred until EEEE!
  rework 3 of 19; Deferred until Celestia 3 of 21. So the prequel-deferred and chapter-note-deferred
  subjects are the beat-dense ones, and the largest label, First Pass, is the least beat-dense.
- seen in: the review labels in the index's description column
- cites:
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-28
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-45
  - exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects/subject-44

### exploration-of-v1-subject-notes-as-scene-sequences/what-the-readers-named-outside-the-three-kinds
- lead: 436 lines over 136 subjects carry a name the readers invented rather than one of the three asked
  for. The recurring words are: world (85 lines), real (64), design (56), authorial (55), author (43), open
  question (42), commentary (38), note (35), analogy (29), arc (24), planning (23), historical reference
  (21). So beside statements and beats the subject notes hold real-world analogies, commentary on the
  source show, open questions to the author, arc outlines and planning remarks about where to develop
  something.
- seen in: subjects across the archive
- query:
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages where kind~"^(?!.*(statement about the subject|scene beat))" view=terms col=kind top=16

### exploration-of-v1-subject-notes-as-scene-sequences/a-few-subjects-carry-most-of-the-text
- lead: The passage count per subject runs from 1 to 137, with a median of 13. The Predator's Dilemma holds
  48 notes and 80 passages, Moriset Discret's reforms 20 notes and 137 passages, The True Actions and
  Motivations of Chrysalis 35 notes and 55 passages. So a small number of subjects hold a large share of
  the archive's subject-level text, and those are the subjects that also hold the scene sequences.
- seen in: subjects across the archive
- query:
  - rq1 batch=exploration-of-v1-subject-notes-as-scene-sequences/01-archive-subjects answered=210 field=passages view=health

## Proposed questions

- Which of the ordered scene sequences in the v1 archive's subject notes have a plot point in the working
  plan that carries the same beats, and which have none?
- Where a v1 subject note's sequence counts up in years or ages, does the working plan hold those dates on
  a dated track?
- The subjects densest in scene beats are mostly Aquileian and Griffonian institutions rather than
  characters: does the working plan hold that material as plot points, as subjects, or not at all?
- Of the v1 subject notes that are a statement about the subject rather than a beat, how many state
  something the working plan's subject-level tracks now ask for by name?
- Do the v1 subjects the review marked "Deferred until chapter notes" hold sequences that later became
  chapters, and which ones?
