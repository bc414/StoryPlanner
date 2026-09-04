---
id: 47
status: untested
baselined: false
created: 2026-09-03
---

## Hypothesis

Pathfinder, slice-reader and classifier passes over the same material miss different
things — the pathfinder misses locally (attention dilution over a long context), slice
readers miss globally (patterns spanning slices), classifiers miss contextually (no
surrounding knowledge) — so their disagreements are mostly disjoint by locus, and a union
of passes catches more than any single pass at higher effort.

## Record

- created | 2026-09-03T16:00: Originated from Brian's statement (2026-09-02): the
  long-context single agent "gives a starting point and holistic analysis that may not be
  deep but has seen everything"; the mid tier of smaller-corpus agents "produces
  intermediate artifacts that are more credible on deepness/correctness for their smaller
  slice"; a third, granular tier does "narrow single step analysis" that a smaller model can
  do. The prediction that follows — that the three kinds of reader fail in *different
  places*, which is what would make combining them worth the cost — was drawn out in the
  same conversation and Brian endorsed the statement on 2026-09-03. WU1.4's redesign
  (two blind reading conditions, adjudicated by binned disagreements) already assumes
  something like this; this file makes it a claim the bin counts can refute.
