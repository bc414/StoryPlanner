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
