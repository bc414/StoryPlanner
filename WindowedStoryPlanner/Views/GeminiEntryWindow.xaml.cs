using System.Windows;
using StoryPlanner.Models;

namespace WindowedStoryPlanner.Views;

public partial class GeminiEntryWindow : Window
{
    public GeminiEntryWindow()
    {
        InitializeComponent();

        this.Loaded += OnWindowLoaded;
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is GeminiEntry entry)
        {
            if (!string.IsNullOrEmpty(entry.ResponseHtml))
            {
                string styledHtml = GetStyledHtml(entry.ResponseHtml);
                ResponseBrowser.NavigateToString(styledHtml);
            }
        }
    }
    
    private string GetStyledHtml(string rawContent)
    {
        // The CSS mimics the clean, spacious look of the Gemini Web UI
        string css = @"
            <style>
                :root {
                    --bg-color: #ffffff;
                    --text-main: #374151; 
                    --text-strong: #111827; 
                    --accent-color: #1a73e8; 
                }

                body { 
                    font-family: 'Google Sans', 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; 
                    font-size: 15px;
                    line-height: 1.7; 
                    color: var(--text-main);
                    padding: 20px; 
                    margin: 0;
                    background-color: var(--bg-color);
                }

                h1, h2, h3, h4 { 
                    color: var(--text-strong); 
                    margin-top: 24px; 
                    margin-bottom: 12px; 
                    font-weight: 500;
                }
                
                h3 { font-size: 18px; border-bottom: 1px solid #eee; padding-bottom: 8px; }

                p { margin-bottom: 16px; }

                strong {
                    color: var(--text-strong);
                    font-weight: 600;
                }

                ul, ol { 
                    margin-top: 8px;
                    margin-bottom: 16px;
                    padding-left: 24px; 
                }
                
                li { 
                    margin-bottom: 8px; 
                    padding-left: 4px;
                }

                ul ul, ol ol, ul ol, ol ul {
                    margin-top: 8px;
                    margin-bottom: 8px;
                    font-size: 0.95em; /* Slightly smaller */
                    color: #4b5563; /* Slightly lighter gray */
                }

                pre { 
                    background-color: #f1f3f4; 
                    padding: 16px; 
                    border-radius: 8px; 
                    overflow-x: auto; 
                    font-family: Consolas, 'Courier New', monospace;
                    font-size: 13px;
                    border: 1px solid #e0e0e0;
                }
            </style>";

        // Wrap the raw content
        return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <meta http-equiv='X-UA-Compatible' content='IE=edge' />
                {css}
            </head>
            <body>
                {rawContent}
            </body>
            </html>";
    }
}