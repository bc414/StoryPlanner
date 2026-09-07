# study-registry-schema

`docs/v3-framework/studies.md` — one row per study of a chain, appended by the
preparing activity at the moment Brian approves its plan, which is his go. Never edited: a
study abandoned is a fact the artifacts show, not a row removed. Every artifact path a
study produces is named by its id.

```markdown
| id | type | corpus | go |
|---|---|---|---|
| exploration-of-v1-archive | exploratory | v1-archive | 2026-09-12 |
| round-of-fimfiction-stories-1 | verification | fimfiction-stories | 2026-09-20 |
```

`id` is `exploration-of-<corpus>[-<n>]`, `round-of-<corpus>-<n>`, or `referee-<n>`; a
round always carries its ordinal, an exploration only when the corpus is explored again,
and the referee's each time its codebook is prepared anew. `type` ∈ `exploratory |
verification` names the chain: exploratory runs preparing-to-explore-a-corpus,
exploring-a-corpus and reviewing-leads; verification runs preparing-to-verify-a-corpus,
conducting-a-verification-round, writing-candidates-from-verification,
refereeing-a-candidate and promoting-checked-candidates. `referee-<n>` is a verification
study that stops after preparing: its corpus is `candidates`, its folder is
`fanout/referee/`, and what it produces — the codebook, its calibration, the
materialise itemizer — is what every referee run under every round then uses. It is
prepared once and again only when a ruling changes the codebook; that is the whole of
what "bootstrap" means here, and it is a fact about studies, never about activities.
`corpus` is a name from `CORPORA.md`, `verified-artifacts` for an exploration over
the buildout's own outputs, or `candidates` for the referee. Nothing else is authored
here: where a study stands is derived by the tool from its artifacts into `state.md`,
and a tool a study needs is built as its first task.
