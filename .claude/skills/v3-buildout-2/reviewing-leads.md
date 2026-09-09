# reviewing-leads

Enables preparing-to-verify-a-corpus.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| review-leads | hitl | git | leads-artifact corpus hypothesis-statement | leads-artifact question-list | specified | Brian and the session over a leads artifact, or two of one corpus: leads challenged at the source and corrected, the differences between explorations read as leads about the readers, and the questions Brian raises written into the corpus's list |
| ask | hitl | git | hypothesis-index hypothesis-statement question-list | question-list | built | Ad hoc: a question Brian raises in conversation about the framework, written into a corpus's list |

## Preconditions

For `review-leads`: a leads artifact whose `## Corrections` section is empty and whose
proposed questions have not been written to any list; where the corpus was explored again
under another model or another reading, both artifacts. For `ask`: nothing; it is any hitl
session in which Brian asks a question about a corpus.

## review-leads

The session opens the leads artifact and, where a second exploration of the corpus
exists, the second beside it: their differences are read as leads about the readers, never
counted, and what each saw that the other did not is laid out for Brian.

Brian challenges leads. For each, the session verifies against the corpus itself, where
the lead says it was seen, never against the artifact, and reports what the corpus shows;
a lead that does not hold is written in `## Corrections`, dated, with what the source
showed. The lead itself is not edited.

Brian raises questions: from the artifact's `## Proposed questions`, from the differences,
from his own recall, which enters only as a question with its provenance ("Brian's recall,
<date>: does the v1 archive show X?"). The session writes each into the corpus's question
list in his words, with what raised it, the hypotheses it concerns and a suggested test
where one suggests itself. A lead that shows a different hypothesis is needed is
handed to minting-a-hypothesis in the same session. One commit.

## ask

Brian asks; the session writes the entry into the named corpus's list in his words, with
what raised it, the hypotheses it concerns, and a suggested test if one suggests itself.
One commit.

## Never

Edits a lead; writes a candidate or evidence; rewords a hypothesis; writes a question Brian
did not ask; enters recall as anything but a question; counts disagreements between lead
sets.
