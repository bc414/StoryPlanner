using System.Linq;
using System.Text.Json.Nodes;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The engine that holds a Shape (schemas/skill-schema.md § A schema file, "the last level"):
/// the grammar is held here and by no document. The Shape reader's binding of field tables
/// to sections, the type vocabulary and its fragments, the document mapping, the compiled
/// JSON Schema and the standard validator's findings named by section and key. Tier: pure.
/// </summary>
public class EngineTests
{
    const string TwoTables = """
        | section | present | holds |
        |---|---|---|
        | frontmatter, between `---` lines | required | fields |
        | `## Given` | required | prose |
        | `## Classes` | optional | entries, one line per class |
        | `## Produce` | required | fields |

        **Frontmatter**:

        | key | present | type | value |
        |---|---|---|---|
        | `questions` | optional | list of token of question-list | the questions |

        **Produce**:

        | key | present | type | value |
        |---|---|---|---|
        | `<field>` | required, one or more | line | the kind |
        """;

    [Fact]
    public void Field_tables_are_claimed_by_fields_sections_first_and_an_entries_section_without_one_holds_line_entries()
    {
        var shape = ShapeReader.Parse(TwoTables);
        Assert.Empty(shape.Problems);
        Assert.Equal(["frontmatter", "Given", "Classes", "Produce"], shape.Sections.Select(s => s.Name));
        Assert.Equal([SectionKind.Frontmatter, SectionKind.Heading, SectionKind.Heading, SectionKind.Heading], shape.Sections.Select(s => s.Kind));
        Assert.NotNull(shape.Sections[0].Fields);
        Assert.Null(shape.Sections[2].Fields);
        Assert.NotNull(shape.Sections[3].Fields);
        Assert.True(shape.Sections[3].Fields!.Fields[0].Wildcard);
        Assert.True(shape.Sections[3].Fields!.Fields[0].Multiple);
        Assert.False(shape.Sections[2].Required);
    }

    [Theory]
    [InlineData("slug", TypeName.Slug, null)]
    [InlineData("token of question-list", TypeName.Token, "question-list")]
    [InlineData("id of hypothesis-file", TypeName.Id, "hypothesis-file")]
    [InlineData("path to directions", TypeName.PathTo, "directions")]
    [InlineData("path", TypeName.Path, null)]
    [InlineData("date", TypeName.Date, null)]
    [InlineData("timestamp", TypeName.Timestamp, null)]
    [InlineData("hash", TypeName.Hash, null)]
    [InlineData("line", TypeName.Line, null)]
    [InlineData("block", TypeName.Block, null)]
    public void Every_type_in_the_vocabulary_parses_and_compiles_to_its_fragment(string cell, TypeName name, string? target)
    {
        var t = TypeSpec.Parse(cell);
        Assert.NotNull(t);
        Assert.Equal(name, t!.Name);
        Assert.Equal(target, t.TargetClass);
        Assert.NotNull(ShapeEngine.Fragment(t));
    }

    [Fact]
    public void Enums_close_over_their_backticked_literals_and_are_open_when_the_cell_names_a_set_outside_the_schema()
    {
        var closed = TypeSpec.Parse("enum: `sample`, `full`")!;
        Assert.Equal(["sample", "full"], closed.EnumLiterals);
        Assert.False(closed.EnumOpen);
        var open = TypeSpec.Parse("enum: the ids in CORPORA.md, `candidates`, `skill`")!;
        Assert.True(open.EnumOpen);
        Assert.Equal(["candidates", "skill"], open.EnumLiterals);
        Assert.Equal(["candidates", "skill", "v1-archive"], ShapeEngine.Fragment(open, openEnum: ["v1-archive"])["enum"]!.AsArray().Select(n => n!.GetValue<string>()));
    }

    [Fact]
    public void Lists_nest_one_level_and_a_word_outside_the_vocabulary_is_no_type()
    {
        var list = TypeSpec.Parse("list of token of question-list")!;
        Assert.Equal(TypeName.List, list.Name);
        Assert.Equal(TypeName.Token, list.Item!.Name);
        Assert.True(list.IsInlineList);
        Assert.False(TypeSpec.Parse("list of line")!.IsInlineList);
        Assert.Null(TypeSpec.Parse("label"));
        Assert.Null(TypeSpec.Parse("list of list of line"));
        Assert.Null(TypeSpec.Parse("conditional"));
    }

    [Fact]
    public void A_shape_outside_the_grammar_reports_each_problem()
    {
        var shape = ShapeReader.Parse("""
            | section | present | holds |
            |---|---|---|
            | `## A` | sometimes | fields |
            | `## B` | required | bullets |

            | key | present | type | value |
            |---|---|---|---|
            | `x` | required | label | y |
            """);
        Assert.Contains(shape.Problems, p => p.Contains("present"));
        Assert.Contains(shape.Problems, p => p.Contains("holds"));
        Assert.Contains(shape.Problems, p => p.Contains("outside the vocabulary"));
        Assert.Empty(ShapeReader.Parse("no tables here").Sections);
    }

    /// <summary>
    /// The two silences found at the 2026-09-09 review of the run: a field table no section
    /// claims passed and was never used, and an entries section whose table was forgotten
    /// silently became one-line entries. Both are now the schema's problem, and a `###` entry
    /// under a one-line section is the file's.
    /// </summary>
    [Fact]
    public void An_unclaimed_table_and_a_fixture_with_headed_entries_under_a_tableless_section_are_problems()
    {
        // One extra table is claimed by the Classes entries section (the grammar working); two leave one over.
        var extra = ShapeReader.Parse(TwoTables + "\n\n" + """
            | key | present | type | value |
            |---|---|---|---|
            | `label` | required | line | a table Classes claims |

            | key | present | type | value |
            |---|---|---|---|
            | `orphan` | required | line | a table for no section |
            """);
        Assert.Single(extra.Problems);
        Assert.Contains("no section claims", extra.Problems[0]);
        Assert.Contains("line ", extra.Problems[0]);

        var shape = ShapeReader.Parse(TwoTables);
        Assert.Empty(ShapeReader.FixtureProblems(shape, "# t\n\n## Classes\n\n- a: one\n- b: two\n"));
        var problems = ShapeReader.FixtureProblems(shape, "# t\n\n## Classes\n\n### a\n\n- x: 1\n");
        Assert.Single(problems);
        Assert.Contains("'Classes'", problems[0]);

        var doc = DocumentReader.Read(shape, "---\nquestions: q/a\n---\n\n## Given\n\ntext\n\n## Classes\n\n### a\n\n- x: 1\n\n## Produce\n\n- f: line, the kind\n");
        Assert.Contains(doc.Problems, p => p.Kind == ProblemKind.Form && p.Section == "Classes" && p.Message.Contains("one-line entries"));
    }

    [Fact]
    public void A_document_maps_to_one_object_by_the_shape_and_the_standard_validator_names_what_fails()
    {
        var shape = ShapeReader.Parse(TwoTables);
        var doc = DocumentReader.Read(shape, """
            ---
            questions: a/b c/d
            ---

            ## Given

            Some prose.

            ## Classes

            - x: one
            - y: two

            ## Produce

            - class: enum
            - why: line, the reason

            """);
        Assert.Empty(doc.Problems);
        Assert.Equal(["a/b", "c/d"], doc.Root["frontmatter"]!["questions"]!.AsArray().Select(n => n!.GetValue<string>()));
        Assert.Equal("Some prose.", doc.Root["Given"]!.GetValue<string>());
        Assert.Equal(["x: one", "y: two"], doc.Root["Classes"]!.AsArray().Select(n => n!.GetValue<string>()));
        Assert.Equal("line, the reason", doc.Root["Produce"]!["why"]!.GetValue<string>());
        Assert.Empty(ShapeEngine.Validate(shape, doc));

        var bad = DocumentReader.Read(shape, "---\nquestions: not-a-token\n---\n\n## Given\n\nx\n\n## Produce\n\n- class: enum\n");
        var problems = ShapeEngine.Validate(shape, bad);
        var p = Assert.Single(problems);
        Assert.Equal("frontmatter", p.Section);
        Assert.Equal("questions", p.Key);
        Assert.Equal(ProblemKind.Type, p.Kind);
    }

    [Fact]
    public void The_document_reader_reports_missing_unknown_out_of_order_and_stray_lines_by_section_and_key()
    {
        var shape = ShapeReader.Parse("""
            | section | present | holds |
            |---|---|---|
            | the whole file after the title | required | fields |

            | key | present | type | value |
            |---|---|---|---|
            | `a` | required | line | x |
            | `b` | optional | date | x |
            | `c` | required | list of slug | x |
            """);
        var doc = DocumentReader.Read(shape, "# t\n\n- c: one two\n- a: v\nstray\n- z: q\n- b: 2026-09-09\n  more\n");
        Assert.Contains(doc.Problems, p => p.Kind == ProblemKind.Order);
        Assert.Contains(doc.Problems, p => p.Kind == ProblemKind.Stray);
        Assert.Contains(doc.Problems, p => p.Kind == ProblemKind.Unknown && p.Key == "z");
        Assert.Contains(doc.Problems, p => p.Kind == ProblemKind.Form && p.Key == "b");
        Assert.Equal(["one", "two"], doc.Root["body"]!["c"]!.AsArray().Select(n => n!.GetValue<string>()));
        Assert.Equal("t", doc.Title);

        var missing = DocumentReader.Read(shape, "# t\n\n- a: v\n");
        Assert.Contains(missing.Problems, p => p.Kind == ProblemKind.Missing && p.Key == "c");
    }

    [Fact]
    public void Entries_with_a_field_table_are_an_array_of_objects_headed_by_their_heading_and_a_repeated_section_is_an_array_of_sections()
    {
        var shape = ShapeReader.Parse("""
            | section | present | holds |
            |---|---|---|
            | the head, between the title and the first section | required | prose |
            | `## Revision N`, N increasing | required, one or more | entries, after at most one paragraph of prose |

            | key | present | type | value |
            |---|---|---|---|
            | `id` | required | slug | x |
            | `date` | required | date | x |
            """);
        var doc = DocumentReader.Read(shape, "# Decisions\n\nhead prose\n\n## Revision 1\n\nlead\n\n### first\n\n- id: d-1\n- date: 2026-09-01\n\n## Revision 2\n\n### second\n\n- id: d-2\n- date: 2026-09-02\n");
        Assert.Empty(doc.Problems);
        var sections = doc.Root["Revision N"]!.AsArray();
        Assert.Equal(2, sections.Count);
        Assert.Equal("Revision 1", sections[0]!["###"]!.GetValue<string>());
        var first = sections[0]!["Revision N"]!.AsArray()[0]!;
        Assert.Equal("first", first["###"]!.GetValue<string>());
        Assert.Equal("d-1", first["id"]!.GetValue<string>());
        Assert.Equal(["lead", ""], doc.LeadProse["Revision 1"].Take(2));
        Assert.False(doc.LeadProse.ContainsKey("Revision 2"));
        Assert.Equal(2, doc.Entries.Count);
        Assert.Empty(ShapeEngine.Validate(shape, doc));
    }

    [Fact]
    public void A_table_section_is_an_array_of_rows_keyed_by_its_columns()
    {
        var shape = ShapeReader.Parse("""
            | section | present | holds |
            |---|---|---|
            | the head, after the title | required | fields |
            | the table, after the head | required | table |

            | key | present | type | value |
            |---|---|---|---|
            | `corpus` | required | line | x |

            | column | present | type | value |
            |---|---|---|---|
            | `item` | required | slug | x |
            | `locator` | required | line | x |
            """);
        var doc = DocumentReader.Read(shape, "# 01-x — index\n\n- corpus: v1\n\n| item | locator |\n|---|---|\n| a | note-1 |\n| B | note-2 |\n");
        Assert.Empty(doc.Problems);
        var rows = doc.Root["table"]!.AsArray();
        Assert.Equal(2, rows.Count);
        Assert.Equal("note-2", rows[1]!["locator"]!.GetValue<string>());
        var problems = ShapeEngine.Validate(shape, doc);
        var p = Assert.Single(problems);
        Assert.Equal("table", p.Section);
        Assert.Equal("item", p.Key);
    }

    [Fact]
    public void The_compiled_schema_substitutes_the_fragments_and_closes_every_object()
    {
        var shape = ShapeReader.Parse(TwoTables);
        var schema = ShapeEngine.Compile(shape);
        Assert.Equal("object", schema["type"]!.GetValue<string>());
        var frontmatter = schema["properties"]!["frontmatter"]!;
        Assert.Equal(false, frontmatter["additionalProperties"]!.GetValue<bool>());
        Assert.Equal("^[a-z0-9-]+/[a-z0-9-]+$", frontmatter["properties"]!["questions"]!["items"]!["pattern"]!.GetValue<string>());
        var produce = schema["properties"]!["Produce"]!;
        Assert.NotNull(produce["patternProperties"]);
        Assert.Equal(1, produce["minProperties"]!.GetValue<int>());
        Assert.Equal(["frontmatter", "Given", "Produce"], schema["required"]!.AsArray().Select(n => n!.GetValue<string>()));
    }

    [Fact]
    public void Every_landed_schema_with_a_sections_table_reads_in_the_grammar()
    {
        foreach (var id in new[] { "decisions-schema", "question-entry-schema", "directions-schema", "index-schema", "definition-schema" })
        {
            var shape = ShapeReader.Read(SchemaExamples.SchemaPath(id));
            Assert.True(shape.Problems.Count == 0, $"{id}: {string.Join("; ", shape.Problems)}");
            Assert.NotEmpty(shape.Sections);
        }
    }
}
