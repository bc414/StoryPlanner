---
name: v3-buildout
description: Methodology for the v3 narrative design framework buildout — hypothesis management, experiment design, forward plans, consolidation, and provenance. Load before any framework buildout work.
---

# V3 Framework Buildout

This skill governs the full v3 framework buildout: managing hypotheses, designing
and executing experiments (work units), maintaining forward plans, running
consolidation sessions, and preserving provenance. The epistemic framework
principles live in CLAUDE.md (evidence-relationship status, baselining, no terminal
states); this skill applies those principles operationally.

## Epistemic framework (applied)

CLAUDE.md establishes the principles. This section applies them to the buildout.

**Scope:** Both the **narrative design framework** (track architecture, cognitive
modes, mechanism/goal/technique model, reader-experience-moment concept,
perception gap delivery, variable focalization, voice registers) and the
**codebase architecture** (Type Object pattern extensions, dimensional
annotations, note-to-note relationships, data-driven levels, MCP server
instructions) are in scope. The framework and the codebase are coupled but
separable: the framework determines WHAT the planner should track; the codebase
determines HOW it's represented in data. Framework hypotheses come first;
codebase implications follow.

**The current analytical framework vocabulary is provisional.** The mechanism ×
inference-stage vocabulary, cognitive modes, the 5-layer split, and the rest of
the v2 framework are best-effort working vocabulary. The buildout may revise any
of it. Do not treat the existing framework terms as settled just because they
appear authoritatively in CLAUDE.md or in track definitions.

**Three epistemic states for hypotheses:**

- `untested` — captured, no evidence examined.
- `evidenced` — evidence gathered and currently supporting. Thin or thick — the
  record shows the weight. Always revisable.
- `challenged` — unresolved counterevidence exists. One unresolved challenging
  entry puts the hypothesis in challenged regardless of how much supporting
  evidence also exists. The supporting evidence doesn't vanish — it's in the
  record. The challenge needs resolution.

Transitions are reversible: evidenced → challenged (counterevidence deposited),
challenged → evidenced (challenge resolved by refining the hypothesis or
addressing the counterevidence), evidenced → untested (never — once evidence
exists, the hypothesis is at minimum evidenced or challenged).

**Baselining is progress tracking, not epistemology.** When Brian reviews a
hypothesis's evidence picture and judges it sufficient to act on, that is a
baseline. The baseline entry in the record captures Brian's rationale. Baselining
does NOT:

- Make the hypothesis stronger or more weighty than a non-baselined hypothesis
  with the same evidence
- Make the hypothesis less challengeable
- Add epistemic weight — evidence supersedes baselining rationale
- Constitute endorsement of truth

See "Status transitions — who can do what" below for the baseline revocation
mechanic.

**Evidence alignment is relative to the current hypothesis.** Each evidence entry
carries an alignment tag (`supporting`, `challenging`, or `contextual`). The
alignment reflects the evidence's relationship to the CURRENT hypothesis
statement. When a hypothesis is rewritten, prior evidence entries' alignment tags
must be re-assessed — a rewrite that accommodates previously challenging evidence
changes that evidence's alignment to supporting. The evidence content (what was
found) never changes. Only the alignment tag changes.

`contextual` alignment means: gathered under a prior framing, relevance to the
current hypothesis unclear. Neither supporting nor challenging — pending
reassessment. Used when the hypothesis has been substantially reframed and old
evidence needs fresh evaluation.

**Recall is atmosphere, evidence is grounding.** When anyone states something
about the data — Brian from recall, a prior session, a memory entry, this skill,
or any document — treat it as a hypothesis about what the data says, not as a
fact. Before acting on a recall-based claim:

1. Query the relevant evidence source for the specific claim
2. Compare what the evidence actually says against the recall
3. Present any discrepancies — the grounded version may confirm, refine, or
   contradict the recall
4. Proceed with the grounded version after Brian reviews

Nothing is exempt.

**Grounding connectors:** MCP server (working plan, v1 archive, conversations,
lineage, source texts), web search (historical parallels), local files (the
112-story corpus in `source_material_references/Reading Archive Analyses/`,
Brian's own fiction, supplementary material). CLAUDE.md has the semantics of each
data source (what it is, what it's for, caveats). CORPUS-STATUS.md in this
skill's folder has the material inventory (what exists, what's pending).

**Status transitions — who can do what:**

- **Agent can transition:** `untested` → `evidenced` (when supporting evidence is
  deposited). `evidenced` → `challenged` (when challenging evidence is deposited).
  `challenged` → `evidenced` (when the hypothesis is rewritten to resolve the
  challenge and no other challenges remain open).
- **Only Brian can baseline.** The agent can surface candidates ("this hypothesis
  has evidence entries and no open challenges — review for potential baselining")
  but never sets the baselined field.
- **Automatic on challenge of baselined hypothesis:** `baselined` resets to
  `false`. The challenging evidence entry is the revocation event.

## Session routing

**Before starting work, determine your activity and read accordingly:**

- **Hypothesis iteration** (updating a hypothesis, depositing evidence, refining
  a statement): Read the hypothesis file you're working on. If its prose
  references other hypotheses, read those too. Focus on: Hypothesis management
  section.

- **WU execution** (running an experiment): Read the active forward plan to find
  your WU spec and target hypotheses. Read those hypothesis files. During primary
  work, focus on target hypotheses. In the wrap-up step, read the full hypothesis
  index to check for far-reach findings. Focus on: Experiment cycle → WU
  execution section + Hypothesis management (for evidence deposit).

- **Consolidation** (restructuring the hypothesis set): Read the full hypothesis
  index, then read ALL hypothesis files. Read the active forward plan (to
  understand what was planned). Focus on: Experiment cycle → Consolidation
  section + Hypothesis management + Forward plan + Provenance.

- **Forward plan creation** (writing a new experimental agenda): Read the full
  hypothesis index and all hypothesis files. Read the consolidation report if one
  just happened. Focus on: Experiment cycle → Forward plan section + Hypothesis
  management (for status assessment).

- **Ad hoc conversation** (Brian asks about the framework, discusses ideas): Read
  the hypothesis index for orientation. Read specific hypothesis files as the
  conversation touches them. If Brian states something that meets the new-
  hypothesis detection criteria, offer to create a file.

## Hypothesis management

### File format

Hypothesis files live in `docs/v3-framework/hypotheses/`. Each file is named
`NNN-slug.md` where NNN is a zero-padded three-digit stable ID and slug is a
descriptive kebab-case name.

```markdown
---
id: 17
status: evidenced
baselined: false
created: 2026-09-01
---

## Hypothesis

[Concise testable prediction. What this claims, what would confirm or refute it.
If you're explaining WHY you think this, you've drifted into rationale. One to
three sentences.]

## Rationale

[Why this hypothesis exists. The provenance, the pattern that prompted it, the
reasoning. Written when the hypothesis is first formulated or substantially
reframed. Not updated on minor refinements — the record captures those. This
section can be as rich as the reasoning requires, but it is supporting context,
not the prediction itself.]

## Record

[Chronological entries — evidence, iteration, and baseline — with full
timestamps. See Record conventions below.]
```

**Frontmatter fields:**

- `id` — stable integer, never reused after supersession. Unique across the
  entire hypothesis set.
- `status` — one of `untested`, `evidenced`, `challenged`.
- `baselined` — `false` or an ISO date (e.g. `2026-09-05`). The date records
  when Brian last baselined this hypothesis. Reset to `false` when challenging
  evidence arrives.
- `created` — ISO date. When the hypothesis was first captured. Stable.

**Verbosity guardrails:**

The hypothesis statement must be readable in isolation as a testable prediction.
If it can't be, it's carrying material that belongs in the rationale or record.

- **Statement:** What this predicts. What would confirm or refute it. 1-3
  sentences. No provenance, no implications, no testing methodology.
- **Rationale:** Why this hypothesis exists. One to three paragraphs. Written
  once or on major reframings. Not grown on every iteration.
- **Record:** Compressed deltas. Each entry is one to three sentences. Not a
  re-explanation of the full hypothesis.

### Record conventions

The record is a single chronological list with three entry types, distinguished
by structured markers. Full ISO timestamps (not just dates — multiple events
happen per day).

**Evidence entries** have a source in parentheses and alignment in brackets:

```
- evidence | 2026-09-02T14:30 | (WU1.3) [supporting]: Corpus shows 73% of
  stories use non-FID mechanisms for perception gap delivery.
```

**Iteration entries** describe a hypothesis text change:

```
- iteration | 2026-09-03T09:15: Narrowed from "FID isn't the only mechanism" to
  "at least four delivery mechanisms including FID." Based on WU1.3 evidence.
  Re-assessed 2026-09-02 evidence from [challenging] to [supporting].
```

**Baseline entries** record Brian's review judgment with rationale:

```
- baselined | 2026-09-05T16:00: Evidence from WU1.3 and WU1.4 converge on the
  same pattern. No open challenges. Sufficient to inform track architecture
  decisions.
```

**Alignment editing:** When a hypothesis is rewritten (iteration entry), re-
assess ALL prior evidence entries' alignment tags. Edit the alignment tag in
place on the original evidence entry line (e.g., change `[challenging]` to
`[supporting]`). The iteration entry records THAT re-assessment happened and
which entries changed. The evidence content (what was found) is never edited —
only the alignment tag.

**Grep patterns:** `^- evidence` finds all evidence entries across files.
`\[challenging\]` finds all currently-challenging evidence. `^- baselined` finds
all baseline events. `^- iteration` finds all hypothesis text changes.

### Ceremony scaling

Ceremony is proportional to the magnitude of the change:

**Minor** (wording tightened, no conceptual change): Update hypothesis statement.
Add a one-line iteration entry with timestamp and reason. Status stays the same
unless the change was evidence-driven.

**Significant** (scope changed, evidence prompted rethink): Full iteration entry
with evidence citations. Re-assess prior evidence alignment. Reconsider status.
Update rationale section if it's now misleading.

**Structural** (split, merge, supersede): Create new file(s). Add a final
iteration entry to the old file recording the structural change and what
replaced it. Update old file's status to `challenged` (supersession is a pattern
of challenge, not a fourth state). Update the hypothesis index. This is
consolidation-session territory.

### How to challenge a hypothesis

Cite specific evidence that conflicts with the hypothesis. Reference note IDs,
conversation blocks, lineage entries, corpus analysis findings, or external
research. "This doesn't feel right" is not a challenge. "Note 1602 says X,
which contradicts the prediction that Y" is a challenge.

A challenge is an evidence entry with `[challenging]` alignment, deposited in
the hypothesis's record. If the challenge is substantive enough to change the
hypothesis's status, update the frontmatter to `challenged`.

Brian may also challenge hypotheses — his challenges carry the weight of his
analytical voice and should be engaged with by grounding against evidence, not
elaborated away.

### New hypothesis creation

The same protocol applies whether creating one hypothesis or forty during a
consolidation. The file must have: frontmatter with all required fields,
a hypothesis statement (concise testable prediction), a rationale, and an
empty or initial record.

**Detection criteria for emergent hypotheses:**

1. **Novelty:** Does this fit as evidence for an existing hypothesis? If yes,
   deposit evidence, don't create a new file. If it doesn't fit any existing
   hypothesis, it might be new.
2. **Testability:** Does it make a prediction that evidence could confirm or
   refute? An observation without a testable claim stays in the WU artifact.
3. **Independence:** Is this genuinely separate from existing hypotheses, or a
   refinement/sub-hypothesis? Refinements are iteration entries on the existing
   hypothesis, not new files.

**Source rules:**

- Brian's explicit statements always get the offer to create a hypothesis file.
- Claude's analysis can surface potential hypotheses only when all three criteria
  above are met. The offer is presented for Brian's endorsement — Claude NEVER
  creates a hypothesis file autonomously.
- When Claude proposes, the proposal must reference Brian's original statement or
  the specific evidence that prompted it. Brian reviews the statement wording and
  either rewrites it in his own words or approves. The file's record captures
  provenance: "originated from Claude's analysis of WU1.3 findings; Brian
  endorsed on [date]."

**Timing during WU execution:** During primary WU work, note unexpected findings
but hold new-hypothesis proposals for the wrap-up step. In all other session
types, propose when the detection criteria are met.

**V1 trap prevention:** The risk is Claude proposing a hypothesis → Brian
casually approving → the hypothesis enters the set in Claude's framing → future
sessions build on Claude's framing → Brian's own thinking gets channeled.
Prevention: Claude's proposal includes the specific evidence, not a synthesis.
Brian reviews the STATEMENT, not Claude's explanation of why it should exist.
The provenance is recorded so future sessions can distinguish Brian-originated
from Claude-originated hypotheses.

### Connections between hypotheses

No formal cross-references in frontmatter or metadata. Connections emerge from
prose — the hypothesis statement, rationale, and record entries naturally mention
other hypotheses when relevant. Formal reference fields are staleness targets.

Consolidation sessions are the maintenance mechanism for the hypothesis graph:
they read across the full set, notice emergent clusters, identify duplicates, and
restructure as needed.

### Hypothesis index

The index lives at `docs/v3-framework/hypotheses/INDEX.md`. It is a routing
table, not a dashboard — no summaries (which drift from the hypothesis file's
statement).

```markdown
| ID | Slug | Status | Baselined |
|----|------|--------|-----------|
| 001 | [wi-terminal-ratio](001-wi-terminal-ratio.md) | untested | — |
| 002 | [dt-knowledge-asymmetry](002-dt-knowledge-asymmetry.md) | evidenced | 2026-09-05 |
```

Four columns. ID for grep. Slug as clickable link. Status and baselined from
frontmatter. Updated whenever a hypothesis file's frontmatter changes.

## Experiment cycle

### Forward plan lifecycle

A forward plan is a **snapshot experimental agenda** — born from the current
hypothesis landscape, guides work for a period, retired when findings or
consolidation change the landscape significantly.

**Numbering:** Forward plans are sequentially numbered: `forward-plan-1.md`,
`forward-plan-2.md`. The active plan is always the highest-numbered one. A
retired plan with no successor yet (consolidation happened, new plan pending)
is signaled by the consolidation report existing without a matching next-
numbered plan.

**Contents:**

```markdown
# Forward Plan [N]

Created: [date]
After: consolidation-[N] (or: priority reassessment from [reason])

## Rationale

[Prose introduction: what the last consolidation or reassessment revealed, what
the hypothesis landscape looks like now, why these experiments in this order.
Written once. Not a summary — enough context for a session to understand the
strategic reasoning. Can be long if the reasoning warrants it.]

## Work Units

### WU [N.Y]: [descriptive title]

**Question:** [what this experiment asks]
**Hypotheses:** [list of hypothesis IDs this WU informs]
**Evidence sources:** [what data to examine]
**Scope:** [what it does and doesn't do, briefly]
**Scale:** [single session / multiple subagents / batched]
**Preconditions:** [blocking items — downloads, exports, etc. — if any]
**Status:** proposed

[Prose description: enough for a plan-mode session to flesh out the full
execution methodology. Not the methodology itself — that's the plan-mode
session's job. The question, the target hypotheses, the evidence sources, and
enough scope context to design the execution. Can be longer when the experiment
is complex or the scope boundaries matter.]

(independent of WU N.2 — can run in parallel)

### WU [N.Y+1]: [title]
...
(most valuable after WU N.1 findings, but can start with partial results)
```

**WU numbering:** The major number is the forward plan number. The minor number
counts from 1 within each plan: WU1.1, WU1.2, ..., WU2.1, WU2.2, etc. The
major number bump signals a new era of work; the minor number resets per plan.

**Hypothesis references live HERE, not in hypothesis files.** The forward plan's
WU specs name which hypotheses each experiment informs. Hypothesis files carry
no "tested by" metadata — that's a staleness target. When the forward plan is
revised or retired, the hypothesis references update here, not across dozens of
hypothesis files.

**Advisory ordering:** WUs are listed in suggested execution order (top to
bottom). Where independence or partial dependency matters, an inline advisory
note says so: "(independent of WU N.2)", "(requires WU N.1 complete)", "(most
valuable after WU N.1-N.3 findings, but can start with partial results)." The
ordering is advice, not a gate.

**WU status values:** `proposed` → `scoped` (plan-mode session has designed the
execution) → `in-progress` → `complete`. Updated in place in the forward plan.

**Plan creation triggers:**

- **After consolidation (mandatory):** The hypothesis landscape changed — IDs
  renumbered, hypotheses merged or split, new hypotheses emerged. The existing
  plan's references are stale. A new plan must follow.
- **After priority reassessment (optional, lighter):** WU findings changed
  priorities but the hypothesis set is structurally unchanged. A new plan can be
  written from the same hypothesis set with reordered/revised WUs.

**Forward plan retirement:** When a consolidation happens or priorities shift
enough, the active plan is retired. Add a header stamp to the file:

```markdown
> **Retired [date].** Succeeded by forward-plan-[N+1].md.
> Reason: [why — consolidation restructured the hypothesis set / WU findings
> changed priorities / etc.]
```

The retired plan is provenance — historical reference for understanding what was
planned and why. Not prescriptive. WUs in a retired plan that were never executed
(status: proposed or scoped) are retired proposals, not evidence. Only executed
WUs have findings.

**The forward plan is expected to be a long document.** It carries the full
experimental agenda with enough detail per WU for plan-mode sessions to work
from. Do not artificially constrain its length. The constraint is: enough detail
for a plan-mode session to scope the execution, not more. That's a function of
the experiments' complexity, not a word budget.

**How the forward plan treats hypotheses by status:**

When designing a forward plan, hypotheses in different states call for different
experimental approaches:

- `challenged` — highest priority. Unresolved tensions block downstream work.
  The experiment is diagnostic: which interpretation does the evidence support?
- `untested` — largest knowledge gaps. The experiment is exploratory: what does
  the evidence say about this claim?
- `evidenced` — may need stress-testing or replication from a different angle.
  Or may not need another experiment at all if the evidence picture is strong.
  Lower priority than challenged and untested.
- `baselined` — Brian has reviewed and is acting on these. Unless new challenges
  emerge, no experiment needed.

This priority ordering is advisory. Brian picks what to work on based on
interest, energy, and what's blocking the creative work he wants to do.

### WU design and execution

**Every WU runs a plan-mode session before execution.** The forward plan's WU
spec is the starting point — the plan-mode session designs the full execution
methodology: exact scope, batching strategy, output format, which hypothesis
files to read, what evidence to look for.

**WU session startup:** Read the active forward plan to find the WU spec and its
target hypothesis list. Read those hypothesis files. This is the primary work's
focus — the target hypotheses.

**Primary work:** Execute the experiment focused on target hypotheses. Deposit
evidence entries in the target hypothesis files as findings emerge. Update the
WU's status in the forward plan (`in-progress` → `complete`).

**Wrap-up step:** After primary work, read the full hypothesis index. Check
whether any findings produced evidence relevant to hypotheses OUTSIDE the target
set. If so, deposit evidence entries in those files. This catches serendipitous
discoveries without skewing the primary work.

**New hypothesis detection during WUs:** During primary work, note unexpected
observations but hold new-hypothesis proposals for the wrap-up step. In the
wrap-up, if an observation meets the detection criteria (novelty, testability,
independence), offer to create a hypothesis file with Brian's endorsement.

**WU artifacts:** WU output (reports, structured data, analyses) goes in
`docs/v3-framework/` with a WU-prefixed filename (e.g.,
`WU1.1-corpus-synthesis.md`). Artifacts are write-once evidence. Once produced,
they record what was found. Future sessions reference them but don't edit them.

### Consolidation protocol

**Trigger:** On demand — Brian decides when the hypothesis set needs it. Common
triggers: the set feels tangled, too many challenged hypotheses, duplicates
noticed, a major WU finding restructured the landscape.

**Scope:** Full set. Read the entire hypothesis index, all hypothesis files, and
the active forward plan.

**What consolidation does:**

- Identifies merges (same claim in different words)
- Identifies splits (one hypothesis conflating separable predictions)
- Notices emergent clusters (several hypotheses that are aspects of one question)
- Identifies orphans (untested hypotheses nobody references)
- Re-assesses statuses against accumulated evidence
- Archives superseded hypotheses (status → `challenged`, iteration entry
  recording what replaced it)

**What consolidation produces:**

1. **Updated hypothesis files** with iteration entries recording every structural
   change: "Consolidated: merged with former hypothesis 11 because both were
   aspects of the same model-comparison question."
2. **A numbered consolidation report** (`docs/v3-framework/consolidation-N.md`):
   the set-level provenance recording all decisions and their reasoning. What
   merged, what split, what was archived, what new hypotheses emerged, what the
   overall landscape looks like now. Write-once — it records what a specific
   consolidation session did.
3. **Updated hypothesis index** reflecting all file changes.

**The consolidation → new forward plan coupling is one-directional:**
Consolidation always requires a new forward plan (the hypothesis landscape
changed, the old plan's references are stale). But a new forward plan does NOT
always require consolidation (priorities can shift without the hypothesis set
needing restructuring).

## Provenance

All artifacts have consistent naming:

```
docs/v3-framework/
  hypotheses/
    INDEX.md
    001-slug.md
    002-slug.md
    ...
  forward-plan-1.md         (retired, with header stamp)
  forward-plan-2.md         (active — highest number)
  consolidation-1.md        (report from the event between plan 1 and plan 2)
  WU1.1-corpus-synthesis.md (WU artifact)
  WU1.3-v1-scene-instincts/ (WU artifact, directory if multiple files)
  ...
```

**Forward plans:** `forward-plan-N.md`. Active = highest number. Retired plans
carry a header stamp with date, successor, and reason. A consolidation report
existing without a matching next-numbered plan = new plan pending.

**Consolidation reports:** `consolidation-N.md`. Numbered sequentially. Each
records the decisions and reasoning of one consolidation session.

**WU artifacts:** `WU[plan].[unit]-descriptive-name.md` (or directory). Write-
once evidence.

**Hypothesis files:** `NNN-slug.md` in the `hypotheses/` directory.

**When to consult provenance:** Understanding why a hypothesis looks the way it
does (read its record + the consolidation report that last touched it).
Understanding why a forward plan was retired (read its header stamp + the
consolidation report). Understanding what an experiment found (read the WU
artifact).

**Provenance informs, never prescribes.** Retired forward plans are reference,
not authority. Consolidation reports are records, not standing instructions.
WU artifacts are evidence, not mandates. The active forward plan and the current
hypothesis files are the working instruments. Everything else is history.

## Context documents

Two companion files live in this skill's folder (`.claude/skills/v3-buildout/`):

**VERSION-HISTORY.md** — Origin of the v3 buildout, version history (v0 through
v3 codebase and framework evolution), and the provenance table (which concept
first appeared where). Stable, written once, rarely updated. Referenced by
retrospective WUs and consolidation sessions that need historical context.

**CORPUS-STATUS.md** — What material exists and what state it's in: the 112-
story analysis corpus, Brian's own fiction, supplementary material (comments,
reviews), and blocking items (downloads/exports pending). Evolves as analyses
complete and new material is added. Referenced by forward plans when designing
WUs that need specific inputs.

## What this skill does NOT govern

- **Story content decisions.** Brian decides story structure, categorization,
  what's interesting, whether a flagged note is resolved.
- **Prose technique.** The planner specifies goals and mechanisms, never how to
  write.
- **Planner features.** When a hypothesis implies a codebase change, the change
  goes through the normal CLAUDE.md / wpf-conventions / FEATURE-AUDIT process.
  The hypothesis provides the evidence; the feature process provides the
  governance. Check `FEATURE-AUDIT.md` before proposing anything that touches
  the planner's feature set.
- **Declaring conclusions.** Every finding is a hypothesis until Brian baselines
  it. This skill surfaces evidence and proposes hypotheses. Brian reviews the
  evidence picture and decides what to act on — including but not limited to:
  what changes to make to the planner, the tracks, the display questions, the
  framework vocabulary, the cognitive modes, and the codebase architecture.
