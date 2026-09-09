# index-schema

`docs/v3-framework/studies/<study>/batches/<batch>/index.md`, the class `index`, frozen:
written once by the batch's itemizer, the list of the items a batch judges and, for each,
what retrieves it from the corpus. The shape below is what the hook holds; the example is
a conforming file an itemizer writes and the block the checker's tests read as their
fixture; then how an index is queried, and the check ids the hook reports.

## Shape

The title is `# <batch> — index`, `<batch>` the folder's own name.

| section | present | holds |
|---|---|---|
| the head, after the title | required | fields |
| the table, after the head | required | table, one row per item, in the order the itemizer produced them |

**Head**:

| key | present | type | value |
|---|---|---|---|
| `itemizer` | required | line | the tool that wrote the index, as `tools/StoryPlanner.<Name>` or a script's path, and its version or commit |
| `corpus` | required | enum: the ids in CORPORA.md, `candidates`, `skill` | the corpus the items were cut from; `candidates` for the referee's, `skill` for an audit's |
| `locator notation` | required | line | how this itemizer writes a locator, in words: what parts it has for this corpus and how the corpus's reader takes them; the notation is the itemizer's and is reviewed at the itemizer's first index |
| `source hash` | optional: when the corpus is one document | hash | the document the itemizer cut, so a regenerated body can be checked against it |

**Table**:

| column | present | type | value |
|---|---|---|---|
| `item` | required | slug | the item's id; the key of the item's body `items/<item>.md`, of its call entries, of its result file and of every citation `<study>/<batch>/<item>` |
| `locator` | required | line | what retrieves the item from the corpus through the reader CORPORA.md names, in the head's locator notation; one part or several as the corpus needs, a note id, a file and a range, a layer id and a turn |
| `description` | required | line | what the item is, for a reader of the table |

The bodies under `items/` are uncommitted and regenerable by the itemizer; the index is
the committed list of what they were, and each call hashes the body it received. Nothing
in the index is written by hand, and nothing in it is derived from a result.

## Example

```markdown
# 03-scene-notes — index

- itemizer: tools/StoryPlanner.ArchiveItemizer, 2026-09-19 a1b2c3d
- corpus: v1-archive
- locator notation: a v1-archive note id, `note-<id>`, as the MCP archive tools take it

| item | locator | description |
|---|---|---|
| note-1630 | note-1630 | Griffonian Republic, History track, 2026-03 |
| note-1702 | note-1702 | Grover III's Enlightenment, Causality of Creation |
```

## Queries

| question | how |
|---|---|
| the items of a batch, in order | `grep -n '^| ' studies/<study>/batches/<batch>/index.md` |
| how to retrieve an item from the corpus | its row's `locator`, read under the head's `locator notation` line, through the reader CORPORA.md names |
| which itemizer and corpus a batch used | the head's first two lines |
| every batch that cut a corpus | `grep -rln '^- corpus: <corpus>' studies/*/batches/*/index.md` |
| every batch an itemizer wrote | `grep -rln '^- itemizer: <tool>' studies/*/batches/*/index.md` |
| whether an item has a result | `results/<item>.md` beside the index, or the batch's `calls.md` |

## Checks

| check | fails when |
|---|---|
| `index.title` | the title is not `# <batch> — index` with the folder's own name |
| `index.head` | a head key is missing, unknown or out of order; `corpus` is not an id in CORPORA.md, `candidates` or `skill`; `source hash` is present and not a SHA-256 |
| `index.table` | the table's columns are not `item · locator · description`, a line sits outside the head and the table, or the table is empty |
| `index.item` | an item id is not a lowercase slug, or repeats in the index |
| `index.locator` | a locator is empty |
