---
name: v3-buildout-2
description: "Methodology for the v3 narrative design framework buildout, revision 2 (in construction from 2026-09-04) — fourteen activities from baselining a hypothesis down to building a tool, each with its own companion file carrying its processes table and procedure; the strong-form evidence pipeline (candidates → referee → promotion) as activities; studies as folders of batches run through the agent runner under directions; the split of verification into preparing (itemize, author, calibrate with Brian) and verifications (autonomous); the constitutional rules including the artifact-mutation rule. Load before any framework buildout work. Not yet the live skill: the live one is v3-buildout until the router swap."
---

# V3 framework buildout — revision 2 (in construction)

This file is a **router**. It carries what every buildout session must never miss — the
epistemic framework, the constitutional rules, how to read the tables — and the table of
activities, each sent to the companion file that holds its processes and its procedure.
**Read the file named for your activity, in full, before acting.** Nothing here is a
summary of a companion. The folder's own schema, for a session that changes it, is
`schemas/skill-schema.md`.

## Epistemic framework (applied)

**Scope.** Two coupled but separable domains: the *narrative design framework* (what the
planner should track — tracks, cognitive modes, the mechanism/goal/technique model,
reader-experience moments, perception-gap delivery, variable focalization, voice registers,
and whatever else the evidence shows the planner should track) and the *codebase
architecture* (how it is represented). Framework hypotheses come first; codebase
implications follow. Nothing in the current framework vocabulary is settled by appearing
authoritatively in CLAUDE.md or a track definition — the buildout may revise any of it.

**Recall and unverified output are not evidence.** What Brian remembers, what a session or a
model concluded without verification, and what an exploration saw as leads may raise a question
or a hypothesis, and never move one: only verified evidence does. A hypothesis file holds a
statement and its evidence relationship together: the statement, current and edited in place;
the record, dated entries never edited, which *is* the evidence relationship rather than a
history of it; and a status computed from the record. Three statuses:

- `untested` — no verified evidence bound to the current wording.
- `evidenced` — verified evidence bound to the current wording supports the statement; thin
  or thick, the record shows the weight; always revisable.
- `challenged` — at least one verified challenging entry bound to the current wording is
  unresolved. One open challenge puts the hypothesis here regardless of support.

An entry is bound to the wording it was verified against. An iteration entry is a wording
boundary: nothing above it is invalidated, and nothing above it counts toward the status
until re-verified against the new wording. A reworded hypothesis with no re-verified
entries is `untested`, honestly, with its whole prior record still in the file.
Baselining is a separate field: Brian's dated
judgment that the evidence picture is sufficient to act on. It is progress tracking, not
epistemology — it adds no weight, removes no challengeability, is not endorsement of truth,
is itself bound to a wording, and resets to `false` when a challenging entry lands or the
wording changes. Only Brian baselines; a session may name candidates ("verified support, no
open challenge — review for baselining") and never sets the field.

**Recall is atmosphere; evidence is grounding.** A statement about the data from anyone —
Brian from memory, a prior session, a memory file, this skill, any document — is
unverified until it is checked at its source. Before acting on it: query the source, compare, present
the discrepancy to Brian, proceed on the grounded version once he has seen it. Nothing is
exempt, including this file. Grounding connectors include the MCP server, the local corpora
named in `CORPORA.md`, `codesessions.db` by sqlite3, and web search; CLAUDE.md carries
each source's semantics and caveats.

## Constitutional rules

These hold in every session type. A companion file elaborates; none overrides.

1. **Only Brian baselines.** Only Brian decides story content, taxonomy, what is
   interesting, whether a flagged note is resolved, what is written to a `.storyplan`, and
   anything else CLAUDE.md reserves to him — that list is not exhaustive either.
2. **Strong form: only verification produces evidence.** Exploration produces leads.
   Evidence enters a hypothesis record only from a verification's finding, claimed, judged a
   candidate by a fresh-context referee, and promoted in a session with Brian in the
   loop deciding each one. Nothing else writes to `docs/v3-framework/hypotheses/`. Every
   finding of every verification is claimed under the same claiming directions, and every
   claim is judged under the same referee directions, each at the same hash: both are parts
   of the method, and a verification runs them and never authors them.
3. **Itemizers read corpora; collators read the method's own files.** An itemizer reads
   corpora, one or several, and derives what it needs from them in its own run; nothing else
   is its input, though anything may inform how its code is written. A collator reads only
   the method's own files its activity names.
4. **Directions are calibrated or piloted before their first batch.** A verification's
   directions, and the method's pipeline directions for claiming and the referee, are
   authored in a session with Brian, against real items, calibrated against his blind
   verdicts before their first full batch, versioned by number and by the hash of the body
   every call cites; a revision is a new version and a new batch, never a re-label. An
   exploration's directions are piloted on the first calls of the batch's execution — as
   many as the host's ceiling, the execution paused after they launch — before the rest
   run. A verification
   whose results show its directions wanting records it as a shortcoming in its findings;
   the question is Brian's to raise, in reviewing-findings, and the fix is a new version
   through preparing-a-verification. Pipeline directions are revised only through
   preparing-pipeline-directions, on Brian's ruling at promotion or baselining that they are
   wrong in general, or on a change of model.
5. **Explicit context for autonomous agents.** Any `agent` process — a reader of one item,
   a classifier, a claiming call, the referee, the calibration sample — is a call through the
   runner from the launch folder outside the repo: the directions body as its system
   prompt and one item as its message, both hashed; no tools; no CLAUDE.md, no skills, no
   memory, no MCP; no transcript persisted. Never through the Agent tool of a repo session, never from a repo cwd.
6. **Claude never creates a hypothesis file autonomously.** Proposals cite the specific
   lead or evidence, Brian rewrites or approves the statement, provenance is recorded.
7. **The story-content boundary.** The framework studies technique, architecture and
   methodology. Thematic content comparisons and what a subject "needs next" are two
   examples of what lies outside it, not the whole list. When a discussion turns to story
   content, the session steers it to what that content asks of the framework, such as whether
   the planner's tracks support it, or says it is out of scope.
8. **Never derive from recall.** Brian's recall is never evidence: it may raise a question,
   or a hypothesis through minting, and a study tests it.
9. **Every artifact declares its mutation, and honours it.** An artifact is edited in
   place, succeeded by a numbered replacement, appended to, or frozen, and its table row
   says which. Appended and frozen artifacts are never edited, save that an entry a session
   wrote may be corrected until it is relied on: conformed to its class without changing what
   it records. An entry is relied on once something that cannot be corrected relies on it,
   directly or through a chain, by a citation the checker resolves or by an entry placed after
   it in a record: a succeeded version, a file the runner or an agent wrote, or an entry itself
   relied on. Until then it is corrected together with whatever relies on it, in one write. A
   file whose sections differ names each section's mutation. Whatever can be derived from an artifact is never
   authored beside it.
10. **Brian's words are quoted where the method keeps its reasoning.** A field that records
   deliberation or reasoning, meaning what raised something, why it exists, what was ruled or
   declined, a judgment, or a suggestion, is composed by the session from the deliberation and
   the records of Brian's typing it draws on. What he typed is his own prose wherever it is
   recorded as his: the deliberation at hand, a user turn or `Typed:` line in a Claude Code
   transcript read from `codesessions.db` (the `code-sessions` corpus), the notes he pasted
   into `source_material_references/hypotheses-google-keep-dump.md` (not the `google-keep`
   corpus), a user block of a conversation in the `conversations` corpus or his navigation note
   on a block. Where the field uses it, it is verbatim inside quotation marks, as typed, with his
   own quotation marks becoming single ones, and needs no pointer to its source; the session's
   words and framing sit outside them. A label he selected, a question put to him, an assistant
   turn or block, session text he approved, and text whose voice is not established as his,
   such as v1 archive and plan notes, are never quoted as his. A field that states a fact or a neutral assertion, such as a finding, a hypothesis statement
   or a question, is not written this way. Each schema's field description says which kind a
   field is. Nothing already written must be rewritten to meet it; Brian may have it rewritten
   to comply.

## Reading the tables

Three tables, parsed by `process-docs/StoryPlanner.DocIntegrity` and held to the folder's
own schema, `schemas/skill-schema.md`: the grammar a session changing this folder follows
is there, with the validator's check ids and a conforming example. A session running an
activity needs only what follows.

**The Router**, below, is one row per activity and what it enables. The companion file
`<id>.md` is the whole instruction for the activity, and that read is the instruction for
every `session` and `hitl` process in it; an `agent` process is instructed by the
directions it reads, and a process that invokes the runner is governed by the
`agent-runner` skill.

**The Processes table**, the first table in each activity file, is one row per process.
`mode` says whose execution it is: `hitl`, a decision that is Brian's is made during it;
`session`, he starts it and reads what it produced; `agent`, it is a call under the
directions with no repo context (rule 5). `reads` and `writes` are the artifact ids the
process takes and leaves, and an `hitl` process writes the artifact that records the
decision made in it; `instruments` are the programs it invokes; `state` says whether the
process type has run under the current text (`built`) or is only written down
(`specified`), never the state of one execution.

**The Artifacts table**, under § Artifacts, is one row per class of files. `path` says
where the files are, with the placeholders § Artifacts lists; `mutation` says how a file
may change (rule 9); `schema` links the file that says what to write and how the class is
read.

**Generated text is files only, and the files are functions of the tables.** `map.md`
holds the activities and their edges, one section per activity with its diagram and what
the tables derive for it (inputs, outputs, instruments, enabled by, enables), the whole
graph, the consumers table and the validation report; `state.md` holds the buildout's
current state (per study, per set of pipeline directions, per question and per hypothesis,
from the artifacts on disk). Both
are written whole by the tool, denied to sessions by path in the project settings, and
rewritten by the write hook after every passing check, so neither can be hand-edited or
stale. Nothing generated sits inside an authored file.

**The tables are checked at the write.** A PostToolUse hook, registered in the project
settings, runs `check` on every file an Edit or Write touches: this folder's shape for a
file inside it, a governed file's schema wherever it lies, by its path or by a reference
that resolves to it. It returns the failures to the session in the same turn and on a pass
rewrites `map.md` and `state.md`. A failure is fixed, row and prose together, before any
other write; it is never worked around, and a write to this folder or to a governed file
never goes through the shell, which the hook cannot see.

## Router — the activities

Read the companion named for your activity in full before acting. An activity not in the
table is not thereby ungoverned: read the closest row's file and say which row was used.

| id | enables | description |
|---|---|---|
| changing-the-planner-for-v3 | | Making the code changes for version 3 of the story planner from baselined hypotheses. The terminus: out of this skill's scope, owns no processes |
| baselining-a-hypothesis | changing-the-planner-for-v3 | Brian's dated judgment, in the record, that a hypothesis's evidence picture is sufficient to act on |
| promoting-refereed-candidates | baselining-a-hypothesis | Brian deciding the pending diagnostic candidates he chooses, by hypothesis or by verification, each after its finding's items are read: promote a verbatim evidence entry to the record, or decline to declined-candidates.md; status recomputed; the candidates view regenerated |
| iterating-a-statement | baselining-a-hypothesis | Brian's rewording of a challenged hypothesis on evidence: the proposed wording re-verified against every current-wording finding, and only if all come out diagnostic-supporting is the statement edited, an iteration entry written as the wording boundary, fresh evidence entries written, and status recomputed |
| minting-a-hypothesis | reviewing-leads reviewing-findings | Creating a hypothesis file on novelty, testability and independence against the current set, in any hitl session, Brian rewriting or approving the statement, provenance in the created entry |
| surfacing-candidates | promoting-refereed-candidates | The autonomous stretch from standing findings to refereed candidates: a claiming batch names, per finding, the hypotheses it bears on; a referee batch judges each claim blind, writing a falsifier and classifying it diagnostic supporting, diagnostic challenging or non-diagnostic; candidates.md is composed from the results as a generated view |
| reviewing-findings | surfacing-candidates | Brian and a session over a verification's findings, or two verifications by their tallies: a finding he doubts checked against the results and the items and withdrawn or superseded, a result he doubts checked at the item and written as a shortcoming, what he raises checked before it is written as a finding, and the questions he raises written into the question list |
| conducting-a-verification | reviewing-findings | One execution of calibrated directions over its itemizer's items for its question, on Brian's approval: the full batch assembled and handed to the host, one call per item, the tally written at completion, and the analysis written as findings |
| preparing-a-verification | conducting-a-verification | Building the measure with Brian for one question: the itemizer built or picked and run, the directions authored against real items, calibrated on a sample batch he scores blind |
| preparing-pipeline-directions | surfacing-candidates iterating-a-statement | Preparing the method's pipeline directions, the referee's or claiming's, with Brian: the plan fixing the model, effort and sample, the sample collated, the directions authored against it and calibrated on a sample batch he scores blind; started by the set's first preparation, his general ruling at promotion or baselining, or a change of model |
| reviewing-leads | preparing-a-verification | Brian and a session over a leads artifact, or two of one question: leads disputed against the source, the differences between explorations read as leads about the readers, and write the questions Brian raises into the question list |
| conducting-an-exploration | reviewing-leads | Reading its itemizer's items discovery-first with its question in view and no hypothesis targeted: one item that is the whole of what the itemizer cuts, or one per slice, each a call through the runner under the study's directions; the results written as leads |
| preparing-an-exploration | conducting-an-exploration | Scoping an exploration with Brian: its question, the scale, the directions written and, for slices, piloted on the batch's first calls |
| asking-a-question | preparing-an-exploration preparing-a-verification | A question Brian raises in a session no other activity's processes cover, written into the question list with what raised it; the one route for a question that arises outside the activities that write their own |
| building-a-tool | preparing-an-exploration preparing-a-verification | Code with tests that carries no judgment: ingests, readers, itemizers, collators, the runner, the validator; CORPORA.md updated when a corpus becomes readable |
| revising-the-method | preparing-an-exploration preparing-a-verification | Changing how the buildout is run: the skill's files and tables rewritten, the validator passing, a write-once revision note recording what changed and why |

## Artifacts — the classes

Every artifact a process in this skill reads or writes. An artifact is a class; the files
matching its pattern, or reached by a reference a governed file declares, are its files. A
file is a document: fixed fields and arrays of entries under one line grammar, with
references the checker resolves; its class's schema is its shape, its mutation its write
discipline, a grep on its line grammar a query, and the generated files its views.
Consumers are never written here; the validator derives them. A schema is the file
`schemas/<name>-schema.md` the `schema` column links to, in four sections each named by its
consumer: Shape, the grammar a writer follows and the hook holds, in the one grammar
`schemas/skill-schema.md` defines; Example, a conforming file with placeholders whose first
fenced block is the fixture its checker in `process-docs/StoryPlanner.DocIntegrity` is
tested against, so a schema edited without its checker fails a test; Queries, one grep per
question a reader asks of the class; Checks, the check ids the hook reports and when each
fails.

Placeholders in paths, the same everywhere: `<study>` a study's folder, which is its
registry id, `<type>-of-<question>` with a slug after it only for a further study of the same
question and type;
`<container>` the folder a batch's home sits under, `studies`, `iterations` or `pipeline`;
`<home>` the folder under it a batch belongs to, a study's id, an iteration's folder,
`referee` or `claiming`; `<batch>` a batch's folder, `<nn>-<slug>`, numbered from 01 within
its home; `<date>` an ISO date; `<Name>` a tool project's name; `NNN` a hypothesis id; `N` a
version number. A study is one directory, `docs/v3-framework/studies/<study>/`: at its top its
authored artifacts, its `directions-N.md` versions and their `calibration-<date>.md` files,
and nothing else; under `batches/<batch>/`, one folder per batch holding what that execution
took in and produced, a verification's claiming and referee batches among them. An
iteration's re-verification batches sit the same way under
`docs/v3-framework/iterations/iteration-of-<hypothesis-file-name>-<N>/batches/<batch>/`, which
is why the batch classes' paths carry `<container>/<home>` rather than naming `studies`
outright. The pipeline directions, the referee's and claiming's, belonging to no study, sit in
`docs/v3-framework/pipeline/referee/` and `docs/v3-framework/pipeline/claiming/`, each laid
out as a study's, its `batches/` holding its calibration samples and nothing else; they are
governed by reference: a batch's definition names them by path, and the checker follows the
path. Everything closed sits in `docs/v3-framework-historical/`, governed by nothing.

| id | path | mutation | schema | description |
|---|---|---|---|---|
| hypothesis-statement | docs/v3-framework/hypotheses/NNN-slug.md § Hypothesis | in-place | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | The current wording of one hypothesis |
| hypothesis-origin | docs/v3-framework/hypotheses/NNN-slug.md § Origin | frozen | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | Why one hypothesis exists: its founding reasoning, written at its mint |
| hypothesis-record | docs/v3-framework/hypotheses/NNN-slug.md § Record | append | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | The evidence relationship: dated entries, never edited |
| hypothesis-index | docs/v3-framework/hypotheses/INDEX.md | in-place | [hypothesis-index-schema](schemas/hypothesis-index-schema.md) | Id and slug per hypothesis, id order |
| question-list | docs/v3-framework/questions.md | append | [question-entry-schema](schemas/question-entry-schema.md) | Brian's open questions, the buildout's one list |
| studies | docs/v3-framework/studies.md | append | [study-registry-schema](schemas/study-registry-schema.md) | One entry per study, declared at Brian's approval of its plan; the ids every study folder is named by |
| state | .claude/skills/v3-buildout/state.md | in-place | | Generated from the registry and the artifacts: per study its batches and where it is; per set of pipeline directions, its versions, the accepted one and its calibration batches; the open questions and whether calibrated directions cover them; per hypothesis, its status |
| revision-note | docs/v3-framework/methodology-revision-N.md | frozen | | What one methodology revision changed and why |
| decisions | docs/v3-framework/decisions.md | append | [decisions-schema](schemas/decisions-schema.md) | The method's decisions: one titled entry per decision, written by a session during revising-the-method as it lands, read only there |
| leads | docs/v3-framework/studies/<study>/leads.md | append | [leads-schema](schemas/leads-schema.md) | What one exploration observed, organised by what was seen |
| findings | docs/v3-framework/studies/<study>/findings.md | append | [findings-schema](schemas/findings-schema.md) | What one verification's analysis drew from its results and tally: findings citing the tally sections and items they rest on, each naming the frozen question it answers if one did; what the data raised as proposed questions; what the results showed wrong with the study's own instrument as shortcomings; entries never edited, only withdrawn or superseded at the review |
| candidates | docs/v3-framework/studies/<study>/candidates.md | in-place | | Generated by compose-candidates through DocIntegrity: the verification's diagnostic candidates, each materialising its finding, verdict, falsifier and outcome, and the non-diagnostic claims at the foot; a view of the claiming and referee results and the outcomes, never hand-edited |
| declined-candidates | docs/v3-framework/studies/<study>/declined-candidates.md | append | [declined-candidates-schema](schemas/declined-candidates-schema.md) | One entry per diagnostic candidate Brian declined, its (finding, target) heading, the date and his reason; a decline is terminal and never edited |
| corpora | .claude/skills/v3-buildout/CORPORA.md | in-place | [corpora-schema](schemas/corpora-schema.md) | The inventory of corpora: per id, what it is, where it lives, how an itemizer reads it, its caveats |
| directions | docs/v3-framework/studies/<study>/directions-N.md | succeeded | [directions-schema](schemas/directions-schema.md) | The system prompt of a batch's calls, hashed as the version, with the questions it freezes or reads with; the pipeline directions', in their folders under docs/v3-framework/pipeline/, reached by reference |
| calibration | docs/v3-framework/studies/<study>/calibration-<date>.md | frozen | [calibration-schema](schemas/calibration-schema.md) | One directions version's agreement with Brian's blind verdicts, the rulings, and whether it is accepted at its hash; the pipeline directions', in their folders under docs/v3-framework/pipeline/, reached by reference |
| definition | docs/v3-framework/<container>/<home>/batches/<batch>/definition.md | frozen | [definition-schema](schemas/definition-schema.md) | What a batch runs under: its directions by path, its kind, its calibration, model and effort; authored by a session, never edited after the first execution |
| index | docs/v3-framework/<container>/<home>/batches/<batch>/index.md | frozen | [index-schema](schemas/index-schema.md) | The items a batch judges, one row each, with what retrieves each from its source; written by the itemizer or the collator |
| items | docs/v3-framework/<container>/<home>/batches/<batch>/items/ | frozen | | The item bodies, one file per item, written by the itemizer or the collator; uncommitted and regenerable |
| calls | docs/v3-framework/<container>/<home>/batches/<batch>/calls.md | append | | The runner's file of calls: the definition's hash at its head, then one entry per call with its hashes, times, exit and check |
| results | docs/v3-framework/<container>/<home>/batches/<batch>/results/ | frozen | | The model's answer for one item as the runner rendered it: the directions' declared fields as keyed lines, one file per item |
| tally | docs/v3-framework/<container>/<home>/batches/<batch>/tally.md | frozen | | The runner's counts over a batch's results: per enum field, the malformed, the missing, the fields not counted; written by the host when the last item has a successful call, once |
| skill | .claude/skills/v3-buildout/ | in-place | [skill-schema](schemas/skill-schema.md) | The method's instructions: the router with its two tables, the activity files and the schema files |
| runner-skill | .claude/skills/agent-runner/SKILL.md | in-place | | The runner's instructions, which govern every process that invokes it |
| map | .claude/skills/v3-buildout/map.md | in-place | | Generated: the whole graph, consumers, validation report |
| tool-source | tools/StoryPlanner.<Name>/ | in-place | | Code with its tests: ingests, readers, itemizers, collators, the runner; the validator lives under process-docs/ and is a free-name instrument, not an artifact |
| corpus | no single pattern | in-place | | The corpora named in CORPORA.md, primary-source data and never a lossy summary made from it, where CORPORA.md says, read by an itemizer as its entry's read through says |

## Companions that are not activities

`schemas/<name>-schema.md` — one file per schema the Artifacts table links to in its
`schema` column: Shape, Example, Queries, Checks, each section named by its consumer; a
schema file not yet in that shape is rewritten to it when it is reviewed, one at a time.
The suffix keeps a schema's file name apart from the file of a singleton class, which
carries the class's own name.
`CORPORA.md` — the corpora: what each is, where it lives, how an itemizer reads it; a fact
file, not an instruction. `map.md` and `state.md` — generated only. The `agent-runner` skill governs
the runner and is read in full by any process that invokes it.

## Vocabulary

- **study**: the life of one set of its own directions over the items of one itemizer under
  one model and effort, for one question, registered at Brian's approval of its plan, one
  folder holding its batches, a verification's holding also the claiming and referee batches
  its chain runs under the pipeline directions; a different model, itemizer or set of its
  own directions is a different study, and a question has any number of studies.
- **verification**: a study of the verification type, one execution of calibrated
  directions over its itemizer's items for its question, its findings the only source of
  candidates.
- **exploration**: a study of the exploration type, a discovery-first reading of its
  itemizer's items with its question in view under piloted directions; its output is a
  leads artifact.
- **batch**: one execution of a set of items under one definition; a study's batches are
  its calibration samples, its full batches and, for a verification, its claiming and
  referee batches; the pipeline directions' calibration samples sit in their own folders,
  and an iteration's re-verify batches under `iterations/`.
- **item**: the one thing a call judges, cut from corpora by an itemizer or collated from
  the method's own files by a collator; a slice is an exploration's item, a partition of a
  corpus; one item may be a corpus whole.
- **call**: one execution of the CLI for one item under a batch's definition; the batch's
  `calls.md` holds one entry per call, and a call is cited by its item and its number.
- **directions**: the file whose body is the system prompt of a batch's calls, versioned by
  number and cited by the hash of its body.
- **definition**: a batch's authored settings, the one authored file in a batch folder.
- **index**: a batch's list of items, written by its itemizer or collator, with a locator
  per item.
- **result**: the model's answer for one item, rendered by the runner as the directions'
  declared fields.
- **tally**: the runner's counts over a batch's results.
- **itemizer**: code with tests under `tools/` that cuts corpora into items, run once per
  batch into the batch's folder; it reads corpora and nothing else, and may read several to
  cut, label and fill the items.
- **collator**: code with tests under `tools/` that collates a pipeline batch's items from
  the method's own files its activity names (findings, claiming results, statements,
  evidence entries), run once per batch into the batch's folder.
- **corpus**: a set of primary-source data the buildout studies, text as it was written
  where it was written: Brian's own, another author's, or an AI's reply in the exchange it
  answered; listed by id in CORPORA.md with where it lives and how it is read; the only
  input of an itemizer. Never a lossy summary or report made afterwards from other text,
  and never a file of the method's other artifact classes.
- **comparison**: two studies over the same items read against each other: by the tally
  where they share their directions, or at the review for two explorations.
- **pilot**: the first calls of a batch's execution, as many as the host's ceiling, the
  execution paused after they launch and their results read by a person before it resumes;
  a one-item batch needs none. The runner knows no pilot: no flag names one and no call is
  marked.
- **dry run**: the checks an execution would make, in memory, with nothing written.
- **rule**: one of the ten constitutional rules above, cited by number.
- **check**: one thing the tool holds, named by its id in a schema's Checks section; the
  verb `check` runs every check that applies to a path.
- **criterion**: a rule in a directions body that decides an item between classes; what a
  classifier applies, what a calibration ruling adds or changes.
- **schema**: the shape of one artifact class's files, in `schemas/<name>-schema.md`;
  `schemas/skill-schema.md` is this folder's own.
- **governed file**: a file of an artifact class that has a schema, matched by its path or
  reached by a declared reference; what a checker holds to that schema.
- **slug**: a machine identifier that reads in a sentence, lowercase `[a-z0-9-]+`, created by
  the session with what it names and never changed; each class's schema says where it is
  unique.
- **lead**: what was seen and what it was seen in, in words; the output of exploration,
  consolidated from its results and citing the slices it came from; an idea for a question
  and for what to itemize; never a finding, never names a position inside a slice.
- **question**: Brian's testable question, in the question list.
- **predicate**: the test a directions version freezes for one question and a classifier
  applies to every item; never written in a question entry, whose `suggested test` is a note.
- **finding**: a conclusion a session drew over a verification's results and tally, the
  verified layer, with the questions in view: a count, a pattern, a contrast, a null
  answer; in the verification's findings file, cited by token, checked at the review,
  never edited, withdrawn or superseded; the only input of claiming.
- **claim**: a (finding, hypothesis) pair a claiming call named, asserting only that the
  finding bears on the hypothesis, never in which direction; unverified; a claiming batch's
  results are its claims, and the referee judges each.
- **candidate**: a claim the referee judged diagnostic, supporting or challenging, and so
  eligible for promotion; a claim judged non-diagnostic is refereed and is not a candidate.
- **falsifier**: what the finding would have been if the statement were false, written
  blind by the referee.
- **evidence**: a promoted finding, in a hypothesis record.
- **challenge**: verified evidence, bound to the current wording, that disagrees with the
  statement; it is carried by an evidence entry tagged `challenging`, it puts the hypothesis
  in `challenged`, and only a reword clears it.
- **dispute**: what Brian raises in a review against a finding, a result or a lead, and the session's
  return to the source to report what it shows: a finding, checked against the results, the
  tally and the items it cites; a result, checked at its item's locator; or a lead, checked
  against the corpus through its cited slices, which takes a reread line. It is never
  evidence and never reaches a hypothesis.
- **outcome**: what Brian did with a diagnostic candidate: promoted or declined.

## Provenance

This skill is the instruction; its provenance lives outside it and is read in one
activity. `docs/v3-framework/decisions.md` holds the method's decisions, written and
read only in revising-the-method; `docs/v3-framework/methodology-revision-N.md` is each
revision's write-once note. `docs/v3-framework/` also holds what the buildout produces:
`hypotheses/`, `questions.md`, `studies.md` and the study folders under `studies/`, and the
pipeline directions' folders under `pipeline/`.
`docs/v3-framework-historical/` holds everything closed: the founding record of decisions
and of questions, the retired plans and handoffs, the retroactive explorations and the two
audit batches of 2026-09, cited as history and never as the method. There is no plan: what
to do next is read from the generated `state.md`, and the pick is Brian's. Provenance
informs and never prescribes.

**What survives a session.** When a session ends, or when Brian asks what must survive it,
the session drafts what it believes must and presents it, as options with their trade-offs
where a choice remains, one decision at a time. A decision about how the buildout
is run enters `decisions.md` only after his approval, as a titled entry in its schema; a
study's own conclusions enter that study's artifacts by its activity's rows; nothing
lands anywhere autonomously, and nothing survives in a handoff.

## What this skill does not govern

Story content decisions; prose technique; planner features (a hypothesis supplies evidence,
CLAUDE.md, `wpf-conventions` and FEATURE-AUDIT supply governance); declaring conclusions.
Every finding is a hypothesis until Brian baselines it, and baselining is his. The buildout
keeps no record of ideas for the planner, Brian's or a session's: changing the planner for v3
reasons from baselined hypotheses alone, and a factual premise an idea rests on may be offered
for minting, where the idea itself is refused as a prescription.
