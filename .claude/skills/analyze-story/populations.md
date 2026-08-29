# Analysis Brief v4 — Corpus Populations

Ground truth for which stories belong to which tier and subset.
Referenced by the analyze-story skill and all meta-analysis work.
The skill checks off each story after writing its analysis to Drive.

## Pruning boundary

40,580 bytes (Drive Doc file size) — the smallest story Brian quarantined
into Large Queue (`the-best-night-ever-repeat`). Below = short corpus
(clean cloud analysis). At or above = long corpus (compacted or never analyzed).

## Output naming convention

- `Story - analysis v4` — cloud Cowork analysis (may be compacted for long stories)
- `Story - analysis v4 (1M)` — local Claude Code, Opus 4.6, 1M context
- `Story - analysis v4 (1M, Part N)` — split story, one of two parts
- A story with BOTH `v4` and `v4 (1M)` is Subset A (the 4.3 comparison population)

## Drive folder IDs

- Reading Archive: `1upGz5lDqojWnuV-2S2K_m7ReiAz3dkeC`
- Queue: `1MKVZxR9R3J4ualUcrvRhhrh5wc7ftD3z`
- Output: `1Sqf9j3v5Fi78ipDrA5VzpLJS2Q2b9TFQ`
- Done: `13BLadzbqFpZrcyjfjWfWWFNa__r0xPW0`
- Large Queue: `1t-IAjXPtQ9aBCXH7EJgJl-f_YsVqnyYq`
- Analysis Brief (v4): `1VUYIhCd70oU0Uyh0sOE88Hf4POxqpRQvaHUYV7oQtEA`

## Tier 1: Short corpus (57 stories)

Cloud v4 analyses, no compaction. Population for meta-analyses 4.1a and 4.1b.
These are every story in Done NOT listed in Subset A below. Not tracked
individually — their analyses are already complete.

## Tier 2: Long corpus — 1M single session (52 stories)

### Subset A: Compaction re-runs (25)

Have existing cloud analysis AND get a 1M re-run. The pair is the 4.3 population.

- [x] trial-run
- [x] the-frozen-north
- [x] controlling-your-desires
- [x] not-unless-you-mean-it
- [x] the-twilight-hours
- [x] maidens-day-1
- [x] kindnesss-reward
- [x] the-appledash-project
- [x] promises
- [x] third-times-a-charm
- [x] professor-raritys-totally-platonic-romance-curriculum
- [x] the-haunting-of-carousel-boutique
- [x] the-sky-is-falling
- [x] magic-tutor
- [x] the-last-train-home
- [x] injuring-eternity
- [x] the-gemmed-satyr
- [x] ribbons-and-lace
- [x] spread
- [x] the-parent-trap
- [x] ill-do-anything-for-you
- [x] perfect-on-paper
- [x] you-make-my-whole-life-worthwhile
- [x] filly-fooling
- [x] romance-reports

### Subset B: Quarantined, never analyzed (24)

Were in Large Queue on Drive. 1M analysis only (no cloud version to compare).

- [x] the-best-night-ever-repeat
- [x] cuddling
- [x] lets-find-you-a-date
- [x] where-earth-meets-sky
- [x] i-love-to-see-you-smile
- [x] carrot-top-season
- [x] fixing-up-miss-smartypants
- [x] twilights-list-1
- [x] feels-like-the-first-time
- [x] bechdels-law
- [x] the-best-night-ever
- [x] dashs-new-mom
- [x] about-last-night
- [x] those-blue-wings
- [x] longest-night-longest-day
- [x] crisis-on-two-equestrias
- [x] ill-always-be-here-for-you
- [x] on-a-cross-and-arrow
- [x] flying-high-falling-hard
- [x] rainbooms-and-royalty-new
- [x] inner-strength
- [x] clocktower-society-your-safe-word-is-law
- [x] salvation
- [x] unexpected-confessions

### Subset C: Never in Drive pipeline (3)

Too large for Drive upload. 1M analysis only (no cloud version).

- [x] a-delicate-balance (1,594 KB, markdowns1)
- [x] pax-chrysalia (1,935 KB, markdowns — converted from epub 2026-08-26)
- [x] the-moons-apprentice (2,349 KB, markdowns1)

## Tier 3: Manual — split or special handling (4 stories)

Too large for 1M single session, or requires targeted re-analysis.

- [ ] green (2,656 KB, markdowns — borderline, attempt single first)
- [ ] fallout-equestria (3,509 KB, markdowns1)
- [ ] your-human-and-you (3,834 KB, markdowns1)
- [ ] romance-reports (1,049 KB, markdowns1 — 1M analysis has shallow late-chapter coverage;
      needs full re-run with output-conciseness instruction, not a continuation split)

## Truncated 1M analyses — continuation + merge needed

These stories' initial 1M analyses are truncated: the model read the full story but only
analyzed part of it. Each needs `--continue-from` to analyze the back half, then `--merge`
to produce a canonical single analysis.

| Story | Source ch | 1M stopped at | Continue from | Subset |
|---|---|---|---|---|
| the-gemmed-satyr | 22 | 12 | 13 | A |
| spread | 24 | 12 | 13 | A |
| perfect-on-paper | 23 | 14 | 15 | A |
| filly-fooling | 29 | 15 | 16 | A |
| ill-do-anything-for-you | 30 | 15 | 16 | A |
| unexpected-confessions | 33 | 28 | 29 | B |
| pax-chrysalia | 33 | 22 | 23 | C |
| the-moons-apprentice | 43 | 28 | 29 | C |

Continuation status:
- [x] the-gemmed-satyr --continue-from 13
- [x] spread --continue-from 13
- [x] perfect-on-paper --continue-from 15
- [x] filly-fooling --continue-from 16
- [x] ill-do-anything-for-you --continue-from 16
- [x] unexpected-confessions --continue-from 29
- [x] pax-chrysalia --continue-from 23
- [x] the-moons-apprentice --continue-from 29

Merge status (after both parts exist):
- [x] the-gemmed-satyr --merge
- [x] spread --merge
- [x] perfect-on-paper --merge
- [x] filly-fooling --merge
- [x] ill-do-anything-for-you --merge
- [x] unexpected-confessions --merge
- [x] pax-chrysalia --merge
- [x] the-moons-apprentice --merge

## Pipeline divergence notes (for grand synthesis)

The 1M and cloud pipelines use the same v4 Analysis Brief but different surrounding context:
- Cloud (cowork): 67-line task prompt + brief read from Drive + story from Drive. No CLAUDE.md.
- 1M (skill): full StoryPlanner CLAUDE.md + skill with embedded brief + story from local disk.

Known systematic divergences (from 4.3, 2026-08-27):
- M4 (Perception Gap) calibration: cloud applies a lower threshold for "sufficient FID,"
  1M applies a stricter threshold. Both are defensible readings of the brief. M4 counts
  are not comparable across pipeline types.
- Enumeration style: cloud is exhaustive (more instances), 1M is selective (fewer instances,
  deeper per-instance analysis).
- Factual precision: 1M is more precise for what it covers. Cloud substitutes details
  (mirror for door, fabricated chapter titles, merged events).
- These divergences are analytical, not coverage-driven. They affect all stories regardless
  of size.

## Meta-analyses

- [x] 4.1a — Fresh short corpus meta-analysis (2026-08-26, 59 stories)
- [x] 4.2 — Fresh long corpus meta-analysis (2026-08-28, 53 stories in 5 categories)
  - [x] 4.2a — Ensemble Stories (2026-08-28, 6 stories)
  - [x] 4.2b — Emotional Romance and Slice of Life (2026-08-28, 15 stories)
  - [x] 4.2c — Dark Premise (2026-08-28, 10 stories)
  - [x] 4.2d — Alternate Universe (2026-08-28, 11 stories)
  - [x] 4.2e — Explicit Content as Plot (2026-08-28, 11 stories)
- [x] 4.3 — Compaction vs 1M comparison (2026-08-27, 10 of 25 sampled)
- [ ] 4.4 — Grand synthesis + hypothesis testing (after 4.1a + 4.2 + 4.3)

## Local story file directories

Under `C:\Users\Brian\Documents\Fimfiction Favorites\`:
- `markdowns/*.md` — top favorites (1st batch download)
- `markdowns1/*.txt` — other favorites + unfinished/disliked
- Skill searches both, preferring markdowns/ when both have a match
