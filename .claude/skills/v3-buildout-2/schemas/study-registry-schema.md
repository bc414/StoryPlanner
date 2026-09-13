# study-registry-schema

`docs/v3-framework/studies.md` — one row per study, appended by the preparing activity at
the moment Brian approves its plan, which is his go, and by revising-the-method for an
audit. Never edited: a study abandoned is a fact the artifacts show, not a row removed.
Every study's folder under `docs/v3-framework/studies/` is named by its id.

```markdown
| id | type | corpus | go |
|---|---|---|---|
| exploration-of-v1-archive | exploration | v1-archive | 2026-09-12 |
| verification-of-fimfiction-stories-fid-primary | verification | fimfiction-stories | 2026-09-20 |
| audit-of-revision-3 | audit | skill | 2026-10-02 |
```

`id` is `<type>-of-<corpus>-<slug>`, the slug lowercase `[a-z0-9-]+` naming what the
study's directions do, unique across the registry, authored at the go and never changed:
`verification-of-<corpus>-<slug>` always carries its slug; `exploration-of-<corpus>`
carries one only when the corpus is explored again under a different reading; an audit is
`audit-of-<slug>`, its corpus the skill. No ordinal. `type` ∈ `verification | exploration
| audit` is the one the id's prefix names, and names the chain: exploration runs
preparing-to-explore-a-corpus, exploring-a-corpus and reviewing-leads; verification runs
preparing-to-verify-a-corpus, verifying-a-corpus, reviewing-findings,
surfacing-candidates and promoting-refereed-candidates; audit runs revising-the-method.
`corpus` is a name from `CORPORA.md`, or `skill` for an audit. The referee is no study: its directions and
calibrations sit in `docs/v3-framework/referee/` and its batches under the verifications
they judge. Nothing else is authored here: where a study stands is derived by the tool
from its artifacts into `state.md`, and a tool a study needs is built as its first task.
