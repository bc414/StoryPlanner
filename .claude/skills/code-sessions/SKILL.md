---
name: code-sessions
description: Query codesessions.db — the sealed Claude Code transcript archive (engineering-process provenance, deliberately NOT in the MCP server). Use when asking how or why a planner feature was built, what was tried and cut, what an old ingest reported, or what an earlier session actually did — the record of *why*, authoritative for *nothing*. Also covers the ingest procedure and the retention decisions behind the archive.
---

# Code sessions — the sealed-but-greppable engineering archive

`C:\Users\Brian\Desktop\TLTT CodeSessions.db` holds reduced Claude Code session transcripts
from the projects where the planner and its data pipelines were built. It is the sixth corpus,
and the only one **deliberately not served by the MCP server**: the split mirrors CLAUDE.md's
Two AI roles. LINEAGE answers *story* lineage ("where did this story decision come from") for
Desktop; this archive answers *instrument* lineage ("how did the planner come to be this way")
for Claude Code sessions — which have a shell and query it directly. Desktop never sees it,
and its story needs are met by the products these sessions shipped (reports in lineage.db,
docs, briefs).

## Standing before you query

- **Authority order** (CLAUDE.md): live data > code > CLAUDE.md / FEATURE-AUDIT > transcripts.
  This corpus sits at the transcript level — the record of *why*, authoritative for *nothing*.
- **The standing trap, same as design-conversations:** transcripts contain complete, persuasive
  arguments for features that were later cut. Establish what is LIVE from FEATURE-AUDIT and the
  code first; only then read the transcript for the why.
- Session `Title` values are the platform's machine-generated labels (last `ai-title` wins) —
  labels for navigation, never the author's words. Brian's words are the `user`-role bodies.

## What was kept and what the stubs mean (extraction policy, 2026-08-17)

Communication kept, computation dropped:

| In a transcript | In the db |
|---|---|
| user / assistant text | **verbatim** |
| subagent transcripts (`subagents/agent-*.jsonl`) | own Sessions rows (`Kind='subagent'`, `ParentSessionId` = containing session) |
| tool call | `[tool_use: Edit — WorldDateModel.cs]` — mechanical name + main argument |
| tool result | `[tool result elided — 12,345 chars]` — **never stored, not withheld** |
| thinking | dropped, no marker |
| user paste > 20k words | `[Large paste — N words, M chars]` (gemini's plan-paste rule) |
| images | `[image attached]` |
| `ai-title`, queue/file-history/metadata records | not dialogue — dropped (allow-list: user, assistant, ai-title) |

Records keep `Uuid`/`ParentUuid` in timestamp order — a rewound session shows **both branches**;
the DAG is never linearized into one reconstructed thread. Full fidelity (thinking, tool
payloads) exists only in the raw JSONL: the live `~/.claude/projects/` (retention raised to
3650 days on 2026-08-17) and the one-time snapshot
`C:\Users\Brian\Documents\ClaudeCode Projects Snapshot 2026-08-17\`.

## Schema

```sql
Sessions(Id, SessionId /*file stem; UNIQUE*/, ProjectDir, Kind /*main|subagent*/,
         ParentSessionId, Title, Slug, FirstTimestamp, LastTimestamp, RecordCount,
         TotalChars, SubagentCount, MalformedLines, SourceBytes, SourceMtimeUtc,
         FirstIngestedUtc, LastSeenUtc)
Records(Id, SessionId, Uuid, ParentUuid, Seq /*timestamp order*/, Timestamp,
        Role /*user|assistant*/, Body, BodyChars)   -- UNIQUE(SessionId, Uuid)
```

`LastSeenUtc` is proof the source file still existed on that ingest run; a session whose
`LastSeenUtc` is older than the newest run has **aged off disk and lives only here** — that is
the archive working, not staleness.

## Query recipes

`sqlite3` is on PATH (Downloads\sqlite-tools). Quote the db path; open read-only when the
ingest might be running: `sqlite3 "file:C:/Users/Brian/Desktop/TLTT CodeSessions.db?mode=ro"`.
(`LIKE` is case-insensitive for ASCII — usually what you want.)

**Sessions overview, newest first:**
```sql
SELECT substr(SessionId,1,8), ProjectDir, Kind, substr(FirstTimestamp,1,10) AS started,
       RecordCount, Title
FROM Sessions ORDER BY FirstTimestamp DESC LIMIT 30;
```

**Which sessions touched a file or topic** (tool stubs make file-touch greppable):
```sql
SELECT DISTINCT r.SessionId, s.Title, substr(s.FirstTimestamp,1,10)
FROM Records r JOIN Sessions s ON s.SessionId = r.SessionId
WHERE r.Body LIKE '%WorldDateModel.cs%'
ORDER BY s.FirstTimestamp;
```

**The session behind a dated CLAUDE.md decision** — bound the date, then keyword:
```sql
SELECT r.SessionId, r.Seq, r.Role, substr(r.Body,1,200)
FROM Records r
WHERE r.Timestamp LIKE '2026-07-31%' AND r.Body LIKE '%coverage%suggestion%'
ORDER BY r.Timestamp;
```

**Brian's own framing on a topic** (his words carry the citation weight):
```sql
SELECT SessionId, Seq, substr(Body,1,300) FROM Records
WHERE Role='user' AND Body LIKE '%retrieval%not%suggestion%' ORDER BY Timestamp;
```

**Context around a hit** (the exchange, not the needle):
```sql
SELECT Seq, Role, substr(Body,1,400) FROM Records
WHERE SessionId='<full-session-id>' AND Seq BETWEEN <hitSeq>-3 AND <hitSeq>+3
ORDER BY Seq;
```

**Page a session start to finish:**
```sql
SELECT Seq, Role, Body FROM Records
WHERE SessionId='<full-session-id>' AND Seq BETWEEN 1 AND 25 ORDER BY Seq;
```

**Branch points** (a rewound/edited session — more than one child of the same parent):
```sql
SELECT ParentUuid, COUNT(*) FROM Records
WHERE SessionId='<full-session-id>' AND ParentUuid IS NOT NULL
GROUP BY ParentUuid HAVING COUNT(*) > 1;
```

**A session's subagents, and what each was asked:**
```sql
SELECT s.SessionId, s.Slug, s.RecordCount,
       (SELECT substr(Body,1,200) FROM Records WHERE SessionId=s.SessionId ORDER BY Seq LIMIT 1)
FROM Sessions s WHERE s.ParentSessionId='<full-session-id>';
```

**Archive health — what has aged off disk and survives only here:**
```sql
SELECT substr(SessionId,1,8), ProjectDir, substr(LastSeenUtc,1,10) AS last_seen, Title
FROM Sessions
WHERE LastSeenUtc < (SELECT MAX(LastSeenUtc) FROM Sessions)
ORDER BY LastSeenUtc;
```

## Ingest (progressive — run it to catch the archive up)

```
dotnet run --project tools/StoryPlanner.CodeSessions -- tools/StoryPlanner.CodeSessions/configs/code-sessions.json [--apply] [--project NAME]
```

- **Authored include-list** in the config (2026-08-17: StoryPlanner, Gemini-Full-Analysis,
  Takeout-Scan, Fimfiction-Comments-Capture — where planner process knowledge was created).
  Never add a dir because it "looks related"; the list is Brian's.
- Per-session replace keyed on `(SourceBytes, SourceMtimeUtc)`; unchanged files are only
  touched; **there is no delete path** — absent-but-retained is the point. `projectsRoot` can
  be re-aimed at the 2026-08-17 snapshot to backfill; per-session replace makes running against
  both sources safe in any order.
- **Authored exclusion rule** (`excludeFirstUserMessage`, 2026-09-03): a main session whose
  first human user message matches a configured regex is never ingested, and its subagents go
  with it. Currently `^/analyze-story ` — the 2026-08-27 incident, when a runner's infinite
  retry of `claude -p /analyze-story` left 9,245 transcripts in the StoryPlanner project dir.
  There is no delete path, so the rule is what makes a re-run safe after a manual cleanup of
  the db: **never run `--apply` with the rule absent.** Tool results and images are
  array-content user records, not human messages, and are skipped when finding the first one.
- **Prevention, not curation.** The archive holds human-rooted interactive session trees only
  (a subagent of an interactive session is part of the tree). Autonomous agents — classifiers,
  investigators, auditors, referees launched by `tools/StoryPlanner.AgentRunner` — run from a
  folder **outside** the repo with `--no-session-persistence`, so their transcripts never reach
  an included project dir; the runner's ledger is their record. A batch that must be excluded
  by rule is a batch that was launched wrong.
- Dry run first; it prints per-project new/changed/unchanged/absent-but-retained/excluded
  tallies and the per-rule match counts. `--list-excluded` prints the excluded transcript
  paths (kind + path, one per line) and exits — the one sanctioned way to act on the
  excluded set outside the ingest, so a cleanup selects by the ingest's own predicate and
  never by one of its own. For rows whose transcripts are already gone from disk, the one
  purge that has been ruled is `scripts/purge-excluded-2026-09-03.sql` beside the config —
  a dated op, run by Brian, applying the same predicate inside the db; not a delete path.
- A torn trailing line (live session appending mid-copy) is counted in `MalformedLines`, never
  fatal — the next run picks the completed line up via the changed stamp.

## What NOT to do

- Don't surface this corpus to Desktop or add it to the MCP server without a decision from
  Brian — the no-surface posture is itself a recorded decision (2026-08-18).
- Don't treat a transcript's argument as a feature's rationale until FEATURE-AUDIT confirms the
  feature survived.
- Don't reconstruct "the" conversation thread through a branch point — both branches happened;
  report both.
- Don't mine story content from here for Desktop-style analysis; story lineage lives in the
  LINEAGE corpus and the archive, through their own tools.
