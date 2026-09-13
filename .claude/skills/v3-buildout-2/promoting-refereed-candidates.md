# promoting-refereed-candidates

Enables baselining-a-hypothesis.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| promote | hitl | DocIntegrity | candidates findings index corpus hypothesis-statement hypothesis-record | hypothesis-record declined-candidates question-list | specified | Brian decides the pending diagnostic candidates he chooses, by hypothesis or by verification, each after the items its finding cites are read: promote — a verbatim evidence entry to the hypothesis record; decline — an entry to declined-candidates.md; candidates.md regenerated |

## Preconditions

Every candidate in scope is diagnostic — carries a referee verdict under a directions hash that
has an accepting calibration — and has no outcome yet: it is neither cited by a hypothesis record
(promoted) nor in declined-candidates.md (declined). candidates.md, the generated view,
materialises each candidate's finding, verdict and falsifier for reading.

## promote

Brian names the scope: a hypothesis, or a verification. The session reads candidates.md, which
already materialises, for each diagnostic candidate, its finding, the referee's verdict and
falsifier, and its status; the non-diagnostic ones sit at the foot as context and get no decision.
It opens each target's statement and record.

For each pending diagnostic candidate, in whatever order Brian takes them:

1. The session reads what the finding cites — the tally section, and the items, at the locators
   their batch's index gives, through the reader CORPORA.md names — and reports whether the
   finding is there as stated, quoting what it found. This read precedes any decision to promote;
   it is skipped only when Brian declines without it.
2. Brian decides, after whatever analysis he asks for. The session writes the decision as it
   lands: promote — an `evidence` entry appended to the target's record with the finding and
   falsifier verbatim, tagged by the verdict, citing the candidate's token; decline — an entry in the study's
   `declined-candidates.md` with the candidate's (finding, target) heading, the date, and Brian's
   reason. A candidate he leaves undecided is written nowhere and stays pending.
3. A disagreement with the referee is his to rule, and it is a decline, whether he judges the
   finding to bear on nothing or to bear the other way: the session writes his ruling as the
   decline's reason. A ruling that shows the referee wrong in general is what sends its
   directions back through preparing-to-verify-a-corpus.

A rethink of a statement that the evidence prompts is iterating-a-statement, done in its own
session under its own file; nothing here rewords.

When he stops, the session regenerates candidates.md through DocIntegrity so each candidate
shows its outcome (promoted, declined or pending), and writes any question he raised into its
corpus's list. Promotions are read from the hypothesis records, declines from
declined-candidates.md; nothing is summarised on candidates.md by hand.

## Never

Writes a candidate; hand-edits candidates.md; changes a finding, a falsifier or a verdict;
promotes with a tag other than the verdict; edits
a statement; promotes a candidate that lacks a referee line or whose finding's items were not
read; promotes anything Brian did not decide; writes to a findings file; puts a decline reason on
the hypothesis record.
