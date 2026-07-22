using System.Globalization;
using System.Text.RegularExpressions;

namespace Models.Datasets;

public static class DatasetHelpers
{
    private static readonly Regex TimeRegex = new(@"\d+([.,]\d+)?", RegexOptions.Compiled);

    /// <summary>
    /// Calculates the horse's age based on the race date and year of birth.
    /// </summary>
    public static int? CalculateHorseAge(DateTime raceDate, int horseYob)
    {
        if (horseYob <= 1900 || horseYob > raceDate.Year) 
            return null;

        return raceDate.Year - horseYob;
    }

    /// <summary>
    /// Extracts the kilometer time as a double from a raw string (e.g., "17,1ag" -> 17.1).
    /// Returns null for non-numeric status codes like "dg", "STR", etc.
    /// </summary>
    public static double? ParseKmTime(string rawTime)
    {
        if (string.IsNullOrWhiteSpace(rawTime)) 
            return null;

        var match = TimeRegex.Match(rawTime);
        if (!match.Success) 
            return null;

        var cleanValue = match.Value.Replace(',', '.');
        if (double.TryParse(cleanValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }
        return null;
    }
}