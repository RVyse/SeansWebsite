using System.Globalization;
using System.Text;

namespace SeansWebsite.Services;

/// <summary>
/// Minimal CSV parser supporting comma delimiters and quoted fields (with embedded commas / escaped quotes).
/// </summary>
public static class CsvHelper
{
    public static List<Dictionary<string, string>> Parse(string csv)
    {
        var rows = new List<Dictionary<string, string>>();

        if (string.IsNullOrWhiteSpace(csv))
        {
            return rows;
        }

        var lines = csv.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        if (lines.Length == 0)
        {
            return rows;
        }

        var headers = ParseLine(lines[0]);

        for (var i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            var values = ParseLine(lines[i]);
            var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (var j = 0; j < headers.Count; j++)
            {
                row[headers[j].Trim()] = j < values.Count ? values[j].Trim() : string.Empty;
            }

            rows.Add(row);
        }

        return rows;
    }

    public static string Get(this Dictionary<string, string> row, string key) =>
        row.TryGetValue(key, out var value) ? value : string.Empty;

    public static bool GetBool(this Dictionary<string, string> row, string key) =>
        row.Get(key).Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Parses a CSV date column stored in ISO format (yyyy-MM-dd). Returns DateTime.MinValue if unparseable.
    /// </summary>
    public static DateTime GetIsoDate(this Dictionary<string, string> row, string key) =>
        DateTime.TryParseExact(row.Get(key), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : DateTime.MinValue;

    private static List<string> ParseLine(string line)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (inQuotes)
            {
                if (c == '"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    current.Append(c);
                }
            }
            else if (c == '"')
            {
                inQuotes = true;
            }
            else if (c == ',')
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        result.Add(current.ToString());
        return result;
    }
}
