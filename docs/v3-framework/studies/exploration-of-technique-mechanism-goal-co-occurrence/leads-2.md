# exploration-of-technique-mechanism-goal-co-occurrence — leads

## Leads

### exploration-of-technique-mechanism-goal-co-occurrence/operation-is-the-part-left-none
- lead: Over the 11,070 moment lines of the 602 answered chapters, the technique part was never
  given as none; the experience part was none on 8 lines and none followed by more words on 8
  more; the kind-of-words part was none on 78 and a qualified none on 24; the operation part was
  none on 599 and a qualified none on 218. Where one of the three is absent it is the operation,
  on about one moment in fourteen. A chapter carried from 1 to 58 moments, 18 at the median, and
  from 1 to 5 unplaced lines, 2 at the median.
- seen in: every answered chapter of the batch, taken together
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=health
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=health

### exploration-of-technique-mechanism-goal-co-occurrence/none-operation-lines-carry-comic-dialect-and-typographic-techniques
- lead: On the lines whose operation is none, the technique part opens with comic, italicized,
  slapstick, hyperbolic, phonetic, paratextual, typographic, mock, dialect, eye, physical; its
  repeated two-word runs are physical comedy, dialect rendering, eye dialect, italicized
  interior, phonetic dialect, typographic emphasis, author note, comic aside, italicized
  emphasis, comic misdirection, comic simile, dialect orthography. The experience on these
  lines opens with comic, humor, brief, light, small, mild, amusement, wry, broad, quick,
  whimsical, playful. Such lines are in 87 of the 113 stories, most in a-delicate-balance,
  clocktower-society-your-safe-word-is-law, filly-fooling, unexpected-confessions and green.
- seen in: the moments given no operation, across most of the stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=technique n=2 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=technique position=1 top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=terms col=experience position=1 top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^none view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/comic-and-dialect-techniques-do-nothing-or-reinforce
- lead: Read from the technique side the same pairing holds. On lines whose technique names a
  comic form, none is the most frequent opening of the operation part, ahead of establishes,
  reinforces and characterizes. On lines whose technique names dialect or phonetic rendering,
  none leads again, then reinforces, marks, characterizes, differentiates, distinguishes. In
  every other technique family queried, none is fourth or lower.
- seen in: the comic-form and dialect moments
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~comic|slapstick|gag|banter|deflation|bathos view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialect|phonetic view=terms col=moment position=1 top=15

### exploration-of-technique-mechanism-goal-co-occurrence/none-operation-qualified-as-tonal-or-characterizing
- lead: Where the readers wrote none for the operation and went on, what follows names what the
  moment does instead: purely tonal; purely characterizing; none beyond reinforcing an
  established trait or an established comic dynamic; none to world facts, but reinforces season
  and isolation; none for the story world, the words being an author's note outside it; none
  new to the facts, but signals that the tension carries into the next chapter. The readers set
  characterization by reinforcement, tone and paratext outside an operation on what the reader
  knows, and still wrote them in the operation part.
- seen in: the moments whose operation part opens with none and continues
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"^none\\W+\\w" sample=25 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch35
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/bechdels-law-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/boast-busted-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/finding-your-rhythm-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/head-in-the-clouds-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/ill-always-be-here-for-you-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/unexpected-confessions-ch15

### exploration-of-technique-mechanism-goal-co-occurrence/kind-none-on-breaks
- lead: The kind-of-words part is none almost only where the technique is a scene break, section
  break, break marker, cross-cutting, a POV switch, a time skip or a structure laid across scene
  breaks: the reader had a technique with no words to take. Two lines in one chapter gave none
  for explicit sexual narration and for a comic anticlimax.
- seen in: the scene-break and structural moments
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~^none view=terms col=technique n=2 top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~^none sample=6 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/about-last-night-ch08
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/crisis-on-two-equestrias-ch14
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/filly-fooling-ch15
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/inner-strength-ch12
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch09

### exploration-of-technique-mechanism-goal-co-occurrence/experience-none-on-paratext-and-bare-information
- lead: The experience part is none on 16 lines: a byline credit, a music credit list, reader
  contact information, an editorial bracketed continuity note, a coined in-world term, an
  in-universe idiom, expository dialogue traded as policy, a plain exposition reminder, an
  offhand backstory reference, a cutie-mark exposition aside, a time-skip marker, a typographic
  scene break, an offhand pairing reveal. Most of these lines still carry an operation, which
  supplies the story's vocabulary, situates the tale in the show's timeline, establishes a fact
  or advances the clock; two carry none in both parts.
- seen in: paratext and bare-information moments in a dozen chapters
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~^none view=list
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~^none view=terms col=technique n=2 top=20
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/bechdels-law-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/cuddling-ch07
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-frozen-north-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-gemmed-satyr-ch22
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-last-train-home-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-princess-and-the-kaiser-ch095
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/twilights-list-ch13

### exploration-of-technique-mechanism-goal-co-occurrence/operation-written-verb-first-from-a-repeating-set
- lead: The operation part opens with a verb on nearly every line. Its first words number 517
  distinct over 11,070 lines, and a short set covers most of them: establishes, reveals,
  confirms, signals, shows, reinforces, characterizes, marks, implies, introduces, plants, sets,
  primes, supplies, recasts, builds, frames, leaves, recontextualizes, reframes, resolves,
  reminds, adds, shifts, informs, reaffirms, retroactively, tells, closes, undercuts, ties,
  withholds, opens, deepens, overturns, updates, seeds. The repeated two-word runs add sets up,
  primes the reader, reminds the reader, signals the reader, informs, tells, lets, updates the
  reader, without stating, without adding, without confirming, raising stakes.
- seen in: every answered chapter of the batch, taken together
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=moment position=1 top=50
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=moment n=2 top=50

### exploration-of-technique-mechanism-goal-co-occurrence/operation-objects-are-characters-before-world
- lead: After the reader, the words most frequent in the operation part are character names:
  twilight, rarity, dash, rainbow, applejack, fluttershy, pinkie, celestia, spike, luna, flurry;
  each of the first six is on more lines than world, plot, setting, stakes, relationship,
  feelings or history. By the readers' wording, what the operation acts on is mostly what the
  reader knows or believes about a character, and what one character knows of another.
- seen in: every answered chapter of the batch, taken together
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=moment top=60

### exploration-of-technique-mechanism-goal-co-occurrence/kind-of-words-written-as-a-contrast
- lead: The kind-of-words part is phrased as a contrast on 1,511 lines, the words taken as one
  thing rather than another, and the other names the default the reader would otherwise take:
  plain, literal, plot, ordinary, mere, scene, narration, narrated, neutral, new, present,
  continuous, incidental, spoken, sincere, dialogue, face value.
- seen in: every answered chapter of the batch, taken together
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=kind n=2 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~"rather than" view=terms col=kind n=2 top=30
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~"rather than" sample=6 seed=2 view=list

### exploration-of-technique-mechanism-goal-co-occurrence/kind-of-words-core-nouns
- lead: Apart from the contrast phrasing, the kind-of-words part converges on a few nouns,
  dialogue, narration, description, speech, action, thought, prose, aside, question, with
  recurring modifiers: private, spoken, narrated, plain, literal, italicized, physical, sensory,
  descriptive, first-person, third-person, present-tense, set off, cut off, matter-of-fact,
  unspoken. First-person words were named in 57 of the stories and present-tense words in 38.
- seen in: every answered chapter; the person and tense counts by story
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=kind top=50
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~first.person view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where kind~present.tense view=by-story

### exploration-of-technique-mechanism-goal-co-occurrence/experience-opens-with-intensity-then-feeling
- lead: The experience part often opens with an intensity or duration word before a feeling:
  quiet, mild, brief, small, sudden, mounting, light, rising, quick, visceral; or with the
  reader as subject, the reader feels, the reader senses. The feelings named most are comic,
  humor, amusement and comedy, then tension, unease, relief, curiosity, warmth, dread, suspense,
  anticipation, sympathy, surprise, intimacy, shock, embarrassment, recognition, tenderness,
  discomfort, disorientation, reassurance, deflation.
- seen in: every answered chapter of the batch, taken together
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=experience position=1 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=experience top=60
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=experience n=2 top=40

### exploration-of-technique-mechanism-goal-co-occurrence/technique-names-carry-experience-and-operation-words
- lead: The technique part, asked for the way the words are taken, often names an experience or
  an operation with it or instead. Comic is the most frequent first word of the technique part,
  and comedy words are on 1,102 technique lines over 466 chapters; operation words, reveal,
  establish, confirm, signal, reinforce, foreshadow, plant, withheld, delayed, setup, payoff,
  are on 1,014 technique lines over 482 chapters. Reveal alone is in 432 technique values, as
  delayed reveal, comic reveal, identity reveal, backstory reveal, retroactive reveal, dialogue
  reveal, name reveal, object reveal.
- seen in: the technique part across most of the chapters
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=technique position=1 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~comic|comed|humor|funny|gag|slapstick|bathos|deflation|joke|amus view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~reveal|establish|confirm|signal|reinforc|recontextual|foreshadow|plant|withheld|delayed|setup|set-up|payoff view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~reveal view=terms col=technique n=2 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~reveal sample=6 seed=1 view=list

### exploration-of-technique-mechanism-goal-co-occurrence/technique-names-built-as-function-via-carrier
- lead: 429 technique values over 276 chapters join a function to a carrier with via or
  through: irony via dialogue, reveal via, exposition via, characterization through, delivered
  through, misdirection through, via italics, via character. The carrier after via or through
  is a way the words are taken, dialogue, italics, description, and the word before it is what
  the moment does.
- seen in: the technique part in about half of the chapters
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~via|through view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~via|through view=terms col=technique n=2 top=20

### exploration-of-technique-mechanism-goal-co-occurrence/repeated-technique-names-are-a-short-list
- lead: The technique values that repeat are few: free indirect discourse, dramatic irony,
  interior monologue, italicized interior monologue or thought, scene break, section break,
  running gag, expository dialogue, interior thought, scene setting, author's note, set piece,
  double entendre, in medias res, self-aware narration, summary narration, first person,
  physical comedy, dialect rendering, direct address, tonal shift, indirect thought,
  self-address, narratorial aside, delayed reveal, withheld information, close third, comic
  deflation, eye dialect, register shift, physical or sensory description, tonal pivot, non
  sequitur, phonetic dialect, continuity callback, symbolic object, time skip, off-page, second
  person, worldbuilding aside, chapter title, direct thought. Beyond these the values are
  unique phrases: 4,257 distinct words and 19,483 distinct two-word runs over 11,070 lines.
- seen in: every answered chapter of the batch, taken together
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=technique n=2 top=60
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=terms col=technique top=60
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments view=pairs col=technique top=40

### exploration-of-technique-mechanism-goal-co-occurrence/dialogue-and-description-establish-rendered-thought-reveals
- lead: The operation's first verb shifts with the technique family. Dialogue lines open with
  establishes, then reveals, confirms, shows. Sensory and descriptive lines open with
  establishes, then none, reveals, signals, confirms, reinforces, marks. Interior monologue and
  direct or interior thought lines open with reveals, more than twice establishes, then shows,
  confirms. Free indirect lines open with establishes and reveals near even, then shows and
  characterizes.
- seen in: the dialogue, description, rendered-thought and free indirect moments
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialogue view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~sensory|descript view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"monologue|interior thought|direct thought" view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~free.indirect view=terms col=moment position=1 top=15

### exploration-of-technique-mechanism-goal-co-occurrence/irony-confirms-backstory-supplies
- lead: Dramatic irony lines open the operation with confirms ahead of establishes and reveals,
  then the reader, sets, signals, plants, reinforces, primes. Backstory, retrospective,
  exposition, flashback and summary lines open with establishes and reveals, then supplies,
  adds, informs, recontextualizes, fills, reminds, verbs that barely appear in the other
  families.
- seen in: the dramatic-irony and backstory moments
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dramatic.irony view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~backstory|retrospective|exposition|flashback|summary view=terms col=moment position=1 top=15

### exploration-of-technique-mechanism-goal-co-occurrence/breaks-signal-and-shift-gags-reinforce-closings-leave
- lead: Scene breaks, section breaks and time skips open the operation with signals and shifts,
  then none, lets, advances, moves. Running gags, callbacks, motifs and recurring devices open
  with reinforces, then none, confirms, ties, reminds, implies, links, reaffirms. Titles and
  closing lines open with leaves and signals, then none, frames, confirms, closes, retroactively,
  reframes, resolves. Withheld, delayed and off-page techniques open with reveals, confirms, the
  reader, signals, plants, implies, opens, recontextualizes, withholds, delays.
- seen in: the structural, recurring, closing and withholding moments
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~scene.break|section.break|time.skip view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"running gag|callback|motif|recurring" view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"title|closing|final line" view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~withheld|delayed|off.page view=terms col=moment position=1 top=15

### exploration-of-technique-mechanism-goal-co-occurrence/letters-frame-and-prime-dreams-reveal-and-recast-asides-signal
- lead: Letters, embedded documents, telegrams and epigraphs open the operation with establishes
  and reveals, then frames, primes, supplies, gives, delivers; frames and primes rank this high
  in no other family. Dream lines open with reveals, then signals, establishes, confirms,
  recasts, retroactively, forces, resets. Narratorial and narrator-aside lines open with signals,
  reveals, establishes, none, tells, confirms, characterizes, frames. Second-person and
  direct-address lines open with establishes, reveals, none, frames.
- seen in: the document, dream, narratorial and direct-address moments
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistol|document|telegram|epigraph view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~narratorial|narrator|omniscient|intrusion view=terms col=moment position=1 top=15
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~second.person|direct.address view=terms col=moment position=1 top=15

### exploration-of-technique-mechanism-goal-co-occurrence/technique-follows-the-operation-verb
- lead: The technique values that repeat under each operation verb differ. Under establishes:
  free indirect discourse, scene setting, interior monologue, expository dialogue, in medias
  res, summary narration, close third. Under reveals: interior monologue far ahead, italicized
  interior, free indirect, interior thought, self-address. Under confirms: dramatic irony far
  ahead. Under signals: scene break, section break, break marker, typographic scene break,
  closing line, single word. Under reinforces: running gag, eye dialect, dialect rendering and
  spelling, phonetic dialect, register contrast, comic callback. Under recasts,
  recontextualizes, reframes, retroactively and overturns: reveal via, comic reveal, delayed
  reveal, title echo, tonal pivot, author's note, identity reveal, register shift. Under
  plants, seeds and primes: dramatic irony, epigraph quotation, foreshadowing, foreboding
  aside, withheld information. Under withholds and leaves: cut-off, closing line, understated,
  unresolved or ambiguous closing, cliffhanger cutoff. Under characterizes: free indirect, comic
  banter, comic dialogue, sight gag, deadpan, dialect. Under supplies, fills and informs:
  author's note, paratextual author, embedded backstory, italicized flashback, embedded
  document, first person.
- seen in: every answered chapter, by the operation's first verb
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^establishes view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^reveals view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^confirms view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^signals view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^reinforces view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^recasts|^recontextualizes|^reframes|^retroactively|^overturns view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^plants|^seeds|^primes view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^withholds|^leaves view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^characterizes view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^supplies|^fills|^informs view=terms col=technique n=2 top=12

### exploration-of-technique-mechanism-goal-co-occurrence/recasting-operations-across-technique-families
- lead: The operations that open with recasts, recontextualizes, reframes, retroactively or
  overturns, 493 lines, come from a confessional backstory speech, a minor-character coda, an
  embedded flashback, a delayed identity reveal, narratorial psychological exposition, a
  scene-break tonal pivot, a Socratic rhetorical challenge and predatory silhouette imagery: a
  different technique family on nearly every sampled line, while the technique values that
  repeat under these verbs are reveal via, comic reveal, delayed reveal, title echo, tonal
  pivot, author's note, identity reveal, register shift, retroactive reveal, retrospective
  reveal.
- seen in: the moments whose operation revises what the reader had already taken
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^recasts|^recontextualizes|^retroactively|^overturns|^reframes sample=8 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-certain-type-of-chic-ch07
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/about-last-night-ch18
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/green-ch46
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/pax-chrysalia-ch17
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/rainbow-factory-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-gemmed-satyr-ch22
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-princess-and-the-kaiser-ch088
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/twilights-list-ch05

### exploration-of-technique-mechanism-goal-co-occurrence/intimacy-and-warmth-sit-with-rendered-thought
- lead: The experience part opens with intimacy or intimate most on interior monologue and
  thought lines, and on free indirect lines with quiet, wry and sympathetic beside it. Read the
  other way, the lines whose experience names warmth, tenderness or intimacy carry interior
  monologue, italicized interior, free indirect discourse, first person, interior thought,
  direct thought, interior aside, close third; the lines naming sympathy, pathos, grief or
  sorrow carry free indirect discourse, interior monologue, first person, confession dialogue,
  emotional or vulnerable confession, tonal pivot, backstory aside or reveal.
- seen in: the rendered-thought, free indirect and confession moments
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"monologue|interior thought|direct thought" view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~free.indirect view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~warm|tender|intimacy|intimate view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~sympathy|pathos|grief|sorrow|heartache view=terms col=technique n=2 top=12

### exploration-of-technique-mechanism-goal-co-occurrence/curiosity-with-withholding-recognition-with-callbacks-reorientation-with-breaks
- lead: Several experiences each sit with one technique family. Curiosity opens the experience
  of withheld, delayed and off-page lines, and the lines naming curiosity carry withheld
  information, withheld backstory, planted mystery, unexplained allusion, withheld identity, in
  medias res, prior event. Recognition sits with title echo, callback, canon callback,
  intertextual allusion, structural echo, identity reveal. Reorientation, abrupt and clean sit
  with scene breaks. Disorientation sits with dreams. Awe, visceral, immersive and wonder sit
  with sensory description. Shock, jolt and surprise sit with reveal via, delayed reveal,
  identity reveal, subverted expectation, retroactive reveal, tonal whiplash, scene break.
  Dread, suspense, tension and unease sit with dramatic irony far ahead of free indirect,
  interior monologue, withheld information, double entendre.
- seen in: the withholding, callback, break, dream, description and reveal moments
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~withheld|delayed|off.page view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~curiosity view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~recognition view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~scene.break|section.break|time.skip view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~sensory|descript view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~shock|jolt|surprise view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~dread|suspense|tension|unease view=terms col=technique n=2 top=12

### exploration-of-technique-mechanism-goal-co-occurrence/comic-experience-crosses-every-technique-family
- lead: Comic opens the experience part in every technique family queried, first on dialogue,
  comic-form, callback and letter lines and within the top three on monologue, description,
  irony, backstory, narratorial-aside and free indirect lines. The lines whose experience names
  humor, comedy or amusement carry dramatic irony, free indirect discourse, interior monologue,
  running gag, italicized interior, physical comedy, comic deflation, double entendre,
  self-aware narration, irony via, non sequitur. Relief and reassurance lines carry tonal
  shift, comic tonal pivot, comic banter, expository dialogue, bait and switch, comic
  deflation.
- seen in: the experience part across every technique family
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~humor|comic|amuse|laugh view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where experience~relief|reassurance view=terms col=technique n=2 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialogue view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~comic|slapstick|gag|banter|deflation|bathos view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~backstory|retrospective|exposition|flashback|summary view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~narratorial|narrator|omniscient|intrusion view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"running gag|callback|motif|recurring" view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistol|document|telegram|epigraph view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dramatic.irony view=terms col=experience position=1 top=12

### exploration-of-technique-mechanism-goal-co-occurrence/experience-follows-the-operation-verb
- lead: The experience's opening word shifts with the operation's verb. Establishes lines open
  with the reader, comic, sense, quiet, mild, warmth. Reveals lines with comic, sudden, quiet,
  sympathy, shock, intimacy, surprise. Confirms lines with comic, quiet, warmth, satisfaction,
  shock, recognition, relief. Recasts, recontextualizes, reframes, retroactively and overturns
  lines with sudden, shock, surprise, jolt, recognition. Plants, seeds and primes lines with
  mild, curiosity, quiet, unease, anticipatory, puzzlement. Withholds and leaves lines with
  suspense, curiosity, frustration, lingering, suspended. Reinforces lines with comic, humor,
  light, warmth, recognition. Signals lines with comic, quiet, brief, mounting, abrupt.
- seen in: every answered chapter, by the operation's first verb
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^establishes view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^reveals view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^confirms view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^recasts|^recontextualizes|^reframes|^retroactively|^overturns view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^plants|^seeds|^primes view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^withholds|^leaves view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^reinforces view=terms col=experience position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~^signals view=terms col=experience position=1 top=12

### exploration-of-technique-mechanism-goal-co-occurrence/free-indirect-named-in-65-stories-over-a-wide-range
- lead: Free indirect discourse is named on 238 lines in 65 of the 113 stories, 1 to 15 a story.
  What the readers put under it ranges over the narrated near-speech of an interrupted boast, a
  bystander's feelings narrated as direct access, an editorial comment folded into narration, a
  short internal exclamation blended into narration, an italicized interpretive aside, and
  fragmented sensory perception; the kind-of-words part on those lines says so each time in
  different words.
- seen in: the free indirect moments, across most of the stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~free.indirect view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~free.indirect sample=8 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/boast-busted-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/filly-fooling-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/filly-fooling-ch13
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/flying-high-falling-hard-ch30
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/green-ch67
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/lets-find-you-a-date-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-princess-and-the-kaiser-ch031
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/third-times-a-charm-ch09

### exploration-of-technique-mechanism-goal-co-occurrence/letters-thin-across-a-third-of-stories
- lead: Letters, epistolary inserts and telegrams are named on 73 lines in 33 of the 113
  stories, at most 7 lines in any story and 1 or 2 in most. They appear as a letter quoted in
  full, a fragment, a dictated correspondence given only by its sign-off, a letter shown with
  words struck through and replaced, a first-person confessional letter; their operations
  reveal what the writer represents to the recipient, confirm a prior chapter's events, advance
  the plot, or let the reader take the sending for granted.
- seen in: the letter moments, in a third of the stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistol|telegram view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~letter|epistol|telegram sample=6 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch32
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/boast-busted-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/flying-high-falling-hard-ch33
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/green-ch39
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/ill-always-be-here-for-you-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/romance-reports-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/romance-reports-ch09

### exploration-of-technique-mechanism-goal-co-occurrence/dreams-concentrate-in-two-stories
- lead: Dreams and daydreams are named on 89 lines in 32 stories, 14 of them in salvation over 7
  chapters and 11 in the-moons-apprentice over 9, the rest 1 to 6 a story. They appear as a
  nightmare recounted in dialogue, a counterfactual daydream, a domestic daydream, an unmarked
  dream sequence revealed as a dream afterwards, a prior scene reframed as a dream, and a dream
  bleeding into waking across a section break; their operations reveal a wish, reset what the
  reader took as real, or introduce a dream-viewing power as fact.
- seen in: the dream moments, concentrated in two stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dream sample=6 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/cuddling-ch12
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/flying-high-falling-hard-ch25
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/promises-ch05
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/salvation-ch18
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/those-blue-wings-ch10
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/you-make-my-whole-life-worthwhile-ch04

### exploration-of-technique-mechanism-goal-co-occurrence/epigraphs-in-one-story
- lead: Epigraphs are named on 52 lines in 8 stories, 39 of them in a-delicate-balance, one in
  each of its 36 chapters; the epigraph's operation there frames the chapter's material as
  belonging to a tradition and cues how to weigh the scene that follows.
- seen in: the epigraph moments, nearly all in one story
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~epigraph view=by-story
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch03

### exploration-of-technique-mechanism-goal-co-occurrence/dialect-in-half-the-stories
- lead: Dialect, phonetic and eye-dialect rendering is named on 150 lines in 52 of the 113
  stories, 1 to 3 lines in most and 12 in filly-fooling. Its operation is none more often than
  any verb, then reinforces, marks, characterizes, differentiates, distinguishes.
- seen in: the dialect moments, in half of the stories
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialect|phonetic view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~dialect|phonetic view=terms col=moment position=1 top=15

### exploration-of-technique-mechanism-goal-co-occurrence/author-notes-and-titles-read-as-moments-and-as-unplaced
- lead: Author's notes and paratextual address are named as moments on 112 lines in 46 stories,
  15 of them in clocktower-society-your-safe-word-is-law; their operation is none, or supplies,
  informs or reveals facts about production, the series and the next chapter; their experience
  is a break out of the fiction. The readers also set author's notes under unplaced in 98
  chapters, chapter titles as moments in 119 chapters and under unplaced in 137, and scene-break
  dividers as moments in 157 chapters and under unplaced in 70: the same three things were
  placed in both lists by different readers.
- seen in: the author's note, title and divider lines of both lists
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~"author note|author's note|paratext" view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~author view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~author view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~title view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~title view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~scene.break|section.break view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~scene.break|section.break|divider|horizontal.rule view=cites
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/about-last-night-ch06
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/boast-busted-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/crisis-on-two-equestrias-ch11
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/get-your-act-together-rainbow-dash-ch01
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/inner-strength-ch28
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/promises-ch07
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/rolling-with-the-punches-ch01

### exploration-of-technique-mechanism-goal-co-occurrence/direct-address-in-a-quarter-of-stories
- lead: Second-person and direct address are named on 57 lines in 27 stories, 9 in
  clocktower-society-your-safe-word-is-law, 6 in salvation, 4 each in a-delicate-balance and
  the-parent-trap, 1 or 2 elsewhere. Its operations open with establishes, reveals, none,
  frames; its experiences with the reader, jolt, playful, shift, wry, unsettling.
- seen in: the second-person and direct-address moments
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~second.person|direct.address view=by-story
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~second.person|direct.address view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where technique~second.person|direct.address view=terms col=experience position=1 top=12

### exploration-of-technique-mechanism-goal-co-occurrence/operations-name-other-chapters-and-the-show
- lead: Each reader had one chapter and nothing else of the story, and still the operation part
  names an earlier, prior or previous chapter on 36 lines in 31 chapters, opening with confirms,
  recaps, reminds, ties, updates, corrects; a later, next or coming chapter on 65 lines in 63
  chapters, opening with leaves, sets, informs, signals, withholds; and the show, an episode,
  canon or the series on 110 lines in 94 chapters, opening with establishes, reveals, signals,
  confirms, supplies, situates, links. The earlier-chapter lines sampled were letters
  condensing prior events, callback dialogue, a plot-correcting revelation and a
  character-narrated flashback; the later-chapter lines were closing images, closing questions,
  author's notes and a sneak-peek insert; the show lines were canon callbacks, an ironic epithet
  on a canonical trait, and author's notes placing the story in a series.
- seen in: the operation part where it reaches outside the chapter that was read
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"earlier chapter|previous chapter|prior chapter|earlier chapters|chapters earlier|chapters ago|preceding chapter" view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"earlier chapter|previous chapter|prior chapter|earlier chapters|chapters earlier|chapters ago|preceding chapter" view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"earlier chapter|previous chapter|prior chapter|earlier chapters|chapters earlier|chapters ago|preceding chapter" sample=6 seed=1 view=list
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"later chapter|next chapter|future chapter|chapters later|later in the story|subsequent chapter|coming chapter|next installment" view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"later chapter|next chapter|future chapter|chapters later|later in the story|subsequent chapter|coming chapter|next installment" view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"later chapter|next chapter|future chapter|chapters later|later in the story|subsequent chapter|coming chapter|next installment" sample=6 seed=1 view=list
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"the show|episode|canon|series|television" view=cites
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"the show|episode|canon|series|television" view=terms col=moment position=1 top=12
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=moments where moment~"the show|episode|canon|series|television" sample=6 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/about-last-night-ch06
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/clocktower-society-your-safe-word-is-law-ch10
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/dashs-new-mom-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/ill-always-be-here-for-you-ch04
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/inner-strength-ch28
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/romance-reports-ch09
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-appledash-project-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-last-train-home-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-moons-apprentice-ch19
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/unexpected-confessions-ch15
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/unexpected-confessions-ch25

### exploration-of-technique-mechanism-goal-co-occurrence/unplaced-holds-titles-notes-dividers-artifacts-and-stray-details
- lead: The 1,362 unplaced lines, 1 to 5 a chapter, hold chapter titles and their relation to
  the chapter, author's notes, scene-break dividers and horizontal rules, real-world and
  outside-story references, apparent typos, grammatical slips and formatting artifacts such as
  stray asterisks and dangling footnote marks, pervasive italic emphasis, character and place
  names, cutie marks, dialect spellings, punning place names, coined in-world terms, throwaway
  jokes and background world details, and details the readers called stray or unexplained.
- seen in: the unplaced lists across the answered chapters
- query:
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=terms col=c1 top=50
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced view=terms col=c1 n=2 top=40
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~"world|lore|detail about|background|worldbuilding" sample=8 seed=1 view=list
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~typo|slip|formatting|artifact sample=5 seed=1 view=list
  - rq1 batch=exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2 answered=602 field=unplaced where c1~title sample=5 seed=1 view=list
- cites:
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/a-delicate-balance-ch10
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/crisis-on-two-equestrias-ch14
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/every-little-bit-ch02
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/green-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/inner-strength-ch27
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/on-a-cross-and-arrow-ch03
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-princess-and-the-kaiser-ch078
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/the-princess-and-the-kaiser-ch088
  - exploration-of-technique-mechanism-goal-co-occurrence/02-chapters-directions-2/where-earth-meets-sky-ch01

## Proposed questions

- Whether the moments the chapter readers gave no operation, comic beats, dialect and typographic emphasis, do an operation on what the reader knows at the scale of a whole story that a reader of one chapter cannot see.
- Whether the operations the readers wrote as reaching an earlier chapter, a later chapter or the show hold when the chapters or episodes they name are read together with the chapter.
- Whether the operation verbs the readers converge on, establishes, reveals, confirms, signals, reinforces, plants, primes, recasts, leaves, are distinct operations on the reader's model of the fabula or one operation met at different points of a story's telling.
- Whether characterizing and reinforcing an established trait are operations on what the reader knows or believes, given that the readers wrote them both as an operation and as what a moment does instead of one.
- Whether the words that open the experience part in every technique family, comic, quiet, mild, mounting, brief, sudden, are experiences or an intensity and register axis apart from the experience.
- Whether technique values built as a function joined to a carrier, irony via dialogue, reveal through description, and the repeated names dramatic irony and delayed reveal, name a way the words are taken or an operation together with its carrier.
- Whether the default reading the kind-of-words part contrasts against, plain, literal, plot, scene, narration, sincere, is the same across the stories or is set by each story.
- Whether letters, dreams, epigraphs and author's notes are chosen once for a story rather than at a moment, given that epigraphs sit in one story's every chapter, dreams concentrate in two stories and letters spread thinly across a third.
