# Decisions

The method's decision record: one titled entry per decision about how the buildout is run,
every entry Brian's, appended after his approval and never edited, sections by revision in
order. The format is `artifacts.md` § Decisions.

**No entry is current on its own.** An entry is what stood on the day it was approved; a
later entry may supersede it, in whole or in part, and says so in its `supersedes` line and
its prose. What the method currently says is the skill's text, never this file; what stands
here is derived from the `supersedes` lines, never read off position.

**This file is provenance, for one activity.** It is written and read in revising-the-method,
to apply decisions to the skill and to trace why a rule is what it is. A session running any
other activity takes its instructions from that activity's file and the formats, which are
these decisions already applied, and has no reason to open this file.

Founded 2026-09-06 with every decision then standing, transcribed from
`methodology-revision-2-rulings.md` (closed that night), handoff 1 § Rulings so far and the
WU2.15 plan's log; decisions already superseded by then were not entered, and those records
hold their original wording.

## Revision 2

### The tables are the topology; the prose is the procedure

- date: 2026-09-04

What exists, who runs it, what it reads and writes is ground truth in the tables; how it is
done stays ground truth in the prose beside them; a conflict between a row and its prose is
a gap, never resolved by precedence. Every change is a row edit and a prose edit together.

### Markdown tables are the source; a C# tool with tests is the engine

- date: 2026-09-04

The method's rows are markdown tables with fixed columns and ids in every cross-reference;
the engine is a small C# console tool under the repo with pure tests, validating and
deriving; the generated views are mermaid and tables the tool writes, never hand-edited.
Rejected: JSON as source, since rationale prose reads worst there; PowerShell as engine,
since it has no tests and 5.1's quirks had already cost the skill-audit scripts; a separate
visualizer. Brian: "this combination works." On 2026-09-04 the generated views were marked
sections inside the authored files; that part is superseded by *Generated text is files
only* below.

### Consumers are never authored

- date: 2026-09-04

A consumer is a query over rows: the processes whose reads include what another writes.
Writing consumers down beside the rows is the stale-mirror failure; the tool derives them.
Five "confident" cuts made on 2026-09-04 reversed to two once consumers were written out,
which is why no cut is decided before the validator runs.

### The validator is built in-house

- date: 2026-09-04

Asked whether a tested, documented tool should replace the validator and its table format,
since process management is not bespoke, Brian ruled to continue on the current path. The
survey's finding: requirements-traceability tools, model-as-code tools, BPMN linting,
in-toto and policy engines each cover the registry layer and none covers the graph policy,
and each would put a second schema beside the prose. Rejected with it: an MCP server as the
validator, since validation is a build step a session must not be able to skip.

### The real-folder test also runs the skill validator

- date: 2026-09-04

`claude plugin validate .claude/skills` checks frontmatter against the published spec and
the in-house line-budget and description-length rules cite the same guidance: one duty in
two instruments. Brian: the integration test runs both, so a frontmatter defect fails
`dotnet test` the same way a row defect does. Lands with the router swap.

### Readable ids

- date: 2026-09-04

Every id in the method is a slug that reads in a sentence. The abbreviated ids of draft 1,
`f.cand`, `V.c`, `G1` colliding with gap `G1`, were the cognitive friction that opened the
2026-09-04 evening session; renaming was mechanical because the validator holds every
reference.

### Three tables, an enables graph, one terminus

- date: 2026-09-04

Activities, Processes and Artifacts. Brian: "an activity is the whole row, id is how we
identify it, the prose on the right is a description." Activities are things Brian does,
gerund with object; the relation between them is *enables*, a DAG ("Yes, a DAG"), with
exactly one terminus owning no processes. Brian: "baselining-hypotheses is the terminal
activity that has a process. It enables changing-the-planner-for-v3, which does not have
processes because it's out of scope."

### No roots table

- date: 2026-09-04

The roots table was invented by a session on 2026-09-04 as the upstream half of a cut
criterion and never ruled; its incidents were dated records standing in for rules, its
rules constraints rather than reasons. The chain of activities replaced it; constitutional
rules appear only as validator checks.

### Mode, not actor

- date: 2026-09-04

Brian: "agent runs autonomously with replaced instructions. session runs autonomously with
story planner's claude.md and skills available. brian is really just human in the loop. I'm
not editing text files." Claude Code CLI is the environment and appears in no table. Mode by
decision: `hitl` if a decision that is Brian's is made during the process; `session` if he
only starts it and reads its output; `agent` under an inlined instrument. Every `hitl`
process writes the artifact that records the decision, "into the artifact the process
already writes."

### A process splits only at a change of mode or when it invokes the runner

- date: 2026-09-04

Brian: "I like the assertion." One session under one activity file is one process however
many steps the prose lists. Process ids are short slugs of their own.

### No instructions column

- date: 2026-09-04

Brian: "Do instructions go on processes or activities?" On activities: the activity file is
the instruction source for its session and hitl processes; an agent's instruction is the
instrument it reads; a process invoking the runner is governed by the agent-runner skill.
The column carried nothing.

### `state` is the development state of a process type

- date: 2026-09-04

`built` or `specified`; never the state of a run. Brian: "state is actually
development-state? Not the state of one instance."

### No edges table, no `when`

- date: 2026-09-04

Brian: "Why are 'edges' and 'data flow' two concepts? Do they have to be?" Order is derived
from reads and writes; every "needed" edge turned out to be a missing artifact for an hitl
decision, and every branch condition a predicate on an artifact the successor reads. `when`
dropped: "We will drop it for now and iterate later."

### No gate column

- date: 2026-09-04

Brian: "I didn't propose gates." The per-artifact gate was a session's generalisation of
the one endorsed check, that every path to a hypothesis write passes an hitl process, which
stays a validator rule.

### Artifacts are classes; files are instances

- date: 2026-09-04

Brian: "an artifact is a set. Like a C# class while individual files are objects of the
class." One `corpus` row with seven instances; the hypothesis file is three artifacts,
statement, record and status, because three processes treat its sections under three
disciplines.

### `mutation`, four values, is every artifact's standing policy

- date: 2026-09-04

`in-place`, `succeeded`, `append`, `frozen`. Seven observed write disciplines reduced to
these. Brian: "should we instead change how we plan to operate with writing artifacts
instead of having a mishmash? We should evolve to something more organized and
systematic." Codebooks and reading protocols are `succeeded`, because a superseded
codebook's text had survived only in git.

### Rule 9 is the mutation rule

- date: 2026-09-04

Replacing the "standing versus dated" draft after the reanalysis found that the hypothesis
record is not history but the evidence relationship, a calibration record is a measurement,
and a revision note is a change record. Brian: "the history of hypothesis file and the
history of the other things [do not] have the same meaning and/or purpose."

### Path is a pattern, never prose; artifacts carry a description column

- date: 2026-09-04

Brian: "Why is there prose in the path column?"

### Placeholders are fixed and `<work>` is retired

- date: 2026-09-04

`<instance>`, `<run>`, `NNN`, `N`, later `<date>` and `<Name>`; referee runs live under the
instance they serve; `fanout/referee/` holds only the shared instrument. Brian: "Are
directory names drifting or staying tight?" and "what is work?"

### Activity files have one shape and carry procedure only

- date: 2026-09-04

Title, the enables line, the Processes table, Preconditions as conditions not lists, one
section per process in table order, Never. Brian: "carefully review requirements for these
files after all the discussion, don't tunnel vision on what landed for the first one." As
decided the shape included a generated section; that part is superseded by *Generated text
is files only* below.

### The skill is rebuilt from scratch in a sibling folder and swapped in one commit

- date: 2026-09-04

Brian: "Why would we not build a new skill from scratch from the ground up incrementally?"
`v3-buildout-2`, one activity file per step, validated as it grows. The map is not a
document: the router is the Activities table in SKILL.md, each activity file opens with its
Processes table, `artifacts.md` holds the Artifacts table and every format, and `map.md`
and `state.md` are generated only.

### `fanout/PROTOCOL.md` retires

- date: 2026-09-04

Its order is the map's; its rules move to the agent-runner skill; the host serves the
rendering.

### baselining-a-hypothesis stands as articulated

- date: 2026-09-05

Brian's dated judgment, one hitl process. The activities from here to *the hypothesis
record is the evidence relationship* were decided across the evening of 2026-09-04 and
2026-09-05 and are dated to the day the session ended.

### promoting-checked-candidates is one hitl session

- date: 2026-09-05

No autonomous part. Brian: "When promoting, I will be saying interactively what to promote,
after iterating with analysis." Scope is his, by hypothesis or by round: "Isn't this
activity invoked ad hoc when I feel something should be [decided], and it's one hypothesis
at a time?" The closing line is `outcome` ("outcome is good"), replacing "disposition" after
its legal sense was explained; `held` dropped as redundant with "no outcome line yet". The
source read is the citation check and is required before promoting, not before declining.

### The referee has two inputs and writes a falsifier

- date: 2026-09-05

The current statement and the finding text, no source locator, no excerpt. Brian, on
2026-09-04: "Referee should be discrimination only, two inputs." And: "Shouldn't it be only
the hypothesis's current statement and the candidate's finding? It has no other context at
all. Its only job is making a ruling." The clause is a falsifier, Popper's term, replacing
"discrimination clause". The codebook hash is SHA-256 of the file's bytes, stamped by the
runner.

### An iteration entry is a wording boundary

- date: 2026-09-05

Revision 1's immediate re-referee was never ruled by Brian; his 2026-08-31 ruling had been
re-tag in place. Now: entries are dated and never invalidated; an iteration entry is a
wording boundary; status derives from current-wording entries and may return to `untested`;
the prior findings become iteration candidates consumed by the next round. Brian: "the
history is part of the epistemic data, and revision adds an entry. This seems to imply that
evidence getting promoted and its falsifier are not necessarily live truth." A statement
iterates on evidence only; a lead never rewords a hypothesis. As decided, promoting alone
enabled iterating; that part is superseded by *iterating-a-statement is a root* below.

### writing-candidates-from-verification is its own activity

- date: 2026-09-05

`session` mode.

### A round is one execution of a calibrated instrument

- date: 2026-09-05

conducting-a-verification-round, renamed from running-verification-cells: "cells" pointed
at the union with exploration, "running" at the runner alone. A round starts on Brian's go,
has no plan approval inside it, and anything the instrument does not cover goes back
through preparing. Fully decoupled from exploration.

### preparing-to-verify-a-corpus is split from the round at the autonomous handoff

- date: 2026-09-05

Once per instrument, not per round: itemize, author the codebook against real items,
calibrate on a sample Brian scores blind. It owns the itemizer ("the plan has to make the
enumeration tool, and then run the enumeration"). Renamed from instrumenting-a-corpus
because "instrument" was ambiguous. It is "building a measuring instrument for a corpus,
with your judgment as its reference standard."

### The work matrix retires, and with it the investigator and focused-reader cells

- date: 2026-09-05

Its three functions are carried by mode and activity shape; cell names survive as
descriptions of agent rows. An investigator has no frozen predicate and unmeasured recall;
itemizing is always mechanical, a script or an authored query in Brian's vocabulary, and
relevance is a calibrated classifier predicate. Brian: "Is this even a necessary mode, or is
it just going to lead to drift?"

### Exploration is three activities

- date: 2026-09-05

Named by Brian: preparing-to-explore-a-corpus (hitl), exploring-a-corpus (session or
agent), reviewing-leads (hitl). No instrumenting for exploration. "Arms" corrected to
"slices" as the unit; arms are the optional A/B design over the same slices. Preparing
reads the corpus's question list and Brian's opening question: "Questions are not
hypotheses, are they? And without questions, what is there to discover?" The read-manifest
is renamed arm-key.

### Vocabulary: lead, question, result, finding, evidence, falsifier, outcome

- date: 2026-09-05

Lead is exploration's output; question and question list replace spec and spec pool, with
`predicate` replacing `candidate-predicate`; finding sits on a candidate only. "Synthesis"
retired entirely: an exploration over verified artifacts is an exploration. "Finding" had
been colliding with the pipeline's word and could not name exploration's output.

### Questions are written only by hitl processes

- date: 2026-09-05

Brian: "I don't think asking questions about a corpus can ever be non hitl." The
exploratory pass writes proposed questions into its artifact and the review writes the
list; every hitl activity may write a question; asking-questions-about-a-corpus merged into
reviewing-leads. The `answered-by` and `frozen-into` statuses are derived, never written.

### Post-WU review is not uniform

- date: 2026-09-05

Brian: "'WU review' is no longer a valid uniform thing. Verification and Exploration are
different shapes that were conflated before." Exploration's review is reviewing-leads;
verification's is promotion; a tool's acceptance is inside building.

### The runner is a process inside each invoking activity

- date: 2026-09-05

Not an activity; one runner row per invoking activity, "Not shared"; each activity keeps
its own agent child row. The pilot lives in the preparing activities: calibration is the
codebook's pilot, one job read by Brian is the protocol's; no pilot inside a round.

### building-a-tool is code with tests that serves more than one activity

- date: 2026-09-05

Renamed from building-an-instrument-or-ingest; the itemizer is preparing-to-verify's own.

### consolidating-the-hypothesis-set retires

- date: 2026-09-05

With refereeing in place its reasons are taken one by one: statuses are computed,
re-verification is the referee's, merges and splits are minting plus iterating in any hitl
session, a structural change is a priority reassessment. Brian: "Not sure this is even
necessary anymore now that refereeing is in the setup."

### writing-a-forward-plan retires; the instance registry survives

- date: 2026-09-05

The plan's bookkeeping is derivable into a generated `state.md`; its judgment, what to do
next, was never a document's to hold. Brian: "How do I know what activity to do when? …
Is workplan outdated or does it still serve a purpose?" What survives is the instance
registry, one row per instance, appended at plan approval, because "the c# tool needs
something to work with" and inference from file names is forbidden. Cards, the status board
and "WU N.M" numbering retire; instances are `exploration-of-<corpus>[-n]`,
`round-of-<corpus>-n`, `referee-<n>`. A tool is built as the first task inside the instance
that needs it, never a card of its own.

### revising-the-method stands

- date: 2026-09-05

Draft 1's G10, no text governing a methodology revision, closes when the revision note is
written.

### minting-a-hypothesis reads only the current set

- date: 2026-09-05

Brian: "Why does mint need anything besides the current hypothesis set?" Nothing enables
it; it enables reviewing-leads; the permission to mint in any hitl session is a sentence in
its file, not an edge.

### The referee's preparation is an instance

- date: 2026-09-05

`referee-<n>`, of preparing-to-verify with `candidates` as its corpus, prepared once and
again only when a decision changes the codebook. Brian: "The referee's preparation is
bootstrapping, right? Not standard operating procedure." Bootstrap is a fact about
instances, never about activities.

### A codebook carries no status line

- date: 2026-09-05

Any line is part of the hash; calibrated means a calibration record exists at the hash.
Closes draft 1's G22.

### The hypothesis record is the evidence relationship, not history

- date: 2026-09-05

Carried so the reasoning is not lost; it lands in `artifacts.md` § Hypothesis file.

### The tool is reworked in place

- date: 2026-09-05

By selection. The from-scratch decision applied to the skill because other sessions load
the live one during construction; the tool had no live consumer. Parser, mermaid scanner
and the SKILL.md checks survive; model, reader, validator, graph and renderer are
rewritten; roots, edges, bootstrap, governed-by, the locus grammar and the codebook-example
check are deleted.

### The validator enforces the enables DAG as decided

- date: 2026-09-05

By selection. A cycle in the rows is a finding for Brian, not something the tool is shaped
around.

### `state` ships with the tool, and what it derives

- date: 2026-09-05

By selection. Per instance, the instance-scoped artifacts present on disk and the furthest
process in chain order whose writes all exist; a question is covered by a codebook when a
calibrated version lists its title in the codebook's `## Questions` section, added to the
format with this decision; per hypothesis, the authored status plus a mismatch flag when
the entries after the last iteration imply a different one.

### The read-and-write row check covers frozen artifacts only

- date: 2026-09-05

By selection. A process row listing one artifact under both reads and writes is reported
when that artifact is `frozen`; a `succeeded` one is not, because reading version N to write
N+1 is what succession is. The check examines rows, never files. Refined by *a frozen
series is exempt* below.

### revising-the-method enables the two preparing activities

- date: 2026-09-05

By selection: confirmed. `explore-plan` and `verify-plan` read `skill`, since the plan is
written against the chain's activity files read whole at preparing. Not taken: counting
`skill` as implicitly read by every session and hitl process; a different target for
revising.

### building-a-tool does not enable revising-the-method; the validator is a free name

- date: 2026-09-05

By selection. The session had named the tool's source as `revise`'s instrument, which
backed an edge; Brian chose the free name and no edge. A revision that needs a tool change
still builds it as its first task. The free name was `ProcessMap` and is `DocIntegrity`
since 2026-09-06.

### iterating-a-statement is a root, like minting

- date: 2026-09-05
- supersedes: d-2026-09-05-4 (in part)

By selection. promoting-checked-candidates no longer enables it; nothing does. The decision
to reword, in a promotion session or any hitl session, is the whole trigger and is a
sentence in the file's Preconditions. It keeps enabling refereeing-a-candidate through the
iteration candidates. What stands of the superseded entry: a statement iterates on evidence
only and a lead never rewords. Not taken: iterating enables baselining instead; retiring
iterating as an activity; overturning the DAG.

### A frozen series is exempt from the read-and-write check

- date: 2026-09-05
- supersedes: d-2026-09-05-25 (in part)

By selection, after Brian typed "What is this tool business logic about in the first
place?" A frozen artifact whose path carries `N` or `<date>` is exempt, on the reasoning
that exempted `succeeded`: reading one member to write the next is not an edit, and
flagging it pushes rows to drop a real read. `<run>` is not a series marker. Not taken:
retiring the check; exempting never-read for frozen; calling the revision note
`succeeded`.

### PROTOCOL's rules move into the agent-runner skill; its order is derived

- date: 2026-09-05

Brian: "Proceed with moving protocol.md's rules." The lifecycle table's order is derived
from the process tables and rendered into `map.md`; the rules PROTOCOL.md alone stated went
into the agent-runner skill: the run folder's tree, the stage strip and what it never
judges, the dry run before the pilot, `run.md` as a pointer to its format for every run.

### The supersession audit is the method's second lint

- date: 2026-09-05

Brian: "You are in charge now. Consider the agent runner for skill audit the 2nd level of
linting after the validator tool. Make the appropriate changes and start the blazor server
process but don't kick off anything yet." A wholesale rewrite runs it; a revision that only
changes rows runs the validator alone.

### CORPUS-STATUS.md is copied into the new folder as the working copy

- date: 2026-09-06

Brian: "Proceed." The copy's forward-plan and "verification pass" wording brought to
revision 2 as facts; the original untouched until the swap deletes the old folder; the
corpus ids await unit-176.

### The pre-referee record entries are leads, and none of them stands

- date: 2026-09-06

Brian: "The stuff from the early run of v3 buildout which treated things as evidence must
not stand. Whatever information is there MUST be completely transformed into the rules,
premises, and paradigms of the new methodology; no shortcuts or workarounds are allowed to
stand by the end of this transitory period. Given the prior work on analysis-corpus has been
determined as exploratory work in scope, its insights should become findings." Asked
whether he meant leads: "Yes, they are leads, not findings." The forty entries, WU1.1 23,
WU1.3 14, WU1.2 3, in twenty files, ten above an iteration line, leave the records. None of
the omissions draft's three alternatives was taken, since each left unrefereed lines under a
rule discounting them. Resolves unit-096. Consequence: there is nothing for a retroactive
referee pass to judge; referee-1's calibration sample comes from the first round's
candidates.

### Three retroactive exploration instances

- date: 2026-09-06

Brian chose "(A) Retroactive instances" and typed: "Agreed that the hypothesis files do not
end up empty, they keep their created entries. all 40 'evidence' lines leave. I agree with
havin three retroactive instances, and this resolves the earlier question about what will
be the first instances that will exercise the usefulness of the tool's verb."
`exploration-of-analysis-corpus`, `exploration-of-own-fiction`, and a Keep instance whose
corpus id is assigned when its row is written, appended to the registry with their real
dates; each gets a leads artifact authored from its synthesis document; every `evidence`
line leaves the fifty records, `created` entries stay, status recomputed to `untested`. Not
taken: the synthesis documents standing as leads with no instance; removal with no
re-homing. Open: whether the six `iteration` entries stay; the Keep corpus id.

### Document integrity is a deterministic binary at the write boundary

- date: 2026-09-06

Brian: "The reason I think tool use hooks is a structural change instead of another
escalation of abstractions is: we identified in past sessions that Claude Code CLI is not an
instrument. It is the environment. […] The tool use hooks are constraints on the previously
unconstrained environment of everything, and they are C# binaries, not the model checking
its own outputs." The design's two axes, his: readability, "a human can plausibly read and
review it efficiently in order to make rulings/judgements", and enforcement, "enforced by
system instructions versus enforced by a binary. There is no unenforced." The medium: "in
markdown, text is everything. It's the model's input and output and what is on disk and
read by me. sqlite and json have other stuff going on." So: markdown as the one medium; a C#
tool, "not fragile powershell"; stateless verbs, "keeping the C# program stateless with one
input (verbs and parameters) and one output"; a PostToolUse hook on Edit and Write running
the check in the same turn, "PostToolUse hook for writing rulings sounds good." Not taken: a
derived SQLite store; a separate hooks program.

### The tool is StoryPlanner.DocIntegrity under process-docs/

- date: 2026-09-06

Brian: "Schema is too generic; can be confused with a sqlite schema." "I like DocIntegrity."
The folder by selection: `process-docs/`, whose object is governing Claude Code's writes of
process documents, data that need a schema enforcing their shape. Renamed and moved from
`tools/StoryPlanner.ProcessMap` the same night.

### Itemizers get their own top-level folder

- date: 2026-09-06

Brian: "I think itemizers should become its own top level folder." One project per corpus
that needs a tool rather than a script, built as the first task of the instance that needs
it. Whether VoiceAttribution moves there is open.

### No separate hooks program; the plan-mode script stays

- date: 2026-09-06

Brian: "I don't think a new Hooks program belongs here either. Perhaps it can stay a
powershell script, unless it ought to be extended by other needs." The hook verb reads the
event's JSON itself, so the settings entry names the exe with no wrapper.

### The agent runner stays where it is

- date: 2026-09-06

Brian: "AgentRunner seems unique, unrelated." It constrains the agents' environment by a
different mechanism, toolset and launch folder enforced by harness flags, and needs no home
but the one it has.

### The omissions adjudication is deferred behind a working example of the hook

- date: 2026-09-06

Brian: "I think this should be deferred because I want a working example of the paradigm
of the post tool use hook first, and use artifacts.md as that. Then we will come back to the
rulings and omissions data." The first pass: "a first pass StoryPlanner.DocIntegrity which
hooks into post tool use on write and edit for certain file patterns which will enshrine the
equivalent of artifacts.md into code for the process docs surrounding the v3 buildout
apparatus. Note that whatever is there in that file right now is naive, written without
knowledge of the doc integrity paradigm." Landed and verified live the same night.

### Generated text is files only

- date: 2026-09-06
- supersedes: d-2026-09-04-2 (in part) d-2026-09-04-20 (in part)

Brian: "I agree with the generated files change." The `level-1` and `activity` sections
leave the authored files, `map.md` gains one section per activity, the marker writer is
deleted, `map.md` and `state.md` are denied to sessions by path in the project settings,
and the hook rewrites `map.md` after every passing validate, measured at a tenth of a
second. Lost: the diagram beside the table in an editor, and the derived summary inline for
a session reading an activity file whole.

### The hook is not described in CLAUDE.md

- date: 2026-09-06

Brian: "I don't think the hook should go in claude.md since it is v3 buildout specific." Its
homes are its own message and the skill's § Schema; CLAUDE.md's file-tools rule is the
repo-wide half and stands as written.

### decisions.md is founded

- date: 2026-09-06

Brian: "Decisions is good." "I don't want special cases." "I'm not going to be editing this
file by hand." "Things that are already superceded need not enter." "Proceed." One file for
every revision, sections by revision, every entry conforming from the first line; the
rulings log closed under its own name; the entry a title, a date, an optional `supersedes`
line naming decision ids only, and prose; ids `d-<date>-<n>` derived by the tool, never
written by hand; written only during revising-the-method, read only there, never cited from
a standard-operating activity file, the validator refusing the id pattern anywhere else in
the skill folder. Founded with every decision then standing, transcribed; superseded ones
left in the closed records. Not taken: one log per revision; a pre-format section with
derived-only ids; a "before the log" section; a `decide` verb; a status field; an id line;
a `by` field, since every entry is Brian's. Alternatives to the word: ruling, resolution,
determination; "decision" taken as the neutral term whose shape is the architecture
decision record with its words kept.

### The file is self-contained; what a decision resolves is prose

- date: 2026-09-06

Brian: "Resolves shouldn't point outside the file? It should be self contained, with
Resolved being prose?" Confirmed. The machine-read lines refer to nothing outside the file;
what a decision resolved, an audit unit, a gap, an engineering item, is a sentence in the
prose. The `open` verb is dropped: Brian, "what should the open verb be doing, if
anything?", answered nothing, since a session can join the tally and this file by grep when
an adjudication needs it.

### Nothing enters this file without Brian's approval; what survives a session is asked for

- date: 2026-09-06

Brian: "I don't think anything lands into decisons.md autonomously. Everything is approved
by me. The historical pattern of these is, at the end of a session, I ask for what must
survive past it. The sessions ended up writing these ad hoc 'rulings' bullet lists in
handoff documents. The skill should say the standard operating procedure is to get these
into decisions.md, after my approval." So SKILL.md carries the procedure: at a session's
end, or when asked, a session drafts what must survive and presents it; a decision about
the method enters here after approval; an instance's conclusions enter its own artifacts;
nothing survives in a handoff.

### Edges into the terminus are exempt from the data-flow check

- date: 2026-09-06

Decided by a session on 2026-09-05 during the tool rework, approved 2026-09-06. The
terminus owns no processes, so no data flow can back an edge into it; the check reports the
edge as vacuous rather than failing.

### An instrument name that is not an artifact id is information

- date: 2026-09-06

Decided by a session on 2026-09-05, approved 2026-09-06. A typo cannot otherwise be told
from a free name such as `dotnet`, `git`, `runner`, `DocIntegrity`.

### The activity file's shape is checked

- date: 2026-09-06

Decided by a session on 2026-09-05, approved 2026-09-06. The title is the activity id; the
level-two sections are Preconditions, one per process id in table order, Never.

### A path cell carrying an alternative is a failure

- date: 2026-09-06

Decided by a session on 2026-09-05, approved 2026-09-06. § Schema's "one pattern, never
prose" checked: a cell carrying ", or" or " and" is a finding.

### The path cells and placeholders of 2026-09-05

- date: 2026-09-06

Decided by a session on 2026-09-05 while fixing the first validate's failures, approved
2026-09-06. `codebook` and `calibration-record` are one pattern each under
`fanout/<instance>/`, with `<instance>` the instance's folder, every `referee-<n>` sharing
`referee`; `itemizer` is `fanout/<instance>/itemize.*`, a tool-project itemizer being
`tool-source`; `runner-skill` is an artifact written by `build` and `revise`; `tool-source`
is `tools/StoryPlanner.<Name>/`; `<date>` and `<Name>` are placeholders.

### `state` is written by no process, and the free names stand

- date: 2026-09-06

Decided by a session on 2026-09-05, approved 2026-09-06. The tool generates `state.md`
whenever a session runs `state`, so the validator's never-written note on it is
informational; `DocIntegrity`, `dotnet`, `git` and `runner` are free names.

### The state verb cannot see referee runs; open

- date: 2026-09-06

Noticed by a session on 2026-09-05, approved as an open item 2026-09-06. Referee runs live
under `fanout/<instance>/referee/<run>/`, but the run-scoped patterns carry one `<run>`
segment, so `state` will not count a referee run's artifacts for the round it serves. Two
fixes were offered in the engineering handoff; neither is decided.

### Revision-2 vocabulary in the agent-runner skill

- date: 2026-09-06

Decided by a session on 2026-09-05 under "Proceed with moving protocol.md's rules", approved
2026-09-06. Instances for WUs, `codebook-N.md`, `protocol-N.md`, `calibration-<date>.md`,
`agent` processes for cells, slice readers for reading arms, investigators gone, the referee
given no excerpt.

### The audit is two process rows and one protocol artifact

- date: 2026-09-06

Decided by a session on 2026-09-05 under "You are in charge now", approved 2026-09-06.
`audit-run` (session; the runner, a generator and a tallier) and `audit-judge` (agent; the
only writer of results), with `revise` adjudicating the tally; the artifact
`audit-protocol` at `fanout/skill-audits/protocol.md`; `skill-audits` admitted to the
`<instance>` placeholder as a work outside the buildout's instances.

### Set B never holds the decision record or a retired instrument

- date: 2026-09-06

Decided by a session on 2026-09-05 under the same delegation, approved 2026-09-06. Intent
is applied at adjudication, never given to the auditor; a retired instrument in B lets its
own rules pass as preserved, which is why the referee codebook left set B.

### fanout/README.md is a pointer

- date: 2026-09-06

Decided by a session on 2026-09-05, approved 2026-09-06. What the folder is; layout and
rules in the agent-runner skill; the order in `map.md`. Not added to document A: it carried
no rule of its own, and a second copy of the tree is the stale-mirror failure.

### Handoff 2's node comparison is void; draft 1's gaps get dispositions

- date: 2026-09-06

Decided by a session on 2026-09-06, approved the same day. Draft 1 has 109 node ids and the
map 67 with none shared, and the audit already mapped the text; the step became one
disposition per gap G1 to G24, drafted for confirmation.

### No migration tool

- date: 2026-09-06

Decided by a session, approved 2026-09-06. The transformations the re-founding needs are
Edit-sized and `validate` is their acceptance; a dry run and atomicity are git's. The
reasoning's second half, derived ids for the old log's entries, was overtaken by the
founding.

### A gap has three homes and no file of its own

- date: 2026-09-06

Decided by a session, approved 2026-09-06. A gap found by a lint is in the lint's output; a
gap closed is in the prose of the decision that closed it; work owed is in the revision
note's owed section or in the plan of the instance that needs the tool. A file whose purpose
is to hold gaps is scratch: draft 1's gap table, the omissions draft, the engineering
handoff's status and the handoffs' owed lists were that pattern. The omissions draft's
fourteen proposals are read once when the adjudication resumes and the draft is then
stamped; the engineering handoff's open items each become a decision or are declined in one.

### artifacts.md stays one file, and its example blocks are fixtures

- date: 2026-09-06

Decided by a session, approved 2026-09-06. The table is one table by schema; the formats sit
beside it because the table's `format` column names them; each format's example block is
the fixture its checker is tested against, so a format edited without its checker fails a
test. It becomes something else only if a format is ever rendered from code.

### The six iteration entries stay through the re-founding

- date: 2026-09-06

Brian chose "Keep them". They record that the statement changed on those dates; removing
them would make each `created` entry read as a description of the current wording, which it
is not. Nothing above an iteration entry counts toward status, and the checker holds them
to shape only, `- iteration | <timestamp>:`. Not taken: removal on the ground that the
rewordings were made on leads, which revision 2 forbids going forward.

### Corpus ids are the question-list filenames, tabled at the top of CORPUS-STATUS

- date: 2026-09-06
- supersedes: d-2026-09-06-4 (in part)

Brian: "filenames as ids, and keep is too ambiguous. Use google-keep. I also don't think
analysis-corpus is good. It should be fimfiction related." Of the possibilities offered,
`fimfiction-stories`: "since not all are strictly my favorites." The eight:
`fimfiction-stories`, `own-fiction`, `v1-archive`, `working-plan`, `lineage`,
`conversations`, `code-sessions`, `google-keep`. Placement is unit-176's alternative (c), a
table at the top of CORPUS-STATUS.md, because four of the eight are the MCP server's corpora
with no section of their own, so "beside each section" could not name them. Resolves
unit-176. Instance names follow: the retroactive instances are
`exploration-of-fimfiction-stories`, `exploration-of-own-fiction` and
`exploration-of-google-keep`, superseding the names in the retroactive-instances decision;
the question list for the 112 stories is `questions/fimfiction-stories.md` when the pools
move.

### CORPUS-STATUS.md becomes CORPORA.md, a fact file with no state in it

- date: 2026-09-06

Brian asked whether the file should be "a factual reflection of the current state, with no
transient parts like readiness or progress", and chose "Yes, as described". One entry per
corpus: its id, what it is, where it lives, how it is read, its caveats. Progress lines
("Analyzed (WU1.3)", "Assessed (WU1.2)", "Current") leave: they are the retroactive
instances' registry rows, and state.md derives progress from the registry and the artifacts
on disk. Readiness is restated as a fact about how a corpus is read, "read by: nothing; the
Takeout export at <path>" included, with no date and no "pending"; the dates go to this
record. Deferrals and judgments ("Defer unless Brian prioritizes", "to be confirmed by
Brian") leave. Counts in prose leave in favour of naming the source of truth, per the
repo's doc rule. The entry's fixed shape is a format, `artifacts.md` § Corpora, and the
file joins the governed set with a checker at its first write under the format, which is
this rewrite. Artifact id `corpora`; `build` keeps writing it, since making a corpus
readable changes a fact in it. Done in the re-founding increment, in the same commit as
the registry rows that take the progress lines over. Not taken: keeping the readiness
dates as a compromise.

### A checker's first run over real records is predicted before it runs

- date: 2026-09-06

Approved by selection. Before a new record checker is armed, the session writes the
expected verdict over the real instances from the decisions and the format: which files
fail, on which rules, and why that is right. The checker runs once and the two are
compared; a discrepancy is either a wrong derivation of the shape or a decision not yet
made, and goes to Brian before anything is enforced. First applied to the hypothesis-file
checker on 2026-09-06: twenty files predicted to fail on citation form and missing
falsifier, no status mismatch, thirty to pass; found exactly that. The rule is in
building-a-tool § build.

### A check is earned by an observed failure

- date: 2026-09-06

Approved by selection. The tool grows by the failures the runs and the instances show,
never by the failures a session can imagine. On 2026-09-06 a derived database, a separate
hooks program, a migration tool and a pre-write mutation check were each proposed before
any run had shown the failure they prevent, and each was refused; the post-write check on
the skill folder was built because a shell write past the hook had been observed that
night. The rule is in building-a-tool § Preconditions.

### Decisions are made one at a time, from options with their trade-offs

- date: 2026-09-06

Approved by selection. Brian's instruction: "Instead of attempting to sign off on the first
protocol you went with, reason through the possible options, their pros and cons, and
present the options for me to decide." And: "One decision at a time." A session presents a
set of options with what each costs and buys, and never one protocol for sign-off; a
recommendation is marked as the session's and listed first. The rule is in
revising-the-method § revise and in SKILL.md's paragraph on what survives a session.

### Study replaces instance; governed file replaces the file sense; record keeps one sense

- date: 2026-09-06
- supersedes: d-2026-09-04-15 (in part) d-2026-09-05-16 (in part)

Brian: "I'm starting to think instance is also too ambiguous." Ordinary English makes an
instance a member of a class, which is the file sense, while the skill had reserved the
word for one run of one activity chain over one corpus; the two senses collided the night
files got checkers. Now: a chain run is a **study**, the registry is `studies.md` (artifact
`studies`, placeholder `<study>`, `fanout/<study>/`), the concrete ids `exploration-of-…`
and `round-of-…` unchanged; a file of an artifact class with a format is a **governed
file**, which is what a checker checks; "instance" and "instances" leave the method
entirely, and "record" leaves as a general word, keeping its one ruled sense, the
hypothesis file's `## Record` section. What stands of the superseded entries: an artifact
is a class and the files matching its pattern are its files; the registry replaces the
forward plan. Of the alternatives, inquiry, experiment, and keeping instance with the file
sense renamed, "study" was chosen as plain and used nowhere else. The rename lands in the
skill, the agent-runner skill, the tool and its tests next session, under the hook, before
the re-founding writes the registry.

### The tool has three verbs: check, render, hook

- date: 2026-09-06

Approved by selection of "A: check / render / hook". `check <path>` checks the governed
files at the path, following the artifacts table: a skill folder gets the thirteen checks of
the method's shape, one file gets its class's format, a folder gets every governed file
under it, so `check .` is the repository and the pre-commit gate's call. `render
<skill-folder>` writes every generated file, `map.md` and `state.md`, so the hook's
regeneration on a pass covers both and state follows record writes. `hook` is the harness
entry, reading the event and doing what the write implies: check, and on a pass render.
`nodes` retires, its purpose voided with the draft-1 comparison; `validate`, `records` and
`state` fold into the three. The four reasons `validate` and the whole-set check stayed
apart survive as scoping: a folder argument bounds what is checked, so the skill folder
stays green while the hypotheses fail until the re-founding. Not taken: renaming only,
leaving two verbs for checking shape and two for deriving files; folding the hook into
`check` as a flag, which would hide a side effect in a check. Executed next session with
the vocabulary rename.
