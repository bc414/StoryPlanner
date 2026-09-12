## Hypothesis

A model's long-context penalty — the gap between its pathfinder read and its slice-reader
read of the same corpus — differs across models by more than the between-model gap at the
slice level: context length and model capability interact rather than add.

## Origin

- date: 2026-09-03
- reasoning: Brian ruled, while designing WU1.4's reading-conditions factorial, that both of
  its conditions — the long-context pathfinder read and the slice-reader read — should each
  run under all three candidate models, "so we can do both the takeaways via disagreement
  within model, and test between models." Reviewing that ruling, Claude flagged the
  interaction it would expose as the single most consequential result the design could
  produce: whether a model's long-context penalty holds roughly constant across models or
  instead varies by model, which bears on whether the same model can be trusted for both a
  context-heavy read and a narrower slice read. That observation is what raised this as a
  candidate hypothesis, and Brian approved it for minting alongside four others ("Mint these
  5 hypotheses").

## Record
