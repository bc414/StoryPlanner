# Pipeline Hypotheses — Organized, Uniterated (2026-08-30)

Source: `source_material_references/hypotheses-google-keep-dump.md` (Google Keep notes,
pasted in chronological order of thought). Organized here from most meta to most
granular. Later thoughts supersede earlier ones where noted; unresolved tensions are
surfaced as their own hypotheses.

These hypotheses are about the **operating pipeline** (model, data, instructions,
harness) — a different layer from the narrative design framework hypotheses (H1-H28
in `docs/ANALYSIS-SYNTHESIS-PLAN.md`), which are about craft technique and what the
planner tracks. Cross-references to H1-H28 noted where they connect.

**Status:** Organized only. None of these have been iterated on, tested, or accepted.

---

## Tier 1: The decomposition framework itself

**D1: Four independent factors.** The AI-assisted planning experience decomposes
into four independently varying factors: **model** (Gemini 2.5 Pro, Gemini 3.1 Pro,
Claude Sonnet 4.6, Opus 4.6, Opus 5, …), **data stream** (none, whole-plan paste,
MCP sidecar queries), **instructions** (platform default, custom gem, AI Studio
system prompt, Claude web default, project + skills), and **harness** (Gemini web
chat, AI Studio, Claude web chat, Claude Desktop, Claude Code). These were
historically confounded — each era changed multiple factors at once — but v3 tooling
may decouple them.

"Independent, not orthogonal" — correlated (a more capable model benefits more from
better instructions) but varying on separate axes. Whether they are truly independent
or merely less coupled is itself a testable claim.

**D2: Version labels are bookmarks, not paradigms.** v0 through v3 are point-in-time
configurations of the four factors, like release tags on a git tree. Each version's
character is the compound effect of its specific (model, data, instructions, harness)
tuple. The underlying factor changes should be dated individually using lineage and
conversation history, not attributed to version boundaries.

The synthesis plan's version history (v0-v3) describes eras as narrative + codebase
shifts. D2 proposes that the eras are also decomposable along the four pipeline
factors, and that some attributed version-level shifts were actually factor-level
shifts.

**D3: Two separable tasks.** Designing the v3 **narrative design framework** (what
the planner tracks — the subject of H1-H28 and WU1-WU6) is related but separate
from designing the v3 **operating pipeline** (how model/data/instructions/harness
compose for the work). These hypotheses are almost entirely about the second task.

Open question: Is this its own work unit? What ordering? Any dependency with the
existing WUs, upstream or downstream?

---

## Tier 2: Factor independence and testability

**D4: V3 tooling actually decouples the factors.** MCP siloing decouples data from
instructions (queries, not paste). Skills decouple instructions from model (the skill
enforces rigor regardless of which model runs it). MCP as open protocol decouples
harness from model (theoretically). Whether this independence is real or aspirational
is testable by varying one factor while holding others constant.

**D5: Testing methodology is unresolved.** To test factor independence, do you run
the same task under different configurations? The full analysis pipeline twice? Who
verifies — cross-verification between configurations, or Brian's subjective judgment?
What constitutes a positive signal that one configuration outperforms another?

**D6: Each era's experience should be retroactively decomposed.** The historical
record (lineage, conversations) contains evidence of how each factor contributed to
each era's outcomes. The decomposition is not theoretical — it can be grounded against
what actually happened, dated, and attributed to specific factor changes.

---

## Tier 3: About models

**D7: Model-intrinsic properties exist after you subtract instructions and data.**
When you decouple the instructional scaffolding (now in skills) and data connectivity
(now in MCP), what remains model-intrinsic? Hypothesized properties: tool-use
affinity, instruction-following fidelity, baseline analytical rigor, voice
warmth/register, capability ceiling.

**D8: The Opus 4.6 vs Opus 5 split.** Opus 4.6 has better literary voice and
conversational warmth. Opus 5 has better agentic tool use and initiative (knows when
to go beyond the box — Anthropic's "agentic" marketing reflects RLHF'd tool use).

This gap may narrow once instructional scaffolding is mature. Skills close 4.6's
tool-use gap; voice instructions close 5's register gap. The question is which gap is
**cheaper to close with instructions**, and whether the answer changes as the
apparatus matures. Once everything is well-defined, 5's initiative advantage may be
less valuable (the instructions tell it what to do), and 4.6's voice advantage may
persist (harder to instruct into existence).

Further: "Does voice matter anymore if AI voice is sidecar-siloed?" If the model's
analytical output never enters the .storyplan (v3 architecture), the prose register
matters less for the data product but still matters for the real-time interaction
experience during sessions.

**D9: Constitutional AI as a constraint on model space.** Attention-based RLHF and
commercial incentives (OpenAI, Gemini) conflict with the analytical rigor this work
needs. Constitutional AI (Claude family) aligns better. This constrains the practical
model space to Claude family members, with MCP as the escape valve if a non-Claude
model ever becomes worth it.

**D10: Fable's role.** Two competing sub-hypotheses: (a) Fable is designed for
open-ended/undefined work and is valuable when definitions are lacking — less so once
the apparatus is well-defined. (b) Fable could be a better model regardless of how
well-defined the apparatus is — marketed capability does not equal actual capability
ceiling. No evidence yet either way.

**D11: Sonnet 4.6 as preferred for narrative analysis (historical claim to test).**
Brian preferred Sonnet 4.6 because it worked better without a system prompt and its
voice was "helpful and accommodating." But Brian still had to ask for grounding. Now
that grounding is in skills, was the preference actually for Sonnet's default
behavior — and would a properly instructed Opus match or exceed it?

---

## Tier 4: About instructions

**D12: Evidence-based instruction design, not top-down.** System prompts and skills
should be designed from evidence of what worked in 9 months of conversation history,
not from first-principles theorizing. The conversation corpus, lineage, and code
sessions contain the prompting patterns that produced accepted or rejected outputs.

Supersedes an implied earlier approach: designing skills from first principles about
what good analytical behavior should look like.

**D13: Copy-pasted text is an acceptance signal.** When Brian copy-pasted AI output
into the plan, that was a positive signal about the output quality — analogous to
RLHF reward. The prompting context that produced copy-paste-worthy output is evidence
for instruction design.

Extension: What other acceptance signals exist beyond copy-paste? Read states in the
Conversation Reader (done, flagged, skipped) are one. Brian's corrections in user
turns are negative signal. The question is whether these signals are systematically
mineable — and whether industrial RLHF/fine-tuning/post-training techniques offer
applicable methodology, or whether the scale is too small and manual review is the
right approach.

**D14: The paradigm shift on out-of-box behavior.** Previously believed Claude was
good because its default behavior was sufficient without a system prompt (contrasted
with Gemini, which needed the custom gem). Evidence from the full timeline may show:
(a) Constitutional AI gives Claude a higher floor. (b) But the floor is not the
ceiling — both Gemini-with-system-prompt and Claude-with-skills outperform their
respective defaults. (c) AI Studio system prompting "felt futile but wasn't a dead
end" — it was a weaker version of the same skill-based approach. (d) The real
paradigm shift is not model choice but instructional scaffolding: from "find a model
that works out of the box" to "build the instructions that make any adequate model
work."

Connects to existing H25/H26 (four independent factors improving outcomes). D14
refines H25's factor (b) — system prompt quality — by tracing the full historical arc
from custom gem → AI Studio → absent (v2) → CLAUDE.md + skills (v3).

---

## Tier 5: About data streams and harnesses

**D15: MCP enables retrospective review.** The MCP server is the first architecture
that lets the AI look backward at its own prior interactions with the plan data.
Before MCP, only the most recent paste existed. This enables the evidence-based
instruction design (D12) and the copy-paste detection (D19).

**D16: Data source unification.** The various corpora (conversations, lineage, code
sessions, source texts) each have bespoke schemas built at different times under
different constraints. Unifying them under a common API standard (conversations API
or similar) would make them harness-agnostic and model-agnostic, easing future
portability.

Open question: Is the conversations API the right standard, or does that force a
shape onto data that isn't conversational (source texts, lineage doc diffs)?

**D17: Desktop vs Code split.** Original reasoning was role separation (Desktop
analyzes story, Code builds planner). But Code can "do way more" — it has skills,
MCP, agentic workflows. Are skills and connectors actually harness-agnostic now?
Should the split be reconsidered, or does the role separation still justify the
architectural boundary?

**D18: The actual usage loop is undefined.** What IS the target workflow pattern for
using the model + harness + data, at a high level, independent of specific tooling?
Historical patterns should be identified across eras and decoupled from their era's
technology. The target hasn't been defined — it's been implicitly whatever the current
tooling afforded.

---

## Tier 6: Implementation-level

**D19: Copy-paste detection DataOp.** A concrete operation: go through each v2 note,
grep against lineage and conversation corpora, flag notes (or spans within notes) as
AI-originated. Store as a state plus start/end character indexes so it surfaces in
the planner UI.

Connects to the downstream voice linting protocol in the synthesis plan. D19 is the
specific mechanism; the plan's "linting protocol" is the broader intent.

**D20: UI surfacing of AI voice.** Visual treatment (gradient color, different
styling) to make AI-originated text visually distinct in the planner. Makes the
contamination visible rather than requiring Brian to mentally track it.

Open question: "Need to deliberate on how to apply the filter properly." The
filtering logic (exact match? fuzzy? threshold?) needs design.

---

## Tensions that are their own hypotheses

**T1: Voice matters vs voice is moot.** D8 says Opus 4.6 has better voice. But if
AI voice is sidecar-siloed (never enters .storyplan), does the real-time session
voice matter enough to choose a model for it? Tension between the interaction
experience (voice matters during the session) and the data product (voice doesn't
enter it).

**T2: Instructions close all gaps vs model-intrinsic properties are irreducible.**
D8 says skills can close tool-use gaps and voice gaps. But if that were fully true,
model choice wouldn't matter at all. The tension: are there properties no instruction
can teach (capability ceiling, reasoning quality), or is the ceiling set by
instructions rather than model?

**T3: V1 bespokeness vs V2 cleanliness vs V3 capability.** V1 was bespoke but
vibe-coded with tech debt. V2 was clean but thin (weak tooling). V3 is a capability
leap. The tension: V1's bespokeness produced something the clean V2 didn't. Does V3's
capability restore bespokeness at V2's quality level, or does it introduce its own
version of the v1 tech-debt trap?

---

## Cross-references to existing hypothesis set (H1-H28)

| Pipeline hypothesis | Existing hypothesis | Relationship |
|---|---|---|
| D1 (four factors) | H25/H26 (grounding factors) | Same decomposition at higher abstraction. H25/H26 is about analytical outcomes; D1 is about the full pipeline. |
| D14 (paradigm shift on out-of-box) | H25 factor (b) | D14 refines the system-prompt factor with the full historical arc. |
| D19/D20 (copy-paste detection, UI) | H22/H23/H24 (voice separation) | Implementation-level hypotheses about the same concern. |
| D13 (acceptance signals) | H27 (five voice registers) | Which voices get copy-pasted is evidence about voice quality as perceived by Brian. |
| D19 (detection DataOp) | Downstream linting protocol | D19 is a specific proposed mechanism for the plan's linting intent. |

**Not yet captured in H1-H28:** D1-D6 (the decomposition framework), D12-D14
(evidence-based instruction design), D17-D18 (harness choice and the undefined usage
loop).
