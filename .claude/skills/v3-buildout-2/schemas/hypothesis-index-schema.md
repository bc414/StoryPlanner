# hypothesis-index-schema

`docs/v3-framework/hypotheses/INDEX.md` — a routing table, two columns (id, slug as link),
in id order so a top-to-bottom scan is comprehension order, every file in the folder listed
and every link resolving. It changes only when a file is minted or superseded. It carries no
status or baselined column; status is read from the files
(`grep -h '^status:' docs/v3-framework/hypotheses/0*.md | sort | uniq -c`).

```markdown
| ID | Slug |
|----|------|
| 001 | [planner-purpose-trajectories](001-planner-purpose-trajectories.md) |
| 002 | [epistemic-method-provenance](002-epistemic-method-provenance.md) |
```
