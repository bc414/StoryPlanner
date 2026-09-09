using System.Text.Json.Nodes;
using StoryPlanner.BatchFiles;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The files of a batch as one library reads and writes them for the runner and the checker
/// alike: the keyed-line grammar, the directions body and its hash, the answer's JSON Schema
/// derived from What to produce, the result rendered from JSON and read back (the render and
/// the parse each other's inverse), the index, the definition, the calls file. Tier: pure.
/// </summary>
public class BatchFilesTests
{
    const string Directions = """
        ---
        questions: v1-archive/a v1-archive/b
        ---

        ## What you are given

        One note.

        ## Classes

        - designed: shows design
        - none: shows nothing
        - cannot-place: the criteria do not decide it

        ## Criteria

        1. Rule one.
        2. Rule two,
           continued.

        ## What to produce

        - class: enum
        - why: line, the criterion
        - notes: block, what stood out
        - quotes: list of line, one passage each

        ## Never

        Guess.

        """;

    [Fact]
    public void Keyed_lines_read_values_continuations_and_list_items_and_render_back()
    {
        var block = KeyedLines.Read(["- a: one", "- b: first", "  second", "- c:", "  - x", "  - y", "stray", "- d:"]);
        Assert.Equal(["a", "b", "c", "d"], block.Fields.Select(f => f.Key));
        Assert.Equal("first\nsecond", block.Field("b")!.BlockText);
        Assert.Equal(["x", "y"], block.Field("c")!.ListItems);
        Assert.Equal("", block.Field("d")!.Value);
        var stray = Assert.Single(block.Stray);
        Assert.Equal(7, stray.Line);
        Assert.Equal("- b: first\n  second", KeyedLines.RenderBlock("b", "first\nsecond"));
        Assert.Equal("- c:\n  - x\n  - y", KeyedLines.RenderList("c", ["x", "y"]));
        Assert.Equal("- a: one", KeyedLines.RenderLine("a", "one"));
    }

    [Fact]
    public void Directions_split_into_frontmatter_and_a_hashed_body_with_classes_criteria_and_declared_fields()
    {
        var d = DirectionsFile.Parse(Directions);
        Assert.Empty(d.Problems);
        Assert.Equal(["v1-archive/a", "v1-archive/b"], d.Questions);
        Assert.StartsWith("## What you are given", d.Body);
        Assert.Equal(64, d.BodyHash.Length);
        Assert.Equal(["What you are given", "Classes", "Criteria", "What to produce", "Never"], d.SectionHeadings);
        Assert.Equal(["designed", "none", "cannot-place"], d.Classes.Select(c => c.Label));
        Assert.Equal(["Rule one.", "Rule two,\ncontinued."], d.Criteria);
        Assert.Equal([OutputKind.Enum, OutputKind.Line, OutputKind.Block, OutputKind.ListOfLine], d.Output.Select(o => o.Kind));
        Assert.Equal("the criterion", d.Output[1].Text);
        Assert.True(d.HasClasses);

        var sameBody = DirectionsFile.Parse(Directions.Replace("v1-archive/b", "v1-archive/c"));
        Assert.Equal(d.BodyHash, sameBody.BodyHash);
        Assert.Equal(d.BodyHash, DirectionsFile.Parse(Directions.Replace("\n", "\r\n")).BodyHash);
        Assert.NotEqual(d.BodyHash, DirectionsFile.Parse(Directions.Replace("Guess.", "Guess!")).BodyHash);
    }

    [Fact]
    public void Directions_problems_name_their_section()
    {
        var d = DirectionsFile.Parse(Directions.Replace("2. Rule two,", "3. Rule two,").Replace("- why: line, the criterion", "- why: label, x").Replace("- none: shows nothing", "- designed: again"));
        Assert.Contains(d.Problems, p => p.Section == "criteria");
        Assert.Contains(d.Problems, p => p.Section == "output");
        Assert.Contains(d.Problems, p => p.Section == "classes");
        Assert.Contains(DirectionsFile.Parse(Directions.Replace("- notes: block, what stood out", "- notes: block")).Problems, p => p.Section == "output");
        Assert.Equal(3, DirectionsFile.VersionOf("directions-3.md"));
        Assert.Null(DirectionsFile.VersionOf("directions.md"));
    }

    [Fact]
    public void The_answers_schema_is_derived_from_what_to_produce_and_the_classes()
    {
        var schema = ResultFile.Schema(DirectionsFile.Parse(Directions));
        Assert.Equal(["designed", "none", "cannot-place"], schema["properties"]!["class"]!["enum"]!.AsArray().Select(n => n!.GetValue<string>()));
        Assert.Equal("^[^\\n]*$", schema["properties"]!["why"]!["pattern"]!.GetValue<string>());
        Assert.Equal("string", schema["properties"]!["notes"]!["type"]!.GetValue<string>());
        Assert.Equal("array", schema["properties"]!["quotes"]!["type"]!.GetValue<string>());
        Assert.Equal(["class", "why", "notes", "quotes"], schema["required"]!.AsArray().Select(n => n!.GetValue<string>()));
        Assert.False(schema["additionalProperties"]!.GetValue<bool>());
    }

    [Fact]
    public void A_result_renders_from_the_models_json_and_parses_back_to_the_same_object()
    {
        var d = DirectionsFile.Parse(Directions);
        var answer = new JsonObject
        {
            ["class"] = "designed",
            ["why"] = "1",
            ["notes"] = "first paragraph\nsecond line",
            ["quotes"] = new JsonArray("one", "two"),
        };
        var text = ResultFile.Render(d, answer);
        Assert.Equal("- class: designed\n- why: 1\n- notes: first paragraph\n  second line\n- quotes:\n  - one\n  - two\n", text);
        var (back, problems) = ResultFile.Parse(d, text);
        Assert.Empty(problems);
        Assert.Equal(answer.ToJsonString(), back.ToJsonString());

        var empty = ResultFile.Render(d, new JsonObject { ["class"] = "none", ["why"] = "2", ["notes"] = "", ["quotes"] = new JsonArray() });
        Assert.Equal("- class: none\n- why: 2\n- notes:\n- quotes:\n", empty);
        var (backEmpty, emptyProblems) = ResultFile.Parse(d, empty);
        Assert.Empty(emptyProblems);
        Assert.Empty(backEmpty["quotes"]!.AsArray());
    }

    [Fact]
    public void A_result_outside_the_declaration_is_malformed()
    {
        var d = DirectionsFile.Parse(Directions);
        Assert.Contains(ResultFile.Parse(d, "- class: other\n- why: 1\n- notes: n\n- quotes:\n").Problems, p => p.Contains("not one of the classes"));
        Assert.Contains(ResultFile.Parse(d, "- why: 1\n- class: designed\n- notes: n\n- quotes:\n").Problems, p => p.Contains("order"));
        Assert.Contains(ResultFile.Parse(d, "- class: designed\n- why: 1\n- notes: n\n").Problems, p => p.Contains("missing"));
        Assert.Contains(ResultFile.Parse(d, "- class: designed\n- why: 1\n- notes: n\n- quotes:\n- extra: x\n").Problems, p => p.Contains("not a declared field"));
        Assert.Contains(ResultFile.Parse(d, "- class: designed\n- why: 1\n  more\n- notes: n\n- quotes:\n").Problems, p => p.Contains("one line"));
    }

    [Fact]
    public void An_index_renders_and_reads_with_its_head_and_rows()
    {
        var text = IndexFile.Render("01-full", "tools/StoryPlanner.X, 1", "v1-archive", "a note id", null, [("note-1", "note-1", "first | note"), ("note-2", "note-2", "second")]);
        var index = IndexFile.Parse(text);
        Assert.Empty(index.Problems);
        Assert.Equal("01-full — index", index.Title);
        Assert.Equal("v1-archive", index.Corpus);
        Assert.Equal(["note-1", "note-2"], index.Rows.Select(r => r.Item));
        Assert.Equal("first | note", index.Rows[0].Description);

        var bad = IndexFile.Parse(text.Replace("| note-2 | note-2 |", "| Note 2 |  |"));
        Assert.Contains(bad.Problems, p => p.Part == "item");
        Assert.Contains(bad.Problems, p => p.Part == "locator");
        Assert.Contains(IndexFile.Parse(text.Replace("- corpus: v1-archive\n", "")).Problems, p => p.Part == "head");
    }

    [Fact]
    public void A_definition_reads_its_fields_and_resolves_its_paths_against_its_folder()
    {
        var root = Path.Combine(Path.GetTempPath(), "sp-def-" + Guid.NewGuid().ToString("N"));
        var batch = Path.Combine(root, "studies", "verification-of-x-y", "batches", "02-full");
        Directory.CreateDirectory(batch);
        try
        {
            var path = Path.Combine(batch, "definition.md");
            File.WriteAllText(path, DefinitionFile.Render("02-full", "../../directions-1.md", "full", "../../calibration-2026-09-01.md", "sonnet", "high", ["Read", "Write"], "../../../../mcp.json"));
            var d = DefinitionFile.Read(path);
            Assert.Empty(d.Problems);
            Assert.Equal("02-full", d.Batch);
            Assert.Equal((2, "full"), d.BatchParts);
            Assert.Equal("verification-of-x-y", d.StudyName);
            Assert.Equal(Path.GetFullPath(Path.Combine(root, "studies", "verification-of-x-y", "directions-1.md")), d.DirectionsPath);
            Assert.Equal("full", d.Kind);
            Assert.Equal("sonnet", d.Model);
            Assert.Equal("high", d.Effort);
            Assert.Equal(["Read", "Write"], d.Tools);
            Assert.EndsWith("mcp.json", d.McpPath);
            Assert.Equal(64, d.Hash.Length);

            File.WriteAllText(path, "# 02-full — definition\n\n- model: sonnet\n- directions: ../../directions-1.md\n- kind: pilot\n- effort: max\n- timeout: 3\n");
            var bad = DefinitionFile.Read(path);
            Assert.Contains(bad.Problems, p => p.Message.Contains("order"));
            Assert.Contains(bad.Problems, p => p.Message.Contains("kind"));
            Assert.Contains(bad.Problems, p => p.Message.Contains("timeout"));
        }
        finally { Directory.Delete(root, true); }
    }

    [Fact]
    public void The_calls_file_appends_entries_and_reads_the_batchs_state_back()
    {
        var head = CallsFile.RenderHead("01-full", "abc");
        var e1 = new CallEntry("note-1", 1, "sonnet", "high", "2.1.258", "d".PadLeft(64, 'd'), "i", "p", "2026-09-09T10:00:00Z", "2026-09-09T10:01:00Z", 0, CallEntry.Ok, 0.0123, 1, "sess", false);
        var e2 = e1 with { Item = "note-2", Exit = 1, Check = "no structured output", Cost = null, Turns = null, SessionId = null, Pilot = true };
        var text = head + CallsFile.RenderEntry(e1) + CallsFile.RenderEntry(e2);
        var calls = CallsFile.Parse(text);
        Assert.Empty(calls.Problems);
        Assert.Equal("01-full — calls", calls.Title);
        Assert.Equal("abc", calls.DefinitionHash);
        Assert.Equal(2, calls.Entries.Count);
        Assert.True(calls.HasSucceeded("note-1"));
        Assert.False(calls.HasSucceeded("note-2"));
        Assert.Equal(0.0123, calls.Entries[0].Cost);
        Assert.True(calls.Entries[1].Pilot);
        Assert.Null(calls.Entries[1].Cost);
        Assert.Equal(1, calls.Executions);
    }

    [Fact]
    public void A_calibration_title_names_the_version_and_hash_and_its_verdict_accepts_or_not()
    {
        var cal = CalibrationFile.Parse("# Calibration — directions-2@abcdef12 — 2026-09-19b\n\n## Verdict\nBrian: accepted at this hash.\n");
        Assert.True(cal.TitleParsed);
        Assert.Equal(2, cal.Version);
        Assert.Equal("abcdef12", cal.Hash);
        Assert.True(cal.Accepted);
        Assert.False(CalibrationFile.Parse("# Calibration — directions-2@abcdef12 — 2026-09-19\n\n## Verdict\nNot accepted; a new version.\n").Accepted);
        Assert.False(CalibrationFile.Parse("# Calibration — codebook-2@abcdef12 — 2026-09-19\n").TitleParsed);
        Assert.True(Hashing.Cites("abcdef12", "abcdef12" + new string('0', 56)));
        Assert.False(Hashing.Cites("abc", "abcdef"));
    }
}
