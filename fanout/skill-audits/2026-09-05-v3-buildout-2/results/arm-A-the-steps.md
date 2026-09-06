## unit-161
- section: ## The steps
- quote: | 1 | **Question** | HITL session | an entry in `docs/v3-framework/spec-pools/<corpus>.md` | …
- counterpart: artifacts.md › Question entry
- relation: narrowed
- note: same gate (a per-corpus question, HITL-written, under `questions/<corpus>.md` not `spec-pools/<corpus>.md`) but B exempts the referee instance, whose question is "the pipeline's own" with no question-list entry required.

## unit-162
- section: ## The steps
- quote: | 2 | **Cell and WU type** | HITL | the WU card: `Type`, `Corpus`, `Scale` (= the matrix cell) | …
- counterpart: none
- relation: absent
- note: B has no Type×Corpus×Scale matrix cell deciding model/context/verification; instances.md registers only id, type, corpus and go, and scale/arms/verification are fixed separately in explore-plan or verify-plan, while models are fixed per-process defaults in the activity tables.

## unit-163
- section: ## The steps
- quote: | 3 | **Work folder** | HITL | `fanout/<work>/` — a WU id, or an action's name (`referee`, `skill-audits`) | …
- counterpart: agent-runner/SKILL.md › Layout: `fanout/`, vertical by work
- relation: restated
- note: —

## unit-164
- section: ## The steps
- quote: | 4 | **Instrument** | HITL | `codebook.md` or `protocol.md` in the work folder; for a codebook, a `calibration-<date>.md` beside it before any batch | …
- counterpart: SKILL.md › Constitutional rules (rule 4)
- relation: broadened
- note: same core rule (an instrument in the work folder, calibrated before any batch) but B additionally requires the file be versioned by number and hash, with a new calibration record for every revision.

## unit-165
- section: ## The steps
- quote: | 5 | **Enumerate** | a tool, once | `items/` + `manifest.md` when the items are regenerable; a committed folder (e.g. `excerpts/`) when they are not | …
- counterpart: agent-runner/SKILL.md › A well-formed job (rule 2) and Layout: what a run commits
- relation: restated
- note: —

## unit-166
- section: ## The steps
- quote: | 6 | **Generate** | `make-jobs.*` beside the instrument | `jobs.json`: one `item` per job, `requireOnce` markers, the run's ceilings, neutral arm names | …
- counterpart: agent-runner/SKILL.md › A well-formed job (rules 1 and 3) and The job file
- relation: restated
- note: —

## unit-167
- section: ## The steps
- quote: | 7 | **Dry run** | CLI, serverless | every prompt composed and sized; nothing launched | …
- counterpart: agent-runner/SKILL.md › commands table / A well-formed job (rule 4)
- relation: restated
- note: —

## unit-168
- section: ## The steps
- quote: | 8 | **Pilot** | CLI `--job <id>` → the host | one attempt, its output read by a person; its ledger row carries `Mode: pilot` | …
- counterpart: agent-runner/SKILL.md › A well-formed job (rule 4)
- relation: restated
- note: —

## unit-169
- section: ## The steps
- quote: | 9 | **Batch** | the host | `ledger.jsonl`, `attempts/`, `results/`; watched on the run's page and steered with harness knobs only; may be scheduled (`--at HH:mm` or `--at reset`) to run when the usage window resets | …
- counterpart: agent-runner/SKILL.md › The host and its page
- relation: restated
- note: —

## unit-170
- section: ## The steps
- quote: | 10 | **Tally** | `tally.*` beside the instrument | counts and flagged rows from `results/` | …
- counterpart: agent-runner/SKILL.md › conventions (tally) / Layout
- relation: restated
- note: —

## unit-171
- section: ## The steps
- quote: | 11 | **Verify and promote** | the referee (its own work folder), then a HITL promotion session — or, for a non-evidence action such as a skill audit, an adjudication document | candidates with verdicts; promoted entries in `docs/v3-framework/hypotheses/` | …
- counterpart: promoting-checked-candidates.md › promote
- relation: restated
- note: —

## unit-172
- section: ## The steps
- quote: | 12 | **Record** | HITL | `run.md` in the run folder; the run committed per the convention; the artifact in `docs/` citing the ledger row | …
- counterpart: agent-runner/SKILL.md › run.md / What a run commits / Citing a result
- relation: restated
- note: —
