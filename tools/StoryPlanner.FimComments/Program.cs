using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

var bookshelfId = "2203032";
var authorId = "665440";
var outputPath = "fimfiction-comments.json";
string? clientId = null;
string? clientSecret = null;
string? bearerToken = null;

for (var a = 0; a < args.Length; a++)
{
    switch (args[a])
    {
        case "--client-id" when a + 1 < args.Length: clientId = args[++a]; break;
        case "--client-secret" when a + 1 < args.Length: clientSecret = args[++a]; break;
        case "--token" when a + 1 < args.Length: bearerToken = args[++a]; break;
        case "-o" when a + 1 < args.Length: outputPath = args[++a]; break;
        default:
            if (!args[a].StartsWith('-')) outputPath = args[a];
            break;
    }
}

clientId ??= Environment.GetEnvironmentVariable("FIMFICTION_CLIENT_ID");
clientSecret ??= Environment.GetEnvironmentVariable("FIMFICTION_CLIENT_SECRET");
bearerToken ??= Environment.GetEnvironmentVariable("FIMFICTION_TOKEN");

var baseUrl = "https://www.fimfiction.net/api/v2";
var delayMs = 500;

using var http = new HttpClient();
http.DefaultRequestHeaders.UserAgent.ParseAdd("StoryPlanner-FimComments/1.0");
http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

if (bearerToken == null && clientId != null && clientSecret != null)
{
    Console.Error.WriteLine("Requesting OAuth token via Client Credentials...");
    bearerToken = await GetClientCredentialsToken(clientId, clientSecret);
    if (bearerToken == null)
    {
        Console.Error.WriteLine("Failed to obtain token. Exiting.");
        return 1;
    }
    Console.Error.WriteLine("Token acquired.");
}

if (bearerToken != null)
{
    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
}
else
{
    Console.Error.WriteLine("""
        No credentials provided. Set one of:
          --token <bearer-token>
          --client-id <id> --client-secret <secret>
          FIMFICTION_TOKEN env var
          FIMFICTION_CLIENT_ID + FIMFICTION_CLIENT_SECRET env vars

        Register an app at: https://www.fimfiction.net/developers/api/v2/docs/applications
        """);
    return 1;
}

var jsonOpts = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};

Console.Error.WriteLine($"Fetching stories from bookshelf {bookshelfId}...");

var storyIds = new List<string>();
var storyNames = new Dictionary<string, string>();
var nextUrl = (string?)$"{baseUrl}/bookshelves/{bookshelfId}/items?page[size]=100&include=story";

while (nextUrl != null)
{
    var resp = await FetchJson(nextUrl);
    if (resp is not { } root) break;

    if (root.TryGetProperty("included", out var included))
    {
        foreach (var item in included.EnumerateArray())
        {
            if (item.GetProperty("type").GetString() == "story")
            {
                var id = item.GetProperty("id").GetString()!;
                storyIds.Add(id);
                if (item.TryGetProperty("attributes", out var attrs) &&
                    attrs.TryGetProperty("title", out var title))
                {
                    storyNames[id] = title.GetString() ?? id;
                }
            }
        }
    }

    nextUrl = GetNextLink(root);
    if (nextUrl != null) await Task.Delay(delayMs);
}

Console.Error.WriteLine($"Found {storyIds.Count} stories.");

var allComments = new List<CapturedComment>();
var storiesWithComments = 0;

for (var i = 0; i < storyIds.Count; i++)
{
    var storyId = storyIds[i];
    storyNames.TryGetValue(storyId, out var storyName);
    storyName ??= storyId;

    Console.Error.Write($"\r[{i + 1}/{storyIds.Count}] {storyName,-60}");

    var commentUrl = (string?)$"{baseUrl}/story_comments?filter[story_id]={storyId}&filter[author_id]={authorId}";
    var storyCommentCount = 0;

    while (commentUrl != null)
    {
        var resp = await FetchJson(commentUrl);
        if (resp is not { } cRoot) break;

        if (cRoot.TryGetProperty("data", out var data))
        {
            foreach (var comment in data.EnumerateArray())
            {
                var attrs = comment.GetProperty("attributes");
                allComments.Add(new CapturedComment
                {
                    CommentId = comment.GetProperty("id").GetString()!,
                    StoryId = storyId,
                    StoryTitle = storyName,
                    DatePosted = attrs.TryGetProperty("date_posted", out var dp) ? dp.GetString() : null,
                    Body = attrs.TryGetProperty("body", out var body) ? body.GetString() : null,
                    BodyHtml = attrs.TryGetProperty("body_html", out var bh) ? bh.GetString() : null,
                });
                storyCommentCount++;
            }
        }

        commentUrl = GetNextLink(cRoot);
        if (commentUrl != null) await Task.Delay(delayMs);
    }

    if (storyCommentCount > 0) storiesWithComments++;
    await Task.Delay(delayMs);
}

Console.Error.WriteLine();
Console.Error.WriteLine($"Captured {allComments.Count} comments across {storiesWithComments} stories.");

var output = JsonSerializer.Serialize(new
{
    capturedAt = DateTime.UtcNow.ToString("o"),
    bookshelfId,
    authorId,
    totalStories = storyIds.Count,
    totalComments = allComments.Count,
    comments = allComments
}, jsonOpts);

await File.WriteAllTextAsync(outputPath, output);
Console.Error.WriteLine($"Written to {outputPath}");
return 0;

async Task<string?> GetClientCredentialsToken(string id, string secret)
{
    var content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        ["client_id"] = id,
        ["client_secret"] = secret,
        ["grant_type"] = "client_credentials"
    });

    try
    {
        var resp = await http.PostAsync("https://www.fimfiction.net/api/v2/token", content);
        var body = await resp.Content.ReadAsStringAsync();

        if (!resp.IsSuccessStatusCode)
        {
            Console.Error.WriteLine($"Token request failed ({(int)resp.StatusCode}): {body}");
            return null;
        }

        var json = JsonSerializer.Deserialize<JsonElement>(body);
        return json.GetProperty("access_token").GetString();
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Token request error: {ex.Message}");
        return null;
    }
}

async Task<JsonElement?> FetchJson(string url)
{
    for (var attempt = 0; attempt < 3; attempt++)
    {
        try
        {
            var response = await http.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                var retryAfter = response.Headers.RetryAfter?.Delta ?? TimeSpan.FromSeconds(10);
                Console.Error.WriteLine($"\nRate limited, waiting {retryAfter.TotalSeconds}s...");
                await Task.Delay(retryAfter);
                continue;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine($"\nHTTP {(int)response.StatusCode}: {errorBody}");
                if (attempt >= 2) return null;
                await Task.Delay(3000);
                continue;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonElement>(json);
        }
        catch (HttpRequestException ex) when (attempt < 2)
        {
            Console.Error.WriteLine($"\nHTTP error: {ex.Message}, retrying in 5s...");
            await Task.Delay(5000);
        }
    }

    return null;
}

string? GetNextLink(JsonElement root)
{
    if (root.TryGetProperty("links", out var links) &&
        links.TryGetProperty("next", out var next) &&
        next.ValueKind == JsonValueKind.String)
    {
        return next.GetString();
    }
    return null;
}

class CapturedComment
{
    [JsonPropertyName("comment_id")] public string CommentId { get; set; } = "";
    [JsonPropertyName("story_id")] public string StoryId { get; set; } = "";
    [JsonPropertyName("story_title")] public string StoryTitle { get; set; } = "";
    [JsonPropertyName("date_posted")] public string? DatePosted { get; set; }
    [JsonPropertyName("body")] public string? Body { get; set; }
    [JsonPropertyName("body_html")] public string? BodyHtml { get; set; }
}
