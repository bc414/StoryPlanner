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
and whatever later revisions add. `fanout/` holds what the agent runner takes in and puts
out, one folder per work — the referee's codebook and runs, a verification WU's codebooks,
items, jobs, results and candidates (`agent-runner` skill); a document in `docs/` cites a
run by ledger row. Provenance informs and never prescribes: a retired plan is reference, a
consolidation report is a record, a WU artifact is evidence, a revision note is history.

## What this skill does not govern

Among other things: story content decisions; prose technique (the planner specifies goals
and mechanisms, never how to write); planner features (a
hypothesis supplies evidence, CLAUDE.md / `wpf-conventions` / FEATURE-AUDIT supply
governance — check FEATURE-AUDIT before proposing anything that touches the feature set);
declaring conclusions. Every finding is a hypothesis until Brian baselines it, and
baselining is his.

# Hypothesis records

Read in full before touching any file in `docs/v3-framework/hypotheses/`. The pipeline that
decides *what may be written* is `evidence-pipeline.md`; this file is the *shape* of the
files and the ceremony around changing them.

## Files and the index

`docs/v3-framework/hypotheses/NNN-slug.md` — `NNN` a zero-padded three-digit id, stable,
unique across the whole set, never reused after supersession; `slug` descriptive kebab-case. `INDEX.md` in the same
directory is a routing table — two columns (ID, slug as link), no summaries, in id order so
a top-to-bottom scan is comprehension order. It changes only when a file is minted or
superseded. **It carries no status or baselined column** — those lived there until
2026-09-04 and were removed as a hand-kept mirror of frontmatter that nothing checked
(the same failure as forward-plan-2's coverage table). Status is read from the files:

```
grep -H '^status:' docs/v3-framework/hypotheses/0*.md          # every status
grep -l '^status: challenged' docs/v3-framework/hypotheses/0*.md # one state
grep -h '^status:' docs/v3-framework/hypotheses/0*.md | sort | uniq -c   # the counts
```

```markdown
---
id: 17
status: evidenced
baselined: false
created: 2026-09-01
---

## Hypothesis

[1–3 sentences. A testable prediction readable in isolation.]

## Record

[Chronological, oldest first, newest appended.]
```

`status` ∈ `untested | evidenced | challenged`; `baselined` is `false` or an ISO date;
`created` never changes. No other frontmatter fields — "tested by", "related", and the
like are staleness targets. Which WUs target a hypothesis lives in the forward plan;
connections between hypotheses live in prose.

## The statement

What the hypothesis predicts, and only that. If it explains *why* the prediction exists,
that is founding reasoning and belongs in the `created` entry. No provenance, no
implications, no testing method, no confirm/refute conditions.

## Record entries

Four kinds, each one citable unit; a two-finding entry is two entries. Full ISO timestamps.

**`created`** — always first. Why the hypothesis exists: the observation, Brian's
assertion, the motivation, in Claude's voice with Brian's assertions as the content. It
does not name the document it was extracted from (the consolidation report holds that
chain) and does not contain testing method.

```
- created | 2026-09-01T10:00: <reasoning>
```

**`evidence`** — written only by a promotion session, only from a referee-checked candidate
(`evidence-pipeline.md`). The entry carries, in order: the source WU and candidate id, the
codebook version when a classifier or auditor produced the finding, the alignment tag, the
finding, and the discrimination clause verbatim from the candidate.

```
- evidence | 2026-09-14T15:20 | (WU2.3 C-014; codebook dt-classes@3f9a1c) [supporting]:
  <finding, with counts, ids, excerpts as the finding requires>
  Would differ if false: <the observable the corpus would have shown instead>
```

The clause is not decoration — it is the referee's verdict written where it can be
re-checked. An entry without one is malformed. Alignment is `supporting` or
`challenging`; there is no third tag. Evidence is grounded in corpora or verifiable
sources — which excludes, among other things, Brian's recall about his own practice, a
restatement of the statement, and an intermediate analysis's classification that no one
read back to the source.

**`iteration`** — a change to the statement, with the reason and, when evidence prompted
it, the citations. Record which evidence entries were re-verified as a consequence (see
`evidence-pipeline.md` § Iteration — alignment tags are never edited in place).

```
- iteration | 2026-09-16T09:15: Narrowed from "…" to "…" after C-014/C-015.
  Entries 2026-09-14T15:20 and 2026-09-14T15:24 returned to candidate status for re-referee;
  re-promoted 2026-09-16T11:40 as [supporting] / [challenging].
```

**`baselined`** — Brian's judgment and rationale. Written only by Brian or at his explicit
direction in his words.

```
- baselined | 2026-09-20T16:00: <rationale>
```

Grep: `^- created`, `^- evidence`, `^- iteration`, `^- baselined`, `\[challenging\]`.

## What never enters a record

Including, and not limited to:

- Brian's recall about his practice (a question for a spec pool).
- Brian's story-design observations (story content).
- Observations that do not change the statement (a spec-pool question, or nothing).
- Methodological pointers ("WU2.4 should check this" — the forward plan or spec pool).
- Findings from an exploratory pass — however relevant. They live in the WU artifact and,
  as questions, in the spec pool. The strong form has no exception for "obviously true".

Test: removing the entry would leave the record incomplete (a statement change, verified
evidence, a baseline event missing) — it belongs. It would only lose a pointer to future
work — it does not.

## Ceremony scaling

- **Minor** (wording tightened, no conceptual change): edit the statement; one-line
  iteration entry. Evidence entries whose clause still discriminates stand — say so.
- **Significant** (scope changed; evidence prompted a rethink): full iteration entry with
  citations; every evidence entry returns to candidate status for re-referee
  (`evidence-pipeline.md`); status re-derived from what is re-promoted; the `created`
  entry amended only if the reframing makes it misleading.
- **Structural** (split, merge, supersede): new file(s); final iteration entry in the old
  file naming what replaced it; the old file's status becomes `challenged` (supersession is
  a pattern of challenge, not a fourth state); index updated. Consolidation territory.

## Challenging a hypothesis

A challenge is evidence: a specific counterexample with ids — a note, a block, a lineage
id, a passage, a corpus finding, or any other citable locus — that goes through the
pipeline like any other candidate and
lands as a `[challenging]` entry. "This doesn't feel right" is not a challenge; it is a
spec-pool question at most. Brian's challenges carry his analytical voice and are
engaged by grounding, never elaborated away.

## Creating a hypothesis

The same file protocol applies whether one hypothesis is created in conversation or forty
in a consolidation. Three criteria, all required: **novelty** (it is not evidence for an existing hypothesis),
**testability** (evidence could confirm or refute it), **independence** (it is not a
refinement — refinements are iteration entries). Brian's explicit statements always get
the offer. Claude's analysis may surface a candidate only when all three hold, and the
proposal cites the specific evidence or Brian's statement that prompted it — not a
synthesis. Brian reviews the *statement*, rewrites it in his words or approves; the
`created` entry records the provenance ("originated from Claude's reading of …; Brian
endorsed on …"). During a WU's primary work, hold proposals for the wrap-up; in every other
session type, propose when the three criteria hold.

The trap this guards (the v1 pattern): Claude proposes → Brian nods → the hypothesis
enters in Claude's framing → later sessions build on that framing → Brian's own thinking is
channelled. Provenance lets a future session tell Brian-originated from Claude-originated.

# The evidence pipeline (strong form)

Read in full before writing anything that could end up in a hypothesis record — a
candidate, a referee verdict, a promoted entry, an iteration. This is the one file that
must be applied whole.

## Goal

Four things at once: every relevant finding is preserved and findable near the hypothesis
it bears on; discriminating evidence is separated from context by *status*, never by
exclusion; the separation is not judged by the party that produced the finding; and the
human-facing record is never touched by unreviewed machine judgment.

Vocabulary the pipeline depends on: a **finding** is what a pass observed; a
**candidate** is a finding a verification pass claims bears on a hypothesis; **evidence**
is a promoted candidate; a **question** is a spec-pool entry. Treating a finding as
evidence is the conflation the pipeline exists to make structurally impossible (the
episode that prompted it is in `docs/v3-framework/methodology-revision-1.md`).

## Where each kind of write goes

| What | Who writes it | Where |
|---|---|---|
| A finding of an exploratory pass | the pass | the WU artifact |
| A question raised about a corpus | any pass, any post-review | `docs/v3-framework/spec-pools/<corpus>.md` |
| A candidate (a finding that a verification pass claims bears on a hypothesis) | the verification pass | `fanout/WU<n>.<m>-…/candidates.md` |
| The referee's verdict on a candidate | the referee (autonomous, fresh context) | appended to the same candidate |
| A promoted evidence entry | a promotion session, Brian reviewing the commit | `docs/v3-framework/hypotheses/NNN-slug.md` |
| A statement change | a HITL session, batched at the end of a review | the hypothesis file (iteration entry) |

Nothing else writes to `hypotheses/` — not a WU session at wrap-up, not a subagent, not a
post-review in the flow of conversation, not any other path however reasonable it looks in
the moment.

## The candidates file

One per verification WU, in the WU's work folder under `fanout/` (the table above; the
`docs/` artifact cites it), **append-only**: a finding is
never edited after it is written; the referee and the promotion session append lines.

```markdown
## C-014
- target: 031
- status: candidate            # candidate | diagnostic | non-diagnostic | promoted | declined | held
- finding: <one citable unit: what was observed, with ids, counts, passages>
- source: <what was read — WU artifact section, corpus locus, note ids, story + chapter>
- proposed-by: <arm or job id> / <model> / <ISO time> / codebook <name>@<hash> / harness <version>
```

Referee append (one per target; a candidate bearing on two hypotheses is two candidates):

```markdown
- clause: Would differ if false: <the observable the excerpt would have shown instead — written fresh>
- referee: <job id> / <model> / <ISO time> / codebook referee@<hash> / verdict diagnostic [supporting|challenging] | non-diagnostic — <one-line reason>
```

Promotion append:

```markdown
- disposition: promoted <ISO time> as evidence entry <timestamp in NNN-slug.md> | declined — <reason> | held — <what it waits on>
```

Status is derived from the last append and is the only field that changes. `held` is for
a candidate that waits on something named (e.g. a codebook revision, a source re-read),
never a soft landing for "unsure".

## The referee

A **classifier in the verify role**, and therefore an instrument: its procedure is a
codebook — `fanout/referee/codebook.md` — not this skill, which the referee
never sees. The codebook is what the runner hands it (protocol file + the candidate + the
source excerpt), and every verdict line cites `codebook referee@<hash>`. Like any
codebook it is calibrated on Brian's verdicts before its first batch and re-hashed on
every ruling; a referee run under an uncalibrated or superseded hash is re-run, not
trusted.

What the skill fixes, and the codebook implements:

- **Inputs are exactly three** — the current statement (no status, no record, no other
  candidates), the candidate's finding and source lines, and the cited source excerpt
  fetched by the promotion session into the job's inputs. The referee never searches, and
  never sees a clause anyone else wrote for the candidate: the fresh attempt is the
  mechanism of its independence, and comparing the two afterwards is a measurement.
- **The task is to attempt the clause blind** — "if the statement were false, this source
  would have shown ___ instead" — and the three classes are `diagnostic [supporting]`,
  `diagnostic [challenging]`, `non-diagnostic`, with the tag following mechanically from
  which side of the named observable the excerpt shows.
- **Tuned to over-flag** (a false non-diagnostic costs one adjudication; a false
  diagnostic costs the record), **report-only** (the two append lines and nothing else),
  and **one target per candidate**.
- **A vacuous clause is non-diagnostic by definition, whoever wrote it** — the clause
  names what the excerpt would contain, never what the claim would be. The codebook's
  decision rules and worked examples are the operational form of this sentence.

Run through `tools/StoryPlanner.AgentRunner` from the fanout folder, Sonnet by default,
`mcp: false`, tools `Read` and `Write` only.

## Promotion

A HITL session, Fable, with the candidates file, the referee verdicts, and the hypothesis
files open. For each `diagnostic` candidate: read the cited source (not the finding — the
source) before promoting; write the evidence entry with finding and clause **verbatim**
from the candidate; append the disposition. Brian adjudicates disagreements with the
referee and may decline a diagnostic candidate (reason recorded) or promote nothing.
`non-diagnostic` candidates stay where they are, visibly, as context — they are not
promoted and not deleted. Then recompute each touched hypothesis's status from its
entries in the file's frontmatter (the index carries no status — `hypothesis-records.md`
§ Files and the index) and commit **once**: the diff is the review surface, and the
commit message names the WU and the candidate ids.

Report to Brian, after the commit — at least: candidates per target; diagnostic /
non-diagnostic / held; promoted supporting / challenging; declined with reasons; referee
disagreements and how they went; anything else the session noticed about the pipeline's
own behaviour. He decides whether the distribution warrants a recheck.

## Iteration → re-referee

When a statement changes (an iteration entry), every evidence entry under the old wording
is a candidate again: its finding and source are copied into a new candidates file for the
iteration (`fanout/referee/iterations/NNN-<date>/candidates.md`), the referee runs against the
*new* statement, and the promotion session re-promotes what survives. Alignment tags are
never edited in place, and the old entries are never deleted — the iteration entry records
which entries were re-verified and their new tags; the superseded entries stay in the
record marked `(superseded by re-referee <date>)` at the end of their line. There is no
state for "not yet re-assessed": re-assessment is the iteration's own step.

## Verification debt and codebook versioning

A corpus's exploratory pass opens a debt paid only by its verification pass. Until then
its findings are leads: no synthesis, comparison, adjudication or forward-plan rationale
cites them as evidence, and its hypotheses stay `untested` on its account. Questions flow
freely — an exploratory pass on one corpus may add to any corpus's spec pool.

Every verification result cites the codebook hash it was produced under. A codebook
revision (new hash) re-runs the affected classifier or auditor jobs; the old results are
kept, marked superseded, never re-labelled by hand. Re-runs are cheap because the work is
classifier-tier — this is what makes the strong form affordable.

## Entries this pipeline did not produce

An evidence entry with no referee line and no codebook hash was not produced by this
pipeline — whatever method wrote it. Such an entry is **unverified**: it and any status
computed from it are leads, not evidence, until the entry has been re-verified as a
candidate through the referee and a promotion session. Re-verification is verification
work and belongs on a forward-plan card, never in this file.

# Work units

Read in full before scoping, planning, executing or reviewing a WU. The evidence rules a
WU must obey are in `evidence-pipeline.md`; this file is what a WU *is* and how it runs.

## Four types

A WU is one of four types, and the card's `Type` field says which. The types are peers;
what distinguishes them is what they read, what they may write, and what they must never
write.

| Type | Reads | Writes | Never writes | Cells and who runs them |
|---|---|---|---|---|
| **Exploratory** | A raw corpus, discovery-first, with no hypothesis in view | A **WU artifact** organised by what was observed (never by hypothesis id); **questions** into spec pools — each a testable question about a corpus, with a candidate predicate where one suggests itself | Candidates; evidence. Its findings are leads. | Pathfinder and slice-reader cells — a HITL session, or arms run through the runner under an explicit reading protocol |
| **Verification** | One corpus's spec pool, and the **source itself** (e.g. the story text, the archive notes, the lineage turn) — never an intermediate analysis alone | A WU artifact (per-item results with codebook hashes, counts, tables); a **candidates file**; referee verdicts; the promotion commit; questions into spec pools (e.g. when a codebook is found wanting) | A codebook revision (that is a HITL task, triggered by its question); evidence by any route but promotion | Classifier, investigator and auditor cells (or any frozen-predicate / method-discretion cell) as runner jobs under calibrated codebooks; the referee likewise; promotion is a HITL session |
| **Synthesis** | The **verified** artifacts of the corpora it names — verification pass results and promoted evidence. It may read exploratory artifacts for the origin of a question, and cites them only as leads | A WU artifact (the comparison, retrospective, adjudication, evaluation, connection, or the like); questions into spec pools — a synthesis's own insight reaches a record only by becoming a question that a verification round answers | Candidates; evidence; anything into `hypotheses/` | A salience-discretion cell over verified artifacts (a pathfinder whose corpus is the buildout's own outputs) — a HITL session, Fable |
| **Infrastructure** | Whatever the thing being built needs | An instrument (tool, ingest, render), a codebook and its calibration record, tests, a `CORPUS-STATUS.md` update; questions into spec pools if the build raises any | Candidates; evidence; prose that is Brian's to author (display questions, definitions — see CLAUDE.md on seeders) | Instrument-authoring: HITL plus code, tested per the `testing` skill |

Every write that could reach a hypothesis record goes through `evidence-pipeline.md`;
the table above says which types can produce candidates (only verification) and which cannot.

## The corpus pair, and what is ordered

Every corpus in the buildout — the 112-story analysis corpus, Brian's own fiction, the v1
archive, the v2 working plan, the lineage layers, the conversations, the code sessions, and
any corpus added later — is worked as an **exploratory/verification pair**, and the one hard ordering rule is
**exploratory pass on c → verification pass on c → any consumer of c**: a synthesis that names corpus c waits on a verification round
of c covering the questions it relies on. Questions flow freely in every direction — an
exploratory pass, a verification pass, a synthesis or an infrastructure WU may write into any corpus's spec
pool. Findings wait. Nothing else is ordered: exploratory passes are unordered among
themselves (Brian's choice which runs first); verification passes are triggered and run
in rounds — a round for corpus c is due when its pool holds questions with a calibrated
codebook ready; infrastructure runs when something needs it (`forward-plans.md`
§ Ordering is structural).

## Design rules for all types

- **Scale is a cell, not a token count.** Decide which matrix cell the work is in; that
  decides context, model and verification. A corpus that does not fit one context is read
  by slice readers with a shared protocol, never by one reader with compaction.
- **Arms and blinding.** When a pass runs more than one reader over the same material —
  two conditions, two models, a factorial of both — each arm gets an identical explicit
  context, arms are blind to each other, record files are labelled neutrally (`arm-A`,
  `arm-B`) and the mapping to condition and model is kept in the WU's `read-manifest.md`
  and not opened until the adjudication has binned the disagreements. A pass may run one
  arm where compute forbids two, but then its artifact names what was not measured (no
  stability figure, no disagreement count) rather than letting a single arm pass as a design.
- **Binning.** Disagreements between arms are sorted into named bins *before* any is
  investigated (expected-structural / missed-by-one / unsupported-by-source, extended as
  the design requires); the counts per bin are findings; only the interesting bins are
  drilled, with Brian adjudicating the drills. Binning is what keeps adjudication from
  becoming "discuss every mismatch".
- **One factor at a time.** A WU whose *method* is under study (e.g. context length,
  reading condition) holds the model constant across arms; a model comparison holds everything
  else constant. Mixed designs are factorial by intent and say so, or they are
  uninterpretable.
- **Codebooks are not written by the pass that applies them.** A verification pass finds its
  codebook wanting → spec-pool entry → stop. Calibration (sample scored, Brian's verdicts
  beside the model's, rulings recorded) precedes any batch.
- **Every autonomous job is a runner job.** Job file in the WU's run folder under
  `fanout/` (`fanout/WU<n>.<m>-<slug>/…/jobs.json`), launched from the external fanout
  folder, items, results, attempts and ledger written beside it, the ledger's prompt and
  codebook hashes cited in the artifact's method section (`agent-runner` skill: one item
  per job, enumeration by an instrument, a checked output, a pilot before the batch; the
  lifecycle in order is `fanout/PROTOCOL.md`, served by the host at `/protocol`). A pass that needs the MCP server sets `mcp: true` on that
  job alone. The Agent tool exists only inside a HITL session and is not a runner job: it
  inherits the instruction stack and its transcript enters the archive, so it serves only
  salience-discretion help to that session in ones and twos — never a batch, never an
  arm, never a cell that calls for explicit context. The Workflow tool is not used
  (`agent-runner` skill § Two mechanisms).

## The four phases

Every WU of every type runs the same four phases in one HITL session (Fable): scope
reconciliation, plan mode, execution, post-WU review. Phases 2 and 4 are the same for all
types with the per-type notes given; phases 1 and 3 differ by type, as the tables say.

**1. Scope reconciliation** (auto mode). Read the active forward plan's card and bring its
metadata (hypotheses, scale, type, corpus) in line with the work the WU now contains —
clerical, not judgment. Per type:

| Type | What is read to reconcile | What is reconciled |
|---|---|---|
| Exploratory | The card; the corpus's `CORPUS-STATUS.md` entry; the reading protocol; the arm design | The protocol and arms are confirmed; the hypothesis list on the card is informational only (an exploratory pass targets no hypothesis — it targets a corpus) |
| Verification | The card; the corpus's spec pool; the named codebooks and their calibration records | The pool's open questions *are* the scope; the card's hypothesis list is recomputed from what those questions bear on; codebooks without a calibration record are flagged as the first task |
| Synthesis | The card; the debt status of every corpus it names (exploratory pass done? which verification rounds? which questions answered?) | The synthesis proceeds only over corpora whose relevant questions are verified; anything it wanted from an unverified corpus is written as a question and dropped from scope |
| Infrastructure | The card; the thing to build and what consumes it | Preconditions and acceptance (tests, calibration) confirmed; nothing hypothesis-related to reconcile |

**2. Plan mode** (all types). Read or size the sources (`CORPUS-STATUS.md` for corpus
work). Collect every open question — ambiguities in the card, decisions that are Brian's,
assumptions to confirm, and anything else unresolved — and ask them all, batched at four
per call, before writing a line of plan. Then write the plan, covering at least: what is
read in what order, the arms and their explicit contexts (exploratory), the codebooks and their
hashes and the job files (verification), the verified inputs and their debt status (synthesis), the
acceptance criteria (infrastructure), what the artifact covers, the binning scheme where
arms exist, and what the WU does **not** do. Exit plan mode; Brian approves.

**3. Execution.** Per type:

| Type | Primary work | Wrap-up sweep (full hypothesis index, for anything touched outside the targets) |
|---|---|---|
| Exploratory | Run the arms; bin disagreements if there is more than one; write the artifact; append spec-pool questions; mark the card `complete` | More spec-pool questions — never candidates |
| Verification | Run the jobs; write the artifact; write the candidates file; run the referee; hold the promotion session; commit once; report the counts (`evidence-pipeline.md` § Promotion) | More candidates through the same referee — never a side door |
| Synthesis | Read the verified artifacts; write the synthesis artifact; append spec-pool questions for every insight that bears on a hypothesis; mark `complete` | Spec-pool questions — a synthesis never writes to `hypotheses/` |
| Infrastructure | Build; test; calibrate if it is a codebook; update `CORPUS-STATUS.md` if it is a corpus; mark `complete` | Spec-pool questions if the build raised any |

New-hypothesis proposals are held for the wrap-up in every type and offered only when
novelty, testability and independence all hold.

**4. Post-WU review** with Brian, same session, two interleaving modes for all types, with
one per-type difference in what "verify against the source" means:

- *Challenge* — Brian questions a finding. Verify against the source, never the
  intermediate analysis: for exploratory and verification that is the corpus itself (the story text, the notes,
  the turn); for a synthesis it is the verified artifact *and* the source that artifact
  cites; for infrastructure it is the test or calibration record. Correct the artifact if
  the finding does not hold. For a verification pass, a corrected finding re-enters as a new candidate
  through the referee.
- *Enrichment* — Brian connects a finding to his practice or recall. That is a question:
  write it to the relevant corpus's spec pool with its provenance ("Brian's recall,
  2026-…: does the v1 archive show X?"). Recall never enters a record.

Statement changes are batched at the end of the review, then handled as iterations
(`evidence-pipeline.md` § Iteration). Story-content drift is redirected to the framework
question or named as out of scope.

## WU artifacts

`docs/v3-framework/WU<plan>.<unit>-<slug>.md` or a directory of that name when the WU
has several files (records, renders, manifests). Runner jobs, items, results, attempts, the
ledger and the candidates file live in the WU's folder under `fanout/`, cited from here.
Write-once evidence: later
sessions cite them, never edit them; a correction is an appended dated section. An
artifact's method section names at least the protocol and codebook hashes, the arms, the
harness version and models, and the read-manifest — whatever it takes that the pass could
be re-run.

Counts in an artifact cite the instrument that produced them; classifications that bear on
a hypothesis cite the source locus and were read there.

# Forward plans

Read in full before creating, revising or retiring a forward plan. A forward plan is a
**snapshot experimental agenda**: born from the hypothesis landscape as it stands,
guiding work for a period, retired when consolidation or findings change the landscape.

## Numbering and lifecycle

`docs/v3-framework/forward-plan-N.md`; the active plan is the highest number. A
consolidation report without a matching next-numbered plan means a plan is pending.
Retirement is a header stamp on the old file — date, successor, reason — and the retired
plan becomes reference: its unexecuted WUs are retired proposals, not evidence.

Two creation triggers. **After consolidation** (mandatory): the hypothesis set changed
shape and every reference is stale. **After a priority reassessment** (lighter): the set is
unchanged but findings or a methodology revision changed what should be run — the same
hypotheses, a rewritten agenda.

## Structure of the agenda

The plan is comprehensive — every hypothesis is targeted by at least one WU — and it is a
**catalog found by id, not a schedule**. Three sections of work units:

1. **Corpus pairs.** For each corpus: an exploratory WU and a verification WU. The exploratory card
   names the reading protocol, the arms, and the corpus; the verification card names the spec pool it
   consumes, the codebooks it needs (existing hash or "to be written and calibrated"), and
   the hypotheses its pool's questions bear on. A verification card's hypothesis list grows as its
   pool grows; scope reconciliation keeps it current.
2. **Synthesis WUs.** Comparison, retrospective, adjudication, evaluation, connection —
   each naming the corpora it consumes, and therefore the verification passes it waits on.
3. **Infrastructure WUs** where the plan needs one: e.g. an instrument to build, a corpus
   to ingest, a codebook family to calibrate.

A revision may add a fourth kind; the card's `Type` field is where it would show.

Each card:

```markdown
### WU N.M: <title>

**Type:** exploratory | verification | synthesis | infrastructure
**Corpus:** <one corpus for exploratory and verification; the consumed corpora for synthesis>
**Question:** <what it asks>
**Hypotheses:** <ids — for a verification card, "per spec pool <corpus>.md" and no id list>
**Evidence sources:** <what is read or run>
**Codebooks:** <names and hashes, or "to calibrate"> (verification only)
**Scope:** <what it does and does not do>
**Scale:** <matrix cell(s); arms; approximate job count>
**Preconditions:** <tooling or Brian-action blockers only — never WU dependencies>
**Status:** proposed | scoped | in-progress | complete
```

Hypothesis references live here and in spec pools, never in hypothesis files. WU numbers
are `<plan>.<unit>`: the major number is the plan's, the minor counts from 1 within it.
Cards are listed in id order. The hypothesis index's tiers (A–J) are comprehension order
for reading the index, never execution order or scoping boundaries — a corpus pair informs
hypotheses across tiers. **A hypothesis-to-corpus edge is authored in exactly one place: a
pool entry's `bears-on` line.** A plan does not copy those ids — not onto verification
cards, not into a per-hypothesis coverage table. There is no generator, so any copy is a
second source that drifts the moment a pool grows (forward-plan-2's table was 13 rows
behind the pools on the day it was written, 2026-09-04, and was removed). A plan may
state the derivation (which pool feeds which card) and the grep that answers a row. The
one legitimate snapshot is a verification round's own run record under `fanout/`, which
freezes the entries that round answered.

## Ordering is structural, not derived

There is no global execution sequence to derive, and no ordering audit. Order follows from
the card types:

- **Exploratory passes consume no other WU** — they read a raw corpus — and are unordered
  among themselves. Which exploratory pass runs first is Brian's choice; the corpus he is working
  in is a legitimate reason (that is judgment, not convenience). Three advisory heuristics
  only, never rules: infrastructure hypotheses early (a claim about the experimental
  infrastructure itself — e.g. a source's existence, a contamination, a separability
  assumption — changes how later work is designed or read); passes whose questions
  would feed many pools early; and foundation before application (a pass that bears on
  the premises other hypotheses rest on before one that bears on the claims built on
  them — not blocking, but findings on an untested foundation are harder to read).
- **Verification passes are triggered, and run in rounds.** A verification round on c is due when its spec pool
  holds questions with a calibrated codebook; a later round is due when the pool has grown
  enough to be worth a batch. A round's card records which pool entries it answered.
- **Synthesis WUs wait on verification debt.** A synthesis runs only when every corpus it
  consumes has a verification round covering the questions it relies on. This is the one
  hard edge in the plan, and it is readable off the cards' `Type` and `Corpus` fields —
  nothing is judged.

Never ordering inputs: readiness, convenience, throughput, estimated duration.
Preconditions gate timing in place, never position (a skill to build or an ingest to run
is the first task *inside* the WU that needs it).

The plan's execution section is a **status board, not a sequence**: per corpus, exploratory pass done or
not, verification rounds run, open pool questions; per synthesis, which debts still block it. It
summarises the cards' `Status` fields in the same file and is updated by hand in the same commit
as the card it summarises; it points at each pool rather than counting its questions.

Hypothesis status is advice for choosing among independent exploratory passes: `challenged` first
(the pool question that would resolve it), `untested` next, `evidenced` for stress-testing
from a new angle, `baselined` only if a challenge appears.

## Writing the plan

Start from the landscape: read the full index and every hypothesis file — and the
consolidation report, when one just happened — treating any
hypothesis whose entries lack referee lines as unverified (`evidence-pipeline.md` § Entries
this pipeline did not produce).
For each hypothesis or cluster ask what verified evidence would move it, which corpus
could supply it, and whether the question already sits in a spec pool. Group by corpus.
Prior plans and syntheses are reference for what was tried, not templates; a plan that
reads as a renumbering has not engaged with the landscape.

The rationale section is written once and can be long — at least: what the last
consolidation, revision or reassessment showed, what the landscape looks like, why this
shape, and whatever else a later session would need to understand the reasoning. The plan
is expected to be a long document — enough detail per card for a plan-mode session to
scope execution, and not more.

# Consolidation

Read in full before restructuring the hypothesis set. Consolidation is the maintenance
mechanism for the hypothesis graph — there are no formal cross-references to maintain, so
this is where connections, duplicates and clusters are noticed and acted on.

## Trigger and scope

On Brian's demand — for example when the set feels tangled, too many are `challenged`,
duplicates have been noticed, or a finding or a methodology revision restructured the
landscape; the trigger is his judgment, not a checklist. Scope is always
the full set — the index, every hypothesis file, the active forward plan, and every spec
pool (questions whose hypothesis was merged or split must be re-pointed).

## What it does

- Merges (one claim in two wordings), splits (one file conflating separable predictions),
  emergent clusters (several files that are aspects of one question), orphans (untested
  and targeted by nothing), supersessions, and any other restructuring the set turns out
  to need.
- Re-derives every status from the verified entries actually in each record — under the
  strong form a status is a computation over promoted entries, never a judgment made
  during consolidation.
- Structural changes follow `hypothesis-records.md` § Ceremony scaling: new files, a
  final iteration entry in each old file naming its replacement, old status
  `challenged`, index updated. Ids are never reused.
- Re-points spec-pool entries and forward-plan references to the surviving ids.

Consolidation writes no evidence and re-tags nothing: an entry whose hypothesis was
rewritten in a merge or split is re-verified through the referee like any iteration
(`evidence-pipeline.md` § Iteration).

## What it produces

1. The updated hypothesis files, each structural change recorded as an iteration entry
   ("Consolidated: merged with former 011 because …").
2. `docs/v3-framework/consolidation-N.md` — write-once, the set-level provenance: what
   merged, split, was archived or created, why, and what the landscape looks like now.
   The `created` entries of hypothesis files do not carry extraction provenance; this
   report does.
3. The updated index.
4. A pending forward plan: consolidation always requires a new plan, because the old
   plan's references are stale. The reverse does not hold — a new plan does not require
   consolidation.

# The experiment lifecycle

One sequence, from a question to a promoted evidence entry, served by the agent runner's
host at `/protocol` and kept in the repo as `fanout/PROTOCOL.md`. The skills hold the
**rules** (`v3-buildout` for what a cell or a WU may do, `agent-runner` for the instrument);
this page holds the **order**, and each step names what it produces and which rule governs
it. A run's page shows, from its folder alone, which of these steps it carries evidence of.

## The steps

| # | Step | Who | Produces | Governed by |
|---|---|---|---|---|
| 1 | **Question** | HITL session | an entry in `docs/v3-framework/spec-pools/<corpus>.md` | `v3-buildout` › `evidence-pipeline.md`. Nothing runs without one. |
| 2 | **Cell and WU type** | HITL | the WU card: `Type`, `Corpus`, `Scale` (= the matrix cell) | `v3-buildout` › work matrix, `wu-execution.md`. The cell decides model, context and the verification owed. |
| 3 | **Work folder** | HITL | `fanout/<work>/` — a WU id, or an action's name (`referee`, `skill-audits`) | `agent-runner` › Layout. Vertical by work; nothing shared across works. |
| 4 | **Instrument** | HITL | `codebook.md` or `protocol.md` in the work folder; for a codebook, a `calibration-<date>.md` beside it before any batch | `v3-buildout` rule 4; `agent-runner` rule 4. |
| 5 | **Enumerate** | a tool, once | `items/` + `manifest.md` when the items are regenerable; a committed folder (e.g. `excerpts/`) when they are not | `agent-runner` rule 2. The agent never enumerates its own items. |
| 6 | **Generate** | `make-jobs.*` beside the instrument | `jobs.json`: one `item` per job, `requireOnce` markers, the run's ceilings, neutral arm names | `agent-runner` rules 1 and 3. Batches are generated from the manifest, never typed. |
| 7 | **Dry run** | CLI, serverless | every prompt composed and sized; nothing launched | `AgentRunner.exe <run>/jobs.json --dry-run` |
| 8 | **Pilot** | CLI `--job <id>` → the host | one attempt, its output read by a person; its ledger row carries `Mode: pilot` | `agent-runner` rule 4. No batch without it. |
| 9 | **Batch** | the host | `ledger.jsonl`, `attempts/`, `results/`; watched on the run's page and steered with harness knobs only; may be scheduled (`--at HH:mm` or `--at reset`) to run when the usage window resets | `agent-runner` › the host. |
| 10 | **Tally** | `tally.*` beside the instrument | counts and flagged rows from `results/` | `agent-runner` (a work ships its tallier). Adjudication reads this, never the raw batch. |
| 11 | **Verify and promote** | the referee (its own work folder), then a HITL promotion session — or, for a non-evidence action such as a skill audit, an adjudication document | candidates with verdicts; promoted entries in `docs/v3-framework/hypotheses/` | `v3-buildout` › `evidence-pipeline.md`. |
| 12 | **Record** | HITL | `run.md` in the run folder; the run committed per the convention; the artifact in `docs/` citing the ledger row | `agent-runner` › what a run commits, citing. |

## What a run folder holds

```
fanout/<work>/
  protocol.md | codebook.md      the instrument (+ calibration-<date>.md for a codebook)
  make-jobs.*  tally.*            the generator and the tallier
  <run>/
    run.md                        the authored front page (below)
    items/manifest.md             the enumeration's index (bodies regenerable, not committed)
    jobs.json                     generated
    ledger.jsonl                  one row per attempt — the record an artifact cites
    results/                      the agents' outputs
    attempts/<job>/attempt-N/     prompt.md and stream.jsonl — local only
```

## `run.md` — the front page of a run

Small, authored, committed, rendered at the top of the run's page. It says: which work and
which question(s) (spec-pool ids) the run serves; the cell; the instrument and its hash at
calibration; the arms and what is deliberately **not** measured (one arm → no stability
figure); where the adjudication or promotion lives. Everything else about the run is
mechanical and lives in the files above.

## What the page shows, and what it never does

The stage strip on a run's page is detected from the folder: instrument present, calibrated
(codebooks only), enumerated, generated, piloted, batch complete, tallier present, `run.md`
present. Nothing is judged; a missing stage is a fact about the folder.

The harness controls — pause, resume, stop after in-flight, cancel a job, the run's and the
host's parallel ceilings, the utilization cap — change how a batch runs and never what a
job is. No button changes a model, a protocol, an input or an instruction, and none
re-launches a succeeded or exhausted job under a new id: those are edits to `jobs.json`,
which is a new run. The same controls exist as JSON routes (`/api/…`) for a terminal or a
Claude Code session.

# Spec pools

One file per corpus (`<corpus>.md`). The seven corpora of the buildout as of 2026-09-03,
each with a pool file here: `analysis-corpus.md` (the 112 analyzed stories),
`own-fiction.md`, `v1-archive.md`, `working-plan.md` (v2), `lineage.md`,
`conversations.md`, `code-sessions.md`. A corpus added later gets a file the same way. A
spec pool is where an
**exploratory** pass, or a post-WU review, deposits the *questions* it raised about that
corpus — never findings, never evidence. Verification passes are built from the pool.

An entry is one question:

```
### <short title>
- asked-by: WU<n>.<m> (<date>)
- bears-on: <hypothesis ids>
- question: <one testable question about this corpus>
- candidate-predicate: <if a frozen predicate suggests itself; may be blank>
- status: open | folded-into <codebook name> | answered-by WU<n>.<m> | withdrawn (<reason>)
```

Rules: questions flow freely — any pass may add to any corpus's pool; findings wait —
nothing here counts toward a hypothesis. Entries are appended, never rewritten; a
superseded entry changes only its `status` line. Governed by the `v3-buildout` skill
(`wu-execution.md`).

Created 2026-09-03 (methodology revision 1).
