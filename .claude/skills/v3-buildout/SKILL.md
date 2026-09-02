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

`contextual` is a **transitional** tag with exactly one use: an entry deposited
under a prior wording of the hypothesis that has not yet been re-assessed
against the rewritten statement. It is applied only by an iteration entry, and
it is resolved — to `supporting` or `challenging` — by the next session that
reads the record. It is never applied at deposit time. **It is not a tag for
inconclusive, mixed, adjacent, or non-discriminating evidence.** Evidence that
neither supports nor challenges the current statement does not enter the record
at all (see Deposit protocol below); it stays in the WU artifact. Between
2026-08-31 and 2026-09-01 the tag was misused as a soft landing for exactly
that kind of evidence, which is why this paragraph is emphatic.

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

**Single-session default.** All buildout activities — planning, exploration,
questioning, and execution — happen in one session. WU sessions run four phases:
scope reconciliation (auto mode), plan mode, execution (auto mode), then post-WU
review. See WU design and execution for the full protocol. No subagents unless
the source material exceeds ~600K tokens (leaving headroom for hypothesis files,
synthesis output, and evidence deposits in the 1M context window). When subagents
are needed, they extract and report summaries; the main session synthesizes.

**Before starting work, determine your activity and read accordingly:**

- **Hypothesis iteration** (updating a hypothesis, depositing evidence, refining
  a statement): Read the hypothesis file you're working on. If its prose
  references other hypotheses, read those too. Focus on: Hypothesis management
  section.

- **WU execution** (running an experiment): Follow the four-phase protocol in
  WU design and execution. Scope reconciliation reads the forward plan and
  hypothesis files; plan mode reads the evidence sources; execution does the
  primary work and wrap-up. Focus on: Experiment cycle → WU execution section
  + Hypothesis management (for evidence deposit).

- **Consolidation** (restructuring the hypothesis set): Read the full hypothesis
  index, then read ALL hypothesis files. Read the active forward plan (to
  understand what was planned). Focus on: Experiment cycle → Consolidation
  section + Hypothesis management + Forward plan + Provenance.

- **Forward plan creation** (writing a new experimental agenda): Read the full
  hypothesis index and all hypothesis files. Read the consolidation report if one
  just happened. Focus on: Experiment cycle → Forward plan section + Hypothesis
  management (for status assessment).

- **Post-WU review** (Brian reviews a WU's findings): Follows WU execution in
  the same session. Two interleaving modes:

  *Challenge mode*: Brian questions a finding. Verify against source evidence —
  this may require tool calls to per-story analyses or original story text, not
  just the meta-analyses. If the evidence doesn't hold, correct the synthesis
  report, evidence entries, and hypothesis statements. The narrator-character
  blend investigation (WU1.1) showed meta-analysis summaries can mischaracterize
  what per-story evidence actually shows.

  *Enrichment mode*: Brian connects a finding to his practice or recall. Add
  testing specs to downstream WU specs in the forward plan. Do NOT add Brian's
  recall to hypothesis records — recall is not evidence and not a statement
  change. WU specs are the stash for "test this in a future WU."

  **Story-content boundary**: thematic content comparisons ("my stories also
  argue vulnerability-as-prerequisite") don't inform the framework. The
  framework studies technique, architecture, and methodology. When the
  discussion drifts into content territory, redirect to the framework-relevant
  question ("do the tracks support this?") or acknowledge it's outside scope.

  **Hypothesis statement updates are batched at the end of the review**, not
  done inline. Enrichment-mode WU spec additions go inline as the discussion
  flows — they serve as memory. After the discussion is complete, sweep the
  hypothesis set: which statements need updating based on the full picture?
  Findings refine each other across the discussion, so statements written at
  the end are more precise than inline updates would be.

  **Standard outputs**: corrected synthesis report (challenge mode), enriched
  downstream WU specs (enrichment mode), batched hypothesis statement updates,
  updated INDEX.md.

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

[Concise testable prediction. 1-3 sentences. If you're explaining WHY the
prediction exists, you've drifted into founding reasoning — that goes in the
record's first entry.]

## Record

[Chronological entries — created, evidence, iteration, and baseline — oldest
first, newest appended to the bottom. See Record conventions below.]
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
If it can't be, it's carrying material that belongs in the record's founding
entry.

- **Statement:** What this predicts. 1-3 sentences. No provenance, no
  implications, no testing methodology, no confirm/refute conditions.
- **Record entries:** Compressed but as long as the finding requires. Iteration
  entries are typically short (one to three sentences). Evidence entries may be
  longer when the finding is dense — specificity (naming mechanisms, citing
  counts, listing examples) is more valuable than brevity. The founding
  `created` entry may also be longer (creation is a bigger event than a typical
  iteration). The constraint is: each entry should be one citable unit, not a
  multi-finding essay. If an entry covers two distinct findings, split it into
  two entries.

### Record conventions

The record is a single chronological list, **oldest first, newest appended to
the bottom.** Four entry types, distinguished by structured markers. Full ISO
timestamps (not just dates — multiple events happen per day).

**Created entries** are always the first entry. They explain why the hypothesis
was created — the reasoning, the motivation, the observation that prompted it.
The hypothesis stands on its own as a primary source; the created entry does
not cite which document or prior numbering it was extracted from (that
provenance chain is in the consolidation report). The entry is written in
Claude's voice with Brian's assertions as the primary content. It does NOT
include confirm/refute conditions or testing methodology — those are the
forward plan's job when designing WUs.

```
- created | 2026-09-01T10:00: Variable focalization may be the master
  perspective principle rather than deep third/FID. The v2 framework centered
  FID as the primary technique; the 112-story corpus and Brian's NLM analysis
  suggest the power comes from variation across the focalization spectrum, not
  from staying deep.
```

**Evidence entries** have a source in parentheses and alignment in brackets:

```
- evidence | 2026-09-02T14:30 | (WU1.3) [supporting]: Corpus shows 73% of
  stories use non-FID mechanisms for perception gap delivery. Would differ if
  false: FID would be the sole or dominant mechanism in the M4 stories.
```

The trailing "Would differ if false:" clause is required on every evidence
entry. It is the discrimination test (Deposit protocol, below) written down
where the tag can be checked against it.

Evidence must be grounded in corpora or verifiable sources — not Brian's recall
about his own practice (that's the hypothesis being tested) and not a restatement
of the hypothesis itself. When depositing evidence from meta-analyses, verify
ambiguous or architecturally significant findings against per-story analyses or
original source text before depositing — meta-analysis summaries can
mischaracterize what the per-story evidence actually shows.

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

**Grep patterns:** `^- created` finds all founding entries. `^- evidence` finds
all evidence entries. `\[challenging\]` finds all currently-challenging evidence.
`^- baselined` finds all baseline events. `^- iteration` finds all hypothesis
text changes.

**What does NOT go in hypothesis records:**

- **Brian's recall about his own practice.** "I think I do this instinctively"
  is the hypothesis being tested, not evidence for it. It goes in the forward
  plan's WU spec as a testing spec ("check whether Brian's fiction shows X").
- **Brian's design observations.** "This connects to my story in way Y" is
  story content, not framework evidence. If it produces a framework question
  ("do the tracks support Y?"), that goes in a WU spec.
- **Observations that don't change the statement.** If an insight informs a
  future WU but doesn't change what the hypothesis predicts, it's a WU spec
  addition, not an iteration entry. Iteration entries record *what changed in
  the statement and why*.
- **Methodological pointers.** "WU1.3 should check this" is a WU spec note,
  not a hypothesis record entry.

The test: if removing the entry would leave the hypothesis record incomplete
(missing a statement change, missing grounded evidence, missing a baseline
event), it belongs. If removing it would only lose a pointer to future work,
it belongs in the WU spec instead.

### Deposit protocol

A WU's analyses run discovery-first with no hypothesis in view; its deposits
are written with the hypothesis list open, by a session that has just spent
hours looking for the predicted patterns. The analyses are impartial by
construction; the deposits are not. The protocol is two writes in sequence —
first the WU artifact, then the hypothesis records — each written once, both
by the main session. Deposits are judgment and are never delegated to a
subagent.

**First write — the WU artifact.** It is organized by what was observed —
texts, passages, counts — not by hypothesis. No hypothesis IDs in its section
structure. It is complete before any hypothesis record is touched.

**Second write — the hypothesis records.** With the artifact finished, for
each hypothesis the WU targets:

1. Locate the relevant findings in the artifact.
2. Write the "Would differ if false:" clause. If it cannot be written, the
   findings do not discriminate and **no entry is written** — the findings
   are already in the artifact; add a pointer in a downstream WU spec if they
   need follow-up. If it can be written, the tag is `supporting` or
   `challenging`.
3. If the statement names specific items, address every named item. Any
   named item contradicted makes the entry `[challenging]` and the hypothesis
   `challenged`, regardless of the overall impression.
4. If the entry rests on a classification made by an intermediate analysis
   (FID vs DT vs blend, mechanism level, obstacle type, perspective mode),
   cite the passage and read the surrounding paragraph in the original source
   before writing the entry — the preceding clauses decide whose register a
   sentence is in. Intermediate analyses assert classifications without showing their
   evidence.

A recall-derived testing spec ("Brian's recall: X — does the evidence confirm
X?") is a prediction to test, not a search target; disconfirmation of the
recall is recorded as prominently as confirmation.

### Ceremony scaling

Ceremony is proportional to the magnitude of the change:

**Minor** (wording tightened, no conceptual change): Update hypothesis statement.
Add a one-line iteration entry with timestamp and reason. Status stays the same
unless the change was evidence-driven.

**Significant** (scope changed, evidence prompted rethink): Full iteration entry
with evidence citations. Re-assess prior evidence alignment. Reconsider status.
Update the founding `created` entry if the reframing makes it misleading.

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
a hypothesis statement (concise testable prediction) and a record with a
founding `created` entry.

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
prose — the hypothesis statement and record entries naturally mention
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

**Designing the experimental agenda:**

The forward plan is a best-effort agenda for the full hypothesis set — not a
partial selection. Every hypothesis should be targeted by at least one WU.
Hypothesis tiers are comprehension order for reading the index, not execution
order or scoping boundaries; WUs cross tiers freely (a corpus synthesis WU
will inform hypotheses from Tiers F, G, and H simultaneously).

Start from the hypothesis landscape, not from prior plans or synthesis
documents. Read the full index and hypothesis files. For each hypothesis or
cluster, ask: what evidence would move this from untested to evidenced or
challenged? The answer is a candidate experiment.

Group candidate experiments by evidence source. Multiple hypotheses testable
from the same evidence (the same corpus read, the same archive mining pass)
share a WU. A WU that can't name specific hypotheses it informs is too vague.

WUs with unmet preconditions (blocked on Brian, needs a skill, needs prior WU
findings) are in the plan with preconditions noted — not deferred to a future
plan. The plan is comprehensive; execution order follows the criteria below.

Prior plans and synthesis documents are reference for understanding what was
tried before — not templates. A forward plan that looks like a renumbering of
a prior document has not engaged with the current landscape.

**Ordering criteria** (in priority order — hard constraints first, then
information flow, then strategic and advisory):

1. **Evidence dependency chains.** WU X needs WU Y's findings as input. A
   retrospective that assesses framework provenance against corpus evidence
   can't run until the corpus synthesis has produced that evidence. These are
   constraints the plan must respect, not preferences.
2. **Enrichment flow.** A WU whose post-review findings would add testing
   specs or scope to another WU goes first, even when the second WU has no
   hard dependency on the first and could execute independently. Discovery
   WUs (open-ended mining, corpus synthesis) produce findings that add
   questions to downstream survey, comparison, and assessment WUs — that
   information flow is one-directional and determines order. Practical
   considerations — which skill is ready first, which WU is quicker, which
   fills a scheduling gap — are not ordering criteria. Convenience and
   throughput do not override information flow.
3. **Precondition blockers.** The experiment needs a skill, a data export, or
   Brian's action that doesn't exist yet. Noted in the WU spec; the experiment
   is in the plan but can't execute until the precondition is met.
   Preconditions gate execution timing in place — they never determine a WU's
   position in the order, and readiness is not an ordering criterion.
4. **Infrastructure hypotheses first.** Some hypotheses predict properties of
   the experimental infrastructure itself — the existence of an evidence source,
   a contamination in the data, a methodological bias, a domain-separability
   assumption. If confirmed or refuted, they change how other experiments are
   designed or how their results are interpreted. Testing them late means
   potentially reworking findings made on wrong assumptions. Their evidence and
   deliberation may warrant a new forward plan. Examples: a hypothesis that an
   un-ingested corpus contains needed provenance (test source existence before
   running experiments that would benefit from it); a hypothesis that the
   working data has voice contamination (assess severity before mining
   experiments that would inherit the confusion); a hypothesis that two domains
   are separable (test before designing experiments scoped to one domain alone).
5. **Unblocking value.** An experiment that produces evidence for many
   hypotheses simultaneously (high evidence-source multiplexing) unblocks more
   downstream work per unit of effort. Run high-yield experiments first.
6. **Foundation before application.** Testing foundational hypotheses (the
   premises downstream claims rest on) before the claims that build on them
   makes findings more interpretable. Not blocking — application experiments
   can run — but results are harder to evaluate on untested foundations.

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
**Scale:** [estimated token volume of source material and whether it fits in one
context window, e.g. "~88K tokens — single context window with ample headroom"
or "~900K tokens — requires subagent extraction and summary"]
**Preconditions:** [tooling or Brian-action blockers — downloads, exports,
skills to build — if any. Never WU dependencies: those live in the ordering
audit, not in WU specs]
**Status:** proposed

[Prose description: the question, the target hypotheses, the evidence sources,
and enough scope context to design the execution in plan mode. Can be longer when
the experiment is complex or the scope boundaries matter.]

### WU [N.Y+1]: [title]
...
```

**WU numbering:** The major number is the forward plan number. The minor number
counts from 1 within each plan: WU1.1, WU1.2, ..., WU2.1, WU2.2, etc. The
major number bump signals a new era of work; the minor number resets per plan.

**Hypothesis references live HERE, not in hypothesis files.** The forward plan's
WU specs name which hypotheses each experiment informs. Hypothesis files carry
no "tested by" metadata — that's a staleness target. When the forward plan is
revised or retired, the hypothesis references update here, not across dozens of
hypothesis files.

**Ordering:** WU specs are listed in numeric id order — the plan is a catalog,
found by id, not a schedule. Execution order is derived and maintained by the
plan's ordering audit (`forward-plan-N-ordering-audit.md`, a living companion
document that carries its own procedure: blind two-pass pairwise evaluation of
consumption and enrichment edges, mechanical assembly, dated amendments). The
plan's execution-sequence section states the current derived order and cites
the audit; it is updated whenever the audit's derived order changes. WU specs
carry no ordering notes — no trailing parentheticals, and no WU-dependency
claims in Preconditions fields.

**WU status values:** `proposed` → `scoped` (plan mode has designed the
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
from. Do not artificially constrain its length. The constraint is: enough detail to
scope the execution in plan mode, not more. That's a function of the
experiments' complexity, not a word budget.

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

This priority ordering is one input to experiment design. The ordering criteria
in "Designing the experimental agenda" above are the primary ordering mechanism;
hypothesis status is a secondary signal within that structure.

### WU design and execution

**Every WU runs four phases in the same session:** scope reconciliation,
plan mode, execution, then post-WU review (described under Session routing).
No handoff to a separate session or agent.

**Scope reconciliation phase** (auto mode, before plan mode):

Read the active forward plan to find the WU spec and its target hypothesis
list. Read those hypothesis files. Prior WU post-reviews add testing specs to
downstream WU specs — the WU's scope at execution time is larger than at plan-
creation time. Reconcile: do the accumulated testing specs touch hypotheses not
in the original list? If so, update the WU's hypothesis list in the forward
plan. Update the per-hypothesis WU coverage table to match. Re-assess the WU's
scale description — a WU characterized as "quick" at plan-creation time may no
longer be quick after upstream reviews have added scope. These are clerical
forward-plan edits (bringing metadata in line with content), not judgment calls.
The planning phase needs accurate scope on the page to work from.

**Plan-mode phase:**

1. **Read.** Read the evidence sources (or assess their size — if they exceed
   ~600K tokens, plan subagent extraction instead of direct reading). Read
   CORPUS-STATUS.md if the WU involves corpus material. The forward plan and
   hypothesis files were already read during scope reconciliation; re-read only
   if the reconciliation changed the hypothesis list.
2. **Identify all open questions.** What is ambiguous in the WU spec? What
   decisions require Brian's input? What assumptions need confirmation? Collect
   every open question before asking any.
3. **Ask all open questions.** Use AskUserQuestion, batching at the 4-question
   limit per call and continuing with remaining questions until all are answered.
   Do not write the plan before questions are resolved — the answers shape the
   plan.
4. **Write the plan.** With all questions answered, write the execution plan to
   the plan file. The plan describes: what to read and in what order, what the
   output document covers, how evidence deposit works, the wrap-up step, and
   what the WU does NOT do.
5. **Exit plan mode.** Brian reviews and approves. The session continues into
   execution.

**Execution phase:**

**Primary work:** Execute the experiment focused on target hypotheses. Deposit
evidence entries in the target hypothesis files after the synthesis is complete
(batch deposit — findings refine each other across sections, so entries are more
precise after the full picture). Update the WU's status in the forward plan
(`in-progress` → `complete`).

**Wrap-up step:** After primary work, read the full hypothesis index. Check
whether any findings produced evidence relevant to hypotheses OUTSIDE the target
set. If so, deposit evidence entries in those files. This catches serendipitous
discoveries without skewing the primary work.

Then report the tag counts to Brian: targets, entries written, supporting,
challenging, and targets where no entry was written because the findings did
not discriminate. Brian decides whether the distribution warrants a recheck.

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

**Implementation candidates:** `implementation-candidates.md`. Proposed codebase
changes gated on hypotheses. Living document — new candidates added as they
emerge. Not a forward plan (doesn't prioritize experiments) and not FEATURE-AUDIT
(doesn't record decisions). Candidates become actionable when their gating
hypotheses are baselined, then enter the normal development process.

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

**VERSION-HISTORY.md** — Verifiable facts about the project timeline and
architecture: dates, commit counts, tool/platform transitions, architectural
changes, key conversation metadata. Facts only — no interpretive claims about
why things happened, where concepts came from, or what characterizes each era.
Interpretive claims are hypotheses and belong in hypothesis files.

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
