# findings-schema

`docs/v3-framework/studies/<study>/findings.md`, at the top of a verification study, the
class `findings`. Written by verifying-a-corpus from the batch's results and tally;
reviewing-findings appends what its checks produce: a withdrawn line under a finding that
did not hold, a superseding entry for one that holds amended, a new entry for what Brian
raised and the session checked against the results and the items before writing. Nothing
downstream of the review writes here. A finding is a conclusion drawn over the verified
layer, the items and the results, with the questions in view: a count over the corpus, a
pattern across items, a contrast between classes, an answer that is null. It cites what it
rests on and names the frozen question it answers, if one asked for it; it never names a
hypothesis, which is the candidate's claim. Per-item results and counts live in the batch
and are cited, never copied. Nothing enters this file as a finding unless it was drawn
from or checked against the results and the items; a finding reaches a record only through
a candidate, and only a standing finding, neither withdrawn nor superseded, is the sweep's
input.

## Shape

The title is `# <study> — findings`, where `<study>` is the id of the study whose folder
holds the file, a verification's id in the registry.

| section | present | holds |
|---|---|---|
| `## Method` | required | prose: only what no batch file says |
| `## Findings` | required | entries |
| `## Proposed questions` | optional | entries, one line each; what the data raised that no question asked and no finding claims |
| `## Shortcomings` | optional | entries, one line each; what the results show wrong with the study's own instrument |

**Findings**: an entry is `### <study>/<slug>`, where `<study>` repeats the id of the
study whose folder holds the file, so that the heading is the token every other file
cites, and `<slug>` is lowercase `[a-z0-9-]+`, unique in the file, authored with the entry
and never changed, naming what was observed and never what it means. Then keyed lines in
this order:

| key | present | type | value |
|---|---|---|---|
| `question` | optional | token of question-list | the frozen question this finding answers, one at most, `<corpus>/<slug>` where `<corpus>` is the corpus the study's id names and the token is in the directions version's frontmatter; absent for a finding the data raised; what the tool derives a question's answered state from |
| `supersedes` | optional | token of findings | the finding this one holds amended, an entry earlier in the file, written only by reviewing-findings after its check |
| `finding` | required | block | what the data shows, in words, with the counts, ids or passages that show it; a null answer stated as plainly as any other |
| `cites` | required | list of line | what the finding rests on: a tally section as `<study>/<batch> § <field>`, an item as `<study>/<batch>/<item>`; at least one line |

An appended line, `- withdrawn: <date> <what the results or the items showed>`, at most
once per entry, written beneath the fields by reviewing-findings when a finding was checked
and did not hold. An entry is never edited; a finding that holds amended is a new entry
naming the old in `supersedes`. A standing finding is one neither withdrawn nor superseded.

**Proposed questions**: `- <what the data raised, in the session's or Brian's words>`, one
per line; none is a question until Brian writes it into the corpus's list, and none is a
claim.

**Shortcomings**: `- <part>: <what the results showed>`, `<part>` one of `item`,
`itemizer`, `directions`, `calibration`, `execution`, `corpus`: the item definition that
cut two things as one, the itemizer that cut short, the reserved class that filled, the
criterion the results split on, the frozen question the fields cannot answer, the sample
that never held a case the batch did, the model that ignored a criterion, the corpus that
is not what CORPORA.md says. Each is a fact about the study's own instrument, never about
the corpus's content, and routes upstream: to preparing, to building-a-tool, or to a second
study under another model.

Method holds prose and nothing derivable: what was deliberately not measured, and any
caveat of the execution. The directions version and its hash, the calibration, the
itemizer, the item count, the model and effort, the harness and the cost are the
definition's, the index's, the calls' and the tally's, read there.

## Example

```markdown
# verification-of-v1-archive-scene-stasis — findings

## Method

The directions ask one class per note and nothing about links between notes; a note's
place in the scene graph was not measured. Two items were malformed at the tally and
are not in any count below.

## Findings

### verification-of-v1-archive-scene-stasis/stasis-in-one-of-nine-scene-notes
- question: v1-archive/scene-notes-carry-designed-stasis
- finding: Of 1,116 scene notes, 124 are class `stasis`, one in nine; the other
  eight in nine split between `event` (803) and `unplaced` (189).
- cites:
  - verification-of-v1-archive-scene-stasis/01-full § class

### verification-of-v1-archive-scene-stasis/unplaced-notes-are-paratext
- finding: The 189 `unplaced` notes are, by their locators, all under the Paratext
  story; none is a scene of the main plot.
- cites:
  - verification-of-v1-archive-scene-stasis/01-full § class
  - verification-of-v1-archive-scene-stasis/01-full/note-1630
  - verification-of-v1-archive-scene-stasis/01-full/note-2044

### verification-of-v1-archive-scene-stasis/unplaced-notes-are-paratext-but-three
- supersedes: verification-of-v1-archive-scene-stasis/unplaced-notes-are-paratext
- finding: Of the 189 `unplaced` notes, 186 are under the Paratext story by their
  locators; the three under TLTT CH#12 are notes 1630, 2044 and 2051.
- cites:
  - verification-of-v1-archive-scene-stasis/01-full § class
  - verification-of-v1-archive-scene-stasis/01-full/note-1630
  - verification-of-v1-archive-scene-stasis/01-full/note-2044
  - verification-of-v1-archive-scene-stasis/01-full/note-2051

### verification-of-v1-archive-scene-stasis/event-notes-cluster-early
- finding: The `event` notes cluster in the first three chapters by their locators.
- cites:
  - verification-of-v1-archive-scene-stasis/01-full § class
- withdrawn: 2026-09-20 the locators put 402 of 803 event notes after chapter three

## Proposed questions

- whether the three `unplaced` notes under TLTT CH#12 are scenes the v1 links never
  placed, or notes that predate the chapter

## Shortcomings

- directions: the reserved class `unplaced` took 189 of 1,116 notes; the criteria do not
  place a note whose scene is named only by a link

```

## Queries

| question | how |
|---|---|
| which questions a verification answered | `grep -h '^- question:' studies/<study>/findings.md` |
| every finding that answers one question | `grep -l '^- question: <corpus>/<slug>' studies/*/findings.md` |
| every finding resting on one item | `grep -l '<study>/<batch>/<item>' studies/<study>/findings.md` |
| the finding a candidate cites | the candidate's token line, then `grep -n '^### <study>/<slug>' studies/<study>/findings.md` |
| the standing findings, the sweep's input | every entry with no `- withdrawn:` line beneath it and no later entry naming it in `- supersedes:` |
| whether a finding fell or was amended | `grep -n -A8 '^### <study>/<slug>' studies/<study>/findings.md` for a withdrawn line; `grep -n '^- supersedes: <study>/<slug>' studies/<study>/findings.md` for its successor |
| what a study's results showed wrong with its instrument | `grep -h '^- ' studies/<study>/findings.md` under Shortcomings, the part as the first word |

## Checks

| check | fails when |
|---|---|
| `findings.title` | the title is not `# <study> — findings` with the id of the study whose folder holds the file, or that study is not a verification in the registry |
| `findings.shape` | a section is missing, out of order or holds other than the table says; Method copies a count or a definition field |
| `findings.entry` | an entry heading is not `### <study>/<slug>` with the id of the study whose folder holds the file, a slug repeats, or a field is missing, out of order or of the wrong type |
| `findings.question` | a question token is not `<corpus>/<slug>` of the study's corpus, names no entry in that list, or is not in the frontmatter of the directions version the study's batches name |
| `findings.cites` | a cited batch is not under the study, a cited field is not an enum field of that batch's directions, a cited item is not in that batch's index, or the list is empty |
| `findings.supersedes` | a supersedes token names no earlier entry in the file, names one already superseded or withdrawn, or an entry is superseded twice |
| `findings.withdrawn` | a withdrawn line is not dated, appears twice under one entry, or sits under an entry a later one supersedes |
| `findings.shortcoming` | a Shortcomings line's first word is not one of the six parts |
