using SeansWebsite.Services;

namespace SeansWebsite.Shared.Domain;

public class VideoItem
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public bool Display { get; set; }
    public string YoutubeUrl { get; set; } = string.Empty;

    public string Description => $"{Type} \u00b7 {Year}";

    public string? EmbedUrl => GetEmbedUrl(YoutubeUrl);

    public static VideoItem FromCsvRow(Dictionary<string, string> row) => new()
    {
        Title = row.Get("Title"),
        Type = row.Get("Type"),
        Year = row.Get("Year"),
        Display = row.GetBool("Display"),
        YoutubeUrl = row.Get("Youtube URL"),
    };

    private static string? GetEmbedUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return null;
        }

        string? videoId = null;

        if (uri.Host.Contains("youtu.be", StringComparison.OrdinalIgnoreCase))
        {
            videoId = uri.AbsolutePath.Trim('/');
        }
        else if (uri.Host.Contains("youtube.com", StringComparison.OrdinalIgnoreCase))
        {
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            videoId = query["v"];
        }

        return string.IsNullOrWhiteSpace(videoId)
            ? null
            : $"https://www.youtube.com/embed/{videoId}";
    }
}
