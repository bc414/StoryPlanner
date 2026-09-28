# exploration-of-notes-mix-cognitive-modes — leads

- items with results: 2065 of 2066
- written by: the Claude Code session that ran the autonomous study campaign of 2026-09-26 to 27,
  which planned this study, wrote its directions, built or chose its itemizer and read its pilot,
  and drew these leads from the batch's results; written before the rule that leads are written by
  a fresh session

## Leads

### exploration-of-notes-mix-cognitive-modes/leads-1/half-the-claims-sit-outside-their-own-tracks-mode
- lead: Over 2,065 of the plan's 2,066 tracked notes the readers named 4,862 claims — a median of 2 per
  note, 12 at the most. Asked of each whether the mode it is written in is the one its own track type
  declares, they answered yes for 2,398 lines, no — it is another of the ten — for 1,671, and outside all
  ten for 783. So a little under half of what the plan's tracked notes say is written in the mode its
  track declares, a third is written in another of the ten, and a sixth is written in no mode the plan
  declares at all. 10 lines of the 4,862 hold the wrong number of parts.
- seen in: tracked notes across every subject type and every owner kind in the working plan
- query:
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims view=health
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims view=terms col=fits top=8

### exploration-of-notes-mix-cognitive-modes/leads-1/which-modes-the-claims-are-written-in
- lead: Of the 4,862 claims: 1,774 over 871 notes are History, an in-universe fact reported; 699 over 416
  are Characterization; 765 over 349 are outside all ten; 369 over 222 are Canon; 314 over 201 Analogies;
  242 over 149 ThematicEvidence; 242 over 166 NarrativeArchitecture; 140 over 89 PageDesign; 134 over 72
  Allegories; 94 over 61 NotesToSelf; and 64 over 46 WorldInference, the fewest of the ten by a wide
  margin. 25 lines over 12 notes carry a name of the reader's own that is neither one of the ten nor
  "outside".
- seen in: tracked notes across the working plan
- query:
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims view=terms col=mode top=22
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~^History$ view=cites
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~^Characterization$ view=cites
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~^WorldInference$ view=cites
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~outside view=cites

### exploration-of-notes-mix-cognitive-modes/leads-1/what-the-claims-outside-all-ten-modes-are
- lead: The 765 claims the readers placed outside all ten modes are overwhelmingly one thing, and they
  named it almost identically each time: 570 of the 765 lines carry the word ontology and 503 the word
  rule, most as "world-rule ontology"; 175 add "god mode". They are standing statements of how the world
  works rather than events a historian could report: crystals are stored magic and transparent
  crystallization is stored friendship; harmony and regulated ambition can coexist without conflict as a
  rule of the system; parts made by an artisan's own machines are enchanted when the artisan uses them;
  the caste system is not fully rigid and has some mobility; literacy in the shared writing system is
  restricted to jaegers. Beside them sit definitions and name glosses (Panzer Haut as "Armor Skin") and
  bare world quantities (Cloudbury has 4.7 million griffons).
- seen in: tracked notes on world laws, civilizational systems, technologies and organizations
- query:
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~outside view=terms col=mode top=18
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~outside sample=12 seed=9 view=list

### exploration-of-notes-mix-cognitive-modes/leads-1/four-tracks-whose-content-is-entirely-outside-the-ten-modes
- lead: Read against the track names in the index's description column, the claims outside all ten modes
  are not spread evenly: System Ontology has 348 of its 381 claim lines outside, Function 222 of 238,
  World Truth 110 of 118, and Causality of Creation 24 outside with a further 82 in another of the ten —
  so on those four tracks, taken together with the other-mode claims, effectively every line sits outside
  the mode its track declares. These are the tracks of world laws, civilizational systems and
  technologies.
- seen in: the System Ontology, Function, World Truth and Causality of Creation tracks
- cites:
  - exploration-of-notes-mix-cognitive-modes/01-tracked-notes/note-205
  - exploration-of-notes-mix-cognitive-modes/01-tracked-notes/note-1168
  - exploration-of-notes-mix-cognitive-modes/01-tracked-notes/note-1638
  - exploration-of-notes-mix-cognitive-modes/01-tracked-notes/note-2420
  - exploration-of-notes-mix-cognitive-modes/01-tracked-notes/note-1091

### exploration-of-notes-mix-cognitive-modes/leads-1/the-planning-tracks-whose-content-belongs-to-another-mode
- lead: Read the same way, the tracks with the most claims in another of the ten modes are the planning
  tracks: Storytelling Plan 236 of 253 lines, Usage Plan 145 of 157, Character Appearance Plan 99 of 122,
  Causality of Creation 82 of 131, Reader Opinion 122 of 171, Reader Opinion Plan 78 of 131, Reader
  Understanding Plan 78 of 123. At the other end the history and invention tracks stay in their mode:
  Historical Events 3 of 127 lines elsewhere, Invention 9 of 112, History 12 of 132, Canon References 12
  of 93, Source Material References 24 of 165, Characterization 36 of 239, Backstory 66 of 382.
- seen in: the tracks named, across subjects of every type
- cites:
  - exploration-of-notes-mix-cognitive-modes/01-tracked-notes/note-241
  - exploration-of-notes-mix-cognitive-modes/01-tracked-notes/note-475

### exploration-of-notes-mix-cognitive-modes/leads-1/claims-drift-toward-history-and-characterization
- lead: Of the 1,671 claims written in another of the ten modes than their own track's, 900 land in
  History and 398 in Characterization — four fifths between them. Then Canon 147, Analogies 63,
  ThematicEvidence 45, NotesToSelf 26, WorldInference 24, PageDesign 23, Allegories 34, and
  NarrativeArchitecture only 10. So wherever a note sits, what it says tends to become either a fact of
  the world reported as history or an assertion about who a character is; it very rarely becomes a plan
  for the reader's experience.
- seen in: tracked notes across the working plan
- query:
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~^History$ where fits~^no view=cites
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~^Characterization$ where fits~^no view=cites
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~^Canon$ where fits~^no view=cites
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~^NarrativeArchitecture$ where fits~^no view=cites

### exploration-of-notes-mix-cognitive-modes/leads-1/every-goal-found-was-named-by-one-of-the-ten-modes
- lead: Asked separately whether a note says what the reader is to get out of it, the readers found a goal
  in only 140 of the 2,065 notes, 164 goal lines in all, and for every one of those 164 they named a mode
  of the ten as written for that kind of goal. None was answered "none of them". The modes named are
  WorldInference 68, NarrativeArchitecture 47, ThematicEvidence 41 and Allegories 8 — four of the ten
  carry every goal the plan's tracked notes state.
- seen in: tracked notes across the working plan
- query:
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=goals view=health
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=goals view=terms col=named top=16
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=goals where named~^none view=cites

### exploration-of-notes-mix-cognitive-modes/leads-1/worldinference-is-the-rarest-mode-in-the-notes-and-the-commonest-in-the-goals
- lead: WorldInference is the mode the fewest claims are written in — 64 lines of 4,862, a hundredth — and
  at the same time the mode named for the largest share of the goals the notes state, 68 of 164. So the
  plan's tracked notes name what the reader is to infer far more often than they are written in the mode
  that designs it.
- seen in: tracked notes across the working plan
- query:
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=claims where mode~^WorldInference$ view=cites
  - rq1 batch=exploration-of-notes-mix-cognitive-modes/01-tracked-notes answered=2065 field=goals view=terms col=named top=16

## Proposed questions

- The readers named 570 claims "world-rule ontology" and placed them outside all ten declared modes: does
  the plan need a mode for a standing rule of the world's mechanics, as against an event a historian
  reports?
- On the tracks whose claims are almost all in another mode — Storytelling Plan, Usage Plan, Character
  Appearance Plan — is the drift the same in each, or does each drift somewhere of its own?
- Where a claim in a planning track is written as History, does the plan hold the same fact on a history
  track of the same subject?
- The plan's tracked notes state a goal in only 140 of 2,065 notes: where else, if anywhere, is what the
  reader is to get out of a scene recorded?
- Do the notes whose claims stay in their track's mode differ from the drifting ones by when they were
  written, or by which subject type they hang off?

## Shortcomings

- directions: directions-1 declares that every track has one of ten types and lists ten modes, but the working plan's track definitions carry twelve: Ontology (world builder in god-mode defining the rules of the universe; the System Ontology, Function and World Truth tracks) and Civilization (world builder building what in-universe agents made in response to their ontology; the Causality of Creation, What it is and Civilizational Impact tracks) are missing. Each item still named its track's real type and mode, so on the 442 notes of those tracks (322 Ontology, 120 Civilization) a reader following the directions faithfully could never answer fits as yes; 760 of the batch's 783 claim lines marked outside all ten come from those notes, and claims there in the track's own mode could only be placed outside or in another of the ten; answered by a new directions version.
