using SeansWebsite.Services;

namespace SeansWebsite.Shared.Domain;

public class PressQuote
{
    public DateTime Date { get; set; }
    public string Quote { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;

    public static PressQuote FromCsvRow(Dictionary<string, string> row) => new()
    {
        Date = row.GetIsoDate("Date"),
        Quote = row.Get("Quote"),
        Source = row.Get("Source"),
    };
}
