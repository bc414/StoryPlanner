using StoryPlanner.CodeSessions;
using StoryPlanner.SessionTree;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Fixture-tier tests for tools/StoryPlanner.SessionTree — a real temp codesessions.db written
/// with the production CodeSessionDb, holding a fork family built the way Claude Code actually
/// forks (observed 2026-09-29): every copied record gets a new Uuid, the record at the fork point
/// is RE-STAMPED to the fork time, and a copy can drop a record (here, a tool result). The
/// invariant that matters most: each stretch the forks share renders once, and the family is the
/// same whichever member names it.
/// </summary>
public class SessionTreeTests : IDisposable
{
    private readonly string _dir = Directory.CreateTempSubdirectory("sessiontree-test").FullName;
    private readonly string _dbPath;

    private const string Orig = "aaaaaaaa-orig", A = "bbbbbbbb-fork-a", B = "cccccccc-fork-b", B2 = "dddddddd-fork-b2";
    private const string Unrelated = "eeeeeeee-unrelated";
    private const string RootTs = "2026-09-01T10:00:00.000Z";

    public SessionTreeTests() => _dbPath = Path.Combine(_dir, "codesessions.db");

    public void Dispose()
    {
        try { Directory.Delete(_dir, recursive: true); } catch (IOException) { }
    }

    private static ExtractedRecord R(string uuid, string? parent, string ts, string role, string body) =>
        new(uuid, parent, ts, role, body);

    /// <summary>The root as each copy holds it: its own uuids, the fork-point reply stamped by the copy.</summary>
    private static List<ExtractedRecord> Root(string p, string replyTs, bool dropSecondResult = false)
    {
        var records = new List<ExtractedRecord>
        {
            R($"{p}1", null, RootTs, "user",
                "<ide_opened_file>The user opened the file x.cs in the IDE.</ide_opened_file>\n\nWhat are the problems?"),
            R($"{p}2", $"{p}1", "2026-09-01T10:00:05.000Z", "assistant", "[tool_use: Bash — ls]"),
            R($"{p}3", $"{p}2", "2026-09-01T10:00:06.000Z", "user", "[tool result elided — 120 chars]"),
            R($"{p}4", $"{p}3", "2026-09-01T10:00:07.000Z", "assistant", "[tool_use: Read — notes.md]"),
            R($"{p}5", $"{p}4", "2026-09-01T10:00:08.000Z", "user", "[tool result elided — 50 chars]"),
            R($"{p}6", $"{p}5", replyTs, "assistant", "Here are the problems."),
        };
        if (dropSecondResult) records.RemoveAt(4);
        return records;
    }

    private void Write(Microsoft.Data.Sqlite.SqliteConnection conn, string sessionId, List<ExtractedRecord> records,
        string kind = "main", string? parent = null, string mtime = "2026-08-01T00:00:00.0000000Z")
    {
        var ordered = records.OrderBy(r => r.Timestamp, StringComparer.Ordinal).ToList();
        CodeSessionDb.ReplaceSession(conn, new SessionRow(
            SessionId: sessionId, ProjectDir: "Proj", Kind: kind, ParentSessionId: parent,
            Title: null, Slug: null,
            FirstTimestamp: ordered[0].Timestamp, LastTimestamp: ordered[^1].Timestamp,
            RecordCount: ordered.Count, TotalChars: ordered.Sum(r => (long)r.Body.Length), SubagentCount: 0,
            MalformedLines: 0, SourceBytes: 100, SourceMtimeUtc: mtime), ordered);
    }

    /// <summary>
    /// orig: the root only. A: forked at the root's reply. B: forked at the root's reply, its copy
    /// missing a tool result. B2: forked from B at B's first reply.
    /// </summary>
    private void SeedFamily()
    {
        using var conn = CodeSessionDb.OpenWrite(_dbPath);
        Write(conn, Orig, Root("o", "2026-09-01T10:00:30.000Z"));

        Write(conn, A, [
            .. Root("a", "2026-09-01T10:05:00.000Z"),
            R("a7", "a6", "2026-09-01T10:05:10.000Z", "user", "Branch A question"),
            R("a8", "a7", "2026-09-01T10:05:20.000Z", "assistant", "Answer A"),
        ]);

        Write(conn, B, [
            .. Root("b", "2026-09-01T10:10:00.000Z", dropSecondResult: true),
            R("b7", "b6", "2026-09-01T10:10:10.000Z", "user", "Branch B question"),
            R("b8", "b7", "2026-09-01T10:10:20.000Z", "assistant", "Answer B"),
            R("b9", "b8", "2026-09-01T10:12:00.000Z", "user", "B deeper"),
            R("b10", "b9", "2026-09-01T10:12:10.000Z", "assistant", "B deeper reply"),
        ]);

        Write(conn, B2, [
            .. Root("c", "2026-09-01T10:10:00.000Z", dropSecondResult: true),
            R("c7", "c6", "2026-09-01T10:10:10.000Z", "user", "Branch B question"),
            R("c8", "c7", "2026-09-01T10:20:00.000Z", "assistant", "Answer B"),
            R("c9", "c8", "2026-09-01T10:20:10.000Z", "user", "B2 question"),
            R("c10", "c9", "2026-09-01T10:20:20.000Z", "assistant", "B2 reply"),
        ]);

        // Same first timestamp, different words: another conversation, never a member.
        Write(conn, Unrelated, [
            R("u1", null, RootTs, "user", "Something else entirely"),
            R("u2", "u1", "2026-09-01T10:00:09.000Z", "assistant", "Unrelated reply"),
        ]);
    }

    private (Family Family, ForkTree Tree) Load(string idOrPrefix)
    {
        using var conn = FamilyReader.Open(_dbPath);
        var family = FamilyReader.Load(conn, FamilyReader.Resolve(conn, idOrPrefix).Single());
        return (family, ForkTree.Build(family.Members.Select(m => (m.SessionId, m.Records)).ToList()));
    }

    private static int Occurrences(string haystack, string needle)
    {
        var count = 0;
        for (var i = haystack.IndexOf(needle, StringComparison.Ordinal); i >= 0; i = haystack.IndexOf(needle, i + needle.Length, StringComparison.Ordinal))
            count++;
        return count;
    }

    [Fact]
    public void Any_member_names_the_same_family_and_a_different_first_record_is_not_a_member()
    {
        SeedFamily();
        foreach (var id in new[] { Orig, A, B, B2 })
        {
            var (family, _) = Load(id[..8]);
            Assert.Equal([Orig, A, B, B2], family.Members.Select(m => m.SessionId));
        }
    }

    [Fact]
    public void Segments_split_exactly_where_the_forks_diverge_despite_restamps_and_dropped_copies()
    {
        SeedFamily();
        var (_, tree) = Load(A);

        Assert.Equal(5, tree.Segments.Count);
        var (s1, s2, s3, s4, s5) = (tree.Segments[0], tree.Segments[1], tree.Segments[2], tree.Segments[3], tree.Segments[4]);

        Assert.Equal([Orig, A, B, B2], s1.Sessions);
        Assert.Equal([Orig], s1.EndingHere);                 // a session that is only a prefix ends inside the tree
        Assert.Equal([s2, s3], s1.Children);                 // in the order they diverged
        Assert.Equal([A], s2.Sessions);
        Assert.Equal([B, B2], s3.Sessions);                  // B's copy lost a tool result; still one stretch
        Assert.Equal([s4, s5], s3.Children);
        Assert.Equal([B], s4.EndingHere);
        Assert.Equal([B2], s5.EndingHere);

        // The shared reply was re-stamped in every fork; the node keeps the original's time.
        Assert.Equal("2026-09-01T10:00:30.000Z", s1.Nodes[^1].Record!.Timestamp);
        Assert.Equal("2026-09-01T10:10:20.000Z", s3.Nodes[^1].Record!.Timestamp);
    }

    [Fact]
    public void Each_shared_stretch_renders_once_and_the_document_does_not_depend_on_the_member_named()
    {
        SeedFamily();
        var (familyA, treeA) = Load(A);
        var (familyB2, treeB2) = Load(B2);
        var docA = TreeRenderer.Render(familyA, treeA, null, _dbPath).Document;
        var docB2 = TreeRenderer.Render(familyB2, treeB2, null, _dbPath).Document;

        Assert.Equal(docA, docB2);
        Assert.Equal(1, Occurrences(docA, "What are the problems?"));
        Assert.Equal(1, Occurrences(docA, "Here are the problems."));
        Assert.Equal(1, Occurrences(docA, "Branch B question"));
        Assert.Equal(1, Occurrences(docA, "\nAnswer B\n"));
        Assert.DoesNotContain("[tool_use:", docA);
        Assert.DoesNotContain("[tool result elided", docA);
        Assert.DoesNotContain("<ide_opened_file>", docA);
        Assert.Empty(TreeRenderer.Warnings(familyA, treeA, null));
    }

    [Fact]
    public void A_subagent_report_is_the_hand_back_and_one_without_a_hand_back_falls_back_to_its_final_message()
    {
        using (var conn = CodeSessionDb.OpenWrite(_dbPath))
        {
            const string main = "hhhhhhhh-main";
            Write(conn, main, [
                R("m1", null, "2026-09-02T11:00:00.000Z", "user", "Trace X and Y"),
                R("m2", "m1", "2026-09-02T11:00:05.000Z", "assistant", "Launching two agents."),
                R("m3", "m2", "2026-09-02T11:00:06.000Z", "assistant", "[tool_use: Agent — Trace X]"),
                R("m4", "m3", "2026-09-02T11:05:00.000Z", CodeSessionExtractor.HarnessRole,
                    "Another Claude session sent a message:\r\n<agent-message from=\"x1\">\r\n" +
                    "[Subagent hand-back] The text below is the final report of a subagent. The report follows:\r\n" +
                    "  ## Report X\r\n  \r\n  The X finding.\r\n</agent-message>\r\n\r\nThat \"other Claude session\" is a subagent."),
                R("m5", "m4", "2026-09-02T11:05:10.000Z", CodeSessionExtractor.HarnessRole,
                    "<task-notification>\n<task-id>x1</task-id>\n<status>completed</status>\n<result>This agent's report was delivered " +
                    "to you as a message from \"x1\" (its SubagentHandback call). Read it there; it is not repeated here.\n</result>\n</task-notification>"),
                R("m6", "m5", "2026-09-02T11:10:00.000Z", "assistant", "Both done."),
            ]);
            Write(conn, "agent-x1", [
                R("x1a", null, "2026-09-02T11:00:06.500Z", "assistant", "Prompt for X"),
                R("x1b", "x1a", "2026-09-02T11:05:00.000Z", "assistant", "I've sent my report."),
            ], kind: "subagent", parent: main);
            Write(conn, "agent-y2", [
                R("y2a", null, "2026-09-02T11:00:07.000Z", "assistant", "Prompt for Y"),
                R("y2b", "y2a", "2026-09-02T11:03:00.000Z", "assistant", "[tool_use: Grep — needle]"),
                R("y2c", "y2b", "2026-09-02T11:06:00.000Z", "assistant", "Final report Y."),
            ], kind: "subagent", parent: main);
        }

        var (family, tree) = Load("hhhhhhhh");
        var doc = TreeRenderer.Render(family, tree, null, _dbPath).Document;

        Assert.Equal(1, Occurrences(doc, "Prompt for X"));
        Assert.Contains("\n## Report X\n\nThe X finding.\n", doc);   // unindented, preamble and frame gone
        Assert.DoesNotContain("[Subagent hand-back]", doc);
        Assert.DoesNotContain("This agent's report was delivered", doc);
        Assert.DoesNotContain("I've sent my report.", doc);        // the hand-back is x1's report, not its last line
        Assert.Equal(1, Occurrences(doc, "Prompt for Y"));
        Assert.Equal(1, Occurrences(doc, "Final report Y."));
        Assert.True(doc.IndexOf("Final report Y.", StringComparison.Ordinal) < doc.IndexOf("Both done.", StringComparison.Ordinal));
    }

    [Fact]
    public void The_disk_check_separates_newer_dialogue_from_metadata_and_finds_forks_not_yet_ingested()
    {
        SeedFamily();
        var root = Path.Combine(_dir, "projects");
        var proj = Directory.CreateDirectory(Path.Combine(root, "Proj")).FullName;
        string Line(string type, string ts) => $"{{\"type\":\"{type}\",\"timestamp\":\"{ts}\",\"uuid\":\"z\"}}";

        // B2: a turn after its archived last record.
        File.WriteAllLines(Path.Combine(proj, B2 + ".jsonl"), [Line("user", RootTs), Line("assistant", "2026-09-01T11:00:00.000Z")]);
        // B: changed, but only by a trailing non-dialogue record.
        File.WriteAllLines(Path.Combine(proj, B + ".jsonl"), [Line("user", RootTs), "{\"type\":\"session-stats\"}"]);
        // A: unchanged since ingest — its mtime is the stamp the row carries.
        var aPath = Path.Combine(proj, A + ".jsonl");
        File.WriteAllLines(aPath, [Line("user", RootTs)]);
        File.SetLastWriteTimeUtc(aPath, new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc));
        // Written after the last ingest: one opens with the family's first record, one does not.
        var later = DateTime.UtcNow.AddMinutes(5);
        foreach (var (stem, ts) in new[] { ("ffffffff-new-fork", RootTs), ("gggggggg-other", "2026-09-05T09:00:00.000Z") })
        {
            var path = Path.Combine(proj, stem + ".jsonl");
            File.WriteAllLines(path, ["{\"type\":\"file-history-snapshot\"}", Line("user", ts)]);
            File.SetLastWriteTimeUtc(path, later);
        }

        var (family, tree) = Load(A);
        var disk = FamilyReader.CheckDisk(family, root);

        Assert.StartsWith("archive only", disk.MemberStatus[Orig]);
        Assert.StartsWith("on disk, ingested", disk.MemberStatus[A]);
        Assert.StartsWith("changed since ingest, no newer dialogue", disk.MemberStatus[B]);
        Assert.StartsWith(FamilyReader.NewerDialogue, disk.MemberStatus[B2]);
        Assert.Equal(["ffffffff-new-fork"], disk.UningestedForks);
        Assert.Equal(2, TreeRenderer.Warnings(family, tree, disk).Count);  // B2's newer dialogue, the new fork
    }
}

/// <summary>Pure-tier tests for TurnFilter and rewind detection: records constructed directly, no db.</summary>
public class SessionTreeTurnFilterTests
{
    private static SessionRecord Rec(string role, string body, string uuid = "u", string? parent = "p", string ts = "2026-09-01T10:00:00.000Z") =>
        new(uuid, parent, 1, ts, role, body);

    [Fact]
    public void Tool_stubs_and_elided_results_are_removed_and_a_record_holding_only_them_is_dropped()
    {
        Assert.Equal("Checking the notes.", TurnFilter.Map(Rec("assistant", "Checking the notes.\n\n[tool_use: Grep — foo|bar]"))!.Text);
        Assert.Null(TurnFilter.Map(Rec("assistant", "[tool_use: Read — C:/x.md]")));
        Assert.Null(TurnFilter.Map(Rec("user", "[tool result elided — 12,345 chars]")));
    }

    [Fact]
    public void Question_answers_verdicts_and_interruptions_are_kept_verbatim_as_his_actions()
    {
        const string answers = "[AskUserQuestion — 2 answered]\n\nQ: Which layout?\nChose: Tree doc only\n\nQ: Why?\nTyped: Because I read it top down.";
        var mixed = TurnFilter.Map(Rec("user", "[tool result elided — 40 chars]\n\n" + answers))!;
        Assert.Equal(ItemKind.Action, mixed.Kind);
        Assert.Equal(answers, mixed.Text);

        Assert.Equal(ItemKind.Action, TurnFilter.Map(Rec("user", "[Plan approved by user]"))!.Kind);
        Assert.Equal("[Rejected by user]\nTyped: not that file", TurnFilter.Map(Rec("user", "[Rejected by user]\nTyped: not that file"))!.Text);
        Assert.Equal(ItemKind.Action, TurnFilter.Map(Rec("user", "[Request interrupted by user]"))!.Kind);
    }

    [Fact]
    public void A_plan_keeps_its_text_and_its_approved_revision_under_a_label()
    {
        var item = TurnFilter.Map(Rec("assistant",
            "[tool_use: ExitPlanMode]\n\n# Plan\nDo the thing.\n\n[Plan as approved — differs from the proposal above]\n\n# Plan\nDo the other thing."))!;
        Assert.StartsWith(TurnFilter.PlanMarker, item.Text);
        Assert.Contains("Do the thing.", item.Text);
        Assert.Contains("[Plan as approved — differs from the proposal above]", item.Text);
        Assert.Contains("Do the other thing.", item.Text);
        Assert.DoesNotContain("[tool_use:", item.Text);
    }

    [Fact]
    public void An_opened_file_notice_is_stripped_from_his_prompt_but_a_selection_is_his_and_stays()
    {
        var prompt = TurnFilter.Map(Rec("user", "<ide_opened_file>The user opened the file a.cs in the IDE.</ide_opened_file>\n\nWhy is this slow?"))!;
        Assert.Equal(ItemKind.Prompt, prompt.Kind);
        Assert.Equal("Why is this slow?", prompt.Text);

        const string selected = "<ide_selection>var x = Load();</ide_selection>\n\nWhy is this slow?";
        Assert.Equal(selected, TurnFilter.Map(Rec("user", selected))!.Text);
    }

    [Fact]
    public void Harness_records_are_dropped_except_hand_backs_and_compaction_markers()
    {
        Assert.Null(TurnFilter.Map(Rec(CodeSessionExtractor.HarnessRole, "Base directory for this skill: c:\\x\n\n# Skill")));
        Assert.Null(TurnFilter.Map(Rec(CodeSessionExtractor.HarnessRole, "<task-notification>\n<task-id>b77</task-id>\n<status>completed</status>\n</task-notification>")));
        Assert.Equal(ItemKind.Compaction, TurnFilter.Map(Rec(CodeSessionExtractor.HarnessRole, "[compaction summary dropped — 9,000 chars]"))!.Kind);

        var older = TurnFilter.Map(Rec(CodeSessionExtractor.HarnessRole,
            "<task-notification>\n<task-id>a4fc</task-id>\n<status>completed</status>\n<result>Here is the summary.\n\n## Findings\n</result>\n<usage></usage>\n</task-notification>"))!;
        Assert.Equal(ItemKind.Handback, older.Kind);
        Assert.Equal("a4fc", older.AgentId);
        Assert.Equal("Here is the summary.\n\n## Findings", older.Text);
    }

    [Fact]
    public void A_rewind_is_two_prompts_answering_one_parent_and_parallel_tool_calls_are_not_one()
    {
        var rewound = new[]
        {
            Rec("user", "First try", uuid: "p1", parent: "a0", ts: "2026-09-01T10:00:00.000Z"),
            Rec("assistant", "Reply one", uuid: "r1", parent: "p1"),
            Rec("user", "Second try", uuid: "p2", parent: "a0", ts: "2026-09-01T10:05:00.000Z"),
        };
        var rewind = Assert.Single(ForkTree.Rewinds("s", rewound));
        Assert.Equal(["2026-09-01T10:00:00.000Z", "2026-09-01T10:05:00.000Z"], rewind.PromptTimestamps);

        var parallel = new[]
        {
            Rec("user", "Look it up", uuid: "p1", parent: "a0"),
            Rec("assistant", "[tool_use: Grep — a]", uuid: "t1", parent: "k1"),
            Rec("user", "[tool result elided — 10 chars]", uuid: "t2", parent: "k1"),
            Rec("assistant", "[tool_use: Grep — b]", uuid: "t3", parent: "k1"),
        };
        Assert.Empty(ForkTree.Rewinds("s", parallel));
    }
}
