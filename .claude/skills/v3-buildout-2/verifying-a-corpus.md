# verifying-a-corpus

Enables writing-candidates-from-verification.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| verification-run | session | runner tool-source | studies directions calibration results | definition index items calls tally | specified | On Brian's registered go: the full batch under the accepted version, its definition written and its items cut by the itemizer; dry-run-batch, execute-batch under the host, tally-batch; no pilot, the calibration was it |
| verification-judge | agent | | directions items | results | specified | A call applies the directions to one item and answers in the fields they declare, an item the criteria cannot place going to the class reserved for it; the only writer of results |
| verification-write | session | | definition index calls results tally question-list | verification-artifact | specified | Writes verification.md: method, the questions the directions froze, counts from the tally, and any way the directions were found wanting, for promotion to raise |

## Preconditions

The study is in the registry with Brian's go. The directions version the batch will name
has an accepting calibration at its body hash, and the itemizer that cut the calibration
sample exists as a tool. Every open question the verification is to answer is named in
that version's frontmatter; a question it does not cover is not this verification's and
goes back through preparing.

## verification-run

One batch folder under the study, `batches/<nn>-<slug>/`, its definition of kind full
naming the accepted version by path and the calibration that accepted it, the study's
model and effort, and any tool the calls opt into. The itemizer runs once into the folder,
writing the index and the item bodies. Then, per the `agent-runner` skill: dry-run-batch,
which checks the definition, the directions, the index and every item in memory and writes
nothing; execute-batch, one call per item under the host; tally-batch once every item has
a result, which writes the tally. No pilot: the calibration was it. An item still without
a result after an execution is called again by a later execution of the same batch;
nothing about the batch changes.

## verification-judge

Instructed by the directions body as its system prompt and nothing else; the item's text
as its message; the study's model and effort; no tools and no MCP unless the definition
opts in, since the itemizer put into the item everything the call needs. One item in, one
answer out in the declared fields, which the runner renders as the result. An item the
criteria cannot place is put in the class the directions reserve for that, never left
blank.

## verification-write

The session writes `verification.md` under the study: the method (the directions version
and body hash, its calibration, the itemizer and item count, the model and effort, the
harness version from the calls, the batch, what was not measured); the questions answered,
each cited by id, being those the version's frontmatter names and whose items the batch
covered; the counts from the tally, each table citing the batch. Per-item results are
cited as `<study>/<batch>/<item>`, not copied. Where the tally shows the directions
wanting — a class the items keep falling outside, a criterion the results split on — the
session writes it in `verification.md` § Corrections as a fact about the verification; the
question it raises is Brian's, in the promotion session.

## Never

Revises the directions after the batch has started (a revision is a new version, a new
calibration and a new batch); writes a candidate, a falsifier or a question; executes a
batch whose definition names a version with no accepting calibration; reads an
intermediate analysis in place of the item.
