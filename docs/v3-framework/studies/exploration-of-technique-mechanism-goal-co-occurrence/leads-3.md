# exploration-of-technique-mechanism-goal-co-occurrence — leads

## Leads

### exploration-of-technique-mechanism-goal-co-occurrence/operation-is-the-part-most-often-absent
- lead: Of the three parts of a moment, the operation on what the reader knows or believes is
  the one most often absent. Over 11070 moment lines from 602 answered chapters, the operation
  part is the word none in 599 lines and opens with none in another 218; the kind part is none
  in 78 and opens with none in 24; the experience part is none in 8 and opens with none in 8.
  The technique part is never none.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=health top=40

### exploration-of-technique-mechanism-goal-co-occurrence/no-operation-lines-are-comic-dialect-and-typographic
- lead: The lines whose operation is none are dominated by comic techniques (comic, slapstick,
  gag, pun, hyperbolic, bathos, wordplay, non sequitur, comic misdirection, mock-epic), by
  dialect rendering (eye dialect, phonetic dialect, dialect spelling), by typographic emphasis
  and italicized interior thought, and by physical or sensory description. Their experiences are
  comic, humor, tension, amusement, relief, light, voice, whimsical, immersion.
- seen in: every answered chapter across the corpus; read in chapters of a-delicate-balance, crisis-on-two-equestrias, green, magic-tutor, the-appledash-project, twilights-list and unexpected-confessions
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=technique top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=technique n=2 top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=experience top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none sample=30 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch28
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/crisis-on-two-equestrias-ch16
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/green-ch37
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/magic-tutor-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-appledash-project-ch17
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/twilights-list-ch09
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/unexpected-confessions-ch30

### exploration-of-technique-mechanism-goal-co-occurrence/reinforcing-a-known-trait-written-both-as-none-and-as-an-operation
- lead: Where a reader hedged a none operation, the hedge most often says "none beyond
  reinforcing" or "none beyond reaffirming" an already-established trait, or "none, purely
  tonal". The same reinforcement of what is already known is elsewhere written as an operation
  in its own right: reinforces opens 402 operation parts, reaffirms 80, confirms 539. Two
  readings of the same kind of moment, one as no operation and one as a confirming one, sit
  across the results.
- seen in: every answered chapter across the corpus; read in chapters of a-delicate-balance, dashs-new-mom, green and trial-run
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=moment n=2 top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=moment position=1 top=60
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch35
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/dashs-new-mom-ch05
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/green-ch36
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/trial-run-ch02

### exploration-of-technique-mechanism-goal-co-occurrence/kind-absent-only-where-there-are-no-words-to-take
- lead: The kind part is none almost only at scene and section breaks, structural transitions,
  point-of-view shifts marked by a divider, cross-cutting via dividers, and repeated actions
  rather than wordings (a mirrored prank). In those lines the technique is the break or the
  structure, the experience is reorientation or a jump in time and place, and the operation is
  a shift of focus, an interlacing of plotlines, or none.
- seen in: every answered chapter across the corpus; read in chapters of about-last-night, crisis-on-two-equestrias, green, inner-strength, salvation and where-earth-meets-sky
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~^none view=terms col=technique top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~^none sample=20 seed=1 view=list
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~^none where moment~^none view=cites
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/about-last-night-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/crisis-on-two-equestrias-ch14
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/green-ch08
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch09
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/where-earth-meets-sky-ch01

### exploration-of-technique-mechanism-goal-co-occurrence/experience-absent-only-at-paratext-and-plain-information
- lead: The experience part is none in sixteen lines, and those lines are paratext (a byline
  credit, a music credit list, reader contact information, an editorial bracketed note placing
  the story in the show's timeline), coined in-world terms and idioms, plain expository
  reminders and dialogue, and time-skip markers. Only two lines have both experience and
  operation none, both paratext at the end of a chapter. No line has both kind and experience
  none.
- seen in: chapters of a-certain-type-of-chic, a-delicate-balance, bechdels-law, cuddling, filly-fooling, ill-always-be-here-for-you, not-unless-you-mean-it, spread, the-frozen-north, the-gemmed-satyr, the-last-train-home, the-princess-and-the-kaiser and twilights-list
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~^none view=list
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~^none where moment~^none view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~^none where experience~^none view=cites
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/bechdels-law-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-frozen-north-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-last-train-home-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/twilights-list-ch13

### exploration-of-technique-mechanism-goal-co-occurrence/comic-experience-lines-most-often-carry-no-operation
- lead: Among the lines whose experience is comic, humor, amusement or comedy, none is the most
  frequent opening word of the operation part (468 of 3369), ahead of establishes and reveals.
  Where a comic line does carry an operation, reinforces and characterizes rank higher than
  they do over all lines: reinforces 200 and characterizes 188 of 3369, against 402 and 261 of
  11070.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~comic|humor|amusement|comedy view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=moment position=1 top=60

### exploration-of-technique-mechanism-goal-co-occurrence/comic-experience-in-a-third-of-moments-and-nearly-every-story
- lead: An experience of comedy, humor or amusement is written on 3369 of 11070 moment lines,
  in 110 of the 113 stories. Its technique words are comic, dialogue, aside, banter, gag,
  slapstick, running gag, mock, callback, italicized interior. The stories with the most such
  lines are a-delicate-balance, green, about-last-night, filly-fooling and
  clocktower-society-your-safe-word-is-law.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~comic|humor|amusement|comedy view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~comic|humor|amusement|comedy view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=experience top=80

### exploration-of-technique-mechanism-goal-co-occurrence/interior-monologue-reveals-free-indirect-establishes
- lead: The two interior techniques differ in which operation leads. Interior monologue, direct
  thought and interior thought lines open their operation with reveals in 88 of 311, far ahead
  of shows (34) and establishes (33); their experiences are private, intimacy, access, irony,
  anxious, wry. Free indirect lines open with establishes (51) and reveals (44) nearly evenly,
  then shows and characterizes; their experiences are intimacy, sympathy, wry, comic, quiet,
  unease.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"interior monologue|interior thought|direct thought|internal monologue" view=terms col=moment position=1 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"interior monologue|interior thought|direct thought|internal monologue" view=terms col=experience top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"free indirect" view=terms col=moment position=1 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"free indirect" view=terms col=experience top=25

### exploration-of-technique-mechanism-goal-co-occurrence/dialogue-spans-every-experience-and-operation
- lead: Dialogue is the most frequent technique word (1098 lines) and carries the widest spread
  of the other two parts: experiences of comedy, tension, relief, unease, curiosity, sympathy,
  warmth, dread, suspense, surprise and embarrassment; operations opening establishes (160),
  reveals (140), confirms, shows, none (43), introduces, reinforces, signals, characterizes,
  supplies, plants, recasts. No family of experience or of operation is particular to it.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialogue view=terms col=experience top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialogue view=terms col=moment position=1 top=20

### exploration-of-technique-mechanism-goal-co-occurrence/dramatic-irony-named-as-technique-and-as-experience
- lead: Dramatic irony is written on both sides of a moment: as the technique in 211 lines and
  as the experience in 219. Where it is the technique, its experience words are reader, gap,
  knowledge, knows, secret, amusement, tension, suspense, anticipation, and its operation opens
  with confirms (21) ahead of establishes and reveals, the only technique family read here where
  confirms leads. Where it is the experience, the technique is interior monologue, italicized
  thought, an aside, a narrator, a parallel or mirrored or cross-cut scene, or a denial in
  dialogue.
- seen in: every answered chapter across the corpus; read in chapters of boast-busted, clocktower-society-your-safe-word-is-law, cuddling, filly-fooling, flying-high-falling-hard, ill-always-be-here-for-you, letters-from-a-secret-admirer, the-moons-apprentice and those-blue-wings
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"dramatic irony" view=terms col=experience top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"dramatic irony" view=terms col=moment position=1 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~"dramatic irony" view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"dramatic irony" sample=12 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/boast-busted-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/ill-always-be-here-for-you-ch12
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/letters-from-a-secret-admirer-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/those-blue-wings-ch02

### exploration-of-technique-mechanism-goal-co-occurrence/callbacks-and-motifs-reinforce-tie-and-remind
- lead: Callback, echo, motif and parallel lines have recognition as their leading experience
  word (116 of 555), with continuity, closure, satisfaction and pattern; their operations open
  with reinforces (48), confirms (36), ties (28), then none, establishes, reveals, implies,
  reminds, signals, links, frames, retroactively. The installing verbs that lead elsewhere,
  establishes and reveals, are minor here.
- seen in: every answered chapter across the corpus; read in chapters of a-delicate-balance, controlling-your-desires, good-things, ill-always-be-here-for-you, sunny-skies-all-day-long, the-moons-apprentice and those-blue-wings
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~callback|echo|motif|parallel view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~callback|echo|motif|parallel view=terms col=moment position=1 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~recognition view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~callback|motif|echo sample=12 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch25
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/good-things-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/ill-always-be-here-for-you-ch21
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/sunny-skies-all-day-long-ch01

### exploration-of-technique-mechanism-goal-co-occurrence/exposition-installs-with-curiosity
- lead: Exposition, expository dialogue, backstory and summary narration lines open their
  operation with establishes (157 of 594), then reveals, supplies, adds, informs,
  recontextualizes, reminds, fills, explains, expands; their experiences are curiosity, sense,
  mild, sympathy, orientation, grounding, history, reassurance. The verbs supplies, adds,
  informs and fills are concentrated in this family.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"exposition|expository|backstory|summary narration" view=terms col=moment position=1 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"exposition|expository|backstory|summary narration" view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^establishes view=terms col=technique top=25

### exploration-of-technique-mechanism-goal-co-occurrence/description-establishes-or-does-nothing
- lead: Sensory and physical description lines open their operation with establishes (44 of
  254) or none (29), then confirms, reveals, marks, signals; after the comic family this is the
  technique family with the largest share of none. Their experiences are quiet, warmth, unease,
  calm, immersive, wonder, romantic, visceral, awe.
- seen in: every answered chapter across the corpus; read in chapters of magic-tutor, i-can-hear-you-scream, ill-do-anything-for-you and the-possibilities-of-potions
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"sensory|physical description|physical detail|descriptive" view=terms col=moment position=1 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"sensory|physical description|physical detail|descriptive" view=terms col=experience top=20
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/magic-tutor-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/i-can-hear-you-scream-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/ill-do-anything-for-you-ch03

### exploration-of-technique-mechanism-goal-co-occurrence/scene-breaks-signal-and-shift
- lead: Scene breaks, section breaks and time skips were listed as moments in their own right.
  Their kind is usually none, their experience is reorientation, a jump in time or place,
  disorientation, a reset of attention, and their operation opens with signals (15 of 83) or
  shifts (13), then confirms, establishes, lets, none. The typographic divider is what marks
  them.
- seen in: every answered chapter across the corpus; read in chapters of about-last-night, a-certain-type-of-chic, a-delicate-balance, ill-always-be-here-for-you, inner-strength, longest-night-longest-day and romance-reports
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"scene break|section break|time skip" view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"scene break|section break|time skip" view=terms col=moment position=1 top=20
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-certain-type-of-chic-ch06
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch34
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/about-last-night-ch08
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/longest-night-longest-day-ch09

### exploration-of-technique-mechanism-goal-co-occurrence/asides-of-five-kinds-reveal-with-humor
- lead: Aside is a technique word on 510 lines, qualified as interior aside, narratorial aside,
  worldbuilding aside, authorial aside, comic aside, private aside, backstory aside, expository
  aside. Their experiences are humor, comic, wry, amusement, irony, quiet, curiosity, unease;
  their operations open with reveals (72), establishes (54), none (46), signals, shows,
  reinforces, adds, confirms, implies, tells.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~aside view=terms col=technique n=2 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~aside view=terms col=experience top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~aside view=terms col=moment position=1 top=10

### exploration-of-technique-mechanism-goal-co-occurrence/chapter-closings-leave-and-withhold
- lead: Closing lines, final lines and cliffhangers have experiences of closure, quiet,
  anticipation, suspense, warmth, resolution, unresolved, bittersweet, lingering; their
  operations open with leaves (34 of 280), signals, none, closes, confirms, reveals, resolves,
  frames, crystallizes, ends, withholds. Across all lines, the withholding verbs (withholds,
  leaves, keeps) go with closing, withheld, unresolved, ending, cliffhanger, interrupted,
  ambiguous and cut-off techniques.
- seen in: every answered chapter across the corpus; read in chapters of clocktower-society-your-safe-word-is-law, filly-fooling, not-unless-you-mean-it and twilights-list
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"closing|final line|last line|cliffhanger" view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"closing|final line|last line|cliffhanger" view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^(withholds|leaves|keeps) view=terms col=technique top=25
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch14
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/filly-fooling-ch15
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/not-unless-you-mean-it-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/twilights-list-ch04

### exploration-of-technique-mechanism-goal-co-occurrence/dialect-rendering-marks-voice-without-operating
- lead: Dialect rendering (eye dialect, phonetic dialect, dialect spelling, muffled-speech
  spelling) is a technique on 126 lines in 50 stories. Its experience words are voice,
  distinct, folksy, rural, regional, hears, warmth, characterization, comic; its operation
  opens with none in 38 lines, then reinforces (25), marks (16), characterizes (10),
  establishes (4).
- seen in: fifty stories across the corpus, most in filly-fooling, a-delicate-balance, about-last-night, cuddling, the-princess-and-the-kaiser, carrot-top-season and crisis-on-two-equestrias
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialect view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialect view=terms col=experience top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialect view=terms col=moment position=1 top=10

### exploration-of-technique-mechanism-goal-co-occurrence/operation-parts-open-with-a-few-recurring-verbs
- lead: The operation part opens with 517 distinct words over 11070 lines, and a few carry
  most lines: establishes 1150, reveals 1038, none 817, confirms 539, signals 529, shows 452,
  reinforces 402, characterizes 261, marks 196, implies 191, introduces 191, plants 179, sets
  175, primes 158, supplies 151, recasts 145, builds 125, frames 124, leaves 111,
  recontextualizes 109, reframes 103, resolves 103, reminds 102. The objects most named in the
  part are the characters by name, the reader, the chapter, the story, the world, the
  relationship, feelings, stakes, plot and setting.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=moment position=1 top=60
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=moment top=60

### exploration-of-technique-mechanism-goal-co-occurrence/seeding-verbs-go-with-foreshadowing-withheld-and-epigraphs
- lead: Lines whose operation opens with plants, seeds or primes have techniques of
  foreshadowing (35 of 384), dialogue, withheld information, planted detail, aside, epigraph
  (18), dramatic irony, opening, physical detail, unexplained detail, hook, setup. The
  experiences of curiosity and anticipation go with the same techniques: withheld (84 of 800),
  foreshadowing, backstory, planted, allusion, hook, unexplained, mystery, unresolved.
- seen in: every answered chapter across the corpus; read in chapters of crisis-on-two-equestrias, ill-always-be-here-for-you, passive-income, the-princess-and-the-kaiser and you-make-my-whole-life-worthwhile
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^(plants|seeds|primes) view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~curiosity|anticipation view=terms col=technique top=25
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/crisis-on-two-equestrias-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/ill-always-be-here-for-you-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/passive-income-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-princess-and-the-kaiser-ch034
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/you-make-my-whole-life-worthwhile-ch04

### exploration-of-technique-mechanism-goal-co-occurrence/revising-verbs-go-with-reveals-reversals-and-titles
- lead: Lines whose operation opens with recasts, recontextualizes, reframes, retroactively,
  overturns, revises or corrects have techniques of reveal (89 of 519), dialogue, reversal,
  retrospective, tonal, delayed, confession, callback, character, title (17), backstory,
  echo, closing. Lines whose technique carries reveal, withheld, delayed, planted, payoff or
  callback open their operation with reveals, confirms, establishes, signals,
  recontextualizes, resolves, retroactively, implies, overturns.
- seen in: every answered chapter across the corpus; read in chapters of dashs-new-mom, romance-reports, those-blue-wings and rainbooms-and-royalty-new
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^(recasts|recontextualizes|reframes|retroactively|overturns|revises|corrects) view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~reveal|withheld|delayed|planted|payoff|callback view=terms col=moment position=1 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^reveals view=terms col=technique top=25
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/dashs-new-mom-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/romance-reports-ch11
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/those-blue-wings-ch10
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/rainbooms-and-royalty-new-ch11

### exploration-of-technique-mechanism-goal-co-occurrence/confirming-verbs-go-with-comic-callback-and-dialect
- lead: Lines whose operation opens with confirms, reinforces or reaffirms have techniques of
  dialogue (97 of 1021), comic (88), callback (55), irony and dramatic irony, character,
  italicized, reveal, aside, description, banter, dialect, running gag, payoff, beat.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^(confirms|reinforces|reaffirms) view=terms col=technique top=25

### exploration-of-technique-mechanism-goal-co-occurrence/operations-on-the-world-add-operations-on-bonds-confirm-and-resolve
- lead: Lines whose operation names the world, history, lore, politics, law, society or an
  institution open with establishes (160 of 937), reveals, reinforces, adds (44), implies,
  supplies (39), introduces (33), signals, confirms, builds, expands, recontextualizes. Lines
  whose operation names a relationship, bond, feelings, attraction, romance or a crush open
  with establishes (127 of 1131), reveals, confirms (92), shows, signals, reinforces, implies,
  plants, marks, leaves, resolves (18), reframes, recasts. The verbs adds, supplies, introduces
  and expands sit on the world side; confirms, resolves, leaves and reframes on the bond side.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"world|history|lore|magic system|politic|law|society|institution" view=terms col=moment position=1 top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~relationship|bond|feelings|attraction|romantic|crush view=terms col=moment position=1 top=25

### exploration-of-technique-mechanism-goal-co-occurrence/world-operations-concentrate-in-a-few-stories
- lead: Operations naming the world, history, lore, politics, law, society, institutions, war,
  a nation or a regime are written on 1701 lines in 103 stories, and a few stories hold most:
  the-princess-and-the-kaiser 248 of its 1003 lines, clocktower-society-your-safe-word-is-law
  164 of 509, a-delicate-balance 78 of 718, pax-chrysalia 76 of 340, crisis-on-two-equestrias
  73 of 279, green 71 of 494.
- seen in: the-princess-and-the-kaiser, clocktower-society-your-safe-word-is-law, a-delicate-balance, pax-chrysalia, crisis-on-two-equestrias and green, against every story
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~world|history|lore|politic|law|society|institution|war|nation|regime view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/operations-name-state-the-text-built-earlier
- lead: In 1567 lines the operation part names earlier, already, established, prior or
  previous state, and those lines open with reveals (166), confirms (137), reinforces (128),
  establishes, implies, recasts, recontextualizes, signals, reaffirms, resolves,
  retroactively, reminds, ties. The state the moment acts on is written as one the text
  itself built before the moment.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~earlier|already|established|prior|previous view=terms col=moment position=1 top=25

### exploration-of-technique-mechanism-goal-co-occurrence/show-canon-named-as-state-brought-to-the-text
- lead: In 58 lines the operation part names canon or the show. Their techniques are canon
  callback, intertextual allusion, embedded backstory, worldbuilding aside, author's note,
  continuity-placement cue, first-person testimony; their operations open with establishes,
  links, recasts, recontextualizes, retroactively, situates, supplies, fixes, grounds. Several
  lines say the moment fixes the story's point in the show's timeline, recasts a canon event to
  a new meaning, or leans on the reader's outside knowledge rather than supplying new
  information.
- seen in: chapters of the-appledash-project, the-best-night-ever, fixing-up-miss-smartypants, boast-busted, clocktower-society-your-safe-word-is-law, the-twilight-hours, good-things, perfect-on-paper, the-moons-apprentice and the-princess-and-the-kaiser
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"canon|the show|show's|from the show" view=terms col=moment position=1 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"canon|from the show|show canon" view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"canon|from the show|show canon" sample=25 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-appledash-project-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-best-night-ever-ch05
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/fixing-up-miss-smartypants-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/boast-busted-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch18
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch22
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-twilight-hours-ch08

### exploration-of-technique-mechanism-goal-co-occurrence/tension-and-warmth-experiences-share-dialogue-and-interior-techniques
- lead: Experiences of dread, unease, tension, suspense and anxiety (2040 lines) have
  techniques of dialogue (291), comic, irony and dramatic irony, withheld (89), italicized
  interior, reveal, narration, description, closing, escalating, confession. Experiences of
  warmth, tenderness, intimacy and sympathy (1129 lines) have techniques of dialogue (150),
  interior monologue, italicized, free indirect, confession and confessional, embedded, banter,
  tonal shift, domestic, backstory, closing. The two families share dialogue, interior
  techniques and confession; withheld and escalating sit on the tension side, domestic and
  backstory on the warmth side.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~dread|unease|tension|suspense|anxiety view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~warmth|tenderness|intimacy|sympathy view=terms col=technique top=25

### exploration-of-technique-mechanism-goal-co-occurrence/technique-names-carry-experience-and-operation-words
- lead: The technique part carries words of experience and of operation. Comic is on 736
  technique parts and is the first word of 522; ironic, tonal, mock and slapstick follow.
  Reveal is on 389, callback 245, withheld 194, delayed 126, planted 92, payoff 89,
  foreshadowing among the plants lines; withheld, delayed and planted are among the most
  frequent first words. Lines whose technique carries comic, ironic, gag, slapstick or banter
  have experiences of humor, comic, amusement, tension, relief; lines whose technique carries
  reveal, withheld, delayed, planted, payoff or callback have operations opening reveals,
  confirms, establishes, recontextualizes, retroactively, resolves.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=technique top=80
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=technique position=1 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~comic|ironic|humor|gag|slapstick|banter view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~reveal|withheld|delayed|planted|payoff|callback view=terms col=moment position=1 top=20

### exploration-of-technique-mechanism-goal-co-occurrence/technique-names-most-repeated-across-chapters
- lead: The technique two-word runs repeated most across chapters are free indirect (238
  lines), dramatic irony (211), interior monologue (207), italicized interior (127), indirect
  discourse (115), scene break (110), running gag (53), section break (50), expository
  dialogue (48), interior thought (43), scene setting (41), author note (40), set piece (40),
  double entendre (39), in medias res (39), self-aware (37), summary narration (37),
  first-person (36), physical comedy (36), dialect rendering (35), direct address (35), tonal
  shift (34), close third (27), eye dialect (27), register shift (27), non sequitur (24),
  time skip (23), second-person (22), off-page (22), worldbuilding aside (22), chapter title
  (21), direct thought (21). The technique part has 4257 distinct words and 19483 distinct
  two-word runs over 11070 lines.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=technique n=2 top=60
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=pairs col=technique top=40

### exploration-of-technique-mechanism-goal-co-occurrence/kind-stated-as-a-contrast-with-a-plain-reading
- lead: The kind part is written as a contrast in 1511 of 11070 lines: the words taken as one
  thing "rather than" plain, literal, plot, ordinary, mere, scene, narration, neutral, new,
  incidental, continuous, sincere, real, casual or stated. "Words taken as" opens 609 kind
  parts and "taken at face value" is on 57. The kind part is the wordiest of the six, with
  7813 distinct words.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=kind n=3 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=kind n=2 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=kind top=60

### exploration-of-technique-mechanism-goal-co-occurrence/narrative-person-and-tense-recorded-mostly-in-kind
- lead: Narrative person and tense are written mostly in the kind part and less in the
  technique part. First person is in the kind part of 198 lines across 57 stories, third
  person 155 lines, present tense 88 lines across 38 stories; in the technique part
  first-person is on 36 lines in 21 stories and second-person on 22 lines in 10 stories, six
  of them in salvation and five in clocktower-society-your-safe-word-is-law. The lines whose
  kind says first person have techniques of interior monologue, italicized interior, direct
  thought, confessional monologue, direct address, free indirect, author note, cold open.
- seen in: every answered chapter across the corpus; second-person in salvation, clocktower-society-your-safe-word-is-law, green, a-delicate-balance and six other stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~first.person view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~present.tense view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~first.person view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~second.person view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~first.person view=terms col=technique n=2 top=15

### exploration-of-technique-mechanism-goal-co-occurrence/italics-the-marker-named-for-interior-thought
- lead: Italics is the marker named on 304 lines, and the techniques on those lines are
  italicized interior monologue and thought, typographic emphasis, dialogue, embedded insert,
  aside, free indirect, dream, flashback, epigraph, quoted and unattributed inserts.
  Italicized is the first word of 352 technique parts. Where italics carries emphasis on a
  spoken word rather than a thought, the operation is none and the experience is a delivery or
  intonation cue.
- seen in: every answered chapter across the corpus; read in chapters of the-appledash-project, magic-tutor, lets-find-you-a-date, the-notebook and head-in-the-clouds
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where marks~italic view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=technique position=1 top=40
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-appledash-project-ch17
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/magic-tutor-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/lets-find-you-a-date-ch06
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-notebook-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/head-in-the-clouds-ch03

### exploration-of-technique-mechanism-goal-co-occurrence/letters-in-a-third-of-stories
- lead: Letters and epistolary inserts are a technique on 73 lines in 33 of 113 stories, at
  most seven lines in a story. Their kind is a quoted, formal, italicized, verbatim, addressed,
  handwritten, first-person letter or a summary of one; their experiences are intimacy, a shift
  of register, formality, warmth, closure, comedy; their operations open with establishes (12),
  reveals (9), delivers, supplies, confirms, gives, introduces, sets, recaps, conveys. Letters
  shown with struck-through revisions, letters dictated in fragments, and pastiche of the
  show's letter convention are among the lines.
- seen in: thirty-three stories, most lines in flying-high-falling-hard, the-princess-and-the-kaiser, a-delicate-balance, perfect-on-paper, pax-chrysalia and unexpected-confessions
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistolary view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistolary view=terms col=kind top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistolary view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistolary view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistolary sample=10 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/green-ch39
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch32
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/injuring-eternity-ch05
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/perfect-on-paper-ch18
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/unexpected-confessions-ch21

### exploration-of-technique-mechanism-goal-co-occurrence/dreams-in-a-third-of-stories-with-disorientation-and-reveals
- lead: Dreams and daydreams are a technique on 89 lines in 32 stories, fourteen of them in
  salvation and eleven in the-moons-apprentice. Their kind is dream narration set against
  waking or real narration, often in present tense or italics, sometimes indistinguishable
  from waking scenes until revealed; their experiences are disorientation (17), jolt, relief,
  surprise, recognition, unease, dread, curdling; their operations open with reveals (16),
  signals, establishes, confirms, recasts, retroactively, forces, resets. Among the lines: a
  delayed reveal reframing the prior scene as a dream, a dream bleeding into waking across a
  section break, a single unitalicized line breaking a dream's formatting.
- seen in: thirty-two stories, most lines in salvation, the-moons-apprentice, filly-fooling, unexpected-confessions, cuddling, a-delicate-balance and perfect-on-paper
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream view=terms col=kind top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream sample=10 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/those-blue-wings-ch10
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/you-make-my-whole-life-worthwhile-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/head-in-the-clouds-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/unexpected-confessions-ch21
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch18

### exploration-of-technique-mechanism-goal-co-occurrence/embedded-documents-flashbacks-and-stories-within
- lead: Embedded is a technique word on 277 lines: embedded document, embedded flashback,
  embedded backstory, embedded first-person, embedded italicized, story within story,
  embedded legend, anecdote, hindsight, letter, quoted. Found and in-universe documents (25
  lines) have a kind of quoted, written, verbatim, actual text taken as an artifact rather than
  narration, and operations opening reveals, supplies, establishes. Documents, reports,
  newspapers, articles and transcripts are a technique on 161 lines in 50 stories, most in
  the-princess-and-the-kaiser and clocktower-society-your-safe-word-is-law; flashbacks on 90
  lines in 41 stories; journals, diaries and log entries on 14 lines in 7 stories; text
  messages, chats, telegrams or passed notes on 2 lines.
- seen in: every answered chapter across the corpus; documents most in the-princess-and-the-kaiser and clocktower-society-your-safe-word-is-law, journals most in the-moons-apprentice
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~embedded view=terms col=technique n=2 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"embedded document|found document|in-universe document|excerpt|in-world document" view=terms col=kind top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"embedded document|found document|in-universe document|excerpt|in-world document" view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"document|report|newspaper|article|transcript|recipe|list format" view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~flashback view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"journal|diary|log entry" view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"text message|chat|telegram|note passed" view=by-story
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/on-a-cross-and-arrow-ch11
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/perfect-on-paper-ch08

### exploration-of-technique-mechanism-goal-co-occurrence/songs-and-verse-in-half-the-stories
- lead: Songs, lyrics, poems and verse are a technique on 98 lines in 52 of 113 stories, at
  most ten lines in a story (a-delicate-balance), with the-princess-and-the-kaiser,
  flying-high-falling-hard, carrot-top-season and the-best-night-ever next.
- seen in: fifty-two stories across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~song|lyric|poem|verse|poetry view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/paratext-placed-both-as-moments-and-as-unplaced
- lead: Author's notes, chapter titles, epigraphs and bylines are written on both sides of the
  results. As moments: author note, paratextual author's note, chapter title, epigraph
  quotation, title echo, authorial aside and direct authorial address on 366 lines, and
  author, paratext or footnote on 202 lines in 69 stories; their experiences are a shift out
  of immersion, a broken frame, awareness of the fiction, meta commentary, a jolt; their
  operations open with none (25 of 112), informs (15), reveals, tells, frames, reframes. As
  unplaced: chapter title on 81 lines and author's note on 37, with author or note on 124
  unplaced lines in 55 stories.
- seen in: every answered chapter across the corpus; most lines in clocktower-society-your-safe-word-is-law, a-delicate-balance, about-last-night and inner-strength
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~author|paratext|title|epigraph view=terms col=technique n=2 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~footnote|author|paratext view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"author.s note|author note|paratext" view=terms col=experience top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"author.s note|author note|paratext" view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~title view=terms col=c1 n=2 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~author|note view=by-story
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch13
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch21
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-twilight-hours-ch08
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/flank-ology-ch01

### exploration-of-technique-mechanism-goal-co-occurrence/epigraphs-and-titles-seed-and-recast
- lead: Epigraphs appear among the techniques of seeding operations (18 of the 384 lines
  opening plants, seeds or primes), and chapter titles among the techniques of revising
  operations (17 of 519) and of recognition experiences (43 of 532), as title echo, title
  echoing, title framing, ironic title.
- seen in: every answered chapter across the corpus; read in a chapter of a-delicate-balance and one of unexpected-confessions
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^(plants|seeds|primes) view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^(recasts|recontextualizes|reframes|retroactively|overturns|revises|corrects) view=terms col=technique top=25
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~recognition view=terms col=technique top=25
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch21
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/unexpected-confessions-ch25

### exploration-of-technique-mechanism-goal-co-occurrence/unplaced-holds-typos-formatting-and-dividers
- lead: What the readers could place in no part includes apparent typos and editing slips
  (typo 36 lines, slip 36), formatting artifacts (formatting 57, artifact 31, asterisk 33:
  stray bold markers, merged lyric lines, a stray space before a closing italic), typographic
  scene-break dividers, and pervasive italic emphasis on single words. There are 1362 unplaced
  lines, one to five per chapter, median two.
- seen in: every answered chapter across the corpus; read in chapters of a-delicate-balance, crisis-on-two-equestrias, on-a-cross-and-arrow, the-best-night-ever, the-moons-apprentice and the-princess-and-the-kaiser
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=health top=10
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=terms col=c1 top=60
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~typo|slip|formatting|artifact sample=15 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch10
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/crisis-on-two-equestrias-ch08
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-best-night-ever-ch05
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-princess-and-the-kaiser-ch037

### exploration-of-technique-mechanism-goal-co-occurrence/unplaced-holds-stray-details-and-outside-references
- lead: Unplaced also holds stray, unexplained or dropped details (stray 58 lines, unexplained
  58, a setup noted and never mentioned again), passing continuity nods to unnarrated events,
  real-world and fandom references, show-canon cameos used as in-jokes, a character's name
  spelled inconsistently, epithets substituted for names in dialogue tags, species-specific
  idioms and minced oaths, and worldbuilding proper nouns assumed familiar from earlier
  chapters.
- seen in: every answered chapter across the corpus; read in chapters of a-delicate-balance, carrot-top-season, clocktower-society-your-safe-word-is-law, cuddling, dashs-new-mom, heat-of-the-moment, rainbooms-and-royalty-new, the-princess-and-the-kaiser and unexpected-confessions
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced sample=40 seed=1 view=list
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~"real world|outside story|reference|allusion" view=terms col=c1 n=2 top=20
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=terms col=c1 n=2 top=40
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch07
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch09
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/cuddling-ch06
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/heat-of-the-moment-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/rainbooms-and-royalty-new-ch25
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-princess-and-the-kaiser-ch020
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/unexpected-confessions-ch27

### exploration-of-technique-mechanism-goal-co-occurrence/moments-per-chapter-range-widely
- lead: The readers listed between 1 and 58 moments per chapter, median 18, over 602 answered
  chapters in 111 stories with at least one line. The stories with the most lines are
  the-princess-and-the-kaiser (1003 lines over 50 chapters), a-delicate-balance (718 over 36),
  filly-fooling (527 over 25), clocktower-society-your-safe-word-is-law (509 over 28) and green
  (494 over 25); were-just-feeling-fine has one line for its one chapter.
- seen in: every answered chapter across the corpus
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=health top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=by-story

## Proposed questions

- Is dramatic irony a prose technique, an experience, or an operation on the gap between what the reader and a character know, given that the readers wrote it on both the technique and the experience side?
- Is reinforcing or reaffirming a trait the reader already holds an operation on what the reader knows or believes, or no operation, given that the readers wrote the same kind of moment both ways?
- Do the operations on what the reader knows fall into a small set of kinds, installing (establishes, reveals, supplies, adds), confirming (confirms, reinforces, reaffirms), seeding (plants, primes, seeds), revising (recasts, recontextualizes, overturns) and withholding (withholds, leaves, keeps), and does each technique family draw on the same set?
- Do operations on the story's world and history differ in kind from operations on characters' feelings and bonds, given that adds, supplies and expands cluster on the world side and confirms, resolves and reframes on the bond side?
- Are narrative person and tense, first person, second person and present tense, prose techniques in the sense of forms the reader takes the words in, or a frame outside any one moment, given that the readers wrote them mostly in the kind part?
- Are paratexts, author's notes, chapter titles, epigraphs and bylines, moments at all, and if so which of the three parts do they occupy, given that the readers placed them both as moments and as unplaced?
- Is a callback's recognition an operation on what the reader knows about the fabula or on what the reader remembers of the text itself?
