# declined-candidates-schema

`docs/v3-framework/studies/<study>/declined-candidates.md`, one per verification, the class
`declined-candidates`, appended by promoting-refereed-candidates when Brian declines a
diagnostic candidate. The shape below is what the hook holds; the example is a conforming file
a writer fills in and the block the checker's tests read as their fixture; then how a list is
queried, and the check ids the hook reports.

## Shape

The title is `# <study> — declined candidates`, where `<study>` is the id of the study whose
folder holds the file, a verification's id in the registry.

| section | present | holds |
|---|---|---|
| the whole file after the title | required | entries; no head prose, no sections |

An entry's heading is `### <finding-slug> → <target>`, where `<finding-slug>` is the slug half
of the finding's `### <study>/<slug>` token in this study's `findings.md`, the study implied by
the file, and `<target>` is the hypothesis file's name, `NNN-<slug>`. The pair (finding, target)
is a candidate; it is unique in the file. Then keyed lines in this order and no other line, a
value continuing on lines indented two spaces:

| key | present | type | value |
|---|---|---|---|
| `date` | required | date | the day Brian declined; never earlier than the entry before it |
| `reason` | required | block | Brian's reason for declining; under rule 10 |

A declined candidate is terminal: there is no withdrawal or supersession here. A candidate is
promoted (its evidence in the hypothesis record), declined (here), or still pending (in
neither); promotion is never recorded here — it is found through the hypothesis records. The
generated `candidates.md` reads this file for a candidate's declined status and reason.

## Example

```markdown
# verification-of-v1-archive-scene-stasis — declined candidates

### unplaced-notes-are-paratext → 031-dt-knowledge-asymmetry

- date: 2026-09-22
- reason: <Brian's reason for declining>

### event-notes-cluster-early → 029-perception-gap-delivery

- date: 2026-09-22
- reason: <Brian's reason>
```

## Queries

| question | how |
|---|---|
| every candidate declined in a study, and why | `grep -n '^### ' studies/<study>/declined-candidates.md`, then each `reason` |
| whether one candidate was declined | its `### <finding-slug> → <target>` heading is present in the study's file |
| which candidates against one hypothesis were declined | `grep -rn ' → <NNN-slug>$' studies/*/declined-candidates.md` |

## Checks

| check | fails when |
|---|---|
| `declined-candidates.title` | the title is not `# <study> — declined candidates` with the id of the study whose folder holds the file, or that study is not a verification |
| `declined-candidates.shape` | there is head prose or a `##` section, or a line sits outside every entry |
| `declined-candidates.entry` | a heading is not `### <finding-slug> → <target>` with a lowercase finding-slug; the (finding, target) pair repeats; a key is missing, unknown or out of order; a line in an entry is neither keyed nor continuation; the date is not `YYYY-MM-DD` or is earlier than the entry before it; `reason` is empty |
| `declined-candidates.references` | the finding-slug names no standing finding in this study's `findings.md`, or the target names no hypothesis file `NNN-slug.md` |
