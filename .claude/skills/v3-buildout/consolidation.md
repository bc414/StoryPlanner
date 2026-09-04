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
