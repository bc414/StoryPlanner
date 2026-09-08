# revising-the-method

Enables preparing-to-explore-a-corpus and preparing-to-verify-a-corpus.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| revise | hitl | DocIntegrity git | skill runner-skill map state revision-note decisions results tally-output | skill runner-skill decisions revision-note map | built | Brian decides, the session edits the skill's tables and prose in place under the hook, each decision is recorded as it lands, the audit's tally is adjudicated with him, and the note is written once at the end |
| audit-run | session | runner generator tallier | skill audit-protocol | items items-manifest jobs ledger run-page tally-output | built | The second lint, after the validator, for a rewrite: the prior text split into units by the runner, one job per section against the new folder, the results tallied |
| audit-judge | agent | | audit-protocol items skill | results | built | One section's units against the new folder under the protocol's three questions; the only writer of results |

## Preconditions

A finding about the method itself: the validator reports a gap, a run shows the method's
text does not hold, an activity file is found wanting in use, or Brian's judgment that the method is
wrong-shaped. A revision is never triggered by a hypothesis's content.

## revise

The procedure serves what Brian asked for: "I want rigor instead of less friction now and
more churn later."

A unit of revision opens with its decision list: the decisions the session proposes, one
line each, ordered by the session widest-reaching first with each dependency on an earlier
one named; Brian reorders, removes or adds, and the first decision is taken only after he
has seen the list.
For each decision the session states the frame before any option:

- the finding;
- what exists under the change, in files, rows, code, tests and decisions;
- the precedent, by a grep of `decisions.md` and a reading of the closed founding record;
- the words the finding and the options will use, checked against SKILL.md § Vocabulary
  and against their ordinary sense; a word that collides is split by its own decision
  before this one;
- the audit units touched, if the revision has a tally.

Then the options, each with what it costs and buys, the session's recommendation marked
as its own and listed first; one decision at a time, never one protocol for sign-off.
Brian decides; each decision is written to `decisions.md` as it lands. The session
applies each decision as a row edit and a prose edit together,
in the activity file, a schema file or this router, never one without the other; a
ruling about how agent jobs are run lands in the `agent-runner` skill the same way; a
schema change (a column, a closed set, a check) is a change to
`schemas/skill-schema.md` and to the tool with its fixtures, and is rare. A decision that
adds or changes a check names the check's id; before a check is added or changed, the
session greps `decisions.md` for the id, and a check that no decision names and the closed
founding record does not describe is unbacked: it gets a decision or is dropped. The first
file written under a new or changed schema, and the first run of a new or changed
procedure, is presented to Brian against its declaration before a second is made; a
discrepancy is a decision not yet made, never a fix to the instance.

A schema review is a unit of revision: it opens with its decision list and each decision
is framed as above. The schema is written to the shape of schema files in
`schemas/skill-schema.md`; its checker is built or amended under building-a-tool, its
first run over the real files predicted before it runs; its Checks section lists what the
checker holds, and the review's decisions name those check ids; the class's files already
on disk are brought to the schema or the class starts again, decided in the review; the
first file written under the schema is reviewed before a second; and the class's readers
and writers, every process and schema `map.md` lists as its consumers, have their
instructions brought current in the same write: an activity file says what an entry
carries in words and names no field, a schema that cites the class cites it in the form
fixed here ahead of its own review, and a grep of the folder for the old names and
citation forms closes the step.

Two lints gate a revision. The first is structure: the write hook runs `check`
(`process-docs/StoryPlanner.DocIntegrity`) over the skill folder at every write and
regenerates `map.md` and `state.md` on a pass; a failure is fixed before the next edit. The
second is content, and it runs only for a revision that rewrites the skill wholesale: every
unit of the prior text
is judged against the new folder by `audit-run` and `audit-judge`, and the session
adjudicates the tally with Brian. Each unit the audit reports narrowed, reversed or absent
gets one line in the revision note's omissions list with his decision — restored to the new
text, or superseded and why; a unit reported restated or broadened gets no line. The
tally is the adjudication's worklist and a decision's prose names the units it resolved;
done is every flagged unit named. A revision that only changes rows runs the first lint
alone.

When the revision is done, the session writes the write-once revision note from the
revision's section of `decisions.md`: what raised it, the decisions, the activities and
processes changed by id from the tables' diff, the omissions list, what was deliberately
not adopted, what is owed. One commit per landed
step; the note lands with the last. A revision that replaces the skill wholesale, as
revision 2 did, is built in a sibling folder and swapped in one commit when both lints have
run; a revision that changes rows is made in place.

## audit-run

Per the `agent-runner` skill, under `fanout/skill-audits/`. The prior text — the whole old
skill folder, and any page of method text the revision retires outside it — is concatenated
in a stated order into one document, split by the runner's `split` verb into units with a
manifest, and the generator writes one job per section: the section's units as its items,
every file of the new folder and of the skills it delegates to as set B, the unit ids as
the output contract. Dry run; pilot on the section the revision changed most, its output
read by Brian; batch; the tallier counts verdicts per kind and flags malformed blocks.
Reads: the prior text and the protocol. Writes: the items and their manifest, the jobs,
the ledger, the tally and `run.md`, which names the files in the document and the unit
range each occupies. `decisions.md` is never in set B: intent is applied at adjudication,
never given to the auditor. Set B holds no instrument the revision retires, since a retired
file in B lets its own text pass as preserved.

## audit-judge

Instructed by `fanout/skill-audits/protocol.md` and nothing else; Sonnet by default; tool
Write; no MCP. Given one section's units and set B, it answers the protocol's three
questions per unit and writes one block per unit under the unit's id.

## Never

- Keeps two copies of a table.
- Changes a row without its prose or prose without its row.
- Rewords text while moving it.
- Treats any row as settled.
- Swaps a rewritten skill in before both lints have run.
- Gives the auditor `decisions.md` or a retired instrument.
- Cites a decision from any other activity file.
- Presents an option before the frame above is complete, or offers two options as the
  only two when the frame has not shown the space they sit in; a question Brian asks
  inside a pair is answered by re-examining the pair, not by picking one.
- Takes a decision the unit's decision list did not carry when Brian saw it; a decision
  that arises mid-unit is added to the list and placed in it before it is taken.
- Mints a check as declared when no schema's Shape states what it holds, or as earned
  when the failure it prevents has not been observed and cited; a check across files,
  between the code and the tables, or over how a run went is earned only.
- Arms a check, which is publishing the exe with it, before the decision that names its
  id has landed, unless the closed founding record describes it in words.
- Exempts one file from its class's check by its name, path or date instead of fixing the
  file to the class or changing the class by a decision; a fix to a file that is wrong
  under a right class is always allowed.
- Writes a one-time instruction into an activity file or a schema; a triage, a migration
  or any other bootstrap is executed from the decision that names it, through the process
  the decision names, and the activity file says only what that process always does.
