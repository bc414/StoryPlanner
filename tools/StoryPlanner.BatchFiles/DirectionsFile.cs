using System.Text.RegularExpressions;

namespace StoryPlanner.BatchFiles;

/// <summary>The kinds a What to produce line may declare (directions-schema): a label from the Classes, one line, a block, or entries in a stated form.</summary>
public enum OutputKind { Enum, Line, Block, ListOfLine }

/// <summary>One declared field of the answer: its key, its kind, and what the model is to put there (empty for an enum).</summary>
public sealed record OutputField(string Key, OutputKind Kind, string Text, int Line);

public sealed record DirectionsClass(string Label, string Text, int Line);

/// <summary>A problem the reader found, named by the section it was in; the checker maps it to a check id.</summary>
public sealed record DirectionsProblem(string Section, string Message, int Line);

/// <summary>
/// A directions file (directions-schema): frontmatter that never enters a call, and a body
/// that is the system prompt of every call, verbatim, hashed as the version. The reader is
/// shared by the runner, which passes the body and derives the answer's JSON Schema from
/// What to produce, and by the checker, which holds the file to its schema.
/// </summary>
public sealed class DirectionsFile
{
    public const string Given = "What you are given";
    public const string HowToRead = "How to read";
    public const string ClassesSection = "Classes";
    public const string CriteriaSection = "Criteria";
    public const string WhatToProduce = "What to produce";
    public const string Never = "Never";

    static readonly Regex ClassLine = new(@"^- (?<label>[^:]+): (?<text>.+)$", RegexOptions.Compiled);
    static readonly Regex CriterionLine = new(@"^(?<n>\d+)\. (?<text>.+)$", RegexOptions.Compiled);
    static readonly Regex Slug = new(@"^[a-z0-9-]+$", RegexOptions.Compiled);
    static readonly Regex FileName = new(@"^directions-(?<n>\d+)\.md$", RegexOptions.Compiled);

    public IReadOnlyDictionary<string, string> Frontmatter { get; }
    public IReadOnlyList<string> Questions { get; }
    /// <summary>Everything after the closing <c>---</c>, LF endings, leading and trailing blank lines removed, one trailing newline.</summary>
    public string Body { get; }
    public string BodyHash { get; }
    /// <summary>The body's <c>##</c> sections in order, each with its lines.</summary>
    public IReadOnlyList<(string Heading, IReadOnlyList<string> Lines, int Line)> Sections { get; }
    public IReadOnlyList<DirectionsClass> Classes { get; }
    public IReadOnlyList<string> Criteria { get; }
    public IReadOnlyList<OutputField> Output { get; }
    public IReadOnlyList<DirectionsProblem> Problems { get; }
    public bool HasFrontmatter { get; }
    public bool HasTitle { get; }

    DirectionsFile(IReadOnlyDictionary<string, string> frontmatter, IReadOnlyList<string> questions, string body, IReadOnlyList<(string, IReadOnlyList<string>, int)> sections,
        IReadOnlyList<DirectionsClass> classes, IReadOnlyList<string> criteria, IReadOnlyList<OutputField> output, IReadOnlyList<DirectionsProblem> problems, bool hasFrontmatter, bool hasTitle)
    {
        Frontmatter = frontmatter; Questions = questions; Body = body; BodyHash = Hashing.Sha256Hex(body);
        Sections = sections; Classes = classes; Criteria = criteria; Output = output; Problems = problems;
        HasFrontmatter = hasFrontmatter; HasTitle = hasTitle;
    }

    public bool HasClasses => Sections.Any(s => s.Heading == ClassesSection);
    public IReadOnlyList<string> SectionHeadings => Sections.Select(s => s.Heading).ToList();

    /// <summary>The version number a file name carries, or null.</summary>
    public static int? VersionOf(string fileName)
    {
        var m = FileName.Match(fileName);
        return m.Success ? int.Parse(m.Groups["n"].Value) : null;
    }

    public static DirectionsFile Read(string path) => Parse(File.ReadAllText(path));

    public static DirectionsFile Parse(string text)
    {
        var lines = Hashing.NormalizeNewlines(text).Split('\n');
        var problems = new List<DirectionsProblem>();
        var fm = new Dictionary<string, string>(StringComparer.Ordinal);
        var bodyStart = 0;
        var hasFrontmatter = false;
        if (lines.Length > 0 && lines[0].Trim() == "---")
        {
            var close = Array.FindIndex(lines, 1, l => l.Trim() == "---");
            if (close < 0) problems.Add(new DirectionsProblem("frontmatter", "the frontmatter opened by --- is never closed", 1));
            else
            {
                hasFrontmatter = true;
                for (var i = 1; i < close; i++)
                {
                    var colon = lines[i].IndexOf(':');
                    if (colon <= 0) { problems.Add(new DirectionsProblem("frontmatter", $"line {i + 1}: not a key: value line", i + 1)); continue; }
                    fm[lines[i][..colon].Trim()] = lines[i][(colon + 1)..].Trim();
                }
                bodyStart = close + 1;
            }
        }
        var questions = fm.TryGetValue("questions", out var q)
            ? q.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
            : [];

        var bodyLines = lines.Skip(bodyStart).ToList();
        while (bodyLines.Count > 0 && bodyLines[0].Trim().Length == 0) { bodyLines.RemoveAt(0); bodyStart++; }
        while (bodyLines.Count > 0 && bodyLines[^1].Trim().Length == 0) bodyLines.RemoveAt(bodyLines.Count - 1);
        var body = string.Join('\n', bodyLines) + "\n";

        var hasTitle = bodyLines.Count > 0 && bodyLines[0].StartsWith("# ", StringComparison.Ordinal);

        var sections = new List<(string, IReadOnlyList<string>, int)>();
        string? heading = null;
        var headingLine = 0;
        var current = new List<string>();
        for (var i = 0; i < bodyLines.Count; i++)
        {
            var l = bodyLines[i];
            if (l.StartsWith("## ", StringComparison.Ordinal))
            {
                if (heading is not null) sections.Add((heading, current, headingLine));
                heading = l[3..].Trim();
                headingLine = bodyStart + i + 1;
                current = [];
                continue;
            }
            if (heading is null)
            {
                if (l.Trim().Length > 0 && !hasTitle)
                    problems.Add(new DirectionsProblem("sections", $"line {bodyStart + i + 1}: text before the first ## section", bodyStart + i + 1));
                continue;
            }
            current.Add(l);
        }
        if (heading is not null) sections.Add((heading, current, headingLine));

        var classes = new List<DirectionsClass>();
        foreach (var s in sections.Where(s => s.Item1 == ClassesSection))
            for (var i = 0; i < s.Item2.Count; i++)
            {
                var l = s.Item2[i];
                if (l.Trim().Length == 0) continue;
                var m = ClassLine.Match(l);
                if (!m.Success) { problems.Add(new DirectionsProblem("classes", $"line {s.Item3 + i + 1}: a class is '- <label>: <what an item shows>'", s.Item3 + i + 1)); continue; }
                var label = m.Groups["label"].Value.Trim();
                if (!Slug.IsMatch(label)) problems.Add(new DirectionsProblem("classes", $"line {s.Item3 + i + 1}: label '{label}' is not a slug", s.Item3 + i + 1));
                if (classes.Any(c => c.Label == label)) problems.Add(new DirectionsProblem("classes", $"line {s.Item3 + i + 1}: label '{label}' repeats", s.Item3 + i + 1));
                classes.Add(new DirectionsClass(label, m.Groups["text"].Value.Trim(), s.Item3 + i + 1));
            }

        var criteria = new List<string>();
        foreach (var s in sections.Where(s => s.Item1 == CriteriaSection))
        {
            var expected = 1;
            for (var i = 0; i < s.Item2.Count; i++)
            {
                var l = s.Item2[i];
                if (l.Trim().Length == 0) continue;
                if (l.StartsWith("  ", StringComparison.Ordinal) && criteria.Count > 0) { criteria[^1] += "\n" + l.Trim(); continue; }
                var m = CriterionLine.Match(l);
                if (!m.Success || int.Parse(m.Groups["n"].Value) != expected)
                {
                    problems.Add(new DirectionsProblem("criteria", $"line {s.Item3 + i + 1}: criteria are numbered in sequence from 1; expected {expected}.", s.Item3 + i + 1));
                    if (m.Success) expected = int.Parse(m.Groups["n"].Value) + 1;
                    criteria.Add(l.Trim());
                    continue;
                }
                expected++;
                criteria.Add(m.Groups["text"].Value.Trim());
            }
        }

        var output = new List<OutputField>();
        foreach (var s in sections.Where(s => s.Item1 == WhatToProduce))
        {
            var block = KeyedLines.Read(s.Item2, s.Item3 + 1);
            foreach (var stray in block.Stray)
                problems.Add(new DirectionsProblem("output", $"line {stray.Line}: a What to produce line is '- <field>: <type>[, <text>]'", stray.Line));
            foreach (var f in block.Fields)
            {
                var value = f.Value;
                if (f.HasContinuation) value += " " + string.Join(' ', f.Continuation.Select(c => c.Trim()));
                var comma = value.IndexOf(',');
                var kindText = (comma < 0 ? value : value[..comma]).Trim();
                var rest = comma < 0 ? "" : value[(comma + 1)..].Trim();
                var kind = kindText switch
                {
                    "enum" => OutputKind.Enum,
                    "line" => OutputKind.Line,
                    "block" => OutputKind.Block,
                    "list of line" => OutputKind.ListOfLine,
                    _ => (OutputKind?)null,
                };
                if (kind is null) { problems.Add(new DirectionsProblem("output", $"line {f.Line}: '{kindText}' is not enum, line, block or list of line", f.Line)); continue; }
                if (output.Any(o => o.Key == f.Key)) problems.Add(new DirectionsProblem("output", $"line {f.Line}: field '{f.Key}' repeats", f.Line));
                if (kind != OutputKind.Enum && rest.Length == 0)
                    problems.Add(new DirectionsProblem("output", $"line {f.Line}: a {kindText} field says what the model is to put there" + (kind == OutputKind.ListOfLine ? ", the form of one line" : ""), f.Line));
                if (kind == OutputKind.Enum && !sections.Any(x => x.Item1 == ClassesSection))
                    problems.Add(new DirectionsProblem("output", $"line {f.Line}: an enum field needs a Classes section", f.Line));
                output.Add(new OutputField(f.Key, kind.Value, rest, f.Line));
            }
        }

        return new DirectionsFile(fm, questions, body, sections, classes, criteria, output, problems, hasFrontmatter, hasTitle);
    }

    /// <summary>The sections a study type's directions have, in order, optional ones marked; the referee's and an audit's are the verification kind.</summary>
    public static IReadOnlyList<(string Heading, bool Optional)> SectionsFor(StudyKind kind) => kind switch
    {
        StudyKind.Exploration => [(Given, false), (HowToRead, false), (WhatToProduce, false), (Never, true)],
        _ => [(Given, false), (ClassesSection, false), (CriteriaSection, false), (WhatToProduce, false), (Never, true)],
    };
}

/// <summary>The kinds of study a directions file can belong to; the referee's and an audit's directions are the verification kind.</summary>
public enum StudyKind { Verification, Exploration, Audit, Referee }

public static class StudyKinds
{
    /// <summary>The kind a study folder's name implies, or the referee folder's; null for a folder that is neither.</summary>
    public static StudyKind? OfFolder(string folderName)
    {
        if (folderName == "referee") return StudyKind.Referee;
        if (folderName.StartsWith("verification-of-", StringComparison.Ordinal)) return StudyKind.Verification;
        if (folderName.StartsWith("exploration-of-", StringComparison.Ordinal)) return StudyKind.Exploration;
        if (folderName.StartsWith("audit-of-", StringComparison.Ordinal)) return StudyKind.Audit;
        return null;
    }

    /// <summary>Whether the kind's directions carry a questions line: a verification's and an exploration's always, never the referee's or an audit's.</summary>
    public static bool HasQuestions(StudyKind kind) => kind is StudyKind.Verification or StudyKind.Exploration;
}
