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

## Brian's own fiction (for WU2a)

6 published stories + naive TLTT chapters:
- THLB (Pokemon, 2015-2017, first-person alternating POV) — fichub epub, verify italics
- Wish (Pokemon, 2020, one-shot) — fichub epub, verify italics
- TEatS (Pokemon, 2020-2021) — fichub epub, verify italics
- NTL (Pokemon, 2021-2023, unfinished) — fichub epub, verify italics
- GIYC (MLP, 2024-2025, unfinished) — Fimfiction epub (italics expected preserved)
- Falldale (EaW, Nov 2025, one-shot) — Fimfiction epub (italics expected preserved)
- Naive TLTT Ch1-2 (June 2025) — already in `source_material_references/` as markdown

Status: epubs downloaded 2026-08-29. Remaining: verify italics in the 4 fichub exports
from fanfiction.net (THLB, Wish, TEatS, NTL).

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

## Planning doc revision histories (for WU3)

- TLTT — already in lineage (53 diffs, `gdoc:` ids)
- KU/NTL — Drive doc IDs known, needs appscript export + GDocHistory ingest
- GIYC — Drive doc ID known, needs appscript export + GDocHistory ingest
- Falldale — Drive doc ID known; planning doc "almost as long as the prose itself"

Status: Blocks on Brian running the appscript to export revision histories from Drive.

## Skills needed

- WU2a needs an adapted analyze-story skill (self-diagnostic framing, handle
  unfinished works, naive chapters as partial text)
- WU2b needs a new skill for v1 archive extraction (multiple subagents, batched by
  chapter arc, Gemini-voice separation via lineage grep, consistent categorization)
