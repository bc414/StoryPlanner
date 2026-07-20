using Markdig;

namespace StoryPlanner.Core;

/// <summary>
/// Converts a block's RawContent (markdown) to a styled HTML page for display in WebView2.
/// Platform determines the CSS theme: "Claude" → Anthropic style, "Gemini" → Gemini AI Studio style.
/// </summary>
public static class ConversationMarkdownRenderer
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public static string Render(string rawContent, string platform)
    {
        string body = Markdown.ToHtml(rawContent, Pipeline);
        string css  = platform == "Gemini" ? GeminiCss : ClaudeCss;
        return WrapHtml(body, css);
    }

    private static string WrapHtml(string body, string css) => $"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='UTF-8'>
            <meta http-equiv='X-UA-Compatible' content='IE=edge' />
            <style>{css}</style>
        </head>
        <body>{body}</body>
        </html>
        """;

    // ── Anthropic / Claude theme ───────────────────────────────────────────────
    // Claude.ai renders assistant text in a serif face (Tiempos Text) — the point of
    // this theme is to look visibly different from Gemini's sans-serif look, not just
    // swap an accent color.
    private const string ClaudeCss = """
        body {
            font-family: 'Tiempos Text', 'Iowan Old Style', Georgia, Cambria, 'Times New Roman', serif;
            font-size: 16px;
            line-height: 1.7;
            color: #1a1a1a;
            background: #ffffff;
            padding: 20px 28px;
            margin: 0;
        }
        h1,h2,h3,h4 {
            font-family: 'Styrene A', 'ui-sans-serif', system-ui, -apple-system, 'Segoe UI', sans-serif;
            font-weight: 600;
            margin-top: 22px;
            margin-bottom: 10px;
            color: #C96442;
        }
        h1 { font-size: 20px; }
        h2 { font-size: 17px; }
        h3 { font-size: 15px; }
        p  { margin: 0 0 14px; }
        a  { color: #C96442; text-decoration: none; }
        code {
            font-family: 'Fira Code', 'Cascadia Code', Consolas, monospace;
            font-size: 13px;
            background: #f3f3f3;
            padding: 1px 5px;
            border-radius: 3px;
        }
        pre {
            background: #f7f7f7;
            border: 1px solid #e5e5e5;
            border-radius: 6px;
            padding: 14px 16px;
            overflow-x: auto;
        }
        pre code { background: none; padding: 0; }
        ul,ol { margin: 0 0 14px 0; padding-left: 22px; }
        li    { margin-bottom: 6px; }
        blockquote {
            border-left: 3px solid #C96442;
            margin: 0 0 14px 0;
            padding: 4px 14px;
            color: #555;
        }
        hr { border: none; border-top: 1px solid #eee; margin: 20px 0; }
        strong { font-weight: 600; }
        """;

    // ── Google Gemini AI Studio theme ──────────────────────────────────────────
    private const string GeminiCss = """
        body {
            font-family: 'Google Sans', 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
            font-size: 15px;
            line-height: 1.7;
            color: #374151;
            background: #ffffff;
            padding: 20px;
            margin: 0;
        }
        h1,h2,h3,h4 {
            color: #111827;
            margin-top: 24px;
            margin-bottom: 12px;
            font-weight: 500;
        }
        h1 { font-size: 20px; }
        h2 { font-size: 17px; }
        h3 { font-size: 15px; border-bottom: 1px solid #eee; padding-bottom: 6px; }
        p  { margin-bottom: 14px; }
        strong { color: #111827; font-weight: 600; }
        code {
            font-family: Consolas, 'Courier New', monospace;
            font-size: 13px;
            background: #f1f3f4;
            padding: 1px 5px;
            border-radius: 3px;
        }
        pre {
            background: #f1f3f4;
            padding: 14px 16px;
            border-radius: 8px;
            overflow-x: auto;
            border: 1px solid #e0e0e0;
        }
        pre code { background: none; padding: 0; }
        ul,ol { margin: 0 0 14px 0; padding-left: 24px; }
        li    { margin-bottom: 8px; }
        li ul, li ol { margin-top: 8px; margin-bottom: 8px; color: #4b5563; font-size: 0.95em; }
        blockquote {
            border-left: 3px solid #1a73e8;
            margin: 0 0 14px 0;
            padding: 4px 14px;
            color: #555;
        }
        hr { border: none; border-top: 1px solid #e5e7eb; margin: 20px 0; }
        a  { color: #1a73e8; text-decoration: none; }
        """;
}
