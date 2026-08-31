---
id: 14
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

Instructions and skills should be iteratively crafted from evidence of what
worked across 9 months of conversation history (not from first principles),
the real paradigm shift is from "find a model that works out of box" to "build
instructions that make any adequate model work," and the full instructional
text stack (CLAUDE.md, skills, MCP instructions, project prompts) has multiple
dimensions deserving the same craft as v1's custom gem.

## Record

- created | 2026-08-31T20:00: Four converging observations. First, the
  conversation corpus, lineage, and code sessions contain the prompting patterns
  that produced accepted or rejected outputs — instruction design should mine
  that evidence rather than theorize from first principles about good analytical
  behavior. Second, the historical arc traces a continuous instructional effort:
  v1's custom Gemini gem (four rules), AI Studio system prompts that "felt futile
  but weren't a dead end" (a weaker version of the same approach), v2's absence
  of instructions (Claude "just works" — a hypothesis about instructional text
  quality, not a fact), and v3's CLAUDE.md + skills. The paradigm shift is not
  model choice but instructional scaffolding — Constitutional AI gives Claude a
  higher floor, but the floor is not the ceiling, and both Gemini-with-gem and
  Claude-with-skills outperform their respective defaults. Third, the
  instructional text stack has multiple dimensions: what to ask (input framing),
  analytical approach (method and rigor), what to retrieve (MCP query strategy),
  and output constraints (format, register, scope) — each deserving independent
  iteration rather than monolithic prompt design. Fourth, the full stack for each
  consumer (Claude Code: CLAUDE.md → skills → MCP ServerInfo.Instructions; Claude
  Desktop: project prompt → project skills → shared MCP instructions) shares the
  MCP server's instructions as infrastructure, and "just works" is the same
  untested assumption Brian correctly challenged about Claude not needing a system
  prompt. The MCP server's instructions are a first-draft "binary help text"
  never audited against the v3 framework findings.
