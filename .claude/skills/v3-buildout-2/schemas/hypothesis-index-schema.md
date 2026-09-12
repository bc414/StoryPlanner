# hypothesis-index-schema

`docs/v3-framework/hypotheses/INDEX.md` — a routing table, two columns (id, slug as link),
in id order so a top-to-bottom scan is comprehension order, every file in the folder listed
and every link resolving. It changes only when a file is minted: nothing retires a hypothesis,
and one whose challenge is never resolved stands as a disproven hypothesis rather than leaving
the set. It carries no status or baselined column; status is derived from each file's record
and rendered for the whole set by `state.md`.

```markdown
| ID | Slug |
|----|------|
| 001 | [planner-purpose-trajectories](001-planner-purpose-trajectories.md) |
| 002 | [epistemic-method-provenance](002-epistemic-method-provenance.md) |
```
