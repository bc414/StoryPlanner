# V3 Buildout — Corpus and Material Status

Updated as analyses complete and new material is added. Referenced by forward plans
when designing WUs that need specific inputs.

## 112-story analysis corpus

59 short-corpus (cloud v4 analyses), 53 long-corpus (1M local analyses). 7 meta-
analysis reports (4.1a, 4.2a-e, 4.3). All in
`source_material_references/Reading Archive Analyses/`.

Ground truth for populations: `.claude/skills/analyze-story/populations.md`.

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

Markdown files in `source_material_references/open_stories_md/`:
- THLB (Pokemon, 2015-2017, first-person alternating POV)
- Wish (Pokemon, 2020, one-shot)
- TEatS (Pokemon, 2020-2021)
- NTL (Pokemon, 2021-2023, unfinished)
- GIYC (MLP, 2024-2025, unfinished)
- Falldale (EaW, Nov 2025, one-shot)
- Naive TLTT Ch1-2 (June 2025) — in `source_material_references/` as markdown

Source epubs in `source_material_references/own_stories_epub/`. Converted via
FicEpubReader (`dotnet run --project tools/StoryPlanner.SourceTexts -- --to-markdown`).

Status: **Ready.** All texts converted with italics intact.

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
  ingest — the forward plan should note the location and scope the preprocessing.

Status: **Raw data available.** Preprocessing and ingest pending.

## Google Keep notes

Brian's Google Keep notes are in `C:/Users/Brian/Google Drive Analysis`. May
contain provenance material (early hypotheses, intuitions, corrections) not in
other corpora. Whether they warrant an ingest path is hypothesis 045.

Status: **Available for assessment.** No ingest path exists yet.

## Lineage corpus

All layers ingested into `lineage.db`:
- Google Doc revision history (TLTT, 53 diffs + 54 snapshots)
- Gemini web conversations
- AI Studio chats (all accounts — multiple accounts processed 2026-08-31)
- NotebookLM captures

Status: **Current.**

## Skills needed

- Own fiction analysis needs an adapted analyze-story skill (self-diagnostic
  framing, handle unfinished works, naive chapters as partial text)
- V1 archive extraction needs a new skill (multiple subagents, batched by
  chapter arc, voice separation via lineage grep, consistent categorization)
