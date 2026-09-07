using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// An artifact's <c>path</c> cell (SKILL.md § Schema): one repo-relative pattern with
/// placeholders in angle brackets, or <c>outside the repo</c>; never prose. Three artifacts
/// may share a file and differ by section, so a pattern may carry <c>§ heading</c> or the word
/// <c>frontmatter</c> after it.
///
/// Strict by the same ruling as everything else here: a cell that lists two patterns with
/// ", or", or a pattern "and its tests", is a syntax error, because a tool that must
/// enumerate files cannot be asked to guess which half it means.
///
/// The placeholders are the ones SKILL.md § Artifacts defines: <c>&lt;study&gt;</c>,
/// <c>&lt;corpus&gt;</c>, <c>&lt;run&gt;</c>, <c>&lt;date&gt;</c>, <c>NNN</c>, <c>N</c>,
/// <c>slug</c>, and <c>.*</c> for any extension.
/// </summary>
public sealed record ArtifactPath(string Pattern, string? Heading, bool Frontmatter, bool OutsideRepo)
{
    public const string OutsideRepoText = "outside the repo";
    public const char SectionSign = '§';
    const string FrontmatterSuffix = " frontmatter";

    static readonly Regex Placeholder = new(
        @"<[A-Za-z]+>|NNN|(?<![A-Za-z])N(?![A-Za-z])|slug|\.\*", RegexOptions.Compiled);

    static readonly Regex SeriesPlaceholder = new(
        @"(?<![A-Za-z])N(?![A-Za-z])|<date>", RegexOptions.Compiled);

    /// <summary>Named by a study: its files live under the study's folder.</summary>
    public bool IsStudyScoped
        => !OutsideRepo && (Pattern.Contains("<study>") || Pattern.Contains("<run>"));

    /// <summary>
    /// A numbered or dated series: the pattern carries <c>N</c> or <c>&lt;date&gt;</c>, so each
    /// new file is written beside the prior ones (a revision note, a calibration).
    /// Ruled 2026-09-05 (handoff 2, step 2): a process that reads one member to write the next
    /// is not editing, so a frozen series is exempt from the read-and-write check the way a
    /// succeeded artifact is. <c>&lt;run&gt;</c> is not a series marker: a run-scoped frozen
    /// artifact read and written by one process is still edit-shaped.
    /// </summary>
    public bool IsSeries => !OutsideRepo && SeriesPlaceholder.IsMatch(Pattern);

    /// <summary>A trailing slash names a directory; presence means it exists and holds a file.</summary>
    public bool IsDirectory => !OutsideRepo && Pattern.EndsWith('/');

    public static bool TryParse(string cell, out ArtifactPath? path, out string? error)
    {
        path = null;
        error = null;
        var text = cell.Trim();
        if (text.Length == 0) { error = "empty path"; return false; }
        if (text == OutsideRepoText) { path = new ArtifactPath("", null, false, true); return true; }

        var pattern = text;
        string? heading = null;
        var frontmatter = false;

        var section = text.IndexOf(SectionSign);
        if (section >= 0)
        {
            pattern = text[..section].Trim();
            heading = text[(section + 1)..].Trim();
            if (heading.Length == 0) { error = $"'{SectionSign}' with no heading after it"; return false; }
        }
        else if (text.EndsWith(FrontmatterSuffix, StringComparison.Ordinal))
        {
            pattern = text[..^FrontmatterSuffix.Length].Trim();
            frontmatter = true;
        }

        if (!TryValidatePattern(pattern, out error)) return false;
        path = new ArtifactPath(pattern, heading, frontmatter, false);
        return true;
    }

    public static bool TryValidatePattern(string pattern, out string? error)
    {
        error = null;
        if (pattern.Length == 0) { error = "empty path"; return false; }
        if (pattern.Contains(' '))
        {
            error = $"'{pattern}' is not one pattern: it contains a space. A path cell states one " +
                    $"repo-relative pattern, or '{OutsideRepoText}'";
            return false;
        }
        if (pattern.Contains('(') || pattern.Contains(')'))
        {
            error = $"'{pattern}' carries a parenthetical. A cell states a pattern or nothing";
            return false;
        }
        if (pattern.StartsWith('/') || pattern.StartsWith('\\') || (pattern.Length > 1 && pattern[1] == ':'))
        {
            error = $"'{pattern}' is absolute or drive-qualified; paths are repo-relative";
            return false;
        }
        if (!pattern.Contains('/'))
        {
            error = $"'{pattern}' has no directory. Paths are repo-relative, so a bare file name " +
                    "cannot be resolved without a search order, and there is none";
            return false;
        }
        if (pattern.Count(c => c == '<') != pattern.Count(c => c == '>'))
        {
            error = $"'{pattern}' has an unclosed placeholder";
            return false;
        }
        return true;
    }

    /// <summary>
    /// The pattern with the study folder and corpus substituted, the rest of the
    /// placeholders left for <see cref="ToRegex"/>.
    /// </summary>
    public string Substitute(string? studyFolder, string? corpus)
    {
        var p = Pattern;
        if (studyFolder is not null) p = p.Replace("<study>", studyFolder);
        if (corpus is not null) p = p.Replace("<corpus>", corpus);
        return p;
    }

    /// <summary>
    /// The directory to enumerate: the substituted pattern up to the last slash before its
    /// first remaining placeholder. Empty means the repo root.
    /// </summary>
    public string FixedPrefix(string? studyFolder = null, string? corpus = null)
    {
        var p = Substitute(studyFolder, corpus);
        var first = Placeholder.Match(p);
        var cut = first.Success ? first.Index : p.Length;
        if (cut == 0) return "";
        var slash = p.LastIndexOf('/', cut - 1);
        return slash < 0 ? "" : p[..slash];
    }

    /// <summary>
    /// A regex over a repo-relative forward-slash path. Directory patterns match the directory
    /// path without its trailing slash.
    /// </summary>
    public Regex ToRegex(string? studyFolder = null, string? corpus = null)
    {
        var p = Substitute(studyFolder, corpus);
        if (IsDirectory) p = p.TrimEnd('/');
        var sb = new StringBuilder("^");
        var i = 0;
        while (i < p.Length)
        {
            var m = Placeholder.Match(p, i);
            if (m.Success && m.Index == i)
            {
                sb.Append(RegexFor(m.Value));
                i += m.Length;
                continue;
            }
            sb.Append(Regex.Escape(p[i].ToString()));
            i++;
        }
        sb.Append('$');
        return new Regex(sb.ToString(), RegexOptions.CultureInvariant);
    }

    static string RegexFor(string placeholder) => placeholder switch
    {
        "NNN" => "[0-9]{3}",
        "N" => "[0-9]+",
        ".*" => @"\.[^/]+",
        _ => "[^/]+",   // <study>, <corpus>, <run>, <date>, <Name>, slug
    };

    public string Display()
    {
        if (OutsideRepo) return OutsideRepoText;
        var s = Pattern;
        if (Heading is not null) s += $" {SectionSign} {Heading}";
        if (Frontmatter) s += FrontmatterSuffix;
        return s;
    }
}
