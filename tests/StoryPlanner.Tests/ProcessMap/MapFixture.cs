using System;
using System.IO;

namespace StoryPlanner.Tests;

/// <summary>
/// A tiny repository on disk holding a process map that validates clean, plus the documents its
/// cells point at. Every failing test starts from this and breaks exactly one thing, so a
/// finding can only come from the mutation.
///
/// Small and inline on purpose: the real skill folder is never a fixture (see the testing
/// skill), because a test that reads it would fail whenever the method changes.
/// </summary>
public sealed class MapFixture : IDisposable
{
    public string RepoRoot { get; }
    public string SkillFolder { get; }
    public string MapPath => Path.Combine(SkillFolder, "process-map.md");

    public MapFixture(string? map = null, string? skill = null)
    {
        RepoRoot = Path.Combine(Path.GetTempPath(), "processmap-" + Guid.NewGuid().ToString("N"));
        SkillFolder = Path.Combine(RepoRoot, ".claude", "skills", "example");
        Directory.CreateDirectory(SkillFolder);
        Directory.CreateDirectory(Path.Combine(RepoRoot, "docs"));
        Directory.CreateDirectory(Path.Combine(RepoRoot, "fanout", "referee"));

        File.WriteAllText(Path.Combine(RepoRoot, "docs", "proc.md"), ProcDoc);
        File.WriteAllText(Path.Combine(RepoRoot, "docs", "goal.md"), GoalDoc);
        File.WriteAllText(Path.Combine(RepoRoot, "fanout", "referee", "codebook.md"), Codebook);
        File.WriteAllText(Path.Combine(SkillFolder, "SKILL.md"), skill ?? Skill);
        File.WriteAllText(MapPath, map ?? ValidMap);
    }

    /// <summary>The valid map with one substring replaced — the shape of every failing case.</summary>
    public static MapFixture With(string find, string replace)
    {
        var map = ValidMap;
        if (!map.Contains(find)) throw new InvalidOperationException($"fixture has no '{find}'");
        return new MapFixture(map.Replace(find, replace));
    }

    public void Dispose()
    {
        try { Directory.Delete(RepoRoot, recursive: true); }
        catch (IOException) { /* a temp dir the OS still holds is not a test failure */ }
    }

    public const string ValidMap = """
        # Example map

        ## Format (the schema)

        Prose above the tables is copied through untouched.

        ## Roots

        | id | kind | root | source |
        |---|---|---|---|
        | G1 | goal | Everything is findable | docs/goal.md § Goal |
        | C1 | rule | Only Brian promotes | docs/goal.md § Rules ¶ 2 |

        ## Files

        | id | path | keep | governed-by |
        |---|---|---|---|
        | f.a | docs/a.md | committed | docs/proc.md |
        | f.b | docs/b.md | committed | docs/proc.md |
        | f.cand | docs/cand.md | committed | docs/proc.md |
        | f.hyp | docs/hyp.md | committed | docs/proc.md |

        ## Processes

        | id | level | kind | process | actor | inputs | outputs | roots | governed-by | state |
        |---|---|---|---|---|---|---|---|---|---|
        | P.0 | P | sop | Seed the cycle | hitl:fable | f.hyp | f.a | G1 | docs/proc.md | exists |
        | P.1 | P | sop | Transform a into b | script | f.a | f.b | G1 | docs/proc.md | exists |
        | P.2 | V | sop | Propose candidates | agent:sonnet | f.b | f.cand | C1 | docs/proc.md | exists |
        | P.3 | M | sop | Promote | brian | f.cand | f.hyp | C1 | docs/proc.md | exists |

        ## Edges

        | from | to | kind | label |
        |---|---|---|---|
        | P.0 | P.1 | flow | |
        | P.1 | P.2 | choice | when there is something to propose |
        | P.2 | P.3 | flow | |

        ## Bootstrap rows and what retires them

        | row | retired by |
        |---|---|

        ## Generated

        <!-- generated:level-1 -->
        <!-- /generated -->

        <!-- generated:level-2 -->
        <!-- /generated -->

        <!-- generated:level-3 -->
        <!-- /generated -->

        <!-- generated:consumers -->
        <!-- /generated -->

        <!-- generated:validation -->
        <!-- /generated -->
        """;

    const string Skill = """
        ---
        name: example
        description: An example skill for the process map tests.
        ---

        # Example skill

        ## Session routing

        Read process-map.md before acting.
        """;

    const string ProcDoc = """
        # Procedure

        ## What it does

        The procedure.
        """;

    const string GoalDoc = """
        # Goals

        ## Goal

        The goal.

        ## Rules

        1. The first rule.
        2. The second rule.
        """;

    const string Codebook = """
        # Codebook

        ## Decision rules

        - **R1 — Observable, not restatement.** A clause naming no content is vacuous.
        - **R2 — The excerpt decides.** The observable must be in the excerpt.

        ## Worked examples

        **E1 — diagnostic.** A worked example.
        Exercises R1.

        **E2 — non-diagnostic.** Another worked example.
        Exercises R1, R2.
        """;
}
