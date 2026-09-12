# Hypothesis migration — the result

Written 2026-09-12, after the migration sessions ruled by `d-2026-09-12-5` ran. It records what
the batch produced, what was verified and how, and what it left for Brian.

**Provenance.** Assembled by a Claude Code session from the migration sessions' own final reports
in `codesessions.db`, with each claim then checked against the artifact it names — the files on
disk, `DocIntegrity check`, git, and the primary sources. Where a report and the evidence
disagreed, the evidence is what is written here. §6 is the sessions' own words, grouped and
attributed; it rules on none of them.

**The counts in §1, §2 and §5 are the state at 2026-09-12** — a record of what was true when the
batch finished, not a standing claim. `DocIntegrity check docs/v3-framework/hypotheses` answers
the checker question at any later date.

---

## 1. What ran

Fifty sessions, 2026-09-12 04:38–05:07, three to six minutes each, in roughly eight parallel
waves. Ids 002–050; 001 had been migrated earlier in the schema session. Id 026 was invoked
twice.

| | |
|---|---|
| Files written | 47 |
| Dropped under `d-2026-09-12-6` | 2 — `017-desktop-vs-code-split`, `027-cognitive-mode-principle` |
| Duplicate invocation | 1 — 026, run by `33dadd79` and again by `8b367fd3` |
| Written files passing the checker | 47 of 47 |
| Records left empty | 47 of 47 |
| Leftover frontmatter | none |
| `INDEX.md` / `state.md` / cross-file edits | none |
| `v3-buildout-2` skill loads | none |
| Interruptions, permission rejections, `AskUserQuestion` calls | none |

The batch ran unattended end to end. No session stopped to ask; every unresolved judgment went
into a report, which is what the skill asked for.

---

## 2. Verification performed, 2026-09-12

Not a re-run of the migration. What was checked, and how:

- **The checker**, over the whole folder rather than per-file as each session ran it.
- **Record emptiness and frontmatter**, by reading every `## Record` section and every file head.
- **Forbidden content classes**, by grep across the written set for cross-hypothesis id
  references, retired vocabulary and `Tested by` provenance.
- **The six mandated statement reverts**, against `git show 6d64c464`.
- **Restoration claims**, against `source_material_references/hypotheses-google-keep-dump.md` —
  the head itself, not a session's quotation of it.
- **Statement drift**, by word-level comparison of each `## Hypothesis` section against `HEAD`,
  in both directions, to find claims that entered rather than only claims that left.
- **Boundary compliance**, by querying each session's tool-call stubs in `codesessions.db` for
  the Keep dump, the MCP server, `INDEX.md` writes and the `v3-buildout-2` skill.

Every restoration spot-checked was verbatim accurate against the head: 010's *"I won't be using
haiku because of a capability ceiling"* (Keep line 16), 011's *"Great for coding work. Makes
friction on non coding. Also friction for iteration"* (line 6) and *"its language is not good"*
(line 61), 044's *"V2's thin non bespoke codebase is because I wanted clean code but tooling was
weaker (web chat)"* (line 29).

---

## 3. The rules that held

**Every K-trajectory session read the Keep dump rather than the `D`-entry.** All fourteen of them
(006–018, 044). This was the briefing's §4 instruction aimed squarely at the layer known to be
lossy, and it has no exceptions in the batch.

**All six mandated reverts are exact.** 013, 029, 030, 033, 034 and 038 match `6d64c464`
verbatim — and the three that also carried a §6 defect (030's *"requiring its own recognition as
a mode or cross-cutting phenomenon"*, 033's trailing question, 034's *"needs evidence-based
definition"*) had that clause cut and nothing else. Two rules composed correctly, unsupervised.

**All six of the briefing's known `D`-layer casualties were found and handled**, including the
inverted claim, which 011 restored into § Origin while leaving the statement's polarity
untouched and escalating it — the line the briefing drew.

**The forbidden content classes are cleared.** The only cross-hypothesis id references surviving
in the written set are 023's split provenance to 024, which is the permitted exception, and 040's
reference to its own id.

---

## 4. What the sessions found that the briefing did not anticipate

The briefing warned about compression and inversion between layers. Five sessions independently
found the failure one level above that: **an AI-authored idea standing in a file as Brian's.**

- **041** traced "sufficient stability" to a 2026-04-20 Claude conversation (conv 21, block 642)
  in which Brian's own stated instinct was fully sequential and the assistant proposed the
  refinement. The file had always read as his articulation.
- **021** found the four/fifth-register split proposed by the assistant at seq 1373 of `ce84e603`
  and approved tersely at 1374; his own words to that point were in a three-voice frame.
- **040** separated his question from Claude's four-name taxonomy, which the old file had blurred
  into a single voice.
- **006** flagged the four-factor conclusion as assistant-synthesized, and found that D1's and
  H25/H26's decompositions are not the 1:1 map the consolidation treated them as.
- **031** declined to report an unratified conclusion as his, noting he changed topic rather than
  agreeing.

Alongside these: **048** corrected two numbers against the transcript (three FID
misclassifications, not four; 10 supporting / 3 challenging, not 11 / 3), and **038** found a
quote misattributed to the naive chapters that is v1-archive Plotpoint 100.

---

## 5. Defects and residue

**The two dropped hypotheses left stale files on disk.** `d-2026-09-12-6` and the skill say a
drop means the file is not written; neither says the pre-existing file is removed. So
`017-desktop-vs-code-split.md` and `027-cognitive-mode-principle.md` remain in the old schema,
and at 2026-09-12 the folder-wide check reports eight failures, all of them from these two files
and none from anything a session wrote. A gap in the instructions, not a session error. Whether
they are deleted or kept as the record of a drop is Brian's.

**Two sessions crossed the MCP boundary.** 041 used `search_conversations` and `get_blocks`; 039
used `list_lineage` to verify a citation. 029 loaded the MCP tool schemas, then stopped itself
and said so in its report. 041's crossing is what produced the sufficient-stability finding in
§4 — the rule was broken and the break was productive, which is worth knowing before the rule is
either tightened or relaxed.

**Statement repair went past what the briefing reserved, in six files.** The briefing leaves a
sound-but-imprecisely-worded statement alone and marks further repair open. Six statements grew
rather than shrank: 037 (+36 words), 031 (+27), 009 (+25), 007 (+19), 016 (+19), 023 (+17). 037
is the clearest — its old statement was already diagnosable, and the session rewrote it into a
sharper and more specific claim. The standing constraint permits removal freely and addition not
at all, so these six are where to look for a claim that entered.

**Retired vocabulary survived twice.** 049's statement still reads `codebook`, since renamed to
`directions`; 042's § Origin cites WU5d. The WU references in 046–050 are the provenance the
briefing explicitly blesses and are not this.

**Fewer than half the sessions loaded the citation rule.** Twenty-three of fifty loaded the
`code-sessions` skill; all fifty queried the archive. The `Typed:` / `Chose:` distinction — the
trap the briefing spends its §5 on — was therefore out of context for more than half the batch.
No instance of the failure was found in the written files, but that is the thinnest margin in the
run.

**026 ran twice, and the duplicate agreed.** The second session found the file already migrated,
re-derived the C8 trajectory itself rather than trusting the working tree, read the head, and
confirmed the result. An accidental double-blind check that came out clean.

---

## 6. The escalation register

Every parting message carries a "For Brian" section. Eleven say nothing is needed; four say
nothing and then attach an observation; thirty-five flag something substantive. Grouped by the
nature of the ask, with the session that raised it.

### 6.1 A ruling only Brian can make

| id | session | the ask |
|---|---|---|
| 011 | `aaa868a8` | **The inverted claim.** § Origin now carries both halves of the Opus 5 initiative — asset for coding, friction for non-coding and for iteration. The statement still carries it as a pure advantage. The briefing reserves the polarity. |
| 041 | `f858ef43` | **"Sufficient stability" did not originate with Brian.** The file asserts the principle is correct on the strength of his 2026-08-29 endorsement, not his formulation. Worth settling before anything is baselined against it. |
| 034 | `f32f0c91` | **Comedy was settled; atmosphere and narrative voice were not.** His 2026-08-18 message raised both as open questions; H6 and everything downstream flattened all three. The migrated file is the first place that asymmetry has been written down. |
| 035 | `31b0a2ac` | **The chain answered in the opposite direction.** The 2026-08-18 exchange concluded embedded texts fall outside M1–M4 *but* route through existing craft tracks. H7 kept the first half only. |
| 021 | `2942fe15` | **The fourth/fifth register split rests on an approved AI proposal**, with his own words still in a three-voice frame at that point. |
| 006 | `f9c38447` | **The merge may not be one.** `harness` and `intent` have no counterpart across D1 and H25/H26, yet the consolidation treated the two lists as one decomposition at two granularities. |

### 6.2 A judgment made under the migration's licence, disclosed for checking

- **027** `bb903ca8` — dropped; the head is a meta-question and the hypothesis half is absorbed by
  023 and 033. Independent tracking would be a fresh mint.
- **017** `cb0ac398` — dropped; the source is a question he was asking himself, and *"remains the
  correct architectural boundary"* was invented at the plan layer.
- **014** `92ff8b89` — split a three-clause statement and kept only the causal clause. Not a
  pre-established call.
- **044** `9b1c6330` — scoped the hallmark-wall and v2-stall content out despite the plan's Tier J
  note naming it, as duplicating 019/020.
- **003** `1f93533a` — the syuzhet-against-three-fabula-layers passage appears to be walked back in
  the same breath; kept as reader-plausibility-only.
- **002** `8e8b6325` — left out the plan's own "core of 002" claim (v0 observation → v3
  experimentation), which does not trace to his words within the search path.
- **013** `7b406f0e` — the domain-qualifier restoration is the session's own identification, and
  the one substantive addition in that file.
- **018** `2a78a79e` — kept a compound statement that sits close to the prescription pattern,
  judging it a necessary-conditions claim.
- **049** `7df239c3` — kept *"FID-centred readings of DT passages"* though it is not anchored in the
  seq 79/80 exchange, removal being an uncompensated cut.

### 6.3 Real content now written nowhere

The migration mints nothing, so each of these is orphaned until Brian rules:

- **005** — **E7**, *"Memory files should be pointers and policies, not state claims."* Never a
  source of any hypothesis; folded into 005 informally at the plan layer, and evicted by the
  narrowing.
- **022** — **E10's second idea**: voice arbitration resting on content-block metadata rather than
  voice, pending a v1-archive data op.
- **011** — his Keep question *"When is 4.6 going to be deprecated?"*
- **030** — mode vs. cross-cutting phenomenon vs. modifier, cut because it had been folded into
  the statement.
- **016** — whether a conversational API shape fits non-conversational data.
- **043** — the `SubjectRelation` / `NarrativePropertyValue` feasibility note: real, sourced, and
  without a home in the schema.
- **004** — whether Audit mode should move from note-by-note to sweep-oriented.
- **033** — the 034 companion pairing, *"not wrong, just unrepresentable in this schema"*.
- **015, 026, 023, 019, 032, 037, 028** — cross-file pointers the schema forbids recording: the
  D13↔H27 link between 015 and 021; the 024/026 triad-coincidence question; goal-category examples
  belonging to 033; the seq-1251 fifth-voice turn flagged *"(another hypothesis?)"*; the
  Kitty/Chrysalis material 032 wrongly gained and 037 should carry; and the plan's "NLM introduced
  vocabulary from Pokemon analysis, not from P&K" line item.

### 6.4 A wording that outruns its source, left alone and flagged

- **012** `e519de75` — "analytical rigor" is a gloss the reorganization added; his sentence names
  RLHF and commercial incentives.
- **039** `1b84b2b1` — H15's "improved" framing is the deliberation's compression; his own words
  are more open.
- **042** `7b85904e` — he named three modes; the five-mode statement is H21's extension.
- **040** `3646b0db` — his question was tentative, and the hypothesis rides on Claude's four-name
  taxonomy being the right cut.
- **031** `3cb322be` — *"or vice versa"* stood in the file from 2026-08-31 with no source at any
  layer.

### 6.5 Corrections to the record

- **048** `4460831a` — two numbers corrected against the transcript: three FID misclassifications,
  not four; 10 supporting / 3 challenging, not 11 / 3.
- **029** `9bb1556f` — **the taxonomy may be wrong at its own head.** Conv 21 block 613, quoted
  inside the same assistant turn that proposed the hypothesis, lists *pure event / ironic / tragic
  / closing*; two sentences later that turn substitutes *aligned*, and the substitution propagated
  into H10, the plan and both file versions. Unresolved — the session was barred from the
  conversations corpus. The place to check is conversation 21, block 613 and nearby.
- **045** `c9bb0705` — a correction to the briefing itself: it calls P the healthiest trajectory,
  but here the plan layer had already added unsupported content.
- **010** `b829da5e` — E13 was left out rather than admitted without promotion; and D7's "reasoning
  quality" element traces to D8's Opus 4.6-vs-5 comparison, a cross-pollination at the
  reorganization layer.

### 6.6 Sessions that flagged nothing

**008, 009, 020, 024, 025, 026 (first run), 036, 038, 046, 047, 050.** **007, 018, 030** and
**049** reported nothing to escalate and then attached the observation recorded above.

---

## 7. Open

Carried forward, not decided here:

1. The inverted claim's polarity — `hypothesis-migration-briefing.md` §6 reserves it, and 011 is
   written on the assumption that it is still reserved.
2. The two dropped files, still on disk in the old schema.
3. Whether the six grown statements of §5 admitted a claim that was not in their sources.
4. `INDEX.md` and `state.md`, regenerated once after the set, with the two dropped ids losing
   their rows.
5. The orphaned content of §6.3, each of which is a mint or a deliberate drop.

The densest cluster is §6.1: five of its six entries are the same failure — an AI-authored idea,
or an AI's flattening of an open question, standing in a file as a settled claim of Brian's. That
pattern is not in the briefing. The sessions found it themselves, which is the strongest evidence
in the batch that one-session-per-hypothesis walking to the head does work the layered chain
could not.
