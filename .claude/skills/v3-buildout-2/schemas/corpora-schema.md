# corpora-schema

`CORPORA.md` in the skill folder, the class `corpora`: the inventory of the corpora the
buildout studies, one entry per corpus, a fact file with no state in it. Written by
building-a-tool's `build` when a corpus becomes readable or how an itemizer reads it changes;
read by the plan processes for whether every corpus a study's itemizer will read is readable
and how, by an itemizer's author, and by a review writing a `corpus` shortcoming. Its entry
ids are the names a locator notation, a deliberation and a `corpus` shortcoming use for a
corpus, held by no machine check. The shape below is what the hook holds; the example is a conforming file with placeholders and the block the
checker's tests read as their fixture; then how the file is queried, and the check ids the
hook reports.

## Shape

The title is `# Corpora`.

| section | present | holds |
|---|---|---|
| the whole file after the title | required | entries, after the head, a paragraph of prose saying what the file is and where the definition of a corpus lives; may hold none |

The head paragraph between the title and the first entry is required; the entries may be
none. An entry's heading is `### <id>`, type slug, unique in the file, authored when the
corpus enters and never changed: the name a locator notation, a deliberation and a `corpus`
shortcoming use for the corpus. Then keyed lines in this order and no other line, a value continuing on lines
indented two spaces, a blank line followed by such a line continuing it too:

| key | present | type | value |
|---|---|---|---|
| `what` | required | block | what the corpus is, as primary-source data under SKILL.md § Vocabulary: whose text and of what kind; its population, naming the file that lists it where one does |
| `where` | required | block | where it lives: the file or database, by its path or by the config entry that names it; outside the repo where it is |
| `read through` | required | block | the code path an itemizer takes from `where` to items: the format, the reader or library in the repo, and where its schema or reading rules are documented; a converter where one runs first |
| `caveats` | optional | block | what a reader must know that the three lines above do not say: what is excluded from the corpus; what sits beside it that is not corpus data; reading semantics; format quirks; provenance limits; absent when there is nothing to say |

No progress, readiness, deferral, judgment or count is written here: where a study of a
corpus stands is derived into `state.md`, a readiness date is a decision, and a count is
answered by the source it names. Nothing here names how a session or the MCP server reads a
corpus, which the planner's `corpora`, `storyplan-data` and `code-sessions` skills carry; an
itemizer is named per batch in its index head, never here.

## Example

```markdown
# Corpora

<what the file is, and where the definition of a corpus lives>

### fimfiction-stories

- what: <what the corpus is, as primary-source data; its population, naming the file that lists it>
- where: <the files, by path; outside the repo where they are>
- read through: <the format, the reader or library in the repo, and where its schema is documented>
- caveats: <what is excluded; what beside it is not corpus data; reading semantics; format quirks; provenance limits,
  continuing on an indented line where it runs long>

### code-sessions

- what: <what the corpus is>
- where: <the database, by the path or the config entry that names it>
- read through: <the format, the library, and where the schema and reading rules are documented>
```

## Queries

| question | how |
|---|---|
| every corpus id | `grep -n '^### ' CORPORA.md` |
| one corpus's entry | `grep -n -A8 '^### <id>' CORPORA.md` |
| how an itemizer reads a corpus | its `- read through:` line and continuation |
| what a corpus excludes, and what beside it is not corpus data | its `- caveats:` block |
| every batch whose itemizer read a corpus | `grep -rn '^- itemizer:' */*/batches/*/index.md`, then each tool's code at the version named, for the corpora it reads |
| how a session or the MCP server reads a corpus | not here: the planner's `corpora`, `storyplan-data` and `code-sessions` skills |

## Checks

| check | fails when |
|---|---|
| `corpora.title` | the title is not `# Corpora` |
| `corpora.shape` | the head paragraph is missing, a line in an entry is neither keyed nor continuation, or a heading other than an entry's appears |
| `corpora.entry` | an entry heading is not a lowercase slug or repeats one; a field is missing, unknown, out of order or of the wrong type; or a required field has no value |
