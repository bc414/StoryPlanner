-- One-time purge of rule-matching sessions from codesessions.db — Brian's ruling, 2026-09-03.
--
-- The ingest has no delete path by design; this script is a deliberate, dated op, not a
-- path. It selects by the ingest's own predicate (excludeFirstUserMessage: ^/analyze-story ,
-- applied to the first human user record of each main session; slash-command markup is the
-- <command-name> form the transcript stores) and takes each match's subagents with it, exactly
-- as the ingest would have excluded them. The transcripts these rows came from were deleted
-- on 2026-09-03 (methodology-revision-1.md § Prompting evidence 4), so no re-run restores them.
--
-- Run from the repo root in PowerShell (after closing anything holding the db; `<` redirection
-- is not PowerShell, hence `.read`):
--   $db = "$env:USERPROFILE\Desktop\TLTT CodeSessions.db"
--   Copy-Item $db "$db.pre-purge-2026-09-03.bak"
--   sqlite3 $db ".read tools/StoryPlanner.CodeSessions/scripts/purge-excluded-2026-09-03.sql"
--
-- Dry-run check (2026-09-03, before the purge): 56 main sessions matched, 0 subagents.

create temp table purge as
with first as (select SessionId, min(Seq) Seq from Records where Role = 'user' group by SessionId)
select s.SessionId from Sessions s
  join first f on f.SessionId = s.SessionId
  join Records r on r.SessionId = f.SessionId and r.Seq = f.Seq
 where s.Kind = 'main'
   and (r.Body like '/analyze-story %' or r.Body like '%<command-name>/analyze-story%');
insert into purge select SessionId from Sessions where Kind = 'subagent' and ParentSessionId in (select SessionId from purge);

select 'sessions to purge', count(*) from purge;
select 'records to purge', count(*) from Records where SessionId in (select SessionId from purge);

begin;
delete from Records where SessionId in (select SessionId from purge);
delete from Sessions where SessionId in (select SessionId from purge);
commit;

select 'sessions left', Kind, count(*) from Sessions group by Kind;
select 'remaining rule matches', count(*) from Records where Role = 'user' and Body like '%<command-name>/analyze-story%';
vacuum;
