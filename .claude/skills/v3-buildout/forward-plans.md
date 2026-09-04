# Forward plans

Read in full before creating, revising or retiring a forward plan. A forward plan is a
**snapshot experimental agenda**: born from the hypothesis landscape as it stands,
guiding work for a period, retired when consolidation or findings change the landscape.

## Numbering and lifecycle

`docs/v3-framework/forward-plan-N.md`; the active plan is the highest number. A
consolidation report without a matching next-numbered plan means a plan is pending.
Retirement is a header stamp on the old file — date, successor, reason — and the retired
plan becomes reference: its unexecuted WUs are retired proposals, not evidence.

Two creation triggers. **After consolidation** (mandatory): the hypothesis set changed
shape and every reference is stale. **After a priority reassessment** (lighter): the set is
unchanged but findings or a methodology revision changed what should be run — the same
hypotheses, a rewritten agenda.

## Structure of the agenda

The plan is comprehensive — every hypothesis is targeted by at least one WU — and it is a
**catalog found by id, not a schedule**. Three sections of work units:

1. **Corpus pairs.** For each corpus: an exploratory WU and a verification WU. The exploratory card
   names the reading protocol, the arms, and the corpus; the verification card names the spec pool it
   consumes, the codebooks it needs (existing hash or "to be written and calibrated"), and
   the hypotheses its pool's questions bear on. A verification card's hypothesis list grows as its
   pool grows; scope reconciliation keeps it current.
2. **Synthesis WUs.** Comparison, retrospective, adjudication, evaluation, connection —
   each naming the corpora it consumes, and therefore the verification passes it waits on.
3. **Infrastructure WUs** where the plan needs one: e.g. an instrument to build, a corpus
   to ingest, a codebook family to calibrate.

A revision may add a fourth kind; the card's `Type` field is where it would show.

Each card:

```markdown
### WU N.M: <title>

**Type:** exploratory | verification | synthesis | infrastructure
**Corpus:** <one corpus for exploratory and verification; the consumed corpora for synthesis>
**Question:** <what it asks>
**Hypotheses:** <ids — for a verification card, "per spec pool <corpus>.md" and no id list>
**Evidence sources:** <what is read or run>
**Codebooks:** <names and hashes, or "to calibrate"> (verification only)
**Scope:** <what it does and does not do>
**Scale:** <matrix cell(s); arms; approximate job count>
**Preconditions:** <tooling or Brian-action blockers only — never WU dependencies>
**Status:** proposed | scoped | in-progress | complete
```

Hypothesis references live here and in spec pools, never in hypothesis files. WU numbers
are `<plan>.<unit>`: the major number is the plan's, the minor counts from 1 within it.
Cards are listed in id order. The hypothesis index's tiers (A–J) are comprehension order
for reading the index, never execution order or scoping boundaries — a corpus pair informs
hypotheses across tiers. **A hypothesis-to-corpus edge is authored in exactly one place: a
pool entry's `bears-on` line.** A plan does not copy those ids — not onto verification
cards, not into a per-hypothesis coverage table. There is no generator, so any copy is a
second source that drifts the moment a pool grows (forward-plan-2's table was 13 rows
behind the pools on the day it was written, 2026-09-04, and was removed). A plan may
state the derivation (which pool feeds which card) and the grep that answers a row. The
one legitimate snapshot is a verification round's own run record under `fanout/`, which
freezes the entries that round answered.

## Ordering is structural, not derived

There is no global execution sequence to derive, and no ordering audit. Order follows from
the card types:

- **Exploratory passes consume no other WU** — they read a raw corpus — and are unordered
  among themselves. Which exploratory pass runs first is Brian's choice; the corpus he is working
  in is a legitimate reason (that is judgment, not convenience). Three advisory heuristics
  only, never rules: infrastructure hypotheses early (a claim about the experimental
  infrastructure itself — e.g. a source's existence, a contamination, a separability
  assumption — changes how later work is designed or read); passes whose questions
  would feed many pools early; and foundation before application (a pass that bears on
  the premises other hypotheses rest on before one that bears on the claims built on
  them — not blocking, but findings on an untested foundation are harder to read).
- **Verification passes are triggered, and run in rounds.** A verification round on c is due when its spec pool
  holds questions with a calibrated codebook; a later round is due when the pool has grown
  enough to be worth a batch. A round's card records which pool entries it answered.
- **Synthesis WUs wait on verification debt.** A synthesis runs only when every corpus it
  consumes has a verification round covering the questions it relies on. This is the one
  hard edge in the plan, and it is readable off the cards' `Type` and `Corpus` fields —
  nothing is judged.

Never ordering inputs: readiness, convenience, throughput, estimated duration.
Preconditions gate timing in place, never position (a skill to build or an ingest to run
is the first task *inside* the WU that needs it).

The plan's execution section is a **status board, not a sequence**: per corpus, exploratory pass done or
not, verification rounds run, open pool questions; per synthesis, which debts still block it. It
summarises the cards' `Status` fields in the same file and is updated by hand in the same commit
as the card it summarises; it points at each pool rather than counting its questions.

Hypothesis status is advice for choosing among independent exploratory passes: `challenged` first
(the pool question that would resolve it), `untested` next, `evidenced` for stress-testing
from a new angle, `baselined` only if a challenge appears.

## Writing the plan

Start from the landscape: read the full index and every hypothesis file — and the
consolidation report, when one just happened — treating any
hypothesis whose entries lack referee lines as unverified (`evidence-pipeline.md` § Entries
this pipeline did not produce).
For each hypothesis or cluster ask what verified evidence would move it, which corpus
could supply it, and whether the question already sits in a spec pool. Group by corpus.
Prior plans and syntheses are reference for what was tried, not templates; a plan that
reads as a renumbering has not engaged with the landscape.

The rationale section is written once and can be long — at least: what the last
consolidation, revision or reassessment showed, what the landscape looks like, why this
shape, and whatever else a later session would need to understand the reasoning. The plan
is expected to be a long document — enough detail per card for a plan-mode session to
scope execution, and not more.
