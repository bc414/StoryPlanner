# Spec pools

One file per corpus (`<corpus>.md`). The seven corpora of the buildout as of 2026-09-03,
each with a pool file here: `analysis-corpus.md` (the 112 analyzed stories),
`own-fiction.md`, `v1-archive.md`, `working-plan.md` (v2), `lineage.md`,
`conversations.md`, `code-sessions.md`. A corpus added later gets a file the same way. A
spec pool is where an
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
