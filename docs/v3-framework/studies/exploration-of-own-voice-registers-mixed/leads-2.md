# exploration-of-own-voice-registers-mixed — leads

- items with results: 641 of 641
- written by: a session opened on 2026-09-27 for this write-up, which did not plan the study,
  write its directions, build its itemizer or read its pilot, and did not open the study's
  earlier leads file; it read the definition, directions-1, the index head, the tally and the
  question list, and read the results only through the results tool's passages and shifts
  fields, so the registers and whole lines, which the tool does not read, were not seen

## Leads

### exploration-of-own-voice-registers-mixed/leads-2/summary-is-the-base-register
- lead: The readers most often named a summary register, plot summary, narrative summary or
  summary narration, in third person and present tense, and most register changes they
  recorded were changes out of it and back into it; around it they named expository and
  worldbuilding exposition, analytical and thematic commentary, quoted dialogue, asides,
  headings and planning notes.
- seen in: scene notes, subject entries and chapter notes across the batch
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages view=terms col=register top=80
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages view=terms col=register n=2 top=60
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts view=terms col=from top=40
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts view=terms col=into top=40

### exploration-of-own-voice-registers-mixed/leads-2/passages-mostly-apart-some-run-in
- lead: Most passages were marked as standing apart from their neighbours; about one in five was
  marked run-in, running into a passage in another register inside one sentence or with no
  break. The run-in passages were most often parenthetical and evaluative asides, quoted or
  unquoted speech, first-person or second-person voice, and summary continuing across the join.
- seen in: scene notes and subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages view=terms col=joined top=10
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where joined~run view=terms col=register n=2 top=40
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where joined~run view=terms col=marks top=40
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where joined~run sample=25 seed=1 view=list

### exploration-of-own-voice-registers-mixed/leads-2/change-marked-by-person-and-tense
- lead: The marks the readers gave for a register change were most often a change of grammatical
  person, between third, first and second, and a change of tense between present and past,
  followed by a new sentence, line or paragraph, an opening or closing parenthesis, and
  quotation marks; a change inside one sentence with no break was also named.
- seen in: scene notes and subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts view=terms col=marks top=60
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts view=terms col=marks n=2 top=50

### exploration-of-own-voice-registers-mixed/leads-2/tense-shift-between-history-and-system
- lead: Tense changes were read at the turn between a past-tense account of events or backstory
  and a present-tense statement of how something is or works, in both directions and sometimes
  repeatedly within one note, and at the turn from present-tense scene summary to a past-tense
  explanation of background.
- seen in: scene notes and lore and history subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where marks~tense view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where marks~tense sample=12 seed=3 view=list
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-3
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-440
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-2
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-240

### exploration-of-own-voice-registers-mixed/leads-2/summary-into-character-speech
- lead: Third-person summary of a scene was read turning into a character's own speech, quoted or
  unquoted, in first or second person, often with no reporting verb or framing and inside the
  same sentence, and turning back into summary after it.
- seen in: scene notes, rarely subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where from~summar|narrat where into~dialogue|speech|voic|quot view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where from~summar|narrat where into~dialogue|speech|voic|quot sample=12 seed=3 view=list
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-13
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-47
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-201
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-339
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-365

### exploration-of-own-voice-registers-mixed/leads-2/first-person-author-and-character
- lead: The first person the readers marked was of two kinds: a character's voiced line inside a
  scene's summary, and the writer's own "I" or "we" asking how to frame a scene, stating an
  intent to portray something, hedging with "I think" or "I believe", or weighing an
  alternative; both appeared inside notes otherwise in third-person summary or exposition.
- seen in: scene notes and subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where marks~"first person|\\bI\\b|\\bwe\\b" sample=20 seed=2 view=list
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~author where marks~"\\bI\\b|first.person|\\bmy\\b" view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~author where marks~"\\bI\\b|first.person|\\bmy\\b" sample=10 seed=4 view=list
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-7
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-162
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-182
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-2
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-414
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-420

### exploration-of-own-voice-registers-mixed/leads-2/summary-into-commentary-and-back
- lead: Summary of events or reported speech was read turning into evaluative, analytical or
  thematic commentary, often at a contrastive "but" or a general timeless claim, sometimes
  within one paragraph with no break; the reverse turn, from analysis back to a plain event or
  exchange, was also read.
- seen in: scene notes and subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where from~summar|narrat|report where into~analy|themat|interpret|commentar|evaluat view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where from~summar|narrat|report where into~analy|themat|interpret|commentar|evaluat sample=12 seed=3 view=list
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where from~analy|themat|interpret|commentar|evaluat|exposit where into~summar|narrat sample=8 seed=4 view=list
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-89
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-160
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-303
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-447
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-154
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-13

### exploration-of-own-voice-registers-mixed/leads-2/aphoristic-statements
- lead: A register of maxims, slogans and named laws or formulas was read, sometimes quoted and
  attributed to a character, sometimes unattributed, standing apart as its own line or running
  on from a summary or exposition.
- seen in: scene notes and subject entries on ideas and institutions
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~aphor|maxim|epigram view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~aphor|maxim|epigram sample=10 seed=4 view=list
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~aphor|maxim|epigram view=terms col=joined top=5
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-159
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-352
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-223
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-408
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-409

### exploration-of-own-voice-registers-mixed/leads-2/parenthetical-asides
- lead: Asides set in parentheses were read inside summary or exposition, carrying a hedge, a
  hope, an irony, a hidden cause, a backstory detail, a joke idea or a doubt about realism;
  more of them were marked run-in than apart.
- seen in: scene notes and subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~aside where marks~parenthes view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~aside where marks~parenthes sample=10 seed=4 view=list
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~aside where marks~parenthes view=terms col=joined top=5
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-27
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-196
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-352
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-28
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-229

### exploration-of-own-voice-registers-mixed/leads-2/planning-notes-and-self-queries
- lead: A planning register was read: questions to self about when or whether to reveal or
  include something, modal proposals with "should" or "has to", imperatives, and to-do
  reminders; it mostly stood apart as its own note or passage, and sometimes sat in a
  parenthesis inside lore or summary.
- seen in: scene notes, subject entries and several chapter notes
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~planning|directive|brainstorm|to-do|todo|memo view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~planning|directive|brainstorm|to-do|todo|memo sample=12 seed=4 view=list
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~planning|directive|brainstorm|to-do|todo|memo view=terms col=joined top=5
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-185
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-284
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-324
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-38
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-409
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/chapter-7-generosity

### exploration-of-own-voice-registers-mixed/leads-2/questions-as-a-register
- lead: Questions were read as a register of their own: open speculation about the world or a
  character, brainstorm questions offering alternatives, rhetorical questions voicing
  incredulity, and questions addressed to "you"; most stood apart.
- seen in: scene notes and subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~question|inquir|wonder view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~question|inquir|wonder sample=10 seed=4 view=list
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=passages where register~question|inquir|wonder view=terms col=joined top=5
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-47
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-95
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-157
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-257
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-258

### exploration-of-own-voice-registers-mixed/leads-2/second-person-address
- lead: Second person was read in three uses: a character addressing another inside a scene's
  summary, a generic "you" explaining how a system works or what one needs, and an address to
  the reader of the note; the readers marked changes into and out of it against third-person
  summary or exposition.
- seen in: scene notes and subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where marks~"second.person|\\byou\\b" view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where marks~"second.person|\\byou\\b" sample=10 seed=4 view=list
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-175
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-254
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-296
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-311
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-402

### exploration-of-own-voice-registers-mixed/leads-2/label-then-prose
- lead: A bare heading, tag or title opening a note was read giving way to full prose after a
  dash, colon or line break: to encyclopedic narrative, definition, analysis, analogy or an
  imperative.
- seen in: subject entries on lore, institutions and ideas more than scene notes
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where from~label|heading|tag view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where from~label|heading|tag sample=12 seed=3 view=list
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-41
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-278
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-285
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-287
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-614

### exploration-of-own-voice-registers-mixed/leads-2/turns-to-canon-and-real-world
- lead: Changes were read at turns from story summary or exposition to the show's canon or its
  episodes, to a named real-world historical case or statistic, or to the note's readers, each
  marked by a phrase naming canon, history or real life.
- seen in: scene notes and subject entries
- query:
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where marks~"real.world|the show|episode|canon" view=cites
  - rq1 batch=exploration-of-own-voice-registers-mixed/01-own-voice-loci answered=641 field=shifts where marks~"real.world|the show|episode|canon" sample=10 seed=4 view=list
- cites:
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-43
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-274
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/pp-287
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-22
  - exploration-of-own-voice-registers-mixed/01-own-voice-loci/subject-429

## Proposed questions

- In the notes credited to Brian's own voice, how often does the writer's own first person
  (framing questions, intents, hedges) share a note with third-person summary of story events,
  and is it set apart from it?
- Do the register changes in the notes credited to Brian's own voice differ between scene
  notes and subject entries?
- Where a character's first-person speech appears unquoted inside a note's summary, can a
  reader tell it from the writer's own first person by the words alone?
