# surfacing-candidates

Enables promoting-refereed-candidates.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| assemble-claim-batch | session | runner tool-source | findings hypothesis-statement hypothesis-index directions calibration | definition index items calls tally | specified | The N×M sweep as a batch under the method's claiming directions: one item per standing finding, each holding the current hypothesis set then the finding; dry-run-batch; execute-batch as the hand-off; the results are the claims — per finding, which hypotheses it bears on, relevance only |
| assess-claim-items | agent | | directions items | results | specified | One call per finding, the hypothesis set and the finding as its message: a bare list of the hypothesis file names the finding bears on, relevance only, no direction; the only writer of its results |
| assemble-referee-batch | session | runner tool-source | results findings hypothesis-statement directions calibration | definition index items calls tally | specified | One referee item per claim (finding, target), holding the target's current statement and the finding's text collated from findings.md; dry-run-batch; execute-batch as the hand-off |
| assess-referee-items | agent | | directions items | results | specified | One blind call per item: the falsifier, and diagnostic supporting, diagnostic challenging or non-diagnostic; the only writer of its results |
| compose-candidates | session | DocIntegrity | results tally findings declined-candidates hypothesis-record | candidates | specified | The generated view: joins the claiming results, findings.md, the referee results, declined-candidates.md and the hypothesis records into candidates.md — the diagnostic candidates in one section that opens the document, each materialising its finding, verdict, falsifier and status, the non-diagnostic claims in a section at the foot |

## Preconditions

For `assemble-claim-batch`: a verification whose findings have been reviewed and which has no
claiming batch yet; the hypothesis set is current; the method's claiming directions, in
`docs/v3-framework/pipeline/claiming/`, have an accepting calibration at their hash. For the
referee rows: the method's referee directions, in `docs/v3-framework/pipeline/referee/`, have
an accepting calibration at their hash.
compose-candidates runs after the referee batch is complete, and again from
promoting-refereed-candidates to reflect outcomes.

## assemble-claim-batch

The session reads every standing finding of the verification, neither withdrawn nor superseded,
and assembles a claiming batch under the study, each finding one item. The collator —
`tools/StoryPlanner.PipelineCollator`, its `claim` subcommand over the study's `findings.md` and
`docs/v3-framework/hypotheses/` into the batch folder — collates the item bodies, each the current
hypothesis set, every statement under its file name, then the finding, and writes the index
naming the collator. The definition, of kind full, names the claiming directions and their
accepting calibration by relative path into the claiming folder, and the model and effort that
calibration measured. Per the `agent-runner` skill: dry-run-batch, execute-batch, the hand-off;
the host calls every finding and writes the tally at completion. No pilot: the claiming
directions are calibrated. The results are the claims on disk — no separate authored artifact.

## assess-claim-items

Instructed by the claiming directions body (the criteria and what to produce) as its system
prompt and nothing else; the hypothesis set and the finding as its message. It answers with a bare list of the
hypothesis file names the finding bears on — relevance only, never a direction: supporting or
challenging is the referee's alone. An empty list is legal — a finding may bear on none.

## assemble-referee-batch

One referee item per claim — per (finding, target) pair the claiming results name. The item
holds exactly two things: the target's current statement, copied from `## Hypothesis` with no
frontmatter, no record; and the finding's text, collated from `findings.md` through the
finding's token. It holds no citation, no locator, no other candidate, and nothing anyone wrote
as a falsifier. The batch folder is under the verification; the collator — the same tool's
`referee` subcommand over the claiming batch, this batch and `docs/v3-framework/hypotheses/` —
reads the claiming results and writes the index (naming the collator, each item's locator its
candidate identity `<finding-slug> → <target>`, which compose reads back) and the item bodies; the
definition, of kind full, names the referee's directions and their accepting calibration by
relative path into the referee folder, and the model and effort that calibration measured.
dry-run-batch; execute-batch; the host calls every item and writes the tally. No pilot: the
referee's directions are calibrated.

## assess-referee-items

Instructed by the referee's directions body as its system prompt and nothing else; no tools, no
MCP. Given the statement and the finding, it writes the falsifier — what the finding would have
been if the statement were false — and classifies the claim by which side of that observable
the finding shows, or non-diagnostic if no such observable can be named or the finding is
consistent with both. Tuned to over-flag: a false non-diagnostic costs one decision of Brian's;
a false diagnostic costs a hypothesis's record.

## compose-candidates

Not a session's judgment: a mechanical join the session runs as `DocIntegrity compose <study>`. It
reads the claiming results (the (finding, target) claims), findings.md (the finding text), the referee
results (verdict and falsifier per candidate), declined-candidates.md (declined, with reasons)
and the hypothesis records (promoted, by their citations), and writes candidates.md whole: the
diagnostic candidates in one section that opens the document, each materialising its identity
(finding token, target), its finding text, the referee's verdict and falsifier, and its status —
promoted (a hypothesis record cites it), declined (with its reason) or pending; the non-diagnostic
claims in a labelled section at the foot, context the referee set aside, not promotion options and
not counted. The file is read-only and regenerable; nothing is hand-edited.

## Never

Gives the referee a citation, an excerpt, a locator, another candidate, or the target's record;
copies a finding's text onto a claim; lets a claiming call assert a direction; lets a session
write a falsifier or a verdict; hand-edits candidates.md; executes under a hash without an
accepting calibration; authors or edits the referee's or the claiming directions here.
