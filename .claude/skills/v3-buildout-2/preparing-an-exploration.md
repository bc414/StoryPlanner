# preparing-an-exploration

Enables conducting-an-exploration.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| explore-plan | hitl | | question-list corpora corpus state skill | studies question-list | specified | The question fixed first, traced through the code-sessions archive to Brian's words and its terms checked against the live data, shown to him and never reworded; then Brian and the session fix the scale, one item or slices, the itemizer, the model and effort; the plan approved registers the study; his question written into the list first where it is not there |
| author-exploration-directions | hitl | | question-list corpus directions | directions | specified | The directions written with Brian against the corpora: what one item is, how to read with the question in view, what to produce as entries; a new numbered version each time |
| assemble-exploration-batch | session | runner tool-source | directions corpus | definition index items calls | specified | The batch's definition written and its items cut by the itemizer, for a new study, a new directions version or a changed itemizer that stays the study's; dry-run-batch; for slices, execute-batch paused as soon as it starts, its first calls in the cut's order the pilot, whose results Brian reads before the rest run |
| explore-pilot-item | agent | | directions items | results | specified | A pilot call, one slice read discovery-first under the directions; the only writer of its result |

## Preconditions

The question is open in the list, or is asked for and written first. Every corpus the
study's itemizer will read is readable and CORPORA.md says how.

## explore-plan

The session first fixes the question the study is of. It traces the question from its
`raised by` back through the code-sessions archive to Brian's typed words, marks each point
where the question's wording entered from a session turn rather than from his typing, a
line he typed that repeats a session's offered wording included, and checks each of the
question's terms against the live schema and data the study will read. It shows him the
chain and the check in the chat, for every question, one from his recall included, and
proposes no rewording: a rewording is his, and goes through write-question as a new entry,
the old one withdrawn.

Then the session presents the question and the shape of every corpus the
itemizer will read, from CORPORA.md, and asks Brian, batched four per call: the scale the
question calls for, one item that is the whole of what the itemizer cuts, where that or a
stated narrowing of it fits one call, or slices, as peers, and the itemizer that cuts them;
the model and effort; what the exploration does not do; and, where Brian has a view, how far
the batch runs before its first write-up. The plan is written against
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
slices execute-batch, paused on the page as soon as it starts; the calls that launched
before the pause, as many as the host's ceiling and the first in the cut's order, are the
pilot, the same items for every version. Brian reads those results and says whether the
directions produce leads of the shape wanted; directions he sends back are a new version and a
new batch, since a definition is never edited. A review that sends the study to building-a-tool
for a change that keeps the itemizer the study's comes back here: the changed tool cuts a new
batch of the same study under its current directions, the index head recording the new commit
and narrowing. A one-item
batch needs no pilot: its one call is the exploration, and conducting-an-exploration runs
it.

## explore-pilot-item

Instructed by the directions body as its system prompt and nothing else; one slice in,
read discovery-first with the questions in view; one lead set out in the declared entries.

## Never

Reads a corpus for content beyond sizing it and tracing the question's origin; names a hypothesis as a target; executes
the rest of a sliced batch before Brian has read the pilot's results; edits a definition.
