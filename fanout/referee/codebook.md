# Codebook: referee — the discrimination test

Status: **draft, uncalibrated** (2026-09-03). Not to be applied in a batch until a
calibration record (`calibration-<date>.md` — that exact prefix: the runner's page detects
the calibrated stage by `calibration-*.md` beside the codebook) exists beside it with Brian's verdicts
on a scored sample. The runner hashes this file; every verdict cites `referee@<hash>`.

## What you are given

Exactly three inputs, and nothing else applies:

1. **Statement** — the hypothesis, as one to three sentences. You do not know its status,
   its record, or any other candidate against it.
2. **Candidate** — a `finding` (what a pass observed, with ids, counts or passages) and a
   `source` (what was read).
3. **Source excerpt** — the material the finding rests on, as text: a passage, notes, an
   artifact section, or another locus. You never search for more; if the excerpt does not
   contain what the finding claims, that is a verdict (see rule R5), not a reason to look.

You will never be shown a clause someone else wrote for this candidate. Do not guess at
one.

## What you produce

Exactly two lines, appended under the candidate, in this form and no other:

```
- clause: Would differ if false: <the observable>
- referee: <job id> / <model> / <ISO time> / codebook referee@<hash> / verdict <class> — <one-line reason>
```

`<class>` is one of the three classes below. Nothing else: no rewording of the finding, no
suggested better statement, no new hypothesis, no commentary beyond the one-line reason.

## The task

Attempt to complete this sentence about the **source excerpt**:

> If the statement were false, this source would have shown ___ instead.

The blank must be an **observable**: something the excerpt could have contained — a
different count, a different technique in a named passage, an absent pattern present, a
present pattern absent, a different label on a named item. It is never a paraphrase of
"the statement would be false" and never a description of the claim.

## Classes

| Class | Definition |
|---|---|
| `diagnostic [supporting]` | An observable can be named, and the excerpt shows the *statement-true* side of it. |
| `diagnostic [challenging]` | An observable can be named, and the excerpt shows the *statement-false* side of it. |
| `non-diagnostic` | No observable can be named — the excerpt would look the same whether the statement were true or false — or the observable named is not one the excerpt could decide (rules R3–R6). |

## Decision rules

- **R1 — Observable, not restatement.** A clause of the form "the evidence would not show
  this", "the pattern would be absent", "the claim would fail", or any wording that names
  no concrete content of the excerpt, is vacuous. Vacuous → `non-diagnostic`, reason
  "vacuous clause".
- **R2 — The excerpt decides, not the corpus at large.** The observable must be something
  *this* excerpt could have shown. If the only discriminating observable lives elsewhere
  (another story, a count over the whole corpus), the candidate is `non-diagnostic`,
  reason "observable not in excerpt" — the promotion session may fetch more and re-run.
- **R3 — Named items must each be addressed.** If the statement names specific items
  (four techniques, three tracks, two classes) and the excerpt bears on some of them, the
  clause names the observable per item it bears on. Any named item the excerpt
  *contradicts* makes the verdict `diagnostic [challenging]` regardless of the overall
  impression.
- **R4 — Consistent-with is not evidence-for.** An excerpt that is compatible with the
  statement but equally compatible with its negation is `non-diagnostic`, reason
  "consistent with both". Thematic relatedness, shared vocabulary, and "this is the kind
  of thing the statement is about" are all this case.
- **R5 — The finding must be in the excerpt.** If the finding asserts something the
  excerpt does not contain (a count the excerpt cannot support, a passage not present, a
  classification the text does not bear), the verdict is `non-diagnostic`, reason "finding
  not supported by excerpt". You are not judging whether the finding is *true elsewhere*.
- **R6 — Classification findings need the register visible.** When the finding is a
  classification (FID vs DT vs blend, mechanism level, obstacle type, perspective mode),
  the surrounding clauses in the excerpt decide whose register a sentence is in. If the
  excerpt is too narrow to show that, R2 applies.
- **R7 — Over-flag.** When genuinely torn between `diagnostic` and `non-diagnostic`,
  choose `non-diagnostic` and say why in the reason. A false non-diagnostic costs one
  adjudication; a false diagnostic costs the record.
- **R8 — Side is decided by the clause.** Once an observable is named, the tag follows
  mechanically from which side the excerpt shows. Do not soften a `challenging` verdict
  because the statement is plausible, and do not promote a `supporting` verdict because
  the finding is impressive.
- **R9 — One target.** You are judging one candidate against one statement. If the
  finding seems to bear on a different claim, ignore that; it is not your job.

## Worked examples

**E1 — diagnostic [supporting].**
Statement: *Perception gaps are delivered by at least four mechanisms besides free
indirect discourse.*
Finding: across the seven analyses in the excerpt, five name DT-based knowledge asymmetry,
three name dual-POV dramatic irony, two name strategic opacity, two name narrated denial;
FID is primary in one.
Clause: *Would differ if false: the seven analyses would name FID as the sole or dominant
delivery mechanism, with at most one non-FID mechanism appearing.*
Verdict: `diagnostic [supporting]` — four non-FID mechanisms named across the set; the
statement-false side would be one or none.

**E2 — diagnostic [challenging].**
Statement: *Brian's bonded stories use structural obstacles as the primary barrier.*
Finding: in the excerpt (the THLB and GIYC obstacle sections), both analyses classify the
primary barrier as characterological, with structural obstacles secondary.
Clause: *Would differ if false: the obstacle sections would classify the primary barrier
as characterological rather than structural.* That is what the excerpt shows.
Verdict: `diagnostic [challenging]` — both named texts show the statement-false side.

**E3 — non-diagnostic (vacuous).**
Statement: *Comedy is placed at structural positions rather than used as a genre.*
Finding: the analyses discuss comedy at length.
Attempted clause: *Would differ if false: the analyses would not discuss comedy at
structural positions.* — names no content; a restatement.
Verdict: `non-diagnostic` — vacuous clause (R1). The excerpt would contain discussion of
comedy either way.

**E4 — non-diagnostic (consistent with both).**
Statement: *Variable focalization, not deep third, is the master perspective principle.*
Finding: the excerpt's story alternates two first-person narrators.
Attempted clause: none nameable — alternating first person is compatible with the
statement and with a deep-third master principle (it is neither).
Verdict: `non-diagnostic` — consistent with both (R4).

**E5 — non-diagnostic (observable not in excerpt).**
Statement: *DT is the primary interiority technique in the majority of the corpus.*
Finding: this story uses DT throughout.
Clause attempt: the observable is a majority count over the corpus; one story cannot show
either side.
Verdict: `non-diagnostic` — observable not in excerpt (R2); promotion may re-run with the
corpus-level table as the excerpt.

## Calibration (required before first use)

A sample of at least twenty candidates spanning the three classes and at least three
hypotheses, scored blind by the referee and independently by Brian; disagreements
adjudicated; each ruling that changes this file is a new hash. The calibration record
lists sample ids, both verdicts, the agreement rate per class, and the rulings. The
retroactive pass may begin only after this record exists.
