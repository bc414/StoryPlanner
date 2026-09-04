namespace StoryPlanner.AgentRunner;

/// <summary>One job as the page and the JSON routes show it. Same shape for a live and a finished run.</summary>
public sealed record JobSnapshot(
    string Id,
    string Item,
    string Model,
    string State,          // Pending | Running | Succeeded | Failed
    int Attempts,
    int? LastExitCode,
    string? LastCheck,
    double CostUsd,
    string? LastStartUtc,
    string? LastEndUtc,
    double? LastSeconds,
    string? StreamPath);

/// <summary>
/// The stages of the lifecycle (fanout/PROTOCOL.md) a run folder shows evidence of, each
/// detected mechanically from what is on disk — never declared, never judged.
/// </summary>
public sealed record RunStages(
    bool Instrument,       // protocol.md or codebook.md in the run folder or its work folder
    bool? Calibrated,      // codebook only: a calibration-*.md beside it; null when the instrument is a protocol
    bool Enumerated,       // items/manifest.md
    bool Generated,        // jobs.json stamped by a generator
    bool Piloted,          // a ledger row with Mode = pilot
    bool BatchComplete,    // every job succeeded or failed
    bool Tallied,          // tally.* in the run folder or its work folder
    bool Documented);      // run.md

/// <summary>
/// The immutable view of one run that both the Razor components and the JSON routes consume:
/// the state, twice. Built from a run folder on disk, with the live batch's harness state
/// layered on when the host is running it.
/// </summary>
public sealed record RunSnapshot(
    string RunId,
    string RunDir,
    string Work,                       // the folder above the run (or the run itself, for a one-level work)
    bool Live,
    bool Completed,
    bool Paused,
    bool StopRequested,
    int? MaxParallel,                  // the run's ceiling (job file / live)
    int InFlight,
    string? HoldReason,
    IReadOnlyList<JobSnapshot> Jobs,
    int Pending,
    int Succeeded,
    int Failed,
    double CostUsd,
    bool InputsResolvable,
    string? Error,
    string? RunMd,
    RunStages Stages,
    string? LastActivityUtc,
    string? NotBeforeUtc = null,
    bool Scheduled = false);
