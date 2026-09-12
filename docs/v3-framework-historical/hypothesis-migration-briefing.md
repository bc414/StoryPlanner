# Hypothesis migration — historical data and tracing methodology

Written 2026-09-12, for the migration of the fifty hypothesis files into the schema of
`hypothesis-file-schema.md`. It is read by **one session per hypothesis**.

**This document holds no citations.** It names what exists, who wrote it, when, and in what
order, so that a session handling one hypothesis can find that hypothesis's own chain itself.
Every claim below about a specific file was verified on 2026-09-12 against the artifacts named;
the per-hypothesis work was not done and is the session's job.

**Why it carries no citations.** The failure this trace uncovered is a session compressing
Brian's words into a summary that downstream readers then treated as the source. A citation
table written by one session and read by fifty would repeat exactly that. Each session reads
the primary text.

---

## 1. The primary sources

| what | where | when | whose words |
|---|---|---|---|
| Google Keep dump | `source_material_references/hypotheses-google-keep-dump.md` | before 2026-08-30 | **Brian, typed** |
| Long-corpus categories | `source_material_references/long-corpus-categories.txt` | 2026-08-28 | **Brian, typed** ("which I authored by hand") |
| Consolidation prompt — the `E`-items | codesessions `0ca45d8f-d8b3-476e-a5ac-79d92c2aabf2`, Seq 1 | 2026-08-31 | **Brian, typed** |
| Deliberation transcripts | codesessions, sessions in §2 | 2026-08-18 → 2026-09-03 | **Brian's `user` rows are typed**; assistant rows are not |
| `D1–D20`, `T1–T3` | `docs/v3-framework-historical/pipeline-hypotheses-raw.md` | 2026-08-30 | a session, reorganizing the Keep dump |
| `H1–H28` and the analysis bodies | `docs/ANALYSIS-SYNTHESIS-PLAN.md` | 2026-08-28/29 | a session, synthesizing corpus analysis + Brian's deliberation |
| Consolidation plan — Summary, Sources, tier notes | `docs/v3-framework-historical/consolidation-1-plan.md` | 2026-08-31 | a session; **Brian reviewed and amended it** |
| The fifty files as they stand | `docs/v3-framework/hypotheses/NNN-slug.md` | 2026-08-31 → 2026-09-03 | see §3 |

Reading the archive: `.claude/skills/code-sessions/SKILL.md`. The citation rule there is
load-bearing — a `Typed:` line is Brian's own prose and weighs as much as a freestanding
prompt; a `Chose:` line is a Claude-authored label he selected and is **never** quoted as his
words.

---

## 2. Chronology

| date | event | artifact produced |
|---|---|---|
| 2026-08-17 | meta-analysis of 42 stories | the findings later labelled "the original 8" |
| **2026-08-18** | **Brian asks for "a 'halfway through corpus analysis' document that holds hypotheses for how the story planner framework should evolve"; answers that it lives in `docs/`** | the practice itself. Session `e16846b2-b99b-4ae4-b3da-7e3e1f60d73e` |
| 2026-08-27 → 29 | the long-corpus run, and Brian's deliberation over its results; hypothesis *testing* becomes a pipeline stage | `ANALYSIS-SYNTHESIS-PLAN.md`, `H1–H28`. Session `ce84e603-f900-46b9-a5c2-d7b7244b20e3` |
| 2026-08-30 | the Keep dump is reorganized; the file format, the epistemic vocabulary and one-file-per-hypothesis are designed | `pipeline-hypotheses-raw.md`. Session `365ef91d-ffa9-4b25-a45d-f5ac00c0a37a` |
| 2026-08-31 | the consolidation plan; then the files | `consolidation-1-plan.md`, the fifty files. Session `0ca45d8f-d8b3-476e-a5ac-79d92c2aabf2` |
| 2026-09-02/03 | design conversation | hypotheses 046–050 |
| 2026-09-04 onward | the strong form — referee, falsifier, promotion | the current method |

**The founding structure, and it still holds.** On 2026-08-18 a *hypothesis* was already an idea
awaiting evidence: written down **because the corpus analysis was unfinished**, to be tested
against the remaining stories once it completed. Brian, 2026-09-12, on what he meant: "It was an
idea that might come to pass, but had to be verified against the rest of the fimfiction stories.
The goal was to write those 8 down, then when the analysis of the other stories finished, I would
test those hypotheses against the evidence. Then that concept expanded to all my corpora and the
method itself."

So the scope widened — one corpus, then every corpus, then the method — while the structure did
not change. The v3 strong form did not replace the founding sense of the word; it formalized it,
adding the referee, the falsifier and promotion. A session should read a 2026-08-31 statement as
what it was written to be: an untested claim awaiting evidence.

---

## 3. The trajectories

Six chains, differing in where they begin and how many hands the content passed through.
A session's search path is determined by its hypothesis's trajectory.

### K — Keep dump lineage

`Brian's Keep note → D/T entry (2026-08-30) → plan Summary → subagent file`

**Search path:** find the passage in `hypotheses-google-keep-dump.md` that the `D`/`T` entry
paraphrases; read `pipeline-hypotheses-raw.md`'s entry; read the plan row; read the file.

**Health: the worst in the set. Hop 1 is verified lossy, and one claim was inverted.** See §4.
Brian's typed words exist for every hypothesis here, so every loss is recoverable.

### P — Consolidation-prompt lineage

`Brian's 2026-08-31 prompt → plan Summary → file`

**Search path:** find the `E`-item in codesessions `0ca45d8f`, Seq 1; read the plan row; read
the file. One hop, his words, shortest chain in the set.

### CD — Corpus-deliberation lineage

`Brian's deliberation (2026-08-27→29) → H-entry "From this conversation" → plan Summary +
a per-file "Created entry should cover:" directive → subagent file`

**Search path:** read the `H`-entry under `ANALYSIS-SYNTHESIS-PLAN.md` § "From this conversation
(2026-08-28/29)", and the document's body sections on the same subject, which are **outside** the
range the subagents were given; then search `ce84e603` for Brian's `user` turns on that subject;
then the plan row and the file.

**Health: good where it was checked.** The main session had read the whole plan and encoded
nuance in the per-file directives, which is why upstream detail arrived intact in the files
spot-checked on 2026-09-12.

### C8 — Original-8 lineage

`42-story meta-analysis (2026-08-17) → H1–H8 → plan Summary → subagent file`

**Search path:** the `H`-entry under § "From the original 8 (2026-08-17, derived from 42
stories)"; then session `e16846b2` (2026-08-18) for Brian's deliberation over those results;
then the plan row and the file.

**Health: the thinnest upstream in the set.** These `H`-entries are one line each. The
plan Summary and then the file each added material; where that material has no source, it is
manufactured (§4).

### S — Synthesis-plan section

Source is a `Synth.§` section rather than a numbered entry. **Search path:** locate the section
in `ANALYSIS-SYNTHESIS-PLAN.md` by its name, then as CD.

### M — Design-conversation mints (046–050)

`Brian's turns in the 2026-09-02/03 conversation → file`, with his endorsement recorded in the
entry ("Brian endorsed the statement on 2026-09-03").

**Search path:** codesessions, 2026-09-02/03, plus the file's own entry, which names what raised
it. Best-attested provenance in the set; these cite WU numbers retrospectively, which is
provenance and not a prospective coupling.

### Assignment

| id | slug | sources | trajectory |
|---|---|---|---|
| 001 | planner-purpose-trajectories | Synth.§Purpose | S |
| 002 | epistemic-method-provenance | E4, E9, E5 | P |
| 003 | epistemic-vocabulary-for-content | E1, E2, E3 | P |
| 004 | working-cadence-sweeps | E6, E18 | P |
| 005 | recall-vs-evidence | E8 | P |
| 006 | four-factor-decomposition | D1, H25, H26 | K + CD |
| 007 | version-labels-as-bookmarks | D2, D6 | K |
| 008 | framework-vs-pipeline-separable | D3 | K |
| 009 | v3-tooling-decouples-factors | D4, D15 | K |
| 010 | model-intrinsic-properties | D7 | K |
| 011 | model-comparison-convergence | D8, D11, T1, T2 | K |
| 012 | constitutional-ai-constraint | D9 | K |
| 013 | fable-role | D10 | K |
| 014 | evidence-based-instruction-design | D12, D14, E14, Synth.§Downstream | K + P + S |
| 015 | acceptance-signals | D13 | K |
| 016 | data-source-unification | D16 | K |
| 017 | desktop-vs-code-split | D17 | K |
| 018 | target-usage-loop | D18, E12 | K + P |
| 019 | ai-context-contradiction | H22, H23, H24, Synth.§AIContradiction, Synth.§V2Stall | CD + S |
| 020 | voice-separation-prerequisite | H22, H23, H24, Synth.§VoicePrereq | CD + S |
| 021 | five-voice-registers | H27 | CD |
| 022 | voice-treatment-protocol | E10, E11 | P |
| 023 | three-concern-separation | H18 | CD |
| 024 | dimensional-vs-hierarchical | H18 | CD |
| 025 | mechanism-hierarchy-is-complexity | H5 | C8 |
| 026 | three-axis-independence | H8 | C8 |
| 027 | cognitive-mode-principle | H17 | CD |
| 028 | variable-focalization-master | H9 | CD |
| 029 | perception-gap-delivery | H10 | CD |
| 030 | narrator-character-blend | H16 | CD |
| 031 | dt-knowledge-asymmetry | H3 | C8 |
| 032 | first-person-m4-effects | H4 | C8 |
| 033 | non-thematic-goal-categories | H11, H12 | CD |
| 034 | prose-craft-boundary | H6 | C8 |
| 035 | embedded-text-category | H7 | C8 |
| 036 | wi-terminal-ratio | H1, H2 | C8 |
| 037 | multi-story-focalization-profiles | H13 | CD |
| 038 | instinctive-mechanism-practice | H14 | CD |
| 039 | fim-reading-effect | H15 | CD |
| 040 | fabula-dialogue-replacement | H19 | CD |
| 041 | sufficient-stability-iterative | H20 | CD |
| 042 | editor-modes-evaluation | H21 | CD |
| 043 | note-design-relationships | H28 | CD |
| 044 | bespokeness-tension | T3 | K |
| 045 | keep-notes-provenance | E16 | P |
| 046–050 | — | not in the consolidation plan | M |

---

## 4. Known failure modes

Established by trace on 2026-09-12. A session should expect these and check for them; the list
is what was found in a sample, not a complete inventory.

**The 2026-08-30 reorganization is the lossy step — not the subagents.** Six assertions of
Brian's are absent from every hypothesis file, and all six died at the `D`-layer: a model
excluded by name on capability grounds; a question about a model's deprecation timing; a
domain qualifier that made a claim about where an advantage lives rather than when; a
characterization of a model's initiative; a causal claim about why v2's codebase was thin; and
one claim that was **inverted** — a cost asserted as a liability for non-coding and iterative
work, rendered downstream as a pure advantage. Every file inherits the inverted sign. A K-
trajectory session must read the Keep passage, not the `D`-entry, and must check polarity.

**Manufactured claim content.** At least one file carries a mechanism claim present in no
source at any layer, accumulated across the plan Summary and the file. Where a clause in the
existing `created` entry asserts something the sources do not, it is not carried.

**Compression at the plan layer was intended.** The plan's Summary column was specified by
Brian as "a one-sentence summary of the hypothesis statement", not as the statement. Statement
authoring was always downstream. A deviation between plan Summary and file is therefore not
by itself a fault; a deviation from the *sources* is.

**The subagents' access was bounded.** Creation agents for the `H`-series were given
`ANALYSIS-SYNTHESIS-PLAN.md` **lines 249–441** — the consolidated index — and not the document's
analysis bodies. `D`/`T`-series agents were given `pipeline-hypotheses-raw.md` and **never the
Keep dump**. Anything outside those ranges reached a file only through a per-file directive.

**Content classes present in the files that the schema does not admit.** Asserted relationships
to other live hypotheses (merge/split provenance excepted); prospective testing method; confirm-
or-refute conditions; testability self-assessment; a second hypothesis argued inside an origin;
retired vocabulary (`codebook`, `synthesis` as a raiser, the work matrix); source-document
labels as citations.

---

## 5. Procedure for one session

1. **Read this document, the schema, and §6.** Take the hypothesis's row from §3.
2. **Read the current file** — statement and `created` entry — as the thing under revision, not
   as the source.
3. **Walk the trajectory to its head**, in the order §3 gives, and read Brian's own words at the
   head. Stop there: the head is the authority, and each intermediate layer is a reading of it.
4. **Diff the file against the head.** Name (a) what the head asserts that the file lost or
   reversed, (b) what the file asserts that no layer supports, (c) which content classes from §4
   are present.
5. **Compose the new file whole** — never edit the existing one into shape — and write it to its
   own path under the armed checker. `StoryPlanner.DocIntegrity` runs on the write.
6. **Report**, in one short block: what was restored, what was cut, what was left alone, and any
   claim the session could not resolve against a source.
7. **Escalate rather than decide** where §6 says so.

### Reading Brian's turns in a transcript

Step 3 sends the session into the archive, where his words and a machine's sit in the same
record. The distinction decides what may be quoted and what may be inferred.

- A **freestanding `user` message** and a **`Typed:`** answer are his prose. Quotable as his
  words, and they carry the weight of anything else he wrote.
- A **`Chose:`** line is a **Claude-authored option label that he selected**. It is a faithful
  record of his decision and no record at all of his phrasing. Never quote it as his words, and
  **never infer from it what he meant** — the wording being reasoned over is the machine's.
- A **declined option** is evidence of what he refused, and of nothing else. It is not a
  paraphrase of his position.

This trap is not hypothetical, and it is easy to fall into precisely when the transcript looks
like a tidy summary of a view. This document's first draft, 2026-09-12, claimed the founding
sense of "hypothesis" was a deferral device — built entirely from two `Chose:` labels in the
2026-08-18 exchange, one of which offered "held as a hypothesis and deployed only after
post-vacation review" and which **Brian rejected**, choosing "Deploy now". A rejected machine
phrasing was read as his definition. He corrected it, and §2 now carries his own account.

When the only trace of a position is a `Chose:`, report it as a decision he took, never as a
view he holds. If what he meant matters and only a label records it, that is a question for him.

---

## 6. What the session writes

The shape is `hypothesis-file-schema.md` and is not restated here. What follows is what the
migration is permitted to do, which the schema does not say.

### § Hypothesis

The statement, one to three sentences, readable in isolation, a prediction and nothing else.
Six statements revert to their pre-iteration wording, recoverable from git commit `6d64c464`
(`d-2026-09-11-6`): 013, 029, 030, 033, 034, 038.

### § Origin

`date` is the day the hypothesis was captured. `reasoning` holds four things and nothing else:
the observation that prompted it; Brian's assertion, as the content; the motivation; and what
raised it. It may **not** hold: claim content absent from the statement and from the sources;
a corpus reading stated as established fact; a synthesis named as the raiser; prospective
testing method; an asserted relationship to another live hypothesis, merge/split provenance
excepted; confirm-or-refute conditions; testability self-assessment; retired vocabulary; or
anything derivable.

### § Record

Empty. Every record in the set is empty as of 2026-09-12 — no evidence entry, no baselined
entry — and the migration creates none.

### The standing constraint

**No claim enters that is not already in the file or in its sources.** Compression, restoration,
repair, removal and reordering are free. A new assertion is not.

### A statement that is not a testable prediction

Brian ruled on 2026-09-12, for the migration only: **a non-diagnosable hypothesis may be dropped,
or salvaged from its chain where that is logical.** The test is the referee's own: could a
referee, handed this statement and a finding, write a falsifier — what the finding would have
been were the statement false? If not, the statement is not a prediction.

Eight are known, from two causes that resolve differently:

- **A trailing question absorbed from the upstream entry** — 033 and 036. The `H`-entry states a
  prediction and then asks an open question, and the statement carried both in. This is the
  salvage case: separating them leaves a diagnosable prediction, and the question goes to the
  corpus's question list.
- **An instrument-scoped design prescription** — 004, 023, 027, 042, 043, and reverted 034.
  "Should be recognized by the planner", "may not be optimal", "may be needed" are claims about
  what the instrument ought to do, and no finding can discriminate one. Salvage only if the
  chain carries a prediction the prescription was built on; otherwise drop.

A dropped hypothesis is not written, and the session says so in its report; it does not become
an empty file, and the id is not reused. **This licence is the migration's alone.** Under
standard operating procedure nothing retires a hypothesis: a challenged one that never recovers
stands as a disproven hypothesis, and `minting-a-hypothesis` is the only gate.

### Open — do not decide in a migration session

- **How far statements may be repaired beyond drop-or-salvage.** Every record being empty, no
  wording is bound to any finding, so a reword now breaks nothing and costs nothing; after the
  first promotion it costs a re-verification batch. What is settled is the non-diagnosable case
  above; a statement that is a sound prediction but imprecisely worded is not, and is left alone.
- **The inverted claim.** Its correct polarity changes what two files claim. Brian's ruling.
