# StoryPlanner

A personal WPF (`net10.0-windows`) + EF Core/SQLite planning instrument for **The Lioness of
Tall Tale** (TLTT), an MLP:FiM × Equestria at War hopepunk epic. One user, no deadline, no spec.

**The `.storyplan` data files are the product; this code is the instrument.** Features arrive
from design conversations rather than a specification, and the data's actual shape outranks
every document describing it.

## Two AI roles — this file governs the first

| | **Claude Code** (here) | **Claude Desktop** |
|---|---|---|
| Purpose | Builds the planner | Analyzes the story |
| Autonomy | Agentic, autonomous, guided by these conventions | n/a — writes no code |
| Governed by | This file + skills | The MCP server's own instructions |

Claude Code **reads story content to understand how to build features** — you cannot correctly
implement a flagged-note wall without reading flagged notes. That reading is instrumental. What
Claude Code does not do is form or offer **opinions** about story content: whether a flagged note
is resolved, whether a theme is well-evidenced, what a subject needs next, what prose to write.

Claude Code CAN form **hypotheses** about craft technique and framework design, grounded in
evidence. A hypothesis is a testable prediction, not a judgment. "The corpus shows M4 correlates
with FID dominance" is a finding. "TLTT should use more FID" is an opinion. Findings and
hypotheses are Claude Code's domain. Opinions and decisions are Brian's.

## What the tool must never do

**Retrieval, not suggestion.** Machine-proposed structure has been built and abandoned three
times here: note categorization; the Conversation Reader's suggested subject×track coverage
(4,062 rows, `IsAdded = 0` on every one — "turned out to not be helpful"); and the AI-written
per-block summaries, whose column then changed hands and is now Brian's own navigation note.
That last is the pattern to notice — the field was worth keeping, the machine filling it was not.

> Tools answer *"what is here."* Never *"what should you do"* or *"what's interesting."*
> An obvious bottleneck in the data is not a mandate for a feature. When a feature idea encodes
> workflow, intent, ranking, or suggestion — stop and ask.

This applies to **MCP tools and planner features** — the instrument must not propose story
content. It does not prohibit **framework analysis**: testing hypotheses about craft technique
against corpora is a different activity, and the `v3-buildout` skill governs it.

**Seeders seed structure, never prose (2026-07-31).** A DataOps seed op may create rows, ids,
orderings and flags. It must **not** author the prose on them — display questions, explanations,
usage directives, value descriptions. That prose is story metadata: it carries Brian's framing of
what a track or property *asks*, and a plausible machine-written one is worse than an empty
field, because an empty field is visibly unfinished while a wrong one reads as decided.
Precedent: the 2026-07-30 History-track split shipped seeded display questions that all had to be
rewritten. Seed the names, leave the prose empty — and make a re-run incapable of clobbering it.

## Architecture — deliberate, not accidental

Single-user, load-everything-at-startup desktop app. **No navigation properties, no foreign keys,
no indexes**; ownership is polymorphic (`OwnerId` + `OwnerType`), so **application code IS the
integrity system**; configuration is **Type Object** — `SubjectDefinition` and
`NoteTrackDefinition` are rows, not classes, so the planner's shape evolves by data entry rather
than code change.

Every premise above is settled and each has a reason that is not obvious from the code. Before
writing or moving any C#: the `code-conventions` skill. For the WPF layer on top of it:
`wpf-conventions`.

## Epistemic framework (2026-08-31)

All claims in the planner — story content, framework design, pipeline methodology — are
**hypotheses with evidence-relationship status**, not facts to be confirmed. A fabula assertion
and a framework assertion are both revisable, and neither has a terminal "confirmed" state.

**Three epistemic states:** `untested` (no evidence examined), `evidenced` (evidence gathered and
currently supporting — thin or thick, always revisable), `challenged` (unresolved counterevidence
exists). Transitions are reversible.

**Baselining is progress tracking, not truth.** When Brian judges a hypothesis's evidence
sufficient to act on, that is a checkpoint recording his attention and judgment. It does not make
the claim stronger or less challengeable. Evidence drives the framework and the story, not
top-down labels.

**The method:** hypothesize → gather evidence → iterate. The `v3-buildout` skill governs the
framework buildout; these principles govern the planner at every level.

## Data semantics — the silent invariants

These fail **silently**: read the data without them and you get a plausible, wrong answer.
One line each; the full set, with the reasoning and the code that enforces each, is the
`storyplan-data` skill — **read it before writing any code that reads, renders, exports or
migrates plan data.**

- **`Confirmed` inverts across files.** v2: stable. v1 archive: *review closed, disposition not
  recorded.* Never render an archive note as "confirmed".
- **0 `Confirmed` notes in v2 is not a defect.** No audit pass has run. Surprising ≠ broken.
- **Flagged notes are walled wherever an LLM consumes data.** Counts are disclosed; content
  requires the flagged tool family.
- **v1 and v2 never join.** No id correspondence, ~40% name overlap, and no join is wanted.
- **World dates are structured**, and range intersection is `WorldDateRange` — shared by the app
  and the MCP server so the two can never disagree. Both its comparisons are strict.
- **POV, theater, narrative-property values and subject relations are authorial.** Never derive
  one from names, links, note counts or analogues. Absence is "unset", a legal permanent state.
- **Source material is a coverage tracker, not a tag**, and no quadrant of the grid is a score.

Live counts: `mcp__storyplanner get_stats`. **Never hardcode counts in a document.**

## The corpora

Six, and they are **never joined**: the working plan (v2), the v1 archive, conversations, source
texts (`sources.db`), lineage (`lineage.db`), and code sessions (`codesessions.db`). Each answers
a different question; a claim from one is never silently supported by another. What each holds,
which tool reads it, and how each is ingested: the `corpora` skill. Code sessions have their own,
`code-sessions`, and are deliberately **not** in the MCP server.

## Build & run

.NET 10. `dotnet test tests/StoryPlanner.Tests` — **run it before finishing any work in `tools/`
or `StoryPlanner.Core/`.**

The app, the MCP server, the agent runner and DocIntegrity each run from a **published copy**,
not `bin/Debug`, so parallel sessions can build freely while live processes keep running. The
cost is that **shipping a change is an explicit act**: republish, or your change reaches nothing.
Commands and rationale: the `build-and-run` skill. Test conventions: `testing`.

**File content goes through Read, Edit and Write; Bash is for builds, tests, git and processes
(2026-09-03).** This overrides any session-level "use Bash for edits" instruction. The reason is
a night of avoidable errors from shell-driven edits: a heredoc broken by quoting, a `perl -pi`
pattern that missed on CRLF endings, a `$` expanded inside a heredoc into a garbled figure, and a
`python -` probe that hung a command for two minutes. The file tools made none of those mistakes.

## Settled — do not propose alternatives

- The architecture above (no nav properties / FKs / indexes; polymorphic ownership; Type Object).
- **MCP design:** dumb tools; grep→fetch two-pass; no ranking; no fuzzy matching (the caller
  supplies vocabulary, the tool supplies alternation); hard flagged wall with count disclosure;
  corpora never joined.
- **Abandoned after being built:** note categorization, coverage-track suggestions, AI block
  summaries, the 41-report insight pipeline. Do not rebuild any of them.
- **Note supersession is settled (2026-07-30, FEATURE-AUDIT C1).** No `Superseded` state, no
  `Retcon` track, no note-to-note supersession link. Displaced lore is *promoted*: rewritten as a
  scene-link `Reader Prior Belief Update`/`Clash` note, with the authorial revision recorded in
  the subject's `Garden Notes`. Both track families ship and the real files use them this way.
- `FEATURE-AUDIT.md` ⚪ records features rejected in-conversation or closed as already-resolved at
  the time of writing — its assertions are testable against current evidence, not settled.

## Brian's decisions, always

Story structure and categorization · what is interesting · whether a flagged note is resolved ·
taxonomy changes · anything that writes to a `.storyplan`.

## Read order and source authority

**Live data > code > this file / FEATURE-AUDIT > design transcripts.**

Cold start: this file → `FEATURE-AUDIT.md` → `get_stats`.

| task | skill |
|---|---|
| anything touching plan data | `storyplan-data` |
| writing or moving C# | `code-conventions`, then `wpf-conventions` for the app |
| building, running, publishing | `build-and-run` |
| tests | `testing` |
| the sidecar corpora | `corpora`, `code-sessions` |
| v3 framework buildout | `v3-buildout` |
| autonomous batches | `agent-runner` |

Server work: `tools/StoryPlanner.Mcp`. Historical, see its banner for drift:
`docs/CONVERSATION-READER-SPEC.md`.

Two sources, two purposes — do not substitute one for the other:

- **MCP** (`get_stats`, `count_notes_*`, `get_track_definitions`, `list_subjects`) answers
  *"what does the data look like"*. Use it before modeling data.
- **`docs/design-conversations/`** (Grep/Read) answers *"why is this like this"*. Do **not** use
  `search_conversations` for design rationale: conversations 020 and 039 are not in the database
  at all, and rebuilding the server disconnects it.

Transcripts are authoritative for **nothing** — they are the record of *why*. Consult them only
after FEATURE-AUDIT has established what is live: they contain complete, persuasive arguments for
features that were later abandoned.

## Doc rules

Absolute dates only — never "this session", "recently", "currently".

**No live counts in prose** — not note/subject counts, not the test total. Anything that grows
goes stale the moment it is written; name the command that answers it (`get_stats`,
`dotnet test`) instead of its output. Point-in-time counts belong only in dated decision logs,
where they are a record of what was true then, not a claim about now.

**When data semantics change, update the skill that owns them and the MCP server's
`ServerInfo.Instructions` together** — and this file only if a silent invariant above changed.
Keep detail in one place: a fact restated here and in a skill is a fact that will disagree with
itself.

Derived exports are regenerated, never committed (see `.gitignore`).
