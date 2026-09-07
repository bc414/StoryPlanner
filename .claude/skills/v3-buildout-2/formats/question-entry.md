# question-entry

`docs/v3-framework/questions/<corpus>.md` — one file per corpus, one entry per question,
appended, never rewritten; only the `status` line changes, and only to `withdrawn`.
Written only by a process with mode `hitl`: a question is Brian's. Which codebook froze a
question and which round answered it are derived by the tool from the codebooks and the
rounds' `round.md`, into `state.md`; they are never written here.

```
### <short title>
- asked-by: <study id, "review of <study id>", "promotion of <scope>", or "ad hoc"> (<date>)
- hypotheses: <ids the answer would be evidence for or against>
- question: <one testable question about this corpus>
- predicate: <the frozen predicate a codebook would apply, if one suggests itself; may be blank>
- status: open | withdrawn (<reason>)
```

The `hypotheses` line is the only authored place a hypothesis-to-corpus edge exists; a plan
never copies it. Questions flow freely: any hitl activity may write into any corpus's list.
Leads never enter one; a question is what a lead raised, in Brian's words.
