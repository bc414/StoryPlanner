# iterating-a-statement

Enables refereeing-a-candidate.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| iterate | hitl | git | hypothesis-statement hypothesis-record | hypothesis-statement hypothesis-record hypothesis-status candidates | specified | Brian rewords a hypothesis on evidence; the statement is edited, an iteration entry marks the boundary, status is recomputed, prior findings are appended as candidates to the studies that produced them |

## Preconditions

Brian has decided to reword, in a promotion session because evidence called for a rethink, or
in any hitl session because a merge or split requires it; that decision is the whole
trigger, and no activity enables this one. A lead never prompts an iteration;
a lead that shows a different hypothesis is needed goes to minting-a-hypothesis.

## iterate

The session shows the current statement and the entries bound to it. Brian gives the new
wording, or approves the session's draft of it in his words. The session then, in one
commit: edits `## Hypothesis` in place; appends an `iteration` entry quoting old and new
wording and his reason, with the sentence that entries above it are bound to the prior
wording; recomputes `status` from the entries bound to the new wording, which is
`untested` when none has been re-verified, and resets `baselined` to `false`; and appends,
to the candidates file of each study a prior `evidence` entry cites, one new candidate:
the finding verbatim, the target, and provenance citing the original candidate's token and
the rewording's date. Each such study then runs a referee batch over its new candidates,
on Brian's go, and promotion reads them where it reads all candidates; nothing is
re-refereed now.

For a merge or split, the same steps run in each affected file: the surviving or new
files are minted (minting-a-hypothesis), each old file gets its iteration entry naming
what replaced it, and its status is set from its own current entries, which after
supersession are none.

## Never

Edits, deletes or re-tags an entry; marks an entry superseded; re-refereees immediately;
rewords on a lead; changes a statement Brian did not word or approve.
