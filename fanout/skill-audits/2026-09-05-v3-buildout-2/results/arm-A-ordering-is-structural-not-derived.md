## unit-140
- section: ## Ordering is structural, not derived
- quote: There is no global execution sequence to derive, and no ordering audit.…
- counterpart: SKILL.md › Schema — the tables every file in this skill obeys
- relation: broadened
- note: Card-type-driven ordering is replaced by a derived dependency graph (reads/writes, the router's `enables` graph) and a per-activity Preconditions section covering every process, not just per-corpus card types.

## unit-141
- section: ## Ordering is structural, not derived
- quote: Exploratory passes consume no other WU — they read a raw corpus — and are unordered among themselves.…
- counterpart: SKILL.md › Provenance
- relation: narrowed
- note: B keeps "the pick is Brian's" for what runs next but drops the three advisory heuristics (infrastructure-first, feeds-many-pools, foundation-before-application); no ordering advice for exploratory instances is stated anywhere.

## unit-142
- section: ## Ordering is structural, not derived
- quote: Verification passes are triggered, and run in rounds.…
- counterpart: conducting-a-verification-round.md › Preconditions
- relation: narrowed
- note: B keeps "a round runs once open questions are covered by a calibrated codebook" but drops the growth-threshold criterion for when a later round becomes "due"; that decision is Brian's, made in verify-plan, and no trigger rule is stated.

## unit-143
- section: ## Ordering is structural, not derived
- quote: Synthesis WUs wait on verification debt.…
- counterpart: SKILL.md › Constitutional rules (rule 3, Verification debt)
- relation: broadened
- note: There is no "synthesis" type or card `Type`/`Corpus` fields; the gate now applies at several points instead of one hard edge — leads cannot be cited as evidence until verified (rule 3), a candidate needs a calibrated, referee-checked line before promotion, and a hypothesis needs a current-wording evidence entry before baselining.

## unit-144
- section: ## Ordering is structural, not derived
- quote: Never ordering inputs: readiness, convenience, throughput, estimated duration.…
- counterpart: building-a-tool.md › build
- relation: restated
- note: —

## unit-145
- section: ## Ordering is structural, not derived
- quote: The plan's execution section is a status board, not a sequence…
- counterpart: SKILL.md › Provenance / artifacts.md › state
- relation: reversed
- note: The status-board function carries over (state.md summarises per-instance, per-corpus and per-hypothesis status), but its authoring mechanism inverts: state.md is a generated section, never hand-edited, where the unit's board was updated by hand in the same commit as the card it summarised.

## unit-146
- section: ## Ordering is structural, not derived
- quote: Hypothesis status is advice for choosing among independent exploratory passes: `challenged` first…
- counterpart: none
- relation: absent
- note: B tracks hypothesis status (`untested`/`evidenced`/`challenged`) but never uses it to prioritize among exploratory instances; state.md just reports status and "the pick is Brian's" with no stated heuristic.
