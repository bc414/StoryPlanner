using System.Linq;
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

        // 2. Replace Headers (Added h1, h2, h5, h6 for completeness)
        markdown = Regex.Replace(markdown, @"(?s)<h1[^>]*>(.*?)</h1>", "\n# $1\n");
        markdown = Regex.Replace(markdown, @"(?s)<h2[^>]*>(.*?)</h2>", "\n## $1\n");
        markdown = Regex.Replace(markdown, @"(?s)<h3[^>]*>(.*?)</h3>", "\n### $1\n");
        markdown = Regex.Replace(markdown, @"(?s)<h4[^>]*>(.*?)</h4>", "\n#### $1\n");
        markdown = Regex.Replace(markdown, @"(?s)<h5[^>]*>(.*?)</h5>", "\n##### $1\n");
        markdown = Regex.Replace(markdown, @"(?s)<h6[^>]*>(.*?)</h6>", "\n###### $1\n");

        // 3. Replace Horizontal Rules
        markdown = Regex.Replace(markdown, @"<hr\s*/?>", "\n---\n");

        // 4. Replace Blockquotes
        markdown = Regex.Replace(markdown, @"(?s)<blockquote[^>]*>(.*?)</blockquote>", m => 
            "\n> " + m.Groups[1].Value.Trim().Replace("\n", "\n> ") + "\n\n");

        // 5. Replace Tables
        markdown = Regex.Replace(markdown, @"(?s)<table[^>]*>(.*?)</table>", m => {
            string tableContent = m.Groups[1].Value;
            
            // Process headers to add the markdown separator (e.g., |---|---|)
            tableContent = Regex.Replace(tableContent, @"(?s)<thead[^>]*>(.*?)</thead>", theadMatch => {
                string headerRow = theadMatch.Groups[1].Value;
                int cols = Regex.Matches(headerRow, @"<th").Count;
                string separator = "| " + string.Join(" | ", Enumerable.Repeat("---", cols)) + " |\n";
                return headerRow + "\n" + separator;
            });

            // Convert Rows and Cells
            tableContent = Regex.Replace(tableContent, @"<tr[^>]*>", "| ");
            tableContent = Regex.Replace(tableContent, @"</tr>", " |\n");
            tableContent = Regex.Replace(tableContent, @"(?s)<th[^>]*>(.*?)</th>", "$1 | ");
            tableContent = Regex.Replace(tableContent, @"(?s)<td[^>]*>(.*?)</td>", "$1 | ");
            
            // Clean up remaining table wrapper tags
            tableContent = Regex.Replace(tableContent, @"</?tbody[^>]*>", "");
            tableContent = Regex.Replace(tableContent, @"</?thead[^>]*>", "");
            
            return "\n\n" + tableContent.Trim() + "\n\n";
        });

        // 6. Replace Lists
        // Remove outer <ul>/<ol> tags but keep their spacing
        markdown = Regex.Replace(markdown, @"</?ul[^>]*>", "\n"); 
        markdown = Regex.Replace(markdown, @"</?ol[^>]*>", "\n");
        // Replace opening <li> with "- " (Handles nested lists perfectly)
        markdown = Regex.Replace(markdown, @"<li[^>]*>\s*", "- ");
        // Strip closing </li>
        markdown = markdown.Replace("</li>", "");

        // 7. Replace Formatting
        markdown = Regex.Replace(markdown, @"(?s)<strong[^>]*>(.*?)</strong>", "**$1**");
        markdown = Regex.Replace(markdown, @"(?s)<b[^>]*>(.*?)</b>", "**$1**");
        markdown = Regex.Replace(markdown, @"(?s)<em[^>]*>(.*?)</em>", "*$1*");
        markdown = Regex.Replace(markdown, @"(?s)<i[^>]*>(.*?)</i>", "*$1*");

        // 8. Replace Paragraphs
        markdown = Regex.Replace(markdown, @"(?s)<p[^>]*>(.*?)</p>", "$1\n\n");
        
        // 9. Replace Line Breaks
        markdown = markdown.Replace("<br>", "\n").Replace("<br/>", "\n");

        // 10. Clean up extra whitespace created by replacements
        // Reduce 3+ consecutive newlines down to 2
        markdown = Regex.Replace(markdown, @"\n{3,}", "\n\n");
        return markdown.Trim();
    }
}