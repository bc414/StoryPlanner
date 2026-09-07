# codebook

`fanout/<study>/codebook-N.md` for a corpus; `fanout/referee/codebook-N.md` for the
referee. The frozen instrument an agent applies to one item with no discretion: all
judgment was spent writing it. Authored in preparing-to-verify-a-corpus against real items,
calibrated before any batch, versioned by number; every edit is a new file, a new hash and
a new calibration. It carries no status line: whether a version is calibrated is read from
a calibration existing at its hash, never from the file, since any line in the file
is part of the hash. The runner inlines it as the agent's entire context, so it must be
complete in itself and must not restate what the process row already says about its
inputs: what the agent is given is the `reads` of the agent process in the activity file,
materialised by the generator, and the codebook names it by reference. Its `## Questions`
section is the one authored place a codebook names the questions it freezes; `state.md`
derives a question's coverage from that section and a calibration at the hash.

```markdown
# Codebook — <name> (version N)

## Item
<what one item is, as the itemizer produces it; the frozen predicate's unit>

## Questions
<the titles, verbatim, of the entries in questions/<corpus>.md this version freezes; none
for the referee, whose question is the pipeline's own>

## Inputs
<by reference: the agent process row in <activity>.md; the item file's headings>

## Output
<the exact lines to write, with the markers the output contract checks; nothing else>

## Classes
<the closed set of labels, each defined by what the item shows, not by what it means>

## Decision rules
<numbered; the boundary cases, each resolved one way; tuned to over-flag where a false
negative costs the record and a false positive costs one adjudication>

## Anchors
<under the rule each anchors: an item from a calibration disagreement, its ruled label,
and the calibration it came from; none until a calibration has produced one>
```

The referee's codebook is this shape with `Item` a candidate's finding beside the target's
current statement, `Output` the falsifier line and the verdict line, and `Classes` the three
verdicts: diagnostic supporting, diagnostic challenging, non-diagnostic. A vacuous
falsifier, one that restates the claim instead of naming what the finding would have been,
is non-diagnostic by definition.
