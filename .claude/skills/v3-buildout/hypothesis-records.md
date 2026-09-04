# Hypothesis records

Read in full before touching any file in `docs/v3-framework/hypotheses/`. The pipeline that
decides *what may be written* is `evidence-pipeline.md`; this file is the *shape* of the
files and the ceremony around changing them.

## Files and the index

`docs/v3-framework/hypotheses/NNN-slug.md` — `NNN` a zero-padded three-digit id, stable,
unique across the whole set, never reused after supersession; `slug` descriptive kebab-case. `INDEX.md` in the same
directory is a routing table — two columns (ID, slug as link), no summaries, in id order so
a top-to-bottom scan is comprehension order. It changes only when a file is minted or
superseded. **It carries no status or baselined column** — those lived there until
2026-09-04 and were removed as a hand-kept mirror of frontmatter that nothing checked
(the same failure as forward-plan-2's coverage table). Status is read from the files:

```
grep -H '^status:' docs/v3-framework/hypotheses/0*.md          # every status
grep -l '^status: challenged' docs/v3-framework/hypotheses/0*.md # one state
grep -h '^status:' docs/v3-framework/hypotheses/0*.md | sort | uniq -c   # the counts
```

```markdown
---
id: 17
status: evidenced
baselined: false
created: 2026-09-01
---

## Hypothesis

[1–3 sentences. A testable prediction readable in isolation.]

## Record

[Chronological, oldest first, newest appended.]
```

`status` ∈ `untested | evidenced | challenged`; `baselined` is `false` or an ISO date;
`created` never changes. No other frontmatter fields — "tested by", "related", and the
like are staleness targets. Which WUs target a hypothesis lives in the forward plan;
connections between hypotheses live in prose.

## The statement

What the hypothesis predicts, and only that. If it explains *why* the prediction exists,
that is founding reasoning and belongs in the `created` entry. No provenance, no
implications, no testing method, no confirm/refute conditions.

## Record entries

Four kinds, each one citable unit; a two-finding entry is two entries. Full ISO timestamps.

**`created`** — always first. Why the hypothesis exists: the observation, Brian's
assertion, the motivation, in Claude's voice with Brian's assertions as the content. It
does not name the document it was extracted from (the consolidation report holds that
chain) and does not contain testing method.

```
- created | 2026-09-01T10:00: <reasoning>
```

**`evidence`** — written only by a promotion session, only from a referee-checked candidate
(`evidence-pipeline.md`). The entry carries, in order: the source WU and candidate id, the
codebook version when a classifier or auditor produced the finding, the alignment tag, the
finding, and the discrimination clause verbatim from the candidate.

```
- evidence | 2026-09-14T15:20 | (WU2.3 C-014; codebook dt-classes@3f9a1c) [supporting]:
  <finding, with counts, ids, excerpts as the finding requires>
  Would differ if false: <the observable the corpus would have shown instead>
```

The clause is not decoration — it is the referee's verdict written where it can be
re-checked. An entry without one is malformed. Alignment is `supporting` or
`challenging`; there is no third tag. Evidence is grounded in corpora or verifiable
sources — which excludes, among other things, Brian's recall about his own practice, a
restatement of the statement, and an intermediate analysis's classification that no one
read back to the source.

**`iteration`** — a change to the statement, with the reason and, when evidence prompted
it, the citations. Record which evidence entries were re-verified as a consequence (see
`evidence-pipeline.md` § Iteration — alignment tags are never edited in place).

```
- iteration | 2026-09-16T09:15: Narrowed from "…" to "…" after C-014/C-015.
  Entries 2026-09-14T15:20 and 2026-09-14T15:24 returned to candidate status for re-referee;
  re-promoted 2026-09-16T11:40 as [supporting] / [challenging].
```

**`baselined`** — Brian's judgment and rationale. Written only by Brian or at his explicit
direction in his words.

```
- baselined | 2026-09-20T16:00: <rationale>
```

Grep: `^- created`, `^- evidence`, `^- iteration`, `^- baselined`, `\[challenging\]`.

## What never enters a record

Including, and not limited to:

- Brian's recall about his practice (a question for a spec pool).
- Brian's story-design observations (story content).
- Observations that do not change the statement (a spec-pool question, or nothing).
- Methodological pointers ("WU2.4 should check this" — the forward plan or spec pool).
- Findings from an exploratory pass — however relevant. They live in the WU artifact and,
  as questions, in the spec pool. The strong form has no exception for "obviously true".

Test: removing the entry would leave the record incomplete (a statement change, verified
evidence, a baseline event missing) — it belongs. It would only lose a pointer to future
work — it does not.

## Ceremony scaling

- **Minor** (wording tightened, no conceptual change): edit the statement; one-line
  iteration entry. Evidence entries whose clause still discriminates stand — say so.
- **Significant** (scope changed; evidence prompted a rethink): full iteration entry with
  citations; every evidence entry returns to candidate status for re-referee
  (`evidence-pipeline.md`); status re-derived from what is re-promoted; the `created`
  entry amended only if the reframing makes it misleading.
- **Structural** (split, merge, supersede): new file(s); final iteration entry in the old
  file naming what replaced it; the old file's status becomes `challenged` (supersession is
  a pattern of challenge, not a fourth state); index updated. Consolidation territory.

## Challenging a hypothesis

A challenge is evidence: a specific counterexample with ids — a note, a block, a lineage
id, a passage, a corpus finding, or any other citable locus — that goes through the
pipeline like any other candidate and
lands as a `[challenging]` entry. "This doesn't feel right" is not a challenge; it is a
spec-pool question at most. Brian's challenges carry his analytical voice and are
engaged by grounding, never elaborated away.

## Creating a hypothesis

The same file protocol applies whether one hypothesis is created in conversation or forty
in a consolidation. Three criteria, all required: **novelty** (it is not evidence for an existing hypothesis),
**testability** (evidence could confirm or refute it), **independence** (it is not a
refinement — refinements are iteration entries). Brian's explicit statements always get
the offer. Claude's analysis may surface a candidate only when all three hold, and the
proposal cites the specific evidence or Brian's statement that prompted it — not a
synthesis. Brian reviews the *statement*, rewrites it in his words or approves; the
`created` entry records the provenance ("originated from Claude's reading of …; Brian
endorsed on …"). During a WU's primary work, hold proposals for the wrap-up; in every other
session type, propose when the three criteria hold.

The trap this guards (the v1 pattern): Claude proposes → Brian nods → the hypothesis
enters in Claude's framing → later sessions build on that framing → Brian's own thinking is
channelled. Provenance lets a future session tell Brian-originated from Claude-originated.
