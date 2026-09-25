# StoryPlanner.ResultsQuery

A query engine over one batch's results. It reads the batch's own files, beside the
`definition.md` every verb takes (the directions it names, `index.md`, `calls.md`, `results/`),
prints one table per query, and writes nothing. Built 2026-09-15 under building-a-tool for
exploration-of-technique-mechanism-goal-co-occurrence (d-2026-09-15-2, -3); tested in
`tests/StoryPlanner.Tests/ResultsQuery/`.

It is what `write-leads` reads results through (the v3-buildout skill,
`conducting-an-exploration.md` § write-leads), and what reviewing-leads re-runs a lead's queries
with. The rules for leads are there and in `schemas/leads-schema.md`; this file says only how to
drive the tool.

## What it reads

- **Fields**: every `list of line` field whose What to produce text declares parts separated by
  ` | ` after the text's first colon. Each part is a **column**: its label is the part's text up
  to its first comma or semicolon, its short id the label's first word that is not a function
  word (`c<k>` when there is none or two columns share one). A query names a column by short id,
  by position or by full label. `columns` prints them.
- **Lines**: one per entry of such a field, split on the bar. Only items with a successful call
  in `calls.md` are read; an item with none is *missing*, and a result that does not parse back
  as the directions declare is *malformed*, both left out of every count.
- **Arity**: a line with more or fewer parts than the field declares is kept and listed under
  `health`. Its parts after a stray bar are shifted, so read such a line with `list --item`.
- **Stories**: a line's story is its item's locator up to the group separator, `#` unless
  `--group` names another. A locator without one makes every item its own story, and `by-story`
  then adds nothing; that is true of the v1 plot-point cut (`pp-<id>`), whose chapter is only in
  the index's description column.

## Running it

The published copy, never `dotnet run`:

```
tools/StoryPlanner.ResultsQuery/publish/StoryPlanner.ResultsQuery.exe <verb> <definition.md> [options]
```

After a change to this project or to `StoryPlanner.BatchFiles`:
`dotnet publish tools/StoryPlanner.ResultsQuery -c Release -o tools/StoryPlanner.ResultsQuery/publish`.

| verb | prints |
|---|---|
| `columns` | the batch's answered, malformed and missing counts; each bar-part field's columns with position, short id and label |
| `health` | items, answered, malformed, missing, lines, lines per item, arity problems by item, per column how many parts are `none`, a hedged none or empty; the stopwords and the normalization |
| `list` | every matching line: item, then each part |
| `cites` | the distinct items the matching lines come from, as citation tokens `<study>/<batch>/<item>` |
| `terms` | the words of one column (`--col`) over the matching lines, by count, with how many lines carry each; `--n 2` or `3` for runs of words, `--position k` for the k-th word only |
| `pairs` | pairs of words that occur together within one value of `--col`, by the number of lines |
| `sort` | the values of `--col` sorted, each with its item |
| `by-story` | lines and items with a line per story, and each story's answered/items |
| `run "<query string>"` | re-runs a printed query string exactly |
| `serve [--url http://127.0.0.1:5191]` | a page taking the same queries, re-reading the batch on each |

| option | does |
|---|---|
| `--field <key>` | the field; the first bar-part field when absent |
| `--where <column>=<regex>` | keeps lines whose part matches, case-insensitive; repeatable, one per column, all must match |
| `--item <id>` or `--story <s>` | scopes to one item or one story; not both |
| `--sample <n> [--seed <s>]` | a seeded sample of the matching lines; the same seed gives the same lines |
| `--col <column>` | the column `terms`, `pairs` and `sort` read; required for those three |
| `--n`, `--position`, `--exclude <regex>`, `--top <n>` | for `terms`: word runs, one position, words to drop, rows shown (50 by default) |
| `--group <sep>` | the story separator, when not `#` |

Counts count lines, not items, except `cites` and the items column of `by-story`. Words are
normalized one fixed way, printed under `health`: lowercase, `'s` dropped, anything not a letter
or digit a space, stopwords dropped; nothing is stemmed, merged or ranked beyond a count.
Grouping `dread` with `foreboding` is a judgment the tool never makes; a regex alternation
(`--where "kind=dread|forebod"`) is how a reader asks for both.

## The query string

The first line of every output is the query's canonical string, the same whichever way it was
asked:

```
rq1 batch=<study>/<batch> answered=<n> field=<key> [where <col>~<regex> ...] [story=<s>] [item=<id>] [sample=<n> seed=<s>] view=<verb> [col=<c>] [n=<n>] [position=<k>] [exclude=<regex>] [top=<n>] [group=<sep>]
```

The command line writes a filter `--where kind=humor`; the string writes it `where kind~humor`.
A value holding a space, a quote or a backslash is double-quoted. `answered` is the number of
answered results the query ran over: `run` on a batch that has moved on prints a note saying so,
which is how a review knows a lead was drawn before the batch finished. A lead's `query` line is
this string copied exactly as printed, never retyped.

## Drawing leads with it

A sequence that works; the questions asked are the leads-writer's, and nothing here is a
finding:

1. `health`: how many answered, missing and malformed; the arity problems to read by item.
2. `columns`: the columns' short ids.
3. `terms --col <the column that names what was seen>`: the vocabulary the readers used, as
   they wrote it.
4. For a term or a set of terms: `list --where <col>=<regex>` to read the lines, then `terms` on
   another column under the same filter to see what they share (where in the item, what marks
   it), then `cites` under the same filter for the items.
5. `list --item <id>` to read everything one reader wrote about one item.

Each query a lead rests on goes into the lead's `query` list as printed, and the items come from
`cites`. Where a lead needs the item's chapter or subject, the index's description column has it.
