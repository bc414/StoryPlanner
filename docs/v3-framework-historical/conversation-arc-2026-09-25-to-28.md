# Conversation arc — the two purposes, the harness, and what compute can't do

One Claude Code session in this repo, `a1ea0453-bc16-4c8a-b43e-cc0886982bec`, from
2026-09-25T23:42Z to 2026-09-28T17:32Z (UTC throughout). There were two sittings:
2026-09-25/26, then 2026-09-28. Context was compacted more than once. Nothing in the repository
was created or changed during it, apart from this file.

Written 2026-09-28 at Brian's request, from the transcript. **Quotation marks hold Brian's own
typed words**, verbatim and extracted from his user turns. His own quotation marks are shown as
single ones. Everything outside quotation marks is the session's account. Where the session
claimed something and Brian corrected it, the correction is recorded next to the claim. Counts
are what the session measured on the date given, and are not current.

This is history and nothing here is settled. The session's proposals were ideas put to Brian,
and most were corrected or declined.

---

## 1 · The opening frame (2026-09-25/26)

Brian opened with a design anchor: "the Story Planner serves two distinct purposes: holding
the planner data so that it can serve AI analysis and AI-assisted expansion of the fabula, and
helping me write the written story." He had in mind a custom harness around `claude -p` calls,
with MCP access and structured JSON output that the app captures into its own silo. The test
would be writing the first few chapters of *The Kitty of Westkeep* (TKOW). He asked for the
minimal critical path and for what to study first.

**First two corrections.**
- The session sent the harness through AgentRunner. Brian: "It has nothing to do with agent
  runner. Agent runner is only for buildout work." The harness is app-side planner work, which
  sits outside the v3 buildout. He also said: "I'm probably going to cut the whole hypotheses
  and referee part of the process so ignore those for now."
- The session treated the Export configuration as a live harness. Brian: "v2's export with Id,
  onwertype and anchors was never used, because I discovered the MCP server capabilities
  after." The paste went straight from v1's full paste to MCP tool calls.

**The design that formed.**
- **The caller inverts.** Brian: "Yes, the caller inverts." The C# app calls `claude -p` and
  collects the output. Transport is a swappable seam in case the terms of service change. The
  fallbacks are a pasteable prompt, or the app auto-detecting dropped JSON. The MCP server
  stays read-only and the app is the only writer.
- **Stateless questions instead of long chats.** "Then I don't even need long lived
  conversations of back and forth turns; the claude -p can be stateless and my custom UI can
  make branches of individual questions instead of bunching multiple questions."
- **Curation had been hidden inside transport.** "the copy and paste was curation." The v1
  paste did two jobs at once. MCP took over transport, and nothing took over curation. On the
  harness: "Previously, the data I could do whatever I wanted but the 'harness' was fixed as a
  web chat."
- **The hard rule is a read-surface partition, not a write rule.** "What the hard rule is: the
  part of the story planner that is accessible during 'writing mode' is only allowed to be
  mine." The Conversations import already lives in the `.storyplan`.
- **No decomposed notes.** "I do not want the structured json to be broken up into formatted
  and decoupled notes because that invites copy/paste, which is not allowed." Dense analysis,
  of the kind in conv:8 (Applejack's Evolved Element of Honesty), is what he valued.
- "I'm thinking of leaving the multi-turn chat paradigm entirely, so think really outside the
  box."

**Session findings in this stretch:**
- The Conversations corpus splits at 2026-07-28, the MCP server's first commit.
- A conversation's title describes only its first prompt. Brian pointed out that the
  instrument-slug conversations "exploded into huge fabula building sessions". The session
  checked conv:75, 76, 78 and 88, and that held.
- The Rarity folder (`~/.claude/projects/c--Users-Brian-Documents-Rarity/`) is story analysis
  over MCP and the closest record of the target loop.
- conv:8 block 21 is Brian's own Rainbow Dash arc design, sitting unread. The session read this
  as the durable content living in his follow-up turns.

## 2 · Git, then transactions

Brian brought git in as an analogy: "I'm thinking a question is a push onto the stack, and a
pop is data that goes into notes *in a staging area*." Then he renamed it: "Instead of commit
let's use transaction, from relational database terminology." The transaction picked up these
properties, in his turns:

- **Rebase.** "No, the transactions have to 'rebase' when another one lands into the working
  write-view".
- **Edits as delete plus add**, with a new id each time. The session added that the recorded
  (deleted id → created id) pair is what makes the transaction archive read like a diff.
- **The transaction archive** is queryable and stands in for version history.
- **Order starts at the root.** The first transaction would be the Unified Theory of Magic,
  followed by species magic, then civilization systems and histories. It covers only what is
  upstream of Minette's first chapters.
- **Tracks are not settled.** He noted that much of TKOW's material sits in v1 and in
  conversations rather than in v2. The session had looked only at v2. A search of v1 and
  conversations then found hundreds of matching notes and blocks.

Brian asked where the transaction design and revising-the-method came from. The session traced
the queue/stack/frame discipline to Brian's own turns of 2026-09-05 to 09-08
(`d-2026-09-07-12`, `d-2026-09-08-20`). It said those were written only from the Claude Code
seat, never from the fabula seat. Brian's reading: the transaction "is meant to replace the long-lived session+bottom
to top read". He also said conv:88 was retroactive recovery, which is a different activity from
live staging. On the manual annotation: "I didn't actually end up using the manual annotation;
not because it was bad, but because I went on vacation".

## 3 · Archaeology of the old loop

- **v1 began as retroactive capture.** "the v1 story planner actually *started* as a means of
  retroactive data capture". Git history agreed: Initial Commit on 2025-12-08, and a Gemini
  entry importer with a reading UI by 2025-12-13.
- **The bottom-to-top method, in his words**, is at `gemini:2246` (2026-02-27), a five-step
  procedure. Searching for `bottom to top` missed it and searching for `wild new ideas` found
  it. The session took this as a live case of the caller supplying the vocabulary.
- **The Gem design prompt** is at `gemini:2254` (2026-02-28). Brian: "the Gem design prompt is a
  useful baseline to iterate off of."
- **Note Organizer / Sorter** is at `aistudio:22–25` (2026-02-21/22). Structured JSON
  extraction was built and later abandoned. Brian: "Note Categorization was an attempt to win
  the previous fight of data capture after the analytical conversation was done. I lost all
  those fights. Now I'm changing the game by building a custom harness." The session's reading
  was that the plumbing is the same but authorship is reversed: in the new design the machine
  analyses and he writes the notes.
- "The 7 orthogonal axes were superseded by the 5 layer model that became labels on note
  tracks."
- **The Faust vs Hasbro rework** is at `aistudio:68` and its branch `aistudio:67` (2026-04-03,
  gemini-3.1-pro-preview), and it took the Stagnation of Harmony from 1000 to 80 years. The
  branch's system prompt contains an anti-drift clause. The session read that clause as the
  staging area written as a plea to the model. The prompt also holds a four-category spark
  taxonomy and a NO PROSE GENERATION constraint.
- **The v1 paste itself** (`TheLionessOfTallTale.db.md`) has two parts, a world bible and a
  narrative timeline. It cites with `[[Name]]` wikilinks, has three typed link payloads, and a
  STORY THREADS section. Brian: "But the plot points and links actually contained data that
  ought to be fabula - that is the v1 cognitive mixing problem and copy/paste problem".

## 4 · Dreamscape and the shape of a transaction

Brian asked for the Rarity sessions to be read: Investiture `8be775b1` and Dreamscape
`29cb7eac`. "Don't sample dreamscape, read it all." Then: "Just all the user turns should be
read."

- The session began building on the model's recurring `Notes to update` tables. Brian: "By the
  way the notes to update was unsanctioned." The session dropped them.
- The session argued a transaction could not be a session. Brian: "I do think a transaction is
  a session." He described the shape as a queue of areas where each settled baseline sparks the
  next. The session added that a stack appears only where the model builds on premises he has
  not ratified. Dreamscape's turn 17 is the example.
- The session said Dreamscape was left open. Brian: "I don't think anything was left open. I
  stopped because I was satisfied. In a chat paradigm, there's no way to signal that."
- "Coltbert's Aquileia already had a materialist historicist buildout." That node was retrieval,
  not derivation. And: "my go to way of writing follow ups is to start typing as I'm reading."
- On nodes and edges: "The edges have payloads." The AI's response is one opaque node that is
  non-authoritative by type. The nodes he types are staged notes, sparked questions and
  constraint violations, each anchored to a span.
- Brian then made the representation a question for study: "The question for study probably
  shouldn't anchor on any existing data model?" The session laid out seven alternatives: no
  structure, an event log, an anchored document, polymorphic rows, a tree, typed turns and a
  claim table. It drafted `deliberation-turn-relations`. That draft is recorded only in this
  transcript and was never written to `questions.md`.

## 5 · The critical path (2026-09-28, second sitting)

Brian came back after the autonomous exploration campaign had run.

- The order was set by his constraints. "I'm not going to write Minette's first chapters until
  the Aquileian fabula is stable." Stable means "No flagged notes related to the areas that
  involve the first few chapters that I'm going to write". Also: "A lot of Aquileia is still in
  v1". And on scope: "I've narrowed that to just Aquileia, but that doesn't mean skipping other
  steps."
- On ordering, a correction: "before the two designs, I need to review leads for explorations
  that could inform the new data model." And: "it's not just reviewing leads, it's making new
  studies accordingly too".
- **Migration and expansion are one mechanism.** "Expansion is not 'open ended'. It's
  specified by how many notes are flagged as needing more deliberation."
- **v3 starts empty.** "during migration, *everything* in v1 and v2 should be treated as
  suspect and subject to change." And: "The old ones don't so they don't get to pretend to be
  v3 fidelity".
- **No disposition ledger.** "My instinct says doing any sort of bookkeeping on the loose data
  of old sources will be futile and unproductive". The session had proposed the ledger and
  withdrew it. It moved to exhaustive coverage at the area grain, with retrieval inside each
  area.
- **Published prose.** "Published serial fanfiction on a website can be edited, unlike a
  printed book that is sold for money", with the subtext "I can edit whatever isn't used as
  foundation for other things (such as reader prior belief)".
- "In v2, every link track is a story thread". The session traced this to conv:17/21 and to
  block:529, which is Brian's, marked done.
- **Scope cut.** "Also a core thing for the v3 data model is actually potentially scope cut from
  what v2 did, because MCP server exists now." "Analogies seem to be the materialist historicist
  citation equivalent of Canon." "theme tracks on subject is good, links and scenes not
  necessary". Ordering bug: "No TKOW text exists until ch1 is written by hand."
- **The rule against skipping.** "Nothing goes on this conversation alone. We can't skip
  studies." The session then audited its own claims under that rule. It had presented block:642
  and block:652 as already answered in April, but both are assistant turns in blocks Brian skipped.
  The session withdrew them as answers.
- **Where the theme-link cut came from.** Brian: "this goes to a code session". The session
  found his turns of 2026-09-17, which lay out most of the v3 direction: the scope cut, two
  fabulas, threads as subjects, and a checker-like constraint on model output.

## 6 · The leads audit

At Brian's request, four subagents read every leads file. The session's summary: roughly 10 of
21 studies asked their own question. Self-authored write-ups were nearly universal. Free-text
aggregation measured the readers rather than the corpora. In `notes-mix-cognitive-modes`, the
directions listed 10 of 12 modes, and the session confirmed that against the file. The three
write-ups of the technique study disagreed by about 30× on the same data.
`subject-modes-in-their-tracks` stood out as a template that works: closed placement fields,
whole-subject items, full coverage and a blind write-up.

Brian: "Should I just stop doing tons of AI analysis on the corpora and just build some v3 and
run it?" Then the reframe: "what I need to optimize is my attention required". And: "The goal
is to spark as many ideas in the least hitl time possible, for the v3 story planner design."

On past words: "It is not ground truth today, it was ground truth at the time. Is today's
working authority something computable or not?" The session's answer was that it is not
computable for the past, and that v3 transactions would make it computable going forward.

## 7 · Why it's taken ten months

"V1 was originally going to be a winter project for a couple months until I could go outside
again."

The session offered seven angles. The one Brian corrected was "no consumer":

- "The process is definitely part of the destination."
- "So the planner did have a consumer for 10 months, the expansion loop"
- "My hope for a custom harness is that it reduces the need for redirects". The session counted
  12 where-to-look redirects in Dreamscape's 99 genuine turns.
- "Tkow will not have prose fragments. Only TLTT in v1 has them."
- "v1 was probably designed more for the loop than for the future writer."
- "a custom one with Claude -p is unmatched and always iterable"

He pointed the session at the escalation ladder session (2026-09-06), the
architectural-gardening conversation, and the v1 paratext note on using AI. He asked: "I
suppose since I set a hard constraint that ai can't generate prose that constrains a lot?" The
session found the ladder in his words. It found conv:36 block 1229 on writing linearly and on
updating published scenes incrementally. The session's reading of that block was that rendering
can always change, while meaning can change only until readers build on it.

## 8 · What compute can and can't do

Brian's HITL budget was three evenings before a vacation from 2026-10-01 to 10-12. After that
he would only send `continue` remotely. The session proposed a prototype transaction.
Brian: "No, it shouldn't be any prototypes at all. The next two weeks should be just for trying
to get ideas that will allow me to discover what to build, what design choices to make." Then:
"what can this compute do and what can it not do?"

- The session proposed three zones: verifiable, generative (acceptable only when labelled as
  sparks) and judgment.
- It built a matrix of eras against story ideas and planner code. Brian corrected it:
  - Eras 5 and 6 are the same as era 4 for story work. He moved Investiture to Claude Code only
    to keep seeing thinking summaries.
  - The buildout "stopped being fun because idea sparking into design decisions got interfered
    with review of generative content masquerading as fact".
  - "the MCP server's innovation is removing the burden of capture before more expansion could
    come. Its effect on the writer consumer is untested."
- **What v1 was for.** "In v1, there was no conception of a separate fabula that doesn't reach
  the page." And: "The fabula layer are design intents, not inference." The session found
  block:1983 (conv:58, 2026-05-30), in his words: everything in subjects had to reach a plot
  point somewhere.
- **v2 and scenes.** "v2 didn't retire scenes. It didn't get to the point of deliberation." And:
  "v1 treating subjects as a *staging area* is a critical insight". The session found this
  written into v1's data: floating plot points (39 of 450 unplaced), `IsIncorporated` (added
  2026-01-07), and triage labels (`Complete` holds 3 empty rows).
- **The story of the reworks.** The Stagnation rework (2026-04-03), then Conscience (conv:8,
  2026-04-09/10), then the commit `feat: Add audit old text functionality` (2026-04-10). "Conscience in v2 is really
  just the 2nd half of Sabotage in v1." The Faust lens traces to Gemini on 2026-01-20
  (`gemini:869–874`). The session verified a Chrysalis scene-note burst in v2 starting
  2026-07-27, the same week as the MCP server's first commit and the last active week of TCL.
- "So I think the 'reversals are costly when more was built on it' is potentially *impossible*
  to design away."
- **Compute history.** "I only got Claude Max on June 23." He bought it for The Canalave
  Library. He added: "Perhaps trying to use all this compute that I purchased for story planner
  is missing the point, that story planner was originally always HITL attention constrained,
  not computed constrained."
- **Architectural gardening.** "deliberating scene details (even without writing them out) does
  change the fabula". The session withdrew its claim that a commit settles fabula and placement
  only happens at writing time.
- **Replacing attention.** The session answered his question about this with the line between
  pointer-shaped and claim-shaped output. Compute replaces attention only when its output is
  cheap to verify. Every abandoned feature was claim-shaped.
- **How the harness helped.** "one is a constraint that the harness doesn't give the model the
  whole plan context, which forced me to articulate what I wanted." And: "I have to step back to
  what the situation was back then." That led to mining his past turns, then asking him.
- **Presets.** "a future custom harness would have presets for the different things I want,
  which are reusable, and the set can grow."
- **Stubbed prompts.** "What was stubbed out can be found in the v1 codebase." The session
  located v1's prompt template in git history. It also found that lineage excludes Gemini
  technology-programming entries that are not tagged creative-writing.

## 9 · The hook question and the close

Brian: "how can we get something like the hook as a hard constraint to prevent futher
assumptions of intent from unbacked inference". The session proposed a Stop hook that verifies
quotes plus heuristics about intent. Brian: "This seems fragile and sloppy with a lot of
unintended consequences." He asked what the docs hook had solved, and whether the problem here
had been identified.

The session answered that the problem had not been identified. It said the missing piece is a
current written statement of what he wants. Brian corrected what the docs hook actually does:
"The docs hook doesn't check decisions.md; it can't because it's only C# code. It only checked
the schema grammar." And: "Perhaps part of the goal is to have to stop correcting. Not through
praying a model gets smarter or follows instructions, but by some structural move like the
hook."

**The session's final reading, offered and not ratified:**
- The hook holds grammar and nothing more.
- Most of this conversation's corrections were content only Brian had, not errors of form.
- For that kind of content, the only structural move is to *remove the slot*: an output grammar
  with nowhere to assert his intent. It would hold only pointers to his record, checkable
  verbatim, and fixed-template questions.
- Chat prose has no grammar. `claude -p --json-schema` is a grammar the CLI enforces, so the fix
  lives in the harness.
- It would not stop him supplying what only he knows. It would stop him having to refute first.

## Where it ended

Decided by Brian in this conversation, in his words above:

- The harness is planner work, and the caller inverts.
- Transactions: rebase, delete plus add, a transaction is a session.
- v3 starts empty and old data is suspect.
- No bookkeeping over old sources.
- The writing-mode read surface is his alone, and AI never writes prose.
- No prototypes in the two weeks before 2026-10-12.
- Attention is the constraint.
- Presets.

Open when it ended:

- The transaction's representation.
- Verification's fate, which waits on reviewing the leads.
- Which leads are redone, and how.
- The mining of his past turns, and the question format for putting what it finds to him.
- Whether any structural remove-the-slot mechanism gets built, and where.
- The drafted `deliberation-turn-relations` question, never written to the question list.
