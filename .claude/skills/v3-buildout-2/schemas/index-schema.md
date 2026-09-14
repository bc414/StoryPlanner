# index-schema

`docs/v3-framework/<container>/<home>/batches/<batch>/index.md`, the class `index`, frozen:
written once by the batch's itemizer or collator, the list of the items a batch judges and, for
each, what retrieves it from its source. The shape below is what the hook holds; the example is
a conforming file an itemizer writes and the block the checker's tests read as their fixture;
then how an index is queried, and the check ids the hook reports.

## Shape

The title is `# <batch> — index`, `<batch>` the folder's own name.

| section | present | holds |
|---|---|---|
| the head, after the title | required | fields |
| the table, after the head | required | table, one row per item, in the order the tool produced them |

**Head**:

| key | present | type | value |
|---|---|---|---|
| `itemizer` | optional: exactly one of `itemizer` and `collator` | line | the itemizer that cut corpora into the items, as `tools/StoryPlanner.<Name>` or a script's path, and its version or commit |
| `collator` | optional: exactly one of `itemizer` and `collator` | line | the collator that collated the items from the method's own files, as `tools/StoryPlanner.<Name>`, and its version or commit |
| `corpus` | optional: present exactly when `itemizer` is | enum: the ids in CORPORA.md | the corpus the items were cut from |
| `utilizes corpora` | optional: only beside `itemizer` | list of enum: the ids in CORPORA.md | the other corpora the itemizer read to cut, label or fill the items; never the `corpus` value |
| `narrowing` | optional: when the tool does not take every item | line | in words, the condition in the tool's code that chooses which items the batch holds |
| `locator notation` | required | line | how this tool writes a locator, in words: what parts it has and how the source's reader takes them; the notation is the tool's and is reviewed at its first index |
| `source hash` | optional: when the source is one document | hash | the document the itemizer cut, so a regenerated body can be checked against it |

**Table**:

| column | present | type | value |
|---|---|---|---|
| `item` | required | slug | the item's id; the key of the item's body `items/<item>.md`, of its call entries, of its result file and of every citation `<home>/<batch>/<item>` |
| `locator` | required | line | what retrieves the item from its source, in the head's locator notation: for an itemizer's batch through the reader CORPORA.md names; one part or several as the source needs, a note id, a file and a range, a layer id and a turn, a finding's token |
| `description` | required | line | what the item is, for a reader of the table |

The bodies under `items/` are uncommitted and regenerable by the tool; the index is the
committed list of what they were, and each call hashes the body it received. Nothing in the
index is written by hand, and nothing in it is derived from a result. An itemizer reads corpora
and nothing else, one or several, and `utilizes corpora` names the ones it read besides the corpus
it cut; a collator reads the method's own files its activity names, and its head names no corpus.
No state of an input is recorded.

## Example

```markdown
# 03-scene-notes — index

- itemizer: tools/StoryPlanner.ArchiveItemizer, 2026-09-19 a1b2c3d
- corpus: v1-archive
- utilizes corpora: lineage
- narrowing: the notes whose text a lineage response contains verbatim or as an edited or framed paste, in the model role
- locator notation: a v1-archive note id, `note-<id>`, the archive database's note id

| item | locator | description |
|---|---|---|
| note-1630 | note-1630 | Griffonian Republic, History track, 2026-03 |
| note-1702 | note-1702 | Grover III's Enlightenment, Causality of Creation |
```

## Queries

| question | how |
|---|---|
| the items of a batch, in order | `grep -n '^| ' <container>/<home>/batches/<batch>/index.md` |
| how to retrieve an item from its source | its row's `locator`, read under the head's `locator notation` line; for an itemizer's batch through the reader CORPORA.md names |
| which tool wrote a batch, and from which corpus | the head's first lines: `itemizer` and `corpus`, or `collator` |
| every batch that cut a corpus | `grep -rln '^- corpus: <corpus>' studies/*/batches/*/index.md` |
| every batch whose itemizer utilized a corpus | `grep -rlnE '^- utilizes corpora: (.* )?<corpus>( |$)' studies/*/batches/*/index.md` |
| what a batch holds of its source | the head's `narrowing` line; absent, every item |
| every batch an itemizer or collator wrote | `grep -rlnE '^- (itemizer|collator): <tool>' */*/batches/*/index.md` |
| whether an item has a result | `results/<item>.md` beside the index, or the batch's `calls.md` |

## Checks

| check | fails when |
|---|---|
| `index.title` | the title is not `# <batch> — index` with the folder's own name |
| `index.head` | a head key is missing, unknown or out of order; both or neither of `itemizer` and `collator` are present; `corpus` is present without `itemizer` or absent with it; `corpus` or a `utilizes corpora` value is not an id in CORPORA.md, or `utilizes corpora` sits beside a collator or repeats `corpus`; `source hash` is present and not a SHA-256 |
| `index.table` | the table's columns are not `item · locator · description`, a line sits outside the head and the table, or the table is empty |
| `index.item` | an item id is not a lowercase slug, or repeats in the index |
| `index.locator` | a locator is empty |
