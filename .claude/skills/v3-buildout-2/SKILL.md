---
name: v3-buildout-2
description: Methodology for the v3 narrative design framework buildout, revision 2 (in construction from 2026-09-04) — thirteen activities from baselining a hypothesis down to building a tool, each with its own companion file carrying its processes table and procedure; the strong-form evidence pipeline (candidates → referee → promotion) as activities; the split of verification into preparing (itemize, author, calibrate with Brian) and rounds (autonomous); the constitutional rules including the artifact-mutation rule. Load before any framework buildout work. Not yet the live skill: the live one is v3-buildout until the router swap.
---

# V3 framework buildout — revision 2 (in construction)

This file is a **router** and a **schema**. It carries what every buildout session must never
miss — the epistemic framework, the constitutional rules — and the table of activities, each
sent to the companion file that holds its processes and its procedure. **Read the file named
for your activity, in full, before acting.** Nothing here is a summary of a companion.

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
   Evidence enters a hypothesis record only as a candidate written from a verification
   round, checked by a fresh-context referee, and promoted in a session with Brian in the
   loop deciding each one. Nothing else writes to `docs/v3-framework/hypotheses/`.
3. **Verification debt.** A corpus whose exploration has run but whose verification round
   has not is *unverified*: nothing cites its leads as evidence, and an exploration over the
   buildout's own outputs reads verified artifacts only. Questions flow freely between
   corpora; leads wait.
4. **A codebook is an instrument.** Authored in a session with Brian, against real items,
   calibrated against his blind verdicts before its first batch, versioned by number and
   hash; every result cites the hash; a revision is a new version and a re-run, never a
   re-label. A round that finds its codebook wanting stops and records it; the question
   is Brian's to raise, in the promotion session.
5. **Explicit context for autonomous agents.** Any agent job — a slice reader, a
   classifier, an auditor, the referee, the calibration sample — runs from
   `RiderProjects\StoryPlanner-fanout` via the runner: instrument + item + output path,
   inlined and hashed; exact toolset; no CLAUDE.md, no skills, no memory, no MCP unless the
   job opts in; no transcript persisted. Never through the Agent tool of a repo session,
   never from a repo cwd.
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

## Schema — the tables every file in this skill obeys

Three tables, fixed columns, parsed by `process-docs/StoryPlanner.DocIntegrity`: header and separator
are structure, every body row is one unit, cells never contain `|`, ids are lowercase
`[a-z0-9-]+` and unique across all tables, lists are space-separated ids.

**Activities** — the router table below. `id · enables · description`. An activity is
something Brian does, named as a gerund with its object. `enables` lists the activities it
enables; the graph is acyclic; exactly one activity enables nothing and owns no processes
(the terminus). The activity's companion file is `<id>.md` in this folder.

**Processes** — the first table in each activity file. `id · mode · instruments · reads ·
writes · state · description`. A process is one run of one mode, reading some artifacts and
writing others; the activity is the file it sits in, and that file is its instruction
source. Two exceptions are derivable: an `agent` process is instructed by the instrument it
reads, and a process that invokes the runner is governed by the `agent-runner` skill.
**A process splits only at a change of mode or when it invokes the runner**; steps done by
the same session in the same activity are one process.
- `mode` ∈ `hitl · session · agent`, **by decision**: `hitl` if a decision that is Brian's
  is made during the process; `session` if he only starts it and reads what it produced;
  `agent` if it runs under an inlined instrument with no repo context (rule 5).
- `instruments` are the programs the process invokes: the artifact id where the program is
  code in the Artifacts table (an itemizer, a generator, a tallier), otherwise its name
  (the runner, dotnet, git), or empty. An artifact named as an instrument counts as read.
- `reads` and `writes` are artifact ids, at least one each. Every `hitl` process writes the
  artifact that records the decision made in it.
- `state` ∈ `built · specified`: executed at least once under the current text, or only
  written down. Development state of the process type; never the state of a run.

**Artifacts** — the table under § Artifacts below. `id · path · mutation · format ·
description`. An artifact is a class; the files matching its pattern are its files. `path`
is one repo-relative pattern, placeholders in angle brackets, or `outside the repo`; never
prose. `mutation` ∈ `in-place · succeeded · append · frozen` (rule 9). `format` is the id
of the file `formats/<id>.md` that specifies the artifact's shape, a lowercase slug whose
file's title is that id, or blank where none is authored.

**An activity file** has one shape and carries procedure only: the title (the activity
id); one line naming what it enables; the Processes table;
`## Preconditions`, the state each input must be in, never a list of inputs; one
`## <process id>` section per process in execution order; `## Never`, activity-specific
only. A session reads the file whole at the start of the activity, and that read is the
instruction for every `session` and `hitl` process in it. An `hitl` section says what the
session prepares and presents, how it batches Brian's questions, and what it writes as each
decision lands; it cannot script the middle. An `agent` section names the instrument and
what the generator materialises for it; the agent never sees the file. A runner section
names the run's reads and writes and defers to the `agent-runner` skill. Nothing in the
prose restates the table, an artifact's mutation, a rule, or a word defined in this file
or a format file. Files are types: no corpus or study appears in one.

**Derived, never authored:** order and data flow (a process reads what another wrote),
consumers of each artifact, each activity's inputs, outputs and instruments, and the check
that every `enables` has a supporting data flow. There is no edges table.

**Generated text is files only, and the files are functions of the tables.** `map.md`
holds the activities and their edges, one section per activity with its diagram and what
the tables derive for it (inputs, outputs, instruments, enabled by, enables), the whole
graph, the consumers table and the validation report; `state.md` holds the buildout's
current state (per corpus and per hypothesis, from the artifacts on disk). Both are written
whole by the tool, denied to sessions by path in the project settings, and rewritten by the
write hook after every passing check, so neither can be hand-edited or stale. Nothing
generated sits inside an authored file.

**The tables are checked at the write.** A PostToolUse hook, registered in the project
settings, runs `check` on every file an Edit or Write touches: this folder's shape for a
file inside it, a governed file's format wherever it lies. It returns the failures to the
session in the same turn and on a pass rewrites `map.md` and `state.md`. A failure is
fixed, row and prose together, before any other write; it is never worked around, and a
write to this folder or to a governed file never goes through the shell, which the hook
cannot see.

## Router — the activities

Read the companion named for your activity in full before acting. An activity not in the
table is not thereby ungoverned: read the closest row's file and say which row was used.

| id | enables | description |
|---|---|---|
| changing-the-planner-for-v3 | | Making the code changes for version 3 of the story planner from baselined hypotheses. The terminus: out of this skill's scope, owns no processes |
| baselining-a-hypothesis | changing-the-planner-for-v3 | Brian's dated judgment, in his words in the record, that a hypothesis's evidence picture is sufficient to act on |
| promoting-checked-candidates | baselining-a-hypothesis | Brian deciding the pending referee-diagnostic candidates he chooses, by hypothesis or by round, each after its cited source is read: promote verbatim or decline; one outcome line per candidate; one commit |
| iterating-a-statement | refereeing-a-candidate | Brian's rewording of a hypothesis on evidence: the statement edited, an iteration entry as the wording boundary, status recomputed, prior findings queued as iteration candidates |
| minting-a-hypothesis | reviewing-leads | Creating a hypothesis file on novelty, testability and independence against the current set, in any hitl session, Brian rewriting or approving the statement, provenance in the created entry |
| refereeing-a-candidate | promoting-checked-candidates | A blind agent given only the current statement and the candidate's finding writes a falsifier and classifies it diagnostic supporting, diagnostic challenging, or non-diagnostic |
| writing-candidates-from-verification | refereeing-a-candidate | A session writes one candidate per finding a round claims bears on a hypothesis: target, finding, source locator, proposer with hash; append-only, no falsifier |
| conducting-a-verification-round | writing-candidates-from-verification | One execution of a calibrated instrument over a corpus's items, on Brian's go: fan out through the runner, tally, write the artifact with per-item results and hashes |
| preparing-to-verify-a-corpus | conducting-a-verification-round refereeing-a-candidate | Building the measuring instrument with Brian: itemize the corpus, author the codebook against real items, calibrate it on a sample he scores blind, hash it |
| reviewing-leads | preparing-to-verify-a-corpus | Brian and a session over a leads artifact: drill the bins, challenge leads against the source, and write the questions Brian raises into the corpus's question list |
| exploring-a-corpus | reviewing-leads | Reading a corpus discovery-first with a question in view and no hypothesis targeted: a pathfinder in one session, or slice readers through the runner, joined and binned; output a leads artifact |
| preparing-to-explore-a-corpus | exploring-a-corpus | Scoping an exploration with Brian: the card's question and the corpus's question list, the scale, the reading protocol and read-manifest if slices, the plan approved, the protocol piloted |
| building-a-tool | preparing-to-explore-a-corpus preparing-to-verify-a-corpus | Code with tests that carries no judgment: ingests, readers, the runner, talliers, renders, the validator; CORPORA.md updated when a corpus becomes readable |
| revising-the-method | preparing-to-explore-a-corpus preparing-to-verify-a-corpus | Changing how the buildout is run: the skill's files and tables rewritten, two lints passing (the validator; for a rewrite, the supersession audit of the prior text), a write-once revision note recording what changed and why |

## Artifacts — the classes

Every artifact a process in this skill reads or writes. An artifact is a class; the files
matching its pattern are its files. Consumers are never written here; the validator
derives them. A format is the file `formats/<id>.md` the `format` column names: its
example block is the fixture its checker in `process-docs/StoryPlanner.DocIntegrity` is
tested against, the block and the grammar sentences around it are what the machine reads,
the guidance sentences are for the author, and a format edited without its checker fails
a test.

Placeholders in paths, the same everywhere: `<corpus>` a name from `CORPORA.md`;
`<study>` a study's folder, which is its registry id (`exploration-of-<corpus>[-<n>]`
or `round-of-<corpus>-<n>`), except that every `referee-<n>` shares the folder `referee`
and the method's own supersession audit, a work outside the buildout's studies, runs
under `skill-audits`; `<run>` a runner run folder, `<date>[-<slug>]`, one per batch
execution; `<date>` an ISO date; `<Name>` a tool project's name; `NNN` a hypothesis id;
`N` a version number. A study's authored artifacts live in one directory named by it
under `docs/v3-framework/`; its runner input and output, including the referee runs that
serve it, live under `fanout/<study>/`. `fanout/referee/` holds only the referee's
shared instrument and the iteration candidates.

| id | path | mutation | format | description |
|---|---|---|---|---|
| hypothesis-statement | docs/v3-framework/hypotheses/NNN-slug.md § Hypothesis | in-place | hypothesis-file | The current wording of one hypothesis |
| hypothesis-record | docs/v3-framework/hypotheses/NNN-slug.md § Record | append | hypothesis-file | The evidence relationship: dated entries, never edited |
| hypothesis-status | docs/v3-framework/hypotheses/NNN-slug.md frontmatter | in-place | hypothesis-file | Status and baselined, computed from the record |
| hypothesis-index | docs/v3-framework/hypotheses/INDEX.md | in-place | hypothesis-index | Id and slug per hypothesis, id order |
| question-list | docs/v3-framework/questions/<corpus>.md | append | question-entry | Brian's open questions about one corpus |
| studies | docs/v3-framework/studies.md | append | study-registry | One row per study of a chain, declared at Brian's go; the ids every artifact path is named by |
| state | .claude/skills/v3-buildout/state.md | in-place | | Generated from the registry and the artifacts: per study where it is; per corpus, open questions and whether a calibrated codebook covers them; per hypothesis, status and whether any open question names it |
| revision-note | docs/v3-framework/methodology-revision-N.md | frozen | | What one methodology revision changed and why |
| decisions | docs/v3-framework/decisions.md | append | decisions | The method's decisions: one titled entry per decision, written by a session during revising-the-method as it lands, read only there |
| leads-artifact | docs/v3-framework/<study>/leads.md | append | leads-artifact | What one exploration observed, organised by locus |
| verification-artifact | docs/v3-framework/<study>/round.md | append | verification-artifact | One round's method, questions answered, counts and promotion summary |
| arm-key | docs/v3-framework/<study>/arm-key.md | frozen | arm-key | The blinding key: arm label to condition and model; opened only after binning |
| candidates | fanout/<study>/candidates.md | append | candidate | One round's findings claimed to bear on a hypothesis, with referee lines and outcomes |
| iteration-candidates | fanout/referee/iterations/NNN-<date>/candidates.md | append | candidate | Prior findings re-queued after a rewording of hypothesis NNN |
| corpora | .claude/skills/v3-buildout/CORPORA.md | in-place | corpora | The inventory of corpora: per id, what it is, where it lives, how it is read, its caveats |
| codebook | fanout/<study>/codebook-N.md | succeeded | codebook | The frozen instrument a round or the referee runs under |
| reading-protocol | fanout/<study>/protocol-N.md | succeeded | reading-protocol | The instruction slice readers run under; piloted, not calibrated |
| calibration | fanout/<study>/calibration-<date>.md | frozen | calibration | One codebook version's agreement with Brian's blind verdicts, and the rulings |
| itemizer | fanout/<study>/itemize.* | in-place | | A script that produces a corpus's items; an itemizer that is a tool project is tool-source |
| generator | fanout/<study>/make-jobs.* | in-place | | Code that writes jobs from the manifest |
| tallier | fanout/<study>/tally.* | in-place | | Code that reduces results to counts and flagged rows |
| items | fanout/<study>/<run>/items/ | frozen | | The units one run judges, one file each; a referee run's sit under fanout/<study>/referee/<run>/ |
| items-manifest | fanout/<study>/<run>/items/manifest.md | frozen | | The index of a run's items |
| jobs | fanout/<study>/<run>/jobs.json | frozen | | One run's job file; an edit is a new run |
| ledger | fanout/<study>/<run>/ledger.jsonl | append | | One row per attempt, with hashes |
| results | fanout/<study>/<run>/results/ | frozen | | The agents' outputs, one per job |
| tally-output | fanout/<study>/<run>/tally.md | frozen | | The tallier's counts and flagged rows for one run |
| run-page | fanout/<study>/<run>/run.md | append | run-page | The authored front page of one run |
| skill | .claude/skills/v3-buildout/ | in-place | | The method's instructions: the router with its two tables, the activity files and the format files |
| runner-skill | .claude/skills/agent-runner/SKILL.md | in-place | | The runner's instructions, which govern every process that invokes it |
| map | .claude/skills/v3-buildout/map.md | in-place | | Generated: the whole graph, consumers, validation report |
| audit-protocol | fanout/skill-audits/protocol.md | in-place | | The supersession audit's instrument: three questions about one unit of a prior method text, judged against the new folder |
| tool-source | tools/StoryPlanner.<Name>/ | in-place | | Code with its tests: ingests, readers, the runner; the validator lives under process-docs/ and is a free-name instrument, not an artifact |
| corpus | outside the repo | in-place | | The corpora named in CORPORA.md, read through the MCP server, files or sqlite3 |

## Companions that are not activities

`formats/<id>.md` — one file per format the Artifacts table names in its `format` column:
the class's schema, its example block as the checker's fixture, and how the class is read.
`CORPORA.md` — the corpora: what each is, where it lives, how it is read; a fact file, not
a rule. `map.md` and `state.md` — generated only. The `agent-runner` skill governs the
runner as an instrument and is read in full by any process that invokes it.

## Vocabulary

- **study**: one run of one activity chain over one corpus, declared in the registry.
- **governed file**: a file of an artifact class that has a format; what a checker holds
  to that format.
- **round**: one execution of a calibrated codebook over a corpus's items; repeats.
- **item**: the unit one agent job judges, produced by an itemizer; a **slice** is the
  exploration's item, a partition of a corpus.
- **arm**: one condition in an exploration that runs the same slices under several.
- **lead**: a locus and what was observed there; the output of exploration; never a claim.
- **question**: Brian's testable question about one corpus, in its question list.
- **result**: an agent's output for one item under an instrument.
- **finding**: what a round observed that a session claims bears on a hypothesis; on a
  candidate.
- **falsifier**: what the finding would have been if the statement were false, written
  blind by the referee.
- **evidence**: a promoted finding, in a hypothesis record.
- **outcome**: what Brian did with a diagnostic candidate: promoted or declined.

## Provenance

This skill is the instruction; its provenance lives outside it and is read in one
activity. `docs/v3-framework/decisions.md` holds the method's decisions, written and
read only in revising-the-method; `docs/v3-framework/methodology-revision-N.md` is each
revision's write-once note. `docs/v3-framework/` also holds what the buildout produces:
`hypotheses/`, `questions/`, the leads and verification artifacts named
`exploration-of-<corpus>` and `round-of-<corpus>-<n>`, the retired forward plans of
revision 1 (reference only), and `implementation-candidates.md` (codebase changes gated on
baselined hypotheses — they enter the ordinary feature process, never this skill).
`fanout/` holds what the runner takes in and puts out, one folder per work. There is no
plan: what to do next is read from the generated `state.md`, and the pick is Brian's.
Provenance informs and never prescribes.

**What survives a session.** When a session ends, or when Brian asks what must survive it,
the session drafts what it believes must and presents it, as options with their trade-offs
where a choice remains, one decision at a time. A decision about how the buildout
is run enters `decisions.md` only after his approval, as a titled entry in its format; a
study's own conclusions enter that study's artifacts by its activity's rows; nothing
lands anywhere autonomously, and nothing survives in a handoff.

## What this skill does not govern

Story content decisions; prose technique; planner features (a hypothesis supplies evidence,
CLAUDE.md, `wpf-conventions` and FEATURE-AUDIT supply governance); declaring conclusions.
Every finding is a hypothesis until Brian baselines it, and baselining is his.
