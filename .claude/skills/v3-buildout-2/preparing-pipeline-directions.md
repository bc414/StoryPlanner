# preparing-pipeline-directions

Enables surfacing-candidates and iterating-a-statement.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| plan-pipeline-directions | hitl | | state directions calibration declined-candidates findings results hypothesis-statement | definition calibration directions | specified | For the set that started it, the referee's or claiming's: what started it, the current version and its calibration presented; Brian fixes the model and effort, what the sample draws from with its strata and held-out split, and for a revision the rulings the new version must answer, each recorded in the file it governs; his approval is the go |
| collate-pipeline-sample | session | dotnet tool-source | findings results hypothesis-statement | index items | specified | The collator run once into a calibration batch in the set's folder: the index and the item bodies of the sample the plan fixed |
| author-pipeline-directions | hitl | | items directions calibration | directions | specified | A new numbered version written with Brian against the collated items: what the call is given, the criteria, what to produce, and for the referee its classes; for a revision, each ruling the plan named answered as a criterion |
| assemble-pipeline-sample-batch | session | runner | directions index | definition calls tally | specified | The calibration batch, kind sample: its definition under the draft version with the plan's model and effort; dry-run-batch; execute-batch as the hand-off, after which the host calls every sample item and writes the tally; its results withheld from Brian until he has scored |
| assess-pipeline-sample-items | agent | | directions items | results | specified | One call per sample item under the draft directions, the answer in the declared fields; the only writer of the sample's results |
| calibrate-pipeline-directions | hitl | | index items results tally directions | calibration directions | specified | Brian scores the sample blind; agreement read per class for the referee and per (finding, hypothesis) pair for claiming, recall deciding claiming's acceptance; a ruling that changes a criterion is a new version; the calibration written, accepting or not |

## Preconditions

What started it names one set, the referee's or claiming's: the set's first preparation, at
Brian's approval; for the referee, his ruling at promotion that the referee is wrong in general;
for claiming, his ruling at promotion or at baselining that claiming misses pairs in general;
for either, a change of model. A hypothesis added to the set starts nothing. What the sample
draws from exists: for claiming, standing findings across several verifications; for the
referee, claims across several hypotheses and verifications. The collator's subcommand for
the set's sample exists, or is built under building-a-tool once the plan has fixed the sample.

## plan-pipeline-directions

The session presents what started it, quoting the ruling where a ruling did, from the decline
it was written as or from the session it was given in; the set's current version and its
accepting calibration, if any, as state.md shows them; and what the sample could draw from.
It asks Brian, batched four per call: the model and effort; which verifications and
hypotheses the sample draws from, its strata, and its held-out split, named before any item
is scored; and, for a revision, which rulings the new version must answer, a pair claiming
missed among them. Nothing is fixed by default. Each answer is recorded in the file it governs
as that file is written: the model and effort in the sample batch's definition, the span,
strata and split in the calibration's Sample, a ruling in the new version's criteria and in
the calibration's Rulings with his reason. His approval of the plan starts the preparation,
and nothing else records it.

## collate-pipeline-sample

The collator, `tools/StoryPlanner.PipelineCollator`, runs once into a new batch folder under
the set's `batches/`, writing the index, which names the collator, and the item bodies of the
sample the plan fixed: for claiming, each sampled standing finding with the current hypothesis
set before it; for the referee, each sampled claim as the target's current statement and the
finding. A finding whose pair claiming missed is among the sampled findings. The subcommand
that collates a set's sample is built the first time the set is prepared, after the plan, as
the first task inside this activity and never as a study of its own.

## author-pipeline-directions

Written with Brian against the collated items, a new numbered file in the set's folder; the
previous version, if any, stays on disk. For the referee: what the call is given, the
statement and the finding and nothing else; the classes and the criteria between them, tuned
to over-flag; what to produce, the falsifier and the class. For claiming: what the call is
given, the hypothesis set and one finding; the criteria that decide whether the finding bears
on a hypothesis, tuned to over-include; what to produce, the list of the hypothesis file names
it bears on. What never to do, for either. For a revision, each ruling the plan named is
stated generally as a criterion, the item it came from staying in the calibration file.

## assemble-pipeline-sample-batch

The calibration batch's definition, of kind sample, names the draft version by path and no
calibration, with the model and effort the plan fixed. Per the `agent-runner` skill:
dry-run-batch, then execute-batch, the hand-off; the host calls every sample item and writes
the tally when the last has a result. The results are withheld from Brian until he has scored.

## assess-pipeline-sample-items

Instructed by the draft directions body as its system prompt and nothing else; one sample
item in, one answer out in the declared fields.

## calibrate-pipeline-directions

Brian scores each sample item blind, in whatever order the session presents them, and the
session writes his verdicts as given: for the referee, a class per item; for claiming, the
hypotheses each finding bears on. Then the two scorings are laid side by side, on the ruled
items and on the held-out items separately: for the referee, agreement per class; for
claiming, per (finding, hypothesis) pair, recall and precision apart, recall deciding
acceptance. For each disagreement Brian rules; a ruling that changes a criterion edits the
directions into a new version, the ruling stated generally as a criterion, the item it came
from staying in the calibration file. The session writes the calibration beside the version
it judged, titled with that version's body hash and citing its batch as `referee/<batch>` or
`claiming/<batch>`. If any ruling changed the criteria, the collator runs again into a new
batch and assemble-pipeline-sample-batch repeats under the new version; when Brian accepts
the agreement, the calibration's verdict says so, and the version is calibrated at that hash
for the model its batch ran. Nothing in the directions says so, since any line of the body is
part of the hash.

## Never

Runs a full batch; shows Brian the agent's answers before his own are written; fixes the
model, the effort or the sample without Brian; draws the sample from one verification alone;
builds a sample subcommand before the plan has fixed the sample; authors directions without
items in front of it; writes a candidate or a claim; carries a calibration ruling into the
directions as the item it came from; starts because a hypothesis was added to the set.
