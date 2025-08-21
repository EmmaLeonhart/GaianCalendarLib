using System;
using System.Collections.Generic;
using System.Globalization;

public readonly struct GaianMonth :
    IEquatable<GaianMonth>,
    IComparable<GaianMonth>,
    IFormattable
{
    // 1..14 inclusive
    public const int MinValue = 1;
    public const int MaxValue = 14;

    public int Value { get; }

    public GaianMonth(int value)
    {
        if (value < MinValue || value > MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value), $"Month must be {MinValue}..{MaxValue}");
        Value = value;
    }

    // Factories (nice for readability)
    public static GaianMonth FromInt32(int value) => new(value);

    // Implicit conversions
    public static implicit operator int(GaianMonth m) => m.Value;
    public static implicit operator GaianMonth(int i) => new(i);

    // Optional: implicit to string (ergonomic but be mindful of accidental use)
    public static implicit operator string(GaianMonth m) => m.ToString();

    // Parsing
    public static bool TryParse(string? text, out GaianMonth month, IFormatProvider? provider = null)
    {
        month = default;
        if (string.IsNullOrWhiteSpace(text)) return false;

        // Numeric? allow "7", "07"
        if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n)
            && n >= MinValue && n <= MaxValue)
        {
            month = new GaianMonth(n);
            return true;
        }

        // Name? (case-insensitive, culture-aware lookup)
        var culture = (provider as CultureInfo) ?? CultureInfo.CurrentCulture;
        if (NameToValueMap(culture).TryGetValue(text.Trim(), out var v))
        {
            month = new GaianMonth(v);
            return true;
        }

        return false;
    }

    public static GaianMonth Parse(string text, IFormatProvider? provider = null)
        => TryParse(text, out var m, provider) ? m
           : throw new FormatException($"Invalid Gaian month: '{text}'");

    // Formatting
    // "G" or null -> localized month name; "N" -> number; "NN" -> zero-padded number
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "G";
        var culture = (formatProvider as CultureInfo) ?? CultureInfo.CurrentCulture;

        return format.ToUpperInvariant() switch
        {
            "G" => GetName(Value, culture),                     // e.g., "Aquarius"
            "N" => Value.ToString(culture),                      // e.g., "1"
            "NN" => Value.ToString("00", culture),                // e.g., "01"
            _ => throw new FormatException($"Unknown format '{format}'")
        };
    }

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    // Comparisons / equality
    public int CompareTo(GaianMonth other) => Value.CompareTo(other.Value);
    public bool Equals(GaianMonth other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is GaianMonth m && Equals(m);
    public override int GetHashCode() => Value;

    public static bool operator ==(GaianMonth left, GaianMonth right) => left.Equals(right);
    public static bool operator !=(GaianMonth left, GaianMonth right) => !left.Equals(right);
    public static bool operator <(GaianMonth left, GaianMonth right) => left.Value < right.Value;
    public static bool operator >(GaianMonth left, GaianMonth right) => left.Value > right.Value;
    public static bool operator <=(GaianMonth left, GaianMonth right) => left.Value <= right.Value;
    public static bool operator >=(GaianMonth left, GaianMonth right) => left.Value >= right.Value;

    // Helpers (wrap-around navigation if you like)
    public GaianMonth Next() => new(Value == MaxValue ? MinValue : Value + 1);
    public GaianMonth Previous() => new(Value == MinValue ? MaxValue : Value - 1);

    // ===== Names / localization hooks =====
    // Swap this out to pull from Resources/resx later if you want.
    private static string GetName(int value, CultureInfo culture)
    {
        // Example English names. Replace/extend as needed per culture.
        // If you have multiple locales, switch on culture.TwoLetterISOLanguageName.
        return value switch
        {
            1 => "Aquarius",
            2 => "Pisces",
            3 => "Aries",
            4 => "Taurus",
            5 => "Gemini",
            6 => "Cancer",
            7 => "Leo",
            8 => "Virgo",
            9 => "Libra",
            10 => "Scorpius",
            11 => "Sagittarius",
            12 => "Capricorn",
            13 => "Ophiuchus",
            14 => "Horus",
            _ => $"Month {value}"
        };
    }

    private static IReadOnlyDictionary<string, int> NameToValueMap(CultureInfo culture)
    {
        // Build a case-insensitive name map for parsing names
        var comp = StringComparer.Create(culture, ignoreCase: true);
        var dict = new Dictionary<string, int>(comp);
        for (int v = MinValue; v <= MaxValue; v++)
        {
            dict[GetName(v, culture)] = v;
        }
        return dict;
    }
}
