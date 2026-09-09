# definition-schema

`docs/v3-framework/studies/<study>/batches/<batch>/definition.md`, the class `definition`,
frozen: the one authored file in a batch, written by a session before any execution and
never edited after the first, saying what the batch runs under. The shape below is what the
hook holds; the example is a conforming file a writer fills in and the block the checker's
tests read as their fixture; then how definitions are queried, and the check ids the hook
reports.

## Shape

The folder is `batches/<nn>-<slug>/`: `<nn>` two digits, sequential within the study from
01, assigned when the batch is defined; `<slug>` lowercase `[a-z0-9-]+`, unique in the
study, naming what the batch does and, when two batches differ in one thing, that thing.
The title is `# <batch> — definition`, `<batch>` the folder's own name.

| section | present | holds |
|---|---|---|
| the whole file after the title | required | fields, in this order and nothing else |

| key | present | type | value |
|---|---|---|---|
| `directions` | required | path to directions | the version the batch runs under, relative to this file, wherever it lives; the runner hashes its body as the system prompt |
| `kind` | optional: present exactly when the directions have Classes | enum: `sample`, `full` | `sample`, the batch a calibration is scored on, running under a version not yet accepted; `full`, a batch under an accepted version |
| `calibration` | optional: present exactly when `kind` is `full` | path to calibration | the accepting calibration of the version named; its title hash equals the directions' body hash and its verdict is accepted |
| `model` | required | line | the model every call runs under, as the CLI names it; every definition in a study names the same |
| `effort` | optional | enum: `low`, `medium`, `high`, `max` | the CLI's effort level; absent means the CLI's default |
| `tools` | optional | list of line | the tools opted in for the agent, as the CLI names them; absent means none |
| `mcp` | optional | path | the MCP config whose servers are attached to every call; absent means none |

A definition names no items and no output: the items are the batch's index, `index.md`
beside it, and the results are written by the runner under `results/`. It names no
directions hash, no version number and no study, all of which it resolves to or sits in,
and no ceiling or limit: the parallel ceiling, the usage cap and the idle limit after
which a silent call is killed are the host's, set in its configuration and adjustable on
its page, never a batch's. The runner reads it at every verb and writes nothing to it;
the checker holds it at the write and, once `calls.md` exists beside it, holds it
unchanged against the hash the calls file recorded at the first execution.

## Example

```markdown
# 03-scene-notes — definition

- directions: ../../directions-3.md
- kind: full
- calibration: ../../calibration-2026-09-19b.md
- model: claude-opus-4-6
- effort: high
```

## Queries

| question | how |
|---|---|
| what a batch ran under | its definition, a handful of lines |
| every batch under one directions version | `grep -rln 'directions-N.md' studies/<study>/batches/*/definition.md` |
| every batch of a study, in order | `ls studies/<study>/batches/` |
| a study's model | any of its definitions' `model` line, which the checker holds equal |
| batches defined and not yet executed | a definition with no `calls.md` beside it; state.md per study |
| a study's calibration samples and full batches | `grep -rn '^- kind:' studies/<study>/batches/*/definition.md` |
| batches that opted into MCP or a tool | `grep -rln '^- mcp:\|^- tools:' studies/*/batches/*/definition.md` |

## Checks

| check | fails when |
|---|---|
| `definition.batch` | the folder is not `<nn>-<slug>`, its number is not the next in the study, or its slug repeats one in the study |
| `definition.title` | the title is not `# <batch> — definition` with the folder's own name |
| `definition.fields` | a key is missing, unknown or out of order; a line is neither keyed nor continuation; a value is not of its type; `kind` is absent where the directions have Classes or present where they do not |
| `definition.directions` | the path does not resolve, or the file it names fails directions-schema |
| `definition.calibration` | the line is absent where `kind` is `full` or present where it is not; the path does not resolve; the calibration's title hash is not the directions' body hash; or its verdict is not accepted |
| `definition.model` | another definition in the study names a different model |
| `definition.frozen` | `calls.md` exists beside it and the file's hash is not the one the calls file recorded |
