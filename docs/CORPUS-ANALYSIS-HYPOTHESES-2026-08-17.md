# Corpus Analysis Hypotheses — Halfway Point (2026-08-17)

Derived from a meta-analysis of the first 42 unique stories (51 output documents) produced
by the Fimfiction analysis pipeline. Brief versions covered: v2 (12 docs), v3 (39 docs).
The remaining ~50 stories in the Queue will be analyzed under v4.

Cross-references `ANALYSIS-PIPELINE-2026-08-17.md` for pipeline design decisions being tested.

## Confirmed Findings

These are not hypotheses — they held without exception across 42 stories and are treated as
established facts for design purposes.

**Mechanism 4 requires third-person limited with sustained FID.** Zero exceptions. Every story
that reaches Mechanism 4 traces it to sustained FID. Every story that identifies a Mechanism 4
ceiling traces it to the absence of sustained FID. The DT/FID distinction in the vocabulary is
genuinely load-bearing for prediction.

**First-person narration forecloses Mechanism 4.** Seven first-person stories all correctly
identify this as a structural property of the mode, not a quality gap. First-person narration
produces Mechanism-4-adjacent effects (retrospective unreliability, dual-POV replay, enacted
irony) that the framework classifies below Mechanism 4 but that readers experience as
functionally similar.

**FID and DT coexist freely.** The two are different rendering tools, not a progression from
amateur to advanced. Stories that use heavy DT alongside strategic FID ("Romance Reports,"
"Unexpected Confessions," "Third Time's A Charm") do sophisticated work that neither tool alone
achieves. The italics epiphany (conv 36 block 1245 — "I've been using italics to tell the reader
what the character is thinking instead of using sensory details and FID") was useful pedagogically
but hardened into an implied hierarchy the corpus does not support.

**Mechanism 2 (Latent / prior belief clash) is the framework's most precise analytical tool.**
The prior-disclosure-revision structure maps cleanly onto nearly every story's major reveals.
Multiple analyses call it "textbook," "the framework's strongest contribution," or "captures
with precision." The requirement to name the source of the prior (canon, genre trope, misdirection,
fanon) forces the analyst to identify exactly what the reader brings.

**Prior belief clash is disproportionately powerful in fanfiction.** The mechanism itself is
universal (defamiliarization, Shklovsky 1917), but in fanfiction the prior comes from an entire
pre-existing text the reader has internalized. The prior is thick, specific, and personally held.
The revision hits harder because the reader loved the prior version. This tracks Brian's own
creative origin (the Pokemon fanfic "Silver" and the Eve battling-prowess reveal that taught him
the mechanism instinctively — conv 47 block 1654).

**The v2→v3 Brief transition was adopted as a complete unit.** No partial adoptions observed.
Every v3 analysis includes all 11 amendments (Opening, Discipline, Character Voice Distinction,
Narrative Shortcuts, etc.). The cowork agent treats the Brief as authoritative.


## Hypotheses for Story Planner Evolution

Each hypothesis states the claim, the evidence from the first 42 stories, the proposed change
to the story planner or framework, and what the remaining corpus should confirm or refute.

### H1: WI-terminal links are legitimate

**Claim:** Plot-point×subject links can exist for investment-building, exposition, prior-belief
setup, or characterization enrichment without carrying a ThematicEvidence note. The audit
validation rule ("every link must have T" — conv 47 block 1520-1521) should be relaxed to
"every link must justify its existence: T, or a named structural purpose."

**Evidence:** Analysts in the corpus force-fit hollow thematic propositions onto moments that
are genuinely doing their work at the World Inference level. "Children express vulnerability
indirectly through behavior" is restated as a theme when it's actually a WI that establishes
Apple Bloom's emotional state for the later Latent clash about earth pony identity. The v4
Brief's Meaning/Theme refinement addresses this at the analysis level; H1 proposes the
corresponding change at the planner level.

**Proposed change:** The audit rule becomes: "A link without T is flagged as a question.
Valid answers: (1) add T, (2) this is setup for [scene X]'s T, (3) this is investment/
exposition/characterization — WI is the terminal destination, (4) this is set dressing — cut
the link and move to Delivery Blueprint."

**What remaining corpus should test:** Do the v4 analyses produce more WI-terminal instances
with named structural purposes? If so, what proportion of instances in a typical story are
WI-terminal vs T-terminal? This ratio would calibrate how many links in the planner should be
expected to have no T.

### H2: World Inference is a superset of Thematic Evidence

**Claim:** Not every WI terminates at T. Some WI is load-bearing infrastructure (setup,
characterization, exposition) that serves the argument structurally without depositing evidence
for a named proposition. The P → WI → T pipeline is a special case of the more general
P → WI → [T or structural purpose] pipeline.

**Evidence:** The corpus shows four categories of WI-terminal work: (1) prior-belief
construction for later Latent clashes, (2) investment-building (making the reader care, which
is necessary for later thematic moments to land), (3) exposition that enables comprehension,
(4) characterization without argument (making a character three-dimensional without arguing a
proposition). All four are load-bearing for the story's architecture without being thematic
evidence.

**Proposed change:** The track definitions' display questions for ThematicEvidence tracks could
acknowledge that an empty T with a populated WI is not always a gap — it may be a correct
reading of the link's structural function.

**What remaining corpus should test:** Does the v4 Meaning column produce a natural distribution
between thematic propositions and named structural purposes? What's the ratio?

### H3: DT-based knowledge asymmetry is a real technique adjacent to Mechanism 4

**Claim:** When a character states a false belief through marked Direct Thought, the reader
knows the DT is wrong but evaluates from outside (sympathetic observation). This is distinct
from Mechanism 4's sympathetic inhabitation through FID. Both create knowledge asymmetry;
the framework now acknowledges this in the v4 vocabulary but the planner does not need a
separate track for it.

**Evidence:** "Romance Reports" invents a split-voice DT technique (italic thought arguing
with unitalicized response). "Third Time's A Charm" has four analysis copies, two of which
disagree on whether the story reaches Mechanism 4 — the disagreement turns on whether specific
passages are FID or DT, confirming the boundary is genuinely hard to adjudicate. The planner's
track 99 (Character-Reader Perception Gap) correctly specifies FID in its Usage directive.
DT-based knowledge asymmetry is a prose-execution choice, not a planning-level track.

**Proposed change:** No change to the planner. Track 99 stays as-is. The v4 Brief's DT
acknowledgment handles the analytical side. The Writing Techniques track (113) is where
notes about DT deployment strategy belong.

**What remaining corpus should test:** Do the v4 analyses distinguish DT-based knowledge
asymmetry from FID-based Perception Gap more cleanly? Does the explicit acknowledgment
produce sharper Framework Fit sections?

### H4: First-person narration produces Mechanism-4-adjacent effects

**Claim:** Retrospective unreliability, dual-POV replay structure, and first-person enacted
irony create reader-character knowledge asymmetry through mechanisms other than FID. The
framework correctly classifies these below Mechanism 4, but multiple analyses note it
"undersells the reader's experience."

**Evidence:** Seven first-person stories all identify the same pattern. "Walk for Me" produces
Level-4-adjacent effects through enacted irony (the reader inhabits Fluttershy's wrong
self-assessment through first-person narration while accumulating contradicting evidence).
"the-sky-is-falling" and "The Gemmed Satyr" use dual-POV replay to produce dramatic irony
the framework classifies as Mechanism 1 but that functions like Mechanism 4.

**Proposed change:** No change to the planner — Brian is committed to third-person limited
for TLTT. The Framework Fit section in analyses will continue to note these effects as
adjacent rather than equivalent. No structural change needed.

**What remaining corpus should test:** Do the remaining first-person stories in the corpus
confirm the same pattern, or do any find a way to produce genuine Mechanism 4 in first person?

### H5: The mechanism hierarchy is structural complexity, not quality

**Claim:** Mechanism 3 can do more narrative work than Mechanism 4 in the same story. The
numbering describes the structural complexity of the reader's cognitive operation, not the
quality or effectiveness of the writing.

**Evidence:** "you-make-my-whole-life-worthwhile" explicitly notes that Development (Mechanism 3)
"does more narrative work than the Perception Gap." "Walk for Me" operates primarily at
Mechanism 1 and 3 and is among the most emotionally powerful analyses. "The Importance of Being
Earth Ponies" tops out at Mechanism 2 and the analysis correctly identifies that for a fable,
low inference is the correct technique.

**Proposed change:** Already enacted in v4 — "Levels" renamed to "Mechanisms." The planner's
track names do not use "Level" terminology, so no planner change is needed.

**What remaining corpus should test:** Does the "Mechanism" terminology produce analyses that
describe technique without implying quality? Does the clarifying note ("A story operating at
Mechanism 2 can be more powerful than one that briefly touches Mechanism 4") appear in analysts'
reasoning?

### H6: Comedy, atmosphere, and narrative voice are prose-craft, not framework

**Claim:** These are prose-level dimensions the mechanism × inference-stage matrix intentionally
does not measure. They belong in Writing Techniques subjects (track 113) as notes-to-self about
craft orientation, not in the fabula/syuzhet database.

**Evidence:** ~15 analyses flag comedy as a "blind spot." The framework captures what the
reader concludes but not what they feel (atmosphere) or why something is funny (comedy). These
are performative and affective dimensions of prose execution, not planning-level decisions
trackable in a SQLite database. Brian confirmed this: "Comedy comes from writing the prose.
I know how to do that in my own way."

**Proposed change:** Already enacted in v4 as a standing constraint. The planner gets no new
tracks. Writing Techniques subjects (track 113) hold prose-craft notes-to-self where needed.

**What remaining corpus should test:** Does the v4 standing constraint stop analyses from
flagging these as framework gaps? Do they instead name them correctly as "the prose-craft layer"?

### H7: Embedded texts and dreams sit outside mechanism categories

**Claim:** Letters, plans, and character-created artifacts sit between Mechanism 1 and 3
(on the page but revealing psychological change through composition). Dreams bypass the
inference ladder through subconscious processing. Both are relevant to TLTT as delivery
techniques but do not need planning-level tracks.

**Evidence:** Four or more analyses flag this pattern. "Romance Reports" catalogs letters
as interiority devices. "Third Time's A Charm" identifies dreams as foreshadowing via
subconscious processing. TLTT has specific instances: AJ/Twilight letters in Agency/Tempest/
Crash chapters, Reni/Minette correspondence during the Cartel in Skyfall/Swashbuckling,
Grover III/Celestia letters, AJ's dream callback to P&K's collaborator parallel.

**Proposed change:** No new tracks. These are delivery decisions that live in the Delivery
Blueprint (track 16) or Craft Notes (track 18) on the relevant plot points. The Writing
Techniques track (113) holds general notes about when to use embedded texts or dream sequences.

**What remaining corpus should test:** Do any remaining stories use embedded texts or dreams
in ways that challenge this classification?

### H8: The three-axis model is structurally independent but semantically coupled

**Claim:** The three axes of the analytical framework — Mechanism (Enacted/Latent/Development/
Perception Gap), Inference Stage (Page/World Inference/Meaning), and Rendering Mode (DT/FID/
psychonarration/behavioral proxy/dialogue) — are structurally independent (all combinations
are logically possible) but semantically coupled (the meaning of a value on one axis shifts
depending on what it's combined with). One specific constraint: Mechanism 4 functionally
requires FID on the Rendering Mode axis.

**Evidence:** The political axes insight from conv 47 block 1476-1477 and conv 76 block 2882:
"structurally orthogonal, semantically coupled." The same mathematical framework (linear
independence without orthogonality) applies. The corpus validates all four mechanisms operating
through multiple rendering modes, with Mechanism 4 as the sole constrained cell.

**Proposed change:** No planner change. This is an analytical insight about the framework's
structure. The rendering mode axis is entirely outside the track system (it's decided at
writing time). The mechanism and inference-stage axes are already captured in the track
architecture.

**What remaining corpus should test:** Do the v4 analyses produce instances that test the
coupling — e.g., the same mechanism producing different reader experiences through different
rendering modes?


## What the Remaining Corpus Should Test (Summary)

- Do the v4 Meaning/Theme column changes produce richer or vaguer per-instance analysis?
- Do longer stories (queued 130KB+ texts) show different mechanism distributions?
- Do P&K and Pax Chrysalia (if added post-vacation) show different patterns than the
  romance corpus — especially around Mechanism 2 with EaW-game-derived priors?
- Does the duplicate detection prevent redundant runs?
- Do any remaining stories challenge the confirmed findings?
- What is the natural ratio of WI-terminal to T-terminal instances in a v4 analysis?


## Open Questions Not Yet at Hypothesis Stage

**Narrative voice and genre-level operations.** Should these have any representation beyond
Writing Techniques subjects? "The Parent Trap" analysis noted the narrator's Wodehousian
voice "is itself a source of meaning independent of the inference ladder." Nine Tales of
Liberty and Green Is Your Color are potential evidence sources for Brian's natural narrative
voice. Currently quarantined to Writing Techniques — needs more deliberation on whether
the planner should represent narrative voice decisions at all, or if voice is purely a
prose-layer property like comedy.

**The semantically coupled three-axis model.** Does this need formalization in the planner, or
is it sufficient as an analytical insight documented here? The political axes formalization
(narrative property definitions with value definitions) was worth building into the planner
because the axes generate assignable positions for Civilizational System subjects. The
mechanism × inference-stage × rendering-mode space does not generate assignable positions for
anything in the planner — it describes the reader's cognitive experience, not a subject's
authored property.

**Long story methodology.** Where is the context-exhaustion threshold for Cowork? Stories above
~100KB may need local Claude Code analysis with 1M context. The exact threshold is an empirical
question that the remaining corpus may answer (several 130KB+ stories are queued). Stories that
were rejected by the Drive upload entirely need an alternative ingest path not yet designed.

**The pre-existing "every link must have T" rule.** H1 proposes relaxing this, but the
relaxation has not been tested in practice. The first test will be Brian's own experience:
when populating link tracks in the planner, does forcing T onto every link produce useful
discipline or hollow propositions? The corpus evidence (analysts force-fitting themes) is
suggestive but not conclusive, because analyzing someone else's finished story is a different
cognitive activity from planning your own.
