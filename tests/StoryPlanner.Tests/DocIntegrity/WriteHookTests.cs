using System.IO;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The PostToolUse write hook, pure tier: payloads are inline JSON in the shape the harness
/// sends, and the governed folder is the <see cref="MapFixture"/>'s skill folder, which sits at
/// <c>.claude/skills/example/</c> under a temp root exactly as a real one does. Assertions are
/// on <see cref="HookOutcomeKind"/> and rule ids, never on message prose.
/// </summary>
public class WriteHookTests
{
    static string EditPayload(string filePath) => $$"""
        {
          "session_id": "abc",
          "transcript_path": "C:/x/transcript.jsonl",
          "cwd": "C:/x",
          "permission_mode": "default",
          "hook_event_name": "PostToolUse",
          "tool_name": "Edit",
          "tool_input": { "file_path": {{Json(filePath)}}, "old_string": "a", "new_string": "b", "replace_all": false },
          "tool_response": { "filePath": {{Json(filePath)}} }
        }
        """;

    static string WritePayload(string filePath) => $$"""
        { "hook_event_name": "PostToolUse", "tool_name": "Write",
          "tool_input": { "file_path": {{Json(filePath)}}, "content": "x" } }
        """;

    const string NotebookPayload = """
        { "hook_event_name": "PostToolUse", "tool_name": "NotebookEdit",
          "tool_input": { "notebook_path": "C:/x/n.ipynb", "new_source": "y" } }
        """;

    static string Json(string s) => System.Text.Json.JsonSerializer.Serialize(s);

    // ---- payload ----

    [Fact]
    public void An_edit_payload_yields_its_file_path()
    {
        var p = HookPayload.Parse(EditPayload("C:/repo/.claude/skills/x/artifacts.md"));
        Assert.Equal("Edit", p.ToolName);
        Assert.Equal("PostToolUse", p.HookEventName);
        Assert.Equal("C:/repo/.claude/skills/x/artifacts.md", p.FilePath);
        Assert.Equal("C:/x", p.Cwd);
    }

    [Fact]
    public void A_write_payload_yields_its_file_path()
        => Assert.Equal("C:/repo/a.md", HookPayload.Parse(WritePayload("C:/repo/a.md")).FilePath);

    [Fact]
    public void A_tool_without_a_file_path_yields_null()
        => Assert.Null(HookPayload.Parse(NotebookPayload).FilePath);

    // ---- scope ----

    [Fact]
    public void A_file_inside_a_skill_folder_with_an_artifacts_table_is_governed()
    {
        using var f = new MapFixture();
        var located = GovernedSkill.Locate(Path.Combine(f.SkillFolder, MapFixture.SkillFile));
        Assert.Equal(Path.GetFullPath(f.SkillFolder), located);
    }

    [Fact]
    public void A_file_outside_any_skill_folder_is_not_governed()
    {
        using var f = new MapFixture();
        Assert.Null(GovernedSkill.Locate(Path.Combine(f.RepoRoot, "docs", "note.md")));
    }

    [Fact]
    public void A_skill_folder_whose_SKILL_md_holds_no_artifacts_table_is_not_governed()
    {
        using var f = MapFixture.With(MapFixture.SkillFile, MapFixture.ArtifactsSection, "");
        Assert.Null(GovernedSkill.Locate(Path.Combine(f.SkillFolder, MapFixture.RefereeingFile)));
    }

    // ---- outcome ----

    [Fact]
    public void A_governed_write_that_checks_clean_is_silent_and_regenerates_both_generated_files()
    {
        using var f = new MapFixture();
        var mapPath = Path.Combine(f.SkillFolder, Render.MapFile);
        var statePath = Path.Combine(f.SkillFolder, Render.StateFile);
        Assert.False(File.Exists(mapPath));
        Assert.False(File.Exists(statePath));

        var outcome = WriteHook.Run(EditPayload(Path.Combine(f.SkillFolder, MapFixture.SkillFile)));
        Assert.Equal(HookOutcomeKind.Silent, outcome.Kind);
        Assert.Equal(WriteHook.Silent, outcome.ExitCode);
        Assert.Equal("", outcome.Message);
        Assert.Equal([mapPath, statePath], outcome.Regenerated);
        Assert.True(File.Exists(mapPath));
        Assert.Contains("## Studies", File.ReadAllText(statePath));
    }

    [Fact]
    public void Regeneration_can_be_withheld_and_then_nothing_is_written()
    {
        using var f = new MapFixture();
        var outcome = WriteHook.Run(EditPayload(Path.Combine(f.SkillFolder, MapFixture.SkillFile)), regenerate: false);
        Assert.Equal(HookOutcomeKind.Silent, outcome.Kind);
        Assert.Empty(outcome.Regenerated);
        Assert.False(File.Exists(Path.Combine(f.SkillFolder, Render.MapFile)));
        Assert.False(File.Exists(Path.Combine(f.SkillFolder, Render.StateFile)));
    }

    [Fact]
    public void A_failing_write_regenerates_nothing()
    {
        using var f = MapFixture.With(MapFixture.SkillFile,
            "| candidates | fanout/<study>/candidates.md |",
            "| items | fanout/<study>/candidates.md |");
        var outcome = WriteHook.Run(EditPayload(Path.Combine(f.SkillFolder, MapFixture.SkillFile)));
        Assert.Equal(HookOutcomeKind.Failed, outcome.Kind);
        Assert.Empty(outcome.Regenerated);
        Assert.False(File.Exists(Path.Combine(f.SkillFolder, Render.MapFile)));
        Assert.False(File.Exists(Path.Combine(f.SkillFolder, Render.StateFile)));
    }

    [Fact]
    public void A_governed_file_write_that_passes_regenerates_the_governing_folder_s_files()
    {
        using var f = new MapFixture().WithStateTree();
        Directory.CreateDirectory(Path.Combine(f.RepoRoot, ".git"));
        var statePath = Path.Combine(f.SkillFolder, Render.StateFile);

        var outcome = WriteHook.Run(EditPayload(f.TreePath("docs", "v3-framework", "hypotheses", "031-dt-classes.md")));
        Assert.Equal(HookOutcomeKind.Silent, outcome.Kind);
        Assert.Contains(statePath, outcome.Regenerated);
        Assert.Contains($"### {MapFixture.Study}", File.ReadAllText(statePath));
    }

    [Fact]
    public void A_governed_file_write_that_fails_regenerates_nothing()
    {
        using var f = new MapFixture().WithStateTree();
        Directory.CreateDirectory(Path.Combine(f.RepoRoot, ".git"));
        var outcome = WriteHook.Run(EditPayload(f.TreePath("docs", "v3-framework", "hypotheses", "032-other.md")));
        Assert.Equal(HookOutcomeKind.Failed, outcome.Kind);
        Assert.Empty(outcome.Regenerated);
        Assert.False(File.Exists(Path.Combine(f.SkillFolder, Render.StateFile)));
    }

    [Fact]
    public void A_governed_write_that_breaks_the_tables_is_feedback_naming_the_rule()
    {
        using var f = MapFixture.With(MapFixture.SkillFile,
            "| candidates | fanout/<study>/candidates.md |",
            "| items | fanout/<study>/candidates.md |");
        var outcome = WriteHook.Run(EditPayload(Path.Combine(f.SkillFolder, MapFixture.SkillFile)));
        Assert.Equal(HookOutcomeKind.Failed, outcome.Kind);
        Assert.Equal(WriteHook.Feedback, outcome.ExitCode);
        var failing = f.Report.Findings.First(x => x.Level == FindingLevel.Failure).CheckId;
        Assert.Contains(failing, outcome.Message);
        Assert.Contains("example", outcome.Message);
    }

    [Fact]
    public void The_fix_instruction_names_the_folder_that_failed_not_a_folder_by_that_name()
    {
        using var f = MapFixture.With(MapFixture.SkillFile,
            "| candidates | fanout/<study>/candidates.md |",
            "| items | fanout/<study>/candidates.md |");
        var outcome = WriteHook.Run(EditPayload(Path.Combine(f.SkillFolder, MapFixture.SkillFile)));
        // The fixture root has no .git, so the folder is named absolutely; a real one is repo-relative.
        Assert.Contains(WriteHook.DisplayPath(f.SkillFolder), outcome.Message);
        Assert.DoesNotContain(".claude/skills/example\n", outcome.Message);
    }

    [Fact]
    public void Display_path_is_repo_relative_inside_a_repository_and_absolute_outside()
    {
        using var f = new MapFixture();
        Assert.Equal(Path.GetFullPath(f.SkillFolder), WriteHook.DisplayPath(f.SkillFolder));
        Directory.CreateDirectory(Path.Combine(f.RepoRoot, ".git"));
        Assert.Equal(".claude/skills/example", WriteHook.DisplayPath(f.SkillFolder));
    }

    [Fact]
    public void A_write_outside_a_governed_folder_is_silent_and_runs_no_validation()
    {
        using var f = new MapFixture();
        var ran = false;
        var outcome = WriteHook.Run(EditPayload(Path.Combine(f.RepoRoot, "docs", "note.md")),
            _ => { ran = true; return f.Report; });
        Assert.Equal(HookOutcomeKind.Silent, outcome.Kind);
        Assert.False(ran);
    }

    [Fact]
    public void A_payload_that_is_not_json_is_feedback_not_silence()
    {
        var outcome = WriteHook.Run("not json");
        Assert.Equal(HookOutcomeKind.PayloadError, outcome.Kind);
        Assert.Equal(WriteHook.Feedback, outcome.ExitCode);
    }

    [Fact]
    public void Report_text_is_the_same_for_the_cli_and_the_hook()
    {
        using var f = MapFixture.With(MapFixture.SkillFile,
            "| candidates | fanout/<study>/candidates.md |",
            "| items | fanout/<study>/candidates.md |");
        var report = f.Report;
        var full = ReportText.Format(report);
        var failures = ReportText.FormatFailures(report);
        foreach (var x in report.Findings.Where(x => x.Level == FindingLevel.Failure))
        {
            Assert.Contains(x.CheckId, full);
            Assert.Contains(x.CheckId, failures);
        }
        Assert.Contains($"check: {report.Failures} failure(s).", full);
    }
}
