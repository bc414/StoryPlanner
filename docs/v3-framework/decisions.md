# Decisions

The method's decisions from 2026-09-07 on: one titled entry per decision about how the
buildout is run, every entry Brian's, appended after his approval and never edited,
sections by revision in order, in the shape `schemas/decisions-schema.md` defines. What the
method currently says is the skill's text, never this file. The founding record,
`decisions-founding.md`, holds the decisions of 2026-09-04 to 2026-09-07 as prose and is
closed; what stands of it is the skill's text, and an entry here that changes one of its
rules names the old id in prose.

## Revision 2

### decisions.md starts again under a schema; the founding record is closed

- id: d-2026-09-07-1
- date: 2026-09-07
- prompted by: Brian, after a schema entry superseding the founding bundle ran to sixty
  lines: "Since the schema and enforcement is brand new, should it start with these
  entries, not bother with supersession, and we only move forward from here? The old
  decisions.md is left as historical record, and whatever is in the skill file right now
  is the current working base." And: "How about if supersession only applies to new
  entries too, not crossing over into old unshaped prose?"
- decision: The file founded 2026-09-06 is closed as `decisions-founding.md`, never
  written again, its ids as they are, one line in its head pointing here. This file
  begins with the schema's own entries and conforms from its first line. What stands of
  the closed record is the skill's text, which its entries were applied to; an entry
  here that changes one of those rules names the old id in prose, and a `supersedes`
  line names ids of this file only. The revision-2 note is written from the closed
  record's section and this file's together, once. This does not decide how a check
  that every enforced rule has an entry would treat rules whose entries are in the
  closed record.
- not taken: migrating the founding record into the schema, splitting its bundles and
  re-deriving its ids, since a shape imposed on entries that never recorded a prompt or
  a rejected option fills the fields with "not recorded", and the split titles would be
  a session's authorship over Brian's record a second time; a date rule inside one file,
  which leaves the checker a file that conforms only from a line it must be told; a
  dated name or `decisions-1.md` for the closed file, since what a reader needs to know
  is that it is the founding transcription.

### An entry is a one-line ruling over keyed fields, as schemas/decisions-schema.md defines

- id: d-2026-09-07-2
- date: 2026-09-07
- prompted by: Brian, reading the entry that moved the Artifacts table: "why is the entry
  into decisions.md so dense? Does it need a better schema?", then "It should be prompted
  by, followed by the decision, then not taken? Is anything else needed?", "One form of
  supersession is good. New entries absorbe the surviving clauses.", "We cannot have
  bundles going forward", and on the draft: "What is the best schema and format from
  first principles but given the data that will populate them, not tunnel visionned on
  what was put in naively before there was ever a schema?"
- decision: The schema is `schemas/decisions-schema.md`, and the tool holds it from this
  entry on. The founding record's entries were a title, a date, an optional supersedes
  line and prose; from here an entry is a title stating the ruling, then keyed fields,
  one decision each, with the two machine-read lines exact and the rest free. The
  founding record's entry on its own shape, d-2026-09-06-12 in the closed file, is
  history, and this supersedes nothing.
- not taken: a `brian` field, since his words are quoted where they bear and the
  code-sessions archive holds the rest; an approval-mode field with values typed,
  selected, delegated, since authorship is a property of a phrase, not of an entry, and
  the quotation rule carries it at that grain; `frame` and `under it` fields,
  decision-time values that the not-taken list and the commit carry; an `owed` field,
  since a claim on future work goes false when the work is done and a file never edited
  cannot close it, the revision note being the change record that holds what is owed;
  a `stands` field with `(in part)` supersession, which made a reader reconstruct
  standing from three entries; several decisions under one title, since the founding
  bundles are what made supersession unable to be whole; bold labels, a heading per
  field, or bare `key:` lines as the field grammar, each losing one of grep, the
  checker or the preview; a count of fields in the title; an optional field written
  with an empty value; six entries recording the schema one aspect each, since the
  definition lives in the schema file and an entry restating it read as the definition;
  the real first entry as the fixture, twenty-five lines exercising no supersedes line,
  where a template does the writer's job and the tests' both.

### A schema file is Shape, Example, Queries, Checks, each section named by its consumer

- id: d-2026-09-07-3
- date: 2026-09-07
- prompted by: Brian, on the rewrite of `schemas/decisions-schema.md`: "Don't match the
  exsting format sturcture since those have no shape and have to be reviewed. You have to
  think about how this will be consumed", and "Should this be the format of schema files
  going forward?"
- decision: A schema file is its title, one sentence saying what follows, then four
  sections in this order. Shape: the grammar a writer follows and the hook holds, saying
  which lines are machine-read and exact and which are free, or that the class is prose
  with no machine-read line. Example: a conforming file with placeholders that a writer
  fills in, and the file's first fenced block, which the checker's tests read as their
  fixture. Queries: one grep per question a reader asks of the class. Checks: the rule
  ids the hook reports and when each fails; a prose class has none. Each of the other
  thirteen schema files is rewritten to this shape when it is reviewed, one at a time,
  leads first; the validator's `schema.shape` rule grows to hold the four sections when
  the last is converted, and until then the shape is held at each rewrite's review.
  This does not decide what a Shape must declare beyond machine-read or prose.
- not taken: matching the existing schema files, a preamble restating the Artifacts row
  and prose per field, since they were written without a schema and are to be reviewed;
  arming the extended `schema.shape` check now, which fails every write in the folder
  until all thirteen are converted; converting all thirteen in one mechanical pass,
  which would put a session's shape over unreviewed content; naming the second section
  Fixture, the tests' word, where the writer is the one who copies it.

### The schema cell is a markdown link, [name-schema](schemas/name-schema.md)

- id: d-2026-09-07-4
- date: 2026-09-07
- prompted by: Anthropic's skill guidance, "keep references one level deep from SKILL.md;
  all reference files should link directly from SKILL.md", against a bare slug that names
  the file without linking it. Brian: "Go with The cell is a link since that is the rigor
  that satisfies the anthropic doc recommendations."
- decision: The `schema` cell of the Artifacts table is
  `[<name>-schema](schemas/<name>-schema.md)`: the text is the schema id, the target is
  the file the text names, the file exists, and its title is the id; the validator holds
  all four, and the reader keeps the id as the row's schema. The table is then the one
  place a schema is named, so the three prose clauses "in the format `formats/<x>.md`"
  in preparing-to-explore-a-corpus, preparing-to-verify-a-corpus and revising-the-method
  leave; the agent-runner skill's pointer to `schemas/run-page-schema.md` stays, a
  cross-reference from another skill. The founding record's d-2026-09-07-1 ruled a bare
  slug for this cell and is history.
- not taken: the bare slug, which names the file without the link the guidance asks for;
  a link whose text may differ from the id, which lets a cell read one name and point at
  another; the prose restatements kept beside the table, a second copy of the column.

### An entry's id is written as its first field

- id: d-2026-09-07-5
- date: 2026-09-07
- prompted by: Brian, reading the new file: "Seems like decisions.md entries are missing
  ids?", "Who decided that ids are not written? That seems like room for error or
  misattribution.", and "Should there be any concept of derived ids anymore? Is there any
  downside to writting an id when writing the decision and it stays?" The derived-only id
  was a session's design in the founding bundle, approved under "Proceed" with no word of
  Brian's on it; it left an id that no grep can find at its entry and a supersedes line
  that a writer fills by counting.
- decision: The first keyed line of an entry is `- id: d-<date>-<n>`, exact, written with
  the entry and never changed. The checker holds it: the date is the entry's `date`, and
  `n` is 1 for the first entry of that date and one more than the previous entry's for
  each after, so ids are unique and in order by construction. Nothing derives an id and
  the tool never writes one; a written id can only disagree with the rule by failing,
  and the failure names the expected id. The closed founding record has no id lines and
  keeps being cited by the same count.
- not taken: ids derived by the tool and never written, the founding session's design,
  unreadable in the file and ungreppable to their entries; a written id held only to
  uniqueness, which leaves `n` an arbitrary label that still reads as an order; a plain
  running number, which drops the date a reader takes from the id; the id in the title,
  an exact token inside free text; the tool stamping the id on a passing write, which
  would make a hook edit the governed file it had just checked, where the tool writes
  only generated files.

### The word is schema: the column, the folder schemas/, and the file <name>-schema.md

- id: d-2026-09-07-6
- date: 2026-09-07
- prompted by: Brian, on the file named after its class: "What can be done about the file
  name colisions?", "are there any issues with changing all the files under format/ to
  have a -schema.md at the end?", "How about we change the folder from format/ to
  schemas/ as well?", and, on the suffix beside the folder: "The redundancy in the folder
  is to disambiguate from any singleton files outside that folder which carry just the
  file name/class name, like decisions.md which has the actual instances." Ten of the
  fourteen format ids were their class's own word, given when a section of artifacts.md
  served one class, and two of the files collided by name with the governed file.
- decision: One word, schema, wherever a class's shape file is named: the Artifacts
  column is `schema`, the folder is `schemas/`, the file is `<name>-schema.md` with that
  name as its title, and the cell is `[<name>-schema](schemas/<name>-schema.md)`. The
  suffix keeps a schema's basename apart from a singleton class's file, which carries
  the bare class name, as decisions.md and studies.md do; the folder groups the files
  and makes the path. A schema id is never a class id, and the validator holds the
  suffix. § Schema in SKILL.md is the same concept at the skill folder's own level and
  keeps its name, retitled to say whose schema it is; moving it into a schema file of
  the `skill` row is not decided here. The rule ids and the tool's names follow the
  word, as the vocabulary rename did. The entries above and the head of this file that
  said formats/decisions.md are amended to the new name in the same write, since none
  is committed.
- not taken: shape names per format, `decision-entry` after `question-entry`, which
  needs a judgment per file and leaves the check unarmed until the last review;
  `-format` as the suffix, which matches the old column and reads "the decisions-format
  format"; the folder renamed without the suffix, which returns the basename collision;
  disambiguating by path alone, which leaves two files called decisions.md in a tab, a
  grep and a sentence.
