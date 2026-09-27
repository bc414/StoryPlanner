# exploration-of-v1-theme-commentaries-content — leads

## Leads

### exploration-of-v1-theme-commentaries-content/kinds-across-the-commentaries
- lead: The readers cut the 144 commentaries into 398 passages, a median of 3 per commentary and 12 at
  the most, one commentary holding a single passage. Under the loose alternations: 204 lines over 124
  commentaries carry a bearing-on-the-theme name, 145 lines over 81 commentaries a fabula-content name,
  39 lines over 29 an on-page-plan name, 11 lines over 9 a name of analogy, real-world parallel or
  canon, 5 lines over 5 a note about the reader, and 2 lines over 2 a note to self. Under the exact
  name alone, bearing on the theme is 145 lines over 90 commentaries. 81 lines carry a name other than
  the five asked for by name, most of them a variant or a joint of two of the five
  ("bearing on theme", "bearing on the theme; fabula content"), so the families above overlap and the
  line counts do not sum to 398.
- seen in: the theme commentaries of plot points across the whole archive, every chapter represented
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages view=health
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~bearing view=cites
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~"^bearing on the theme$" view=cites
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~fabula|world.?building|lore|history view=cites
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~"on-page|page plan|staging|prose" view=cites
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~analog|real.?world|canon|source view=cites
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~reader view=cites
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~"note to self|to self" view=cites

### exploration-of-v1-theme-commentaries-content/commentaries-with-no-statement-of-bearing
- lead: 20 of the 144 commentaries hold no passage the reader named as a statement of the scene's
  bearing on the theme. What they hold instead: a piece of world fact with no mention of the scene (Star
  Energy's strength coming from diversity; how changeling society treats its failures; Eros's creed and
  the logic of his surrender; the Elements built to detect threats to the herd); a recap of what happens
  in the scene (Henri scheduling Applejack's rotation; Henri defining the word "poseur"; Applejack
  missing Fleur and Henri flirting); a bare label naming the beat that carries the link ("Fleur's
  twist"); a real-world argument in the author's own person (specialization and harmonic capitalism over
  standardization; the Griffonian Republic needing asset-specific investment as the New Deal coalition
  did); an addition to the scene (a dream-and-letter beat for Metzli to voice, with a piece of Mali's
  backstory); a production-history aside about Faust's principles; a note on rehabilitating a disliked
  canon episode by retconning the buffalo's fight into a proxy war; and, in one, a single-word
  placeholder instruction to analyze the link.
- seen in: theme commentaries spread across the archive, several on Honesty vs Poseurs, Intimacy and Liberty and Lauren Faust's Original Themes
- cites:
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-125-theme-16
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-130-theme-25
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-145-theme-13
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-222-theme-14
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-23-theme-7
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-233-theme-15
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-241-theme-26
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-247-theme-8
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-291-theme-23
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-312-theme-2
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-315-theme-2
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-34-theme-21
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-34-theme-8
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-35-theme-18
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-386-theme-18
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-40-theme-20
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-44-theme-18
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-66-theme-21
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-70-theme-19
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-79-theme-18

### exploration-of-v1-theme-commentaries-content/commentaries-that-hold-nothing-but-bearing
- lead: 51 of the 144 commentaries hold only passages the reader named as bearing on the theme, and
  nothing else. The other 73 hold a bearing passage together with at least one passage of another kind,
  most often fabula content. So a third of the boxes are pure theme commentary, a seventh hold no theme
  commentary at all, and half hold theme commentary mixed with something else.
- seen in: theme commentaries across the archive
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~bearing view=cites
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~"^(?!.*bearing)" view=cites

### exploration-of-v1-theme-commentaries-content/what-the-fabula-passages-hold
- lead: The 145 fabula-content lines state things true of the story's world without regard to whether
  this scene shows them: how an institution works (the Manehattan tycoons never promoting a thestral to
  management; the Federalist and Industrialist factions each reading the slogan "clothes" their own way;
  Trimmel's original speech as a press-office-vetted meritocracy talk), a character's history or
  psychology (Starlight skipping the school after Sunburst and the manifesto at age 10; Metzli's
  resentment fed by a condolence letter Celestia did not write), a law of the world (the Elements built
  to detect existential threats to the herd and fire a weaponized beam of Harmony), a policy (Equestria
  knowingly fuelling a drug epidemic in Skyfall's underclass to keep its own ponies out of the mud), and
  cast-wide generalisations (the Mane 6 bearing the Elements because their canon personalities mixed
  pink and red love). Several are written with capitalised concept labels and paired contrasts ("The
  ponies wanted Adulthood… The Tycoons gave them Alienation").
- seen in: theme commentaries across the archive, concentrated on the economic and factional themes
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~fabula sample=14 seed=7 view=list
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~fabula|world.?building|lore|history view=cites

### exploration-of-v1-theme-commentaries-content/content-repeated-from-the-synopsis
- lead: 76 lines over 57 of the 144 commentaries restate something the plot point's synopsis already
  says; 317 lines do not. In some the whole commentary is the repetition: one is Applejack's revelation
  speech about abandoned foals and New Mareland copied word for word from the synopsis, another restates
  a line Thorax speaks in the scene and adds nothing, another restates the synopsis's pitch on the
  "clothes for all" slogan and never mentions the theme.
- seen in: theme commentaries across the archive
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where repeated~^yes view=cites
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages view=health
- cites:
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-247-theme-8
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-222-theme-14
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-79-theme-18

### exploration-of-v1-theme-commentaries-content/on-page-plans-inside-the-theme-box
- lead: 39 lines over 29 commentaries plan what is to appear on the page rather than say anything about
  the theme: staging and the order of events, a line of dialogue to be spoken, an image, an exchange to
  be added to the scene. One commentary is a set of additions to the scene — a dream-and-letter beat for
  Metzli to voice — and nothing else; another is a two-sentence summary of an exchange in which Henri
  schedules Applejack's rotation and overrides her refusal of rest.
- seen in: theme commentaries across the archive
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~"on-page|page plan|staging|prose" view=cites
- cites:
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-125-theme-16
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-23-theme-7

### exploration-of-v1-theme-commentaries-content/notes-to-self-and-a-placeholder
- lead: Only 2 lines in the whole set are notes to self. One commentary is a single-word placeholder
  instruction to analyze the link and holds no analysis of its own; another is a two-word label pointing
  at the beat that carries the link ("Fleur's twist") and says nothing further.
- seen in: two theme commentaries, one on Lauren Faust's Original Themes and one on Intimacy and Liberty
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~"note to self|to self" view=cites
- cites:
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-34-theme-21
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-35-theme-18

### exploration-of-v1-theme-commentaries-content/notes-about-the-reader
- lead: 5 lines over 5 commentaries say what the reader is to feel, believe or take away, as against
  what the scene does for the theme. They are the rarest of the five kinds asked for by name after notes
  to self, which is the opposite of what the theme box's own question would suggest, since a thematic
  proposition is something a reader is meant to arrive at.
- seen in: five theme commentaries across the archive
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~reader view=cites

### exploration-of-v1-theme-commentaries-content/real-world-parallels-and-canon-inside-the-theme-box
- lead: 11 lines over 9 commentaries hold a real-world parallel, an analogy or a statement about the
  source show rather than about the scene: the New Deal coalition's decay after its founding
  generation's trauma faded, a Christianity analogy for the unified Republic absorbing Boreas worship,
  specialization and harmonic capitalism argued as real-world political economy, a production-history
  aside about Faust's principles, and the rehabilitation of a disliked canon episode by retconning the
  buffalo's fight into a proxy war.
- seen in: theme commentaries on Rugged Individualism / Wedge Issues Deconstructed, Bottom Up > Top Down, Lauren Faust's Original Themes and Isolationism is Immoral
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages where kind~analog|real.?world|canon|source view=cites
- cites:
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-241-theme-26
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-40-theme-20
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-233-theme-15
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-70-theme-19

### exploration-of-v1-theme-commentaries-content/register-of-the-passages
- lead: The register column names expository prose in 206 of the 398 lines and planning shorthand in 77;
  declarative occurs in 125, statement in 64, terse in 41, flat in 34, plain in 46, thesis in 35. 29
  lines sit in a parenthetical aside and 22 quote the item's own words. So the commentaries read
  predominantly as finished expository prose making assertions, with a smaller seam of clipped planning
  notes; the box holds both and nothing separates them.
- seen in: theme commentaries across the archive
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages view=terms col=register top=25

### exploration-of-v1-theme-commentaries-content/which-themes-carry-the-commentaries
- lead: Read against the index's description column, the 144 filled commentaries fall on 23 of the
  archive's themes, unevenly: Honesty vs Poseurs carries 21, Bottom Up > Top Down 17, Loyalty and
  Kinship and Accelerants used for Evil can be Repurposed for Good 11 each, P&K Subversion 9, Strong to
  be Merciful and Elements of Liberty - Healthy Balance of Pink and Red 8 each, Anti-Racism /
  Anti-Tribalism / Politics of Division 7, Laughter and Resilience 6; and at the other end Conscience,
  Kindness and Grace and Capitalism for Good / Ambition for Good carry one each.
- seen in: the theme names in the index's description column, across every chapter
- query:
  - rq1 batch=exploration-of-v1-theme-commentaries-content/01-theme-commentaries answered=144 field=passages view=health

### exploration-of-v1-theme-commentaries-content/commentaries-that-argue-a-real-world-thesis-in-the-authors-person
- lead: Some commentaries leave the story altogether and argue a position in the author's own person:
  one is a short manifesto of the theme as a real-world political-economic argument, specialization and
  harmonic capitalism against standardization and rugged individualism, with no mention of what the
  letter scene itself does; another asserts that the Griffonian Republic must build asset-specific
  investment so that it does not decay once its founding generation's trauma fades, as the New Deal
  coalition did. Both are written as expository prose with no reference to the scene they hang off.
- seen in: two theme commentaries, on Bottom Up > Top Down and on Conscience
- cites:
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-233-theme-15
  - exploration-of-v1-theme-commentaries-content/01-theme-commentaries/pp-241-theme-26

## Proposed questions

- Of the v1 archive's theme commentaries that hold fabula content, how much of that content is stated
  nowhere else in the archive, so that the theme box is its only home?
- Where a v1 theme commentary restates the plot point's synopsis, did the synopsis or the commentary
  carry the words first?
- Which of the v1 archive's theme commentaries argue a real-world political or economic thesis in the
  author's own person rather than say what the scene does, and on which themes do those fall?
- The theme box's question asks what a reader should conclude, yet only five of 398 passages speak of
  the reader at all: where in the v1 archive is the reader's arrival at a thematic proposition designed,
  if not here?
- How many of the v1 archive's theme commentaries hold a passage that plans what appears on the page,
  and does the plot point's own synopsis hold the same plan?
