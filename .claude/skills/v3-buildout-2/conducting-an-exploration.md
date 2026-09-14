# conducting-an-exploration

Enables reviewing-leads.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| continue-exploration-batch | session | runner | studies definition results | calls tally | specified | On Brian's approval, once the pilot's result has been read: execute-batch over every item without a result, the hand-off, after which the host calls them and writes the tally when the last has a result; a one-item batch is the whole of what the itemizer cuts and needs no pilot |
| explore-items | agent | | directions items | results | specified | A call reads one item, the whole or one slice, discovery-first under the directions and answers with its leads, what was seen and what it was seen in, as the declared entries; the only writer of results |
| write-leads | session | | definition directions index results question-list | leads | specified | Consolidates every result into the leads file, so that Brian reads leads rather than every result: each lead what was seen, coarsely where, and the slices it cites, leads about one thing written together, and the questions the leads raise that no question asks |

## Preconditions

The study is registered and its batch is assembled, itemized and dry-run by
preparing-an-exploration. For slices: the pilot's result has been read by Brian and the
directions stand. For a one-item batch: the whole of what the itemizer cuts, or a stated
narrowing of it, fits one call.

## continue-exploration-batch

Per the `agent-runner` skill: execute-batch on the batch preparing assembled, which calls
every item that has no result yet, the pilot's item excluded since it has one; the host
writes the tally when the last item has a result. A one-item batch's one call is the whole
exploration, watched through the host's stream. A repeat under another model is a second
study, compared at the review.

## explore-items

Instructed by the directions body as its system prompt and nothing else; one item as its
message; the study's model and effort; no tools and no MCP. One item in, one lead set out
in the declared entries: per lead what was seen and what it was seen in. Nothing about what
a lead means for any hypothesis.

## write-leads

The session consolidates every result of the batch into `leads.md` under the study, in its
schema's shape, so that Brian reads leads rather than every result. It reads the results
with the index, and the question in view from the frontmatter of the directions the
definition names. Each lead says what was seen, as a neutral statement, and coarsely and in
words what it was seen in, and cites by item token every slice whose result it came from;
leads the readers repeated across slices become one lead citing them all. Leads about one
subject, pattern or story are written next to one another, never grouped by hypothesis. Each
lead's slug is created here, naming what was seen. What the leads raise that no question in
view or in the question list asks is written under Proposed questions. The procedure is the
same for one whole result as for many slices. It writes no shortcoming, and nothing of
how the exploration ran, which the batch's own files hold.

## Never

Writes a question into a list; writes a candidate or evidence; says what a lead means for a
hypothesis; names a position inside a slice in a lead; writes a shortcoming; runs an
exploration in the repo.
