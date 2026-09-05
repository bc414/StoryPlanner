# Artifacts

Every artifact a process in this skill reads or writes, and every authored format. An
artifact is a class; the files are its instances. Schema and closed sets: `SKILL.md` § Schema.
Consumers are never written here; the validator derives them.

Placeholders in paths, the same everywhere: `<corpus>` a name from `CORPUS-STATUS.md`;
`<instance>` an id from the instance registry (`exploration-of-<corpus>[-<n>]` or
`round-of-<corpus>-<n>`); `<run>` a runner run folder, `<date>[-<slug>]`, one per batch
execution; `NNN` a hypothesis id; `N` a version number. An instance's authored artifacts
live in one directory named by it under `docs/v3-framework/`; its runner input and output,
including the referee runs that serve it, live under `fanout/<instance>/`. `fanout/referee/`
holds only the referee's shared instrument and the iteration candidates.

| id | path | mutation | format | description |
|---|---|---|---|---|
| hypothesis-statement | docs/v3-framework/hypotheses/NNN-slug.md § Hypothesis | in-place | Hypothesis file | The current wording of one hypothesis |
| hypothesis-record | docs/v3-framework/hypotheses/NNN-slug.md § Record | append | Hypothesis file | The evidence relationship: dated entries, never edited |
| hypothesis-status | docs/v3-framework/hypotheses/NNN-slug.md frontmatter | in-place | Hypothesis file | Status and baselined, computed from the record |
| hypothesis-index | docs/v3-framework/hypotheses/INDEX.md | in-place | Hypothesis index | Id and slug per hypothesis, id order |
| question-list | docs/v3-framework/questions/<corpus>.md | append | Question entry | Brian's open questions about one corpus |
| instances | docs/v3-framework/instances.md | append | Instance registry | One row per instance of a chain, declared at Brian's go; the ids every artifact path is named by |
| state | .claude/skills/v3-buildout/state.md | in-place | | Generated from the registry and the artifacts: per instance where it is; per corpus, open questions and whether a calibrated codebook covers them; per hypothesis, status and whether any open question names it |
| revision-note | docs/v3-framework/methodology-revision-N.md | frozen | | What one methodology revision changed and why |
| rulings-log | docs/v3-framework/methodology-revision-N-rulings.md | append | | Brian's rulings during a revision, dated, with reasons |
| leads-artifact | docs/v3-framework/<instance>/leads.md | append | Leads artifact | What one exploration observed, organised by locus |
| verification-artifact | docs/v3-framework/<instance>/round.md | append | Verification artifact | One round's method, questions answered, counts and promotion summary |
| arm-key | docs/v3-framework/<instance>/arm-key.md | frozen | Arm key | The blinding key: arm label to condition and model; opened only after binning |
| candidates | fanout/<instance>/candidates.md | append | Candidate | One round's findings claimed to bear on a hypothesis, with referee lines and outcomes |
| iteration-candidates | fanout/referee/iterations/NNN-<date>/candidates.md | append | Candidate | Prior findings re-queued after a rewording of hypothesis NNN |
| corpus-status | .claude/skills/v3-buildout/CORPUS-STATUS.md | in-place | | What material exists and its state |
| codebook | fanout/<instance>/codebook-N.md, or fanout/referee/codebook-N.md | succeeded | Codebook | The frozen instrument a round or the referee runs under |
| reading-protocol | fanout/<instance>/protocol-N.md | succeeded | Reading protocol | The instruction slice readers run under; piloted, not calibrated |
| calibration-record | fanout/<instance>/calibration-<date>.md, or fanout/referee/calibration-<date>.md | frozen | Calibration record | One codebook version's agreement with Brian's blind verdicts, and the rulings |
| itemizer | fanout/<instance>/itemize.* or tools/StoryPlanner.<Name>/ | in-place | | Code that produces a corpus's items |
| generator | fanout/<instance>/make-jobs.* | in-place | | Code that writes jobs from the manifest |
| tallier | fanout/<instance>/tally.* | in-place | | Code that reduces results to counts and flagged rows |
| items | fanout/<instance>/<run>/items/ | frozen | | The units one run judges, one file each; a referee run's sit under fanout/<instance>/referee/<run>/ |
| items-manifest | fanout/<instance>/<run>/items/manifest.md | frozen | | The index of a run's items |
| jobs | fanout/<instance>/<run>/jobs.json | frozen | | One run's job file; an edit is a new run |
| ledger | fanout/<instance>/<run>/ledger.jsonl | append | | One row per attempt, with hashes |
| results | fanout/<instance>/<run>/results/ | frozen | | The agents' outputs, one per job |
| tally-output | fanout/<instance>/<run>/tally.md | frozen | | The tallier's counts and flagged rows for one run |
| run-record | fanout/<instance>/<run>/run.md | append | run.md | The authored front page of one run |
| skill | .claude/skills/v3-buildout/*.md and .claude/skills/agent-runner/SKILL.md | in-place | | The method's instructions |
| map | .claude/skills/v3-buildout/map.md | in-place | | Generated: the whole graph, consumers, validation report |
| tool-source | tools/StoryPlanner.<Name>/ and its tests | in-place | | Code with tests: ingests, readers, the runner, the validator |
| corpus | outside the repo | in-place | | The corpora named in CORPUS-STATUS.md, read through the MCP server, files or sqlite3 |

## Hypothesis file

`docs/v3-framework/hypotheses/NNN-slug.md` — `NNN` a zero-padded three-digit id, stable,
unique across the set, never reused; `slug` descriptive kebab-case. One file holds three
artifacts, each with its own mutation:

```markdown
---
id: 17
status: evidenced
baselined: false
created: 2026-09-01
---

## Hypothesis

[1–3 sentences. A testable prediction readable in isolation. Edited in place.]

## Record

[Dated entries, oldest first, newest appended, never edited.]
```

**Frontmatter** (`hypothesis-status`, in-place): `id`; `status` ∈ `untested | evidenced |
challenged`, computed from the entries bound to the current wording — `challenged` if any
such challenging entry is unresolved, `evidenced` if any such evidence entry exists,
`untested` otherwise; `baselined`, `false` or an ISO date, reset to `false` when a
challenging entry lands or the wording changes; `created`, never changed. No other fields.

**Statement** (`hypothesis-statement`, in-place): what the hypothesis predicts, and only
that. Founding reasoning belongs in the `created` entry; provenance, implications, testing
method and confirm/refute conditions belong nowhere in this file.

**Record** (`hypothesis-record`, append): the evidence relationship. Four entry kinds, each
one citable unit, full ISO timestamps, grep-able by `^- created`, `^- evidence`,
`^- iteration`, `^- baselined`.

```
- created | 2026-09-01T10:00: <why the hypothesis exists: the observation, Brian's
  assertion, the motivation; in Claude's voice with Brian's assertions as the content>
- evidence | 2026-09-14T15:20 | (round-of-analysis-corpus-1 C-014; codebook dt-classes-3@3f9a1c) [supporting]:
  <the finding, verbatim from the candidate>
  Falsifier: <verbatim from the referee's line>
- iteration | 2026-09-16T09:15: Reworded from "…" to "…" because <reason>. Entries above
  this line are bound to the prior wording.
- baselined | 2026-09-20T16:00: <Brian's rationale, in his words>
```

An `evidence` entry is written only by a promotion session from a referee-checked candidate
and carries the instance and candidate id, the codebook version and hash, the alignment
tag (`supporting` or `challenging`, no third tag), the finding and the falsifier verbatim.
An entry without a falsifier is malformed. An `iteration` entry is a wording boundary:
nothing above it is invalidated or re-tagged, and nothing above it counts toward the
status until re-verified against the new wording; the prior entries' findings are queued
as iteration candidates for the next round. A `baselined` entry is written only by Brian or
at his explicit direction in his words. Entries are never edited; there is no superseded
marker, because binding to a wording is read from position relative to iteration entries.

What never enters a record: Brian's recall about his practice (a question); story-design
observations (story content); observations that do not change the statement; pointers to
future work (a question list); leads from an exploration, however relevant.
Test: removing the entry would leave the evidence relationship incomplete — it belongs.

## Instance registry

`docs/v3-framework/instances.md` — one row per instance of a chain, appended by the
preparing activity at the moment Brian approves its plan, which is his go. Never edited: an
instance abandoned is a fact the artifacts show, not a row removed. Every artifact path an
instance produces is named by its id.

```markdown
| id | type | corpus | go |
|---|---|---|---|
| exploration-of-v1-archive | exploratory | v1-archive | 2026-09-12 |
| round-of-analysis-corpus-1 | verification | analysis-corpus | 2026-09-20 |
```

`id` is `exploration-of-<corpus>[-<n>]`, `round-of-<corpus>-<n>`, or `referee-<n>`; a
round always carries its ordinal, an exploration only when the corpus is explored again,
and the referee's each time its codebook is prepared anew. `type` ∈ `exploratory |
verification` names the chain: exploratory runs preparing-to-explore-a-corpus,
exploring-a-corpus and reviewing-leads; verification runs preparing-to-verify-a-corpus,
conducting-a-verification-round, writing-candidates-from-verification,
refereeing-a-candidate and promoting-checked-candidates. `referee-<n>` is a verification
instance that stops after preparing: its corpus is `candidates`, its folder is
`fanout/referee/`, and what it produces — the codebook, its calibration record, the
materialise itemizer — is what every referee run under every round then uses. It is
prepared once and again only when a ruling changes the codebook; that is the whole of
what "bootstrap" means here, and it is a fact about instances, never about activities.
`corpus` is a name from `CORPUS-STATUS.md`, `verified-artifacts` for an exploration over
the buildout's own outputs, or `candidates` for the referee. Nothing else is authored
here: where an instance stands is derived by the tool from its artifacts into `state.md`,
and a tool an instance needs is built as its first task.

## Hypothesis index

`docs/v3-framework/hypotheses/INDEX.md` — a routing table, two columns (id, slug as link),
in id order so a top-to-bottom scan is comprehension order. It changes only when a file is
minted or superseded. It carries no status or baselined column; status is read from the
files (`grep -h '^status:' docs/v3-framework/hypotheses/0*.md | sort | uniq -c`).

## Question entry

`docs/v3-framework/questions/<corpus>.md` — one file per corpus, one entry per question,
appended, never rewritten; only the `status` line changes, and only to `withdrawn`.
Written only by a process with mode `hitl`: a question is Brian's. Which codebook froze a
question and which round answered it are derived by the tool from the codebooks and the
rounds' `round.md`, into `state.md`; they are never written here.

```
### <short title>
- asked-by: <instance id, "review of <instance id>", "promotion of <scope>", or "ad hoc"> (<date>)
- hypotheses: <ids the answer would be evidence for or against>
- question: <one testable question about this corpus>
- predicate: <the frozen predicate a codebook would apply, if one suggests itself; may be blank>
- status: open | withdrawn (<reason>)
```

The `hypotheses` line is the only authored place a hypothesis-to-corpus edge exists; a plan
never copies it. Questions flow freely: any hitl activity may write into any corpus's list.
Leads never enter one; a question is what a lead raised, in Brian's words.

## Candidate

`fanout/<instance>/candidates.md`, one per round; `fanout/referee/iterations/NNN-<date>/candidates.md`
for the findings re-queued by a rewording of hypothesis NNN. Append-only: a candidate is never edited after it
is written; the referee's lines and the outcome are appended beneath it. A finding bearing
on two hypotheses is two candidates. Status is read from the last line present: a diagnostic
candidate with no outcome line is awaiting Brian's decision.

```markdown
## C-014
- target: 031
- finding: <one citable unit: what was observed, with ids, counts, passages>
- source: <the locator: corpus locus, note ids, story and chapter, artifact section>
- proposed-by: <job id> / <model> / <ISO time> / <codebook id>@<hash> / harness <version>
```

Referee append, from the referee's result for this candidate (the referee saw the target's
current statement and the `finding` line only — never `source`, never a falsifier anyone
else wrote):

```markdown
- falsifier: <if the statement were false, the finding would have been ___ instead>
- referee: <job id> / <model> / <ISO time> / referee-N@<hash> / diagnostic [supporting|challenging] | non-diagnostic — <one-line reason>
```

Outcome append, written by the promotion session for a diagnostic candidate only, recording
Brian's decision; a non-diagnostic candidate gets no further line and stays as context:

```markdown
- outcome: promoted <ISO time> as evidence entry <timestamp in NNN-slug.md> | declined — <Brian's reason>
```

The `source` locator is the citation check: before a candidate is promoted, the promotion
session reads the source it names and confirms the finding is there as stated. A decline
needs no read. Nothing upstream confirms the locus; the classifier named it and the referee
judged only the finding's shape.

## Leads artifact

`docs/v3-framework/<instance>/leads.md`, beside the arm key and any renders the exploration
produced. Written once by exploring-a-corpus; reviewing-leads appends dated correction
sections and never edits a lead in place. Later sessions cite it and never edit it.

```markdown
# exploration-of-v1-archive — leads

## Method
<scale: pathfinder or slices; the reading protocol id and hash; arms and their neutral
labels if any; models; harness version; the run folder; what was deliberately not measured —
one arm means no disagreement count>

## Questions in view
<the entries of questions/v1-archive.md this exploration read with, by title>

## Leads
<organised by what was observed — by locus, subject, pattern — never by hypothesis id. Each
lead: the locus, what was seen there, and nothing about what it means for any hypothesis>

## Bins
<slices with arms only: disagreements between arms sorted into named bins before any was
investigated; the count per bin is the finding; which bins were drilled and the adjudicated
result of each, after the arm key was opened>

## Proposed questions
<what the leads raise, as proposals; none is a question until Brian writes it into the
list in reviewing-leads>

## Corrections
<appended, dated: a lead challenged in review and found not to hold at the source, with what
the source showed>
```

A leads artifact carries no candidates, no findings and no evidence, and is cited as leads
only, by any later session, until a round on its corpus has run.

## Verification artifact

`docs/v3-framework/<instance>/round.md`. Written once by conducting-a-verification-round;
the promotion session appends its summary; corrections are appended, dated. Per-item
results are not copied here: they live in the run's `results/` and `tally.md`, cited by run
folder and ledger row.

```markdown
# round-of-analysis-corpus-1

## Method
<the codebook id and hash and its calibration record; the itemizer and item count; the
generator; models; harness version; the run folders and ledgers; what was not measured>

## Questions answered
<the entries of questions/analysis-corpus.md this round's items and predicates cover, by
title; this list is what the tool derives a question's answered state from>

## Counts
<from tally.md: labels per class, flagged rows, malformed outputs; each table cites the
tallier and run that produced it>

## Promotion
<appended by the promotion session: candidates per target; diagnostic and non-diagnostic;
promoted supporting and challenging; declined with reasons; disagreements with the referee
and how Brian ruled; anything noticed about the pipeline's own behaviour>

## Corrections
<appended, dated>
```

Counts cite the instrument that produced them; a classification that bears on a hypothesis
reaches a record only through a candidate, never from this file.

## Arm key

`docs/v3-framework/<instance>/arm-key.md`, only when an exploration runs its slices under
more than one condition. Written by preparing-to-explore-a-corpus, frozen, and not opened
by exploring-a-corpus: the record files carry the neutral labels only, and the session that
joins and bins them cannot see which condition produced which. Opened in reviewing-leads
after the bins are counted.

```markdown
| label | condition | model |
|---|---|---|
| arm-A | explicit context, protocol-2@<hash> | opus |
| arm-B | explicit context, protocol-2@<hash> | sonnet |
```

One factor varies across arms; the rest is identical. An exploration with one arm has no key
and its leads artifact says no disagreement was measured.

## Reading protocol

`fanout/<instance>/protocol-N.md`, the instruction a slice reader runs under, inlined and
hashed by the runner as the reader's entire context. Authored in preparing-to-explore-a-corpus;
piloted (one job, its output read by Brian) and not calibrated, since a reader exercises
salience and emits leads rather than labels. Versioned by number; an edit is a new file
and a new hash, and a slice read under the old hash is cited as such.

```markdown
# Reading protocol — <corpus> (version N)

## What you are given
<one slice of the corpus, its extent; the questions in view, verbatim>

## What you produce
<a record set: one entry per lead, locus first, then what was observed there, in the
form the joiner expects; the output contract's markers>

## How to read
<discovery-first: report what is there, organised by locus; never a claim about a
hypothesis; never an opinion about story content>

## Never
<search outside the slice; consult anything not inlined; propose what should be done>
```

## Codebook

`fanout/<instance>/codebook-N.md` for a corpus; `fanout/referee/codebook-N.md` for the
referee. The frozen instrument an agent applies to one item with no discretion: all
judgment was spent writing it. Authored in preparing-to-verify-a-corpus against real items,
calibrated before any batch, versioned by number; every edit is a new file, a new hash and
a new calibration. It carries no status line: whether a version is calibrated is read from
a calibration record existing at its hash, never from the file, since any line in the file
is part of the hash. The runner inlines it as the agent's entire context, so it must be
complete in itself and must not restate what the process row already says about its
inputs: what the agent is given is the `reads` of the agent process in the activity file,
materialised by the generator, and the codebook names it by reference.

```markdown
# Codebook — <name> (version N)

## Item
<what one item is, as the itemizer produces it; the frozen predicate's unit>

## Inputs
<by reference: the agent process row in <activity>.md; the item file's headings>

## Output
<the exact lines to write, with the markers the output contract checks; nothing else>

## Classes
<the closed set of labels, each defined by what the item shows, not by what it means>

## Decision rules
<numbered; the boundary cases, each resolved one way; tuned to over-flag where a false
negative costs the record and a false positive costs one adjudication>

## Anchors
<under the rule each anchors: an item from a calibration disagreement, its ruled label,
and the calibration record it came from; none until a calibration has produced one>
```

The referee's codebook is this shape with `Item` a candidate's finding beside the target's
current statement, `Output` the falsifier line and the verdict line, and `Classes` the three
verdicts: diagnostic supporting, diagnostic challenging, non-diagnostic. A vacuous
falsifier, one that restates the claim instead of naming what the finding would have been,
is non-diagnostic by definition.

## Calibration record

`fanout/<instance>/calibration-<date>.md`, or `fanout/referee/calibration-<date>.md`. One
per calibration of one codebook version, frozen. It is the measurement that lets a codebook
be trusted to say what Brian would have said, and it is what "calibrated" means: a codebook
version with no record at its hash is uncalibrated whatever its status line says.

```markdown
# Calibration — <codebook id>@<hash> — <date>

## Sample
<item ids, how drawn (stratified by class, spanning ≥ 3 targets for the referee), count;
the held-out split: which items were ruled on, which were scored after the rulings>

## Scorings
| item | Brian | agent | agree |
<Brian's verdicts written by the session as he gave them, the agent's results withheld
until his were complete; the agent's from the calibration run's results>

## Agreement
<per class, on the ruled items and on the held-out items separately; the model that scored>

## Rulings
<one per disagreement: the item, both verdicts, Brian's ruling, and the codebook edit it
produced, if any — each edit is what makes the next version>

## Verdict
<Brian: accepted at this hash, or re-run after edits>
```

## run.md

`fanout/<instance>/<run>/run.md`, the authored front page of one run, appended to and never
rewritten. Small: which instance and which questions the run serves; the instrument and its
hash; the arms, if any, and what is deliberately not measured; the pilot read, by whom and
what it showed; where the tally, the adjudication or the promotion lives. Everything else
about a run is mechanical and lives in `jobs.json`, `ledger.jsonl`, `items/manifest.md`,
`results/` and `tally.md`; the runner's own `attempts/` folder, each attempt's composed
prompt and stream, is its working store, local and never cited. A document in `docs/`
cites a run by this folder and a ledger row.
