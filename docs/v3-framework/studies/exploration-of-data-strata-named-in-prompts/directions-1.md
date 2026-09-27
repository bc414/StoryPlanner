---
questions: questions/data-strata-named-in-prompts
---

## What you are given

One exchange from an archive of conversations between a person planning a long work of
fiction and an AI model: which layer of the archive it comes from, a label for the
conversation, its date, the ids of the two turns, then the model's turn, and then the user
turn that followed it. A turn is a run of consecutive messages by one side, so either may be
several messages joined. The model turn is there as the context the user turn answers; it is
not what you are reading for. You have nothing else: not the rest of the conversation, not
the data being spoken about.

## How to read

Read the user turn. Find every place where it names a source of data — a body of material
the model is to draw on, or not to draw on. Sources take many forms: a story plan or part of
one, a database, a set of notes, an archive of earlier conversations, a document, an export,
an era or generation of the author's own material, a wiki, a published show or game, a
fandom, the model's own training or general knowledge, the current conversation itself,
something the author says from memory.

For each source the user turn names, record what it is called, in the turn's own words, and
what weight the turn attaches to it — treat as true, prefer over another, read first, check
against, treat as outdated, treat as provisional or as a suggestion rather than settled, do
not use, ignore, and so on, in your own terms. Where the turn sets one source above another,
record that too.

A source is named only where the turn actually points at it. A passing mention of a
character or an event is not a source. Read only what the words say; never supply a source
the turn does not name, and never rank sources yourself.

## What to produce

- sources: list of line, one per source of data the user turn names, four parts separated by a bar: source, as the turn names it | weight, what the turn tells the model to do with it, in your words | what marks it, a few words of the user turn allowed | new, one of first-named where the turn introduces the source as if the model does not yet know of it, or referred-to where it speaks of it as already known; empty where the turn names none
- order: list of line, one per place the turn sets one source above another, two parts separated by a bar: over, which source is put above which, in the turn's own names | what marks it, a few words of the user turn allowed; empty where it sets none
- about: line, what the user turn is doing as a whole, in one sentence

## Never

Never give a position inside a turn: no line or paragraph numbers, and no quotation used
only to say where. Never read the model turn for sources; read it only to understand what
the user turn answers. Never say what the turn should have named.
