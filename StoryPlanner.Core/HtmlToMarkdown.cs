using System.Text;
using System.Text.RegularExpressions;
using System.Web; // You might need System.Net.WebUtility for .NET Core if System.Web isn't available

public static class HtmlToMarkdown
{
    public static string Convert(string html)
    {
        if (string.IsNullOrEmpty(html)) return string.Empty;

        // 1. Decode HTML entities (e.g., &quot; -> ", &#39; -> ')
        string markdown = System.Net.WebUtility.HtmlDecode(html);

        // 2. Replace Headers
        markdown = Regex.Replace(markdown, @"<h3>(.*?)</h3>", "\n### $1\n");
        markdown = Regex.Replace(markdown, @"<h4>(.*?)</h4>", "\n#### $1\n");

        // 3. Replace Lists
        // Remove outer <ul>/<ol> tags but keep their spacing
        markdown = Regex.Replace(markdown, @"</?ul>", "\n"); 
        markdown = Regex.Replace(markdown, @"</?ol>", "\n");
        // Replace list items <li>...</li> with "- ..."
        markdown = Regex.Replace(markdown, @"<li>\s*(.*?)\s*</li>", "- $1\n");

        // 4. Replace Formatting
        markdown = Regex.Replace(markdown, @"<strong>(.*?)</strong>", "**$1**");
        markdown = Regex.Replace(markdown, @"<b>(.*?)</b>", "**$1**");
        markdown = Regex.Replace(markdown, @"<em>(.*?)</em>", "*$1*");
        markdown = Regex.Replace(markdown, @"<i>(.*?)</i>", "*$1*");

        // 5. Replace Paragraphs
        markdown = Regex.Replace(markdown, @"<p>(.*?)</p>", "$1\n\n");
        
        // 6. Replace Line Breaks
        markdown = markdown.Replace("<br>", "\n").Replace("<br/>", "\n");

        // 7. Clean up extra whitespace created by replacements
        return markdown.Trim();
    }
}