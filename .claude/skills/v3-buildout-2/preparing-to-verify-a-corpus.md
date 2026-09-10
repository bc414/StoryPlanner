# preparing-to-verify-a-corpus

Enables verifying-a-corpus and refereeing-candidates.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| verify-plan | hitl | | question-list corpora corpus state skill | studies question-list | specified | Brian and the session fix what one item is, which questions the directions will freeze, the model and effort, and what the calibration sample spans; the plan approved is his go and the study is registered; a repeat under unchanged directions is the whole activity |
| itemize | session | dotnet tool-source | corpus | index items | specified | Build or pick the itemizer tool with tests and run it once into the calibration batch: the index and the item bodies |
| author-directions | hitl | | question-list items directions | directions | specified | The directions written against real items with Brian: what the call is given, the classes, the criteria, what to produce; a new numbered version each time |
| assemble-sample-batch | session | runner | directions index | definition calls tally | specified | The calibration batch, kind sample: its definition written under the draft version; dry-run-batch; execute-batch as the hand-off, after which the host calls every sample item and writes the tally; this is the directions' pilot, and its results are withheld from Brian until he has scored |
| assess-sample-items | agent | | directions items | results | specified | One call per sample item under the draft directions, the answer in the declared fields; the only writer of the sample's results |
| calibrate | hitl | | index items results tally directions | calibration directions | specified | Brian scores the sample blind; the two scorings are compared against the tally; he rules each disagreement; a ruling that changes a criterion is a new version; the calibration is written, accepting or not |

## Preconditions

The corpus is readable and CORPORA.md says how. Its question list holds open questions
whose answers a frozen predicate could give. For the referee: the directions and
calibrations are the method's one set, in the referee folder, prepared here once and again
only when a ruling in promotion changes them; the corpus is the candidates of every
verification, the itemizer is the referee's materialising tool, the question is the
pipeline's own (does this finding discriminate for this statement?), the sample spans
several hypotheses and verifications, and the activity stops after `calibrate`: there is
no verification of its own.

## verify-plan

The session presents the open questions and the corpus's shape as CORPORA.md gives
it, and asks Brian, batched four per call, what one item is for this corpus, which
questions the directions will freeze, the model and effort, and what the calibration
sample should span. It writes the plan naming those, the itemizer to build or reuse, and
what the verification will not do; the plan is written against the chain's activity files,
from verifying-a-corpus to promoting-refereed-candidates, read whole here rather than each
at its own start, since it names what each of them will do for this study. Brian approves.
The session appends the study to `studies.md` with the date as his go, and writes any
question he raised here into the list.

For a repeat verification under unchanged directions — the current version has an
accepting calibration at its hash and names every question the verification is to answer,
and the corpus is current — this is the whole activity: the go is registered and
verifying-a-corpus follows. Anything the directions do not cover runs the rest.

## itemize

The itemizer is a tool under `tools/`, code with tests under the `testing` skill, built or
picked here. It runs once into the calibration batch's folder, `batches/<nn>-<slug>/`,
writing the index in its schema's shape, with the corpus and the locator notation in its
head, and the item bodies under `items/`. What one item is, its grain, is decided here and
nowhere else, and the directions' first section states it, so the two are written
together. An itemizer never selects by judgment; an authored query in Brian's vocabulary
is the only narrowing it may do, and the query is stated in the index's head.

## author-directions

Written against the real items with Brian: what the call is given and where the item
stops; the classes, each a label and what an item shows, one reserved for an item the
criteria cannot place; the criteria at the boundaries, stated generally and tuned to
over-flag; what to produce, one keyed line per field; what never to do. The frontmatter
cites by token the questions the version freezes. A question's suggested test is a naive
starting point and never a criterion: the criteria are authored against the items in front
of Brian, and a suggested test is never carried into the directions unexamined. A new
numbered file under the study, or in the referee folder; the previous version, if any,
stays on disk.

## assemble-sample-batch

The sample of the items, drawn as the plan said, stratified by expected class, with a
held-out split named in advance, is the calibration batch's index, cut by the itemizer.
Its definition is of kind sample, naming the draft version by path and no calibration. Per
the `agent-runner` skill: dry-run-batch, then execute-batch, the hand-off; the host calls
every sample item and writes the tally when the last has a result. This is the directions'
pilot. The results are withheld from Brian until he has scored.

## assess-sample-items

Instructed by the draft directions body as its system prompt and nothing else; one sample
item in, one answer out in the declared fields.

## calibrate

Brian scores each sample item blind, in whatever order the session presents them, and the
session writes his verdicts as given. Then the two scorings are laid side by side: per
class agreement on the ruled items and on the held-out items separately. For each
disagreement Brian rules; a ruling that changes a criterion edits the directions into a
new version, the ruling stated generally as a criterion, the item it came from staying in
the calibration file. The session writes the calibration beside the version it judged,
titled with that version's body hash and citing the batch by token. If any ruling changed
the criteria, assemble-sample-batch repeats on the sample under the new version, a new batch; when
Brian accepts the agreement, the calibration's verdict says so, and the version is
calibrated at that hash. Nothing in the directions says so, since any line of the body is
part of the hash.

## Never

Executes a full batch under a version with no accepting calibration; shows Brian the
agent's answers before his own are written; lets the itemizer narrow by judgment; authors
directions without items in front of it; writes a candidate; carries a calibration ruling
into the directions as the item it came from.
