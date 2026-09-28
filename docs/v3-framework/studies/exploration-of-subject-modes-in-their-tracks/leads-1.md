# exploration-of-subject-modes-in-their-tracks — leads

- items with results: 213 of 213
- written by: a session opened for this write-up alone, which did not plan the study, write its
  directions, build its itemizer or read its pilot; it read the batch's definition, index, calls
  and tally, the directions-3 the definition names, the question list and the skill, read the
  claims of the results through the results tool, and read the whole blocks of some results by
  their item tokens; it did not see the item bodies, the track definitions or the plan

## Leads

### exploration-of-subject-modes-in-their-tracks/leads-1/claims-read-outside-their-tracks-mode
- lead: Of 3751 claim lines, 1145, on 184 of the 213 subjects, were read in a mode other than
  their track's. By mode those lines are History 326, Civilization 218, Ontology 212,
  Characterization 140, ThematicEvidence 64, Analogies 47, Canon 39, NotesToSelf 29, PageDesign
  23, WorldInference 18, NarrativeArchitecture 15, Allegories 8, outside 6. Their placements are
  unmatched 556, empty 338, absent 102, and another note's id 149.
- seen in: the claims of every kind of subject, across subject-wide and scene-link notes
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims view=terms col=placement top=20
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where placement~^(unmatched|empty|absent|[0-9]+)$ view=cites

### exploration-of-subject-modes-in-their-tracks/leads-1/storytelling-plan-holds-history-civilization-ontology
- lead: On the Storytelling Plan track, whose own-mode lines were read as NarrativeArchitecture,
  155 of 196 lines, on 35 subjects, were read in another mode: History 65, Civilization 34,
  Ontology 26, Characterization 11, and a few in six other modes. Most of them were placed
  unmatched (101), then empty (31) and absent (10). The lines are timelines of events, how
  institutions and economies run, biological and social rules, and claims about what named
  characters are like.
- seen in: organization, civilizational-system and faction subjects above all
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Storytelling Plan" where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Storytelling Plan" view=terms col=placement top=5
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Storytelling Plan$" where placement~^(unmatched|empty|absent|[0-9]+)$ sample=40 seed=1 view=list
- cites:
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-53
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-146

### exploration-of-subject-modes-in-their-tracks/leads-1/usage-plan-holds-ontology-civilization-history
- lead: On the Usage Plan track, 136 of 155 lines, on 23 of the 27 subjects that carry it, were
  read in another mode: Ontology 50, Civilization 35, History 26, ThematicEvidence 11, and a few
  in five other modes. Empty is the most frequent placement on this track (60), ahead of
  unmatched (56); the own-mode placement, same, is 19. The lines state how a device or spell
  works, what it is named and why, who built it and for whom, and what happened when it was used.
- seen in: technology subjects, such as weapons, spells, devices and currencies
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Usage Plan" where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Usage Plan" view=terms col=placement top=5
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Usage Plan$" where placement~^(unmatched|empty|absent|[0-9]+)$ sample=30 seed=1 view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Usage Plan$" where placement~^(unmatched|empty|absent|[0-9]+)$ view=cites

### exploration-of-subject-modes-in-their-tracks/leads-1/demonstration-plan-holds-civilization-with-no-civilization-track
- lead: On the Demonstration Plan track, 54 of 64 lines were read in another mode, Civilization
  20, Ontology 16 and History 10 first. Absent is its most frequent placement (23): all 23
  absent lines are Civilization or Characterization, on subjects that hold no Civilization or
  Characterization track. They state how societies and industries use a magic or a biology, the
  in-world names and cultural readings given to it, and, on one subject, what named dragons feel
  and want.
- seen in: world-law and biology subjects, such as kinds of magic, a magical effect and a
  species' biology
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Demonstration Plan" where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Demonstration Plan" view=terms col=placement top=5
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Demonstration Plan$" where placement~^absent$ view=list

### exploration-of-subject-modes-in-their-tracks/leads-1/character-appearance-plan-holds-history-characterization
- lead: On the Character Appearance Plan track, 77 of 107 lines, on 26 of the 32 subjects that
  carry it, were read in another mode: History 31 and Characterization 22 first, then PageDesign
  7, ThematicEvidence 6 and a few others. Unmatched (39) and empty (33) outnumber same (30). The
  lines report what a character did, where they worked and what happened to them, and state what
  a character believes, wants or fears.
- seen in: character subjects
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Character Appearance Plan" where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Character Appearance Plan" view=terms col=placement top=5
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Character Appearance Plan$" where placement~^(unmatched|empty|absent|[0-9]+)$ sample=25 seed=1 view=list

### exploration-of-subject-modes-in-their-tracks/leads-1/reader-tracks-hold-fabula-and-character-psychology
- lead: On the three reader tracks, Reader Opinion, Reader Opinion Plan and Reader Understanding
  Plan, 149 of 290 lines, on 46 of the 62 subjects that carry them, were placed other than same.
  On Reader Opinion the other modes are History 17, Ontology 15, Civilization 13 and
  ThematicEvidence 7 first; on Reader Opinion Plan, Characterization 14 first; on Reader
  Understanding Plan, History 14, Ontology 12 and Civilization 9 first. In several notes the
  readers split what the reader is to learn, read as NarrativeArchitecture, from the thing to be
  learned, a past event, a rule or a character's true motive, read in its own mode.
- seen in: character subjects for Reader Opinion Plan; organization, system and world-law
  subjects for the other two
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Reader Opinion$" where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Reader Opinion Plan" where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Reader Understanding Plan" where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(Reader Opinion|Reader Opinion Plan|Reader Understanding Plan)$" where placement~^(unmatched|empty|absent)$ sample=30 seed=2 view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(Reader Opinion|Reader Opinion Plan|Reader Understanding Plan)$" where placement~^(unmatched|empty|absent|[0-9]+)$ view=cites
- cites:
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-30

### exploration-of-subject-modes-in-their-tracks/leads-1/characterization-track-holds-history
- lead: On the Characterization track, 29 lines on 23 subjects were read as History: where and
  when a character was born or raised, their age at an event, their appearance and cutie mark,
  what they saw or lived through, and policies a character set. 7 of the 29 were placed at a
  note id on the same subject, as the same content in a track of History mode; the rest
  unmatched or empty.
- seen in: character subjects
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~^Characterization$ where mode~^History$ view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~^Characterization$ where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
- cites:
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-1

### exploration-of-subject-modes-in-their-tracks/leads-1/history-tracks-hold-characterization
- lead: On the Backstory, Life Phases and History tracks, 32 lines on 18 subjects were read as
  Characterization: what a character believes, dislikes, wants or feels, and the psychological
  cause given for what they did. 5 of the 32 were placed at a note id on the same subject's
  Characterization track, as the same content; most of the rest unmatched. Together with the
  Characterization track's History lines, the two directions of this exchange sit on the same
  subjects in several items.
- seen in: character subjects, and a few organization subjects on their History track
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(Backstory|Life Phases|History)$" where mode~^Characterization$ view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(Backstory|Life Phases|History)$" where mode~^Characterization$ view=cites
- cites:
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-1

### exploration-of-subject-modes-in-their-tracks/leads-1/ontology-and-civilization-tracks-exchange-content
- lead: On the System Ontology and Function tracks, whose own-mode lines were read as Ontology,
  57 lines on 25 subjects were read as Civilization: how an institution, trade or custom works
  and why agents made it so. On the Causality of Creation track, whose own-mode lines were read
  as Civilization, 37 lines on 15 subjects were read as History (21) or Ontology (16): past
  events and settlement, and biological, climatic or magical rules.
- seen in: organization, civilizational-system and technology subjects
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(System Ontology|Function)$" where mode~^Civilization$ sample=20 seed=1 view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(System Ontology|Function)$" where mode~^Civilization$ view=cites
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Causality of Creation$" where mode~^(History|Ontology)$ sample=16 seed=1 view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Causality of Creation$" where mode~^(History|Ontology)$ view=cites

### exploration-of-subject-modes-in-their-tracks/leads-1/characterization-on-subjects-with-no-characterization-track
- lead: 27 lines on 17 subjects were read as Characterization and placed absent: the subject
  holds no track of that mode. Most of them state what a named character, one who is not the
  subject, feels, believes or is driven by, such as a ruler, a founder or an inventor; the rest
  state how a group or class feels or thinks.
- seen in: organization, technology, civilizational-system and world-law subjects, in their
  plan, reader, ontology and history tracks
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where mode~^Characterization$ where placement~^absent$ view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where mode~^Characterization$ where placement~^absent$ view=cites

### exploration-of-subject-modes-in-their-tracks/leads-1/ontology-and-civilization-on-subjects-with-no-such-track
- lead: 102 lines on 51 subjects were placed absent: Civilization 43, Ontology 29,
  Characterization 27, outside 3. The Ontology lines state rules of magic, biology or law on
  subjects that hold no Ontology track, many of them character subjects, in their
  Characterization, Character Development, The Spark or Garden Notes tracks.
- seen in: character subjects for the Ontology lines; world-law subjects for most Civilization
  lines
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where placement~^absent$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where placement~^absent$ view=cites
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where mode~^Ontology$ where placement~^absent$ sample=12 seed=1 view=list

### exploration-of-subject-modes-in-their-tracks/leads-1/modes-whose-tracks-are-empty
- lead: 338 lines on 114 subjects were placed empty: the subject has a track of that mode and
  every such track is shown with no notes. By mode: History 93, Civilization 61,
  ThematicEvidence 31, Characterization 28, Ontology 26, NotesToSelf 24, Canon 22, Analogies 18,
  PageDesign 15, WorldInference 13, and fewer in three others.
- seen in: subjects of every kind
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where placement~^empty$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where placement~^empty$ view=cites

### exploration-of-subject-modes-in-their-tracks/leads-1/reference-tracks-hold-parallels-plans-and-decisions
- lead: On the Source Material References and Canon References tracks, whose own-mode lines were
  read as Canon, 70 lines on 32 subjects were read in another mode: real-world parallels and
  etymologies as Analogies, what the character does in TLTT as NarrativeArchitecture, the
  author's lore decisions as NotesToSelf, and past events, rules and character psychology in
  their own modes.
- seen in: character subjects for Source Material References; organization and system subjects
  for Canon References
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Source Material" where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Canon References$" where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(Source Material References|Canon References)$" where placement~^(unmatched|empty|absent|[0-9]+)$ sample=25 seed=1 view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(Source Material References|Canon References)$" where placement~^(unmatched|empty|absent|[0-9]+)$ view=cites

### exploration-of-subject-modes-in-their-tracks/leads-1/analogies-and-allegories-carry-in-world-content
- lead: On the Analogies and Allegories tracks, 63 lines on 35 subjects were read in another
  mode. Beside the real-world comparison, a note states the in-world system, rule or event the
  comparison is drawn to, read as Civilization, Ontology or History; the two tracks also each hold
  lines read in the other's mode, and a few storytelling aims read as NotesToSelf.
- seen in: organization, civilizational-system and technology subjects
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~^Analogies$ where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~^Allegories$ where placement~^(unmatched|empty|absent|[0-9]+)$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~^(Analogies|Allegories)$ where placement~^(unmatched|empty|absent|[0-9]+)$ sample=16 seed=1 view=list

### exploration-of-subject-modes-in-their-tracks/leads-1/garden-notes-hold-fabula
- lead: On the Garden Notes track, whose own-mode lines were read as NotesToSelf, 25 of 69 lines
  were read in another mode: History 8, Ontology 5, Characterization 4 and a few others. They
  are events of a character's family or an organization's origin, rules of the world stated as
  design answers, and real-world parallels.
- seen in: character subjects and a few world-law and organization subjects
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^Garden Notes$" where placement~^(unmatched|empty|absent|[0-9]+)$ view=list

### exploration-of-subject-modes-in-their-tracks/leads-1/scene-link-page-tracks-restate-history
- lead: On the scene-link tracks read as PageDesign, Revelation and Character Actions above all,
  17 lines on 13 subjects were read as History, Characterization, Civilization or Ontology. 7 of
  them were placed at a note id, as the same content held in a track of its mode on the same
  subject, six past events and one rule.
- seen in: the scene-link notes of character subjects and a few organization subjects
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(Revelation|Character Actions|Disclosure|Demonstration|Stated Principles)$" where mode~^(History|Ontology|Civilization|Characterization)$ view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~"^(Revelation|Character Actions|Disclosure|Demonstration|Stated Principles)$" view=terms col=placement top=10
- cites:
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-31

### exploration-of-subject-modes-in-their-tracks/leads-1/same-content-in-two-tracks
- lead: 149 lines on 78 subjects were placed at another note's id: the same content in a track
  of its mode on the same subject. By mode: Ontology 49, History 42, Characterization 13,
  ThematicEvidence 12, Civilization 11, and fewer in six others. On one subject a single rule
  was read in three notes on three tracks. On a few subjects one note held several claims each
  matched to a different note.
- seen in: subjects of every kind, most often organizations, technologies and world laws for
  Ontology and characters for History
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where placement~^[0-9]+$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where placement~^[0-9]+$ view=list
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where placement~^[0-9]+$ view=cites
- cites:
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-53

### exploration-of-subject-modes-in-their-tracks/leads-1/same-content-in-two-tracks-of-another-mode
- lead: In their whole blocks, readers of several subjects reported the same content stated in two
  notes neither of which sits in a track of that content's mode, for instance a character's
  belief in two NarrativeArchitecture tracks, a past fact in a ThematicEvidence and a
  NarrativeArchitecture track, and a rule stated twice across reader tracks; each was placed
  unmatched, and the placement part has no value that records the pair.
- seen in: the whole blocks of a character subject, an army and a group of tribes
- cites:
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-30
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-121
  - exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3/subject-146

### exploration-of-subject-modes-in-their-tracks/leads-1/same-named-tracks-read-in-two-modes
- lead: Lines on tracks named Activities and Usage were placed same under History on 19 subjects
  and under PageDesign on 4, 36 lines against 7; one organization subject has Activities lines
  placed same under both.
- seen in: organization and technology subjects
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~^(Activities|Usage)$ where placement~^same$ view=terms col=mode position=1
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~^(Activities|Usage)$ where mode~^PageDesign$ where placement~^same$ view=list

### exploration-of-subject-modes-in-their-tracks/leads-1/things-read-outside-all-modes
- lead: 6 lines were read outside all twelve modes: a cross-reference to the story installment
  where the content appears, stated in two notes on two tracks; an author's decision on where in
  the story to place real-world traditions; and three whose words after the colon name one of
  the twelve, NotesToSelf once and an Ontology-style rule or norm twice.
- seen in: a civilizational-system subject, an organization, a food subject and a group of
  characters
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where mode~^outside view=list

### exploration-of-subject-modes-in-their-tracks/leads-1/unassigned-notes
- lead: 5 lines on 2 subjects came from notes with no track: canon precedents read as Canon,
  author's remarks read as NotesToSelf, and a staged baking exchange read as PageDesign.
- seen in: a world-law subject and a character subject
- query:
  - rq1 batch=exploration-of-subject-modes-in-their-tracks/03-subjects-directions-3 answered=213 field=claims where track~^none view=list

## Proposed questions

- When a working-plan note on one subject is written in a mode that subject's type gives no track for, is what it states about that subject or about another subject?
- When a working-plan note in a NarrativeArchitecture track states fabula, is the fabula stated as what the reader is to learn or experience, or stated on its own?
- How often does a working-plan scene-link note restate a fact that a subject-wide note on the same subject already states?
- On a working-plan subject, is the same content stated in two notes neither of which sits in a track of that content's mode?
- Do working-plan track definitions that share a name on different subject types declare different track types?
