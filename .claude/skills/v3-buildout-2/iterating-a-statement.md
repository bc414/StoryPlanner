# iterating-a-statement

Enables baselining-a-hypothesis.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| assemble-reverify-batch | session | runner tool-source | hypothesis-record directions calibration | definition index items calls tally | specified | On the wording Brian proposes: one referee item per current-wording evidence entry, holding the proposed wording and that entry's frozen finding text, into a batch under iterations/; dry-run-batch; execute-batch as the hand-off |
| assess-reverify-items | agent | | directions items | results | specified | One blind call per item under the method's referee directions: the falsifier, and diagnostic supporting, diagnostic challenging or non-diagnostic; the only writer of results |
| gate-and-commit | hitl | git | results tally hypothesis-statement hypothesis-record | hypothesis-statement hypothesis-record hypothesis-status | specified | The gate: every prior finding diagnostic-supporting of the proposed wording. All pass — the wording, the iteration boundary and fresh evidence entries are written, status recomputed, baselined reset. Any fail — nothing is written and the failing findings are reported |

## Preconditions

The hypothesis is `challenged` — a challenging evidence entry bound to the current wording is
unresolved — or a merge or split requires the reword. Iteration never rewords a hypothesis that
is evidenced and unchallenged; a sharper claim on supporting evidence is a new hypothesis
(minting-a-hypothesis). Brian proposes the new wording in session, or approves the session's
draft of it in his words; the wording is a proposal, held for the reverify batch and written
nowhere until the gate passes, because the meaning is changing to reconcile the challenge and
only a re-verification against all the evidence licenses it. For the batch: the method's referee
directions, in the referee folder, have an accepting calibration at their hash.

## assemble-reverify-batch

One item per current-wording evidence entry — every entry bound to the current wording, the
challenging one included. The item holds exactly two things: the proposed wording Brian gave; and
that entry's finding text, the frozen snapshot the evidence entry carries, read off the
hypothesis file, never resolved through the studies. It holds no falsifier, no citation, no other
entry. The batch folder is under
`docs/v3-framework/iterations/iteration-of-<hypothesis-file-name>-<N>/`, `<N>` the ordinal of
this hypothesis's iterations, the same folder structure as a study so the consumers' lookup
patterns generalise; the reverify itemizer — `tools/StoryPlanner.SurfacingItemizer`, its
`reverify` subcommand over the hypothesis file and the proposed wording into this batch — writes
the index (corpus `candidates`, each item's locator the entry's finding token) and the item
bodies; the definition, of kind full, names the referee's directions and their accepting
calibration by relative path into the referee folder, and the referee's model and effort. Then, per the
`agent-runner` skill, dry-run-batch and execute-batch, the hand-off; the host calls every item
and writes the tally when the last has a result. No pilot: the referee's calibration was it.

## assess-reverify-items

Instructed by the referee's directions body as its system prompt and nothing else; no tools, no
MCP. Given the proposed statement and the finding, it writes the falsifier — what the finding
would have been if the statement were false — and classifies the candidate diagnostic supporting,
diagnostic challenging or non-diagnostic. The declared fields out, which the runner renders as the
result. It is the method's one referee, judging the proposed statement it is handed rather than
one read from a file.

## gate-and-commit

The session reads every result against the tally and applies the gate: every current-wording
evidence finding, the once-challenging one included, must come out diagnostic AND supporting of
the proposed wording. A finding that comes out challenging or non-diagnostic fails it.

All pass, in one commit: the session edits `## Hypothesis` in place to the proposed wording;
appends an `iteration` entry quoting old and new wording and Brian's reason, with the sentence
that entries above it are bound to the prior wording; appends, for each finding, a fresh
`evidence` entry against the new wording carrying the new falsifier the re-referee wrote and
citing the iteration's referee batch; recomputes `status` (evidenced, all supporting and no open
challenge) and resets `baselined` to `false`. The prior evidence entries, with their old
falsifiers, stay above the boundary as history — the evolution of a hypothesis against its
evidence is itself data a later study may analyse. Iterating is what lets a challenged hypothesis
be baselined again: a wording all the evidence supports.

Any finding fails the gate: nothing is written — not the wording, not an entry — and the failing
findings are reported. Brian proposes another wording, or concludes the hypothesis must be split
or abandoned; a wording that sheds a piece of the evidence is not the honest reconciliation of all
of it.

For a merge or split, the surviving or new files are minted (minting-a-hypothesis), each old file
gets its iteration entry naming what replaced it, and its status is set from its own current
entries, which after the split are none.

## Never

Writes to the hypothesis file before the gate passes; refereees against the committed statement
rather than the proposed one; accepts a finding that comes out non-diagnostic or challenging;
re-binds an old entry to the new wording in place, or edits or deletes an entry; rewords on a lead
or to sharpen an unchallenged hypothesis; changes a statement Brian did not word or approve.
