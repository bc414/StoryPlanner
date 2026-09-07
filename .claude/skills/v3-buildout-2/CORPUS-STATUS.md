# V3 Buildout — Corpus and Material Status

Updated as analyses complete and new material is added. A fact file, not a rule: read by
the preparing activities when an instance needs specific inputs, and by building-a-tool
when a corpus becomes readable or changes state.

## Corpus ids

The ids the question lists, the instance registry and every instance folder are named
by. Four corpora are the MCP server's and are described in CLAUDE.md rather than in a
section here.

| id | corpus | where described |
|---|---|---|
| fimfiction-stories | the 112 analyzed Fimfiction stories and their analyses | § 112-story analysis corpus |
| own-fiction | Brian's six published stories and the naive TLTT chapters | § Brian's own fiction |
| google-keep | Brian's Google Keep notes, in the Takeout export | § Google Keep notes |
| lineage | the founding-era material in `lineage.db` | § Lineage corpus |
| v1-archive | the v1 planner's archive `.storyplan` | CLAUDE.md; readiness in § Instruments and readiness |
| working-plan | the v2 working plan | CLAUDE.md |
| conversations | the imported Claude conversations | CLAUDE.md |
| code-sessions | `codesessions.db`, the sealed Claude Code transcript archive | CLAUDE.md |

## 112-story analysis corpus

59 short-corpus (cloud v4 analyses), 53 long-corpus (1M local analyses). 7 meta-
analysis reports (4.1a, 4.2a-e, 4.3). All in
`source_material_references/Reading Archive Analyses/`.

Ground truth for populations: `.claude/skills/analyze-story/populations.md`.

**Story texts** (what a verification round's items are cut from — never the analysis alone): local
markdown conversions, one `<name>.md` per story, in
`C:\Users\Brian\Documents\Fimfiction Favorites\markdowns\` and `…\markdowns1\` (the
`analyze-story` skill's resolution order; it says `.txt`, the files are `.md`). Outside the repo, so a
runner job that reads one gets it as an input file or an `addDirs` entry.

**Unanalyzed stories (4 Tier 3):**
- green (Steel Resolve, 2,656 KB) — Brian read it; was in NLM Perspective Analysis;
  Wings of Dew draws from it. Large. Defer unless Brian prioritizes.
- fallout-equestria (3,509 KB) — Brian has NOT read it. Dropped from active corpus.
- your-human-and-you (3,834 KB) — Brian read snippets early on, doesn't remember.
  Dropped from active corpus.
- romance-reports (1,049 KB) — Abandoned by Brian; partial analysis may cover
  unread content. Existing analysis kept; flagged in favorites as Abandoned.

## Brian's own fiction

6 published stories + naive TLTT chapters. Italics verified 2026-08-31.

Plain-text files (`.txt`, italics as `*...*`) in
`source_material_references/own_stories_md/`:
- THLB (Pokemon, 2015-2017, first-person alternating POV)
- Wish (Pokemon, 2020, one-shot)
- TEatS (Pokemon, 2020-2021)
- NTL (Pokemon, 2021-2023, unfinished)
- GIYC (MLP, 2024-2025, unfinished)
- Falldale (EaW, Nov 2025, one-shot)
- Naive TLTT Ch1-2 (June 2025) — in `source_material_references/` as markdown

Source epubs in `source_material_references/own_stories_epub/`. Converted via
FicEpubReader (`dotnet run --project tools/StoryPlanner.SourceTexts -- --to-markdown`).

Status: **Analyzed (WU1.3, 2026-09-01).** Seven self-diagnostic v4 analyses in
`Reading Archive Analyses/` (`thlb`, `wish`, `teats`, `ntl`, `giyc`,
`falldale`, `naive-tltt`, each `-1m.txt`); correction notes prepended to four
after source verification. Synthesis: `docs/v3-framework/WU1.3-own-voice-synthesis.md`.
Brief additions used: `docs/analysis-briefs/v4-self-diagnostic-additions.txt`.

## Supplementary material

- `P&K comments.md` — 25 comments, chapter-by-chapter reader reactions
- `pax-chrysalia-comments.md` — 15 comments + author replies
- `Comments.md` — index + inline comments for other stories
- `Filly Fooling review.txt` — ~2,500-word essay with proposed structural revision
- `Filly Fooling analysis.csv` — chapter-by-chapter spreadsheet of three parallel arcs
- `The Princess and the Kaiser - Sheet1.csv` — contents to be confirmed by Brian
- `corpus-favorites-tiers.txt` — Brian's subjective tiers (completed 2026-08-29)
- `long-corpus-categories.txt` — TLTT paradigm annotations

All in `source_material_references/`.

## Planning doc revision histories

- TLTT — already in lineage (53 diffs, `gdoc:` ids)
- KU/NTL, GIYC, Falldale — raw exports in `Planning_Document_Revision_History/`.
  Mix of `.txt` (from appscript) and plain text docs (manually copied from Drive
  to fill gaps from the API's limitations). Preprocessing needed before GDocHistory
  ingest — the instance that needs this material scopes the preprocessing and builds it
  as its first task (building-a-tool), and this entry changes state when it lands.

Status: **Raw data available.** Preprocessing and ingest pending.

## V1 database snapshots

Dated `.db` files in Google Drive, titled `TheLionessOfTallTale[date].db`. Time-
series of the v1 planner's state during v1's lifetime (Dec 2025 – Apr 2026).
Enables temporal analysis: when subjects, notes, and scene-graph links appeared,
and how growth correlates with dated Gemini conversations — useful for voice
attribution (notes appearing after a conversation are copy-paste candidates) and
for tracing the plan's evolution under the full-plan-paste paradigm.

Status: **Local and in use (2026-09-02).** 15 of the 16 Drive snapshots are in
`source_material_references/v1 sqlite/` as `TheLionessOfTallTale yyyy-MM-dd.db`
(2025-12-26 → 2026-04-18; the 2025-12-23 backup not downloaded). Read raw and
immutable by `tools/StoryPlanner.VoiceAttribution` — every TEXT column of every
table except `GeminiEntries` — to date each archive note's first appearance in
the plan (the PlanFirst rule). No schema migration; note ids never joined.

## Google Keep notes

Brian's Google Keep notes: 5,583 notes (Oct 2015 – Aug 2026) in a Google
Takeout export at `C:/Users/Brian/Documents/Google Drive Analysis/
takeout-20260810T030233Z-1-001.zip`. WU1.2 confirmed unique provenance
material (8/8 key moments absent from all lineage layers). Five Claude Code
analysis artifacts in the same directory provide a curatorial guide. ~300–500
notes carry framework provenance value; ~130 contain credentials (must be
excluded from any ingest). Hypothesis 045 → evidenced.

Status: **Assessed (WU1.2, 2026-08-31).** Selective ingest warranted, not yet
built. Recommended approach: authored include-list config following the
NotebookLM/code-sessions precedent.

## Lineage corpus

All layers ingested into `lineage.db`:
- Google Doc revision history (TLTT, 53 diffs + 54 snapshots)
- Gemini web conversations
- AI Studio chats (all accounts — multiple accounts processed 2026-08-31)
- NotebookLM captures

Status: **Current.**

## Instruments and readiness

- Own fiction: analyzed without a skill change — the self-diagnostic additions were
  embedded in subagent prompts (`docs/analysis-briefs/`).
- V1 archive: readable since 2026-09-02 — `tools/StoryPlanner.VoiceAttribution` and the
  `v1-archive-mining` skill. Voice attribution is mechanical (evidence set
  `docs/v3-framework/WU1.4-v1-scene-instincts/attribution.csv`). The reading itself is an
  exploration instance still to be registered (preparing-to-explore-a-corpus); the
  five-arm design drafted for it under revision 1 sits in the retired forward-plan-2 as
  reference, and whether the `v1-archive-mining` skill becomes that instance's
  `protocol-1.md` or stays a skill is an open point (omissions draft, appendix).
  `WU1.4-execution-plan.md` is superseded.
