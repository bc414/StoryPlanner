# exploration-of-narrator-register-or-focalizer-voice — leads

## Leads

### exploration-of-narrator-register-or-focalizer-voice/how-the-renderings-divide-between-the-two-answers
- lead: Over the 74 chapters of the six stories the readers named 1,043 stretches of narration that render
  what a character perceives or judges — a median of 14 per chapter, 7 at the least and 25 at the most. Of
  those, 483 lines over 61 chapters are carried in the character's own words, 230 lines over 51 chapters
  are mixed, and 169 lines over 45 chapters are carried in a narrator's register the character would not
  use. 65 of the 1,043 lines hold more or fewer than the six declared parts, most of them in three
  chapters, so the three counts are a few per cent short of the whole.
- seen in: chapters of all six stories
- query:
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings view=health
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where carrier~"own words" view=cites
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where carrier~^mixed view=cites
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where carrier~narrator view=cites

### exploration-of-narrator-register-or-focalizer-voice/the-answer-differs-by-story-not-by-chapter
- lead: The split is a property of the story, not of the chapter. In to-hone-a-leaf-blade 230 lines are the
  character's own words against 9 mixed and 11 in a narrator's register, and the 11 all fall in one
  chapter. In nine-tales-of-liberty the three are level: 81 own words, 81 mixed, 76 narrator's. In
  the-ember-and-the-spark it is 137, 105 and 60; in green-is-your-color 34, 20 and 15; in
  the-harvest-of-falldale 1, 15 and 7, so that story's narration is mostly mixed.
- seen in: the six stories, read one chapter at a time
- query:
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where carrier~"own words" view=by-story
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where carrier~^mixed view=by-story
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where carrier~narrator view=by-story
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings view=by-story col=carrier

### exploration-of-narrator-register-or-focalizer-voice/one-story-has-no-narrating-voice-apart-from-its-character
- lead: Asked separately whether the chapter has a narrating voice distinct from every character's, the
  readers answered none for all 21 chapters of to-hone-a-leaf-blade and for one chapter of
  the-ember-and-the-spark, and present for the other 52. The reason given for to-hone-a-leaf-blade is the
  same in every chapter: it is first person, and the narration is the focalizing character's own voice
  with no wording, knowledge or judgment beyond hers. So of the six stories, one carries no separate
  narrator at all and five carry one in every chapter but one.
- seen in: the six stories, chapter by chapter
- cites:
  - exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters/to-hone-a-leaf-blade-by-bc414-cwgz7xzg-ch08
  - exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters/green-is-your-color-ch01
  - exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters/wish-by-chlorophyll19-gyqecqhk-ch01
  - exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters/nine-tales-of-liberty-by-bc414-8gqvftrm-ch15
  - exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters/the-ember-and-the-spark-by-bc414-w9hrsy73-ch12

### exploration-of-narrator-register-or-focalizer-voice/what-marks-a-narrators-register
- lead: Where a rendering was called a narrator's words, the marks named are of a few recurring kinds: an
  aside that knows more than the character ("Little did Helena know that most of the dukes did want a
  cowardly king"); a dry or mocking verdict on the character ("Ludwig lied", "diverted his brainpower to
  fantasizing"); a literary reading of a feeling in words the character does not use ("It looked like
  sorrow had overtaken him", set against her blunt speech); an explaining aside giving a reason as common
  sense ("No one wanted flying intruders… after all"); a summing epigram no character speaks ("starvation
  was a much angrier motivator"); descriptive narration of sensation in a register the focalizer does not
  use; and a romance-novel register for a character whose own speech is "Oh shit" and "Geez".
- seen in: chapters of nine-tales-of-liberty, the-ember-and-the-spark, green-is-your-color and wish
- query:
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where carrier~narrator sample=10 seed=4 view=list

### exploration-of-narrator-register-or-focalizer-voice/whether-the-wording-echoes-the-characters-own-speech
- lead: Asked whether wording of the same kind appears in that character's own speech or thought elsewhere
  in the same chapter, the readers answered yes for 717 lines over 70 chapters, no for 267 lines over 62
  chapters, and none — the character never speaks or thinks in words in that chapter — for 17 lines over
  14 chapters. So two thirds of the renderings are in wording the chapter itself shows the character
  using, and a quarter are not.
- seen in: chapters of all six stories
- query:
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where echoed~^yes view=cites
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where echoed~^no view=cites
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings where echoed~^none view=cites

### exploration-of-narrator-register-or-focalizer-voice/the-distinct-narrator-moves-between-heads
- lead: Several of the chapters where a distinct narrating voice was found describe it as one that moves
  between characters' heads within the chapter: a light comic narrator shifting between Rarity's and
  Fluttershy's with a wry aside ("Meanwhile, in the studio, Fluttershy wished she could trudge away"); a
  summarising voice labelling characters ("the hyperactive couple", "the grizzled Gogoat"), reporting what
  villagers in general thought and moving between heads; a voice that cuts away with "Meanwhile" and
  speaks to the reader ("don't get the wrong idea!"), opening with a content warning and running jokes; and
  a tidy battle-report register with capitalised move names giving verdicts and epithets no character
  voices.
- seen in: chapters of green-is-your-color, the-ember-and-the-spark, wish and nine-tales-of-liberty
- cites:
  - exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters/green-is-your-color-ch01
  - exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters/the-ember-and-the-spark-by-bc414-w9hrsy73-ch12
  - exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters/wish-by-chlorophyll19-gyqecqhk-ch01
  - exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters/nine-tales-of-liberty-by-bc414-8gqvftrm-ch15

### exploration-of-narrator-register-or-focalizer-voice/how-many-renderings-a-chapter-carries
- lead: Read against the index's story and chapter labels, the count of rendered perceptions per chapter is
  steady within a story and differs between them: nine-tales-of-liberty 311 lines over 23 chapters,
  the-ember-and-the-spark 324 over 23, to-hone-a-leaf-blade 280 over 21, green-is-your-color 87 over 5,
  the-harvest-of-falldale 23 in its single chapter, wish 18 in its single chapter. Falldale and wish are
  one-chapter stories in this corpus, so their story-level figures rest on one item each.
- seen in: the index's story and chapter labels for the six stories
- query:
  - rq1 batch=exploration-of-narrator-register-or-focalizer-voice/01-own-fiction-chapters answered=74 field=renderings view=by-story col=carrier

## Proposed questions

- In the stories that carry a narrating voice apart from their characters, does that voice appear in every
  chapter to the same degree, or is it concentrated where the chapter moves between heads?
- Where a rendering is in a register the focalizing character would not use, is the character one whose
  own speech the chapter shows at all, and does the answer change with how much they speak?
- Does the single first-person story differ from the third-person ones in anything besides the absence of a
  separate narrator, such as how much of the world it can state as fact?
- In the one chapter of the-ember-and-the-spark with no distinct narrating voice, what does it do
  differently from the other twenty-two?
- Where the narration mixes the character's register with a narrator's inside one stretch, what does the
  mixing fall on — a feeling, a judgment, a description, or a piece of world knowledge?
