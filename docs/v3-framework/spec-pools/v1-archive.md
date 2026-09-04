# Spec pool — v1 archive

Questions about the v1 archive corpus awaiting a verification pass. Format and rules:
`README.md` in this directory. Append only; a superseded entry changes its `status` line.

### Do the reading conditions fail at different loci?
- asked-by: methodology revision 1 design conversation (2026-09-02); recorded 2026-09-03
- bears-on: 047
- question: Across the WU1.4 exploratory arms (pathfinder read vs per-arc slice readers, same model), are the loci in the "missed by one" bins disjoint between conditions, or do the two conditions miss the same loci?
- candidate-predicate: per locus, present-in-arm(A) × present-in-arm(B) from the mechanically joined record sets; disjointness = share of missed loci missed by exactly one condition
- status: open

### Is the long-context penalty the same size for every model?
- asked-by: methodology revision 1 design conversation (2026-09-02); recorded 2026-09-03
- bears-on: 050, 010
- question: With WU1.4 run as a factorial (two reading conditions × three models, subset arcs for the slice-reader condition), is the pathfinder-vs-slice gap per model larger in its variation across models than the between-model gap at the slice level?
- candidate-predicate: gap(model) = |records(pathfinder, model) Δ records(slice, model)| on the subset arcs; compare spread of gap(model) across models to spread of records(slice, model) across models
- status: open — scope narrowed 2026-09-03 (Brian): Sonnet is not a pathfinder option, so gap(model) exists for Fable and Opus only; the slice-level comparison still spans all three models

### Does the instruction stack change what a classifier sees?
- asked-by: methodology revision 1 design conversation (2026-09-03); recorded 2026-09-03
- bears-on: 049
- question: On the same v1 note set and the same codebook, does an agent run with CLAUDE.md and the buildout skill in context assign framework-vocabulary labels (e.g. FID where the codebook's DT class applies) at a higher rate than an explicit-context runner job?
- candidate-predicate: two arms, identical items and codebook hash, with/without the instruction stack; label agreement rate and the direction of disagreements
- status: open
