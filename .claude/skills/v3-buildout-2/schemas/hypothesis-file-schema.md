# hypothesis-file-schema

`docs/v3-framework/hypotheses/NNN-slug.md` — `NNN` a zero-padded three-digit id, stable,
unique across the set, never reused; `slug` a descriptive slug. One file holds three
artifacts under three write disciplines, which is why the Artifacts table carries three rows
over it. The shape below is what the hook holds; the example is a conforming file whose first
fenced block the checker's tests read as their fixture; then how a hypothesis is queried, and
the check ids the hook reports.

**Why the record is in the file, and when to read it.** The record is not the file's history:
it is the evidence relationship the epistemic framework names, present-tense content that
happens to be dated. Remove it and the file is not a hypothesis with less provenance — it is a
hypothesis with no evidence. An iteration entry sits in it not as an edit log but because
evidence is bound to the wording it was verified under, and a re-verification has to know which
entries a wording change unbound. Read the record when you need to know what the statement
rests on: what was promoted and against which wording, whether a challenge stands, and whether
Brian has judged the picture sufficient to act on. Read § Origin instead when you need to know
why the hypothesis exists at all; it says nothing about whether the statement is true.

**An empty record is the normal state of an untested hypothesis**, not an unfinished file. A
hypothesis has no entries until a candidate is promoted to it.

## Shape

The file has no title: the id and the slug are its name. Nothing derivable is authored in it —
not the id, not a status, not a creation date beside § Origin's, not a baselined flag beside
its entry. Status is read from the entries below the last iteration entry: `challenged` if an
unresolved challenging one stands there, `evidenced` if an evidence entry does, `untested`
otherwise; `state.md` renders it for every hypothesis.

| section | present | holds |
|---|---|---|
| `## Hypothesis` | required | prose: the prediction, and only that |
| `## Origin` | required | fields |
| `## Record` | required | entries |

**Hypothesis** is what the hypothesis predicts, in one to three sentences readable in
isolation. Founding reasoning belongs in § Origin; provenance, implications, testing method and
confirm-or-refute conditions belong nowhere in this file. It carries the prediction and nothing
beside it: no reference to another hypothesis by id or slug; no evidence claimed for it, which
is the record's to hold; and no open question, which is not a prediction — a statement that ends
in one is two things, and the question belongs in the corpus's question list.

**Origin**:

| key | present | type | value |
|---|---|---|---|
| `date` | required | date | the day the hypothesis was captured |
| `reasoning` | required | block | why the hypothesis exists: the observation, Brian's assertion, the motivation, and what raised it; in Claude's voice; under rule 10 |

**What `reasoning` may not hold.** It explains why the hypothesis exists and never extends what
it asserts, so: no claim the statement does not carry; no corpus reading stated as established
fact, that being evidence, which enters only as an entry below; no prospective testing method,
naming which study will test it; no asserted relationship to another live hypothesis, merge and
split provenance excepted, which names the files this one came from; no confirm-or-refute
condition; no assessment of the hypothesis's own testability or thinness, which the record
answers by what it holds; no synthesis named as what raised it (`minting-a-hypothesis`); and
nothing derivable. Each of these is a thing the field can be written to say and a thing that
stops being true when a statement is reworded or the set changes around it.

**Record**: an entry's heading is `### <kind>`, type enum of `evidence`, `iteration`,
`baselined`. A heading repeats — a hypothesis with eight evidence entries has eight
`### evidence` headings — and nothing cites an individual entry, its position and its date
being what locate it. Entries are in the order they were written, oldest first, and that order
is the record's own: an iteration entry is a boundary read by position, and nothing sorts.

| key | present | type | value |
|---|---|---|---|
| `date` | required | date | the day the entry was written |
| `candidate` | optional | token of findings | evidence only: the promoted candidate, `<study>/<finding-slug>`, naming the study whose findings file holds the finding, for an iteration-sourced entry as for any other |
| `tag` | optional | enum: `supporting`, `challenging` | evidence only: which side of the falsifier the finding fell on, as the referee's verdict classified it; there is no third tag |
| `finding` | optional | block | evidence only: the finding, verbatim and frozen at promotion, so a later supersession of it cannot alter what was promoted |
| `falsifier` | optional | block | evidence only: what the finding would have been were the statement false, verbatim from the referee |
| `from` | optional | block | iteration only: the wording that stood before this entry; the wording after it is the next iteration's `from`, or the statement |
| `reason` | optional | block | iteration only: Brian's reason for the reword; under rule 10 |
| `rationale` | optional | block | baselined only: Brian's judgment; under rule 10 |

Which kind requires which of the optional fields is the class's own rule, in Checks, the
grammar admitting one field table per entries section and only `required` or `optional`.

**One finding per entry.** An entry carrying two findings is two entries. No check holds this —
nothing can count findings in prose — and it is the writer's rule.

**Who writes what.** `mint` writes § Hypothesis and § Origin; `promote` writes an evidence
entry; `gate-and-commit` writes an iteration entry, the fresh evidence entries below it, and
the new statement; `baseline` writes a baselined entry. Nothing else writes here. **A challenge
enters the same way any evidence does** — as a finding of a verification, claimed, refereed and
promoted — and there is no channel for one authored by hand: what Brian raises in a review is
checked against that verification's results and items before it is a finding, and anything
unmeasured is a question in the corpus's list. The absence of a hand-authored route is the
strong form, not a gap.

## Example

```markdown
## Hypothesis

Scenes that open on a character already in motion carry their orientation cues in the first
paragraph more often than scenes that open on a static tableau, which defer them to the
second or third.

## Origin

- date: 2026-09-14
- reasoning: Raised by Brian while reading two chapter openings back to back and noticing
  that the one opening mid-action told him where he was almost at once, while the tableau
  opening left him placing the scene for several lines. His assertion: the difference is not
  about pace but about what the opening move forces the prose to answer. Whether deferred
  orientation is a fault is his craft judgment and is deliberately outside the statement.

## Record

### evidence
- date: 2026-09-20
- candidate: verification-of-example-corpus-scene-openings/cues-in-first-paragraph
- tag: supporting
- finding: Of 180 scene openings, 96 open in motion and 84 on a tableau; 71 of the 96 carry
  an orientation cue in the first paragraph against 28 of the 84.
- falsifier: If the opening move did not bear on where cues fall, the two classes would have
  carried first-paragraph cues at about the same rate.

### evidence
- date: 2026-10-02
- candidate: verification-of-example-corpus-scene-openings/deferral-tracks-length
- tag: challenging
- finding: Among the 84 tableau openings, the 31 shortest carry a first-paragraph cue at the
  same rate as the in-motion class; deferral appears only in the longer openings.
- falsifier: If the opening move alone bore on where cues fall, opening length would not have
  separated the tableau class into two rates.

### iteration
- date: 2026-10-05
- from: Scenes that open on a character already in motion carry their orientation cues in the
  first paragraph more often than scenes that open on a static tableau, which defer them to
  the second or third.
- reason: The short tableau openings behave like the in-motion ones, so the claim I actually
  hold is about long openings, where the prose has room to defer. Narrowing it to those.

### evidence
- date: 2026-10-05
- candidate: verification-of-example-corpus-scene-openings/cues-in-first-paragraph
- tag: supporting
- finding: Of 180 scene openings, 96 open in motion and 84 on a tableau; 71 of the 96 carry
  an orientation cue in the first paragraph against 28 of the 84.
- falsifier: If length rather than the opening move governed deferral, the in-motion class
  would have deferred at the same rate as the long tableau openings.

### baselined
- date: 2026-10-06
- rationale: Two readings and 180 openings, and the narrowed wording holds on both. I am
  comfortable planning against it. The short-opening case is its own question and I have
  written it into the list.
```

## Queries

| question | how |
|---|---|
| every hypothesis and its status | `state.md`, which derives status from each record |
| one hypothesis's evidence, in order | `grep -n '^### ' docs/v3-framework/hypotheses/NNN-slug.md` |
| whether a hypothesis is challenged | the entries below its last `### iteration`: a `- tag: challenging` among them |
| which hypotheses a study's findings reached | `grep -rn '^- candidate: <study>/' docs/v3-framework/hypotheses/` |
| whether a candidate was promoted, and where | `grep -rln '^- candidate: <study>/<finding-slug>' docs/v3-framework/hypotheses/`; absent and present in the study's declined-candidates file means declined, absent from both means pending |
| every baselined hypothesis | `grep -rl '^### baselined' docs/v3-framework/hypotheses/` |
| how a statement has changed | its `### iteration` entries' `from` values, oldest first, then the statement |
| why a hypothesis exists | its § Origin |

## Checks

| check | fails when |
|---|---|
| `hypothesis.shape` | a section is missing, out of order, or holds other than the sections table says |
| `hypothesis.entry` | an entry's key is unknown, out of order or of the wrong type; a line is neither keyed nor a two-space continuation; a heading is outside the kind enum; or a `candidate` resolves to no finding |
| `hypothesis.evidence.fields` | an `evidence` entry lacks `candidate`, `tag`, `finding` or `falsifier`, or carries a field of another kind |
| `hypothesis.iteration.fields` | an `iteration` entry lacks `from` or `reason`, or carries a field of another kind |
| `hypothesis.baselined.fields` | a `baselined` entry lacks `rationale`, or carries a field of another kind |
| `hypothesis.baselined.challenged` | a `baselined` entry follows an unresolved challenging evidence entry within the current wording |
| `hypothesis.entry.date` | an entry's date is earlier than the entry before it |

The first two carry what the engine finds — the sections and their order, each field's presence
and type, the heading enum, `candidate` resolving to a finding entry, `tag` closed to its two
values, and the two-space continuation of a block — and the other five are the class's own
rules, which no schema language expresses.
