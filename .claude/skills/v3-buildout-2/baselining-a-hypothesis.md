# baselining-a-hypothesis

Enables changing-the-planner-for-v3.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| baseline | hitl | | hypothesis-statement hypothesis-origin hypothesis-record | hypothesis-record question-list | specified | Brian judges the evidence picture sufficient to act on; the session writes his entry |

## Preconditions

The hypothesis's record holds at least one `evidence` entry bound to the current wording
and no unresolved challenging entry bound to it, and no `baselined` entry bound to it.

## baseline

The session presents the file: the statement, the founding reasoning, and the
current-wording entries, supporting and challenging, with their falsifiers. Nothing else —
no summary of what the evidence means, no recommendation. If Brian raised the hypothesis
for baselining himself, that is the whole preparation; if the session is naming it as a
candidate, it says so in the words "verified support, no open challenge — review for
baselining" and waits.

Brian decides. If he baselines, the session appends the `baselined` entry with his
rationale; the status the file reads from is recomputed by nothing, being
derived from the entries. If he does not, nothing is written to the hypothesis; a reason
he gives that is a question about a corpus is written into that corpus's question list,
with the hypothesis that raised it.

## Never

Writes the entry without Brian's explicit direction; baselines against an empty
current-wording record or an open challenge.
