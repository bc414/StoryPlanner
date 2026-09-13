# Corpora

The corpora of the buildout, one section per corpus id: what it is, where it lives, how it
is read, then its caveats. A fact file: no progress and no readiness, which `state.md`
derives from the study registry and the artifacts on disk; no counts, which the named
sources answer. The corpora the MCP server reads carry their semantics in the `corpora` and
`storyplan-data` skills.

## fimfiction-stories

- what: the Fimfiction stories analyzed under the v4 brief, and their analyses; the population is `.claude/skills/analyze-story/populations.md`
- where: the texts as one markdown file per story in `C:\Users\Brian\Documents\Fimfiction Favorites\markdowns\` and `…\markdowns1\`, outside the repo; the per-story analyses and the meta-analysis reports (4.1a, 4.2a–e, 4.3) in `source_material_references/Reading Archive Analyses/`
- read by: files; a runner job takes a story as an input file or an `addDirs` entry

Four stories in the favorites are outside the corpus, unread, abandoned or dropped, named
in `populations.md`. The analyses are the map to what to itemize; a verification's items
are cut from the texts, never from an analysis alone. The `analyze-story` skill's resolution order says
`.txt`; the files are `.md`. Two analytical pipelines produced the analyses, cloud and 1M,
and report 4.3 catalogues their calibration differences. Reader and author material on the
stories sits in `source_material_references/`: `P&K comments.md`,
`pax-chrysalia-comments.md`, `Comments.md`, `Filly Fooling review.txt`,
`Filly Fooling analysis.csv`, `The Princess and the Kaiser - Sheet1.csv`, Brian's tiers in
`corpus-favorites-tiers.txt`, and the TLTT paradigm annotations in
`long-corpus-categories.txt`.

## own-fiction

- what: Brian's six published stories (THLB, Wish, TEatS, NTL, GIYC, Falldale) and the naive TLTT chapters 1–2
- where: plain-text files, italics as `*...*`, in `source_material_references/own_stories_md/`; the naive chapters as markdown in `source_material_references/`; the source epubs in `source_material_references/own_stories_epub/`
- read by: files; the epubs convert through FicEpubReader (`dotnet run --project tools/StoryPlanner.SourceTexts -- --to-markdown`)

The self-diagnostic v4 analyses, one per text (`thlb`, `wish`, `teats`, `ntl`, `giyc`,
`falldale`, `naive-tltt`, each `-1m.txt`), sit in `Reading Archive Analyses/` with
correction notes prepended to four after source verification; the brief additions they ran
under are `docs/analysis-briefs/v4-self-diagnostic-additions.txt`, embedded in the
subagent prompts rather than in a skill. The planning-document revision histories of
KU/NTL, GIYC and Falldale are raw exports in `Planning_Document_Revision_History/`, a mix
of `.txt` from appscript and plain-text copies from Drive, read by nothing; TLTT's own
revision history is in `lineage`.

## google-keep

- what: Brian's Google Keep notes, in a Google Takeout export
- where: `C:/Users/Brian/Documents/Google Drive Analysis/takeout-20260810T030233Z-1-001.zip`, JSON, one file per note, named by its creation timestamp
- read by: nothing; the export is read directly as files, and the five Claude Code analysis artifacts in the same directory are the curatorial guide to it

Notes carrying plaintext credentials are catalogued by date in the security appendix of
`keep-archaeology.html` and are excluded from any ingest. The framework-relevant notes are
a small share of the export; the analysis artifacts name them.

## lineage

- what: the founding-era material: the pre-AI Google Doc revision history, the Gemini conversations and their weekly reports, the AI Studio chats never imported into Conversations, the NotebookLM captures
- where: `lineage.db`, the file `STORYPLAN_LINEAGE` names in the MCP configs
- read by: the MCP server (`list_lineage`, `search_lineage`, `get_lineage`), by source-prefixed id

Provenance, never ground truth; the per-layer caveats and the capture procedure are in
CLAUDE.md.

## v1-archive

- what: the v1 planner's archive `.storyplan`, the capture-era dataset that holds the scene graph
- where: the archive file the MCP configs name
- read by: the MCP server (`*_archive` tools); `tools/StoryPlanner.VoiceAttribution` with the `v1-archive-mining` skill for attribution, its evidence set `docs/v3-framework/WU1.4-v1-scene-instincts/attribution.csv`

`Confirmed` in the archive means review closed with the disposition not recorded; v1 and
v2 never join (CLAUDE.md). Dated snapshots of the v1 database,
`TheLionessOfTallTale yyyy-MM-dd.db` in `source_material_references/v1 sqlite/`,
downloaded from Google Drive, are read raw and immutable by VoiceAttribution, every TEXT
column of every table except `GeminiEntries`, to date each archive note's first
appearance; the 2025-12-23 backup is not among them.

## working-plan

- what: the v2 working plan `.storyplan`
- where: the plan file the MCP configs name
- read by: the MCP server (`*_plan` tools, `get_stats`)

Flagged notes are walled wherever an LLM consumes data; the data semantics are in CLAUDE.md
and the `storyplan-data` skill.

## conversations

- what: the imported Claude Desktop conversations, with Brian's per-block read states and navigation notes
- where: the Conversations tables of the v2 `.storyplan`
- read by: the MCP server (`list_conversations`, `search_conversations`, `get_blocks`)

A block summary is Brian's own navigation note, never a machine's; conversations 020 and
039 are not in the database (CLAUDE.md).

## verified-findings

- what: the standing findings of every verification, each finding entry one item, standing as of the itemizer's run
- where: `docs/v3-framework/studies/<study>/findings.md`, each finding located by its `<study>/<slug>` heading
- read by: files; the claim command of `tools/StoryPlanner.SurfacingItemizer` cuts one study's `findings.md` into one item per standing finding

Explored only, never verified: a verification of it would write findings about findings,
and claiming could promote one beside the finding it is about, counting the same evidence
twice; an exploration writes leads, which never become evidence. Findings of different
verifications rest on different items under different directions, so a pattern read across
them is a lead and never a joined claim. A finding withdrawn after the itemizer's run is
caught at the lead review, against its source, like any lead.

## code-sessions

- what: the sealed archive of Claude Code transcripts from the projects on the ingest's include-list
- where: `codesessions.db`, Brian's at `Desktop/TLTT CodeSessions.db`
- read by: sqlite3 directly, recipes in the `code-sessions` skill; no MCP surface

Human-in-the-loop sessions only; a hook's feedback to the model (a `hook_blocking_error`
attachment) is dropped by the ingest; `Typed:` is Brian's prose and `Chose:` a label he
selected (CLAUDE.md).
