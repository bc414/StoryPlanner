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
- raised by: Brian, after a schema entry superseding the founding bundle ran to sixty
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
- raised by: Brian, reading the entry that moved the Artifacts table: "why is the entry
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
- raised by: Brian, on the rewrite of `schemas/decisions-schema.md`: "Don't match the
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
- raised by: Anthropic's skill guidance, "keep references one level deep from SKILL.md;
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
- raised by: Brian, reading the new file: "Seems like decisions.md entries are missing
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
- raised by: Brian, on the file named after its class: "What can be done about the file
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

### A schema file no row links to is a failure

- id: d-2026-09-07-7
- date: 2026-09-07
- raised by: the review of the validator rules written on 2026-09-07 without an entry:
  `file.orphan-schema` was the analogue of the activity-file orphan rule and no decision
  backed it. Brian: "What do I need to see regarding the validator rules?"
- decision: A file under `schemas/` that no Artifacts row's `schema` cell links to fails
  `file.orphan-schema`. The table is the one place a schema is named, and the guidance the
  link ruling was made for, every reference file linked directly from SKILL.md, holds for
  the folder; the rule is the file-side mirror of `ref.schema`, so a rename or a
  retirement is reported from both ends. A schema file is written with its row, never
  before it. This is structure stated once, following from the link ruling, not a check
  earned by a failure: the rule has fired only in transient states and never on a real
  orphan.
- not taken: dropping the rule, which leaves the file side of the guidance unheld and an
  unreachable schema for a human to notice; reporting an orphan as information, which
  leaves a folder green while it breaks the guidance.

### The skill folder's schema is schemas/skill-schema.md; SKILL.md keeps what a running session reads

- id: d-2026-09-07-8
- date: 2026-09-07
- raised by: Brian, on the special case entry 6 left open: "So the purpose of this
  decision is to move details about how to change the skill file into a different file,
  since only sessions who need to change it need to see the schema, not all of them? Or
  should standard operating procedure sessions still need to see the schema so that they
  can follow it? Like query patterns or how to find the files, meaning it's better
  inline?" Then: "Yes, make the changes to skill.md and create the skill-schema.md."
- decision: § Schema is split by consumer. What a session needs to act on a row it reads
  stays in SKILL.md, under "Reading the tables": what the columns mean when the session is
  the one running the process, where the generated files are, and what the hook does at a
  write. What only a session changing the folder needs becomes `schemas/skill-schema.md`
  in the four-section shape, linked from the `skill` row: the tables' grammar and closed
  sets, the activity file's shape, what is derived and never authored, an Example that is
  a conforming folder, a writer's Queries, and Checks listing the validator's rule ids,
  which no file in the skill listed before. The query patterns a running session needs
  are per class, in that class's schema, reached from its row, as before. The validator's
  tests keep building their fixture in code; whether they read the Example instead is not
  decided here.
- not taken: moving § Schema whole, which takes the operating instructions away from
  every session that loads SKILL.md; keeping it whole, which keeps the one class whose
  schema is inline and leaves the rule ids unlisted; deciding now and executing as unit
  4, since a decision is applied as it lands.

### Rule is a constitutional rule; check is what the tool holds

- id: d-2026-09-07-9
- date: 2026-09-07
- raised by: Brian, before the decision on backing the tool's checks: "Is 'rule' clearly
  defined anywhere in the skill folder?" It was not: the folder used the word for the nine
  constitutional rules, for what the tool holds, for a codebook's boundary statements and
  for his rulings. Brian: "Rule stays for constitutional rule, and 'check' for the tool."
- decision: A rule is one of the nine constitutional rules in SKILL.md, cited by number. A
  check is one thing the tool holds, named by its id in a schema's Checks section; the
  verb `check` runs every check that applies to a path. Both enter § Vocabulary. Brian's
  rulings keep their word, and the codebook's sense is decided in the next entry. The
  Checks tables' first column, every sentence that said rule id or validator rule, and
  the tool's own names follow the word, as the schema rename did; decisions already
  landed keep their words.
- not taken: defining rule as the tool's sense and qualifying the constitutional rules by
  name, which leaves the plain word overloaded in prose; leaving the word undefined while
  the next decision uses it.

### A codebook's boundary statement is a criterion

- id: d-2026-09-07-10
- date: 2026-09-07
- raised by: Brian, once rule and check were split: "What should be the term for the
  third distinct thing related to codebooks? Need to sort this out now." The codebook had
  called it a decision rule, two words the vocabulary now reserves.
- decision: A criterion is a codebook's statement that admits an item to a class or keeps
  it out: what a classifier applies, what an anchor sits under, what a calibration ruling
  changes. The codebook schema's section is `## Criteria`, the sentences in
  preparing-to-verify-a-corpus and conducting-a-verification-round say criterion, and
  § Vocabulary carries it. A question's predicate stays the test a codebook freezes for
  one question; a criterion is one class boundary within it.
- not taken: predicate, already the question-level test, which would muddle the levels;
  boundary, which says where a criterion applies and not what it is; keeping decision
  rule, two reserved words.

### Every check the tool holds is backed by a decision that names its id

- id: d-2026-09-07-11
- date: 2026-09-07
- raised by: Brian, reviewing the validator's checks: "What do I need to see regarding
  the validator rules?", which found `file.orphan-schema` enforced with no decision behind
  it; then, on the machinery proposed to hold this, "So now there's even more machinery I
  wasn't aware of? What does it buy us and what failure modes does it prevent?" and "A
  standard operating procedure session which writes to a governed file and gets an error
  code shouldn't need to look at the rationale for revising the method."
- decision: A decision that adds or changes a check names the check's id, so the question
  of what backs a check is a grep of decisions.md by id, asked in revising-the-method and
  nowhere else. Before a check is added or changed, the session greps for its id; a check
  that no decision names and the closed founding record does not describe is unbacked,
  and gets a decision or is dropped. Telling a founding check from an unbacked one is a
  reading of the closed record, done at each schema's review. A session that fails a
  check reads its meaning in the schema's Checks section and never its rationale.
- not taken: an `entry` column in the Checks tables naming each check's decision, which
  puts decision ids in files every session reads and fails the folder under
  `decision.id-outside-revising`; a check that every check id in the code appears in a
  Checks table, unearned until a check is found missing from one; a grep of decisions.md
  held by the tool, which cannot tell a founding check from an unbacked one.

### A decision is framed before its options are presented

- id: d-2026-09-07-12
- date: 2026-09-07
- raised by: Brian, when a session put several decisions in front of him at once:
  "There's too many different things here, and not enough detail to make a judgement. Go
  one at a time, in order of most wide reaching to least, with full context of what the
  judgement is about." The observed failure on 2026-09-07 was an option, an `entry` column
  in the Checks tables, presented without a precedent check when an enforced check already
  forbade it.
- decision: Before any option, the session states the frame: the finding; what exists
  under the change, in files, rows, code, tests and decisions; the precedent, by a grep of
  decisions.md and a reading of the closed founding record and § Vocabulary; and the
  audit units touched, if the revision has a tally. Then the options, each with what it
  costs and buys, the session's recommendation marked as its own and listed first. The
  frame is prose in revising-the-method § revise; nothing in the tool holds it.
- not taken: "with full context" added to § revise, which names no parts, so an omission
  does not show; a proposal artifact with a schema and a checker, machinery ahead of any
  failure that needs it.

### A unit of revision opens with its decision list, ordered widest-first by the session

- id: d-2026-09-07-13
- date: 2026-09-07
- raised by: Brian, after a schema decision was taken ahead of the fresh-start decision
  that dissolved it: "Go one at a time, in order of most wide reaching to least, with full
  context of what the judgement is about"; and on the list for this unit: "The list should
  appear, but already ordered by widest-first/most impactful."
- decision: At the start of a unit of revision the session lists the decisions it
  proposes, one line each, already ordered by the session with the widest-reaching or
  most impactful first and each dependency on an earlier one named; Brian reorders,
  removes or adds, and the first decision is taken only after he has seen the list. The
  list is a message, not an artifact.
- not taken: taking decisions as they arise, widest-first by the session's judgment
  alone, which is how a schema entry came to supersede a bundle the next decision
  dissolved; the list as a governed artifact with a schema, machinery ahead of any
  failure that needs it.

### A check is minted in one of two ways, by declaration or by an observed failure

- id: d-2026-09-07-14
- date: 2026-09-07
- raised by: the same evening arguing one proposed check as structure and withdrawing it
  as unearned, with entry 7 the first to say which side a check is on. Brian, on the
  wording: "So in order to state it more clearly, the prose should say there are two
  ways a check is minted: declaring the structure, and then from observed failures?"
- decision: A check comes to exist in one of two ways. Declared, it holds what a schema's
  Shape says about one class's files and is held from the schema's first write. Earned,
  it prevents a failure that has been observed, which is the only way a check that spans
  files, or the code and the tables, or a run comes to exist. The decision that mints a
  check says which way. The sentence lives in building-a-tool § Preconditions beside the
  one it refines.
- not taken: earned by failure only, which leaves a declared shape unheld until someone
  violates it; declared only, which builds every check a session can state, ahead of any
  failure.

### The frame checks the words a decision will use against § Vocabulary and their ordinary sense

- id: d-2026-09-07-15
- date: 2026-09-07
- supersedes: d-2026-09-07-12
- raised by: Brian, on a vocabulary check proposed as a step after the frame: "Isn't it
  earlier than a decision? The frame of the decision already has to do a vocab check for
  non collisions first." Four collisions had been caught the same evening after use:
  "map" for the decision list, "rule" in four senses, "decision rule" of two reserved
  words, "entry" where "decision" was the word.
- decision: Before any option, the session states the frame: the finding; what exists
  under the change, in files, rows, code, tests and decisions; the precedent, by a grep
  of decisions.md and a reading of the closed founding record; the words the finding and
  the options will use, checked against § Vocabulary and against their ordinary sense,
  with a word that collides split by its own decision before this one; and the audit
  units touched, if the revision has a tally. Then the options, each with what it costs
  and buys, the session's recommendation marked as its own and listed first. The frame
  is prose in revising-the-method § revise, as a list; nothing in the tool holds it.
- not taken: the vocabulary check as a step after the frame, too late by the evidence of
  the evening; a tool check over § Vocabulary, since a sense is not something a tool can
  read; "with full context" added to § revise and a proposal artifact with a schema, both
  declined in the entry this supersedes.

### The first instance under a new or changed declaration is a review

- id: d-2026-09-07-16
- date: 2026-09-07
- raised by: the leads schema, declared with headings only and used for three
  artifacts before its first was held against it, which is what opened this revision when
  Brian saw real data; the founding decisions file, eighty-two entries transcribed with no
  first entry reviewed, which is where the bundles came from. Brian, on the option: "The
  first option makes sense."
- decision: The first file written under a new or changed schema, and the first run of a
  new or changed procedure, is presented to Brian against its declaration before a second
  is made. A discrepancy is a decision not yet made, never a fix to the instance. This
  generalises the predicted first run of a checker, which stays in building-a-tool; the
  sentence lives in revising-the-method § revise.
- not taken: the checker's first-run rule alone, which reviews the checker while the
  schema it follows goes unreviewed; a first-instance sentence in each schema file,
  thirteen copies of a procedure in files that hold shapes.

### revising-the-method's Never lines carry this unit's decisions, and § revise says what the procedure serves

- id: d-2026-09-07-17
- date: 2026-09-07
- raised by: Brian, on eight candidate lines: "How many generalize versus are overfit and
  could overconstrain?", then "All of these need to be made more specific with their
  nuances." Three of the eight contradicted a standing decision or forbade a legitimate
  act as first written.
- decision: § Never gains five lines, each an act with its nuance: presenting an option
  before the frame is complete, or two options as the only two when the frame has not
  shown the space they sit in; taking a decision the unit's decision list did not carry
  when Brian saw it, an addition being placed in the list before it is taken; minting a
  check as declared when no schema's Shape states what it holds, or as earned when the
  failure it prevents has not been observed and cited; arming a check, which is publishing
  the exe with it, before the decision that names its id has landed, unless the closed
  founding record describes it; exempting one file from its class's check by name, path
  or date instead of fixing the file or changing the class by a decision, a fix to a file
  that is wrong under a right class being always allowed. § Never becomes a list, its
  seven earlier lines kept as written. § revise opens with what the procedure serves, in
  Brian's words: "I want rigor instead of less friction now and more churn later."
- not taken: the eight lines as first drafted, three of them overfit: a ban on minting any
  check without an observed failure, which contradicts the declared way; a ban on
  enforcing a check no decision names by id, which would prohibit the forty founding
  checks; a ban on fixing an instance without its class, which forbids correcting a wrong
  file; the rigor line as a Never, a value rather than an act; three frame lines where
  one covers them.

### A class's files are documents: fields and entries under one grammar, with references the checker resolves

- id: d-2026-09-07-18
- date: 2026-09-07
- raised by: Brian, the afternoon the leads schema was found to hold only headings: "Is
  the document model is what I needed all along, implemented in markdown with a check verb
  program instead of json and whatever json validation MongoDB uses?" The model had driven
  every schema since and was stated nowhere in the skill.
- decision: An artifact class is a collection and each of its files a document: fixed
  fields and arrays of entries under one line grammar, with references the checker
  resolves. The class's schema is the document's shape, its mutation the collection's
  write discipline, a grep on the line grammar a query, and the generated files its views.
  Stated once for the reader, in SKILL.md § Artifacts beside the sentence that a class is
  its files; the writer meets it as the list of what a Shape declares, decided next, in
  skill-schema; the checkers embody it in code.
- not taken: a second prose copy as the lead of skill-schema's Shape, the stale-mirror
  shape; the entry alone with no skill prose, leaving the pattern to be inferred from
  fourteen schemas.

### A Shape declares five things, and a schema review runs as one procedure from revise

- id: d-2026-09-07-19
- date: 2026-09-07
- raised by: entry 3's open clause, "what a Shape must declare beyond machine-read or
  prose", with the leads schema about to be the first written under the answer. Brian:
  "Who are the consumers? Where does this go?", "What about the revising the method
  file?", "Seems like revising-the-method should be the primary entry point, so it should
  be firmer or more elaborate or not?"
- decision: A Shape declares the sections that partition the class's file, in order; the
  fixed fields, their order, and for each machine-read one its exact grammar; each entry
  array's line form and its continuation; each reference a line may carry, its form, what
  it resolves to and which checker resolves it; or, for a class with no machine-read
  line, that the class is prose. It declares and never restates its row's path or
  mutation. The rule lives in skill-schema's Shape as the writer's schema of schema files;
  SKILL.md § Artifacts keeps its one reader's sentence. revising-the-method § revise
  carries the schema review as one procedure of pointers: a unit of revision with its
  decision list and frames; the schema written to skill-schema's shape; the checker built
  under building-a-tool with its first run predicted; the Checks section listing what
  the checker holds and the review's decisions naming the ids; the class's files on disk
  brought to the schema or the class started again, decided in the review; the first
  file under the schema reviewed before a second.
- not taken: the five things in SKILL.md § Artifacts, a writer's rule in front of every
  session; the current sentence alone, from which the two Shapes written so far showed no
  checker can be written; a formal Shape grammar the tool parses, a language and a
  generator ahead of any failure; one pointer sentence in § revise, too thin for the file
  a schema review opens.

### The field for what raised an entry is raised by, in decisions and in questions alike

- id: d-2026-09-07-20
- date: 2026-09-07
- raised by: the question-entry review, where the same field was being named. Brian:
  "Prompted is ambiguous with a 'prompt' as in the human turns of a claude code session."
  Then: "I wanted to change prompted by back then in the decisions schema but too much was
  happening. It should be done now."
- decision: The field that holds what raised an entry, and in whose words, is `raised by`,
  in decisions-schema and in question-entry-schema alike: the method's own verb, "a
  question is what a lead raised". The nineteen decisions already landed have their key
  line renamed in the same write, a rename of a key and not of a word of content; the
  checker's field set, the schema's Shape, Example and Queries, and the activity files'
  "what prompted it" follow. Brian's quoted words keep the old word where he used it.
- not taken: two words for one concept, `prompted by` kept in decisions for the cost of
  the rename; `origin`, which names where and not what; `provenance`, the word for the
  decisions file itself; `occasion`, which names the session and not the lead.

### The spec pools close as the founding record of questions; the lists are filled by triage

- id: d-2026-09-07-21
- date: 2026-09-07
- raised by: Brian: "The content of spec-pools right now are not all leads. There are some
  open questions that haven't been investigated yet", then "A triage will be needed of
  the deprecated spec pools to backfill the new schema." The closed record's rulings that
  a question is Brian's and that question lists replace spec pools; the pools hold three
  kinds of entry, re-housed pre-revision entries, plan-1 specs, and open questions never
  investigated, only the first of which is the leads material.
- decision: `docs/v3-framework-historical/spec-pools/` is closed as the founding record of
  questions, never written again, one pointer line in its README. `questions/<corpus>.md`
  starts empty and is filled by a triage per corpus in `ask`: Brian goes through the
  pool's entries, keeping those that are his questions, each written under the schema in
  his words with `raised by` saying it was carried from the founding pool; the rest stay
  in the closed pool as history. The triage is the schema's first-instance review.
- not taken: migrating all ninety entries mechanically, which writes questions Brian did
  not ask into lists whose first rule is that a question is his; closing the pools with
  no triage, which drops the open questions he did ask; a triage before the schema, which
  has nothing to write into.

### A question's id is its slug, cited as <corpus>/<slug>

- id: d-2026-09-07-22
- date: 2026-09-07
- raised by: Brian: "Should the id just include the corpus in it? Or we should just go
  with a slug so that it's self descriptive instead of needing an id lookup?", "Should it
  be a single slug like own-fiction-heavy-dt-two-classes?", "The slash works." The title
  had been the key, cited verbatim by four schemas and matched by state.md.
- decision: An entry's heading is its slug, lowercase `[a-z0-9-]+`, unique within its
  list, authored once and never changed; a reworded question is a new entry and the old
  one withdrawn. It is cited everywhere as `<corpus>/<slug>`, one token that splits on
  its one slash, since every corpus id contains hyphens. No separate title, no number;
  order is the file's. No length cap: the charset rule, the readable-ids ruling and the
  hitl write bound it, and a cap is earned by a failure. The checker holds the slug as
  `question.slug`. The schemas that cited titles cite the id at their reviews; the
  coverage derivation matches ids.
- not taken: the title as the key held exact, which makes a title unrewordable and a
  citation a full line; `Q-NNN` with the corpus prefixed, which needs a lookup; a
  hyphen-joined `<corpus>-<slug>`, unsplittable without the corpus set; a two-token
  `<corpus> <slug>`, which breaks space-separated lists; a length cap with no failure.

### A question entry is date, hypotheses, raised by, question, suggested test

- id: d-2026-09-07-23
- date: 2026-09-07
- raised by: Brian, on the pool's fields: "asked-by doesn't seem to make sense? I'm the
  only one asking", then "I'm not liking how we have many polymorphic fields. Do we need
  more fields with less values? Or do we not need all of this? What does the consumer
  need? Who are the consumers?" Listed, the consumers showed that only the reader tracing
  provenance wanted the occasion and the citation, and nothing derives from them.
- decision: The file is `questions/<corpus>.md`, titled `# <corpus> — questions`,
  entries only, no head prose. An entry is `### <slug>`, then `date`, exact and never
  earlier than the entry before; `hypotheses`, exact ids the checker resolves against
  the hypothesis files, present only when there are any; `raised by`, free, the occasion
  and what raised it with the citation as a token; `question`, free, Brian's words;
  `suggested test`, free, present only when one suggests itself. Two exact lines, and no
  field with a closed set of forms. The checker holds `question.title`,
  `question.entry.fields`, `question.entry.date` and `question.hypotheses`.
- not taken: an exact `asked-by` with seven origin forms and an exact `from` with four
  kinds, polymorphic fields serving no machine; the pool's shape, the date inside the
  provenance sentence; the question first and the provenance after.

### A question list is append, and a withdrawal is an appended line

- id: d-2026-09-07-24
- date: 2026-09-07
- raised by: Brian: "Should the whole thing be in-place or append?" The schema had let a
  status line change to withdrawn, an edit inside an append row.
- decision: The class is `append` for every line. A withdrawal is `- withdrawn: <date>
  <reason>` appended beneath the entry by the hitl process that withdraws it, at most
  once; open is an entry with no such line, derived; frozen and answered are derived from
  a codebook's and a round's citations of the id. There is no status line. The precedent
  is the candidate, never edited, its outcome appended beneath. The checker holds the
  line as `question.withdrawn`.
- not taken: `in-place` for one field, which needs a second row for the class or gives up
  never-edited for the file; a status line written by hand.

### A predicate is the test a codebook freezes; a question carries only a suggested test

- id: d-2026-09-07-25
- date: 2026-09-07
- raised by: Brian: "Where did the predicate field come from?", then "This is free text
  and a suggestion about predicates, so the field name shouldn't read as 'predicate' as
  if authoritative and structured, right?", and "Suggested test is fine, along with
  instructions for preparing to verify being firm that these are naive suggestions as a
  starting point." The field came from revision 1's spec pools as `candidate-predicate`,
  a session's guess at the test written when the question was raised.
- decision: `predicate` enters § Vocabulary as the test a codebook freezes for one
  question and a classifier applies to every item; it is never written in a question
  entry. The entry's optional line is `suggested test`, free and unquoted, a naive note on
  how the question might be tested. preparing-to-verify-a-corpus says, in author-codebook,
  that a suggested test is a starting point and never a criterion: the criteria are
  authored against the real items with Brian, and a suggested test is never carried into
  a codebook unexamined.
- not taken: a field named `predicate`, which reads as the frozen test before any codebook
  exists; dropping the line, since the codebook author can use a starting point.

### A schema review brings every reader's and writer's instructions current

- id: d-2026-09-07-26
- date: 2026-09-07
- raised by: Brian, after the question-entry review had left three schemas citing a
  question by title and three activity files naming a retired field: "do the other
  consumers and writers have updated instructions? Do they need anything?" Then: "Put
  this instruction to make reader/writer instructions clear in the relevant part of
  revising-the-method."
- decision: A schema review ends by walking the class's consumers as map.md lists them,
  every process that reads or writes the class and every schema that cites its entries,
  and bringing each one's instructions current with the schema in the same write: an
  activity file says what an entry carries in words and names no field, and a schema that
  cites the class cites it in the form the reviewed schema fixes, ahead of its own
  review. A grep of the folder for the old names and citation forms closes the step. It
  is the last step of the schema-review procedure in revising-the-method § revise.
- not taken: leaving each consumer to its own review, which leaves the tool's derivation
  and a schema disagreeing in the meantime, as they did for an evening; a check that a
  citation form is used consistently across schemas, unearned until a mismatch is found by
  something other than a grep.

### A one-time instruction never enters an activity file or a schema

- id: d-2026-09-07-27
- date: 2026-09-07
- raised by: a session writing "the triage of a closed founding pool runs here" into
  reviewing-leads § ask, a bootstrap instruction in a standard-operating file. Brian:
  "Isn't the triage a one time activity, not SOP?" Then: "Also do that decision about one
  time instructions not going into activity files." The closed record already holds the
  ground: bootstrap is a fact about the work, never about activities, and the skill
  carries no one-time instruction by design.
- decision: A triage, a migration, a re-founding or any other bootstrap is executed from
  the decision that names it, through the process the decision names, and the activity
  file says only what that process always does. Nothing one-time enters an activity file
  or a schema. revising-the-method § Never carries the line.
- not taken: a one-time sentence in the activity file with a date, which is the special
  case by date the method refuses elsewhere; a bootstrap activity of its own, which the
  closed record retired with the roots table.

### The question lists start empty; the founding pools are not triaged

- id: d-2026-09-07-28
- date: 2026-09-07
- supersedes: d-2026-09-07-21
- raised by: Brian, on the first pool entry of the triage, traced to a session's provenance
  narrative that a later session had turned into a point-check: "Is the data too
  corrupted? Once I set up the schema, should I just start over with a proper
  exploration-of-fimfiction-stories using the new methodology, and not bother with any
  triage?" The pools are session-authored specs from plans that mostly never ran; nothing
  in them is on record as a question he asked.
- decision: `docs/v3-framework-historical/spec-pools/` stays closed as the founding record, never
  written again. `questions/<corpus>.md` starts empty and is filled only by the standing
  processes that write it. A pool question enters a list when Brian asks it, through
  `ask`, with `carried from the founding pool` as its provenance. The first list a
  process writes is the schema's first-instance review. The README's closing paragraph
  says the lists are filled by the standing processes.
- not taken: a triage of the recall entries only, which keeps the archaeology for the
  entries the pool marks least reliably; keeping the triage as a deferred instruction,
  which ages while the method moves.

### A verification study is a verification, never a round; a run is one batch

- id: d-2026-09-07-29
- date: 2026-09-07
- raised by: Brian, on the candidate schema's decision list: "Round is really ambiguous.
  What is this referring to?", "So the equivalent is exploration-of?", "It should be
  verification. This needs to be fixed first, and then come back to the candidates
  question with the vocab sorted out so it doesn't leak any further." Round named the
  study while run named one batch under the runner, and the process id `round-run` put
  the two side by side; the exploratory chain has no such pair.
- decision: The word round leaves the method. A study of the verification type is a
  verification, id `verification-of-<corpus>-<n>`, the mirror of
  `exploration-of-<corpus>`; its activity is `verifying-a-corpus`, the mirror of
  exploring-a-corpus, with processes `verification-run`, `verification-judge` and
  `verification-write`; its artifact is `docs/v3-framework/<study>/verification.md`, the
  mirror of `leads.md`. A run is one batch under the runner, `fanout/<study>/<run>/`, on
  either chain, and a study may have several. § Vocabulary defines verification and run
  and no longer defines round. The registry checker's `registry.id` holds the new form.
  The frozen record keeps the old word.
- not taken: renaming only the study id and the file, which leaves `round-run`; renaming
  without defining run, which leaves the second word of the collision where it was.

### Leads, proposals and candidates are identified by slugs, on a first pass

- id: d-2026-09-07-30
- date: 2026-09-07
- raised by: Brian, on the candidate schema's first decision: "I'm not really liking
  C-NNN, L-NNN, etc. But I haven't seen real data yet. What are the pros and cons of
  using slugs?", then "Have the first pass use slugs". Questions already carry slugs by
  d-2026-09-07-22; the retroactive leads artifacts open every lead with a bold title; no
  candidate exists.
- decision: An entry of a leads artifact, a lead or a proposal, and an entry of a
  candidates or iteration-candidates file is headed by a slug, lowercase `[a-z0-9-]+`,
  unique within its file across both entry kinds, authored with the entry and never
  changed. The slug names what was observed, never what it means for a hypothesis; a
  finding that is per item carries the item in its slug. It is cited joined to the
  file's own id, `<study>/<slug>`. The first leads file and the first candidates file
  written under their schemas are the reviews at which the form is reconsidered against
  real slugs.
- not taken: numbered ids `L-NNN`, `P-NNN`, `C-NNN`, opaque everywhere they are cited
  and needing a sequence check; a number with a slug appended, two ids for one entry.

### An entry with a local id is headed by its citation token

- id: d-2026-09-07-31
- date: 2026-09-07
- supersedes: d-2026-09-07-22
- raised by: Brian: "Should citation tokens differ from what is within the file?" and
  "Which rule serves the consumers better, once documented in queries?" A question was
  headed `### <slug>` and cited `<corpus>/<slug>`, so one grep could not find the
  definition and the uses together, and a copy of an entry carried its token only if the
  copying session joined the prefix on.
- decision: An entry whose id is unique only within its file is headed by the token every
  other file cites: the file's own id, then a slash, then the entry's slug. Questions:
  `### <corpus>/<slug>`. Leads, proposals and candidates: `### <study>/<slug>`. The
  checker holds that the prefix is the file's own id, so the redundancy with the path
  cannot drift. What stands of d-2026-09-07-22: the slug's charset, uniqueness within the
  list, that it is authored once and never changed, and the one-token citation form.
- not taken: a short heading with the prefix joined at citation, which serves the writer
  and not the reader; ids unique across the repo, which needs a counter no file holds.

### A candidates file is its title, then entries and nothing else

- id: d-2026-09-07-32
- date: 2026-09-07
- raised by: the candidate schema review's first decision: the schema showed a fragment
  from `## C-014` on, with no title line, no rule for what else the file may hold and no
  rule for ids. Brian: "The title line mirror is fine", after d-2026-09-07-30 and
  d-2026-09-07-31 had settled the entry heading.
- decision: A file of the `candidates` or `iteration-candidates` class is
  `# <study> — candidates`, the study the file's own, then entries and nothing else: no
  head prose, no sections. An entry is `### <study>/<slug>` under d-2026-09-07-30 and
  d-2026-09-07-31, its keyed lines beneath it, the referee's and the outcome lines
  appended beneath those. What the entry carries, what the iteration file's prefix is
  and what the checker holds are the unit's later decisions.
- not taken: the fragment as it stood, with no whole-file shape for a writer to fill or
  a checker to hold; a file per target hypothesis, which would scatter one
  verification's findings across files.

### A verification's candidates file is an authored file, under docs beside verification.md

- id: d-2026-09-07-33
- date: 2026-09-07
- raised by: Brian: "Why is it under fanout? Where does other stuff live about the
  study?" then "Option 1 makes more sense". The file sat at `fanout/<study>/candidates.md`
  because the referee consumes it, never by a ruling; it is session-written, carries
  Brian's outcome lines and is read by him in promotion, the profile of an authored file.
- decision: The `candidates` class is `docs/v3-framework/<study>/candidates.md`, beside
  `verification.md`: the authored tree holds a study's two human-written records, the
  account and the findings with their outcomes, and `fanout/<study>/` holds only what the
  runner inlined or produced. The referee's itemizer reads the file there and the append
  step writes the referee's lines back there. Where a rewording's candidates live is
  decided with the rewording, after the verification case is settled.
- not taken: leaving it under fanout, the one authored document in the runner's tree,
  placed by the same reasoning that put the iterations folder under `referee/`.

### A lead carries no address; the word locus leaves the method

- id: d-2026-09-08-1
- date: 2026-09-08
- raised by: Brian, on the run-family unit's first dependency, a place grammar per corpus:
  "I never liked locus. I don't think I made it. Where did it come from?" and "A lead
  comes out of an exploration study. Why would it even cite any specific locations?" The
  word entered on 2026-09-02 in a session's design of the v1-archive reading pass, one
  word for a place that could be a plot point, a link, a chapter or a subject, and spread
  through forward-plan-2 into the skill. The frame read the leads' consumers from the
  skill and the record: the questions raised in review; the hypotheses minted from a
  lead under rule 6; what to itemize in preparing-to-verify; the bins, with arms, as a
  finding about the readers; corrections to a reading protocol; coverage and debt under
  rule 3; Brian's own reading of a corpus; questions across corpora. None resolves a
  lead to a passage; two need what the lead read, coarse; the join across arms needs a
  key, and the slice is one. The three retroactive artifacts point at stories and report
  sections, never at a passage.
- decision: A lead is what was seen and what it was seen in, in words, organised by what
  was seen; it is never joined on, checked at or organised by an address. Precision
  lives on one row only, the items manifest's, where an itemizer, code carrying no
  judgment, records the position it cut each item from in the form its corpus's reader
  gives; that form is declared with the manifest and reviewed at the first itemizer. The
  word locus leaves § Vocabulary, the leads-artifact row and schema, the reading-protocol
  schema, exploring-a-corpus, reviewing-leads and the corpora file. How the arms' lead
  sets are joined and binned is the leads review's decision; until then join-and-bin
  says only that it joins them. Two consumers the tables do not carry, minting reading
  the leads artifact and preparing-to-verify reading it, go on the leads review's list.
- not taken: an address grammar per corpus declared ahead of the manifest, which stalls
  the run family on a leaf only the itemizer needs; keeping the word for the manifest
  row alone, one word for one field; grounding the decision on what exploratory readers
  can cite, a claim about agents the method treats as a hypothesis.

### A study id names what its directions do, never a count

- id: d-2026-09-08-2
- date: 2026-09-08
- raised by: Brian, on the study folder: "This means the study slugs have to be more
  specific than just verification of v1 archive. It's not all verifications. That was
  what confused me earlier, until you reminded me that one study is one execution and
  repeat under the same instrument is a new study." The registry id form was
  `verification-of-<corpus>-<n>`, a corpus and an ordinal, which says nothing about
  what the study's directions do; § Vocabulary already makes a verification one
  execution of one set of directions over a corpus's items, a repeat being a new study.
- decision: A study id is `<type>-of-<corpus>-<slug>`, the slug lowercase `[a-z0-9-]+`
  naming what the directions do, unique across the registry, authored at the go and
  never changed: `verification-of-v1-archive-scene-notes`; `exploration-of-own-fiction`
  carries a slug only when the corpus is explored again under a different reading. No
  ordinal. The three registered explorations keep their ids, each being its corpus's
  only reading. The registry schema's id line and the tool's `registry.id` check change
  to this form.
- not taken: keeping the ordinal beside a slug, two ids for one study; an id naming the
  directions version, which changes as they are calibrated.

### A study is one set of directions over one itemizer's items; itemizers are tools

- id: d-2026-09-08-3
- date: 2026-09-08
- raised by: Brian, on the study folder: "Should a study even have multiple itemizers?
  What about an itemizer used in two different ways, via two different directions.md
  (formely a codebook) that have their own calibrations?" and, on calibrating with a
  subset, "This seems to point to there being a set of work and folders that sits
  between study and batch." § Vocabulary makes a verification one execution of one set
  of directions, a repeat a new study; preparing-to-verify itemizes once, authors the
  directions against real items, calibrates on a sample and only then runs the batch.
- decision: A study is the life of one set of directions over the items of one
  itemizer: the directions' versions and their calibrations, the calibration batches
  over a sample, the main batches, and for a verification its referee batches. Two
  itemizers, or one itemizer under two sets of directions, are two studies; no folder
  level sits between a study and its batches. An itemizer is code with tests under
  `tools/`, outside the record, run once per batch, writing the batch's index and its
  item bodies into the batch folder; a study holds no itemizer, and a second study
  cutting the same corpus the same way runs the same tool into its own folders. Item
  bodies are regenerable and uncommitted; the index is committed and names the itemizer
  and the corpus it read; each call hashes the item it received.
- not taken: a study holding several itemizers, which makes "one execution of one set
  of directions" false and needs a third folder level; an itemizer script per study,
  copied between studies that cut a corpus the same way; committed item bodies,
  regenerable data authored beside its source.

### A definition names its directions by path; the runner takes a definition and knows no root

- id: d-2026-09-08-4
- date: 2026-09-08
- raised by: Brian: "I thought there will be no concept of a 'root setting' in the
  runner. Every verb is parameterized by a file name for a batch's definition.md. The
  runner uses that to know what file paths for everything else relative to the passed in
  path of definition.md. It is the job of the skill's instruction text and the
  DocIntegrity checker to instruct and enforce that a claude code sessions follows the
  studies/<study>/batches/<batch>/definition.md rule." And: "what I need is the ability
  for a definition.md to define its directions file somewhere other than a fixed
  relative path?" The runner had refused a launch outside `fanout/` and scanned it for
  runs, a guard and a shape it owned; the referee's batches run under directions that
  belong to no study.
- decision: Every batch verb, dry-run, launch and tally, takes the path of a batch's
  `definition.md` and resolves the batch's other files relative to it; the host verbs
  take nothing. The runner holds no root, no folder rule and no notion of a study, and
  does not re-check a definition: where it sits and what it says are the Artifacts
  row's pattern and the definition's schema, held by the checker at the write. The
  definition's `directions` line is a relative path, resolved by the checker to a file
  that exists and passes its schema, and by the runner to the bytes it hashes and
  passes as the system prompt; the hash is the identity, and the line may point into
  any folder. The host's page lists the batches whose definitions it finds beneath its
  working directory, which is wherever it was started.
- not taken: a root setting that also fixed `<root>/<study>/<batch>`, which makes the
  runner know what a study is and leaves the referee's batches nowhere; a directions
  line confined to the batch's own study, the invariant that yielded to one referee for
  the method; a copy of shared directions into each study, same hash, drift on the
  first edit; the runner running the checker before a launch, which couples the two
  tools for a case the hook already covers.

### One referee for the method, never a study

- id: d-2026-09-08-5
- date: 2026-09-08
- raised by: Brian, on the referee's batches: "But that makes the referee a special case,
  which I'm trying to avoid. What is the conceptual contradiction happening, between what
  two proposed invariants?", then "What referee study? Is that a thing? Did I make it or
  not?", "But it is not a study. It is one step in verification.", and "What is the
  invariant that says referee is shared and reused? That feels stronger." The founding
  record's entry of 2026-09-05, the referee's preparation is an instance, made the
  referee a study of preparing-to-verify with `candidates` as its corpus, a session's
  construction under Brian's question whether it was bootstrapping; the registry schema
  carried it as `referee-<n>` with two exceptions. Rule 2, the runner skill and the
  two-inputs ruling each state the sharing without naming it as the invariant.
- decision: Every candidate of every verification is judged under the same directions
  at the same hash; the referee is one instrument of the method, and a verification
  runs it and never authors it. That sentence enters the constitutional rules. The
  referee's directions and their calibrations live in `docs/v3-framework/referee/`,
  belonging to no study; they are authored and calibrated by preparing-to-verify's
  author and calibrate processes once, and again only when a ruling in promotion
  changes them, with a sample spanning several hypotheses and verifications. Refereeing
  is a step of each verification: its batches, calibration batches included, are that
  verification's, under its folder, their definitions naming the referee's directions
  by path; a calibration file cites the batch it came from. The referee's itemizer is a
  tool under `tools/` reading a verification's candidates file and its targets'
  statements. The `referee-<n>` rows, the registry's exceptions for them and state.md's
  referee chain leave.
- not taken: a study type `referee`, regular in form but a study that is one step of
  every other; a standing folder that also holds batches, a second home for the
  definition class; a directions line confined to the batch's study, which the sharing
  overrides.

### A study's directions sit at its top, versioned by number, each calibration beside its version

- id: d-2026-09-08-6
- date: 2026-09-08
- raised by: Brian, on the study folder: "Directions are nested under batches, right?"
  then, on the two placements, "What is the difference between directions.md and
  calibration.md?", "How would a batch point to directions-N.md?", and "would a
  plausible setup in a real study folder be: a few different direction iterations, a few
  different calibration batches with the slugs to self describe, and then finally the
  full corpus batch that runs with a settled directions.md, whichever the final
  calibration I chose is?" The referee's folder took this shape under d-2026-09-08-5.
- decision: A study's directions are `directions-N.md` at the study's top, succeeded by
  number, each version a new file and never edited; a calibration is
  `calibration-<date>.md` beside them, frozen, titled with the body hash of the version
  it judged, citing the calibration batch it came from by token, and carrying the
  labels beside Brian's blind verdicts, the agreement per class, each disagreement with
  his ruling, and whether the version is accepted at that hash. A version is calibrated
  when an accepting calibration exists at its hash. A batch names its version by one
  relative path in its definition, `- directions: ../../directions-N.md`, and its calls
  record the hash; neither a number nor a hash is written in the definition. Nothing
  says which version is current: the accepted calibration and the full batch's
  definition say it.
- not taken: directions inside each batch folder, self-contained but two placements
  for one class once the referee's are outside every batch, and no citable version but
  a batch; a `current` pointer, derived text authored beside its source.

### A study is one folder, its batches inside it

- id: d-2026-09-08-7
- date: 2026-09-08
- raised by: Brian, on the run family: "I'm thinking a flat structure of batches/ at the
  repo level is incoherent. Why was that made and what are the alternatives?" A study's
  files sat in two trees whose only relation was a shared folder name, authored under
  `docs/v3-framework/<study>/` and the runner's under `fanout/<study>/`; the split fell
  out of the launch folder's name on 2026-09-03 and was never chosen; `fanout/` also
  held two things that were not studies, the skill audits and the smoke test, under a
  word, work, coined to cover the union. The contents were settled first, by
  d-2026-09-08-2 to d-2026-09-08-6.
- decision: A study is one directory, `docs/v3-framework/studies/<study>/`, named by its
  registry id. At its top: its authored artifacts; its `directions-N.md` versions and
  their `calibration-<date>.md` files; nothing else. Under `batches/<batch>/`, one
  folder per batch holding what that execution took in and produced, its files a later
  decision. No itemizer, which is a tool under `tools/`. The registry `studies.md` sits
  beside `studies/`. The audit is a study type, `audit-of-<slug>`, its corpus the skill,
  so the skill audits are study folders like any other. The smoke test is the runner's
  own check and leaves the record for the runner's tests. `fanout/` and the word work
  leave; the launch folder outside the repo is unchanged.
- not taken: a top-level `studies/`, which parts a study's folder from the hypotheses it
  cites; two trees rooted at `studies/`, the seam under better names.

### A call is the directions as system prompt, one item as the message, and JSON back

- id: d-2026-09-08-8
- date: 2026-09-08
- raised by: Brian, on the runner: "What is going into the input of a single claude code
  call, precisely, and delineated?", "I don't the model even needs to know the id of the
  item. The C# code can hold that in memory.", "I like replacing the coding agent
  prompt from claude code with a system prompt.", "It seems like it would be better if
  agent runner C# binary takes the claude code cli response text and does the writing
  instead.", "The call only has directions and the item?", and "The effort level is
  also a knob alongside model". The runner had composed one user message of a job
  header, an item line, boilerplate instructions, an output path, markers, the protocol
  and every input under hashed headings, and had the agent write its result with the
  Write tool; Claude Code's print mode validates output against a JSON Schema and the
  docs confirm no such thing for YAML.
- decision: A call is one execution of the Claude Code CLI in print mode with exactly
  these inputs: the body of the batch's directions, passed as the system prompt in
  place of Claude Code's own and hashed as the version every result cites; the item's
  text alone as the user message; and the batch's model and effort, both settings of
  the definition and recorded on every call. No tools unless the definition opts one
  in; no MCP; no transcript persisted; the launch folder outside the repo as before.
  Whatever a study needs the agent to hold beyond the item is in the directions body
  or in the item, by the itemizer; a call has no third input. The answer is JSON,
  enforced by the CLI against the JSON Schema the directions declare; the runner lifts
  it from the result event, renders it as the Markdown result file named by the item's
  id, and keeps the event stream. The model writes no file and is told no id, no path
  and no marker.
- not taken: the Write tool with a path in the prompt, an opaque string nobody
  validated; YAML, which nothing enforces; standing context as a third input of the
  call, a second thing to hash; keeping the model's JSON on disk beside the Markdown,
  two results for one item; model or effort per item, which makes two calls of one
  batch incomparable.

### The item is the unit; executing a batch is one call per item still without a result

- id: d-2026-09-08-9
- date: 2026-09-08
- raised by: Brian: "I don't like job as one launch of one child. Isn't a job usually
  the term for a batch execution..."; on the unit, "The item is the unit sounds good,
  but please check against potential ways this could bite"; on retries, "I don't want
  a failed out item to be have to go again in a new batch. There should be retries",
  then "Agreed on retry being a mode that launches only the items in a batch that don't
  have an entry yet. Then there is no retry logic within one batch."; on timeouts, "I
  don't think there should be absolute timeouts. Only a timeout if nothing is received
  from the streamed output."; on the verb, "Can you make it execute-batch?" The
  runner's rule one, one job one item, stood beside a convention that put several
  items in one job for shared context, which the two audit batches used; a job was
  retried automatically up to a ceiling, then failed and never launched again except
  under a new id; a twenty-minute wall clock applied to every job.
- decision: A call judges one item and never more; a batch is one call per item; and
  the item's id is the key of the index, the calls, the results and every citation,
  `<study>/<batch>/<item>`. What an item is, its grain, is decided at itemization and
  nowhere else. The verb is `execute-batch`: given a definition, the host makes exactly
  one call for every item that has no successful call yet, so the first execution
  calls everything, a later one calls what failed, and an execution naming one item is
  the pilot. The runner never retries on its own, and a call's number is the execution
  it belongs to. No call has an absolute time limit; a call is killed and recorded
  failed only when its stream has been silent for the definition's idle limit. A later
  execution changes nothing about the batch; anything else is a new batch.
- not taken: task or assignment as a noun for the unit, needed only while a call could
  take several items; automatic retries up to a ceiling, the guard against the runaway
  of 2026-08-27 in the one form that still decides on its own; a new id to run an item
  again; a wall-clock limit, which cuts off a long slice read for being long; launch,
  enqueue and submit as the verb, the first two the runner's own words for starting a
  process and for its queue.

### Directions are one class for every batch: frontmatter as record, body as the system prompt

- id: d-2026-09-08-10
- date: 2026-09-08
- raised by: Brian: "There should not be different terms for the system instruction
  equivalent. There should be one term, regardless of whether its contents are a
  directive to explore or a frozen an calibrated predicate and classes."; "Codebook is
  too unfamiliar to me as well."; "I think directions should just absorb what was
  formerly codebook, and applies to both verification and exploration. Each activity's
  instructions specify what the directions should be."; on the sections, "are these
  all going into the call's input? Do they all ought to be? Or is there too much
  content?" and "some fields need to be optional, because exploration has less rules
  than verification?"; on the output, "just as we're making schemas for the governed
  files, a directions.md also has to make a schema for that batch's outputs?" The
  codebook, from content analysis, and the reading protocol, from forward-plan-1, were
  two sessions' names for one kind of file, and the codebook's shape mixed what the
  agent needs with record-keeping under one hash.
- decision: One class, `directions`, for every batch of every study type and for the
  referee, in place of codebook and reading protocol; the word instrument names a
  text nowhere. A directions file has two parts. Frontmatter is record, never in the
  call: the questions the version freezes or reads with, cited `<corpus>/<slug>`,
  always for a verification and an exploration and never for the referee; other
  record fields as the schema declares. The body is the system prompt, verbatim, and
  its hash alone is the version every call cites, so a change to the frontmatter is a
  new file with the same hash and the same calibration. The body's sections are: what
  the agent is given, always; how to read, for an exploration and never otherwise;
  classes, criteria and, once a calibration has produced one, anchors, for a
  verification and the referee and never for an exploration; what to produce, always,
  as a JSON Schema whose fields are of four kinds, a label from the classes, one line,
  a block, or entries in a stated form; never, when there is one. The schema declares
  which sections each study type has and which are optional; the activity that
  authors a study's directions says what goes in them and how they are tested,
  calibration or a pilot. Rule 4 is reworded from codebook to directions. A pathfinder
  reads under Brian's prompt and has directions only when the same read is repeated
  under another model, and then one file both cite. The schema written under this
  entry is reviewed closely by Brian at its first file, and its refinement by later
  entries is expected.
- not taken: two classes under two names, when the study type already says which
  kind; the whole file as the system prompt, which made a citation edit a new
  calibration; a machine copy of the output declaration in the frontmatter beside
  prose for the agent, two declarations that can disagree; brief, procedure, rubric,
  guide and charge as the umbrella word, each colliding or covering one kind only.

### A batch folder is its definition, index, items, calls, results and tally, named by number and slug

- id: d-2026-09-08-11
- date: 2026-09-08
- raised by: Brian: "Fanout also seems awkward as a folder name"; "I don't really like
  manifest either?"; "Ledger reads like a financial tracking system"; "Not sure if
  journal is right anymore, and it was your word. Also not sure if jsonl is correct or
  if it should be markdown."; "batch.md semantically collides with index.md"; "Why is
  batch.json a json instead of markdown like the rest?"; "I don't think definition.md
  should be created by a define verb anymore. Should it be defined in a -schema... thus
  needing a DocIntegrity check?"; on the name, "Doesn't it need more than just date and
  model, since it has to describe what it's doing" and "Where did date come from? I'm
  going to be running batches on the same day. Seems like a number is needed." A run
  folder had held `run.md`, `jobs.json`, `items/manifest.md`, `ledger.jsonl`,
  `results/`, `tally.md` and `attempts/`, in three notations, two of them generated by
  scripts per study, under a name of `<date>[-<slug>]` from 2026-09-04.
- decision: A batch is `batches/<nn>-<slug>/`: the number sequential within the study
  from 01, assigned when the batch is defined and held by the checker; the slug
  lowercase `[a-z0-9-]+`, unique in the study, authored with the batch and never
  changed, naming what the batch does and, when two batches differ in one thing, that
  thing. Its files: `definition.md`, authored by a session, governed by its schema and
  held by the checker, naming the directions by path, the index, the model, the
  effort, the idle limit and any opted-in tool, never edited after its first
  execution; `index.md`, written by the itemizer, governed by its schema; `items/`,
  the bodies, uncommitted; `calls.md`, written by the runner, one entry per call in
  the record's keyed-line grammar; `results/<item id>.md`, written by the runner from
  the model's JSON, one per item; `tally.md`, written once by the runner;
  `attempts/`, the runner's prompts and streams, uncommitted. Nothing authored beyond
  the definition sits in a batch; the run page is cut and what it carried in words
  goes to the study's artifact. Everything is Markdown; nothing is JSON on disk.
- not taken: `run.md`, whose facts were derivable and whose words belong to the
  study; `jobs.json` and a generator, since the index is the item list; a `define`
  verb, since the checker holds the definition at the write; JSONL for the calls, one
  file in another notation; a date in the name, derived from the calls and useless on
  a day with three batches; batch, job, manifest, ledger and journal as names; a name
  that encodes the model or the directions version, derived text authored beside the
  definition.

### The runner's verbs are dry-run-batch, execute-batch, tally-batch, start and stop

- id: d-2026-09-08-12
- date: 2026-09-08
- raised by: Brian: "The tallier needs to become C# code instead of a powershell
  script"; "make-jobs.ps1 should become a verb of the agent runner console application,
  just like tally. Then generator is gone from the vocabulary."; "So split is from what
  was essentially one particular batch's itemizer? Not to be part of standard operating
  procedure generally? Should this be moved to its own C# program?"; "Is there nothing
  for a define verb anymore?"; "I do like the sibling verbs"; and "So the tally verb
  is going to be code that parses the markdown that comes out of the json to markdown
  converter that is made?" The runner had verbs for enqueue, a pilot, a dry run, split
  and stop, and each study was to bring a generator and a tallier as scripts beside
  its codebook; the one tallier that existed hardcoded the audit protocol's labels.
- decision: Three batch verbs, each taking a definition's path: `dry-run-batch`
  composes every call's prompt, hashes it and makes no call; `execute-batch` as
  d-2026-09-08-9; `tally-batch` writes `tally.md` once, reading the definition, the
  directions' what-to-produce and classes, the index, the calls and the result files
  as rendered, by the same parser that holds them to the declaration, the render and
  the parse being each other's inverse under one test and the model's JSON never read
  back; it gives counts per label field, the items whose label is in a set named on
  the command line, the malformed and the missing, free-text fields listed and never
  counted, and a grouping by any column of the index. Two host verbs, `start` and
  `stop`. The tally is C# with tests; the audit's `tally.ps1` retires with the verb's
  first run. No generator, no `define`, no `split`: the Markdown itemizer that was the
  split verb becomes a tool project of its own under `tools/`, the audit's itemizer.
  The runner reads no calibration and refuses nothing on that ground; that a batch
  runs under a calibrated version is rule 4's, held by the activity text and shown by
  state.md. Harness control on the page as before; nothing that changes what a call
  is.
- not taken: a tallier per study, a reading of the directions coded by hand each
  time; a generator, since the index is the item list; `define`, since the checker
  holds the definition; `split` inside the runner, an itemizer for one format that
  only the audit uses; the runner refusing an uncalibrated execution, which couples it
  to a file it has no other reason to read; the JSON kept beside the Markdown or its
  labels copied onto the call, two results for one item or derived text beside its
  source.

### Pathfinding is a batch of one item, the whole corpus, through the runner

- id: d-2026-09-08-13
- date: 2026-09-08
- raised by: Brian: "Pathfinding is full corpus one and done."; then, on where it runs,
  "There really should be a middle ground: outside the repo so there is no claude.md
  (which is about the story planner engineering and other stuff), no memories (which
  could be different on repeat runs), no skills loaded. MCP server optional. And the
  system prompt for claude code which is about being an agentic coding harness should
  be overridden with the directions I want"; and "Okay, make pathfinding a one-item
  batch. This is fine because we have the stream so I can see what it's doing without
  the interactive session." Every property asked for is a property of a runner call
  under d-2026-09-08-8, and the two a repo session lacks, the directions hash and the
  calls record, are what a repeat under another model needs. The pathfind row was a
  session in the repo; the 2026-09-04 factorial had pathfinder jobs under a 1M context
  as an exception to one job one item.
- decision: A pathfinder is an `agent` process: one batch whose index has one item, the
  corpus whole, written by an itemizer that concatenates a file corpus's texts or, for
  a corpus read through the MCP server, writes the item as what to read and how, with
  the definition opting the MCP server in. Its directions are the study's, how to read
  and what to produce as entries; its result is the leads as rendered; `leads.md` is
  written from that result by the session that joins or reviews, as from slice
  results. No pilot, since the one item is the batch. A repeat under another model is
  a second one-item batch, and the two are a comparison like any other. The
  exploration's scales are one item or many, under one activity. No exploration runs
  in the repo, and the 2026-09-04 pathfinder jobs are superseded in prose by this
  regular form.
- not taken: a session in the repo, which inherits CLAUDE.md, the skills and memory
  and records neither hash nor call; a bare CLI session started by hand outside the
  repo with a system-prompt flag, the runner with its guarantees removed, its write
  into the repo unhooked and its transcript archived nowhere.

### A study is one model too; a comparison is between studies, by the tally or by a study over them

- id: d-2026-09-08-14
- date: 2026-09-08
- raised by: Brian: "I don't like arm either."; "Do we need to name this concept or have
  to care about this concept, really?"; "I never deliberated this. What is the goal,
  what are the options from first principles?"; "Should a comparison between studies
  be a study of its own, whose corpus is the two studies? It's a higher order study.";
  "So should I allow two full batches within the same study (alongside the calibration
  batches) using different models? Or is a different model a different study, perhaps
  with the same instructions?" then "Go with the first". Arms, bins and the blinding
  key came in with the 2026-09-03 audit generator and the WU2.5 factorial, for
  explorations only; verification comparisons existed only as hypotheses 011, 047 and
  050. A calibration measures a model under the directions against Brian's verdicts,
  so a second model under the same directions is uncalibrated until it has its own.
- decision: A study is one set of directions, one itemizer, one model and effort; every
  definition in a study names the study's model, and the checker holds that they
  agree. A different model over the same directions is a different study, the
  directions the same file named by path, the calibration its own, the id's slug
  carrying the model when that is what differs. A comparison is therefore always
  between studies. Where the two share their directions, the tally compares them
  item by item and the agreement per class is a finding of a verification, a
  candidate like any other; for two explorations, the review reads both leads
  artifacts and their differences are leads about the readers, never counted. Where
  the comparison needs judgment, two sets of directions over one corpus or two
  explorations' lead sets matched, it is a study of its own whose corpus is the
  artifacts compared, itemized, under calibrated directions; rule 3's guard on
  reading the buildout's own outputs guards claims about a corpus, not claims about
  the method. No arm, no bin, no reason assigned at a join, no key, no blinding: the
  runner knows nothing of a comparison, and a blind sort, if ever wanted, is one call
  under explicit context. The arm-key row and schema, the leads schema's Bins section
  and the Never lines about opening the key leave; § Vocabulary gains comparison and
  nothing else.
- not taken: the model as a setting of the batch with a calibration per model inside
  one study, which makes calibrated a two-key question and a study two things;
  condition, trial, variant, setup, reader and pass as names for the varying thing,
  none needed once each model is its own study; a reason list applied at the join,
  two of whose three reasons could only be known by checking; counts of disagreements
  between free-text lead sets, a session's matching presented as a number.

### Pathfinding is a batch of one item, the whole corpus, through the runner

- id: d-2026-09-08-15
- date: 2026-09-08
- supersedes: d-2026-09-08-13
- raised by: d-2026-09-08-14, which made the model a property of the study; the
  superseded entry had said a repeat under another model is a second one-item batch of
  the same study. Brian's words that founded it stand: "Pathfinding is full corpus one
  and done."; "There really should be a middle ground: outside the repo so there is no
  claude.md ... no memories ... no skills loaded. MCP server optional. And the system
  prompt for claude code which is about being an agentic coding harness should be
  overridden with the directions I want"; "Okay, make pathfinding a one-item batch.
  This is fine because we have the stream so I can see what it's doing without the
  interactive session."
- decision: A pathfinder is an `agent` process: one batch whose index has one item, the
  corpus whole, written by an itemizer that concatenates a file corpus's texts or, for
  a corpus read through the MCP server, writes the item as what to read and how, with
  the definition opting the MCP server in. Its directions are the study's, how to read
  and what to produce as entries; its result is the leads as rendered; `leads.md` is
  written from that result by the session that joins or reviews, as from slice
  results. No pilot, since the one item is the batch. A repeat under another model is
  a second study, and the two are compared at the review as d-2026-09-08-14 says. The
  exploration's scales are one item or many, under one activity. No exploration runs
  in the repo, and the 2026-09-04 pathfinder jobs are superseded in prose by this
  regular form.
- not taken: a session in the repo, which inherits CLAUDE.md, the skills and memory
  and records neither hash nor call; a bare CLI session started by hand outside the
  repo with a system-prompt flag, the runner with its guarantees removed, its write
  into the repo unhooked and its transcript archived nowhere; a second model as a
  second batch of the same study, uncalibrated by that study's calibration.

### The runner's words: batch, item, call, directions, definition, index, calls, result, tally, itemizer

- id: d-2026-09-08-16
- date: 2026-09-08
- raised by: Brian: "What are all the jargon in the system as of today and what they
  mean? I want to replace all the vocabulary with my own. I need to know where each one
  came from." The review found four words his, fan-out, head, harness and itemizer,
  and the rest sessions' from the evenings of 2026-09-03 and 09-04; the renames were
  settled one by one in d-2026-09-08-8 to d-2026-09-08-15, and this entry records the
  set so that the sweep runs once.
- decision: The words of the runner and its artifacts, each defined in § Vocabulary or
  by its schema, are these and no others.
  The words in use: batch, one execution of a set of items under one definition; item,
  the one thing a call judges; call, one execution of the CLI for one item;
  directions, the file whose body is a batch's system prompt; definition, the batch's
  authored settings; index, the batch's list of items; calls, the runner's file of
  calls; result, the model's answer for one item as rendered; tally, the counts over a
  batch's results; itemizer, code that cuts a corpus into items; comparison, two
  studies over the same items; pilot, an execution naming one item; dry run, the
  checks an execution would make, in memory, with nothing written; host, page and
  ceilings as before; fan-out, head and harness in prose only.
  The words abolished, each removed from the skill folder, the agent-runner skill,
  CLAUDE.md and the tool's code wherever it appears: run, job, attempt, ledger,
  journal, manifest, protocol as the name of a file, instrument as the name of a
  text, instructions as the name of a field, generator, enumerator, arm, bin,
  condition, work, fanout, locus, codebook, reading protocol, split, define, launch,
  enqueue, markers, run page, and cell. The registry's `<run>` placeholder becomes
  `<batch>`. The frozen founding record and the retired plans keep their words.
- not taken: renaming only what collided and keeping the rest, which leaves a
  session's coinages beside Brian's; a glossary file, when § Vocabulary is the one
  place words are defined.

### Historical material moves to docs/v3-framework-historical/; paths to it are edited in place

- id: d-2026-09-08-17
- date: 2026-09-08
- raised by: Brian: "I don't want to pollute v3-framework with more historical record.
  What if all historical record goes to a new docs/v3-framework-historical?" and
  "attribution.csv is an itemizer which will be used. The rest should be moved out to
  historical, and paths referencing them should be edited in place." The two audit
  batches under `docs/v3-framework-historical/skill-audits/` are the runner's only executions, their results
  written by agents as Markdown blocks with no JSON to render from; beside them under
  `docs/v3-framework/` sat the closed founding record and spec pools, the retired
  plans, and the handoffs and drafts of revisions 1 and 2.
- decision: `docs/v3-framework-historical/` holds every closed thing: the founding
  record and the spec pools; forward-plan-1, forward-plan-2 and its handoff; the
  revision-2 handoffs, omissions draft and rulings; the retroactive referee handoff;
  the 2026-09-05 engineering handoff; process-map draft 1; VERSION-HISTORY draft 1;
  the WU1.4 execution plan and the WU2.15 plan; and `docs/v3-framework-historical/skill-audits/` whole, its
  protocol, scripts and two batches, unchanged. Closed: never written again, governed
  by no checker, one README saying what the folder is. `docs/v3-framework/` holds only
  live artifacts, and `WU1.4-v1-scene-instincts/` stays, its `attribution.csv` being
  the output of an itemizer a study will use. Every path that names a moved file is
  edited in place to the new location, in decisions.md's entries included, a path
  change being no change to a ruling; the rule that entries are never edited stands
  for their text. `fanout/referee/codebook.md` and `fanout/smoke-test/` are deleted,
  both in git. No batch is converted; the audit type starts with the next audit.
- not taken: converting the two batches, fabricating definitions and results for
  executions under other rules; leaving history under `docs/v3-framework/` beside the
  live artifacts; leaving stale paths in frozen text and a pointer beside them.

### A rewording re-queues findings into the studies that produced them; no rewording study

- id: d-2026-09-08-18
- date: 2026-09-08
- raised by: d-2026-09-08-16 abolished `fanout/`, where the iteration-candidates
  artifact sat; Brian asked whether parking the row was "a contradiction between
  invariants happening that is leading to a special case", and then "So it rewording
  ought to be a study?" It is not: a study is one set of directions over one
  itemizer's items under one model, and a rewording authors, cuts and calibrates
  nothing; it puts findings back in front of the referee. Every prior evidence entry
  cites the candidate it came from, `<study>/<slug>`, in a study whose candidates
  file is append-only.
- decision: When Brian rewords hypothesis NNN, iterate appends, to the candidates file
  of each study a prior evidence entry cites, one new candidate: the finding verbatim,
  the target NNN, provenance citing the original candidate's token and the rewording's
  date. Each such study then runs a referee batch over its new candidates, on Brian's
  go, and promotion reads them where it reads all candidates. The
  `iteration-candidates` row leaves; iterate writes `candidates`; referee-materialise,
  referee-append and promote read `candidates` alone. No rewording study and no
  fourth type.
- not taken: a study type `rewording`, the referee-as-study construction under another
  name; parking the row with a placeholder path, a second path for one class; folding
  the re-queue into whichever verification comes next, which ties a hypothesis's
  status to an unrelated study's timing.

### The leads checker holds five sections; the retroactive artifacts drop their empty Bins

- id: d-2026-09-08-19
- date: 2026-09-08
- raised by: d-2026-09-08-14 removed the leads schema's Bins section without naming the
  check that holds the section list, which d-2026-09-07-11 requires; `leads.sections`
  holds six sections in order, and the three retroactive artifacts each carry a Bins
  section whose whole content is "One arm: no disagreement was measured."
- decision: `leads.sections` holds Method, Questions in view, Leads, Proposed
  questions, Corrections, in that order, and `leads.title` is unchanged. The three
  retroactive leads artifacts have their Bins section removed, a fix to a file that is
  wrong under a right class, since the section records nothing; their Method
  sentence about one arm goes with it. The schema's Method placeholder no longer
  names arms or labels. Whether the retroactive artifacts are brought to the rest of
  the schema or the class starts again stays the leads review's decision.
- not taken: an optional Bins section, a section the class no longer has kept for
  three files that never used it; leaving the section and failing the three files at
  every check until their review.

### A unit's decisions are a queue of starting points, each the bottom of a stack

- id: d-2026-09-08-20
- date: 2026-09-08
- supersedes: d-2026-09-07-13
- raised by: Brian, on a unit whose first starting point became seven entries before it
  was written: "The 1 starting decision turned into 3 decisions, and now it's more. But
  please present these 3+ ones, one at at time... Treat it like a stack, where
  decisions that come out of deliberation are pushed on incrementally, and when we
  finally write something, it's popping from the stack."; on a list of eleven shown as
  a plan, "You cannot prescribe a list because deliberation before a decision can
  change the order or answers or frame."; and "Should it be a queue of widest first,
  where each is the bottom of a potential stack?" The superseded entry had the unit
  open with a list ordered widest-first and seen by Brian before the first decision,
  which stands, and a mid-unit decision "added to the list and placed in it", which
  the stack replaces.
- decision: A unit opens with a queue of starting points, one line each, ordered
  widest-first by the session and seen by Brian before any is taken; he reorders,
  removes or adds. Taking a starting point opens a stack with it at the bottom;
  deliberating any decision on the stack pushes the decisions it surfaces on top;
  nothing is written until it is popped, and the pop is from the top, so the starting
  point is written last and may name the entries written above it. A stack is worked
  to empty before the next starting point is taken. After each stack empties the
  queue is presented again, re-ordered and re-formed, since what was just decided
  changes what remains; the queue is never presented as settled, and the session
  never describes a remaining item as if it would land as written.
- not taken: no queue, the widest alone named each time, which hides the unit's scope
  from Brian for nothing since the session must know the rest to name the widest; a
  flat list a mid-unit decision is inserted into, which loses the order in which
  decisions depend on one another.

### A special case names the invariants in conflict, and one yields by decision

- id: d-2026-09-08-21
- date: 2026-09-08
- raised by: Brian, twice in one unit: on the referee's batches, "But that makes the
  referee a special case, which I'm trying to avoid. What is the conceptual
  contradiction happening, between what two proposed invariants? One of them has to
  yield."; on a parked row, "Is there a contradiction between invariants happening
  that is leading to a special case?" Both special cases dissolved once the two
  invariants were named: the referee's, between a batch belonging to the study whose
  directions it runs and everything about a verification's candidates sitting in its
  folder, and the row's, between one class one path and a batch belonging to the
  study it is a step of.
- decision: A proposal that carries an exception, a second path for one class, a
  placeholder, a row with a special corpus or folder, a rule with a named exemption,
  is not presented as an option until the session has named the two invariants whose
  collision produced it and shown the space of what each would cost to yield. The
  decision then says which invariant yields and why, and the exception is gone, or
  the entry records the exception as the deliberate cost of keeping both. The frame's
  vocabulary line gains this beside the word check: what invariants the finding
  touches.
- not taken: an exception recorded in a schema's prose, the founding record's habit
  with the referee, which carried two exceptions for four days; treating a special
  case as a naming problem, which today produced three names for one collision.

### A word's or a design's origin is read from the archive before it is kept; a recall about the method is checked the same way

- id: d-2026-09-08-22
- date: 2026-09-08
- raised by: Brian: "What are all the jargon in the system as of today and what they
  mean? I want to replace all the vocabulary with my own. I need to know where each one
  came from."; "Which files were provisioned by me versus thrown together ad hoc to
  meet my requirements/demands?"; "I also need to know when each thing entered the
  picture. Was it uniform or layers on layers?"; and, on a recall of his own, "please
  check code sessions for whether exploration studies are supposed to use agent
  runner. The previous method was my recall, which is insufficient." The archive
  showed four runner words his and the rest sessions', nine layers over five days,
  and an exploration design he had selected against a recall that said otherwise.
- decision: The frame's precedent step reads the code-sessions archive as well as
  decisions.md and the closed record: for a word, who first used it and when, and
  whether Brian typed it, selected it, or never used it; for a design, when each part
  entered, whose requirement it answered, and whether it was built for one instance
  and then generalised. A recall of Brian's about the method is a hypothesis under
  rule 8 like any other and is checked against the archive before a decision rests on
  it; the entry says what the archive showed. A session's word that Brian never used
  is presumed replaceable, and his typed word is presumed kept, until a consumer says
  otherwise.
- not taken: provenance from decisions.md and the closed record alone, which hold
  rulings and not coinages; asking Brian whether a word was his, which is the recall
  the step exists to check.

### A concept is named only after its consumers are listed; a part is placed only after its origin is known

- id: d-2026-09-08-23
- date: 2026-09-08
- raised by: Brian: "Do we need to name this concept or have to care about this
  concept, really?"; "Who will be reading these and cross referencing?"; "What is the
  generator for?"; "What is 'tallier' as of today?"; "Is this only about exploration
  or is there also comparison between verification?" Under those questions arm,
  condition, generator, define, run, work and the pre-sorted disagreement dissolved,
  each having been named before anyone asked who used it; and the runner's files
  turned out to be one audit's conveniences generalised into the layout.
- decision: Before a word is chosen for a thing, the session lists the thing's
  consumers, who reads it, who writes it, what each does with it, and asks whether
  the thing needs a name at all; a thing no consumer reaches for by name is not
  named, and a thing whose consumers are all one other thing's is folded into it.
  The frame's "what exists under the change" gains, per part, when it entered, whose
  requirement it answered, and whether it was built for one instance and then
  generalised; a part built for one instance is presumed that instance's until a
  second consumer is shown. The first real files of a class are evidence of what the
  class must hold and never a template for how its files look.
- not taken: naming for readability alone, which is how a session's metaphor becomes
  a term; treating the audit's two runs as the model of a batch, which would have
  designed for the skill audit again.

### Everything decidable is decided before an autonomous run; the run ends at the first act reserved to Brian

- id: d-2026-09-08-24
- date: 2026-09-08
- raised by: Brian: "What will be the next run of autonomous work, what causes it to
  stop, and what has to be decided before that can start?"; "And what else is still
  open before the autonomous work? Consider carefully. I am skeptical that this is
  everything that blocks the autonomous work."; and, on a run with stops planned
  inside it, "I don't want planned stops. By definition it's autonomous. The planned
  stop is when auto ends. The methodology should try to get as much as possible
  decided so that things aren't made on the fly." The second enumeration had found
  four choices the first missed, among them the check ids of three schemas, which
  d-2026-09-07-11 requires a decision to name before a checker is armed; the
  session had proposed stopping inside the run for them.
- decision: Before a run of autonomous work, the session enumerates every choice the
  run would otherwise make on its own, walking everything the run will write and
  asking of each thing what an entry has fixed and what it has not; schemas, rows,
  paths, checks, verbs and files are where such choices have hidden so far, and the
  walk is not confined to them. Each choice found is decided before the run, as an
  entry, or the run does not start. A run has no planned stop. It ends at the first
  act the method reserves to Brian, among them an entry to approve, a check to arm
  without its decision, a first file to review, a commit. A run that meets a choice
  it did not enumerate ends there, pushes the choice, and takes nothing; what it has
  done to that point stands as far as it is consistent with what was decided.
  Brian's skepticism is answered by the enumeration being shown, never by a sentence
  that nothing remains.
- not taken: stops planned inside a run for decisions the session foresaw, which
  makes the run a schedule of Brian's decisions instead of autonomous work; starting
  the run and returning with what it decided, the founding pattern this method was
  rewritten to end; a run that decides small things and reports them, since which
  things are small is the choice.

### A file is governed by its path or by reference; the checker follows the reference both ways

- id: d-2026-09-08-25
- date: 2026-09-08
- raised by: Brian, on the referee's directions, which no study's row could match: "I
  thought directions became a relative path that definition.md writes, and the
  consumers must follow the trail" and "It should be governed by reference only, with
  C# Code that follows the reference? Otherwise there is a special case which should
  not happen." A second row for the referee's file had been proposed; d-2026-09-08-4
  had already made the directions line a path resolved wherever it points.
- decision: A file is governed in one of two ways. By path, when an Artifacts row's
  pattern matches it, as before. By reference, when a governed file's declared
  reference resolves to it: the checker holds the target to the schema the reference
  declares. The checker follows the trail both ways: checking a referencing file
  checks its targets, and on a write to a file no row matches, the hook searches the
  referencing classes for a reference that resolves to it and checks it as that
  target; `check .` walks every reference. Where a file sits is never a ground for
  governing it. The referee's directions and calibrations are governed this way and
  by nothing else; no row names them.
- not taken: a second class for the referee's files, one row per home for one shape;
  a file left unchecked at its own write until a definition names it; a file governed
  because it sits beside a governed one, a location made into a rule.

### A definition names the calibration that licenses its batch; the checker holds the three agree

- id: d-2026-09-08-26
- date: 2026-09-08
- raised by: Brian, on a sentence in the draft directions schema: "'The schema declares
  a calibration sits besides the version it judges' - what is doing this and where did
  it come from? Is this just something that wasn't deliberated or tightened?" It was
  not: d-2026-09-08-6 placed a calibration beside its version as a location, the
  session had turned the location into a ground for governing the referee's
  calibration, and nothing in the record referenced a calibration at all, since a
  calibration references its version by the hash in its title and no file references
  it. d-2026-09-08-12 had left "a batch runs under a calibrated version" to the
  activity text and state.md.
- decision: A definition whose batch runs under calibrated directions carries one
  line, `- calibration: <relative path>`, naming the accepting calibration of the
  version its `directions` line names; absent for an exploration's batch and for a
  calibration batch. The checker holds the three together at the definition's write:
  the directions resolve, the calibration resolves, the calibration's title hash
  equals the directions' body hash, and its verdict is accepted. Rule 4's gate is
  thereby held at the write, and the referee's calibrations are governed by reference
  through the referee batches' definitions. Beside is where a calibration sits and
  nothing more.
- not taken: a calibration governed by sitting beside a referenced file, a location
  made into a rule; the gate left to prose and state.md, which lets an uncalibrated
  batch be defined and noticed later; the runner reading calibrations, which
  d-2026-09-08-12 refused and this does not need.

### Directions are one class for every batch: frontmatter as record, body as the system prompt; classes and criteria, no anchors; fields declared, the schema derived

- id: d-2026-09-09-1
- date: 2026-09-09
- supersedes: d-2026-09-08-10
- raised by: Brian, reviewing the draft directions schema: "where did Criteria come from?
  What about boundary statement? What about anchors? What do these do and why do they
  exist?", "Are anchors going to lead to overfitting?", "If classes and criteria are
  givens, do we even need custom json schema or is it always going to be the same? Or,
  we still need to specify free text fields, telling the model what to put in there.
  Where would that go?" The archive shows the triad of definition, decision rule and
  anchor example entering on 2026-09-04 from a session's reanalysis of the referee's
  codebook, the content-analysis coding-scheme shape; Brian selected criteria as the
  name for decision rule on 2026-09-07 and typed none of the three. Anchors drawn
  from calibration disagreements overfit the model to those items and leak them into
  the next calibration's sample; a hand-written JSON Schema repeats the classes as an
  enum and is derivable from a declaration of the fields. The superseded entry's
  founding words and the rest of its ruling stand.
- decision: One class, `directions`, for every batch of every study type and for the
  referee, in place of codebook and reading protocol; the word instrument names a
  text nowhere. A directions file has two parts. Frontmatter is record, never in the
  call: the questions the version freezes or reads with, cited `<corpus>/<slug>`,
  always for a verification and an exploration and never for the referee or an
  audit. The body is the system prompt, verbatim, and its hash alone is the version
  every call cites, so a change to the frontmatter is a new file with the same hash
  and the same calibration. The body's sections: what the agent is given, always;
  how to read, for an exploration and never otherwise; classes, each a label and
  what an item shows, one reserved for an item the criteria cannot place, and
  criteria, the rules that decide an item between classes, both for a verification,
  the referee and an audit and never for an exploration; what to produce, always, as
  keyed lines, one per field, each naming the field's kind, a label from the classes,
  one line, a block, or entries in a stated form, and for a free field what the
  model is to put in it; never, when there is one. No anchors: a calibration ruling
  becomes a criterion stated generally, and the item it came from stays in the
  calibration file. The JSON Schema the CLI enforces is derived by the runner from
  the what-to-produce lines and the classes, and is written nowhere. The schema
  declares which sections each study type has and which are optional; the activity
  that authors a study's directions says what goes in them and how they are tested.
  Rule 4 is reworded from codebook to directions. A pathfinder's directions exist
  only when the same read is repeated under another model. The schema written under
  this entry is reviewed closely by Brian at its first file, and its refinement by
  later entries is expected.
- not taken: anchors under criteria, examples harvested from disagreements, which
  overfit and leak; a hand-written JSON Schema in the body, an enum repeating the
  classes and a form a tool derives; classes and anchors with no criteria, rules by
  example only; two classes under two names; the whole file as the system prompt;
  brief, procedure, rubric, guide and charge as the umbrella word.

### The directions schema is written; its six checks and its row are named

- id: d-2026-09-09-2
- date: 2026-09-09
- raised by: d-2026-09-09-1 fixed the class; the schema file was drafted from it and
  reviewed by Brian: "Good enough for a first draft, to be tested later." Under
  d-2026-09-07-11 a check is armed only after a decision names its id.
- decision: `schemas/directions-schema.md` is the schema of the class `directions`,
  in the four sections, its Example the checker's fixture. The Artifacts row is
  `directions` at `docs/v3-framework/studies/<study>/directions-N.md`, mutation
  succeeded; the referee's directions are the same class governed by reference under
  d-2026-09-08-25, with no row. The checks, held by DocIntegrity from the run that
  builds them: `directions.frontmatter`, `directions.sections`, `directions.classes`,
  `directions.criteria`, `directions.output`, `directions.version`, each failing as
  the schema's Checks section says. The codebook and reading-protocol rows and their
  schema files leave. The first directions file written under the schema is
  presented to Brian before a second, and the schema is expected to be refined by
  later entries.
- not taken: arming the checks from the draft without this entry; keeping
  codebook-schema and reading-protocol-schema beside the new file until their
  classes' next review, two schemas for classes that no longer exist.

### A Shape types every field from one vocabulary; a checker holds the types through one engine

- id: d-2026-09-09-3
- date: 2026-09-09
- raised by: Brian, reviewing the draft index schema: "There's no column defining types
  of the columns/keys? Wouldn't sqlite require that? Would a document model require
  it?", and on the enum, "Is label a document model established word or not?" then
  "Yes, use enum." The Shape tables of the landed schemas said each field's type in
  prose, and each checker re-implemented the prose by hand; the results grammar
  already had four kinds, one of them called label, a classifier's word and no schema
  language's. Entry d-2026-09-07-3 refused converting unreviewed schemas in one pass.
- decision: The skill schema declares a type vocabulary for the record's fields: slug,
  a lowercase `[a-z0-9-]+` unique in its file; token, `<file's id>/<slug>`, resolving
  to an entry of the class named; id, `NNN`, resolving to a hypothesis file; path, a
  relative path resolving to a file of the class named; date, `YYYY-MM-DD`; timestamp,
  `YYYY-MM-DDTHH:MM`; hash, SHA-256 hex; enum, one of a closed set the schema names,
  or for a result field the directions' Classes; line, free text without a newline;
  block, free text with continuation lines; list, space-separated values of one of
  the above. A Shape table has a `type` column from now, each field one type with any
  further constraint stated beside it; the two landed schemas, decisions and
  questions, gain the column at once from their prose; a directions declaration says
  `enum` where it said label, and label is a prose word only. DocIntegrity holds a
  Shape's types through one engine that reads them; the directions, index and
  definition checkers are built on it, and the decisions and questions checkers are
  migrated onto it in the same run with a predicted first run of zero failures. A
  checker whose schema has no typed Shape stays as it is and migrates at its class's
  review, when its Shape is written.
- not taken: types in prose, eleven parsers for one grammar; label as a type, which no
  schema language has; writing typed Shapes now for the four unreviewed classes so
  their checkers could migrate, the mechanical pass d-2026-09-07-3 refused.

### The index schema is written; its five checks and its row are named; the field is locator

- id: d-2026-09-09-4
- date: 2026-09-09
- raised by: d-2026-09-08-3 and d-2026-09-08-9 fixed the class; the schema file was
  drafted and reviewed by Brian, who ruled the field's name on the way: "Isn't origin
  (the owner) and 'position' different concepts? ... It needs to be corpus agnostic.
  v1 archive is a single sqlite db while fimfiction stories are different markdown
  files for each story.", "Locator is best", and for the head line "locator notation
  makes the most sense"; then "Good for first pass, proceed to next step". Position
  had been the session's word in d-2026-09-08-1 and locus before it.
- decision: `schemas/index-schema.md` is the schema of the class `index`, in the four
  sections, its Example the checker's fixture. The Artifacts row is `index` at
  `docs/v3-framework/studies/<study>/batches/<batch>/index.md`, mutation frozen. An
  item's field for retrieval is its `locator`: whatever retrieves the item from the
  corpus through the reader CORPORA.md names, one part or several as the corpus needs,
  in the form the index head's `locator notation` line states, which is the
  itemizer's and is reviewed at that itemizer's first index. The word position leaves
  with the rest. The checks: `index.title`, `index.head`, `index.table`, `index.item`,
  `index.locator`, each failing as the schema's Checks section says. The first index
  written under the schema is presented to Brian before a second.
- not taken: origin, which named the owner and not the place; position, which named
  the place and not the owner; a grammar per corpus fixed in the schema ahead of any
  itemizer, which d-2026-09-08-1 refused; arming the checks without this entry.

### A Shape is written in one grammar; the engine derives a JSON Schema from it, validates by the standard, and resolves references itself

- id: d-2026-09-09-5
- date: 2026-09-09
- supersedes: d-2026-09-07-19
- raised by: Brian, on the definition schema: "Are the values for the present column ad
  hoc or governed?", "Shouldn't this be tightened in skill-schema?", "What else is
  lacking? This seems like an obvious miss", "Is this the end of the escalation of
  governance rules?", "Will this simplify the code?" and "Is this the right way to do
  it from first principles?" then "Let's go with that". The superseded entry had a
  Shape declare five things in prose, which each schema then declared in its own
  prose and each checker parsed by hand; the walk found references naming their
  class only in the value column, sections declared three different ways, entry
  headings and appended lines as sentences, table columns with no present rule, six
  title forms, and Checks, Queries and Example held by nothing. A home-made walk of a
  file against a table would have been the validator the results decision refused
  in favour of a derived JSON Schema. The superseded entry's schema-review procedure
  stands as written in revising-the-method.
- decision: A Shape is written in one grammar, declared once in the skill schema, and
  it is the last level: what holds it is the engine's tests, and no document governs
  it. The grammar: a title of the form `# <own id> — <class word>` for every class,
  the own id being the file's, the folder's or the study's as the schema says; a
  sections table with the columns section, present and holds, holds one of fields,
  entries, table, prose, fenced; a field table with the columns key, present, type and
  value; a type from the vocabulary of d-2026-09-09-3, a reference type naming its
  target class, `path to <class>`, `token of <class>`, `id of <class>`, and `list of`
  any type; present one of `required`, `optional`, or `conditional: <condition in
  words>`, every conditional field named in a Checks row that holds its condition; an
  entry array declared by its heading's type and its field table; an appended line
  declared by its form, its multiplicity and the process that writes it; a table
  class declared by a column table with present and type; a file whose sections
  carry different mutations naming each; Checks with the columns check and fails
  when, ids `<class>.<name>` unique in the file; Queries with the columns question
  and how; an Example whose first fenced block is the fixture. The engine holds a
  Shape thus: under the document model a governed file is one object, keyed lines its
  properties, `###` entries an array of objects, a table an array of rows,
  frontmatter an object, prose a string, by one fixed mapping that the runner uses
  in the other direction to render a result; the engine parses the file to its
  object, derives a JSON Schema from the Shape as the runner derives one from
  What to produce, validates the object by a standard validator, resolves every
  typed reference itself, and applies the class's own rules, the ones no schema
  language expresses. The skill schema's "A schema file" paragraph states the
  grammar, and `schema.fields` holds it for every schema in the four-section shape;
  `schema.shape` holds the title alone until the last converts. The five Shape
  tables that exist are brought to the grammar now. A new type, present value or
  holds value is an entry adding a word, never a new level.
- not taken: declarations in each schema's own prose, one parser per schema; a
  home-made validator walking files against tables, when a standard does that part;
  a condition grammar the engine evaluates, a language for a handful of cells; a
  document above the skill schema, which would govern nothing the engine's tests do
  not; waiting for the last schema to convert before holding the five that have.

### A Shape is one grammar, defined in the skill schema, compiled to JSON Schema and held by the standard

- id: d-2026-09-09-6
- date: 2026-09-09
- supersedes: d-2026-09-09-5
- raised by: Brian: "Are the values for the present column ad hoc or governed?",
  "Shouldn't this be tightened in skill-schema?", "Is this the end of the escalation
  of governance rules?", "Is this the right way to do it from first principles?", on
  the title clause "What is this all for?", and on the whole "keep things as simple
  as possible while keeping the rigor such that this is the last escalation". The
  superseded entry carried the grammar's definition and a title rule of the
  session's that had two exceptions before it was written.
- decision: Every Shape is written in one grammar, defined in `schemas/skill-schema.md`
  § A schema file and nowhere else: sections, fields and entries as tables with the
  columns key, present, type and value; present one of required, optional,
  conditional; types from d-2026-09-09-3, each defined there by its JSON Schema
  fragment, references naming their target class. The engine compiles a Shape to a
  JSON Schema by substituting those fragments, parses a governed file to one object
  by the mapping the runner also renders results with, validates by a standard
  validator, resolves references itself, and applies the class's own rules named in
  its Checks. `schema.fields` holds every converted schema to the grammar. This is
  the last level: the grammar is held by the engine's tests, and a new word in a
  vocabulary is an entry, never a new level. A title is a class's own to declare and
  hold; the grammar says nothing of titles.
- not taken: the grammar restated in the entry, the definition beside its record; a
  title rule in the grammar; a home-made validator; Shapes written as raw JSON Schema,
  which loses the writer's table for nothing the fragments do not keep.

### The item is the unit; executing a batch is one call per item still without a result; the idle limit is the host's

- id: d-2026-09-09-7
- date: 2026-09-09
- supersedes: d-2026-09-08-9
- raised by: Brian, reviewing the draft definition schema: "idle limit should be a
  property of the long lived agent runner program, adjustable via the head". The
  superseded entry had made it a field of the definition; its founding words stand.
- decision: A call judges one item and never more; a batch is one call per item; the
  item's id is the key of the index, the calls, the results and every citation,
  `<study>/<batch>/<item>`; what an item is, its grain, is decided at itemization
  and nowhere else. `execute-batch`, given a definition, makes one call for every
  item with no successful call yet: the first execution calls everything, a later
  one what failed, and one naming an item is the pilot. The runner never retries on
  its own; a call's number is its execution's. No call has an absolute time limit; a
  call is killed and recorded failed only when its stream has been silent for the
  host's idle limit, a host setting beside its parallel ceiling and usage cap, in its
  configuration and adjustable on its page, never a batch's. A later execution
  changes nothing about the batch; anything else is a new batch.
- not taken: an idle limit per batch, a harness setting in the record; task or
  assignment as the unit's noun; automatic retries; a new id to run an item again; a
  wall-clock limit; launch, enqueue and submit as the verb.

### A Shape is one grammar, defined in the skill schema, compiled to JSON Schema and held by the standard; present is required or optional

- id: d-2026-09-09-8
- date: 2026-09-09
- supersedes: d-2026-09-09-6
- raised by: Brian: "Should conditional be a thing at all? That just seems to make the
  checker's job harder and the grammar more complex. Where did it come from?" It was
  the session's word from the morning's sorting of the present column; a condition
  in words is held by a hand-written check in every case, so the word promised what
  the engine does not do. The rest of the superseded entry stands, with its founding
  words.
- decision: Every Shape is written in one grammar, defined in `schemas/skill-schema.md`
  § A schema file and nowhere else: sections, fields and entries as tables with the
  columns key, present, type and value; present `required` or `optional`, any rule
  about when an optional field appears being a class's own, named in its Checks;
  types from d-2026-09-09-3, each defined there by its JSON Schema fragment,
  references naming their target class. The engine compiles a Shape to a JSON Schema
  by substituting those fragments, parses a governed file to one object by the
  mapping the runner also renders results with, validates by a standard validator,
  resolves references itself, and applies the class's own rules. `schema.fields`
  holds every converted schema to the grammar. This is the last level. A title is a
  class's own to declare and hold.
- not taken: `conditional: <words>` as a present value, a promise the engine could not
  keep; conditions in a grammar the engine evaluates, a language for a handful of
  cells.

### The definition schema is written; its seven checks and its row are named; a batch declares its kind

- id: d-2026-09-09-9
- date: 2026-09-09
- raised by: d-2026-09-08-4, -8, -11, -14 and -26 fixed the class; the schema file was
  drafted and reviewed by Brian, whose idle-limit ruling became d-2026-09-09-7 and
  who asked of the calibration line "How would the conditional calibration present
  be checked by the checker? Is it incoherent to let that stand?" It was: whether a
  batch is a calibration sample is known only to the session defining it, and the
  draft had the checker read it from the line's own absence or from a slug. The draft
  also left out an `index` line, the index being always `index.md` beside the
  definition.
- decision: `schemas/definition-schema.md` is the schema of the class `definition`,
  in the four sections, its Example the checker's fixture. The Artifacts row is
  `definition` at `docs/v3-framework/studies/<study>/batches/<batch>/definition.md`,
  mutation frozen. Its fields are `directions`, `kind`, `calibration`, `model`,
  `effort`, `tools` and `mcp`: `kind`, present exactly when the directions have
  Classes, is `sample`, the batch a calibration is scored on under a version not yet
  accepted, or `full`, a batch under an accepted version; `calibration` is present
  exactly when `kind` is `full`. No `index` line. The checks: `definition.batch`,
  `definition.title`, `definition.fields`, `definition.directions`,
  `definition.calibration`, `definition.model`, `definition.frozen`, each failing as
  the schema's Checks section says; `definition.frozen` reads the hash the calls file
  records at the first execution. The first definition written under the schema is
  presented to Brian before a second.
- not taken: a calibration batch told apart by its missing line or its slug; an
  `index` line always holding `index.md`; arming the checks without this entry.

### Historical material moves to docs/v3-framework-historical/; the retroactive explorations go with it

- id: d-2026-09-09-10
- date: 2026-09-09
- supersedes: d-2026-09-08-17
- raised by: the run's enumeration found eight files under `docs/v3-framework/` on no
  list, and Brian ruled: "Those 8 files are historical. Also the leads are historical
  retroactive or not?" The three retroactive explorations were in-repo sessions of
  2026-08-31 and 09-01 under the full stack, re-housed with registry rows on 09-06;
  under d-2026-09-08-15 an exploration is a batch through the runner with directions
  and a calls file, and none can be brought to that without running again. The
  superseded entry's founding words and ruling stand, extended.
- decision: `docs/v3-framework-historical/` holds every closed thing: the founding
  record and the spec pools; forward-plan-1, its ordering audit, forward-plan-2 and
  its handoff; the revision-2 handoffs, omissions draft and rulings; the retroactive
  referee handoff; the 2026-09-05 engineering handoff; process-map draft 1;
  VERSION-HISTORY draft 1; the WU1.1, WU1.2 and WU1.3 syntheses; the WU1.4 execution
  plan and the WU2.15 plan; the consolidation plan and consolidation 1; the raw
  pipeline hypotheses; the 2026-09-06 code-session audit; the three retroactive
  explorations, `exploration-of-fimfiction-stories`, `-google-keep` and
  `-own-fiction`, whose registry rows leave, so that the registry is empty until the
  first study under the method; and `docs/v3-framework-historical/skill-audits/` whole. Closed: never
  written again, governed by no checker, one README. `docs/v3-framework/` holds only
  live artifacts: decisions.md, studies.md, the hypotheses, the questions, the
  studies, `methodology-revision-1.md`, `implementation-candidates.md`, and
  `WU1.4-v1-scene-instincts/`, an itemizer's output a study will use. Every path
  naming a moved file is edited in place, decisions.md's entries included, a path
  change being no change to a ruling. `fanout/referee/codebook.md` and
  `fanout/smoke-test/` are deleted, both in git. No batch and no exploration is
  converted; what a retroactive artifact proposed enters a question list when Brian
  asks it.
- not taken: the three explorations brought to the leads schema, sessions with no
  hash and no calls dressed as batches; their rows kept in the registry, three studies
  the method cannot account for; converting the audit batches; leaving history beside
  the live artifacts.

### Results are the runner's: no schema file; the mapping is code; the tally and the checker both hold them

- id: d-2026-09-09-11
- date: 2026-09-09
- raised by: the run's enumeration asked where the sentence "a result's keys are its
  directions' declaration" lives; the session offered a results-schema file, and
  Brian: "I thought this is defined in C# code", "are query patterns needed in a skill
  somewhere?", and on who confirms a result matches its directions, "Have both". The
  mapping from the model's JSON to the file is the runner's, and the tally parses it
  back by the same code.
- decision: The `results` row links no schema, like `calls` and `tally`. A result is
  `results/<item>.md`, one per item of the index, its keys the directions' What to
  produce in that order, an enum or line as `- key: value`, a block with continuation
  lines, a list of line as bullets; the runner skill states this where it documents
  the mapping, and carries one Queries table for results, calls and tally, one grep
  per question a reader asks of them; the row's description says what a result is in
  a few words. Two tools hold a result: the tally reports any that do not match the
  declaration as malformed when it is written, and DocIntegrity's `check .` follows
  a batch's definition to its directions, compiles the declaration as it compiles a
  Shape, and validates every result file, so a bad or edited result is caught at any
  check; the check id is `results.declared`.
- not taken: a results-schema file, the mapping restated in words beside the code
  that is the mapping; the tally alone, which reads results once and never again;
  queries nowhere, which leaves the most-read file without its greps.

### Three existing checks change what they hold: registry.type, registry.corpus, hypothesis.evidence.citation

- id: d-2026-09-09-12
- date: 2026-09-09
- raised by: the run's enumeration, under d-2026-09-07-11, which requires a decision to
  name a check it changes. d-2026-09-08-2, -5, -7 and -16 changed what three checks
  hold without naming them.
- decision: `registry.type` holds that a study's type is the one its id's prefix
  names, `verification-of-` verification, `exploration-of-` exploration, `audit-of-`
  audit, with no referee case. `registry.corpus` holds that the corpus cell is an id
  in CORPORA.md, `verified-artifacts` for a study over the buildout's own outputs, or
  `skill` for an audit, with `candidates` no longer a value. `hypothesis.evidence.citation`
  holds the evidence entry's citation as `(<study>/<slug>; directions-N@<hash>)`, the
  candidate's token and the directions version and body hash it was judged under, in
  place of a codebook. Each fails as before otherwise.
- not taken: arming the changed checks from the entries that changed the classes,
  which did not name them.

### The findings artifact replaces the verification artifact

- id: d-2026-09-09-13
- date: 2026-09-09
- supersedes: d-2026-09-07-29
- raised by: Brian, reading the candidate unit's frame: "a call is only answering one
  instruction about one item. That is not a finding. Someone is aggregating all of
  those results/<item>.md and analyzing that data and the counts in order to get
  meaning out of it"; then "Agreed that findings.md is what holds analysis, and
  verification.md is dropped", and on the frame, "Yes, replacing with findings". No
  process in revision 2 wrote the analysis of a verification: revision 1 held it
  inside the one session between "write the artifact" and "write the candidates
  file", unnamed; the 2026-09-05 rewrite made the artifact a shell of tool output,
  four sections now derivable from the batch files and one a copy of tally.md, and
  made write-candidates a per-result mapping; state derived a question's answered
  state from a list of questions with no answer behind it.
- decision: The class `findings`, `docs/v3-framework/studies/<study>/findings.md`,
  schema `findings-schema.md`, replaces `verification-artifact`. It holds Method, only
  what no batch file says: what was deliberately not measured and any caveat of the
  execution; Findings, entries headed `### <study>/<slug>`, each naming the question
  it answers, the finding in words, and its citations to the tally sections and the
  items it rests on; Corrections, appended and dated; Promotion, appended by the
  promotion session. Counts leave, cited from the batch's tally; the derivable method
  leaves, the definition, index and calls being the method; Questions answered leaves,
  a question being answered when a finding names it, derived. A finding reaches a
  record only through a candidate. What stands of the superseded entry: the study
  type, its id and its activity; only the artifact's name and content change.
- not taken: a Findings section added to verification.md, which keeps the mirror of
  the tally and a name that says nothing about findings; two files, the shell beside
  the findings; a findings class apart from the study's account, whose readers are
  exactly this file's; findings born on candidates, which puts the analysis in the
  party that claims.

### The findings file is Method, Findings, Proposed questions and Shortcomings; an entry is never edited

- id: d-2026-09-09-14
- date: 2026-09-09
- supersedes: d-2026-09-09-13
- raised by: Brian, iterating the findings schema's first draft: on Corrections, "correction
  is caused by me reading the finding and my instinct/recall disagree, so I want to
  investigate, human in the loop style, by having claude code check the items"; on
  Promotion, "This should be dropped then, because promotion is about candidate ->
  evidence, not finding -> candidate"; and on the shortcomings of the itemizer and the
  rest of preparing, "What about analysis of shortcomings of the itemizer or other parts
  of the prep stage?" The superseded entry had Corrections as a section and Promotion
  appended by the promotion session, the latter the persisted form of revision 1's
  end-of-promotion report, every part of which is now derivable from candidates.md or
  sits on an outcome line.
- decision: `studies/<study>/findings.md` holds Method, only what no batch file says;
  Findings, entries `### <study>/<slug>`; Proposed questions, one line each, what the
  data raised that no question asked and no finding claims, none a question until Brian
  writes it into the list; and Shortcomings, one line each, what the results showed
  wrong with the study's own instrument, the line's first word naming the part, `item`,
  `itemizer`, `directions`, `calibration`, `execution` or `corpus`. No Corrections
  section and no Promotion section. A finding entry is never edited: one checked and
  found not to hold takes an appended `withdrawn` line with the date and what the
  results or items showed; one that holds amended is a new entry naming the old in
  `supersedes`; a standing finding is one neither withdrawn nor superseded. What stands
  of the superseded entry: the class, its path and schema, that counts and the
  derivable method leave, and that a finding reaches a record only through a candidate.
- not taken: a Corrections section of dated lines, which amends a finding from outside
  its entry and leaves the sweep reading text the review did not confirm; a Promotion
  section, whose authored remainder, remarks about the pipeline, has no reader in any
  row; a `checked` line on findings the review confirmed, when the review's own
  instruction is that nothing enters as a finding unchecked.

### A finding is a conclusion over the verified layer, and the review writes findings too

- id: d-2026-09-09-15
- date: 2026-09-09
- raised by: Brian: "findings are subjective conclusions. The only thing treated as
  ground truth is the items and the calls that did some analysis on the items and gave
  structured output. So findings can still be part of candidates once checked against
  the structured output or items?"; then "Prose is not allowed to enter the findings.md
  file in the first place until already checked. I'm not writing to the file by hand,
  it's all emergent from a HITL session. The instructions for that HITL activity have to
  say that anything that comes up in it must be checked before entering the file at all
  as a finding." The session had drawn the line at pre-registration, treating an
  observation the data raised as post-hoc; pre-registration is a property of the
  measurement, which is frozen under every finding alike.
- decision: The verified layer is the items and the results, never edited; a result found
  wrong for its item is a shortcoming of the directions and a new version, never a
  correction. A finding is a conclusion drawn over that layer by a session, usable when it
  has been drawn from or checked against the results and the items. Two processes write
  findings: the analysis in verifying-a-corpus, from the tally and the results, and
  reviewing-findings, which writes a finding only after the session has checked it
  against the results and the items in that sitting, whether it is a superseding entry
  for one that held amended or a new one for what Brian raised; Brian's recall enters as
  the check, never as the finding. Nothing else writes a finding. The sweep's input is
  every standing finding, whoever wrote it; nothing else in the file feeds a candidate.
- not taken: the sweep skipping any finding with a correction, which treats a checked
  amendment as a disqualification; a new calibration after a check, when nothing was
  re-measured; findings born only in the analysis, which leaves what Brian sees at the
  review with no way in except a later study.

### A finding answers at most one frozen question; everything else becomes a question

- id: d-2026-09-09-16
- date: 2026-09-09
- raised by: Brian: "The findings-schema has a finding tied to a question. I don't think
  the relationship or cardinality was ever deliberated. What are the options and their
  consequences?"; then "So we should have finding as one to one with question, and then
  other free text fields to cover other things, and these are not findings? ... All other
  data can be used as input for making new questions, feeding subsequent studies?" A
  directions version names in its frontmatter the questions it freezes; the first draft
  required one question per finding by habit.
- decision: A finding names at most one question, and only one the study's directions
  version froze, in its frontmatter; a question is answered when a standing finding names
  it, derived, and stays frozen and unanswered otherwise; a finding may name none, being
  one the data raised, and is a candidate's input all the same once it stands. A finding
  bearing on two frozen questions is two findings. Proposed questions, shortcomings and
  withdrawn findings feed the corpus's question list through reviewing-findings and the
  next study, never a candidate.
- not taken: exactly one question per finding, which leaves an observation the data
  raised with no home but a later batch; one or more, which reads one entry as two
  answers; findings grouped under question headings, which the Shape grammar does not
  nest and every grep would slice.

### reviewing-findings is an activity, between verifying a corpus and the sweep

- id: d-2026-09-09-17
- date: 2026-09-09
- raised by: Brian: "Just like how there is reviewing-leads, does there need to be a
  reviewing-findings activity?", then on the chain, "HITL reviewing-findings where I
  challenge things and it checks evidence, and also can add questions for future
  studies", and on the frame, "Yes, this is good". The founding ruling "Post-WU review
  is not uniform" made promotion the verification's review when the artifact held
  counts; promotion reads candidates, ad hoc per hypothesis, after the sweep and the
  referee, so a null finding, a finding answering a question with no hypotheses, or a
  wrong finding was never read by Brian; the 2026-09-05 self-check's "the question it
  raises is Brian's, in the promotion session" was never wired into promote's reads;
  and d-2026-09-08-14's comparison of verifications by the tally had no row.
- decision: `reviewing-findings` is an hitl activity, enabled by verifying-a-corpus and
  enabling the sweep and preparing-to-verify-a-corpus, with one row, `review-findings`,
  reading findings, tally, results, index, corpus and the hypothesis statements and
  writing findings and the question list. Brian reads the findings; a finding he doubts
  is checked by the session against the results and the items and withdrawn or
  superseded per d-2026-09-09-14; a result he doubts is checked at the item and, if
  wrong for it, written as a shortcoming, never corrected; what he raises is checked
  the same way before it is written as a finding, per d-2026-09-09-15; two
  verifications sharing directions are read against each other by their tallies; the
  questions he raises go into the corpus's list in his words with the finding's token
  as what raised them; a finding that shows a hypothesis is missing is handed to
  minting-a-hypothesis in the same session; one commit. The founding ruling stands
  refined: exploration's review is reviewing-leads, verification's is reviewing-findings
  for its findings and promotion for its candidates.
- not taken: one reviewing activity over either artifact, whose two procedures differ
  where it matters, the source being the corpus for a lead and the batch files for a
  finding, and a leads comparison never counted; promotion grown to read the findings,
  which reaches a finding only after it is a candidate and only when a hypothesis
  prompts a session; nothing, the current state, in which a null finding is never read;
  a targets line appended at the review, which Brian ruled out, the sweep being the
  session's.

### verifying-a-corpus is assemble, assess and write-findings; the host writes the tally at completion

- id: d-2026-09-09-18
- date: 2026-09-09
- raised by: Brian: "Then I'm not seeing what verification-run is. Scrap the current
  verifying-a-corpus.md and start over with how many processes should be here and what
  they should be called"; "These processes are meant to be instructions to claude code
  on how to do this activity. What will the claude code auto session actually be
  calling? ... the cmd line that launches the batch is part of the agent process - it's
  what the session does to start it. Also, doesn't it do the tally as part of the C#
  code at the end?"; "Run and Judge do not seem like good terms"; "Execute is too
  generic for whatever work comes before the calls. Also can it be plural items?";
  "Good. I like the assemble and then assess prefixes." On the tally's options: "Do
  --flag and --group-by become tools that the later process putting together findings
  uses? Or are they overfit from wherever the tallier came from and should just be
  deleted?" d-2026-09-08-10, whose ruling stands under d-2026-09-09-1 and cannot be
  superseded again, made `tally-batch` a verb the session runs after the calls, with a
  flag list and a grouping written into the frozen file, both carried from the audit's
  script of 2026-09-03; `execute-batch` posts to the host and returns, and nothing in
  the host wrote the tally. This entry changes what that verb does and leaves the rest.
- decision: verifying-a-corpus has three processes. `assemble-full-batch`, session, the
  itemizer and the runner as its instruments: the definition written with kind full, the
  itemizer run into the batch, dry-run-batch, then execute-batch as the hand-off; it
  writes definition, index and items, and, by the runner it invoked, calls and tally.
  `assess-items`, agent: one call per item, the directions as its system prompt, the item
  as its message, the answer in the declared fields. `write-findings`, session: the
  analysis into findings.md. The host writes tally.md, the fixed sections only, counts
  per enum field, malformed, missing and not counted, when the last item has a
  successful call, and never again; `tally-batch` writes it only when it is absent and
  otherwise prints; `--group-by <column>` prints a cross-tab of classes by an index
  column and writes nothing, the analysis's and the review's tool; `--flag` is deleted,
  being a grep the runner skill's Queries table already gives. What stands of
  d-2026-09-08-10: the verbs dry-run-batch, execute-batch, tally-batch, start and stop,
  each batch verb taking a definition's path; the tally's parser as the reader's
  inverse; no generator, no define, no split; the Markdown itemizer as its own tool;
  that the runner reads no calibration. Preparing-to-verify's rows mirror these as
  `assemble-sample-batch` and `assess-sample-items`, its calibration comparing Brian's
  blind scoring against a tally that now exists without a step.
- not taken: four processes with the tally its own row, an act the session can forget
  and whose sections vary by what was typed; the host tallying with flags from a line
  in the definition, a reading choice in a frozen authored file; the hand-off folded
  into the agent row, which makes an agent row write calls and tally; `--flag` kept as
  a view, when it is one grep; the `verification-` prefix, `run` and `judge`, the first
  redundant where the kind and the act already locate the row, the others a session's
  words, `run` abolished by d-2026-09-08-16 and left in six ids by its sweep.

### refereeing-candidates is one activity: the sweep, the referee's batch and the verdicts

- id: d-2026-09-09-19
- date: 2026-09-09
- raised by: Brian: "I intended for candidates to be a massive flood of salience
  judgements that goes wide. It is a lot of findings and a lot of hypotheses, like an
  N x M. I made corrections for the findings, but I'm not going to connect hypotheses
  one by one. I'll be back for the judgements on the candidates that pass rigor,
  because then the whole context is there, like the 'would differ if false' data. So
  should writing-candidates-from-findings and refereeing-a-candidate be one activity?";
  on the name, "Refereeing-findings undersells what is actually happening"; on the
  rows, "Looks good". The founding record split writing-candidates-from-verification
  from refereeing-a-candidate on 2026-09-05, when the referee was prepared as its own
  instance; since d-2026-09-08-5 the referee is one set of directions for the method
  and its batches sit under the verifications they judge, so no decision of Brian's
  sits on the seam, and the stretch from the review to promotion is one autonomous
  run of session and call rows.
- decision: `refereeing-candidates` is one activity, enabled by reviewing-findings and
  by iterating-a-statement, enabling promoting, with four processes.
  `write-candidates`, session, reads findings, the question list, the hypothesis
  statements and index and writes candidates: one candidate per standing finding and
  per hypothesis the session claims it bears on, wide, the finding cited by its token,
  no falsifier and no verdict. `assemble-referee-batch`, session, the runner as its
  instrument: for each candidate with no referee line, one item holding the target's
  current statement and the finding's text materialised from findings.md and nothing
  else, into a batch under the verification whose definition names the referee's
  directions and calibration; dry-run-batch; execute-batch as the hand-off; it writes
  definition, index and items and, by the runner, calls and tally.
  `assess-referee-items`, agent: one blind call per item writing a falsifier and
  classifying diagnostic supporting, diagnostic challenging or non-diagnostic.
  `append-verdicts`, session: each well-formed result's two lines copied under its
  candidate, a malformed one left for a later execution. A session entering from
  iterating-a-statement finds no standing finding without candidates and runs the
  last three over the re-queued ones. The candidate carries the finding's token and
  never its text. writing-candidates-from-verification and refereeing-a-candidate
  retire.
- not taken: two activities as they stood, the sweep renamed
  writing-candidates-from-findings, a seam no decision sits on; the sweep folded into
  reviewing-findings, which Brian ruled out; the sweep as a batch of calls claiming
  hypotheses, salience in a blind call with the hypothesis set as its item, judged
  twice; `refereeing-findings` as the name, which undersells the sweep; the finding's
  text copied onto the candidate, a second home for one text.

### Process ids name the act and the batch's kind: assemble, assess, explore, write

- id: d-2026-09-09-20
- date: 2026-09-09
- raised by: Brian, on the ids left by d-2026-09-08-16's sweep and by the founding
  rewrite: "Run and Judge do not seem like good terms"; "where did 'judge' come from?
  What are the alternatives?"; "I don't think there needs to be a 'verification' prefix";
  "I just want it to be more representative of what that step actually does"; on the
  sweep, "The only one that looks weak is read-items. The rest are good", then on
  `explore-items`, "Looks good". `run` was abolished by d-2026-09-08-16 and its sweep
  left it in six process ids; `judge` entered on 2026-09-05 from a session's
  LLM-as-judge framing, questioned by Brian and never endorsed; `join` named the
  retired arms. Ids are unique across the folder, and the new names are unique because
  the act or the batch's kind differs per row.
- decision: A row that invokes the runner is `assemble-<kind>-batch`: the definition
  written, the items cut or built, dry-run-batch, and execute-batch as the hand-off; a
  call row is `<act>-<kind>-items`, the act `assess` where the call applies frozen
  criteria to an item and `explore` where it reads an item for leads; an analysis row
  is `write-<artifact>`. So: in preparing-to-verify-a-corpus `assemble-sample-batch`
  and `assess-sample-items`; in verifying-a-corpus `assemble-full-batch`, `assess-items`
  and `write-findings`; in preparing-to-explore-a-corpus `assemble-exploration-batch`,
  whose hand-off names the pilot's one item, and `explore-pilot-item`; in
  exploring-a-corpus `continue-exploration-batch`, whose whole act is execute-batch over
  the items the pilot left once Brian has read the pilot's result, `explore-items` and
  `write-leads`; in revising-the-method `assemble-audit-batch` and `assess-audit-items`;
  in refereeing-candidates the rows of d-2026-09-09-19. `run`, `judge` and `join`
  leave the folder's ids; the founding record keeps them.
- not taken: renaming only the `-run` ids, which removes the abolished word and keeps
  the session's; leaving the ids until something cites them, which the first study's
  state render would; `read-items`, which dropped the kind and named an act the call
  does not do plainly; `read-exploration-items`, the pattern without the act; `assess`
  for an exploring call, which applies no criteria; `execute` in a row id, the runner's
  verb kept for the runner.

### promoting-checked-candidates is promoting-refereed-candidates

- id: d-2026-09-09-21
- date: 2026-09-09
- raised by: Brian: "Maybe 'checked' is colliding with the checker program, so it should
  become promoting-refereed-candidates?", then "Yes, this rename is good,
  promoting-refereed-candidates". The id was a session's of 2026-09-03, when checked
  meant refereed; since d-2026-09-07-9 `check` is the tool's word, one thing the tool
  holds, and a checked candidate reads as one the hook passed.
- decision: The activity is `promoting-refereed-candidates`: the file, the Router row
  and its description, baselining-a-hypothesis's enables line. Refereed is the state
  its precondition names, a candidate carrying a referee line. `check` and `checked`
  belong to the tool. The founding entry keeps the old id.
- not taken: keeping the id and rewording the description, which leaves the collision
  where every session reads first; `promoting-candidates`, which drops the precondition
  from the name.

### The exploration's artifact is the class leads

- id: d-2026-09-09-22
- date: 2026-09-09
- raised by: the findings class of d-2026-09-09-13 beside `leads-artifact`, its mirror
  under d-2026-09-07-29, one carrying a suffix the other does not; Brian: "Yes, this
  rename is fine". The suffix was a session's of 2026-09-05, when artifact named a
  work unit's output document; since 2026-09-07 the artifacts are the governed files
  as a whole, and the class's checks were already `leads.title` and `leads.sections`.
- decision: The class is `leads`, its schema `schemas/leads-schema.md`, its path
  `studies/<study>/leads.md` and its checks unchanged. The rows that name it, the
  map's constant and the tests follow in the same pass as d-2026-09-09-20's sweep.
  d-2026-09-07-34 keeps the old id as a record.
- not taken: leaving the asymmetry; `findings-artifact` for the mirror, a suffix the
  vocabulary has emptied.

### reviewing-findings enables only refereeing-candidates; an enables edge is a forward handoff, not a re-entry

- id: d-2026-09-10-1
- date: 2026-09-10
- supersedes: d-2026-09-09-17
- raised by: Brian, on the run's writes: "I agree that there shouldn't be a loop in the
  DAG. Why was it suggested to add that dependency? What do the edges in the DAG really
  mean?" d-2026-09-09-17 gave reviewing-findings two enabled activities,
  refereeing-candidates and preparing-to-verify-a-corpus; the second closes a cycle,
  preparing-to-verify → verifying-a-corpus → reviewing-findings → preparing-to-verify,
  which the checker's DAG rule refused, so the edge never shipped and the entry alone
  carried it.
- decision: An enables edge X → Y means a process of X writes an artifact a process of Y
  reads to carry one line of work a step further toward the terminus; enables.unbacked
  guards the floor, an edge with no data flow is an error, and the acyclic-DAG rule the
  ceiling, the pipeline produces forward toward one terminus. A data flow that would
  close a cycle is a re-entry, the same corpus going round again as a new study, carried
  by the standing, append question list and described in prose, never an edge; otherwise
  every question-writing activity, ask, promote and the plan rows, would enable
  preparing, and none does. reviewing-findings enables refereeing-candidates alone. What
  stands of d-2026-09-09-17: the activity, its row review-findings, its reads, its writes
  and its procedure; only the preparing edge is dropped, its work now the prose that a
  shortcoming sends the study back through preparing, the loop the exploration side
  already runs without an edge.
- not taken: the DAG rule yielding for re-entry edges, which makes the pipeline topology
  unreadable and every shared-artifact write a candidate edge; leaving -17's clause
  standing against a skill that never had the edge.

### DocIntegrity governs authored files, not generated ones; the checker dispatch is the line, not WellKnown membership

- id: d-2026-09-10-2
- date: 2026-09-10
- raised by: Brian, on whether appending the referee's verdicts is a session step: "This
  should not be a session step because there is no reasoning involved. That would be a
  waste of tokens. It can be done by a C# program. If the shape is known and static
  because it's produced by a tool or structured json, a hook is not needed at all"; and
  that "the referee outputs are all programmatic. The put together final doc of candidates
  that passed refereeing and could be promoted can be assembled programmatically for
  review."
- decision: The hook governs authored files — those a session or Brian composes by
  judgment, where a composer can get the shape wrong: the findings, the hypothesis
  records, the registry, the question lists, the leads, declined-candidates, and the
  directions, index and definition a session writes. A file a deterministic program emits
  from already-validated inputs has its shape guaranteed by that program, its tests and
  the shared BatchFiles library, so it carries no checker and the hook rejects nothing in
  it: the batch results, the tally, and the composed candidates view. The line between the
  two is membership in the checker dispatch — the ids that dispatch to a checker and sit
  in CheckedIds — never membership in the WellKnown registry, which names generated
  artifacts too (results, tally, calls, candidates) so the map and the state render can
  cite one without checking it. This is the rule d-2026-09-09-11 held for results, stated
  once for every artifact.
- not taken: a checker over a generated file, which re-validates a shape that cannot vary
  and drifts from the single generator that already holds it; WellKnown membership as the
  governed line, which would pull results, tally and calls wrongly into checking; a
  session performing a mechanical, no-reasoning assembly, a waste of tokens for a transform
  a program does deterministically.

### The funnel is findings, claims, candidates, evidence; a claim is unverified, a candidate has passed the referee

- id: d-2026-09-10-3
- date: 2026-09-10
- raised by: Brian, on the shape of the sweep-to-promotion stretch: "I like the idea of
  the NxM judgement creating "claims" (means unverified), and only after the referee they
  become "candidates" (eligible to become evidence)"; and, on what the stretch is for,
  that "Creating candidates, refereeing them, and putting them together all serves a
  purpose of presenting promotion options to me".
- decision: The evidence pipeline is a funnel from a verification's findings to a
  hypothesis's evidence. A verification's standing findings feed claiming, which produces
  claims; the referee turns claims into candidates; promotion turns candidates into
  evidence. A claim is a (finding, hypothesis) assertion that the finding bears on the
  hypothesis, unverified, made wide across findings and hypotheses. A candidate is a claim
  the referee judged diagnostic, supporting or challenging, and so eligible to become
  evidence; a claim judged non-diagnostic is refereed but is not a candidate. The two ends
  are the governed authored records — findings.md, what a session concluded, and the
  hypothesis records, what Brian accepted; everything between them, the claims, the
  referee's verdicts and the candidates view, is machinery whose only purpose is to present
  promotion options to Brian. The wide sweep d-2026-09-09-19 named "candidates" is
  claiming, and its output is claims.
- not taken: the single word "candidate" for both the unverified assertion and the
  refereed one, which hid the boundary the referee draws and which Brian's split names;
  "candidate" for a non-diagnostic claim, which would call a thing ineligible for evidence
  a candidate.

### One finding text, in findings.md; the referee adds a falsifier; the evidence entry is a frozen snapshot; the candidates view materialises

- id: d-2026-09-10-4
- date: 2026-09-10
- raised by: Brian, that the referee adds a text of its own — "But doesn't the referee
  create an additional "would differ if false" line?" — and that the generated view must
  carry the finding's prose — "If candidates.md is generated by C#, it needs to copy the
  text of the finding, not just the token, since I'll be reading that file in the
  promotion session".
- decision: The finding text has one home, the finding in findings.md; d-2026-09-09-19
  ruled the candidate carries the finding's token and never its text. From that single
  text: the referee's item is the target's current statement and the finding materialised
  from findings.md and nothing else, and the referee authors a distinct text of its own,
  the falsifier — what the finding would have been were the statement false — one per
  candidate. On promotion the evidence entry takes a frozen, verbatim snapshot of the
  finding and the falsifier, so a later supersession of the finding cannot alter what was
  promoted. The candidates view, being generated, materialises the finding text inline,
  with the referee's verdict and falsifier and the promotion status, so the promotion
  session reads prose and not tokens; this is not a second authored home but a projection
  re-derived from findings.md on each compose, which cannot drift. The one text is
  authored once, referenced by the candidate as a token, materialised for the referee and
  the view, and frozen once at the endpoint. This does not settle the re-queued candidate
  of d-2026-09-08-18, phrased "the finding verbatim" before the token rule; the re-queue
  is decided with the first study.
- not taken: copying the finding text onto the candidate, the second home d-2026-09-09-19
  refused; the evidence entry holding a token rather than a snapshot, which would let a
  later change to findings.md rewrite a promoted record; the candidates view showing bare
  tokens, unreadable in promotion; treating the view's materialised text as an authored
  copy that could drift, when it is re-derived on each compose.

### surfacing-candidates: one autonomous activity — claim, referee, compose — with compose in the checker's tool, not the runner

- id: d-2026-09-10-5
- date: 2026-09-10
- supersedes: d-2026-09-09-19
- raised by: Brian: "It's one activity with multiple processes, because it's kicked off
  once and not HITL. That one claude code session is following the directions of the
  activity file and will call the C# steps"; on where that C# runs, "Does the code go in
  the agent runner exe or not?"; on the name, "Building, as it's used in building a tool,
  implies HITL. Let's use surfacing-candidates"; on the rows, "The process rows are good."
- decision: The activity d-2026-09-09-19 called `refereeing-candidates` is
  `surfacing-candidates`: one autonomous activity, one session orchestrating, kicked off
  once after reviewing-findings, enabled also by iterating-a-statement, enabling
  promoting-refereed-candidates. Its rows are `assemble-claim-batch`, the session reading a
  verification's standing findings from findings.md into a claiming batch whose directions
  carry the hypothesis set, executed through the runner; `assess-claim-items`, one call per
  finding naming the hypotheses it bears on; `assemble-referee-batch`, one referee item per
  claim holding the target's current statement and the finding materialised from
  findings.md and nothing else, executed through the runner; `assess-referee-items`, one
  blind call per item writing a falsifier and classifying diagnostic supporting, diagnostic
  challenging or non-diagnostic; and `compose-candidates`, which joins the claiming
  results, findings.md, the referee results, declined-candidates.md and the hypothesis
  records into the candidates view. compose-candidates is study-scoped — it spans two
  batches and reads study-level files — so it runs in DocIntegrity, the tool that holds the
  checker, the map and the shared batch-file reader, never in the runner, which by
  d-2026-09-08-4 knows no study. What stands of d-2026-09-09-19: one autonomous activity
  from the review to promotion; the referee's item is the statement and the finding
  materialised from findings.md; the referee's call is blind and classifies diagnostic
  supporting, diagnostic challenging or non-diagnostic; the candidate carries the finding's
  token and never its text; Brian judges only the survivors, at promotion;
  writing-candidates-from-verification and refereeing-a-candidate stay retired. The sweep's
  output is claims (d-2026-09-10-3); the reviewing-findings enables edge of d-2026-09-10-1
  now names surfacing-candidates. append-verdicts retires: the referee's verdicts are the
  referee results, materialised by compose-candidates, never appended to an authored file.
  The re-entry from iterating-a-statement over re-queued claims is settled with the first
  study.
- not taken: append-verdicts as a session writing verdicts into candidates.md, no
  reasoning and a waste of tokens (d-2026-09-10-2), and a hand-edit of a generated file;
  compose in the runner, which d-2026-09-08-4 keeps study-agnostic; the names
  refereeing-candidates, which hides the claiming that produces the claims, and
  building-candidates, whose "build" reads as the human act of building-a-tool; claiming
  and refereeing as two activities, when one autonomous kick-off runs the whole stretch.

### Claiming is a per-finding runner batch, hypotheses in the directions, model a parameter, relevance only

- id: d-2026-09-10-6
- date: 2026-09-10
- raised by: Brian, weighing a holistic sweep against a batch — "One strong model that
  reads all N+M and reasons which pertain to which... seems more efficient? Or are there
  theoretical downsides?" — then "Per finding runner batch makes sense, but how do I
  actually get the cache hits?"; "just don't specify a model, let that be parameterizable
  for this process"; "Relevance only"; and, first-passing the output now, "Yes, bare list".
- decision: `assess-claim-items` is a per-finding runner batch: one call per standing
  finding, the hypothesis set carried in the batch's directions so it is the cached prefix
  across the calls, the finding as the call's message. The batch names no model; the model
  is the definition's parameter, as for any batch. A claiming call asserts only relevance —
  which hypotheses the finding bears on — never a direction; supporting or challenging is
  the referee's alone, in d-2026-09-10-5's assess-referee-items. Claims are the claiming
  batch's own results on disk, not a separate authored artifact; a candidate is a
  (finding-token, target) drawn from them. A claiming call's output, first pass, is a bare
  list of the hypothesis file names the finding bears on — each a real hypothesis, the
  list possibly empty — relevance only and no reason, since nothing downstream reads one
  and the referee is blind; refined against real findings (d-2026-09-07-30). Calibration
  stays the first study's: it tunes directions against expected results, which needs real
  findings.
- not taken: a single holistic call over all findings and all hypotheses, cheaper on
  tokens but correlated in its errors and recall-suppressing where the funnel needs recall,
  and a poor fit to re-queue and to the runner's per-item provenance; the per-pair batch,
  N x M calls re-sending every finding and hypothesis, quadratic and defeating the cache; a
  model fixed in the process, when the model is a batch parameter; a claim carrying a
  supporting or challenging lean, which either biases the blind referee (d-2026-09-10-5) or
  is discarded.

### candidates.md is a generated, regenerable view; it materialises finding, verdict and status, and is regenerated at promotion

- id: d-2026-09-10-7
- date: 2026-09-10
- supersedes: d-2026-09-07-32
- raised by: Brian: "I agree that candidates.md should not be edited and should be fully
  regenerable"; "The regenerated candidates.md which pulls from hypotheses and declined
  candidates can have the full finding text in it"; choosing the status-annotated form,
  "Go with option 3"; and, on the layout, "Yes, option 1. The candidates should be one
  section that starts the document. The non diagnostic ones are at the bottom".
- decision: candidates.md is generated by compose-candidates, read-only and fully
  regenerable, never hand-edited, and not governed (d-2026-09-10-2). Each entry is a
  candidate, a (finding-token, target): the finding-token the finding's own
  `### <study>/<slug>` in findings.md, the target the hypothesis's file name `NNN-<slug>` —
  the candidate has no slug of its own, the refinement d-2026-09-07-30 left to first use.
  The view materialises, per candidate, the finding text from findings.md, the referee's
  verdict and falsifier from the referee results, and the promotion status; status is
  derived, never stored here — promoted where a hypothesis record cites the candidate,
  declined with its reason from declined-candidates.md, else pending. It is composed twice:
  at the end of surfacing-candidates, every candidate pending, and again in promotion, to
  show the outcomes. Its first-pass shape: the diagnostic candidates in one section that
  opens the document, each showing its identity, the materialised finding, the referee's
  verdict and falsifier, and its status; the non-diagnostic claims in a labeled section at
  the foot, context the referee set aside, not promotion options and not in the promoted,
  declined and pending counts (d-2026-09-10-9); the shape refined against the first real
  candidates (d-2026-09-07-30), never withheld. What stands of d-2026-09-07-32: a
  candidates file is one per study, its shape otherwise the compose step's output and not
  an authored grammar a checker holds; the iteration-candidates class left with
  d-2026-09-08-18.
- not taken: editing candidates.md in place with outcomes, which forfeits regenerability
  and mixes generated with authored (d-2026-09-10-2); bare tokens, unreadable in promotion
  (d-2026-09-10-4); storing the promoted status on the file rather than deriving it from
  the hypothesis records, a second home for an outcome; a status view separate from
  candidates.md, when one regenerated file carries the options and their fate; omitting the
  non-diagnostic claims, which hides what the referee set aside.

### Promotion splits outcomes: promoted to the hypothesis record, declined to declined-candidates.md, a new governed class

- id: d-2026-09-10-8
- date: 2026-09-10
- supersedes: d-2026-09-07-33
- raised by: Brian: "Decline reasons should not go to the hypothesis file"; "there must be
  a way to see which candidates were considered and not promoted"; "Go with option 3, and
  use declined-candidates.md to be the most clear. Declines only in this file, and promoted
  to evidence is found through hypotheses"; and, on the fields, "the declined candidates
  just needs finding token, hypothesis slug, reason and date", with the target as the
  "Full stem".
- decision: Promotion's outcomes go to two governed authored records, never onto the
  generated view. A promoted candidate becomes an evidence entry in the hypothesis record,
  a frozen snapshot (d-2026-09-10-4); that it was promoted is read from the citation there,
  with no second store. A declined candidate is recorded in `declined-candidates.md`, a new
  governed class, one per study at docs/v3-framework/studies/<study>/declined-candidates.md:
  declines only, one entry per declined candidate carrying the finding-token, the target's
  file name, Brian's reason and the date, as block entries. Reasons never go to the
  hypothesis record. What stands of d-2026-09-07-33: the candidates file is under its study
  in the docs tree, now docs/v3-framework/studies/<study>/ (d-2026-09-08-7), and where a
  rewording's candidates live stays decided with the rewording; what is overturned — the
  file is generated, not authored (d-2026-09-10-7), the referee's lines are materialised,
  not written back, and Brian's outcomes are the two records above, not lines he reads off
  that file.
- not taken: decline reasons on the hypothesis record, which Brian ruled out and which
  would mix a rejected candidate into the record of accepted evidence; a single outcomes
  record holding promoted and declined, duplicating the promotion the hypothesis citation
  already carries; deriving declined as considered-minus-promoted with no reason, which
  drops the reason Brian keeps; recording the referee's directions on a decline, when the
  candidate's referee provenance is already in the view.

### The candidate cluster's surface: candidate-schema deleted, candidates kept ungoverned, declined-candidates added governed

- id: d-2026-09-10-9
- date: 2026-09-10
- raised by: Brian: "I agree that declined-candidates.md is a new governed authored class
  and candidates-schema.md is deleted. What is WellKnown.Candidates?"; and, pressing to
  first-pass the schema now rather than defer it, "Why are we discussing how to make
  special cases? Why can't we decide the schema first pass now?" — then choosing the
  composite `### <finding-slug> → <target>` heading and the four checks.
- decision: The candidate cluster's surface in the checker. candidate-schema.md is
  deleted; a candidates class is no longer checked, so the file would be a schema no row
  links to, which d-2026-09-07-7 fails, and its content is now the surfacing-candidates
  and promoting-refereed-candidates process text. `WellKnown.Candidates` stays, the id of
  the generated view, kept like `Tally` and `Results` so the map and the state render can
  name the file, but in no checker dispatch and no CheckedIds (d-2026-09-10-2);
  StateBuilder's CandidateCounts, which counts the retired referee and outcome append
  lines, is rewritten to the promoted, declined and pending counts read from
  declined-candidates.md and the hypothesis records. declined-candidates is governed
  (d-2026-09-10-8): declined-candidates-schema.md is written now as a first pass, a
  per-study class whose entries are `### <finding-slug> → <target>` with a `reason`
  block and a `date`, held by four checks — `declined-candidates.title`,
  `declined-candidates.shape`, `declined-candidates.entry`, and
  `declined-candidates.references`, the last resolving that the finding-slug names a
  standing finding in the study's findings.md and the target a real hypothesis; the
  class takes `WellKnown.DeclinedCandidates`, a map row linking its schema, and a checker
  in the dispatch and CheckedIds. Its fields are reviewed against the first real declines
  (d-2026-09-07-30), never withheld until then. Claims take no id: they are the claiming
  batch's results.
- not taken: keeping candidate-schema.md as a spec of the generated view, which
  d-2026-09-07-7 fails as a schema no row links to and which the process file already
  documents; removing WellKnown.Candidates, which the state render needs to name the file;
  a checker over the generated candidates.md, which d-2026-09-10-2 leaves ungoverned;
  deferring the declined-candidates schema to the first promotion, when its fields are
  already decided (d-2026-09-10-8) and a first pass reviewed at first use is the pattern
  candidate-schema.md itself set.

### Iteration is challenge-triggered and is how a challenge is resolved; it never sharpens a supported hypothesis

- id: d-2026-09-11-1
- date: 2026-09-11
- raised by: Brian: "Iteration is only going to happen after evidence comes in that
  challenges it. So the hypothesis is already no longer supported at that point";
  "Confirmed not about sharpening an evidenced unchallenged hypothesis"; and, on its
  purpose, "I had a hypothesis that had supporting evidence. Then new evidence came in
  that challenges it. Now I have to change the content of the hypothesis so that both old
  and new are in support. By virtue of challenging, the hypothesis has to CHANGE MEANING …
  my adherence to rigor says I am not allowed to baseline a hypothesis that has
  challenging evidence."
- decision: iterating-a-statement is triggered only by a challenge, or by a merge or
  split; it never rewords a hypothesis that is evidenced and unchallenged — a sharper
  claim on supporting evidence is a new hypothesis (minting-a-hypothesis) or nothing,
  never an iteration. Because a challenging entry lands before the reword, the hypothesis
  is already `challenged`, not `evidenced`, when iteration begins: the reword loses no
  supported state. Iteration is the mechanism by which a `challenged` hypothesis returns
  to `evidenced` — the meaning is changed so the challenging evidence and the prior
  supporting evidence all support the new wording, then re-verified against it; there is
  no other way to resolve a challenging entry, and how a challenge is cleared, left open
  until now, is answered here. A challenge Brian judges spurious is declined at promotion
  and never becomes an entry, so every challenging entry is one he accepted, and only a
  reword clears it.
- not taken: iteration as a general reword for precision, which would drop an evidenced
  hypothesis to untested for a claim that is either independent (mint) or already
  supported (leave it); a separate resolve-a-challenge marker or entry kind, when the
  reword is the resolution; treating the transient untested state after a reword as a
  regression, when the challenge had already unsettled the hypothesis.

### iterating-a-statement is a gated re-verification of the hypothesis's own evidence against a proposed wording

- id: d-2026-09-11-2
- date: 2026-09-11
- supersedes: d-2026-09-08-18
- raised by: Brian: "I want the surfacing candidates workflow when iterating a hypothesis.
  But the input set of claims is not from the studies. It is the set of evidence that is
  already on the hypothesis file at the time of the proposed rework"; and "The processes
  you listed are fine."
- decision: iterating-a-statement runs the referee machinery over the hypothesis's own
  evidence, gated. Its rows: `propose-wording`, hitl, Brian gives the new wording and
  nothing is written to the file; `assemble-reverify-batch`, session, the runner as its
  instrument, one referee item per current-wording evidence entry holding the proposed
  wording and that entry's finding text — the frozen snapshot d-2026-09-10-4 keeps on the
  entry, so the evidence is read off the hypothesis file, never resolved through the
  studies; `assess-reverify-items`, agent, one blind call per item under the method's
  referee directions writing the falsifier and classifying diagnostic supporting,
  diagnostic challenging or non-diagnostic; `gate-and-commit`, hitl, the gate of
  d-2026-09-11-3 and, on a pass, the write. The referee is the method's one referee
  (d-2026-09-08-5), judging the proposed statement it is handed rather than one read from
  a file. What stands of d-2026-09-08-18: a rewording is triggered by evidence and
  concerns the hypothesis's prior evidence, and the re-verification is no study; what is
  overturned — nothing is re-queued to any study's candidates file, iterate writes no
  candidate, and re-verification is the referee over the file's own evidence against the
  proposed wording, its batch under `iterations/` (d-2026-09-11-4). d-2026-09-10-4
  deferred this to the first study; it is decided here instead.
- not taken: re-queuing findings as candidates into the studies that produced them
  (d-2026-09-08-18), which the generated candidates.md (d-2026-09-10-7) no longer allows;
  refereeing against the committed statement, which would require writing the reword
  before it is verified; a fresh claim sweep over the studies' standing findings, when the
  input is exactly the evidence already on the file and no more.

### The reword is written only if every prior evidence finding comes out diagnostic-supporting of the new wording; on a pass all evidence is re-entered with new falsifiers

- id: d-2026-09-11-3
- date: 2026-09-11
- raised by: Brian: "Every piece of old evidence has to come out diagnostic and
  supporting of the new hypothesis in order to write that hypothesis into the file";
  "even the previously challenging finding must become actively supporting. I'm not doing
  this to self affirm my hypotheses; I'm doing this to be a better writer"; and "If the
  new wording passes, all the evidence gets rewritten as new entries with their new
  falsifiers. The old falsifiers are left as historical record, because the evolution of a
  hypothesis against the evidence is part of the epistemic data that can be analyzed. On
  failure, nothing is written."
- decision: The gate is that every current-wording evidence finding, the once-challenging
  one included, comes out diagnostic AND supporting of the proposed wording; a finding that
  comes out challenging or non-diagnostic fails it. All pass: the new wording is written to
  `## Hypothesis`, an iteration entry marks the boundary, and each finding is re-entered as
  a fresh evidence entry against the new wording carrying the new falsifier the re-referee
  wrote and citing the iteration's referee batch; status recomputes to evidenced and
  baselined resets. The prior evidence entries, with their old falsifiers, stay above the
  boundary as history, since the evolution of a hypothesis against its evidence is itself
  data a later study may analyse. Any finding fails: nothing is written — not the wording,
  not an entry — and the failing findings are reported; Brian proposes another wording or
  concludes the hypothesis must be split or abandoned. A non-diagnostic result fails
  because a wording that sheds a piece of the evidence is not the honest reconciliation of
  all of it: the bar is that the new meaning is supported by every finding, not merely
  un-contradicted.
- not taken: accepting a finding that turns non-diagnostic under the new wording, which
  lets a reword quietly drop evidence and self-affirm; re-binding the old entries to the
  new wording in place, which the record's positional binding to iteration boundaries does
  not allow and which would erase the falsifier history; writing the reword first and
  re-verifying after, the rubber-stamp the rigor forbids.

### The iteration batch mirrors a study's under iterations/, matched by a container placeholder and cited by the iteration slug

- id: d-2026-09-11-4
- date: 2026-09-11
- raised by: Brian: "Make these go under a folder called iterations which is a sibling to
  studies under v3-framework, and name the folders iteration-of-<hypothesis file
  name>-<number, the Nth iteration>"; then, on the structure, "it should be
  iterations/<slug>/… where slug would be something like iteration-of-<hypothesis>-1 …
  this should then be the same folder structure as studies so the consumers' lookup
  patterns are generalizable"; approving "option a with the container placeholder"; and
  "iterating allows baselining."
- decision: An iteration's re-verification is no study, but its folder mirrors a study's
  exactly: `docs/v3-framework/iterations/<slug>/`, a sibling of `studies/`, `<slug>` the
  folder name `iteration-of-<hypothesis-file-name>-<N>` with `<N>` the ordinal of that
  hypothesis's iterations, holding `batches/<batch>/` with the referee batch's definition
  (naming the method's referee directions and calibration by reference), index, items,
  calls, results and tally, as a verification's referee batch does. The batch-artifact path
  patterns gain a `<container>` placeholder — `docs/v3-framework/<container>/<study>/batches/<batch>/…` —
  matching a study folder or an iteration folder alike: an unknown placeholder is a
  one-segment wildcard in the engine, so the same checkers govern both with no code change,
  and only `studies/` and `iterations/` ever hold `…/batches/`. An iteration-sourced
  evidence entry cites the iteration slug in the study position,
  `(iteration-of-<hyp>-<N>/<finding-slug>; directions-N@<hash>)`, which the existing
  citation regex accepts unchanged, the position being the owner — study or iteration — the
  candidate's referee batch sat under. An iteration is not registered in `studies.md`; its
  existence and ordinal are read from the folder, which the state render scans.
  iterating-a-statement enables baselining-a-hypothesis — it writes the evidence directly on
  a gate pass — and no longer enables the sweep.
- not taken: a study type for the rewording, which d-2026-09-08-18 declined and this keeps
  declined; iterations under `studies/`, which the existing `<study>` pattern would match
  with no change but which conflates the trees and puts an unregistered folder among the
  studies; a precise `(studies|iterations)` owner matcher, tighter than `<container>` but a
  code change for a distinction only these two folders make; a new citation form for an
  iteration-sourced evidence entry, when the iteration slug fits the study position the
  existing regex already reads.

### A unit's entries are written as one batch at its end and scanned, not drafted one at a time for approval

- id: d-2026-09-11-5
- date: 2026-09-11
- supersedes: d-2026-09-08-20
- raised by: the hypothesis-file schema review, which produced a dozen rulings in one
  session and would, under the superseded entry, have put each drafted entry in front of
  Brian before the next could be taken. Brian: "I don't really care how many entries and
  where the lines fall. It is too tedious to review everything. The initial strictness did
  not anticipate the scale of the task. When the time comes, write all the decisions into
  the file and I'll scan it afterward."
- decision: A unit opens with a queue of starting points, one line each, ordered
  widest-first by the session and seen by Brian before any is taken; he reorders, removes
  or adds. Taking a starting point opens a stack with it at the bottom; deliberating any
  decision on the stack pushes the decisions it surfaces on top, and the pop is from the
  top, so the starting point is settled last and may name the decisions settled above it.
  A stack is worked to empty before the next starting point is taken, and after each stack
  empties the queue is presented again, re-ordered and re-formed; it is never presented as
  settled, and no remaining item is described as if it would land as written. What changes
  is when the text is written and what Brian reviews. A decision is ruled in session as
  before; its entry is drafted at the end of the unit, with every entry of that unit, in
  the order the decisions were settled, and Brian scans the file. He approves the ruling,
  never the drafted sentence. How many entries a unit's rulings become, and where the lines
  between them fall, is the session's judgment under the one-decision rule the schema
  states. An entry he corrects on the scan is corrected before the commit, which is the
  only moment an entry may be edited; from the commit the file's never-edited rule stands
  unchanged.
- not taken: writing each entry as it lands, the superseded rule, which a review of this
  scale made tedious without adding rigor, the ruling already being made by the time the
  text is drafted; batching the writes with no review at all, which leaves the record
  unchecked by the one person who can tell a ruling from a lapse; a summary of the unit in
  chat in place of the entries, which puts his review on a rendering instead of on the
  record; treating the relaxation as a one-off for this unit, which would leave a later
  session reading a dozen entries landed at once with no rule behind them.

### The record as it stands is the starting point; the six iteration entries are not migrated and their statements revert

- id: d-2026-09-11-6
- date: 2026-09-11
- raised by: Brian, on being shown that a migration of the fifty files was implied by the
  schema review: "In order to not have to migrate, forget the iteration entries as they
  are. Treat whatever the hypotheses are right now as the starting point. This is because
  that 'iteration' was done with what is retrospectively lead-tier insight, NOT
  evidence-tier insight." Then: "I do not want to migrate the iteration entries. Those can
  be lost to the void… The hypothesis statements are the only thing to migrate; there
  should be no other data"; "I do want the created entries"; and "the 6 hypotheses that
  were iterated on should enter the new shape with their *original* statements, since
  whatever iteration was on leads, not evidence, back then. They should not have changed
  their wording in the first place." The closed founding record had kept the six entries on
  the reasoning that removing them would leave each `created` entry describing a wording it
  did not describe; reverting the statements answers that reason rather than overriding it.
- decision: The fifty files enter the new shape carrying their statement, their founding
  reasoning and its date, and nothing else. The six iteration entries of 2026-09-01 and
  2026-09-03 are not carried, and the six statements they reworded revert to the wording
  that stood before them, recovered from git: 013, 029, 030, 033, 034 and 038. Five were
  reworded on corpus readings the founding record has since classed as leads; 013 was
  reworded on a design conversation whose construct, the work matrix, has itself retired,
  and its current wording uses two words the method has abolished. Neither leads nor a
  design conversation is evidence, and under d-2026-09-11-1 an iteration is challenge-
  triggered and gated, which none of the six was. The created entries are unchanged from
  before those rewordings, so each reverted statement is again the one its reasoning
  describes. 038's iteration also added a second claim; it is not carried, and if it is
  wanted it is a new hypothesis through minting.
- not taken: carrying the six entries into the new shape, which would migrate as history
  six rewordings the method would now refuse to make; keeping the reworded statements and
  dropping the entries, which leaves six founding reasonings describing wordings that no
  longer stand — the failure the founding record named; rewriting the six created entries
  to match the reworded statements, which edits Brian's record to fit a machine's tidiness.

### The record's entries are `###` entries, held by the engine

- id: d-2026-09-11-7
- date: 2026-09-11
- raised by: the conversion of `hypothesis-file-schema.md` to the four-section shape, which
  found the class's entry form expressible in neither of the grammar's two forms: a record
  entry is a `- ` bullet carrying a kind, a timestamp, a citation, a tag and continuations,
  while the grammar has `###` entries with a field table and untabled one-line entries and
  nothing between. The form was a session's of 2026-08-31, never ruled, and by 2026-09-11
  four programs parsed it by hand — the checker, the state render, the candidates compose
  and the surfacing itemizer — three of them written in the preceding two days, each
  re-deriving the form from the files because no declaration existed to read. Brian, given
  the options: "Go with option 1 then."
- decision: The Record holds `###` entries with a field table, the form the engine already
  parses, types and resolves; the class is held by its Shape and no longer by hand. One
  field table serves every kind, with a discriminator heading and the kind-specific fields
  optional, since the grammar admits one field table per entries section; which kind
  requires which field is the class's own rule, named in its Checks. The migration
  authorised by d-2026-09-11-6 is what makes the form a free choice, the fifty files being
  rewritten in any case.
- not taken: a new grammar word for a keyed-bullet entry array, which would extend the
  grammar for a single instance with no second consumer shown; a re-founded bullet form
  declared as untabled one-line entries, which leaves the class's whole internal grammar in
  four hand-written parsers and says nothing in the Shape — and which would also be untrue,
  the entries carrying continuations.

### The founding reasoning leaves the record: the file holds Hypothesis, Origin and Record

- id: d-2026-09-11-8
- date: 2026-09-11
- raised by: the record's four kinds proving to be one structured kind and three carrying a
  date and a prose blob, which asked whether they belong in one array. The founding record
  of 2026-09-05 answers it, in reasoning Brian raised — "I don't think the history of
  hypothesis file and the history of the other things have the same meaning and/or purpose"
  — and then asked be preserved: "This reasoning should carry on somewhere so that it is
  not lost in the future." That reasoning distinguishes provenance from evidence in as many
  words: remove the record and the file is not a hypothesis with less provenance, it is a
  hypothesis with no evidence. Brian, on the split: "Origin is fine."
- decision: The file is three sections. `## Hypothesis`, the statement. `## Origin`, the
  founding reasoning, written once at mint and never edited. `## Record`, the evidence
  relationship: evidence, iteration and baselined entries, in one ordered array. The
  founding reasoning is provenance, which the 2026-09-05 reasoning defines the record
  against; it stays inside the file, which is all the 2026-08-30 requirement asks, and
  `hypothesis.created-first` retires, singleton-ness becoming structural. Baselining stays
  in the Record: the same reasoning names the baseline as part of the relationship, and its
  binding to a wording is positional, which a separate section would break. The schema's
  opening carries the 2026-09-05 reasoning and the third clause of the 2026-08-30
  requirement, when to use the record and why it exists, neither of which any text has ever
  carried.
- not taken: all four kinds in one Record, which keeps a check whose whole job is to
  simulate a section and mixes provenance, evidence and judgment under a table whose only
  shared fields are a date and a prose blob; baselining moved to its own array, the purest
  reading of "the record is the evidence relationship", which breaks the positional binding
  the status and the iteration boundary depend on; splitting the sections into separate
  files, which the 2026-08-30 requirement refuses.

### The Artifacts rows become statement, origin and record; hypothesis-status leaves

- id: d-2026-09-11-9
- date: 2026-09-11
- raised by: d-2026-09-11-8's split and d-2026-09-11-12's cuts together changing the row
  set of the one file that has ever carried more than one. Brian, on the new id: "Row id
  hypothesis-origin is fine"; and, on who reads it, "I would be reading the whole hypothesis
  file before baselining."
- decision: The file's rows are `hypothesis-statement` at `§ Hypothesis`, in-place;
  `hypothesis-origin` at `§ Origin`, frozen; and `hypothesis-record` at `§ Record`, append.
  All three link `hypothesis-file-schema`, as the three rows did before. `hypothesis-status`
  is removed, nothing authoring status or baselined any longer. `mint` writes the origin and
  `baseline` reads it, which is the fact rather than a contrivance and keeps a frozen
  artifact from being read and written by one process; `baselining-a-hypothesis` says so in
  its procedure, the presentation becoming the file while the clause forbidding summary and
  recommendation stands. Every cell naming `hypothesis-status` — in `baseline`, `promote`,
  `gate-and-commit` and `mint` — is edited in the same write as the rows.
- not taken: keeping a `hypothesis-status` row for a value nothing authors, which the never-
  read and never-written checks would report from both ends; naming `mint` the reader, which
  would have one process read and write a frozen artifact; leaving the origin unread, which
  fails `artifact.never-read` and would be the validator reporting that the split is
  bookkeeping rather than structure.

### A section carrying its own mutation is its own Artifacts row; a Shape never names a mutation

- id: d-2026-09-11-10
- date: 2026-09-11
- raised by: two sentences in `schemas/skill-schema.md` that meet only in this file — "A
  Shape declares and never restates its row's path or mutation" and, in the grammar, "a file
  whose sections carry different mutations names each" — written two days apart and both
  before any multi-mutation file had a Shape. Rule 9 already settles it in the constitution:
  an artifact's mutation is what "its table row says". Brian: "Go with A."
- decision: A section carrying its own mutation is its own Artifacts row, which is what the
  three-rows-for-one-file pattern of 2026-09-04 already did; rule 9's requirement that a
  multi-section file name each mutation is then satisfied by the rows in every case, and the
  Shape stays silent as the other clause requires. The grammar's "names each" clause is
  deleted from `skill-schema.md` as a duplicate that was never exercised. Nothing in the
  engine reads a per-section mutation, and no checker can detect an edit to a frozen section,
  having no history to compare against.
- not taken: a `mutation` column on the sections table, which restates the row's mutation in
  a second place that nothing holds equal, and changes both the grammar and `schema.fields`
  for a fact no engine reads; a prose sentence in the schema file naming which section is
  which, the same second home with no checker over it.

### An entry is headed by its kind, and a kind's heading repeats

- id: d-2026-09-11-11
- date: 2026-09-11
- raised by: the `###` form needing a heading type, and the choice between the kind as an
  enum with the date as a field and the date as the heading with the kind as a field. A
  promotion session writes several evidence entries in one sitting, so a date heading would
  repeat identically across entries that differ. Brian: "the repeated headings are fine. I
  don't need the margin to corry dates."
- decision: An entry's heading is `### <kind>`, typed as an enum over the kinds the Record
  holds. Headings repeat — a hypothesis with eight evidence entries has eight `### evidence`
  headings — which the engine permits, uniqueness being a property of the slug type and not
  of entry headings, and which nothing needs to disambiguate, no artifact citing an
  individual record entry. The kind is what the left margin carries and the date is a field.
- not taken: the date as the heading with the kind as a field, which puts dates in the margin
  but produces identical headings for entries written in one sitting; a heading carrying both,
  which must be typed as a free line and so has the engine validating neither the kind nor the
  date — giving up what the `###` form was chosen for.

### The file authors nothing derivable: no frontmatter, no title

- id: d-2026-09-11-12
- date: 2026-09-11
- raised by: the conversion finding all four frontmatter keys derivable — `id` from the file
  name, `created` from the founding reasoning's date, `baselined` from its entry, and
  `status` from the record, which the checker already recomputed and compared, `hypothesis.
  status.mismatch` existing only to police the copy. Rule 9's last sentence forbids authoring
  beside a source. Brian's own 2026-08-30 reasoning for the frontmatter was a grep across
  files, which `state.md` has since answered for the whole set on every write.
- decision: The file carries no frontmatter and no title. `id`, `created`, `baselined` and
  `status` are read where they are: the file name, the Origin date, the baselined entry, and
  the entries below the last iteration boundary. `state.md` renders all four for every
  hypothesis on every passing write, which is where the corpus-level question is answered.
  `hypothesis.frontmatter`, `hypothesis.status.mismatch` and the baselined coupling of
  `hypothesis.baselined` retire with the fields; the baselined rule returns in changed form
  under d-2026-09-11-19.
- not taken: keeping the frontmatter as a checked mirror, which is the hand-kept mirror
  pattern that took status and baselined off `INDEX.md` on 2026-09-04, differing only in
  being policed; keeping `id` alone, a second home for the file name with no query behind it;
  a title, which would restate the file name.

### An entry carries a date at day precision, not a timestamp

- id: d-2026-09-11-13
- date: 2026-09-11
- raised by: Brian, on the field's name: "'at' is the wrong word. It should be more clear,
  like date." The value was a minute-precision timestamp, and nothing consumes the minutes:
  order is positional, the status computation is positional, and entries written in one
  sitting share a minute in any case. The existing stamps are synthetic — thirty-eight of
  the fifty created entries read `T20:00`.
- decision: Every entry, and the Origin section, carries `date`, typed `date`, at day
  precision. This is the form `decisions`, `question-list` and `declined-candidates` already
  use, so the record joins the rest of the method rather than keeping a precision of its own.
  The migration writes the day and drops the invented minutes.
- not taken: keeping minute precision under a clearer name, which preserves a precision no
  consumer reads and which the existing data never honestly carried; a full ISO timestamp,
  which the prose claimed and no file has ever written.

### The Origin section carries date and reasoning, and no provenance field

- id: d-2026-09-11-14
- date: 2026-09-11
- raised by: Brian, applying the consumer test — "Every field should serve a consumer. Cut
  anything that is dead weight or drift potential" — and then, on the field that would record
  whether a hypothesis was his or Claude's: "Not sure if this field is necessary. Everything
  comes from my thoughts and opinions." The 2026-08-31 investigation of exactly that question
  had reached the same answer, retracting its own framing: the material was a dump of his
  notes and a synthesis of his analysis, and what the consolidation contributed was ordering
  and wording.
- decision: `## Origin` carries `date` and `reasoning`, both required, and nothing else. There
  is no provenance field. Rule 6 makes Brian the author or approver of every statement, so the
  bit such a field would record is a constant; `reasoning` already carries what raised a
  hypothesis in the files as they stand; nothing downstream weights a hypothesis by origin; and
  a self-reported flag would be written by the party whose influence it purports to measure,
  where the code-sessions archive answers the question by what a session read rather than by
  what it claimed. `minting-a-hypothesis` states the provenance in words instead of a form, and
  says that the trap it names is guarded by rule 6 and the archive, so a later session does not
  restore the field as a missing safeguard.
- not taken: a free prose provenance, which leaves the one bit it exists for unrecorded
  whenever a session writes vaguely; a declared line in minting's two forms, which no migrated
  file fits; an enum of `brian` and `claude` beside a free line, which makes the bit greppable
  but records a constant and asks the interested party to report on itself.

### An evidence entry carries date, candidate, tag, finding and falsifier

- id: d-2026-09-11-15
- date: 2026-09-11
- raised by: decomposing the composite citation `(<study>/<slug>; directions-N@<hash>)
  [tag]` into fields, which forced two questions the single string had hidden: what the
  directions version referred to, which d-2026-09-09-12 answered two incompatible ways, and
  how long the hash is, which `StateBuilder` rendered at six characters while the checker
  accepted six to sixty-four and the type vocabulary defined as sixty-four. Brian: "I don't
  think a referee's directions are relevant for the hypothesis file."
- decision: An evidence entry carries `date`, `candidate`, `tag`, `finding` and `falsifier`,
  and nothing else. The directions version and its hash are cut: the referee batch that judged
  a candidate is found from the `<finding-slug> → <target>` locator its index records, and rule
  2 makes the referee's version identical across every entry until a promotion ruling revises
  it, so the field would carry a constant that is also derivable. Cutting it closes
  d-2026-09-09-12's ambiguity by deletion rather than by ruling, and retires the hash-length
  question with it. `tag` stays because promotion may overrule the referee's verdict and
  because the status computation reads it in the file; `finding` and `falsifier` stay as the
  frozen verbatim snapshots of d-2026-09-10-4, the one deliberate denormalization here.
  `hypothesis.evidence.citation` and `hypothesis.evidence.no-falsifier` retire into the
  engine and into the class rule of d-2026-09-11-19.
- not taken: the directions as a path with the hash cut, which still authors a derivable
  constant; the hash alone, self-verifying but unreadable and equally derivable; keeping the
  composite citation as one line, which is what hid both questions for two days.

### An iteration entry carries date, from and reason

- id: d-2026-09-11-16
- date: 2026-09-11
- raised by: the same consumer test over the iteration entry, whose draft carried the old
  wording, the new wording and the reason. The 2026-09-05 reasoning makes the entry a wording
  boundary rather than an edit log, and the deliberation that produced it noted that an
  iteration entry quotes old and new wording "next to the evidence they invalidated, which is
  the referee's re-run trigger".
- decision: An iteration entry carries `date`, `from` — the wording that stood before it — and
  `reason`, Brian's reason for the reword. The new wording is not carried: it is the `from` of
  the next iteration, or the current statement when there is no next, so authoring it is both
  derivable and drift potential, two fields that can disagree with nothing holding them equal.
  The prior wording is not derivable at all, git being excluded by the 2026-08-30 requirement
  that the file hold its own history, so it is carried.
- not taken: carrying both wordings, readable in isolation but a second home for a value the
  chain already fixes; carrying neither and leaving the reword to git, which the 2026-08-30
  requirement refuses.

### A baselined entry carries date and rationale

- id: d-2026-09-11-17
- date: 2026-09-11
- raised by: the same pass over the last kind. The field began on 2026-08-30 as `approved:
  false`, a boolean whose stated shortcoming was that it recorded no date.
- decision: A baselined entry carries `date` and `rationale`, Brian's words, written only by
  him or at his explicit direction. Nothing else: the date the frontmatter used to carry is
  this entry's own, and the judgment adds no weight to the evidence, so there is nothing
  further to record.
- not taken: a rationale-free flag, which loses the words the class exists to hold; the date in
  the frontmatter beside the entry, cut by d-2026-09-11-12 as derivable.

### A candidate token names the finding's home study, and is a token of findings

- id: d-2026-09-11-18
- date: 2026-09-11
- supersedes: d-2026-09-11-4
- raised by: typing the `candidate` field, where the superseded entry's owner position —
  "the owner, study or iteration, the candidate's referee batch sat under" — would give one
  field two owner kinds, only one of which resolves to a findings file, an iteration folder
  holding none. Brian: "Go with B." The superseded entry's own not-taken list declines a
  precise `(studies|iterations)` matcher as a code change for a distinction only two folders
  make, which argues the same way here.
- decision: An evidence entry's `candidate` is `<study>/<finding-slug>`, typed `token of
  findings`, naming the study whose `findings.md` holds the finding, for an iteration-sourced
  entry as for any other; the engine resolves every citation. What the owner position carried
  is doubly derivable: the referee batch is found from the `<finding-slug> → <target>` locator
  its index records, and the iteration is given by position, d-2026-09-11-3 re-entering every
  finding below the boundary. What stands of d-2026-09-11-4: an iteration's re-verification is
  no study; its folder mirrors a study's under `docs/v3-framework/iterations/<slug>/`; the
  batch-artifact paths carry the `<container>` placeholder and the same checkers govern both
  trees; an iteration is not registered in `studies.md` and its existence and ordinal are read
  from the folder. What is overturned: the citation's owner position names where the finding
  lives, not where the referee batch sat.
- not taken: the field typed as a free line with resolution as a hand-written class rule,
  which puts the file's one remaining reference beyond the engine; a two-owner reference type,
  an engine change for the distinction the superseded entry declined to make in code.

### The class's five checks are named

- id: d-2026-09-11-19
- date: 2026-09-11
- raised by: the conversion moving most of what the hand-written checker held into the engine,
  and d-2026-09-07-11 requiring a decision to name every check an entry adds or changes. Brian,
  on the granularity: "Do five ids if it means sharper failure attribution."
- decision: The class's checks are `hypothesis.evidence.fields`, failing when an evidence entry
  lacks `candidate`, `tag`, `finding` or `falsifier`; `hypothesis.iteration.fields`, failing
  when an iteration entry lacks `from` or `reason`; `hypothesis.baselined.fields`, failing when
  a baselined entry lacks `rationale`; `hypothesis.baselined.challenged`, failing when a
  baselined entry follows an unresolved challenging evidence entry within the current wording;
  and `hypothesis.entry.date`, failing when an entry's date is earlier than the entry before it,
  the rule `decisions`, `question-list` and `declined-candidates` already hold. Everything else
  the engine holds: the sections and their order, field presence and type, the heading enum,
  `candidate` resolving, `tag` closed, and the two-space continuation, which the engine's parser
  enforces and the retired hand-written parser ignored. The cardinality rule that a two-finding
  entry is two entries is stated in the Shape as prose and is not a check, no machine being able
  to count findings in prose. All five rules are dormant until the first promotion, the fifty
  migrated files having empty records, so the checker's predicted first run is zero failures and
  that is a statement about coverage rather than a pass.
- not taken: one id covering the kind-to-field rule across three kinds, which is what
  `definition.fields` and `question.entry.fields` do but which Brian declined for attribution;
  leaving the baselining rule and the date order to be earned, which drops a rule he has stated
  in his own words before any file exists to violate it.

### A graph check over the skill's own tables is declared by structure

- id: d-2026-09-11-20
- date: 2026-09-11
- raised by: `artifact.never-read` being found enforced in the validator with no entry naming
  it and no description in the closed founding record, which d-2026-09-07-11 makes an unbacked
  check, while d-2026-09-07-14 admits a check spanning files only as earned from an observed
  failure. Brian: "A graph check is part of getting some reasonable starting point"; and, on
  its scope, "The principle graph-check decision can list all of the relevant ones."
- decision: A check over the skill's own three tables is declared by structure: it holds what
  the tables' shape asserts and is armed with the tables, not earned from a failure. This
  refines d-2026-09-07-14, whose earned-only clause continues to cover checks across a class's
  files, between the code and the tables, and over how a batch went. The checks it backs are
  `artifact.never-read`, `artifact.path-syntax`, `enables.cycle`, `enables.terminus-count`,
  `enables.terminus-owns-processes`, `enables.unbacked`, `gate.ungated`,
  `question-list.writer-not-hitl`, `mutation.read-and-write`, `id.charset`, `id.duplicate`,
  `ref.enables`, `ref.reads`, `ref.writes`, `ref.companion`, `ref.schema`, `file.orphan-activity`
  and `file.orphan-schema`.
- not taken: backing `artifact.never-read` alone, which leaves the same question open for every
  other check of its kind; dropping it as unbacked, which removes the check that reports an
  artifact nothing reads — the artifact-level form of the consumer test Brian applied to every
  field of this class; citing today's deliberation as the observed failure, which would make a
  check earned by having been consulted rather than by a failure in the tree.

### The migration is run by a session, prose verbatim, under the armed checker

- id: d-2026-09-11-21
- date: 2026-09-11
- raised by: d-2026-09-07-27 requiring the decision that names a one-time migration to name the
  process that runs it. Brian: "A session should do the migration, taking the prose from the old
  data and writing it into the new files according to the new guidelines for what the reason
  field ought to be"; "Migration can be verbatim"; and, on the checker, "The checker should be
  armed during that backfill so that the files don't get malformed"; and "The existing files
  should be copied to scratchpad. Then write the new files that conform to the schema from
  scratch, not editing in place."
- decision: A session runs the migration, one file at a time, after the schema, the tables and
  the checker have landed and the exe is published, so that every file is checked against the new
  shape at its write. The fifty existing files are copied to the scratchpad first, and each new
  file is then composed whole and written to its own path: never edited into shape, because a
  surgical edit over fifty different prose bodies can leave a file half-converted, and a file
  that is its own source loses its prose to an interruption between the read and the write.
  Composition from a frozen source is what makes each file idempotent to re-run; the path it is
  written to is not what carries the risk. The statement and the founding reasoning are carried
  verbatim — the words, the line breaks re-wrapping where a block field's continuations are
  indented — and the six statements of d-2026-09-11-6 are recovered from git. The whole migration
  is one commit.
- not taken: a one-off tool, which would author prose that is Brian's; editing the carried prose
  to fit the new field's description, when the description was widened to hold what the entries
  already say; migrating before the checker is armed, which is the state the arming exists to
  prevent; surgical edits to the existing files, which can leave one half-converted and makes
  every file a read-then-overwrite pair; staging the new files at another path and moving them
  when the set is complete, which buys a green whole-repository check during the migration that
  nothing consults, the hook checking only the file written and `check .` being a manual verb.

### Challenge is the epistemic sense only; a dispute is what Brian raises against a finding, a result or a lead

- id: d-2026-09-11-22
- date: 2026-09-11
- raised by: the vocabulary step of the hypothesis-file schema review, which found `challenge`
  carrying two senses in live text: nineteen uses of `challenging` and fourteen of `challenged`
  for verified evidence disagreeing with a statement, and three sites using it as a verb and a
  noun for Brian disputing a claim in a review — the router row for reviewing-leads,
  `reviewing-leads.md`, and `reviewing-findings.md`, whose "for each challenge" is the second
  sense as a noun. The same shape as d-2026-09-07-9, which split rule from check when one word
  carried four senses. Brian: "keep challenge in just the sense of epistemically relative to
  hypothesis"; then, on the other sense, "Go with dispute."
- decision: A **challenge** is verified evidence, bound to the current wording, that disagrees
  with the statement: it is carried by an evidence entry tagged `challenging`, it puts the
  hypothesis in `challenged`, and only a reword clears it (d-2026-09-11-1). A **dispute** is what
  Brian raises in a review against a claim in an artifact, and the session's going back to the
  source to report what it shows. It has three targets and its outcomes are already named: a
  finding, checked against the results, the tally and the items it cites, which is withdrawn or
  superseded; a result, checked at its item's locator, which becomes a shortcoming of the
  directions and never a correction; and a lead, checked against the corpus where the lead says
  it was seen, which is written into Corrections. A dispute is never evidence, never reaches a
  hypothesis, and is resolved by reading the source rather than by a reword. Both words enter
  § Vocabulary, and the three sites that use `challenge` in the second sense are swept in the
  same write.
- not taken: `doubt`, which `reviewing-findings.md` already uses twice as the operative noun and
  which would have cost no new word, declined for reading poorly as a verb; `query`, `check`,
  `verify` and `flag`, each colliding with a word the method or the planner has already reserved;
  leaving the word undefined, which is the state that let two senses run under one word through
  two revisions.

### The class's checks are seven: the engine's failures report under hypothesis.shape and hypothesis.entry

- id: d-2026-09-11-23
- date: 2026-09-11
- supersedes: d-2026-09-11-19
- raised by: building the checker, which showed the superseded entry wrong in one respect: it
  retired `hypothesis.sections` and `hypothesis.entry` into the engine, but the engine holds a
  shape and does not report under one — every other engine-held class carries its problems under
  ids of its own, as `declined-candidates.shape` and `declined-candidates.entry` do. Retiring the
  two would leave a section out of order or a key of the wrong type failing under no id at all.
  Brian, on the granularity: "I don't really care about the granular details of the code. As long
  as it fits the requirements."
- decision: The class has seven checks. Five are its own rules, as the superseded entry named
  them: `hypothesis.evidence.fields`, `hypothesis.iteration.fields`,
  `hypothesis.baselined.fields`, `hypothesis.baselined.challenged` and `hypothesis.entry.date`,
  each failing as the schema's Checks section says. Two carry what the engine finds, surviving
  with changed content rather than retiring: `hypothesis.shape`, failing when a section is
  missing, out of order, or holds other than the sections table says; and `hypothesis.entry`,
  failing when an entry's key is unknown, out of order or of the wrong type, when a line is
  neither keyed nor a two-space continuation, when a heading is outside the kind enum, or when a
  `candidate` resolves to no finding. Both are declared checks under d-2026-09-07-14, holding
  what the Shape asserts and armed with it. What stands of the superseded entry: the five rules
  and their content; that the engine holds the sections and their order, field presence and type,
  the heading enum, `candidate` resolving, `tag` closed and the two-space continuation; that the
  one-finding-per-entry rule is prose and not a check, nothing being able to count findings in
  prose; and that all five rules are dormant until the first promotion, so the predicted first
  run of zero failures is a statement about coverage and not a pass.
- not taken: attributing the engine's failures to the five rule ids, none of which covers a
  section out of order or an unresolvable reference; one id for both, which loses the split
  between a file shaped wrongly and an entry filled wrongly that every other class keeps;
  leaving them unnamed and armed, which is the unbacked state d-2026-09-07-11 exists to prevent.

### Nothing retires a hypothesis; one that cannot recover stands as disproven

- id: d-2026-09-12-1
- date: 2026-09-12
- raised by: a trace of the fifty files against their sources, which found statements that are
  not predictions and asked what becomes of one. The class has no retirement: no activity
  removes a hypothesis, and its three rows are in-place, frozen and append, while
  `hypothesis-index-schema` said the index changes "when a file is minted or superseded",
  presuming a path nothing provides. Brian, given a supersession mechanism, a decision-executed
  retirement and neither: "For SOP, no supersession. If a hypothesis is challenged and can't
  recover from that it just stays as a disproven hypothesis."
- decision: A hypothesis, once minted, stays in the set. A challenge that is never resolved
  leaves it `challenged`, which is the honest reading of its evidence and is where it rests; the
  set is a record of what was claimed and how it fared, not a list of surviving claims. There is
  no `superseded` entry kind, no retirement activity, and no decision-executed removal under
  standard operating procedure. `hypothesis-index-schema`'s "or superseded" is struck, the index
  changing only when a file is minted. This does not decide what the migration of the founding
  fifty may leave unwritten, which d-2026-09-12-6 rules separately, a file never created being a
  different act from one removed.
- not taken: a `superseded` entry kind with a status semantic and a checker rule, which builds
  machinery for what should be rare and lets a claim leave the record that the record exists to
  hold; retirement as a one-time decision per hypothesis, which is the same exit at a higher
  ceremony and would make each disproven hypothesis a question of whether to keep it.

### Testability is the referee's own test, and mint is the only gate it passes

- id: d-2026-09-12-2
- date: 2026-09-12
- raised by: the same trace, which found six statements that prescribe what the instrument ought
  to do — "should be recognised separately by the planner", "may not be optimal and should be
  evaluated against evidence", "may be needed" — and no finding can discriminate one. All six
  passed `minting-a-hypothesis`'s testability criterion as it read, "evidence could confirm or
  refute it". Under d-2026-09-12-1 nothing downstream can repair such a statement: it never
  acquires evidence, so it is never `challenged`, so `iterating-a-statement`'s precondition is
  never met. Brian, asked whether testability was a new field or an existing one, then: "Yes do
  the reword."
- decision: The criterion is stated as the test the method already owns: a referee handed the
  statement and one finding could write its falsifier, what the finding would have been were the
  statement false. A statement admitting no falsifier is not a hypothesis and is not minted;
  `minting-a-hypothesis` § Never names it. The activity file carries why the criterion is
  phrased as an act rather than a possibility — that mint is the only gate, the wording of an
  unfalsifiable statement changing thereafter only through a merge or split. No check holds
  this and none can: whether a falsifier could be written is a judgment, like the
  one-finding-per-entry rule of d-2026-09-11-23.
- not taken: leaving the criterion as written and catching the shape at promotion, where the
  referee returns non-diagnostic for every candidate and the hypothesis is already in the set
  with entries pointing at it; a check on the statement's grammar, which would refuse "should"
  and "may" in statements that legitimately hedge a prediction; a fourth criterion beside the
  three, when the third already names the property and only its test was weak.

### A statement carries the prediction alone

- id: d-2026-09-12-3
- date: 2026-09-12
- raised by: the trace finding three classes in the fifty statements that the schema's sentence
  — provenance, implications, testing method and confirm-or-refute conditions belong nowhere —
  does not reach: a cross-reference to another hypothesis by id; an assertion of the evidence
  the statement rests on, where the record holds evidence; and a trailing open question absorbed
  from an upstream entry that had stated a prediction and then asked one. Brian: "Fix the A
  defects, add the deny instructions."
- decision: § Hypothesis carries the prediction and nothing beside it. No reference to another
  hypothesis by id or slug, a relation between two statements being unheld by anything when
  either is reworded. No evidence claimed for the statement, which is the record's to hold and
  which, asserted here, is recall standing in for a finding. No open question: a question is not
  a prediction, a statement ending in one is two things, and the question belongs in the corpus's
  question list where `ask` writes it. The rule is in `hypothesis-file-schema`, beside the
  sentence it extends, and governs every mint.
- not taken: a check over the statement's prose, which cannot tell a cross-reference from a
  shared noun, nor a question from a sentence ending in a question mark inside a quotation;
  allowing a cross-reference where it names a merge or split, which § Origin already carries as
  provenance and which does not belong in the prediction.

### What § Origin's reasoning may not hold

- id: d-2026-09-12-4
- date: 2026-09-12
- raised by: Brian, on what the migration and every later mint may write: "what should be
  determined now is: what are the exact source materials to use and what is allowed or not
  allowed in the new hypothesis file's origin reason". The field's description says what it
  holds and has never said what it must not; the trace found seven classes present in the
  founding entries, each of which a future mint can write as easily.
- decision: `reasoning` explains why the hypothesis exists and never extends what it asserts.
  It may not hold: a claim the statement does not carry; a corpus reading stated as established
  fact, that being evidence, which enters only as an entry in the record; a prospective testing
  method naming which study will test it; an asserted relationship to another live hypothesis,
  merge and split provenance excepted, which names the files this one came from; a
  confirm-or-refute condition; an assessment of the hypothesis's own testability or thinness,
  which the record answers by what it holds; a synthesis named as what raised it, which
  `minting-a-hypothesis` already refuses; and anything derivable. The list is in
  `hypothesis-file-schema` beneath the field table, with what unites them: each is a thing the
  field can be written to say that stops being true when a statement is reworded or the set
  changes around it. No check holds it, prose being what it governs.
- not taken: leaving the field to its positive description, which is how all seven entered the
  founding set; naming the classes in `minting-a-hypothesis` instead, where a migration session
  and a future reader of the class would not look; a check on the words themselves, which would
  refuse a legitimate motivation that happens to mention a study.

### The migration is run by one session per hypothesis, briefed and composing rather than transcribing

- id: d-2026-09-12-5
- date: 2026-09-12
- supersedes: d-2026-09-11-21
- raised by: a trace of the founding fifty against their sources, which found the chains uneven
  — some beginning in Brian's typed notes, some in a session's reading of a corpus — and one
  layer, the reorganisation of 2026-08-30, lossy enough to have inverted a claim. Brian, on
  verbatim carriage: "This cannot be straight up verbatim copy; some logical sanitation is
  warranted"; on which rules bind: "this is a migration of old, ungoverned data, so many
  restrictions for SOP may not apply"; on the chain: "The key is that the reasoning chain has to
  begin at words that I typed"; and on the form of the preparation: "I don't need citations now.
  I need the historical data and methodology so that one session per hypothesis can search for
  the citations themselves."
- decision: One session handles one hypothesis. It is briefed by
  `docs/v3-framework-historical/hypothesis-migration-briefing.md`, which names the primary
  sources and whose words each holds, the chronology, the trajectory each hypothesis sits on and
  the search path for it, the failure modes the trace established, and the procedure; it carries
  no citations, a citation table written once and read fifty times being the compression that
  produced the defect it exists to catch. The session walks its trajectory to the head, reads
  Brian's own words there, and composes the file whole against them. Composition is permitted
  and transcription is not required, the SOP rules of mint and iteration not binding a bootstrap
  under d-2026-09-07-27; the one constraint is that no claim enters that is not already in the
  file or in its sources. What stands of d-2026-09-11-21: the migration runs after the schema,
  the tables and the checker have landed and the exe is published, so every file is checked at
  its write; the fifty existing files are copied to the scratchpad first; each new file is
  composed whole and written to its own path, never edited into shape, because a surgical edit
  over fifty prose bodies can leave a file half-converted and a file that is its own source loses
  its prose to an interruption; and the six statements of d-2026-09-11-6 are recovered from git.
  Each session commits its own file, one commit per hypothesis, the single commit of the
  superseded entry having assumed a single session.
- not taken: one session over all fifty, which is what the superseded entry assumed and which
  cannot walk fifty chains into three corpora within one context; a citation table assembled by
  one session for the rest to read, which repeats at the briefing layer exactly the compression
  the trace found at the 2026-08-30 layer; deletion-only sanitation, proposed to keep a session's
  authorship off Brian's record and declined because it imports rule 6, which governs `mint` and
  not a bootstrap, and because it cannot repair the defect that most needs repairing; re-deriving
  each reasoning from the upstream sources wholesale, which authors fifty new texts and enlarges
  rather than reduces what Brian must read.

### In the migration, a non-diagnosable hypothesis may be dropped or salvaged

- id: d-2026-09-12-6
- date: 2026-09-12
- raised by: the eight statements of d-2026-09-12-2's finding, which under d-2026-09-12-1 would
  enter the new set unfalsifiable and stay there. Brian: "Hypotheses that ought not to be
  hypotheses can be ignored, though any proper hypothesis that can come out can be brought to my
  attention"; then, on scope: "For the migration specifically, non diagnosable hypotheses can be
  dropped (or salvaged based on the chain if logical)."
- decision: A migration session applies d-2026-09-12-2's test to its statement. Failing it, the
  session either salvages a prediction its chain carries — the clearest case being a statement
  that absorbed a trailing question from its upstream entry, where separating them leaves a
  prediction and the question goes to the corpus's question list — or drops the hypothesis, which
  means the file is not written. A dropped id is not reused and does not become an empty file,
  and the session reports the drop with what the chain showed. A proper hypothesis found inside
  a discarded one is brought to Brian and minted, never written in passing. This licence is the
  migration's alone and does not reach standard operating procedure, where d-2026-09-12-1 holds
  and a hypothesis that should not have been minted is prevented at the gate rather than removed
  after.
- not taken: carrying the eight in as they stand, which fills the new set with statements that
  can never acquire evidence nor ever be reworded; holding all eight for Brian before any file is
  written, which blocks the whole migration on eight judgments; rewording them into predictions
  in session, which authors the claim the hypothesis makes and is his under rule 1.

### An enables edge is a forward handoff; what bars a question-writer's edge is the cycle, not the questions

- id: d-2026-09-12-7
- date: 2026-09-12
- supersedes: d-2026-09-10-1
- raised by: the deliberation that made asking a question its own activity, which needs an
  outgoing edge, since `Termini` is every activity whose `enables` is empty and a second
  terminus fails `enables.terminus-count` and `enables.terminus-owns-processes`. The only
  backed targets are the preparing activities, and the superseded entry says in as many words
  that no question-writing activity has such an edge. That clause was an inventory of the
  activity set of 2026-09-10, when every question-writer sat downstream of preparing; read as a
  rule it forbids an edge the cycle guard permits. Brian chose whole supersession over naming
  the clause in prose.
- decision: An enables edge X → Y means a process of X writes an artifact a process of Y reads
  to carry one line of work a step further toward the terminus; `enables.unbacked` guards the
  floor, an edge with no data flow being an error, and the acyclic-DAG rule the ceiling, the
  pipeline producing forward toward one terminus. A data flow that would close a cycle is a
  re-entry — the same corpus going round again as a new study — carried by the standing, append
  question list and described in prose, never an edge. What bars an edge is therefore the cycle
  and nothing else: `promote` and the plan rows sit downstream of preparing, so an edge from
  them would close one and is a re-entry; an activity nothing enables closes no cycle, and its
  handoff to preparing is an ordinary backed edge. reviewing-findings enables
  surfacing-candidates alone, the name d-2026-09-10-5 gave it. What stands of the superseded
  entry: the meaning of an edge, the floor and the ceiling, the re-entry rule, and what it kept
  of d-2026-09-09-17 — reviewing-findings as an activity, its row `review-findings`, its reads,
  its writes and its procedure, with the preparing edge dropped and its work carried as the
  prose that a shortcoming sends the study back through preparing, the loop the exploration
  side already runs without an edge.
- not taken: naming the stale clause in prose beside a standing entry, which leaves a false
  sentence inside the record's own account of what an edge means, in the entry a later revision
  reads first; re-deriving the edge semantics from scratch, when only the illustration was
  wrong.

### Asking a question is its own activity, enabling both preparing activities

- id: d-2026-09-12-8
- date: 2026-09-12
- raised by: Brian, reviewing the migrated hypothesis files and finding among the statements
  material that is a directive for a question or a study rather than a prediction, then on the
  row that would park it: "So is ask even valid anymore or it the 'ad hoc question raised in
  conversation' in the description just part of the HITL review-leads, as well as all the other
  HITL processes that can add questions?"; then "So if all these processes can write questions
  into question-lists that are relevant to their domain, that would make ask as a standalone
  unit incoherent."; then "Yes, it seems like asking a question would be its own activity."

  The archive says where the row came from. In session `9bd56b73`, 2026-09-05, he typed at seq
  293 "I don't think asking questions about a corpus can ever be non hitl", which ruled the
  mode and merged nothing; at seq 295 he selected a merge of `asking-questions-about-a-corpus`
  into `reviewing-findings-with-brian`, an activity the question's own wording described as
  holding "post-WU review and ad hoc conversation as its two processes". He dissolved that host
  two turns later, typing "We have to go over reviewing a wu. I'm not sure WU even applies
  anymore. Isn't it reviewing an exploratory pass's synthesis?" and then "'WU review' is no
  longer a valid uniform thing. Verification and Exploration are different shapes that were
  conflated before." The merge into `reviewing-leads` was never put to him: the row landed there
  because that is what exploration's review became once the approved host ceased to exist, and
  the founding record's entry compresses the two steps into one. The folder's own splitting
  rule, written later, forbids the shape that resulted — a process splits only at a change of
  mode or when it invokes the runner, and `ask` and `review-leads` share a mode and invoke
  nothing — and `ask`'s whole procedure had become a restatement of `review-leads`'s question
  paragraph.
- decision: `asking-a-question` is an activity of the Router, its file `asking-a-question.md`,
  with one hitl process, `ask`, moved whole out of reviewing-leads: it reads
  `hypothesis-index`, `hypothesis-statement` and `question-list` and writes `question-list`. It
  enables preparing-to-explore-a-corpus and preparing-to-verify-a-corpus, both edges backed by
  the plans' reads of the list and neither closing a cycle, nothing enabling it (d-2026-09-12-7).
  It is the route for a question that arises where no activity's own processes cover the
  asking; a question raised inside an activity that writes questions stays that activity's, and
  reviewing-leads keeps `review-leads` and its question paragraph unchanged. The founding
  record's entry of 2026-09-05, *Questions are written only by hitl processes*, is history as to
  the merge; its ruling that every writer of a question list is hitl stands, and is what
  `question-list.writer-not-hitl` holds. The id is Brian's, chosen from three forms. In the same
  write three `state` cells reading `built` are corrected to `specified` — `ask`, `baseline` and
  `mint` — none having run under revision 2's text: no question list has ever existed, no
  `baselined` entry exists in any hypothesis file, and the fifty files were composed under
  d-2026-09-12-5's bootstrap licence rather than minted. No check is added or changed.
- not taken: keeping `ask` inside reviewing-leads, which the splitting rule forbids and which
  leaves a process whose procedure restates another's paragraph; folding `ask` into
  `review-leads`, which the splitting rule would otherwise require and which leaves a question
  raised outside every activity with no process to write it; dissolving the row and carrying the
  permission as a sentence in SKILL.md, which leaves that occasion with no writer at all;
  `asking-questions-about-a-corpus`, the 2026-09-05 name, and `asking-a-question-about-a-corpus`,
  its singular, both longer in every citation and explicit about a corpus the class already
  binds; an edge to preparing-to-explore-a-corpus alone, which leaves verification's plan reading
  the list with no edge behind it; edges to exploring-a-corpus and verifying-a-corpus as well,
  whose `write-leads` and `write-findings` read the list as the questions in view, four edges
  where two carry the handoff.

### A question entry carries no hypotheses; a question reaches a hypothesis only through a finding, at claiming

- id: d-2026-09-12-9
- date: 2026-09-12
- supersedes: d-2026-09-07-23
- raised by: the first run of `ask`. Brian: "The scope of a study is reconciled at the start of
  it, right? That's when questions get hypotheses attached to them, not when they are written?";
  then "Then we don't need to read hypotheses at all to ask questions"; then "Since no one uses
  hypotheses attached to a question, and hypotheses are connected with findings independently in
  its own process of surfacing candidates, the hypotheses field should be dropped from question
  entries. That is my lean. Before accepting it, check codesessions and decisions to figure out
  why that hypotheses field is on the question list in the first place and whether the rework
  that led to surfacing candidates properly and logically supersedes the intial assignment of
  the field or not."

  The trace. Revision 1's spec pools carried `bears-on`, and verification's scope reconciliation
  recomputed a card's hypothesis list from it: the reconciliation his recall described is a
  revision 1 step with no revision 2 counterpart. On 2026-09-05 he rejected the name, "I don't
  like bears-on", and typed no purpose for the field. At the question-entry review in session
  `a8e72688`, 2026-09-08, the session kept it for one consumer, write-candidates, as the one
  authored edge from a corpus's questions to the hypotheses a round's candidates target. On
  2026-09-10 he ruled the sweep wide, "I intended for candidates to be a massive flood of
  salience judgements that goes wide. It is a lot of findings and a lot of hypotheses, like an N
  x M." and "I'm not going to connect hypotheses one by one.", and d-2026-09-10-6 gave claiming
  the whole hypothesis set per finding, its reads no longer including the question list, with no
  entry retiring the field. What still read it only displayed it: `question.hypotheses`, and
  state.md's hypotheses column per question and open-questions column per hypothesis, the second
  backed by no decision. Asked what that view would earn him, the session found nothing the
  method uses, and found the edge descended from hypotheses read as study requests, the `Tested
  by` lines of the synthesis plan's entries, revision 1's cards and `bears-on`: the prospective
  testing method hypothesis-file-schema bars from an Origin. Brian: "I agree with this
  assessment."
- decision: A question entry has no `hypotheses` field. The file is `questions/<corpus>.md`,
  titled `# <corpus> — questions`, entries only, no head prose. An entry is its heading, then
  `date`, exact and never earlier than the entry before; `raised by`, free, the occasion and what
  raised it with the citation as a token; `question`, free, the question Brian raised;
  `suggested test`, free, present only when one suggests itself. One exact line, and no field
  with a closed set of forms. The checker holds `question.title`, `question.entry.fields` and
  `question.entry.date`; `question.hypotheses` is retired, and a `hypotheses` line fails as an
  unknown key under `question.entry.fields`. A question reaches a hypothesis only through a
  finding, at claiming, blind and against the current set. No writer of a question list reads a
  hypothesis to fill an entry; state.md shows no hypotheses per question and no open questions
  per hypothesis. The hypothesis statements `review-leads` and `review-findings` read for their
  handoff to minting are untouched.
- not taken: the field written with the question, an edge authored before any finding exists,
  on an append line no later mint or reword can update, whose only readers display it; the edge
  moved to where a study is scoped, which claiming's sweep over every hypothesis would leave a
  display as well; state.md's per-hypothesis view kept, which shows a guess as coverage and is
  wrong in both directions.

### A process mandates a commit only where the commit carries something the method needs

- id: d-2026-09-12-10
- date: 2026-09-12
- raised by: the first run of `ask`, whose procedure closed on one commit. Brian: "I don't think
  a commit is necessary, so that can be taken out of the asking-a-question.md."; then "Nothing
  should mandate commits in the whole skill."; then, over the session's walk of every commit in
  the folder, "For the ones where commit has meaning, keep it. For the ones where it deletes
  cleanly delete it, including asking-a-question. Keep 'a commit' as an act reserved for me.";
  and "Delete the promote commit message references. I agree with the keep and delete rulings."
- decision: A commit stays where it carries something: `gate-and-commit`, whose id and single
  commit hold the new wording, the iteration entry and the fresh evidence entries as one write or
  none; building-a-tool, where CORPORA.md and the `agent-runner` skill change in the same commit
  as the code they describe; revise, one commit per landed step with the note in the last, and a
  wholesale rewrite swapped in one commit; and the commit among the acts reserved to Brian at
  which a run ends. Where a commit carried nothing it goes, and `git` leaves the process's
  instruments with it: the process of asking-a-question, `mint`, `review-leads` and
  `review-findings`; `baseline`, whose `git` named no commit in its procedure; and `promote`,
  which loses its single commit, the scope and candidate tokens named in the commit, and the
  commit in its router row. Nothing reads a commit message: promotions are read from the
  hypothesis records and declines from declined-candidates.md.
- not taken: no commit anywhere, which drops the single write of `gate-and-commit`, the co-change
  building-a-tool holds and the atomic swap revise makes; one commit per hitl process as before,
  which mandates a commit where it carries nothing; promote's commit message kept, which nothing
  reads.

### Rule 10: every field that records Brian's words is composed from the deliberation, his words in quotation marks

- id: d-2026-09-12-11
- date: 2026-09-12
- raised by: the first run of `ask`, where the session read the procedure as transcribing one
  typed question and listed its shortcomings. Brian: "It shouldn't be that strict. We had a HITL
  deliberation. You should be taking my words from that deliberation and putting it as a
  question for the question list. That's what the activity is about."; "This setup needs to
  apply uniformly. And since we shouldn't be writing duplicates that can drift, where should it
  go? In SKILL.md, with the processes referring to it?"; "Put in skill.md and it should be every
  field that records my words. Put my words in quotation marks. Make it a constitutional rule.";
  "Decisions.md does not have to be retrofit but future entries should conform to the new
  quoting rules."; "Really, all text from now on ought to follow this new principle of, the
  instruction is to put together a conform entry (whatever it is) from the deliberation session,
  and my words are quoted. This should supersede any other ad hoc instructions. Does this apply
  uniformly or not?"; and, on the session's answer, "Classifier system prompt does not need
  quotes or my wording. It ought to be the wording that works best for claude, which claude is
  better at writing. Agreed on quotation marks recording that they are my words, not to be used
  for words written by claude that I approved. Agreed on the nothing to act on cases."
- decision: SKILL.md gains a tenth constitutional rule. Every field an hitl process writes that
  records Brian's words is composed by the session from the deliberation that raised it, his
  typed words verbatim inside quotation marks and the session's own words, in Claude's voice,
  outside them. Quotation marks hold only what he typed: his words that themselves hold
  quotation marks take single ones inside, and a label he selected, a question put to him and
  session text he approved are never quoted, approval being his decision and never his words.
  A hypothesis statement he types is quoted; one the session drafted and he approved is not.
  The rule has nothing to act on where no deliberation exists, in `session` and `agent`
  processes, nor in verbatim copies, an evidence entry's finding and falsifier and an
  iteration's `from`, which carry their source's text under their own rule. A directions body
  is written in the wording that works best for the model, which Claude writes, with neither
  quotation marks nor his wording; his calibration rulings are quoted in the calibration file.
  The rule supersedes every manner instruction the folder carried for his words: `in his words`
  and `in Brian's words` in the question-entry, declined-candidates and hypothesis-file schemas,
  in `baseline`, `mint`, iterating's precondition, `review-leads`, `review-findings`, `ask` and
  two router rows; `with his assertions as the content` in hypothesis-file-schema and `mint`;
  decisions-schema's quoting bullet; findings-schema's `in the session's or Brian's words`; and
  baseline's Never against paraphrase. Each field keeps what it holds, and the rule says how his
  words enter it. Vocabulary's rule entry counts ten. The rule binds what is written from the
  day it lands: decisions.md and the hypothesis files are not retrofitted.
- not taken: transcribing one typed question, which the procedure read as and Brian ruled too
  strict; a manner instruction per field or per process, the copies that drift; his wording or
  quotation marks in a directions body; retrofitting decisions.md and the hypothesis files.

### Showing an entry in the chat before writing it is a process's own instruction, decided at its first run

- id: d-2026-09-12-12
- date: 2026-09-12
- raised by: the first run of `ask`, where the session showed its drafted entry before writing
  though the procedure did not ask it to. Brian: "Draft should be shown in the chat before
  written."; then "Show the draft first only for question entries right now. Each other to be
  decided when moving from specified to built. For decisions, I said to not put it in the
  session first because it got too tedious with the scope."; then "Show the draft first can be a
  per process instruction. Does this work?"; and "Yes, only ask right now." The record bore out
  his recall about decisions: d-2026-09-11-5 writes a unit's entries together at its end for him
  to scan, and revise's text still wrote each as it landed.
- decision: Showing an entry to Brian in the chat before it is written is an instruction of the
  process that writes it, never a rule of a class or of the folder. The process of
  asking-a-question carries it for its question entry. Every other process that writes a field
  recording Brian's words has it decided at its first run, where revise presents the first run
  against its declaration and the process moves from `specified` to `built`; revise's text says
  so. Decision entries stay under d-2026-09-11-5, and revise's text, which still wrote each
  decision as it lands, is brought to that entry in the same write.
- not taken: a rule in question-entry-schema, which binds the four other writers of a question
  list before any has run; showing every entry of every process now, decision entries included,
  the tedium d-2026-09-11-5 removed; leaving the other processes' choice to be remembered at
  their first run with nothing in the text to raise it.

### Asking a question's process is write-question, reading the corpora and then that corpus's list

- id: d-2026-09-12-13
- date: 2026-09-12
- supersedes: d-2026-09-12-8
- raised by: the first run of `ask`, which did not match its declaration: it read the
  deliberation, CORPORA.md for the corpus id the title check requires, and state.md in place of
  an INDEX.md gone stale, and read hypotheses only for the field d-2026-09-12-9 drops. Brian: "It
  should read the corpora and then the question-list for that corpora."; "Ask can go to built.";
  "But 'ask' is ambiguous and that should be made unambiguous as a process name as well."; and
  "write-question works." The archive showed `ask` coined by a session, first in d-2026-09-07-21,
  with no typing of Brian's behind it, and the folder's own text using asking for Brian asking,
  for the session asking him, and for a process that asks nothing.
- decision: `asking-a-question` is an activity of the Router, its file `asking-a-question.md`,
  with one hitl process, `write-question`: it reads `corpora`, then the `question-list` of the
  corpus the question is about; it writes `question-list`; it invokes no instrument; its state is
  `built`. It composes the entry from the deliberation under rule 10 and shows it to Brian before
  writing it (d-2026-09-12-12). It enables preparing-to-explore-a-corpus and
  preparing-to-verify-a-corpus, both edges backed by the plans' reads of the list and neither
  closing a cycle, nothing enabling it (d-2026-09-12-7). It is the route for a question that
  arises where no activity's own processes cover the asking; a question raised inside an activity
  that writes questions stays that activity's, and reviewing-leads keeps `review-leads` and its
  question paragraph. The founding record's entry of 2026-09-05, *Questions are written only by
  hitl processes*, is history as to the merge; its ruling that every writer of a question list
  is hitl stands, and is what `question-list.writer-not-hitl` holds. The id `asking-a-question`
  is Brian's, chosen from three forms. `baseline` and `mint` stay `specified`, neither having
  run under revision 2's text. No check is added or changed.
- not taken: `ask`, which reads as either party asking; `compose-question`, whose verb
  `compose-candidates` uses for a generated view; `write-question-entry`, longer and in the
  schema's word; `record-question` and `file-question`, colliding with a hypothesis file's
  § Record and with file as a noun; reading the hypotheses, which nothing the entry holds needs.

### Rule 7 steers a discussion that turns to story content; it never writes a question

- id: d-2026-09-12-14
- date: 2026-09-12
- raised by: walking rule 10 against the question writers, where rule 7's closing clause,
  redirect to the framework-relevant question or say it is out of scope, read as an
  instruction to author a question entry. Brian: "So are Rule 7 and Rule 10 in tension? That
  would apply to all HITL about questions, not just this freestanding asking-a-question
  activity."; then "What does 'redirect' mean here, precisely?"; and "A sounds good". The
  trace: the clause entered on 2026-08-31 in session `6e5d2e65` (commit `1b6e705`), a session's
  proposal approved by Brian's "Please make the appropriate changes to the skill", written for
  the one discussion at seq 809–818 where he named "story content comparison" and answered the
  session's trimmed framework question with "Trim it back". It read: when the discussion drifts
  into content territory, redirect to the framework-relevant question ("do the tracks support
  this?") or acknowledge it's outside scope. Revision 1's constitutional rule 7 of 2026-09-03
  dropped the trigger, the object and the example; revision 2 carried it verbatim, after
  question became a defined word.
- decision: Rule 7's clause says what is steered and toward what: when a discussion turns to
  story content, the session steers it to what that content asks of the framework, such as
  whether the planner's tracks support it, or says it is out of scope. The word question leaves
  the rule, which no longer collides with § Vocabulary's question. A steer writes nothing;
  whether a framework question that arises is written is the question writers' own rule. The
  rest of rule 7 is unchanged.
- not taken: replacing the collided word alone, which drops the example that made the clause
  concrete; a Vocabulary note that rule 7 uses question in its ordinary sense, two senses of a
  defined word in one folder.

### Rule 10 applies to fields that record deliberation and reasoning, never to facts or neutral assertions

- id: d-2026-09-12-15
- date: 2026-09-12
- supersedes: d-2026-09-12-11
- raised by: walking rule 10 against every writer and every copy of Brian's words, which
  found a hypothesis statement quoted by rule 10 and copied into claiming directions that rule
  10 kept unquoted. Brian: "Once rule 7's wording is cleaned up, are ther no special cases to
  rule 10?"; then "Yes, A. The hypothesis statement is not quoted. It's just a statement. The
  quoting rules have to apply only to fields that are capturing my words from an HITL session
  and distinguishing them from claude's words or framing of my words. This makes sense for the
  reasoning: field on hypothesis and for the raised by, decision, and not taken fields in
  decisions.md (note that quoting isn't a hard requirement). Remains to be seen what other
  fields it applies to. Quote everywhere seems like an incorrect initial instinct."; "Note that
  not all free form fields are the same. I had a typo omission."; "I think this quoting
  discipline doesn't apply to facts. It applies to deliberation and reasoning bookkeeping.";
  and "This rule 10 wording is good." d-2026-09-11-14's refusal of a provenance field, and its
  reason that a self-reported flag is written by the party whose influence it purports to
  measure, bore on the statement.
- decision: SKILL.md's tenth constitutional rule quotes Brian where the method keeps its
  reasoning. A field that records deliberation or reasoning, what raised something, why it
  exists, what was ruled or declined, a judgment or a suggestion, is composed by the session
  from the deliberation; where it uses what Brian typed, that text is verbatim inside quotation
  marks, his own quotation marks becoming single ones, and the session's words and framing sit
  outside them. Quoting is not a requirement to use his words, only the form they take when
  used. A label he selected, a question put to him and session text he approved are never
  quoted. A field that states a fact or a neutral assertion, a finding, a hypothesis statement,
  a question, is not written this way, however much of it came from him; a statement is the
  claim, and his wording of it, where it matters, is quoted in an Origin or an iteration
  `reason`. Each schema's field description says which kind a field is, `under rule 10` marking
  the reasoning fields: an Origin's `reasoning`, an iteration's `reason`, a baselined
  `rationale`, a decline's `reason`, a calibration's rulings, a decision's `raised by`,
  `decision` and `not taken`, a question's `raised by` and `suggested test`. What stands of the
  superseded entry: the rule is constitutional; it replaced every manner instruction the folder
  carried for his words, each field keeping what it holds; § Vocabulary counts ten rules; and
  it governs what is written from its landing, decisions.md and the hypothesis files not
  retrofitted. Its clauses on directions bodies, on processes with no deliberation and on
  verbatim copies leave, those being fields that record no reasoning; a directions body stays
  in the wording that works best for the model.
- not taken: quoting every field that records his words, the superseded rule, which quotes a
  statement into claiming directions and referee items and restores as punctuation the
  provenance marker d-2026-09-11-14 refused; quoting in the file and stripping quotation marks
  on the way to an agent, which no machine can tell from quotation marks that mention a term,
  as statements 001, 027, 030 and 048 use them; quoting everywhere with the directions clause
  yielding, which reverses Brian's directions ruling on the text a blind call judges.

### Rule 9: an entry a session wrote is corrected until something that cannot be corrected relies on it

- id: d-2026-09-12-16
- date: 2026-09-12
- raised by: the first question entry, `v1-archive/scene-detail-scrap-rate`, written that day
  under the rule 10 then standing and wrong under d-2026-09-12-15, in an append class with no
  process that withdraws. Brian: "Yes, we should fix the v1-archive/scene-detail-scrap-rate
  entry to be inline with the new guideline."; "Rule 9 should not apply if we haven't even
  settled the framework yet."; "If we are in revising-the-method then rule 9 should be
  loosened, right?"; then, on the session's commit boundary, "It should not have to do with git
  commit status. That is irregular and ungoverned."; "Reliance makes sense."; and "Conforming an
  entry to its class without changing what it records - this is fine. The chain for riliance
  works." The session had shown that loosening for revising-the-method or until the framework
  settles has no end, revise being continuous and the epistemic framework having no settled
  state, and that hypothesis records and decisions are already relied on.
- decision: Rule 9 keeps appended and frozen artifacts unedited, save that an entry a session
  wrote may be corrected until it is relied on: conformed to its class without changing what it
  records. An entry is relied on once something that cannot be corrected relies on it, directly
  or through a chain, by a citation the checker resolves or by an entry placed after it in a
  record; what cannot be corrected is a succeeded version, a file the runner or an agent wrote,
  or an entry itself relied on. Until then it is corrected together with whatever relies on it,
  in one write. A change to what an entry records is never a correction: a finding that does
  not hold is withdrawn or superseded, a disputed lead is a line under Corrections, a reworded
  question is a new entry. revising-the-method's § Never reads that a fix to a file wrong under
  a right class is allowed within rule 9. The boundary is reliance and never the state of git.
- not taken: loosening rule 9 while revising the method, which never ends and does not reach
  the question list; suspending it until the swap, which strips protection from records and
  decisions already relied on; the commit as the boundary, irregular and governed by nothing;
  any citation as the boundary, under which a unit's cross-cited decision entries could never
  be corrected on its scan; the session that wrote the entry, not a governed artifact either;
  Brian's review of it, which question entries pass before they are written.

### A unit's entries are written together at its end and scanned, and corrected within rule 9

- id: d-2026-09-12-17
- date: 2026-09-12
- supersedes: d-2026-09-11-5
- raised by: d-2026-09-12-16, which replaced the commit as the moment a written entry stops
  being correctable; the superseded entry made the commit that moment for decision entries.
  Brian: "It should not have to do with git commit status. That is irregular and ungoverned."
  revising-the-method § revise still wrote each decision as it landed, the superseded entry
  never having reached its text, and the text was brought to it on 2026-09-12.
- decision: A unit opens with a queue of starting points, one line each, ordered widest-first
  by the session and seen by Brian before any is taken; he reorders, removes or adds. Taking a
  starting point opens a stack with it at the bottom; deliberating any decision on the stack
  pushes the decisions it surfaces on top, and the pop is from the top, so the starting point
  is settled last and may name the decisions settled above it. A stack is worked to empty
  before the next starting point is taken, and after each stack empties the queue is presented
  again, re-ordered and re-formed; it is never presented as settled. A decision is ruled in
  session; its entry is drafted at the end of the unit, with every entry of that unit, in the
  order the decisions were settled, and Brian scans the file. He approves the ruling, never the
  drafted sentence. How many entries a unit's rulings become, and where the lines between them
  fall, is the session's judgment under the one-decision rule. An entry he corrects on the scan
  is corrected within rule 9: while nothing that cannot be corrected relies on it, with the
  entries relying on it corrected in the same write.
- not taken: the commit as the last moment of correction, the superseded rule, which ties the
  record's discipline to an act nothing governs; writing each entry as it lands, which a review
  at scale made tedious without adding rigor; batching with no review at all.

### A question writer never writes a question Brian did not raise

- id: d-2026-09-12-18
- date: 2026-09-12
- raised by: d-2026-09-12-15 making a question's text a neutral assertion in the session's
  wording, after which the Never line "writes a question Brian did not ask" could not mean did
  not type. Brian: "A works".
- decision: The Never lines of `write-question`, `review-leads` and `review-findings` read
  writes a question Brian did not raise, the verb the activity files already use for his act:
  he raises a question by typing it or by taking up a session's wording, a proposed question or
  a steer's framework question among them. `write-question`'s line also closes on writes more of
  a test than a naive suggestion, a suggested test coming from either Brian or the session
  (d-2026-09-12-19).
- not taken: did not decide to enter, explicit but a new phrase in three files where the
  method has a verb; deleting the line, which removes the only guard in the two reviews that
  show no draft.

### A question entry: no hypotheses; a neutral question; a reasoned raised by; a procedural suggested test

- id: d-2026-09-12-19
- date: 2026-09-12
- supersedes: d-2026-09-12-9
- raised by: the first question entry and the questions its writing surfaced. Brian: "Suggested
  test can be either one. The quotes determine where the suggestion came from."; "Question
  should be a neutral assertion, fact-like. It's not a fact, but it's a non-opinionated thing.
  Raised by and suggested tests have opinions."; "What should be the line between raised by and
  suggested test?", then "This makes sense."; and, on whether `raised by` holds a closed set of
  tokens, "Cite a thing by its own token where it has one. This is a guideline, not a hard rule.
  The things will be in context. It's to leave a trail for future HITL sessions to follow when
  asked for." The session found the listed token forms stale both ways: a proposal and a
  candidate have no token of their own, and a finding and a hypothesis, which writers cite,
  were missing.
- decision: A question entry has no `hypotheses` field. The file is `questions/<corpus>.md`,
  titled `# <corpus> — questions`, entries only, no head prose. An entry is its heading, then
  `date`, exact and never earlier than the entry before; `raised by`, why the question exists,
  the occasion and what raised it and Brian's beliefs, recollections and motivation behind it,
  what raised it cited by its own token where it has one, as a trail a later session can follow
  when asked, and `recall` or `carried from the founding pool` where those raised it, under rule
  10; `question`, a neutral assertion of what is asked; `suggested test`, present only when one
  suggests itself, a naive note of procedure that holds whatever the answer and never a belief
  about it, from Brian or the session, rule 10's quotation marks showing which, never a
  criterion. The line between the last two: a sentence that would be wrong if the study came out
  a certain way is a belief and belongs in `raised by`. One exact line, and no field with a
  closed set of forms. The checker holds `question.title`, `question.entry.fields` and
  `question.entry.date`; `question.hypotheses` is retired, and a `hypotheses` line fails as an
  unknown key under `question.entry.fields`. A question reaches a hypothesis only through a
  finding, at claiming, blind and against the current set; no writer of a question list reads a
  hypothesis to fill an entry, and state.md shows no hypotheses per question and no open
  questions per hypothesis. The hypothesis statements `review-leads` and `review-findings` read
  for their handoff to minting are untouched.
- not taken: a `raised by` list of token forms repaired, a second copy of each class's citation
  form that goes stale when a class changes; a checked `cites` line beside a free `raised by`,
  the exact provenance line d-2026-09-07-23 refused, with nothing deriving from it; Brian's
  opinions in `question`, which makes the neutral part of the entry his framing; beliefs in
  `suggested test`, which can enter directions as a criterion and decide part of the answer
  first; a suggested test from Brian only.

### Rule 10's quoted words come from wherever Brian's typing is recorded as his

- id: d-2026-09-12-20
- date: 2026-09-12
- raised by: rule 10's composed from the deliberation, set against the writers that quote his
  typing from elsewhere: d-2026-09-12-9 quoting session `9bd56b73` of 2026-09-05 and session
  `a8e72688` of 2026-09-10, the migrated Origins quoting archived sessions, and the migration
  briefing naming the Keep dump as Brian, typed. Brian: "Yes, option A"; and "Not just
  'archived session'; be more specific that it's about reading transcripts from the codesessions
  db".
- decision: A reasoning field is composed from the deliberation and the records of Brian's
  typing it draws on. What he typed is his own prose wherever it is recorded as his: the
  deliberation at hand, a user turn or `Typed:` line in a Claude Code transcript read from
  `codesessions.db` (the `code-sessions` corpus), a Keep note, his navigation note on a
  conversation block. Used, it is verbatim inside quotation marks and cited to its source when
  it is not the deliberation at hand. Never quoted as his: a label he selected, a question put to
  him, session text he approved, and text whose voice is not established as his, such as v1
  archive and plan notes, which are corpus content cited by locator. Rule 8 is unchanged: quoting
  past typing makes it no evidence.
- not taken: quoting only the present deliberation and paraphrasing the past with a citation,
  the compression the migration trace found and d-2026-09-12-9 already did not practise;
  quoting AI-session transcripts but not Keep notes or planning documents, a special case with
  no reason, the Keep dump being the most direct record of his words.

### A disagreement with the referee's verdict is a decline; a promoted tag is always the verdict

- id: d-2026-09-12-21
- date: 2026-09-12
- supersedes: d-2026-09-11-15
- raised by: promote's step 3, promoting against the verdict with the ruling in the evidence
  entry, an entry with no field for it. Brian: "There were a lot of discussions on the evidence
  entries in codesessions and decisions. Check those again"; "Wasn't there a new artifact made
  to hold those decisions?"; "I am getting lost in what the case is that this is about, and how
  it differs from the happy path"; "I don't see how 'The referee says supporting; you think it
  challenges' is different than 'The referee says challenging; you think that's spurious' - the
  polarity is reversed, but what is different?"; and "Yes, option D works". The record:
  revision 1's promotion let Brian decline a diagnostic candidate or promote nothing; the
  2026-09-05 walkthrough had him decline, hold or promote nothing when he disagreed, and his
  typing there was "Thus the source is only used by me if I disagree with the referee in
  promoting-a-candidate?"; d-2026-09-10-8 kept declines only in `declined-candidates.md` and
  reasons off the record; d-2026-09-11-1 declines a spurious challenge. The override path stood
  only in step 3's text of commit `3eb5515` and in the superseded entry's reason for `tag`.
- decision: An evidence entry carries `date`, `candidate`, `tag`, `finding` and `falsifier`, and
  nothing else. `tag` is the referee's verdict and stays because the status computation reads it
  in the file; promotion never records a tag other than the verdict. A disagreement with the
  verdict, whether Brian judges the finding to bear on nothing or to bear the other way, is a
  decline, his ruling its `reason` in `declined-candidates.md`; a ruling that shows the referee
  wrong in general is what sends its directions back through preparing-to-verify-a-corpus.
  What stands of the superseded entry: the directions version and its hash are cut, the referee
  batch that judged a candidate being found from the `<finding-slug> → <target>` locator its
  index records; `finding` and `falsifier` are the frozen verbatim snapshots of d-2026-09-10-4;
  `hypothesis.evidence.citation` and `hypothesis.evidence.no-falsifier` retire into the engine
  and the class rule of d-2026-09-11-19. promote's step 2 cites the candidate's token alone, its
  § Never gains promotes with a tag other than the verdict, and `tag`'s description reads as the
  referee's verdict classified it.
- not taken: a `ruling` field on the evidence entry, which puts a reason on the record
  d-2026-09-10-8 keeps reasons off and lets a tag stop being the blind referee's; widening
  `declined-candidates.md` into an outcomes file, against "Declines only in this file"; allowing
  only the flip to challenging, a special case by polarity whose ruling has no home; keeping the
  override with no reason, which loses what a referee revision needs.

### The verified findings are a corpus, explored only; the registry keeps no special corpus

- id: d-2026-09-12-22
- date: 2026-09-12
- supersedes: d-2026-09-09-12
- raised by: where a question comparing corpora would live, `verified-artifacts` having no
  question list. Brian: "A cross corpus comparison question does not seem valid for the way I am
  using question for corpora which requires rigor. So is this a different kind of question?";
  "Questions for a corpus no longer associate with a hypothesis, right?"; "It would be reading
  and comparing verified artifacts, which is a subjective thing, not going over itemized units.
  Should verified-artifacts be a corpus? What would it have, concretely? Is it over findings?";
  "The items are the finding entries."; "Exploration only makes sense."; "verified-findings is
  good."; "Put exploration only in corpora.md."; and "4 sounds good". The trace: revision 1's
  Synthesis type read verified artifacts across corpora for comparison, retrospective,
  adjudication, evaluation and connection; the founding record's ruling that an exploration
  over verified artifacts is an exploration retired it; `verified-artifacts` then survived only
  as a registry value and a precondition clause. Of synthesis's aims, adjudication is the
  evidence pipeline and baselining, evaluation is the planner's terminus, connection is story
  content, and the wide reading across verifications had no home.
- decision: A question is about one corpus's items; a comparison across corpora is a hypothesis
  whose evidence arrives from each corpus through claiming, never a question. The standing
  findings of every verification are a corpus, `verified-findings`, in CORPORA.md: each finding
  entry one item, located by its `<study>/<slug>` heading in its study's `findings.md`, standing
  as of the itemizer's run. It is explored only, never verified, CORPORA.md's caveat giving the
  reason: a finding about findings could be promoted beside the finding it is about, counting
  one piece of evidence twice. Findings of different verifications rest on different items under
  different directions, so a pattern across them is a lead, never a joined claim, and a finding
  withdrawn after the run is caught at the lead review. preparing-to-explore-a-corpus loses its
  clause for the verified artifacts of promoted verifications. The itemizer across studies is
  the first task of the first study that needs it. What stands of the superseded entry:
  `registry.type` holds that a study's type is the one its id's prefix names, with no referee
  case; `registry.corpus` holds that the corpus cell is an id in CORPORA.md, or `skill` for an
  audit, and now has no `verified-artifacts` value, the registry checker dropping its constant.
  Its clause on `hypothesis.evidence.citation` was retired by d-2026-09-11-15.
- not taken: a question list for cross-corpus questions, whose questions no frozen predicate
  over one corpus's items could answer; verifying the findings corpus, which double-counts;
  retiring the reading altogether, which leaves synthesis's one homeless aim with none;
  `findings` as the id, which is the Artifacts table's id for the class; `verified-artifacts`
  kept, which names more than the items are.

### An Artifacts path is one pattern or `no single pattern`

- id: d-2026-09-12-23
- date: 2026-09-12
- raised by: the corpus row's path, `outside the repo`, beside corpora that live in the repo and
  now one made of the repo's own findings. Brian: "It should say where CORPORA.md says. There
  are other corpora in the repo so this is already wrong."; and "A works". No decision had named
  the literal; skill-schema's grammar allowed exactly one non-pattern value.
- decision: `artifact.path-syntax` holds that a path cell is one repo-relative pattern with
  placeholders in angle brackets, or `no single pattern`, the one non-path value, meaning there
  is no pattern the tool can list files from, in the repo or out of it. The `corpus` row's path
  is `no single pattern`, and its description says the corpora are where CORPORA.md says.
  skill-schema and `ArtifactPath` change with their tests.
- not taken: `where CORPORA.md says` as the literal, a grammar value named after one file that
  the next patternless artifact would need changed again; `outside the repo` kept, false of
  several corpora.

### A slug is a machine identifier the session creates, never changed

- id: d-2026-09-12-24
- date: 2026-09-12
- raised by: the first question entry's slug, and each schema saying when a slug is made but not
  who makes it, with no stability stated for a hypothesis's. Brian: "Slug can be created by the
  session. It's a machine identifier that is stable, not something that has to be chosen by
  me."; and "Hypothesis slugs are stable. Migration is an exception because the earlier one
  wasn't legitimate and had an inverted meaning. Vocab entry for slug that says created by the
  session works".
- decision: § Vocabulary defines slug: a machine identifier that reads in a sentence, lowercase
  `[a-z0-9-]+`, created by the session with what it names and never changed, each class's schema
  saying where it is unique. A hypothesis file's slug is a slug, so it is never changed; a
  declined candidate's heading and a claiming result cite it. The migration of the founding
  hypothesis files under d-2026-09-12-5 is the one exception, a bootstrap: its sessions may
  rename a founding file's slug where the file's claim was never legitimate or its meaning was
  inverted, as 041's was on 2026-09-12. The exception is carried here and in no activity file or
  schema (d-2026-09-07-27).
- not taken: a slug fixed only once something cites it, which leaves hypothesis slugs renamable
  outside the bootstrap; the slug's author stated in question-entry-schema only, which leaves the
  other classes silent.

### A question is withdrawn and reinstated by appended lines; adding one reads the list first

- id: d-2026-09-12-25
- date: 2026-09-12
- supersedes: d-2026-09-07-24
- raised by: the schema naming a withdrawing process no activity described. Brian: "Is
  withdrawal even necessary? And why is questions append only so strictly? What led to that?";
  "Actually option A seems good because it prevents 'malformed questions' from reentering a week
  later? What would make me want to withdraw something anyway?"; "How about if withdrawn can be
  added, but if I decide I do want the question after all a week later, the withdrawn can be
  removed and the question stands? But asking a different iteration of the question has to be a
  new one."; "Reinstated works. Code can adapt to the new requirements."; and "Yes, the skill
  should be updated to give instructions to hitl sessions that attempting to add a question
  should compel the session to read what is already there first and point out any withdrawn
  ones along with reasoning that matter." The archive showed append chosen on 2026-09-08 in
  session `a8e72688` on the session's recommendation, from a candidate precedent since retired
  and the reading that a reworded question is a new question, Brian moving on without a typed
  reason; what still justifies it is reliance, a cited question's wording standing under what
  froze or answered it.
- decision: The class is append for every line. A question is withdrawn by `- withdrawn: <date>
  <reason>` and reinstated by `- reinstated: <date> <reason>`, lines appended beneath the fields,
  alternating and starting with withdrawn, each reason under rule 10, written by the hitl
  process in which Brian withdraws or reinstates it, `write-question` outside every other
  activity. A question he wants back is reinstated under its own slug; a different iteration of
  it is a new entry. Open is derived: an entry with no withdrawn line, or whose last withdrawn
  line is followed by a reinstated line; frozen and answered are derived from a directions
  version's and a verification's citations; no status line is written. Before any entry is
  added, the session reads the list and puts in front of Brian every withdrawn entry that bears
  on the question, with its reasons, so that he reinstates it, writes a new entry, or lets it
  rest; the rule is question-entry-schema's, binding every writer. `write-question` shows a
  withdrawal or a reinstatement before writing it, and its § Never gains withdraws or reinstates
  a question he did not. `question.withdrawn` holds that each appended line is a date then a
  reason, sits beneath the fields and before no keyed line, and that the two alternate starting
  with withdrawn; state.md reads a reinstated question as open.
- not taken: removing the withdrawn line, an edit to one line of an append class and the loss of
  the reason a question was dropped; re-entering a wanted question under a new slug, which leaves
  the old one withdrawn beneath what cites it; rewording or deleting an uncited question in place,
  which leaves no trail against a malformed question returning; no withdrawal, which keeps every
  question ever raised open in every plan; an activity of its own for appending one line.

### Rule 10 requires no rewrite of what is already written, and permits one Brian asks for

- id: d-2026-09-12-26
- date: 2026-09-12
- supersedes: d-2026-09-12-15
- raised by: the tightening of the migrated hypothesis files, which rewrites Origins written
  before rule 10 while the superseded entry said nothing already written is rewritten to meet
  it. Brian: "Rule 10 should be 'nothing already written must be rewritten'. It can be if I want
  to, in order to meet compliance."
- decision: SKILL.md's tenth constitutional rule quotes Brian where the method keeps its
  reasoning. A field that records deliberation or reasoning, what raised something, why it
  exists, what was ruled or declined, a judgment or a suggestion, is composed by the session
  from the deliberation; where it uses what Brian typed, that text is verbatim inside quotation
  marks, his own quotation marks becoming single ones, and the session's words and framing sit
  outside them. Quoting is not a requirement to use his words, only the form they take when
  used. A label he selected, a question put to him and session text he approved are never
  quoted. A field that states a fact or a neutral assertion, a finding, a hypothesis statement,
  a question, is not written this way, however much of it came from him; a statement is the
  claim, and his wording of it, where it matters, is quoted in an Origin or an iteration
  `reason`. Each schema's field description says which kind a field is, `under rule 10` marking
  the reasoning fields: an Origin's `reasoning`, an iteration's `reason`, a baselined
  `rationale`, a decline's `reason`, a calibration's rulings, a decision's `raised by`,
  `decision` and `not taken`, a question's `raised by` and `suggested test`. The rule is
  constitutional and replaced every manner instruction the folder carried for his words, each
  field keeping what it holds; § Vocabulary counts ten rules. Nothing already written must be
  rewritten to meet it, and Brian may have anything already written rewritten to comply;
  decisions.md is not rewritten.
- not taken: nothing already written is rewritten, the superseded clause, which forbids the
  compliance Brian wants for the migrated hypothesis files; a retrofit of every file, which he
  ruled against on the day the rule landed.

### Rule 10's quoted words include Brian's user blocks in the conversations corpus

- id: d-2026-09-12-27
- date: 2026-09-12
- supersedes: d-2026-09-12-20
- raised by: 041's trace, whose provenance runs through conversation 21, where block 641 is
  Brian's own plan and block 642 the assistant's answer, while the superseded entry named only
  his navigation note on a conversation block. Brian: "The 'my typing' should extend to
  conversations corpus, since those convos were about framework decisions."
- decision: A reasoning field is composed from the deliberation and the records of Brian's
  typing it draws on. What he typed is his own prose wherever it is recorded as his: the
  deliberation at hand, a user turn or `Typed:` line in a Claude Code transcript read from
  `codesessions.db` (the `code-sessions` corpus), a Keep note, a user block of a conversation in
  the `conversations` corpus, and his navigation note on a block. Used, it is verbatim inside
  quotation marks and cited to its source when it is not the deliberation at hand: session and
  seq, Keep line, conversation and block. Never quoted as his: a label he selected, a question
  put to him, an assistant turn or block, session text he approved, and text whose voice is not
  established as his, such as v1 archive and plan notes, which are corpus content cited by
  locator. Rule 8 is unchanged: quoting past typing makes it no evidence.
- not taken: his navigation notes alone from the conversations corpus, which leaves out the
  typing in the conversations where the framework's decisions were made; quoting only the
  present deliberation, the compression the migration trace found.

### The migrated hypothesis files are tightened by resuming each migration session under a resume file

- id: d-2026-09-12-28
- date: 2026-09-12
- raised by: the migration's result and the rulings of 2026-09-12 that post-date it. Brian: "I
  will have to go through all the hypotheses and tighten them, following the escalation register
  from hypothesis-migration-result.md. This session is only the 2nd of 50."; "I need to do the
  hypothesis rewrites now that we've tightened several things. It ran with migrate-hypothesis
  skill and the results came out in @docs/v3-framework-historical/hypothesis-migration-result.md .
  What are the possible paths forward and why?"; "What about continuing the sessions that already
  ran, since they have the full trace already?"; "MCP should be allowed."; "Please make a file
  that contains the instructions for resuming. I'll have the session read it before resuming.";
  "Let's run it on 041 which is what this conversation was originally about"; and, on 041's
  rewritten file, "This works".
- decision: The tightening pass continues the bootstrap of d-2026-09-12-5. Brian resumes each
  migration session with `claude --resume <session-id>`, and it reads
  `docs/v3-framework-historical/hypothesis-migration-resume.md` whole before anything else; where
  that file and the session's earlier instructions disagree, the file governs. The session is
  human-in-the-loop: every line of the migration result's escalation register naming its id is
  put to Brian and waited on. What binds it: rule 10 as d-2026-09-12-26 and -27 have it, the
  Origin's `reasoning` quoting his typing with its sources and the statement never quoted; rule
  9's reliance, nothing yet relying on a hypothesis file; slugs never changed, save the
  bootstrap's rename of d-2026-09-12-24 with Brian's approval; the question rules of
  d-2026-09-12-19 and -25, a question written only when he raises one; and no mint. The MCP
  server may be used for provenance, the conversations corpus and lineage above all, lifting the
  migration's ban. The session loads the `code-sessions` skill, rechecks every attribution at its
  source, composes the Origin under rule 10, checks the statement, the six grown statements
  diffed clause by clause and retired vocabulary removed, shows the whole file before writing it,
  and reports. It writes nothing in a record, touches no other hypothesis file, `INDEX.md` or
  `state.md`, deletes neither dropped file, and applies no gate of minting or iterating. The
  first run was 041's, in session `f858ef43`, approved by Brian. The resume file sits with the
  migration's briefing and result in the historical folder, and nothing of the pass enters an
  activity file or a schema (d-2026-09-07-27).
- not taken: a second unattended batch under a rewritten briefing, which puts the rewriting of
  attributions where Brian is absent, when fewer than half the first batch loaded the citation
  rule; fresh sessions per hypothesis, which walk again the traces the migration sessions hold;
  triaging the register first and touching only files with a ruling, which leaves paraphrasing
  Origins standing; re-minting every hypothesis from its head, which discards the verified work;
  the MCP ban kept, which would have left 041's provenance in conversation 21 unread.

### The resume pass is Brian's extraction of a hypothesis from malformed data, bound by the constitutional rules and the hypothesis-file schema alone

- id: d-2026-09-12-29
- date: 2026-09-12
- supersedes: d-2026-09-12-28
- raised by: a framing of how far a resumed session may rewrite a sound but loosely worded
  statement. Three texts answered three ways: d-2026-09-12-5 let no claim enter beyond the file
  and its sources; the migration briefing's § 6, written by the session that drafted it and not
  ruled, left such a statement "alone" under "Open — do not decide in a migration session"; and
  the resume file checked a statement against its sources and said nothing of rewording. Brian:
  "I think the reality is that these are all human in the loop sessions outside of standard
  operating procedure, with the full source chain brought up, so whatever I decide can be written
  with no restrictions except the constitutional rules in the skill.md". The archive then showed
  what the resumed sessions of 011, 034, 035 and 041 had done: in three of them the statement was
  not tightened but rebuilt, with Brian stating the claim after the chain was taken apart. Brian:
  "The point is, I dissected, iteratively what was in the file and chain. The hypothesis that
  came out is much simpler: Dreams and letters are distict from normal third-person narration.
  This is much more direct and decouples other stuff. So this activity is about
  extracting/transforming out of malformed data that ought not be there." He selected keeping the
  schema's content rules binding, minting in the pass under his approval, the day rebuilt as a
  rebuilt statement's Origin date, and an end when every id but the two drops has reported, with
  001's fresh session walking its chain first.
- decision: The pass continues the bootstrap of d-2026-09-12-5 as Brian's human-in-the-loop
  extraction: a resumed session takes its hypothesis file and the file's chain apart with him,
  removes what ought not be there, and writes the hypothesis he states or a draft he approves.
  Brian resumes each migration session with `claude --resume <session-id>`, and it reads
  `docs/v3-framework-historical/hypothesis-migration-resume.md` whole before anything else; where
  that file and the session's earlier instructions disagree, the file governs.

  What binds the session is the constitutional rules of SKILL.md and `hypothesis-file-schema`:
  its shape, which the hook holds, and its content rules, that a statement is a prediction and
  nothing beside it, testable by the referee's test of d-2026-09-12-2, with no reference to
  another hypothesis, no evidence claimed and no open question, and the list of what an Origin's
  `reasoning` may not hold. Nothing else binds it: not d-2026-09-12-5's limit on claims beyond
  the file and its sources, not the briefing's line leaving a loosely worded statement alone, and
  neither the gates of `minting-a-hypothesis` nor those of `iterating-a-statement`. The record
  stays empty. A new hypothesis found in the session may be minted there when Brian words or
  approves its statement. A statement rebuilt in the pass takes the day it was rebuilt as its
  Origin's `date`; an unchanged statement keeps its date. The MCP server may be used for
  provenance. Every line of the migration result's escalation register naming the session's id
  is put to Brian and waited on. A question is written only when he raises one, under
  d-2026-09-12-19 and -25. The session loads the `code-sessions` skill, rechecks every attribution
  at its source, composes the Origin under rule 10, shows the whole file before writing it, and
  reports; it touches no other hypothesis file or `state.md` and deletes neither dropped file.

  The pass ends when every id in the resume file's table but 017 and 027 has reported. 001, which
  has no migration session to resume, is taken by a fresh session that reads the migration
  briefing's procedure and 001's trajectory row, walks the chain to Brian's typed words, and then
  follows the resume file. 017 and 027 are resumed only if he chooses to revisit a drop, before
  the end. From the end, standard procedure governs the hypothesis files. The resume file sits
  with the briefing and the result in the historical folder, and nothing of the pass enters an
  activity file or a schema (d-2026-09-07-27).
- not taken: the resume file's limits as written, d-2026-09-12-5's limit on claims, the
  briefing's line leaving a loose statement alone and the ban on minting, which the rebuilt
  statements of 011, 035 and 041 had already gone past with Brian; lifting the schema's content
  rules as well, which Brian declined, since a statement the referee and the claiming calls read
  must still be a lone testable prediction; tightening against cutting as the frame, which named
  a narrower activity than the sessions performed; an end when the last session in the table
  reports, which the two drops and 001's missing session leave without an end; 001 left as
  migrated in the schema session; the original capture day as a rebuilt statement's date.

### A slug is a machine identifier the session creates, never changed; the resume pass may rename one to fit its statement

- id: d-2026-09-12-30
- date: 2026-09-12
- supersedes: d-2026-09-12-24
- raised by: 035's resumed session, which rebuilt its statement to "Dreams and letters are
  distinct from normal third-person narration" while its slug stayed `embedded-text-category`,
  and d-2026-09-12-24's exception, which covers only a claim never legitimate or a meaning
  inverted. Brian's words that founded the superseded entry stand: "Slug can be created by the
  session. It's a machine identifier that is stable, not something that has to be chosen by
  me."; and "Hypothesis slugs are stable. Migration is an exception because the earlier one
  wasn't legitimate and had an inverted meaning." Given the rebuilt statements of the pass, he
  selected renaming with his approval.
- decision: § Vocabulary defines slug: a machine identifier that reads in a sentence, lowercase
  `[a-z0-9-]+`, created by the session with what it names and never changed, each class's schema
  saying where it is unique. A hypothesis file's slug is a slug, so it is never changed; a
  declined candidate's heading and a claiming result cite it. The migration of the founding
  hypothesis files, with its resume pass (d-2026-09-12-29), is the one exception, a bootstrap:
  with Brian's approval, a session may rename a founding file's slug where the file's claim was
  never legitimate, where its meaning was inverted, as 041's was on 2026-09-12, or where its
  statement was rebuilt in the pass and the slug no longer names it. The exception ends with the
  pass and is carried here and in no activity file or schema (d-2026-09-07-27).
- not taken: d-2026-09-12-24's exception as written, which leaves a rebuilt statement under a
  slug naming the claim it replaced; renaming after the pass, when a declined candidate or a
  claiming result may cite the slug; a slug fixed only once something cites it, which leaves
  hypothesis slugs renamable outside the bootstrap.

### A unit deliberates until nothing gating remains, then writes every entry, then makes the edits

- id: d-2026-09-12-31
- date: 2026-09-12
- supersedes: d-2026-09-12-17
- raised by: this unit, where the session offered to draft the resume file and the skill edits
  with the entries to follow at the unit's end. Brian: "The decision entries should be written
  first, and then acted on. Where did this get reversed?" The trace: until 2026-09-11, revise read
  "each decision is written to `decisions.md` as it lands. The session applies each decision as a
  row edit and a prose edit together", and the session that ruled d-2026-09-11-5 wrote its
  entries before the schema edits (session `991a8481`, entries at seq 343 to 369, edits from
  376). On 2026-09-12 session `9730601e` applied edits at Brian's direction and wrote
  d-2026-09-12-1 to -6 after them; in session `f858ef43`, d-2026-09-12-12 rewrote revise to "The
  unit's decision entries are drafted together at its end", after which each ruling was applied
  as it landed and d-2026-09-12-14 to -28 were written after the edits. Brian: "So to be clear,
  what I want is pure framing, making decisions, and deliberation of emergent new things until
  there is nothing to deliberate, and then write all the decisions into decisions.md, and then
  do the file edits/code changes."; and "Not everything in the queue has to be deliberated. Only
  the first thing and anything stacked on top, plus any dependencies that gate work."
- decision: A unit opens with a queue of starting points, one line each, ordered widest-first by
  the session and seen by Brian before any is taken; he reorders, removes or adds. Taking a
  starting point opens a stack with it at the bottom; deliberating any decision on the stack
  pushes the decisions it surfaces on top, and the pop is from the top, so the starting point is
  settled last and may name the decisions settled above it. A unit deliberates only the starting
  point it takes, what is stacked on it, and the dependencies that gate the work it serves; the
  rest of the queue waits for a later unit, and after each stack empties the queue is presented
  again, re-ordered and re-formed, never as settled.

  A unit runs in three phases, in this order. First, deliberation: framing, Brian's rulings and
  the new items they surface, until nothing on the stack remains. Second, every entry of the
  unit is written to `decisions.md` at once, in the order the decisions were settled, and Brian
  scans the file; he approves the ruling, never the drafted sentence, and how many entries the
  rulings become and where the lines between them fall is the session's judgment under the
  one-decision rule. Third, and only then, the file edits and code changes that apply them. An
  entry he corrects on the scan is corrected within rule 9, while nothing that cannot be
  corrected relies on it, with the entries relying on it corrected in the same write. revise's
  row and § revise say so.
- not taken: applying each ruling as it lands and writing the entries afterwards, the practice
  of 2026-09-12, which put every edit ahead of the record that authorises it; writing entries at
  the end of each stack, which records rulings a later stack of the same unit may still overturn;
  deliberating the whole queue before anything is written, which holds work nothing on the queue
  gates.

### Rule 10's records of Brian's typing include the hypotheses dump, quoted with no pointer; no source citation is required

- id: d-2026-09-12-32
- date: 2026-09-12
- supersedes: d-2026-09-12-27
- raised by: the framing of a citation convention per corpus, which found d-2026-09-12-27's parts
  weak: subagent session prefixes collide in `codesessions.db`, a seq is recomputed at every
  ingest of a session, and "block 641" reads as a position within conversation 21 when it is the
  block's id. The citation clause itself was a session's wording, the tail of option A in session
  `f858ef43` at seq 616, which Brian selected ("Yes, option A", seq 617). Brian asked: "Should
  citations to code sessions be a skill wide convention? Are they always followable?" Then: "The
  'keep dump' has nothing to do with the Google Keep Corpus. keep dump is just a bunch of text
  dumped in a file. Not historical google keep notes."; "Keep dump can just be quoted as me
  without any pointers"; "To be clear, I wrote the notes in Google Keep on the side, then pasted
  them into a file. Then in one of the sessions making hypotheses I asked it to consolidate all
  the dumped text into hypotheses, and the conclusion recently is that it did a poor job, which
  is why the new instructions say to look at that file, which is in source material references
  as kypotheses-google-keep-dump.md. Those are my words and quotable. Google Keep as a corpus is
  distinct."; "I'm now thinking the citations are overkill and not needed."; and "governed
  artifact citations are good. Any further than that is not necessary."
- decision: A reasoning field is composed from the deliberation and the records of Brian's typing
  it draws on. What he typed is his own prose wherever it is recorded as his: the deliberation at
  hand; a user turn or `Typed:` line in a Claude Code transcript read from `codesessions.db` (the
  `code-sessions` corpus); `source_material_references/hypotheses-google-keep-dump.md`, the notes
  he wrote in Google Keep and pasted into one file, which is not the `google-keep` corpus; a user
  block of a conversation in the `conversations` corpus; and his navigation note on a block. Used,
  it is verbatim inside quotation marks, as typed, his own quotation marks becoming single ones.
  No pointer to its source is required, a verbatim quote being found in its record by searching
  for it; a session may name the occasion in its own words where that helps a reader. A reference
  to a governed artifact by its own token stays, under d-2026-09-12-19. Never quoted as his: a
  label he selected, a question put to him, an assistant turn or block, session text he approved,
  and text whose voice is not established as his, such as v1 archive and plan notes. The
  `google-keep` corpus is not named among the records of his typing. Rule 8 is unchanged: quoting
  past typing makes it no evidence. SKILL.md's rule 10 names the dump in place of a Keep note,
  gains the assistant turn or block, and loses its citation clause; citations already written
  stay, nothing already written having to be rewritten.
- not taken: a `cited as` line per corpus in CORPORA.md, which Brian selected and then withdrew
  before it was written, pointers being overkill once a quote can be searched for; the parts of
  d-2026-09-12-27 as written, weak in the three ways found; corpus content cited by locator, a
  pointer beyond a governed artifact's token; the `google-keep` corpus named beside the dump,
  which no session has needed to quote.

### The resumed sessions carry guards against the slips the first four runs made

- id: d-2026-09-12-33
- date: 2026-09-12
- raised by: the archive's record of the resumed runs of 011 (`aaa868a8`), 034 (`f32f0c91`), 035
  (`31b0a2ac`) and 041 (`f858ef43`), read at Brian's request: "Check code sessions for what the
  sessions so far have actually been doing, especially 31b0a2ac. Before continuing down this
  adjudication list". 011's session wrote its own recommended statement before Brian saw it (seq
  45), cut the Origin's coding-asset and non-coding-friction content that the briefing reserved to
  him under an instruction to "sanitize the origin of irrelevant parts" (seq 59 to 64), and later
  described as his own a wording it had drafted at seq 54 and he had restated at seq 55. 034's
  session edited its Origin before showing it (seq 98), quoted "atomsphere" as "atmosphere", and
  told him that keeping the statement bundled "costs nothing" because narrowing later would be
  "an ordinary iteration entry". 035's session read other hypothesis files as settled vocabulary
  and wrote into the Origin its own generalisation over five story analyses as what the analyses
  said. None of the four stated the referee test's result. Brian selected all four groups of
  guards.
- decision: The resume file gives every resumed session these instructions, beside the steps it
  already carries. The whole file is shown before any write, and an Edit is a write. A ruling
  that is Brian's is put to him before any edit would settle it, including an edit made under a
  general instruction such as cleaning up an Origin. A wording the session drafts in the
  deliberation and Brian then repeats is recorded as the session's wording he took up, as step 3
  already requires for an idea that began with an assistant. A generalisation over sources is
  labelled as the session's own and never given as what the sources said. Verbatim means as
  typed, misspellings included. The referee test is run on the statement and its result stated.
  Another hypothesis file is never used as settled vocabulary or as authority, every hypothesis in
  the set being untested. Any cost the session states for an option is checked against the method
  as written before Brian rules on it. `INDEX.md` stays untouched until after the pass, and a
  session that mints confirms the next unused id with Brian and names it in its report.
- not taken: the resume file's steps as they stood, under which each of these slips happened; a
  check over the Origins, which cannot tell a session's generalisation from a source's claim.

### The Origin's exclusions bar the session's own framing, never Brian's quoted words

- id: d-2026-09-12-34
- date: 2026-09-12
- supersedes: d-2026-09-12-4
- raised by: two resumed sessions reading the list of what `reasoning` may not hold two ways.
  041's Origin quotes Brian on a corpus reading, "v2 has almost no scene level notes yet, v1
  archive has a lot of instinctual, raw capture, scene level notes.", and on a test, "That now has
  to be *tested* from v1 historical data before carried forward"; 035's session dropped his typed
  "There are notes about dreams and letters (embedded texts) in v1 archive", his plan to verify
  against the fimfiction stories and then study the v1 archive, and his motivation for the planner,
  as barred. Brian asked "What is the origin exclusions you are talking about?", and with the list
  in front of him selected that the clause holds everywhere, having selected earlier that the list
  bars the session's framing only. The superseded entry's founding words stand: "what should be
  determined now is: what are the exact source materials to use and what is allowed or not allowed
  in the new hypothesis file's origin reason".
- decision: `reasoning` explains why the hypothesis exists and never extends what it asserts. It
  may not hold: a claim the statement does not carry; a corpus reading stated as established fact,
  that being evidence, which enters only as an entry in the record; a prospective testing method
  naming which study will test it; an asserted relationship to another live hypothesis, merge and
  split provenance excepted, which names the files this one came from; a confirm-or-refute
  condition; an assessment of the hypothesis's own testability or thinness, which the record
  answers by what it holds; a synthesis named as what raised it; and anything derivable. Each is a
  thing the field can be written to say that stops being true when a statement is reworded or the
  set changes around it. The list bars what the session writes in its own words and framing; it
  never bars Brian's typed words quoted under rule 10, a dated quote of what he typed staying true
  when the statement is reworded or the set changes. The clause holds for every writer of an
  Origin, `mint` and the resume pass alike, and sits in `hypothesis-file-schema` beneath the list.
  No check holds it, prose being what it governs.
- not taken: the list applied to Brian's quotes as well, the reading 035's session took, which
  removes his own observation, plan and motivation from the record of why a hypothesis exists;
  the clause for the resume pass only, which leaves a later mint reading the list the other way.

### implementation-candidates.md moves whole to docs/v3-framework-historical/

- id: d-2026-09-12-35
- date: 2026-09-12
- supersedes: d-2026-09-09-10
- raised by: the resumed migration session of 003 (`1f93533a`), whose rebuilt statement left two
  entries of `implementation-candidates.md` gated on a claim it no longer makes, as 014's rebuild
  had before it. Brian asked "Is @docs/v3-framework/implementation-candidates.md  a governed file?
  Is it planned to be or not?"; it was not, and no decision planned it. Then: "Where did that file
  come from? Should I make it governed, and then put the items in there? Or at least have
  instructions to check if the implementation candidate is already there". The archive showed it
  created on 2026-08-31 in answer to his "I need a home for proposed codebase changes. More may
  come up beyond what I stated now.", its name, its entry shape and its first contents an
  assistant's. Once he had ruled "No prior anchoring to the design. No home needed for past
  unfounded ideas that don't have evidence to back them.", he ruled on the file "It can go to
  historical" and "MOve the whole file", and selected superseding this entry whole over editing
  its list. On the folder's README, which no longer listed all it held: "Edit the readme to make
  it up to date for all files." The superseded entry's ruling stands, extended by this file.
- decision: `docs/v3-framework-historical/` holds every closed thing: the founding record and the
  spec pools; forward-plan-1, its ordering audit, forward-plan-2 and its handoff; the revision-2
  handoffs, omissions draft and rulings; the retroactive referee handoff; the 2026-09-05
  engineering handoff; process-map draft 1; VERSION-HISTORY draft 1; the WU1.1, WU1.2 and WU1.3
  syntheses; the WU1.4 execution plan and the WU2.15 plan; the consolidation plan and
  consolidation 1; the raw pipeline hypotheses; the 2026-09-06 code-session audit; the three
  retroactive explorations, `exploration-of-fimfiction-stories`, `-google-keep` and
  `-own-fiction`, whose registry rows leave, so that the registry is empty until the first study
  under the method; `docs/v3-framework-historical/skill-audits/` whole; and
  `implementation-candidates.md`, moved whole, its Tasks section and its Keep sidecar ingest entry
  with it and nothing carved out. Closed: never written again, governed by no checker, one README,
  which lists everything the folder holds.
  `docs/v3-framework/` holds only live artifacts: decisions.md, studies.md, the hypotheses, the
  questions, the studies, `methodology-revision-1.md`, and `WU1.4-v1-scene-instincts/`, an
  itemizer's output a study will use. Every path naming a moved file is edited in place,
  decisions.md's entries included, a path change being no change to a ruling, save the paths to
  `implementation-candidates.md`, which are removed rather than repointed, a repointed path still
  pointing at the prior Brian ruled out. `fanout/referee/codebook.md` and `fanout/smoke-test/` are
  deleted, both in git. No batch and no exploration is converted; what a retroactive artifact
  proposed enters a question list when Brian asks it.
- not taken: deleting the file with git holding it, which leaves the retired plans' mentions
  naming no file on disk; the Keep sidecar ingest's design carved into CORPORA.md, a tool design no
  study needs in a live fact file whose standing facts about `google-keep` are already there; the
  Tasks carved into a live file, a home the ruling removes; a new entry beside d-2026-09-09-10
  superseding nothing, two standing entries disagreeing about where the file is; d-2026-09-09-10
  edited in place under its path clause, a change of ruling presented as a path change.

### SKILL.md loses its pointer to implementation-candidates.md; revision 1's waits for the swap

- id: d-2026-09-12-36
- date: 2026-09-12
- raised by: the move of d-2026-09-12-35, after which § Provenance of the revision-2 SKILL.md still
  named the file among what the buildout produces, "(codebase changes gated on baselined
  hypotheses — they enter the ordinary feature process, never this skill)", and revision 1's
  `SKILL.md` and `process-map.md` named it as live. Brian selected removing the clause and leaving
  revision 1 for the swap.
- decision: The clause naming `implementation-candidates.md` leaves § Provenance of
  `.claude/skills/v3-buildout-2/SKILL.md`, its parenthetical with it; § What this skill does not
  govern and the terminus row already hold that planner changes are outside the skill. Whether a
  sentence takes its place is not decided here.

  Revision 1's `.claude/skills/v3-buildout/SKILL.md` and `process-map.md` stand as they are until
  the swap retires the folder. Two invariants collide there: that a pointer to a moved file does
  not stay behind, and the hook, which holds any edit under that folder's path to `skill-schema`, a
  shape revision 1 does not meet. The first yields, as it did for `spec-pools/` under
  d-2026-09-09-10, at the cost that a session loading `v3-buildout` before the swap still finds the
  file named as live.
- not taken: repointing the clause to the historical path, which lists a closed file among what
  the buildout produces and keeps the pointer; writing its replacement here, which decides the
  router's standing text inside a pointer; editing revision 1 now, which the hook holds to a schema
  the folder does not meet.

### VoiceMatch.cs's summary loses its sentence about a future DataOp

- id: d-2026-09-12-37
- date: 2026-09-12
- raised by: the move of d-2026-09-12-35, after which the summary comment in
  `StoryPlanner.Core/VoiceMatch.cs` still read "the same engine is what a future
  copy-paste-detection DataOp (implementation-candidates D19) would wrap with a write path", a
  pointer to the closed file and a design idea kept in code. Brian selected cutting the clause.
- decision: The summary of `VoiceText` keeps what the engine is and what it was built for; the
  clause about a future DataOp goes, and nothing names the historical path in its place. The change
  is to the comment only.
- not taken: repointing the parenthetical to the historical path, which keeps the prior in code;
  leaving the comment, a dangling pointer and a prior both.

### The router says the buildout keeps no ideas for the planner

- id: d-2026-09-12-38
- date: 2026-09-12
- raised by: with `implementation-candidates.md` closed, nothing in the skill says what becomes of
  an idea for the planner when one comes up. Sessions had written such ideas to that file at
  Brian's request, "Put a note about this in implementation candidates", and into hypothesis
  statements, the six prescriptions of d-2026-09-12-2; and decisions.md is read only in
  revising-the-method, so a ruling with no text in the skill reaches no session running an
  activity. Brian selected one sentence in the router.
- decision: § What this skill does not govern in the revision-2 SKILL.md carries one sentence: the
  buildout keeps no record of ideas for the planner, Brian's or a session's; the code changes of the
  terminus are reasoned from baselined hypotheses alone; and a factual premise an idea rests on may
  be offered for minting, whose prescription criterion already refuses the idea itself. The ruling
  fixes what the sentence says, not its wording.
- not taken: no new text, leaving the ruling where no activity reads it and a session free to
  invent a home for an idea; a sentence in `minting-a-hypothesis.md` only, which reaches only the
  sessions that mint while ideas come up in any.

### The resumed migration sessions let a planner design idea in a chain go

- id: d-2026-09-12-39
- date: 2026-09-12
- raised by: the same closing. The resume file's disposition for content written nowhere, a
  question about a corpus, a hypothesis to mint, or let go, never named `implementation-candidates.md`,
  yet 014's resumed session had used it as a home; the result's register still holds design ideas
  for the planner, 043's feasibility note on `SubjectRelation` and `NarrativePropertyValue` and
  004's question of Audit mode's cadence; and a resumed session reads SKILL.md's constitutional
  rules, not the router's sentence. Brian selected one line in the resume file's § What else holds.
- decision: `docs/v3-framework-historical/hypothesis-migration-resume.md` § What else holds carries
  one item: a design idea for the planner found in a hypothesis's chain is let go, no file carrying
  it, and a factual premise under it may be minted under the item on minting. The sessions already
  reported stand as they are, 014's gate moving with the file. The file's head loses its clause
  naming the day it was revised and the decisions it was revised under, Brian: "Just take out the
  part about revision day and decision."; the file is otherwise unchanged.
- not taken: nothing, since the three dispositions already leave the file out, which has Brian
  ruling again wherever a session proposes the file, as 014's did; the router's section added to
  the resumed sessions' reads, a wider read than one sentence needs.

### The buildout keeps no ideas for the planner before baselining

- id: d-2026-09-12-40
- date: 2026-09-12
- raised by: the question of whether and how to govern `implementation-candidates.md`, which
  Brian took into this unit: "This session is now revising the method for governing that file.";
  "First off, candidate collides, so we need a new term."; "Also, how should this even be tracked?
  None of these choices are settled because I haven't studied them. That's why they were thrown
  into hypotheses. But they are not the same as testable statements. What is actually the
  relationship between a hypothesis and an implementation?"; and "Or, should I not be thinking
  about the new story planner at all? Once hypotheses are baselined, then we reason about changes,
  with no prior pointing towards past code ideas?". The session's answer: a hypothesis claims what
  is, and a finding can come out against it; an implementation is a choice of what to build,
  resting on factual premises, which may be hypotheses, and on his goals, which no study settles; a
  gate names a hypothesis where the choice needed a premise, and goes quietly false when the
  statement is reworded, as 003's rebuild showed. The archive showed where the two had been joined:
  on 2026-08-31 an assistant argued that a feature proposal was a prediction of the same structure
  as a hypothesis, the route by which prescriptions entered the set. Against it stood his own
  requirement of that day, "I need a home for proposed codebase changes. More may come up beyond
  what I stated now." He ruled: "No prior anchoring to the design. No home needed for past unfounded
  ideas that don't have evidence to back them."
- decision: The buildout keeps no ideas for the planner, proposed codebase changes among them,
  before hypotheses are baselined, and changing the planner for v3 reasons from the baselined
  hypotheses and from no list of past code ideas. The requirement of 2026-08-31, a home for
  proposed codebase changes, yields. A factual premise an idea rests on is a hypothesis to mint and
  never a gate beside the idea. The terminus keeps owning no processes, as the founding record
  ruled. The entries above apply it: the file closes to the historical folder whole
  (d-2026-09-12-35), its pointers go (-36, -37), the router says so (-38), and the resumed migration
  sessions let such ideas go (-39). The new term for what the file held dissolves, no concept
  remaining to name; and 003's open disposition on its note-state aspiration is settled, let go
  with the file.
- not taken: an ungoverned stash of his ideas outside the method, which acts as a prior whatever
  its label and drifts, as this file did; the file governed inside the method, each entry naming
  its premises by id and a checker resolving its gates, an epistemic method tracking choices it
  cannot evidence against wordings that change, and a class across the terminus; an instruction to
  look for an existing entry before adding one, which keeps the home; a new term for the file's
  entries, which needs a concept to name.

### An itemizer may utilize other corpora to cut, label and fill a target corpus's items; the study stays that corpus's

- id: d-2026-09-13-1
- date: 2026-09-13
- raised by: hypothesis 022's resumed migration, where the session applied the resume file's line
  that a comparison across corpora is a hypothesis, never a question, to v1 archive text Brian
  recalled as copy-pasted from Gemini. Brian: "The cross-corpus = hypothesis rule seems wrong or
  incomplete. I already built the voice attribution tool, and it uses lineage and v1 archive. But
  it's a tool."; then "Investigate where the 'a comparison across corpora is a hypothesis, never a
  question' came from, and how does it clash with the fact that I built a deterministic tool that
  uses two corpora as inputs? It's an itemizer. Perhaps that itemizer can be used for questions in
  both files? I'm wondering if whatever came upstream of that rule neglected to account for what
  the voice attribution tool already did." The archive showed the line entering d-2026-09-12-22
  from an assistant's answer about comparing two verifications' results, in a deliberation that
  never mentioned the tool; d-2026-09-13-7 carries the trace. `tools/StoryPlanner.VoiceAttribution`,
  built 2026-09-02, reads the v1 archive, lineage and the dated v1 snapshots and writes one row per
  v1 note; on 2026-09-08 Brian had typed "attribution.csv is an itemizer which will be used." He
  ruled: "So this means itemizers can use other corpora to cut a target corpus's data into items.
  That still makes the study about the target corpus. This needs to supersede any decisions that
  contradict."; and, asked whether an item may carry a passage another corpus supplies, "Allowed".
- decision: An itemizer may read corpora besides the one it cuts and use what it reads to cut the
  target corpus's items, to label them, and to fill their bodies, a passage selected from another
  corpus included. The items stay the target corpus's, cut through its reader and located by its
  locators; the study, its question list and its findings are that corpus's, and a finding
  describes the target corpus's items. A question stays about one corpus's items, whatever else its
  itemizer read. What such an itemizer may read is d-2026-09-13-2, how its index records it -5, and
  where its limits are said -6. SKILL.md § Vocabulary's itemizer, preparing-to-verify-a-corpus
  § itemize and preparing-to-explore-a-corpus § assemble-exploration-batch carry the permission.
- not taken: the other corpus used only to cut and label, its text never reaching a call, which
  makes a call that needs the matched passage a study of the other corpus; a study of two corpora,
  with no one corpus for its id, its question list or its findings to name.

### An itemizer that cuts a CORPORA.md corpus reads corpus data only; a deterministic output over corpus data is corpus data

- id: d-2026-09-13-2
- date: 2026-09-13
- raised by: what an itemizer may read under d-2026-09-13-1. Brian: "The inputs to itemizers can
  only be corpus data." Then, on whether an itemizer may read `attribution.csv`, a committed output
  of VoiceAttribution, or must recompute from the corpora in its own run, which the session
  recommended since nothing ties a stored output to the corpus state it was computed from:
  "Deterministic output counts as corpus data". Drafting this entry, the session found three
  itemizers reading what is not corpus data, each writing an index whose `corpus` the index schema
  already carves out: the claiming itemizer over `findings.md` and the referee's over claiming
  results, findings and hypothesis statements, both `candidates`, and the audit's over the skill's
  prior text, `skill`. Offered the rule scoped to itemizers that cut a CORPORA.md corpus, Brian:
  "Go with A".
- decision: An itemizer whose index names a corpus in CORPORA.md reads corpus data and nothing
  else: a corpus in CORPORA.md, or the output of a deterministic tool over corpus data, such as
  `docs/v3-framework/WU1.4-v1-scene-instincts/attribution.csv`. A study's leads, results, tallies
  and findings are never its input, save as the items of `verified-findings`, which is a corpus.
  The itemizers of `candidates` and `skill` keep the inputs their activities give them. An output
  is read as it stands, and no state of it or of any corpus is recorded by hash.
- not taken: recomputing an output inside the itemizer's run, which keeps stored outputs out of the
  inputs at the cost of rerunning the tool per batch; `candidates` and `skill` counted as corpus
  data, which puts model results inside the term and undoes the line drawn at deterministic output;
  the rule general and the referee's item materialised by something other than an itemizer, which
  reworks surfacing-candidates and its tool to keep one sentence unscoped; a hash of each input,
  which Brian declined.

### "Never joined" bars MCP tools that merge corpora and identity mappings between them, never a study reading one corpus with another's data

- id: d-2026-09-13-3
- date: 2026-09-13
- raised by: d-2026-09-13-1 against CLAUDE.md § The corpora, "Six, and they are **never joined**",
  and the corpora skill, "never joined — not by the MCP tools, not by the app, not in analysis" and
  "No tool crosses them". Brian first scoped CLAUDE.md's v1/v2 line: "claude.md's statement is about
  the ids not matching. It is for using the mcp server, not for the study of the corpora."; then
  "Never joined is clearly wrong given the itemizer. Perhaps it means literal sqlite joins? Where did
  that statement come from?" The archive showed the phrase first in an assistant's turn of the MCP
  server's design, session `4a3d3d61`, after Brian typed "Treat Conversation Reader's contents and
  the 'story planner proper' as two bifurcated sources of data. Each should have their own tool calls
  that each invoke independently based on the prompt?" and "maybe tools should reflect that by
  having different tools for archive versus working?"; and again as "never joined by the tool" after
  he typed "V1 and V2 do not match each other on purpose. They have different organization
  principles entirely." and "Is there any other genuine use case for the mapping?" CLAUDE.md recorded
  it on 2026-07-28 among the MCP design rulings. "not in analysis", "No tool crosses them" and "never
  silently supported by another" entered on 2026-09-12, written by the session that cut CLAUDE.md into
  skills after he typed "corpora skill is fine." and "The newly proposed skills are good. Make a copy
  of the current claude.md for historical purposes and then make the changes directly"; the lines
  were not shown to him before they were written. CLAUDE.md had already sanctioned a join of source
  texts to the plan by name and part code. Offered the scope restored, Brian: "Yes".
- decision: That the corpora are never joined means two things and no more: the MCP server's tools
  serve each corpus through its own calls and never merge corpora in one result, and no identity
  mapping is kept between corpora's rows by id or by name, v1 and v2 above all. It is a rule for
  using the corpora through the MCP server and does not bar a study from reading one corpus with
  another's data, which d-2026-09-13-1 permits. A claim sourced from one corpus is never supported by
  another without saying so. CLAUDE.md § The corpora and the corpora skill's opening and its standing
  rule are brought back to that scope; CLAUDE.md's "v1 and v2 never join" and its MCP design ruling
  "corpora never joined" stand as written.
- not taken: the wording of 2026-09-12 kept, which the attribution tool of 2026-09-02 already
  contradicted; an exception for itemizers beside the unscoped rule, a named exemption where the
  rule was never meant to reach.

### Narrowing is a condition in the itemizer's code, stated in the index head

- id: d-2026-09-13-4
- date: 2026-09-13
- raised by: whether an itemizer may narrow a batch by a label another tool computed, against
  preparing-to-verify-a-corpus § itemize: "An itemizer never selects by judgment; an authored query
  in Brian's vocabulary is the only narrowing it may do". The line came from the closed founding
  record's ruling of 2026-09-05, "The work matrix retires, and with it the investigator and
  focused-reader cells", where itemizing is "always mechanical, a script or an authored query in
  Brian's vocabulary"; the vocabulary clause was the session's wording there. Brian: "What does
  narrowing mean?", then "Narrowing is going to be deterministic, not subjective. It's done by C#
  code. I don't see how judgement is relevant."
- decision: Narrowing, the choice of which of a corpus's items a batch holds, is a condition in the
  itemizer's code, deterministic as every tool is, and the index head states it. The itemize step's
  clauses on judgment and on Brian's vocabulary leave; that an itemizer carries no judgment stays
  where building-a-tool's row says it of every tool. This changes the founding record's ruling of
  2026-09-05 named above only in its words on queries, and supersedes no entry.
- not taken: a narrowing condition written in Brian's words, a tool's label names usable only once he
  adopts them, a vocabulary rule over a condition no judgment enters; "never selects by judgment"
  kept at the step, a guard building-a-tool already holds.

### The index head records what an itemizer utilized, in utilizes corpora and utilizes outputs, unhashed

- id: d-2026-09-13-5
- date: 2026-09-13
- raised by: what an index records under d-2026-09-13-1 and -2, its head naming one `corpus` and a
  `source hash` only when the corpus is one document. The session proposed a repeated line naming
  each input with a hash. Brian: "The corpora are frozen now."; then "Maybe they aren't totally
  frozen but I don't really care about hashing. reads can be added. Can it have multiple values?
  Also reads seems ambiguous, is there a better term?" `reads` is every Processes table's column, and
  `inputs`, `sources` and `uses` are taken in the skill too. Brian: "I was thinking 'utilizes'. Use
  two fields"; then "What would it be with one field?", and shown it, "Go with two".
- decision: The index head gains two optional keys after `corpus`. `utilizes corpora`, a list of
  enum of the ids in CORPORA.md, names the other corpora the itemizer read to cut, label or fill the
  items, and never repeats `corpus`. `utilizes outputs`, a list of line, names the deterministic
  outputs over corpus data it read, one repo-relative path per line. Neither carries a hash, and
  `source hash` stays as it is. `index.head` also fails when a `utilizes corpora` value is not an id
  in CORPORA.md or repeats `corpus`, and when a `utilizes outputs` path does not exist at the check.
  The schema's Example shows both keys. d-2026-09-09-4 still backs the check and is not superseded.
- not taken: one `utilizes` field of lines the checker sorts into id or path, whose corpus query is a
  continuation-line match; `reads`, `inputs`, `sources`, `uses`, `cut with` and `drawn from` as the
  name; a hash per input, or a state marker per corpus declared in CORPORA.md; a freeze of the
  corpora recorded in CORPORA.md, which Brian did not take up.

### The known limits of what an itemizer utilized are said where each reader looks

- id: d-2026-09-13-6
- date: 2026-09-13
- raised by: where the limits of a utilized corpus or output are disclosed, the index holding
  nothing written by hand. Brian: "What is a limit?" The session's answer: what the utilized data
  cannot show, known before a batch runs from how a corpus was captured or how a tool works, such as
  VoiceAttribution's six-word matching, which cannot see paraphrase, and lineage's capture gaps; a
  shortcoming being what the results reveal afterwards. The readers: the call, given only the
  directions and the item; the referee, given only a statement and a finding's text; the review and
  claiming, given the findings file. Brian: "Go with A".
- decision: A known limit of a utilized corpus or output is said where the reader it bears on looks.
  The directions' § What you are given tells the call what the utilized data in an item shows and
  what it does not. A finding states in its own words a limit that bounds it, its text being all the
  referee receives. A limit the results reveal that was not known is a shortcoming, `corpus` or
  `itemizer`. The standing caveats stay in CORPORA.md and in the tool's own documentation. No field or
  section is added; the directions schema's § What you are given and verifying-a-corpus's writing of
  a finding carry it.
- not taken: findings' § Method listing each utilized input's limits, which reaches neither the call
  nor the referee and restates CORPORA.md; an entry in CORPORA.md per deterministic output with its
  caveats, a schema change where v1-archive's read-by line already names the output and its tool.

### The verified findings are a corpus, explored only; a comparison of what two corpora's results show is not a question

- id: d-2026-09-13-7
- date: 2026-09-13
- supersedes: d-2026-09-12-22
- raised by: d-2026-09-13-1, which found d-2026-09-12-22's "a comparison across corpora is a
  hypothesis whose evidence arrives from each corpus through claiming, never a question" too wide.
  The archive traced it to session `f858ef43` on 2026-09-12. Brian typed "A cross corpus comparison
  question does not seem valid for the way I am using question for corpora which requires rigor. So
  is this a different kind of question?"; the assistant answered that such a comparison's answer is a
  relation between two verifications' results and offered, as its option 1, a hypothesis predicting
  the relation; he turned to what the verified artifacts were, and his rulings that followed were on
  the findings corpus alone. The entry carried option 1 in its decision; it was written on his "Yes
  write them" and shown to him afterwards as its title in a table. Offered his own typed position as
  the replacement, Brian: "Go wit hA".
- decision: A question is about one corpus's items; a comparison of what two corpora's results show
  is not a question, and this entry does not decide what it is. The standing findings of every
  verification are a corpus, `verified-findings`, in CORPORA.md: each finding entry one item,
  located by its `<study>/<slug>` heading in its study's `findings.md`, standing as of the
  itemizer's run. It is explored only, never verified, CORPORA.md's caveat giving the reason: a
  finding about findings could be promoted beside the finding it is about, counting one piece of
  evidence twice. Findings of different verifications rest on different items under different
  directions, so a pattern across them is a lead, never a joined claim, and a finding withdrawn after
  the run is caught at the lead review. preparing-to-explore-a-corpus has no clause for the verified
  artifacts of promoted verifications. The itemizer across studies is the first task of the first
  study that needs it. `registry.type` holds that a study's type is the one its id's prefix names,
  with no referee case; `registry.corpus` holds that the corpus cell is an id in CORPORA.md, or
  `skill` for an audit, with no `verified-artifacts` value. The clause of d-2026-09-09-12 on
  `hypothesis.evidence.citation` stays retired by d-2026-09-11-15.
- not taken: the old line bounded to results, "a hypothesis whose evidence arrives from each corpus
  through claiming", which keeps an assistant's option as a rule; the sentence dropped, leaving the
  comparison unmentioned beside a question defined by one corpus's items.

### The index head states narrowing in an optional narrowing line

- id: d-2026-09-13-8
- date: 2026-09-13
- raised by: applying d-2026-09-13-4 and -5, the session found no head key where narrowing could be
  stated: preparing-to-verify-a-corpus had said "the query is stated in the index's head" since
  2026-09-05, and d-2026-09-09-4's head never carried one. The two itemizers built so far narrow
  without stating it, each appending its invocation to the `itemizer` line. Offered a new key, the
  `itemizer` line, or no statement in the index with -4 corrected, Brian: "Go with A".
- decision: The index head gains an optional key, `narrowing`, a line, after `utilizes outputs` and
  before `locator notation`: in words, the condition in the itemizer's code that chooses which of
  the corpus's items the batch holds; absent when the itemizer cuts every item of its corpus.
  `index.head` holds its place in the key order, and d-2026-09-09-4 still backs the check.
- not taken: the condition in the `itemizer` line with the invocation, readable only as far as the
  arguments are; no statement in the index, the itemizer's commit standing for the condition, which
  corrects -4's "stated in the index head".

### Where claim meant an assertion, the skill folder uses the more specific word

- id: d-2026-09-13-9
- date: 2026-09-13
- raised by: a session's reports using the word two ways. Brian: "You're using 'claim' a lot. But
  this isn't in the Vocabulary section of the skill file. Are you using it to refer to multiple
  different things? Is there text that refers to the word claim?"; then "There are two things to
  sort out: which of the sense 2 usages can be replaced with an existing word that is more
  specific, and then, once that is done, whether the remaining usages should stay as ordinary and
  sense 1 gets a proper vocab defined, or if claim stays as the sense 1 and goes in as vocab."; and,
  on the rewords proposed, "I agree with all the rewords in Groups A and B." The trace: sense 1, the
  claiming batch's unverified (finding, hypothesis) pair, is Brian's typed word of 2026-09-10
  (d-2026-09-10-3); every use of `claim` as an ordinary assertion in the skill folder was a
  session's, the oldest from the epistemic section of 2026-08-31; neither sense is in § Vocabulary.
- decision: Where the skill folder uses `claim` for an assertion and an existing word names the
  thing, that word replaces it: § Vocabulary's *dispute* is raised against a finding, a result or a
  lead; *lead* is never a finding; rule 3's study is one whose leads or findings are about a corpus,
  or about the method; minting-a-hypothesis asks whether evidence could bear on the statement;
  iterating-a-statement's Preconditions speak of a sharper statement; exploring-a-corpus § Never
  reads says what a lead means for a hypothesis, the file's own wording in its procedure;
  findings-schema's Proposed questions are none of them a finding; verifying-a-corpus's
  write-findings puts under Proposed questions what the data raised that is not a finding; and
  CORPORA.md's `verified-findings` reads a pattern across findings as a lead and never a finding.

  Where no word names it, the noun goes: hypothesis-file-schema § Hypothesis carries no evidence for
  the statement; its list for § Origin bars nothing asserted that the statement does not carry;
  findings-schema's sections table has Proposed questions hold what no finding states; and the
  schema's Example has its iteration reason read what I actually hold. SKILL.md's opening sentence
  of the epistemic framework is d-2026-09-13-10's. decisions.md, CLAUDE.md and the skills outside
  this folder keep their uses, § Vocabulary governing the folder alone.
- not taken: the ordinary sense kept beside sense 1 in one folder, the shape d-2026-09-12-14
  declined as two senses of a defined word; `assertion` as the replacement noun, the word rule 10
  uses for a question, which would read every question as a hypothesis; rewording decisions.md,
  relied on under rule 9.

### The epistemic framework opens by saying recall and unverified output are not evidence

- id: d-2026-09-13-10
- date: 2026-09-13
- raised by: SKILL.md's opening `Every claim is a hypothesis with an evidence relationship`, the one
  ordinary use of `claim` no existing word replaced under d-2026-09-13-9. Brian: "Then the skill's
  use of claim at line 25 should be replaced with something related to recall and unverified
  leads. That's what the spirit is about. Past failure modes is treating my recall or an LLM's
  results as fact. The new epistemic framework makes a distinction."; and, shown a draft, "THe
  draft works". The distinction already stood in four places, none of them the opening: the bold
  sentence that only verified evidence moves a hypothesis, rule 2, rule 8 and the recall paragraph.
- decision: The first paragraph of SKILL.md § Epistemic framework (applied), after its Scope
  paragraph, reads:

  **Recall and unverified output are not evidence.** What Brian remembers, what a session or a
  model concluded without verification, and what an exploration saw as leads may raise a question
  or a hypothesis, and never move one: only verified evidence does. A hypothesis file holds a
  statement and its evidence relationship together: the statement, current and edited in place;
  the record, dated entries never edited, which *is* the evidence relationship rather than a
  history of it; and a status computed from the record.

  The bold sentence `Only verified evidence moves a hypothesis.` later in the section is deleted,
  the opening now saying it; the rest of its paragraph stands. The sentence draws its line by
  verification and not by who produced the text: the results of calls under calibrated directions
  are the verified layer (d-2026-09-09-15), so a model's output is not unverified as such.
  CLAUDE.md's matching line is unchanged.
- not taken: a neutral opening about the hypothesis file with the distinction left where it
  already stood, which leaves the spirit Brian named unstated at the head of the section; the recall
  paragraph moved up in place of the sentence, which puts the grounding procedure ahead of the
  statuses it is read against; the later bold sentence kept, a second statement of the opening's
  rule; `results` as the word for what a model produced, a word § Vocabulary gives to the verified
  layer.

### A statement about the data that has not been checked is unverified, not a hypothesis

- id: d-2026-09-13-11
- date: 2026-09-13
- raised by: the framing of d-2026-09-13-10, after which two sentences use the name of an artifact in
  its ordinary sense: the recall paragraph's `is a hypothesis about what the data says` and rule
  8's `is the hypothesis under test`. Brian typed neither: rule 8's phrase entered revision 1 on
  2026-09-03 in a session's wording and was carried verbatim, and the recall paragraph's came with
  the epistemic section of 2026-08-31. His own typing on the point: "If it's my recall, it's
  logically not evidence." (2026-08-31) and "my recall is only atmosphere, not grounding"
  (2026-09-06). Brian: "Option 1 works".
- decision: The recall paragraph of SKILL.md reads that a statement about the data from anyone,
  Brian from memory, a prior session, a memory file, this skill or any document, is unverified until
  it is checked at its source; the procedure after it is unchanged. `Unverified` is the word the
  opening of d-2026-09-13-10 and rule 3 already use. Rule 8's wording is d-2026-09-13-12's.
  d-2026-09-08-22's phrase `a hypothesis under rule 8` keeps its words, and its ruling, that a
  recall about the method is checked against the archive before a decision rests on it, stands.
- not taken: the ordinary use of `hypothesis` left beside the artifact of that name, the collision
  d-2026-09-13-9 removed for `claim`.

### Brian's recall is never evidence, and may raise a question or a hypothesis through minting

- id: d-2026-09-13-12
- date: 2026-09-13
- raised by: the opening of d-2026-09-13-10, under which recall may raise a question or a
  hypothesis, set against rule 8 and three activity files that route recall to a question alone.
  minting-a-hypothesis already admits Brian's own statement as what raises a hypothesis, and the
  resume pass rebuilt 041 from his recall with him, on "I'm leaning towards mostly scrap,
  actually."; on 2026-09-13 he typed of another chain "does this require a lean to be written as a
  hypothesis? Is there a lean in the chain?" Brian: "Option 1 works".
- decision: Rule 8 reads: **Never derive from recall.** Brian's recall is never evidence: it may
  raise a question, or a hypothesis through minting, and a study tests it. Minting's testability
  criterion is what keeps a recall that makes no prediction out of the set; no rule names a lean.
  Brought current in the same write: reviewing-leads' procedure, where his recall enters as a
  question with its provenance, or goes to minting when he states a prediction, and its § Never,
  which reads enters recall as a lead, a finding or evidence; reviewing-findings, where a recall the
  data does not bear out becomes a question, or a hypothesis through minting, if it is worth
  pursuing, and otherwise nothing, recall still entering as the check and never as the finding; and
  asking-a-question's § Never, which reads enters his recall as a finding or evidence.
- not taken: recall routed to a question alone, with d-2026-09-13-10's opening narrowed to match,
  which would send a hypothesis grounded in Brian's memory through a study's leads before it could
  be minted, against minting's own precondition and the rebuilt 041; a lean named in the rule, when
  the referee's test already refuses a recall that predicts nothing.

### Claim and candidate enter § Vocabulary as the funnel's words, and the sites misusing them follow

- id: d-2026-09-13-13
- date: 2026-09-13
- raised by: d-2026-09-10-3, which named the funnel's steps in Brian's words, "I like the idea of the
  NxM judgement creating 'claims' (means unverified), and only after the referee they become
  'candidates' (eligible to become evidence)", and entered neither word in § Vocabulary. With
  d-2026-09-13-9 and -10 applied, the folder uses `claim` in that sense alone. Three sites still used
  `candidate` for the unrefereed pair, which d-2026-09-10-3's not-taken list refused; two wrote
  claiming as the funnel does not work; and § Vocabulary's *finding* ends on a step named before
  claiming existed. Brian: "Yes, option 1 makes sense".
- decision: § Vocabulary gains two entries after *finding* and before *falsifier*:

  - **claim**: a (finding, hypothesis) pair a claiming call named, asserting only that the finding
    bears on the hypothesis, never in which direction; unverified; a claiming batch's results are
    its claims, and the referee judges each.
  - **candidate**: a claim the referee judged diagnostic, supporting or challenging, and so eligible
    for promotion; a claim judged non-diagnostic is refereed and is not a candidate.

  *finding* ends the only input of claiming. Brought to the definitions in the same write: rule 2,
  under which evidence enters a hypothesis record only from a verification's finding, claimed,
  judged a candidate by a fresh-context referee, and promoted in a session with Brian deciding each
  one; surfacing-candidates' assess-referee-items, which classifies the claim; iterating-a-statement's
  assess-reverify-items, which classifies the entry's finding against the proposed wording, its
  items being evidence entries re-judged and neither claims nor candidates; findings-schema's head,
  where a finding never names a hypothesis, which claiming does; and CORPORA.md's
  `verified-findings`, where a finding about findings could be promoted beside the finding it is
  about, as d-2026-09-13-7 words it. Nothing in the tool reads § Vocabulary, and no check is added
  or changed.
- not taken: sense 1 renamed, a reason that left with the last ordinary use, and a rename of
  `assemble-claim-batch`, `assess-claim-items`, the SurfacingItemizer's `claim` subcommand and the
  composed view's section against Brian's typed word; `claim` alone in § Vocabulary, leaving
  undefined the word that still carried the refused sense.

### Rule 3's guard on the buildout's own outputs names verified-findings

- id: d-2026-09-13-14
- date: 2026-09-13
- raised by: rule 3's `reads verified artifacts only`, a value d-2026-09-12-22 retired, its standing
  successor d-2026-09-13-7 making the standing findings of every verification a corpus,
  `verified-findings`, explored only; found while framing d-2026-09-13-9's reword of the same
  sentence. Brian: "Yes, option 1 makes sense".
- decision: Rule 3's second sentence reads: A study over the buildout's own outputs whose leads or
  findings are about a corpus reads `verified-findings` only; one whose leads or findings are about
  the method is not so guarded. d-2026-09-13-2, that an itemizer cutting a CORPORA.md corpus reads
  corpus data only, is unchanged and holds the same line at the itemizer.
- not taken: the retired value left in a constitutional rule, naming a corpus CORPORA.md does not
  carry.

### The three corpora entries of 2026-09-13 are premature, and each is superseded when the question it went ahead of is ruled

- id: d-2026-09-13-15
- date: 2026-09-13
- raised by: the corpora-schema review, opened with the brief that what an itemizer may take from
  another study is ruled only after the five conversions, and with the grounding that the entries
  of the day about corpora had been ruled ahead of what a corpus is. Brian: "Treat the 3 decisions
  on 09-13 about the corpora (2, 7, 10) as premature." Later entries lean on them: d-2026-09-13-1,
  -5, -6, -9, -11, -12, -13 and -14, SKILL.md's epistemic opening and rule 3; d-2026-09-12-22,
  superseded by -7, cannot be superseded again.
- decision: d-2026-09-13-2, -7 and -10 are premature: no deliberation rests on them. Each is
  superseded by the entry that rules the question it went ahead of, and not before: what CORPORA.md's
  `read by` names and what a corpus is, in the unit that takes corpora-schema, and what an itemizer
  may take from another study, after the five schema conversions. That entry restates what then
  stands and brings the dependents' text current in the same write. Until then the applied text
  stays as it is.
- not taken: superseding all three now, restating only what does not depend on the open questions,
  which sorts clauses ahead of the deliberation that decides them and gives each entry two
  successors; reverting their applied text and superseding them as void, which cannot revive
  d-2026-09-12-22 and reopens the collisions d-2026-09-13-9, -11, -12 and -13 closed.

### A call receives its directions and its item and nothing else: no MCP server, no tools

- id: d-2026-09-13-16
- date: 2026-09-13
- supersedes: d-2026-09-08-8
- raised by: the frame of what a call may receive while it runs, which found d-2026-09-08-8's "no
  MCP … a call has no third input" standing beside d-2026-09-08-15's MCP opt-in, and the runner
  skill claiming a call's stream is reconstructible from its hashes. The archive showed the call
  ruled while pathfinding was a session outside the runner, and MCP opt-in built on 2026-09-03 for
  the investigator cell, retired on 2026-09-05. Brian: "I believe all of those call rows should not
  have MCP except for the two explore ones"; then "since exploration may need searches and targeted
  sampling I think the MCP is valid"; then "Comparing two explorations is only valid if no MCP
  server is involved, then."; then, weighing a session outside the repo, "Losing the hook is
  unacceptable because that governs all.", and "It seems like search and sample is potentially not
  what I want anyway. That's how story planning goes, not studying historical corpus data."; and
  "No call through the runner reads through the MCP server." The `tools` line had been built on
  2026-09-03 for the Write tool, whose use ended when the runner began writing results. The
  superseded entry's founding words stand: "What is going into the input of a single claude code
  call, precisely, and delineated?", "The call only has directions and the item?" and "The effort
  level is also a knob alongside model".
- decision: A call is one execution of the Claude Code CLI in print mode with exactly these inputs:
  the body of the batch's directions, passed as the system prompt in place of Claude Code's own and
  hashed as the version every result cites; the item's text alone as the user message; and the
  batch's model and effort, both settings of the definition and recorded on every call. No call
  reads through the MCP server, and no call has tools: the runner passes `--tools ""` and
  `--strict-mcp-config` on every call and no `--mcp-config`. No transcript is persisted, and the
  launch folder is outside the repo. Whatever a study needs the agent to hold beyond the item is in
  the directions body or in the item, by the itemizer; a call has no third input. The answer is
  JSON, enforced by the CLI against the JSON Schema the directions declare; the runner lifts it from
  the result event, renders it as the Markdown result file named by the item's id, and keeps the
  event stream. The model writes no file and is told no id, no path and no marker. Rule 5 reads no
  tools and no MCP where it read an exact toolset and MCP unless opted in; the agent-runner skill's
  § The call and invariants lose their MCP and tools opt-ins, its `allowedTools` trap stays as
  history; `configs/storyplanner-mcp.json` is deleted; verifying-a-corpus' assess-items loses its
  "unless the definition opts in"; index-schema's Example notation reads as the archive database's
  note id. Comparing two explorations, valid only where no MCP server is involved, is valid for
  every exploration.
- not taken: MCP for exploration calls alone, which admits an unhashed input no committed file
  records, a second instruction text in the server's instructions, and reads beyond the study's
  corpus; exploration through the MCP server in a HITL session outside the repo, which loses the
  write hook on the governed files it writes and the codesessions archive; keeping the `tools` line,
  an unused channel for a third input; restricting it to tools that read nothing, a list of harness
  tool names kept for no consumer.

### An exploration runs only through the runner, its scale set by its questions: the whole corpus where it fits one call, or slices

- id: d-2026-09-13-17
- date: 2026-09-13
- supersedes: d-2026-09-08-15
- raised by: d-2026-09-13-16, which removed the MCP route the superseded entry gave a corpus read
  through the server, and the question whether a whole corpus preprocessed into one item could
  replace it; measured, working-plan, v1-archive and own-fiction fit one 1M call and conversations,
  lineage and fimfiction-stories do not. Brian: "Not confining exploration to slices."; then "There
  is no exploration outside the runner."; and "An exploration's scale is dependent on what the
  questions are. Whole corpus if it fits is one valid configuration, as a peer to slices." The
  superseded entry's founding words stand: "Pathfinding is full corpus one and done."; "There
  really should be a middle ground: outside the repo so there is no claude.md ... no memories ... no
  skills loaded. MCP server optional. And the system prompt for claude code which is about being an
  agentic coding harness should be overridden with the directions I want"; "Okay, make pathfinding a
  one-item batch. This is fine because we have the stream so I can see what it's doing without the
  interactive session."
- decision: There is no exploration outside the runner. An exploration's scale is set by its
  questions at the plan: one item that is the corpus whole, where the corpus or a stated narrowing of
  it fits one call, as a peer to slices. A whole-corpus item is written by an itemizer that renders
  or concatenates the corpus, never as what to read and how. Its directions are the study's; its
  result is leads as rendered, consolidated by `write-leads` as slice results are. No pilot, since
  the one item is the batch. A repeat under another model is a second study. No exploration runs in
  the repo. preparing-to-explore-a-corpus and exploring-a-corpus lose their MCP clauses.
- not taken: a HITL exploration with MCP outside the repo, which loses the hook; slices only, which
  drops the whole read where it fits; a whole-corpus item for every corpus, which three corpora
  exceed.

### A batch's definition carries no tools line and no mcp line

- id: d-2026-09-13-18
- date: 2026-09-13
- supersedes: d-2026-09-09-9
- raised by: d-2026-09-13-16, under which no call has tools or reads through the MCP server, and the
  superseded entry's field list carrying both. Brian, on the `tools` line: "Go with option 1". The
  superseded entry's founding words stand, Brian asking of the calibration line "How would the
  conditional calibration present be checked by the checker? Is it incoherent to let that stand?"
- decision: `schemas/definition-schema.md` is the schema of the class `definition`, in the four
  sections, its Example the checker's fixture. The Artifacts row is `definition` at
  `docs/v3-framework/<container>/<study>/batches/<batch>/definition.md`, mutation frozen. Its fields
  are `directions`, `kind`, `calibration`, `model` and `effort`: `kind`, present exactly when the
  directions have Classes, is `sample` or `full`; `calibration` is present exactly when `kind` is
  `full`. No `index` line, no `tools` line and no `mcp` line; a definition carrying either of the
  last two fails `definition.fields` as an unknown key. The checks: `definition.batch`,
  `definition.title`, `definition.fields`, `definition.directions`, `definition.calibration`,
  `definition.model`, `definition.frozen`. `DefinitionFile` and `Batch.BuildArgs` lose the two
  fields, with their tests.
- not taken: keeping either line as optional and unused; a calibration batch told apart by its
  missing line or its slug; an `index` line always holding `index.md`.

### write-leads consolidates results for Brian's attention; the review reads any result or the corpus on request

- id: d-2026-09-13-19
- date: 2026-09-13
- raised by: the review's reach into the slices, after d-2026-09-13-17. Brian: "Reviewing leads will
  take into account all slices."; then "The consolidated leads.md is so that my attention is drained
  from reading all the individual results, which are already lossy; they are not verified data. They
  are leads. Semantics about the corpus that was studied, which provide some more info about
  questions, but cannot verify any answer, which is verification's job. That's why write-leads exist.
  In the HITL review-leads activity I should be able to ask it to read the individual results and
  also reread the corpus itself."
- decision: `write-leads` stays: a session consolidates every result into `leads.md`, so that Brian
  reads leads and not every result. In `review-leads` the session reads any individual result, or
  the corpus itself, when Brian asks; reading a result traces what a slice reader wrote and checks
  nothing, the corpus being where a lead is checked. `review-leads` reads `results` and `index`
  beside what it read.
- not taken: consolidating in the review, reading every result there, which spends the attention
  the consolidation exists to save; no `leads.md`, the results being the leads, which leaves no
  citable lead and nowhere for the review's lines; `write-leads` consolidating and the review
  checking every result against it.

### A lead cites the slices it came from and names no position inside a slice

- id: d-2026-09-13-20
- date: 2026-09-13
- supersedes: d-2026-09-08-1
- raised by: tracing a consolidated lead back to its results under d-2026-09-13-19, which without a
  link is a search across every result. The superseded entry was ruled when an address meant a place
  a reader agent wrote inside what it read, before rendered per-slice results existed and before
  the arms retired. Brian: "So adding 'cites' would only refer to the results of each slice"; and
  "Ok, so in the case of exploration, the citation would be whole slice? No individual items. So
  this doesn't clash with the earlier intent … 'specific things' in that 9/8 exchange referred to an
  item like a single note or paragraph. Here, the 'cites' of leads.md refers to an item that is the
  whole slice of a corpus." The superseded entry's founding words stand: "I never liked locus. I
  don't think I made it. Where did it come from?" and "A lead comes out of an exploration study. Why
  would it even cite any specific locations?"
- decision: A lead is what was seen and, coarsely and in words, whatever it was seen in, at the
  grain the reader had, for instance a story, a subject or a stretch of the corpus. It never names a
  position inside a slice. Each consolidated lead carries `cites`, the item token
  `<study>/<batch>/<item>` of every slice whose result it came from, written by `write-leads` and
  resolved by the checker; a whole-corpus lead cites the corpus item. The words are kept beside the
  tokens: a slice's description says what the slice is, not where in it or across which slices the
  thing was seen. Precision lives on one row only, the index's, whose locator an itemizer, code
  carrying no judgment, records in the form its corpus's reader gives. A dispute over a lead traces
  through `cites` to the results, the item bodies and the corpus at the locators. The word locus
  stays out of the method. § Vocabulary's lead reads never names a position inside a slice where it
  read never checked at an address.
- not taken: no link, every trace a search; `cites` only for a batch of more than one item, two
  shapes of a lead; slices named in words, resolving to nothing; dropping the words, which leaves a
  whole-corpus lead no where; words only for one-item explorations.

### leads.md gains Shortcomings, written only by the review and routed as findings' are

- id: d-2026-09-13-21
- date: 2026-09-13
- raised by: a trace under d-2026-09-13-20 showing which layer of the instrument failed, with no home
  for it and no route to whoever fixes it. Brian: "Yes, we need a shortcomings section."; "Only the
  review writes them."; and, on the route, "Option 1 is good. I don't see the costs as costs because
  I don't want exploding scope here; only the things I raised should be acted on."
- decision: `leads.md` holds an optional `## Shortcomings` section, last in the file, appended only by
  `review-leads` when a reading at the review shows the study's own instrument at fault. `write-leads`
  writes none. The review says which activity a shortcoming sends the study back through and does
  not start it; what is carried to a later study travels only as a question Brian raises into the
  corpus's list. `explore-plan` gains no read of prior leads.
- not taken: `write-leads` also writing shortcomings visible in the results, a judgment beyond
  consolidating; the part named on a Correction line; no record of the failed layer;
  `explore-plan` and the directions author reading prior leads, a second carrier for re-entry beside
  the question list.

### A leads shortcoming opens with one of six parts, mirroring findings' meanings

- id: d-2026-09-13-22
- date: 2026-09-13
- raised by: the part words, and the two splits a draft of the schema left blurred. Brian: "Go with
  option 2."; "Yes, mirror findings"; "Mirror findings"; and "Go with 1".
- decision: A Shortcomings line is `- <part>: <what the review found>`, naming no token, `<part>` one
  of `slice`, `itemizer`, `directions`, `consolidation`, `execution`, `corpus`, held by
  `leads.shortcoming`. `slice` is the choice made at the plan of what one slice is, not suiting the
  reading, a whole corpus read as one item included, fixed by a new plan and a new exploration;
  `itemizer` is the tool not cutting what its stated grain, narrowing or locators say, fixed in its
  code; `directions` is the text wanting, where a reader following it faithfully still went wrong,
  fixed by a new version; `consolidation` is the consolidation not saying what the cited results
  say; `execution` is a reader not doing what clear directions asked, fixed by a second study under
  another model or effort; `corpus` is the corpus not being what CORPORA.md says. The schema's
  descriptions give examples that lead and do not exhaust. A question raised from a shortcoming names
  the lead that showed it by the lead's token.
- not taken: `item` for `slice`, findings' word exactly; `write-leads` for `consolidation`, a
  process id as a part; findings' six with `calibration`, which never applies; parts split by symptom,
  which stops naming where the fix lands; every reader fault under `directions`; the line ending with
  lead tokens, a second shape.

### A lead the review checked at the source takes a reread line per check; the Corrections section leaves

- id: d-2026-09-13-23
- date: 2026-09-13
- raised by: the Corrections section, a first pass from revision 1's "a correction is an appended
  dated section", naming no lead, sitting outside the entry, using rule 9's word, and standing in
  for a review marker. Brian: "The use of 'correction' in rule 9 is more general, so I think 'a lead
  that didn't hold' needs a new thing. So getting rid of Corrections section and replacing it with a
  withdrawn line for the leads is good"; "The withdrawn line has the new nuance. Perhaps withdrawn
  needs a better term, though?"; "I thought findings and leads are not doing the same thing?"; "Any
  lead the review checked at the source. These are only unverified leads anyway"; "It should be per
  check, so that means there can be multiple entries?"; and "Use reread".
- decision: A lead the review checked at the source takes `- reread: <date> <what the source
  showed>`, one line per check, zero or more per lead, in date order, after every field and followed
  by no keyed field line, written by `review-leads` and held by `leads.reread`. What the source showed
  includes whatever part of the lead holds; there is no other form for a lead that holds in part, and
  a lead is never edited. The `## Corrections` section leaves `leads.md`. Findings' and questions'
  `withdrawn` lines are untouched.
- not taken: `withdrawn`, which names the author retracting and hides the check; `not borne out`,
  said only of a failure; `updated`, the skill's word for an in-place edit; `reexplored`, which names
  a review's reading as an exploration against d-2026-09-13-17; a superseding lead written by the
  review, which authors a lead from the session's reading outside the runner; a renamed section
  naming each lead, which a reader citing a lead must scan.

### Challenge is the epistemic sense only; a dispute over a lead ends in a reread line

- id: d-2026-09-13-24
- date: 2026-09-13
- supersedes: d-2026-09-11-22
- raised by: d-2026-09-13-23, which replaced the Corrections section the superseded entry named as a
  disputed lead's outcome. The superseded entry's founding words stand: "keep challenge in just the
  sense of epistemically relative to hypothesis" and "Go with dispute."
- decision: A **challenge** is verified evidence, bound to the current wording, that disagrees with
  the statement: it is carried by an evidence entry tagged `challenging`, it puts the hypothesis in
  `challenged`, and only a reword clears it (d-2026-09-11-1). A **dispute** is what Brian raises in a
  review against a finding, a result or a lead, and the session's going back to the source to report
  what it shows. It has three targets: a finding, checked against the results, the tally and the
  items it cites, which is withdrawn or superseded; a result, checked at its item's locator, which
  becomes a shortcoming of the directions and never a correction; and a lead, checked against the
  corpus through its cited slices and where it says it was seen, which takes a reread line. A dispute
  is never evidence, never reaches a hypothesis, and is resolved by reading the source rather than by
  a reword. Both words are in § Vocabulary.
- not taken: `doubt`, `query`, `check`, `verify` and `flag`, declined in the superseded entry;
  leaving the lead's outcome at Corrections, a section that has left.

### Rule 9: an entry is corrected until relied on; a lead the review checks takes a reread line

- id: d-2026-09-13-25
- date: 2026-09-13
- supersedes: d-2026-09-12-16
- raised by: d-2026-09-13-23, the superseded entry naming "a disputed lead is a line under
  Corrections" among the changes that are never a correction. The superseded entry's founding words
  stand: "Rule 9 should not apply if we haven't even settled the framework yet."; "It should not have
  to do with git commit status. That is irregular and ungoverned."; "Reliance makes sense."; and
  "Conforming an entry to its class without changing what it records - this is fine. The chain for
  riliance works."
- decision: Rule 9 keeps appended and frozen artifacts unedited, save that an entry a session wrote
  may be corrected until it is relied on: conformed to its class without changing what it records.
  An entry is relied on once something that cannot be corrected relies on it, directly or through a
  chain, by a citation the checker resolves or by an entry placed after it in a record; what cannot
  be corrected is a succeeded version, a file the runner or an agent wrote, or an entry itself relied
  on. Until then it is corrected together with whatever relies on it, in one write. A change to what
  an entry records is never a correction: a finding that does not hold is withdrawn or superseded, a
  lead the review checks at the source takes a reread line, a reworded question is a new entry.
  revising-the-method's § Never reads that a fix to a file wrong under a right class is allowed
  within rule 9. The boundary is reliance and never the state of git.
- not taken: loosening rule 9 while revising the method; suspending it until the swap; the commit as
  the boundary; any citation as the boundary; the writing session; Brian's review of the entry.

### review-leads requires only that leads.md exists

- id: d-2026-09-13-26
- date: 2026-09-13
- raised by: review-leads' precondition, "a leads artifact whose `## Corrections` section is empty",
  testing a section d-2026-09-13-23 removed, and a repeatable review under d-2026-09-13-19 and -23.
  Brian: "Yes, the precondition is simply that leads.md exists."
- decision: The precondition of `review-leads` is that the study's `leads.md` exists. The review runs
  any number of times, each sitting leaving its reread lines, shortcomings and questions. Nothing
  records whether a leads file has been reviewed. The same gap on the findings side is not decided
  here.
- not taken: a reviewed state derived from reread lines, shortcomings or citing questions, which
  reads an empty sitting as none; an authored `reviewed` line, for a state no process reads.

### Leads are flat entries, organised by the order write-leads writes them in

- id: d-2026-09-13-27
- date: 2026-09-13
- raised by: leads-schema's "organised by what was observed — by subject, pattern, story" against a
  grammar whose sections hold flat entries and no level above them. Brian: "Go with option 1".
- decision: The Leads section holds flat `###` entries. `write-leads` writes leads about one subject,
  pattern or story next to one another; the instruction is in its procedure and not in the schema's
  Shape, and no check holds an order.
- not taken: a keyed field naming what a lead is about, free words that split a group on wording;
  no organising instruction, which leaves a consolidation in slice order.

### leads.md holds no Questions in view; the review brings them up from the directions

- id: d-2026-09-13-28
- date: 2026-09-13
- raised by: the Questions in view section, which since directions gained `questions` frontmatter
  copies it. Brian: "Option 1 is good, and have the reviewing-leads instructions say to bring these
  up".
- decision: `leads.md` has no Questions in view section. The questions an exploration read with are
  the frontmatter of the directions its batch ran under. `review-leads` brings them up at the review,
  reading `directions`; `write-leads` reads them there for its proposed questions.
- not taken: the section kept as a copy checked equal to the frontmatter, the policed mirror
  d-2026-09-11-12 declined; the section kept unchecked.

### leads.md holds no Method; the review brings up what oriented Brian from where it lives

- id: d-2026-09-13-29
- date: 2026-09-13
- raised by: the Method section, carried from revision 1's method section, whose content is the
  definition's, the index's and the calls', and whose "what was deliberately not measured" had no
  source, `explore-plan` recording the plan nowhere. Brian: "Method leaves entirely, but the
  review-leads instructions tell the session to bring what use to be here up in the session (since
  it's derived). Its purpose was to orient me. It doesn't have to live in the file. It lives in the
  session's instructions, so that session gets the info from the places where it does live on disk."
- decision: `leads.md` has no Method section. At the review the session brings up what oriented
  Brian from where each fact lives: the directions version with its frontmatter, § What you are given
  and § Never; the model and effort from the definition; the hash and the harness from `calls.md`;
  the itemizer and any narrowing from the index head; the malformed and missing results from the
  tally. `review-leads` reads `definition`, `calls` and `tally` beside what it reads.
- not taken: Method mirroring findings, holding what no batch file says, whose plan exclusions have no
  writer; each exclusion placed where it acts, with a caveat-only Method; a record field in the
  directions' frontmatter.

### A leads file's proposed questions mirror findings'

- id: d-2026-09-13-30
- date: 2026-09-13
- raised by: leads-schema's Proposed questions, a placeholder with no line form. Brian: "Mirror
  findings".
- decision: `## Proposed questions` is optional and holds one-line entries, `- <what the leads raise
  that no question asks>`, written by `write-leads`; none is a question until Brian raises it into the
  corpus's list, and none is a lead. A question raised from one names in its `raised by` the leads it
  came from by their tokens.
- not taken: each proposal naming its lead tokens on its line, a form apart from findings.

### write-leads runs the same way for one result or many

- id: d-2026-09-13-31
- date: 2026-09-13
- raised by: `write-leads`' "From one result the file is that result's leads as rendered", a
  transcription d-2026-09-10-2 gives a program, set against what the rulings of the unit give the
  step even for one result: order, slugs, cites, the coarse words and proposed questions. Brian:
  "Option 1".
- decision: `write-leads` runs the same procedure for a whole-corpus result and for many slices: it
  orders the leads, creates their slugs, writes each lead's words and `cites`, and writes the proposed
  questions. The sentence about one result leaves its procedure.
- not taken: a program rendering `leads.md` from one result, which can create no slug, order nothing,
  and makes a file the review appends to a generated one; no `leads.md` for one item.

### write-leads reads the definition, the directions, the index, the results and the question list

- id: d-2026-09-13-32
- date: 2026-09-13
- raised by: `write-leads`' row, set by the run of 2026-09-09 in one write, reading `tally` its
  procedure never used and missing `directions` it reads. Brian: "Go with option 1".
- decision: `write-leads` reads `definition directions index results question-list` and writes
  `leads`; its procedure is rewritten to the unit's rulings in the same write. `write-findings`' row
  is not decided here.
- not taken: keeping `tally`, whose malformed and missing sections are empty by construction when the
  step runs.

### A lead entry is lead, seen in and cites, then its reread lines

- id: d-2026-09-13-33
- date: 2026-09-13
- raised by: the keyed lines holding what d-2026-09-13-20 and -23 ruled a lead carries. Brian:
  "Option 1 is good"; and, on the draft, "seen in's value needs to be more open-ended instead of
  reading like a closed set."
- decision: A lead entry is headed `### <study>/<slug>`, the form of d-2026-09-07-30 and -31, to be
  reconsidered at the first real leads file. Its keyed lines, in order: `lead`, required, a block,
  what was seen; `seen in`, required, a line, coarsely and in words whatever it was seen in, its
  description giving examples and no closed set; `cites`, required, a list of line, the item tokens.
  Then its reread lines.
- not taken: one `lead` block carrying both what was seen and where, which holds the coarse where
  nowhere.

### leads.md is Leads, then optional Proposed questions, then optional Shortcomings

- id: d-2026-09-13-34
- date: 2026-09-13
- raised by: the sections left after d-2026-09-13-23, -28 and -29. Brian: "Option 1 is good".
- decision: The file is its title `# <study> — leads`, no head prose, then `## Leads`, required,
  entries, which may hold none; `## Proposed questions`, optional, one-line entries; and
  `## Shortcomings`, optional, one-line entries, last, its heading added with the first shortcoming.
- not taken: all three required with empty headings; Shortcomings before Proposed questions, which
  inserts a section mid-file.

### The leads class has six checks; leads.sections retires

- id: d-2026-09-13-35
- date: 2026-09-13
- supersedes: d-2026-09-08-19
- raised by: the leads Shape of d-2026-09-13-20 to -34, which the checker's two checks do not hold,
  and d-2026-09-07-11 requiring a decision to name every check added or changed. Brian: "Option 1
  works". The superseded entry's finding was the Bins section d-2026-09-08-14 removed.
- decision: The leads checker moves onto the engine and holds six declared checks: `leads.title`, the
  title is `# <study> — leads` with the folder's study, an exploration in the registry;
  `leads.shape`, the sections present, in order and holding what the table says; `leads.entry`, the
  heading `### <study>/<slug>` with the folder's study, a unique slug, and fields present, known, in
  order and typed, every line keyed or continuation; `leads.cites`, every token an item in the index
  of a batch under the study, the list not empty; `leads.reread`, each reread line a date then text,
  after every field, followed by no keyed field line and in date order; `leads.shortcoming`, a
  Shortcomings line's first word one of the six parts. `leads.sections` retires. No leads file
  exists, so the checker's predicted first run is zero failures, a statement about coverage.
- not taken: three checks with the cites, reread and shortcoming rules reported under two; the title
  without the registry clause.

### This unit closes on the leads first pass; its remaining stack opens the next unit

- id: d-2026-09-13-36
- date: 2026-09-13
- raised by: the unit taken to convert corpora-schema, whose stack grew through the runner and
  exploration into the leads schema, with three items left beneath: what CORPORA.md's `read by`
  names, what a corpus is, and corpora-schema itself; none bears on a ruling above. Brian: "There was
  a lot settled, and context is getting tight. Can we record decisions, act on them, and then return
  to the queue now, or is something critical blocking?", then "Yes, close the unit on leads.md
  deliberation first pass here."
- decision: For this unit only, its entries are written and its edits made before its stack is
  empty, at the close of the leads first pass. The three remaining items open the next unit, where
  they sit, with corpora-schema its starting point. d-2026-09-12-31 is unchanged.
- not taken: working the stack to empty first, which holds the rulings above behind items that
  cannot overturn them; writing this unit's entries at the end of each stack, the practice
  d-2026-09-12-31 declined, as a standing rule.

### Item names what one item is, and grain leaves the skill folder

- id: d-2026-09-13-37
- date: 2026-09-13
- raised by: the frame of what a study may take in, where the session used grain for two things:
  what one item is, and whether one set of criteria can judge items from different corpora.
  Brian: "First off, what do you mean by grain, exactly? Are there other already established
  terms or do I have to make one?" The archive showed the word a session's, first in
  d-2026-09-08-9's "What an item is, its grain, is decided at itemization", never typed by
  Brian in that sense. Brian: "Let's not use grain and stick to item for sense 1."
- decision: What one item is is named by item, and the word grain leaves the skill folder:
  preparing-to-verify-a-corpus § itemize, leads-schema's `seen in` and its `itemizer`
  shortcoming say what one item is where they said grain. Whether one set of criteria can judge
  items from different corpora gets no name here; it waits with the shape of a study over
  several corpora. decisions.md keeps its uses.
- not taken: grain kept as a defined synonym, a second word for one thing; a borrowed term,
  unit of analysis or recording unit, where item already names it; one word for both senses,
  the conflation that raised the entry.

### The buildout keeps one question list, questions.md, its entries headed questions/<slug>, naming no data set

- id: d-2026-09-13-38
- date: 2026-09-13
- supersedes: d-2026-09-07-31 d-2026-09-12-13 d-2026-09-12-19
- raised by: the tie of a question to one corpus, which the archive traced to a spec pool per
  corpus on 2026-09-03, the assistant's proposal Brian selected, scoping revision 1's
  per-corpus verification rounds; renamed question lists on 2026-09-05; carried into
  d-2026-09-07-23 undeliberated; and defended on 2026-09-08 only against a list per study.
  Brian had opened: "I'm now thinking corpora and batch inputs are no longer tied, and batches
  can come from itemizers which combine multiple corpora." Then: "If corpora no longer act as
  the scope key for studies, what are the options for how questions are tracked, and anything
  else?"; "One question list for the buildout does sound good. Please check against the history
  to see if this was ever rejected or became unwieldy.", the archive showing it never offered
  and never rejected; "Single list for questions works."; "questions/slug sounds good. Since
  explore plan and verify plan are to no longer be restricted to one corpus, but rather one
  itemizer (which may be built as part of the plan), I don't think any question entry has to
  name its data set as a checked field."; and "studies.md with the questions header is good.
  ... Checks changes are fine. state.md one questions section works. The first 3 wording
  changes are fine."
- decision: The buildout's questions are one list, `docs/v3-framework/questions.md` beside
  `studies.md`, the class `question-list`, append for every line, titled `# Questions`, entries
  only and no head prose. No field names the data sets a question concerns; what a question is
  about, one corpus's items or several, is not decided here.

  What stands of d-2026-09-07-31: an entry whose id is unique only within its file is headed by
  the token every other file cites, the file's own id, a slash, then the entry's slug; questions
  are now `### questions/<slug>`, and leads, proposals and candidates stay `### <study>/<slug>`;
  the checker holds that the prefix is the file's own id. Of d-2026-09-07-22 through it: the
  slug is lowercase `[a-z0-9-]+`, unique within the list, authored once and never changed, and
  cited as one token.

  What stands of d-2026-09-12-19, the file aside: a question entry has no `hypotheses` field. An
  entry is its heading, then `date`, exact and never earlier than the entry before; `raised by`,
  why the question exists, the occasion and what raised it and Brian's beliefs, recollections
  and motivation behind it, what raised it cited by its own token where it has one, as a trail a
  later session can follow when asked, and `recall` or `carried from the founding pool` where
  those raised it, under rule 10; `question`, a neutral assertion of what is asked; `suggested
  test`, present only when one suggests itself, a naive note of procedure that holds whatever
  the answer and never a belief about it, from Brian or the session, rule 10's quotation marks
  showing which, never a criterion. A sentence that would be wrong if the study came out a
  certain way is a belief and belongs in `raised by`. One exact line, and no field with a closed
  set of forms. `question.hypotheses` stays retired, a `hypotheses` line failing as an unknown key
  under `question.entry.fields`. A question reaches a hypothesis only through a finding, at
  claiming, blind and against the current set; no writer of the list reads a hypothesis to fill
  an entry, and state.md shows no hypotheses per question and no open questions per hypothesis.
  The hypothesis statements `review-leads` and `review-findings` read for their handoff to
  minting are untouched.

  What stands of d-2026-09-12-13: `asking-a-question` is an activity of the Router, its file
  `asking-a-question.md`, with one hitl process, `write-question`, which now reads and writes
  `question-list` and no longer reads `corpora`; it invokes no instrument; its state is `built`.
  It composes the entry from the deliberation under rule 10 and shows it to Brian before writing
  it. It enables preparing-to-explore-a-corpus and preparing-to-verify-a-corpus, nothing enabling
  it. It is the route for a question that arises where no activity's own processes cover the
  asking; a question raised inside an activity that writes questions stays that activity's, and
  reviewing-leads keeps `review-leads` and its question paragraph. The founding record's entry of
  2026-09-05, *Questions are written only by hitl processes*, is history as to the merge; its
  ruling that every writer of a question list is hitl stands, and is what
  `question-list.writer-not-hitl` holds. The id `asking-a-question` is Brian's. Its procedure
  loses reading CORPORA.md for the corpus Brian names and "Which corpus the question is about is
  his", and its § Never loses filing a question under a corpus he did not name.

  Checks: `question.title` fails when the title is not `# Questions`; `question.slug` when a
  heading is not `questions/<slug>` with a lowercase slug, or repeats a slug in the list;
  `findings.question` when a question token is not `questions/<slug>` naming an entry in the
  list, or is not in the frontmatter of the directions version the study's batches name, its
  clause on the study's corpus leaving; `directions.frontmatter` holds the new token form.
  `question.entry.fields`, `question.entry.date` and `question.withdrawn` stand. state.md's
  per-corpus tables become one `## Questions` section. explore-plan's precondition and row read
  that if no open question is in view for the plan, Brian's opening question is asked for and
  written first. Every other "the corpus's question list" in the activity files and schemas
  reads the question list. corpora-schema's line reads the corpus ids the registry uses.
  § Vocabulary's question is d-2026-09-13-56's, and rule 3's clause on questions leaves with its
  rewrite (d-2026-09-13-50).
- not taken: scoping questions by itemizer, unbuilt ones included, which fixes the item and the
  itemizer when a question is asked, before verify-plan fixes them, and leaves a question with no
  suitable itemizer homeless; per-corpus lists with a question on several filed in each, the
  duplication that raised the entry; a list per combination of corpora, which fixes the sets at
  asking time; a field naming a question's data sets, which no plan needs once plans take one
  itemizer; a bare slug as the token, which a grep cannot tell from same-named slugs elsewhere.

### The six per-corpus question lists become questions.md: ten entries carried, the two loop entries replaced by two questions of two corpora each, the old files deleted

- id: d-2026-09-13-39
- date: 2026-09-13
- raised by: d-2026-09-13-38, whose one list the twelve entries on disk had to enter, two pairs of
  them sharing a slug. The session read each pair as one question filed twice. Brian: "Merge the
  pairs and preserve the rest."; then, on the merged drafts, "Actually, these don't need to be
  merged. Here is what happened, and the questions require something more complex. First, one
  analysis-loop-pattern wanted to study the v1 loop, which requires data from v1-archive and
  lineage. Second analysis-loop-pattern wanted to study the v2 loop, which requires data from v2
  and conversations. So the merge was wrong. It's not that there was one question that had two
  corpora and was written twice. It's that there were two questions that needed 4 corpora, both
  questions were written but only with 1 of 2 corpora taken into account. For
  note-relationships-in-prose this is also two separate questions, one in v1-archive and one in
  v2."; then "The reworded question clause does not apply here because we are upending the shape.
  The two reworded questions and slugs are fine. ... Entry order is fine. The six old files can
  be deleted. The one citation can be modified. Leave decisions.md's historical wording."; and,
  shown the two new entries, "The two entries stand."
- decision: Executed once, from this entry. `questions.md` holds the entries in date order and,
  within a date, in the order of the old files' names and of each file's own entries:
  - the ten entries other than the two pairs, each carried verbatim under `questions/<slug>` with
    its old slug;
  - `v1-archive/note-relationships-in-prose` and `working-plan/note-relationships-in-prose`, two
    questions, carried verbatim as `questions/v1-archive-note-relationships-in-prose` and
    `questions/working-plan-note-relationships-in-prose`;
  - in the places of `conversations/analysis-loop-pattern` and `lineage/analysis-loop-pattern`,
    two new entries dated 2026-09-13 with their raised by as shown to Brian:
    `questions/v2-analysis-loop-pattern`, what the v2 loop of using the model and harness with
    the story plan data was, as the conversations corpus and the working plan record it; and
    `questions/v1-analysis-loop-pattern`, what the v1 loop was, as lineage and the v1 archive
    record it.

  The two old loop entries are neither carried nor withdrawn: the class changes shape, and no
  question is reworded within it. The one citation inside a carried entry, "Asked beside
  v1-archive/humor-warmth-prior-belief-setups." in `goals-no-track-type-names`, is conformed to
  `questions/humor-warmth-prior-belief-setups`, which changes nothing it records. The six files
  under `docs/v3-framework/questions/` are deleted, git holding them. decisions.md's citations of
  `v1-archive/scene-detail-scrap-rate` keep their words.
- not taken: merging each pair into one entry naming two corpora, drafted from a misreading of
  what the pairs asked; withdrawing the two loop entries beside their replacements, the rule for
  a reworded question applied to a class being replaced; prefixing every carried slug with its
  old corpus, which renames eight entries no collision touches; a historical copy of the six
  files, a second copy of entries carried verbatim.

### The audit retires entirely

- id: d-2026-09-13-40
- date: 2026-09-13
- supersedes: d-2026-09-08-7
- raised by: the frame of what a study may take in, which listed the skill's own text as an
  audit's input. Brian: "The skill's text as input for an audit should just be retired as a use
  case. No need for that complexity. the old skill is so outdated compared to the new setup that
  it would be noise."; "Frame Audit retirement first. check for consequences. Killing it should
  make the rest of the decisions easier by lowering scope."; and "Retire the audit entirely." The
  archive showed the audit built for one instance: on 2026-09-03 Brian typed "Then at the end, we
  have to compare the new from scratch skill with the old one (a separate agent, perhaps? the
  adversarial insight kicking in)", and on 2026-09-05 "Consider the agent runner for skill audit
  the 2nd level of linting after the validator tool."; its one downstream use, the omissions
  adjudication, was closed by d-2026-09-09-10.
- decision: The audit leaves the method. revising-the-method loses `assemble-audit-batch` and
  `assess-audit-items` with their sections, the second lint, the frame's item on audit units, the
  revision note's omissions list, and its § Never lines on swapping a rewrite in before both
  lints and on what the auditor is given; `revise` no longer reads `results` and `tally` or
  writes `studies` and `directions`. The validator alone gates a revision, a wholesale one
  included, and a rewrite built in a sibling folder is swapped in when it passes. SKILL.md's
  router description of revising-the-method, the `audit-of-<slug>` placeholder and § Vocabulary's
  audit leave; rule 5's list is d-2026-09-13-54's. study-registry-schema loses the `audit` type,
  the `audit-of-<slug>` id, the corpus value `skill`, its Example row and revising-the-method as a
  writer, `registry.type`, `registry.id` and `registry.corpus` changing with it. index-schema's
  `corpus` loses `skill` (d-2026-09-13-49), and directions-schema its audit clauses, with
  `directions.frontmatter` and `directions.sections`. In DocIntegrity, `audit` leaves
  SchemaCheckers' types and id handling, `skill` leaves `ExtraCorpora`, and StateBuilder's audit
  chain goes, with their tests. `tools/StoryPlanner.MarkdownItemizer`, used by nothing but the
  audit, is deleted with its solution entry, the test project's reference and
  `MarkdownItemizerTests`. The agent-runner skill loses its audit examples. The founding record's
  entry of 2026-09-05, *The supersession audit is the method's second lint*, is history, and
  `docs/v3-framework-historical/skill-audits/` stays closed.

  What stands of d-2026-09-08-7: a study is one directory, `docs/v3-framework/studies/<study>/`,
  named by its registry id; at its top its authored artifacts, its `directions-N.md` versions and
  their `calibration-<date>.md` files, and nothing else; under `batches/<batch>/` one folder per
  batch holding what that execution took in and produced. No itemizer sits in it, an itemizer
  being a tool under `tools/`. The registry `studies.md` sits beside `studies/`. The smoke test is
  the runner's own check; `fanout/` and the word work are gone; the launch folder outside the repo
  is unchanged. Which batches a study's folder holds is d-2026-09-13-41's.
- not taken: the audit retired and wholesale rewrites also forbidden once the swap lands, which
  rules out a future from-scratch revision; the audit skipped for this swap and its machinery
  kept for a rewrite no one has proposed.

### A study is not one set of directions over one itemizer: its folder holds its whole chain, and each batch sits under what it serves

- id: d-2026-09-13-41
- date: 2026-09-13
- supersedes: d-2026-09-08-3 d-2026-09-08-14
- raised by: telling apart the runner's two uses, which found § Vocabulary's study, one set of
  directions over one itemizer's items under one model, beside claiming and referee batches that
  sit in a verification's folder under other directions and another tool, and `definition.model`
  requiring one model across a study while a referee batch runs the model the referee's
  calibration measured. The session proposed a home for pipeline batches outside study folders.
  Brian: "To clarify, pipeline steps are one use case of the runner. The current discussion is
  about corpus data or derived corpus data itemizers going into the runner. Completely separate
  uses of agent runner which need to be distinguished."; "'Study batches' cannot work, because
  think about what a study has. One example is that it runs a few verification calibration
  samples, then it runs the full batch. Then that same study has to do a claiming batch, and then
  a referee batch."; and "To clarify, I do think the kinds of inputs allowed is birfurcated along
  the separation you listed, but that doesn't mean locations should vary. And yes, a study is not
  one set of directions and one itemizer. Re-verify batches are under iterations/."
- decision: A study is the life of one set of its own directions over the items of one itemizer
  under one model and effort, registered at Brian's go, one folder holding its batches; a
  verification's folder also holds the claiming and referee batches its chain runs under the
  pipeline directions. A different model, itemizer or set of its own directions is a different
  study, the directions the same file named by path, the calibration its own, the id's slug
  carrying the model when that is what differs. A batch sits under what it serves: a study's
  calibration samples, full batches, and claiming and referee batches under
  `studies/<study>/batches/`; an iteration's re-verify batches under `iterations/`; the pipeline
  directions' calibration samples in their own folders (d-2026-09-13-43). What a batch may take
  in follows its use (d-2026-09-13-50); where it sits does not. Which use a batch serves is read
  from the directions its definition names, and nothing records it. `definition.model` fails when
  another definition naming the same directions version names a different model. § Vocabulary's
  study and batch are d-2026-09-13-56's.

  What stands of d-2026-09-08-3: no folder level sits between a study and its batches; an itemizer
  is code with tests under `tools/`, outside the record, run once per batch, writing the batch's
  index and item bodies into the batch folder; a study holds no itemizer, and a second study
  cutting the same corpus the same way runs the same tool into its own folders; item bodies are
  regenerable and uncommitted; the index is committed and names the tool that wrote it
  (d-2026-09-13-49); each call hashes the item it received.

  What stands of d-2026-09-08-14: a comparison is always between studies; where two share their
  directions, the tally compares them item by item and the agreement per class is a finding of a
  verification, a candidate like any other; for two explorations, the review reads both leads
  artifacts and their differences are leads about the readers, never counted; no arm, no bin, no
  reason assigned at a join, no key, no blinding, the runner knowing nothing of a comparison; the
  arm-key row and schema, the leads schema's Bins section and the Never lines about opening the
  key stay gone, and § Vocabulary keeps comparison. Its third branch, a study of its own over the
  artifacts compared, and its sentence on rule 3's guard, are retired by d-2026-09-13-50.
- not taken: a home for pipeline batches outside study folders, which splits one verification
  across two folders against the registry's chain and Brian's "It is one step in verification.";
  pipeline batches left in the study folder with a line marking their use and a condition in
  every check that assumes a study batch; the uses told apart in words only, which leaves
  `definition.model` failing the first referee batch in a study that runs another model.

### The claiming directions follow the referee's: one version for the method, the hypothesis set carried in each item

- id: d-2026-09-13-42
- date: 2026-09-13
- supersedes: d-2026-09-10-6
- raised by: telling apart the runner's two uses, which found the claiming directions with no
  home. Brian: "The claiming directions should follow whatever is established for referee
  directions. What is that?" The referee's: one set for the method at one hash, rule 2 and
  d-2026-09-08-5; in a folder of its own belonging to no study; governed by reference
  (d-2026-09-08-25); reached by the path a definition names (d-2026-09-08-4); authored and
  calibrated once and revised only on a ruling; no questions in its frontmatter. d-2026-09-10-6 had
  carried the hypothesis set in the claiming directions' body, so that every mint or reword would
  be a new version. Offered the set moved into each item, Brian: "Go with option A".
- decision: The claiming directions are one set for the method, as the referee's are: versioned in
  their own folder (d-2026-09-13-55), belonging to no study, governed by reference, reached by the
  path a claiming batch's definition names, authored and calibrated in preparing-pipeline-directions
  (d-2026-09-13-46) and revised only as d-2026-09-13-45 says, their frontmatter carrying no
  questions. A claiming item holds the current hypothesis set, each statement, then one standing
  finding's text; the directions hold the criteria and what to produce, and a change to the
  hypothesis set changes no directions. Whether the CLI caches the set as a repeated opening of the
  message across calls is not established here.

  What stands of d-2026-09-10-6: `assess-claim-items` is a per-finding runner batch, one call per
  standing finding; a claiming call asserts only relevance, which hypotheses the finding bears on,
  never a direction, supporting or challenging being the referee's alone; claims are the claiming
  batch's own results on disk, not a separate authored artifact, and a candidate is a
  (finding-token, target) drawn from them; a claiming call's output is a bare list of the hypothesis
  file names the finding bears on, each a real hypothesis, the list possibly empty, with no reason.
  The model a claiming batch runs is the one its directions version's calibration measured, and the
  calibration is d-2026-09-13-44's, where that entry left the model a free parameter and the
  calibration to the first study.
- not taken: the hypothesis set kept in the directions with a new version at every change of the
  set, which departs from the referee's one version and yields a stream of uncalibrated versions;
  directions composed per batch from fixed criteria and the current set, a composed body whose hash
  `definition.calibration` would have to exempt; claiming directions per verification at the
  study's top, a second set of directions in one study.

### The referee's and claiming's folders hold their own calibration batches, laid out as a study's

- id: d-2026-09-13-43
- date: 2026-09-13
- supersedes: d-2026-09-08-5
- raised by: the list of every kind of batch, which found the referee's calibration sample spanning
  several hypotheses and verifications while d-2026-09-08-5 put its calibration batches under one
  verification's folder, and a calibration citing its batch as `<study>/<batch>`. d-2026-09-08-5 had
  declined "a standing folder that also holds batches, a second home for the definition class", and
  `iterations/` later became such a home through `<container>`. Brian: "Go with A".
- decision: The referee's directions and calibrations, and claiming's, each sit in a folder of their
  own laid out as a study's: the directions versions, their calibrations beside them, and `batches/`
  holding their calibration samples and nothing else. A calibration there cites its batch as
  `referee/<batch>` or `claiming/<batch>`. Their full batches sit under what they serve
  (d-2026-09-13-41). The folders' paths are d-2026-09-13-55's.

  What stands of d-2026-09-08-5: every candidate of every verification is judged under the same
  referee directions at the same hash, the referee being one part of the method that a verification
  runs and never authors, a sentence of the constitutional rules whose wording is now
  d-2026-09-13-52's; the referee's directions and calibrations belong to no study; they are authored
  and calibrated once, with a sample spanning several hypotheses and verifications, and again only
  when a ruling changes them, now in preparing-pipeline-directions (d-2026-09-13-46); refereeing is a
  step of each verification, its full batches under the verification's folder, their definitions
  naming the referee's directions by path; a calibration file cites the batch it came from; the
  referee's items are cut by a tool reading the claiming results, the findings and the targets'
  statements, a collator (d-2026-09-13-47); the `referee-<n>` rows, the registry's exceptions for
  them and state.md's referee chain stay gone.
- not taken: the calibration sample under one verification's folder with items drawn from others,
  a part of the method owned by an arbitrary study; a sample drawn from one verification alone,
  narrower than d-2026-09-08-5 asked and each recalibration owned by whichever verification prompted
  it.

### Claiming is calibrated per (finding, hypothesis) pair, recall deciding acceptance

- id: d-2026-09-13-44
- date: 2026-09-13
- raised by: d-2026-09-13-42, under which claiming's directions follow the referee's, which are
  calibrated, while claiming's answer is a bare list with no classes and d-2026-09-13-18 gave a
  definition `kind` and `calibration` only when the directions have Classes; rule 4 covered
  verifications and explorations and not claiming. A missed pair never reaches the referee or
  Brian; an extra pair costs one referee call. d-2026-09-10-6's "Calibration stays the first
  study's" was a session's sentence. Brian: "Option 1".
- decision: Claiming's directions are calibrated before their first batch. The calibration sample
  is drawn from standing findings across several verifications; Brian marks, blind, which
  hypotheses each sampled finding bears on; agreement is read per (finding, hypothesis) pair, recall
  and precision separately, recall deciding acceptance, the criteria tuned to over-include. A
  claiming directions version is calibrated for the model its calibration measured, and its batches
  run that model. calibration-schema says that claiming's agreement is read per pair. When a
  definition carries `kind` and `calibration` is d-2026-09-13-57's, and rule 4's wording
  d-2026-09-13-53's.
- not taken: claiming piloted as an exploration is, recall never measured and a missed pair
  invisible; claiming run with no gate, rule 4 leaving a set of directions it does not cover.

### A general ruling at promotion or baselining, or a change of model, sends the pipeline directions back for revision

- id: d-2026-09-13-45
- date: 2026-09-13
- raised by: d-2026-09-13-44, calibrating claiming with nothing that sends its directions back; only
  the referee had a route, promote's step 3, "A ruling that shows the referee wrong in general is
  what sends its directions back". Brian asked "Is this only about the bootstrapping of setting up
  the referee and claiming instructions for SOP?", the session showing the preparation recurring on
  each such ruling and on a change of model. A missed claim has no candidate to decline and no side
  door into the pipeline. Brian: "Go with A".
- decision: The referee's directions go back for revision on Brian's ruling at promotion that the
  referee is wrong in general, as promote's step 3 has it; claiming's on his ruling, at promotion or
  at baselining, that claiming misses pairs in general. A missed pair is written as no candidate: it
  enters the next claiming calibration's sample, and that calibration's Rulings record it with his
  reason. A change of model sends either set back for a new calibration. A hypothesis added to the
  set sends neither back. The route is preparing-pipeline-directions (d-2026-09-13-46), named in
  promote's step 3 and in baselining in prose and by no enables edge, the graph being acyclic.
- not taken: recalibrating claiming whenever the hypothesis set gains a hypothesis, a heavy blind
  task after every mint that blocks refereeing until it ends; a missed pair Brian notices written
  as a claim by hand, a side door into the evidence pipeline.

### The pipeline directions are prepared in their own activity, preparing-pipeline-directions, which opens with a plan

- id: d-2026-09-13-46
- date: 2026-09-13
- raised by: preparing-to-verify-a-corpus's preconditions carrying the referee's preparation as a
  clause, "the corpus is the candidates of every verification, the itemizer is the referee's
  materialising tool, the question is the pipeline's own …, and the activity stops after
  `calibrate`", a variant the founding record's referee-as-study of 2026-09-05 left behind when
  d-2026-09-08-5 ruled the referee no study; claiming now needing the same. Brian asked "You're
  saying copies of the authoring and calibration procedure can drift apart. But are there already
  different copies, or ought to be? Are they actually different or the same?"; the session found
  authoring and sampling different in substance, assessing and calibration's mechanics the same,
  and what must not drift already held by the schemas. Brian: "Go with option 1"; on the missing
  counterpart of `verify-plan`, "Go with A"; and on the rows and the id, "This is good. Go with
  preparing-pipeline-directions".
- decision: `preparing-pipeline-directions` is an activity of the Router that prepares the referee's
  and claiming's directions, one of the two per execution, as what started it names. Its processes,
  in order:
  - `plan-pipeline-directions`, hitl: the session presents what started it, the current version
    and its calibration; Brian fixes the model and effort, what the sample draws from with its
    strata and held-out split, and for a revision the rulings the new version must answer; each is
    recorded in the file it governs, and his approval is the go;
  - `collate-pipeline-sample`, session: the collator cuts the sample into the folder's `batches/`;
  - `author-pipeline-directions`, hitl: a new version written against the collated items;
  - `assemble-pipeline-sample-batch`, session: the definition under the draft version with the
    plan's model and effort, dry-run-batch, and execute-batch as the hand-off;
  - `assess-pipeline-sample-items`, agent: one call per sample item under the draft;
  - `calibrate-pipeline-directions`, hitl: Brian scores blind; agreement is read per class for the
    referee and per pair for claiming; a ruling that changes a criterion makes a new version; the
    calibration is accepted at a hash or not.

  It enables surfacing-candidates and iterating-a-statement. preparing-to-verify-a-corpus loses the
  referee clause and its enables edge to surfacing-candidates, which reviewing-findings still
  enables. Its first execution for each set is the bootstrap, begun at Brian's go; later ones are
  started as d-2026-09-13-45 says.
- not taken: the referee's clause kept in preparing-to-verify-a-corpus, reworded for both sets and
  named the deliberate exception, a case with no corpus, study, question or itemizer inside an
  activity named for a corpus; authoring and calibrating split into one activity both paths share,
  reshaping every study's frequent preparation for the rare one; planning folded into authoring, the
  sample's span fixed after its items are cut; the model and span fixed as defaults in the activity's
  text, against Brian's "just don't specify a model, let that be parameterizable for this process";
  `preparing-to-surface-candidates`, which names one of the two activities it enables;
  `preparing-the-referee-and-claiming`, stale if a third set appears.

### An itemizer cuts corpora; a collator collates a pipeline batch's items from the method's own files

- id: d-2026-09-13-47
- date: 2026-09-13
- raised by: § Vocabulary's itemizer, "code with tests under `tools/` that cuts a corpus into
  items", beside surfacing-candidates' "The claiming itemizer" and iterating-a-statement's "the
  reverify itemizer", one word for two tools whose inputs Brian had ruled apart. On 2026-09-09 he had
  typed "referee has to get an itemizer built, and its corpus is the governed files set that are
  relevant (candidates.md for a study, hypothesis files). An SOP itemizer has a corpus like
  v1-archive or fimfiction-stories." Offered one word with the split carried by the input rule,
  Brian: "Go with option 2, having the vocabulary reinforce the split"; and, of the words checked
  against the skill, assembler, composer, materializer, builder, binder and joiner each colliding,
  "Go with collator".
- decision: An itemizer is a tool that cuts corpora into items. A collator is a tool that collates a
  pipeline batch's items from the method's own files its activity names: a claiming item from the
  hypothesis set and a finding, a referee item from a statement and a finding, a re-verify item from
  a proposed wording and a frozen finding, and the calibration samples of these. Both are code with
  tests under `tools/`, run once per batch, writing the batch's index and item bodies, and both are
  built under building-a-tool, whose preconditions name a collator beside an itemizer. The two kinds
  of batch get no names of their own. surfacing-candidates, iterating-a-statement and
  preparing-pipeline-directions name the tool a collator, and the new activity's row is
  `collate-pipeline-sample`. § Vocabulary's entries are d-2026-09-13-56's, the project's name
  d-2026-09-13-48's, the index head d-2026-09-13-49's.
- not taken: one word for both tools with the split stated only in rule 3, which Brian declined for a
  vocabulary that shows it; names for the two kinds of batch, derivable from the directions a
  definition names; assembler, beside four `assemble-*-batch` processes; composer and materializer,
  beside `compose-candidates` and its materialised view; builder, beside building-a-tool; binder,
  beside an entry bound to a wording; joiner, beside the retired join and corpora never joined; a
  compound on item, which carries one of those collisions.

### The SurfacingItemizer project becomes StoryPlanner.PipelineCollator

- id: d-2026-09-13-48
- date: 2026-09-13
- raised by: d-2026-09-13-47, under which `tools/StoryPlanner.SurfacingItemizer` is a collator, its
  name also too narrow, its `reverify` subcommand serving iterating-a-statement and the tool now
  cutting preparing-pipeline-directions' samples. Brian: "PipelineCollator works".
- decision: The project is `tools/StoryPlanner.PipelineCollator`: its folder, project file, namespace
  and `Itemizers.cs` take collator names; the solution entry and the test project's reference
  follow; `SurfacingItemizerTests` becomes the collator's test file; the tool line of the indexes it
  writes names the new project. decisions.md's mention keeps its words.
- not taken: `StoryPlanner.Collator`, no longer unique if a second collator is built; keeping
  `SurfacingItemizer`, a name the vocabulary now contradicts.

### An index head names exactly one of itemizer or collator; corpus and utilizes corpora go only with itemizer

- id: d-2026-09-13-49
- date: 2026-09-13
- supersedes: d-2026-09-13-5 d-2026-09-13-8
- raised by: d-2026-09-13-47, under which the head's required `itemizer` would name a collator for
  every claiming, referee and re-verify batch, and its `corpus` would hold `candidates`, a value
  naming neither a corpus nor what the collator read, while a collator batch's locators already say
  what each item came from. The runner reads no head key. Brian: "Go with A".
- decision: An index head carries exactly one of `itemizer` or `collator`, a line naming the tool
  that wrote the index as `tools/StoryPlanner.<Name>` or a script's path, with its version or
  commit. `corpus`, an id in CORPORA.md, is present exactly when `itemizer` is; `candidates` and
  `skill` leave its values. `utilizes corpora`, optional and only beside `itemizer`, a list of ids in
  CORPORA.md, names the other corpora the itemizer read to cut, label or fill the items, never
  repeats `corpus`, and carries no hash. `narrowing`, optional for either tool, a line, states in
  words the condition in the tool's code that chooses which items the batch holds, absent when every
  item is taken. `locator notation` is required; `source hash` stays optional, when the source is one
  document. The key order is `itemizer` or `collator`, `corpus`, `utilizes corpora`, `narrowing`,
  `locator notation`, `source hash`; `utilizes outputs` leaves (d-2026-09-13-50). `index.head` fails
  when a key is missing, unknown or out of order; when both or neither of `itemizer` and `collator`
  are present; when `corpus` is present without `itemizer` or absent with it; when `corpus` or a
  `utilizes corpora` value is not an id in CORPORA.md, or `utilizes corpora` repeats `corpus`; and
  when `source hash` is present and not a SHA-256. index-schema's prose, its Example and the `index`
  Artifacts row say what retrieves an item from its source, the Example showing an itemizer head
  with no `utilizes outputs`. The runner's messages say the itemizer or collator regenerates missing
  bodies. d-2026-09-09-4 still backs `index.head` and is not superseded.

  What stands of d-2026-09-13-5 and d-2026-09-13-8 is restated above: `utilizes corpora` as the
  first had it, beside `itemizer` only, and `narrowing` as the second had it, for either tool.
- not taken: one neutral key naming either tool, a file that no longer shows the split Brian chose;
  two index classes at one path, which the path cannot tell apart, governance moving to the
  definition's reference.

### Rule 3 is the input rule: itemizers read corpora, collators read the method's own files

- id: d-2026-09-13-50
- date: 2026-09-13
- supersedes: d-2026-09-13-2 d-2026-09-13-6 d-2026-09-13-7 d-2026-09-13-14
- raised by: rule 3's clauses, whose revision-1 grounds, a verification pass per corpus and
  synthesis as their consumer, had gone. Brian: "Rule 3 seems like it is no longer reflective of the
  new setup. The clauses that back 'verification debt' need to be rewritten at some point in this
  deliberation, not patched."; on rewriting it as the input rule, "Option 1 works"; on the input
  rule's split, "Go with option 1"; and of the premature entries, "the premature decisions about the
  corpora were about these points, but they didn't get the proper deliberation since it was in the
  middle of (prematurely) looking at hypotheses." The archive showed exploration over findings the
  assistant's proposal of 2026-09-12 for synthesis's comparison across corpora, after Brian had typed
  "It would be reading and comparing verified artifacts, which is a subjective thing, not going over
  itemized units." Brian: "VoiceAttribution's development is incomplete. Once the skill is done, it
  ought to become a proper itemizer"; "Let's keep verification results as not valid for now."; "What
  if standing findings cannot be used as input to an itemizer, but their analytical content is used
  to inform the plan of what code to write for a new itemizer, which uses the corpora for input?";
  "Results and Two studies' outputs, compared - these seem like variations of standing findings, all
  pointing to the same potential actual use case of using these things to build a new itemizer.";
  "Deterministic outputs over corpus data like attribution.csv is not necessary. Remember that
  VoiceAttribution still has work to do to become a proper itemizer; no need to carve out a special
  case for where things stand now."; "Yes, but with the minor correction that findings, results,
  leads and analyses may inform the itemizer's logic too."; "What I meant by 'analyses' is the leads
  and findings which can inform itemizer design. Don't include that term because it's imprecise.";
  "Anything can inform an itemizer's code; it is not limited to just leads and findings or other
  outputs like evidence or results."; and, shown the draft, "That definition and new draft work."
- decision: Rule 3 reads:

  3. **Itemizers read corpora; collators read the method's own files.** An itemizer reads corpora,
     one or several, and derives what it needs from them in its own run; nothing else is its input,
     though anything may inform how its code is written. A collator reads only the method's own
     files its activity names.

  The clauses on verification debt, on the buildout's own outputs with their exemption for studies
  of the method, and "Questions flow freely between corpora; leads wait." leave; rule 2 already
  holds that exploration produces leads and never evidence. leads-schema loses "until a verification
  of its corpus has run". Retired with the old text: `verified-findings` as a corpus, and exploration
  over findings, its CORPORA.md section leaving; d-2026-09-08-14's third branch, a study of its own
  over compared outputs; and `utilizes outputs`, with the reading of a deterministic tool's stored
  output, `docs/v3-framework/WU1.4-v1-scene-instincts/attribution.csv` among them, CORPORA.md's
  v1-archive `read by` line dropping it. A study's results, tallies, leads and findings are never an
  itemizer's input. preparing-to-verify-a-corpus § itemize and § Never, preparing-to-explore-a-corpus's
  batch step, directions-schema's What you are given and verifying-a-corpus's writing of a finding
  lose their words on outputs. What d-2026-09-13-15 deferred, what an itemizer may take from another
  study, is nothing.

  What stands of d-2026-09-13-2: no state of a corpus is recorded by hash; the pipeline's tools read
  what their activities give them, now rule 3's collators.

  What stands of d-2026-09-13-6: a known limit of a utilized corpus is said where the reader it bears
  on looks; the directions' What you are given tells the call what the utilized data in an item shows
  and what it does not; a finding states in its own words a limit that bounds it, its text being all
  the referee receives; a limit the results reveal that was not known is a shortcoming, `corpus` or
  `itemizer`; the standing caveats stay in CORPORA.md and in the tool's own documentation; no field or
  section is added.

  What stands of d-2026-09-13-7: `registry.type` holds that a study's type is the one its id's prefix
  names, with no referee case; `registry.corpus` holds that the corpus cell is an id in CORPORA.md,
  `skill` leaving by d-2026-09-13-40; preparing-to-explore-a-corpus carries no clause for the verified
  artifacts of promoted verifications; the clause of d-2026-09-09-12 on `hypothesis.evidence.citation`
  stays retired by d-2026-09-11-15. Its ruling that a question is about one corpus's items does not
  stand as the list's scope (d-2026-09-13-38), and what a question is about is not decided here. What
  a comparison of what two corpora's results show is, it left undecided, and so does this entry
  (d-2026-09-13-61).

  Of d-2026-09-13-14 nothing stands beyond rule 3's text. d-2026-09-13-1 stands; its references to
  -2, -5 and -6 read through their successors. Of the other entries d-2026-09-13-15 named,
  d-2026-09-13-9, -11, -12 and -13 and SKILL.md's epistemic opening need no change;
  d-2026-09-13-10 is d-2026-09-13-59's.
- not taken: d-2026-09-13-2 restated as it stood, a named exemption predating the single list and
  the ruling on results; everything an itemizer reads counted a corpus, `candidates`, `skill` and
  `verified-findings` each an entry with a caveat on who may read it, which puts the buildout's own
  outputs back under the word; exploration over findings kept, explored only; results as an
  itemizer's input, which Brian declined for now; verification debt counted per question, which
  nothing reads, state.md already deriving what covers and answers each question; rule 3 retired
  and its number left vacant, the guard below the constitutional rules; "analyses" in the rule,
  which Brian called imprecise; a list of what may inform an itemizer, which Brian ruled
  unrestricted.

### A corpus is primary-source data; a lossy summary built on it is not corpus data

- id: d-2026-09-13-51
- date: 2026-09-13
- raised by: the word corpus, which § Vocabulary never defined and the skill folder used in several
  roles. Asked whether to retire it, the session found it first a session's word, on 2026-07-01, and
  typed by Brian since 2026-07-16. Brian: "I'm thinking C, with corpus defined as a set of data
  available for itemizers. Then all the other usages can be made more precise. But is this the way
  it is used in the majority of usages today?"; counted, 135 of the folder's 310 uses named data an
  itemizer reads and 124 used a corpus as the owner or namespace of questions and studies, the rest
  file names and incidental. On the definition, "A works". Tested against CORPORA.md's entries,
  Brian: "The historical analyses of fimfiction data are not corpus data."; "own-fiction's analyses
  are not corpus data. They can not be used for itemizers because they are lossy summaries,
  equivalent to leads but without the rigor. The same applies to Lineage's weekly summaries. Only
  lineage's actual conversational text is corpus data, not the lossy summaries built on top of
  them."; and "You can add the principle to an updated definition of corpora. I'm thinking the
  distinction is primary source or not, but not sure if that fully overlaps with the principle.
  Google Doc revision history is in. arc summaries are out. block summaries can be excluded; they
  are currently not populated at all. Fimfiction's supplementary material is a primary source from
  me. Perhaps it comes out to: my words, or an AI's direct response to my words. Not an AI lossy
  summary." The session found that formulation leaving out the fimfiction stories, other authors'
  fiction, and primary source holding. Brian: "That definition and new draft work."
- decision: The word corpus stays in the skill folder, defined in § Vocabulary:

  - **corpus**: a set of primary-source data the buildout studies, text as it was written where it
    was written: Brian's own, another author's, or an AI's reply in the exchange it answered; listed
    by id in CORPORA.md with where it lives and how it is read; the only input of an itemizer. Never
    a lossy summary or report made afterwards from other text, and never a file of the method's
    other artifact classes.

  A lossless view computed from a corpus, such as the Google Doc layer's line diffs beside its
  snapshots, is how a corpus is read and is not a lossy summary. CORPORA.md is brought to the
  definition: fimfiction-stories names the stories and Brian's supplementary material, the v4
  per-story analyses and the meta-analysis reports leaving; own-fiction's self-diagnostic analyses
  leave; lineage's Gemini weekly reports leave and its Google Doc revision history stays;
  conversations' arc summaries and block summaries are named as not corpus data; google-keep's Claude
  Code analysis artifacts are named as not corpus data. The `corpus` Artifacts row's description
  follows. Uses of corpus as the owner or namespace of questions and studies are reworded where
  their own decisions land; the planner-wide sense in CLAUDE.md and the `corpora` skill is untouched.
  Whether code-sessions' compaction summaries and subagent reports are corpus data, and what
  `read by` names, are not decided here; they sit with the corpora-schema conversion.
- not taken: the word retired in the skill and the set of data named anew, which rewrites the 178
  uses that already say what Brian means; his words as typed, "a set of data available for
  itemizers", availability unanchored so that any readable result or finding satisfies them; a
  corpus defined as an entry of CORPORA.md, circular; "my words, or an AI's direct response to my
  words", which leaves out the fimfiction stories.

### Rule 2 names claiming beside the referee

- id: d-2026-09-13-52
- date: 2026-09-13
- raised by: d-2026-09-13-42, claiming's directions following the referee's, against rule 2's last
  sentence naming the referee alone. Shown the draft, Brian: "Looks good, next item".
- decision: Rule 2's last sentence reads: Every finding of every verification is claimed under the
  same claiming directions, and every claim is judged under the same referee directions, each at the
  same hash: both are parts of the method, and a verification runs them and never authors them. The
  rest of rule 2 is unchanged.
- not taken: the sentence left naming the referee alone, claiming's one set held only in activity
  text.

### Rule 4 covers the pipeline directions and says what revises them

- id: d-2026-09-13-53
- date: 2026-09-13
- raised by: d-2026-09-13-44 and -45, claiming calibrated and both sets of pipeline directions revised
  on a ruling or a change of model, against rule 4 covering verifications and explorations, its "the
  fix is a new version through preparing" naming one preparing activity where there are now two.
  Shown the draft, Brian: "Looks good, next item".
- decision: Rule 4 reads: **Directions are calibrated or piloted before their first batch.** A
  verification's directions, and the method's pipeline directions for claiming and the referee, are
  authored in a session with Brian, against real items, calibrated against his blind verdicts before
  their first full batch, versioned by number and by the hash of the body every call cites; a revision
  is a new version and a new batch, never a re-label. An exploration's directions are piloted on one
  item before the rest run. A verification whose results show its directions wanting records it as a
  shortcoming in its findings; the question is Brian's to raise, in reviewing-findings, and the fix is
  a new version through preparing-to-verify-a-corpus. Pipeline directions are revised only through
  preparing-pipeline-directions, on Brian's ruling at promotion or baselining that they are wrong in
  general, or on a change of model.
- not taken: the pipeline directions' calibration and revision left to activity text beside a rule
  naming only verifications and explorations.

### Rule 5 lists a claiming call where it listed an auditor

- id: d-2026-09-13-54
- date: 2026-09-13
- raised by: d-2026-09-13-40 retiring the audit and d-2026-09-13-42 making claiming a set of pipeline
  directions, against rule 5's list of `agent` processes. Shown the draft, Brian: "Looks good, next
  item".
- decision: Rule 5's list reads a reader of one item, a classifier, a claiming call, the referee, the
  calibration sample; the rest of rule 5 is unchanged.
- not taken: the list left naming an auditor the method no longer has.

### The pipeline directions live under docs/v3-framework/pipeline/, and a batch path's second slot is home

- id: d-2026-09-13-55
- date: 2026-09-13
- raised by: d-2026-09-13-43, the referee's and claiming's folders holding calibration batches, against
  the batch classes' path `docs/v3-framework/<container>/<study>/batches/<batch>/`, one level deeper
  than `referee/batches/`; the referee folder, empty, named in four texts and at three sites in the
  code, BatchFiles' `DirectionsFile` reading the referee kind from the folder's name. Brian: "Go with
  A".
- decision: The referee's directions, calibrations and calibration batches live in
  `docs/v3-framework/pipeline/referee/`, claiming's in `docs/v3-framework/pipeline/claiming/`.
  `<container>` is `studies`, `iterations` or `pipeline`, and the slot after it in the six batch
  classes' paths is `<home>`: a study's id, an iteration's folder, `referee` or `claiming`; `<study>`
  stays wherever it means a study. `definition.batch` numbers a batch within its home. SKILL.md,
  directions-schema, calibration-schema and study-registry-schema name the new folders, and the
  `directions` and `calibration` Artifacts rows the pipeline folders; `Compose.RefereeFolder`,
  StateBuilder's referee path and `DirectionsFile`'s kind detection follow, the last also reading the
  claiming kind; surfacing-candidates' and iterating-a-statement's preconditions name the referee's
  new folder.
- not taken: `referee/` and `claiming/` as siblings under `docs/v3-framework/`, one level shallower
  than every other batch and a second path pattern per class; claiming nested in the referee's
  folder, one peer under the other; `<owner>` for the slot, the planner's word for polymorphic
  ownership.

### § Vocabulary is brought to this unit's rulings

- id: d-2026-09-13-56
- date: 2026-09-13
- raised by: the unit's rulings on questions, studies, the pipeline directions, the input rule, the
  audit and corpus, each leaving an entry of § Vocabulary wrong or missing. Shown ten drafts, and two
  options for verification and exploration, whose "over a corpus's items" and "a reading of a corpus"
  rule 3 now contradicts, Brian: "The ten drafts are good, and go with 1".
- decision: § Vocabulary reads:
  - **study**: the life of one set of its own directions over the items of one itemizer under one
    model and effort, registered at Brian's go, one folder holding its batches, a verification's
    holding also the claiming and referee batches its chain runs under the pipeline directions; a
    different model, itemizer or set of its own directions is a different study.
  - **verification**: a study of the verification type, one execution of calibrated directions over
    its itemizer's items, its findings the only source of candidates.
  - **exploration**: a study of the exploration type, a discovery-first reading of its itemizer's
    items under piloted directions; its output is a leads artifact.
  - **batch**: one execution of a set of items under one definition; a study's batches are its
    calibration samples, its full batches and, for a verification, its claiming and referee
    batches; the pipeline directions' calibration samples sit in their own folders, and an
    iteration's re-verify batches under `iterations/`.
  - **item**: the one thing a call judges, cut from corpora by an itemizer or collated from the
    method's own files by a collator; a slice is an exploration's item, a partition of a corpus; one
    item may be a corpus whole.
  - **index**: a batch's list of items, written by its itemizer or collator, with a locator per item.
  - **itemizer**: code with tests under `tools/` that cuts corpora into items, run once per batch into
    the batch's folder; it reads corpora and nothing else, and may read several to cut, label and
    fill the items, which stay the cut corpus's.
  - **collator**: code with tests under `tools/` that collates a pipeline batch's items from the
    method's own files its activity names (findings, claiming results, statements, evidence entries),
    run once per batch into the batch's folder.
  - **corpus**: as d-2026-09-13-51 defines it.
  - **comparison**: two studies over the same items read against each other: by the tally where they
    share their directions, or at the review for two explorations.
  - **question**: Brian's testable question, in the question list.

  **audit** leaves. itemizer keeps d-2026-09-13-1's "which stay the cut corpus's": what a study and a
  question are about, and the split of comparison into its senses, are not decided here.
- not taken: verification and exploration left naming a corpus until what a study is about is
  decided, the vocabulary contradicting rule 3 meanwhile.

### A definition carries kind exactly when its directions are not an exploration's, and claiming's directions have their own sections

- id: d-2026-09-13-57
- date: 2026-09-13
- supersedes: d-2026-09-13-18
- raised by: d-2026-09-13-44, claiming calibrated with no Classes, against d-2026-09-13-18's `kind`,
  present exactly when the directions have Classes, under which a claiming batch would carry no
  calibration and run uncalibrated; directions-schema reading a kind from a study id's prefix or the
  referee folder, and claiming's directions having no section shape. The session found a list judged
  per member declared nowhere a checker can read. Brian: "Go with A".
- decision: directions-schema's kinds are verification, exploration, referee and claiming, read from
  the study id's prefix or from `pipeline/referee/` or `pipeline/claiming/`. A claiming version's
  sections are What you are given; Criteria, each entry a rule that decides whether the item's finding
  bears on a hypothesis, stated generally and numbered from 1; What to produce, a `list of line`
  field; and Never; with no Classes, no How to read, and no `questions` in its frontmatter.
  `directions.sections` holds each kind's sections, `directions.criteria` the numbering of every
  kind's Criteria, and `directions.frontmatter` that `questions` is never the referee's or claiming's.

  What stands of d-2026-09-13-18, `kind` changed: `schemas/definition-schema.md` is the schema of the
  class `definition`, in the four sections, its Example the checker's fixture; the Artifacts row is
  `definition` at `docs/v3-framework/<container>/<home>/batches/<batch>/definition.md`, mutation
  frozen; its fields are `directions`, `kind`, `calibration`, `model` and `effort`; `kind`, `sample`
  or `full`, is present exactly when the directions are not an exploration's; `calibration` is present
  exactly when `kind` is `full`; no `index` line, no `tools` line and no `mcp` line, a definition
  carrying either of the last two failing `definition.fields` as an unknown key; the checks are
  `definition.batch`, `definition.title`, `definition.fields`, `definition.directions`,
  `definition.calibration`, `definition.model` as d-2026-09-13-41 has it, and `definition.frozen`;
  `DefinitionFile` and `Batch.BuildArgs` carry no tools or MCP field.
- not taken: the condition on a declared output, classes or a list judged per member, which nothing
  declares; Classes for claiming as one enum field per hypothesis, against Brian's "Yes, bare list"
  and an output shaped by the size of the set.

### state.md gains a Pipeline directions section

- id: d-2026-09-13-58
- date: 2026-09-13
- raised by: the gates on the pipeline directions' calibration, surfacing-candidates' and
  iterating-a-statement's, shown nowhere in state.md, StateBuilder reading the referee's directions
  only for question coverage, to which they add nothing, and the pipeline folders' calibration batches
  under no study. Brian: "Go with A".
- decision: state.md gains `## Pipeline directions`, derived like its other sections: for the referee
  and for claiming, each directions version in order with its body hash; which version, if any, is
  accepted, and the model its calibration batch ran under; and each calibration batch and where it
  stands, defined, executed or tallied, as `## Studies` shows a study's batches. Nothing in it is
  authored.
- not taken: no section, the preconditions reading the folders directly and the gate every
  verification's surfacing waits on invisible where Brian picks the next step.

### The epistemic framework's opening stands as d-2026-09-13-10 ruled it, the question it went ahead of settled

- id: d-2026-09-13-59
- date: 2026-09-13
- supersedes: d-2026-09-13-10
- raised by: d-2026-09-13-15, which marked d-2026-09-13-10 premature for its clause that a model's
  output is not unverified as such, and promised it superseded by the entry ruling the question it
  went ahead of, whether model output may feed another study; d-2026-09-13-50 ruled that no study's
  results are an itemizer's input, which leaves their verification status as it was. Brian: "Go
  with 1".
- decision: Restated whole and unchanged. The first paragraph of SKILL.md § Epistemic framework
  (applied), after its Scope paragraph, reads:

  **Recall and unverified output are not evidence.** What Brian remembers, what a session or a
  model concluded without verification, and what an exploration saw as leads may raise a question
  or a hypothesis, and never move one: only verified evidence does. A hypothesis file holds a
  statement and its evidence relationship together: the statement, current and edited in place;
  the record, dated entries never edited, which *is* the evidence relationship rather than a
  history of it; and a status computed from the record.

  The bold sentence `Only verified evidence moves a hypothesis.` later in the section stays deleted,
  the opening saying it. The sentence draws its line by verification and not by who produced the
  text: the results of calls under calibrated directions are the verified layer (d-2026-09-09-15), so
  a model's output is not unverified as such; that no study's results are an itemizer's input is
  d-2026-09-13-50's. CLAUDE.md's matching line is unchanged.
- not taken: d-2026-09-13-10 left premature and unsuperseded, d-2026-09-13-15's promise open with no
  question left to settle it; the clause "so a model's output is not unverified as such" dropped,
  which nothing in this unit argued against.

### This unit changes only the collator's existing code; its sample subcommands are built for preparing-pipeline-directions' first execution

- id: d-2026-09-13-60
- date: 2026-09-13
- raised by: `collate-pipeline-sample`, whose sample subcommands do not exist, beside the existing
  subcommands the unit's rulings change, with no finding or claim yet on disk to build a sample
  against, and the registry schema's "a tool a study needs is built as its first task". Brian: "This
  unit only for existing code."
- decision: The unit's changes to the collator are its rename (d-2026-09-13-48); claiming items that
  carry the hypothesis set and a finding, the `claim` subcommand reading the hypothesis files
  (d-2026-09-13-42); and the `collator` line in the indexes it writes (d-2026-09-13-49). The
  subcommands that collate the referee's and claiming's calibration samples are built under
  building-a-tool as the first task of preparing-pipeline-directions' first execution for each set,
  after its plan fixes the sample's span, strata and held-out split.
- not taken: the sample subcommands built now, with stratification and split as parameters guessed
  before any real finding or claim exists, the plan then fitted to the code.

### What a comparison of two studies' findings is stays undecided, and the question entries that imply one are carried as they stand

- id: d-2026-09-13-61
- date: 2026-09-13
- raised by: Brian, of the note-relationship questions: "There is a separate implication about
  whether v1's and v2's are to different degrees. This is a question that actually spans cross corpus
  content comparison, but it's comparing findings. Not comparing notes(v1) and notes(v2) data sets.";
  then "What about the other 2 plus the 3rd? Are the tensions that blocked them settled now or not? If
  they are not settled then defer." The session found the two questions settled by
  d-2026-09-13-39's slugs, and the comparison not: a comparison of two studies' findings is no frozen
  predicate over one itemizer's items; whether one set of criteria can judge v1 notes and v2 notes,
  which are different items, is open; no findings file holds a finding spanning two studies; and
  exploration over findings is retired (d-2026-09-13-50), the referee judging one finding at a time.
- decision: What a comparison of what two studies' findings show is, whether a question and where its
  answer would live, is not decided; it waits on the split of comparison into its senses and the
  shape of a study over several corpora. No question entry is written for the comparison of degree
  between v1's and v2's note relationships. `questions/v1-archive-note-relationships-in-prose` and
  `questions/working-plan-note-relationships-in-prose` stand as two questions, each of one corpus.
  `questions/notes-mix-cognitive-modes` and `questions/goals-no-track-type-names`, the latter asked
  beside `questions/humor-warmth-prior-belief-setups`, are carried as they stand, each of one corpus,
  the comparisons they imply undecided with the rest. Reading two verifications' tallies side by side
  in reviewing-findings is unchanged.
- not taken: the comparison of degree written as a question now, with no study shape able to answer
  it.

### A pilot is only an execution naming one item, and a calibration batch is not called one

- id: d-2026-09-14-1
- date: 2026-09-14
- raised by: a review of the working tree against d-2026-09-13-37 to -61, which found
  preparing-pipeline-directions' "This is the directions' pilot." calling a calibration batch a
  pilot, against § Vocabulary's pilot, "an execution of a batch naming one item, whose result a
  person reads before the rest run; a one-item batch needs none". The sentence was carried from
  preparing-to-verify-a-corpus's `assemble-sample-batch` row and section; the agent-runner skill
  had "a calibration batch is a verification's pilot, and a one-item batch is its own", and four
  lines read "No pilot: the calibration was it." or its claiming and referee forms. A calibration
  batch's results are withheld from Brian until he has scored, the opposite of a pilot's. The
  archive showed the wider sense a session's: revision 1's runner skill called calibration the
  pilot for a codebook, and on 2026-09-05 Brian selected an option that called calibration the
  codebook's pilot and one job he read the protocol's; the one-item sense is § Vocabulary's and
  the runner's `pilot` mark on a call. Brian: "Ok, then just fix the wording clash (item 5) right
  now. What are the options?"; and, of keeping the one-item sense, "Go ahead with option 1".
- decision: pilot keeps § Vocabulary's one sense, and a calibration batch is not called a pilot.
  preparing-to-verify-a-corpus's `assemble-sample-batch` row and § assemble-sample-batch, and
  preparing-pipeline-directions' § assemble-pipeline-sample-batch, lose the sentence calling the
  batch the directions' pilot. The agent-runner skill says directions already calibrated need no
  pilot, and neither does a one-item batch. verifying-a-corpus, surfacing-candidates at both of its
  batches, and iterating-a-statement say there is no pilot because the directions are calibrated,
  naming the claiming or the referee's directions where they did. § Vocabulary and rule 4's
  "calibrated or piloted", two alternatives, are unchanged.
- not taken: § Vocabulary's pilot widened to take in a calibration batch, whose results are
  withheld rather than read before the rest run, which makes rule 4's "calibrated or piloted"
  redundant and parts the skill from the runner's `pilot` mark; deleting only the sentences that
  call a calibration batch a pilot, which leaves four lines saying a calibration was one.

### directions.output holds claiming's What to produce to one list of line field

- id: d-2026-09-14-2
- date: 2026-09-14
- raised by: the same review, which found directions-schema's Shape giving claiming "one `list of
  line` field" in What to produce (d-2026-09-13-57) while `directions.output` held no such rule and
  the checker held only claiming's section headings. The collator's `referee` subcommand reads the
  list items of the first field of each claiming result, so a claiming version declaring any other
  shape would pass every check and its referee batch would collate no items. Brian: "Go ahead with
  option 1, plus the easy fixes of items 1 (state.md is out of date) and item 4 (claiming's output
  shape isn't enforced)".
- decision: `directions.output` also fails when claiming's directions declare in What to produce
  other than exactly one field, or a field that is not `list of line`. It is a declared check, the
  Shape stating what it holds; directions-schema's Checks row names the clause, and the checker and
  its tests hold it.
- not taken: no alternative was put to Brian; the check holds what d-2026-09-13-57's Shape already
  states.

### A What to produce field's name is one lowercase word

- id: d-2026-09-14-3
- date: 2026-09-14
- raised by: a probe of 2026-09-14 of prompt caching for claiming calls, run through the Claude Code
  CLI with the runner's flags, whose six calls each failed before any tokens were billed:
  "API Error: 400 tools.0.custom.input_schema.properties: Property keys should match pattern
  '^[a-zA-Z0-9_.-]{1,64}$'". The CLI sends `--json-schema` as a tool's input schema, the runner
  passes each What to produce field's name through as a property key, and the probe's field was
  named `bears on`. directions-schema's Example declared `decided by`, and the checker's claiming
  fixtures `bears on`, so directions written from either passed every check and would fail every
  call. Brian first ruled lowercase slugs: "Go with the requirement of lowercase slug names for
  directions.output". Applying it found the method's keyed-line grammar, which reads a key only as
  `[a-z][a-z0-9 ]*` in every governed file and in a finding's `§ <field>` citation, reading no
  hyphen, so a slug of two words is no field at all; the names both accept are one lowercase word.
  Brian: "Go with option A, but claims is not allowed. Present the context and options for what
  the fields are about". Shown that the two names are an illustration in directions-schema's
  Example and a string in the checker's tests, and that real directions name their fields when
  authored with him, Brian: "Go ahead".
- decision: A What to produce field's name is one lowercase word, `[a-z][a-z0-9]*`, at most 64
  characters: a name the API accepts as a property key and the keyed-line grammar reads as a key.
  The shared directions reader reports any other name, so `directions.output` fails it at the write
  and the runner's dry run refuses the batch before a call is made. directions-schema's Shape and
  its `directions.output` row say so. The Example's `decided by` becomes `basis`, and the claiming
  fixtures' `bears on` becomes `relevant`; both are fixtures, and neither names a field of real
  directions, which `author-directions` and `author-pipeline-directions` name with Brian. A result
  file's keys, a tally's sections and a finding's `§ <field>` citations carry the name as declared.
- not taken: lowercase slugs with the keyed-line grammar widened to read hyphens, which changes how
  every governed file and every `§` citation is parsed; names with spaces that the runner turns
  into underscores at the API and back when it reads the answer, which leaves the call reading one
  name in its directions and another in its schema; `claims` for the claiming fixture, of which
  Brian said "claims is not allowed".

### A study is of one itemizer's items, and the itemizer is built inside the study

- id: d-2026-09-14-4
- date: 2026-09-14
- supersedes: d-2026-09-13-1
- raised by: the study-registry conversion, taken as this unit's first starting point after Brian
  typed "Treat the previous session's queue and stack as only advisory. My goal is to get the
  corpora schema and study registry schema complete in this unit. Reorder the queue and stack
  according to the method's criteria of widest first, present it and the first decision's
  framing." The frame found two definitions of a study in force at once: § Vocabulary's, since
  d-2026-09-08-3 and restated by d-2026-09-13-41 and -56, the life of one set of directions over
  the items of one itemizer, naming no corpus; and the registry's id form, its `corpus` cell, the
  four `-a-corpus` activity ids, the two plan preconditions and d-2026-09-13-1, all making a study
  one corpus's. The archive showed the corpus in the id entering on 2026-09-05 as a session's form
  carried from revision 1's verification round per corpus, never deliberated on its own, and
  Brian's typed positions of 2026-09-13 in order: at 04:59, "That still makes the study about the
  target corpus."; at 21:12, "I'm now thinking corpora and batch inputs are no longer tied, and
  batches can come from itemizers which combine multiple corpora."; at 23:30, "explore plan and
  verify plan are to no longer be restricted to one corpus, but rather one itemizer". The `corpus`
  cell's only code reader was a placeholder substitution no path has used since batch paths took
  `<container>` and `<study>`. Brian: "I think a study is of one itemizer's items, and making the
  itemizer if it doesn't already exist is part of the study too. It begins with putting together
  an itemizer."
- decision: A study is of one itemizer's items. Building the itemizer where it does not exist is
  part of the study, as building-a-tool already says: the first task inside the study that needs
  it, never a study of its own; the plan names the itemizer to build or reuse, and the itemize
  step builds or picks it once the plan is approved. No corpus names a study: what the study's
  itemizer cut and utilized is recorded per batch in the index head, and nowhere else.

  What stands of d-2026-09-13-1: an itemizer may read corpora besides the ones it cuts and use
  what it reads to cut its items, to label them and to fill their bodies, a passage from another
  corpus included; the study and its findings are about the itemizer's items, and a finding
  describes those items. What leaves: "the target corpus" as the owner of the study, its
  questions and its findings; "cut through its reader and located by its locators", an item being
  located by the notation its index head declares; and "A question stays about one corpus's
  items", which d-2026-09-14-6 rules. What d-2026-09-13-1 sent to -2, -5 and -6 reads through
  d-2026-09-13-50 and -49 as before. The index head's single `corpus` key under an itemizer that
  cuts several corpora with no one target is not decided here.
- not taken: a study of its target corpus with other corpora utilized, d-2026-09-13-1 as it stood,
  which keeps a special case, the target, that no artifact records, gives an itemizer combining
  corpora equally no id, and overrides Brian's later typed position with his earlier one; a study
  of every corpus its itemizer reads, the id naming them all, which authors in the registry what
  each index head already records.

### A study has exactly one question, and a question has any number of studies

- id: d-2026-09-14-5
- date: 2026-09-14
- raised by: Brian, on the first ruling: "Should this be one question or can it be multiple?",
  then "One question-list entry or multiple can be addressed?" The text held several: a
  directions version cites the questions it freezes, both plan processes ask which questions, and
  each finding names at most one. Shown that, Brian: "I think a question can be about anything,
  and it's one study per question. Itemizer is made to answer the question. What do I lose by
  getting rid of many questions to a study?" The session named the losses, the item sent once per
  question instead of once per study, contrasts between two predicates on the same items becoming
  a comparison across studies, explorations reading once with several questions in view, and
  per-study overhead, and the gains, the registry named by the question, item and predicate and
  question decided together, the derivations one-to-one. Brian: "A study can have at most one
  question. A question can have multiple studies, such as different models or a different
  approach."; "Per study overhead is less of an issue because I intend to really cull the
  questions and keep them what I want to investigate and not suggested questions over lossy
  summaries of past deliberation."; and, asked whether "at most" admits a study with none, "I
  meant study has exactly one question."
- decision: A study has exactly one question, and a question has any number of studies: a repeat
  under another model, a different itemizer or different directions, an exploration and a
  verification of the same question, each a study of its own under d-2026-09-13-41. § Vocabulary's
  study, verification and exploration lines, d-2026-09-13-56's, gain the question: a study is the
  life of one set of its own directions over the items of one itemizer under one model and
  effort, for one question. A directions version freezes one predicate, for its study's question.
  A question a study's items and predicate cannot answer is another study's. The findings
  `question` field and the directions frontmatter's `questions` list, each now derivable from the
  study, are left to their own schemas' reviews under rule 9. Culling the list is Brian's, by
  withdrawn lines under the question list's append discipline, and no schema changes for it.
- not taken: several questions per study, the text as it stood, which forces one item definition
  and one sample stratification to serve every question a study takes; one study per question in
  the literal sense, which bars the repeats d-2026-09-13-41 makes separate studies; a study with
  no question, which "at most one" admitted and Brian corrected.

### A question is about anything a study can be made to answer, and names no data set

- id: d-2026-09-14-6
- date: 2026-09-14
- raised by: d-2026-09-13-38 and -50, which each left what a question is about undecided once the
  question list dropped its data-set field, and d-2026-09-13-7's "A question is about one
  corpus's items", retired as the list's scope. Brian: "I think a question can be about anything,
  and it's one study per question. Itemizer is made to answer the question."
- decision: A question is about anything a study can be made to answer: it names no corpus, no
  data set and no itemizer, and its entry carries no field for one, as d-2026-09-13-38 already
  holds. The study's itemizer is made for the question, and the corpora it reads are recorded in
  its index heads. This closes what d-2026-09-13-38 and -50 left open; d-2026-09-13-7's clause on
  one corpus's items does not return.
- not taken: a question tied to one corpus's items, d-2026-09-13-7's ruling, which
  d-2026-09-13-50 already declined as the list's scope; a question tied to an itemizer, built or
  not, which d-2026-09-13-38 declined as fixing the item before the plan does.

### The study id is the type, then the question's slug, then a slug only for a further study

- id: d-2026-09-14-7
- date: 2026-09-14
- supersedes: d-2026-09-08-2
- raised by: d-2026-09-14-4, after which the id `<type>-of-<corpus>-<slug>` embedded a corpus the
  study is not of. The frame: the id is the folder name, the `<study>` segment of every batch
  path, the prefix of every citation and item token, the title of three files and the study half
  of an evidence citation; the directions schema reads a version's kind from the prefix; three
  checkers hard-code the prefixes; no live artifact carries any study id and no hypothesis record
  holds an entry, so the form migrates nothing. Offered the type prefix with the question's slug
  and a distinguishing slug only for a further study of the same question and type, the
  question's slug with the type in a cell, or a free slug with a `question` cell, Brian: "Option 1
  is good".
- decision: A study id is `<type>-of-<question>` or `<type>-of-<question>-<slug>`. `<type>` is
  `verification` or `exploration`. `<question>` is the slug of an entry in the question list, the
  one question the study is of; since question slugs may contain hyphens and one may be a prefix
  of another, the question is the longest slug heading an entry in the list that the id carries
  after `<type>-of-`. `<slug>` is present only for a further study of the same question and type,
  a lowercase slug naming what sets the study apart, the model where the model differs, the
  itemizer or the directions otherwise; the first study of a question and type carries none and
  is never renamed when a second arrives. The id is authored at approval and never changed. The
  `<corpus>` placeholder leaves § Artifacts' placeholders, no path using it, and the `<study>`
  gloss reads the new form. The directions kind derivation, the three title checks and the
  citation forms stand unchanged.

  What stands of d-2026-09-08-2: no ordinal; a slug is lowercase `[a-z0-9-]+`, authored at
  approval and never changed; an id is unique across the registry. What leaves: the corpus
  segment, and a slug that "names what the directions do", the question now saying that.
- not taken: the question's slug then a slug, with the type in a registry cell, which changes the
  kind derivation and three checkers and leaves a token silent on whether it cites an exploration
  or a verification; a free slug after the type with a `question` cell, which puts the question
  in no token, so that finding a question's studies means reading the registry.

### The four corpus activities are named by study type, and their preconditions name the question and every corpus

- id: d-2026-09-14-8
- date: 2026-09-14
- raised by: d-2026-09-14-4, against the ids preparing-to-explore-a-corpus, exploring-a-corpus,
  preparing-to-verify-a-corpus and verifying-a-corpus, and the preconditions "The corpus is
  readable and CORPORA.md says how." The archive showed the ids an assistant's at 2026-09-04
  23:46, reshaped on 2026-09-05 where Brian selected a label, and typed by him since only as file
  names; the object entered with revision 1's verification round per corpus. Their consumers: four
  Router rows and seven `enables` cells, four file names, rule 4's sentence, twelve
  cross-references in six activity files, one prose line in each of three schemas, and the state
  builder's two chain arrays; no path and no token. Brian's typed verbs are "explore" and
  "verify", his nouns "exploration", "verification" and "study"; "conducting" appears in the
  archive only in a label he selected. Offered the object as the study type, the object as the
  question, or `-a-corpus` kept, Brian: "Option 1 is good".
- decision: The four activities are `preparing-an-exploration`, `conducting-an-exploration`,
  `preparing-a-verification` and `conducting-a-verification`, their files renamed with them,
  every `enables` cell, cross-reference, schema line and chain array following; their process ids
  are unchanged. Their preconditions name the one question and every corpus: for a verification,
  the question is open in the list and its answer is one a frozen predicate could give, and every
  corpus the study's itemizer will read is readable and CORPORA.md says how; for an exploration,
  the question is open or is asked for and written first, then the same corpus clause. The two
  plan rows' "the corpus's shape as CORPORA.md gives it" reads the corpora the itemizer will read.
  The sentences are composed at the edit; this entry rules what they name.
- not taken: the question as the object, `verifying-a-question`, which is not ordinary English
  though it keeps Brian's verbs; `-a-corpus` kept and read as the corpora the itemizer reads,
  which leaves the word colliding with § Vocabulary's corpus; `running-` and `executing-` as the
  verb, the first colliding with the method's run of autonomous work, the second with the
  runner's verb and naming only the batch.

### The registry's type and corpus cells leave as derivable

- id: d-2026-09-14-9
- date: 2026-09-14
- raised by: d-2026-09-14-7, after which `type` is the id's prefix and the question its second
  segment, and `corpus` names nothing a study is of. Rule 9: "Whatever can be derived from an
  artifact is never authored beside it." The frame: `type` picks the chain in the state builder
  and is checked equal to the prefix; `corpus` is checked equal to the id's corpus and has no
  reader but the render and an unused placeholder; the founding's readability axis is met by the
  prefix reading as the type in the id itself. Offered the id with the date only, `type` kept as
  checked redundancy, or the question as a cell, Brian did not rule the cells apart: his ruling
  on the date, "Let's cut the go/approved field.", left the registry its ids only, and he approved
  the set of entries so drafted, "Looks good, proceed".
- decision: The registry authors no `type` and no `corpus` cell. The type is read from the id's
  prefix, by the state builder for the chain and by the title checks of leads, findings and
  declined candidates; the corpora a study reads are read from its index heads. `registry.type`
  and `registry.corpus` retire (d-2026-09-14-12).
- not taken: `type` kept as checked redundancy, an exception to rule 9 with no invariant on its
  side, the prefix already reading as the type; the question as a cell in place of the id's
  segment, which contradicts d-2026-09-14-7.

### The approval date leaves, and the act is called approval, not go

- id: d-2026-09-14-10
- date: 2026-09-14
- raised by: Brian, on the last authored cell: "What is 'go' about?", then "So should it be
  'approved' instead? 'go' doesn't read right". The archive showed no user turn using "go" in
  this sense; it entered with the founding registry design as a session's word, and the plan rows
  gloss it as "the plan approved is his go". The date's only reader was the state render. Brian:
  "So this only matters if I start a study but don't carry it out? I thought the plan writes the
  entry into the registry file, which wouldn't happen until the plan is approved?"; then "Let's
  cut the go/approved field."
- decision: The registry records no date. The entry's existence is the approval: the plan process
  appends it at the moment Brian approves the plan and never before, so a registered study is an
  approved one, and when it was approved is read from nothing. The act is called approval
  wherever the method names it: § Vocabulary's study, the `studies` Artifacts row, the two plan
  sections, the two conducting preconditions and preparing-pipeline-directions' plan read
  approval in place of go. `registry.go` retires (d-2026-09-14-12).
- not taken: the date kept under `approved`, whose only use was a registered study with no batch,
  the case Brian named, and a render line nothing reads; an ordering check over the date, a check
  on a value no process consumes.

### The registry is a title and one-line entries, with no table

- id: d-2026-09-14-11
- date: 2026-09-14
- raised by: d-2026-09-14-9 and -10, after which the registry's one column was the id. Brian: "And
  make the registry an entries section with no table", the grammar holding that an entries
  section with no field table holds one-line entries, each `- ` line one entry.
- decision: `docs/v3-framework/studies.md` is the title `# Studies` followed by one entry per
  study, `- <id>`, in the order the studies were approved, with no head prose, no sections and no
  table; it may hold none. The class stays append: an entry is never edited, and a study
  abandoned is a fact its folder shows, not an entry removed. The `studies` Artifacts row's
  description reads one entry per study. Nothing else is authored there.
- not taken: a one-column table, a list wearing a table's clothes; head prose, which the question
  list also declines.

### The registry has four checks, the question segment resolved as declared

- id: d-2026-09-14-12
- date: 2026-09-14
- raised by: the checks after d-2026-09-14-7 to -11: six ids and one info existed,
  `registry.table`, `registry.duplicate`, `registry.go`, `registry.id`, `registry.type`,
  `registry.corpus` and `registry.corpora-unavailable`, four of them without a subject.
  `registry.id` was changed by d-2026-09-08-2, `registry.type` and `registry.corpus` named by
  d-2026-09-09-12 and re-held by d-2026-09-13-50, the rest described in words by the founding
  record's entry of 2026-09-06. The question segment resolves against another file, and the
  method holds a check across files earned only while resolving a reference type is declared:
  `findings.question` resolves to the list, `leads.cites` to an index, and `registry.corpus`
  resolved a segment to the corpora file from its first write. Offered four checks with the
  resolution declared, or the same without it, Brian: "Go with option 1".
- decision: The registry's checks are: `registry.title`, the title is not `# Studies`;
  `registry.shape`, the engine's, a line after the title is other than a `- ` entry or blank;
  `registry.id`, an entry is not `verification-of-<question>[-<slug>]` or
  `exploration-of-<question>[-<slug>]` where `<question>` is the longest slug heading an entry in
  the question list and `<slug>`, when present, is lowercase `[a-z0-9-]+`; `registry.duplicate`,
  an id appears twice. Resolving the question segment is a declared check, a reference resolved
  as `registry.corpus` was, existence only, since a check cannot know whether the question was
  open at approval. `registry.table`, `registry.go`, `registry.type` and `registry.corpus`
  retire; the info is `registry.questions-unavailable`, reported and never failed, when the list
  cannot be read. d-2026-09-09-12's clauses on the two retired checks are history. A
  folder-to-registry consistency check is not minted: it is a check across files, earned only,
  and no failure has been observed.
- not taken: `registry.id` holding the prefix and the charset only, under which a study of a
  misspelt or unwritten question registers cleanly and is caught only at a directions frontmatter
  or a finding.

### The corpora file's read through line names the code path an itemizer takes

- id: d-2026-09-14-13
- date: 2026-09-14
- raised by: the corpora conversion, this unit's second starting point, with what CORPORA.md's
  `read by` names at the top of its stack as d-2026-09-13-51 and -36 left it. The archive showed
  the line entering in an assistant turn at 2026-09-07 03:20 restating readiness "as a fact about
  how the corpus is read", Brian selecting a label two minutes later and never typing the phrase;
  its values mixed a session's reader such as MCP tools or sqlite3, a repo tool, "files",
  "nothing", and a runner mechanism the runner skill no longer has. The consumers: the plan
  processes, `build`'s entry "that must be true afterwards", the index schema's locator "through
  the reader CORPORA.md names", and the `corpus` row's "read through the MCP server, files or
  sqlite3". An itemizer is code under `tools/` reading the file or database at `where` directly,
  so four of eight entries named a reader no itemizer can use. Offered the session's reader, two
  keys, or the code's reader, Brian: "Is this for how an itemizer would get the data as input?",
  and, shown that sense, "Use read through and go with this".
- decision: The line is `read through`: for each corpus, the code path an itemizer takes from
  `where` to items, the format, the reader or library in the repo, and where its schema or
  reading rules are documented, a converter where one runs first. Readable, in the plan
  preconditions and in building-a-tool's "a corpus to make readable", means an itemizer can be
  built over it. What a session or the MCP server reads a corpus through is not in the file: the
  planner's `corpora`, `storyplan-data` and `code-sessions` skills carry it for the corpora they
  cover, and the rest are files. The index schema's locator is resolved by the parts the index
  head's notation declares, its clause naming CORPORA.md leaving; the `corpus` row's description
  follows. An itemizer is named per batch in its index head, never in an entry; the runner
  mechanism leaves the fimfiction entry under rule 5; "nothing" leaves, every corpus at `where`
  being readable by code. The founding record's entry of 2026-09-06, readiness restated as how a
  corpus is read, is history as to the line's sense.
- not taken: the session's reader for a locator, one value per corpus, which names for four
  corpora a reader an itemizer cannot use and what the planner's skills already document; two
  keys, the session's and the code's, the second restating what `where` and the repo's
  conventions fix.

### Compaction summaries are not corpus data, and the ingest drops them

- id: d-2026-09-14-14
- date: 2026-09-14
- raised by: d-2026-09-13-51's deferral. The archive stores a compaction summary as a user-role
  record whose body opens "This session is being continued from a previous conversation that ran
  out of context.", twenty-five of them in six sessions; rule 10 says a user turn in a
  code-sessions transcript is Brian's own prose, and d-2026-09-13-51's definition says a corpus
  is "never a lossy summary or report made afterwards from other text". The ingest examines no
  compaction flag, though it already drops a hook's feedback and elides tool results. Brian:
  "Compaction summaries are not corpus data, and this seems like a bug that should be fixed in
  the ingest and code sessions skill. All the transcripts are still on disk." Checked at the
  source: by date no session has aged off, and the six sessions holding a compaction record were
  seen on disk on 2026-09-14; the code-sessions skill's archive-health recipe compares exact
  timestamps and so flags every session but the last one touched in a run. The conversations
  corpus holds no compaction block, the parser marking them and `get_blocks` labelling them.
- decision: A compaction summary is not corpus data. The ingest drops it at extraction, as it
  drops a hook's feedback; whether a bare marker stands in its place is fixed in `build`. The
  archive is re-extracted whole. The code-sessions skill says so, its archive-health recipe is
  fixed to compare by run date, and the code-sessions entry's caveats say it, all in the same
  commit as the ingest. The build runs under building-a-tool after this unit's edits, together
  with d-2026-09-14-16. Rule 10 is unchanged: no such record then exists in a user turn.
- not taken: a reading rule only, in the skill and the caveats, each itemizer narrowing them out,
  which leaves rule 10's letter wrong and repeats the exclusion per itemizer; a mark rather than a
  drop, which the no-delete design does not require of records, the raw transcripts holding full
  fidelity; compaction summaries as corpus data, against the definition.

### Subagent transcripts are corpus data, and a subagent's opening prompt is the parent assistant's

- id: d-2026-09-14-15
- date: 2026-09-14
- raised by: d-2026-09-13-51's other deferral. A subagent session opens with a user-role record
  that is the prompt the parent's assistant wrote and closes with the assistant's report, which
  the parent holds only as an elided tool result; 262 of the archive's 431 sessions are
  subagents. d-2026-09-13-51's definition includes "an AI's reply in the exchange it answered"
  and excludes a report "made afterwards from other text"; Brian's typed formulation behind it
  was "my words, or an AI's direct response to my words. Not an AI lossy summary." The ingest
  keeps subagents as part of the tree on Brian's design. Offered corpus data as part of the tree,
  not corpus data with the rows kept as provenance, or the exchange in and the report out, Brian:
  "I agree that it is corpus data, and it is assistant role, not my words, instead of being
  lumped as my words based on user role metadata".
- decision: A subagent's transcript is corpus data: an exchange as written, an AI's prompt and an
  AI's reply, its report's lossiness being that of any assistant turn that reads and answers. A
  subagent's opening prompt is the parent assistant's text and is recorded as assistant role by
  the ingest (d-2026-09-14-16), never as Brian's. An itemizer that wants Brian's decisions
  narrows to main sessions and says so in its index head. The code-sessions entry's `what` is
  unchanged, and its caveats say what a subagent's first record is.
- not taken: subagent rows outside the corpus while kept in the archive, a corpus narrower than
  its database for the first time, and a report a main session acted on unreadable by any
  itemizer; the exchange in and the closing report out, which nothing marks apart.

### The ingest records who authored a record, not the transcript's role

- id: d-2026-09-14-16
- date: 2026-09-14
- raised by: d-2026-09-14-14 and -15, which the frame found to be two cases of one bug: the
  ingest inherits the transcript's user role for everything the harness injects. Counted in main
  sessions' user-role records: skill loads, "Base directory for this skill", 180; interruption
  markers, 177; task notifications, 162; rejected-tool markers, 101; local command caveats and
  output, command invocations and IDE events, 408; compaction summaries, 25; subagent sessions
  adding their 262 opening prompts and a few dozen of the same classes. The ingest already sorts
  tool results, hook feedback and question dialogs at the boundary. Brian's words: "lumped as my
  words based on user role metadata".
- decision: The ingest records who authored each record, not the role the transcript gives it:
  Brian's typed turns and his `Typed:` lines as his; a subagent's opening prompt as the parent
  assistant's; the harness's injections, the classes above, recorded as the harness's or dropped;
  compaction summaries dropped. The treatment of each class is fixed in `build` with Brian, and
  the archive is re-extracted whole. Rule 10's "a user turn or `Typed:` line in a Claude Code
  transcript" is then true by its letter and is not edited. The code-sessions skill's reading
  rules say which records are his and which are the harness's, in the same commit.
- not taken: rule 10 qualified in words while the archive stays as it is, which leaves over a
  thousand machine records in user role for any reader to misread; a fix confined to compaction
  summaries and subagent prompts, which leaves the other classes.

### A corpus entry's heading is at the third level

- id: d-2026-09-14-17
- date: 2026-09-14
- raised by: the corpora conversion against the grammar: the engine parses an entry at `###`
  only, the folder's schema says a new level is never added, and the file's eight entries were
  `##`. The checker read level-2 headings as the corpus ids, whose only consumer after this
  unit's registry rulings is the index head's `corpus` and `utilizes corpora` enums. Offered the
  entries at `###` or the grammar gaining a level, Brian: "Go with option 1".
- decision: A corpus entry's heading is `### <id>`, a slug unique in the file, the head paragraph
  prose before the first entry; the id source reads level 3, and the index checker's enums
  follow. The grammar is unchanged.
- not taken: the grammar gaining `##` entries, a new level the folder's schema bars and nothing
  argued for.

### The caveat prose is one optional block field, caveats

- id: d-2026-09-14-18
- date: 2026-09-14
- raised by: the grammar holding no prose inside an entry, against eight caveat blocks of five
  kinds: what is excluded from the corpus, what sits beside it that is not corpus data, reading
  semantics, format quirks and provenance limits; d-2026-09-13-50's standing clause that "the
  standing caveats stay in CORPORA.md". Brian had typed "caveat" in use on 2026-09-03, not for
  this file. Offered one field, two fields with the not-corpus-data exclusions apart, or no field,
  Brian: "Go with one field".
- decision: An entry's fourth field is `caveats`, type block, optional: what a reader must know
  that `what`, `where` and `read through` do not say, the five kinds above among it, present when
  there is something to say and absent otherwise, so that no entry invents one. The exclusions
  d-2026-09-13-51 named as not corpus data sit in it.
- not taken: a second field for what is not corpus data, a name and a sorting for a query only an
  itemizer's author asks; the prose dropped to the planner's skills, which three corpora do not
  have.

### The corpora class's fields, types, title and three checks

- id: d-2026-09-14-19
- date: 2026-09-14
- raised by: the conversion's remaining shape after d-2026-09-14-13, -17 and -18: the types and
  presence of the three keyed lines, each already a sentence or two long and one naming three
  locations; the title `# Corpora` and the head paragraph; and the three existing checks,
  `corpora.section`, `corpora.duplicate` and `corpora.fields`, described in words by the founding
  record's entry of 2026-09-06 and holding a shape that no longer exists. Shown the shape, Brian:
  "This is fine". On the draft's sentence that a corpus enters only when a study needs it: "No,
  we'll be putting in the corpora as part of the work unit. They don't need studies first."
- decision: The class `corpora` is: the title `# Corpora`; the head between the title and the
  first entry, required, prose; the whole file after the head, required, entries, which may be
  none. An entry is `### <id>`, then in order `what`, required block, what the corpus is as
  primary-source data, whose text and of what kind, its population naming the file that lists it
  where one does; `where`, required block, the file or database by its path or the config entry
  that names it; `read through`, required block, per d-2026-09-14-13; `caveats`, optional block,
  per d-2026-09-14-18. No progress, readiness, deferral, judgment or count is written. The checks
  are `corpora.title`, the title is not `# Corpora`; `corpora.shape`, the engine's, the head
  missing, a line outside the head and the entries, or a heading other than an entry's;
  `corpora.entry`, the engine's, a heading not a slug or repeated, a field missing, unknown, out
  of order or of the wrong type, a line neither keyed nor continuation. `corpora.section`,
  `corpora.duplicate` and `corpora.fields` retire; the founding entry of 2026-09-06 is history as
  to them. No minimum count of entries and no cross-file check. The `corpora` Artifacts row's
  description reads how an itemizer reads each corpus. The eight corpora are brought to the
  schema in this unit, no study needed first; the rewritten file is the first under the schema
  and is shown to Brian against it before any other edit relies on it.
- not taken: line types for the three required fields, forcing the longest values onto one line
  for no reader; a minimum of one entry, which the index checker's info already covers; an entry
  written only when a study needs the corpus, which Brian declined.

### The build row loses "the corpus's state is recorded"

- id: d-2026-09-14-20
- date: 2026-09-14
- raised by: building-a-tool's `build` row, whose description ends "an ingest writes what it
  ingests and the corpus's state is recorded", against the corpora schema's "a fact file with no
  state in it", d-2026-09-13-50's standing clause that no state of a corpus is recorded, and
  `state` as the name of a generated artifact. The clause dates from the revision-2 draft, when
  the file still carried progress and readiness; the founding record's entry of 2026-09-06
  removed them from the file and the row was not brought with it. The row's section already says
  the entry is updated in the same commit. Brian: "Yes, ditch this clause."
- decision: The clause leaves. The `build` row reads that an ingest writes what it ingests and
  brings the corpus's entry true, which is what its section says.
- not taken: the words kept in a loose sense, a class name in a row meaning something else.

### The index head names no corpus: the corpus and utilizes corpora keys leave

- id: d-2026-09-14-21
- date: 2026-09-14
- supersedes: d-2026-09-13-49
- raised by: Brian, after the session's account of what stands before a first exploration: "Can
  the index head's corpus-related keys be cut? What is the file for, who writes it, and who
  reads it?" The frame: the index is the committed list of a batch's items with a locator each,
  written once by the itemizer or the collator and never by hand; its table is read by the
  runner for the items and their order, by the checker resolving `leads.cites` and findings
  citations, by `tally-batch --group-by`, by write-leads, write-findings and the reviews; of its
  head, the reviews read `itemizer` and `narrowing` and every reader of a locator reads
  `locator notation`, while `corpus` and `utilizes corpora` were read by the checker alone, to
  hold them to CORPORA.md's ids and refuse a repeat, the runner reading no head key
  (d-2026-09-13-49) and the state builder counting rows. `corpus` entered with the index schema,
  d-2026-09-09-4, in the session's draft, carrying the per-corpus study form d-2026-09-14-4
  traced to 2026-09-05; Brian's typed rulings there were on `locator` and `locator notation`.
  `utilizes corpora` is d-2026-09-13-5's, under d-2026-09-13-1's "That still makes the study
  about the target corpus."; its name is his, "I was thinking 'utilizes'. Use two fields", the
  two being `utilizes corpora` and `utilizes outputs`, and the second left the same day under
  d-2026-09-13-50. d-2026-09-14-4 retired the target corpus at the study level on "I think a
  study is of one itemizer's items", left the split standing in the head as the record of what
  the itemizer "cut and utilized", and did not decide the single `corpus` key under an itemizer
  that cuts several corpora with no one target. § Vocabulary takes no side: an itemizer "reads
  corpora, one or several". The index schema's queries "every batch that cut a corpus" and
  "every batch whose itemizer utilized a corpus" were the keys' only prose consumers, and no
  activity file reads either; the corpora an itemizer read are in its code at the version the
  `itemizer` line names, and a locator notation names its source in words. Shown that, with one
  `corpora` list as the alternative, Brian: "Proceed with the cut".
- decision: The index head names no corpus. `corpus` and `utilizes corpora` leave the head; the
  key order is `itemizer` or `collator`, `narrowing`, `locator notation`, `source hash`. Which
  corpora an itemizer read is read from its code at the version its head line names, and what
  a locator addresses from the `locator notation` line; the index schema's queries say so, and
  the corpora file's entry ids are the names a locator notation, a deliberation and a `corpus`
  shortcoming use, with no machine consumer. `index.head` fails when a key is missing, unknown
  or out of order, when both or neither of `itemizer` and `collator` are present, or when
  `source hash` is present and not a SHA-256; its clauses on the two keys retire, the info
  `index.corpora-unavailable` retires, and the checker no longer reads CORPORA.md for an index.
  `IndexFile.Render` takes no corpus. The clause of d-2026-09-14-4 that what the itemizer cut
  and utilized is "recorded per batch in the index head, and nowhere else" is history; what it
  left undecided, the single key under a multi-corpus itemizer, no longer arises. Not decided
  here: whether a locator covers every corpus an item draws from, whether an item tells the
  call which corpus it came from, and a tally split by corpus.

  What stands of d-2026-09-13-49: an index head carries exactly one of `itemizer` or
  `collator`, a line naming the tool that wrote the index as `tools/StoryPlanner.<Name>` or a
  script's path, with its version or commit; `narrowing`, optional for either tool, a line,
  states in words the condition in the tool's code that chooses which items the batch holds,
  absent when every item is taken; `locator notation` is required; `source hash` stays
  optional, when the source is one document; the runner's messages say the itemizer or
  collator regenerates missing bodies; d-2026-09-09-4 still backs `index.head` and is not
  superseded. What leaves: `corpus` present exactly when `itemizer` is, `utilizes corpora`
  beside `itemizer` only, and their checks.
- not taken: one `corpora` key listing every corpus read, with no split, which keeps a
  per-batch machine record and the enum check for a grep no activity asks and a typo check on
  a value code writes, against building-a-tool's rule that a check is added for a failure a
  batch has shown; the keys kept as they stand, which keeps the target corpus at the batch
  level after d-2026-09-14-4 retired it at the study level and leaves the multi-corpus
  itemizer's single key undecided.
