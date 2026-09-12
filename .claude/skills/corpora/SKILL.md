---
name: corpora
description: The six corpora around the planner — the working plan, the v1 archive, conversations, source texts, lineage, and code sessions — what each holds, where it lives, which tool reads it, how each is ingested, and the standing rules that keep them apart. Read before working with sources.db or lineage.db, before writing or changing an ingest, before adding an MCP tool over a corpus, or when deciding which corpus answers a question.
---

# The corpora

Six bodies of text sit around the planner. They are **never joined** — not by the MCP tools, not
by the app, not in analysis. Each answers a different question, and a claim sourced from one is
never silently supported by another.

| # | corpus | holds | read by |
|---|---|---|---|
| 1 | **working plan** (v2 `.storyplan`) | the current plan | the app; MCP `*_plan` tools |
| 2 | **v1 archive** (`.storyplan`) | the capture-era dataset and the scene graph | MCP `*_archive` tools |
| 3 | **conversations** | imported AI chat transcripts, with Brian's read states | MCP `search_conversations` / `get_blocks` |
| 4 | **source texts** (`sources.db`) | the published material a citation points at | MCP source-text tools |
| 5 | **lineage** (`lineage.db`) | the founding-era material, four source layers | MCP lineage tools |
| 6 | **code sessions** (`codesessions.db`) | Claude Code transcripts — instrument lineage | `sqlite3` directly; **never** the MCP server |

Corpora 1–3 live inside `.storyplan` files; their semantics are in the `storyplan-data` skill.
Corpus 6 has its own skill, `code-sessions`. This file carries 4 and 5 in full, and the rules
that hold across all six.

## Source text — the fourth corpus, outside the `.storyplan` (2026-08-03)

The published material a citation points at — FiM episode transcripts, the fanfics' chapters,
EaW's per-country flavour text (one unit per localisation key) — is ingested by
`tools/StoryPlanner.SourceTexts` into a standalone `sources.db` (`STORYPLAN_SOURCE_TEXTS`,
Brian's is `Desktop/TLTT Sources.db`, ~52 MB).

**Not in the `.storyplan`**: the app eager-loads its whole database and has no use for prose it
doesn't own, and every `VACUUM INTO` backup would carry it. The MCP server reads it; the WPF app
never opens it.

Joined to the plan **only by `(SourceMaterial.Name, SourceMaterialPart.Code)`** — never by id,
since a reseed must not silently re-point text — and the ingest reports both directions of
mismatch rather than zipping a short list onto a long one. Bodies are streamed per query and
never cached; only a manifest is resident, which is also why this one file carries an index (the
`.storyplan`'s "nothing queries it after load" premise is exactly what is untrue here).

**Acquire as EPUB/structured, never as plain text**: the `.txt` Fimfiction export silently drops
every italic, and this author sets internal monologue in them. Partial coverage is normal and is
reported as coverage, never as a defect — the fic is ongoing, the movie was never transcribed.

**Splitting a Part is authorial**: P&K's two "Wind that Fanned the Flames" chapters are an
ontology of role-vignettes, so their 22 sections were promoted to Parts of their own
(`ch121-queens-scientist`, …) via a **P&K-scoped** seed config — never re-run the full
`source-material.v2.json`, which would recreate the 14 FiM two-parters Brian merged in-app.

**Ingest.** Offline, and separate from DataOps because it writes `sources.db`, not a
`.storyplan` — it opens the plan `Mode=ReadOnly` purely to learn the Work/Part spine.

```
dotnet run --project tools/StoryPlanner.SourceTexts -- <config.json> [--apply] [--work NAME]
```

Dry run prints the full chapter↔Part mapping and both directions of mismatch, and refuses to
write if anything is unresolved. Re-ingest replaces a Work wholesale, so a re-download can shed
chapters that vanished upstream. `STORYPLAN_SOURCE_TEXTS` is **optional** in all three MCP
configs — absent, the source-text tools say so and the rest of the server is unaffected.

## Lineage — the fifth corpus: four layers, ONE db, ONE tool family

2026-08-18; it absorbed the 2026-08-16 `gemini.db` as its first layer, and on 2026-08-27 gained
the pre-AI Google Doc revision history as its zeroth.

The layers:

- **The pre-AI Google Doc revision history** (Apr 2025 – Jan 2026, `tools/StoryPlanner.GDocHistory`)
  — 53 diffs between daily snapshots searched by default, 54 full snapshots retrievable under
  scope `snapshots` only; ids `gdoc:` / `gdoc-snapshot:`.
- **The Gemini web-app conversations** (Sep 2025 – Jun 2026) with their curated weekly reports.
- **The early-2026 Google AI Studio chats**, never imported into Conversations — the populations
  are disjoint by construction, the ingest excluding any raw chat whose `<name>.json` sits in
  `Selected_Chats`, plus an authored `exclude` list for near-miss filenames.
- **NotebookLM captures.**

**One tool family** (`list_lineage` / `search_lineage` / `get_lineage`, source-prefixed ids)
because the caller's question is lineage-shaped — *"where did this come from / when was X
decided"* — not platform-shaped. `STORYPLAN_LINEAGE` replaced `STORYPLAN_GEMINI_CORPUS` in all
three MCP configs, and the four gemini-specific tools retired with it.

Same sidecar pattern as `sources.db`: bodies streamed, manifest-resident, guarded on
`IsConfigured`; a shared `IngestRuns` ledger lets the tools disclose "never ingested" distinctly
from "zero rows", since each ingest creates only its own tables.

**The chain** (2026-08-17): founding chats → v1 archive (absorbed them, plus more) → v1 freeze →
Conversations (post-freeze, unmined) → v2 plan. Lineage is **opt-in archeology** — the default
for any question is the working plan. **Provenance, never ground truth.**

**Per-layer caveats, all mechanical.** Gemini's export is damaged (its APPENDIX-D catalogues it)
and giant plan-paste prompts are stubbed. AI Studio thinking chunks were stripped at ingest and a
Drive-document turn is a placeholder (never captured ≠ withheld); system instructions are
searched only under `scope=system` (boilerplate dedup). **NotebookLM captures carry no timestamps,
so a notebook's date is Brian's authored assignment in the ingest config** — year or year-month
precision; undated = not yet resolved, flagged on every apply run, never inferred from content —
and studio notes are title-only until a capture that opens them exists.

**NotebookLM capture procedure:** manual Ctrl+S of the notebook page with the chat panel scrolled
fully to the TOP (history is server-side lazy-loaded; once loaded the DOM retains all turns) into
`Documents/NotebookLM Captures/`, then a config entry with authored slug + date.

**Ingest — three tools writing ONE `lineage.db`, each replacing only its own tables.**

```
# zeroth layer: the Google Doc revision history
dotnet run --project tools/StoryPlanner.GDocHistory -- \
  tools/StoryPlanner.GDocHistory/configs/gdoc-history.json [--apply]

# gemini layer (source corpus static; a re-run replaces its tables)
dotnet run --project tools/StoryPlanner.GeminiCorpus -- <gemini_markdown_dir> <lineage.db> [--apply]

# AI Studio + NotebookLM layers
dotnet run --project tools/StoryPlanner.Lineage -- \
  tools/StoryPlanner.Lineage/configs/lineage.json [--apply] [--source aistudio|notebooklm]
```

The gdoc ingest reads the 54 merged daily snapshots from
`source_material_references/TLTT Story Plan Revision History (merged)/` and computes line-level
diffs with DiffPlex. The gemini ingest reads `gemini_markdown/corpus_index.json` + entry files +
the sibling `story_development_report/`. The Lineage ingest's dry run names every
included / excluded / dropped chat so the population is eyeballed before a write, refuses on any
unparseable non-ignored candidate, and every apply run re-prints the NotebookLM date status
(undated notebooks are a standing flag, resolved only by an authored `authoredDate` in the
config). Each ingest appends to the shared `IngestRuns` ledger. `STORYPLAN_LINEAGE` is
**optional** in all three MCP configs.

## Code sessions — the sixth corpus

`codesessions.db`, and **deliberately not in the MCP server**: the split mirrors CLAUDE.md's Two
AI roles. Lineage answers *story* lineage for Desktop; this archive answers *instrument* lineage
for Claude Code sessions, which have a shell and query it directly. Its extraction policy,
citation rules, schema, query recipes and ingest are the `code-sessions` skill's. Read that
skill before querying it — the `Typed:` / `Chose:` / `Q:` citation rule in particular, which
governs what may be quoted as Brian's words and what may be inferred from a selection.

## The standing rules

- **Corpora are never joined.** No tool crosses them, and a claim from one is never supported by
  another without saying so. v1 and v2 in particular share no ids, overlap ~40% by name, and no
  join is wanted.
- **Retrieval, not suggestion.** A tool over a corpus answers *"what is here"*, never *"what
  should you do"* or *"what's interesting"*. Never rank parts by likely yield, propose what to
  write from a passage, or propose a citation.
- **Partial coverage is coverage, not a defect**, and is reported as such.
- **Sidecar dbs are optional.** Absent, their tools say so and the rest of the server works.
- **Nothing here is ground truth.** These corpora are provenance; the working plan is the
  current claim.
