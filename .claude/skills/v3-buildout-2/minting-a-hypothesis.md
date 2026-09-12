# minting-a-hypothesis

Enables reviewing-leads.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| mint | hitl | git | hypothesis-index hypothesis-statement | hypothesis-statement hypothesis-origin hypothesis-index | built | A new hypothesis file on novelty, testability and independence against the current set; Brian words or approves the statement; the origin records why it exists and what raised it |

## Preconditions

Something raised it: Brian's own statement, a lead, evidence in a record, or a merge or
split. Three criteria hold, all required: novelty (it is not evidence for an existing
hypothesis), testability (a referee handed the statement and one finding could write its
falsifier — what the finding would have been were the statement false), independence (it is
not a refinement, which would be an iteration).

**Testability is the criterion with no second chance**, which is why it is stated as the
referee's own test rather than as whether evidence could bear on the claim. A statement no
falsifier can discriminate — a prescription about what the instrument ought to do is the
common shape, and reads as "should be recognised", "may not be optimal", "may be needed" —
can never acquire evidence, so it can never be `challenged`, so `iterating-a-statement`'s
precondition is never met and the wording can only ever change through a merge or split. And
nothing retires a hypothesis: one that is disproven stands as disproven, and one that was
never a prediction stands unfalsifiable. Mint is the only gate it passes through.

## mint

Brian's explicit statements always get the offer. A session's own reading may surface a
proposal only when the three criteria hold, and the proposal cites the specific lead,
entry or statement that raised it, never a synthesis. During an exploration's or a
verification's autonomous part the proposal is held for the review or the promotion session.
Independence is read against the current set's statements, which is all mint reads of the
existing files: a proposal that refines one of them is an iteration and not a mint.

Brian reviews the statement: rewrites it in his words, or approves. The session writes the
file with the next unused id: the statement alone under § Hypothesis, and under § Origin
today's date and the reasoning — why the hypothesis exists, the observation, his assertion,
the motivation, and what raised it, in Claude's voice with his assertions as the content.
The record is created empty, which is the state of an untested hypothesis. The index gains
its row; one commit. For a merge or split, the reasoning names the files it came from.

The trap this guards: a proposal in Claude's framing, nodded through, on which later
sessions build. What guards it is rule 6, which puts Brian's wording or approval on every
statement, and the code-sessions archive, which answers what a session had read when it
proposed. There is no provenance field, and one is not to be restored as a missing
safeguard: a flag recording whose idea it was would be written by the party whose influence
it purports to measure.

## Never

Mints without Brian's rewrite or approval; reuses an id; writes a statement that carries
provenance, implications or testing method; mints a statement no falsifier can discriminate;
mints a refinement.
