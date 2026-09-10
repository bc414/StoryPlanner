using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.DocIntegrity;

public enum ProblemKind { Missing, Unknown, Order, Duplicate, Stray, Form, Type, Reference }

/// <summary>What the engine found against a Shape, named by section and key so a class checker can map it to its own check id.</summary>
public sealed record EngineProblem(string Section, string? Key, ProblemKind Kind, string Message, int Line);

/// <summary>A governed file read to one object by the Shape: keyed lines its properties, entries an array of objects, a table an array of rows, frontmatter an object, prose a string.</summary>
public sealed record ParsedDocument(JsonObject Root, string? Title, int TitleLine, IReadOnlyList<EngineProblem> Problems, IReadOnlyList<EntryPosition> Entries, IReadOnlyDictionary<string, IReadOnlyList<string>> LeadProse)
{
    public JsonNode? this[string property] => Root[property];
}

/// <summary>Where an entry sits: its section, its heading, its heading's line and each field's line, for messages.</summary>
public sealed record EntryPosition(string Section, string Heading, int Line, IReadOnlyDictionary<string, int> FieldLines);

/// <summary>
/// The one fixed mapping from a governed file to the object the engine validates: sections
/// as the Shape partitions them, keyed lines by <see cref="KeyedLines"/> (the grammar the runner
/// renders results with), <c>###</c> entries as an array of objects, a table as an array of
/// rows, prose as a string. Type-directed only where the line grammar needs it: a list on the
/// line is split on spaces for slug, token, id, date, timestamp, hash and enum; a list of line
/// or block is one bullet per item beneath the key.
/// </summary>
public static class DocumentReader
{
    public const string HeadingProperty = "###";

    public static ParsedDocument Read(Shape shape, string text)
    {
        var lines = Hashing.NormalizeNewlines(text).Split('\n');
        var problems = new List<EngineProblem>();
        var root = new JsonObject();
        var entries = new List<EntryPosition>();
        var leadProse = new Dictionary<string, IReadOnlyList<string>>(StringComparer.Ordinal);
        var i = 0;

        // ---- frontmatter ----
        var fm = shape.Section(SectionKind.Frontmatter);
        if (lines.Length > 0 && lines[0].Trim() == "---")
        {
            var close = Array.FindIndex(lines, 1, l => l.Trim() == "---");
            if (close < 0) { problems.Add(new EngineProblem("frontmatter", null, ProblemKind.Form, "the frontmatter opened by --- is never closed", 1)); i = lines.Length; }
            else
            {
                if (fm is null) problems.Add(new EngineProblem("frontmatter", null, ProblemKind.Unknown, "a frontmatter the shape does not declare", 1));
                else root[fm.Property] = ReadFrontmatter(fm, lines, 1, close, problems);
                i = close + 1;
            }
        }
        else if (fm is { Required: true })
            problems.Add(new EngineProblem("frontmatter", null, ProblemKind.Missing, "no frontmatter between --- lines at the top", 1));

        // ---- title ----
        string? title = null;
        var titleLine = 0;
        while (i < lines.Length && lines[i].Trim().Length == 0) i++;
        if (i < lines.Length && lines[i].StartsWith("# ", StringComparison.Ordinal)) { title = lines[i][2..].Trim(); titleLine = i + 1; i++; }

        // ---- the rest: the whole file, or a head, a table and ## sections ----
        var body = shape.Section(SectionKind.Body);
        if (body is not null)
        {
            ReadSection(body, lines, i, lines.Length, root, problems, entries, leadProse);
            return new ParsedDocument(root, title, titleLine, problems, entries, leadProse);
        }

        var firstHeading = Array.FindIndex(lines, i, l => l.StartsWith("## ", StringComparison.Ordinal));
        var preEnd = firstHeading < 0 ? lines.Length : firstHeading;
        var head = shape.Section(SectionKind.Head);
        var table = shape.Section(SectionKind.Table);
        if (head is not null || table is not null)
        {
            var tableStart = Array.FindIndex(lines, i, preEnd - i < 0 ? 0 : preEnd - i, l => l.TrimStart().StartsWith('|'));
            if (tableStart < 0 || tableStart >= preEnd) tableStart = preEnd;
            if (head is not null) ReadSection(head, lines, i, tableStart, root, problems, entries, leadProse);
            else if (lines.Skip(i).Take(tableStart - i).Any(l => l.Trim().Length > 0))
                problems.Add(new EngineProblem("head", null, ProblemKind.Stray, $"line {i + 1}: text before the table that the shape does not declare", i + 1));
            if (table is not null)
            {
                if (tableStart >= preEnd) { if (table.Required) problems.Add(new EngineProblem("table", null, ProblemKind.Missing, "no table", preEnd)); }
                else ReadSection(table, lines, tableStart, preEnd, root, problems, entries, leadProse);
            }
            else if (tableStart < preEnd)
                problems.Add(new EngineProblem("table", null, ProblemKind.Unknown, $"line {tableStart + 1}: a table the shape does not declare", tableStart + 1));
        }
        else if (lines.Skip(i).Take(preEnd - i).Any(l => l.Trim().Length > 0))
            problems.Add(new EngineProblem("head", null, ProblemKind.Stray, $"line {i + 1}: text before the first section that the shape does not declare", i + 1));

        // ---- ## sections ----
        var found = new List<(ShapeSection Section, string Heading, int Start, int End)>();
        for (var h = firstHeading; h >= 0 && h < lines.Length;)
        {
            var heading = lines[h][3..].Trim();
            var end = Array.FindIndex(lines, h + 1, l => l.StartsWith("## ", StringComparison.Ordinal));
            if (end < 0) end = lines.Length;
            var section = shape.HeadingSections.FirstOrDefault(s => s.Matches(heading));
            if (section is null) problems.Add(new EngineProblem(heading, null, ProblemKind.Unknown, $"line {h + 1}: a section '## {heading}' the shape does not declare", h + 1));
            else found.Add((section, heading, h, end));
            h = end;
        }
        var order = found.Select(f => shape.Sections.ToList().IndexOf(f.Section)).ToList();
        if (order.Zip(order.Skip(1)).Any(p => p.Second < p.First))
            problems.Add(new EngineProblem("sections", null, ProblemKind.Order, "the sections are not in the shape's order: " + string.Join(", ", shape.HeadingSections.Select(s => s.Name)), found[0].Start + 1));
        foreach (var s in shape.HeadingSections)
        {
            var mine = found.Where(f => f.Section == s).ToList();
            if (mine.Count == 0) { if (s.Required) problems.Add(new EngineProblem(s.Name, null, ProblemKind.Missing, $"no '## {s.Name}' section", lines.Length)); continue; }
            if (mine.Count > 1 && !s.Repeated) problems.Add(new EngineProblem(s.Name, null, ProblemKind.Duplicate, $"line {mine[1].Start + 1}: '## {s.Name}' appears twice", mine[1].Start + 1));
            if (s.Repeated)
            {
                var arr = new JsonArray();
                foreach (var m in mine)
                {
                    var one = new JsonObject { [HeadingProperty] = m.Heading };
                    ReadSectionInto(s, lines, m.Start + 1, m.End, one, s.Property, problems, entries, leadProse, s.Property, m.Heading);
                    arr.Add(one);
                }
                root[s.Property] = arr;
            }
            else ReadSection(s, lines, mine[0].Start + 1, mine[0].End, root, problems, entries, leadProse);
        }
        return new ParsedDocument(root, title, titleLine, problems, entries, leadProse);
    }

    static void ReadSection(ShapeSection s, string[] lines, int start, int end, JsonObject into, List<EngineProblem> problems, List<EntryPosition> entries, Dictionary<string, IReadOnlyList<string>> leadProse)
        => ReadSectionInto(s, lines, start, end, into, s.Property, problems, entries, leadProse, s.Property, s.Property);

    /// <param name="sectionLabel">The section as problems and entries name it: the shape section's property.</param>
    /// <param name="leadKey">The key lead prose is kept under: the heading itself for a repeated section, so each section's lead is its own.</param>
    static void ReadSectionInto(ShapeSection s, string[] lines, int start, int end, JsonObject into, string property, List<EngineProblem> problems, List<EntryPosition> entries, Dictionary<string, IReadOnlyList<string>> leadProse, string sectionLabel, string leadKey)
    {
        var slice = lines.Skip(start).Take(Math.Max(0, end - start)).ToList();
        switch (s.Holds)
        {
            case Holds.Prose:
                into[property] = string.Join('\n', slice).Trim();
                break;
            case Holds.Fenced:
                into[property] = Fenced(slice);
                break;
            case Holds.Fields:
                into[property] = ReadFields(s, s.Fields!, slice, start + 1, sectionLabel, problems);
                break;
            case Holds.Table:
                into[property] = ReadTable(s, slice, start + 1, sectionLabel, problems);
                break;
            case Holds.Entries:
                into[property] = s.Fields is null
                    ? ReadLineEntries(slice, start + 1, sectionLabel, problems)
                    : ReadEntries(s, slice, start + 1, sectionLabel, problems, entries, leadProse, leadKey);
                break;
        }
    }

    static string Fenced(List<string> slice)
    {
        var open = slice.FindIndex(l => l.TrimStart().StartsWith("```", StringComparison.Ordinal));
        if (open < 0) return "";
        var close = slice.FindIndex(open + 1, l => l.TrimStart().StartsWith("```", StringComparison.Ordinal));
        return string.Join('\n', slice.Skip(open + 1).Take((close < 0 ? slice.Count : close) - open - 1));
    }

    static JsonObject ReadFrontmatter(ShapeSection fm, string[] lines, int from, int to, List<EngineProblem> problems)
    {
        var obj = new JsonObject();
        var seen = new List<string>();
        for (var i = from; i < to; i++)
        {
            var colon = lines[i].IndexOf(':');
            if (lines[i].Trim().Length == 0) continue;
            if (colon <= 0) { problems.Add(new EngineProblem("frontmatter", null, ProblemKind.Stray, $"line {i + 1}: not a key: value line", i + 1)); continue; }
            var key = lines[i][..colon].Trim();
            var value = lines[i][(colon + 1)..].Trim();
            var spec = fm.Fields?.Fields.FirstOrDefault(f => f.Wildcard || f.Key == key);
            if (spec is null) { problems.Add(new EngineProblem("frontmatter", key, ProblemKind.Unknown, $"line {i + 1}: '{key}' is not a frontmatter key", i + 1)); continue; }
            if (seen.Contains(key)) { problems.Add(new EngineProblem("frontmatter", key, ProblemKind.Duplicate, $"line {i + 1}: '{key}' appears twice", i + 1)); continue; }
            seen.Add(key);
            obj[key] = spec.Type.Name == TypeName.List
                ? new JsonArray(value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => (JsonNode)v).ToArray())
                : value;
        }
        CheckPresence(fm.Fields, obj, seen, "frontmatter", from + 1, problems);
        return obj;
    }

    static JsonObject ReadFields(ShapeSection s, FieldTable table, List<string> slice, int firstLine, string section, List<EngineProblem> problems)
    {
        var block = KeyedLines.Read(slice, firstLine);
        foreach (var st in block.Stray)
            problems.Add(new EngineProblem(section, null, ProblemKind.Stray, $"line {st.Line}: neither a keyed line nor a two-space continuation", st.Line));
        var obj = new JsonObject();
        var seen = new List<string>();
        foreach (var f in block.Fields)
        {
            var spec = table.Fields.FirstOrDefault(x => !x.Wildcard && x.Key == f.Key) ?? table.Fields.FirstOrDefault(x => x.Wildcard);
            if (spec is null) { problems.Add(new EngineProblem(section, f.Key, ProblemKind.Unknown, $"line {f.Line}: '{f.Key}' is not a key; the keys are {string.Join(", ", table.Fields.Select(x => x.Key))}", f.Line)); continue; }
            if (seen.Contains(f.Key)) { problems.Add(new EngineProblem(section, f.Key, ProblemKind.Duplicate, $"line {f.Line}: '{f.Key}' appears twice", f.Line)); continue; }
            seen.Add(f.Key);
            obj[f.Key] = Convert(spec, f, section, problems);
        }
        CheckPresence(table, obj, seen, section, firstLine, problems);
        return obj;
    }

    static void CheckPresence(FieldTable? table, JsonObject obj, List<string> seen, string section, int line, List<EngineProblem> problems)
    {
        if (table is null) return;
        foreach (var spec in table.Fields.Where(x => x.Required && !x.Wildcard && !seen.Contains(x.Key)))
            problems.Add(new EngineProblem(section, spec.Key, ProblemKind.Missing, $"line {line}: missing {spec.Key}", line));
        if (table.Fields.Any(x => x.Wildcard && x.Required) && seen.Count == 0)
            problems.Add(new EngineProblem(section, null, ProblemKind.Missing, $"line {line}: at least one keyed line", line));
        var declared = table.Fields.Where(x => !x.Wildcard).Select(x => x.Key).ToList();
        var order = seen.Where(declared.Contains).Select(k => declared.IndexOf(k)).ToList();
        if (order.Zip(order.Skip(1)).Any(p => p.Second <= p.First))
            problems.Add(new EngineProblem(section, null, ProblemKind.Order, $"line {line}: the keys are in the order {string.Join(", ", declared)}", line));
    }

    /// <summary>The keyed value as the node the type maps to; a one-line type with a continuation is a Form problem.</summary>
    static JsonNode? Convert(ShapeField spec, KeyedField f, string section, List<EngineProblem> problems)
    {
        var t = spec.Type;
        if (t.Name == TypeName.List)
        {
            if (t.IsInlineList)
            {
                if (f.HasContinuation) problems.Add(new EngineProblem(section, f.Key, ProblemKind.Form, $"line {f.Line}: '{f.Key}' is one line, its values separated by single spaces", f.Line));
                if (f.Value.Contains("  ", StringComparison.Ordinal) || f.Value.Contains(',')) problems.Add(new EngineProblem(section, f.Key, ProblemKind.Form, $"line {f.Line}: '{f.Key}' holds values separated by single spaces and nothing else; found '{f.Value}'", f.Line));
                return new JsonArray(f.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => (JsonNode)v).ToArray());
            }
            if (f.Value.Length > 0) problems.Add(new EngineProblem(section, f.Key, ProblemKind.Form, $"line {f.Line}: '{f.Key}' is a list, one '  - item' per line beneath the key", f.Line));
            return new JsonArray(f.ListItems.Select(v => (JsonNode)v).ToArray());
        }
        if (t.Name == TypeName.Block)
        {
            // A block begins on its keyed line; the continuations carry the rest.
            if (f.Value.Length == 0) problems.Add(new EngineProblem(section, f.Key, ProblemKind.Form, $"line {f.Line}: '{f.Key}' has no value on its line", f.Line));
            return f.BlockText;
        }
        if (f.HasContinuation) problems.Add(new EngineProblem(section, f.Key, ProblemKind.Form, $"line {f.Line}: '{f.Key}' is one line", f.Line));
        if (f.Value.Length == 0) problems.Add(new EngineProblem(section, f.Key, ProblemKind.Form, $"line {f.Line}: '{f.Key}' has no value on its line", f.Line));
        return f.Value;
    }

    static readonly Regex EntryHeading = new(@"^### (?<h>.*)$", RegexOptions.Compiled);

    static JsonArray ReadEntries(ShapeSection s, List<string> slice, int firstLine, string section, List<EngineProblem> problems, List<EntryPosition> entries, Dictionary<string, IReadOnlyList<string>> leadProse, string leadKey)
    {
        var arr = new JsonArray();
        var lead = new List<string>();
        string? heading = null;
        var headingLine = 0;
        var body = new List<string>();
        var bodyStart = 0;
        void Flush()
        {
            if (heading is null) return;
            var obj = new JsonObject { [HeadingProperty] = heading };
            var block = KeyedLines.Read(body, bodyStart);
            foreach (var st in block.Stray)
                problems.Add(new EngineProblem(section, null, ProblemKind.Stray, $"line {st.Line}: neither a keyed line nor a two-space continuation", st.Line));
            var seen = new List<string>();
            var fieldLines = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var f in block.Fields)
            {
                var spec = s.Fields!.Fields.FirstOrDefault(x => !x.Wildcard && x.Key == f.Key) ?? s.Fields.Fields.FirstOrDefault(x => x.Wildcard);
                if (spec is null) { problems.Add(new EngineProblem(section, f.Key, ProblemKind.Unknown, $"line {f.Line}: '{f.Key}' is not a key; the keys are {string.Join(", ", s.Fields!.Fields.Select(x => x.Key))}", f.Line)); continue; }
                if (seen.Contains(f.Key)) { problems.Add(new EngineProblem(section, f.Key, ProblemKind.Duplicate, $"line {headingLine}: '{f.Key}' appears twice", f.Line)); continue; }
                seen.Add(f.Key);
                fieldLines[f.Key] = f.Line;
                obj[f.Key] = Convert(spec, f, section, problems);
            }
            CheckPresence(s.Fields, obj, seen, section, headingLine, problems);
            entries.Add(new EntryPosition(section, heading, headingLine, fieldLines));
            arr.Add(obj);
            heading = null;
            body = [];
        }
        for (var i = 0; i < slice.Count; i++)
        {
            var m = EntryHeading.Match(slice[i]);
            if (m.Success)
            {
                Flush();
                heading = m.Groups["h"].Value.Trim();
                headingLine = firstLine + i;
                bodyStart = firstLine + i + 1;
                continue;
            }
            if (heading is null) { if (slice[i].Trim().Length > 0 || lead.Count > 0) lead.Add(slice[i]); continue; }
            body.Add(slice[i]);
        }
        Flush();
        var leadText = lead.Where(l => l.Trim().Length > 0).ToList();
        if (leadText.Count > 0)
        {
            if (s.LeadProseAllowed) leadProse[leadKey] = lead;
            else problems.Add(new EngineProblem(section, null, ProblemKind.Stray, $"line {firstLine + lead.FindIndex(l => l.Trim().Length > 0)}: a line outside every entry; the section is entries", firstLine + lead.FindIndex(l => l.Trim().Length > 0)));
        }
        if (s.Repeated == false && arr.Count == 0 && s.Required) { /* an entries section may be empty; a class rule says otherwise */ }
        return arr;
    }

    static readonly Regex LineEntry = new(@"^(?:- |\d+\. )(?<t>.*)$", RegexOptions.Compiled);

    static JsonArray ReadLineEntries(List<string> slice, int firstLine, string section, List<EngineProblem> problems)
    {
        var arr = new JsonArray();
        string? current = null;
        for (var i = 0; i < slice.Count; i++)
        {
            var l = slice[i];
            if (l.Trim().Length == 0) continue;
            var m = LineEntry.Match(l);
            if (m.Success) { if (current is not null) arr.Add(current); current = m.Groups["t"].Value.Trim(); continue; }
            if (l.StartsWith("  ", StringComparison.Ordinal) && current is not null) { current += "\n" + l.Trim(); continue; }
            if (l.StartsWith("### ", StringComparison.Ordinal))
            {
                // The schema declares one-line entries here (an entries section with no field
                // table); a `###` entry means the schema and the file disagree about the form.
                problems.Add(new EngineProblem(section, null, ProblemKind.Form, $"line {firstLine + i}: a `###` entry in a section whose schema holds one-line entries (an entries section with no field table)", firstLine + i));
                continue;
            }
            problems.Add(new EngineProblem(section, null, ProblemKind.Stray, $"line {firstLine + i}: not an entry line", firstLine + i));
        }
        if (current is not null) arr.Add(current);
        return arr;
    }

    static JsonArray ReadTable(ShapeSection s, List<string> slice, int firstLine, string section, List<EngineProblem> problems)
    {
        var arr = new JsonArray();
        var columns = s.Fields!.Fields.Select(f => f.Key).ToList();
        var start = slice.FindIndex(l => l.TrimStart().StartsWith('|'));
        if (start < 0) { if (s.Required) problems.Add(new EngineProblem(section, null, ProblemKind.Missing, "no table", firstLine)); return arr; }
        var header = IndexFile.SplitCells(slice[start]).Select(h => h.ToLowerInvariant()).ToList();
        if (!header.SequenceEqual(columns))
            problems.Add(new EngineProblem(section, null, ProblemKind.Form, $"line {firstLine + start}: the columns are {string.Join(" | ", columns)}; found [{string.Join(" | ", header)}]", firstLine + start));
        var i = start + 1;
        if (i >= slice.Count || !IsSeparator(slice[i])) problems.Add(new EngineProblem(section, null, ProblemKind.Form, $"line {firstLine + i}: the header is followed by a separator row", firstLine + i));
        else i++;
        for (; i < slice.Count; i++)
        {
            var l = slice[i];
            if (l.Trim().Length == 0) continue;
            if (!l.TrimStart().StartsWith('|')) { problems.Add(new EngineProblem(section, null, ProblemKind.Stray, $"line {firstLine + i}: a line outside the table", firstLine + i)); continue; }
            var cells = IndexFile.SplitCells(l);
            if (cells.Count != columns.Count) { problems.Add(new EngineProblem(section, null, ProblemKind.Form, $"line {firstLine + i}: {cells.Count} cells, the header has {columns.Count}", firstLine + i)); continue; }
            var row = new JsonObject { ["_line"] = firstLine + i };
            for (var c = 0; c < columns.Count; c++) row[columns[c]] = cells[c];
            arr.Add(row);
        }
        if (arr.Count == 0) problems.Add(new EngineProblem(section, null, ProblemKind.Missing, $"line {firstLine + start}: the table is empty", firstLine + start));
        return arr;
    }

    static bool IsSeparator(string line)
    {
        var t = line.Trim();
        return t.StartsWith('|') && t.All(c => c is '|' or '-' or ':' or ' ') && t.Contains('-');
    }
}
