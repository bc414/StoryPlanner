# The evidence pipeline (strong form)

Read in full before writing anything that could end up in a hypothesis record — a
candidate, a referee verdict, a promoted entry, an iteration. This is the one file that
must be applied whole.

## Goal

Four things at once: every relevant finding is preserved and findable near the hypothesis
it bears on; discriminating evidence is separated from context by *status*, never by
exclusion; the separation is not judged by the party that produced the finding; and the
human-facing record is never touched by unreviewed machine judgment.

Vocabulary the pipeline depends on: a **finding** is what a pass observed; a
**candidate** is a finding a verification pass claims bears on a hypothesis; **evidence**
is a promoted candidate; a **question** is a spec-pool entry. Treating a finding as
evidence is the conflation the pipeline exists to make structurally impossible (the
episode that prompted it is in `docs/v3-framework/methodology-revision-1.md`).

## Where each kind of write goes

| What | Who writes it | Where |
|---|---|---|
| A finding of an exploratory pass | the pass | the WU artifact |
| A question raised about a corpus | any pass, any post-review | `docs/v3-framework/spec-pools/<corpus>.md` |
| A candidate (a finding that a verification pass claims bears on a hypothesis) | the verification pass | `fanout/WU<n>.<m>-…/candidates.md` |
| The referee's verdict on a candidate | the referee (autonomous, fresh context) | appended to the same candidate |
| A promoted evidence entry | a promotion session, Brian reviewing the commit | `docs/v3-framework/hypotheses/NNN-slug.md` |
| A statement change | a HITL session, batched at the end of a review | the hypothesis file (iteration entry) |

Nothing else writes to `hypotheses/` — not a WU session at wrap-up, not a subagent, not a
post-review in the flow of conversation, not any other path however reasonable it looks in
the moment.

## The candidates file

One per verification WU, in the WU's work folder under `fanout/` (the table above; the
`docs/` artifact cites it), **append-only**: a finding is
never edited after it is written; the referee and the promotion session append lines.

```markdown
## C-014
- target: 031
- status: candidate            # candidate | diagnostic | non-diagnostic | promoted | declined | held
- finding: <one citable unit: what was observed, with ids, counts, passages>
- source: <what was read — WU artifact section, corpus locus, note ids, story + chapter>
- proposed-by: <arm or job id> / <model> / <ISO time> / codebook <name>@<hash> / harness <version>
```

Referee append (one per target; a candidate bearing on two hypotheses is two candidates):

```markdown
- clause: Would differ if false: <the observable the excerpt would have shown instead — written fresh>
- referee: <job id> / <model> / <ISO time> / codebook referee@<hash> / verdict diagnostic [supporting|challenging] | non-diagnostic — <one-line reason>
```

Promotion append:

```markdown
- disposition: promoted <ISO time> as evidence entry <timestamp in NNN-slug.md> | declined — <reason> | held — <what it waits on>
```

Status is derived from the last append and is the only field that changes. `held` is for
a candidate that waits on something named (e.g. a codebook revision, a source re-read),
never a soft landing for "unsure".

## The referee

A **classifier in the verify role**, and therefore an instrument: its procedure is a
codebook — `fanout/referee/codebook.md` — not this skill, which the referee
never sees. The codebook is what the runner hands it (protocol file + the candidate + the
source excerpt), and every verdict line cites `codebook referee@<hash>`. Like any
codebook it is calibrated on Brian's verdicts before its first batch and re-hashed on
every ruling; a referee run under an uncalibrated or superseded hash is re-run, not
trusted.

What the skill fixes, and the codebook implements:

- **Inputs are exactly three** — the current statement (no status, no record, no other
  candidates), the candidate's finding and source lines, and the cited source excerpt
  fetched by the promotion session into the job's inputs. The referee never searches, and
  never sees a clause anyone else wrote for the candidate: the fresh attempt is the
  mechanism of its independence, and comparing the two afterwards is a measurement.
- **The task is to attempt the clause blind** — "if the statement were false, this source
  would have shown ___ instead" — and the three classes are `diagnostic [supporting]`,
  `diagnostic [challenging]`, `non-diagnostic`, with the tag following mechanically from
  which side of the named observable the excerpt shows.
- **Tuned to over-flag** (a false non-diagnostic costs one adjudication; a false
  diagnostic costs the record), **report-only** (the two append lines and nothing else),
  and **one target per candidate**.
- **A vacuous clause is non-diagnostic by definition, whoever wrote it** — the clause
  names what the excerpt would contain, never what the claim would be. The codebook's
  decision rules and worked examples are the operational form of this sentence.

Run through `tools/StoryPlanner.AgentRunner` from the fanout folder, Sonnet by default,
`mcp: false`, tools `Read` and `Write` only.

## Promotion

A HITL session, Fable, with the candidates file, the referee verdicts, and the hypothesis
files open. For each `diagnostic` candidate: read the cited source (not the finding — the
source) before promoting; write the evidence entry with finding and clause **verbatim**
from the candidate; append the disposition. Brian adjudicates disagreements with the
referee and may decline a diagnostic candidate (reason recorded) or promote nothing.
`non-diagnostic` candidates stay where they are, visibly, as context — they are not
promoted and not deleted. Then recompute each touched hypothesis's status from its
entries in the file's frontmatter (the index carries no status — `hypothesis-records.md`
§ Files and the index) and commit **once**: the diff is the review surface, and the
commit message names the WU and the candidate ids.

Report to Brian, after the commit — at least: candidates per target; diagnostic /
non-diagnostic / held; promoted supporting / challenging; declined with reasons; referee
disagreements and how they went; anything else the session noticed about the pipeline's
own behaviour. He decides whether the distribution warrants a recheck.

## Iteration → re-referee

When a statement changes (an iteration entry), every evidence entry under the old wording
is a candidate again: its finding and source are copied into a new candidates file for the
iteration (`fanout/referee/iterations/NNN-<date>/candidates.md`), the referee runs against the
*new* statement, and the promotion session re-promotes what survives. Alignment tags are
never edited in place, and the old entries are never deleted — the iteration entry records
which entries were re-verified and their new tags; the superseded entries stay in the
record marked `(superseded by re-referee <date>)` at the end of their line. There is no
state for "not yet re-assessed": re-assessment is the iteration's own step.

## Verification debt and codebook versioning

A corpus's exploratory pass opens a debt paid only by its verification pass. Until then
its findings are leads: no synthesis, comparison, adjudication or forward-plan rationale
cites them as evidence, and its hypotheses stay `untested` on its account. Questions flow
freely — an exploratory pass on one corpus may add to any corpus's spec pool.

Every verification result cites the codebook hash it was produced under. A codebook
revision (new hash) re-runs the affected classifier or auditor jobs; the old results are
kept, marked superseded, never re-labelled by hand. Re-runs are cheap because the work is
classifier-tier — this is what makes the strong form affordable.

## Entries this pipeline did not produce

An evidence entry with no referee line and no codebook hash was not produced by this
pipeline — whatever method wrote it. Such an entry is **unverified**: it and any status
computed from it are leads, not evidence, until the entry has been re-verified as a
candidate through the referee and a promotion session. Re-verification is verification
work and belongs on a forward-plan card, never in this file.
