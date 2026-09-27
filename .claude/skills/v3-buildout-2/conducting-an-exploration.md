# conducting-an-exploration

Enables reviewing-leads.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| continue-exploration-batch | session | runner | studies definition results | calls tally | specified | On Brian's approval, once the pilot's results have been read: the paused execution resumed, the hand-off, after which the host calls every item without a result in the cut's order and writes the tally when the last has one; an execution stopped at any count, by Brian or by the usage cap, is continued by a later execute-batch, which calls whatever it left; a one-item batch is the whole of what the itemizer cuts and needs no pilot |
| explore-items | agent | | directions items | results | specified | A call reads one item, the whole or one slice, discovery-first under the directions and answers with its leads, what was seen and what it was seen in, as the declared entries; the only writer of results |
| write-leads | session | tool-source | definition directions index results question-list skill | leads | specified | In a fresh session, at any count of items with results once the pilot has been read: the batch's results written up as the study's next numbered leads file, so that Brian reads leads rather than every result; its head that count and who wrote it; each lead what was seen, coarsely where, and what it was drawn from, the queries put to the results where a tool answers them and the slices it came from, leads about one thing written together, and the questions the leads raise that no question asks |

## Preconditions

The study is registered and its batch is assembled, itemized and dry-run by
preparing-an-exploration. For slices: the pilot's results have been read by Brian and the
directions stand. For a one-item batch: the whole of what the itemizer cuts, or a stated
narrowing of it, fits one call. For a write-up: the batch holds results past its pilot, or
its one item's, and the session that writes it has not planned the study, written its
directions, built its itemizer or read its pilot.

## continue-exploration-batch

Per the `agent-runner` skill: resume the execution preparing paused, which goes on to call
every item that has no result yet in the cut's order, the pilot's items excluded since they
have theirs; the host writes the tally when the last item has a result. An execution may be
stopped at any count, by Brian or by the usage cap, and continued later: if that execution is
gone — the host restarted, or stop was requested — execute-batch on the batch calls whatever
it left, in the same order. How far to run before a write-up is a judgment made at the plan or
at a review, never a threshold. A one-item batch's one call is the whole exploration, watched
through the host's stream. A repeat under another model is a second study, compared at the
review.

## explore-items

Instructed by the directions body as its system prompt and nothing else; one item as its
message; the study's model and effort; no tools and no MCP. One item in, one lead set out
in the declared entries: per lead what was seen and what it was seen in. Nothing about what
a lead means for any hypothesis.

## write-leads

A fresh session writes the leads: one that did not plan the study, write its directions, build
its itemizer or read its pilot. A new write-up is written because the batch moved on, the
directions or the itemizer changed, or a review asked for a new leads file; the study's
write-ups form one sequence, the latest current. During the write-up the session reads only the
definition, the directions, the index, the results through the results tool, the question list,
and the skill and the results tool's own documentation or source; what changed since an earlier
write-up shows in those. It draws the leads from one batch's results, at whatever count the
batch has reached, into the study's next numbered leads file, in its schema's shape, so that
Brian reads leads rather than every result. The head records how many of the batch's items have a
result, out of its index's items, and in words that this session wrote the file and what it could see. It
reads the results with the index, and the question in view from the frontmatter of the
directions the definition names. Where the results are structured and a tool with tests answers queries
over them, printing each query's one canonical string, the session reads through the tool
and draws its leads from what the queries show; it consolidates no values first, and the
tool groups, merges and ranks nothing. Each lead says what was seen, as a neutral
statement, and coarsely and in words what it was seen in, and points back at what it was
drawn from: the query strings, as the tool printed them, of the queries it rests on, and the
item tokens of the slices whose results it came from, both where both apply and always at
least one; leads the readers repeated across slices become one lead. A study need have no
query tool; its leads then carry the item tokens alone. Leads about one
subject, pattern or story are written next to one another, never grouped by hypothesis. Each
lead's slug is created here, naming what was seen, and its heading carries the file's own
number. What the leads raise that no question in
view or in the question list asks is written under Proposed questions. The procedure is the
same for one whole result as for many slices. It writes no shortcoming, and nothing of
how the exploration ran, which the batch's own files hold.

## Never

Writes a question into a list; writes a candidate or evidence; says what a lead means for a
hypothesis; names a position inside a slice in a lead; writes a shortcoming or a next step;
runs an exploration in the repo; writes leads in a session that planned the study, wrote its
directions, built its itemizer or read its pilot; reads a session transcript, `codesessions.db`
or another leads file of the study, its review lines included, during a write-up; draws one
leads file from two batches.
