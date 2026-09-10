# promoting-refereed-candidates

Enables baselining-a-hypothesis.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| promote | hitl | git | candidates findings index corpus hypothesis-statement hypothesis-record | hypothesis-record hypothesis-status candidates question-list | specified | Brian decides the pending diagnostic candidates he chooses, by hypothesis or by verification, each after the items its finding cites are read; entries and outcomes written; status recomputed; one commit |

## Preconditions

Every candidate in scope carries the referee's `falsifier` and `referee` lines under a
directions hash that has an accepting calibration, and no `outcome` line.

## promote

Brian names the scope: a hypothesis, or a verification. The session gathers every
diagnostic candidate in that scope with no outcome line, from the verifications'
candidates files, the re-queued candidates included, opens each candidate's finding
through its token in the study's findings file, and opens each target's statement and
record. It lists them by target with their findings, verdicts and falsifiers, and shows
the non-diagnostic ones beside them for context; those get no further line.

For each diagnostic candidate, in whatever order Brian takes them:

1. The session reads what the finding cites: the tally section, and the items,
   `<study>/<batch>/<item>`, at the locators their batch's index gives, through the reader
   CORPORA.md names, and reports whether the finding is there as stated, quoting what it
   found. This read precedes any decision to promote; it is skipped only when Brian
   declines without it.
2. Brian decides, after whatever analysis he asks for. The session writes the decision as
   it lands: promote — an `evidence` entry appended to the target's record with the
   finding and falsifier verbatim, tagged by the verdict, citing the candidate's token and
   the directions version and hash it was judged under, then `outcome: promoted …` on the
   candidate; decline — `outcome: declined — <his reason>` on the candidate. A candidate
   he leaves undecided keeps the referee line as its last line.
3. A disagreement with the referee is his to rule; the session writes the ruling in the
   outcome's reason.

A rethink of a statement that the evidence prompts is iterating-a-statement, done in the
same session under its own file; nothing here rewords.

When he stops: the session recomputes each touched hypothesis's status from its
current-wording entries and resets `baselined` where a challenging entry landed; writes
any question he raised into its corpus's list, with the candidate that raised it; and
makes one commit naming the scope and the candidate tokens. What this session decided is
read from the candidates file, where every outcome sits beside its verdict; nothing is
summarised elsewhere.

## Never

Writes a candidate; changes a finding, a falsifier or a verdict; edits a statement;
promotes a candidate that lacks a referee line or whose finding's items were not read;
promotes anything Brian did not decide; writes to a findings file.
