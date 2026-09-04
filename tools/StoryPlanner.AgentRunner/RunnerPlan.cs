using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoryPlanner.AgentRunner;

/// <summary>
/// A batch of autonomous-agent jobs, authored as JSON. Every job is launched by
/// <c>claude -p</c> from <see cref="LaunchDir"/> — a folder OUTSIDE the repo, so no CLAUDE.md
/// or skill reaches the agent — with its whole context passed explicitly (instructions +
/// protocol files + input files, inlined and hashed), an exact toolset, and no transcript
/// persisted. The runner is the instrument; the job file is the experiment's authored record.
/// The folder holding the job file is the <b>run folder</b>: relative paths resolve against
/// it, and the ledger and attempt records are written into it, so one folder is one run.
/// </summary>
public sealed record JobFile(
    string LaunchDir,
    int MaxAttempts,
    int UtilizationCap,
    int TimeoutMinutes,
    int MaxParallel,
    JobDefaults? Defaults,
    List<JobSpec> Jobs,
    string? McpConfig)
{
    public int MaxAttempts { get; init; } = MaxAttempts <= 0 ? 2 : MaxAttempts;
    public int UtilizationCap { get; init; } = UtilizationCap <= 0 ? 80 : UtilizationCap;
    /// <summary>Wall-clock cap per attempt; a child past it is killed and the attempt fails.</summary>
    public int TimeoutMinutes { get; init; } = TimeoutMinutes <= 0 ? 20 : TimeoutMinutes;
    /// <summary>Children in flight at once. 1 keeps the original sequential behaviour.</summary>
    public int MaxParallel { get; init; } = MaxParallel <= 0 ? 1 : MaxParallel;
    public List<JobSpec> Jobs { get; init; } = Jobs ?? [];
}


public sealed record JobDefaults(
    string? Model,
    string? Effort,
    string? PermissionMode,
    List<string>? Tools,
    List<string>? AllowedTools,
    List<string>? AddDirs,
    bool? Mcp,
    int? TimeoutMinutes);

public sealed record JobSpec(
    string Id,
    string? Item,
    string OutputPath,
    string? Model,
    string? Effort,
    string? Instructions,
    string? InstructionsFile,
    List<string>? ProtocolFiles,
    List<string>? InputFiles,
    List<string>? RequireOnce,
    string? PermissionMode,
    List<string>? Tools,
    List<string>? AllowedTools,
    List<string>? AddDirs,
    bool? Mcp,
    int? TimeoutMinutes);

/// <summary>A job with the file-level defaults applied and paths made absolute — what the launcher actually runs.</summary>
public sealed record ResolvedJob(
    string Id,
    string Item,
    string Model,
    string? Effort,
    string PermissionMode,
    IReadOnlyList<string> Tools,
    IReadOnlyList<string> AllowedTools,
    IReadOnlyList<string> AddDirs,
    bool Mcp,
    string? Instructions,
    string? InstructionsFile,
    IReadOnlyList<string> ProtocolFiles,
    IReadOnlyList<string> InputFiles,
    IReadOnlyList<string> RequireOnce,
    string OutputPath,
    int TimeoutMinutes);

/// <summary>One attempt, appended to <c>ledger.jsonl</c>. The ledger is the queue state.</summary>
public sealed record LedgerRow(
    string JobId,
    int Attempt,
    string Model,
    string HarnessVersion,
    string PromptSha256,
    Dictionary<string, string> ProtocolShas,
    Dictionary<string, string> InputShas,
    string StartUtc,
    string EndUtc,
    int ExitCode,
    string ResultPath,
    bool OutputExists,
    double? CostUsd,
    int? Turns,
    string? SessionId,
    string? OutputCheck = null,
    string? Mode = null)
{
    /// <summary>"pilot" when the attempt came from a <c>--job</c> enqueue — the mechanical mark of the pilot step; null for a batch launch.</summary>
    public string? Mode { get; init; } = Mode;
    /// <summary>
    /// Exit 0, the output file present, and — when the job named markers — every marker
    /// present exactly once. A row from before <c>OutputCheck</c> existed has null there and
    /// is judged on the first two alone.
    /// </summary>
    public bool Succeeded => ExitCode == 0 && OutputExists && (OutputCheck is null || OutputCheck == RunnerPlan.OutputOk);
}

public enum JobState { Pending, Succeeded, Failed }

public sealed record ComposedPrompt(
    string Text,
    string Sha256,
    Dictionary<string, string> ProtocolShas,
    Dictionary<string, string> InputShas);

public sealed record ResultSummary(double? CostUsd, int? Turns, string? SessionId, string? ResultText);

public static class RunnerPlan
{
    public const string DefaultModel = "sonnet";
    public const string DefaultPermissionMode = "auto";
    public const string OutputOk = "ok";
    /// <summary>A classifier needs to read what it is handed and write one file. Nothing else.</summary>
    public static readonly string[] DefaultTools = ["Read", "Write"];

    private static readonly JsonSerializerOptions Json = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static JobFile ParseJobFile(string json) =>
        JsonSerializer.Deserialize<JobFile>(json, Json) ?? throw new JsonException("job file deserialized to null");

    /// <summary>
    /// Applies defaults and makes every path absolute against <paramref name="runDir"/>, the
    /// folder holding the job file. A job must name its <c>item</c> — the one thing the agent
    /// judges — in a line; a job whose item cannot be named is not a job yet.
    /// </summary>
    public static IReadOnlyList<ResolvedJob> Resolve(JobFile file, string runDir)
    {
        var d = file.Defaults;
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var result = new List<ResolvedJob>();
        foreach (var j in file.Jobs)
        {
            if (string.IsNullOrWhiteSpace(j.Id)) throw new InvalidOperationException("a job has no id");
            if (!seen.Add(j.Id)) throw new InvalidOperationException($"duplicate job id: {j.Id}");
            if (string.IsNullOrWhiteSpace(j.Item)) throw new InvalidOperationException($"job {j.Id} has no item — name the one thing the agent judges");
            if (string.IsNullOrWhiteSpace(j.OutputPath)) throw new InvalidOperationException($"job {j.Id} has no outputPath");
            if (string.IsNullOrWhiteSpace(j.Instructions) && string.IsNullOrWhiteSpace(j.InstructionsFile))
                throw new InvalidOperationException($"job {j.Id} needs instructions or instructionsFile");

            var tools = j.Tools ?? d?.Tools ?? DefaultTools.ToList();
            result.Add(new ResolvedJob(
                Id: j.Id,
                Item: j.Item.Trim(),
                Model: j.Model ?? d?.Model ?? DefaultModel,
                Effort: j.Effort ?? d?.Effort,
                PermissionMode: j.PermissionMode ?? d?.PermissionMode ?? DefaultPermissionMode,
                Tools: tools,
                // Auto-permit exactly the toolset unless the job says otherwise.
                AllowedTools: j.AllowedTools ?? d?.AllowedTools ?? tools,
                AddDirs: (j.AddDirs ?? d?.AddDirs ?? []).Select(p => Abs(p, runDir)).ToList(),
                Mcp: j.Mcp ?? d?.Mcp ?? false,
                Instructions: j.Instructions,
                InstructionsFile: j.InstructionsFile is null ? null : Abs(j.InstructionsFile, runDir),
                ProtocolFiles: (j.ProtocolFiles ?? []).Select(p => Abs(p, runDir)).ToList(),
                InputFiles: (j.InputFiles ?? []).Select(p => Abs(p, runDir)).ToList(),
                RequireOnce: j.RequireOnce ?? [],
                OutputPath: Abs(j.OutputPath, runDir),
                TimeoutMinutes: j.TimeoutMinutes ?? d?.TimeoutMinutes ?? file.TimeoutMinutes));
        }
        return result;
    }

    private static string Abs(string path, string baseDir) =>
        Path.IsPathRooted(path) ? Path.GetFullPath(path) : Path.GetFullPath(path, baseDir);

    public static JobState StateOf(ResolvedJob job, IReadOnlyList<LedgerRow> ledger, int maxAttempts)
    {
        var rows = ledger.Where(r => r.JobId == job.Id).ToList();
        if (rows.Any(r => r.Succeeded)) return JobState.Succeeded;
        return rows.Count >= maxAttempts ? JobState.Failed : JobState.Pending;
    }

    public static int AttemptsOf(ResolvedJob job, IReadOnlyList<LedgerRow> ledger) =>
        ledger.Count(r => r.JobId == job.Id);

    /// <summary>
    /// The first job, in file order, that has neither succeeded nor exhausted its attempts.
    /// A job at <paramref name="maxAttempts"/> is never relaunched — the fix for the
    /// 2026-08-27 incident, when a runner retried the same failing job 9,245 times.
    /// </summary>
    public static ResolvedJob? NextPending(IReadOnlyList<ResolvedJob> jobs, IReadOnlyList<LedgerRow> ledger, int maxAttempts) =>
        jobs.FirstOrDefault(j => StateOf(j, ledger, maxAttempts) == JobState.Pending);

    /// <summary>
    /// The mechanical output contract: every marker in <c>requireOnce</c> appears exactly once
    /// in the output text. Returns <see cref="OutputOk"/> or a one-line reason naming what is
    /// missing or duplicated. No human reads a batch to discover a skipped item.
    /// </summary>
    public static string CheckOutput(string outputText, IReadOnlyList<string> requireOnce)
    {
        if (requireOnce.Count == 0) return OutputOk;
        var missing = new List<string>();
        var duplicated = new List<string>();
        foreach (var marker in requireOnce)
        {
            var n = CountOccurrences(outputText, marker);
            if (n == 0) missing.Add(marker);
            else if (n > 1) duplicated.Add(marker);
        }
        if (missing.Count == 0 && duplicated.Count == 0) return OutputOk;
        var parts = new List<string>();
        if (missing.Count > 0) parts.Add("missing: " + string.Join(", ", missing));
        if (duplicated.Count > 0) parts.Add("duplicated: " + string.Join(", ", duplicated));
        return string.Join("; ", parts);
    }

    private static int CountOccurrences(string text, string marker)
    {
        if (marker.Length == 0) return 0;
        var count = 0;
        var idx = 0;
        while ((idx = text.IndexOf(marker, idx, StringComparison.Ordinal)) >= 0)
        {
            count++;
            idx += marker.Length;
        }
        return count;
    }

    /// <summary>
    /// The agent's entire context, as one document: the job id and item, the instructions,
    /// the output contract, then each protocol file and each input file inlined under a
    /// heading that carries its hash. Deterministic for identical inputs, so the prompt hash
    /// in the ledger is a real provenance key.
    /// </summary>
    public static ComposedPrompt ComposePrompt(ResolvedJob job, Func<string, string> readFile)
    {
        var sb = new StringBuilder();
        sb.Append("# Job: ").Append(job.Id).Append("\n\n");
        sb.Append("Item: ").Append(job.Item).Append("\n\n");

        var instructions = job.Instructions ?? readFile(job.InstructionsFile!);
        sb.Append(instructions.Trim()).Append("\n\n");

        sb.Append("## Output\n\n");
        sb.Append("Write your complete output to exactly this path, creating parent directories if needed, and write nothing else anywhere:\n\n");
        sb.Append("    ").Append(job.OutputPath).Append("\n\n");
        if (job.RequireOnce.Count > 0)
        {
            sb.Append("The output must contain each of the following markers exactly once; an output that omits or repeats one is rejected mechanically:\n\n");
            foreach (var m in job.RequireOnce) sb.Append("- `").Append(m).Append("`\n");
            sb.Append('\n');
        }
        sb.Append("Your context is exactly what follows: the protocol(s) and the input(s). Nothing else applies.\n\n");

        var protocolShas = new Dictionary<string, string>();
        foreach (var path in job.ProtocolFiles)
        {
            var content = readFile(path);
            var sha = Sha256Hex(content);
            var label = Label(path, protocolShas);
            protocolShas[label] = sha;
            sb.Append("## Protocol: ").Append(label).Append(" (sha256 ").Append(sha).Append(")\n\n");
            sb.Append(content.TrimEnd()).Append("\n\n");
        }

        var inputShas = new Dictionary<string, string>();
        foreach (var path in job.InputFiles)
        {
            var content = readFile(path);
            var sha = Sha256Hex(content);
            var label = Label(path, inputShas);
            inputShas[label] = sha;
            sb.Append("## Input: ").Append(label).Append(" (sha256 ").Append(sha).Append(")\n\n");
            sb.Append(content.TrimEnd()).Append("\n\n");
        }

        var text = sb.ToString();
        return new ComposedPrompt(text, Sha256Hex(text), protocolShas, inputShas);
    }

    /// <summary>
    /// The heading and ledger key for an inlined file: its file name, unless an earlier file
    /// in the same list already took that name (two skills are both <c>SKILL.md</c>), in which
    /// case the parent folder is prefixed, and failing that the whole path. Without this the
    /// second file's hash silently overwrote the first's in the ledger, and the agent saw two
    /// identical headings.
    /// </summary>
    private static string Label(string path, IReadOnlyDictionary<string, string> taken)
    {
        var name = Path.GetFileName(path);
        if (!taken.ContainsKey(name)) return name;
        var parent = Path.GetFileName(Path.GetDirectoryName(path) ?? "");
        var two = string.IsNullOrEmpty(parent) ? name : parent + "/" + name;
        return taken.ContainsKey(two) ? path.Replace('\\', '/') : two;
    }

    /// <summary>
    /// The <c>claude</c> argument list. Always: print mode, pinned model, no transcript,
    /// JSON result, restricted mode (no code-running tools, user/project/local settings
    /// ignored), the exact toolset, no skills, strict MCP (drops the user-level connectors).
    /// MCP config only when the job opted in. The output directory is granted via
    /// <c>--add-dir</c> because the launch directory is outside the repo.
    /// </summary>
    public static IReadOnlyList<string> BuildArgs(ResolvedJob job, string? mcpConfigPath)
    {
        var args = new List<string>
        {
            "-p",
            "--model", job.Model,
            "--no-session-persistence",
            // One JSON event per line as it happens (print mode requires --verbose for it), so
            // the runner can tee the inside of a running session to stream.jsonl.
            "--output-format", "stream-json",
            "--verbose",
            "--permission-mode", job.PermissionMode,
            "--restricted",
            "--disable-slash-commands",
            "--strict-mcp-config",
            "--tools",
        };
        // `--tools ""` disables all tools; a non-empty list is the exact available set.
        if (job.Tools.Count == 0) args.Add("");
        else args.AddRange(job.Tools);

        if (!string.IsNullOrWhiteSpace(job.Effort))
        {
            args.Add("--effort");
            args.Add(job.Effort);
        }
        if (job.Mcp)
        {
            if (string.IsNullOrWhiteSpace(mcpConfigPath))
                throw new InvalidOperationException($"job {job.Id} sets mcp but the job file has no mcpConfig");
            args.Add("--mcp-config");
            args.Add(mcpConfigPath);
        }
        if (job.AllowedTools.Count > 0)
        {
            args.Add("--allowed-tools");
            args.AddRange(job.AllowedTools);
        }
        var dirs = new List<string>();
        var outputDir = Path.GetDirectoryName(job.OutputPath);
        if (!string.IsNullOrEmpty(outputDir)) dirs.Add(outputDir);
        dirs.AddRange(job.AddDirs);
        if (dirs.Count > 0)
        {
            args.Add("--add-dir");
            args.AddRange(dirs);
        }
        return args;
    }

    /// <summary>
    /// Cost, turn count, session id and the final reply from the child's output: a
    /// <c>stream-json</c> file (one event per line — the live form), a <c>json</c> array of
    /// events (the pre-streaming form), or a single result object. The <c>result</c> event
    /// carries the totals. Nulls when anything is missing — never throws.
    /// </summary>
    public static ResultSummary ParseResultSummary(string text)
    {
        JsonElement? result = null;
        try
        {
            using var doc = JsonDocument.Parse(text);
            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in doc.RootElement.EnumerateArray())
                    if (IsResultEvent(el)) result = el.Clone();
            }
            else if (doc.RootElement.ValueKind == JsonValueKind.Object)
            {
                result = doc.RootElement.Clone();
            }
        }
        catch (JsonException)
        {
            // Not one document: read it as JSON lines, keeping the last result event.
            foreach (var line in text.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                try
                {
                    using var doc = JsonDocument.Parse(line);
                    if (IsResultEvent(doc.RootElement)) result = doc.RootElement.Clone();
                }
                catch (JsonException) { /* a partial or non-JSON line — skip it */ }
            }
        }
        if (result is null) return new ResultSummary(null, null, null, null);
        var r = result.Value;
        double? cost = r.TryGetProperty("total_cost_usd", out var c) && c.ValueKind == JsonValueKind.Number ? c.GetDouble() : null;
        int? turns = r.TryGetProperty("num_turns", out var n) && n.ValueKind == JsonValueKind.Number ? n.GetInt32() : null;
        string? session = r.TryGetProperty("session_id", out var s) && s.ValueKind == JsonValueKind.String ? s.GetString() : null;
        string? reply = r.TryGetProperty("result", out var rt) && rt.ValueKind == JsonValueKind.String ? rt.GetString() : null;
        return new ResultSummary(cost, turns, session, reply);
    }

    private static bool IsResultEvent(JsonElement el) =>
        el.ValueKind == JsonValueKind.Object && el.TryGetProperty("type", out var t) && t.GetString() == "result";


    public static string Sha256Hex(string text)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
        return Convert.ToHexStringLower(bytes);
    }

    public static string SerializeLedgerRow(LedgerRow row) => JsonSerializer.Serialize(row, Json);

    public static IReadOnlyList<LedgerRow> ParseLedger(IEnumerable<string> lines)
    {
        var rows = new List<LedgerRow>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var row = JsonSerializer.Deserialize<LedgerRow>(line, Json);
            if (row is not null) rows.Add(row);
        }
        return rows;
    }
}
