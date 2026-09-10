# preparing-to-explore-a-corpus

Enables exploring-a-corpus.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| explore-plan | hitl | | question-list corpora corpus state skill | studies question-list | specified | Brian and the session fix the scale, one item or slices, the questions in view, the model and effort; the plan approved is his go and the study is registered; his opening question written into the list if the corpus has none |
| author-exploration-directions | hitl | | question-list corpus directions | directions | specified | The directions written with Brian against the corpus: what one item is, how to read with the questions in view, what to produce as entries; a new numbered version each time |
| assemble-exploration-batch | session | runner tool-source | directions corpus | definition index items calls | specified | The batch's definition written and its items cut by the itemizer; dry-run-batch; for slices, execute-batch naming one item, the pilot, whose result Brian reads before the rest run |
| explore-pilot-item | agent | | directions items | results | specified | The pilot's call, one slice read discovery-first under the directions; the only writer of its result |

## Preconditions

The corpus is readable and CORPORA.md says how, or the corpus is the verified artifacts
of verifications already promoted. If its question list is empty, Brian's opening question
is asked for and written first.

## explore-plan

The session presents the corpus's shape from CORPORA.md and its open questions, and asks
Brian, batched four per call: the scale (one item that is the corpus whole, or slices, and
the itemizer that cuts them), the questions in view, the model and effort, and what the
exploration does not do. The plan is written against the chain's activity files,
exploring-a-corpus and reviewing-leads, read whole here rather than each at its own start,
since it names what each of them will do for this study. Brian approves; the session
appends the study to `studies.md` with the date as his go.

## author-exploration-directions

Written with Brian against the corpus, or against the real slices once cut: what the call
is given and where the item stops, how to read it with the questions in view, which the
frontmatter cites by token, what to produce as entries, and what never to do. A new
numbered file under the study; a one-item exploration has directions all the same, so that
a repeat under another model cites the same file.

## assemble-exploration-batch

The batch folder under the study, `batches/<nn>-<slug>/`, its definition naming the
directions by path, the study's model and effort, and the MCP server when the item is what
to read through it. The itemizer runs once into the folder: for slices a tool with tests
that cuts the corpus into one item each; for one item, a tool that concatenates a file
corpus's texts or writes the item as what to read and how. Then, per the `agent-runner`
skill, dry-run-batch, and for slices execute-batch naming one item. Brian reads that
result and says whether the directions produce leads of the shape wanted; directions he
sends back are a new version and a new batch, since a definition is never edited. A
one-item batch needs no pilot: its one call is the exploration, and exploring-a-corpus
runs it.

## explore-pilot-item

Instructed by the directions body as its system prompt and nothing else; one slice in,
read discovery-first with the questions in view; one lead set out in the declared entries.

## Never

Reads the corpus for content beyond sizing it; names a hypothesis as a target; executes
the rest of a sliced batch before Brian has read the pilot's result; edits a definition.
