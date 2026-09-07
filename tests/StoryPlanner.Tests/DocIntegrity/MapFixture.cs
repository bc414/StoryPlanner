using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using StoryPlanner.DocIntegrity;

namespace StoryPlanner.Tests;

/// <summary>
/// A tiny repository on disk holding a skill folder that validates clean: a three-row router
/// (a terminus, an hitl activity that writes hypothesis artifacts from candidates, a session
/// activity that writes candidates), two activity files and an Artifacts table. Every failing
/// test starts from this and breaks exactly one thing, so a finding can only come from the
/// mutation. <see cref="WithStateTree"/> adds the docs and fanout files the <c>state</c> verb
/// reads.
///
/// Small and inline on purpose: the real skill folder is never a fixture (see the testing
/// skill), because a test that reads it would fail whenever the method changes. The activity
/// ids are the real ones, since the state builder's chain membership is keyed on them.
/// </summary>
public sealed class MapFixture : IDisposable
{
    public const string SkillFile = "SKILL.md";
    public const string ArtifactsFile = "artifacts.md";
    public const string RefereeingFile = "refereeing-a-candidate.md";
    public const string PromotingFile = "promoting-checked-candidates.md";
    public const string Instance = "round-of-analysis-corpus-1";
    public const string OpenQuestion = "Does the DT class split?";

    public string RepoRoot { get; }
    public string SkillFolder { get; }

    public MapFixture(IReadOnlyDictionary<string, string?>? overrides = null)
    {
        RepoRoot = Path.Combine(Path.GetTempPath(), "processmap-" + Guid.NewGuid().ToString("N"));
        SkillFolder = Path.Combine(RepoRoot, ".claude", "skills", "example");
        Directory.CreateDirectory(SkillFolder);

        var files = new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            [SkillFile] = Skill,
            [ArtifactsFile] = Artifacts,
            [RefereeingFile] = Refereeing,
            [PromotingFile] = Promoting,
        };
        if (overrides is not null)
            foreach (var (name, content) in overrides) files[name] = content;

        foreach (var (name, content) in files)
            if (content is not null) File.WriteAllText(Path.Combine(SkillFolder, name), content);
    }

    /// <summary>One file with one substring replaced — the shape of every failing case.</summary>
    public static MapFixture With(string file, string find, string replace)
    {
        var source = Default(file);
        if (!source.Contains(find, StringComparison.Ordinal))
            throw new InvalidOperationException($"fixture file {file} has no '{find}'");
        return new MapFixture(new Dictionary<string, string?> { [file] = source.Replace(find, replace) });
    }

    public static MapFixture Without(string file)
        => new(new Dictionary<string, string?> { [file] = null });

    public static MapFixture WithExtra(string file, string content)
        => new(new Dictionary<string, string?> { [file] = content });

    public static string Default(string file) => file switch
    {
        SkillFile => Skill,
        ArtifactsFile => Artifacts,
        RefereeingFile => Refereeing,
        PromotingFile => Promoting,
        _ => throw new InvalidOperationException($"no default fixture file '{file}'"),
    };

    public string Read(string file) => File.ReadAllText(Path.Combine(SkillFolder, file));

    public SkillDocument Doc => SkillReader.Read(SkillFolder);

    public ValidationReport Report => Validator.Validate(SkillFolder);

    /// <summary>The docs and fanout files <c>state</c> reads: one verification instance, one question list, two hypotheses.</summary>
    public MapFixture WithStateTree()
    {
        var docs = Path.Combine(RepoRoot, "docs", "v3-framework");
        var fanout = Path.Combine(RepoRoot, "fanout", Instance);
        Directory.CreateDirectory(Path.Combine(docs, "questions"));
        Directory.CreateDirectory(Path.Combine(docs, "hypotheses"));
        Directory.CreateDirectory(Path.Combine(docs, Instance));
        Directory.CreateDirectory(Path.Combine(fanout, "2026-09-20", "items"));
        Directory.CreateDirectory(Path.Combine(fanout, "2026-09-20", "results"));

        File.WriteAllText(Path.Combine(docs, "instances.md"), $"""
            | id | type | corpus | go |
            |---|---|---|---|
            | {Instance} | verification | analysis-corpus | 2026-09-20 |

            """);

        File.WriteAllText(Path.Combine(docs, "questions", "analysis-corpus.md"), $"""
            # Questions — analysis-corpus

            ### {OpenQuestion}
            - asked-by: ad hoc (2026-09-10)
            - hypotheses: 031
            - question: Does it?
            - predicate: per item, the class
            - status: open

            ### An old one
            - asked-by: ad hoc (2026-09-01)
            - hypotheses: 032
            - question: Old?
            - predicate:
            - status: withdrawn (superseded)

            """);

        File.WriteAllText(Path.Combine(docs, "hypotheses", "031-dt-classes.md"), """
            ---
            id: 31
            status: evidenced
            baselined: false
            created: 2026-09-01
            ---

            ## Hypothesis

            DT has two classes.

            ## Record

            - created | 2026-09-01T10:00: why it exists
            - evidence | 2026-09-14T15:20 | (round-of-analysis-corpus-1 C-001; codebook-1@abc) [supporting]:
              the finding
              Falsifier: the falsifier

            """);

        File.WriteAllText(Path.Combine(docs, "hypotheses", "032-other.md"), """
            ---
            id: 32
            status: untested
            baselined: false
            created: 2026-09-02
            ---

            ## Hypothesis

            Something else.

            ## Record

            - created | 2026-09-02T10:00: why
            - evidence | 2026-09-15T10:00 | (round-of-analysis-corpus-1 C-003; codebook-1@abc) [supporting]:
              a finding
              Falsifier: f

            """);

        File.WriteAllText(Path.Combine(docs, Instance, "round.md"), $"""
            # {Instance}

            ## Method
            the method

            ## Questions answered
            - {OpenQuestion}

            ## Counts
            the counts

            """);

        var codebook = Path.Combine(fanout, "codebook-1.md");
        File.WriteAllText(codebook, $"""
            # Codebook — dt-classes (version 1)

            ## Item
            one item

            ## Questions
            - {OpenQuestion}

            ## Classes
            a, b

            """);
        var hash = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(codebook))).ToLowerInvariant();
        File.WriteAllText(Path.Combine(fanout, "calibration-2026-09-19.md"), $"""
            # Calibration — codebook-1@{hash[..8]} — 2026-09-19

            ## Sample
            six items

            ## Verdict
            Brian: accepted at this hash.

            """);

        File.WriteAllText(Path.Combine(fanout, "candidates.md"), """
            # candidates

            ## C-001
            - target: 031
            - finding: the finding
            - source: the source
            - proposed-by: job-1 / sonnet / 2026-09-20T12:00 / codebook-1@abc / harness 1
            - falsifier: what it would have been
            - referee: job-9 / sonnet / 2026-09-20T13:00 / referee-1@def / diagnostic supporting — reason
            - outcome: promoted 2026-09-21T10:00 as evidence entry 2026-09-14T15:20

            ## C-002
            - target: 031
            - finding: another
            - source: the source
            - proposed-by: job-2 / sonnet / 2026-09-20T12:00 / codebook-1@abc / harness 1
            - falsifier: y
            - referee: job-10 / sonnet / 2026-09-20T13:00 / referee-1@def / non-diagnostic — reason

            """);

        File.WriteAllText(Path.Combine(fanout, "2026-09-20", "items", "item-001.md"), "# item\n");
        File.WriteAllText(Path.Combine(fanout, "2026-09-20", "results", "item-001.md"), "# result\n");
        return this;
    }

    public string TreePath(params string[] parts)
        => Path.Combine(RepoRoot, Path.Combine(parts));

    public void Dispose()
    {
        try { Directory.Delete(RepoRoot, recursive: true); }
        catch (IOException) { /* a temp dir the OS still holds is not a test failure */ }
    }

    public const string Skill = """
        ---
        name: example
        description: An example skill for the process map tests.
        ---

        # Example skill

        ## Router

        | id | enables | description |
        |---|---|---|
        | changing-the-planner-for-v3 | | The terminus: out of scope, owns no processes |
        | promoting-checked-candidates | changing-the-planner-for-v3 | Brian decides the referee-checked candidates |
        | refereeing-a-candidate | promoting-checked-candidates | A blind agent classifies each candidate |

        ## Companions

        artifacts.md holds the Artifacts table; map.md and state.md are generated only.
        """;

    public const string Refereeing = """
        # refereeing-a-candidate

        Enables promoting-checked-candidates.

        | id | mode | instruments | reads | writes | state | description |
        |---|---|---|---|---|---|---|
        | referee-run | session | runner | instances calibration-record codebook candidates | items | specified | The batch under the host |
        | referee-judge | agent | | codebook items | results | specified | Writes the falsifier blind |
        | referee-append | session | | results candidates | candidates | specified | Copies each verdict under its candidate |

        ## Preconditions

        The codebook is calibrated.

        ## referee-run

        Runs the batch.

        ## referee-judge

        Judges one item.

        ## referee-append

        Appends the lines.

        ## Never

        Gives the referee a source.
        """;

    public const string Promoting = """
        # promoting-checked-candidates

        Enables changing-the-planner-for-v3.

        | id | mode | instruments | reads | writes | state | description |
        |---|---|---|---|---|---|---|
        | promote | hitl | git | candidates hypothesis-record hypothesis-status question-list verification-artifact | hypothesis-record hypothesis-status candidates question-list verification-artifact | specified | Brian decides each candidate |

        ## Preconditions

        Every candidate carries a referee line.

        ## promote

        Brian decides.

        ## Never

        Promotes what Brian did not decide.
        """;

    public const string Artifacts = """
        # Artifacts

        | id | path | mutation | format | description |
        |---|---|---|---|---|
        | hypothesis-record | docs/v3-framework/hypotheses/NNN-slug.md § Record | append | Hypothesis file | The evidence relationship |
        | hypothesis-status | docs/v3-framework/hypotheses/NNN-slug.md frontmatter | in-place | Hypothesis file | Status and baselined |
        | question-list | docs/v3-framework/questions/<corpus>.md | append | Question entry | Brian's open questions |
        | instances | docs/v3-framework/instances.md | append | Instance registry | One row per instance |
        | candidates | fanout/<instance>/candidates.md | append | Candidate | One round's findings |
        | codebook | fanout/<instance>/codebook-N.md | succeeded | Codebook | The frozen instrument |
        | calibration-record | fanout/<instance>/calibration-<date>.md | frozen | | One version's agreement |
        | verification-artifact | docs/v3-framework/<instance>/round.md | append | | One round's method and counts |
        | items | fanout/<instance>/<run>/items/ | frozen | | The units one run judges |
        | results | fanout/<instance>/<run>/results/ | frozen | | The agents' outputs |

        ## Hypothesis file

        The shape.

        ## Question entry

        The shape.

        ## Instance registry

        The shape.

        ## Candidate

        The shape.

        ## Codebook

        The shape.
        """;
}
