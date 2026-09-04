---
name: v3-buildout
description: Methodology for the v3 narrative design framework buildout, revision 1 (2026-09-03) — the work matrix (instrument / classifier / investigator / auditor / focused reader / slice reader / census / pathfinder), the strong-form evidence pipeline (candidates → referee → promotion), four work-unit types (exploratory and verification per corpus, synthesis over verified corpora, infrastructure) with verification debt, spec pools and codebooks, and the explicit-context rule for autonomous agents run by tools/StoryPlanner.AgentRunner. Load before any framework buildout work — hypothesis work, WU planning or execution, forward plans, consolidation, promotion sessions. Revision 1 replaced the original skill of this name on 2026-09-03 (docs/v3-framework/methodology-revision-1.md).
---

# V3 framework buildout — revision 1

This file is a **router**. It carries what every buildout session must never miss — the
epistemic framework, the work matrix, the constitutional rules — and sends each activity to
the companion file that holds its full protocol. The companion files are not loaded with
this one: **read the file named for your activity, in full, before acting.** Nothing here
is a summary of them.

Why revision 1 exists, and what it replaced, is recorded once in
`docs/v3-framework/methodology-revision-1.md`. That note is provenance; this skill is the
instruction.

## Epistemic framework (applied)

**Scope.** Two coupled but separable domains: the *narrative design framework* (what the
planner should track — tracks, cognitive modes, the mechanism/goal/technique model,
reader-experience moments, perception-gap delivery, variable focalization, voice registers,
and whatever else the evidence shows the planner should track) and the *codebase
architecture* (how it is represented — Type Object extensions, annotations, note
relationships, MCP instructions, among others). Framework hypotheses come first;
codebase implications follow. Nothing in the current framework vocabulary is settled by
appearing authoritatively in CLAUDE.md or a track definition — the buildout may revise any
of it.

**Every claim is a hypothesis with an evidence relationship.** Three states, in the
hypothesis file's frontmatter:

- `untested` — no verified evidence in the record.
- `evidenced` — verified evidence currently supports the statement; thin or thick, the
  record shows the weight; always revisable.
- `challenged` — at least one verified challenging entry is unresolved. One open challenge
  puts the hypothesis here regardless of how much support also exists.

Transitions are reversible except one: once verified evidence exists a hypothesis never
returns to `untested`. **Only verified evidence moves a hypothesis** — see the strong form
below. Baselining is a separate field: Brian's dated judgment that the evidence picture is
sufficient to act on. It is progress tracking, not epistemology — it adds no weight, removes
no challengeability, is not endorsement of truth, and resets to `false` automatically when a
challenging entry lands.
Only Brian baselines; a session may name candidates ("verified support, no open challenge —
review for baselining") and never sets the field.

**Recall is atmosphere; evidence is grounding.** A statement about the data from anyone —
Brian from memory, a prior session, a memory file, this skill, any document, anything
else — is a hypothesis about what the data says. Before acting on it: query the source,
compare, present the discrepancy to Brian, proceed on the grounded version once he has
seen it. Nothing is exempt,
including this file. Grounding connectors include, and are not limited to: the MCP server
(working plan, v1 archive, conversations, lineage, source texts), the local corpora named
in `CORPUS-STATUS.md` (the 112-story analyses, Brian's fiction, supplementary material, and
more), `codesessions.db` by sqlite3, and web search for historical parallels. CLAUDE.md
carries each source's semantics and caveats.

## The work matrix

Every piece of buildout work sits in one cell of a 3 × 3 matrix. The cell decides what
context the work gets, which model does it, how its output is verified, and where it runs.
**Cell names are the vocabulary; there are no tier numbers.**

| context scope ↓ / judgment → | **frozen predicate** — question and criteria fully specified | **method discretion** — fixed question, adaptive search | **salience discretion** — decides what matters |
|---|---|---|---|
| **item** — e.g. one note, passage, claim, candidate | **classifier** | **investigator** — also the shape of the planner's eventual per-claim evidence mode: *"what evidence bears on this claim, go find it"* (a question posed, in the verify role) | *(the line the planner must never cross on story content — deciding what is interesting about an item is Brian's; see CLAUDE.md, Two AI roles)* |
| **slice** — e.g. one story, arc, analysis section | **auditor** | **focused reader** | **slice reader** |
| **corpus** — everything, one context | **census** (usually a tool) | *(rare)* | **pathfinder** |
| *(no LLM judgment at runtime)* | **instrument** — all judgment spent at design time; runtime is computation (e.g. voice attribution, `WorldDateRange`) | — | — |

The cell names are the current set; a revision may add cells or rename them, and a piece
of work that fits none is a finding about the matrix, to be recorded, not forced in.

Orthogonal to the cell: **role** — *generate* (produce records, labels, findings) or
*verify* (check something already produced: the referee, the auditor).

What the cell decides:

- **Context.** Salience-discretion cells get the full instruction stack: they are HITL
  sessions in the repo (CLAUDE.md, skills, memory, MCP). Frozen-predicate and
  method-discretion cells get **a protocol and an item, nothing else** — see the
  explicit-context rule below.
- **Model, as revisable doctrine (2026-09-03), not a finding:** Fable for HITL design and
  adjudication (pathfinder sessions that decide things, promotion, forward plans, and the
  like); Opus for slice readers, focused readers and investigators; Sonnet for
  classifiers, auditors and referees. The allocation is a default per cell, not a rule per
  task. A model-comparison experiment varies exactly one factor; a WU whose *method* is
  under study holds the model constant across its arms.
- **Verification owed.** The more discretion, the more checking downstream: an
  instrument's output is trusted after calibration; a classifier's after calibration of its
  codebook; a slice reader's by a second blind reader or adjudication; a pathfinder's output
  is *leads only* — an index of where to look, never a finding.
- **Where it runs.** HITL cells in the repo. Everything else through
  `tools/StoryPlanner.AgentRunner` from the fanout folder — load the `agent-runner` skill
  before writing a job file; it governs the instrument, this skill governs when a cell
  calls for it.

## Constitutional rules

These hold in every session type. A companion file elaborates; none overrides.

1. **Only Brian baselines.** Only Brian decides story content, taxonomy, what is
   interesting, whether a flagged note is resolved, what is written to a `.storyplan`, and
   anything else CLAUDE.md reserves to him — that list is not exhaustive either.
2. **Strong form: only verification passes produce evidence.** Exploratory work produces
   findings (in the WU artifact) and *questions* (in a spec pool). Evidence enters a
   hypothesis record only as a candidate written by a verification pass, checked by a
   fresh-context referee, and promoted by Brian in a reviewed commit. Nothing else writes
   to `docs/v3-framework/hypotheses/`. (`evidence-pipeline.md`)
3. **Verification debt.** A corpus whose exploratory pass has run but whose verification
   pass has not is *unverified*: no synthesis, comparison or adjudication consumes its
   findings. Questions flow freely between corpora; findings wait. (`wu-execution.md`)
4. **A codebook is an instrument.** Written in a HITL session, calibrated against Brian's
   verdicts before use, versioned by hash; every verification result cites the hash; a
   revision re-runs, never re-labels. A pass that finds its codebook wanting writes that to
   the spec pool and stops. (`agent-runner` skill § Layout)
5. **Explicit context for autonomous agents.** Any agent in a frozen-predicate or
   method-discretion cell — classifier, investigator, auditor, referee, or any cell added
   later — runs from `RiderProjects\StoryPlanner-fanout` via the runner: protocol file +
   item + output path, inlined and hashed; exact toolset; no CLAUDE.md, no skills, no memory,
   no MCP unless the job opts in; no transcript persisted. Never through the Agent tool of
   a repo session, never from a repo cwd.
6. **Claude never creates a hypothesis file autonomously.** Proposals cite the specific
   evidence, Brian rewrites or approves the statement, provenance is recorded.
7. **The story-content boundary.** The framework studies technique, architecture and
   methodology. Thematic content comparisons and what a subject "needs next" are two
   examples of what lies outside it, not the whole list; redirect to the framework-relevant
   question or say it is out of scope.
8. **Never derive from recall.** Brian's recall about his own practice is the hypothesis
   under test, not evidence for it; it goes to a spec pool as a question.

## Session routing — read the named file in full before acting

| Activity | Read first | Then |
|---|---|---|
| Hypothesis iteration (e.g. rewording, re-tagging, statement sweep) | `hypothesis-records.md` | `evidence-pipeline.md` § Iteration |
| Promotion session (turning referee-checked candidates into record entries) | `evidence-pipeline.md` | `hypothesis-records.md` |
| Exploratory WU (e.g. discovery reading, pathfinding, slice reading) | `wu-execution.md` | the active forward plan's WU card; `CORPUS-STATUS.md` |
| Verification WU (classifier / investigator / auditor fanout under a codebook, or any other frozen-predicate work) | `wu-execution.md` | `evidence-pipeline.md`; the corpus's spec pool; the `agent-runner` skill |
| Synthesis WU (comparison, retrospective, adjudication, evaluation, connection, and the like) | `wu-execution.md` | the debt status of every corpus it names; the verified artifacts |
| Infrastructure WU (an instrument, ingest, codebook or calibration to build) | `wu-execution.md` | the `testing` skill; the `agent-runner` skill for a codebook or protocol |
| Post-WU review with Brian | `wu-execution.md` § Post-WU review | — |
| Forward plan creation or revision | `forward-plans.md` | the full hypothesis index and every hypothesis file |
| Consolidation | `consolidation.md` | every hypothesis file; the active plan |
| Ad hoc conversation about the framework | this file; the hypothesis index, for orientation | hypothesis files as touched |

The routing entries are instructions, not cross-references: a session that deposits without
having read `evidence-pipeline.md` in full is operating outside the method. An activity not
in the table is not thereby ungoverned — read the closest row's files and say which row
was used.

## Context documents

Two files travel with this skill and are facts, not rules: `VERSION-HISTORY.md` (dated
project timeline — no interpretive claims) and `CORPUS-STATUS.md` (what material exists and
its state). Interpretive claims about either are hypotheses and live in hypothesis files.

## Provenance

`docs/v3-framework/` holds the buildout's record, including: `hypotheses/` (the working
instrument), `forward-plan-N.md` (active = highest number; retired plans carry a header
stamp), `consolidation-N.md`, `methodology-revision-N.md`, `WU<plan>.<unit>-…` artifacts
(write-once), `spec-pools/`, `implementation-candidates.md` (codebase changes
gated on baselined hypotheses — they enter the ordinary feature process, never this skill),
and whatever later revisions add. Provenance informs and never prescribes: a retired plan is
reference, a consolidation report is a record, a WU artifact is evidence, a revision note is
history.

## What this skill does not govern

Among other things: story content decisions; prose technique (the planner specifies goals
and mechanisms, never how to write); planner features (a
hypothesis supplies evidence, CLAUDE.md / `wpf-conventions` / FEATURE-AUDIT supply
governance — check FEATURE-AUDIT before proposing anything that touches the feature set);
declaring conclusions. Every finding is a hypothesis until Brian baselines it, and
baselining is his.
