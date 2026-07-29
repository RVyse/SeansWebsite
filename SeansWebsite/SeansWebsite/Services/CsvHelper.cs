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

        var recordRows = ParseRecords(csv);
        if (recordRows.Count == 0)
        {
            return rows;
        }

        var headers = recordRows[0];

        for (var i = 1; i < recordRows.Count; i++)
        {
            var values = recordRows[i];

            if (values.Count == 1 && string.IsNullOrWhiteSpace(values[0]))
            {
                continue;
            }

            var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (var j = 0; j < headers.Count; j++)
            {
                row[headers[j].Trim()] = j < values.Count ? values[j].Trim() : string.Empty;
            }

            rows.Add(row);
        }

        return rows;
    }

    /// <summary>
    /// Parses a headerless "field,value" CSV where the first column is a field name and the
    /// second column is its value. Rows with a blank field name are treated as additional
    /// values for the most recently seen field name (e.g. extra paragraphs of body text).
    /// </summary>
    public static Dictionary<string, List<string>> ParseKeyValue(string csv)
    {
        var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(csv))
        {
            return result;
        }

        var currentKey = string.Empty;

        foreach (var values in ParseRecords(csv))
        {
            if (values.Count == 1 && string.IsNullOrWhiteSpace(values[0]))
            {
                continue;
            }

            var key = values.Count > 0 ? values[0].Trim() : string.Empty;
            var value = values.Count > 1 ? values[1].Trim() : string.Empty;

            if (!string.IsNullOrEmpty(key))
            {
                currentKey = key;
            }

            if (string.IsNullOrEmpty(currentKey) || string.IsNullOrEmpty(value))
            {
                continue;
            }

            if (!result.TryGetValue(currentKey, out var list))
            {
                list = new List<string>();
                result[currentKey] = list;
            }

            list.Add(value);
        }

        return result;
    }

    public static string Get(this Dictionary<string, string> row, string key) =>
        row.TryGetValue(key, out var value) ? value : string.Empty;

    public static bool GetBool(this Dictionary<string, string> row, string key) =>
        row.Get(key).Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase);

    private static readonly string[] SupportedDateFormats =
    {
        "yyyy-MM-dd",
        "dd/MM/yyyy",
        "M/d/yyyy",
    };

    /// <summary>
    /// Parses a CSV date column. Tolerates ISO format (yyyy-MM-dd) as well as dd/MM/yyyy,
    /// since spreadsheet apps (e.g. Excel) often re-save dates in the latter format when the
    /// CSV is edited and re-exported. Returns DateTime.MinValue if unparseable.
    /// </summary>
    public static DateTime GetIsoDate(this Dictionary<string, string> row, string key)
    {
        var value = row.Get(key);

        if (DateTime.TryParseExact(value, SupportedDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var exactDate))
        {
            return exactDate;
        }

        return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)
            ? parsedDate
            : DateTime.MinValue;
    }

    /// <summary>
    /// Tokenizes an entire CSV document into rows of fields, quote-aware. Unlike splitting the
    /// document into lines first, this correctly handles quoted fields that contain embedded
    /// newlines (e.g. multi-paragraph text), which would otherwise fracture a single logical
    /// row into multiple malformed rows.
    /// </summary>
    private static List<List<string>> ParseRecords(string csv)
    {
        var records = new List<List<string>>();
        var currentRow = new List<string>();
        var current = new StringBuilder();
        var inQuotes = false;
        var rowHasContent = false;

        for (var i = 0; i < csv.Length; i++)
        {
            var c = csv[i];

            if (inQuotes)
            {
                if (c == '"')
                {
                    if (i + 1 < csv.Length && csv[i + 1] == '"')
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

                continue;
            }

            switch (c)
            {
                case '"':
                    inQuotes = true;
                    rowHasContent = true;
                    break;
                case ',':
                    currentRow.Add(current.ToString());
                    current.Clear();
                    rowHasContent = true;
                    break;
                case '\r':
                    break;
                case '\n':
                    currentRow.Add(current.ToString());
                    current.Clear();
                    records.Add(currentRow);
                    currentRow = new List<string>();
                    rowHasContent = false;
                    break;
                default:
                    current.Append(c);
                    rowHasContent = rowHasContent || !char.IsWhiteSpace(c);
                    break;
            }
        }

        if (current.Length > 0 || currentRow.Count > 0 || rowHasContent)
        {
            currentRow.Add(current.ToString());
            records.Add(currentRow);
        }

        return records;
    }
}
