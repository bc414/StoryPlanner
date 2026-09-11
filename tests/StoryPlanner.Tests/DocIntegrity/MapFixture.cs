using System;
using System.Collections.Generic;
using System.IO;
using StoryPlanner.BatchFiles;
using StoryPlanner.DocIntegrity;

namespace StoryPlanner.Tests;

/// <summary>
/// A tiny repository on disk holding a skill folder that validates clean: a three-row router
/// (a terminus, an hitl activity that writes hypothesis artifacts from candidates, a session
/// activity that composes candidates), the Artifacts table beside it in SKILL.md, two activity
/// files and one schema file per schema the table names, copied from the real skill folder so
/// the engine reads the real Shapes. Every failing test starts from this and breaks exactly
/// one thing, so a finding can only come from the mutation.
/// <see cref="WithStateTree"/> adds the docs and study files the state derivation reads.
///
/// Small and inline on purpose: the real skill folder is never a fixture for a verdict (see
/// the testing skill), because a test that reads it would fail whenever the method changes.
/// The activity ids are the real ones, since the state builder's chain membership is keyed on
/// them. Candidates is a generated view (no schema), as in the real skill.
/// </summary>
public sealed class MapFixture : IDisposable
{
    public const string SkillFile = "SKILL.md";
    public const string SurfacingFile = "surfacing-candidates.md";
    public const string PromotingFile = "promoting-refereed-candidates.md";
    public const string Study = "verification-of-analysis-corpus-dt-classes";
    public const string Batch = "01-full";
    public const string OpenQuestion = "does-the-dt-class-split";

    public static readonly string[] SchemaIds =
        ["hypothesis-file-schema", "question-entry-schema", "study-registry-schema", "directions-schema", "definition-schema", "index-schema", "findings-schema"];

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
            [SurfacingFile] = Surfacing,
            [PromotingFile] = Promoting,
        };
        if (overrides is not null)
            foreach (var (name, content) in overrides) files[name] = content;

        foreach (var (name, content) in files)
            if (content is not null) File.WriteAllText(Path.Combine(SkillFolder, name), content);

        SchemaExamples.CopyInto(SkillFolder, SchemaIds);
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
        SurfacingFile => Surfacing,
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

    public string StudyDir => Path.Combine(RepoRoot, "docs", "v3-framework", "studies", Study);
    public string BatchDir => Path.Combine(StudyDir, "batches", Batch);

    public const string DirectionsText = """
        ---
        questions: analysis-corpus/does-the-dt-class-split
        ---

        ## What you are given

        One note of the archive, whole.

        ## Classes

        - a: the note shows a
        - b: the note shows b
        - cannot-place: the criteria do not decide it

        ## Criteria

        1. A rule that decides between a and b.

        ## What to produce

        - class: enum
        - why: line, the number of the criterion that decided it

        """;

    /// <summary>The docs and study files the state derivation reads: one verification study with one full batch, one question list, two hypotheses.</summary>
    public MapFixture WithStateTree()
    {
        var docs = Path.Combine(RepoRoot, "docs", "v3-framework");
        Directory.CreateDirectory(Path.Combine(docs, "questions"));
        Directory.CreateDirectory(Path.Combine(docs, "hypotheses"));
        Directory.CreateDirectory(Path.Combine(BatchDir, "items"));
        Directory.CreateDirectory(Path.Combine(BatchDir, "results"));

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

        File.WriteAllText(Path.Combine(docs, "hypotheses", "031-dt-classes.md"), $"""
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
            - evidence | 2026-09-14T15:20 | ({Study}/the-first-finding; directions-1@abc123) [supporting]:
              the finding
              Falsifier: the falsifier

            """);

        File.WriteAllText(Path.Combine(docs, "hypotheses", "032-other.md"), $"""
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
            - evidence | 2026-09-15T10:00 | ({Study}/a-third-finding; directions-1@abc123) [supporting]:
              a finding
              Falsifier: f

            """);

        File.WriteAllText(Path.Combine(StudyDir, "findings.md"), $"""
            # {Study} — findings

            ## Method

            Nothing about links was measured.

            ## Findings

            ### {Study}/the-first-finding
            - question: analysis-corpus/{OpenQuestion}
            - finding: Of the notes, most are class a.
            - cites:
              - {Study}/{Batch} § class
              - {Study}/{Batch}/item-001

            ### {Study}/a-withdrawn-one
            - question: analysis-corpus/an-old-one
            - finding: Something that did not hold.
            - cites:
              - {Study}/{Batch}/item-001
            - withdrawn: 2026-09-22 the item shows otherwise

            """);

        var directions = Path.Combine(StudyDir, "directions-1.md");
        File.WriteAllText(directions, DirectionsText);
        var hash = DirectionsFile.Read(directions).BodyHash;
        File.WriteAllText(Path.Combine(StudyDir, "calibration-2026-09-19.md"), $"""
            # Calibration — directions-1@{hash[..8]} — 2026-09-19

            ## Sample
            six items

            ## Verdict
            Brian: accepted at this hash.

            """);

        // candidates.md is a generated view (no schema, not hand-authored); this stands in for the
        // compose-candidates output, in that shape, so CandidateCounts has statuses to tally.
        File.WriteAllText(Path.Combine(StudyDir, "candidates.md"), $"""
            # {Study} — candidates

            Generated by compose-candidates; read-only and regenerable, never hand-edited.

            ## Diagnostic candidates

            ### the-first-finding → 031-dt-classes

            - verdict: diagnostic supporting
            - status: promoted
            - falsifier: what it would have been
            - finding: Of the notes, most are class a.

            ### another-finding → 031-dt-classes

            - verdict: diagnostic supporting
            - status: pending
            - falsifier: y
            - finding: another

            ## Non-diagnostic claims

            None.

            """);

        File.WriteAllText(Path.Combine(BatchDir, "definition.md"), $"""
            # {Batch} — definition

            - directions: ../../directions-1.md
            - kind: full
            - calibration: ../../calibration-2026-09-19.md
            - model: sonnet

            """);
        File.WriteAllText(Path.Combine(BatchDir, "index.md"), $"""
            # {Batch} — index

            - itemizer: tools/StoryPlanner.TestItemizer, 1
            - corpus: analysis-corpus
            - locator notation: a note id, `note-<id>`

            | item | locator | description |
            |---|---|---|
            | item-001 | note-1 | the first note |

            """);
        File.WriteAllText(Path.Combine(BatchDir, "items", "item-001.md"), "# item\n");
        File.WriteAllText(Path.Combine(BatchDir, "results", "item-001.md"), "- class: a\n- why: 1\n");
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
        | candidates | docs/v3-framework/studies/<study>/candidates.md | in-place | | The generated candidates view |
        | directions | docs/v3-framework/studies/<study>/directions-N.md | succeeded | [directions-schema](schemas/directions-schema.md) | The system prompt of a batch's calls |
        | calibration | docs/v3-framework/studies/<study>/calibration-<date>.md | frozen | | One version's agreement |
        | findings | docs/v3-framework/studies/<study>/findings.md | append | [findings-schema](schemas/findings-schema.md) | One verification's findings |
        | definition | docs/v3-framework/<container>/<study>/batches/<batch>/definition.md | frozen | [definition-schema](schemas/definition-schema.md) | What a batch runs under |
        | index | docs/v3-framework/<container>/<study>/batches/<batch>/index.md | frozen | [index-schema](schemas/index-schema.md) | The items a batch judges |
        | items | docs/v3-framework/<container>/<study>/batches/<batch>/items/ | frozen | | The item bodies |
        | results | docs/v3-framework/<container>/<study>/batches/<batch>/results/ | frozen | | The model's answers as rendered |

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
        | promoting-refereed-candidates | changing-the-planner-for-v3 | Brian decides the refereed candidates |
        | surfacing-candidates | promoting-refereed-candidates | A blind agent classifies each candidate |

        """ + ArtifactsSection + """
        ## Companions

        schemas/ holds one file per schema the Artifacts table links to; map.md and state.md are generated only.
        """;

    public const string Surfacing = """
        # surfacing-candidates

        Enables promoting-refereed-candidates.

        | id | mode | instruments | reads | writes | state | description |
        |---|---|---|---|---|---|---|
        | assemble-referee-batch | session | runner | studies calibration directions | definition index items | specified | The batch assembled and handed off |
        | assess-referee-items | agent | | directions items | results | specified | Writes the falsifier blind |
        | compose-candidates | session | | definition index results | candidates | specified | Composes the generated candidates view |

        ## Preconditions

        The referee's directions are calibrated.

        ## assemble-referee-batch

        Assembles the batch.

        ## assess-referee-items

        Assesses one item.

        ## compose-candidates

        Composes the view.

        ## Never

        Gives the referee a source.
        """;

    public const string Promoting = """
        # promoting-refereed-candidates

        Enables changing-the-planner-for-v3.

        | id | mode | instruments | reads | writes | state | description |
        |---|---|---|---|---|---|---|
        | promote | hitl | git | candidates findings hypothesis-record hypothesis-status question-list | hypothesis-record hypothesis-status question-list | specified | Brian decides each candidate |

        ## Preconditions

        Every candidate carries a referee line.

        ## promote

        Brian decides.

        ## Never

        Promotes what Brian did not decide.
        """;
}
