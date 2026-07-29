using SeansWebsite.Services;

namespace SeansWebsite.Shared.Domain;

public class VideoItem
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public bool Display { get; set; }

    public string Description => $"{Type} \u00b7 {Year}";

    public static VideoItem FromCsvRow(Dictionary<string, string> row) => new()
    {
        Title = row.Get("Title"),
        Type = row.Get("Type"),
        Year = row.Get("Year"),
        Display = row.GetBool("Display"),
    };
}
