using System.Globalization;
using SeansWebsite.Services;

namespace SeansWebsite.Shared.Domain;

public class TourShow
{
    public DateTime Date { get; set; }
    public string Venue { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Weblink { get; set; } = string.Empty;

    public string DisplayDate => Date == DateTime.MinValue
        ? string.Empty
        : Date.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);

    public static TourShow FromCsvRow(Dictionary<string, string> row) => new()
    {
        Date = row.GetIsoDate("Date"),
        Venue = row.Get("Venue"),
        Location = row.Get("Location"),
        Weblink = row.Get("Weblink"),
    };
}
