# study-registry-schema

`docs/v3-framework/studies.md` — one row per study, appended by the preparing activity at
the moment Brian approves its plan, which is his go. Never edited: a study abandoned is a
fact the artifacts show, not a row removed. Every study's folder under
`docs/v3-framework/studies/` is named by its id.

```markdown
| id | type | corpus | go |
|---|---|---|---|
| exploration-of-v1-archive | exploration | v1-archive | 2026-09-12 |
| verification-of-fimfiction-stories-fid-primary | verification | fimfiction-stories | 2026-09-20 |
```

`id` is `<type>-of-<corpus>-<slug>`, the slug lowercase `[a-z0-9-]+` naming what the
study's directions do, unique across the registry, authored at the go and never changed:
`verification-of-<corpus>-<slug>` always carries its slug; `exploration-of-<corpus>`
carries one only when the corpus is explored again under a different reading. No ordinal.
`type` ∈ `verification | exploration` is the one the id's prefix names, and names the chain:
exploration runs preparing-to-explore-a-corpus, exploring-a-corpus and reviewing-leads;
verification runs preparing-to-verify-a-corpus, verifying-a-corpus, reviewing-findings,
surfacing-candidates and promoting-refereed-candidates. `corpus` is a name from
`CORPORA.md`. The pipeline directions are no study: the referee's and claiming's directions,
calibrations and calibration batches sit in `docs/v3-framework/pipeline/referee/` and
`docs/v3-framework/pipeline/claiming/`, and their full batches under the verifications they
serve. Nothing else is authored here: where a study stands is derived by the tool from its
artifacts into `state.md`, and a tool a study needs is built as its first task.
