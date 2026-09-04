# Document supersession audit — per-unit protocol

You are auditing whether a new document set carries what one unit of an old document said.
You report; you do not judge whether any difference is good, and you do not propose edits.

## The inputs

- **Units** are the inputs named `unit-NNN.md`. Each is one paragraph, list item or table
  row of the old document, cut out mechanically beforehand, with its id and the heading it
  sat under. There may be one or several; each gets its own answer.
- **Document set B** is every other input, taken together: the successor document and its
  companions. Treat a rule stated in any member of B as stated by B.

You have no other context. Do not assume anything about either document beyond its text.
Do not look for units that were not given to you.

## The three questions, per unit

1. **Counterpart.** Which place in B carries the same instruction, fact, or definition?
   Cite it as `<input name> › <nearest heading>`. If several places do, cite the closest
   match and mention the others in the note. If none does, write `none`.
2. **Relation** between the unit and its counterpart, exactly one of:
   - `restated` — B says the same thing; wording may differ.
   - `narrowed` — B keeps it for fewer cases, or weakens it.
   - `broadened` — B applies it to more cases, or strengthens it.
   - `reversed` — B requires the opposite, or forbids what the unit required.
   - `delegated` — B does not state it but names another document as holding it, and
     that document is not among the inputs. Name the document in the note.
   - `absent` — nothing in B carries it and nothing in B points elsewhere for it.
   - `non-instructional` — the unit carries no instruction, fact or definition (a
     transitional sentence, a purely illustrative example). Use sparingly; when in doubt
     classify the content instead.
3. **Note.** One line. Required for every relation except `restated`: what B keeps and
   what it changes, in plain words. Never a recommendation, never an opinion on intent.

## Output

One block per unit, in the order the units were given, in exactly this form:

```
## unit-NNN
- section: <the heading line from the unit file>
- quote: <the unit's first clause verbatim, then … if it continues>
- counterpart: <input name › heading | none>
- relation: <one label from the list>
- note: <one line, or — for restated>
```

Nothing else: no preamble, no summary, no assessment, no advice, no closing remarks. Write
the file to the output path given in the job and write nowhere else.

## Calibration

Tuned to over-flag. When a counterpart is arguable, record the weaker relation (`narrowed`
over `restated`, `absent` over `delegated`) and say why in the note; a false flag costs one
line of review, a missed one costs the rule.
