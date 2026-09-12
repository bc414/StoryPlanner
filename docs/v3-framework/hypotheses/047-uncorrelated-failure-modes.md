## Hypothesis

Pathfinder, slice-reader and classifier passes over the same material miss different
things — the pathfinder misses locally (attention dilution over a long context), slice
readers miss globally (patterns spanning slices), classifiers miss contextually (no
surrounding knowledge) — so their disagreements are mostly disjoint by locus, and a
union of passes catches more than any single pass at higher effort.

## Origin

- date: 2026-09-03
- reasoning: Raised by Brian on 2026-09-02, while thinking through which models to
  assign to which parts of the v3 buildout. He noted that WU1.4-execution-plan.md
  already calls out a failure mode of a single agent over long context missing
  nuance, and that running a parallel experiment of different agents on smaller
  subsets and combining their synthesis documents has its own failure mode — lossy
  data during synthesis — which he judged acceptable. His assertion: the
  long-context single agent "gives a starting point and holistic analysis that may
  not be deep but has seen everything"; the mid tier of smaller-corpus agents
  "produces intermediate artifacts that are more credible on deepness/correctness
  for their smaller slice"; and a third, granular tier does narrow single-step
  analysis that a smaller model can do. On 2026-09-03 this was drawn out into a
  candidate prediction — that the three kinds of reader fail in different places
  rather than the same ones — which Brian endorsed by approving it for minting
  alongside four other candidates.

## Record
