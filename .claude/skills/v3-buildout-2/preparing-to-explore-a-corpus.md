# preparing-to-explore-a-corpus

Enables exploring-a-corpus.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| explore-plan | hitl | | question-list corpus-status corpus state | instances arm-key question-list | built | Brian and the session fix the scale, the questions in view, the arms if any; the plan approved is his go, the instance is registered, the arm key written and closed; his opening question written into the list if the corpus has none |
| slice | session | itemizer | corpus | items items-manifest | specified | Slices only: the corpus cut into one file per slice, by a tool, with a manifest |
| author-protocol | hitl | | question-list items reading-protocol | reading-protocol generator | specified | Slices only: the reading protocol written against the real slices, and the generator that encodes its record-set contract; a new numbered version each time |
| pilot-run | hitl | generator runner | reading-protocol items results | jobs ledger run-record | specified | Slices only: one job under the protocol's hash; Brian reads its record set and rules whether the protocol stands; his verdict in run.md |
| pilot-read | agent | | reading-protocol items | results | specified | The pilot's slice reader; the only writer of its result |

<!-- generated:activity -->
<!-- /generated -->

## Preconditions

The corpus is readable and CORPUS-STATUS says so, or the corpus is the verified artifacts
of rounds already promoted. If its question list is empty, Brian's opening question is
asked for and written first.

## explore-plan

The session presents the corpus's shape from CORPUS-STATUS and its open questions, and
asks Brian, batched four per call: the scale (whole corpus in one context, or slices), the
questions in view, whether arms are wanted and what one factor varies across them, the
binning scheme, and what the exploration does not do. It writes the plan; Brian approves;
the session appends the instance to `instances.md` with the date as his go. With arms it
writes `arm-key.md` and does not open it again.

## slice

A tool cuts the corpus into slices, one file each, with a manifest, under
`fanout/<instance>/<run>/items/`; the runner's split verb where the corpus is markdown
units, otherwise a script with tests. The cut is mechanical and recorded in the manifest.

## author-protocol

Written against the real slices in the format `artifacts.md` § Reading protocol, with the
questions in view verbatim and the record-set form the joiner expects. A new numbered
file.

## pilot-run

One job, one slice, under the protocol's hash, through the runner per the `agent-runner`
skill. Brian reads its record set and says whether the protocol produces leads of the
shape wanted; the read and his verdict are written into the run's `run.md`. A protocol he
sends back is a new version and a new pilot.

## pilot-read

Instructed by the reading protocol and nothing else; one slice in, one record set out.

## Never

Reads the corpus for content beyond sizing it; names a hypothesis as a target; opens the
arm key after writing it; batches before the pilot is read.
