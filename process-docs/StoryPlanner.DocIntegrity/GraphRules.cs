namespace StoryPlanner.DocIntegrity;

public sealed record ArtifactTraffic(
    string ArtifactId,
    IReadOnlyList<string> Writers,
    IReadOnlyList<string> Readers,
    IReadOnlyList<string> InstrumentOf)
{
    /// <summary>Read by a process, or named as one's instrument (§ Schema: that counts as read).</summary>
    public bool IsRead => Readers.Count > 0 || InstrumentOf.Count > 0;
}

/// <summary>
/// Everything the tables derive rather than state (SKILL.md § Derived): consumers of each
/// artifact, data-flow edges between processes, the <c>enables</c> graph's shape, whether an
/// <c>enables</c> edge is backed by data flow, and the hitl gate as a reachability question.
/// There is no edges table; every edge here is computed from reads and writes.
/// </summary>
public static class GraphRules
{
    /// <summary>Writers, readers and instrument-readers of every artifact, in table order.</summary>
    public static IReadOnlyList<ArtifactTraffic> Traffic(SkillDocument doc)
        => doc.Artifacts.Select(a => new ArtifactTraffic(
                a.Id,
                doc.Processes.Where(p => p.Writes.Contains(a.Id)).Select(p => p.Id).ToList(),
                doc.Processes.Where(p => p.Reads.Contains(a.Id)).Select(p => p.Id).ToList(),
                doc.Processes.Where(p => p.Instruments.Contains(a.Id)).Select(p => p.Id).ToList()))
            .ToList();

    /// <summary>
    /// Data-flow edges: p → q when something p writes is something q reads or runs as an
    /// instrument. A process is not linked to itself.
    /// </summary>
    public static IReadOnlyList<(string From, string To, string Via)> DataEdges(SkillDocument doc)
    {
        var edges = new List<(string, string, string)>();
        foreach (var p in doc.Processes)
        foreach (var q in doc.Processes)
        {
            if (p.Id == q.Id) continue;
            foreach (var a in p.Writes)
                if (q.ReadsOrInstruments(a))
                    edges.Add((p.Id, q.Id, a));
        }
        return edges;
    }

    public static IReadOnlyDictionary<string, List<string>> DataGraph(SkillDocument doc)
    {
        var g = doc.Processes.ToDictionary(p => p.Id, _ => new List<string>(), StringComparer.Ordinal);
        foreach (var (from, to, _) in DataEdges(doc))
            if (g.TryGetValue(from, out var list) && !list.Contains(to))
                list.Add(to);
        foreach (var list in g.Values) list.Sort(StringComparer.Ordinal);
        return g;
    }

    /// <summary>Activities that enable nothing. Exactly one is the terminus; the check is the validator's.</summary>
    public static IReadOnlyList<string> Termini(SkillDocument doc)
        => doc.Activities.Where(a => a.Enables.Count == 0).Select(a => a.Id).ToList();

    /// <summary>Activities whose <c>enables</c> names this one, in router order.</summary>
    public static IReadOnlyList<string> EnabledBy(SkillDocument doc, string activityId)
        => doc.Activities.Where(a => a.Enables.Contains(activityId)).Select(a => a.Id).ToList();

    /// <summary>
    /// Every cycle in the <c>enables</c> graph, each as the ids around it starting at its
    /// smallest id. Targets that are not activity ids are ignored here; they are the
    /// validator's <c>ref.enables</c> finding.
    /// </summary>
    public static IReadOnlyList<IReadOnlyList<string>> EnablesCycles(SkillDocument doc)
    {
        var ids = doc.Activities.Select(a => a.Id).ToHashSet(StringComparer.Ordinal);
        var next = doc.Activities.ToDictionary(
            a => a.Id, a => a.Enables.Where(ids.Contains).ToList(), StringComparer.Ordinal);

        var colour = new Dictionary<string, int>(StringComparer.Ordinal); // 0 white, 1 grey, 2 black
        var path = new List<string>();
        var cycles = new List<IReadOnlyList<string>>();
        var seen = new HashSet<string>(StringComparer.Ordinal);

        void Visit(string id)
        {
            colour[id] = 1;
            path.Add(id);
            foreach (var to in next[id])
            {
                var c = colour.GetValueOrDefault(to);
                if (c == 0) Visit(to);
                else if (c == 1)
                {
                    var start = path.IndexOf(to);
                    var cycle = path.Skip(start).ToList();
                    var rotate = cycle.IndexOf(cycle.Min(StringComparer.Ordinal)!);
                    var canonical = cycle.Skip(rotate).Concat(cycle.Take(rotate)).ToList();
                    if (seen.Add(string.Join(">", canonical))) cycles.Add(canonical);
                }
            }
            path.RemoveAt(path.Count - 1);
            colour[id] = 2;
        }

        foreach (var a in doc.Activities)
            if (colour.GetValueOrDefault(a.Id) == 0) Visit(a.Id);

        return cycles;
    }

    /// <summary>
    /// The artifacts that back an <c>enables</c> edge: written by a process of
    /// <paramref name="from"/> and read (or run as an instrument) by a process of
    /// <paramref name="to"/>. Empty means the edge is asserted and nothing flows along it.
    /// </summary>
    public static IReadOnlyList<string> Backing(SkillDocument doc, string from, string to)
    {
        var written = doc.ProcessesOf(from).SelectMany(p => p.Writes).Distinct().ToList();
        var readers = doc.ProcessesOf(to).ToList();
        return written.Where(a => readers.Any(q => q.ReadsOrInstruments(a))).ToList();
    }

    public sealed record GatePath(IReadOnlyList<string> Nodes)
    {
        public override string ToString() => string.Join(" → ", Nodes);
    }

    /// <summary>
    /// The hitl gate. Finds a path over data-flow edges from a process that reads
    /// <paramref name="sourceArtifact"/> to the first process on that path writing any of
    /// <paramref name="targetArtifacts"/>, with no <c>hitl</c> process anywhere along it, the
    /// writer included.
    ///
    /// The path ENDS at the write, deliberately: a review after the write is detection, and
    /// the rule it enforces (only verification produces evidence; Brian decides each
    /// promotion) is preventive. An hitl process downstream of the write does not satisfy it.
    ///
    /// Returns one shortest violating path per reader, or none.
    /// </summary>
    public static IReadOnlyList<GatePath> UngatedPaths(
        SkillDocument doc, string sourceArtifact, IReadOnlyCollection<string> targetArtifacts)
    {
        var byId = doc.Processes.ToDictionary(p => p.Id, StringComparer.Ordinal);
        var graph = DataGraph(doc);
        var found = new List<GatePath>();

        bool IsHitl(string id) => byId[id].Mode == ClosedSets.Hitl;
        bool Writes(string id) => byId[id].Writes.Any(targetArtifacts.Contains);

        foreach (var start in doc.Processes.Where(p => p.ReadsOrInstruments(sourceArtifact)))
        {
            if (IsHitl(start.Id)) continue;

            // A single row that both reads the source and writes the target is a path of one.
            if (Writes(start.Id))
            {
                found.Add(new GatePath([start.Id]));
                continue;
            }

            var queue = new Queue<List<string>>();
            var seen = new HashSet<string>(StringComparer.Ordinal) { start.Id };
            queue.Enqueue([start.Id]);
            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                foreach (var next in graph[path[^1]])
                {
                    if (IsHitl(next)) continue;          // gated: this branch is fine
                    if (!seen.Add(next)) continue;
                    var extended = new List<string>(path) { next };
                    if (Writes(next))
                    {
                        found.Add(new GatePath(extended));
                        queue.Clear();
                        break;
                    }
                    queue.Enqueue(extended);
                }
            }
        }

        return found;
    }
}
