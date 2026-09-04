# Spec pools

One file per corpus (`<corpus>.md` — e.g. `v1-archive.md`, `own-fiction.md`,
`analysis-corpus.md`, `lineage.md`, `working-plan.md`). A spec pool is where an
**exploratory** pass, or a post-WU review, deposits the *questions* it raised about that
corpus — never findings, never evidence. Verification passes are built from the pool.

An entry is one question:

```
### <short title>
- asked-by: WU<n>.<m> (<date>)
- bears-on: <hypothesis ids>
- question: <one testable question about this corpus>
- candidate-predicate: <if a frozen predicate suggests itself; may be blank>
- status: open | folded-into <codebook name> | answered-by WU<n>.<m> | withdrawn (<reason>)
```

Rules: questions flow freely — any pass may add to any corpus's pool; findings wait —
nothing here counts toward a hypothesis. Entries are appended, never rewritten; a
superseded entry changes only its `status` line. Governed by the `v3-buildout` skill
(`wu-execution.md`).

Created 2026-09-03 (methodology revision 1).
