# Consolidation 1 (2026-08-31)

## Trigger

Initial population. Three source documents contained hypotheses in incompatible
formats with overlapping, ungrouped, and unnumbered claims:

- `docs/ANALYSIS-SYNTHESIS-PLAN.md` — H1-H28 (narrative design framework)
- `docs/v3-framework/pipeline-hypotheses-raw.md` — D1-D20, T1-T3 (pipeline/apparatus)
- Emergent hypotheses E1-E14 from the 2026-08-31 design conversation that
  produced the v3-buildout skill

The consolidation extracted these into the skill's hypothesis file format with
stable IDs, conceptual-hierarchy ordering, and a single provenance chain.

## What was done

**Design:** `consolidation-1-plan.md` proposed 44 hypotheses with merge/split
decisions, tier ordering, and exclusion rationale. During review, one hypothesis
was added (045 keep-notes-provenance), four hypothesis statements were tightened
for testability (013, 017, 024, 044), and the NLM vocabulary provenance claim
was resolved as a verification target for existing hypotheses rather than a new
one. Final count: 45.

**Execution:** 45 hypothesis files created in `docs/v3-framework/hypotheses/`,
each with frontmatter, a hypothesis statement, and a founding `- created` record
entry. Written in two phases: 001-007 by the main session (format testing), then
008-045 by seven parallel agents (one per tier batch).

**Review:** Three review agents checked all 38 agent-written files. Eight files
had issues — source document labels cited in records (020, 021, 034, 038, 039,
040), a testing-methodology sentence in a hypothesis section (021), a dropped
qualifier (008), and quoted confirm language (025). All fixed.

**Other changes in this session:**
- CLAUDE.md: FEATURE-AUDIT line softened from "lists" to "records...at the time
  of writing — its assertions are testable against current evidence, not settled"
- VERSION-HISTORY.md: rewritten to facts only (interpretive claims extracted to
  consolidation-1-plan.md's interpretive claims table; original preserved as
  VERSION-HISTORY-DRAFT1.md)
- Memory file `v3-framework-plan.md`: updated to reflect that the synthesis plan
  is historical provenance, not the first forward plan

## Merges

Nine merges took multiple source items into single hypotheses:

| ID | Merged | Rationale |
|----|--------|-----------|
| 002 | E4 + E9 + E5 | Epistemic method provenance — three angles on one claim |
| 003 | E1 + E2 + E3 | Epistemic vocabulary for content — one question (what "evidenced" means per layer) |
| 004 | E6 + E18 | Working cadence — same observation from two sources |
| 006 | D1 + H25 + H26 | Four-factor decomposition — same model at different granularity |
| 007 | D2 + D6 | Version labels — same claim plus its testing method |
| 009 | D4 + D15 | V3 tooling — both consequences of MCP/skills architecture |
| 011 | D8 + D11 + T1 + T2 | Model convergence — four aspects of one question |
| 014 | D12 + D14 + E14 + Synth.§Downstream | Instruction design — one claim about the paradigm shift |
| 019 | H22 + H23 + H24 + Synth.§AIContradiction + Synth.§V2Stall | AI context contradiction — the feedback loop diagnosis |

## Splits

| Original | Split into | Rationale |
|----------|-----------|-----------|
| H18 | 023 + 024 | Three-concern existence vs structural relationship — separable predictions |
| H22/H23/H24 | 019 + 020 | Historical diagnosis vs forward prerequisite — independently refutable |

## Exclusions

| Source | Reason |
|--------|--------|
| D5 | Task item (testing methodology design), not a prediction |
| D19, D20 | Implementation proposals downstream of 019/020; FEATURE-AUDIT territory |
| E7 | Methodological policy already in the skill, not a testable prediction |
| E13 | Evidence for 010/012, not its own hypothesis |
| E15 | Question without testable prediction |
| E17 | Resolved in conversation (notes are snapshots, not revision histories) |
| Synth.§HallmarkWall | Interpretive observation; context for 044's created entry |
| NLM vocabulary provenance | Too narrow; verification target for retrospective WU, evidence deposited into 002/028 |

## Interpretive claims extracted from VERSION-HISTORY.md

The original VERSION-HISTORY.md mixed facts with interpretive claims. Nine
claims were extracted and mapped to hypotheses or flagged for WU verification.
Full table in consolidation-1-plan.md § "Interpretive claims extracted from
VERSION-HISTORY.md."

## Current landscape

45 hypotheses, all `untested`, none baselined. Ten tiers from meta to specific:

| Tier | Range | Count | Domain |
|------|-------|-------|--------|
| A | 001-005 | 5 | Purpose and epistemology |
| B | 006-009 | 4 | Pipeline decomposition |
| C | 010-013 | 4 | Model properties |
| D | 014-018 | 5 | Instructions and environment |
| E | 019-022 | 4 | Voice and interaction |
| F | 023-027 | 5 | Framework architecture |
| G | 028-032 | 5 | Perspective and perception |
| H | 033-037 | 5 | Goals, scope, and boundaries |
| I | 038-040 | 3 | Brian's practice |
| J | 041-045 | 5 | Planner instrument |

Pipeline hypotheses (Tiers B-D, 13 hypotheses) are uniterated from a Google
Keep dump and may consolidate further as pipeline work gets scoped.

No forward plan exists yet. Forward-plan-1.md follows this consolidation.
