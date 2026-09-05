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
named in `CORPUS-STATUS.md`, `codesessions.db` by sqlite3, and web search; CLAUDE.md carries
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

Three tables, fixed columns, parsed by `tools/StoryPlanner.ProcessMap`: header and separator
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

**Artifacts** — the table in `artifacts.md`. `id · path · mutation · format · description`.
An artifact is a class; the files are its instances. `path` is one repo-relative pattern,
placeholders in angle brackets, or `outside the repo`; never prose. `mutation` ∈ `in-place ·
succeeded · append · frozen` (rule 9). `format` is the heading in `artifacts.md` that
specifies the artifact's shape, or blank where none is authored.

**An activity file** has one shape and carries procedure only: the title (the activity
id); one line naming what it enables; the Processes table; the generated section;
`## Preconditions`, the state each input must be in, never a list of inputs; one
`## <process id>` section per process in execution order; `## Never`, activity-specific
only. A session reads the file whole at the start of the activity, and that read is the
instruction for every `session` and `hitl` process in it. An `hitl` section says what the
session prepares and presents, how it batches Brian's questions, and what it writes as each
decision lands; it cannot script the middle. An `agent` section names the instrument and
what the generator materialises for it; the agent never sees the file. A runner section
names the run's reads and writes and defers to the `agent-runner` skill. Nothing in the
prose restates the table, an artifact's mutation, a rule, or a word defined in this file
or `artifacts.md`. Files are types: no corpus or instance appears in one.

**Derived, never authored:** order and data flow (a process reads what another wrote),
consumers of each artifact, each activity's inputs, outputs and instruments, and the check
that every `enables` has a supporting data flow. There is no edges table.

**Generated sections** sit between `<!-- generated:<name> -->` and `<!-- /generated -->`
and are never hand-edited: `level-1` in this file, `activity` in each activity file, the
whole graph, the consumers table and the validation report in `map.md`, and the buildout's
current state in `state.md` (per corpus and per hypothesis, from the artifacts on disk).
Neither generated file holds anything authored.

## Router — the activities

Read the companion named for your activity in full before acting. An activity not in the
table is not thereby ungoverned: read the closest row's file and say which row was used.

| id | enables | description |
|---|---|---|
| changing-the-planner-for-v3 | | Making the code changes for version 3 of the story planner from baselined hypotheses. The terminus: out of this skill's scope, owns no processes |
| baselining-a-hypothesis | changing-the-planner-for-v3 | Brian's dated judgment, in his words in the record, that a hypothesis's evidence picture is sufficient to act on |
| promoting-checked-candidates | baselining-a-hypothesis iterating-a-statement | Brian deciding the pending referee-diagnostic candidates he chooses, by hypothesis or by round, each after its cited source is read: promote verbatim or decline; one outcome line per candidate; one commit |
| iterating-a-statement | refereeing-a-candidate | Brian's rewording of a hypothesis on evidence: the statement edited, an iteration entry as the wording boundary, status recomputed, prior findings queued as iteration candidates |
| minting-a-hypothesis | reviewing-leads | Creating a hypothesis file on novelty, testability and independence against the current set, in any hitl session, Brian rewriting or approving the statement, provenance in the created entry |
| refereeing-a-candidate | promoting-checked-candidates | A blind agent given only the current statement and the candidate's finding writes a falsifier and classifies it diagnostic supporting, diagnostic challenging, or non-diagnostic |
| writing-candidates-from-verification | refereeing-a-candidate | A session writes one candidate per finding a round claims bears on a hypothesis: target, finding, source locator, proposer with hash; append-only, no falsifier |
| conducting-a-verification-round | writing-candidates-from-verification | One execution of a calibrated instrument over a corpus's items, on Brian's go: fan out through the runner, tally, write the artifact with per-item results and hashes |
| preparing-to-verify-a-corpus | conducting-a-verification-round refereeing-a-candidate | Building the measuring instrument with Brian: itemize the corpus, author the codebook against real items, calibrate it on a sample he scores blind, hash it |
| reviewing-leads | preparing-to-verify-a-corpus | Brian and a session over a leads artifact: drill the bins, challenge leads against the source, and write the questions Brian raises into the corpus's question list |
| exploring-a-corpus | reviewing-leads | Reading a corpus discovery-first with a question in view and no hypothesis targeted: a pathfinder in one session, or slice readers through the runner, joined and binned; output a leads artifact |
| preparing-to-explore-a-corpus | exploring-a-corpus | Scoping an exploration with Brian: the card's question and the corpus's question list, the scale, the reading protocol and read-manifest if slices, the plan approved, the protocol piloted |
| building-a-tool | preparing-to-explore-a-corpus preparing-to-verify-a-corpus revising-the-method | Code with tests that carries no judgment: ingests, readers, the runner, talliers, renders, the validator; CORPUS-STATUS updated when a corpus becomes readable |
| revising-the-method | preparing-to-explore-a-corpus preparing-to-verify-a-corpus | Changing how the buildout is run: the skill's files and tables rewritten, the validator passing, a write-once revision note recording what changed and why |

<!-- generated:level-1 -->
<!-- /generated -->

## Companions that are not activities

`artifacts.md` — the Artifacts table and every authored format (hypothesis file, index,
question entry, instance registry, candidate, leads and verification artifacts, arm key,
reading protocol, codebook, calibration record, run.md). `CORPUS-STATUS.md` — what
material exists and its state; a fact file, not a rule. `map.md` and `state.md` —
generated only. The `agent-runner` skill governs the runner as an instrument and is read in
full by any process that invokes it.

## Vocabulary

- **instance**: one run of one activity chain over one corpus, declared in the registry.
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

Revision 2 of this skill, built from 2026-09-04 in this folder beside the live
`v3-buildout` (revision 1, 2026-09-03) and swapped in one commit when the validator passes.
Why revision 2 exists and what it replaced is recorded once in
`docs/v3-framework/methodology-revision-2.md`; the rulings it rests on are appended as made
to `docs/v3-framework/methodology-revision-2-rulings.md`. Both are provenance; this skill is
the instruction. `docs/v3-framework/` holds the buildout's record: `hypotheses/`,
`questions/`, `methodology-revision-N.md` with its rulings log, the leads and verification
artifacts named `exploration-of-<corpus>` and `round-of-<corpus>-<n>`, the retired forward
plans of revision 1 (reference only), and `implementation-candidates.md` (codebase changes
gated on baselined hypotheses — they enter the ordinary feature process, never this
skill). `fanout/` holds what the runner takes in and puts out, one folder per work. There
is no plan: what to do next is read from the generated `state.md`, and the pick is Brian's.
Provenance informs and never prescribes.

## What this skill does not govern

Story content decisions; prose technique; planner features (a hypothesis supplies evidence,
CLAUDE.md, `wpf-conventions` and FEATURE-AUDIT supply governance); declaring conclusions.
Every finding is a hypothesis until Brian baselines it, and baselining is his.
