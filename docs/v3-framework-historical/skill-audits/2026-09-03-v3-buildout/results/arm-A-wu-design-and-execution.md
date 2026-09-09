## unit-131
- section: ### WU design and execution
- quote: Every WU runs four phases in the same session:…
- counterpart: wu-execution.md › The four phases
- relation: restated
- note: —

## unit-132
- section: ### WU design and execution
- quote: Scope reconciliation phase (auto mode, before plan mode):
- counterpart: wu-execution.md › The four phases (phase 1, Scope reconciliation)
- relation: restated
- note: —

## unit-133
- section: ### WU design and execution
- quote: Read the active forward plan to find the WU spec and its target hypothesis list.…
- counterpart: wu-execution.md › The four phases (phase 1 table, per-type reconciliation)
- relation: narrowed
- note: B keeps clerical metadata reconciliation (hypotheses, scale, type, corpus brought in line with the WU's actual content) but drives hypothesis-list updates per WU type via spec-pool questions (verification only) rather than "prior WU post-review testing specs"; exploratory WUs no longer recompute a hypothesis list at all ("informational only"), and the per-hypothesis coverage table is now system-regenerated rather than a clerical hand-edit.

## unit-134
- section: ### WU design and execution
- quote: Plan-mode phase:
- counterpart: wu-execution.md › The four phases (phase 2, Plan mode)
- relation: non-instructional
- note: bare phase heading; carries no instruction, fact or definition beyond naming the phase that follows.

## unit-135
- section: ### WU design and execution
- quote: Read the evidence sources…
- counterpart: wu-execution.md › The four phases (phase 2, "Read or size the sources")
- relation: narrowed
- note: "read or size the sources" persists, but B replaces "plan subagent extraction" for oversized sources with mandatory slice-reader arms under a shared protocol (wu-execution.md › Design rules for all types: "never … one reader with compaction"); B does not restate the "already read during scope reconciliation, re-read only if the hypothesis list changed" bookkeeping.

## unit-136
- section: ### WU design and execution
- quote: Identify all open questions.…
- counterpart: wu-execution.md › The four phases (phase 2, Plan mode)
- relation: restated
- note: —

## unit-137
- section: ### WU design and execution
- quote: Ask all open questions.…
- counterpart: wu-execution.md › The four phases (phase 2, Plan mode)
- relation: restated
- note: —

## unit-138
- section: ### WU design and execution
- quote: Write the plan.…
- counterpart: wu-execution.md › The four phases (phase 2, what the plan covers)
- relation: narrowed
- note: the plan-contents list carries over (read order, artifact coverage, what the WU does not do) but "how evidence deposit works" is no longer a uniform plan item — B's strong-form pipeline confines evidence writing to a verification WU's candidate/referee/promotion flow, and the corresponding plan-contents items are type-specific (arms/explicit contexts, codebooks and job files, verified-input debt status, acceptance criteria).

## unit-139
- section: ### WU design and execution
- quote: Exit plan mode.…
- counterpart: wu-execution.md › The four phases (phase 2, Plan mode)
- relation: restated
- note: —

## unit-140
- section: ### WU design and execution
- quote: Execution phase:
- counterpart: wu-execution.md › The four phases (phase 3, Execution)
- relation: non-instructional
- note: bare phase heading; carries no instruction, fact or definition beyond naming the phase that follows.

## unit-141
- section: ### WU design and execution
- quote: Primary work: Execute the experiment focused on target hypotheses.…
- counterpart: evidence-pipeline.md › Where each kind of write goes
- relation: reversed
- note: B forbids a WU from depositing evidence entries into hypothesis files directly — a WU may write only a *candidate* (verification type only), which a referee checks and a separate HITL promotion session (Brian) promotes into the hypothesis record; "batch deposit after synthesis" has no counterpart under this pipeline. The forward-plan status update (in-progress → complete) is the one part that is retained (wu-execution.md › The four phases).

## unit-142
- section: ### WU design and execution
- quote: Wrap-up step: After primary work, read the full hypothesis index.…
- counterpart: wu-execution.md › The four phases (execution table, wrap-up sweep column)
- relation: reversed
- note: B forbids depositing evidence entries at wrap-up for any WU type — wrap-up yields more candidates through the same referee (verification only) or spec-pool questions (exploratory/synthesis/infrastructure), "never a side door" into hypotheses/.

## unit-143
- section: ### WU design and execution
- quote: Then report the tag counts to Brian: targets, entries written, supporting, challenging, and targets where no entry was written because the findings did not discriminate.…
- counterpart: evidence-pipeline.md › Promotion
- relation: narrowed
- note: the report-then-Brian-decides-on-recheck pattern survives, but it now happens after the promotion session's commit rather than at WU wrap-up, and the tag vocabulary changes (candidates per target, diagnostic/non-diagnostic/held, promoted supporting/challenging, declined, referee disagreements) rather than "entries written / no entry written because findings did not discriminate."

## unit-144
- section: ### WU design and execution
- quote: New hypothesis detection during WUs: During primary work, note unexpected observations but hold new-hypothesis proposals for the wrap-up step.…
- counterpart: wu-execution.md › The four phases (end of Execution); hypothesis-records.md › Creating a hypothesis
- relation: restated
- note: —

## unit-145
- section: ### WU design and execution
- quote: WU artifacts: WU output (reports, structured data, analyses) goes in `docs/v3-framework/` with a WU-prefixed filename (e.g., `WU1.1-corpus-synthesis.md`).…
- counterpart: wu-execution.md › WU artifacts
- relation: restated
- note: —
