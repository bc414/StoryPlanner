# exploring-a-corpus

Enables reviewing-leads.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| continue-exploration-batch | session | runner | studies definition results | calls tally | specified | On Brian's go, once the pilot's result has been read: execute-batch over every item without a result, the hand-off, after which the host calls them and writes the tally when the last has a result; a one-item batch is the corpus whole and needs no pilot |
| explore-items | agent | | directions items | results | specified | A call reads one item, the corpus whole or one slice, discovery-first under the directions and answers with its leads, what was seen and what it was seen in, as the declared entries; the only writer of results |
| write-leads | session | | definition index results tally question-list | leads | specified | Writes the leads file from the results: method, questions in view, leads organised by what was seen, proposed questions |

## Preconditions

The study is registered with Brian's go and its batch is assembled, itemized and dry-run
by preparing-to-explore-a-corpus. For slices: the pilot's result has been read by Brian and
the directions stand. For a one-item batch: the corpus fits one item, whole or as what to
read and how.

## continue-exploration-batch

Per the `agent-runner` skill: execute-batch on the batch preparing assembled, which calls
every item that has no result yet, the pilot's item excluded since it has one; the host
writes the tally when the last item has a result. A one-item batch's one call is the whole
exploration, watched through the host's stream. A repeat under another model is a second
study, compared at the review.

## explore-items

Instructed by the directions body as its system prompt and nothing else; one item as its
message; the study's model and effort; the MCP server only when the definition opts in,
which a one-item batch over a corpus read through the server does. One item in, one lead
set out in the declared entries: per lead what was seen and what it was seen in. Nothing
about what a lead means for any hypothesis.

## write-leads

The session reads the results through the tally and the index and writes `leads.md` under
the study: the method (the directions version and hash, the itemizer, the model and effort,
the batch, what was deliberately not measured), the questions in view from the directions'
frontmatter, the leads organised by what was observed — by subject, pattern, story, never
by hypothesis — each what was seen and what it was seen in, in words, and the proposed
questions as proposals. From one result the file is that result's leads as rendered.

## Never

Writes a question into a list; writes a candidate or evidence; makes a claim about a
hypothesis; cites a lead at an address; runs an exploration in the repo.
