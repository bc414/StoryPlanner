# reviewing-findings

Enables surfacing-candidates.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| review-findings | hitl | git | findings tally results index corpus hypothesis-statement | findings question-list | specified | Brian and the session over a verification's findings, or two verifications sharing directions by their tallies: a finding he doubts checked against the results and the items and withdrawn or superseded; a result he doubts checked at the item and written as a shortcoming; what he raises checked the same way before it is written as a finding; the questions he raises written into the corpus's list; a missing hypothesis handed to minting |

## Preconditions

A verification's `findings.md` exists and its findings have not been reviewed; where the
corpus was verified again under another model with the same directions, both studies'
tallies. Nothing has been written to the findings file since the analysis except by this
activity.

## review-findings

The session opens the findings and, where a second verification of the corpus under the
same directions exists, its tally beside the first's: the two are compared by the tally,
class by class, and what differs is laid out for Brian as a fact about the models, never
counted as a finding of either.

Brian reads the findings and challenges what his recall or his reading of the tally
disagrees with. For each challenge the session says which of two things it is checking,
because the same reading of the same item ends in different places. When the doubt is
about a finding, the session checks it against the results and the tally, and against the
items the finding cites, and reports what the data shows; a finding that does not hold
takes a `withdrawn` line with the date and what the data showed, and a finding that holds
amended is written as a new entry naming the old in `supersedes`; the old entry is never
edited. When the doubt is about a result, the session reads the item at its locator and
reports whether the class fits it; a result wrong for its item is a shortcoming of the
directions, written under Shortcomings with its part, never a correction, since the
result is never edited and the fix is a new version.

What Brian raises over the tally is checked the same way before anything is written: the
session checks it against the results and the items in this sitting, and only what holds
is written, as a finding entry with its citations. His recall enters as the check and never
as the finding; a recall the data does not bear out becomes a question if it is worth
asking of a later version, and otherwise nothing.

Brian raises questions: from the findings, from the Proposed questions, from the
Shortcomings, from the differences between two verifications, from his own recall. The
session writes each into the corpus's question list in his words, with the finding's token
as what raised it where a finding did, the hypotheses it concerns, and a suggested test
where one suggests itself. A finding that shows a different hypothesis is needed is handed
to minting-a-hypothesis in the same session. A shortcoming that needs a new version or a
new tool sends the study back through preparing-to-verify-a-corpus or building-a-tool;
the session says which and does not start it here. One commit.

## Never

Edits a finding, a result or a tally; writes a finding it did not check against the
results and the items in this sitting; enters recall as a finding; writes a candidate, a
falsifier or evidence; rewords a hypothesis; writes a question Brian did not ask; counts
the differences between two verifications as a finding.
