# Forward Plan 2

Created: 2026-09-04
After: methodology revision 1 (2026-09-03) — a **priority reassessment**, not a
consolidation. The hypothesis set is structurally unchanged since consolidation-1: 046
hypotheses at consolidation, 047–050 minted 2026-09-03, 013 iterated the same day.
Predecessor: `forward-plan-1.md`, retired 2026-09-04 (header stamp), with its ordering audit.
One-time instructions applied here: `forward-plan-2-handoff.md` (rulings of 2026-09-03 and
2026-09-04). Method: the `v3-buildout` skill, revision 1 — `forward-plans.md` for the shape,
`wu-execution.md` for what a card is, `evidence-pipeline.md` for what may enter a record, the
`agent-runner` skill and `fanout/PROTOCOL.md` for every runner job named below.

## Rationale

### What the revision showed

Methodology revision 1 re-typed every work unit. Plan 1 had one WU type — a pass per
evidence source that read, found, and deposited — and the revision's prompting evidence
(`methodology-revision-1.md`) is that this conflated findings with evidence: the session
that searched for a pattern also wrote its "would differ if false" clause; WU1.3's first
deposits ran 13 supporting to 0 challenging with four per-story FID misclassifications
caught only on source check; verification tasks accreted onto exploratory cards as
"testing specs" until WU1.4 carried roughly twenty named checks on top of an open-ended
read and WU1.9 was becoming a source-verification campaign under a comparison's name.

The revision's answer is structural: two WU types per corpus (exploratory, which writes
findings and questions; verification, which writes candidates that a fresh-context referee
checks and a reviewed promotion commit records), a synthesis type that consumes only
verified corpora, and infrastructure. Questions have a home independent of order — the
spec pools — so the plan no longer needs a derived execution sequence; ordering is
structural (exploratory passes unordered, verification rounds triggered, syntheses gated
by debt). The work matrix replaced tier numbers, and the agent runner replaced in-session
subagents for every autonomous cell. Plan 1's WUs are re-housed here under those types.

### What the landscape looks like

Fifty hypotheses, and under the strong form **every one is unverified**. The census on
2026-09-03 (`grep -c '^- evidence' docs/v3-framework/hypotheses/*.md`; re-run before
relying on it) found forty evidence entries across twenty hypotheses (023–040, 045, 046),
none carrying a referee line or codebook hash. The twenty `evidenced`/`challenged`
statuses in `INDEX.md` are computed from those entries and are read by this plan as
leads. All forty come from passes the revision types as exploratory — WU1.1 (twenty-three
entries, over the meta-analyses rather than the story text), WU1.3 (fourteen, over the
per-story analyses with some source checks), WU1.2 (three, over the Keep export). The
retroactive referee pass (WU2.15) will re-check them; because their originating passes
were exploratory, the expected outcome is that most return to spec-pool status rather than
stand, and the twenty statuses recompute to `untested`. That is not a loss: the questions
are preserved in the pools this plan seeds, the pass measures what 048 predicts, and the
first verification round on each corpus is where evidence re-enters.

The set spans two domains that hypothesis 008 predicts are separable: the narrative design
framework (001–005, 019–046 in the main; thirty-odd hypotheses) and the operating pipeline
(006–018, plus the four minted with the revision, 047–050, which are pipeline claims about
the buildout's own machinery). Plan 1 gave the pipeline domain one WU and called its
model-intrinsic sub-question untestable. The runner changed that: a controlled one-factor
cell is now routine, and the v1 exploratory factorial (WU2.5) is the first — three of the
four new hypotheses (047, 049, 050) are answered from its record sets, and 048 from the
retroactive pass.

The seven corpora and their debt as of this date:

| Corpus | Exploratory pass | Verification round | Pool entries (seeded here) |
|---|---|---|---|
| 112-story analysis corpus | done as pathfinding (WU1.1 → WU2.1) | none | yes |
| Brian's own fiction | done (WU1.3 → WU2.3) | none | yes |
| v1 archive | instrument built; reading not started (WU2.5) | none | yes (3 prior) |
| v2 working plan | not started (WU2.7) | none | yes (1 prior) |
| lineage | not started (WU2.9) | none | yes |
| conversations | not started (WU2.11) | none | yes |
| code sessions | not started (WU2.13) | none | yes |

No synthesis can run: every synthesis of plan 1 (1.5, 1.9, 1.11–1.14) names at least one
corpus with no verification round. That is the debt this plan is shaped to pay down.

Grounded facts this plan rests on (all mechanical, dated 2026-09-04):

- v1 archive (`get_stats archive`): 5,843 notes (4,821 open, 934 closed-disposition-
  not-recorded, 88 flagged), 450 plot points, 1,125 links, 34 chapters, 226 subjects under
  ten triage labels. Scene-level notes in `attribution.csv` (plot point, link, chapter
  owners; Paratext excluded): ~173K words, ~230K tokens before render prefixes — consistent
  with the ~265K-token render measured 2026-09-02.
- Per-arc census from `attribution.csv` (AI paste = role `model`, label verbatim /
  edited-paste / framed-paste, not PlanFirst, not Echo):

  | Arc | Notes | Words | Plot points | Links | AI-paste notes | AI-paste words |
  |---|---|---|---|---|---|---|
  | 1–5 | 381 | 14,447 | 166 | 200 | 19.9% | 28.7% |
  | 6–9 | 661 | 28,462 | 341 | 302 | 22.5% | 25.9% |
  | 10–13 | 650 | 29,946 | 317 | 316 | 21.5% | 22.9% |
  | 14–18 | 510 | 20,559 | 275 | 217 | 19.8% | 25.4% |
  | 19–22 | 512 | 23,102 | 291 | 213 | 16.4% | 18.6% |
  | 23–26 | 553 | 25,463 | 325 | 224 | 25.0% | 24.4% |
  | 27–32 | 504 | 25,326 | 279 | 210 | 25.6% | 30.8% |
  | Aris | 36 | 2,055 | 33 | 0 | 0% | 0% |

  Point-in-time figures for the subset-arc ruling below; regenerate from the CSV, never
  cite from here.
- The 18 triage-labelled subjects are the 14 *Deferred for a plot point* (230, 261, 215,
  288, 244, 250, 226, 23, 424, 236, 283, 233, 234, 608) and the 4 *Deferred until chapter
  notes* (626, 418, 617, 615) — `list_subjects archive`.
- The check-named subjects resolved by census (`list_subjects archive`, `search_archive`):
  **Chrysalis** = every subject matching /Chrysalis/: 12, 612, 266, 216. **Aquileian** =
  every subject matching /Aquilei/ — 232, 269, 230, 271, 276, 280, 281, 282, 284, 297,
  298 — plus the two intimacy subjects 236 and 418 (Brian, 2026-09-04). **Pinkie Pie** =
  23. **AJ The Collaborator** = 264 (one open note, two flagged, seven scene links).
  **TwiJack** = 601. **Friendship Letters** is not a subject: the item is chapters 10, 11
  and 12 whole — Extraction (chapter:28), Tempest (chapter:32), Crash (chapter:10) — per
  Brian's ruling of 2026-09-04; the search hits outside those chapters (CH#13 note 2247,
  CH#23 note 2021, link note 3625) are recorded in the pool entry so the reader can say
  whether the letters' design extends past the three chapters.
- codesessions.db purge precondition: **met**. The script's two selects
  (`purge-excluded-2026-09-03.sql`) return zero rule-matching main sessions and zero
  remaining rule matches on 2026-09-04; the db holds 69 main and 185 subagent sessions.
- Model ids for runner arms (confirmed by one-turn `claude -p` on 2026-09-04): Fable
  `claude-fable-5-1[1m]`; Opus `claude-opus-4-6[1m]` for any item over 200K tokens (every
  pathfinder arm) and `claude-opus-4-6` below; Sonnet `claude-sonnet-5`, slice arms only.
  Whether the Fable arm actually gets the 1M window is settled by WU2.5's pathfinder pilot
  (the init event echoed the id without the suffix).

### Rulings recorded here (2026-09-03 handoff; 2026-09-04 plan session)

- R = 8 stays; the render is generated with it; nothing regenerates.
- Hypothesis 013's refinement stays an iteration; this plan covers 050 hypotheses.
- Sonnet is not a pathfinder option: five arms, not six. The Sonnet pathfinder cell is
  named as not measured.
- Subject reading splits by what names the subject: the check-named sets above go to the
  v1 verification pass as focused-reader items; the 18 triage-labelled subjects go to the
  v1 exploratory pass as slice-reader items. Pinkie Pie (23) is in both sets and is read in
  both passes — discovery-first in the exploratory pass, under its frozen rule in the
  verification pass.
- Flagged archive notes: the exploratory render omits them (the 2026-09-02 instrument
  decision, `Outputs.cs`). The **verification** pass includes them for its subject items:
  a HITL step fetches the walled notes through the flagged tool family and writes them into
  the run folder's committed `excerpts/` (Brian, 2026-09-04).
- The v1 factorial's subset arcs are **6–9 and 19–22** (the largest arc, above-median
  contamination; the cleanest arc). The subset runs first under all three models, and
  **which model runs the remaining arcs is decided from the subset's adjudication**, not
  in advance (Brian, 2026-09-04). Consequence for blinding: the subset arms' read-manifest
  is opened at that adjudication; the later arms get fresh neutral labels and a manifest
  that stays closed until the condition-effect binning.
- Plan 1's WU1.12 (hypothesis adjudication) is not re-housed. Under the strong form a
  status is a computation over promoted entries made at each promotion session, and the
  set-level review it described is consolidation, on Brian's demand (`consolidation.md`).
- The three leftover `/analyze-story` transcripts on disk stay; the ingest rule keeps them
  out.

### Why this shape

Corpus pairs because verification debt is per corpus and the one hard edge in the plan
(exploratory on c → verification on c → consumers of c) is readable off `Type` and
`Corpus`. Every plan-1 synthesis survives as a synthesis card, each naming the corpora it
consumes, so a reader can see from the status board which rounds unblock it. Infrastructure
cards exist only where a Brian-owned precondition or an ingest gates a corpus's contents
(the revision-history preprocessing, the Keep ingest); codebooks and calibrations live
inside the verification cards that use them, as the first task. The retroactive referee
pass is one verification card whose "corpus" is the record itself.

Among exploratory passes, **v1 runs first** — Brian's stated priority, and the corpus he is
working in. The three advisory heuristics point the same way: 047, 049 and 050 are
infrastructure hypotheses (claims about how the buildout's own readers fail) and the v1
factorial is where they are measured; its questions feed the working-plan pool (the same
instrument pointed at v2 is the voice-lint census); and the archive is the foundation the
v2 taxonomy was built from.

Plan 1 is reference for what was tried. Its cards' testing specs are not carried on the
cards below; they are seeded into the pools (`spec-pools/`), with provenance, and each
verification card's hypothesis list reads "per spec pool". A plan-1 finding is a lead
until a verification round says otherwise.

## Work units

Cards in id order. Sections: corpus pairs (2.1–2.14), the retroactive pass (2.15),
syntheses (2.16–2.21), infrastructure (2.22–2.23).

### WU2.1: Analysis-corpus exploratory pass

**Type:** exploratory
**Corpus:** 112-story analysis corpus
**Question:** What do the 112 analyzed stories do at the scene level — the cross-category
patterns, the M4/FID coupling, the unnamed techniques, the framework gaps?
**Hypotheses:** informational only (an exploratory pass targets a corpus): the WU1.1 card
listed 023–026, 028–036, 040.
**Evidence sources:** The seven meta-analysis reports (4.1a, 4.2a–e, 4.3) and the per-story
v4 analyses in `source_material_references/Reading Archive Analyses/`.
**Scope:** This is plan 1's WU1.1, re-read as **pathfinding-complete** (Brian: "1.1 isn't
really complete, only the broad pathfinding part"). It was a pathfinder over the
meta-analyses, not a read of the story texts; the per-story analyses are themselves slice
reads over the source under the v4 brief (Opus, Aug 2026). Its artifact
`WU1.1-corpus-synthesis.md` stands as leads. Its twenty-three deposits are unverified and
are re-checked by WU2.15; every claim they made is a question in
`spec-pools/analysis-corpus.md`. No further exploratory work is scheduled on this corpus
in plan 2: a later round (a slice-reader pass over the story texts themselves, discovery-
first) is possible and would be a new card if a verification round finds the analyses
systematically wrong.
**Scale:** pathfinder (meta-analyses); complete.
**Preconditions:** none.
**Status:** complete (2026-08-31, as WU1.1; re-typed 2026-09-04)

---

### WU2.2: Analysis-corpus verification pass

**Type:** verification
**Corpus:** 112-story analysis corpus (the story text is the source; the analyses are never
the sole excerpt)
**Question:** Which of the pool's questions about the corpus survive a frozen-predicate
read of the story text?
**Hypotheses:** per spec pool `analysis-corpus.md`, currently: 023, 024, 025, 026, 027,
028, 029, 030, 031, 032, 033, 034, 035, 036, 037, 038, 046.
**Evidence sources:** Story markdowns in `C:\Users\Brian\Documents\Fimfiction
Favorites\markdowns\` and `…\markdowns1\` (outside the repo — `addDirs` or copied
excerpts); the per-story analyses as the map to loci; the meta-analyses as the source of
corpus-level counts. Populations: `.claude/skills/analyze-story/populations.md`.
**Codebooks:** to calibrate, all in `fanout/WU2.2-analysis-verification/`:
- `dt-classes` — the 046 two-class DT classification (class A gap-producing / class B
  told interiority, with the sub-types in 046's first entry); the template codebook named
  by the revision note. Item = one italic-span passage with its surrounding paragraph.
- `counterargument` — genuine counterargument present / absent, read against the
  structural plot's thesis rather than the bond's (the WU1.3 post-review correction);
  item = one story's theme-bearing passages as located by its analysis.
- `obstacle-type` — primary barrier characterological / structural / combined; item = one
  story's bond arc as located by its analysis.
- `perspective-mode` — FID / DT / told / blend per passage, with the register visible
  (referee rule R6); item = one passage with paragraph context. Needed by the 028/030/031
  questions and by the M4-availability coupling.
- Others as the pool requires; a question with no calibratable codebook stays open and is
  said so in the round's artifact.
**Scope:** Runs in rounds. Round 1 is due when `dt-classes` and `counterargument` are
calibrated — they answer the questions plan 1's WU1.9 had accreted (the 046 classification
across DT-dominant corpus stories; the counterargument re-read of romance/SoL analyses
against source; the per-story obstacle breakdown). Each round: enumerate items with a
script beside the codebook (loci from the analyses, text from the markdown), generate jobs,
dry run, pilot one, batch under Sonnet (classifier cell; `claude-sonnet-5`), tally, write
`candidates.md`, referee run under `fanout/referee/<date>-WU2.2/`, promotion session,
one commit. A classification that bears on a hypothesis cites the story and line. Does
**not** rank stories, apply the favorites lens, compare to Brian's fiction, or read TLTT.
**Scale:** classifier (Sonnet) per item; auditor where a whole-story judgment is the
predicate. Round 1 is on the order of a few hundred passage items for `dt-classes` (the
DT-dominant subset of the corpus) and one item per story for `counterargument` and
`obstacle-type` in the romance/SoL, ensemble and explicit/plot populations.
**Preconditions:** none (story files present per `CORPUS-STATUS.md`).
**Status:** proposed

---

### WU2.3: Own-fiction exploratory pass

**Type:** exploratory
**Corpus:** Brian's own fiction
**Question:** What does Brian instinctively do when he writes — mechanism profiles,
interiority technique, perspective discipline, structural patterns?
**Hypotheses:** informational only: the WU1.3 card listed 023, 024, 028–040 and created
046.
**Evidence sources:** The seven texts in `source_material_references/own_stories_md/` and
the naive TLTT chapters; the seven self-diagnostic v4 analyses (`Reading Archive
Analyses/`, `-1m.txt`); `WU1.3-own-voice-synthesis.md`.
**Scope:** Plan 1's WU1.3, complete. Seven slice reads under an adapted brief plus a
synthesis; its fourteen deposits are unverified (re-checked by WU2.15) and every claim,
including the four post-review corrections, is a question in `spec-pools/own-fiction.md`.
Brian's-recall items from the WU1.3 card (the split-converge pattern; structural obstacles
as primary; dual-POV irony as the foundational mechanism) are in the pool marked as
recall.
**Scale:** slice reader × 7 (Opus, `analyze-story` subagents — the pre-runner mechanism,
recorded as such); complete.
**Preconditions:** none.
**Status:** complete (2026-09-01, as WU1.3; re-typed 2026-09-04)

---

### WU2.4: Own-fiction verification pass

**Type:** verification
**Corpus:** Brian's own fiction (the prose is the source)
**Question:** Which of the pool's questions about Brian's fiction survive a frozen-predicate
read of the prose?
**Hypotheses:** per spec pool `own-fiction.md`, currently: 023, 024, 028, 029, 030, 031,
032, 033, 034, 036, 037, 038, 039, 040, 046.
**Evidence sources:** The text files (`.txt`, italics as `*…*`) and the two naive chapters;
the analyses only as locus maps.
**Codebooks:** to calibrate, in `fanout/WU2.4-own-fiction-verification/`: `dt-classes`
(shared with WU2.2 by copy at the same hash — a codebook is the analysis corpus WU's
instrument, and this WU cites the hash it applied rather than authoring a second one;
if this corpus needs a ruling the other did not, that is a revision under the owning
WU); `perspective-mode`; `comedy-position` (a comedic beat's structural position:
introduction / post-intensity / pre-reveal / irony setup / none — the 033/034 question);
`gap-delivery` (which of the corpus's named delivery mechanisms a passage instantiates —
the 038 question, one item per candidate passage); `fabula-delivery` (a fabula-bearing
passage delivered by dialogue exposition / behavioral evidence / designed incomplete
understanding / revelation architecture / designed mistake — the 040 question).
**Scope:** Rounds as codebooks calibrate. Round 1 is due with `dt-classes` (re-classifying
046's forty-line sample and extending it to the full 621-line italic population, with
Brian's borderline adjudications as calibration rulings) and `gap-delivery` (the 038
challenge — strategic opacity and narrated denial absent — as a classifier over every
passage the analyses flag as gap-bearing). The 039 pre/post-FiM comparison is a census
over classifier output per text, not a judgment. Enumeration by script over the text files
(italic spans; analysis-cited loci). Does **not** judge quality, propose changes to Brian's
writing, or compare to the corpus (that is WU2.16).
**Scale:** classifier (Sonnet) per passage; ~600 items for the DT population, fewer for the
others.
**Preconditions:** none.
**Status:** proposed

---

### WU2.5: V1-archive exploratory pass — the five-arm factorial

**Type:** exploratory
**Corpus:** v1 archive (scene-level notes: plot point, link, chapter; plus the 18
triage-labelled subjects)
**Question:** What scene-level design did Brian capture in v1 before he had vocabulary for
it, and whose voice is doing the capturing — read discovery-first, by two reading
conditions and three models, so that the disagreements between readers are themselves a
measurement.
**Hypotheses:** informational only. The reading targets the corpus. The factorial's bin
counts are the source for the v1-pool questions on 047, 049 and 050, which WU2.6 answers;
plan 1's twelve target hypotheses (019, 020, 021, 022, 028, 029, 031, 035, 038, 040, 043,
044) are pool questions, not targets of this read.
**Evidence sources:** The reading render — one markdown per arc (Brian's boundaries 1–5,
6–9, 10–13, 14–18, 19–22, 23–26, 27–32) plus Aris, chapter → plot point → link order,
attribution prefix per note, bold = AI text — generated once from `attribution.csv` by
`tools/StoryPlanner.VoiceAttribution --render` (the `v1-archive-mining` skill, "The
reading view"), Paratext and flagged notes excluded; `read-manifest.md`; the CSV by
script; lineage by id for verification of any classification a later candidate rests on.
Subject files for the 18 triage-labelled subjects, each with its scene-link list.
**Scope:**

*Design.* Two factors. Reading condition: **pathfinder** (the whole scene corpus in one
context, one job) versus **slice reader** (one job per arc file, plus Aris, plus one per
triage-labelled subject file). Model: Fable 5.1, Opus 4.6, Sonnet 5. Five arms, not six —
the pathfinder condition runs under Fable and Opus only (Sonnet is not a pathfinder
option, Brian 2026-09-03); the slice-reader condition runs under all three. The slice
condition is **staged**: arcs 6–9 and 19–22 first under all three models; the remaining
arcs, Aris and the subject files under **one** model chosen by Brian from the subset's
adjudication. The Sonnet pathfinder cell is not measured, and the card says so in every
artifact that reports the interaction.

*What each arm is.* A runner job under `fanout/WU2.5-v1-exploratory/`, launched from the
external fanout folder, with an identical explicit protocol: the `v1-archive-mining`
skill's reading protocol and locus-level record format handed over as `protocol.md` (unit
of attention = plot point or link; discovery-first; records not prose; one record per line
in the skill's exact shape; a plot point with nothing design-shaped gets no record), no
CLAUDE.md, no skills, no MCP, tools Read and Write. Arms are labelled neutrally (`arm-A`…)
and are blind to each other; the label → (condition, model) map lives in `read-manifest.md`
and is opened only at the adjudication step that needs it.

*The pathfinder arms are a deliberate exception to the runner's "one job, one item" rule.*
The item *is* the whole scene corpus, because the condition under test (047, 050) is
exactly that context. The card records this so no one re-splits it: `item` names the
render whole; `requireOnce` carries one marker per arc file so a partial read is a failed
attempt; `timeoutMinutes` is set generously (hours, not the classifier default) and a
pathfinder arm that times out is **a finding, not a failure** — recorded in the artifact
as such, with the attempt's ledger row. The first corpus-scale single job of 2026-09-03
produced nothing in 39 minutes; that is the prior. Model ids: `claude-fable-5-1[1m]`,
`claude-opus-4-6[1m]`. The Fable arm is scheduled `--at reset` when the weekly window is
fresh. Whether the Fable arm actually receives the 1M window is settled by its pilot: a
~265K-token prompt either runs or errors at once.

*Slice arms.* One job per arc file; the arc files are the enumeration (the render tool
lists them into `items/` with `manifest.md`); `make-jobs.*` beside the protocol generates
the jobs with neutral ids; `requireOnce` carries the record format's per-arc markers.
Model ids: `claude-fable-5-1[1m]` (an arc file is ~33K tokens, so the non-1m id would do;
the 1m id is used so the Fable arms share one id across conditions), `claude-opus-4-6`,
`claude-sonnet-5`. Subject files are slice items under the same protocol, read after the
arcs by the chosen model.

*Sequence inside the WU* (Brian, 2026-09-04): (1) pilot — one arc-file job under Opus, its
output read by a person, ledger row `Mode: pilot`; (2) batch 1 — arcs 6–9 and 19–22 under
all three models (six jobs); (3) adjudication 1 — the three subset record sets joined
mechanically on locus, disagreements binned, the read-manifest for those six arms opened,
drills adjudicated by Brian, and **Brian chooses the model for the remaining slice arms**
from what the bins show (a judgment, not a metric); (4) batch 2 — the remaining five arcs,
Aris and the 18 subject files under the chosen model, with fresh neutral labels; (5) the
two pathfinder arms, scheduled on a fresh window (may run any time after the pilot; their
manifest entries stay closed); (6) adjudication 2 — condition effect and interaction.

*Adjudication design.* Do not join all pairs. Spine: (a) **model effect** — the three
slice-reader sets against each other on the subset arcs; (b) **condition effect** (047's
disjointness question) — each of Fable's and Opus's slice-reader sets against the
*same-model* pathfinder set, on every arc that model read in both conditions; (c)
**interaction** (050) — from the two models that ran both conditions. Bins, extended
beyond plan 1's three: *cross-arc* (a `+` locus spanning arcs — a slice reader could not
have seen it; expected); *missed by the pathfinder*; *over-read by the fresh reader*
(unsupported by the notes at that locus); *missed by one model*; *missed by all but one
model*; *missed by every arm* (found only by drilling — recorded when adjudication
notices one). Counts per bin are findings; only the interesting bins are drilled, by
reading the render at that locus; Brian adjudicates the drills. "Supported by the notes
at that locus" needs a written rule before adjudication 1 — the WU's plan mode writes it.
Vocabulary is normalised across sets after the join, with the merge list kept.

*Artifact.* `WU2.5-v1-exploratory/` (directory): `read-manifest.md`, the per-arm record
sets as delivered (never edited), the two adjudications with bin counts and merge lists,
the adjudicated pattern inventory (pattern → loci → note ids → voice), and the synthesis
organised by what was observed — method (protocol hash, arms, models, harness, timeouts,
what was not measured), attribution picture from the CSV (owner type, arc, label, role;
pastes and lifts separately; PlanFirst / echo / stitched counts), arc sections, the
inventory, unexpected observations. Questions to the pools: the v1 pool (whatever the
read raises), the working-plan pool (whatever the archive shows that v2 should be checked
for), and any other pool a finding touches. Writes no candidates and no evidence.

*Does not.* Migrate anything; propose tracks or display questions; judge whether an
instinct is correct; decide whether a pasted proposal was adopted except per note from
plain neighbours and edits; open the named checks or hypothesis statements during the
read; run the attribution tool on the v2 plan; write to a `.storyplan`.

**Scale:** pathfinder × 2 (Fable, Opus, 1M context) + slice reader × (3 models × 2 subset
arcs + 1 model × 5 arcs + Aris + 18 subject files) ≈ 32 runner jobs plus one pilot; two
HITL adjudication sessions (Fable) and one synthesis session. Standing caveats on every
number: text-level provenance only; weekly snapshot resolution from 2025-12-26; R = 8.
**Preconditions:** none — the instrument, `attribution.csv`, the calibration records and
the arc boundaries exist (`CORPUS-STATUS.md`; `WU1.4-execution-plan.md` § Background as the
record of instrument validation). The 2025-12-23 backup is not needed.
**Status:** scoped

---

### WU2.6: V1-archive verification pass

**Type:** verification
**Corpus:** v1 archive (the notes are the source; the WU2.5 inventory is a locus map, never
the excerpt)
**Question:** For each frozen question in the v1 pool — the ~20 named checks plan 1 had
accreted, the factorial's three measurements, the voice-attribution census — what does the
archive show, with absence written as prominently as presence?
**Hypotheses:** per spec pool `v1-archive.md`, currently: 019, 020, 021, 022, 028, 029,
031, 035, 038, 040, 043, 044, 047, 049, 050, 010.
**Evidence sources:** The archive via the render and via MCP (`get_plot_points_archive`,
`get_links_archive`, `get_subjects_archive`, `get_chapters_archive`; the flagged tool
family for the walled notes of the subject items — fetched by the HITL session into the
run's committed `excerpts/`); `attribution.csv` by script; lineage by id; the WU2.5
adjudications (bin counts) for the 047/049/050 questions.
**Codebooks:** to calibrate, in `fanout/WU2.6-v1-verification/`: one **decision rule per
check** — evidence source, rule, what is written if absent — frozen and calibrated on a
sample with Brian's verdicts before the batch; the check names are the pool's entry
titles. The instruction-stack cell (049) uses the same items and codebook hash in two arms,
one explicit-context runner job and one HITL-context run (the only cell in the plan where
an agent deliberately carries CLAUDE.md), with label agreement as the measurement.
**Scope:** Focused-reader items (method discretion, fixed question): the check-named
subject sets ruled above — Chrysalis {12, 612, 266, 216}; Aquileian {232, 269, 230, 271,
276, 280, 281, 282, 284, 297, 298, 236, 418}; Pinkie Pie {23}; AJ The Collaborator {264};
TwiJack {601}; Friendship Letters = chapters 10–12 whole — each read under its frozen rule
with flagged notes included. Classifier items: the checks whose predicate is per-note or
per-locus (e.g. "does this note articulate an opposing position", "is this Demonstration-
shaped note behavioral or declarative"), enumerated by script from the inventory or by
search. Census items: the 019/020 contamination figures from the CSV (counts, no LLM), the
043 note-to-note question as a count of `+` loci in the adjudicated inventory, the 021
register question as whether Brian's `none`-tier annotations resolve into more than the
CSV's two roles. Factorial measurements: the 047 disjointness share, the 050 gap
comparison, the 049 agreement rate — computed from the WU2.5 adjudications by script and
written as candidates. Then `candidates.md`, referee run under `fanout/referee/<date>-
WU2.6/`, promotion, one commit. Rounds: round 1 when the first decision rules and the
factorial measurements are ready; later rounds as the exploratory pass and the pools grow.
Does **not** migrate, propose tracks, or clean data (the lint is a later activity with the
same tool).
**Scale:** focused reader (Opus, `claude-opus-4-6`) × 6 subject-set items; classifier
(Sonnet) over a few hundred note items; census by script; ~two dozen runner jobs per round
plus the referee's.
**Preconditions:** none.
**Status:** proposed

---

### WU2.7: Working-plan exploratory pass

**Type:** exploratory
**Corpus:** v2 working plan
**Question:** What does the working plan's data actually look like — state distributions,
track density, scene-level versus subject-level content, cognitive-mode population — and
what is the plan doing informally that no track was designed for?
**Hypotheses:** informational only; plan 1's WU1.7 listed 003, 004, 041, 042.
**Evidence sources:** MCP over the working plan: `get_stats`, `count_notes_plan`,
`get_track_definitions`, `list_subjects`, `list_stories`, `get_chapters_plan`,
`get_notes_plan`, `list_narrative_properties`, `list_subject_relations`.
**Scope:** Plan 1's WU1.7, re-typed: **mostly census** (tool-level counts — the note-state
distribution, per-track counts, plot-point-level versus subject-level note counts, ZF versus
ND population, tracks defined versus used) with a **small exploratory residue** — a
discovery read of the Reader Prior Belief Update, Reader Opinion, Character-Reader
Perception Gap and Theme Plan populations for what they carry informally (cross-focalizer
knowledge management, designed asymmetry, counterargument, affective investment). The
residue is slice readers by track (runner jobs under an explicit protocol, `mcp: true`
only if the item is not pre-fetched; prefer pre-fetched note dumps as items). The census
is a script and its output is a table in the artifact. Writes questions to the
working-plan pool (and, where v2 lacks what v1 had, to the v1 pool). Does **not** evaluate
the framework, mine note content for v1-style pattern inventories, or propose changes to
NoteState or tracks. Counts cite the tool; never hardcode them in a document.
**Scale:** census (script over MCP output) + slice reader × ~4 track populations (Opus);
one HITL session.
**Preconditions:** none.
**Status:** proposed

---

### WU2.8: Working-plan verification pass

**Type:** verification
**Corpus:** v2 working plan
**Question:** Which of the pool's questions about the plan survive a frozen-predicate read
of the notes — including the trial of the planner's eventual per-claim evidence mode?
**Hypotheses:** per spec pool `working-plan.md`, currently: 001, 003, 004, 019, 020, 028,
029, 031, 033, 041, 042, 044.
**Evidence sources:** The notes via MCP (pre-fetched into items); `attribution.csv`'s
sibling for v2 (the same `VoiceAttribution` invocation pointed at the working plan — the
lint census, read-only, no write path; implementation candidate "Voice linting protocol"
stays gated).
**Codebooks:** to calibrate, in `fanout/WU2.8-working-plan-verification/`: `line-crossing`
(the pool's investigator-trial predicate: does an output line propose, rank or evaluate
story content); `counterargument` (by copy from WU2.2 at its hash); `asymmetry-design`
(does a link's notes design one bonded partner for more interiority than the other);
others as the pool requires.
**Scope:** Round 1 is the investigator trial the pool already holds: a bounded set of
working-plan notes, "what evidence bears on this claim" as investigator jobs (`mcp: true`,
method discretion), with a `line-crossing` classifier over every output — the count of
crossings is the finding. The voice-lint census on v2 is a census item for 019/020.
Candidates, referee, promotion, one commit. Does **not** write to the `.storyplan`, propose
what a subject needs next, or persist lint spans.
**Scale:** investigator (Opus) × a bounded note set (tens); classifier (Sonnet) over their
outputs; census by tool.
**Preconditions:** none.
**Status:** proposed

---

### WU2.9: Lineage exploratory pass

**Type:** exploratory
**Corpus:** lineage (Google Doc revision history, Gemini conversations and reports, AI
Studio chats, NotebookLM captures; the Keep layer once WU2.23 lands)
**Question:** Read discovery-first, what does the founding-era record show about how
Brian's planning practice, vocabulary and perspective decisions moved — and where the
framework's concepts first appear?
**Hypotheses:** informational only; plan 1's WU1.5 and WU1.8 listed 002, 004, 005, 007,
014, 018, 019, 022, 028, 029, 039, 041, 042, 044.
**Evidence sources:** `search_lineage` / `get_lineage` / `list_lineage`; the gdoc diffs
(53) and, under scope `snapshots`, the 54 snapshots; the NLM notebooks (`nlm:3` Perspective
Analysis, 172 turns; `nlm:6` Aquileian Lore); `aistudio:6` (the fabula session); the
Gemini weekly reports.
**Scope:** Absorbs plan 1's WU1.8 (planning evolution) — the TLTT revision-history reading
runs now over the gdoc layer; the KU/NTL, GIYC and Falldale histories join when WU2.22 has
ingested them (a later round on the same card, not a new WU). Also the exploratory residue
of WU1.10's sub-questions 1 and 3 (era transitions as factor changes; the instruction
evolution from the Gemini gem's four rules through AI Studio system prompts). Slice
readers by layer and era (one job per notebook, per gdoc month of diffs, per Gemini week)
under an explicit protocol with the same locus-level record discipline as WU2.5 (locus =
lineage id; voice from the record's speaker; no hypothesis ids). Every per-layer caveat in
CLAUDE.md applies (damaged Gemini export; stubbed plan-pastes; NLM dates are authored). The
provenance point-checks — the thirteen chains and the nine-row table — are **not** read
here: they are frozen questions and belong to WU2.10. Questions to the lineage pool and, for
anything about what entered the plan, the v1 pool.
**Scale:** slice reader × ~20 (Opus) under the runner; one HITL adjudication where two
readers cover the same layer (run the NLM Perspective notebook under two arms; the rest
single-arm, with "no stability figure" stated).
**Preconditions:** none for the TLTT/Gemini/AI Studio/NLM layers.
**Status:** proposed

---

### WU2.10: Lineage verification pass

**Type:** verification
**Corpus:** lineage
**Question:** For each provenance claim in the pool — when a concept first appears, which
chain produced a vocabulary, whether v1's practice was hypothesize-gather-iterate — what
does the dated record show?
**Hypotheses:** per spec pool `lineage.md`, currently: 002, 004, 006, 007, 014, 015, 018,
019, 022, 028, 029, 034, 036, 039, 040, 041, 042, 045.
**Evidence sources:** Lineage by id (the turn, the diff, the report — fetched by the HITL
session into `excerpts/` for the referee; investigator jobs run with `mcp: true` against
the lineage tools).
**Codebooks:** to calibrate, in `fanout/WU2.10-lineage-verification/`: `first-appearance`
(a frozen rule for "the earliest dated lineage locus at which term T appears in sense S",
with the search alternation recorded); `chain-step` (does locus L show the step the chain
asserts — adopted / narrowed / rejected / absent); `practice-shape` (for a dated stretch of
turns: does Brian test and correct a claim, or accumulate it).
**Scope:** The **investigator** cell in the verify role — plan 1's WU1.5 provenance
point-checks (the nine-row table from `VERSION-HISTORY-DRAFT1.md`; the thirteen chains:
FID fixation, NLM vocabulary, epistemic method, M2, the Spark, Track 99's FID
prescription, perception gap's elevation and peers, every-link-must-have-T, the four gap
types, the v2 workflow stall, the read-generate-paste loop, the founding motivation, the
hopepunk thesis), moved here from WU1.5. One job per chain step or table row, the question
fixed and the search adaptive, the answer a locus with a date. Absence is a finding.
Candidates, referee, promotion, one commit. Does **not** judge whether prior work was
"wrong", propose replacements, or read the Keep layer before WU2.23 has ingested it (the
pool marks which questions wait on it).
**Scale:** investigator (Opus, `mcp: true`) × ~40 items; classifier (Sonnet) for the
`practice-shape` stretches.
**Preconditions:** none for the ingested layers. The Keep-dependent questions are gated on
WU2.23 (an infrastructure precondition, marked on each pool entry).
**Status:** proposed

---

### WU2.11: Conversations exploratory pass

**Type:** exploratory
**Corpus:** conversations (the imported Claude chat transcripts in the `.storyplan`, with
Brian's block states and navigation notes)
**Question:** Read discovery-first, what do the v2-era design conversations show about how
the framework's concepts were formulated and how Brian used the AI — and what do his
block states and notes mark?
**Hypotheses:** informational only; plan 1 pointed 002, 015, 017, 021, 022, 028, 029,
036, 041, 042 at this corpus through WU1.5 and WU1.10.
**Evidence sources:** `list_conversations`, `search_conversations`, `get_blocks`. The
framework-origin conversations — 8 (Conscience), 17 (multi-story fabula), 21 (perception
gap + data architecture, 151 blocks), 36 (planning vs writing, 72 blocks), 47 (note
categorization bootstrapping, 285 blocks), 53 (track taxonomy) — and 64 (P&K ASOIAF
inspirations, 289 blocks). Conversations 020 and 039 are **not** in the database (local
files only, `docs/design-conversations/`).
**Scope:** Slice readers, one job per conversation (block dumps pre-fetched as items;
`mcp` off), under an explicit protocol whose unit is the block and whose voice field
distinguishes Brian's turns from the assistant's and a block's authored `Summary` (his
navigation note) from `RawContent`. Records, not prose. Also the residue of WU1.10's
sub-question 4 on this corpus (Desktop interaction patterns). Questions to the
conversations pool; anything about what entered v2 to the working-plan pool.
**Scale:** slice reader × 7 (Opus); conversation 47 may need two slices (285 blocks).
**Preconditions:** none.
**Status:** proposed

---

### WU2.12: Conversations verification pass

**Type:** verification
**Corpus:** conversations
**Question:** For each pool question — which block narrowed the gap layer to FID, whether
block 1520 of conversation 47 states the every-link-must-have-T rule and on what reasoning,
whether the block states carry acceptance signal — what do the cited blocks show?
**Hypotheses:** per spec pool `conversations.md`, currently: 002, 015, 017, 021, 022, 028,
029, 036, 041, 042.
**Evidence sources:** Blocks by id (fetched into `excerpts/`); the block-state and
`Summary` fields as data.
**Codebooks:** to calibrate, in `fanout/WU2.12-conversations-verification/`: `chain-step`
(by copy from WU2.10 at its hash); `acceptance-signal` (does a block's state and
subsequent plan content show adoption / rejection / no signal); `speaker-register`
(Brian's analytical voice / assistant framing / Brian's navigation note — per block).
**Scope:** Investigator and classifier jobs over cited blocks; candidates, referee,
promotion, one commit. Does **not** rewrite anyone's summary, and never substitutes an
excerpt for an absent one.
**Scale:** investigator (Opus) × ~15 items; classifier (Sonnet) over a few hundred blocks
for the signal and register questions.
**Preconditions:** none.
**Status:** proposed

---

### WU2.13: Code-sessions exploratory pass

**Type:** exploratory
**Corpus:** code sessions (`codesessions.db`)
**Question:** Read discovery-first, what does the engineering archive show about how the
instrument was built — which factors changed when, how instructions evolved, what was tried
and cut, and how the models behaved across activity types?
**Hypotheses:** informational only; plan 1's WU1.10 listed 006–018 and its two observed
model properties.
**Evidence sources:** `codesessions.db` by sqlite3 (`code-sessions` skill recipes); the git
log of CLAUDE.md and the skills as the dated instruction stack; `VERSION-HISTORY.md` for
the factual timeline.
**Scope:** A **census** first (script: sessions by project and month, tool-call stubs by
tool, MCP tool use by name, subagent counts, user-turn correction patterns by keyword) —
the material for 016/017 (which consumer queries which source) and for 009's retrospective
claim. Then slice readers over authored session extracts (the query is the enumerator: a
HITL session writes the matching turns into `items/`; a runner child cannot execute
sqlite3) for the instruction-evolution and model-behaviour questions — including WU1.10's
two observations of 2026-08-31 (systematic principle application; scope of initiative),
read as questions, not findings. Records with session id and `Seq`. Questions to the
code-sessions pool. Does **not** treat a transcript as authoritative for anything
(FEATURE-AUDIT first), and does not read the fanout launch folder's transcript directory
(there is none by construction).
**Scale:** census by script; slice reader × ~10 extracts (Opus).
**Preconditions:** the purge — **met 2026-09-04** (both selects zero). Re-check before
enumerating: a later ingest could not restore the purged rows, but the check is cheap.
**Status:** proposed

---

### WU2.14: Code-sessions verification pass

**Type:** verification
**Corpus:** code sessions
**Question:** For the pool's questions about the buildout's own process — does a design
authored in a session violate rules loaded in that session at a measurable rate; does an
auditor tuned to over-flag, given the author's drop list, flag fewer intended changes
without drifting; do per-section batching and per-unit jobs yield the same relation labels;
plus the model-behaviour and instruction-design questions — what do the classified turns
show?
**Hypotheses:** per spec pool `code-sessions.md`, currently: 005, 009, 010, 011, 013, 014,
015, 016, 017, 044, 048, 049.
**Evidence sources:** Turns and tool stubs written into `items/` by an enumerator script
beside the codebook (the query is recorded and hashed with the run); for the batching
question, the `fanout/skill-audits/2026-09-03-v3-buildout/` results as one arm and a
per-unit re-run as the other.
**Codebooks:** to calibrate, in `fanout/WU2.14-code-sessions-verification/`:
`rule-violation` (given a rule text and a session turn, does the turn's action violate it —
the 048-family predicate); `principle-error` (WU1.10's error class: convenience overriding a
stated principle — per decision turn); `initiative-scope` (narrow / proactive per response,
with the activity type from the enumerator); `relation-label` (the skill-audit protocol's
restated / narrowed / broadened / reversed / absent / delegated / non-instructional — by
copy from `fanout/skill-audits/protocol.md` at its hash).
**Scope:** Classifier jobs over enumerated turns; the auditor-with-drop-list cell as a
two-arm run over the skill-audit's own units (with / without the drop list); the batching
cell as per-unit jobs over the same 174 units compared to the per-section results. Absence
of an error class is a finding. Candidates, referee, promotion, one commit. Does **not**
run controlled model comparisons outside the runner's one-factor cells, and does not
build an instruction audit framework (implementation candidate, gated on 014).
**Scale:** classifier (Sonnet) × a few hundred turn items; auditor (Sonnet) × 18 sections
× 2 arms; ~200 per-unit jobs for the batching cell.
**Preconditions:** the purge (met 2026-09-04, re-checked at enumeration).
**Status:** proposed

---

### WU2.15: Retroactive referee pass

**Type:** verification
**Corpus:** the hypothesis record — every pre-revision evidence entry (forty across
twenty hypotheses on 2026-09-03); the sources are whichever corpora the entries cite
(analysis corpus, own fiction, Keep export)
**Question:** Which pre-revision entries survive a blind discrimination test, and by how
much does the blind clause differ from the self-administered one (048)?
**Hypotheses:** 048 (the measurement); every hypothesis carrying a pre-revision entry
(023–040, 045, 046) as targets whose statuses recompute.
**Evidence sources:** The entries' cited sources, fetched by the promotion session into
`excerpts/` — the meta-analysis section, the per-story analysis section, the story text
where an entry cites a line, the Keep note; the entries themselves as candidates.
**Codebooks:** `referee` — `fanout/referee/codebook.md`, drafted and **uncalibrated**;
calibration is this WU's first task and its calibration record is
`fanout/referee/calibration-<date>.md` (the runner's stage detector matches that prefix).
Sample ≥ 20 candidates spanning the three classes and ≥ 3 hypotheses, drawn from the
pre-revision entries; referee blind, Brian independently, adjudicated, rulings recorded,
re-hashed; no batch before the record exists.
**Scope:** The sequence in `retroactive-referee-pass-handoff.md`: calibrate; build
`fanout/WU2.15-retroactive-referee/candidates.md` (one candidate per entry, finding and
source copied verbatim, `proposed-by: retroactive / <original WU> / <original timestamp>`;
entries citing only an intermediate analysis marked for R2/R5 scrutiny); run the referee
under `fanout/referee/<date>-retroactive/` (the enumerator `fanout/referee/make-jobs.*`,
one job per candidate and target, excerpts committed outside `items/`, `requireOnce` the two
verdict-line markers, pilot on three); measure — per candidate, same / narrower / different
/ original vacuous, the table that is 048's evidence and is itself a candidate; promotion
session with Brian — entries from exploratory WUs return to spec-pool status pending that
corpus's round (this plan has already seeded the questions; the pass links each declined
entry to its pool entry), entries whose blind clause survives stand re-promoted verbatim
with the new referee line, the rest declined with reasons, old entries never deleted
(`(superseded by re-referee <date>)`), statuses recomputed, `INDEX.md` updated, **one
commit**; report counts per class, per originating WU, agreement between clauses, and what
the pipeline's first live run revealed about itself. Must not: rewrite a finding, edit a
tag in place, delete an entry, promote anything Brian has not reviewed in the diff, run
under an uncalibrated hash, treat "consistent with" as diagnostic.
**Scale:** classifier (referee, Sonnet, `mcp: false`, tools Read/Write) × ~40 jobs plus
the calibration sample; one HITL promotion session (Fable).
**Preconditions:** none. Ruled 2026-09-03 not to precede this plan.
**Status:** proposed

---

### WU2.16: Cross-corpus comparison

**Type:** synthesis
**Corpus:** analysis corpus, own fiction, v1 archive
**Question:** What do the stories Brian reads do, versus what Brian instinctively does,
versus what v1 captured — and where do the three converge or diverge?
**Hypotheses:** 023, 024, 025, 026, 027, 028, 029, 030, 033, 034, 035, 037, 038, 039, 040,
046.
**Evidence sources:** The verified artifacts of WU2.2, WU2.4 and WU2.6 (candidates with
verdicts, promoted entries) and the exploratory artifacts of WU2.1, WU2.3 and WU2.5 as
leads for where a question came from.
**Scope:** Plan 1's WU1.9, stripped of the verification-shaped specs it had accreted (the
046 classification, the counterargument re-read, the obstacle breakdown are now WU2.2's and
WU2.4's pool questions). Four axes: confirmation of instinct; techniques in the corpus Brian
does not use ("potential learning" is a description, never a recommendation); Brian's
distinctive patterns; the voice-separation overlay from the v1 attribution picture. The
comparison questions plan 1 listed (dream-function mix; letters as subplot transition;
canon-virtue traps across Salvation, Dash's New Mom, P&K and TLTT; Celestia as
recontextualization vessel; shame-about-desire mechanisms) run only over verified rounds
and otherwise become pool questions. Writes the artifact and pool questions; never a
candidate. Does **not** rank, propose tracks, or apply the favorites lens.
**Scale:** HITL session, Fable, over verified artifacts.
**Preconditions:** none. (Waits on verification rounds of all three corpora — readable off
the status board, not a precondition.)
**Status:** proposed

---

### WU2.17: Retrospective — framework provenance

**Type:** synthesis
**Corpus:** lineage, conversations; the analysis corpus and own fiction where a chain
compares a concept against them
**Question:** Where did the framework's vocabulary come from, why did some prescriptive
reasoning land (M2, the Spark) and some overfit (the FID prescription), and was v1's
practice already hypothesize-gather-iterate?
**Hypotheses:** 002, 004, 005, 007, 014, 019, 022, 028, 029, 034, 036, 041, 042, 044.
**Evidence sources:** WU2.10's and WU2.12's verified chain steps and first-appearance loci;
WU2.2's and WU2.4's verified findings where a chain assesses a concept against them;
`VERSION-HISTORY.md` for the factual timeline.
**Scope:** Plan 1's WU1.5 without its point-checks (moved to the lineage pool as WU2.10's
work). Per-concept provenance narratives assembled from verified chain steps, positioned
without judgment (best effort with the data available). Every insight that bears on a
hypothesis becomes a pool question. Does **not** propose replacements or write to
`hypotheses/`.
**Scale:** HITL session, Fable.
**Preconditions:** the Keep ingest (WU2.23) gates the chains that need pre-AI timestamps
(2, 3, 11); the synthesis may run on the other chains and say which it left out.
**Status:** proposed

---

### WU2.18: Favorites and supplementary lens

**Type:** synthesis
**Corpus:** analysis corpus (verified rounds); supplementary material (Brian's tiers,
comments and reviews — his analytical voice, read as data, not as evidence about the
stories)
**Question:** Does what sticks with Brian correlate with verified technique patterns, and
do his instinctive reactions in comments and reviews name the patterns the analyses name?
**Hypotheses:** 021, 028, 029, 033, 034, 038.
**Evidence sources:** WU2.2's verified per-story classifications; WU2.16's artifact;
`corpus-favorites-tiers.txt`; `P&K comments.md`, `pax-chrysalia-comments.md`,
`Comments.md`; `Filly Fooling review.txt` and `Filly Fooling analysis.csv`; the Special
tier's context.
**Scope:** Plan 1's WU1.11. Tier clustering is a census over verified classifications by
tier (counting, never a score); comment mapping and the Special-tier reading are pool
questions for the analysis corpus where a frozen predicate exists ("does comment C name
pattern P that analysis A names"), and otherwise a synthesis observation. The biasing
caveats travel with the artifact: comments skew to P&K and Pax Chrysalia; the review is
Brian theorizing; Abandoned-tier analyses may cover unread content. Does **not** rank
stories or propose adoption.
**Scale:** HITL session, Fable.
**Preconditions:** none (tiers complete 2026-08-29).
**Status:** proposed

---

### WU2.19: Pipeline synthesis

**Type:** synthesis
**Corpus:** lineage, code sessions, conversations; the v1 archive for the factorial's
verified measurements
**Question:** What does the verified evidence say about the four-factor decomposition,
model-intrinsic properties and their interaction with context and instruction stack, and
instruction design — and are the pipeline hypotheses separable from the framework ones?
**Hypotheses:** 006, 007, 008, 009, 010, 011, 012, 013, 014, 015, 016, 017, 018, 047, 048,
049, 050.
**Evidence sources:** WU2.6's verified factorial measurements (047, 049, 050), WU2.15's
measurement (048), WU2.10's, WU2.12's and WU2.14's verified findings.
**Scope:** Plan 1's WU1.10, with its evidence-gathering moved into the corpus pairs and the
"untestable" sub-question now answered by the runner's one-factor cells. The 008
separability test is a comparison of evidence bases across the verified candidates (which
corpora each domain's promoted entries cite). Hypothesis 012 is likely to remain untested
by this plan: the factorial compares Claude models only, and a cross-family comparison is
not in scope — the synthesis says so rather than inferring. Writes the artifact and pool
questions. Does **not** build an instruction audit framework or implement data-source
unification.
**Scale:** HITL session, Fable.
**Preconditions:** none.
**Status:** proposed

---

### WU2.20: Framework and architecture evaluation

**Type:** synthesis
**Corpus:** all seven, as verified
**Question:** What should the planner track, at what scope, and in what conceptual
hierarchy — on the verified evidence?
**Hypotheses:** 001, 023, 024, 025, 026, 027, 028, 029, 030, 033, 034, 035, 036, 037, 041,
042, 043, 044, 046.
**Evidence sources:** WU2.16, WU2.17, WU2.18 artifacts; WU2.8's verified findings; the v2
track definitions via `get_track_definitions`; the promoted record.
**Scope:** Plan 1's WU1.13 — the seven evaluation areas (three concerns; scope levels;
track coverage including Track 99's FID prescription, the Character Development track's
direction assumption, the Writing Techniques track's shape, M3's gradient, counterargument
support, DT-based gap vocabulary, the three candidate design targets; goal categories and
the boundary; the multi-story dimension; note-to-note relationships; the bespokeness
tension). Each area is written over verified rounds only; an area whose evidence is
unverified is named as waiting, not filled from leads. Presents findings and tradeoffs;
Brian decides. Does **not** propose specific tracks, write display questions, or author
cognitive-mode definitions.
**Scale:** HITL session, Fable, large.
**Preconditions:** none.
**Status:** proposed

---

### WU2.21: Connection to TLTT

**Type:** synthesis
**Corpus:** the working plan (verified), the analysis corpus (verified), and WU2.20's
artifact
**Question:** What do the framework findings mean for TLTT and the multi-story project —
per-story focalization strategy, scene-level readiness, paradigm connections, reading-order
implications, trajectory management, thematic vocabulary fit, the Faust-era versus
Hasbro-mandate characterization question?
**Hypotheses:** 001, 037, 044.
**Evidence sources:** WU2.20; `long-corpus-categories.txt`; the working plan via MCP; the
multi-story architecture as Brian has entered it.
**Scope:** Plan 1's WU1.14. The Faust/Hasbro and scene-readiness items are pool questions
for the working plan (seeded) and are consumed here once verified. Does **not** propose
plot changes, write prose, suggest content, or say what any subject needs next.
**Scale:** HITL session, Fable.
**Preconditions:** none.
**Status:** proposed

---

### WU2.22: Revision-history preprocessing and ingest

**Type:** infrastructure
**Corpus:** lineage (the gdoc layer, extended to KU/NTL, GIYC and Falldale)
**Question:** Can the non-TLTT planning-document histories be ingested through
`GDocHistory` so the lineage passes read them?
**Hypotheses:** none targeted; unblocks lineage-pool questions on 002, 004, 018, 039.
**Evidence sources:** Raw exports in `source_material_references/
Planning_Document_Revision_History/` (a mix of appscript `.txt` and hand-copied plain
text); the TLTT merged-snapshot precedent.
**Scope:** Brian preprocesses the exports into per-story daily-snapshot folders of the
shape the TLTT ingest consumed; then `dotnet run --project tools/StoryPlanner.GDocHistory
-- <config> [--apply]` per story (dry run first; the ingest replaces only its own tables
and appends to `IngestRuns`). Acceptance: the dry run's diff count per story, tests green,
`CORPUS-STATUS.md` updated. Any question the build raises goes to the lineage pool.
**Scale:** instrument; Brian's preprocessing plus one Claude Code session per ingest.
**Preconditions:** Brian's preprocessing (unchanged from plan 1; an infrastructure gate,
not a WU dependency).
**Status:** proposed

---

### WU2.23: Keep sidecar ingest

**Type:** infrastructure
**Corpus:** lineage (a new `keep:` layer)
**Question:** Can the provenance-relevant Keep notes be ingested selectively into
`lineage.db` under an authored include-list, credentials excluded?
**Hypotheses:** none targeted; 045's already-assessed feasibility is the design input
(its entries are re-checked by WU2.15 like any other); unblocks the lineage-pool
questions that need pre-AI timestamps.
**Evidence sources:** The Takeout export at `C:/Users/Brian/Documents/Google Drive
Analysis/`; the five curatorial artifacts beside it; `WU1.2-keep-assessment.md`.
**Scope:** A new ingest tool in the sidecar pattern (own tables, `IngestRuns` ledger,
`keep:` ids, authored include-list config distinguishing content-unique, timestamp-unique
and non-unique notes; the ~130 credential-bearing notes excluded by rule and by list;
dry run names every included and excluded note). Tests per the `testing` skill.
`CORPUS-STATUS.md` updated; the lineage tools disclose the layer. The include-list is
authored by Brian from the curatorial guide — the ingest never auto-detects relevance.
**Scale:** instrument; one or two Claude Code sessions plus Brian's list.
**Preconditions:** Brian's authored include-list.
**Status:** proposed

## Execution — status board

Regenerated from the cards and pools whenever they change. Not a sequence.

| Corpus | Exploratory | Verification rounds | Open pool questions |
|---|---|---|---|
| Analysis corpus | complete (WU2.1) | none run (WU2.2 proposed) | see `spec-pools/analysis-corpus.md` |
| Own fiction | complete (WU2.3) | none run (WU2.4 proposed) | `own-fiction.md` |
| v1 archive | scoped, not run (WU2.5) — **Brian's first choice** | none run (WU2.6 proposed) | `v1-archive.md` |
| Working plan | proposed (WU2.7) | none run (WU2.8) | `working-plan.md` |
| Lineage | proposed (WU2.9) | none run (WU2.10) | `lineage.md` |
| Conversations | proposed (WU2.11) | none run (WU2.12) | `conversations.md` |
| Code sessions | proposed (WU2.13) | none run (WU2.14) | `code-sessions.md` |

| Synthesis | Waits on |
|---|---|
| WU2.16 cross-corpus comparison | a verification round on the analysis corpus, own fiction and the v1 archive covering its questions |
| WU2.17 retrospective | rounds on lineage and conversations; WU2.23 for the Keep-dependent chains |
| WU2.18 favorites lens | a round on the analysis corpus; WU2.16 |
| WU2.19 pipeline synthesis | rounds on the v1 archive (the factorial measurements), code sessions, lineage, conversations; WU2.15 |
| WU2.20 framework evaluation | WU2.16, WU2.17, WU2.18; a round on the working plan |
| WU2.21 connection to TLTT | WU2.20; a round on the working plan |

| Cross-cutting / infrastructure | State |
|---|---|
| WU2.15 retroactive referee pass | proposed; referee codebook uncalibrated |
| WU2.22 revision-history ingest | waits on Brian's preprocessing |
| WU2.23 Keep ingest | waits on Brian's include-list |

Exploratory passes are unordered among themselves; v1 first is Brian's choice. A
verification round is due when its pool holds questions with a calibrated codebook.
Readiness, convenience, throughput and duration are never ordering inputs.

## Per-hypothesis coverage

Derived from the cards and the pools on 2026-09-04; regenerated, never maintained by hand.
Verification WUs are listed by the pool that names the hypothesis; exploratory WUs are
omitted (they target corpora, not hypotheses).

| ID | Verification | Synthesis / other |
|---|---|---|
| 001 | 2.8 | 2.20, 2.21 |
| 002 | 2.10, 2.12 | 2.17 |
| 003 | 2.8 | — |
| 004 | 2.8, 2.10 | 2.17 |
| 005 | 2.14 | 2.17 |
| 006 | 2.10 | 2.19 |
| 007 | 2.10 | 2.17, 2.19 |
| 008 | — (a comparison of verified evidence bases) | 2.19 |
| 009 | 2.14 | 2.19 |
| 010 | 2.6, 2.14 | 2.19 |
| 011 | 2.14 | 2.19 |
| 012 | — (likely untested by this plan; 2.19 says so) | 2.19 |
| 013 | 2.14 | 2.19 |
| 014 | 2.10, 2.14 | 2.17, 2.19 |
| 015 | 2.10, 2.12, 2.14 | 2.19 |
| 016 | 2.14 | 2.19 |
| 017 | 2.12, 2.14 | 2.19 |
| 018 | 2.10 | 2.19 |
| 019 | 2.6, 2.8, 2.10 | 2.17 |
| 020 | 2.6, 2.8 | — |
| 021 | 2.6, 2.12 | 2.18 |
| 022 | 2.6, 2.10, 2.12 | 2.17 |
| 023 | 2.2, 2.4, 2.15 | 2.16, 2.20 |
| 024 | 2.2, 2.4, 2.15 | 2.16, 2.20 |
| 025 | 2.2, 2.15 | 2.16, 2.20 |
| 026 | 2.2, 2.15 | 2.16, 2.20 |
| 027 | 2.2, 2.15 | 2.16, 2.20 |
| 028 | 2.2, 2.4, 2.6, 2.8, 2.10, 2.12, 2.15 | 2.16, 2.17, 2.18, 2.20 |
| 029 | 2.2, 2.4, 2.6, 2.8, 2.10, 2.12, 2.15 | 2.16, 2.17, 2.18, 2.20 |
| 030 | 2.2, 2.4, 2.15 | 2.16, 2.20 |
| 031 | 2.2, 2.4, 2.6, 2.8, 2.15 | — |
| 032 | 2.2, 2.4, 2.15 | — |
| 033 | 2.2, 2.4, 2.8, 2.15 | 2.16, 2.18, 2.20 |
| 034 | 2.2, 2.4, 2.10, 2.15 | 2.16, 2.18, 2.20 |
| 035 | 2.2, 2.6, 2.15 | 2.16, 2.20 |
| 036 | 2.2, 2.4, 2.10, 2.12, 2.15 | 2.20 |
| 037 | 2.2, 2.4, 2.15 | 2.16, 2.20, 2.21 |
| 038 | 2.2, 2.4, 2.6, 2.15 | 2.16, 2.18 |
| 039 | 2.4, 2.10, 2.15 | 2.16 |
| 040 | 2.4, 2.6, 2.10, 2.15 | 2.16 |
| 041 | 2.8, 2.10, 2.12 | 2.17, 2.20 |
| 042 | 2.8, 2.10, 2.12 | 2.17, 2.20 |
| 043 | 2.6 | 2.20 |
| 044 | 2.6, 2.8, 2.14 | 2.17, 2.20, 2.21 |
| 045 | 2.10, 2.15 | 2.23 (infrastructure) |
| 046 | 2.2, 2.4, 2.15 | 2.16, 2.20 |
| 047 | 2.6 | 2.19 |
| 048 | 2.15, 2.14 | 2.19 |
| 049 | 2.6, 2.14 | 2.19 |
| 050 | 2.6 | 2.19 |

## Codebooks this plan names

No shared codebooks folder: an instrument lives with the work that authors and calibrates
it, and each verification card names its codebooks as "to calibrate" with calibration as
the card's first task.

| Codebook | Lives in | State |
|---|---|---|
| `referee` | `fanout/referee/codebook.md` (every referee run under `fanout/referee/`) | drafted, uncalibrated — WU2.15's first task |
| `dt-classes` (046) | `fanout/WU2.2-analysis-verification/` | to write and calibrate; WU2.4 applies it at the same hash |
| `counterargument`, `obstacle-type`, `perspective-mode` | `fanout/WU2.2-analysis-verification/` | to write and calibrate |
| `comedy-position`, `gap-delivery`, `fabula-delivery` | `fanout/WU2.4-own-fiction-verification/` | to write and calibrate |
| the v1 check decision rules (one per check) | `fanout/WU2.6-v1-verification/` | to write and calibrate |
| `line-crossing`, `asymmetry-design` | `fanout/WU2.8-working-plan-verification/` | to write and calibrate |
| `first-appearance`, `chain-step`, `practice-shape` | `fanout/WU2.10-lineage-verification/` | to write and calibrate |
| `acceptance-signal`, `speaker-register` | `fanout/WU2.12-conversations-verification/` | to write and calibrate |
| `rule-violation`, `principle-error`, `initiative-scope` | `fanout/WU2.14-code-sessions-verification/` | to write and calibrate |
| `relation-label` | `fanout/skill-audits/protocol.md` | exists (`protocol.md@f30011c4ba9a` at the audit); WU2.14 cites it |

Reading protocols (exploratory arms) are not codebooks — they are piloted, not calibrated —
and live with their WU: `fanout/WU2.5-v1-exploratory/protocol.md` and the equivalents for
WU2.7, WU2.9, WU2.11 and WU2.13.

## Preconditions summary

Tooling or Brian-action blockers only; never WU dependencies.

| Precondition | Gates | Owner | State (2026-09-04) |
|---|---|---|---|
| Referee codebook calibration | WU2.15's batch (and every later referee run) | Claude Code + Brian's verdicts | not started; first task of WU2.15 |
| Revision-history preprocessing | WU2.22, hence the non-TLTT lineage questions | Brian | not started |
| Keep include-list | WU2.23, hence the Keep-dependent lineage questions | Brian | not started |
| codesessions.db purge | WU2.13, WU2.14 | Brian | **met** (both selects zero, 2026-09-04) |
| Fable 1M window in print mode | WU2.5's Fable pathfinder arm | settled by the pilot | open — a check, not a decision |
| Story markdowns reachable by a runner job | WU2.2 | `addDirs` or copied excerpts | available |

## What this plan does not do

Write to any `.storyplan`. Write to `hypotheses/` (only WU2.15's and the verification
passes' promotion sessions do, in reviewed commits). Order the exploratory passes beyond
Brian's stated first choice. Carry plan 1's testing specs on cards (they are in the pools).
Re-house WU1.12 (dissolved into promotion and consolidation). Propose story content,
tracks, display questions, or what a subject needs next.
