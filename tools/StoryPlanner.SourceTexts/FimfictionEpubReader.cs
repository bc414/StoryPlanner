using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;

namespace StoryPlanner.SourceTexts;

/// <summary>
/// Reads a Fimfiction EPUB export into one unit per chapter.
///
/// EPUB rather than the .txt export for two reasons, both measured rather than assumed: the .txt
/// loses every italic (this author sets internal monologue in them throughout), and the EPUB
/// carries an explicit chapter list, which removes the ordinal guessing that the .txt's
/// "&gt; Title / &gt; ----" delimiters would have required. Chapter order IS the mapping onto the
/// Part set; chapter titles are the audit of that mapping, printed for every pair.
/// </summary>
public static class FimfictionEpubReader
{
    private static readonly Regex NavPoint = new(
        """<navLabel>\s*<text>(.*?)</text>\s*</navLabel>\s*<content src="(.*?)"\s*/?>""",
        RegexOptions.Singleline | RegexOptions.IgnoreCase);

    private static readonly Regex ChapterFile = new(@"^chapter-\d+\.html$", RegexOptions.IgnoreCase);

    /// <summary>Front matter that ships as a chapter file but is not a chapter.</summary>
    private static readonly string[] NonChapterTitles = ["table of contents", "title", "cover"];

    public sealed record EpubChapter(string File, string Title, string Xhtml);

    /// <summary>Chapters in reading order, front matter removed.</summary>
    public static IReadOnlyList<EpubChapter> ReadChapters(string epubPath)
    {
        using var zip = ZipFile.OpenRead(epubPath);

        var ncxEntry = zip.Entries.FirstOrDefault(e => e.FullName.EndsWith(".ncx", StringComparison.OrdinalIgnoreCase))
                       ?? throw new InvalidOperationException($"No .ncx navigation file in {epubPath}");
        var ncx = ReadEntry(ncxEntry);

        var chapters = new List<EpubChapter>();
        foreach (Match m in NavPoint.Matches(ncx))
        {
            var title = WebUtility.HtmlDecode(m.Groups[1].Value).Trim();
            var src = m.Groups[2].Value.Trim();
            if (!ChapterFile.IsMatch(src)) continue;
            if (NonChapterTitles.Contains(title.ToLowerInvariant())) continue;

            var entry = zip.Entries.FirstOrDefault(e =>
                e.FullName.Equals(src, StringComparison.OrdinalIgnoreCase));
            if (entry is null) continue;

            chapters.Add(new EpubChapter(src, title, ReadEntry(entry)));
        }
        return chapters;
    }

    /// <summary>
    /// Turns ordered chapters into units against ordered Part codes.
    ///
    /// Refuses to map when the counts differ. Zipping a short list onto a long one silently
    /// off-by-ones every chapter after the gap, and a citation pointing at the wrong chapter is
    /// worse than no text at all — so the mismatch is reported and the work is skipped.
    /// </summary>
    public static IReadOnlyList<SourceTextUnit> ToUnits(
        string workName,
        IReadOnlyList<EpubChapter> chapters,
        IReadOnlyList<(string Code, string Name)> parts,
        IReadOnlySet<string> splitCodes,
        string sourceRef,
        IngestReport report)
    {
        if (chapters.Count != parts.Count)
        {
            report.Error(
                $"{workName}: {chapters.Count} chapter(s) in the EPUB but {parts.Count} Part(s) in the plan. " +
                "Refusing to map by order — re-download the EPUB or reconcile the Part set first.");
            report.ListMismatch(workName, chapters.Select(c => c.Title), parts.Select(p => $"{p.Code} — {p.Name}"));
            return [];
        }

        var units = new List<SourceTextUnit>();
        for (var i = 0; i < chapters.Count; i++)
        {
            var (code, partName) = parts[i];
            var ch = chapters[i];
            report.Mapping(workName, code, partName, ch.Title, TitlesAgree(partName, ch.Title));

            var sections = splitCodes.Contains(code) ? FicHtml.SplitSections(ch.Xhtml) : [];
            if (sections.Count == 0)
            {
                units.Add(new SourceTextUnit
                {
                    WorkName = workName,
                    PartCode = code,
                    UnitLabel = ch.Title,
                    Kind = SourceTextKind.Prose,
                    OrderIndex = i,
                    Body = FicHtml.ChapterMarkdown(ch.Xhtml),
                    SourceRef = $"{sourceRef}#{ch.File}"
                });
                continue;
            }

            // A chapter that is an ontology of snippets: each section is its own Part, because
            // each is one unit of a mining pass and a citation to the whole chapter would be
            // barely more precise than citing the fic.
            for (var s = 0; s < sections.Count; s++)
            {
                units.Add(new SourceTextUnit
                {
                    WorkName = workName,
                    PartCode = SectionCode(code, sections[s].Title),
                    UnitLabel = sections[s].Title,
                    Kind = SourceTextKind.Prose,
                    OrderIndex = s,
                    Body = sections[s].Markdown,
                    SourceRef = $"{sourceRef}#{ch.File}"
                });
            }
            report.Note($"{workName}: {code} \"{ch.Title}\" split into {sections.Count} section Part(s).");
        }
        return units;
    }

    /// <summary>Stable, readable sub-part code: "ch121" + "The Queen's Scientist" -> "ch121-queens-scientist".</summary>
    public static string SectionCode(string parentCode, string sectionTitle) =>
        $"{parentCode}-{Slug(sectionTitle)}";

    public static string Slug(string s)
    {
        s = s.ToLowerInvariant();
        s = Regex.Replace(s, "['’]", "");        // possessives collapse rather than split
        s = Regex.Replace(s, @"\bthe\b", " ");         // half these titles start with "The"
        s = Regex.Replace(s, @"[^a-z0-9]+", "-");
        return s.Trim('-');
    }

    private static bool TitlesAgree(string partName, string chapterTitle) =>
        Normalize(partName).Length > 0 && Normalize(chapterTitle).Contains(Normalize(partName));

    private static string Normalize(string s) =>
        Regex.Replace(s.ToLowerInvariant(), "[^a-z0-9]", "");

    private static string ReadEntry(ZipArchiveEntry entry)
    {
        using var stream = entry.Open();
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
