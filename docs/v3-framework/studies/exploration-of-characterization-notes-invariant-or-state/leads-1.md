# exploration-of-characterization-notes-invariant-or-state — leads

- items with results: 101 of 110
- written by: the Claude Code session that ran the autonomous study campaign of 2026-09-26 to 27,
  which planned this study, wrote its directions, built or chose its itemizer and read its pilot,
  and drew these leads from the batch's results; written before the rule that leads are written by
  a fresh session

## Leads

### exploration-of-characterization-notes-invariant-or-state/leads-1/how-the-claims-divide-by-reach
- lead: Over 101 of the plan's 110 Characterization notes the readers named 290 claims about the character —
  a median of 3 per note, 8 at the most. Of those, only 26 lines over 19 notes hold of the character
  throughout; 140 lines over 67 notes hold only over some stretch of the story world's time; and 124 lines
  over 65 notes are unfixed, the words showing neither. So under a tenth of what the Characterization track
  holds is an invariant of who the character is, and nearly half describes a state. 7 of the 290 lines hold
  the wrong number of parts.
- seen in: Characterization notes of characters across the working plan
- query:
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims view=health
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims view=terms col=reach top=8
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^throughout view=cites
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^span view=cites
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^unfixed view=cites

### exploration-of-characterization-notes-invariant-or-state/leads-1/what-fixes-a-state-claim-in-time-is-the-tracks-own-question
- lead: Of the 140 claims that hold only over a stretch of time, 89 over 49 notes are bound by the track's
  display question or by "the start of TLTT" — that is, by the track's anchor and not by anything in the
  note — and 51 over 32 notes are bound by something the note itself names: an event, a phase, a role, a
  war, a journey, named episodes of the source show. 69 of the 140 lines say in so many words that the note
  gives no date. The binds column names "no date" in 151 lines and "the start of TLTT" in 135; the words
  "display" and "question" occur in 32 and 88 lines. So for most of the Characterization track's state
  claims, the only thing fixing when they hold is the question the track asks.
- seen in: Characterization notes of characters across the working plan
- query:
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^span where binds~"display question|start of TLTT|track.s question" view=cites
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^span where binds~"^(?!.*(display question|start of TLTT|track.s question))" view=cites
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^span where binds~"no date|no other date|gives no date" view=cites
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims view=terms col=binds top=22
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^span sample=10 seed=2 view=list

### exploration-of-characterization-notes-invariant-or-state/leads-1/the-unfixed-claims-are-dispositions-with-nothing-binding-them
- lead: 124 claims over 65 notes were named unfixed: the note states something about the character in the
  present tense with nothing in its words fixing when it holds. Examples: that she will not ask others to do
  what she is not willing to do herself; that she has to be at the tip of the spear to feel honest; that she
  hates industry because her parents left the farm for it; that she views industry as dishonest. These read
  as dispositions, and the directions had the readers record them as unfixed rather than as invariants
  precisely because nothing in the words says so.
- seen in: Characterization notes of characters across the working plan
- query:
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^unfixed view=cites
- cites:
  - exploration-of-characterization-notes-invariant-or-state/01-characterization-notes/note-19
  - exploration-of-characterization-notes-invariant-or-state/01-characterization-notes/note-2

### exploration-of-characterization-notes-invariant-or-state/leads-1/even-the-invariant-claims-mostly-have-nothing-binding-them
- lead: Of the 26 claims that were said to hold throughout, 17 over 16 notes have "none" in the binds column:
  the reader judged the claim to reach across the character's whole life on the sense of the words, with
  nothing in the note pointing at a time. So the throughout answer and the unfixed answer rest on the same
  absence of a time index, and what separates them is whether the claim reads as a disposition or as a
  condition.
- seen in: Characterization notes of characters across the working plan
- query:
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^throughout where binds~^none view=cites

### exploration-of-characterization-notes-invariant-or-state/leads-1/the-dated-notes-beside-a-note-rarely-fix-it
- lead: The dated Backstory and Life Phases notes were put beside each Characterization note so that a
  stretch of the character's life it speaks of would be visible. Only 5 claims over 4 notes name one of
  those dated notes as what binds them. On the separate whole-note question of whether a dated note beside
  it speaks of the same thing, the readers said yes for 8 notes, no for 50, and gave an answer of another
  shape for 43 — usually that a dated note covers the same ground without stating the same claim, as with a
  note on the training she gave and the solidarity she built standing beside a claim about what she believed
  that training would achieve.
- seen in: Characterization notes read beside the same character's Backstory and Life Phases notes
- query:
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where binds~"Backstory note|Life Phases note|dated note" view=cites
- cites:
  - exploration-of-characterization-notes-invariant-or-state/01-characterization-notes/note-4
  - exploration-of-characterization-notes-invariant-or-state/01-characterization-notes/note-2
  - exploration-of-characterization-notes-invariant-or-state/01-characterization-notes/note-19

### exploration-of-characterization-notes-invariant-or-state/leads-1/what-a-note-bound-by-its-own-words-looks-like
- lead: Where a claim is bound by something in the note rather than by the track's question, what binds it is
  usually a named event or a stretch of the plot rather than a date: "the event of leaving Equestria and
  arriving at and witnessing Skyfall"; "the infantry advance and the conscripts' condition in the war"; "the
  father's crusades against the Riverlands, before the start of TLTT"; "the named episodes Wonderbolts
  Academy, Rainbow Falls and Newbie Dash". A tense or a word such as "initially" also binds: one note's
  claim is bound by "initially" alone.
- seen in: Characterization notes of characters across the working plan
- query:
  - rq1 batch=exploration-of-characterization-notes-invariant-or-state/01-characterization-notes answered=101 field=claims where reach~^span sample=10 seed=2 view=list

## Proposed questions

- Where a Characterization claim is bound only by the track's display question, would the plan's Life
  Phases track hold it instead, and does that character's Life Phases track already cover the span?
- Of the Characterization claims bound by a named event rather than a date, how many of those events the
  plan holds as a plot point with a world date?
- The readers separated "holds throughout" from "unfixed" without anything in the words fixing either:
  what in a note makes a claim read as a disposition rather than a condition?
- If the Characterization track's anchor to the start of TLTT were removed, which of its claims would
  still say when they hold?
- Do the characters whose Characterization notes are mostly state claims differ from those whose notes are
  mostly dispositions by how many Life Phases notes they carry?
