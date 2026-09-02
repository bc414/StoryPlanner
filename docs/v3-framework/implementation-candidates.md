# Implementation Candidates

Proposed codebase changes gated on v3 framework hypotheses. Each candidate
becomes actionable when its gating hypotheses are baselined. Actionable
candidates enter the normal development process (CLAUDE.md, wpf-conventions,
FEATURE-AUDIT for the decision record).

This is a living document — new candidates are added as they emerge during
buildout work. It is not a backlog with priority ordering; the forward plan
handles prioritization of experiments, and Brian decides implementation order
when candidates become actionable.

## Candidates

### Voice linting protocol

**Gated on:** 019 (ai-context-contradiction), 020 (voice-separation-prerequisite)

Identify legacy AI voice in v2 working-plan notes by grepping .storyplan note
text against the lineage corpus. Flag matches as AI-originated. Either remove
or rewrite in Brian's voice. WU2b's Gemini-voice separation methodology is the
prototype for this. The goal: the .storyplan contains only Brian's voices; AI
analytical voice is retrievable via MCP sidecars but not embedded in the notes.

Includes the more specific proposals from the design conversation:
- Copy-paste detection DataOp (D19): go through each note, flag spans as
  copy-pasted from lineage or conversations, store state + start/end indexes
- UI surfacing (D20): visual treatment (gradient color or similar) to make
  AI-originated text visually distinct in the planner

Source: synthesis plan downstream section (2026-08-29), Keep dump D19/D20
(2026-08-30)

### NoteState vocabulary change

**Gated on:** 003 (epistemic-vocabulary-for-content)

Replace or extend the v2 NoteState enum (Unset / Confirmed / Flagged) to
reflect the v3 epistemic vocabulary (untested / evidenced / challenged). May be
a rename of existing values, a new parallel system, or a migration. The v1
archive's Confirmed retains its distinct meaning and is permanently outside this
change's scope. Audit mode's semantics change alongside (hypothesis 004).

Source: design conversation (2026-08-31)

### Codebase architecture for dimensional annotations

**Gated on:** 024 (dimensional-vs-hierarchical)

If the framework decision is that reader-experience-moments are dimensional (not
hierarchical), the existing codebase patterns — NarrativePropertyValue
(single-select from closed vocabulary), NoteSourceReference (many-to-many
junction), nullable scalar properties on Note — may serve without new
architecture. Whether to use these, whether a third data-driven level is needed
(GoalCategory as data rows instead of TrackType enum), and whether note-to-note
design edges (hypothesis 043) require a new NoteRelation table are code design
questions downstream of the framework decision.

Source: synthesis plan downstream section (2026-08-29)

### Instructional text audit

**Gated on:** 014 (evidence-based-instruction-design)

Iterative review of the full instructional text stack against v3 framework
findings. Two consumers:
- Claude Code: CLAUDE.md → skills → MCP ServerInfo.Instructions (never audited
  by Brian — first-draft "binary help text")
- Claude Desktop: Project prompt (doesn't exist yet) → Project skills → MCP
  ServerInfo.Instructions (shared with Code)

The MCP server's instructions are shared infrastructure — improvements benefit
both consumers. The Desktop project prompt would be the v3 descendant of v1's
TLTT Analyzer Gem's four rules.

Source: synthesis plan downstream section (2026-08-29)

### Audit mode redesign

**Gated on:** 004 (working-cadence-sweeps), 003 (epistemic-vocabulary-for-content)

Audit mode was designed for note-by-note promotion to Confirmed. Under the v3
framework: Confirmed becomes "baselined" (progress tracking, not finality), and
the natural cadence may be batch sweeps rather than note-by-note. The mode's
purpose, what it surfaces, and what actions it offers need rethinking.

Source: design conversation (2026-08-31)

### Keep sidecar ingest

**Gated on:** 045 (keep-notes-provenance) — already evidenced

Selective ingest of provenance-relevant Google Keep notes into `lineage.db`,
following the sidecar pattern (own tables, `IngestRuns` ledger, `keep:` source-
prefixed ids, authored include-list config). Source: 5,583 notes in the Google
Takeout at `C:/Users/Brian/Documents/Google Drive Analysis/`. Three content
categories: (1) content-unique notes (no lineage echo — full ingest),
(2) timestamp-unique notes (content in lineage via Dec 2025 copy-paste into
Gemini, Keep adds prior capture timestamp — metadata ingest), (3) non-unique
(skip). ~130 credential-containing notes must be excluded.

**Timing:** Build before WU1.5 (Retrospective), which lists it as a
precondition. WU1.5 traces provenance chains that need pre-AI timestamps from
Keep to establish when framework concepts formed before AI engagement. WU1.8
(Planning Evolution) also benefits — for pre-Gemini stories (KU/NTL, GIYC),
Keep is the only source of Brian's thinking alongside the planning doc
revisions. Can be built in parallel with WU1.1 (corpus synthesis), which has
no dependency on Keep. The existing Claude Code analysis artifacts
(`C:/Users/Brian/Documents/Google Drive Analysis/`) provide the curatorial
guide for the authored include-list.

Source: WU1.2 assessment (2026-08-31)

## Tasks (not feature proposals — gated on framework decisions but not codebase changes)

These are content/data tasks that become relevant when framework decisions are
made. They don't go through FEATURE-AUDIT; they're Brian's authorial work.

- **V1 archive migration:** Migrate Brian's scene-level instincts from v1 into
  v3 .storyplan notes, in Brian's voice (not Gemini's). Gated on: framework
  decisions (which tracks exist) + hypothesis 020 (voice separation).
- **Conversation content migration:** Unmigrated fabula from conversations
  (Chrysalis, Aquileia, etc.) into v3 .storyplan. Gated on: framework decisions.
- **New story entities:** Story entities for planned stories not yet in v2
  (Daring Do, Applejack's Parents, etc.). Gated on: nothing — Brian can add
  these whenever, but they inform WU6 (connection to TLTT project).
