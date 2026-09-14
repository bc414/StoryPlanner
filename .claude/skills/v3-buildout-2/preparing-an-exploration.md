# preparing-an-exploration

Enables conducting-an-exploration.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| explore-plan | hitl | | question-list corpora corpus state skill | studies question-list | specified | Brian and the session fix the question, the scale, one item or slices, the itemizer, the model and effort; the plan approved registers the study; his question written into the list first where it is not there |
| author-exploration-directions | hitl | | question-list corpus directions | directions | specified | The directions written with Brian against the corpora: what one item is, how to read with the question in view, what to produce as entries; a new numbered version each time |
| assemble-exploration-batch | session | runner tool-source | directions corpus | definition index items calls | specified | The batch's definition written and its items cut by the itemizer; dry-run-batch; for slices, execute-batch naming one item, the pilot, whose result Brian reads before the rest run |
| explore-pilot-item | agent | | directions items | results | specified | The pilot's call, one slice read discovery-first under the directions; the only writer of its result |

## Preconditions

The question is open in the list, or is asked for and written first. Every corpus the
study's itemizer will read is readable and CORPORA.md says how.

## explore-plan

The session presents the question the study is of and the shape of every corpus the
itemizer will read, from CORPORA.md, and asks Brian, batched four per call: the scale the
question calls for, one item that is the whole of what the itemizer cuts, where that or a
stated narrowing of it fits one call, or slices, as peers, and the itemizer that cuts them;
the model and effort; and what the exploration does not do. The plan is written against
the chain's activity files, conducting-an-exploration and reviewing-leads, read whole here
rather than each at its own start, since it names what each of them will do for this study.
Brian approves; the session appends the study's id to `studies.md`, which is its
registration.

## author-exploration-directions

Written with Brian against the corpora, or against the real slices once cut: what the call
is given and where the item stops, how to read it with the question in view, which the
frontmatter cites by token, what to produce as entries, and what never to do. A new
numbered file under the study; a one-item exploration has directions all the same, so that
a repeat under another model cites the same file.

## assemble-exploration-batch

The batch folder under the study, `batches/<nn>-<slug>/`, its definition naming the
directions by path and the study's model and effort. The itemizer runs once into the folder:
for slices a tool with tests that cuts the corpora into one item each; for one item, a tool
that renders or concatenates the whole of what it cuts into the item, never instructions
for reading it elsewhere. It reads corpora and nothing else, and may read other corpora to
cut, label and fill the items, as preparing-a-verification § itemize says, stating any
narrowing in the index head. Then, per the `agent-runner` skill, dry-run-batch, and for
slices execute-batch naming one item. Brian reads that result and says whether the
directions produce leads of the shape wanted; directions he sends back are a new version
and a new batch, since a definition is never edited. A one-item batch needs no pilot: its
one call is the exploration, and conducting-an-exploration runs it.

## explore-pilot-item

Instructed by the directions body as its system prompt and nothing else; one slice in,
read discovery-first with the questions in view; one lead set out in the declared entries.

## Never

Reads a corpus for content beyond sizing it; names a hypothesis as a target; executes
the rest of a sliced batch before Brian has read the pilot's result; edits a definition.
