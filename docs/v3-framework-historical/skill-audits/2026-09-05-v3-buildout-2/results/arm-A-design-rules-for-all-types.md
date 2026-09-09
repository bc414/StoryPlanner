## unit-105
- section: ## Design rules for all types
- quote: **Scale is a cell, not a token count.**…
- counterpart: preparing-to-explore-a-corpus.md › explore-plan
- relation: narrowed
- note: B keeps scale deciding whole-corpus-vs-slices and slice readers sharing a protocol (never one reader with compaction is implicit in pathfind/slice-run split), but drops the "matrix cell" framing that ties scale to model and verification together as one decision.

## unit-106
- section: ## Design rules for all types
- quote: **Arms and blinding.**…
- counterpart: exploring-a-corpus.md › join-and-bin
- relation: restated
- note: —

## unit-107
- section: ## Design rules for all types
- quote: **Binning.**…
- counterpart: exploring-a-corpus.md › join-and-bin
- relation: restated
- note: The drilling and adjudication half of this unit is carried by reviewing-leads.md › review-leads, cited here as the other location.

## unit-108
- section: ## Design rules for all types
- quote: **One factor at a time.**…
- counterpart: artifacts.md › Arm key
- relation: narrowed
- note: B keeps "one factor varies across arms, the rest is identical," but does not restate that a mixed design must declare factorial intent or else is uninterpretable.

## unit-109
- section: ## Design rules for all types
- quote: **Codebooks are not written by the pass that applies them.**…
- counterpart: SKILL.md › Constitutional rules
- relation: narrowed
- note: B keeps authoring-with-Brian, calibration against his blind verdicts before any batch, and stopping when a round finds the codebook wanting, but routes the finding through round.md § Corrections and Brian's promotion session rather than a named "spec-pool entry," which does not appear anywhere in B.

## unit-110
- section: ## Design rules for all types
- quote: **Every autonomous job is a runner job.**…
- counterpart: agent-runner/SKILL.md › Agent runner
- relation: narrowed
- note: B restates the runner-job requirement, job-file schema, per-job mcp:true opt-in, and the Agent-tool/Workflow-tool restrictions, but run folders are now named fanout/<instance>/<run>/ rather than fanout/WU<n>.<m>-<slug>/, and B says fanout/PROTOCOL.md retired 2026-09-05 with the host's /protocol route still an owed runner change, so the lifecycle order now lives in the v3-buildout skill's process tables rendered into map.md rather than in PROTOCOL.md as served at /protocol.
