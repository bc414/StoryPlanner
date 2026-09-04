# Handoff: the retroactive referee pass

Written 2026-09-03 for a future session. One-time work: re-verify every hypothesis
evidence entry deposited before methodology revision 1 through the revision-1 pipeline. It
is a **verification WU** and belongs on a forward-plan-2 card; it does not have to precede
plan 2 — plan 2 treats all pre-revision statuses as unverified and seeds spec pools from
the existing records itself.

Read first, in full: `v3-buildout` SKILL.md, `evidence-pipeline.md`,
`hypothesis-records.md`, `fanout/referee/codebook.md`, and
`methodology-revision-1.md` (why this pass exists).

## Why

Pre-revision entries were deposited by the session that searched for the pattern, with
the "would differ if false" clause self-administered, and several came from exploratory
passes (WU1.1, WU1.3 among them) that the strong form no longer recognises as evidence.
Hypothesis 048 predicts a blind re-check demotes a non-trivial share; this pass is its
test as well as the cleanup.

## Sequence

1. **Calibrate the referee codebook first.** `fanout/referee/codebook.md` is a draft with no
   calibration record. Draw a sample (≥ 20 entries, spanning the three classes and ≥ 3
   hypotheses — the pre-revision entries themselves are a fine source), run the referee
   on it blind, score the same sample with Brian, adjudicate, record rulings in
   `fanout/referee/calibration-<date>.md`, re-hash. No batch before this exists.
2. **Build the candidates file** — `fanout/WU2.<n>-retroactive-referee/candidates.md`,
   the WU's own record. One candidate per existing evidence entry: `finding` and `source`
   copied from the entry; `proposed-by: retroactive / <original WU> / <original
   timestamp>`; target = the hypothesis it sits in. Where the entry cites only an
   intermediate analysis, fetch the analysis *and* mark the candidate for R2/R5 scrutiny.
3. **Run the referee** as a run under the referee's own work folder,
   `fanout/referee/<date>-retroactive/` (vertical by work: every referee run lives there,
   the candidates file stays with the WU). Per the lifecycle (`fanout/PROTOCOL.md`): the
   enumerator is a script beside the codebook (`fanout/referee/make-jobs.*`) that turns
   each candidate plus its statement into one item — one job, one candidate, one target —
   and writes the fetched source excerpts **outside `items/`** (e.g. `excerpts/C-014.md`),
   because they are not regenerable and with `prompt.md` gitignored the run folder is the
   only place they will exist; `requireOnce` carries the two verdict-line markers; the
   tallier (`fanout/referee/tally.*`) produces step 4's table; `run.md` names the sample,
   the codebook hash and the arms. Sonnet, `mcp: false`, tools Read/Write, codebook hash
   cited on every verdict; the referee never sees the entry's original clause. Pilot on
   three candidates with `--job` before the batch; the promotion session copies each
   verdict from `results/` into the WU's candidates file.
4. **Measure** — per candidate, does the blind clause name the same observable as the
   original clause? Same / narrower / different / original vacuous. This table is the
   WU's artifact and the evidence 048 needs; deposit it through the ordinary route (it is
   a finding of a verification pass, so it is itself a candidate).
5. **Promotion session with Brian.** Entries from exploratory WUs return to spec-pool
   status — a question in the relevant corpus's pool with provenance — pending that
   corpus's verification round; the strong form recognises no exception for work already
   done. Entries whose blind clause survives stand, re-promoted verbatim with the new
   referee line. The rest are declined with reasons. Old entries are never deleted:
   each gets `(superseded by re-referee <date>)` or stands as promoted. Statuses
   recompute from what remains; `INDEX.md` updated; **one commit**.
6. **Report**: counts per class, per originating WU, agreement between original and blind
   clauses, and anything the pipeline's first live run revealed about itself (job
   failures, codebook rulings, referee drift).

## Must not

Rewrite any finding; edit any tag in place; delete an entry; promote anything Brian has
not reviewed in the commit diff; run the batch under an uncalibrated codebook hash; treat
"consistent with the hypothesis" as diagnostic.
