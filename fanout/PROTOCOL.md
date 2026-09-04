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
