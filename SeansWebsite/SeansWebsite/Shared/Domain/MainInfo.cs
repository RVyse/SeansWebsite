using System.Net;
using SeansWebsite.Services;

namespace SeansWebsite.Shared.Domain;

public class MainInfo
{
    public string Heading { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string PageTitle { get; set; } = string.Empty;

    public static MainInfo FromCsv(Dictionary<string, List<string>> data) => new()
    {
        Heading = data.TryGetValue("Heading", out var heading) ? heading.FirstOrDefault() ?? string.Empty : string.Empty,
        Tag = data.TryGetValue("Tag", out var tag) ? tag.FirstOrDefault() ?? string.Empty : string.Empty,
        PageTitle = data.TryGetValue("Page Title Main", out var pageTitle)
            ? WebUtility.HtmlDecode(pageTitle.FirstOrDefault() ?? string.Empty)
            : string.Empty,
    };
}
