# revising-the-method

Enables preparing-to-explore-a-corpus and preparing-to-verify-a-corpus.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| revise | hitl | ProcessMap git | skill map state revision-note rulings-log | skill rulings-log revision-note map | built | Brian rules, the session edits the skill's tables and prose in place, the validator passes, the rulings are logged as made and the note is written once at the end |

<!-- generated:activity -->
<!-- /generated -->

## Preconditions

A finding about the method itself: the validator reports a gap, a run shows a rule does not
hold, an activity file is found wanting in use, or Brian's judgment that the method is
wrong-shaped. A revision is never triggered by a hypothesis's content.

## revise

The session names the finding and the rows or prose it touches. Brian rules; each ruling
is appended to the revision's rulings log as it is made, in his words where he gave them,
with the reason. The session applies each ruling as a row edit and a prose edit together,
in the activity file or `artifacts.md` or this router, never one without the other; a
schema change (a column, a closed set, a validator rule) is a change to `SKILL.md` § Schema
and to the tool with its fixtures, and is rare. After every step `validate` runs over the
skill folder and must pass before the next; `render` regenerates the marked sections and
`map.md`. When the revision is done, the session writes the write-once revision note: what
prompted it, the rulings, the activities and processes changed by id from the tables'
diff, what was deliberately not adopted, what is owed. One commit per landed step; the
note lands with the last.

A revision that replaces the skill wholesale, as revision 2 did, is built in a sibling
folder and swapped in one commit when the validator passes; a revision that changes rows
is made in place.

## Never

Edits a generated section by hand; keeps two copies of a table; changes a row without its
prose or prose without its row; rewrites a rule while moving it; treats any row as settled.
