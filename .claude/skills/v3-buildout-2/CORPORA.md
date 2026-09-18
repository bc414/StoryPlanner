# Corpora

The corpora of the buildout, one entry per corpus id: what it is, where it lives, how an
itemizer reads it, then its caveats. A corpus is primary-source data, as SKILL.md § Vocabulary
defines it; a lossy summary or report made afterwards from it is not corpus data. A fact file:
no progress and no readiness, which `state.md` derives from the study registry and the
artifacts on disk; no counts, which the named sources answer. How a session or the MCP server
reads a corpus is not here: the `corpora`, `storyplan-data` and `code-sessions` skills carry it
for the corpora they cover, and the rest are files.

### fimfiction-stories

- what: the Fimfiction stories analyzed under the v4 brief, whose population is `.claude/skills/analyze-story/populations.md`, plus The Princess and the Kaiser, added on 2026-09-14; and Brian's supplementary material on them
- where: the texts as one markdown file per story, `<slug>.md`, in `source_material_references/fimfiction-favorites/`, gitignored with the rest of that folder, converted on 2026-09-14 by the converter in `tools/StoryPlanner.SourceTexts` (`--to-markdown`) from the epubs in `C:\Users\Brian\Documents\Fimfiction Favorites\epubs\` and `…\epubs1\`, and for pax-chrysalia and the-princess-and-the-kaiser from epubs Brian placed in the folder and deleted after conversion; the supplementary material in `source_material_references/`
- read through: the story files as text, one file per story, each chapter under a `## Chapter N — <title>` heading, cut into one item per chapter by `tools/StoryPlanner.ChapterItemizer` under its config `configs/fimfiction-favorites.json`, which names the folder and the excluded stories, or into one item per story whole under `configs/fimfiction-favorites-stories.json`; the supplementary material as markdown, plain-text and CSV files, with no reader in the repo
- caveats: The two stories Brian has not read, fallout-equestria and your-human-and-you, are
  not in the folder (his ruling of 2026-09-14); their text exports sit at the top of
  `C:\Users\Brian\Documents\Fimfiction Favorites\`. green and romance-reports are in.
  the-princess-and-the-kaiser is not in the v4 population of
  `populations.md`. Three stories lost a `-1` suffix at the 2026-09-14 conversion:
  maidens-day, nightly-rendezvous, twilights-list. The supplementary material on the stories in
  `source_material_references/`:
  `P&K comments.md`, `pax-chrysalia-comments.md`, `Comments.md`, `Filly Fooling review.txt`,
  `Filly Fooling analysis.csv`, `The Princess and the Kaiser - Sheet1.csv`, Brian's tiers in
  `corpus-favorites-tiers.txt`, and the TLTT paradigm annotations in
  `long-corpus-categories.txt`. The v4 per-story analyses and the meta-analysis reports made
  from the stories are not corpus data.

### own-fiction

- what: Brian's six published stories (THLB, Wish, TEatS, NTL, GIYC, Falldale) and the naive TLTT chapters 1–2
- where: plain-text files, italics as `*...*`, in `source_material_references/own_stories_md/`; the naive chapters as markdown in `source_material_references/`; the source epubs in `source_material_references/own_stories_epub/`
- read through: the plain-text and markdown files as text; the six stories' `.txt` files are in the converter's format, each chapter under a `## Chapter N — <title>` heading, and `tools/StoryPlanner.ChapterItemizer` cuts them under its config `configs/own-fiction.json`, which leaves the naive chapters out; the epubs converted first by FicEpubReader in `tools/StoryPlanner.SourceTexts` (`dotnet run --project tools/StoryPlanner.SourceTexts -- --to-markdown`)
- caveats: The planning-document revision histories of KU/NTL, GIYC and Falldale are raw exports in
  `Planning_Document_Revision_History/`, a mix of `.txt` from appscript and plain-text copies
  from Drive, with no reader in the repo; TLTT's own revision history is in `lineage`. The
  self-diagnostic analyses made from the stories are not corpus data.

### google-keep

- what: Brian's Google Keep notes, in a Google Takeout export
- where: `C:/Users/Brian/Documents/Google Drive Analysis/takeout-20260810T030233Z-1-001.zip`, JSON, one file per note, named by its creation timestamp
- read through: the JSON files inside the zip, one note each, with a JSON reader; no reader in the repo
- caveats: Notes carrying plaintext credentials are catalogued by date in the security appendix of
  `keep-archaeology.html` and are excluded from any ingest and from any itemizer's items. The
  framework-relevant notes are a small share of the export. The five Claude Code analysis
  artifacts in the export's directory, which name them, are not corpus data. The notes Brian
  pasted into `source_material_references/hypotheses-google-keep-dump.md` are his own words
  under rule 10 and are not this corpus.

### lineage

- what: the founding-era material: the pre-AI Google Doc revision history, the Gemini conversations, the AI Studio chats never imported into Conversations, the NotebookLM captures
- where: `lineage.db`, the file `STORYPLAN_LINEAGE` names in the MCP configs
- read through: a SQLite database, opened with a SQLite client; its layers, tables and source-prefixed ids are documented in the `corpora` skill; written by the ingest `tools/StoryPlanner.Lineage`, the Google Doc layer by `tools/StoryPlanner.GDocHistory`
- caveats: Provenance, never ground truth; the per-layer caveats and the capture procedure are in
  CLAUDE.md. The Gemini weekly reports the database also holds are not corpus data. A Gemini
  entry is one prompt and its response; its thread and position come from the Gemini corpus
  index, whose grouping method the repo does not document; `activity` entries are Canvas
  records with no prompt, not dialogue. An AI Studio user turn that is a placeholder stands for
  an attached Drive document that was never captured. The Google
  Doc layer's line diffs, computed beside its snapshots, are a lossless view and how the layer
  is read.

### v1-archive

- what: the v1 planner's archive `.storyplan`, the capture-era dataset that holds the scene graph
- where: the archive file the MCP configs name, `STORYPLAN_ARCHIVE`; the dated snapshots of the v1 database, `TheLionessOfTallTale yyyy-MM-dd.db` in `source_material_references/v1 sqlite/`, downloaded from Google Drive
- read through: the archive as a SQLite database in the planner's schema, opened through `AppDbContext` in `StoryPlanner.Core` or a SQLite client; the semantics of its values are in the `storyplan-data` skill and in the `v1-archive-mining` skill for attribution. A snapshot as a SQLite database in the v1 planner's own schema, opened read-only and immutable: `Chapters`, `PlotPoints` (synopsis, outcome, stakes), the entities `Characters`, `CodexEntries`, `Themes`, `Threads` and `Locations`, and four link tables, `PlotPointCharacters`, `PlotPointThemes`, `PlotPointThreads` and `PlotPointCodexEntries`, each link with its own payload text; the reader is `NativeV1` in `tools/StoryPlanner.V1Itemizer`, whose summary gives the columns and the v1 enums' names
- caveats: `Confirmed` in the archive means review closed with the disposition not recorded (CLAUDE.md);
  v1 and v2 rows never correspond by id or by name (the `storyplan-data` skill). The archive is
  the 2026-04-18 snapshot converted in 2026-05 into the planner's schema and then reviewed: the
  conversion split text fields into notes at their blank lines, as a plot point's synopsis
  shows, merged the four link kinds into untyped links, held the entity kinds only as subjects,
  and dropped the links' enum values; the subject types, which are the review's triage labels,
  and the note states have no column in the native schema. A native note carries no plot point
  id: it hangs off a chapter, an entity or a source material. `tools/StoryPlanner.VoiceAttribution`
  reads the snapshots raw, every TEXT column of every table except `GeminiEntries`, to date each
  archive note's first appearance; the 2025-12-23 backup is not among them.

### working-plan

- what: the v2 working plan `.storyplan`
- where: the plan file the MCP configs name, `STORYPLAN_WORKING`
- read through: a SQLite database in the planner's schema, opened through `AppDbContext` in `StoryPlanner.Core` or a SQLite client; the semantics of its values are in the `storyplan-data` skill
- caveats: Flagged notes are walled wherever an LLM consumes data (CLAUDE.md), and an item is
  consumed by one, so an itemizer never puts a flagged note's content into an item; the data
  semantics are in CLAUDE.md and the `storyplan-data` skill.

### conversations

- what: the imported Claude Desktop conversations, with Brian's per-block read states
- where: the Conversations tables of the v2 `.storyplan`
- read through: the same database as working-plan, its `Conversations` and `ConversationBlocks` tables, through `AppDbContext` in `StoryPlanner.Core` or a SQLite client
- caveats: A conversation's arc summary and its block summaries are not corpus data. A block summary
  is Brian's own navigation note, never a machine's; conversations 020 and 039 are not in the
  database (CLAUDE.md). A block the import marks as a compaction, `IsCompaction`, is not
  corpus data.

### code-sessions

- what: the sealed archive of Claude Code transcripts from the projects on the ingest's include-list
- where: `codesessions.db`, Brian's at `Desktop/TLTT CodeSessions.db`
- read through: a SQLite database, `Sessions` and `Records`, opened with a SQLite client; the schema, the reading rules and the query recipes are in the `code-sessions` skill; written by the ingest `tools/StoryPlanner.CodeSessions`
- caveats: Human-in-the-loop session trees only, subagent sessions included. A record's role is
  who authored it, from extract version 3 (`Sessions.ExtractVersion`, 2026-09-14): `user` is
  Brian, his action markers included; `assistant` is the model, a subagent's parent-written
  turns included; `harness` is what the harness put in front of the model, a skill load, a
  notification, local command output, kept verbatim. A compaction summary is not corpus data
  and is never stored; a `harness` marker stands where it was. A session still at version 2
  carries all of that in the `user` role, and an itemizer over it excludes those records by
  their openings, as the `code-sessions` skill lists them. A hook's feedback to the model (a
  `hook_blocking_error` attachment) is dropped by the ingest; `Typed:` is Brian's prose and
  `Chose:` a label he selected (CLAUDE.md).
