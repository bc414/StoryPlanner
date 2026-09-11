---
name: v3-buildout-2
description: "Methodology for the v3 narrative design framework buildout, revision 2 (in construction from 2026-09-04) — thirteen activities from baselining a hypothesis down to building a tool, each with its own companion file carrying its processes table and procedure; the strong-form evidence pipeline (candidates → referee → promotion) as activities; studies as folders of batches run through the agent runner under directions; the split of verification into preparing (itemize, author, calibrate with Brian) and verifications (autonomous); the constitutional rules including the artifact-mutation rule. Load before any framework buildout work. Not yet the live skill: the live one is v3-buildout until the router swap."
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

**Every claim is a hypothesis with an evidence relationship.** A hypothesis file holds the
claim and that relationship together: the statement, current and edited in place; the
record, dated entries never edited, which *is* the evidence relationship rather than a
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
**Only verified evidence moves a hypothesis.** Baselining is a separate field: Brian's dated
judgment that the evidence picture is sufficient to act on. It is progress tracking, not
epistemology — it adds no weight, removes no challengeability, is not endorsement of truth,
is itself bound to a wording, and resets to `false` when a challenging entry lands or the
wording changes. Only Brian baselines; a session may name candidates ("verified support, no
open challenge — review for baselining") and never sets the field.

**Recall is atmosphere; evidence is grounding.** A statement about the data from anyone —
Brian from memory, a prior session, a memory file, this skill, any document — is a
hypothesis about what the data says. Before acting on it: query the source, compare, present
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
   Evidence enters a hypothesis record only as a candidate written from a verification,
   checked by a fresh-context referee, and promoted in a session with Brian in the
   loop deciding each one. Nothing else writes to `docs/v3-framework/hypotheses/`. Every
   candidate of every verification is judged under the same referee directions at the
   same hash: the referee is one part of the method, and a verification runs it and never
   authors it.
3. **Verification debt.** A corpus whose exploration has run but whose verification
   has not is *unverified*: nothing cites its leads as evidence. A study over the
   buildout's own outputs that makes a claim about a corpus reads verified artifacts only;
   a claim about the method is not so guarded. Questions flow freely between corpora;
   leads wait.
4. **Directions are calibrated or piloted before their first batch.** A verification's
   directions are authored in a session with Brian, against real items, calibrated against
   his blind verdicts before their first full batch, versioned by number and by the hash
   of the body every call cites; a revision is a new version and a new batch, never a
   re-label. An exploration's directions are piloted on one item before the rest run. A
   verification whose results show its directions wanting records it as a shortcoming in
   its findings; the question is Brian's to raise, in reviewing-findings, and the fix is a
   new version through preparing.
5. **Explicit context for autonomous agents.** Any `agent` process — a reader of one item,
   a classifier, an auditor, the referee, the calibration sample — is a call through the
   runner from the launch folder outside the repo: the directions body as its system
   prompt and one item as its message, both hashed; an exact toolset; no CLAUDE.md, no
   skills, no memory, no MCP unless the batch's definition opts in; no transcript
   persisted. Never through the Agent tool of a repo session, never from a repo cwd.
6. **Claude never creates a hypothesis file autonomously.** Proposals cite the specific
   lead or evidence, Brian rewrites or approves the statement, provenance is recorded.
7. **The story-content boundary.** The framework studies technique, architecture and
   methodology. Thematic content comparisons and what a subject "needs next" are two
   examples of what lies outside it, not the whole list; redirect to the framework-relevant
   question or say it is out of scope.
8. **Never derive from recall.** Brian's recall about his own practice is the hypothesis
   under test, not evidence for it; it goes to a question list as a question.
9. **Every artifact declares its mutation, and honours it.** An artifact is edited in
   place, succeeded by a numbered replacement, appended to, or frozen, and its table row
   says which. Appended and frozen artifacts are never edited. A file whose sections differ
   names each section's mutation. Whatever can be derived from an artifact is never
   authored beside it.

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
current state (per study, per corpus and per hypothesis, from the artifacts on disk). Both
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
| baselining-a-hypothesis | changing-the-planner-for-v3 | Brian's dated judgment, in his words in the record, that a hypothesis's evidence picture is sufficient to act on |
| promoting-refereed-candidates | baselining-a-hypothesis | Brian deciding the pending diagnostic candidates he chooses, by hypothesis or by verification, each after its finding's items are read: promote a verbatim evidence entry to the record, or decline to declined-candidates.md; status recomputed; the candidates view regenerated; one commit |
| iterating-a-statement | baselining-a-hypothesis | Brian's rewording of a challenged hypothesis on evidence: the proposed wording re-verified against every current-wording finding, and only if all come out diagnostic-supporting is the statement edited, an iteration entry written as the wording boundary, fresh evidence entries written, and status recomputed |
| minting-a-hypothesis | reviewing-leads reviewing-findings | Creating a hypothesis file on novelty, testability and independence against the current set, in any hitl session, Brian rewriting or approving the statement, provenance in the created entry |
| surfacing-candidates | promoting-refereed-candidates | The autonomous stretch from standing findings to refereed candidates: a claiming batch names, per finding, the hypotheses it bears on; a referee batch judges each claim blind, writing a falsifier and classifying it diagnostic supporting, diagnostic challenging or non-diagnostic; candidates.md is composed from the results as a generated view |
| reviewing-findings | surfacing-candidates | Brian and a session over a verification's findings, or two verifications by their tallies: a finding he doubts checked against the results and the items and withdrawn or superseded, a result he doubts checked at the item and written as a shortcoming, what he raises checked before it is written as a finding, and the questions he raises written into the corpus's question list |
| verifying-a-corpus | reviewing-findings | One execution of calibrated directions over a corpus's items, on Brian's go: the full batch assembled and handed to the host, one call per item, the tally written at completion, and the analysis written as findings |
| preparing-to-verify-a-corpus | verifying-a-corpus surfacing-candidates | Building the measure with Brian: itemize the corpus by a tool, author the directions against real items, calibrate them on a sample batch he scores blind |
| reviewing-leads | preparing-to-verify-a-corpus | Brian and a session over a leads artifact, or two of one corpus: challenge leads against the source, read the differences between explorations as leads about the readers, and write the questions Brian raises into the corpus's question list |
| exploring-a-corpus | reviewing-leads | Reading a corpus discovery-first with a question in view and no hypothesis targeted: one item that is the corpus whole, or one per slice, each a call through the runner under the study's directions; the results written as leads |
| preparing-to-explore-a-corpus | exploring-a-corpus | Scoping an exploration with Brian: the questions in view, the scale, the directions written and, for slices, piloted on one item |
| building-a-tool | preparing-to-explore-a-corpus preparing-to-verify-a-corpus | Code with tests that carries no judgment: ingests, readers, itemizers, the runner, the validator; CORPORA.md updated when a corpus becomes readable |
| revising-the-method | preparing-to-explore-a-corpus preparing-to-verify-a-corpus | Changing how the buildout is run: the skill's files and tables rewritten, two lints passing (the validator; for a rewrite, the supersession audit of the prior text as an audit study), a write-once revision note recording what changed and why |

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

Placeholders in paths, the same everywhere: `<corpus>` a name from `CORPORA.md`;
`<study>` a study's folder, which is its registry id, `<type>-of-<corpus>-<slug>` for a
verification, `exploration-of-<corpus>` with a slug only when the corpus is explored again,
`audit-of-<slug>` for an audit of the skill; `<batch>` a batch's folder, `<nn>-<slug>`,
numbered from 01 within its study; `<container>` the folder a batch's directory sits under,
`studies` for a study or `iterations` for an iteration's re-verification; `<date>` an ISO
date; `<Name>` a tool project's name; `NNN` a hypothesis id; `N` a version number. A study is
one directory, `docs/v3-framework/studies/<study>/`: at its top its authored artifacts, its
`directions-N.md` versions and their `calibration-<date>.md` files, and nothing else; under
`batches/<batch>/`, one folder per batch holding what that execution took in and produced. An
iteration's re-verification batches sit the same way under
`docs/v3-framework/iterations/iteration-of-<hypothesis-file-name>-<N>/batches/<batch>/`, which
is why the batch classes' paths carry `<container>` rather than naming `studies` outright.
The referee's directions and calibrations, belonging to no study, sit in
`docs/v3-framework/referee/` and are governed by reference: a batch's definition names them
by path, and the checker follows the path. Everything closed sits in
`docs/v3-framework-historical/`, governed by nothing.

| id | path | mutation | schema | description |
|---|---|---|---|---|
| hypothesis-statement | docs/v3-framework/hypotheses/NNN-slug.md § Hypothesis | in-place | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | The current wording of one hypothesis |
| hypothesis-record | docs/v3-framework/hypotheses/NNN-slug.md § Record | append | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | The evidence relationship: dated entries, never edited |
| hypothesis-status | docs/v3-framework/hypotheses/NNN-slug.md frontmatter | in-place | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | Status and baselined, computed from the record |
| hypothesis-index | docs/v3-framework/hypotheses/INDEX.md | in-place | [hypothesis-index-schema](schemas/hypothesis-index-schema.md) | Id and slug per hypothesis, id order |
| question-list | docs/v3-framework/questions/<corpus>.md | append | [question-entry-schema](schemas/question-entry-schema.md) | Brian's open questions about one corpus |
| studies | docs/v3-framework/studies.md | append | [study-registry-schema](schemas/study-registry-schema.md) | One row per study, declared at Brian's go; the ids every study folder is named by |
| state | .claude/skills/v3-buildout/state.md | in-place | | Generated from the registry and the artifacts: per study its batches and where it is; per corpus, open questions and whether calibrated directions cover them; per hypothesis, status and whether any open question names it |
| revision-note | docs/v3-framework/methodology-revision-N.md | frozen | | What one methodology revision changed and why |
| decisions | docs/v3-framework/decisions.md | append | [decisions-schema](schemas/decisions-schema.md) | The method's decisions: one titled entry per decision, written by a session during revising-the-method as it lands, read only there |
| leads | docs/v3-framework/studies/<study>/leads.md | append | [leads-schema](schemas/leads-schema.md) | What one exploration observed, organised by what was seen |
| findings | docs/v3-framework/studies/<study>/findings.md | append | [findings-schema](schemas/findings-schema.md) | What one verification's analysis drew from its results and tally: findings citing the tally sections and items they rest on, each naming the frozen question it answers if one did; what the data raised as proposed questions; what the results showed wrong with the study's own instrument as shortcomings; entries never edited, only withdrawn or superseded at the review |
| candidates | docs/v3-framework/studies/<study>/candidates.md | in-place | | Generated by compose-candidates through DocIntegrity: the verification's diagnostic candidates, each materialising its finding, verdict, falsifier and outcome, and the non-diagnostic claims at the foot; a view of the claiming and referee results and the outcomes, never hand-edited |
| declined-candidates | docs/v3-framework/studies/<study>/declined-candidates.md | append | [declined-candidates-schema](schemas/declined-candidates-schema.md) | One entry per diagnostic candidate Brian declined, its (finding, target) heading, the date and his reason; a decline is terminal and never edited |
| corpora | .claude/skills/v3-buildout/CORPORA.md | in-place | [corpora-schema](schemas/corpora-schema.md) | The inventory of corpora: per id, what it is, where it lives, how it is read, its caveats |
| directions | docs/v3-framework/studies/<study>/directions-N.md | succeeded | [directions-schema](schemas/directions-schema.md) | The system prompt of a batch's calls, hashed as the version, with the questions it freezes or reads with; the referee's, in the referee folder, reached by reference |
| calibration | docs/v3-framework/studies/<study>/calibration-<date>.md | frozen | [calibration-schema](schemas/calibration-schema.md) | One directions version's agreement with Brian's blind verdicts, the rulings, and whether it is accepted at its hash; the referee's reached by reference |
| definition | docs/v3-framework/<container>/<study>/batches/<batch>/definition.md | frozen | [definition-schema](schemas/definition-schema.md) | What a batch runs under: its directions by path, its kind, its calibration, model, effort, tools and MCP; authored by a session, never edited after the first execution |
| index | docs/v3-framework/<container>/<study>/batches/<batch>/index.md | frozen | [index-schema](schemas/index-schema.md) | The items a batch judges, one row each, with what retrieves each from the corpus; written by the itemizer |
| items | docs/v3-framework/<container>/<study>/batches/<batch>/items/ | frozen | | The item bodies, one file per item, written by the itemizer; uncommitted and regenerable |
| calls | docs/v3-framework/<container>/<study>/batches/<batch>/calls.md | append | | The runner's file of calls: the definition's hash at its head, then one entry per call with its hashes, times, exit and check |
| results | docs/v3-framework/<container>/<study>/batches/<batch>/results/ | frozen | | The model's answer for one item as the runner rendered it: the directions' declared fields as keyed lines, one file per item |
| tally | docs/v3-framework/<container>/<study>/batches/<batch>/tally.md | frozen | | The runner's counts over a batch's results: per enum field, the malformed, the missing, the fields not counted; written by the host when the last item has a successful call, once |
| skill | .claude/skills/v3-buildout/ | in-place | [skill-schema](schemas/skill-schema.md) | The method's instructions: the router with its two tables, the activity files and the schema files |
| runner-skill | .claude/skills/agent-runner/SKILL.md | in-place | | The runner's instructions, which govern every process that invokes it |
| map | .claude/skills/v3-buildout/map.md | in-place | | Generated: the whole graph, consumers, validation report |
| tool-source | tools/StoryPlanner.<Name>/ | in-place | | Code with its tests: ingests, readers, itemizers, the runner; the validator lives under process-docs/ and is a free-name instrument, not an artifact |
| corpus | outside the repo | in-place | | The corpora named in CORPORA.md, read through the MCP server, files or sqlite3 |

## Companions that are not activities

`schemas/<name>-schema.md` — one file per schema the Artifacts table links to in its
`schema` column: Shape, Example, Queries, Checks, each section named by its consumer; a
schema file not yet in that shape is rewritten to it when it is reviewed, one at a time.
The suffix keeps a schema's file name apart from the file of a singleton class, which
carries the class's own name.
`CORPORA.md` — the corpora: what each is, where it lives, how it is read; a fact file, not
an instruction. `map.md` and `state.md` — generated only. The `agent-runner` skill governs
the runner and is read in full by any process that invokes it.

## Vocabulary

- **study**: the life of one set of directions over the items of one itemizer under one
  model and effort, registered at Brian's go, one folder holding its batches; a different
  model, itemizer or set of directions is a different study.
- **verification**: a study of the verification type, one execution of calibrated
  directions over a corpus's items, its findings the only source of candidates.
- **exploration**: a study of the exploration type, a reading of a corpus discovery-first
  under piloted directions; its output is a leads artifact.
- **audit**: a study of the audit type, whose corpus is the skill's own text, run by
  revising-the-method as the second lint of a rewrite.
- **batch**: one execution of a set of items under one definition; a study's batches are
  its calibration samples, its full batches and, for a verification, its referee batches.
- **item**: the one thing a call judges, cut from the corpus by an itemizer; a slice is an
  exploration's item, a partition of the corpus; one item may be the corpus whole.
- **call**: one execution of the CLI for one item under a batch's definition; the batch's
  `calls.md` holds one entry per call, and a call is cited by its item and its number.
- **directions**: the file whose body is the system prompt of a batch's calls, versioned by
  number and cited by the hash of its body.
- **definition**: a batch's authored settings, the one authored file in a batch folder.
- **index**: a batch's list of items, written by its itemizer, with a locator per item.
- **result**: the model's answer for one item, rendered by the runner as the directions'
  declared fields.
- **tally**: the runner's counts over a batch's results.
- **itemizer**: code with tests under `tools/` that cuts a corpus into items, run once per
  batch into the batch's folder.
- **comparison**: two studies over the same items read against each other: by the tally
  where they share their directions, at the review for two explorations, or by a study of
  its own where judgment is needed.
- **pilot**: an execution of a batch naming one item, whose result a person reads before
  the rest run; a one-item batch needs none.
- **dry run**: the checks an execution would make, in memory, with nothing written.
- **rule**: one of the nine constitutional rules above, cited by number.
- **check**: one thing the tool holds, named by its id in a schema's Checks section; the
  verb `check` runs every check that applies to a path.
- **criterion**: a rule in a directions body that decides an item between classes; what a
  classifier applies, what a calibration ruling adds or changes.
- **schema**: the shape of one artifact class's files, in `schemas/<name>-schema.md`;
  `schemas/skill-schema.md` is this folder's own.
- **governed file**: a file of an artifact class that has a schema, matched by its path or
  reached by a declared reference; what a checker holds to that schema.
- **lead**: what was seen and what it was seen in, in words; the output of exploration;
  an idea for a question and for what to itemize; never a claim, never checked at an
  address.
- **question**: Brian's testable question about one corpus, in its question list.
- **predicate**: the test a directions version freezes for one question and a classifier
  applies to every item; never written in a question entry, whose `suggested test` is a note.
- **finding**: a conclusion a session drew over a verification's results and tally, the
  verified layer, with the questions in view: a count, a pattern, a contrast, a null
  answer; in the verification's findings file, cited by token, checked at the review,
  never edited, withdrawn or superseded; the only input of a candidate.
- **falsifier**: what the finding would have been if the statement were false, written
  blind by the referee.
- **evidence**: a promoted finding, in a hypothesis record.
- **outcome**: what Brian did with a diagnostic candidate: promoted or declined.

## Provenance

This skill is the instruction; its provenance lives outside it and is read in one
activity. `docs/v3-framework/decisions.md` holds the method's decisions, written and
read only in revising-the-method; `docs/v3-framework/methodology-revision-N.md` is each
revision's write-once note. `docs/v3-framework/` also holds what the buildout produces:
`hypotheses/`, `questions/`, `studies.md` and the study folders under `studies/`, the
referee's folder, and `implementation-candidates.md` (codebase changes gated on baselined
hypotheses — they enter the ordinary feature process, never this skill).
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
Every finding is a hypothesis until Brian baselines it, and baselining is his.
