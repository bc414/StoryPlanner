# corpora-schema

`CORPORA.md` in the skill folder — the inventory of corpora, a fact file with no state in
it: one section per corpus, its id as the heading, then what it is, where it lives and how
it is read, then caveats. No progress (derived into `state.md` from the registry), no
readiness dates (they are decisions), no deferrals, no counts in prose (name the source of
truth). Its section headings are the corpus ids the question lists and the registry use.
Written by `build` when a corpus appears or a reader for it is built.

```markdown
## fimfiction-stories

- what: the Fimfiction stories analyzed under the v4 brief, and their analyses; the population is `.claude/skills/analyze-story/populations.md`
- where: `Documents/Fimfiction Favorites/markdowns/` and `markdowns1/` for the texts, outside the repo; `source_material_references/Reading Archive Analyses/` for the analyses
- read by: files; a runner job takes a story as an input file

Four stories in the favorites are outside the corpus: unread, abandoned, or dropped, named
in `populations.md`. The analyses are the map to loci; a verification's items are cut from the
texts, never from an analysis alone.
```
