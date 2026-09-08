# verifying-a-corpus

Enables writing-candidates-from-verification.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| verification-run | session | generator runner tallier | studies items items-manifest codebook calibration results | jobs ledger tally-output run-page | specified | On Brian's registered go: generate one job per item under the calibrated hash, dry run, batch under the host, tally the results; no pilot, calibration was it |
| verification-judge | agent | | codebook items | results | specified | A classifier or auditor applies the codebook's frozen predicate to one item and emits its label in the output contract's form; the only writer of results |
| verification-write | session | | results tally-output ledger run-page question-list | verification-artifact | specified | Writes verification.md: method, the questions the verification's items and predicates cover, counts from the tally, and any way the codebook was found wanting, for promotion to raise |

## Preconditions

The study is in the registry with Brian's go. The corpus's codebook at its current
version has a calibration at its hash, and its items and manifest exist from
preparing-to-verify-a-corpus. Every open question the verification is to answer is
covered by a predicate in that codebook; a question it does not cover is not this
verification's and goes back through preparing.

## verification-run

One run folder under `fanout/<study>/<run>/`. The generator writes one job per item
from the manifest, each carrying the codebook at its calibrated hash, the item, the output
contract's markers and neutral names; the dry run composes and sizes every prompt and
launches nothing; the batch runs under the host per the `agent-runner` skill, and the
tallier reduces `results/` to `tally.md`. No pilot job: the codebook's calibration was its
pilot. A later pass under the same hash over new items is a new run folder, not an edit.

## verification-judge

Instructed by the corpus's codebook and nothing else; Sonnet by default; tools Read and
Write; no MCP, since the item was pre-fetched by the itemizer. One item in, one labelled
result out in the contract's form. An item the codebook cannot classify is labelled with
the class the codebook reserves for that, never left blank.

## verification-write

The session writes `docs/v3-framework/<study>/verification.md`: the method (codebook id
and hash, calibration, itemizer and item count, generator, models, harness, run
folders and ledgers, what was not measured); the questions answered, each cited by id, being
those whose predicates the codebook froze and whose items the runs covered; the counts
from `tally.md`, each table citing its tallier and run. Per-item results are cited, not
copied. Where the tally shows the codebook wanting — a class the items keep falling
outside, a criterion the results split on — the session records it in `verification.md`
§ Corrections as a fact about the verification; the question it raises is Brian's, in the
promotion session.

## Never

Revises the codebook after the batch has started (a revision is a new version, a new
calibration and a re-run); writes a candidate, a falsifier or a question; runs a job under
a hash with no calibration; reads an intermediate analysis in place of the item.
