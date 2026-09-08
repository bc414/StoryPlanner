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
- decision: `docs/v3-framework/spec-pools/` is closed as the founding record of
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
