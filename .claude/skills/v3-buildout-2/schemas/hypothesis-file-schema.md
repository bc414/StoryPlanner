# hypothesis-file-schema

`docs/v3-framework/hypotheses/NNN-slug.md` — `NNN` a zero-padded three-digit id, stable,
unique across the set, never reused; `slug` descriptive kebab-case. One file holds three
artifacts, each with its own mutation:

```markdown
---
id: 17
status: evidenced
baselined: 2026-09-20
created: 2026-09-01
---

## Hypothesis

[1–3 sentences. A testable prediction readable in isolation. Edited in place.]

## Record

- created | 2026-09-01T10:00: <why the hypothesis exists: the observation, Brian's
  assertion, the motivation; in Claude's voice with Brian's assertions as the content>
- iteration | 2026-09-10T09:15: Reworded from "…" to "…" because <reason>. Entries above
  this line are bound to the prior wording.
- evidence | 2026-09-14T15:20 | (verification-of-fimfiction-stories-1/fid-primary-in-one-of-seven; codebook-3@3f9a1c) [supporting]:
  <the finding, verbatim from the candidate>
  Falsifier: <verbatim from the referee's line>
- baselined | 2026-09-20T16:00: <Brian's rationale, in his words>
```

**Frontmatter** (`hypothesis-status`, in-place): `id`; `status` ∈ `untested | evidenced |
challenged`, computed from the entries bound to the current wording — `challenged` if any
such challenging entry is unresolved, `evidenced` if any such evidence entry exists,
`untested` otherwise; `baselined`, `false` or an ISO date, reset to `false` when a
challenging entry lands or the wording changes; `created`, never changed. No other fields.

**Statement** (`hypothesis-statement`, in-place): what the hypothesis predicts, and only
that. Founding reasoning belongs in the `created` entry; provenance, implications, testing
method and confirm/refute conditions belong nowhere in this file.

**Record** (`hypothesis-record`, append): the evidence relationship. Four entry kinds, each
one citable unit, `- <kind> | YYYY-MM-DDTHH:MM` with continuation lines indented two spaces,
the first entry always `created`, grep-able by `^- created`, `^- evidence`, `^- iteration`,
`^- baselined`.

An `evidence` entry is written only by a promotion session from a referee-checked candidate
and carries the candidate's token, `<study>/<slug>`, the codebook version and hash, the alignment
tag (`supporting` or `challenging`, no third tag), the finding and the falsifier verbatim.
An entry without a falsifier is malformed. An `iteration` entry is a wording boundary:
nothing above it is invalidated or re-tagged, and nothing above it counts toward the
status until re-verified against the new wording; the prior entries' findings are queued
as iteration candidates for the next verification. A `baselined` entry is written only by Brian or
at his explicit direction in his words. Entries are never edited; there is no superseded
marker, because binding to a wording is read from position relative to iteration entries.

What never enters a record: Brian's recall about his practice (a question); story-design
observations (story content); observations that do not change the statement; pointers to
future work (a question list); leads from an exploration, however relevant.
Test: removing the entry would leave the evidence relationship incomplete — it belongs.
