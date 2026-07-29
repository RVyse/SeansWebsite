using SeansWebsite.Services;

namespace SeansWebsite.Shared.Domain;

public class GalleryPhoto
{
    public string Image { get; set; } = string.Empty;
    public bool Display { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool Snapshot { get; set; }

    public static GalleryPhoto FromCsvRow(Dictionary<string, string> row) => new()
    {
        Image = row.Get("Image"),
        Display = row.GetBool("Display"),
        Date = row.GetIsoDate("Date"),
        Description = row.Get("Description"),
        Snapshot = row.GetBool("Snapshot"),
    };
}
