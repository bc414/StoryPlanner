using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.BatchFiles;

/// <summary>
/// What every itemizer does with the items it cut (index-schema): the bodies under
/// <c>items/</c>, one file per item, and the index beside them, written once per batch. The
/// itemizer line names the tool and the commit it ran at, marked dirty when the tool's own folder
/// has uncommitted changes. Shared so that every itemizer refuses the same way, validates ids the
/// same way the index checker will, and records its version the same way.
/// </summary>
public static class ItemizerOutput
{
    public sealed record Item(string Id, string Body, string Locator, string Description);

    static readonly Regex Slug = new(@"^[a-z0-9-]+$", RegexOptions.Compiled);

    /// <summary>Why a batch folder cannot take an itemizer's run, or null when it can: it must exist and hold no index, since an itemizer runs once per batch.</summary>
    public static string? Refusal(string batchDir)
    {
        if (!Directory.Exists(batchDir)) return $"batch folder not found: {batchDir}";
        var indexPath = Path.Combine(batchDir, "index.md");
        return File.Exists(indexPath) ? $"{indexPath} exists — an itemizer runs once per batch; a new cut is a new batch." : null;
    }

    /// <summary>Ids that are not lowercase slugs or that repeat, as messages; empty when the cut is writable.</summary>
    public static IReadOnlyList<string> Problems(IReadOnlyList<Item> items)
    {
        var problems = new List<string>();
        if (items.Count == 0) problems.Add("the cut holds no items");
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var it in items)
        {
            if (!Slug.IsMatch(it.Id)) problems.Add($"item '{it.Id}' is not a lowercase slug");
            else if (!seen.Add(it.Id)) problems.Add($"item '{it.Id}' repeats");
            if (it.Locator.Trim().Length == 0) problems.Add($"item '{it.Id}' has an empty locator");
        }
        return problems;
    }

    /// <summary>Writes the item bodies and the index into <paramref name="batchDir"/>. Call <see cref="Refusal"/> and <see cref="Problems"/> first.</summary>
    public static void Write(string batchDir, string itemizerLine, string locatorNotation, IReadOnlyList<Item> items, string? narrowing)
    {
        var itemsDir = Path.Combine(batchDir, "items");
        Directory.CreateDirectory(itemsDir);
        var utf8 = new UTF8Encoding(false);
        foreach (var it in items)
            File.WriteAllText(Path.Combine(itemsDir, it.Id + ".md"), it.Body, utf8);
        var index = IndexFile.Render(Path.GetFileName(Path.TrimEndingDirectorySeparator(batchDir)), itemizerLine, locatorNotation,
            sourceHash: null, items.Select(it => (it.Id, it.Locator, it.Description)), narrowing);
        File.WriteAllText(Path.Combine(batchDir, "index.md"), index, utf8);
    }

    /// <summary>The index head's itemizer line: <c>tools/&lt;tool&gt;, yyyy-MM-dd &lt;commit&gt;</c>.</summary>
    public static string ItemizerLine(string toolProject) => $"tools/{toolProject}, {DateTime.Now:yyyy-MM-dd} {Commit()}";

    /// <summary>
    /// The short commit the running tool was built from, <c>-dirty</c> when its project folder has
    /// uncommitted changes; the project folder is found by walking up from the binary to the folder
    /// holding a <c>.csproj</c>, which is where <c>dotnet run --project</c> builds from. "unknown"
    /// when git or the folder cannot be found.
    /// </summary>
    public static string Commit()
    {
        try
        {
            var dir = ProjectFolder(AppContext.BaseDirectory);
            if (dir is null) return "unknown";
            var sha = Git(dir, "rev-parse --short HEAD");
            if (sha.Length == 0) return "unknown";
            return Git(dir, "status --porcelain -- .").Length == 0 ? sha : sha + "-dirty";
        }
        catch { return "unknown"; }
    }

    static string? ProjectFolder(string start)
    {
        for (var d = new DirectoryInfo(start); d is not null; d = d.Parent)
            if (d.EnumerateFiles("*.csproj").Any()) return d.FullName;
        return null;
    }

    static string Git(string cwd, string arguments)
    {
        var psi = new ProcessStartInfo("git", arguments) { WorkingDirectory = cwd, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
        using var p = Process.Start(psi)!;
        var output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        return p.ExitCode == 0 ? output.Trim() : "";
    }
}
