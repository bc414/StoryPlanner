# Resuming a hypothesis migration session

Written 2026-09-12 for the pass that resumes each migration session, and revised the same day
under `d-2026-09-12-29` to `-34`. A resumed session reads this file whole before it does
anything else.

## Why you are resumed

You migrated one hypothesis under the `migrate-hypothesis` skill and
`hypothesis-migration-briefing.md` (`d-2026-09-12-5`, `-6`). Your context still holds those
instructions and your final report. Since then, rulings of 2026-09-12 changed what a hypothesis
file holds and how Brian's words enter it, and the batch's result,
`hypothesis-migration-result.md`, recorded what the fifty sessions left for him.

**Where this file and your earlier instructions disagree, this file governs.** If you were
resumed under an earlier version of this file, re-read it now and continue under this one.

**Brian is present.** This is a human-in-the-loop session, not the unattended batch. Anything
that is his to rule is put to him in this session and waited on, never decided and reported.

## What the pass is

The migration left files built from malformed data: statements compressed, extended or inverted
on the way from Brian's words, and ideas an assistant proposed standing as his. With Brian, you
take your file and its chain apart, remove what ought not be there, and write the hypothesis he
states or a draft he approves. A statement may be rebuilt from the ground up.

## What binds you

- **The constitutional rules**, SKILL.md § Constitutional rules of `.claude/skills/v3-buildout-2/`.
- **`schemas/hypothesis-file-schema.md`**: the shape the hook holds, and the content rules. A
  statement is a prediction and nothing beside it: a referee handed the statement and one finding
  could write its falsifier, what the finding would have been were the statement false; it names
  no other hypothesis, claims no evidence and asks no question. An Origin's `reasoning` does not
  hold what the schema's list bars, and that list bars your own words and framing, never Brian's
  typed words quoted under rule 10.

Nothing else binds you: not `d-2026-09-12-5`'s limit on claims beyond the file and its sources,
not the briefing's line leaving a loosely worded statement alone, and not the gates of
`minting-a-hypothesis` or `iterating-a-statement`.

## Brian's words

Rule 10, in short; read it whole in SKILL.md.

- A field that records reasoning, such as an Origin's `reasoning`, quotes what Brian typed
  verbatim, as typed, misspellings included, inside quotation marks, his own quotation marks
  becoming single ones. Your framing sits outside them.
- His typing is his prose wherever it is recorded as his: the discussion in this session; a user
  turn or `Typed:` line in a transcript read from `codesessions.db`;
  `source_material_references/hypotheses-google-keep-dump.md`, the notes he wrote in Google Keep
  and pasted into one file, which is not the `google-keep` corpus; a user block of a conversation
  in the `conversations` corpus; and his navigation note on a block.
- A quote needs no pointer to its source. Name the occasion in your own words where it helps a
  reader.
- Never quoted as his: a `Chose:` label, a `Q:` line or any question put to him, an assistant
  turn or block, session text he approved, and text whose voice is not established as his, such
  as v1 archive and plan notes.
- A hypothesis statement is never quoted. It is the claim.
- Quoting does not oblige the reasoning to use his words; it is the form they take when used.

## What else holds

1. **The MCP server may be used** for provenance: the `conversations` corpus
   (`search_conversations`, `get_blocks`, `list_conversations`) and `lineage`. A block's speaker
   says whose words it is, and a block's summary is Brian's own navigation note. The story plan
   has no bearing on this work.
2. **The record stays empty.**
3. **Slugs.** A slug is never changed, save that with Brian's approval you may rename your file's
   slug where its claim was never legitimate, its meaning was inverted, or its statement was
   rebuilt in this pass and the slug no longer names it.
4. **The Origin's date.** A statement rebuilt in this pass takes the day it was rebuilt; an
   unchanged statement keeps its date.
5. **Minting.** A new hypothesis found in the session may be minted in it when Brian words or
   approves its statement. Confirm the next unused id with him before writing it, and name it in
   your report. A hypothesis he does not mint is named in the report.
6. **Questions.** Write none on your own. If Brian raises a question about a corpus,
   `asking-a-question` applies: read CORPORA.md and that corpus's question list first, put every
   withdrawn entry that bears on it in front of him with its reasons, and show the entry before
   writing it. A `question` is a neutral assertion; `raised by` holds why it exists and quotes
   him; `suggested test` holds procedure only. A comparison across corpora is a hypothesis, never
   a question.

## What to do

1. **Load the `code-sessions` skill**, even if you loaded it before. Its rule on `Typed:`,
   `Chose:` and `Q:` decides what counts as Brian's words.
2. **Read**, in full: SKILL.md § Constitutional rules; `schemas/hypothesis-file-schema.md`; your
   hypothesis file as it stands on disk; your own final report; and every line naming your id in
   `hypothesis-migration-result.md` §§ 4–6.
3. **Lay out the chain end to end, from the sources**, never from memory of the earlier run: for
   each claim in the file, where it first appears and whose words it is, typed by Brian, a label
   he chose, a question put to him, or an assistant's proposal he approved. Where an idea began
   with an assistant and he took it up, the Origin says so plainly. That includes wording you draft
   in this session and he then repeats: it is your wording he took up.
4. **The statement.** Brian states it, or approves a draft he asked for. Run the referee test on
   it and state the result. Never use another hypothesis file as settled vocabulary or as
   authority: every hypothesis in the set is untested. Retired vocabulary goes: `codebook` is
   `directions`, and a work-unit number outside 046–050's provenance is not cited. If your id is
   one of the six grown statements of the result's § 5 (007, 009, 016, 023, 031, 037), diff each
   clause against its source.
5. **The Origin's `reasoning`**, under rule 10: the observation, Brian's assertion, the motivation
   and what raised it, his typed words quoted, the framing yours. A generalisation you draw over
   several sources is labelled as yours, never given as what the sources said.
6. **Take your register lines to Brian**, one at a time, framed with what the source shows:
   - § 6.1, a ruling only he can make: present it and wait.
   - § 6.2, a judgment made under the migration's licence: state it for him to confirm or undo.
   - § 6.3, content now written nowhere: ask whether it is a question about a corpus, a
     hypothesis to mint, or let go.
   - § 6.4, a wording that outruns its source: show the source beside the wording; he rules.
   - § 6.5, a correction to the record: apply it and show it.

   A ruling that is his is put to him before any edit would settle it, including an edit made
   under a general instruction such as cleaning up the Origin. Any cost you state for an option is
   checked against the method as written before he rules: narrowing a statement once evidence has
   landed, for instance, goes through `iterating-a-statement`, which needs a challenge first and
   fails its gate if any prior finding comes out non-diagnostic of the new wording.
7. **Show the whole file in the chat before writing it.** Write it only once Brian approves, and
   an Edit is a write. `DocIntegrity` checks it at the write; a failure is fixed in the file, never
   in the schema.
8. **Report**, short, in this order: what changed and why; attributions corrected; register lines
   and how Brian ruled; anything unresolved; a hypothesis minted, or found and not minted.

## What not to do

- Write anything in `## Record`.
- Touch another hypothesis file, `INDEX.md` or `state.md`, or delete the dropped 017 and 027
  files. `INDEX.md` is regenerated once, after the pass.
- Quote Brian from your earlier report or your context without returning to the source.
- Decide a register line that is his.

## When the pass ends

When every id in the table below but 017 and 027 has reported. 001 has no migration session to
resume: a fresh session reads the migration briefing's § 5 procedure and 001's row in its § 3
assignment, walks the chain to Brian's typed words, and then follows this file from step 2 of
What to do, the parts about its own report and result lines not applying. 017 and 027 are resumed
only if Brian chooses to revisit a drop, before the end. From the end, standard procedure governs
the hypothesis files.

## Sessions

Resumed with `claude --resume <session-id>` from the repository root.

| id | session | note |
|---|---|---|
| 001 | — | migrated in the schema session; a fresh session walks its chain first (§ When the pass ends) |
| 002 | `8e8b6325-e634-4f0e-ae97-789ce34e1893` | |
| 003 | `1f93533a-55d1-46ea-9730-aa4deace132c` | |
| 004 | `30e877a6-0fb4-4bd4-ba00-a2a9a190a9e4` | |
| 005 | `a78d46df-4b45-4d92-95c9-99e204dbf487` | |
| 006 | `f9c38447-8480-467c-b4db-15e234e680dd` | |
| 007 | `f8726321-7ddc-4730-8a71-f433ad11aabb` | |
| 008 | `d04946cc-00ac-4d04-9d5d-59a829dd176b` | |
| 009 | `d541b886-4a58-4507-8051-99e1c85205ec` | |
| 010 | `b829da5e-18ac-4869-8e85-e8b3af5fe015` | |
| 011 | `aaa868a8-d3e7-4432-bd7f-4dde64ca9ad4` | resumed 2026-09-12 under the earlier file and waiting on two register lines: re-read this file and continue. Its Origin's coding-asset and non-coding-friction content, which the briefing reserved to Brian, was cut before he ruled on it |
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
| 034 | `f32f0c91-54da-42d2-9cd4-74e8be145d0b` | resumed 2026-09-12 and reported; the statement was kept bundled after a cost stated wrongly, so revisit only if Brian chooses |
| 035 | `31b0a2ac-c18d-4dc0-82cf-6f02f950d716` | resumed 2026-09-12, statement rebuilt and file written; report owed. Its Origin dropped Brian's quoted observation, testing plan and motivation under a reading of the exclusions that `d-2026-09-12-34` does not hold |
| 036 | `1e5372c3-d782-47ec-807d-857e522f6a7d` | |
| 037 | `a9663800-8515-4f79-8e20-8105180849d7` | |
| 038 | `a80af9d7-1cfd-4871-9f26-c3e97cc89167` | |
| 039 | `1b84b2b1-3ad7-49ba-80b9-20d02d311c10` | |
| 040 | `3646b0db-b439-47c3-a169-c383635a3bbf` | |
| 041 | `f858ef43-a580-48d5-8f13-ced302ab2666` | rewritten with Brian on 2026-09-12, slug renamed; done |
| 042 | `7b85904e-2326-47a9-bb02-a43aab7c4fea` | |
| 043 | `107272b5-4861-4729-9a47-88b2d18bf91f` | |
| 044 | `9b1c6330-d150-4dc3-af00-4689a9ffd15a` | |
| 045 | `c9bb0705-4941-455d-95a9-37215452a2af` | |
| 046 | `a9cf8ee5-0cd0-4cb0-be3a-d663fd0fc14e` | |
| 047 | `99111878-1c98-4e47-b727-3e72e7658c26` | |
| 048 | `4460831a-9ef9-4ee4-a579-b4c4b2378cd0` | |
| 049 | `7df239c3-9238-4dba-9ad6-51b21b4bd88c` | |
| 050 | `9e53a4f2-8182-429e-9f69-0a9ffa559457` | |
