# Resuming a hypothesis migration session

Written 2026-09-12, for the pass that tightens the migrated hypothesis files. A migration
session that Brian resumes reads this file whole before it does anything else.

## Why you are resumed

You migrated one hypothesis under the `migrate-hypothesis` skill and
`hypothesis-migration-briefing.md` (`d-2026-09-12-5`, `-6`). Your context still holds those
instructions and your final report. Since then, rulings of 2026-09-12 changed what a hypothesis
file holds and how Brian's words enter it, and the batch's result,
`hypothesis-migration-result.md`, recorded what the fifty sessions left for him.

**Where this file and your earlier instructions disagree, this file governs.** The pass
continues the migration's bootstrap: it may rewrite the file you wrote. Nothing requires a file
to be rewritten; Brian has asked for these to be.

**Brian is present.** This is a human-in-the-loop session, not the unattended batch. Anything
that is his to rule is put to him in this session and waited on, never decided and reported.

## What changed since you ran

1. **Rule 10**, SKILL.md § Constitutional rules of `.claude/skills/v3-buildout-2/`. Read it
   whole. In short:
   - A field that records deliberation or reasoning quotes Brian's typed words verbatim, inside
     quotation marks, his own quotation marks becoming single ones, with the session's framing
     outside them. An Origin's `reasoning` is such a field.
   - His typing is his own prose wherever it is recorded as his: a user turn or `Typed:` line in
     a transcript read from `codesessions.db`, a Keep note, a user block of a conversation in the
     `conversations` corpus, his navigation note on a block. Quoted from anywhere but the
     discussion at hand, it is cited to its source: session and seq, Keep dump line,
     conversation and block.
   - Never quoted as his: a `Chose:` label, a `Q:` line, an assistant turn or block, session
     text he approved, and text whose voice is not established as his, such as v1 archive and
     plan notes.
   - A hypothesis statement is never quoted. It is the claim.
   - Quoting does not oblige the reasoning to use his words; it is the form they take when used.
2. **The MCP server may be used.** The migration's ban is lifted. For provenance read the
   `conversations` corpus (`search_conversations`, `get_blocks`, `list_conversations`) and
   `lineage`; a block's speaker says whose words it is, and a block's summary is Brian's own
   navigation note. The story plan has no bearing on this work.
3. **Rule 9's reliance clause.** Nothing relies on a hypothesis file yet: every record is empty
   and nothing cites one. The file is composed whole and written to its own path, as before.
4. **Slugs are never changed.** Within this bootstrap alone a slug is renamed where the file's
   claim was never legitimate or its meaning was inverted, and only when Brian approves it, as
   041's was.
5. **Questions.** The pass writes no question on its own. If Brian raises a question about a
   corpus, `asking-a-question` applies: read CORPORA.md and that corpus's question list first,
   put every withdrawn entry that bears on it in front of him with its reasons, and show the
   entry before writing it. A `question` is a neutral assertion; `raised by` holds why it exists
   and quotes him; `suggested test` holds procedure only. A comparison across corpora is a
   hypothesis, never a question.
6. **A new hypothesis is never minted here.** Name it in the report.

## What to do

1. **Load the `code-sessions` skill**, whatever you loaded before. Its citation rule decides
   every quotation below.
2. **Read**, in full: SKILL.md § Constitutional rules; `schemas/hypothesis-file-schema.md`;
   your hypothesis file as it stands on disk; your own final report; and every line naming your
   id in `hypothesis-migration-result.md` §§ 4–6.
3. **Recheck every attribution at its source**, never from memory of the earlier run. For each
   thing the file gives as Brian's, find the words: typed by him, or a label he chose, a question
   put to him, an assistant's proposal he approved. Where an idea began with an assistant and he
   took it up, the Origin says so plainly, as 041's trace did.
4. **Compose the Origin's `reasoning` under rule 10**: the observation, his assertion, the
   motivation and what raised it, his typed words quoted with their sources, the framing yours.
   The rest of § 6 of the migration briefing still bounds what it may hold. The Origin's `date`
   stays the day the hypothesis was captured.
5. **Check the statement**: a prediction and nothing beside it, unquoted, readable alone, with
   no claim its sources do not carry. If your id is one of the six grown statements of
   § 5 (007, 009, 016, 023, 031, 037), diff each clause against its source and cut what no
   source carries. Retired vocabulary goes: `codebook` is `directions`, and a work-unit number
   outside 046–050's provenance is not cited.
6. **Take your register lines to Brian**, one at a time, framed with what the source shows:
   - § 6.1, a ruling only he can make: present it and wait.
   - § 6.2, a judgment made under the migration's licence: state it for him to confirm or undo.
   - § 6.3, content now written nowhere: ask whether it is a question about a corpus, a
     hypothesis to mint later, or let go.
   - § 6.4, a wording that outruns its source: show the source beside the wording; he rules.
   - § 6.5, a correction to the record: apply it and show it.
7. **Show the whole file in the chat** before writing it. Write it once Brian approves.
   `DocIntegrity` checks it at the write; a failure is fixed in the file, never in the schema.
8. **Report**, short, in this order: what changed and why; attributions corrected; register
   lines and how Brian ruled; anything unresolved; a hypothesis found that is not yet minted.

## What not to do

- Write anything in `## Record`.
- Touch another hypothesis's file, `INDEX.md` or `state.md`, or delete the dropped 017 and 027
  files; those are Brian's, after the pass.
- Mint a hypothesis, or apply `minting-a-hypothesis` or `iterating-a-statement`'s gates to this
  file.
- Quote Brian from your earlier report or your context without returning to the source.
- Decide a register line that is his.

## Sessions

Resumed with `claude --resume <session-id>` from the repository root.

| id | session | note |
|---|---|---|
| 001 | — | migrated in the schema session, not by a migration session |
| 002 | `8e8b6325-e634-4f0e-ae97-789ce34e1893` | |
| 003 | `1f93533a-55d1-46ea-9730-aa4deace132c` | |
| 004 | `30e877a6-0fb4-4bd4-ba00-a2a9a190a9e4` | |
| 005 | `a78d46df-4b45-4d92-95c9-99e204dbf487` | |
| 006 | `f9c38447-8480-467c-b4db-15e234e680dd` | |
| 007 | `f8726321-7ddc-4730-8a71-f433ad11aabb` | |
| 008 | `d04946cc-00ac-4d04-9d5d-59a829dd176b` | |
| 009 | `d541b886-4a58-4507-8051-99e1c85205ec` | |
| 010 | `b829da5e-18ac-4869-8e85-e8b3af5fe015` | |
| 011 | `aaa868a8-d3e7-4432-bd7f-4dde64ca9ad4` | |
| 012 | `e519de75-a88a-402e-8e18-ed638097ab02` | |
| 013 | `7b406f0e-e76b-49a2-8055-6287bdbfad8e` | |
| 014 | `92ff8b89-68c4-4bbc-af00-8adbfe03e560` | |
| 015 | `a406a82c-f9b3-4db4-a2d0-e085772c6fd7` | |
| 016 | `ddc8d32e-cb38-457b-acb4-c1574fea411d` | |
| 017 | `cb0ac398-2dfc-403c-9bd4-f412c762ad29` | dropped; resume only to revisit the drop |
| 018 | `2a78a79e-2348-438a-b5da-0d658fdf9340` | |
| 019 | `30c0ff33-bd2d-4185-a3d0-3195850cfca7` | |
| 020 | `05f138b7-0503-4d96-ac31-3309546f4456` | |
| 021 | `2942fe15-525f-4772-8a8e-e21ee6fb7f9f` | |
| 022 | `17c2da1d-2d8c-453a-b752-c3da1a9ff4e8` | |
| 023 | `f2738a83-0a07-4bcb-89d7-e8c0e181c966` | |
| 024 | `d71a4cad-69a3-43bd-b1fe-4e9f9b8d8b5b` | |
| 025 | `d6357533-e23b-4119-9fe8-4ee6e916fd5c` | |
| 026 | `33dadd79-b620-4141-821a-426b04c52ae1` | also run by `8b367fd3-08dd-4f77-9987-8cd36214d6ca`; resume one |
| 027 | `bb903ca8-b4aa-4623-baf8-16b8c2c496c8` | dropped; resume only to revisit the drop |
| 028 | `63af134b-4464-4a7b-bb99-6bb5698864f7` | |
| 029 | `9bb1556f-d434-4ea6-8377-c0a1ee413848` | |
| 030 | `c2b26e13-da76-4293-8a64-9742316993df` | |
| 031 | `3cb322be-0044-4958-9284-19e35c8754d3` | |
| 032 | `504c0b29-e189-4d4a-a2b0-552fb4894ef5` | |
| 033 | `b67843b0-04ed-4d4b-bd27-8189a4fe93bc` | |
| 034 | `f32f0c91-54da-42d2-9cd4-74e8be145d0b` | |
| 035 | `31b0a2ac-c18d-4dc0-82cf-6f02f950d716` | |
| 036 | `1e5372c3-d782-47ec-807d-857e522f6a7d` | |
| 037 | `a9663800-8515-4f79-8e20-8105180849d7` | |
| 038 | `a80af9d7-1cfd-4871-9f26-c3e97cc89167` | |
| 039 | `1b84b2b1-3ad7-49ba-80b9-20d02d311c10` | |
| 040 | `3646b0db-b439-47c3-a169-c383635a3bbf` | |
| 041 | `f858ef43-a580-48d5-8f13-ced302ab2666` | rewritten with Brian on 2026-09-12; its Origin still paraphrases him |
| 042 | `7b85904e-2326-47a9-bb02-a43aab7c4fea` | |
| 043 | `107272b5-4861-4729-9a47-88b2d18bf91f` | |
| 044 | `9b1c6330-d150-4dc3-af00-4689a9ffd15a` | |
| 045 | `c9bb0705-4941-455d-95a9-37215452a2af` | |
| 046 | `a9cf8ee5-0cd0-4cb0-be3a-d663fd0fc14e` | |
| 047 | `99111878-1c98-4e47-b727-3e72e7658c26` | |
| 048 | `4460831a-9ef9-4ee4-a579-b4c4b2378cd0` | |
| 049 | `7df239c3-9238-4dba-9ad6-51b21b4bd88c` | |
| 050 | `9e53a4f2-8182-429e-9f69-0a9ffa559457` | |
