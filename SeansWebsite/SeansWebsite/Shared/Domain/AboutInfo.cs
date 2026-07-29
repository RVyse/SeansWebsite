using SeansWebsite.Services;

namespace SeansWebsite.Shared.Domain;

public class AboutInfo
{
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public List<string> Paragraphs { get; set; } = new();

    public static AboutInfo FromCsv(Dictionary<string, List<string>> data) => new()
    {
        Title = data.TryGetValue("Title", out var title) ? title.FirstOrDefault() ?? string.Empty : string.Empty,
        Image = data.TryGetValue("Image", out var image) ? image.FirstOrDefault() ?? string.Empty : string.Empty,
        Paragraphs = data.TryGetValue("Text", out var text) ? text : new List<string>(),
    };
}
