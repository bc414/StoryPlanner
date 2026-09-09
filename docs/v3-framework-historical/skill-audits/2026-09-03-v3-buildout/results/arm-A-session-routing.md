## unit-030
- section: ## Session routing
- quote: **Single-session default.** All buildout activities — planning, exploration, questioning, and execution — happen in one session…
- counterpart: wu-execution.md › The four phases / agent-runner/SKILL.md › Two mechanisms
- relation: reversed
- note: B forbids the permitted mechanism — in-session subagents extracting/reporting summaries when material exceeds a token threshold — replacing it with matrix-cell routing to the external runner (explicit context, no session inheritance) for scale work and restricting the Agent tool to ones-and-twos salience help; the four-phase WU protocol itself is restated in wu-execution.md.

## unit-031
- section: ## Session routing
- quote: Before starting work, determine your activity and read accordingly:
- counterpart: SKILL.md › Session routing — read the named file in full before acting
- relation: restated
- note: —

## unit-032
- section: ## Session routing
- quote: **Hypothesis iteration** (updating a hypothesis, depositing evidence, refining a statement): Read the hypothesis file you're working on…
- counterpart: SKILL.md › Session routing (table row "Hypothesis iteration")
- relation: broadened
- note: B requires reading the governing companion files in full (hypothesis-records.md, then evidence-pipeline.md § Iteration) rather than just the specific hypothesis file(s) being worked on.

## unit-033
- section: ## Session routing
- quote: **WU execution** (running an experiment): Follow the four-phase protocol in WU design and execution…
- counterpart: wu-execution.md › Four types
- relation: broadened
- note: B replaces the single generic "WU execution" case with four distinct types (exploratory, verification, synthesis, infrastructure), each with its own reads, writes, and never-writes, rather than one uniform phase description.

## unit-034
- section: ## Session routing
- quote: **Consolidation** (restructuring the hypothesis set): Read the full hypothesis index, then read ALL hypothesis files…
- counterpart: consolidation.md › Trigger and scope
- relation: broadened
- note: B adds every spec pool to the required reading scope (so merged/split hypothesis references can be re-pointed), beyond the index, hypothesis files, and active plan named in the unit.

## unit-035
- section: ## Session routing
- quote: **Forward plan creation** (writing a new experimental agenda): Read the full hypothesis index and all hypothesis files…
- counterpart: forward-plans.md › Writing the plan
- relation: narrowed
- note: B restates reading the full index and every hypothesis file, but does not carry an explicit instruction to read the consolidation report as a routing step — it is only implied by the rationale section needing to reflect "what the last consolidation … showed."

## unit-036
- section: ## Session routing
- quote: **Post-WU review** (Brian reviews a WU's findings): Follows WU execution in the same session…
- counterpart: wu-execution.md › Post-WU review
- relation: restated
- note: —

## unit-037
- section: ## Session routing
- quote: *Challenge mode*: Brian questions a finding…
- counterpart: wu-execution.md › Post-WU review (Challenge)
- relation: reversed
- note: B forbids directly correcting evidence entries and hypothesis statements as this unit describes; corrections to a verification pass's findings must re-enter as a new candidate through the referee, and statement changes are handled separately as batched iterations — the WU1.1 illustrative example is also not carried.

## unit-038
- section: ## Session routing
- quote: *Enrichment mode*: Brian connects a finding to his practice or recall…
- counterpart: wu-execution.md › Post-WU review (Enrichment)
- relation: narrowed
- note: B keeps the rule that recall never enters a hypothesis record, but the stash mechanism changes from "downstream WU specs in the forward plan" to a question written into the relevant corpus's spec pool.

## unit-039
- section: ## Session routing
- quote: **Story-content boundary**: thematic content comparisons ("my stories also argue vulnerability-as-prerequisite") don't inform the framework…
- counterpart: SKILL.md › Constitutional rules (rule 7, The story-content boundary)
- relation: restated
- note: —

## unit-040
- section: ## Session routing
- quote: **Hypothesis statement updates are batched at the end of the review**, not done inline…
- counterpart: wu-execution.md › Post-WU review
- relation: narrowed
- note: B restates that statement changes are batched at the end of the review (handled as iterations), but the description of inline "enrichment-mode WU spec additions" reflects the superseded mechanism — enrichment now writes to a corpus spec pool, not inline WU-spec text, and B has no explicit "sweep the hypothesis set" reasoning.

## unit-041
- section: ## Session routing
- quote: **Standard outputs**: corrected synthesis report (challenge mode), enriched downstream WU specs (enrichment mode)…
- counterpart: wu-execution.md › Post-WU review
- relation: narrowed
- note: B has no consolidated "standard outputs" list for post-WU review; the individual items map to changed mechanisms — corrected artifacts re-enter as candidates via the referee rather than being directly edited, and enrichment writes to a spec pool rather than a WU spec.

## unit-042
- section: ## Session routing
- quote: **Ad hoc conversation** (Brian asks about the framework, discusses ideas): Read the hypothesis index for orientation…
- counterpart: SKILL.md › Session routing (table row "Ad hoc conversation about the framework")
- relation: narrowed
- note: B keeps reading hypothesis files as the conversation touches them and offering to create a file when the new-hypothesis criteria are met, but drops the explicit instruction to read the hypothesis index for orientation first.
