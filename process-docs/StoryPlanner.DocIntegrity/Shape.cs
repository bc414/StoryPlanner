using System.Text.RegularExpressions;

namespace StoryPlanner.DocIntegrity;

/// <summary>The type vocabulary of schemas/skill-schema.md § A schema file, each name defined there by its JSON Schema fragment.</summary>
public enum TypeName { Slug, Token, Id, PathTo, Path, Date, Timestamp, Hash, Enum, Line, Block, List }

/// <summary>
/// One type as a Shape's type cell declares it: the name, the class a reference type names,
/// the literals an enum closes over (with <see cref="EnumOpen"/> when the cell also names a set
/// the class supplies, such as the ids in CORPORA.md), and the item type of a list.
/// </summary>
public sealed record TypeSpec(TypeName Name, string? TargetClass = null, IReadOnlyList<string>? EnumLiterals = null, bool EnumOpen = false, TypeSpec? Item = null)
{
    public bool IsReference => Name is TypeName.Token or TypeName.Id or TypeName.PathTo or TypeName.Path;
    /// <summary>A list whose items sit on the keyed line, space-separated; every other list is one item per bullet beneath the key.</summary>
    public bool IsInlineList => Name == TypeName.List && Item is not null && Item.Name is not (TypeName.Line or TypeName.Block);

    public override string ToString() => Name switch
    {
        TypeName.Token => $"token of {TargetClass}",
        TypeName.Id => $"id of {TargetClass}",
        TypeName.PathTo => $"path to {TargetClass}",
        TypeName.List => $"list of {Item}",
        TypeName.Enum => "enum",
        _ => Name.ToString().ToLowerInvariant(),
    };

    static readonly Regex Backtick = new(@"`([^`]+)`", RegexOptions.Compiled);

    /// <summary>Parses a type cell; null when the cell is outside the vocabulary.</summary>
    public static TypeSpec? Parse(string cell)
    {
        var t = cell.Trim();
        if (t.Length == 0) return null;
        if (t.StartsWith("enum", StringComparison.Ordinal))
        {
            var rest = t.Length > 4 ? t[4..].TrimStart(':').Trim() : "";
            var literals = Backtick.Matches(rest).Select(m => m.Groups[1].Value).ToList();
            var open = rest.Length > 0 && Backtick.Replace(rest, "").Replace(",", "").Trim().Length > 0;
            return new TypeSpec(TypeName.Enum, EnumLiterals: literals, EnumOpen: open);
        }
        if (t.StartsWith("list of ", StringComparison.Ordinal))
        {
            var item = Parse(t["list of ".Length..]);
            return item is null || item.Name == TypeName.List ? null : new TypeSpec(TypeName.List, Item: item);
        }
        if (t.StartsWith("token of ", StringComparison.Ordinal)) return Target(TypeName.Token, t["token of ".Length..]);
        if (t.StartsWith("id of ", StringComparison.Ordinal)) return Target(TypeName.Id, t["id of ".Length..]);
        if (t.StartsWith("path to ", StringComparison.Ordinal)) return Target(TypeName.PathTo, t["path to ".Length..]);
        return t switch
        {
            "slug" => new TypeSpec(TypeName.Slug),
            "path" => new TypeSpec(TypeName.Path),
            "date" => new TypeSpec(TypeName.Date),
            "timestamp" => new TypeSpec(TypeName.Timestamp),
            "hash" => new TypeSpec(TypeName.Hash),
            "line" => new TypeSpec(TypeName.Line),
            "block" => new TypeSpec(TypeName.Block),
            _ => null,
        };
    }

    static TypeSpec? Target(TypeName name, string rest)
    {
        var target = rest.Trim().Trim('`');
        return ClosedSets.IdPattern.IsMatch(target) ? new TypeSpec(name, target) : null;
    }
}

public enum SectionKind { Frontmatter, Head, Table, Body, Heading }

public enum Holds { Fields, Entries, Table, Prose, Fenced }

/// <summary>One field of a field or column table: the key (a wildcard when written in angle brackets), its presence, its type and the writer's value text.</summary>
public sealed record ShapeField(string Key, bool Wildcard, bool Required, bool Multiple, TypeSpec Type, string ValueText, int Line);

public sealed record FieldTable(IReadOnlyList<ShapeField> Fields, bool IsColumns, int Line);

/// <summary>One row of the sections table, with the field or column table it claimed, if any.</summary>
public sealed record ShapeSection(
    string Name,
    SectionKind Kind,
    Regex? HeadingPattern,
    bool Required,
    bool Repeated,
    Holds Holds,
    bool LeadProseAllowed,
    FieldTable? Fields,
    int Line)
{
    /// <summary>The property name the section's value takes in the document object.</summary>
    public string Property => Kind switch
    {
        SectionKind.Frontmatter => "frontmatter",
        SectionKind.Head => "head",
        SectionKind.Table => "table",
        SectionKind.Body => "body",
        _ => Name,
    };

    public bool Matches(string heading) => Kind == SectionKind.Heading && (HeadingPattern?.IsMatch(heading) ?? Name == heading);
}

/// <summary>A schema file's Shape as the engine reads it: its sections in order, each with its field table, and what could not be read.</summary>
public sealed record Shape(IReadOnlyList<ShapeSection> Sections, IReadOnlyList<string> Problems)
{
    public ShapeSection? Section(SectionKind kind) => Sections.FirstOrDefault(s => s.Kind == kind);
    public IEnumerable<ShapeSection> HeadingSections => Sections.Where(s => s.Kind == SectionKind.Heading);
    /// <summary>Every reference type the Shape's fields declare, with the class each names.</summary>
    public IEnumerable<(ShapeSection Section, ShapeField Field, TypeSpec Type)> References()
    {
        foreach (var s in Sections)
        {
            if (s.Fields is null) continue;
            foreach (var f in s.Fields.Fields)
            {
                var t = f.Type.Name == TypeName.List ? f.Type.Item! : f.Type;
                if (t.IsReference) yield return (s, f, t);
            }
        }
    }
}

/// <summary>
/// Reads a schema file's § Shape into a <see cref="Shape"/>: the sections table (columns
/// section, present, holds), then the field tables (key, present, type, value) and column
/// tables (column, …), claimed in order first by the sections that hold fields, then by those
/// that hold entries; an entries section with no table holds one-line entries. This is the
/// one grammar of skill-schema.md, held by the engine's tests; a Shape outside it is the
/// validator's <c>schema.fields</c> finding.
/// </summary>
public static class ShapeReader
{
    static readonly string[] SectionCols = ["section", "present", "holds"];
    static readonly string[] FieldCols = ["key", "present", "type", "value"];
    static readonly string[] ColumnCols = ["column", "present", "type", "value"];
    static readonly Regex HeadingSpan = new(@"`##\s+(?<h>[^`]+)`", RegexOptions.Compiled);

    public static Shape Read(string schemaPath)
    {
        var text = File.ReadAllText(schemaPath);
        var shapeText = StateBuilder.Section(text, "Shape");
        if (shapeText.Trim().Length == 0) return new Shape([], ["no § Shape section"]);
        return Parse(shapeText);
    }

    /// <summary>Whether a Shape declares a sections table at all; one that does not is prose and is not held to the grammar.</summary>
    public static bool HasSectionsTable(string shapeText)
    {
        try { return MapTables.ReadAll(shapeText).Any(t => Sig(t).SequenceEqual(SectionCols)); }
        catch (MapFormatException) { return false; }
    }

    public static Shape Parse(string shapeText)
    {
        var problems = new List<string>();
        IReadOnlyList<MarkdownTable> tables;
        try { tables = MapTables.ReadAll(shapeText); }
        catch (MapFormatException ex) { return new Shape([], [$"a Shape table does not parse: {ex.Message}"]); }

        var sectionsTable = tables.FirstOrDefault(t => Sig(t).SequenceEqual(SectionCols));
        if (sectionsTable is null) return new Shape([], ["no sections table with the columns section, present, holds"]);

        var fieldTables = new List<FieldTable>();
        foreach (var t in tables)
        {
            var sig = Sig(t);
            if (sig.SequenceEqual(FieldCols) || sig.SequenceEqual(ColumnCols))
                fieldTables.Add(ReadFields(t, sig.SequenceEqual(ColumnCols), problems));
            else if (!sig.SequenceEqual(SectionCols))
                problems.Add($"line {t.HeaderLine}: a Shape table with the columns [{string.Join(" | ", t.Headers)}] is none of sections, fields or columns");
        }

        var rows = sectionsTable.Rows.Select(r => (Row: r, Kind: KindOf(r.Cells[0], out var pattern), Pattern: pattern, Holds: HoldsOf(r.Cells[2], problems, r.Line))).ToList();
        var claimed = new Dictionary<int, FieldTable>();
        var next = 0;
        FieldTable? Claim(bool columns)
        {
            while (next < fieldTables.Count && fieldTables[next].IsColumns != columns) next++;
            return next < fieldTables.Count ? fieldTables[next++] : null;
        }
        // Fields sections first, then entries, each in table order; the field tables in document order.
        foreach (var i in Enumerable.Range(0, rows.Count).Where(i => rows[i].Holds == Holds.Fields))
            if (Claim(false) is { } ft) claimed[i] = ft;
        foreach (var i in Enumerable.Range(0, rows.Count).Where(i => rows[i].Holds == Holds.Entries))
            if (Claim(false) is { } ft) claimed[i] = ft;
        next = 0;
        foreach (var i in Enumerable.Range(0, rows.Count).Where(i => rows[i].Holds == Holds.Table))
            if (Claim(true) is { } ft) claimed[i] = ft;
            else problems.Add($"line {rows[i].Row.Line}: a section that holds a table has no column table");

        var sections = new List<ShapeSection>();
        for (var i = 0; i < rows.Count; i++)
        {
            var (row, kind, pattern, holds) = rows[i];
            if (kind is null) { problems.Add($"line {row.Line}: section '{row.Cells[0]}' is neither a `## heading`, the frontmatter, the head, the table nor the whole file"); continue; }
            var (required, multiple) = PresentOf(row.Cells[1], problems, row.Line);
            if (holds == Holds.Fields && !claimed.ContainsKey(i)) problems.Add($"line {row.Line}: a section that holds fields has no field table");
            var name = kind switch
            {
                SectionKind.Heading => HeadingSpan.Match(row.Cells[0]).Groups["h"].Value.Trim(),
                SectionKind.Frontmatter => "frontmatter",
                SectionKind.Head => "head",
                SectionKind.Table => "table",
                _ => "body",
            };
            sections.Add(new ShapeSection(name, kind.Value, pattern, required, multiple, holds,
                LeadProseAllowed: holds == Holds.Entries && Regex.IsMatch(row.Cells[2], @"\bafter\b.*\bprose\b"),
                claimed.GetValueOrDefault(i), row.Line));
        }
        return new Shape(sections, problems);
    }

    static string[] Sig(MarkdownTable t) => t.Headers.Select(h => h.ToLowerInvariant().Trim()).ToArray();

    static SectionKind? KindOf(string cell, out Regex? pattern)
    {
        pattern = null;
        var t = cell.Trim();
        var span = HeadingSpan.Match(t);
        if (span.Success)
        {
            var h = span.Groups["h"].Value.Trim();
            // A lone N is a number placeholder (`## Revision N`); an N inside a word is a letter.
            var hasPlaceholder = Regex.IsMatch(h, @"(?<![A-Za-z])N(?![A-Za-z])");
            var rx = "^" + Regex.Replace(Regex.Escape(h), @"(?<![A-Za-z])N(?![A-Za-z])", "[0-9]+") + "$";
            pattern = hasPlaceholder ? new Regex(rx, RegexOptions.CultureInvariant) : null;
            return SectionKind.Heading;
        }
        if (t.StartsWith("frontmatter", StringComparison.OrdinalIgnoreCase)) return SectionKind.Frontmatter;
        if (t.StartsWith("the head", StringComparison.OrdinalIgnoreCase)) return SectionKind.Head;
        if (t.StartsWith("the table", StringComparison.OrdinalIgnoreCase)) return SectionKind.Table;
        if (t.StartsWith("the whole file", StringComparison.OrdinalIgnoreCase)) return SectionKind.Body;
        return null;
    }

    static Holds HoldsOf(string cell, List<string> problems, int line)
    {
        var first = cell.Trim().Split([' ', ',', ':', ';'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
        switch (first.ToLowerInvariant())
        {
            case "fields": return Holds.Fields;
            case "entries": return Holds.Entries;
            case "table": return Holds.Table;
            case "prose": return Holds.Prose;
            case "fenced": return Holds.Fenced;
            default:
                problems.Add($"line {line}: holds '{cell}' is not fields, entries, table, prose or fenced");
                return Holds.Prose;
        }
    }

    static (bool Required, bool Multiple) PresentOf(string cell, List<string> problems, int line)
    {
        var t = cell.Trim();
        var first = t.Split([' ', ',', ':', ';'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
        var multiple = t.Contains("one or more", StringComparison.OrdinalIgnoreCase);
        switch (first.ToLowerInvariant())
        {
            case "required": return (true, multiple);
            case "optional": return (false, multiple);
            default:
                problems.Add($"line {line}: present '{cell}' is not required or optional");
                return (true, multiple);
        }
    }

    static FieldTable ReadFields(MarkdownTable t, bool columns, List<string> problems)
    {
        var fields = new List<ShapeField>();
        foreach (var r in t.Rows)
        {
            var keyCell = r.Cells[0].Trim().Trim('`').Trim();
            var wildcard = keyCell.StartsWith('<') && keyCell.EndsWith('>');
            var key = wildcard ? keyCell[1..^1] : keyCell;
            var (required, multiple) = PresentOf(r.Cells[1], problems, r.Line);
            var type = TypeSpec.Parse(r.Cells[2]);
            if (type is null)
            {
                problems.Add($"line {r.Line}: type '{r.Cells[2]}' is outside the vocabulary");
                type = new TypeSpec(TypeName.Line);
            }
            fields.Add(new ShapeField(key, wildcard, required, multiple, type, r.Cells[3], r.Line));
        }
        return new FieldTable(fields, columns, t.HeaderLine);
    }
}
