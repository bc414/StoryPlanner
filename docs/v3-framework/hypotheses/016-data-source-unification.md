## Hypothesis

The corpora's bespoke per-source schemas for conversations, lineage, code sessions, and
source texts — each shaped by whatever harness and constraints existed when it was built —
would be easier for another harness or model to consume if unified under a common API
standard modeled on the conversations API than the bespoke schemas are today.

## Origin

- date: 2026-08-31
- reasoning: Captured in Brian's pre-2026-08-30 Google Keep notes, during a review of his
  own hypotheses about model and harness choice for v3 ("I think I need to review the
  hypotheses again about what v3 and the task ahead is, since some premises changed based
  on evidence"). His observation: each corpus — conversations, lineage, code sessions,
  source texts — has its own bespoke schema, shaped by "whatever Claude code decided at the
  time of build" rather than by a shared standard. His assertion, tentatively framed as his
  notes typically are: "Maybe I need to unify my conversations, lineage, all those data
  sources under the conversations API standard instead of each one being bespoke... Then it
  should be easier for other harnesses and models to use them?" The motivation was
  portability — elsewhere in the same notes he was weighing model and harness choice itself
  as unsettled, and a shared schema across the corpora would keep the data usable
  regardless of which model or harness ends up reading it.

## Record
