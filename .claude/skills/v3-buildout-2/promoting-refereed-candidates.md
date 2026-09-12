# promoting-refereed-candidates

Enables baselining-a-hypothesis.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| promote | hitl | DocIntegrity git | candidates findings index corpus hypothesis-statement hypothesis-record | hypothesis-record declined-candidates question-list | specified | Brian decides the pending diagnostic candidates he chooses, by hypothesis or by verification, each after the items its finding cites are read: promote — a verbatim evidence entry to the hypothesis record; decline — an entry to declined-candidates.md; candidates.md regenerated; one commit |

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
   falsifier verbatim, tagged by the verdict, citing the candidate's token and the directions
   version and hash it was judged under; decline — an entry in the study's
   `declined-candidates.md` with the candidate's (finding, target) heading, the date, and Brian's
   reason. A candidate he leaves undecided is written nowhere and stays pending.
3. A disagreement with the referee is his to rule; the session writes the ruling in the decline's
   reason, or promotes against the verdict with the ruling in the evidence entry.

A rethink of a statement that the evidence prompts is iterating-a-statement, done in its own
session under its own file; nothing here rewords.

When he stops, in one commit: the session regenerates
candidates.md through DocIntegrity so each candidate shows its outcome (promoted, declined or
pending); writes any question he raised into its corpus's list; and names the scope and the
candidate tokens in the commit. Promotions are read from the hypothesis records, declines from
declined-candidates.md; nothing is summarised on candidates.md by hand.

## Never

Writes a candidate; hand-edits candidates.md; changes a finding, a falsifier or a verdict; edits
a statement; promotes a candidate that lacks a referee line or whose finding's items were not
read; promotes anything Brian did not decide; writes to a findings file; puts a decline reason on
the hypothesis record.
