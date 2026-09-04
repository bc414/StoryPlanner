# V3 Buildout — Version History and Provenance

## Origin

On 2026-08-14, Brian set up automated analysis of his Fimfiction favorites during a
vacation week: *"I am going to be on vacation for a week but don't want my Claude
subscription to go to waste."* The analysis framework was not invented for the
pipeline — it was derived from Brian's own shipped track system (the mechanism ×
inference-stage matrix from conv 47's note categorization bootstrapping). The v2
framework, designed to organize the story planner's note tracks, was repurposed as a
measurement instrument applied to 112 other people's stories.

The results surprised. 42 stories under v1-v3 Briefs produced 8 hypotheses about
where the framework was overfit or incomplete. 112 stories under the v4 Brief
produced 7 meta-analysis reports that confirmed some hypotheses and challenged
foundational assumptions: FID as the only delivery mechanism for perception gap,
theme as the only goal category, the P→WI→T chain as universal. The measurement
instrument revealed its own systematic biases when applied at scale.

## Version history

Two things evolve across versions: the **codebase** (the software — WPF app, database
schema, tools, MCP server) and the **narrative design framework** (the conceptual
system that determines what the planner tracks and how stories are analyzed — the
mechanism × inference-stage vocabulary, cognitive modes, track definitions, scope
levels, EditorModes, and the analysis brief that applies this vocabulary to other
stories).

In v0 through v2, the codebase and the narrative design framework evolved together —
each code rewrite was also a framework rewrite. v3 is the first time they diverge:
the v3 codebase shipped in Jul 2026 (MCP server, external corpora, agentic workflows),
and the v3 framework evolves now, on top of the existing code, through evidence
rather than through a rebuild.

**v0 (Apr-Dec 2025):** Single Google Doc (22K→132K chars). Brian wrote naive TLTT
chapters 1-2 and hit a wall: fabula leaking into the syuzhet (worldbuilding delivered
through dialogue, revelation architecture unplanned). The Canalave Library (TCL) website
was a parallel project (Oct-Nov 2025). Gemini web chat began Nov 30-Dec 7 for TLTT.

*Codebase:* none (Google Doc).
*Narrative design framework:* implicit. Brian knew instinctively that stories need
dramatic irony (from Silver, 2014), that worldbuilding should drive plot (from strategy
games and Pokemon writing), and that perspective restriction creates reader engagement
(from THLB's alternating first-person). None of this was named. The naive TLTT chapters
show FID instincts ("as if roses were red"), behavioral proxy ("Is the castle secured?"),
and fabula-through-dialogue as the default delivery mechanism. The Google Doc mixed
fabula, syuzhet plans, chapter drafts, and character notes inline — no separation.

**v1 (Dec 8, 2025 - Apr 25, 2026):** Gemini-built story planner. 42 commits, 17-day
build sprint, served 4.5 months. "Don't lose the thought" — raw capture into free-form
textboxes. NotebookLM (NLM) Perspective Analysis notebook (Feb 2026, 172 turns) and
Refinement of Aquileian Lore notebook ran in parallel; the Aquileian lore overflow
birthed the multi-story fabula paradigm. NLM introduced deep third/FID/DT vocabulary
from analyzing Pokemon stories Brian read (not from P&K — the vocabulary predated
P&K analysis). Late in v1's life, Claude web chat arrived (Apr 9) and immediately
produced insights (Conscience as 6th Element, conv 8) too dense for v1's architecture.

*Codebase:* WPF + EF Core + SQLite, hardcoded navigation properties, free-form
textboxes per entity, no track system.
*Narrative design framework:* partially named. NLM gave Brian vocabulary (deep third,
FID, DT, Architect/Director), and AI Studio gave him fabula/syuzhet separation
(aistudio:6, Mar 27 2026). But the planner had no structure to hold this vocabulary —
the framework lived in NLM notebooks and AI conversations, not in the tool. The
planner was a capture instrument, not a design instrument. v1's notes contain Brian's
instincts alongside Gemini's analytical voice, unseparated.

**v2 (Apr 26 - Jul 18, 2026):** Claude web chat + GitHub Copilot. 54-day build
(17 paradigm + 16 code + 21 bootstrapping). Served ~7 weeks of fabula migration at
subject level. Scene-level work stalled — the FID/perception-gap prescription from
NLM and early Claude conversations made scene design feel premature. Meanwhile Brian
learned Claude Code via The Canalave Library (Jun 13 - Jul 31).

*Codebase:* rebuilt from ground up. Type Object pattern (SubjectDefinition,
NoteTrackDefinition), polymorphic ownership, no navigation properties, data-driven
configuration.
*Narrative design framework:* formally named and systematized. The 5-layer split
(world truth / omniscient timeline / character psychology / narrative architecture /
thematic argument), cognitive mode separation (ZeroFocalization / NarrativeDesign),
113 tracks with display questions, 5 EditorModes (Expansion / Linking / Gardener /
Audit / Scene Design), the P→WI→T inference chain, perception gap taxonomy (ironic /
tragic / closing / aligned). Derived from 5 Claude conversations totaling 571 blocks +
NLM notebooks + 42 analyzed stories. Untested against broader evidence.

**v3 codebase (Jul 19 - present):** Born from TCL's Claude Code apprenticeship.
Conversation Reader (Jul 19), CLAUDE.md + MCP server (Jul 28), explosive 4-day feature
burst (Jul 28-31), external corpora absorbed (Aug 16-26). The MCP server is the v3
paradigm: external corpora (lineage, conversations, source texts, code sessions) live
OUTSIDE the .storyplan, queryable but not mixed with Brian's notes. The analysis
pipeline (112 stories, Aug 18-28) is the first experiment testing v2's framework
assertions against evidence.

**v3 framework buildout (Aug 29 - present):** The v3 codebase enables evidence-based
framework revision for the first time. v0 was observation (unnamed instinct). v1
was partial naming (vocabulary from NLM/AI Studio without structure to hold it). v2
was hypothesis formation (formal system from conversations, untested). v3 adds
experimentation: test hypotheses against 112 analyzed stories + Brian's own fiction +
v1 archive mining + planning doc revision history + framework provenance. Governed by
the `v3-buildout` skill and the epistemic framework in CLAUDE.md.

## Provenance table

Which concept first appeared where and when:

| Concept | First appeared | Source |
|---|---|---|
| Fabula/syuzhet separation | Mar 27, 2026 | AI Studio `aistudio:6` |
| Architect/Gardener | Feb 2026 | NLM `nlm:3` |
| Deep third / FID / DT distinction | Feb 2026 | NLM `nlm:3` t#54 (Bandits of the Forest) |
| Variable focalization | Feb 2026 | NLM `nlm:3` t#94 |
| 5-layer split | Apr 15, 2026 | Conv 17 |
| Perception gap taxonomy | Apr 20, 2026 | Conv 21 |
| P→WI→T chain | May 11, 2026 | Conv 47 |
| Gardening the architecture | May 4, 2026 | Conv 36 (Brian's phrase) |
| Conscience as 6th Element | Apr 10, 2026 | Conv 8 |
