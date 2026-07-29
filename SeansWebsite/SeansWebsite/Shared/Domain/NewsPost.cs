using System.Globalization;
using SeansWebsite.Services;

namespace SeansWebsite.Shared.Domain;

public class NewsPost
{
    public DateTime Date { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;

    public string DisplayDate => Date == DateTime.MinValue
        ? string.Empty
        : Date.ToString("MMM yyyy", CultureInfo.InvariantCulture);

    public static NewsPost FromCsvRow(Dictionary<string, string> row) => new()
    {
        Date = row.GetIsoDate("Date"),
        Title = row.Get("Title"),
        Excerpt = row.Get("Detail"),
    };
}
