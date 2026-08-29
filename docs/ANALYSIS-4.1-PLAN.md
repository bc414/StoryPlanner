# Analysis Pipeline — Post-Vacation Plan (2026-08-26)

Successor to `ANALYSIS-PIPELINE-2026-08-17.md`. That document covers the origin
through v4 Brief; this one picks up where the vacation left off.

## Situation

The v4 Analysis Brief ran via Cowork scheduled tasks during Brian's vacation. All short
stories completed. Some long stories also completed under compaction (the cloud task's
context filled and compacted mid-read). Partway through, the remaining large stories were
quarantined out of Queue into `Reading Archive/Large Queue` on Drive because the scheduled
tasks were constantly compacting and consuming compute. A further set of stories were never
uploaded to Drive at all — too large for the cloud pipeline entirely.

## Corpus split

Three tiers, 112 stories total.

The **pruning boundary** is the smallest Large Queue entry: 40,580 bytes (Drive Doc size),
which is `the-best-night-ever-repeat`. Everything below that threshold completed cloud
analysis without compaction issues; everything at or above it either compacted during
analysis, was quarantined before analysis, or was never in the Drive pipeline.

The **split threshold** is ~2,500 KB of local markdown (~830K tokens at 3 chars/token).
After overhead (~10K tokens for system/brief/skill) and output budget (~50K tokens for the
analysis), stories above this cannot fit in a single 1M-context session. Determined from the
1M token budget:

| Budget line | Tokens |
|---|---|
| Total context (Opus 4.6, 1M mode) | 1,000K |
| System prompt + skill + embedded v4 Brief | −10K |
| Output budget (analysis for a novel) | −50K |
| **Available for story text** | **940K** |
| At 3 chars/token (conservative) | ~2,820 KB |
| At 4 chars/token (typical English) | ~3,760 KB |

### Tier 1: Short corpus (57 stories)

Done entries below the pruning boundary. Clean cloud v4 analyses. No further per-story
action needed. Population for meta-analyses 4.1a and 4.1b.

### Tier 2: Long corpus — 1M single session (52 stories)

Three subsets with different prior states, all getting a local 1M analysis in a single
session:

**Subset A — Compaction re-runs (25 stories):** Done entries at or above the pruning
boundary. Analyzed by the cloud under compaction — potentially degraded. Existing compacted
output in `Reading Archive/Output`. These get 1M re-analysis AND are the population for the
4.3 compaction-vs-1M comparison.

| Story (Done, ≥ threshold) | Drive size |
|---|---|
| trial-run | 40,999 |
| the-frozen-north | 41,522 |
| controlling-your-desires | 45,542 |
| not-unless-you-mean-it | 46,622 |
| the-twilight-hours | 52,147 |
| maidens-day-1 | 53,074 |
| kindnesss-reward | 59,614 |
| the-appledash-project | 64,842 |
| promises | 68,487 |
| third-times-a-charm | 70,181 |
| professor-raritys-totally-platonic-romance-curriculum | 72,593 |
| the-haunting-of-carousel-boutique | 72,912 |
| the-sky-is-falling | 75,640 |
| magic-tutor | 91,449 |
| the-last-train-home | 92,959 |
| injuring-eternity | 94,569 |
| the-gemmed-satyr | 108,196 |
| ribbons-and-lace | 131,160 |
| spread | 131,161 |
| the-parent-trap | 142,276 |
| ill-do-anything-for-you | 155,064 |
| perfect-on-paper | 192,238 |
| you-make-my-whole-life-worthwhile | 214,775 |
| filly-fooling | 255,582 |
| romance-reports | 384,058 |

**Subset B — Quarantined, never analyzed (24 stories):** Pulled from Queue into
`Reading Archive/Large Queue` before the cloud task reached them. No existing analysis.

| Story (Large Queue) | Drive size | Local dir |
|---|---|---|
| the-best-night-ever-repeat | 40,580 | markdowns |
| cuddling | 59,464 | markdowns |
| lets-find-you-a-date | 78,409 | markdowns |
| where-earth-meets-sky | 82,154 | markdowns |
| i-love-to-see-you-smile | 84,695 | markdowns1 |
| carrot-top-season | 97,253 | markdowns1 |
| fixing-up-miss-smartypants | 100,496 | markdowns |
| twilights-list-1 | 101,587 | markdowns |
| feels-like-the-first-time | 107,619 | markdowns1 |
| bechdels-law | 107,807 | markdowns1 |
| the-best-night-ever | 111,059 | markdowns |
| dashs-new-mom | 123,838 | markdowns1 |
| about-last-night | 126,998 | markdowns |
| those-blue-wings | 138,911 | markdowns |
| longest-night-longest-day | 151,111 | markdowns |
| crisis-on-two-equestrias | 171,010 | markdowns1 |
| ill-always-be-here-for-you | 179,221 | markdowns |
| on-a-cross-and-arrow | 187,900 | markdowns |
| flying-high-falling-hard | 235,205 | markdowns1 |
| rainbooms-and-royalty-new | 246,749 | markdowns |
| inner-strength | 286,897 | markdowns |
| clocktower-society-your-safe-word-is-law | 292,172 | markdowns1 |
| salvation | 321,387 | markdowns |
| unexpected-confessions | 428,524 | markdowns |

**Subset C — Never in Drive pipeline (3 stories):** Too large to upload to the cloud
pipeline at all. No existing analysis. Converted from EPUB locally and fit in 1M as single
sessions.

| Story | Local size | ~Tokens (3c/t) | Remaining | Local dir |
|---|---|---|---|---|
| a-delicate-balance | 1,594 KB | ~531K | 409K | markdowns1 |
| pax-chrysalia | 1,935 KB | ~660K | 280K | markdowns (converted 2026-08-26) |
| the-moons-apprentice | 2,349 KB | ~783K | 157K | markdowns1 |

### Tier 3: Extra long — split required (3 stories)

Too large for 1M single session. Never in the Drive pipeline. No existing analysis. Each is
split at the chapter midpoint into Part 1 and Part 2, analyzed in separate sessions.

| Story | Local size | ~Tokens (3c/t) | Overshoot | Local dir |
|---|---|---|---|---|
| green | 2,656 KB | ~885K | −55K (borderline) | markdowns |
| fallout-equestria | 3,509 KB | ~1,170K | −230K | markdowns1 |
| your-human-and-you | 3,834 KB | ~1,278K | −338K | markdowns1 |

`green` is borderline: at 3 chars/token it overshoots by only 55K tokens; at 4 chars/token
it fits with 276K to spare. Attempt single session first, fall back to split if it fails.

**Local story files** live in two directories under
`C:\Users\Brian\Documents\Fimfiction Favorites\`:
- `markdowns/` (`.md`) — Brian's genuine top favorites (first batch download)
- `markdowns1/` (`.txt`) — other favorited stories plus some unfinished/disliked

The skill searches both, preferring `markdowns/` when a match exists in both.

## Story splitting

Precedent: The Princess and the Kaiser (124 chapters, 4.4 MB / 27,272 lines). The original
analysis was reactive — the model honestly read through Part 61 (~73%) before admitting it
had started skimming. Two separate docs were produced: Part 1 covering Parts 1–61, Part 2
as a fresh session covering Parts 62–end.

**For the skill:** Stories above the split threshold (~2,500 KB local markdown) are split
deliberately at the chapter midpoint. Each half is analyzed as a separate session, producing
`<Title> - analysis v4 (1M, Part 1)` and `Part 2`. The split point is a chapter boundary
nearest 50% of the file, not an arbitrary byte offset.

## The 51-story meta-analysis

The meta-analysis that produced the v4 Brief and the 8 hypotheses was done in-session
(sessions `4c27bcd7` and `e16846b2`, 2026-08-17/18). **No standalone document exists on
Drive.** Its findings were distilled into `CORPUS-ANALYSIS-HYPOTHESES-2026-08-17.md` (in
this repo), which is the foundation that 4.1b builds on.

## Phases

### Phase 1a: Local 1M-context analysis of long and extra-long stories

**What:** A Claude Code skill (`/analyze-story`) that reads a local markdown file with the
full v4 Brief embedded, runs the analysis using Opus 4.6 at 1M context (no compaction), and
writes the result to Google Drive as a Doc in `Reading Archive/Output`.

**Mechanism:** The skill is invoked per-story from separate Claude Code sessions for
parallelism. Brian opens N terminal windows and runs `/analyze-story <story-name>` in each.

**Skill design:**
- Accepts a story identifier (the markdown filename stem, e.g. `salvation`)
- Resolves to the local file: first `markdowns/<name>.md`, then `markdowns1/<name>.txt`
- Embeds the v4 Brief (pulled from Drive and cached locally in the skill)
- Adapts the brief's formatting notes for local files (no backslash-escaped asterisks from
  Drive connector; standard `*...*` markdown emphasis)
- Reads the ENTIRE story into context (the point of 1M context — no sampling, no compaction)
- For stories above the split threshold (~2,500 KB): splits at chapter midpoint, analyzes
  Part 1, then tells Brian to invoke `/analyze-story <name> --part 2` in a fresh session
- Writes the analysis to Drive `Reading Archive/Output` folder
  (id: `1Sqf9j3v5Fi78ipDrA5VzpLJS2Q2b9TFQ`) as a Google Doc titled
  `<Story Title> - analysis v4 (1M)` (or `(1M, Part N)` for splits)
- Metadata block includes `MODEL: claude-opus-4-6`, `CONTEXT: 1M (full read, no compaction)`,
  and `DATE: <current date>`

**Population (55 stories):**
- **Subset A — Compaction re-runs (25):** Done entries at or above the pruning boundary.
  These already have cloud-compacted v4 output; the 1M re-run produces a second analysis
  for the same story, used in the 4.3 comparison.
- **Subset B — Quarantined (24):** Everything in `Reading Archive/Large Queue`. Never
  analyzed.
- **Subset C — Never in pipeline (3):** a-delicate-balance, pax-chrysalia,
  the-moons-apprentice. Fit in 1M as single sessions. Never analyzed.
- **Tier 3 — Split required (3):** green (attempt single first), fallout-equestria,
  your-human-and-you. Each produces two analysis docs.

### Phase 1b: Meta-analyses 4.1a and 4.1b (short-story corpus only)

Two parallel meta-analyses of the **short corpus only** (57 stories below the pruning
boundary), each with a different lens:

**4.1a — Fresh corpus meta-analysis:**
- Reads all short-corpus v4 output docs from `Reading Archive/Output` from scratch
- Examines: vocabulary/brief compliance, cross-story mechanism distribution, perspective
  technique patterns (FID prevalence, DT usage, mode distribution), bond analysis patterns,
  framework fit gaps and unnamed techniques
- Output: a Google Doc in `Reading Archive/` titled
  `Meta-Analysis 4.1a — Fresh Short Corpus Analysis`

**4.1b — Hypothesis-testing pass:**
- Builds on `CORPUS-ANALYSIS-HYPOTHESES-2026-08-17.md` (the distillation of the in-session
  51-story meta-analysis — no standalone doc on Drive exists)
- Tests the 8 hypotheses against the short-corpus v4 results
- Output: a Google Doc in `Reading Archive/` titled
  `Meta-Analysis 4.1b — Hypothesis Testing`

### Phase 2: Meta-analysis 4.2 (full corpus including 1M long stories)

**What:** After Phase 1a and 1b are both complete, a meta-analysis that includes the
1M long-story results alongside the short-story corpus.

**Scope:** All v4 analyses — short-corpus cloud results (57) + 1M long-story results from
Phase 1a (55). For stories that have both a compacted cloud version and a 1M version, use
the 1M version (the compacted one is reserved for the 4.3 comparison).

**Output:** A Google Doc in `Reading Archive/` titled `Meta-Analysis 4.2 — Full Corpus (1M)`.

**Depends on:** Phase 1a complete + Phase 1b complete (1b's findings inform 4.2's
methodology).

### Phase 3: Meta-analysis 4.3 (compaction vs 1M comparison)

**What:** A comparative analysis of the cloud-compacted v4 results against the 1M Claude
Code results for the **same 25 stories** from Phase 1a Subset A.

**Questions to answer:**
- Did compaction cause the analysis to miss mechanisms, misclassify FID vs DT, or produce
  shallower inference chains?
- Are there systematic patterns in what compaction loses (e.g. always misses late-story
  instances, always loses thread of multi-chapter development arcs)?
- Does the 1M analysis produce qualitatively different Framework Fit findings?
- Is the compacted version "good enough" for some stories but not others — and is there a
  size threshold?

**Output:** A Google Doc in `Reading Archive/` titled `Meta-Analysis 4.3 — Compaction vs 1M`.

**Depends on:** Phase 1a Subset A complete (needs both versions of the same stories).

### Phase 4: Grand synthesis (4.4)

**What:** A highest-level analysis that examines 4.1a, 4.1b, and 4.3 against each other.

**Questions to answer:**
- Do the fresh corpus findings (4.1a) and the hypothesis-testing findings (4.1b) converge
  or diverge? Where do they see different things?
- What does the compaction comparison (4.3) say about the reliability of the short-story
  results that 4.1a and 4.1b are built on?
- What are the biggest-picture findings across all three lenses?

**Output:** A Google Doc in `Reading Archive/` titled
`Meta-Analysis 4.4 — Grand Synthesis`.

**Depends on:** 4.1a, 4.1b, and 4.3 all complete.

## Dependency graph

```
Phase 1a (1M runs, 55 stories)
  Subset A: compaction re-runs (25) ────┐
  Subset B: quarantined (24)            │
  Subset C: never in pipeline (3)       │
  Tier 3: split required (3)            │
                                        │
Phase 1b (short corpus only, 57)        │
  4.1a: fresh corpus analysis ──────────┤
  4.1b: hypothesis testing ────────────┤
                                        │
              ┌─────────────────────────┤
              v                         v
          Phase 3                   Phase 2
          (4.3 comp., uses          (4.2 full corpus)
           Subset A only)              │
              │                         │
              v                         │
          Phase 4  <────────────────────┘
          (4.4 grand synthesis)
```

Phase 1a and 1b run in parallel.
Phase 2 (4.2) waits for Phase 1a + Phase 1b.
Phase 3 (4.3) waits for Phase 1a Subset A.
Phase 4 (4.4) waits for 4.1a, 4.1b, and 4.3.

## Work status

### Common root (prerequisite for all branches) — COMPLETE

- [x] Investigate corpus: identify all stories, sizes, local files, Drive state (2026-08-26)
- [x] Determine pruning boundary: 40,580 bytes Drive Doc size (2026-08-26)
- [x] Determine split threshold: ~2,500 KB local markdown / ~830K tokens (2026-08-26)
- [x] Classify all stories into three tiers and subsets (2026-08-26)
- [x] Verify all 55 target stories have local markdown/txt copies (2026-08-26)
- [x] Convert Pax Chrysalia from EPUB to markdown (1,935 KB, `markdowns/pax-chrysalia.txt`)
- [x] Pull v4 Analysis Brief from Drive, adapt for local use (`docs/analysis-brief-v4.md`)
- [x] Verify `create_file` MCP tool can write Google Docs from plain text
- [x] Build `/analyze-story` skill (`.claude/skills/analyze-story/SKILL.md`)
- [x] Create ground-truth populations registry (`.claude/skills/analyze-story/populations.md`)
- [x] Update code-sessions DB with recent sessions

### Branch 1a: Per-story 1M analyses — PENDING

Run `/analyze-story <name>` in parallel Claude Code sessions. All 55 stories are
independent; order does not matter.

**Per-story progress is tracked in `.claude/skills/analyze-story/populations.md`** — the
skill checks off each story after writing its analysis to Drive.

### Branch 1b: Meta-analyses of short corpus — PENDING (can run in parallel with 1a)

Progress tracked in `.claude/skills/analyze-story/populations.md` under "Meta-analyses".

### After branches converge

Progress tracked in `.claude/skills/analyze-story/populations.md` under "Meta-analyses".

## Drive folder IDs (reference)

| Folder | ID |
|---|---|
| Reading Archive | `1upGz5lDqojWnuV-2S2K_m7ReiAz3dkeC` |
| Queue | `1MKVZxR9R3J4ualUcrvRhhrh5wc7ftD3z` |
| Output | `1Sqf9j3v5Fi78ipDrA5VzpLJS2Q2b9TFQ` |
| Done | `13BLadzbqFpZrcyjfjWfWWFNa__r0xPW0` |
| Large Queue | `1t-IAjXPtQ9aBCXH7EJgJl-f_YsVqnyYq` |
| Analysis Brief (v4) | `1VUYIhCd70oU0Uyh0sOE88Hf4POxqpRQvaHUYV7oQtEA` (Doc) |
| Analysis Brief (v3 superseded) | `1I7dwAPBWAS0UaVvHeRmKik4nvuS1F0-bKfIAPCgBk6I` (Doc) |
