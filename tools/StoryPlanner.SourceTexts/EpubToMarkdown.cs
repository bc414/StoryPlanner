namespace StoryPlanner.SourceTexts;

public static class EpubToMarkdown
{
    public static string Convert(string epubPath)
    {
        var chapters = FicEpubReader.ReadChapters(epubPath);
        if (chapters.Count == 0)
            throw new InvalidOperationException($"No chapters found in {epubPath}");

        var storyTitle = Path.GetFileNameWithoutExtension(epubPath);

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"# {storyTitle}");
        sb.AppendLine();

        for (var i = 0; i < chapters.Count; i++)
        {
            var ch = chapters[i];
            sb.AppendLine($"## Chapter {i + 1} — {ch.Title}");
            sb.AppendLine();
            sb.AppendLine(FicHtml.ChapterMarkdown(ch.Xhtml));
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static int Run(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: dotnet run -- --to-markdown <input> <output-dir>");
            Console.Error.WriteLine("  <input>       A single .epub file or a folder of .epub files");
            Console.Error.WriteLine("  <output-dir>  Directory to write .txt files into");
            return 2;
        }

        var input = args[0];
        var outputDir = args[1];

        var epubs = new List<string>();

        if (Directory.Exists(input))
            epubs.AddRange(Directory.GetFiles(input, "*.epub"));
        else if (File.Exists(input) && input.EndsWith(".epub", StringComparison.OrdinalIgnoreCase))
            epubs.Add(input);
        else
        {
            Console.Error.WriteLine($"Not found or not an .epub: {input}");
            return 2;
        }

        if (epubs.Count == 0)
        {
            Console.Error.WriteLine($"No .epub files found in {input}");
            return 2;
        }

        Directory.CreateDirectory(outputDir);

        Console.WriteLine($"Converting {epubs.Count} EPUB(s) to Markdown...");

        var succeeded = 0;
        var failed = 0;

        foreach (var epub in epubs)
        {
            var name = Path.GetFileNameWithoutExtension(epub);
            var outputPath = Path.Combine(outputDir, $"{Slug(name)}.txt");

            try
            {
                var md = Convert(epub);
                File.WriteAllText(outputPath, md);
                Console.WriteLine($"  {name} -> {outputPath} ({md.Length:N0} chars)");
                succeeded++;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"  FAILED: {name} — {ex.Message}");
                failed++;
            }
        }

        Console.WriteLine($"Done. {succeeded} succeeded, {failed} failed.");
        return failed > 0 ? 1 : 0;
    }

    private static string Slug(string s)
    {
        s = s.ToLowerInvariant();
        s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-z0-9]+", "-");
        return s.Trim('-');
    }
}
