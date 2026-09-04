namespace StoryPlanner.ProcessMap;

public sealed record FileTraffic(
    string FileId,
    IReadOnlyList<string> Producers,
    IReadOnlyList<string> Consumers);

/// <summary>
/// Everything the map derives rather than states: consumers, data-flow edges, the union graph,
/// and the promotion gate as a reachability question.
///
/// Consumers are never authored (<c>process-map.md</c> § the note above Format). A hand-kept
/// consumers column is the stale-mirror failure the map exists to stop repeating.
/// </summary>
public static class GraphRules
{
    /// <summary>Producers and consumers of every file row, in table order.</summary>
    public static IReadOnlyList<FileTraffic> Traffic(ProcessMapDocument doc)
        => doc.Files.Select(f => new FileTraffic(
                f.Id,
                doc.Processes.Where(p => p.Outputs.Contains(f.Id)).Select(p => p.Id).ToList(),
                doc.Processes.Where(p => p.Inputs.Contains(f.Id)).Select(p => p.Id).ToList()))
            .ToList();

    /// <summary>
    /// Data-flow edges: p → q when something p writes is something q reads. A process is not
    /// linked to itself.
    /// </summary>
    public static IReadOnlyList<(string From, string To, string Via)> DataEdges(ProcessMapDocument doc)
    {
        var edges = new List<(string, string, string)>();
        foreach (var p in doc.Processes)
        foreach (var q in doc.Processes)
        {
            if (p.Id == q.Id) continue;
            foreach (var f in p.Outputs)
                if (q.Inputs.Contains(f))
                    edges.Add((p.Id, q.Id, f));
        }
        return edges;
    }

    /// <summary>
    /// The union graph the promotion gate walks: authored control edges plus derived data-flow
    /// edges. Control alone would miss a hand-off that happens purely through a file; data alone
    /// would make a Brian node that gates by sequence rather than by file invisible.
    /// </summary>
    public static IReadOnlyDictionary<string, List<string>> UnionGraph(ProcessMapDocument doc)
    {
        var g = doc.Processes.ToDictionary(p => p.Id, _ => new List<string>());
        void Add(string from, string to)
        {
            if (g.TryGetValue(from, out var list) && g.ContainsKey(to) && !list.Contains(to))
                list.Add(to);
        }
        foreach (var e in doc.Edges) Add(e.From, e.To);
        foreach (var (from, to, _) in DataEdges(doc)) Add(from, to);
        foreach (var list in g.Values) list.Sort(StringComparer.Ordinal);
        return g;
    }

    public sealed record GatePath(IReadOnlyList<string> Nodes)
    {
        public override string ToString() => string.Join(" → ", Nodes);
    }

    /// <summary>
    /// The promotion gate. Finds a path from a process that reads <paramref name="sourceFile"/>
    /// to the first process on that path writing <paramref name="targetFile"/>, with no
    /// <c>brian</c> actor anywhere along it.
    ///
    /// The path ENDS at the write, deliberately: a review after the write is detection, and the
    /// constitutional rules it enforces (only Brian baselines; nothing but a verification pass
    /// writes to hypotheses/) are preventive. A Brian node downstream of the write does not
    /// satisfy the gate.
    ///
    /// Returns one shortest violating path per reader, or none. A null
    /// <paramref name="sourceFile"/> means "from anything": every process is a start.
    /// </summary>
    public static IReadOnlyList<GatePath> UngatedPaths(
        ProcessMapDocument doc, string? sourceFile, string targetFile)
    {
        var byId = doc.Processes.ToDictionary(p => p.Id);
        var graph = UnionGraph(doc);
        var found = new List<GatePath>();

        bool IsBrian(string id) => byId[id].Actor == "brian";
        bool Writes(string id) => byId[id].Outputs.Contains(targetFile);

        var starts = sourceFile is null
            ? doc.Processes
            : doc.Processes.Where(p => p.Inputs.Contains(sourceFile));

        foreach (var start in starts)
        {
            if (IsBrian(start.Id)) continue;

            // A single row that both reads the source and writes the target is a path of one.
            if (Writes(start.Id))
            {
                found.Add(new GatePath([start.Id]));
                continue;
            }

            var queue = new Queue<List<string>>();
            var seen = new HashSet<string> { start.Id };
            queue.Enqueue([start.Id]);
            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                foreach (var next in graph[path[^1]])
                {
                    if (IsBrian(next)) continue;          // gated: this branch is fine
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

    /// <summary>Rows sharing one governing file. An over-broad governor shows up as a big number.</summary>
    public static IReadOnlyList<(string File, IReadOnlyList<string> Rows)> GovernorFanIn(
        ProcessMapDocument doc)
        => doc.Processes
            .Where(p => p.GovernedBy.Length > 0)
            .GroupBy(p => p.GovernedBy, StringComparer.Ordinal)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => (g.Key, (IReadOnlyList<string>)g.Select(p => p.Id).ToList()))
            .ToList();
}
