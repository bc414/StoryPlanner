using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure-tier tests for the agent runner's decision logic: job resolution against defaults
/// and the run folder, the ledger-derived queue (the 2026-08-27 infinite-retry fix), the
/// mechanical output contract, deterministic prompt composition with per-file hashes, the
/// claude argument list, and result parsing.
/// </summary>
public class RunnerPlanTests
{
    private static readonly string Out = Path.Combine("C:", "repo", "docs", "out");

    private static ResolvedJob Job(string id, bool mcp = false, IReadOnlyList<string>? protocols = null, IReadOnlyList<string>? inputs = null, string? effort = null, IReadOnlyList<string>? requireOnce = null) =>
        new(id, "one item", "sonnet", effort, "auto", ["Read", "Write"], ["Read", "Write"], [], mcp, "Do the thing.", null,
            protocols ?? [], inputs ?? [], requireOnce ?? [], Path.Combine(Out, id + ".md"), 20);

    private static LedgerRow Row(string jobId, int attempt, int exit, bool output, string? check = null) =>
        new(jobId, attempt, "sonnet", "2.1.0", "abc", [], [], "t0", "t1", exit, "r.json", output, null, null, null, check);

    private static int IndexOf(IReadOnlyList<string> args, string flag) => Array.IndexOf(args.ToArray(), flag);

    [Fact]
    public void Resolve_applies_file_defaults_and_job_overrides()
    {
        var file = RunnerPlan.ParseJobFile("""
            {
              "launchDir": "X:/fanout",
              "defaults": { "model": "opus", "tools": ["Read"], "mcp": true, "effort": "low" },
              "mcpConfig": "mcp.json",
              "jobs": [
                { "id": "a", "item": "item a", "outputPath": "X:/out/a.md", "instructions": "go" },
                { "id": "b", "item": "item b", "outputPath": "X:/out/b.md", "instructions": "go", "model": "sonnet", "mcp": false, "allowedTools": [], "timeoutMinutes": 5 }
              ]
            }
            """);
        var jobs = RunnerPlan.Resolve(file, "X:/run");

        Assert.Equal(2, file.MaxAttempts);
        Assert.Equal(80, file.UtilizationCap);
        Assert.Equal(20, file.TimeoutMinutes);
        Assert.Equal(1, file.MaxParallel);
        Assert.Equal("opus", jobs[0].Model);
        Assert.Equal("low", jobs[0].Effort);
        Assert.True(jobs[0].Mcp);
        Assert.Equal(["Read"], jobs[0].Tools);
        Assert.Equal(["Read"], jobs[0].AllowedTools);   // auto-permit follows the toolset
        Assert.Equal(20, jobs[0].TimeoutMinutes);
        Assert.Equal("sonnet", jobs[1].Model);
        Assert.False(jobs[1].Mcp);
        Assert.Empty(jobs[1].AllowedTools);              // unless the job says otherwise
        Assert.Equal(5, jobs[1].TimeoutMinutes);
    }

    [Fact]
    public void Resolve_makes_relative_paths_absolute_against_the_run_folder()
    {
        var file = RunnerPlan.ParseJobFile("""
            {
              "launchDir": "X:/fanout",
              "jobs": [ { "id": "a", "item": "unit-001", "outputPath": "results/a.md", "instructions": "go",
                          "protocolFiles": ["../protocol.md"], "inputFiles": ["items/unit-001.md", "X:/abs/b.md"] } ]
            }
            """);
        var run = Path.Combine("X:", "fanout-work", "run-1");
        var job = Assert.Single(RunnerPlan.Resolve(file, run));

        Assert.Equal(Path.Combine(run, "results", "a.md"), job.OutputPath);
        Assert.Equal(Path.Combine("X:", "fanout-work", "protocol.md"), job.ProtocolFiles[0]);
        Assert.Equal(Path.Combine(run, "items", "unit-001.md"), job.InputFiles[0]);
        Assert.Equal(Path.GetFullPath("X:/abs/b.md"), job.InputFiles[1]);
    }

    [Fact]
    public void Resolve_rejects_duplicate_ids_missing_items_and_jobs_without_instructions()
    {
        var dup = RunnerPlan.ParseJobFile("""{ "launchDir": "X:/f", "jobs": [ { "id": "a", "item": "i", "outputPath": "o", "instructions": "x" }, { "id": "a", "item": "i", "outputPath": "o", "instructions": "x" } ] }""");
        Assert.Throws<InvalidOperationException>(() => RunnerPlan.Resolve(dup, "X:/r"));

        var none = RunnerPlan.ParseJobFile("""{ "launchDir": "X:/f", "jobs": [ { "id": "a", "item": "i", "outputPath": "o" } ] }""");
        Assert.Throws<InvalidOperationException>(() => RunnerPlan.Resolve(none, "X:/r"));

        // A job that cannot name its item is not a job yet.
        var noItem = RunnerPlan.ParseJobFile("""{ "launchDir": "X:/f", "jobs": [ { "id": "a", "outputPath": "o", "instructions": "x" } ] }""");
        Assert.Throws<InvalidOperationException>(() => RunnerPlan.Resolve(noItem, "X:/r"));
    }

    [Fact]
    public void A_job_is_pending_until_it_succeeds_or_exhausts_attempts()
    {
        var jobs = new[] { Job("a"), Job("b") };

        Assert.Equal("a", RunnerPlan.NextPending(jobs, [], 2)!.Id);

        // One failed attempt: still pending.
        Assert.Equal("a", RunnerPlan.NextPending(jobs, [Row("a", 1, 1, false)], 2)!.Id);

        // Exit 0 but no output file is NOT success.
        Assert.Equal(JobState.Pending, RunnerPlan.StateOf(jobs[0], [Row("a", 1, 0, false)], 2));

        // Exit 0 with output but a failed marker check is NOT success either.
        Assert.Equal(JobState.Pending, RunnerPlan.StateOf(jobs[0], [Row("a", 1, 0, true, "missing: unit-003")], 2));

        // Two failed attempts at maxAttempts 2: FAILED, and the queue moves on — never relaunched.
        var twoFails = new[] { Row("a", 1, 1, false), Row("a", 2, 1, false) };
        Assert.Equal(JobState.Failed, RunnerPlan.StateOf(jobs[0], twoFails, 2));
        Assert.Equal("b", RunnerPlan.NextPending(jobs, twoFails, 2)!.Id);

        // Success ends it — with the check passed, or (older rows) with no check recorded.
        Assert.Equal(JobState.Succeeded, RunnerPlan.StateOf(jobs[0], [Row("a", 1, 0, true, "ok")], 2));
        Assert.Equal(JobState.Succeeded, RunnerPlan.StateOf(jobs[0], [Row("a", 1, 0, true)], 2));
        var allDone = new[] { Row("a", 1, 0, true, "ok"), Row("b", 1, 0, true, "ok") };
        Assert.Null(RunnerPlan.NextPending(jobs, allDone, 2));
    }

    [Fact]
    public void Output_check_requires_each_marker_exactly_once()
    {
        string[] markers = ["unit-001", "unit-002", "unit-003"];
        Assert.Equal("ok", RunnerPlan.CheckOutput("| unit-001 | .. |\n| unit-002 | .. |\n| unit-003 | .. |", markers));
        Assert.Equal("missing: unit-003", RunnerPlan.CheckOutput("unit-001 unit-002", markers));
        Assert.Equal("duplicated: unit-002", RunnerPlan.CheckOutput("unit-001 unit-002 unit-002 unit-003", markers));
        Assert.Equal("missing: unit-001; duplicated: unit-003", RunnerPlan.CheckOutput("unit-002 unit-003 unit-003", markers));
        Assert.Equal("ok", RunnerPlan.CheckOutput("anything", []));
    }

    [Fact]
    public void Prompt_is_deterministic_and_carries_per_file_hashes_in_its_headings()
    {
        var files = new Dictionary<string, string>
        {
            ["P:/codebooks/rule.md"] = "Classify each item as A or B.\n",
            ["P:/inputs/item-1.md"] = "The item.\n",
        };
        var job = Job("j1", protocols: ["P:/codebooks/rule.md"], inputs: ["P:/inputs/item-1.md"], requireOnce: ["unit-001"]);

        var p1 = RunnerPlan.ComposePrompt(job, path => files[path]);
        var p2 = RunnerPlan.ComposePrompt(job, path => files[path]);

        Assert.Equal(p1.Sha256, p2.Sha256);
        Assert.Equal(p1.Text, p2.Text);
        var ruleSha = RunnerPlan.Sha256Hex(files["P:/codebooks/rule.md"]);
        Assert.Equal(ruleSha, p1.ProtocolShas["rule.md"]);
        Assert.Contains($"## Protocol: rule.md (sha256 {ruleSha})", p1.Text);
        Assert.Contains("## Input: item-1.md (sha256 ", p1.Text);
        Assert.Contains(job.OutputPath, p1.Text);
        Assert.StartsWith("# Job: j1\n\nItem: one item\n", p1.Text);
        Assert.Contains("- `unit-001`", p1.Text);

        files["P:/codebooks/rule.md"] = "Classify each item as A, B or C.\n";
        var p3 = RunnerPlan.ComposePrompt(job, path => files[path]);
        Assert.NotEqual(p1.Sha256, p3.Sha256);
        Assert.NotEqual(p1.ProtocolShas["rule.md"], p3.ProtocolShas["rule.md"]);
    }

    [Fact]
    public void Inputs_sharing_a_file_name_get_distinct_headings_and_ledger_keys()
    {
        var files = new Dictionary<string, string>
        {
            ["P:/skills/old/SKILL.md"] = "Old skill.\n",
            ["P:/skills/new/SKILL.md"] = "New skill.\n",
            ["P:/skills/new/companion.md"] = "Companion.\n",
        };
        var job = Job("j2", inputs: ["P:/skills/old/SKILL.md", "P:/skills/new/SKILL.md", "P:/skills/new/companion.md"]);

        var p = RunnerPlan.ComposePrompt(job, path => files[path]);

        Assert.Equal(3, p.InputShas.Count);
        Assert.Equal(RunnerPlan.Sha256Hex("Old skill.\n"), p.InputShas["SKILL.md"]);
        Assert.Equal(RunnerPlan.Sha256Hex("New skill.\n"), p.InputShas["new/SKILL.md"]);
        Assert.Equal(RunnerPlan.Sha256Hex("Companion.\n"), p.InputShas["companion.md"]);
        Assert.Contains("## Input: SKILL.md (sha256 ", p.Text);
        Assert.Contains("## Input: new/SKILL.md (sha256 ", p.Text);
    }

    [Fact]
    public void Args_pin_model_disable_persistence_restrict_and_name_the_exact_toolset()
    {
        var args = RunnerPlan.BuildArgs(Job("j"), null);

        Assert.Equal("-p", args[0]);
        Assert.Contains("--no-session-persistence", args);
        Assert.Contains("--restricted", args);
        Assert.Contains("--disable-slash-commands", args);
        Assert.Contains("--strict-mcp-config", args);
        Assert.DoesNotContain("--mcp-config", args);
        Assert.DoesNotContain("--effort", args);
        Assert.Equal("sonnet", args[IndexOf(args, "--model") + 1]);
        Assert.Equal("stream-json", args[IndexOf(args, "--output-format") + 1]);
        Assert.Contains("--verbose", args);
        var tools = IndexOf(args, "--tools");
        Assert.Equal("Read", args[tools + 1]);
        Assert.Equal("Write", args[tools + 2]);
        Assert.Equal("--allowed-tools", args[tools + 3]);
        // The output directory is granted because the launch dir is outside the repo.
        Assert.Equal(Out, args[IndexOf(args, "--add-dir") + 1]);
    }

    [Fact]
    public void Empty_toolset_passes_an_empty_tools_argument_and_effort_is_forwarded()
    {
        var job = Job("j", effort: "low") with { Tools = [], AllowedTools = [] };
        var args = RunnerPlan.BuildArgs(job, null);
        Assert.Equal("", args[IndexOf(args, "--tools") + 1]);
        Assert.Equal("low", args[IndexOf(args, "--effort") + 1]);
        Assert.DoesNotContain("--allowed-tools", args);
    }

    [Fact]
    public void Mcp_config_is_passed_only_when_the_job_opts_in()
    {
        var args = RunnerPlan.BuildArgs(Job("j", mcp: true), "C:/cfg/mcp.json");
        Assert.Equal("C:/cfg/mcp.json", args[IndexOf(args, "--mcp-config") + 1]);
        Assert.Contains("--strict-mcp-config", args);

        Assert.Throws<InvalidOperationException>(() => RunnerPlan.BuildArgs(Job("j", mcp: true), null));
    }

    [Fact]
    public void Ledger_rows_round_trip_through_jsonl_and_older_rows_without_a_check_still_parse()
    {
        var row = new LedgerRow("j", 1, "sonnet", "2.1.220", "deadbeef",
            new() { ["rule.md"] = "aa" }, new() { ["item.md"] = "bb" }, "t0", "t1", 0, "r.json", true, 0.067, 2, "sess", "ok");
        var line = RunnerPlan.SerializeLedgerRow(row);
        var back = Assert.Single(RunnerPlan.ParseLedger([line, ""]));
        // Records compare dictionary members by reference; the serialized form is the identity.
        Assert.Equal(line, RunnerPlan.SerializeLedgerRow(back));
        Assert.Equal("aa", back.ProtocolShas["rule.md"]);
        Assert.Equal(0.067, back.CostUsd);
        Assert.Equal(2, back.Turns);
        Assert.True(back.Succeeded);

        const string older = """{"JobId":"s","Attempt":1,"Model":"sonnet","HarnessVersion":"2.1.258","PromptSha256":"c4","ProtocolShas":{},"InputShas":{},"StartUtc":"t","EndUtc":"t","ExitCode":0,"ResultPath":"r","OutputExists":true}""";
        var old = Assert.Single(RunnerPlan.ParseLedger([older]));
        Assert.Null(old.OutputCheck);
        Assert.True(old.Succeeded);
    }

    [Fact]
    public void Result_summary_is_read_from_the_result_event_of_the_json_stream()
    {
        const string stream = """
            [
              {"type":"system","subtype":"init","cwd":"X:/fanout","tools":["Read","Write"],"mcp_servers":[]},
              {"type":"assistant","message":{"role":"assistant","content":[{"type":"text","text":"working"}]}},
              {"type":"result","subtype":"success","total_cost_usd":0.0674,"num_turns":2,"session_id":"55f2","result":"Wrote the file."}
            ]
            """;
        var s = RunnerPlan.ParseResultSummary(stream);
        Assert.Equal(0.0674, s.CostUsd);
        Assert.Equal(2, s.Turns);
        Assert.Equal("55f2", s.SessionId);
        Assert.Equal("Wrote the file.", s.ResultText);

        var single = RunnerPlan.ParseResultSummary("""{"type":"result","total_cost_usd":1.5,"num_turns":1,"session_id":"s","result":"ok"}""");
        Assert.Equal(1.5, single.CostUsd);

        var none = RunnerPlan.ParseResultSummary("not json at all");
        Assert.Null(none.CostUsd);
        Assert.Null(none.SessionId);

        // stream-json: one event per line, as the runner tees it live; the last result event wins.
        const string lines = "{\"type\":\"system\",\"subtype\":\"init\"}\n"
            + "{\"type\":\"assistant\",\"message\":{}}\n"
            + "{\"type\":\"result\",\"total_cost_usd\":0.21,\"num_turns\":2,\"session_id\":\"ab\",\"result\":\"done\"}\n";
        var streamed = RunnerPlan.ParseResultSummary(lines);
        Assert.Equal(0.21, streamed.CostUsd);
        Assert.Equal("ab", streamed.SessionId);
        Assert.Equal("done", streamed.ResultText);
    }
}
