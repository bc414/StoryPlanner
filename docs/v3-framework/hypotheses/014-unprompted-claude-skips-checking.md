## Hypothesis

Across the pipeline's history, what has actually driven better AI-assisted planning work is
not which model was in the seat but whether that model had instructional scaffolding — a
system prompt, a custom gem, or a skill — and how well it was crafted. A sufficiently capable
model given well-crafted instructions does the work adequately regardless of which specific
capable model it is; the same model left to its unscaffolded defaults does not.

## Origin

- date: 2026-08-31
- reasoning: Raised while Brian traced the pipeline's own history back through nine months of
  conversation, lineage, and code-session records — the same corpora that hold the prompting
  patterns behind every accepted and rejected output, so instruction design does not have to
  theorize from first principles about what good analytical behavior looks like. His account of
  the arc: v1's custom Gemini gem (the TLTT Analyzer Gem's four rules — "The Canon is Sacred,"
  "Mandatory Knowledge Retrieval," "Materialist Worldbuilding," and "The user is the sole author
  and visionary; you are the analyst"), then an AI Studio system prompt he built himself with
  rules like "limited bullet points," "no direct address," "no prose," "stay analytical and
  ground in materialism instead of tropes," and "ask socratic questions" — which, in his words,
  "felt futile but it wasn't a dead end" — then v2, where he ran with no system prompt at all
  because Claude Sonnet 4.6's early results (Applejack's evolved element of honesty) convinced
  him Claude simply worked out of the box; and now v3's CLAUDE.md and skills. He later corrected
  that v2 belief himself: "my old working theory was Claude=good, Gemini=bad ... New hypothesis
  is that the system prompt is still universally helpful" — and, separately, realized he had
  "literally never looked at" the MCP server's ServerInfo.Instructions, having assumed it "just
  worked" the same way. His motivation: build v3's full instructional stack — CLAUDE.md, skills,
  the MCP server's instructions, and eventually a Desktop project prompt — with the same
  deliberate craft as the old Gem, mined from what the historical record shows worked rather
  than theorized fresh, because the stack does several distinct jobs (what to ask, how to
  analyze, what must be retrieved, what the output should look like) that one monolithic prompt
  would conflate. What raised it to a hypothesis rather than a private lesson learned: this same
  review is where he first framed the shift as a general claim about the pipeline rather than an
  account of his own past mistake — that adequate instructions, not model choice, are what make
  the difference.

## Record
