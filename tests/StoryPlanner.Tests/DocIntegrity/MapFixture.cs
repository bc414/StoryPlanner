using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using StoryPlanner.DocIntegrity;

namespace StoryPlanner.Tests;

/// <summary>
/// A tiny repository on disk holding a skill folder that validates clean: a three-row router
/// (a terminus, an hitl activity that writes hypothesis artifacts from candidates, a session
/// activity that writes candidates), the Artifacts table beside it in SKILL.md, two activity
/// files and one schema file per schema the table names. Every failing test starts from this
/// and breaks exactly one thing, so a finding can only come from the mutation.
/// <see cref="WithStateTree"/> adds the docs and fanout files the state derivation reads.
///
/// Small and inline on purpose: the real skill folder is never a fixture (see the testing
/// skill), because a test that reads it would fail whenever the method changes. The activity
/// ids are the real ones, since the state builder's chain membership is keyed on them.
/// </summary>
public sealed class MapFixture : IDisposable
{
    public const string SkillFile = "SKILL.md";
    public const string RefereeingFile = "refereeing-a-candidate.md";
    public const string PromotingFile = "promoting-checked-candidates.md";
    public const string Study = "verification-of-analysis-corpus-1";
    public const string OpenQuestion = "does-the-dt-class-split";

    public static readonly string[] SchemaIds =
        ["hypothesis-file-schema", "question-entry-schema", "study-registry-schema", "candidate-schema", "codebook-schema"];

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
            [RefereeingFile] = Refereeing,
            [PromotingFile] = Promoting,
        };
        if (overrides is not null)
            foreach (var (name, content) in overrides) files[name] = content;

        foreach (var (name, content) in files)
            if (content is not null) File.WriteAllText(Path.Combine(SkillFolder, name), content);

        foreach (var id in SchemaIds) WriteSchema(id, $"# {id}\n\nThe shape.\n");
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
        RefereeingFile => Refereeing,
        PromotingFile => Promoting,
        _ => throw new InvalidOperationException($"no default fixture file '{file}'"),
    };

    public string Read(string file) => File.ReadAllText(Path.Combine(SkillFolder, file));

    /// <summary>Writes (or overwrites) one schema file under schemas/.</summary>
    public void WriteSchema(string id, string content)
    {
        var schemas = Path.Combine(SkillFolder, SkillReader.SchemasFolder);
        Directory.CreateDirectory(schemas);
        File.WriteAllText(Path.Combine(schemas, id + ".md"), content);
    }

    public SkillDocument Doc => SkillReader.Read(SkillFolder);

    public ValidationReport Report => Validator.Validate(SkillFolder);

    /// <summary>The docs and fanout files the state derivation reads: one verification study, one question list, two hypotheses.</summary>
    public MapFixture WithStateTree()
    {
        var docs = Path.Combine(RepoRoot, "docs", "v3-framework");
        var fanout = Path.Combine(RepoRoot, "fanout", Study);
        Directory.CreateDirectory(Path.Combine(docs, "questions"));
        Directory.CreateDirectory(Path.Combine(docs, "hypotheses"));
        Directory.CreateDirectory(Path.Combine(docs, Study));
        Directory.CreateDirectory(Path.Combine(fanout, "2026-09-20", "items"));
        Directory.CreateDirectory(Path.Combine(fanout, "2026-09-20", "results"));

        File.WriteAllText(Path.Combine(docs, "studies.md"), $"""
            | id | type | corpus | go |
            |---|---|---|---|
            | {Study} | verification | analysis-corpus | 2026-09-20 |

            """);

        File.WriteAllText(Path.Combine(docs, "questions", "analysis-corpus.md"), $"""
            # analysis-corpus — questions

            ### analysis-corpus/an-old-one

            - date: 2026-09-01
            - hypotheses: 032
            - raised by: recall, ad hoc
            - question: Old?
            - withdrawn: 2026-09-05 superseded

            ### analysis-corpus/{OpenQuestion}

            - date: 2026-09-10
            - hypotheses: 031
            - raised by: recall, ad hoc
            - question: Does it?
            - suggested test: per item, the class

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
            - evidence | 2026-09-14T15:20 | (verification-of-analysis-corpus-1/the-first-finding; codebook-1@abc123) [supporting]:
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
            - evidence | 2026-09-15T10:00 | (verification-of-analysis-corpus-1/a-third-finding; codebook-1@abc123) [supporting]:
              a finding
              Falsifier: f

            """);

        File.WriteAllText(Path.Combine(docs, Study, "verification.md"), $"""
            # {Study}

            ## Method
            the method

            ## Questions answered
            - analysis-corpus/{OpenQuestion}

            ## Counts
            the counts

            """);

        var codebook = Path.Combine(fanout, "codebook-1.md");
        File.WriteAllText(codebook, $"""
            # Codebook — dt-classes (version 1)

            ## Item
            one item

            ## Questions
            - analysis-corpus/{OpenQuestion}

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

        File.WriteAllText(Path.Combine(docs, Study, "candidates.md"), """
            # verification-of-analysis-corpus-1 — candidates

            ### verification-of-analysis-corpus-1/the-first-finding
            - target: 031
            - finding: the finding
            - source: the source
            - proposed-by: job-1 / sonnet / 2026-09-20T12:00 / codebook-1@abc / harness 1
            - falsifier: what it would have been
            - referee: job-9 / sonnet / 2026-09-20T13:00 / referee-1@def / diagnostic supporting — reason
            - outcome: promoted 2026-09-21T10:00 as evidence entry 2026-09-14T15:20

            ### verification-of-analysis-corpus-1/another-finding
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

    /// <summary>The Artifacts table as it sits in SKILL.md; a test removes it to make the folder ungoverned.</summary>
    public const string ArtifactsSection = """
        ## Artifacts

        | id | path | mutation | schema | description |
        |---|---|---|---|---|
        | hypothesis-record | docs/v3-framework/hypotheses/NNN-slug.md § Record | append | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | The evidence relationship |
        | hypothesis-status | docs/v3-framework/hypotheses/NNN-slug.md frontmatter | in-place | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | Status and baselined |
        | question-list | docs/v3-framework/questions/<corpus>.md | append | [question-entry-schema](schemas/question-entry-schema.md) | Brian's open questions |
        | studies | docs/v3-framework/studies.md | append | [study-registry-schema](schemas/study-registry-schema.md) | One row per study |
        | candidates | docs/v3-framework/<study>/candidates.md | append | [candidate-schema](schemas/candidate-schema.md) | One verification's findings |
        | codebook | fanout/<study>/codebook-N.md | succeeded | [codebook-schema](schemas/codebook-schema.md) | The frozen instrument |
        | calibration | fanout/<study>/calibration-<date>.md | frozen | | One version's agreement |
        | verification-artifact | docs/v3-framework/<study>/verification.md | append | | One verification's method and counts |
        | items | fanout/<study>/<run>/items/ | frozen | | The units one run judges |
        | results | fanout/<study>/<run>/results/ | frozen | | The agents' outputs |

        """;

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

        """ + ArtifactsSection + """
        ## Companions

        schemas/ holds one file per schema the Artifacts table links to; map.md and state.md are generated only.
        """;

    public const string Refereeing = """
        # refereeing-a-candidate

        Enables promoting-checked-candidates.

        | id | mode | instruments | reads | writes | state | description |
        |---|---|---|---|---|---|---|
        | referee-run | session | runner | studies calibration codebook candidates | items | specified | The batch under the host |
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
}
