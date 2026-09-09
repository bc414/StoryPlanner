# Units of SKILL.md

Source sha256 e653707eb49408a91bd2c61587750acf0399e84e114662ea789d04ba68efd27b; 174 units.

| Unit | Section | First line |
|---|---|---|
| unit-001 | (frontmatter) | --- |
| unit-002 | # V3 Framework Buildout | This skill governs the full v3 framework buildout: managing hypotheses, designing |
| unit-003 | ## Epistemic framework (applied) | CLAUDE.md establishes the principles. This section applies them to the buildout. |
| unit-004 | ## Epistemic framework (applied) | **Scope:** Both the **narrative design framework** (track architecture, cognitive |
| unit-005 | ## Epistemic framework (applied) | **The current analytical framework vocabulary is provisional.** The mechanism × |
| unit-006 | ## Epistemic framework (applied) | **Three epistemic states for hypotheses:** |
| unit-007 | ## Epistemic framework (applied) | `untested` — captured, no evidence examined. |
| unit-008 | ## Epistemic framework (applied) | `evidenced` — evidence gathered and currently supporting. Thin or thick — the |
| unit-009 | ## Epistemic framework (applied) | `challenged` — unresolved counterevidence exists. One unresolved challenging |
| unit-010 | ## Epistemic framework (applied) | Transitions are reversible: evidenced → challenged (counterevidence deposited), |
| unit-011 | ## Epistemic framework (applied) | **Baselining is progress tracking, not epistemology.** When Brian reviews a |
| unit-012 | ## Epistemic framework (applied) | Make the hypothesis stronger or more weighty than a non-baselined hypothesis |
| unit-013 | ## Epistemic framework (applied) | Make the hypothesis less challengeable |
| unit-014 | ## Epistemic framework (applied) | Add epistemic weight — evidence supersedes baselining rationale |
| unit-015 | ## Epistemic framework (applied) | Constitute endorsement of truth |
| unit-016 | ## Epistemic framework (applied) | See "Status transitions — who can do what" below for the baseline revocation |
| unit-017 | ## Epistemic framework (applied) | **Evidence alignment is relative to the current hypothesis.** Each evidence entry |
| unit-018 | ## Epistemic framework (applied) | `contextual` is a **transitional** tag with exactly one use: an entry deposited |
| unit-019 | ## Epistemic framework (applied) | **Recall is atmosphere, evidence is grounding.** When anyone states something |
| unit-020 | ## Epistemic framework (applied) | Query the relevant evidence source for the specific claim |
| unit-021 | ## Epistemic framework (applied) | Compare what the evidence actually says against the recall |
| unit-022 | ## Epistemic framework (applied) | Present any discrepancies — the grounded version may confirm, refine, or |
| unit-023 | ## Epistemic framework (applied) | Proceed with the grounded version after Brian reviews |
| unit-024 | ## Epistemic framework (applied) | Nothing is exempt. |
| unit-025 | ## Epistemic framework (applied) | **Grounding connectors:** MCP server (working plan, v1 archive, conversations, |
| unit-026 | ## Epistemic framework (applied) | **Status transitions — who can do what:** |
| unit-027 | ## Epistemic framework (applied) | **Agent can transition:** `untested` → `evidenced` (when supporting evidence is |
| unit-028 | ## Epistemic framework (applied) | **Only Brian can baseline.** The agent can surface candidates ("this hypothesis |
| unit-029 | ## Epistemic framework (applied) | **Automatic on challenge of baselined hypothesis:** `baselined` resets to |
| unit-030 | ## Session routing | **Single-session default.** All buildout activities — planning, exploration, |
| unit-031 | ## Session routing | **Before starting work, determine your activity and read accordingly:** |
| unit-032 | ## Session routing | **Hypothesis iteration** (updating a hypothesis, depositing evidence, refining |
| unit-033 | ## Session routing | **WU execution** (running an experiment): Follow the four-phase protocol in |
| unit-034 | ## Session routing | **Consolidation** (restructuring the hypothesis set): Read the full hypothesis |
| unit-035 | ## Session routing | **Forward plan creation** (writing a new experimental agenda): Read the full |
| unit-036 | ## Session routing | **Post-WU review** (Brian reviews a WU's findings): Follows WU execution in |
| unit-037 | ## Session routing | *Challenge mode*: Brian questions a finding. Verify against source evidence — |
| unit-038 | ## Session routing | *Enrichment mode*: Brian connects a finding to his practice or recall. Add |
| unit-039 | ## Session routing | **Story-content boundary**: thematic content comparisons ("my stories also |
| unit-040 | ## Session routing | **Hypothesis statement updates are batched at the end of the review**, not |
| unit-041 | ## Session routing | **Standard outputs**: corrected synthesis report (challenge mode), enriched |
| unit-042 | ## Session routing | **Ad hoc conversation** (Brian asks about the framework, discusses ideas): Read |
| unit-043 | ### File format | Hypothesis files live in `docs/v3-framework/hypotheses/`. Each file is named |
| unit-044 | ### File format | **Frontmatter fields:** |
| unit-045 | ### File format | `id` — stable integer, never reused after supersession. Unique across the |
| unit-046 | ### File format | `status` — one of `untested`, `evidenced`, `challenged`. |
| unit-047 | ### File format | `baselined` — `false` or an ISO date (e.g. `2026-09-05`). The date records |
| unit-048 | ### File format | `created` — ISO date. When the hypothesis was first captured. Stable. |
| unit-049 | ### File format | **Verbosity guardrails:** |
| unit-050 | ### File format | The hypothesis statement must be readable in isolation as a testable prediction. |
| unit-051 | ### File format | **Statement:** What this predicts. 1-3 sentences. No provenance, no |
| unit-052 | ### File format | **Record entries:** Compressed but as long as the finding requires. Iteration |
| unit-053 | ### Record conventions | The record is a single chronological list, **oldest first, newest appended to |
| unit-054 | ### Record conventions | **Created entries** are always the first entry. They explain why the hypothesis |
| unit-055 | ### Record conventions | **Evidence entries** have a source in parentheses and alignment in brackets: |
| unit-056 | ### Record conventions | The trailing "Would differ if false:" clause is required on every evidence |
| unit-057 | ### Record conventions | Evidence must be grounded in corpora or verifiable sources — not Brian's recall |
| unit-058 | ### Record conventions | **Iteration entries** describe a hypothesis text change: |
| unit-059 | ### Record conventions | **Baseline entries** record Brian's review judgment with rationale: |
| unit-060 | ### Record conventions | **Alignment editing:** When a hypothesis is rewritten (iteration entry), re- |
| unit-061 | ### Record conventions | **Grep patterns:** `^- created` finds all founding entries. `^- evidence` finds |
| unit-062 | ### Record conventions | **What does NOT go in hypothesis records:** |
| unit-063 | ### Record conventions | **Brian's recall about his own practice.** "I think I do this instinctively" |
| unit-064 | ### Record conventions | **Brian's design observations.** "This connects to my story in way Y" is |
| unit-065 | ### Record conventions | **Observations that don't change the statement.** If an insight informs a |
| unit-066 | ### Record conventions | **Methodological pointers.** "WU1.3 should check this" is a WU spec note, |
| unit-067 | ### Record conventions | The test: if removing the entry would leave the hypothesis record incomplete |
| unit-068 | ### Deposit protocol | A WU's analyses run discovery-first with no hypothesis in view; its deposits |
| unit-069 | ### Deposit protocol | **First write — the WU artifact.** It is organized by what was observed — |
| unit-070 | ### Deposit protocol | **Second write — the hypothesis records.** With the artifact finished, for |
| unit-071 | ### Deposit protocol | Locate the relevant findings in the artifact. |
| unit-072 | ### Deposit protocol | Write the "Would differ if false:" clause. If it cannot be written, the |
| unit-073 | ### Deposit protocol | If the statement names specific items, address every named item. Any |
| unit-074 | ### Deposit protocol | If the entry rests on a classification made by an intermediate analysis |
| unit-075 | ### Deposit protocol | A recall-derived testing spec ("Brian's recall: X — does the evidence confirm |
| unit-076 | ### Ceremony scaling | Ceremony is proportional to the magnitude of the change: |
| unit-077 | ### Ceremony scaling | **Minor** (wording tightened, no conceptual change): Update hypothesis statement. |
| unit-078 | ### Ceremony scaling | **Significant** (scope changed, evidence prompted rethink): Full iteration entry |
| unit-079 | ### Ceremony scaling | **Structural** (split, merge, supersede): Create new file(s). Add a final |
| unit-080 | ### How to challenge a hypothesis | Cite specific evidence that conflicts with the hypothesis. Reference note IDs, |
| unit-081 | ### How to challenge a hypothesis | A challenge is an evidence entry with `[challenging]` alignment, deposited in |
| unit-082 | ### How to challenge a hypothesis | Brian may also challenge hypotheses — his challenges carry the weight of his |
| unit-083 | ### New hypothesis creation | The same protocol applies whether creating one hypothesis or forty during a |
| unit-084 | ### New hypothesis creation | **Detection criteria for emergent hypotheses:** |
| unit-085 | ### New hypothesis creation | **Novelty:** Does this fit as evidence for an existing hypothesis? If yes, |
| unit-086 | ### New hypothesis creation | **Testability:** Does it make a prediction that evidence could confirm or |
| unit-087 | ### New hypothesis creation | **Independence:** Is this genuinely separate from existing hypotheses, or a |
| unit-088 | ### New hypothesis creation | **Source rules:** |
| unit-089 | ### New hypothesis creation | Brian's explicit statements always get the offer to create a hypothesis file. |
| unit-090 | ### New hypothesis creation | Claude's analysis can surface potential hypotheses only when all three criteria |
| unit-091 | ### New hypothesis creation | When Claude proposes, the proposal must reference Brian's original statement or |
| unit-092 | ### New hypothesis creation | **Timing during WU execution:** During primary WU work, note unexpected findings |
| unit-093 | ### New hypothesis creation | **V1 trap prevention:** The risk is Claude proposing a hypothesis → Brian |
| unit-094 | ### Connections between hypotheses | No formal cross-references in frontmatter or metadata. Connections emerge from |
| unit-095 | ### Connections between hypotheses | Consolidation sessions are the maintenance mechanism for the hypothesis graph: |
| unit-096 | ### Hypothesis index | The index lives at `docs/v3-framework/hypotheses/INDEX.md`. It is a routing |
| unit-097 | ### Hypothesis index | Four columns. ID for grep. Slug as clickable link. Status and baselined from |
| unit-098 | ### Forward plan lifecycle | A forward plan is a **snapshot experimental agenda** — born from the current |
| unit-099 | ### Forward plan lifecycle | **Numbering:** Forward plans are sequentially numbered: `forward-plan-1.md`, |
| unit-100 | ### Forward plan lifecycle | **Designing the experimental agenda:** |
| unit-101 | ### Forward plan lifecycle | The forward plan is a best-effort agenda for the full hypothesis set — not a |
| unit-102 | ### Forward plan lifecycle | Start from the hypothesis landscape, not from prior plans or synthesis |
| unit-103 | ### Forward plan lifecycle | Group candidate experiments by evidence source. Multiple hypotheses testable |
| unit-104 | ### Forward plan lifecycle | WUs with unmet preconditions (blocked on Brian, needs a skill, needs prior WU |
| unit-105 | ### Forward plan lifecycle | Prior plans and synthesis documents are reference for understanding what was |
| unit-106 | ### Forward plan lifecycle | **Ordering criteria** (in priority order — hard constraints first, then |
| unit-107 | ### Forward plan lifecycle | **Evidence dependency chains.** WU X needs WU Y's findings as input. A |
| unit-108 | ### Forward plan lifecycle | **Enrichment flow.** A WU whose post-review findings would add testing |
| unit-109 | ### Forward plan lifecycle | **Precondition blockers.** The experiment needs a skill, a data export, or |
| unit-110 | ### Forward plan lifecycle | **Infrastructure hypotheses first.** Some hypotheses predict properties of |
| unit-111 | ### Forward plan lifecycle | **Unblocking value.** An experiment that produces evidence for many |
| unit-112 | ### Forward plan lifecycle | **Foundation before application.** Testing foundational hypotheses (the |
| unit-113 | ### Forward plan lifecycle | **Contents:** |
| unit-114 | ### Forward plan lifecycle | **WU numbering:** The major number is the forward plan number. The minor number |
| unit-115 | ### Forward plan lifecycle | **Hypothesis references live HERE, not in hypothesis files.** The forward plan's |
| unit-116 | ### Forward plan lifecycle | **Ordering:** WU specs are listed in numeric id order — the plan is a catalog, |
| unit-117 | ### Forward plan lifecycle | **WU status values:** `proposed` → `scoped` (plan mode has designed the |
| unit-118 | ### Forward plan lifecycle | **Plan creation triggers:** |
| unit-119 | ### Forward plan lifecycle | **After consolidation (mandatory):** The hypothesis landscape changed — IDs |
| unit-120 | ### Forward plan lifecycle | **After priority reassessment (optional, lighter):** WU findings changed |
| unit-121 | ### Forward plan lifecycle | **Forward plan retirement:** When a consolidation happens or priorities shift |
| unit-122 | ### Forward plan lifecycle | The retired plan is provenance — historical reference for understanding what was |
| unit-123 | ### Forward plan lifecycle | **The forward plan is expected to be a long document.** It carries the full |
| unit-124 | ### Forward plan lifecycle | **How the forward plan treats hypotheses by status:** |
| unit-125 | ### Forward plan lifecycle | When designing a forward plan, hypotheses in different states call for different |
| unit-126 | ### Forward plan lifecycle | `challenged` — highest priority. Unresolved tensions block downstream work. |
| unit-127 | ### Forward plan lifecycle | `untested` — largest knowledge gaps. The experiment is exploratory: what does |
| unit-128 | ### Forward plan lifecycle | `evidenced` — may need stress-testing or replication from a different angle. |
| unit-129 | ### Forward plan lifecycle | `baselined` — Brian has reviewed and is acting on these. Unless new challenges |
| unit-130 | ### Forward plan lifecycle | This priority ordering is one input to experiment design. The ordering criteria |
| unit-131 | ### WU design and execution | **Every WU runs four phases in the same session:** scope reconciliation, |
| unit-132 | ### WU design and execution | **Scope reconciliation phase** (auto mode, before plan mode): |
| unit-133 | ### WU design and execution | Read the active forward plan to find the WU spec and its target hypothesis |
| unit-134 | ### WU design and execution | **Plan-mode phase:** |
| unit-135 | ### WU design and execution | **Read.** Read the evidence sources (or assess their size — if they exceed |
| unit-136 | ### WU design and execution | **Identify all open questions.** What is ambiguous in the WU spec? What |
| unit-137 | ### WU design and execution | **Ask all open questions.** Use AskUserQuestion, batching at the 4-question |
| unit-138 | ### WU design and execution | **Write the plan.** With all questions answered, write the execution plan to |
| unit-139 | ### WU design and execution | **Exit plan mode.** Brian reviews and approves. The session continues into |
| unit-140 | ### WU design and execution | **Execution phase:** |
| unit-141 | ### WU design and execution | **Primary work:** Execute the experiment focused on target hypotheses. Deposit |
| unit-142 | ### WU design and execution | **Wrap-up step:** After primary work, read the full hypothesis index. Check |
| unit-143 | ### WU design and execution | Then report the tag counts to Brian: targets, entries written, supporting, |
| unit-144 | ### WU design and execution | **New hypothesis detection during WUs:** During primary work, note unexpected |
| unit-145 | ### WU design and execution | **WU artifacts:** WU output (reports, structured data, analyses) goes in |
| unit-146 | ### Consolidation protocol | **Trigger:** On demand — Brian decides when the hypothesis set needs it. Common |
| unit-147 | ### Consolidation protocol | **Scope:** Full set. Read the entire hypothesis index, all hypothesis files, and |
| unit-148 | ### Consolidation protocol | **What consolidation does:** |
| unit-149 | ### Consolidation protocol | Identifies merges (same claim in different words) |
| unit-150 | ### Consolidation protocol | Identifies splits (one hypothesis conflating separable predictions) |
| unit-151 | ### Consolidation protocol | Notices emergent clusters (several hypotheses that are aspects of one question) |
| unit-152 | ### Consolidation protocol | Identifies orphans (untested hypotheses nobody references) |
| unit-153 | ### Consolidation protocol | Re-assesses statuses against accumulated evidence |
| unit-154 | ### Consolidation protocol | Archives superseded hypotheses (status → `challenged`, iteration entry |
| unit-155 | ### Consolidation protocol | **What consolidation produces:** |
| unit-156 | ### Consolidation protocol | **Updated hypothesis files** with iteration entries recording every structural |
| unit-157 | ### Consolidation protocol | **A numbered consolidation report** (`docs/v3-framework/consolidation-N.md`): |
| unit-158 | ### Consolidation protocol | **Updated hypothesis index** reflecting all file changes. |
| unit-159 | ### Consolidation protocol | **The consolidation → new forward plan coupling is one-directional:** |
| unit-160 | ## Provenance | All artifacts have consistent naming: |
| unit-161 | ## Provenance | **Forward plans:** `forward-plan-N.md`. Active = highest number. Retired plans |
| unit-162 | ## Provenance | **Consolidation reports:** `consolidation-N.md`. Numbered sequentially. Each |
| unit-163 | ## Provenance | **WU artifacts:** `WU[plan].[unit]-descriptive-name.md` (or directory). Write- |
| unit-164 | ## Provenance | **Hypothesis files:** `NNN-slug.md` in the `hypotheses/` directory. |
| unit-165 | ## Provenance | **Implementation candidates:** `implementation-candidates.md`. Proposed codebase |
| unit-166 | ## Provenance | **When to consult provenance:** Understanding why a hypothesis looks the way it |
| unit-167 | ## Provenance | **Provenance informs, never prescribes.** Retired forward plans are reference, |
| unit-168 | ## Context documents | Two companion files live in this skill's folder (`.claude/skills/v3-buildout/`): |
| unit-169 | ## Context documents | **VERSION-HISTORY.md** — Verifiable facts about the project timeline and |
| unit-170 | ## Context documents | **CORPUS-STATUS.md** — What material exists and what state it's in: the 112- |
| unit-171 | ## What this skill does NOT govern | **Story content decisions.** Brian decides story structure, categorization, |
| unit-172 | ## What this skill does NOT govern | **Prose technique.** The planner specifies goals and mechanisms, never how to |
| unit-173 | ## What this skill does NOT govern | **Planner features.** When a hypothesis implies a codebase change, the change |
| unit-174 | ## What this skill does NOT govern | **Declaring conclusions.** Every finding is a hypothesis until Brian baselines |
