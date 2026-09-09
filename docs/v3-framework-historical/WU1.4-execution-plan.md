# WU1.4 — execution plan for the remaining work

> **Superseded 2026-09-03, pending forward-plan-2.** Methodology revision 1
> (`methodology-revision-1.md`; skill `v3-buildout`) re-types every WU as an exploratory or
> verification pass per corpus, and WU1.4 is to be re-specced whole in forward-plan-2 —
> the reading as the v1 archive's exploratory pass (six arms: two reading conditions × three
> models), the ~20 named checks as its verification pass under calibrated decision rules.
> **Do not execute the session sequence below.** What stands unchanged: the attribution
> instrument, `attribution.csv`, the calibration records, and the rulings on the WU card in
> `forward-plan-1.md`. The background section remains the record of the instrument
> validation.

> **NEXT STEP (not yet done as of 2026-09-02): a dedicated planning session, before any
> reading session.** Fresh context; loads the `v3-buildout` and `v1-archive-mining` skills,
> the WU1.4 card, the twelve hypothesis files, and this file; runs plan mode properly (read,
> collect every open question, ask in batches, write). It must produce: (1) the check-by-check
> design for the WU card's ~20 named checks — evidence source, decision rule, what is written
> if absent, and whether Session 4 does targeted re-reads at check-named loci and how those
> stay separate from the discovery records; (2) which of the twelve hypotheses can be
> deposited from the CSV alone (019/020/022 largely can) versus which wait on the inventory;
> (3) the settled instrument settings — confirm R = 8 tokens against borderline `fragment` rows on
> the scan (contractions count as two tokens by design, e.g. "doesn't" = does + not; the
> question is only whether 8 is the right line) and the six named subjects' ids (the
> 2025-12-23 backup is not needed — Brian, 2026-09-02); (4) written adjudication criteria for Session 3 ("supported by
> the notes at that locus" needs a rule); (5) the literal subagent prompt for Session 2;
> (6) this file, revised. It writes no records and reads no arc file in full.
> Sequence after it: commit → Session 1 / Session 2 (either order or parallel) → Session 3
> → Session 4.

Created 2026-09-02, after the instrument-validation session. The attribution instrument is
built and validated (decisions on the WU1.4 card in `forward-plan-1.md`); the reading has
not started. Every session below loads the `v1-archive-mining` skill for the per-session
method and this file for its place in the sequence. Each session runs the normal WU
protocol — short scope check, short plan mode, execute — and reads nothing it isn't given.
This file is the only carrier of WU1.4 session state — nothing about WU1.4 lives in memory
files, so that when the WU completes its working context retires with it.

## Background from the instrument-validation session (2026-09-01 → 02)

What that session established, beyond the rulings on the WU card, that a later session
would otherwise have to rediscover:

**The evidence set is the CSV, and only the CSV.** Two designs for a second, reading-side
file were proposed and rejected by Brian: a row-per-note category table (atomises the
archive; repeats the v1 Sorter's "route every atomic thought into buckets"; mass-classifies
in Claude's framing) and a row-per-instance table (same problem at coarser grain — a
structured file of Claude's categories becomes "data" that downstream sessions query as if
it were evidence). The reading's output is therefore locus-level *records* during the pass
(scratchpad, raw material) and a *synthesis* with a pattern inventory at the end; nothing in
between is a dataset.

**How the instrument was validated.** A first calibration sheet (30 notes per provisional
tier; `calibration-sample-1.md`, kept as record with Brian's 112 verdicts) showed: prompt-
origin matches are Brian's in every case; the provisional "partial" tier was mostly whole
pastes with a phrase cut, not pastes with additions — which drove the shift from coverage
tiers to structural labels (token coverage + gap position + run length); "trace" held three
populations (Brian's prompts, boilerplate, and real lifts), which drove the fragment/phrase
split at R; Gemini quoting Brian's notes back ("In your notes… you explicitly left a
placeholder") is a recurring pattern the snapshot rule misses within a week, which drove the
echo rule; and one note (4116) whose only captured source was the plan dump is a concrete
instance of a paste whose original response was never captured. A second sheet was
generated and discarded — Brian judged the census scan (`scan.html`) the better validation
and did not need row-by-row arbitration. Brian's verdicts on the `none` tier also carried
register annotations (fabula, syuzhet, prose fragment, prior-belief setup, structural-irony
setup, "speccing out inferences", subtext) — a preview, in his words, of the vocabulary the
reading will meet.

**The Note Organizer chats were verified, not assumed.** `aistudio:24` (the Sorter) model
turns are 43–77% verbatim other lineage, dominated by Brian's own Gemini prompt text; its
inputs are Drive-document placeholders. `aistudio:22/23/25` do author original prose (bucket
definitions, strategy) but no archive note originates in them. Excluding all four moved 82
notes: 32 to `none` (Brian's by ruling — the Sorter was the only echo of his uncaptured raw
notes) and 27 to `trace`/`fragment` on their true earlier source.

**PlanFirst's population.** Of 240 flips at validation, 143 were on `phrase` rows (no claim
to override), 51 fragment, 34 framed, 9 verbatim, 3 edited — i.e. the rule does real work
on ~100 rows. Echo flagged 35, mostly verbatim/framed/edited: the model quoting Brian at
length.

**Display encodings** (for anyone touching `scan.html`): bold = AI text, strictly; purple
underline + purple edge = matched an AI source but ruled Brian's (PlanFirst / echo); dotted
underline = phrase, no claim; fill red / orange / yellow = AI note / mixed / one borrowed
sentence; edge light blue-green = Brian's, uncaptured; green = Brian's, matched his own
prompt. The render carries only bold (AI) and plain; provenance is in the prefix.

**Sizes.** CSV ~6.7 MB / ~1.7M tokens (never read whole; `SourceWindow` and `Content` are
2 MB each); scene-level note text ~265K tokens; all subject notes ~250K, the 24 in scope a
fraction of that. The archive has 5,843 notes; 5,731 after Paratext.

**Consumers after WU1.4.** The same tool pointed at the v2 working plan is the voice-lint
report (implementation candidate D19's read-only half already exists in
`StoryPlanner.Core/VoiceMatch.cs` — per-span source credit with offsets — and its write
path stays gated on 019/020). The Conversations corpus (Claude web chat, Apr 2026 overlap
with late v1) is not indexed; one `none` note in the sample had a Claude-voiced sentence.
Not needed for WU1.4; would be a small extra layer for the lint.

## Why four sessions

Two historical failure modes: lossy consolidation of intermediate analyses (WU1.1's
meta-analyses, WU1.3's per-story analyses), and lossy reading of long inputs. The scene
corpus is therefore read twice by readers that fail differently, blind to each other, then
adjudicated. The subject checks are a separate, later session so the scene reading stays
focused on the scene graph. The disagreement counts are themselves evidence for the pipeline
hypotheses. (Brian, 2026-09-02.)

## Session 1 — long read (scene corpus)

- Generate the render (skill: "The reading view"); arcs only, no subject files; Paratext
  excluded. Commit `read-manifest.md` to the artifact directory.
- Read every arc file in order, then Aris (~265K tokens). Discovery-first; unit = plot point
  or link; write each arc's working file immediately after reading it (skill: record format).
- Output: `WU1.4-v1-scene-instincts/records-long-read/` — the per-arc record files, dated.
  No synthesis, no vocabulary normalisation, no deposits.
- R is settled by the planning session (see the note at the top); if it changes, the CSV and
  render are regenerated with the new value before either scene pass reads, and the WU card
  notes it.

## Session 2 — per-arc subagents (scene corpus)

- Same render, produced once (reuse Session 1's, or regenerate with identical arguments).
- One subagent per arc file (seven + Aris), each given only its file and the skill's reading
  protocol, emitting the same record format. The main session launches, collects, and
  writes nothing else — it does not read the arc files and does not synthesise.
- Output: `WU1.4-v1-scene-instincts/records-subagents/` — one record file per arc, dated.
- Blind: this session does not open `records-long-read/`.

Sessions 1 and 2 may run in either order or in parallel.

## Session 3 — adjudication

- Input: both record sets, the render at contested loci, the CSV by script.
- Join the two sets mechanically on locus; compare pattern names only after the join.
- Bin every locus present in one set and absent from the other:
  *cross-arc* (a `+` locus spanning arcs — the subagent could not have seen it; expected),
  *missed by the long read* (in a subagent's records, absent from the long read),
  *over-read by the fresh reader* (in a subagent's records, not supported by the notes).
  Drill only the last two, by reading the render at that locus; Brian adjudicates the
  drills.
- Normalise the pattern vocabulary across both sets (keep the merge list).
- Output: `WU1.4-v1-scene-instincts/inventory.md` — the adjudicated pattern inventory
  (pattern → loci → note ids → voice), plus the disagreement counts by bin and the merge
  list. The raw record sets stay as evidence of the method.

## Session 4 — subject pass, synthesis, deposits

- Input: `inventory.md` (not the raw record sets or arc files), the subject render (the six
  subjects named by the WU card's checks — Aquileian, Chrysalis, Pinkie Pie, AJ the
  Collaborator, Friendship Letters, TwiJack-related — plus the 18 triage-labelled *Deferred
  for a plot point* / *Deferred until chapter notes* subjects, each with its scene-link
  list), the CSV by script, lineage by id.
- Read the subject files under the skill's protocol; flag any scene-level design found
  parked in subject notes back into the inventory.
- Only now open the WU card's named checks and the twelve hypothesis statements and locate
  each against the inventory; verify anything a deposit rests on by reading the full turn.
- Write the synthesis (below), then the deposits under the Deposit protocol (`v3-buildout`
  skill), then the wrap-up: index sweep for serendipitous evidence, tag counts to Brian,
  new-hypothesis offers only if novelty / testability / independence all hold, WU status
  `complete` with a dated completion note on the card.

## The synthesis

`WU1.4-v1-scene-instincts/WU1.4-v1-scene-instincts.md`, write-once, organised by what was
observed, never by hypothesis id:

1. Method — tool and rulings, render, what was read (`read-manifest.md`), the two-pass
   disagreement counts by bin, the vocabulary merge list.
2. Attribution picture — tables from the CSV by owner type, arc, origin layer/role, label;
   pastes and lifts separately; PlanFirst / echo / stitched / snapshot-absent counts. The
   whole-archive figure at validation: 16% of notes AI pastes, 9% mixed, 3% one borrowed
   sentence, 72% Brian's; 30% of characters AI-attributed; links more contaminated than
   plot points, subjects most, mostly as mixed notes.
3. Arc sections (Brian's seven ranges + Aris) — the design patterns found, with note ids,
   voice, excerpts.
4. The pattern inventory — the lookup surface for WU1.9 and WU1.7.
5. The WU card's named checks answered from the evidence, "not found" stated as
   prominently as found.
6. Unexpected observations, held for the wrap-up.

Counts cite the CSV; classifications that bear on a hypothesis cite the lineage id and were
read in full.

## Standing caveats (state them wherever a number appears)

Text-level provenance only: paraphrase and Brian's own syntheses of AI ideas count as his;
a paste whose response was never captured counts as his; a same-week echo without the "in
your notes" lead-in counts as AI. Snapshot resolution is weekly, and the snapshot series
starts 2025-12-26 (the 2025-12-23 backup was judged unnecessary). None of these has a
measured size.
