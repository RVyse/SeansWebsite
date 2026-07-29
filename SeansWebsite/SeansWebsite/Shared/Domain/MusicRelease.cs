using System.Globalization;
using SeansWebsite.Services;

namespace SeansWebsite.Shared.Domain;

public class MusicRelease
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public bool Display { get; set; }
    public string Image { get; set; } = string.Empty;
    public string SpotifyUrl { get; set; } = string.Empty;
    public string AppleMusicUrl { get; set; } = string.Empty;
    public string YoutubeUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Displays just the year, whether the CSV column contains a plain year (e.g. "2025")
    /// or a full date (ISO "yyyy-MM-dd" or "dd/MM/yyyy").
    /// </summary>
    public string DisplayYear
    {
        get
        {
            if (DateTime.TryParseExact(Year, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var isoDate))
            {
                return isoDate.Year.ToString(CultureInfo.InvariantCulture);
            }

            if (DateTime.TryParseExact(Year, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var slashDate))
            {
                return slashDate.Year.ToString(CultureInfo.InvariantCulture);
            }

            if (DateTime.TryParse(Year, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return parsedDate.Year.ToString(CultureInfo.InvariantCulture);
            }

            return Year;
        }
    }

    /// <summary>
    /// Parses the CSV "Year" column (plain year, ISO date, or dd/MM/yyyy date) into a
    /// comparable <see cref="DateTime"/> for sorting purposes.
    /// </summary>
    public DateTime Date
    {
        get
        {
            if (DateTime.TryParseExact(Year, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var isoDate))
            {
                return isoDate;
            }

            if (DateTime.TryParseExact(Year, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var slashDate))
            {
                return slashDate;
            }

            if (DateTime.TryParse(Year, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return parsedDate;
            }

            return DateTime.MinValue;
        }
    }

    public bool HasSpotifyUrl => IsValidUrl(SpotifyUrl);
    public bool HasAppleMusicUrl => IsValidUrl(AppleMusicUrl);
    public bool HasYoutubeUrl => IsValidUrl(YoutubeUrl);

    public static MusicRelease FromCsvRow(Dictionary<string, string> row) => new()
    {
        Title = row.Get("Title"),
        Type = row.Get("Type"),
        Year = row.Get("Year"),
        Display = row.GetBool("Display"),
        Image = row.Get("Image"),
        SpotifyUrl = row.Get("Spotify URL"),
        AppleMusicUrl = row.Get("Apple Music URL"),
        YoutubeUrl = row.Get("Youtube URL"),
        Description = row.Get("Description"),
    };

    private static bool IsValidUrl(string url) =>
        !string.IsNullOrWhiteSpace(url) && url.Trim() != "#";
}

