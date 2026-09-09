using System.Security.Cryptography;
using System.Text;

namespace StoryPlanner.BatchFiles;

/// <summary>
/// One hash for every file of a batch. Text is hashed as UTF-8 without a byte-order mark and
/// with LF line endings, so a checkout that rewrites CRLF does not move a directions version
/// or a definition's hash out from under the calls that cite it.
/// </summary>
public static class Hashing
{
    public static string NormalizeNewlines(string text) => text.Replace("\r\n", "\n").Replace('\r', '\n');

    public static string Sha256Hex(string text)
        => Convert.ToHexStringLower(SHA256.HashData(new UTF8Encoding(false).GetBytes(NormalizeNewlines(text))));

    /// <summary>The hash of a file's text under the same normalisation.</summary>
    public static string Sha256HexOfFile(string path) => Sha256Hex(File.ReadAllText(path));

    /// <summary>Whether a cited hash, possibly a prefix of at least six characters, names this one.</summary>
    public static bool Cites(string cited, string full)
        => cited.Length >= 6 && full.StartsWith(cited, StringComparison.OrdinalIgnoreCase);
}
