# V3 Buildout — Project History (facts only)

This document records verifiable facts about the project's timeline and
architecture. Interpretive claims (why things happened, where concepts came
from, what characterizes each era) are hypotheses — they live in hypothesis
files, not here. The draft1 version of this file mixed facts with
interpretations; this version retains only what is verifiable from git, lineage,
the codebase, or Brian's own dated statements.

## Timeline

**v0 (Apr-Dec 2025):** Single Google Doc, grew from 22K to 132K chars (verifiable
from gdoc revision history). Brian wrote naive TLTT chapters 1-2 (in
`source_material_references/` as markdown). The Canalave Library (TCL) website was
a parallel project (Oct-Nov 2025). Gemini web chat began Nov 30-Dec 7 for TLTT
(verifiable from lineage).

**v1 (Dec 8, 2025 - Apr 25, 2026):** WPF + EF Core + SQLite app. 42 commits,
17-day build sprint, served 4.5 months. Architecture: hardcoded navigation
properties, free-form textboxes per entity, no track system. NotebookLM (NLM)
Perspective Analysis notebook ran in parallel (Feb 2026, 172 turns, lineage
`nlm:3`). Refinement of Aquileian Lore notebook also ran in parallel (lineage
`nlm:6`). AI Studio fabula session: `aistudio:6`, Mar 27 2026. Claude web chat
arrived Apr 9 2026 (conv 8: Conscience as 6th Element). V1 archive contains 5,843
notes, all untracked (predates the track system); 450 TLTT plot points, 1,125
links.

**v2 (Apr 26 - Jul 18, 2026):** Claude web chat + GitHub Copilot. 54-day build
(17 paradigm + 16 code + 21 bootstrapping). Architecture: rebuilt from ground up
with Type Object pattern (SubjectDefinition, NoteTrackDefinition), polymorphic
ownership, no navigation properties, data-driven configuration. Framework
artifacts: 5-layer split, ZeroFocalization/NarrativeDesign cognitive modes, 113
tracks with display questions, 5 EditorModes, P→WI→T inference chain, perception
gap taxonomy (ironic/tragic/closing/aligned). Derived from 5 Claude conversations
(conv 17, 21, 36, 47, 53; totaling 571 blocks) + NLM notebooks + 42 analyzed
stories. Brian learned Claude Code via TCL (Jun 13 - Jul 31).

**v3 codebase (Jul 19 - present):** Conversation Reader (Jul 19). CLAUDE.md + MCP
server (Jul 28). Feature burst Jul 28-31. External corpora absorbed Aug 16-26
(lineage.db, codesessions.db). Architecture: MCP server as sidecar (external
corpora queryable outside the .storyplan). Analysis pipeline: 112 stories analyzed
Aug 18-28 under v4 Brief, producing 7 meta-analysis reports (4.1a, 4.2a-e, 4.3).

**v3 framework buildout (Aug 29 - present):** Governed by the `v3-buildout` skill
and the epistemic framework in CLAUDE.md. Hypothesis files and forward plans in
`docs/v3-framework/`.

## Architectural facts

- V1 → v2: navigation properties removed, free-form textboxes replaced by
  polymorphic note ownership (OwnerId + OwnerType), Type Object pattern adopted
- V2 → v3 codebase: MCP server added, external corpora (lineage.db, sources.db,
  codesessions.db) created as sidecars outside the .storyplan
- The .storyplan is SQLite in WAL mode, eager-loaded at startup
- No foreign keys, no indexes, no navigation properties (by design — see CLAUDE.md
  architecture section)

## Key conversations (verifiable from MCP)

| Conversation | Date | Blocks | Topic (from metadata, not interpretation) |
|---|---|---|---|
| Conv 8 | Apr 10, 2026 | — | Conscience as 6th Element |
| Conv 17 | Apr 15, 2026 | — | Multi-story fabula |
| Conv 21 | Apr 20, 2026 | 151 | Perception gap + data architecture |
| Conv 36 | May 4, 2026 | 72 | Planning vs writing |
| Conv 47 | May 11, 2026 | 285 | Note categorization bootstrapping |
| Conv 53 | — | — | Track taxonomy |
| Conv 64 | — | 289 | P&K ASOIAF inspirations (multi-topic) |

## What is NOT in this document

The following were in the draft1 version and are interpretive claims, not facts:

- Why v0 "hit a wall" (diagnosis of fabula-through-dialogue) → hypothesis 040
- Where vocabulary came from (NLM vs P&K provenance) → unverified, candidate for
  consolidation plan
- Why v2 scene-level work stalled → hypothesis 019
- What characterizes each era's paradigm ("capture instrument," "design
  instrument") → hypothesis 002
- The "v0=observation, v1=naming, v2=hypothesis, v3=experimentation" arc →
  hypothesis 002
- Whether v3 is "the first time" codebase and framework diverge → challenged by
  hypothesis 007
- The provenance table (which concept "first appeared" where) → unverified
  provenance claims, moved to consolidation plan for verification
