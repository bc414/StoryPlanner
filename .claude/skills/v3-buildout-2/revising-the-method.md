# revising-the-method

Enables preparing-to-explore-a-corpus and preparing-to-verify-a-corpus.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| revise | hitl | DocIntegrity git | skill runner-skill map state revision-note decisions | skill runner-skill decisions revision-note map | built | Brian decides; once nothing gating is left to deliberate, the unit's decision entries are written together for him to scan, and only then does the session edit the skill's tables and prose in place under the hook; the note is written once at the end |

## Preconditions

A finding about the method itself: the validator reports a gap, a batch shows the method's
text does not hold, an activity file is found wanting in use, or Brian's judgment that the method is
wrong-shaped. A revision is never triggered by a hypothesis's content.

## revise

The procedure serves what Brian asked for: "I want rigor instead of less friction now and
more churn later."

A unit of revision opens with a queue of starting points: the decisions the session
proposes, one line each, ordered widest-reaching first; Brian reorders, removes or adds,
and none is taken until he has seen the queue. Taking a starting point opens a stack with
it at the bottom: deliberating any decision on the stack pushes the decisions it surfaces
on top; the pop is from the top, so the starting point is settled last and may name the
decisions settled above it. A stack is worked to empty before the next starting point is
taken, and the queue is then presented again, re-ordered and re-formed; it is never
presented as settled, and no remaining item is described as if it would land as written.
A unit deliberates only the starting point it takes, what is stacked on it, and the
dependencies that gate the work it serves; the rest of the queue waits for a later unit.
For each decision the session states the frame before any option:

- the finding;
- what exists under the change, in files, rows, code, tests and decisions, and per part
  when it entered, whose requirement it answered, and whether it was built for one
  instance and then generalised; a part built for one instance is presumed that
  instance's until a second consumer is shown;
- the precedent, by a grep of `decisions.md`, a reading of the closed founding record,
  and the code-sessions archive: for a word, who first used it and when, and whether
  Brian typed it, selected it or never used it; for a design, when each part entered and
  in answer to what; a recall of Brian's about the method is checked there before a
  decision rests on it, and the entry says what the archive showed;
- the consumers of anything the decision would name: who reads it, who writes it, what
  each does with it, and whether it needs a name at all; a thing no consumer reaches for
  by name is not named, and a thing whose consumers are all another's is folded into it;
- the words the finding and the options will use, checked against SKILL.md § Vocabulary
  and against their ordinary sense; a word that collides is split by its own decision
  before this one; a session's word Brian never used is presumed replaceable, his typed
  word presumed kept, until a consumer says otherwise;
- the invariants the finding touches; a proposal that carries an exception, a second
  path for one class, a placeholder, a row with a special corpus or folder, a rule with a
  named exemption, is not an option until the two invariants whose collision produced it
  are named and the cost of each yielding shown; the decision says which yields and
  why, or records the exception as the deliberate cost of keeping both.

Then the options, each with what it costs and buys, the session's recommendation marked
as its own and listed first; one decision at a time, never one bundle for sign-off.
Brian decides. A unit runs in three phases, in this order: deliberation, until nothing on the
stack remains; then every entry of the unit, written to `decisions.md` at once in the order
the decisions were settled, for him to scan, he approving the ruling and never the drafted
sentence; then, and only then, the edits. The session applies each decision as a row edit and a prose edit together,
in the activity file, a schema file or this router, never one without the other; a
ruling about how calls are run lands in the `agent-runner` skill the same way; a
schema change (a column, a closed set, a check) is a change to
`schemas/skill-schema.md` and to the tool with its fixtures, and is rare. A decision that
adds or changes a check names the check's id; before a check is added or changed, the
session greps `decisions.md` for the id, and a check that no decision names and the closed
founding record does not describe is unbacked: it gets a decision or is dropped. The first
file written under a new or changed schema, and the first run of a new or changed
procedure, is presented to Brian against its declaration before a second is made; a
discrepancy is a decision not yet made, never a fix to the instance. At that first run, where
a process moves from `specified` to `built`, a process that writes a field recording Brian's
words has it decided whether its entry is shown to him in the chat before it is written, and
its procedure carries the instruction when it is.

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
citation forms closes the step. The first real files of a class are evidence of what the
class must hold, never a template for how its files look.

Before a run of autonomous work, the session enumerates every choice the run would
otherwise make on its own, walking everything the run will write and asking of each
thing what an entry has fixed and what it has not; schemas, rows, paths, checks, verbs
and files are where such choices have hidden so far, and the walk is not confined to
them. Each choice found is decided before the run, as an entry, or the run does not
start. A run has no planned stop: it ends at the first act the method reserves to Brian,
among them an entry to approve, a check to arm without its decision, a first file to
review, a commit. A run that meets a choice it did not enumerate ends there, pushes the
choice, and takes nothing; what it has done to that point stands as far as it is
consistent with what was decided. Brian's skepticism that everything is decided is
answered by the enumeration being shown, never by a sentence that nothing remains.

The validator gates a revision, a wholesale one included: the write hook runs `check`
(`process-docs/StoryPlanner.DocIntegrity`) over the skill folder at every write and
regenerates `map.md` and `state.md` on a pass; a failure is fixed before the next edit.

When the revision is done, the session writes the write-once revision note from the
revision's section of `decisions.md`: what raised it, the decisions, the activities and
processes changed by id from the tables' diff, what was deliberately not adopted, what is
owed. One commit per landed step; the note lands with the last. A revision that replaces the
skill wholesale, as revision 2 did, is built in a sibling folder and swapped in one commit
when it passes the validator; a revision that changes rows is made in place.

## Never

- Keeps two copies of a table.
- Changes a row without its prose or prose without its row.
- Rewords text while moving it.
- Treats any row as settled.
- Cites a decision from any other activity file.
- Presents an option before the frame above is complete, or offers two options as the
  only two when the frame has not shown the space they sit in; a question Brian asks
  inside a pair is answered by re-examining the pair, not by picking one.
- Takes a decision that was not on the stack when Brian saw it; a decision that arises
  mid-unit is pushed, and taken only when it is popped.
- Edits a file to apply a ruling before every entry of its unit is written.
- Presents the remaining queue as settled, or plans a stop inside an autonomous run.
- Names a concept, or keeps a session's word, before its consumers are listed.
- Offers a special case as an option before the invariants that produced it are named.
- Mints a check as declared when no schema's Shape states what it holds, or as earned
  when the failure it prevents has not been observed and cited; a check across files,
  between the code and the tables, or over how a batch went is earned only.
- Arms a check, which is publishing the exe with it, before the decision that names its
  id has landed, unless the closed founding record describes it in words.
- Exempts one file from its class's check by its name, path or date instead of fixing the
  file to the class or changing the class by a decision; a fix to a file that is wrong
  under a right class is allowed within rule 9.
- Writes a one-time instruction into an activity file or a schema; a triage, a migration
  or any other bootstrap is executed from the decision that names it, through the process
  the decision names, and the activity file says only what that process always does.
