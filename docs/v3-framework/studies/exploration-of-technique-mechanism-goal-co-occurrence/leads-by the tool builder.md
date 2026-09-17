# exploration-of-technique-mechanism-goal-co-occurrence — leads

## Leads

### exploration-of-technique-mechanism-goal-co-occurrence/none-by-column
- lead: Of the three parts a moment carries, the operation is the one the readers leave empty:
  over 11,070 moment lines from 602 chapters, the operation part is the word none on 599 lines
  and opens with none on a further 218; the experience part is none on 8 lines and opens with
  none on 8 more; the kind-of-words part is none on 78 and opens with none on 24; the technique
  part is never none.
- seen in: the moments lines of every answered chapter, all 113 stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=health

### exploration-of-technique-mechanism-goal-co-occurrence/operation-none-with-comic-and-typographic-techniques
- lead: Where the operation is none, the technique names lean comic and typographic. Of the 817
  lines whose operation opens with none, the technique part holds the word comic on 161, aside
  on 44, dialogue on 43, dialect on 38, italicized on 35, emphasis on 34, slapstick on 30,
  simile, pun and typographic on 18 each; the recurring two-word runs are physical comedy,
  dialect rendering, eye dialect, italicized interior, phonetic dialect, typographic emphasis,
  author note, comic aside. The experience part of the same lines holds comic on 213, humor on
  180, tension on 70, amusement on 57, relief on 53, voice on 37. These lines occur in 87 of the
  113 stories.
- seen in: the moments lines whose operation is none, across most stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=technique top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=technique n=2 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/experience-none-on-paratext-and-bare-information
- lead: The experience part is none on 16 lines in 13 chapters, and those lines are paratext or
  bare information: a byline credit, a music credit list, reader contact information, an
  editorial bracketed note, scene-break dividers, a time-skip statement, a coined in-world term,
  an in-universe idiom, plain exposition and a policy exchange. On most of them the operation
  part is filled; on the music credit list and the contact information both operation and
  experience are none.
- seen in: thirteen chapters across twelve stories, at paratext and bare statements
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~^none view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-certain-type-of-chic-ch06
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/bechdels-law-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/cuddling-ch07
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/filly-fooling-ch18
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/ill-always-be-here-for-you-ch06
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/not-unless-you-mean-it-ch07
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/spread-ch15
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-frozen-north-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-gemmed-satyr-ch22
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-last-train-home-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-princess-and-the-kaiser-ch095
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/twilights-list-ch13

### exploration-of-technique-mechanism-goal-co-occurrence/kind-none-on-scene-breaks
- lead: The kind-of-words part is none mostly where the technique is a break: of the 102 lines
  whose kind opens with none, the technique part holds break on 40, scene on 34, section on 18,
  structure on 10, transition and marker on 6 and 7.
- seen in: the moments lines named as scene or section breaks, throughout the batch
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~^none view=terms col=technique top=15

### exploration-of-technique-mechanism-goal-co-occurrence/operation-verbs
- lead: The operation part is written as a verb-first sentence, and the verbs repeat: over all
  11,070 lines the first word is establishes on 1,150, reveals on 1,038, none on 817, confirms
  on 539, signals on 529, shows on 452, reinforces on 402, characterizes on 261, marks on 196,
  implies and introduces on 191 each, plants on 179, sets on 175, primes on 158, supplies on
  151, recasts on 145, builds on 125, frames on 124, leaves on 111, recontextualizes on 109,
  reframes and resolves on 103 each, reminds on 102; 517 distinct first words in all.
- seen in: the operation part of every moment line, all stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=moment position=1 top=40

### exploration-of-technique-mechanism-goal-co-occurrence/technique-column-words
- lead: The technique part is a short label, 4,257 distinct words over 11,070 lines. The words
  that recur: dialogue on 1,098 lines, comic on 736, aside on 488, italicized on 476, scene on
  466, narration on 432, interior on 409, reveal on 389, description and monologue on 318 each,
  embedded and exposition on 277 each, irony on 259, callback on 245, speech on 218, withheld on
  194, banter on 154, confession on 145, sensory on 145, retrospective on 139. The recurring
  two-word names: free indirect on 238, dramatic irony on 211, interior monologue on 207, scene
  break on 110, running gag on 53, section break on 50, expository dialogue on 48, author note on
  40, double entendre on 39, medias res on 39, first person on 36, physical comedy on 36, dialect
  rendering on 35, direct address on 35. On 417 lines in 267 chapters the name is of the form one
  thing via or through another.
- seen in: the technique part of every moment line, all stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=technique top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=technique n=2 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"\\bvia\\b|\\bthrough\\b" view=cites

### exploration-of-technique-mechanism-goal-co-occurrence/kind-of-words-core-vocabulary
- lead: The kind-of-words part, the longest free description, converges on a small set of nouns:
  dialogue on 1,610 lines, narration on 989, description on 926, speech on 576, physical on 542,
  action on 515, private on 509, narrated on 438, spoken on 421, thought on 415, plain on 382,
  literal on 333, italicized on 323, first person on 198, third person on 155, present tense on
  88. It is written as a contrast on 1,511 lines, with the words rather than, and as words taken
  on 609, at face value on 65.
- seen in: the kind-of-words part of every moment line, all stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=kind top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=kind n=2 top=30

### exploration-of-technique-mechanism-goal-co-occurrence/experience-column-words
- lead: The experience part is affect vocabulary with an intensity modifier. The affects: comic
  on 1,602 lines, humor on 962, tension on 727, amusement on 544, recognition on 532, unease on
  510, relief on 508, curiosity on 480, warmth on 419, dread on 391, suspense on 350,
  anticipation on 347, sympathy on 304, surprise on 301, intimacy on 280, shock on 274,
  embarrassment on 242, tenderness on 180, discomfort on 177, disorientation on 165, romantic on
  162, deflation and reassurance on 134 each. The modifiers: mild on 642, quiet on 576, mounting
  on 359, sudden on 312, brief on 300, small on 273. The recurring pairs: dramatic irony on 219,
  comic relief on 133, comic deflation on 87, secondhand embarrassment on 71, wry amusement on
  69, mounting dread on 57, tonal whiplash on 45, dark humor on 45. The word comic is in the
  technique part of 737 lines in 101 of the 113 stories.
- seen in: the experience part of every moment line, all stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=experience top=60
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=experience n=2 top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~comic view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/dialogue-moments-establish-and-reveal
- lead: Moments whose technique names dialogue, 1,098 lines in 93 of the 113 stories, carry an
  operation whose first word is establishes on 160, reveals on 140, confirms on 53, shows on 48,
  none on 43, introduces on 36, reinforces on 35, signals on 33, characterizes on 32, and an
  experience holding comic on 178, tension on 143, humor on 88, relief on 76, mounting on 70,
  unease on 60, sympathy on 55, warmth on 54, dread on 45, suspense on 41.
- seen in: the moments named dialogue, in most stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialogue view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialogue view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialogue view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/interior-thought-moments-reveal
- lead: Moments whose technique names interior monologue, italicized thought, interior thought,
  direct thought or an interior aside, 649 lines in 88 stories, carry an operation whose first
  word is reveals on 141, then shows on 63, establishes on 44, confirms, none and signals on 37
  each; reveals leads here where establishes leads for dialogue and description. Their
  experience holds comic on 112, intimacy on 91, private on 89, access on 67, irony on 57, sudden
  on 56, intimate on 53, wry on 38, dread on 30, anxious on 29.
- seen in: the moments named as a character's rendered thought, in most stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"interior monologue|italicized|interior thought|direct thought|interior aside" view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"interior monologue|italicized|interior thought|direct thought|interior aside" view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"interior monologue|italicized|interior thought|direct thought|interior aside" view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/free-indirect-moments
- lead: Moments named free indirect discourse, 232 lines in 65 stories, carry an operation whose
  first word is establishes on 51, reveals on 44, shows on 20, characterizes on 13, and an
  experience holding self on 39 lines, comic on 36, intimacy on 31, sympathy on 22, wry on 19,
  unease on 15, anxiety on 13, private on 13, access on 12.
- seen in: the moments named free indirect discourse, in more than half the stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"free indirect|indirect discourse" view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"free indirect|indirect discourse" view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"free indirect|indirect discourse" view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/dramatic-irony-moments-confirm
- lead: Moments named dramatic irony, 171 lines in 70 stories, are the one group whose operation
  opens with confirms more often than with establishes or reveals: confirms on 21, establishes
  on 18, reveals on 15, sets on 9, signals on 9, reinforces on 7. Their experience holds
  amusement on 25, tension on 23, suspense on 20, anticipation on 14, unease on 14, dread on 12,
  the word gap on 12, knowledge on 10.
- seen in: the moments named dramatic irony, in most stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"dramatic irony" view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"dramatic irony" view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"dramatic irony" view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/description-moments-establish-or-do-nothing
- lead: Moments named description, sensory detail or detail, 546 lines, carry an operation whose
  first word is establishes on 86 and none on 45, then reveals on 34, signals on 28, confirms on
  27, reinforces on 25, primes on 14, plants on 13; none is second here and nowhere else among
  the named groups. Their experience holds quiet on 58, unease on 54, comic on 40, warmth on 29,
  awe and wonder on 26 each, dread on 22, tenderness on 19, arousal and romantic on 17 each.
- seen in: the moments named as description, throughout the batch
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~description|sensory|detail view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~description|sensory|detail view=terms col=experience top=20

### exploration-of-technique-mechanism-goal-co-occurrence/retrospective-moments-supply-and-recontextualize
- lead: Moments named flashback, retrospective, backstory or exposition, 623 lines, carry an
  operation whose first word is establishes on 103, reveals on 91, then supplies on 41, adds on
  26, recontextualizes on 24, informs on 23, reminds on 19, fills on 18, recasts on 12; supplies,
  recontextualizes, informs and fills lead in no other named group. Their experience holds
  curiosity on 72, sympathy on 54, and the words history on 30 and earlier on 26.
- seen in: the moments named as backstory or exposition, throughout the batch
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~flashback|retrospective|backstory|exposition view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~flashback|retrospective|backstory|exposition view=terms col=experience top=15

### exploration-of-technique-mechanism-goal-co-occurrence/scene-breaks-signal-and-shift
- lead: Moments named scene or section breaks, 81 lines, carry an operation whose first word is
  signals on 15 and shifts on 13, then confirms, establishes, lets and none on 4 each, and an
  experience holding time on 18, reorientation on 16, jump on 9, abrupt and disorientation on 7
  each.
- seen in: the moments named as breaks, throughout the batch
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"scene break|section break" view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"scene break|section break" view=terms col=experience top=15

### exploration-of-technique-mechanism-goal-co-occurrence/running-gags-and-callbacks-reinforce
- lead: Moments named running gag or callback, 283 lines, carry an operation whose first word
  is reinforces on 32, reminds on 21, confirms on 20, implies on 18, none and ties on 16 each,
  establishes on 15, reveals on 12, reaffirms on 11; reinforces and reminds lead in no other
  named group. Their experience holds recognition on 55, humor on 46, amusement on 34,
  continuity on 29, history on 16.
- seen in: the moments named as gags and callbacks, throughout the batch
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"running gag|callback" view=terms col=moment position=1 top=10
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"running gag|callback" view=terms col=experience top=12

### exploration-of-technique-mechanism-goal-co-occurrence/withheld-moments-produce-curiosity
- lead: Moments named withheld, delayed or withholding, 330 lines, carry an operation whose
  first word is reveals on 27, confirms on 25, signals on 21, plants on 18, opens on 14, implies
  on 13, withholds on 12, and an experience holding curiosity on 95, suspense on 51, recognition
  on 32, unease on 31, anticipation on 25; curiosity leads the experience in no other named
  group.
- seen in: the moments named as withholding, throughout the batch
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~withheld|delayed|withholding view=terms col=moment position=1 top=8
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~withheld|delayed|withholding view=terms col=experience top=10

### exploration-of-technique-mechanism-goal-co-occurrence/closing-moments-leave-and-signal
- lead: Moments the readers named as a closing line, closing image or last line, 233 lines in
  219 chapters, carry an operation whose first word is leaves on 27, signals on 23, closes on
  15, none on 15, confirms on 13, resolves and reveals on 9 each, frames on 8; leaves and closes
  lead in no other named group.
- seen in: the moments named as a chapter's close, in most answered chapters
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"closing|final line|last line" view=terms col=moment position=1 top=8
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"closing|final line|last line" view=cites

### exploration-of-technique-mechanism-goal-co-occurrence/direct-address-rare
- lead: Moments named direct address, second person or addressing the reader are 24 lines in 18
  stories, and on 5 of the 24 the operation is none, the highest share of none among the named
  groups.
- seen in: a few moments in a sixth of the stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"direct address|second person|addresses the reader" view=terms col=moment position=1 top=8
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"direct address|second person|addresses the reader" view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/experience-by-operation-verb
- lead: The experience words shift with the operation verb. Where the operation opens with
  establishes, 1,150 lines, the experience holds sense on 196, comic on 139, amusement on 79,
  warmth on 61, quiet on 62, intimacy on 50; where it opens with reveals, 1,038 lines, comic on
  142, sympathy on 81, sudden on 77, surprise on 58, irony on 58, private on 57, recognition on
  50; where it opens with confirms, 539 lines, comic on 96, recognition on 50, relief on 36,
  warmth on 35, shock on 25, satisfaction on 24. Sympathy, sudden and surprise sit with reveals;
  relief and satisfaction with confirms; warmth and intimacy with establishes.
- seen in: the moments lines grouped by their operation's first word, all stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^establishes view=terms col=experience top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^reveals view=terms col=experience top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^confirms view=terms col=experience top=15

### exploration-of-technique-mechanism-goal-co-occurrence/letters-in-a-third-of-stories
- lead: Moments named letter or epistolary occur on 73 lines in 33 of the 113 stories, at most
  7 lines in one story and one or two in most; their operation opens with establishes on 12,
  reveals on 9, delivers and supplies on 4 each.
- seen in: a third of the stories, a few chapters each
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistolary view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistolary view=terms col=moment position=1 top=10

### exploration-of-technique-mechanism-goal-co-occurrence/dreams-concentrate-in-two-stories
- lead: Moments named dream occur on 89 lines in 32 stories, and a quarter of them sit in two:
  14 lines across 7 chapters of salvation and 11 lines across 9 chapters of the-moons-apprentice;
  every other story has at most 6. Their operation opens with reveals on 16, signals on 8,
  establishes on 7, confirms on 5, recasts and retroactively on 4 each.
- seen in: the chapters of salvation and the-moons-apprentice, and thinly across thirty other stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream view=terms col=moment position=1 top=10
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream story=salvation view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream story=the-moons-apprentice view=cites
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch06
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch09
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch12
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch16
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch18
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch05
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch11
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch20
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch21
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch33
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch41
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch43

### exploration-of-technique-mechanism-goal-co-occurrence/dialect-in-half-the-stories
- lead: Moments named dialect occur on 126 lines in 50 of the 113 stories: 12 lines in
  filly-fooling, 8 each in a-delicate-balance and about-last-night, 7 in cuddling, 6 each in
  carrot-top-season and crisis-on-two-equestrias, one or two in most of the rest. Dialect is also
  the fourth most frequent technique word among the lines whose operation is none.
- seen in: half the stories, most often one line per chapter
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialect view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=technique top=30

### exploration-of-technique-mechanism-goal-co-occurrence/author-notes-read-as-moments
- lead: Author's notes and other paratext were read as moments on 93 lines in 40 stories; the
  most in one story is clocktower-society-your-safe-word-is-law, 13 lines in 12 of its 28
  chapters. The readers also set 37 author-note lines and 81 chapter-title lines under unplaced.
- seen in: the notes after chapters in a third of the stories, most of all in clocktower-society
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"author.?s? note|paratextual" view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"author.?s? note|paratextual" story=clocktower-society-your-safe-word-is-law view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=terms col=c1 n=2 top=25
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch06
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch08
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch10
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch11
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch17
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch18
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch21
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch25
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch28

### exploration-of-technique-mechanism-goal-co-occurrence/operations-reaching-before-the-chapter
- lead: Although each reader had one chapter and nothing else of the story, the operation part
  refers to what came before on 1,238 lines, with the words earlier, previous, prior, already or
  so far; those operations open with reveals on 153, confirms on 110, establishes on 77,
  recontextualizes on 46, signals on 46, implies on 43, recasts on 43, reinforces on 40,
  resolves and retroactively on 30 each. On 112 lines in 38 stories the reference is explicit,
  an earlier chapter, the story so far or what the reader already knows.
- seen in: the operation part across most stories, densest in filly-fooling, unexpected-confessions, green, romance-reports and a-certain-type-of-chic
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"earlier|previous|prior|before this chapter|so far|already" view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"earlier chapter|previous chapter|prior chapter|earlier in the story|so far|already established|already knows|already know" view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/operations-reaching-after-the-chapter
- lead: The operation part refers to what comes after on 1,310 lines, with the words later, next
  chapter, to come, sets up, seeds, plants, primes or foreshadow; those operations open with
  plants on 179, primes on 158, sets on 131, establishes on 127, reveals on 69, signals on 58,
  seeds on 47, foreshadows on 21.
- seen in: the operation part across the batch
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"later|next chapter|to come|sets up|seeds|plants|primes|foreshadow" view=terms col=moment position=1 top=12

### exploration-of-technique-mechanism-goal-co-occurrence/operations-naming-the-show
- lead: The operation part names the show, its canon, the series or an episode on 110 lines in
  53 stories; the techniques on those lines are named callback, canon, embedded, continuity,
  intertextual, allusion, backstory, retrospective. Two stories hold the most: 11 lines in
  clocktower-society-your-safe-word-is-law and 10 lines in two chapters of
  the-best-night-ever-repeat.
- seen in: half the stories, one or two lines each, and thickly in the-best-night-ever-repeat
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"the show|canon|series|episode|show s" view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"the show|canon|series|episode|show s" view=terms col=technique top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"the show|canon|series|episode|show s" story=the-best-night-ever-repeat view=cites
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-best-night-ever-repeat-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-best-night-ever-repeat-ch06

### exploration-of-technique-mechanism-goal-co-occurrence/unplaced-holds-titles-notes-artifacts-and-world-details
- lead: The unplaced field holds 1,362 lines, a median of two per chapter. What the readers put
  there: the chapter's title on 141 lines in 137 chapters; author's notes on 37; scene-break
  dividers on 27 and horizontal rules on 11; typos, stray asterisks and formatting artifacts on
  191 lines in 148 chapters; details of the world or its lore on 112 lines in 48 stories, 9 of
  them in a-delicate-balance; and references to the real world or outside the story on 48.
- seen in: the unplaced lines of most chapters, all stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=health
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=terms col=c1 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=terms col=c1 n=2 top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~"chapter title|title" view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~typo|misspell|grammar|formatting|artifact|asterisk|mojibake|encoding view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~world|lore view=by-story

## Proposed questions

- Which of the operations the chapter readers named as reaching before or after their chapter hold when the chapters before it are read with it?
- Are the moments whose operation is none, comic beats, dialect and typographic emphasis, a layer that recurs story-wide rather than moment by moment?
- Do the kind-of-words values the readers converge on, dialogue, narration, description, speech, thought, action, first or third person, present tense, sit at the grain the technique dimension wants, with the readers' technique names as their sub-kinds?
- Do the operation verbs the readers converge on, establishes, reveals, confirms, signals, reinforces, plants, primes, recasts, form the values of the operation dimension?
- Are letters and dreams choices made once per story rather than at a moment, given that dreams concentrate in two stories and letters spread thinly across a third?
- What do the world and lore details the readers set under unplaced hold that the moments do not?
