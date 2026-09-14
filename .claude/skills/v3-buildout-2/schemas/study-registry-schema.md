# study-registry-schema

`docs/v3-framework/studies.md`, the buildout's study registry, the class `studies`: one
entry per study, appended by the plan process of the preparing activity at the moment Brian
approves the plan, and never edited. A study's folder under `docs/v3-framework/studies/` is
named by its id, and the id is what every path and citation of the study carries. The shape
below is what the hook holds; the example is a conforming file and the block the checker's
tests read as their fixture; then how the registry is queried, and the check ids the hook
reports.

## Shape

The title is `# Studies`.

| section | present | holds |
|---|---|---|
| the whole file after the title | required | entries, one line each; no head prose, no sections; may hold none |

An entry is `- <id>`, one per line, in the order the studies were approved. `<id>` is
`<type>-of-<question>` or `<type>-of-<question>-<slug>`:

- `<type>` is `verification` or `exploration`, and names the chain the study runs: an
  exploration runs preparing-an-exploration, conducting-an-exploration and reviewing-leads; a
  verification runs preparing-a-verification, conducting-a-verification, reviewing-findings,
  surfacing-candidates and promoting-refereed-candidates.
- `<question>` is the slug of an entry in the question list, `questions/<question>`: the one
  question the study is of. Question slugs may contain hyphens and one may be a prefix of
  another, so the question is the longest slug heading an entry in the list that the id
  carries after `<type>-of-`.
- `<slug>`, present only for a further study of the same question and type, is a lowercase
  slug naming what sets the study apart: the model, where the model is what differs; the
  itemizer or the directions otherwise. The first study of a question and type carries none
  and is never renamed when a second arrives.

The id is authored at approval and never changed. Nothing else is authored here: the study's
type and question are read from the id; its model, effort, itemizer and directions from its
batches' definitions and index heads; where it stands is derived by the tool into
`state.md`. A study abandoned is a fact its folder shows, not an entry removed. The pipeline
directions, the referee's and claiming's, are no study and have no entry; their folders sit
under `docs/v3-framework/pipeline/`. An iteration's re-verification is no study either and
is read from its folder under `docs/v3-framework/iterations/`.

## Example

```markdown
# Studies

- exploration-of-scene-detail-scrap-rate
- verification-of-scene-detail-scrap-rate
- verification-of-scene-detail-scrap-rate-opus
```

## Queries

| question | how |
|---|---|
| every study, in approval order | `grep -n '^- ' studies.md` |
| every study of one question | `grep -n '^- [a-z]*-of-<question>' studies.md`, each hit read under the longest-match rule |
| the studies of one type | `grep -n '^- verification-of-' studies.md`, or `^- exploration-of-` |
| a study's question | the id's segment after `<type>-of-`, the longest slug heading an entry in `questions.md`; `grep -n '^### questions/<question>' questions.md` |
| where a study stands | `state.md` § Studies |
| a study's model, effort, itemizer and directions | its batches under `studies/<study>/batches/`: each `definition.md` and `index.md` head |
| whether a study is registered | `grep -n '^- <study>$' studies.md` |

## Checks

| check | fails when |
|---|---|
| `registry.title` | the title is not `# Studies` |
| `registry.shape` | a line after the title is other than a `- ` entry or blank: a heading, a table, prose |
| `registry.id` | an entry is not `verification-of-<question>[-<slug>]` or `exploration-of-<question>[-<slug>]`, where `<question>` is the longest slug heading an entry in the question list and `<slug>`, when present, is lowercase `[a-z0-9-]+` |
| `registry.duplicate` | an id appears twice |
| `registry.questions-unavailable` | reported, never failed: the question list could not be read, so the question segment is not checked |
