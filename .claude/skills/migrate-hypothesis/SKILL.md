---
name: migrate-hypothesis
description: Migrate ONE founding hypothesis file into the current schema — a one-time bootstrap, invoked once per hypothesis with its id (e.g. /migrate-hypothesis 013). Walks that hypothesis's chain back to Brian's own words, then composes the new file whole. Delete this skill when the set is migrated.
---

# Migrate one hypothesis

**One invocation, one hypothesis.** The id is the argument. If none was given, ask which.

This is a **bootstrap, not standard operating procedure.** The fifty founding files were
written on 2026-08-31 under a method that no longer exists, and `d-2026-09-12-5` rules that the
SOP rules of `mint` and `iterate` do not bind their migration. Do **not** load the
`v3-buildout-2` skill: its preconditions, its three mint criteria and its gates govern new
hypotheses and iterations, and applying them here will produce the wrong refusals. Read its
schema file — that is a file, not the skill.

## First action

Read, in this order, and do not start before all three are read:

1. `docs/v3-framework-historical/hypothesis-migration-briefing.md` — the historical data and the
   tracing methodology. It holds the sources, the chronology, the six trajectories and which one
   this hypothesis is on, the known failure modes, the procedure, and what may be written.
2. `.claude/skills/v3-buildout-2/schemas/hypothesis-file-schema.md` — the shape to write, and
   what each section may and may not hold.
3. The hypothesis's current file, `docs/v3-framework/hypotheses/NNN-*.md`.

The briefing is the instruction set. This skill is only the entry point and the boundaries.

## What this session does

The briefing's §5 is the procedure; follow it rather than a summary of it. In outline: take the
trajectory from §3, walk the chain **to its head**, read Brian's own words there, diff the
current file against them, and compose the new file whole.

Two things decide almost every judgment on the way:

- **The head is the authority.** Every layer between it and the file is a reading of it, and at
  least one of those layers is known to have compressed and in one case inverted what he wrote.
  Never treat an intermediate summary as the source.
- **No claim enters that is not already in the file or in its sources.** Compression,
  restoration, repair, removal and reordering are free. A new assertion is not.

`codesessions.db` is where his typed turns live; the `code-sessions` skill has the query
recipes and, in its citation rule, the distinction that matters most here — a `Typed:` line is
his prose, a `Chose:` line is a machine's label he picked and supports no inference about what
he meant, and the `Q:` line is a machine's too.

## What to write

One file: `docs/v3-framework/hypotheses/NNN-slug.md`, composed whole and written to its own
path. Never edit the existing file into shape. The record is created **empty** — the migration
promotes no evidence and writes no entry.

If the statement fails the test in the briefing's §6 — could a referee, handed it and a finding,
write a falsifier — either salvage the prediction its chain carries, or **drop the hypothesis**,
which means the file is simply not written. A dropped id is not reused and does not become an
empty file.

The checker runs on the write. A failure is a real failure: fix the file, never the schema.

## What not to do

Touch another hypothesis's file. Edit `INDEX.md` or `state.md` — both are regenerated once
after the whole set, and forty sessions editing one shared file would collide. Write a question
list; the migration collects no questions. Mint a new hypothesis, even when the chain clearly
holds one — that goes in the report for Brian. Write a record entry. Load `v3-buildout-2`.
Consult the MCP server: the story plan has no bearing on this.

## Report at the end

Short, and in this order:

- **Written or dropped**, and for a drop, what the chain showed.
- **Restored** — anything in his words that an intermediate layer had lost or reversed.
- **Cut** — each excision and which rule it fell under.
- **Unresolved** — any claim in the old file that no layer supports, and what was done with it.
- **For Brian** — a proper hypothesis found inside a discarded one, an inverted claim, or
  anything the session had to guess at.
