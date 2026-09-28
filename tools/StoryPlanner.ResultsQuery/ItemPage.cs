using StoryPlanner.BatchFiles;

namespace StoryPlanner.ResultsQuery;

/// <summary>
/// One item of the batch as the page shows it beside a view: its index row, the body the reader
/// was given, and the result the runner rendered, both as the files hold them. The body is null
/// when the items folder does not hold it (items are uncommitted and regenerable); the result is
/// null when the item has no successful call, the same rule the views count by.
/// </summary>
public sealed record ItemPage(string Item, string Locator, string Description, string? Body, string? Result)
{
    /// <summary>
    /// The item a view's cell names: an item id as the list view prints it, or a cites token
    /// <c>&lt;study&gt;/&lt;batch&gt;/&lt;item&gt;</c> of this batch. Null for a token of another batch.
    /// </summary>
    public static string? ItemOf(string batchId, string token)
    {
        token = token.Trim();
        var prefix = batchId + "/";
        if (token.StartsWith(prefix, StringComparison.Ordinal)) return token[prefix.Length..];
        return token.Contains('/') ? null : token;
    }

    /// <summary>Reads one item from the batch's own files; null when the index holds no such item.</summary>
    public static ItemPage? Load(string definitionPath, string item)
    {
        var definition = DefinitionFile.Read(definitionPath);
        if (!File.Exists(definition.IndexPath)) return null;
        var row = IndexFile.Read(definition.IndexPath).Rows.FirstOrDefault(r => r.Item == item);
        if (row is null) return null;
        var bodyPath = Path.Combine(definition.ItemsDir, item + ".md");
        var resultPath = Path.Combine(definition.ResultsDir, item + ".md");
        var succeeded = CallsFile.Read(definition.CallsPath).HasSucceeded(item);
        return new ItemPage(
            row.Item, row.Locator, row.Description,
            File.Exists(bodyPath) ? File.ReadAllText(bodyPath) : null,
            succeeded && File.Exists(resultPath) ? File.ReadAllText(resultPath) : null);
    }
}
